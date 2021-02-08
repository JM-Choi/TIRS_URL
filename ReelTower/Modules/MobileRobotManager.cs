#region Licenses
///////////////////////////////////////////////////////////////////////////////
/// MIT License
/// 
/// Copyright (c) 2019 Marcus Software Ltd.
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
/// SOFTWARE.
///////////////////////////////////////////////////////////////////////////////
///          Copyright Joe Coder 2004 - 2006.
/// Distributed under the Boost Software License, Version 1.0.
///    (See accompanying file LICENSE_1_0.txt or copy at
///          https://www.boost.org/LICENSE_1_0.txt)
///////////////////////////////////////////////////////////////////////////////
/// 저작권 (c) 2019 Marcus Software Ltd. (isadrastea.kor@gmail.com)
///
/// 본 라이선스의 적용을 받는 소프트웨어와 동봉된 문서(소프트웨어)를 획득하는 
/// 모든 개인이나 기관은 소프트웨어를 Marcus Software (isadrastea.kor@gmail.com)
/// 에 신고하고, 허용 의사를 서면으로 득하여, 사용, 복제, 전시, 배포, 실행 및
/// 전송할 수 있고, 소프트웨어의 파생 저작물을 생성할 수 있으며, 소프트웨어가
/// 제공된 제3자에게 그러한 행위를 허용할 수 있다. 단, 이 모든 행위는 다음과 
/// 같은 조건에 의해 제한 한다.:
///
/// 소프트웨어의 저작권 고지, 그리고 위의 라이선스 부여와 이 규정과 아래의 부인 
/// 조항을 포함한 이 글의 전문이 소프트웨어를 전체적으로나 부분적으로 복제한 
/// 모든 복제본과 소프트웨어의 모든 파생 저작물 내에 포함되어야 한다. 단, 해당 
/// 복제본이나 파생저작물이 소스 언어 프로세서에 의해 생성된, 컴퓨터로 인식 
/// 가능한 오브젝트 코드의 형식으로만 되어 있는 경우는 제외된다.
///
/// 이 소프트웨어는 상품성, 특정 목적에의 적합성, 소유권, 비침해에 대한 보증을 
/// 포함한, 이에 국한되지는 않는, 모든 종류의 명시적이거나 묵시적인 보증 없이 
///“있는 그대로의 상태”로 제공된다. 저작권자나 소프트웨어의 배포자는 어떤 
/// 경우에도 소프트웨어 자체나 소프트웨어의 취급과 관련하여 발생한 손해나 기타 
/// 책임에 대하여, 계약이나 불법행위 등에 관계 없이 어떠한 책임도 지지 않는다.
///////////////////////////////////////////////////////////////////////////////
/// project ReelTower 
/// @author Marcus (isadrastea.kor@gmail.com)
/// @namespace Marcus.Solution.TechFloor
/// @file MobileRobotManager.cs
/// @brief
/// @details
/// @date 2019, 5, 21, 오후 1:13
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Marcus.Solution.TechFloor.Util;
using Marcus.Solution.TechFloor.Object;
using System.Net;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Security;
#endregion

