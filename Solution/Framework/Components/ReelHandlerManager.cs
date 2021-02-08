#region Imports
using TechFloor.Device.CommunicationIo;
using TechFloor.Object;
using TechFloor.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
#endregion

#region Program
#pragma warning disable CS0067
namespace TechFloor.Components
{
    public class ReelHandlerManager : SimulatableDevice
    {
        #region Constants
        protected const string CONST_TOWER_IDS                      = "T0101;T0102;T0103;T0104;";

        protected readonly char[] CONST_MATERIAL_DELIMETERS         = { ';' };

        protected readonly char[] CONST_CARRIER_LIST_DELIMITER      = { '|' };
        #endregion

        #region Fields
        protected bool stop                                                                 = false;

        protected bool requiredLoadCancelIfFailure                                          = false;

        protected bool serviceout                                                           = false;

        protected bool responseFailure                                                      = false;

        protected int reconnectCount                                                        = 0;

        protected int receivedStateResponseFlag                                             = 0;

        protected int statePollingTick                                                      = 0;

        protected int clientId                                                              = 0;

        protected int lastLoadedTower                                                       = 0;

        protected int receivedTowerIdOfState                                                = -1;

        protected int receivedTowerIdOfResponse                                             = -1;

        protected int timeoutOfResponse                                                     = 3000;

        protected int intervalOfPing                                                        = 1000;

        protected int limitOfRetry                                                          = 3;

        protected string currentLoadTowerId                                                 = string.Empty;

        protected string currentUnloadTowerId                                               = string.Empty;

        protected string lastRequestedLoadTowerId                                           = string.Empty;

        protected string lastReceivedMessage                                                = string.Empty;

        protected string lastCommunicationStateLog                                          = string.Empty;

        protected XmlDocument receivedXml                                                   = new XmlDocument();

        protected MaterialStorageMessage receivedData                                       = new MaterialStorageMessage();

        protected MaterialData receivedBarcode                                              = new MaterialData();

        protected Pair<int, string> queuedMessage                                           = new Pair<int, string>(0, string.Empty);

        protected Thread threadReelHandlerStateWatcher                                      = null;

        protected Dictionary<int, ReelTowerObject> clients                                  = new Dictionary<int, ReelTowerObject>();

        protected ConcurrentQueue<ReelTowerMessage> messageQueue                            = new ConcurrentQueue<ReelTowerMessage>();

        protected ConcurrentQueue<Pair<int, string>> receivedMessages                       = new ConcurrentQueue<Pair<int, string>>();

        protected List<int> loadAvailableTowers                                             = new List<int>();

        protected List<MaterialStorageState> unloadRequests                                 = new List<MaterialStorageState>();

        protected List<MaterialStorageState> towerStates                                    = new List<MaterialStorageState>();

        protected Dictionary<string, ReelTowerCommands> reelTowerCommands                   = new Dictionary<string, ReelTowerCommands>()
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
            { "REQUEST_CART_LOAD_FINISH",       ReelTowerCommands.REQUEST_CART_LOAD_FINISH },
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
            { "REPLY_ALL_LOAD_RESET",           ReelTowerCommands.REPLY_ALL_LOAD_RESET },
            { "REPLY_ALL_UNLOAD_RESET",         ReelTowerCommands.REPLY_ALL_UNLOAD_RESET },
            { "REPLY_ALL_ALARM_RESET",          ReelTowerCommands.REPLY_ALL_ALARM_RESET },
            { "REQUEST_ALL_STOP",               ReelTowerCommands.REQUEST_ALL_STOP },
            { "REPLY_ALL_STOP",                 ReelTowerCommands.REPLY_ALL_STOP },
            { "REPORT_SET_ALARM",               ReelTowerCommands.REPORT_SET_ALARM },
            { "REPORT_CLEAR_ALARM",             ReelTowerCommands.REPORT_CLEAR_ALARM },
        };

        protected IPEndPoint endPoint                                                       = null;

