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
/// project Object 
/// @author Marcus (isadrastea.kor@gmail.com)
/// @namespace Marcus.Solution.TechFloor
/// @file ComponentReelStorageState.cs
/// @brief
/// @details
/// @date 2019, 5, 21, 오후 1:13
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using System;
using System.Collections.Generic;
using System.Threading;
using Marcus.Solution.TechFloor;
#endregion

#region Program
namespace Marcus.Solution.TechFloor
{
    #region Enumerations
    public enum ReelTypes : int
    {
        Unknown,
        ReelDiameter7 = 7,
        ReelDiameter13 = 13
    }

    public enum LoadTypes : int
    {
        Cart,
        Return
    }

    public enum ReelTowerCommands
    {
        SEND_PICKING_LIST,

        REQUEST_LINK_TEST,
        REQUEST_TOWER_STATE,
        REQUEST_TOWER_STATE_ALL,
        REQUEST_REEL_LOAD_MOVE,
        REQUEST_BARCODEINFO_CONFIRM,
        REQUEST_REEL_LOAD_ASSIGN,
        REQUEST_LOAD_COMPLETE,
        REQUEST_UNLOAD_COMPLETE,
        REQUEST_REEL_UNLOAD_MOVE,
        REQUEST_REEL_UNLOAD_ASSIGN,
        REQUEST_LOAD_RESET,
        REQUEST_UNLOAD_RESET,
        REQUEST_ALL_LOAD_RESET,
        REQUEST_ALL_UNLOAD_RESET,
        REQUEST_ALL_ALARM_RESET,

        REPLY_LINK_TEST,
        REPLY_TOWER_STATE,
        REPLY_TOWER_STATE_ALL,
        REPLY_REEL_LOAD_MOVE,
        REPLY_BARCODEINFO_CONFIRM,        
        REPLY_REEL_LOAD_ASSIGN,
        REPLY_REEL_UNLOAD_MOVE,
        REPLY_REEL_UNLOAD_ASSIGN,
        REPLY_LOAD_COMPLETE,
        REPLY_UNLOAD_COMPLETE,
        REPLY_LOAD_RESET,
        REPLY_UNLOAD_RESET,
    }
    #endregion

    public class MaterialData
    {
        public string Sid;
        public string LotId;
        public string Maker;
        public string Qty;
        public string Rid;
        public string Mfg;
        public string Text;
        public LoadTypes LoadType;
        public ReelTypes ReelType;

        public bool IsValid => (Sid.Length == 9 &&!string.IsNullOrEmpty(Sid) && !string.IsNullOrEmpty(LotId) && !string.IsNullOrEmpty(Mfg) && !string.IsNullOrEmpty(Rid));

        public MaterialData()
        {
        }

        public void SetRid(string rid, bool reset = true)
        {
            if (reset)
                Clear();

            Rid = rid;
        }

        public void SetValues(string sid, string lot, string mkr, string rid, string qty, string mfg, LoadTypes loadtype = LoadTypes.Cart, ReelTypes reeltype = ReelTypes.ReelDiameter7, string data = null)
        {
            Sid     = sid;
            LotId   = lot;
            Maker   = mkr;
            Rid     = rid;
            Qty     = qty;
            Mfg     = mfg;
            Text    = data;
            LoadType = loadtype;
            ReelType = reeltype;
        }

        public void SetLoadType(LoadTypes loadtype)
        {
            LoadType = loadtype;
        }

        public void SetReelType(ReelTypes reeltype)
        {
            ReelType = reeltype;
        }

        public void SetReelInformation(LoadTypes loadtype, ReelTypes reeltype)
        {
            LoadType = loadtype;
            ReelType = reeltype;
        }

        public void Clear()
        {
            Sid     = string.Empty;
            LotId   = string.Empty;
            Maker   = string.Empty;
            Qty     = string.Empty;
            Rid     = string.Empty;
            Mfg     = string.Empty;
            Text    = string.Empty;
            LoadType = LoadTypes.Cart;
        }