#region Program
namespace Marcus.Solution.TechFloor
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

    public enum MobileRobotCommands
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
        protected MobileRobotCommands lastCommand           = MobileRobotCommands.None;

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
        public MobileRobotCommands LastSentCommand          => lastCommand;
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
                    if (client.Value.IsOpen)
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
                IPEndPoint s1 = client.Value.Sock.RemoteEndPoint as IPEndPoint;
                IPEndPoint s2 = e.Worker.RemoteEndPoint as IPEndPoint;

                if (s1.Address.ToString().Contains(s2.Address.ToString()))
                    client.Value.Sock.Disconnect(true);
            }

            FireCommunicationStateChanged(CommunicationStates.Accepted);
            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={++clientId}");

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
                FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}({e.ID})={e.AsyncSocketException.Message}");

                lock (clients)
                {
                    foreach (var obj in clients)
                    {
                        clients[obj.Key].Sock.Disconnect(true);
                        RemoveClient(e.ID);
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
            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.ID}");
        }

        protected void OnClientDisconnected(object sender, AsyncSocketConnectionEventArgs e)
        {
            lastMessage = string.Empty;
            lastSentMessage = string.Empty;
            FireCommunicationStateChanged(CommunicationStates.Disconnected);
            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.ID}");
            RemoveClient(e.ID);
        }

        protected void OnClientReceived(object sender, AsyncSocketReceiveEventArgs e)
        {
            if (e == null || e.ReceiveData.Length <= 0)
                return;

            string replyMessage_ = string.Empty;
            string receivedMessage_ = Encoding.Default.GetString(e.ReceiveData).TrimEnd(CONST_RESPONSE_DELIMITERS).Replace("\r\n", string.Empty);
            string[] tokens_ = receivedMessage_.Split(CONST_TOKEN_DELIMITER, StringSplitOptions.RemoveEmptyEntries);

            if (lastMessage != receivedMessage_ && !string.IsNullOrEmpty(receivedMessage_))
                FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={lastMessage = receivedMessage_}");

            // UPDATED: 20191028
            // DESCRIPTION: You have to insert the mobile robot operation state (Auto or Manual) to state of operation column.
            // (App.MainForm as FormMain).UseMobileRobot
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
                                                    if ((App.DigitalIoManager as DigitalIoManager).IsCartClamped || (App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
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
            //     FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={lastMessage = receivedMessage_}");
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

            server.Start();
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
            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}");
            (threadCartStateWatcher_ = new Thread(new ThreadStart(Run)))?.Start();
        }

        public void Destroy()
        {
            serviceout = true;
            stop = false;
            threadCartStateWatcher_.Join();
            StopServer(true);
            CommunicationStateChanged -= (App.MainForm as FormMain).OnChangedMobileRobotCommunicationState;
            ReportRuntimeLog -= (App.MainForm as FormMain).OnReportRobotRuntimeLog;
        }

        public void FireCommunicationStateChanged()
        {
            CommunicationStateChanged?.Invoke(this, communicationState);
        }

        public void FireUpdateRuntimeLog(string text = null)
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
                        switch ((App.MainSequence as ReelTowerGroupSequence).GetDockingStates())
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
                    case MobileRobotCommands.WorkZoneMoveIn:
                    case MobileRobotCommands.WorkZoneLoad:
                    case MobileRobotCommands.WorkZoneUnload:
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
                lastCommand = MobileRobotCommands.None;
                cartDockingSequence = CartDockingSequences.Ready;
            }
        }

        public void SendCommand(MobileRobotCommands command)
        {
            string commandText_ = string.Empty;

            ResetConversationFlags();

            switch (command)
            {
                case MobileRobotCommands.None:
                case MobileRobotCommands.WorkZoneMoveIn:
                    return;
                case MobileRobotCommands.WorkZoneLoad:
                    {
                        commandText_        = "TRANSFERREQ;WORKZONELOAD;\r\n";
                        lastCommand         = MobileRobotCommands.WorkZoneLoad;
                        cartDockingSequence = CartDockingSequences.RequestLoadCart;
                    }
                    break;
                case MobileRobotCommands.WorkZoneUnload:
                    {
                        commandText_        = "TRANSFERREQ;WORKZONEUNLOAD;\r\n";
                        lastCommand         = MobileRobotCommands.WorkZoneUnload;
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
                FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={lastSentMessage = commandText_}");
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
                            lastCommand = MobileRobotCommands.WorkZoneLoad;
                            cartDockingSequence = CartDockingSequences.RequestLoadCart;
                        }
                        break;
                    case "TRANSFERREQ;WORKZONEUNLOAD;\r\n":
                        {
                            lastCommand = MobileRobotCommands.WorkZoneUnload;
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
                    FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={lastSentMessage = message}");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
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