        protected AsyncSocketClient client                                                  = null;

        protected CommunicationStates communicationState                                    = CommunicationStates.None;

        protected System.Timers.Timer responseWatcher                                       = null;

        protected DateTime lastPingDatetime                                                 = DateTime.Now;

        protected RobotActionStates actionStates                                            = RobotActionStates.Unknown;

        protected List<MaterialStoragePacket> sentPackets                                   = new List<MaterialStoragePacket>();
        #endregion

        #region Properties
        public virtual int TimeoutOfTowerResponse
        {
            get => timeoutOfResponse;
            set => timeoutOfResponse = value;
        }

        public virtual int IntervalOfPing
        {
            get => intervalOfPing;
            set => intervalOfPing = value;
        }

        public virtual int LimitOfRetry
        {
            get => limitOfRetry;
            set => limitOfRetry = value;
        }

        public virtual bool IsConnected                                                     => (client == null ? false : client.IsConnected);     

        public virtual CommunicationStates CommunicationState                               => communicationState;

        public virtual IReadOnlyList<MaterialStorageState> TowerStates                      => towerStates;

        public virtual IReadOnlyDictionary<string, ReelTowerCommands> ReelTowerCommandList  => reelTowerCommands;

        public virtual RobotActionStates ActionState
        {
            get => actionStates;
            set => actionStates = value;
        }

        public virtual bool IsFailure                                                       => responseFailure;
        #endregion

        #region Events
        public event EventHandler<CommunicationStates> CommunicationStateChanged;

        public event EventHandler<string> ReportRuntimeLog;

        public event EventHandler<string> ReportException;

        public event EventHandler<MaterialPackage> ReceivedMaterialPackage;

        public event EventHandler<Pair<TechFloor.ReelTowerCommands, MaterialStorageMessage>> RequestCommandReceived;
        #endregion

        #region Costructors
        protected ReelHandlerManager() { }
        #endregion

        #region Protected methods
        #region Dispose methods
        protected override void DisposeManagedObjects()
        {
            if (responseWatcher != null)
                responseWatcher.Stop();

            if (threadReelHandlerStateWatcher != null)
                threadReelHandlerStateWatcher.Join();

            responseFailure = false;
            clients.Clear();
            ClearAllWaitMessages();
            base.DisposeManagedObjects();
        }
        #endregion

        #region Received message pump methods
        protected void LoadProperties()
        {
            // timeoutOfResponse = Config.TimeoutOfReelTowerResponse;
        }