        public void CopyFrom(MaterialData src)
        {
            Sid     = src.Sid;
            LotId   = src.LotId;
            Maker   = src.Maker;
            Qty     = src.Qty;
            Rid     = src.Rid;
            Mfg     = src.Mfg;
            Text    = src.Text;
            LoadType = src.LoadType;
        }
    }

    public struct MaterialStorageMessage
    {
        #region Fields
        public string Name;
        public string TransactionID;
        public string TimeStamp;
        public string TowerId;
        public string TowerState;
        public string LoadState;
        public string ReturnCode;
        public string ReturnMessage;
        public string OutputStage;
        public string ErrorMessage;
        public MaterialData Data;
        #endregion

        public void CreateData()
        {
            Data = new MaterialData();
        }

        public void CopyFrom(MaterialStorageMessage src)
        {
            Name            = src.Name;
            TransactionID   = src.TransactionID;
            TimeStamp       = src.TimeStamp;
            TowerId         = src.TowerId;
            TowerState      = src.TowerState;
            LoadState       = src.LoadState;
            ReturnCode      = src.ReturnCode;
            ReturnMessage   = src.ReturnMessage;
            OutputStage     = src.OutputStage;
            ErrorMessage    = src.ErrorMessage;

            if (Data != null && src.Data != null)
                Data.CopyFrom(src.Data);
        }

        public void Clear()
        {
            Name            = string.Empty;
            TransactionID   = string.Empty;
            TimeStamp       = string.Empty;
            TowerId         = string.Empty;
            TowerState      = string.Empty;
            LoadState       = string.Empty;
            ReturnCode      = string.Empty;
            ReturnMessage   = string.Empty;
            OutputStage     = string.Empty;
            ErrorMessage    = string.Empty;

            if (Data != null)
                Data.Clear();
        }
    }

    namespace Object
    {
        public class MaterialStorageState : AbstractClassDisposable
        {
            #region Enumerations
            public enum StorageOperationStates
            {
                Unknown,
                Idle,
                Run,
                Down,
                Error,
                Load,
                Unload,
                Wait,
                Full
            }

            public enum StoragePortStates
            {
                Unknown,
                Load,
                Unload
            }
            #endregion

            #region Fields
            protected bool waitResponseFlag                     = false;
            protected bool assignedToUnload                     = false;
            protected bool returnReel                           = false;
            protected bool failure                              = false;
            protected int receivedFlag                          = 0;
            protected int outputStageIndex                      = -1;
            protected string name                               = string.Empty;
            protected string outputStage                        = string.Empty;
            protected string returnCode                         = string.Empty;
            protected string returnMessage                      = string.Empty;
            protected MaterialData data                         = new MaterialData();
            protected DateTime requestTime                      = DateTime.MaxValue;
            protected ReelTypes reelType                        = ReelTypes.Unknown;
            protected ReelTowerCommands command                 = ReelTowerCommands.REQUEST_TOWER_STATE;
            protected StorageOperationStates state              = StorageOperationStates.Unknown;
            protected StoragePortStates mode                    = StoragePortStates.Unknown;
            protected List<ComponentReelObject> requestItems    = new List<ComponentReelObject>();
            #endregion

            #region Events
            public virtual event EventHandler<string> ChangedStorageOperationState;
            public virtual event EventHandler<string> ChangedStoragePortState;
            #endregion

            #region Properties
            public bool IsWaitResponse          => waitResponseFlag;

            public bool IsAssignedToUnload      => assignedToUnload;

            public bool IsReceived              => receivedFlag == 1;

            public bool IsFailure               => failure;

            public bool IsReturnReel            => returnReel;

            public int Index
            {
                get;
                protected set;
            }

            public string OutputStage
            {
                get => outputStage;
                set => outputStage = value;
            }

            public int OutputStageIndex         => outputStageIndex;

            public MaterialData Data            => data;

