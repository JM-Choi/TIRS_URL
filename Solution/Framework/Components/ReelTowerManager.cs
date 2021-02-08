#region Imports
using System;
using System.Net;
using System.Text;
using System.Xml;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TechFloor.Device.CommunicationIo;
using TechFloor.Object;
using TechFloor.Util;
using TechFloor.Components;
#endregion

#region Program
namespace TechFloor.Components
{
    public class ReelTowerManager : SimulatableDevice
    {
        #region Constants
        protected const string CONST_TOWER_IDS                      = "T0101;T0102;T0103;T0104;";

        protected readonly char[] CONST_MATERIAL_DELIMETERS         = { ';' };

        protected readonly char[] CONST_CARRIER_LIST_DELIMITER      = { '|' };
        #endregion

        #region Fields
        protected bool stop                                         = false;

        protected bool requiredLoadCancelIfFailure                  = false;

        protected bool serviceout                                   = false;

        protected int receivedStateResponseFlag                     = 0;

        protected int statePollingTick                              = 0;

        protected int clientId                                      = 0;

        protected int lastLoadedTower                               = 0;

        protected int receivedTowerIdOfState                        = -1;

        protected int receivedTowerIdOfResponse                     = -1;

        protected int timeoutOfResponse                             = 3000;

        protected string currentLoadTowerId                         = string.Empty;

        protected string currentUnloadTowerId                       = string.Empty;

        protected string lastRequestedLoadTowerId                   = string.Empty;

        protected string lastReceivedMessage                        = string.Empty;

        protected string lastCommunicationStateLog                  = string.Empty;

        protected XmlDocument receivedXml                           = new XmlDocument();

        protected MaterialStorageMessage receivedData               = new MaterialStorageMessage();

        protected MaterialData receivedBarcode                      = new MaterialData();

        protected Pair<int, string> queuedMessage                   = new Pair<int, string>(0, string.Empty);

        protected Thread threadReelTowerStateWatcher                = null;

        protected MaterialStorageState towerResponse                = new MaterialStorageState();

        protected Dictionary<int, ReelTowerObject> clients          = new Dictionary<int, ReelTowerObject>();

        protected ConcurrentQueue<ReelTowerMessage> messageQueue    = new ConcurrentQueue<ReelTowerMessage>();

        protected ConcurrentQueue<Pair<int, string>> receivedMessages  = new ConcurrentQueue<Pair<int, string>>();

        protected List<int> loadAvailableTowers                     = new List<int>();

        protected Dictionary<int, Pair<string, string>> reelTowerIds = new Dictionary<int, Pair<string, string>>();

        // protected List<MaterialPackage> materialPackages         = new List<MaterialPackage>();

        protected List<MaterialStorageState> unloadRequests         = new List<MaterialStorageState>();

        protected List<MaterialStorageState> towerStates            = new List<MaterialStorageState>();

        protected Dictionary<string, ReelTowerCommands> reelTowerCommands = new Dictionary<string, ReelTowerCommands>()
        {
            { "REQUEST_TOWER_STATE",            ReelTowerCommands.REQUEST_TOWER_STATE },
            { "REQUEST_TOWER_STATE_ALL",        ReelTowerCommands.REQUEST_TOWER_STATE_ALL },
            { "REQUEST_BARCODEINFO_CONFIRM",    ReelTowerCommands.REQUEST_BARCODEINFO_CONFIRM },
            { "REQUEST_REEL_LOAD_MOVE",         ReelTowerCommands.REQUEST_REEL_LOAD_MOVE },
            { "REQUEST_REEL_LOAD_ASSIGN",       ReelTowerCommands.REQUEST_REEL_LOAD_ASSIGN },
            { "REQUEST_LOAD_COMPLETE",          ReelTowerCommands.REQUEST_LOAD_COMPLETE },
            { "REQUEST_UNLOAD_COMPLETE",        ReelTowerCommands.REQUEST_UNLOAD_COMPLETE },
            { "REQUEST_LINK_TEST",              ReelTowerCommands.REQUEST_LINK_TEST },
            { "REQUEST_REEL_UNLOAD_MOVE",       ReelTowerCommands.REQUEST_REEL_UNLOAD_MOVE },
            { "REQUEST_REEL_UNLOAD_ASSIGN",     ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN },
            { "REQUEST_LOAD_RESET",             ReelTowerCommands.REQUEST_LOAD_RESET },
            { "REQUEST_UNLOAD_RESET",           ReelTowerCommands.REQUEST_UNLOAD_RESET },
            { "REPLY_TOWER_STATE",              ReelTowerCommands.REPLY_TOWER_STATE },
            { "REPLY_TOWER_STATE_ALL",          ReelTowerCommands.REPLY_TOWER_STATE_ALL },
            { "REPLY_BARCODEINFO_CONFIRM",      ReelTowerCommands.REPLY_BARCODEINFO_CONFIRM },
            { "REPLY_REEL_LOAD_MOVE",           ReelTowerCommands.REPLY_REEL_LOAD_MOVE },
            { "REPLY_REEL_LOAD_ASSIGN",         ReelTowerCommands.REPLY_REEL_LOAD_ASSIGN },
            { "REPLY_REEL_UNLOAD_MOVE",         ReelTowerCommands.REPLY_REEL_UNLOAD_MOVE },
            { "REPLY_REEL_UNLOAD_ASSIGN",       ReelTowerCommands.REPLY_REEL_UNLOAD_ASSIGN },
            { "REPLY_LOAD_COMPLETE",            ReelTowerCommands.REPLY_LOAD_COMPLETE },
            { "REPLY_UNLOAD_COMPLETE",          ReelTowerCommands.REPLY_UNLOAD_COMPLETE },
            { "REPLY_LINK_TEST",                ReelTowerCommands.REPLY_LINK_TEST },
            { "REPLY_LOAD_RESET",               ReelTowerCommands.REPLY_LOAD_RESET },
            { "REPLY_UNLOAD_RESET",             ReelTowerCommands.REPLY_UNLOAD_RESET },
            { "SEND_PICKING_LIST",              ReelTowerCommands.SEND_PICKING_LIST },
            { "REQUEST_ALL_LOAD_RESET",         ReelTowerCommands.REQUEST_ALL_LOAD_RESET },
            { "REQUEST_ALL_UNLOAD_RESET",       ReelTowerCommands.REQUEST_ALL_UNLOAD_RESET },
            { "REQUEST_ALL_ALARM_RESET",        ReelTowerCommands.REQUEST_ALL_ALARM_RESET },
            { "REQUEST_ALL_STOP",               ReelTowerCommands.REQUEST_ALL_STOP },
            { "REPLY_ALL_STOP",                 ReelTowerCommands.REPLY_ALL_STOP },
            { "REPORT_SET_ALARM",               ReelTowerCommands.REPORT_SET_ALARM },
            { "REPORT_CLEAR_ALARM",             ReelTowerCommands.REPORT_CLEAR_ALARM },
        };

        protected AsyncSocketServer server                          = null;

        protected CommunicationStates communicationState            = CommunicationStates.None;

        protected System.Timers.Timer responseWatcher               = null;
        #endregion

