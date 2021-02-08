#region Imports
using System;
using System.Collections.Generic;
using System.Threading;
#endregion

#region Program
namespace TechFloor.Object
{
    public class MaterialStorageState // : AbstractClassDisposable
    {
        #region Enumerations
        public enum MaterialHandlingSource
        {
            None,
            LoadFromCart,
            LoadFromReturn,
            UnloadFromStorage,
            RejectFromCart,
            RejectFromReturn,
            RejectFromStorage
        }

        public enum MaterialHandlingDestination
        {
            None,
            LoadToStorage,
            UnloadToOutStage,
            UnloadToReject
        }

        public enum StorageOperationStates
        {
            Abort = -1,
            Unknown,
            Idle,
            Run,
            Down,
            Error,
            RequestedToLoad,
            PrepareToLoad,
            Load,
            RequestedToUnload,
            PrepareToUnload,
            Unload,
            Wait,
            Full
        }

        public enum StoragePortStates
        {
            Unknown,
            Load,
            Unload,
            Reject
        }
        #endregion

        #region Fields
        protected bool waitResponseFlag = false;

        protected bool assignedToUnload = false;

        protected bool loadableState = false;

        protected bool returnReel = false;

        protected bool failure = false;

        protected int receivedFlag = 0;

        protected int outputStageIndex = -1;

        protected string name = string.Empty;

        protected string outputStage = string.Empty;

        protected string returnCode = string.Empty;

        protected string returnMessage = string.Empty;

        protected MaterialData pendingData = new MaterialData();

        protected DateTime requestTime = DateTime.MaxValue;

        protected ReelDiameters reelType = ReelDiameters.Unknown;

        protected ReelTowerCommands command = ReelTowerCommands.REQUEST_TOWER_STATE;

        protected StorageOperationStates operationState = StorageOperationStates.Unknown;

        protected StoragePortStates portState = StoragePortStates.Unknown;

        protected List<ComponentReelObject> requestItems = new List<ComponentReelObject>();
        #endregion

        #region Events
        public virtual event EventHandler<string> StorageOperationStateChanged;

        public virtual event EventHandler<string> StoragePortStateChanged;
        #endregion

        #region Properties
        public virtual bool IsWaitResponse => waitResponseFlag;

        public virtual bool IsAssignedToUnload => assignedToUnload;

        public virtual bool IsReceived => receivedFlag == 1;

        public virtual bool IsFailure => failure;

        public virtual bool IsReturnReel => returnReel;

        public virtual int Index
        {
            get;
            protected set;
        }

        public virtual string Id
        {
            get;
            protected set;
        }

        public virtual string OutputStage
        {
            get => outputStage;
            set => outputStage = value;
        }

        public virtual int OutputStageIndex => outputStageIndex;

        public virtual MaterialData PendingData => pendingData;

        public virtual string Sid => pendingData.Category;

        public virtual string LotId => pendingData.LotId;

        public virtual string Maker => pendingData.Supplier;

        public virtual string Qty => pendingData.Quantity.ToString();

        public virtual string Mfg => pendingData.ManufacturedDatetime;

        //public virtual string Rid => pendingData.Text;

        public virtual string Uid => pendingData.Name;

        // public virtual string RequestUid
        // {
        //     get => pendingData.Text;
        //     set => pendingData.Text = value;
        // }

        public virtual string RequestStage
        {
            get => outputStage;
            set => outputStage = value;
        }

        public virtual string ReturnCode => returnCode;

        public virtual string ReturnMessage => returnMessage;

        public virtual DateTime RequestTime => requestTime;

        public virtual ReelDiameters ReelType => reelType;

        public virtual ReelTowerCommands Command => command;

        public virtual StorageOperationStates State
        {
            get => operationState;
            set
            {
                if (operationState != value)
                {
                    switch (operationState = value)
                    {
                        case StorageOperationStates.Unknown:
                        case StorageOperationStates.Idle:
                        case StorageOperationStates.Run:
                        case StorageOperationStates.Down:
                        case StorageOperationStates.Error:
                        case StorageOperationStates.Load:
                        case StorageOperationStates.Wait:
                        case StorageOperationStates.Full:
                            assignedToUnload = false;
                            break;
                        case StorageOperationStates.Unload:
                            break;
                    }

                    FireStorageOperationStateChanged($"tower={name}, state=({operationState} -> {value})");
                }
            }
        }