            public string Sid                   => data.Sid;

            public string LotId                 => data.LotId;

            public string Maker                 => data.Maker;

            public string Qty                   => data.Qty;

            public string Mfg                   => data.Mfg;

            public string Rid                   => data.Rid;

            public string Uid                   => data.Text;

            public string RequestUid
            {
                get => data.Text;
                set => data.Text = value;
            }

            public string RequestStage
            {
                get => outputStage;
                set => outputStage = value;
            }

            public string ReturnCode            => returnCode;

            public string ReturnMessage         => returnMessage;

            public DateTime RequestTime         => requestTime;

            public ReelTypes ReelType           => reelType;

            public ReelTowerCommands Command    => command;

            public StorageOperationStates State
            {
                get => state;
                set
                {
                    if (state != value)
                    {
                        
                        switch (state = value)
                        {
                            case StorageOperationStates.Unknown:
                            case StorageOperationStates.Idle:
                            case StorageOperationStates.Run:
                            case StorageOperationStates.Down:
                            case StorageOperationStates.Error:
                            case StorageOperationStates.Load:
                            case StorageOperationStates.Wait:
                            case StorageOperationStates.Full:
                                assignedToUnload    = false;
                                break;
                            case StorageOperationStates.Unload:
                                break;
                        }

                        FireChangedStorageOperationState($"tower={name}, state=({state} -> {value})");
                    }
                }
            }

            public StoragePortStates Mode
            {
                get => mode;
                set
                {
                    if (mode != value)
                    {
                        mode = value;
                        FireChangedStoragePortState($"tower={name}, state=({mode} -> {value})");
                    }
                }
            }

            public string Name
            {
                get => name;
                set
                {
                    if (name != value)
                        name = value;
                }
            }

            public IReadOnlyList<ComponentReelObject> RequestItems => requestItems;
            #endregion

            #region Constructors
            public MaterialStorageState()
            {
                Index   = -1;
                name    = string.Empty;
            }

            public MaterialStorageState(int index, string id)
            {
                Index   = index;
                name    = id;
            }

            public MaterialStorageState(int index, string id, MaterialStorageMessage message, int stageno = 0)
            {
                Index = index;
                name = id;

                if (string.IsNullOrEmpty(message.Data.Text) && Data.Text == message.Data.Text)
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
                    if (data == null)
                        data = new MaterialData();

                    data.CopyFrom(message.Data);
                }
                else
                {
                    if (data != null)
                        data.Clear();
                }
            }
            #endregion

            #region Public methods
            public void CopyFrom(MaterialStorageState src)
            {
                Index           = src.Index;
                state           = src.state;
                mode            = src.mode;
                name            = src.name;
                assignedToUnload= src.assignedToUnload;
                requestTime     = src.requestTime;
                outputStage     = src.outputStage;
                outputStageIndex = src.outputStageIndex;
                returnCode      = src.returnCode;
                returnMessage   = src.returnMessage;

                data.CopyFrom(src.data);
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
                reelType = ReelTypes.Unknown;
                command = ReelTowerCommands.REQUEST_TOWER_STATE;
                state = StorageOperationStates.Unknown;
                mode = StoragePortStates.Unknown;

                if (data != null)
                    data.Clear();

                foreach (var obj in requestItems)
                    obj.Dispose();
            }

            public void Clear()
            {
                name                = string.Empty;
                InitData();
            }

            public void SetReelType(ReelTypes reeltype)
            {
                reelType = reeltype;
            }

            public void SetLoadReelType(ReelTypes reeltype, bool fromreturn = false)
            {
                reelType    = reeltype;
                returnReel  = fromreturn;
            }

            public void SetCommand(ReelTowerCommands cmd, bool waitresponse = true)
            {
                Clear();
                Reset();
                command             = cmd;
                waitResponseFlag    = waitresponse;
                requestTime         = DateTime.Now;
            }

