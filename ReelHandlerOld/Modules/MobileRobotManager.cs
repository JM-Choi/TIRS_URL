#region Imports
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TechFloor.Util;
using TechFloor.Object;
using System.Net;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Security;
using TechFloor.Components;
#endregion

#region Program
namespace TechFloor.Components
{
    #region Enumerations
    public enum MobileRobotOperationModes
    {
        Idle,
        Ready,
        Load,
        Unload
    }

    public enum CartDockingSequences
    {
        Unknown,
        Ready,
        RequestLoadCart,
        ReceivedLoadCartResponse,
        StartLoadCart,
        ArriveAtWorkZoneToLoad,
        MoveToLoadWorkZone,
        CompleteLoadCart,
        RequestUnloadCart,
        ReceivedUnloadCartResponse,
        StartUnloadCart,
        ArriveAtWorkZoneToUnload,
        MoveToUnloadWorkZone,
        CameOutWorkZone,
        CompleteUnloadCart,
    };

    public enum MobileRobotManagerCommands
    {
        None,
        WorkZoneMoveIn,
        WorkZoneLoad,
        WorkZoneUnload,
    }
    #endregion

    public class MobileRobotManager : SimulatableDevice
    {
        #region Constants
        protected readonly char[] CONST_RESPONSE_DELIMITERS = new char[] { '\r', '\n', '\0' };
        protected readonly char[] CONST_TOKEN_DELIMITER = new char[] { ';' };
        protected const int CONST_DEACULT_SENSOR_DETECTION_DELAY = 5000;
        #endregion

        #region Fields
        protected bool stop                                 = false;
        protected bool received                             = false;
        protected bool failure                              = false;
        protected bool serviceout                           = false;
        protected bool initialized                          = false;
        protected int sentTick                              = 0;
        protected int workZoneSensorTick                        = 0;
        protected int bufferZoneSensorTick                      = 0;
        protected int clientId                              = 0;
        protected string lastMessage                        = string.Empty;
        protected string lastSentMessage                    = string.Empty;
        protected string lastLogMessage                     = string.Empty;
        protected AsyncSocketServer server                  = null;
        protected Thread threadCartStateWatcher_            = null;
        protected Dictionary<int, AsyncSocketClient> clients= new Dictionary<int, AsyncSocketClient>();
        protected CartDockingSequences cartDockingSequence  = CartDockingSequences.Unknown;
        protected CommunicationStates communicationState    = CommunicationStates.None;
        protected MobileRobotOperationModes cartMode        = MobileRobotOperationModes.Idle;
        protected MobileRobotManagerCommands lastCommand    = MobileRobotManagerCommands.None;

        string sStatusIO_Work = string.Empty;
        string sStatusIO_Buffer = string.Empty;
        #endregion

        #region Properties
        public bool IsReceived                              => received;
        public bool IsFailure                               => failure;
        public bool IsConnected                             => (clients.Count > 0);
        public CommunicationStates CommunicationState       => communicationState;
        public CartDockingSequences CartDockingSequence     => cartDockingSequence;
        public MobileRobotOperationModes CartMode           => cartMode;
        public MobileRobotManagerCommands LastSentCommand          => lastCommand;
        public bool IsOverDelayTime(int delay, int tick)    => (TimeSpan.FromMilliseconds(App.TickCount - tick).TotalMilliseconds >= delay);
        #endregion

        #region Events
        public event EventHandler<CommunicationStates> CommunicationStateChanged;
        public event EventHandler<string> ReportRuntimeLog;
        #endregion

        #region Protected methods
        protected void RemoveAllClients()
        {
            if (clients.Count > 0)
            {
                foreach (var client in clients)
                {
                    if (client.Value.IsConnected)
                        client.Value.Disconnect();
                }

                clients.Clear();
            }
        }

        protected void RemoveClient(int id)
        {
            foreach (var client in clients)
            {
                if (id == client.Key)
                {
                    clients.Remove(id);
                    break;
                }
            }
        }