        public virtual StoragePortStates PortState
        {
            get => portState;
            set
            {
                if (portState != value)
                {
                    portState = value;
                    FireStoragePortStateChanged($"tower={name}, state=({portState} -> {value})");
                }
            }
        }

        public virtual string Name
        {
            get => name;
            set
            {
                if (name != value)
                    name = value;
            }
        }

        public virtual IReadOnlyList<ComponentReelObject> RequestItems => requestItems;
        #endregion

        #region Constructors
        public MaterialStorageState()
        {
            this.Index = -1;
            this.name = string.Empty;
        }

        public MaterialStorageState(int index, string id, string name= null)
        {
            this.Index = index;
            this.Id = id;
            this.name = name;
        }

        public MaterialStorageState(int index, string name, MaterialStorageMessage message, int stageno = 0)
        {
            this.Index = index;
            this.name = name;

            if (string.IsNullOrEmpty(message.Data.Text) && PendingData.Text == message.Data.Text)
            {   // Duplicated request
                return;
            }

            assignedToUnload = true; // Actually, a reel is already placed on unload port of reel tower.
            requestTime = DateTime.Now;
            outputStageIndex = stageno;
            outputStage = message.OutputStage;
            returnCode = message.ReturnCode;
            returnMessage = message.ReturnMessage;

            if (message.Data != null)
            {
                if (pendingData == null)
                    pendingData = new MaterialData();

                pendingData.CopyFrom(message.Data);
            }
            else
            {
                if (pendingData != null)
                    pendingData.Clear();
            }
        }
        #endregion

        #region Public methods
        public void CopyFrom(MaterialStorageState src)
        {
            Index = src.Index;
            operationState = src.operationState;
            portState = src.portState;
            name = src.name;
            assignedToUnload = src.assignedToUnload;
            requestTime = src.requestTime;
            outputStage = src.outputStage;
            outputStageIndex = src.outputStageIndex;
            returnCode = src.returnCode;
            returnMessage = src.returnMessage;

            pendingData.CopyFrom(src.pendingData);
        }

        public void InitData()
        {
            waitResponseFlag = false;
            assignedToUnload = false;
            returnReel = false;
            outputStageIndex = -1;
            outputStage = string.Empty;
            returnCode = string.Empty;
            returnMessage = string.Empty;
            requestTime = DateTime.MaxValue;
            reelType = ReelDiameters.Unknown;
            command = ReelTowerCommands.REQUEST_TOWER_STATE;
            operationState = StorageOperationStates.Unknown;
            portState = StoragePortStates.Unknown;

            if (pendingData != null)
                pendingData.Clear();
        }

        public void Clear()
        {
            name = string.Empty;
            InitData();
        }

        public void SetReelType(ReelDiameters reeltype)
        {
            reelType = reeltype;
        }

        public void SetLoadReelType(ReelDiameters reeltype, bool fromreturn = false)
        {
            reelType = reeltype;
            returnReel = fromreturn;
        }

        public void SetCommand(ReelTowerCommands cmd, bool waitresponse = true)
        {
            Clear();
            Reset();
            command = cmd;
            waitResponseFlag = waitresponse;
            requestTime = DateTime.Now;
        }

        public bool IsResponseTimeout(int timeout = 1000)
        {
            return (failure = (DateTime.Now - requestTime).TotalMilliseconds > timeout);
        }