        #region Properties
        public virtual int TimeoutOfTowerResponse
        {
            get => timeoutOfResponse;
            set => timeoutOfResponse = value;
        }

        public virtual bool IsRunning                                                   => !stop;

        public virtual bool IsServiceNow                                                => (server == null? false : server.IsRunning && clients.Count > 0);

        public virtual bool IsServerRunning                                             => (server == null? false : server.IsRunning);

        public virtual bool IsConnected                                                 => clients.Count > 0;     

        public virtual CommunicationStates CommunicationState                           => communicationState;

        public virtual IReadOnlyList<MaterialStorageState> TowerStates                  => towerStates;

        public virtual MaterialStorageState TowerResponses                              => towerResponse;

        public virtual IReadOnlyDictionary<string, ReelTowerCommands> ReelTowerCommandList => reelTowerCommands;

        public virtual IReadOnlyDictionary<int, Pair<string, string>> ReelTowerIds => reelTowerIds;
        #endregion

        #region Events
        public virtual event EventHandler<CommunicationStates> CommunicationStateChanged;

        public virtual event EventHandler<string> ReportRuntimeLog;

        public virtual event EventHandler<string> ReportException;

        public virtual event EventHandler<MaterialPackage> ReceivedMaterialPackage;
        #endregion

        #region Costructors
        protected ReelTowerManager() { }
        #endregion

        #region Protected methods
        #region Dispose methods
        protected override void DisposeManagedObjects()
        {
            if (responseWatcher != null)
                responseWatcher.Stop();

            if (threadReelTowerStateWatcher != null)
                threadReelTowerStateWatcher.Join();

            ClearMessageQueue();

            // foreach (var obj in towerStates)
            //     obj.Dispose();
            // 
            // towerResponse.Dispose();

            clients.Clear();
            Singleton<MaterialPackageManager>.Instance.RemoveAllMaterialPackages();
            base.DisposeManagedObjects();
        }
        #endregion

        #region Received message pump methods
        protected void LoadProperties(string file = null)
        {
            // timeoutOfResponse = Config.TimeoutOfReelTowerResponse;
        }

        protected void Run()
        {
            Logger.Trace($"Started thread={ClassName}.{MethodBase.GetCurrentMethod().Name}");

            while (!stop)
            {
                if (serviceout || (App.ShutdownEvent as ManualResetEvent).WaitOne(10))
                {
                    Logger.Trace($"Stopped thread={MethodBase.GetCurrentMethod().Name}");
                    stop = true;
                }
                else
                {
                    if (receivedMessages.Count > 0)
                    {
                        ParseReceivedMessage();
                    }
                }
            }
        }
        #endregion

        #region Communication state change event methods
        protected virtual void FireCommunicationStateChanged(CommunicationStates state)
        {
            CommunicationStateChanged?.Invoke(this, communicationState = state);
        }
        #endregion

        #region Service event methods
        protected virtual void OnServiceStarted(object sender, EventArgs e)
        {
            FireCommunicationStateChanged(CommunicationStates.Listening);
            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}");

            responseWatcher = new System.Timers.Timer();
            responseWatcher.Interval = 100;
            responseWatcher.AutoReset = true;            
            responseWatcher.Elapsed += OnTickWatcher;
        }