        protected void FireCommunicationStateChanged(CommunicationStates state)
        {
            CommunicationStateChanged?.Invoke(this, communicationState = state);
        }

        protected void OnServerAccept(object sender, AsyncSocketAcceptEventArgs e)
        {
            foreach (var client in clients)
            {
                IPEndPoint s1 = client.Value.Socket.RemoteEndPoint as IPEndPoint;
                IPEndPoint s2 = e.Worker.RemoteEndPoint as IPEndPoint;

                if (s1.Address.ToString().Contains(s2.Address.ToString()))
                    client.Value.Socket.Disconnect(true);
            }

            FireCommunicationStateChanged(CommunicationStates.Accepted);
            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={++clientId}");

            Thread.Sleep(100);
            AsyncSocketClient newClient = new AsyncSocketClient(clientId, e.Worker, OnClientConnected, OnClientSent, OnClientReceived);
            newClient.OnDisconnected    += new AsyncSocketDisconnectedEventHandler(OnClientDisconnected);
            newClient.OnError           += new AsyncSocketErrorEventHandler(OnClientError);

            lock (clients)
                clients.Add(clientId, newClient);
        }

        protected void OnClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            try
            {
                lastMessage = string.Empty;
                lastSentMessage = string.Empty;
                FireCommunicationStateChanged(CommunicationStates.Error);
                FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}({e.Id})={e.AsyncSocketException.Message}");

                lock (clients)
                {
                    foreach (var obj in clients)
                    {
                        clients[obj.Key].Socket.Disconnect(true);
                        RemoveClient(e.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
                //RemoveClient(e.ID);
                RemoveAllClients();
            }
        }

        protected void OnClientConnected(object sender, AsyncSocketConnectionEventArgs e)
        {
            lastMessage = string.Empty;
            lastSentMessage = string.Empty;
            FireCommunicationStateChanged(CommunicationStates.Connected);
            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.Id}");
        }

        protected void OnClientDisconnected(object sender, AsyncSocketConnectionEventArgs e)
        {
            lastMessage = string.Empty;
            lastSentMessage = string.Empty;
            FireCommunicationStateChanged(CommunicationStates.Disconnected);
            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.Id}");
            RemoveClient(e.Id);
        }