        public void UpdateResponse(ReelTowerCommands response, MaterialStorageMessage message, int stageindex = 0)
        {
            TechFloor.ReelTowerCommands response_ = ReelTowerCommands.REPLY_LINK_TEST;

            switch (command)
            {
                case ReelTowerCommands.REQUEST_TOWER_STATE:
                case ReelTowerCommands.REQUEST_TOWER_STATE_ALL:   // Temporary using
                    response_ = ReelTowerCommands.REPLY_TOWER_STATE;
                    break;
                case ReelTowerCommands.REQUEST_BARCODEINFO_CONFIRM:
                    response_ = ReelTowerCommands.REPLY_BARCODEINFO_CONFIRM;
                    break;
                case ReelTowerCommands.REQUEST_REEL_LOAD_MOVE:
                    response_ = ReelTowerCommands.REPLY_REEL_LOAD_MOVE;
                    break;
                case ReelTowerCommands.REQUEST_REEL_LOAD_ASSIGN:
                    response_ = ReelTowerCommands.REPLY_REEL_LOAD_ASSIGN;
                    break;
                case ReelTowerCommands.REQUEST_REEL_UNLOAD_MOVE:
                    response_ = ReelTowerCommands.REPLY_REEL_UNLOAD_MOVE;
                    break;
                case ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN:
                    response_ = ReelTowerCommands.REPLY_REEL_UNLOAD_ASSIGN;
                    break;
                case ReelTowerCommands.REQUEST_LOAD_COMPLETE:
                    response_ = ReelTowerCommands.REPLY_LOAD_COMPLETE;
                    break;
                case ReelTowerCommands.REQUEST_LOAD_RESET:
                    response_ = ReelTowerCommands.REPLY_LOAD_RESET;
                    break;
                case ReelTowerCommands.REQUEST_UNLOAD_COMPLETE:
                    response_ = ReelTowerCommands.REPLY_UNLOAD_COMPLETE;
                    break;
                case ReelTowerCommands.REQUEST_UNLOAD_RESET:
                    response_ = ReelTowerCommands.REPLY_UNLOAD_RESET;
                    break;
                case ReelTowerCommands.REQUEST_LINK_TEST:
                    response_ = ReelTowerCommands.REPLY_LINK_TEST;
                    break;
            }

            if (message.Data != null)
            {
                if (pendingData == null)
                    pendingData = new MaterialData();

                pendingData.CopyFrom(message.Data);
            }
            else
            {
                if (pendingData != null)
                    pendingData.Clear();
            }

            name = message.TowerId;
            waitResponseFlag = false;
            outputStage = message.OutputStage;
            outputStageIndex = stageindex;
            returnCode = message.ReturnCode;
            returnMessage = message.ReturnMessage;

            SetConversationFlag(response == response_);
        }

        public void RequestUnload(MaterialStorageMessage message, int stageno = 0)
        {
            if (string.IsNullOrEmpty(message.Data.Text) && PendingData.Text == message.Data.Text)
            {   // Duplicated request
                return;
            }

            assignedToUnload = true; // Actually, a reel is already placed on unload port of reel tower.
            requestTime = DateTime.Now;
            outputStageIndex = stageno;
            outputStage = message.OutputStage;
            returnCode = message.ReturnCode;
            returnMessage = message.ReturnMessage;

            if (message.Data != null)
            {
                if (pendingData == null)
                    pendingData = new MaterialData();

                pendingData.CopyFrom(message.Data);
            }
            else
            {
                if (pendingData != null)
                    pendingData.Clear();
            }
        }

        public void Reset()
        {
            SetConversationFlag(false);
            failure = false;
        }

        public void SetConversationFlag(bool state = true)
        {
            if (state)
                Interlocked.Exchange(ref receivedFlag, 1);
            else
            {
                Interlocked.Exchange(ref receivedFlag, 0);
                failure = false;
            }
        }

        public virtual void FireStorageOperationStateChanged(string text = null)
        {
            StorageOperationStateChanged?.Invoke(this, text);
        }

        public virtual void FireStoragePortStateChanged(string text = null)
        {
            StoragePortStateChanged?.Invoke(this, text);
        }

        public virtual string PrintData()
        {
            return $"{Uid};{Sid};{LotId};{Maker};{Mfg};{Qty};";
        }

        public virtual void SetRejectStage(string name, int index)
        {
            outputStage = name;
            outputStageIndex = index;
        }
        #endregion
    }
}
#endregion