        protected virtual void OnServiceStopped(object sender, EventArgs e)
        {
            FireCommunicationStateChanged(CommunicationStates.Disconnected);
            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}");
        }
        #endregion

        #region Client communication event methods
        protected virtual void OnAcceptClientConnection(object sender, AsyncSocketAcceptEventArgs e)
        {
            foreach (var client in clients)
            {
                IPEndPoint s1 = client.Value.AsyncSocket.Socket.RemoteEndPoint as IPEndPoint;
                IPEndPoint s2 = e.Worker.RemoteEndPoint as IPEndPoint;

                if (s1.Address.ToString().Contains(s2.Address.ToString()))
                    client.Value.AsyncSocket.Disconnect();
            }

            FireCommunicationStateChanged(CommunicationStates.Accepted);
            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={++clientId}");

            Thread.Sleep(100);
            AsyncSocketClient newClient = new AsyncSocketClient(clientId, e.Worker, OnClientConnect, OnClientSent, OnClientReceived);
            newClient.OnDisconnected    += new AsyncSocketDisconnectedEventHandler(OnClientDisconnect);
            newClient.OnError           += new AsyncSocketErrorEventHandler(OnClientError);

            lock (clients)
                clients.Add(clientId, new ReelTowerObject(newClient, clientId));
        }

        protected virtual void OnClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            try
            {
                FireCommunicationStateChanged(CommunicationStates.Error);
                FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.AsyncSocketException.Message}");

                ClearMessageQueue();
                ClearReelTowerResponse();
                receivedStateResponseFlag = 0;

                lock (clients)
                {
                    foreach (var obj in clients)
                        clients[obj.Key].AsyncSocket.Disconnect();

                    clients.Clear();
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }
        }

        protected virtual void OnClientConnect(object sender, AsyncSocketConnectionEventArgs e)
        {
            lastReceivedMessage = string.Empty;
            receivedStateResponseFlag = 0;
            FireCommunicationStateChanged(CommunicationStates.Connected);
            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.Id}");
        }

        protected virtual void OnClientDisconnect(object sender, AsyncSocketConnectionEventArgs e)
        {
            lock (clients)
            {
                if (clients.ContainsKey(e.Id))
                    clients.Remove(e.Id);
            }

            if (clients.Count <= 0)
                FireCommunicationStateChanged(CommunicationStates.Disconnected);

            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.Id}");
            lastReceivedMessage = string.Empty;
            receivedStateResponseFlag = 0;
        }

        protected virtual void OnTickWatcher(object sender, EventArgs e)
        {
            responseWatcher.Stop();

            if (IsConnected)
            {
                if (towerResponse.IsWaitResponse)
                {
                    int timeout_ = timeoutOfResponse;
                    
                    switch (towerResponse.Command)
                    {
                        case ReelTowerCommands.SEND_PICKING_LIST:
                        case ReelTowerCommands.REQUEST_LINK_TEST:
                        case ReelTowerCommands.REQUEST_TOWER_STATE:
                        case ReelTowerCommands.REQUEST_TOWER_STATE_ALL:
                        case ReelTowerCommands.REQUEST_BARCODEINFO_CONFIRM:
                        case ReelTowerCommands.REQUEST_REEL_LOAD_MOVE:
                        case ReelTowerCommands.REQUEST_REEL_LOAD_ASSIGN:
                        case ReelTowerCommands.REQUEST_LOAD_COMPLETE:
                        case ReelTowerCommands.REQUEST_UNLOAD_COMPLETE:
                        case ReelTowerCommands.REQUEST_REEL_UNLOAD_MOVE:
                        case ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN:
                        case ReelTowerCommands.REQUEST_LOAD_RESET:
                        case ReelTowerCommands.REQUEST_ALL_ALARM_RESET:
                        case ReelTowerCommands.REQUEST_ALL_LOAD_RESET:
                        case ReelTowerCommands.REQUEST_ALL_UNLOAD_RESET:
                        case ReelTowerCommands.REQUEST_UNLOAD_RESET:
                        case ReelTowerCommands.REPLY_LINK_TEST:
                        case ReelTowerCommands.REPLY_TOWER_STATE:
                        case ReelTowerCommands.REPLY_TOWER_STATE_ALL:
                        case ReelTowerCommands.REPLY_BARCODEINFO_CONFIRM:
                        case ReelTowerCommands.REPLY_REEL_LOAD_MOVE:
                        case ReelTowerCommands.REPLY_REEL_LOAD_ASSIGN:
                        case ReelTowerCommands.REPLY_REEL_UNLOAD_MOVE:
                        case ReelTowerCommands.REPLY_REEL_UNLOAD_ASSIGN:
                        case ReelTowerCommands.REPLY_LOAD_COMPLETE:
                        case ReelTowerCommands.REPLY_UNLOAD_COMPLETE:
                        case ReelTowerCommands.REPLY_LOAD_RESET:
                        case ReelTowerCommands.REPLY_UNLOAD_RESET:
                            timeout_ = timeoutOfResponse;
                            break;
                    }

                    if (IsResponseTimeout(timeout_))
                        ClearReelTowerResponse(true);
                }
            }

            responseWatcher.Start();
        }

        protected virtual void OnClientReceived(object sender, AsyncSocketReceiveEventArgs e)
        {
            receivedMessages.Enqueue(new Pair<int, string>(e.Id, new string(Encoding.Default.GetChars(e.ReceiveData))));
        }

        protected virtual void OnClientSent(object sender, AsyncSocketSendEventArgs e)
        {
        }
        #endregion

        #region Parse received message
        protected virtual bool ParseReelTowerState(string data, ref MaterialStorageState.StorageOperationStates state)
        {
            bool result_ = true;

            switch (data.ToLower())
            {
                case "unknown":
                    state = MaterialStorageState.StorageOperationStates.Unknown;
                    break;
                case "run":
                    state = MaterialStorageState.StorageOperationStates.Run;
                    break;
                case "down":
                    state = MaterialStorageState.StorageOperationStates.Down;
                    break;
                case "error":
                    state = MaterialStorageState.StorageOperationStates.Error;
                    break;
                case "load":
                    state = MaterialStorageState.StorageOperationStates.Load;
                    break;
                case "unload":
                    state = MaterialStorageState.StorageOperationStates.Unload;
                    break;
                case "wait":
                    state = MaterialStorageState.StorageOperationStates.Wait;
                    break;
                case "full":
                    state = MaterialStorageState.StorageOperationStates.Full;
                    break;
                case "idle":
                    state = MaterialStorageState.StorageOperationStates.Idle;
                    break;
                default:
                    result_ = false;
                    break;
            }

            return result_;
        }

        protected virtual void ParseReceivedMessage()
        {
            bool abortUnloadJob_                                    = false;
            int tagBegin_                                           = 0;
            int tagEnd_                                             = 0;
            int tagLength_                                          = 0;
            int outputStage_                                        = 0;
            int clientId_                                           = 0;
            int receivedTowerIndex_                                 = -1;
            int pickcount_                                          = 0;
            string receiveMessage_                                  = string.Empty;
            string messageType_                                     = string.Empty;
            string messageTagBegin_                                 = "<messageName>";
            string messageTagEnd_                                   = "</messageName>";
            string pickid_                                          = string.Empty;
            string targetloc_                                       = string.Empty;
            string receivedId_                                      = string.Empty;
            string[] messages_                                      = null;
            List<int> indices_                                      = new List<int>();
            List<List<string>> uids_                                = new List<List<string>>();
            Dictionary<int, Pair<string, string>> receivedStates_   = new Dictionary<int, Pair<string, string>>();
            ReelTowerCommands messageCommand_                       = ReelTowerCommands.REQUEST_TOWER_STATE;
            MaterialStorageState.StorageOperationStates state_      = MaterialStorageState.StorageOperationStates.Unknown;

            if (!receivedMessages.TryDequeue(out queuedMessage))
                return;

            clientId_ = queuedMessage.first;
            messages_ = queuedMessage.second.Split(new char[] { (char)AsciiControlCharacters.Etx }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (string message_ in messages_)
            {
                receiveMessage_ = message_.Replace("\0", string.Empty);

                if (receiveMessage_[0] == (char)AsciiControlCharacters.Stx)
                    receiveMessage_ = receiveMessage_.Substring(1, receiveMessage_.Length - 1);
                else
                {
                    Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} : Dumped packet = {receiveMessage_}");
                    continue;
                }

                try
                {
                    tagBegin_ = receiveMessage_.IndexOf(messageTagBegin_);
                    tagEnd_ = receiveMessage_.IndexOf(messageTagEnd_);
                    tagLength_ = messageTagBegin_.Length;
                    messageType_ = receiveMessage_.Substring(tagBegin_ + tagLength_, tagEnd_ - tagBegin_ - tagLength_);

                    if (reelTowerCommands.ContainsKey(messageType_))
                    {
                        if ((messageCommand_ = reelTowerCommands[messageType_]) == ReelTowerCommands.REQUEST_LINK_TEST)
                        {
                            ReplyPing();
                        }
                        else
                        {
                            receivedData.Clear();
                            receivedXml.LoadXml(receiveMessage_);
                            XmlNode root = receivedXml.SelectSingleNode("message");
                            XmlNode header = root.SelectSingleNode("//header");
                            XmlNode body = root.SelectSingleNode("//body");
                            XmlNode tail = root.SelectSingleNode("//return");

                            if (header != null)
                            {   // Parse header of message
                                receivedData.Name = header.SelectSingleNode("messageName").InnerText;
                                receivedData.TransactionID = header.SelectSingleNode("transactionId").InnerText;
                                receivedData.TimeStamp = header.SelectSingleNode("timeStamp").InnerText;
                            }
                            else
                                return;

                            if (body != null)
                            {   // Parse body of message
                                foreach (XmlNode element in body.ChildNodes)
                                {
                                    switch (element.Name)
                                    {
                                        case "MATERIALTOWER":
                                            {   // State of all towers
                                                foreach (XmlNode child in element.ChildNodes)
                                                {
                                                    switch (child.Name)
                                                    {
                                                        case "ID":
                                                            {
                                                                switch (receivedTowerIndex_ = Convert.ToInt32(child.InnerText[child.InnerText.Length - 1].ToString()))
                                                                {
                                                                    case 1:
                                                                    case 2:
                                                                    case 3:
                                                                    case 4:
                                                                        {
                                                                            receivedId_ = child.InnerText;
                                                                            receivedTowerIndex_ -= 1;

                                                                            if (!receivedStates_.ContainsKey(receivedTowerIndex_))
                                                                            {
                                                                                receivedStates_.Add(receivedTowerIndex_, new Pair<string, string>(receivedId_, string.Empty));
                                                                            }
                                                                            else
                                                                            {
                                                                                receivedStates_[receivedTowerIndex_].first = receivedId_;
                                                                                receivedStates_[receivedTowerIndex_].second = string.Empty;
                                                                            }
                                                                        }
                                                                        break;
                                                                    default:
                                                                        return;
                                                                }
                                                            }
                                                            break;
                                                        case "STATE":
                                                            {
                                                                if (!string.IsNullOrEmpty(receivedId_) && receivedTowerIndex_ >= 0)
                                                                {
                                                                    if (receivedStates_.ContainsKey(receivedTowerIndex_) && receivedStates_[receivedTowerIndex_].first == receivedId_)
                                                                    {
                                                                        receivedStates_[receivedTowerIndex_].second = child.InnerText;
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                        case "MATERIALTOWERID":
                                            {   // Single tower state report
                                                Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Received_MaterialTowerId> {element.InnerText}");

                                                if (string.IsNullOrEmpty(receivedData.TowerId = element.InnerText))
                                                    return;

                                                if (receivedData.TowerId.Contains(";"))
                                                {
                                                    string[] ids_ = receivedData.TowerId.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                                                    foreach (string id_ in ids_)
                                                    {
                                                        switch (receivedTowerIndex_ = Convert.ToInt32(id_[id_.Length - 1].ToString()))
                                                        {
                                                            case 1:
                                                            case 2:
                                                            case 3:
                                                            case 4:
                                                                {
                                                                    receivedId_ = id_;
                                                                    receivedTowerIndex_ -= 1;

                                                                    if (!receivedStates_.ContainsKey(receivedTowerIndex_))
                                                                    {
                                                                        receivedStates_.Add(receivedTowerIndex_, new Pair<string, string>(receivedId_, string.Empty));
                                                                    }
                                                                    else
                                                                    {
                                                                        receivedStates_[receivedTowerIndex_].first = receivedId_;
                                                                        receivedStates_[receivedTowerIndex_].second = string.Empty;
                                                                    }
                                                                }
                                                                break;
                                                            default:
                                                                return;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    switch (receivedTowerIndex_ = Convert.ToInt32(receivedData.TowerId[receivedData.TowerId.Length - 1].ToString()))
                                                    {
                                                        case 1:
                                                        case 2:
                                                        case 3:
                                                        case 4:
                                                            {
                                                                receivedId_ = receivedData.TowerId;
                                                                receivedTowerIndex_ -= 1;
                                                            }
                                                            break;
                                                        default:
                                                            return;
                                                    }
                                                }
                                            }
                                            break;
                                        case "MATERIALTOWERSTATE":
                                            {   // Single tower state report
                                                if (string.IsNullOrEmpty(receivedData.TowerState = element.InnerText))
                                                    return;

                                                if (receivedData.TowerState.Contains(";"))
                                                {
                                                    string[] states_ = receivedData.TowerState.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                                                    for (int i_ = 0; i_ < states_.Length; i_++)
                                                    {
                                                        receivedStates_[i_].second = states_[i_];
                                                    }
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(receivedId_) && receivedTowerIndex_ >= 0)
                                                    {
                                                        if (!receivedStates_.ContainsKey(receivedTowerIndex_))
                                                            receivedStates_.Add(receivedTowerIndex_, new Pair<string, string>(receivedId_, receivedData.TowerState));
                                                    }
                                                    else
                                                        return;
                                                }
                                            }
                                            break;
                                        case "UID":
                                            {
                                                receivedData.Data.Name = element.InnerText;
                                                // receivedData.Data.Text = element.InnerText;
                                            }
                                            break;
                                        case "LOADSTATE":
                                            {
                                                receivedData.LoadState = element.InnerText;
                                            }
                                            break;
                                        case "STAGE":
                                            {
                                                if (!string.IsNullOrEmpty(receivedData.OutputStage = element.InnerText))
                                                {   // S0101,S0102,S0103,S0104,S0105,S0106       need to add X0101
                                                    switch (outputStage_ = Convert.ToInt32(receivedData.OutputStage[receivedData.OutputStage.Length - 1].ToString()))
                                                    {
                                                        case 1:
                                                        case 2:
                                                        case 3:
                                                        case 4:
                                                        case 5:
                                                        case 6:
                                                            break;
                                                        default:
                                                            return;
                                                    }
                                                }
                                                else
                                                    return;
                                            }
                                            break;
                                        case "BARCODEINFO":
                                            {
                                                foreach (XmlNode child in element.ChildNodes)
                                                {
                                                    switch (child.Name)
                                                    {
                                                        case "SID":
                                                            receivedData.Data.Category = child.InnerText;
                                                            break;
                                                        case "LOTID":
                                                            receivedData.Data.LotId = child.InnerText;
                                                            break;
                                                        case "SUPPLIER":
                                                            receivedData.Data.Supplier = child.InnerText;
                                                            break;
                                                        case "QTY":
                                                            receivedData.Data.Quantity = int.Parse(child.InnerText);
                                                            break;
                                                        case "MFG":
                                                            receivedData.Data.ManufacturedDatetime = child.InnerText;
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                        case "PICK_ID":
                                            {
                                                pickid_ = element.InnerText;
                                            }
                                            break;
                                        case "TARGET_LOCATION":
                                            {
                                                targetloc_ = element.InnerText;
                                            }
                                            break;
                                        case "COUNT":
                                            {
                                                pickcount_ = int.Parse(element.InnerText);
                                            }
                                            break;
                                        case "UID_LIST":
                                            {
                                                string[] items_ = element.InnerText.Split(CONST_CARRIER_LIST_DELIMITER, StringSplitOptions.RemoveEmptyEntries);

                                                foreach (string temp_ in items_)
                                                {
                                                    string[] vals_ = temp_.Split(';');
                                                    List<string> temp = new List<string>();

                                                    for (int i = 0; i < vals_.Length; i++)
                                                        temp.Add(vals_[i]);

                                                    uids_.Add(temp);
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                                return;

                            if (tail != null)
                            {   // Parse tail of message
                                foreach (XmlNode element in tail.ChildNodes)
                                {
                                    switch (element.Name)
                                    {
                                        case "returnCode":
                                            receivedData.ReturnCode = element.InnerText;
                                            break;
                                        case "returnMessage":
                                            receivedData.ReturnMessage = element.InnerText;
                                            break;
                                    }
                                }
                            }
                            else
                                return;

                            switch (messageCommand_)
                            {
                                case ReelTowerCommands.SEND_PICKING_LIST:
                                    {   // Forced assignment to process material package from tower.
                                        receivedTowerIndex_ = 0;
                                    }
                                    break;
                                case ReelTowerCommands.REQUEST_LOAD_COMPLETE:
                                    break;
                                case ReelTowerCommands.REQUEST_UNLOAD_COMPLETE:
                                    {
                                        if (receivedTowerIndex_ < 0)
                                            return;
                                    }
                                    break;
                                case ReelTowerCommands.REPLY_TOWER_STATE:
                                    {   // Process response of single tower state report from tower interface.                                    
                                        if (receivedStates_.Count == 1 && receivedTowerIndex_ >= 0 && !string.IsNullOrEmpty(receivedData.TowerState))
                                        {
                                            if (ParseReelTowerState(receivedData.TowerState, ref state_))
                                            {
                                                lock (towerStates)
                                                {
                                                    if (receivedTowerIndex_ >= 0 && !string.IsNullOrEmpty(receivedId_) && towerStates[receivedTowerIndex_].Name == receivedId_)
                                                        towerStates[receivedTowerIndex_].State = state_;
                                                }

                                                Interlocked.Exchange(ref receivedStateResponseFlag, 2);
                                            }
                                            else
                                                return;
                                        }
                                        else if (receivedStates_.Count > 1)
                                        {
                                            if (receivedStates_.Count > 0)
                                            {
                                                foreach (var obj in receivedStates_)
                                                {
                                                    if (ParseReelTowerState(obj.Value.second, ref state_))
                                                    {
                                                        lock (towerStates)
                                                        {
                                                            if (obj.Key >= 0 && !string.IsNullOrEmpty(obj.Value.first) && towerStates[obj.Key].Name == obj.Value.first)
                                                                towerStates[obj.Key].State = state_;
                                                        }

                                                        Interlocked.Exchange(ref receivedStateResponseFlag, 2);
                                                    }
                                                    else
                                                        return;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Logger.Trace("Received state of tower report is not processed properly.");
                                            return;
                                        }
                                    }
                                    break;
                                case ReelTowerCommands.REPLY_TOWER_STATE_ALL:
                                    {   // Process response of all tower state report from tower interface.
                                        if (receivedStates_.Count > 0)
                                        {
                                            foreach (var obj in receivedStates_)
                                            {
                                                if (ParseReelTowerState(obj.Value.second, ref state_))
                                                {
                                                    lock (towerStates)
                                                    {
                                                        if (obj.Key - 1 >= 0 && !string.IsNullOrEmpty(obj.Value.first) && towerStates[obj.Key - 1].Name == obj.Value.first)
                                                            towerStates[obj.Key - 1].State = state_;
                                                    }

                                                    Interlocked.Exchange(ref receivedStateResponseFlag, 2);
                                                }
                                                else
                                                    return;
                                            }
                                        }
                                        else
                                        {
                                            Logger.Trace("Received state of tower report is not processed properly.");
                                            return;
                                        }
                                    }
                                    break;
                                case ReelTowerCommands.REPLY_REEL_LOAD_MOVE:
                                case ReelTowerCommands.REPLY_BARCODEINFO_CONFIRM:
                                case ReelTowerCommands.REPLY_REEL_LOAD_ASSIGN:
                                    {
                                        if (towerResponse.IsWaitResponse)
                                        {
                                            switch (towerResponse.Command)
                                            {
                                                case ReelTowerCommands.REQUEST_REEL_LOAD_MOVE:
                                                case ReelTowerCommands.REQUEST_BARCODEINFO_CONFIRM:
                                                case ReelTowerCommands.REQUEST_REEL_LOAD_ASSIGN:
                                                    {
                                                        lock (towerResponse)
                                                        {
                                                            towerResponse.UpdateResponse(messageCommand_,
                                                                receivedData,
                                                                outputStage_);
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            if (messageCommand_ == ReelTowerCommands.REQUEST_REEL_LOAD_ASSIGN)
                                                towerResponse.UpdateResponse(messageCommand_,
                                                    receivedData,
                                                    outputStage_);
                                        }
                                    }
                                    break;
                                case ReelTowerCommands.REPLY_REEL_UNLOAD_MOVE:
                                case ReelTowerCommands.REPLY_REEL_UNLOAD_ASSIGN:
                                    {
                                        if (towerResponse.IsWaitResponse)
                                        {
                                            switch (towerResponse.Command)
                                            {
                                                case ReelTowerCommands.REQUEST_REEL_UNLOAD_MOVE:
                                                case ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN:
                                                    {
                                                        lock (towerResponse)
                                                        {
                                                            towerResponse.UpdateResponse(messageCommand_,
                                                                receivedData,
                                                                outputStage_);
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            if (messageCommand_ == ReelTowerCommands.REPLY_REEL_UNLOAD_ASSIGN)
                                                towerResponse.UpdateResponse(messageCommand_,
                                                    receivedData,
                                                    outputStage_);
                                        }
                                    }
                                    break;
                                // UPDATED: 20200316 (Marcus)
                                // Don't use REQUEST_ALL_UNLOAD_RESET.
                                case ReelTowerCommands.REQUEST_ALL_UNLOAD_RESET:
                                case ReelTowerCommands.REPLY_LOAD_RESET:
                                case ReelTowerCommands.REPLY_UNLOAD_RESET:
                                    {
                                        if (towerResponse.IsWaitResponse)
                                        {
                                            switch (towerResponse.Command)
                                            {
                                                case ReelTowerCommands.REQUEST_LOAD_RESET:
                                                case ReelTowerCommands.REQUEST_UNLOAD_RESET:
                                                    {
                                                        lock (towerResponse)
                                                        {
                                                            towerResponse.UpdateResponse(messageCommand_,
                                                                receivedData,
                                                                outputStage_);
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    {
                                                        if (messageCommand_ == TechFloor.ReelTowerCommands.REQUEST_ALL_UNLOAD_RESET)
                                                            abortUnloadJob_ = true;
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                            abortUnloadJob_ = true;
                                    }
                                    break;
                            }

                            if ((messageCommand_ != ReelTowerCommands.REPLY_TOWER_STATE &&
                                messageCommand_ != ReelTowerCommands.REPLY_TOWER_STATE_ALL) ||
                                lastReceivedMessage != receivedData.TowerState)
                            {
                                responseWatcher.Stop();
                                lastReceivedMessage = receivedData.TowerState;
                                FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={receiveMessage_}");
                            }

                            if (receivedTowerIndex_ >= 0)
                            {
                                switch (messageCommand_)
                                {
                                    case ReelTowerCommands.REQUEST_LOAD_COMPLETE:
                                        {
                                            SendTowerMessage(ReelTowerCommands.REPLY_LOAD_COMPLETE, receivedData.Data, towerStates[receivedTowerIndex_].Name);
                                        }
                                        break;
                                    case ReelTowerCommands.REQUEST_UNLOAD_COMPLETE:
                                        {
                                            bool result_ = true;

                                            // Add a request to unload reel from tower
                                            lock (unloadRequests)
                                            {
                                                if (receivedTowerIndex_ >= 0 && receivedData.Data != null && !string.IsNullOrEmpty(receivedId_) && towerStates[receivedTowerIndex_].Name == receivedId_)
                                                {
                                                    if (unloadRequests.Find(x => x.PendingData.Name == receivedData.Data.Name) == null)
                                                    {
                                                        unloadRequests.Add(new MaterialStorageState(receivedTowerIndex_ + 1, receivedId_, receivedData, outputStage_));
                                                        Logger.Trace($"Added a new material (TOWER{receivedTowerIndex_ + 1},{receivedData.Data.Name})");
                                                        Debug.WriteLine($"Added a new material (TOWER{receivedTowerIndex_ + 1},{receivedData.Data.Name})");
                                                        result_ = true;
                                                    }
                                                    else if (receivedData.Data == null)
                                                    {
                                                        Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}=receivedData.Data is empty");
                                                    }
                                                    else if ((unloadRequests.Find(x => x.PendingData.Name == receivedData.Data.Name) != null))
                                                    {
                                                        Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}=Duplicated RID");
                                                    }
                                                }
                                                else if (towerStates[receivedTowerIndex_].Name != receivedId_)
                                                {
                                                    Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}=Tower States don't Match");
                                                }
                                                else if (string.IsNullOrEmpty(receivedId_))
                                                {
                                                    Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}=Received Tower ID is empty");
                                                }
                                                else if (receivedTowerIndex_ < 0)
                                                {
                                                    Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}=Received Tower ID is less than zero");
                                                }
                                            }

                                            if (result_)
                                                SendTowerMessage(ReelTowerCommands.REPLY_UNLOAD_COMPLETE, receivedData.Data, towerStates[receivedTowerIndex_].Name);
                                        }
                                        break;
                                    case ReelTowerCommands.SEND_PICKING_LIST:
                                        {
                                            if (!string.IsNullOrEmpty(pickid_) &&
                                                !string.IsNullOrEmpty(targetloc_) &&
                                                pickcount_ > 0 &&
                                                uids_.Count == pickcount_)
                                            {
                                                FireReceivedMaterialPackager(new MaterialPackage(pickid_, targetloc_, pickcount_, uids_));
                                            }
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                if (abortUnloadJob_)
                                {
                                    // ClearAllReelTowerStates();
                                    // Only reply about REQUEST_ALL_UNLOAD_RESET.
                                    SendTowerMessage(TechFloor.ReelTowerCommands.REPLY_ALL_UNLOAD_RESET);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    FireReportException($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={ex.Message}");
                }
            }
        }
        #endregion

        #region Send message
        protected virtual void SendSocketData(string data)
        {
            foreach (var client in clients)
                client.Value.Send(data);
        }
        #endregion

        #region Message assembly methods
        protected virtual void MakeXmlHeader(ref XmlDocument xml, ref XmlNode root, string messageName)
        {
            string transactionID    = DateTime.Now.ToString("yyyyMMddhhmmssff");
            string timeStamp        = DateTime.Now.ToString("yyyyMMddhhmmss");

            XmlNode nodeHeader      = xml.CreateElement("header");
            AddXmlItem(xml, nodeHeader, "messageName", messageName);
            AddXmlItem(xml, nodeHeader, "transactionId", transactionID);
            AddXmlItem(xml, nodeHeader, "timeStamp", timeStamp);
            root.AppendChild(nodeHeader);
        }

        protected virtual void AddXmlItem(XmlDocument xml, XmlNode parentsNode, string key, string value)
        {
            XmlNode node    = xml.CreateElement(key);
            node.InnerText  = value;
            parentsNode.AppendChild(node);
        }

        protected virtual void AddXmlOfBarcode(ref XmlDocument xml, ref XmlNode body, MaterialData barcode)
        {
            XmlNode node = xml.CreateElement("BARCODEINFO");
            AddXmlItem(xml, node, "SID", barcode.Category);
            AddXmlItem(xml, node, "LOTID", barcode.LotId);
            AddXmlItem(xml, node, "SUPPLIER", barcode.Supplier);
            AddXmlItem(xml, node, "QTY", barcode.Quantity.ToString());
            AddXmlItem(xml, node, "MFG", barcode.ManufacturedDatetime);
            AddXmlItem(xml, node, "LOADTYPE", barcode.LoadType.ToString().ToUpper());
            AddXmlItem(xml, node, "SIZE", barcode.ReelType == ReelDiameters.ReelDiameter7? "7" : "13");
            body.AppendChild(node);
        }
        #endregion

        #region Fire received material package
        protected virtual void FireReceivedMaterialPackager(MaterialPackage pkg)
        {
            if (pkg != null)
                ReceivedMaterialPackage?.Invoke(this, pkg);
        }
        #endregion
        #endregion

        #region Public methods
        #region Element create methods
        public virtual void Start(int portno)
        {
            LoadProperties();

            serviceout = false;            
            StartServer(portno);

            if (receivedData.Data == null)
                receivedData.CreateData();

            FireCommunicationStateChanged(CommunicationStates.Listening);
            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}");
            (threadReelTowerStateWatcher = new Thread(new ThreadStart(Run)))?.Start();
        }

        public virtual void Stop()
        {
            serviceout = true;
            stop = false;
            threadReelTowerStateWatcher.Join();
            StopServer();
        }
        #endregion

        #region Server service control methods
        public virtual void StartServer(int portno = 0)
        {
            if (server == null)
            {
                server = new AsyncSocketServer(portno);
                server.OnAccepted += new AsyncSocketAcceptedEventHandler(OnAcceptClientConnection);
                server.OnError += new AsyncSocketErrorEventHandler(OnClientError);
                server.OnServiceStarted += OnServiceStarted;
                server.OnServiceStopped += OnServiceStopped;
            }

            server.Start();
        }

        public virtual void StopServer(bool forced = false)
        {
            server.Stop(forced);
            server.OnAccepted -= new AsyncSocketAcceptedEventHandler(OnAcceptClientConnection);
            server.OnError -= new AsyncSocketErrorEventHandler(OnClientError);
            server.OnServiceStarted -= OnServiceStarted;
            server.OnServiceStopped -= OnServiceStopped;            
            server = null;

            if (towerResponse.IsWaitResponse)
            {
                lock (towerResponse)
                {
                    if (towerResponse.Command == ReelTowerCommands.REQUEST_REEL_LOAD_MOVE)
                    {
                        towerResponse.Clear();
                        towerResponse.Reset();
                    }
                }
            }
        }
        #endregion

        #region Event notification method
        public virtual void FireCommunicationStateChanged()
        {
            CommunicationStateChanged?.Invoke(this, CommunicationState);
        }

        public virtual void FireReportRuntimeLog(string text = null)
        {
            ReportRuntimeLog?.Invoke(this, text);
        }

        public virtual void FireReportException(string text = null)
        {
            ReportException?.Invoke(this, text);
        }
        #endregion

        #region Unload request check methods
        public virtual bool HasUnloadRequest()
        {
            return (unloadRequests.Find(x => x.IsAssignedToUnload) != null);
        }

        public virtual int GetUnloadRequest(ref MaterialStorageState obj)
        {   // Take the oldest reel assignment information to unload
            if (unloadRequests.Count > 0)
            {
                obj.CopyFrom(unloadRequests[0]);
                return obj.Index;
            }
            return -1;
        }

        // Not use
        public virtual int GetOldestUnloadRequest(ref MaterialStorageState obj)
        {   // Take the oldest reel assignment information to unload
            int oldest_ = -1;
            TimeSpan age_ = TimeSpan.Zero;

            for (int i = 0; i < towerStates.Count; i++)
            {
                MaterialStorageState item = towerStates[i];

                if (item.IsAssignedToUnload && DateTime.Now - item.RequestTime >= age_)
                {
                    age_ = DateTime.Now - item.RequestTime;
                    oldest_ = i;
                }
            }

            if (oldest_ >= 0)
            {
                obj.CopyFrom(towerStates[oldest_]);
                return obj.Index;
            }

            return oldest_;
        }
        #endregion

        #region Clear reel tower state methods after robot handling
        public virtual void ClearAllReelTowerStates()
        {
            lock (unloadRequests)
            {
                // foreach (var obj in unloadRequests)
                //     obj.Dispose();

                unloadRequests.Clear();
            }
        }

        public virtual void ClearReelTowerStates(string towerid, string uid)
        {
            if (string.IsNullOrEmpty(towerid))
            {
                foreach (var obj in towerStates)
                    obj.Clear();
            }
            else
            {
                lock (unloadRequests)
                {
                    foreach (var obj in unloadRequests)
                    {
                        if (obj.Name == towerid && obj.Uid == uid)
                        {
                            // obj.Dispose();
                            unloadRequests.Remove(obj);
                            break;
                        }
                    }
                }
            }
        }

        // Not use
        public virtual void ClearReelTowerStates(string towerid = null)
        {
            if (string.IsNullOrEmpty(towerid))
            {
                foreach (var obj in towerStates)
                    obj.Clear();
            }
            else
            {
                lock (towerStates)
                {
                    foreach (var obj in towerStates)
                    {
                        if (obj.Name == towerid)
                        {
                            obj.Clear();
                            break;
                        }
                    }
                }
            }
        }

        public virtual void ClearMessageQueue()
        {
            Pair<int, string> meesage_ = new Pair<int, string>(0, string.Empty);

            while (!receivedMessages.IsEmpty)
                receivedMessages.TryDequeue(out meesage_);
        }
        #endregion

        #region Clear response message state
        public virtual void ClearReelTowerResponse(bool resetflag = false)
        {
            lock (towerResponse)
            {
                towerResponse.Clear();
                if (resetflag)
                    towerResponse.Reset();
            }
        }
        #endregion

        #region Loadable state check methods
        public virtual bool IsPossibleLoadReel(ref MaterialStorageState obj)
        {
            bool result_ = false;

            if (HasUnloadRequest())
                return false;

            lock (towerStates)
            {   // Rotate load tower index
                if (lastLoadedTower == 4)
                    lastLoadedTower = 0;

                loadAvailableTowers.Clear();

                foreach (MaterialStorageState towerState_ in towerStates)
                {
                    if (towerState_.State == MaterialStorageState.StorageOperationStates.Idle)
                    {
                        if (towerState_.Index > lastLoadedTower && towerState_.Index >= lastLoadedTower + 1)
                        {
                            lastLoadedTower = towerState_.Index;
                            obj.CopyFrom(towerStates[lastLoadedTower - 1]);
                            result_ = true;
                            break;
                        }

                        loadAvailableTowers.Add(towerState_.Index);
                    }
                }

                if (!result_ && loadAvailableTowers.Count > 0)
                {
                    loadAvailableTowers.Sort();
                    lastLoadedTower = loadAvailableTowers[0];
                    obj.CopyFrom(towerStates[lastLoadedTower - 1]);
                    result_ = true;
                }

                // Initialize
                foreach (MaterialStorageState state in towerStates)
                    state.InitData();
            }

            return result_;
        }
        #endregion

        #region Response check methods
        public virtual bool IsReceivedStateResponse(ref bool timedout)
        {
            bool result_ = false;
            timedout = false;

            if (receivedStateResponseFlag >= 2)
            {
                Interlocked.Exchange(ref receivedStateResponseFlag, 0);
                result_ = true;
            }
            else if (TimeSpan.FromMilliseconds(App.TickCount - statePollingTick).TotalMilliseconds >= timeoutOfResponse)
            {
                Interlocked.Exchange(ref receivedStateResponseFlag, 0);
                timedout = true;
            }

            return result_;
        }

        public virtual bool IsReceivedResponse()
        {
            return (towerResponse.IsReceived);
        }

        public virtual bool IsFailure()
        {
            return (towerResponse.IsFailure);
        }

        public virtual bool IsResponseTimeout(int timeout = 10000)
        {
            return (towerResponse.IsResponseTimeout(timeout));
        }

        public virtual void ResetResponse()
        {
            towerResponse.Clear();
        }

        public virtual void IgnoreResponse()
        {
            ClearReelTowerResponse();
            ResetResponse();
        }

        public virtual bool GetReceivedResponse(ref string rc)
        {
            bool result_ = false;

            try
            {
                if (towerResponse.IsReceived)
                {
                    if (!string.IsNullOrEmpty(towerResponse.ReturnCode))
                    {
                        rc = towerResponse.ReturnCode;
                        result_ = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }

            return result_;
        }

        public virtual bool GetReceivedResponse(ref MaterialStorageState obj)
        {
            if (towerResponse.IsReceived)
            {
                obj.CopyFrom(towerResponse);
                ClearReelTowerResponse();
                return true;
            }

            return false;
        }

        public virtual bool IsValidMaterialData(MaterialStorageState obj, MaterialStorageState response, MaterialData src)
        {
            bool result_ = false;

            try
            {
                if (obj.Name == response.Name)
                {
                    if (response.ReturnCode == "0" &&
                        (response.Sid == src.Category ||
                        response.LotId == src.LotId))
                        result_ = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }

            return result_;
        }

        public virtual bool IsValidMaterialData(MaterialStorageState obj, MaterialData src)
        {
            bool result_ = false;

            try
            {
                if (towerResponse.Name == obj.Name)
                {
                    if (towerResponse.ReturnCode == "0" &&
                        (towerResponse.Sid == src.Category ||
                        towerResponse.LotId == src.LotId))
                        result_ = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }

            return result_;
        }
        #endregion

        #region Send message methods
        public virtual void ReplyPing(string towerid = null)
        {
            SendTowerMessage(ReelTowerCommands.REPLY_LINK_TEST);
        }

        public virtual bool QueryStates(int towerindex = 0, bool prefix = false)
        {   // Check which reel tower is available to load a reel.
            if (HasUnloadRequest())
                return false;
            else
            {
                string towerid_ = string.Empty;
                switch (towerindex)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        towerid_ = ReelTowerIds[towerindex].second;
                        break;
                }

                return SendTowerMessage(ReelTowerCommands.REQUEST_TOWER_STATE,
                    null,
                    string.IsNullOrEmpty(towerid_) ? CONST_TOWER_IDS : towerid_,
                    prefix);
            }
        }

        public virtual bool QueryStatesAll(bool prefix = false)
        {   // Check which reel tower is available to load a reel.
            if (HasUnloadRequest())
                return false;
            else
            {
                string towerid_ = string.Empty;

                foreach (Pair<string, string> value_ in ReelTowerIds.Values)
                    towerid_ += string.Format($"{value_.second};");

                towerid_ = towerid_.Remove(towerid_.Length - 1, 1);
                return SendTowerMessage(ReelTowerCommands.REQUEST_TOWER_STATE, null, towerid_, prefix);
                // return SendTowerMessage(ReelTowerCommands.REQUEST_TOWER_STATE_ALL, null, null, prefix);
            }
        }

        public virtual bool RequestReelUnloadAssign(string towerid, MaterialData barcode)
        {   // Send reel unload assign to reel tower
            if (SendTowerMessage(ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN,
                barcode,
                towerid))
            {
                // Clear reel unload tower state
                ClearReelTowerStates(towerid);
                return true;
            }

            return false;
        }

        public virtual bool SendTowerMessage(ReelTowerCommands command, MaterialData barcode = null, string towerid = null, bool prefix = true, string code = "0", string message = "done")
        {
            if (communicationState != CommunicationStates.Connected || (towerResponse.IsWaitResponse && command < ReelTowerCommands.REQUEST_REEL_LOAD_MOVE))
            {
                if (lastCommunicationStateLog != $"{ClassName}.{MethodBase.GetCurrentMethod().Name}=Send Tower Message returned false, Waitresponse={towerResponse.IsWaitResponse}, command = {command}, CommunicationStates = {communicationState}")
                    FireReportRuntimeLog(lastCommunicationStateLog = $"{ClassName}.{MethodBase.GetCurrentMethod().Name}=Send Tower Message returned false, Waitresponse={towerResponse.IsWaitResponse}, command = {command}, CommunicationStates = {communicationState}");
                return false;
            }

            bool printout_      = true;
            bool waitresponse_  = true;
            XmlDocument xml     = new XmlDocument();
            XmlNode header      = xml.CreateElement("message");
            XmlNode body        = xml.CreateElement("body");
            XmlNode tail        = xml.CreateElement("return");

            if (string.IsNullOrEmpty(code))
                code = "0";

            switch (command)
            {
                case ReelTowerCommands.REPLY_LINK_TEST:
                    {
                        waitresponse_ = false;
                    }
                    break;
                case ReelTowerCommands.REQUEST_TOWER_STATE:
                    {
                        printout_ = waitresponse_ = false;
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                    }
                    break;
                case ReelTowerCommands.REQUEST_TOWER_STATE_ALL:
                    {
                        printout_ = waitresponse_ = false;
                    }
                    break;
                case ReelTowerCommands.REQUEST_LOAD_COMPLETE:
                    {
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                        AddXmlItem(xml, body, "UID", barcode.Name);
                    }
                    break;
                case ReelTowerCommands.REQUEST_BARCODEINFO_CONFIRM:
                    {   // 투입되는 Reel의 Unit ID는 아래와 같은 조합으로 매번 생성하여 Reel Reel Tower에 넘겨준다
                        // barcode.UID = barcode.Sid + barcode.Maker + DateTime.Now.ToString("hhmmss");
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                        AddXmlItem(xml, body, "UID", barcode.Name);
                        AddXmlOfBarcode(ref xml, ref body, barcode);
                    }
                    break;
                case ReelTowerCommands.REQUEST_REEL_LOAD_MOVE:
                    {
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                        AddXmlItem(xml, body, "LOADSTATE", "LOAD");
                        AddXmlItem(xml, body, "UID", barcode.Name);
                    }
                    break;
                case ReelTowerCommands.REQUEST_REEL_UNLOAD_MOVE:
                    {
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                        AddXmlItem(xml, body, "LOADSTATE", "UNLOAD");
                        AddXmlItem(xml, body, "UID", barcode.Name);
                        AddXmlItem(xml, tail, "returnCode", code);
                        AddXmlItem(xml, tail, "returnMessage", message);
                    }
                    break;
                case ReelTowerCommands.REQUEST_REEL_LOAD_ASSIGN:
                case ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN:
                    {
                        waitresponse_ = false;
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                        AddXmlItem(xml, body, "UID", barcode.Name);
                    }
                    break;
                case ReelTowerCommands.REPLY_LOAD_COMPLETE:
                case ReelTowerCommands.REPLY_UNLOAD_COMPLETE:
                    {
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                        AddXmlItem(xml, body, "UID", barcode.Name);
                        AddXmlItem(xml, tail, "returnCode", code);
                        AddXmlItem(xml, tail, "returnMessage", message);
                        waitresponse_ = false;
                    }
                    break;
                case ReelTowerCommands.REQUEST_LOAD_RESET:
                    {
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                    }
                    break;
                case ReelTowerCommands.REQUEST_UNLOAD_RESET:
                    {
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                    }
                    break;
                case ReelTowerCommands.REQUEST_ALL_LOAD_RESET:
                    {
                        waitresponse_ = false;
                    }
                    break;
                case ReelTowerCommands.REQUEST_ALL_UNLOAD_RESET:
                    {
                        waitresponse_ = false;
                    }
                    break;
                case ReelTowerCommands.REQUEST_ALL_ALARM_RESET:
                    {
                        waitresponse_ = false;
                    }
                    break;
                default:
                    return false;
            }

            MakeXmlHeader(ref xml, ref header, command.ToString());
            header.AppendChild(body);
            header.AppendChild(tail);
            xml.AppendChild(header);

            if (prefix)
                SendSocketData(string.Concat((char)AsciiControlCharacters.Stx, xml.OuterXml, (char)AsciiControlCharacters.Etx));
            else
                SendSocketData(xml.OuterXml);

            if (printout_)
                FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={xml.OuterXml}");

            if (waitresponse_)
            {
                ClearReelTowerResponse();
                towerResponse.SetCommand(command, waitresponse_);

                switch (command)
                {
                    case ReelTowerCommands.REQUEST_REEL_LOAD_MOVE:
                        {
                            requiredLoadCancelIfFailure = true;
                            lastRequestedLoadTowerId = towerid;
                        }
                        break;
                    // case ReelTowerCommands.REQUEST_LOAD_RESET:
                    // case ReelTowerCommands.REQUEST_ALL_LOAD_RESET:
                    // case ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN:
                    // case ReelTowerCommands.REQUEST_REEL_UNLOAD_MOVE:
                    //     {
                    //         requiredLoadCancelIfFailure = false;
                    //         lastRequestedLoadTowerId = string.Empty;
                     //     }
                    //     break;
                }

                if (!responseWatcher.Enabled)
                    responseWatcher.Start();
            }
            else
            {
                switch (command)
                {
                    case ReelTowerCommands.REQUEST_TOWER_STATE:
                    case ReelTowerCommands.REQUEST_TOWER_STATE_ALL:
                        {
                            statePollingTick = App.TickCount;
                            Interlocked.Exchange(ref receivedStateResponseFlag, 1);
                        }
                        break;
                }
            }

            return true;
        }
        #endregion

        #region Automatic reel load reset
        public virtual void SetReelLoadReset()
        {
            if (requiredLoadCancelIfFailure)
            {
                responseWatcher.Stop();

                if (!string.IsNullOrEmpty(lastRequestedLoadTowerId))
                {
                    ClearReelTowerResponse(true);
                    SendReelLoadReset();
                }

                requiredLoadCancelIfFailure = false;
                lastRequestedLoadTowerId = string.Empty;
            }
        }

        public virtual void SendReelLoadReset()
        {
            SendTowerMessage(ReelTowerCommands.REQUEST_ALL_LOAD_RESET, null, lastRequestedLoadTowerId);
        }
        #endregion

        public virtual void RestartReelTowerManager(int portno)
        {
            Stop();
            Start(portno);
        }

        public virtual void ResetResponseTimeout()
        {
            ClearMessageQueue();
            ClearReelTowerResponse(true);
        }

        public virtual void Init(IReadOnlyDictionary<int, Pair<string, string>> towerids)
        {
            reelTowerIds.Clear();
            foreach (KeyValuePair<int, Pair<string, string>> item_ in towerids)
            {
                reelTowerIds.Add(item_.Key, item_.Value);
                towerStates.Add(new MaterialStorageState(item_.Key, item_.Value.first, item_.Value.second));
            }
        }
        #endregion
    }
}
#endregion