        protected void OnClientReceived(object sender, AsyncSocketReceiveEventArgs e)
        {
            if (e == null || e.ReceiveData.Length <= 0)
                return;

            string replyMessage_ = string.Empty;
            string receivedMessage_ = Encoding.Default.GetString(e.ReceiveData).TrimEnd(CONST_RESPONSE_DELIMITERS).Replace("\r\n", string.Empty);
            string[] tokens_ = receivedMessage_.Split(CONST_TOKEN_DELIMITER, StringSplitOptions.RemoveEmptyEntries);

            if (lastMessage != receivedMessage_ && !string.IsNullOrEmpty(receivedMessage_))
                FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={lastMessage = receivedMessage_}");

            switch (tokens_[0].ToUpper())
            {
                case "STATUS":
                    {
                        if (tokens_.Length >= 2)
                        {
                            receivedMessage_ = string.Empty;

                            switch (tokens_[1].ToUpper())
                            {
                                default:
                                    break;
                                case "READY":
                                    {
                                        replyMessage_ = "STATUS;ready;\r\n";
                                        cartMode = MobileRobotOperationModes.Ready;
                                    }
                                    break;
                                case "LOADING":
                                    {
                                        replyMessage_ = "STATUS;loading;\r\n";
                                        cartMode = MobileRobotOperationModes.Load;
                                    }
                                    break;
                                case "UNLOADING":
                                    {
                                        replyMessage_ = "STATUS;unloading;\r\n";
                                        cartMode = MobileRobotOperationModes.Unload;
                                    }
                                    break;
                            }

                            if (!string.IsNullOrEmpty(replyMessage_))
                                SendSocketData(replyMessage_);
                        }
                    }
                    break;
                case "TRANSFERREQ":
                    {
                        if (tokens_.Length >= 2 && !failure)
                        {
                            switch (tokens_[1].ToUpper())
                            {
                                default:
                                    break;
                                case "WORKZONELOAD":
                                    {
                                        received = true;
                                        cartDockingSequence = CartDockingSequences.ReceivedLoadCartResponse;
                                    }
                                    break;
                                case "WORKZONEUNLOAD":
                                    {
                                        received = true;
                                        cartDockingSequence = CartDockingSequences.ReceivedUnloadCartResponse;
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "TRANSFERSTATE":
                    {
                        if (tokens_.Length >= 3)
                        {
                            switch (tokens_[1].ToUpper())
                            {
                                default:
                                    break;
                                case "WORK":
                                    {
                                        switch (tokens_[2].ToUpper())
                                        {
                                            case "LOADSTART":
                                                {
                                                    if ((App.DigitalIoManager as DigitalIoManager).IsCartHooked || (App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                                                        tokens_[2] = "LOADSTART_FAILURE";
                                                    else
                                                        cartDockingSequence = CartDockingSequences.StartLoadCart;
                                                }
                                                break;
                                            case "UNLOADSTART":
                                                {
                                                    if ((App.DigitalIoManager as DigitalIoManager).IsCartReleased & (App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone) // || !(App.MainForm as FormMain).UseMobileRobot)
                                                        cartDockingSequence = CartDockingSequences.StartUnloadCart;
                                                    else
                                                        tokens_[2] = "UNLOADSTART_FAILURE";
                                                }
                                                break;
                                            case "LOADDONE":
                                                cartDockingSequence = CartDockingSequences.CompleteLoadCart;
                                                break;
                                            case "UNLOADDONE":
                                                cartDockingSequence = CartDockingSequences.CompleteUnloadCart;
                                                break;
                                            case "ARRIVEZONE":
                                                {
                                                    switch (cartDockingSequence)
                                                    {
                                                        case CartDockingSequences.StartLoadCart:
                                                            cartDockingSequence = CartDockingSequences.ArriveAtWorkZoneToLoad;
                                                            break;
                                                        case CartDockingSequences.StartUnloadCart:
                                                            cartDockingSequence = CartDockingSequences.ArriveAtWorkZoneToUnload;
                                                            break;
                                                    }
                                                }
                                                break;
                                            case "CAMEOUTZONE":
                                                cartDockingSequence = CartDockingSequences.CameOutWorkZone;
                                                break;
                                        }
                                    }
                                    break;
                            }

                            SendSocketData($"{tokens_[0]};{tokens_[1]};{tokens_[2]};\r\n");
                        }
                    }
                    break;
            }

            // if (lastMessage != receivedMessage_ && !string.IsNullOrEmpty(receivedMessage_))
            // {
            //     FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={lastMessage = receivedMessage_}");
            // }
        }

        protected void OnClientSent(object sender, AsyncSocketSendEventArgs e)
        {
        }
        #endregion

        #region Public methods

        #region Server service control methods
        public void StartServer(int portno = 0)
        {
            if (server == null)
            {
                server = new AsyncSocketServer(portno);
                server.OnAccepted += new AsyncSocketAcceptedEventHandler(OnServerAccept);
                server.OnError += new AsyncSocketErrorEventHandler(OnClientError);
            }

            stop = !(server.Start());
        }

        public void StopServer(bool forced = false)
        {
            server.Stop(forced);
            server.OnAccepted -= new AsyncSocketAcceptedEventHandler(OnServerAccept);
            server.OnError -= new AsyncSocketErrorEventHandler(OnClientError);
            server = null;

            ResetConversationFlags();
        }
        #endregion

        public void Create(int portno)
        {
            serviceout                  = false;
            clientId                    = 0;            
            CommunicationStateChanged   += (App.MainForm as FormMain).OnChangedMobileRobotCommunicationState;
            ReportRuntimeLog            += (App.MainForm as FormMain).OnReportRobotRuntimeLog;
            StartServer(portno);

            FireCommunicationStateChanged(CommunicationStates.Listening);
            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}");
            (threadCartStateWatcher_ = new Thread(new ThreadStart(Run)))?.Start();
        }

        public void Destroy()
        {
            serviceout = true;
            threadCartStateWatcher_.Join();
            StopServer(true);
            CommunicationStateChanged -= (App.MainForm as FormMain).OnChangedMobileRobotCommunicationState;
            ReportRuntimeLog -= (App.MainForm as FormMain).OnReportRobotRuntimeLog;
        }

        public void FireCommunicationStateChanged()
        {
            CommunicationStateChanged?.Invoke(this, communicationState);
        }

        public void FireReportRuntimeLog(string text = null)
        {
            ReportRuntimeLog?.Invoke(this, text);
        }

        public void Run()
        {
            string stateMessage_ = string.Empty;
            Logger.Trace($"Started: Thread={MethodBase.GetCurrentMethod().Name}");

            while (!stop)
            {
                if (serviceout || (App.ShutdownEvent as ManualResetEvent).WaitOne(1000, false))
                {
                    Logger.Trace($"Stopped: Thread={MethodBase.GetCurrentMethod().Name}");
                    stop = true;
                }
                else if (IsConnected)
                {
                    if ((App.DigitalIoManager as DigitalIoManager).WorkZoneCartPresentSensor1 &&
                        (App.DigitalIoManager as DigitalIoManager).WorkZoneCartPresentSensor2)
                    {
                        if (workZoneSensorTick == 0)
                        {
                            workZoneSensorTick = App.TickCount;
                        }
                        else if (IsOverDelayTime(CONST_DEACULT_SENSOR_DETECTION_DELAY, workZoneSensorTick))
                        {
                            sStatusIO_Work = "ON;";
                        }
                    }
                    else
                    {
                        workZoneSensorTick = 0;
                        sStatusIO_Work = "OFF;";
                    }

                    if ((App.DigitalIoManager as DigitalIoManager).BufferZoneCartPresentSensor1 &&
                        (App.DigitalIoManager as DigitalIoManager).BufferZoneCartPresentSensor2)
                    {
                        if (bufferZoneSensorTick == 0)
                        {
                            bufferZoneSensorTick = App.TickCount;
                        }
                        else if (IsOverDelayTime(CONST_DEACULT_SENSOR_DETECTION_DELAY, bufferZoneSensorTick))
                        {
                            sStatusIO_Buffer = "ON;";
                        }
                    }
                    else
                    {
                        bufferZoneSensorTick = 0;
                        sStatusIO_Buffer = "OFF;";
                    }

                    switch (App.MainSequence.OperationState)
                    {
                        case OperationStates.Run:
                            {
                                if ((App.MainForm as FormMain).UseMobileRobot)
                                    stateMessage_ = "STATUS;RUN;";
                                else
                                    stateMessage_ = "STATUS;IDLE;";
                            }
                            break;
                        case OperationStates.Alarm:
                            stateMessage_ = "STATUS;ERR;";
                            break;
                        default:
                            stateMessage_ = "STATUS;IDLE;";
                            break;
                    }

                    // unknown, ready, manual, loadreq, unloadreq, runnning
                    string mode = "MANUAL;";

                    if ((App.MainForm as FormMain).UseMobileRobot)
                    {
                        switch ((App.MainSequence as ReelTowerRobotSequence).GetDockingStates())
                        {
                            case CartDockingStates.Unknown:
                                {
                                    mode = "UNKNOWN;";
                                }
                                break;
                            case CartDockingStates.LoadStarted:
                            case CartDockingStates.Loading:
                                {
                                    mode = "LOADREQ;";
                                }
                                break;
                            case CartDockingStates.LoadCompleted:
                                {
                                    mode = "RUNNING;";
                                }
                                break;
                            case CartDockingStates.UnloadStarted:
                            case CartDockingStates.Unloading:
                                {
                                    mode = "UNLOADREQ;";
                                }
                                break;
                            case CartDockingStates.UnloadCompleted:
                                {
                                    mode = "READY;";
                                }
                                break;
                        }
                    }

                    SendSocketData($"{stateMessage_}{sStatusIO_Work}{sStatusIO_Buffer}{mode}\r\n");
                }
            }
        }

        public bool IsResponseTimeout(int timeout = 1000)
        {
            bool result = false;

            if (!received)
            {
                switch (lastCommand)
                {
                    case MobileRobotManagerCommands.WorkZoneMoveIn:
                    case MobileRobotManagerCommands.WorkZoneLoad:
                    case MobileRobotManagerCommands.WorkZoneUnload:
                        {
                            if (TimeSpan.FromMilliseconds(App.TickCount - sentTick).TotalMilliseconds > timeout)
                            {   // Remove message from queue.
                                ResetConversationFlags(result = true);
                                sentTick    = 0;
                                failure     = true;
                            }
                        }
                        break;
                }
            }

            return result;
        }

        public void ResetConversationFlags(bool all = false)
        {
            received        = false;
            failure         = false;

            if (all)
            {
                lastCommand = MobileRobotManagerCommands.None;
                cartDockingSequence = CartDockingSequences.Ready;
            }
        }

        public void SendCommand(MobileRobotManagerCommands command)
        {
            string commandText_ = string.Empty;

            ResetConversationFlags();

            switch (command)
            {
                case MobileRobotManagerCommands.None:
                case MobileRobotManagerCommands.WorkZoneMoveIn:
                    return;
                case MobileRobotManagerCommands.WorkZoneLoad:
                    {
                        commandText_        = "TRANSFERREQ;WORKZONELOAD;\r\n";
                        lastCommand         = MobileRobotManagerCommands.WorkZoneLoad;
                        cartDockingSequence = CartDockingSequences.RequestLoadCart;
                    }
                    break;
                case MobileRobotManagerCommands.WorkZoneUnload:
                    {
                        commandText_        = "TRANSFERREQ;WORKZONEUNLOAD;\r\n";
                        lastCommand         = MobileRobotManagerCommands.WorkZoneUnload;
                        cartDockingSequence = CartDockingSequences.RequestUnloadCart;
                    }
                    break;
            }

            foreach (var client in clients.Values)
                client.Send(Encoding.Default.GetBytes(commandText_));

            switch (cartDockingSequence)
            {
                case CartDockingSequences.RequestLoadCart:
                case CartDockingSequences.RequestUnloadCart:
                    sentTick = App.TickCount;
                    break;
            }

            if (!string.IsNullOrEmpty(commandText_) && lastSentMessage != commandText_)
                FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={lastSentMessage = commandText_}");
        }

        public void SendSocketData(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            try
            {
                ResetConversationFlags();

                switch (message)
                {
                    case "TRANSFERREQ;WORKZONELOAD;\r\n":
                        {
                            lastCommand = MobileRobotManagerCommands.WorkZoneLoad;
                            cartDockingSequence = CartDockingSequences.RequestLoadCart;
                        }
                        break;
                    case "TRANSFERREQ;WORKZONEUNLOAD;\r\n":
                        {
                            lastCommand = MobileRobotManagerCommands.WorkZoneUnload;
                            cartDockingSequence = CartDockingSequences.RequestUnloadCart;
                        }
                        break;
                }

                foreach (var client in clients.Values)
                    client.Send(Encoding.Default.GetBytes(message));

                switch (cartDockingSequence)
                {
                    case CartDockingSequences.RequestLoadCart:
                    case CartDockingSequences.RequestUnloadCart:
                        sentTick = App.TickCount;
                        break;
                }

                if (!string.IsNullOrEmpty(message) && lastSentMessage != message)
                    FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={lastSentMessage = message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public void ResetCartDockingSequence()
        {
            cartDockingSequence = CartDockingSequences.Ready;
        }

        public void RestartMobileRobotServiceManager(int portno)
        {
            Destroy();
            RemoveAllClients();
            Create(portno);
        }
        #endregion
    }
}
#endregion