        protected void Run()
        {
            string message_ = string.Empty;
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
                       ParseReceivedMessage();
                }
            }
        }
        #endregion

        #region Lost message and retry send message
        protected virtual void RemoveSentMessage(ReelTowerCommands command, string towerid)
        {
            try
            {
                ReelTowerCommands sentcommand_ = ReelTowerCommands.REQUEST_LINK_TEST;

                switch (command)
                {
                    case ReelTowerCommands.REQUEST_UNLOAD_COMPLETE:
                    case ReelTowerCommands.REPLY_UNLOAD_COMPLETE:
                        sentcommand_ = ReelTowerCommands.REQUEST_UNLOAD_COMPLETE;
                        break;
                    case ReelTowerCommands.REQUEST_ALL_UNLOAD_RESET:
                    case ReelTowerCommands.REPLY_ALL_UNLOAD_RESET:
                        sentcommand_ = ReelTowerCommands.REQUEST_ALL_UNLOAD_RESET;
                        break;
                }

                if (sentcommand_ != ReelTowerCommands.REQUEST_LINK_TEST)
                {
                    MaterialStoragePacket packet_ = sentPackets.Find(x_ => string.IsNullOrEmpty(towerid) ? x_.Command == sentcommand_ : x_.IsMatched(sentcommand_, towerid));

                    if (packet_ != null)
                    {
                        packet_.Dispose();
                        lock (sentPackets)
                            sentPackets.Remove(packet_);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected virtual void RetrySendMessage(object sender, int retry)
        {
            bool result_ = false;

            if (sender is MaterialStoragePacket)
            {
                MaterialStoragePacket packet_ = new MaterialStoragePacket(sender as MaterialStoragePacket);

                if (communicationState == CommunicationStates.Connected && IsConnected)
                {
                    result_ = SendMessage(sender as MaterialStoragePacket);

                    lock (sentPackets)
                        sentPackets.Remove(sender as MaterialStoragePacket);
                }

                if (result_)
                {
                    packet_.Dispose();
                }
                else
                {
                    new TaskFactory().StartNew(new Action<object>((x_) =>
                    {
                        if (x_ != null)
                        {
                            lock (sentPackets)
                                sentPackets.Add(x_ as MaterialStoragePacket);
                        }
                    }), packet_);
                }
            }
        }
        #endregion

        #region Communication state change event methods
        protected virtual void FireCommunicationStateChanged(CommunicationStates state)
        {
            if (state != CommunicationStates.Connected)
                actionStates = RobotActionStates.Unknown;

            CommunicationStateChanged?.Invoke(this, communicationState = state);
        }
        #endregion

        #region Client communication event methods
        protected virtual void OnError(object sender, AsyncSocketErrorEventArgs e)
        {
            try
            {
                FireCommunicationStateChanged(CommunicationStates.Error);
                FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.AsyncSocketException.Message}");
                ClearMessageQueue();
                receivedStateResponseFlag = 0;

                if (client != null)
                    client.Disconnect();
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }
        }

        protected virtual void OnConnected(object sender, AsyncSocketConnectionEventArgs e)
        {
            lastReceivedMessage         = string.Empty;
            receivedStateResponseFlag   = 0;
            FireCommunicationStateChanged(CommunicationStates.Connected);
            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}=({endPoint.Address}:{endPoint.Port}[{e.Id}])");
        }

        protected virtual void OnDisconnected(object sender, AsyncSocketConnectionEventArgs e)
        {
            lock (clients)
            {
                if (clients.ContainsKey(e.Id))
                    clients.Remove(e.Id);
            }

            if (clients.Count <= 0)
                FireCommunicationStateChanged(CommunicationStates.Disconnected);

            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}=({endPoint.Address}:{endPoint.Port}[{e.Id}])");
            lastReceivedMessage         = string.Empty;
            receivedStateResponseFlag   = 0;
        }

        protected virtual void OnTickWatcher(object sender, EventArgs e)
        {
            responseWatcher.Stop();

            if (communicationState == CommunicationStates.Connected)
            {
                if (sentPackets.Count > 0)
                    responseFailure = IsResponseTimeout(timeoutOfResponse);
                
                if ((DateTime.Now - lastPingDatetime).TotalMilliseconds >= intervalOfPing)
                {
                    RequestPing();
                    lastPingDatetime = DateTime.Now;
                }
            }
            else if (reconnectCount++ > 30)
            {
                TryClientConnecting(endPoint.Address, endPoint.Port);
                reconnectCount = 0;
            }

            responseWatcher.Start();
        }

        protected virtual void OnReceived(object sender, AsyncSocketReceiveEventArgs e)
        {
            receivedMessages.Enqueue(new Pair<int, string>(e.Id, new string(Encoding.Default.GetChars(e.ReceiveData))));
        }

        protected virtual void OnSent(object sender, AsyncSocketSendEventArgs e)
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
            int tagBegin_                       = 0;
            int tagEnd_                         = 0;
            int tagLength_                      = 0;
            int outputStage_                    = 0;
            int clientId_                       = 0;
            int pickcount_                      = 0;
            int delpos_                         = 0;
            string receiveMessage_              = string.Empty;
            string messageType_                 = string.Empty;
            string messageTagBegin_             = "<messageName>";
            string messageTagEnd_               = "</messageName>";
            string pickid_                      = string.Empty;
            string targetloc_                   = string.Empty;
            string receivedId_                  = string.Empty;
            string stage_                       = string.Empty;
            string[] messages_                  = null;
            List<int> indices_                  = new List<int>();
            List<List<string>> uids_            = new List<List<string>>();
            ReelTowerCommands messageCommand_   = ReelTowerCommands.REQUEST_TOWER_STATE;

            if (!receivedMessages.TryDequeue(out queuedMessage))
                return;

            clientId_       = queuedMessage.first;
            messages_       = queuedMessage.second.Split(new char[] { (char)AsciiControlCharacters.Etx }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string message_ in messages_)
            {
                receiveMessage_ = message_.Replace("\0", string.Empty);

                if (receiveMessage_[0] == (char)AsciiControlCharacters.Stx)
                    receiveMessage_ = receiveMessage_.Substring(1, receiveMessage_.Length - 1);
                else
                {
                    Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Dumped packet={receiveMessage_}");
                    continue;
                }

                try
                {
                    tagBegin_       = receiveMessage_.IndexOf(messageTagBegin_);
                    tagEnd_         = receiveMessage_.IndexOf(messageTagEnd_);
                    tagLength_      = messageTagBegin_.Length;
                    messageType_    = receiveMessage_.Substring(tagBegin_ + tagLength_, tagEnd_ - tagBegin_ - tagLength_);

                    if (reelTowerCommands.ContainsKey(messageType_))
                    {
                        if ((messageCommand_ = reelTowerCommands[messageType_]) != ReelTowerCommands.REPLY_LINK_TEST)
                        {
                            receivedData.Clear();
                            receivedXml.LoadXml(receiveMessage_);
                            XmlNode root    = receivedXml.SelectSingleNode("message");
                            XmlNode header  = root.SelectSingleNode("//header");
                            XmlNode body    = root.SelectSingleNode("//body");
                            XmlNode tail    = root.SelectSingleNode("//return");

                            if (header != null)
                            {   // Parse header of message
                                receivedData.Name           = header.SelectSingleNode("messageName").InnerText;
                                receivedData.TransactionID  = header.SelectSingleNode("transactionId").InnerText;
                                receivedData.TimeStamp      = header.SelectSingleNode("timeStamp").InnerText;
                            }
                            else
                                return;

                            if (body != null)
                            {   // Parse body of message
                                foreach (XmlNode element in body.ChildNodes)
                                {
                                    switch (element.Name)
                                    {
                                        case "MATERIALTOWERID":
                                            {   // Single tower state report
                                                Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Received_MaterialTowerId> {element.InnerText}");

                                                if (string.IsNullOrEmpty(receivedData.TowerId = element.InnerText))
                                                    return;
                                            }
                                            break;
                                        case "MATERIALTOWERSTATE":
                                            {   // Single tower state report
                                                if (string.IsNullOrEmpty(receivedData.TowerState = element.InnerText))
                                                    return;
                                            }
                                            break;
                                        case "UID":
                                            {
                                                receivedData.Data.Name = element.InnerText;
                                            }
                                            break;
                                        case "LOADSTATE":
                                            {
                                                receivedData.LoadState = element.InnerText;
                                            }
                                            break;
                                        case "STAGE":
                                            {
                                                if (!string.IsNullOrEmpty(receivedData.OutputStage = element.InnerText) && receivedData.OutputStage.Contains("No_"))
                                                {
                                                    delpos_ = receivedData.OutputStage.LastIndexOf("_");
                                                    stage_ = receivedData.OutputStage.Substring(delpos_, receivedData.OutputStage.Length - delpos_ - 1);
                                                    switch (outputStage_ = Convert.ToInt32(stage_))
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
                                                        case "LOADTYPE":
                                                            {
                                                                receivedData.Data.LoadType = element.InnerText.ToLower().Contains("cart") ? LoadMaterialTypes.Cart : LoadMaterialTypes.Return;
                                                            }
                                                            break;
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
                                                        case "SIZE":
                                                            {
                                                                receivedData.Data.Size = int.Parse(child.InnerText);

                                                                if (receivedData.Data.LoadType == LoadMaterialTypes.Return)
                                                                {
                                                                    switch (receivedData.Data.Size)
                                                                    {
                                                                        case 4:
                                                                            receivedData.Data.ReelType = ReelDiameters.ReelDiameter4;
                                                                            break;
                                                                        case 7:
                                                                            receivedData.Data.ReelType = ReelDiameters.ReelDiameter7;
                                                                            break;
                                                                        case 13:
                                                                            receivedData.Data.ReelType = ReelDiameters.ReelDiameter13;
                                                                            break;
                                                                        case 15:
                                                                            receivedData.Data.ReelType = ReelDiameters.ReelDiameter15;
                                                                            break;
                                                                    }
                                                                }
                                                            }
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
                            {
                                try
                                {
                                    if (tail.HasChildNodes)
                                    {
                                        receivedData.ReturnCode = tail.SelectSingleNode("returnCode").InnerText;
                                        receivedData.ReturnMessage = tail.SelectSingleNode("returnMessage").InnerText;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message);
                                }
                            }

                            RemoveSentMessage(messageCommand_, receivedData.TowerId);
                            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={receiveMessage_}");

                            switch (messageCommand_)
                            {
                                case ReelTowerCommands.REQUEST_ALL_LOAD_RESET:  // After initialize or cancel current load reel process.
                                case ReelTowerCommands.REQUEST_ALL_UNLOAD_RESET:
                                case ReelTowerCommands.REQUEST_ALL_ALARM_RESET:                                    
                                case ReelTowerCommands.REPLY_LOAD_COMPLETE:
                                case ReelTowerCommands.REPLY_UNLOAD_COMPLETE:
                                case ReelTowerCommands.REQUEST_TOWER_STATE:
                                case ReelTowerCommands.REQUEST_TOWER_STATE_ALL:
                                case ReelTowerCommands.REQUEST_REEL_LOAD_MOVE:
                                case ReelTowerCommands.REQUEST_BARCODEINFO_CONFIRM:
                                case ReelTowerCommands.REQUEST_REEL_LOAD_ASSIGN:
                                case ReelTowerCommands.REQUEST_REEL_UNLOAD_MOVE:
                                case ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN:
                                case ReelTowerCommands.REQUEST_LOAD_RESET:
                                case ReelTowerCommands.REQUEST_UNLOAD_RESET:
                                case ReelTowerCommands.REQUEST_LOAD_COMPLETE:
                                case ReelTowerCommands.REQUEST_UNLOAD_COMPLETE:
                                case ReelTowerCommands.REQUEST_CART_LOAD_FINISH:
                                case ReelTowerCommands.REPLY_ALL_LOAD_RESET:
                                case ReelTowerCommands.REPLY_ALL_UNLOAD_RESET:
                                case ReelTowerCommands.REPLY_ALL_ALARM_RESET:                                    
                                    {
                                        MaterialStorageMessage data_ = new MaterialStorageMessage();
                                        data_.CopyFrom(receivedData);
                                        Pair<ReelTowerCommands, MaterialStorageMessage> arg_ = new Pair<ReelTowerCommands, MaterialStorageMessage>(messageCommand_, data_);
                                        new TaskFactory().StartNew(new Action<object>((obj_) =>
                                        {
                                            if (obj_ != null)
                                                FireReceivedRequestCommand(obj_ as Pair<ReelTowerCommands, MaterialStorageMessage>);
                                        }), arg_);
                                    }
                                    break;
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
        protected virtual bool Send(string data)
        {
            bool result_ = false;

            if (client != null && client.IsConnected)
                result_ = client.Send(Encoding.Default.GetBytes(data));

            return result_;
        }
        #endregion

        #region Fire received material package
        protected virtual void FireReceivedRequestCommand(Pair<TechFloor.ReelTowerCommands, MaterialStorageMessage> obj)
        {
            RequestCommandReceived?.Invoke(this, new Pair<TechFloor.ReelTowerCommands, MaterialStorageMessage>(obj.first, obj.second));
        }
        #endregion
        #endregion

        #region Public methods
        #region Element create methods
        public virtual void Create(IPEndPoint endpoint)
        {
            if (endpoint != null)
                Create(endpoint.Address, endpoint.Port);
        }

        public virtual void Create(IPAddress address, int portno)
        {
            stop = false;
            serviceout = false;
            LoadProperties();

            if (receivedData.Data == null)
                receivedData.CreateData();

            if (responseWatcher == null)
            {
                responseWatcher             = new System.Timers.Timer();
                responseWatcher.Interval    = 100;
                responseWatcher.AutoReset   = true;
                responseWatcher.Elapsed     += OnTickWatcher;
                responseWatcher.Start();
            }

            if (threadReelHandlerStateWatcher == null)
                (threadReelHandlerStateWatcher = new Thread(new ThreadStart(Run)))?.Start();

            FireCommunicationStateChanged(CommunicationStates.Connecting);
            Connect(address, portno);            
            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}");
        }

        public virtual void Destroy()
        {
            serviceout = true;

            if (threadReelHandlerStateWatcher != null)
                threadReelHandlerStateWatcher.Join();
            
            threadReelHandlerStateWatcher = null;
            Disconnect();
        }
        #endregion

        #region Server service control methods
        public virtual bool Connect(IPAddress address, int portno = 0)
        {
            if (address != null)
            {
                if (endPoint == null)
                    endPoint = new IPEndPoint(address, portno);
                else
                {
                    endPoint.Address = address;
                    endPoint.Port = portno;
                }
            }

            if (client == null)
            {
                client                  = new AsyncSocketClient(portno);
                client.OnError          += new AsyncSocketErrorEventHandler(OnError);
                client.OnConnected      += OnConnected;
                client.OnDisconnected   += OnDisconnected;
                client.OnSent           += OnSent;
                client.OnReceived       += OnReceived;
            }

            return client.Connect(address, portno);
        }

        public virtual void Disconnect(bool forced = false)
        {
            if (client != null)
            {
                client.Disconnect();
                client.OnError          -= new AsyncSocketErrorEventHandler(OnError);
                client.OnConnected      -= OnConnected;
                client.OnDisconnected   -= OnDisconnected;
                client.OnSent           -= OnSent;
                client.OnReceived       -= OnReceived;
                client                  = null;
            }
        }
        #endregion

        #region Event notification method
        public virtual void FireCommunicationStateChanged()
        {
            CommunicationStateChanged?.Invoke(this, CommunicationState);
        }

        public virtual void FireReportRuntimeLog(string log)
        {
            if (!string.IsNullOrEmpty(log))
                ReportRuntimeLog?.Invoke(this, log);
        }

        public virtual void FireReportException(string log)
        {
            if (!string.IsNullOrEmpty(log))
                ReportException?.Invoke(this, log);
        }
        #endregion

        #region Clear reel handler message queue methods after robot handling
        public virtual void ClearMessageQueue()
        {
            Pair<int, string> meesage_ = new Pair<int, string>(0, string.Empty);

            while (!receivedMessages.IsEmpty)
                receivedMessages.TryDequeue(out meesage_);
        }

        public virtual void ClearAllWaitMessages()
        {
            lock (sentPackets)
            {
                foreach (MaterialStoragePacket packet_ in sentPackets)
                    packet_.Dispose();

                sentPackets.Clear();
            }

            ClearMessageQueue();
        }
        #endregion

        #region Response check methods
        protected virtual bool IsResponseTimeout(int timeout = 10000, bool retry = true)
        {
            bool result_ = false;

            try
            {
                MaterialStoragePacket packet_ = sentPackets.Find(x_ => x_.IsOverTimeout(timeout));

                if (packet_ != null)
                {
                    if (packet_.IsOverRetry(limitOfRetry))
                    {
                        RemoveSentMessage(packet_.Command, packet_.TowerId);
                        result_ = true;
                    }
                    else
                        RetrySendMessage(packet_, packet_.Retry);
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public virtual void TryClientConnecting(IPAddress address, int port)
        {
            if (IsConnected)
                return;

            Create(address, port);
        }
        #endregion

        #region Send message methods
        public virtual void RequestPing(string towerid = null)
        {
            SendMessage(ReelTowerCommands.REQUEST_LINK_TEST);
        }

        public virtual bool SendMessage(ReelTowerCommands command, object data = null, string towerid= null, string state= null, string code = "0", string message = "done", bool prefix = true)
        {
            return SendMessage(new MaterialStoragePacket(command, data, towerid, state, code, message, prefix));
        }

        public virtual bool SendMessage(MaterialStoragePacket packet)
        { 
            bool result_ = false;

            try
            {
                if (packet != null)
                {
                    bool printout_      = true;
                    bool waitresponse_  = false;
                    string sentmessage_ = string.Empty;

                    switch (packet.Command)
                    {
                        case ReelTowerCommands.REQUEST_LINK_TEST:
                        case ReelTowerCommands.SEND_PICKING_LIST:
                            break;
                        case ReelTowerCommands.REQUEST_UNLOAD_COMPLETE:
                        case ReelTowerCommands.REQUEST_ALL_UNLOAD_RESET:
                            waitresponse_ = true;
                            break;
                        case ReelTowerCommands.REPLY_TOWER_STATE:
                        case ReelTowerCommands.REPLY_REEL_LOAD_MOVE:
                        case ReelTowerCommands.REPLY_BARCODEINFO_CONFIRM:
                        case ReelTowerCommands.REPLY_REEL_UNLOAD_MOVE:
                        case ReelTowerCommands.REPLY_REEL_UNLOAD_ASSIGN:
                        case ReelTowerCommands.REPLY_LOAD_COMPLETE:
                        case ReelTowerCommands.REPLY_UNLOAD_COMPLETE:                        
                            break;
                        default:
                            return false;
                    }

                    if (communicationState == CommunicationStates.Connected)
                    {
                        if (result_ = Send(sentmessage_ = packet.CreatePacket()))
                        {
                            packet.Retry++;
                            
                            if (printout_)
                                FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={sentmessage_}");

                            if (waitresponse_)
                            {
                                new TaskFactory().StartNew(new Action<object>((x_) =>
                                {
                                    if (x_ != null)
                                    {
                                        lock (sentPackets)
                                        {
                                            sentPackets.Add(x_ as MaterialStoragePacket);
                                        }
                                    }
                                }), new MaterialStoragePacket(packet));

                                switch (packet.Command)
                                {
                                    case ReelTowerCommands.REQUEST_REEL_LOAD_MOVE:
                                        {
                                            requiredLoadCancelIfFailure = true;
                                            lastRequestedLoadTowerId = packet.TowerId;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (lastCommunicationStateLog != $"{ClassName}.{MethodBase.GetCurrentMethod().Name}=Send Tower Message returned false, command = {packet.Command}, CommunicationStates = {communicationState}")
                            FireReportRuntimeLog(lastCommunicationStateLog = $"{ClassName}.{MethodBase.GetCurrentMethod().Name}=Send Tower Message returned false, command = {packet.Command}, CommunicationStates = {communicationState}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
            finally
            {
                if (packet != null)
                    packet.Dispose();
            }

            return result_;
        }
        #endregion

        #region Restart reel handler manager
        public virtual void RestartReelHandlerManager(IPAddress address, int portno)
        {
            Destroy();
            Create(address, portno);
        }
        #endregion
        #endregion
    }
}
#endregion