            public bool IsResponseTimeout(int timeout = 1000)
            {
                return (failure = (DateTime.Now - requestTime).TotalMilliseconds > timeout);
            }

            public void UpdateResponse(ReelTowerCommands response, MaterialStorageMessage message, int stageindex = 0)
            {
                TechFloor.ReelTowerCommands response_ = TechFloor.ReelTowerCommands.REPLY_LINK_TEST;

                switch (command)
                {
                    case TechFloor.ReelTowerCommands.REQUEST_TOWER_STATE:
                    case TechFloor.ReelTowerCommands.REQUEST_TOWER_STATE_ALL:   // Temporary using
                        response_ = TechFloor.ReelTowerCommands.REPLY_TOWER_STATE;
                        break;
                    case TechFloor.ReelTowerCommands.REQUEST_BARCODEINFO_CONFIRM:
                        response_ = TechFloor.ReelTowerCommands.REPLY_BARCODEINFO_CONFIRM;
                        break;
                    case TechFloor.ReelTowerCommands.REQUEST_REEL_LOAD_MOVE:
                        response_ = TechFloor.ReelTowerCommands.REPLY_REEL_LOAD_MOVE;
                        break;
                    case TechFloor.ReelTowerCommands.REQUEST_REEL_LOAD_ASSIGN:
                        response_ = TechFloor.ReelTowerCommands.REPLY_REEL_LOAD_ASSIGN;
                        break;
                    case TechFloor.ReelTowerCommands.REQUEST_REEL_UNLOAD_MOVE:
                        response_ = TechFloor.ReelTowerCommands.REPLY_REEL_UNLOAD_MOVE;
                        break;
                    case TechFloor.ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN:
                        response_ = TechFloor.ReelTowerCommands.REPLY_REEL_UNLOAD_ASSIGN;
                        break;
                    case TechFloor.ReelTowerCommands.REQUEST_LOAD_COMPLETE:
                        response_ = TechFloor.ReelTowerCommands.REPLY_LOAD_COMPLETE;
                        break;
                    case TechFloor.ReelTowerCommands.REQUEST_LOAD_RESET:
                        response_ = TechFloor.ReelTowerCommands.REPLY_LOAD_RESET;
                        break;
                    case TechFloor.ReelTowerCommands.REQUEST_UNLOAD_COMPLETE:
                        response_ = TechFloor.ReelTowerCommands.REPLY_UNLOAD_COMPLETE;
                        break;
                    case TechFloor.ReelTowerCommands.REQUEST_UNLOAD_RESET:
                        response_ = TechFloor.ReelTowerCommands.REPLY_UNLOAD_RESET;
                        break;
                    case TechFloor.ReelTowerCommands.REQUEST_LINK_TEST:
                        response_ = TechFloor.ReelTowerCommands.REPLY_LINK_TEST;
                        break;
                }
                                
                if (message.Data != null)
                {
                    if (data == null)
                        data = new MaterialData();

                    data.CopyFrom(message.Data);
                }
                else
                {
                    if (data != null)
                        data.Clear();
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
                if (string.IsNullOrEmpty(message.Data.Text) && Data.Text == message.Data.Text)
                {   // Duplicated request
                    return;
                }

                assignedToUnload    = true; // Actually, a reel is already placed on unload port of reel tower.
                requestTime         = DateTime.Now;
                outputStageIndex    = stageno;
                outputStage         = message.OutputStage;
                returnCode          = message.ReturnCode;
                returnMessage       = message.ReturnMessage;

                if (message.Data != null)
                {
                    if (data == null)
                        data = new MaterialData();

                    data.CopyFrom(message.Data);
                }
                else
                {
                    if (data != null)
                        data.Clear();
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

            public virtual void FireChangedStorageOperationState(string text = null)
            {
                ChangedStorageOperationState?.Invoke(this, text);
            }

            public virtual void FireChangedStoragePortState(string text = null)
            {
                ChangedStoragePortState?.Invoke(this, text);
            }
            #endregion
        }
    }
}
#endregion