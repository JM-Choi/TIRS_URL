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
/// project Gem 
/// @author Marcus (isadrastea.kor@gmail.com)
/// @namespace Marcus.Solution.TechFloor.Gem
/// @file AbstractClassHsmsDriverLinkgenesis.cs
/// @brief
/// @details
/// @date 2019, 1, 9, 오후 5:41
///////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using GEM_XGemPro;
using Marcus.Solution.TechFloor.Device;
using Marcus.Solution.TechFloor.Object;

namespace Marcus.Solution.TechFloor.Gem
{
    public abstract class AbstractClassHsmsDriverLinkgenesis : AbstractClassDisposable, IHsmsBroker
    {
        #region Constants
        public const string CONST_HOST_DRIVER_CONFIG_FILE = @"Config\HostDriver.cfg";
        public const string CONST_HOST_SERVICE_DEFINITION_FILE = @"Config\HostServiceDefinition.cfg";
        #endregion

        #region Fields
        private bool initialized_ = false;
        private string configFile_ = CONST_HOST_DRIVER_CONFIG_FILE;
        private HsmsDriverStates driverState_ = HsmsDriverStates.Unknown;
        private ControlStates currentControlState_ = ControlStates.None;
        private ControlStates previousControlState_ = ControlStates.None;
        private CommunicationEstablishStates communicationEstablishState_ = CommunicationEstablishStates.Disabled;
        private CommunicationStates currentCommunicationState_ = CommunicationStates.None;
        private CommunicationStates previousCommunicationState_ = CommunicationStates.None;
        protected Dictionary<int, Pair<int, bool>> equipmentConstantTable = new Dictionary<int, Pair<int, bool>>();
        protected Dictionary<int, Pair<int, bool>> collectionEventTable = new Dictionary<int, Pair<int, bool>>();
        protected Dictionary<int, Pair<int, VariableTypes>> variableTable = new Dictionary<int, Pair<int, VariableTypes>>();
        protected Dictionary<int, Pair<int, bool>> alarmTable = new Dictionary<int, Pair<int, bool>>();
        protected XGemProNet driver = null;
        protected Dictionary<string, InvalidParameterType> invalidParameters = new Dictionary<string, InvalidParameterType>();
        #endregion

        #region Properties
        public Manufacturer Manufacturer => Manufacturer.Linkgenesis;
        public HsmsDriverStates DriverState => driverState_;
        public ControlStates CurrentControlState => currentControlState_;
        public ControlStates PreviousControlState => previousControlState_;
        public CommunicationEstablishStates CommunicationEstablishState => communicationEstablishState_;
        public CommunicationStates CurrentCommunicationState => currentCommunicationState_;
        public CommunicationStates PreviousCommunicationState => previousCommunicationState_;
        public IReadOnlyDictionary<string, InvalidParameterType> InvalidParameters => invalidParameters;
        public IReadOnlyDictionary<int, Pair<int, bool>> EquipmentConstantTable => equipmentConstantTable;
        public IReadOnlyDictionary<int, Pair<int, bool>> CollectionEventTable => collectionEventTable;
        public IReadOnlyDictionary<int, Pair<int, VariableTypes>> VariableTable => variableTable;
        public IReadOnlyDictionary<int, Pair<int, bool>> AlarmTable => alarmTable;

        public LogicalDevices Category { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Board { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string IDeviceManufacturer.Manufacturer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Model { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string PartNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SerialNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion

        #region Events
        public abstract event EventHandler<string> RuntimeTrace;
        public abstract event EventHandler ChangedControlState;
        #endregion

        #region Constructors
        public AbstractClassHsmsDriverLinkgenesis()
        {
            if (driver == null) driver = new XGemProNet();
            DefineIdentifiers();
        }
        #endregion

        #region Protected methods
        protected override void DisposeManagedObjects()
        {
            DetachEventHandlers();
            Unload();
        }

        protected virtual void AttachEventHandlers()
        {
            if (driver == null && initialized_) return;

            driver.OnSECSMessageReceived += OnReceivedSecsMessage;
            driver.OnGEMCommStateChanged += OnChangedCommunicationState;
            driver.OnGEMControlStateChanged += OnChangedControlState;
            driver.OnGEMReqChangeECV += OnRequestChangeEquipmentConstantValues;
            driver.OnGEMECVChanged += OnChangedEquipmentConstants;
            driver.OnGEMReqGetDateTime += OnRequestGetDateTime;
            driver.OnGEMRspGetDateTime += OnResponseDateTime;
            driver.OnGEMReqDateTime += OnRequestSetDateTime;
            driver.OnXGEMStateEvent += OnDriverStateEvent;
            driver.OnGEMErrorEvent += OnDriverErrorEvent;
            driver.OnGEMReqRemoteCommand += OnReceivedRemoteCommand;
            driver.OnGEMTerminalMessage += OnSingleTerminalMessage;
            driver.OnGEMTerminalMultiMessage += OnMultiTerminalMessage;
            driver.OnGEMReqOffline += OnRequestOffline;
            driver.OnGEMReqOnline += OnRequestOnline;
        }

        protected virtual void DetachEventHandlers()
        {
            if (driver == null) return;

            driver.OnSECSMessageReceived -= OnReceivedSecsMessage;
            driver.OnGEMCommStateChanged -= OnChangedCommunicationState;
            driver.OnGEMControlStateChanged -= OnChangedControlState;
            driver.OnGEMReqChangeECV -= OnRequestChangeEquipmentConstantValues;
            driver.OnGEMECVChanged -= OnChangedEquipmentConstants;
            driver.OnGEMReqGetDateTime -= OnRequestGetDateTime;
            driver.OnGEMRspGetDateTime -= OnResponseDateTime;
            driver.OnGEMReqDateTime -= OnRequestSetDateTime;
            driver.OnXGEMStateEvent -= OnDriverStateEvent;
            driver.OnGEMErrorEvent -= OnDriverErrorEvent;
            driver.OnGEMReqRemoteCommand -= OnReceivedRemoteCommand;
            driver.OnGEMTerminalMessage -= OnSingleTerminalMessage;
            driver.OnGEMTerminalMultiMessage -= OnMultiTerminalMessage;
            driver.OnGEMReqOffline -= OnRequestOffline;
            driver.OnGEMReqOnline -= OnRequestOnline;
        }

        // TODO: 20190111 - Have to implement in delivered class
        protected virtual void DefineIdentifiers()
        {
        }

        protected virtual void SetVariable(ref long obj, Variable data)
        {
            switch (data.Type)
            {
                case VariableDataType.List: driver.SetListItem(obj, Convert.ToInt64(data.Value)); break;
                case VariableDataType.Ascii: driver.SetStringItem(obj, data.Value); break;
                case VariableDataType.Bin: driver.SetBinaryItem(obj, Convert.ToByte(data.Value)); break;
                case VariableDataType.Bool: driver.SetBoolItem(obj, Convert.ToBoolean(data.Value)); break;
                case VariableDataType.Uint1: driver.SetUint1Item(obj, Convert.ToByte(data.Value)); break;
                case VariableDataType.Uint2: driver.SetUint2Item(obj, Convert.ToUInt16(data.Value)); break;
                case VariableDataType.Uint4: driver.SetUint4Item(obj, Convert.ToUInt32(data.Value)); break;
                case VariableDataType.UInt8: driver.SetUint8Item(obj, Convert.ToUInt64(data.Value)); break;
                case VariableDataType.Int1: driver.SetInt1Item(obj, Convert.ToSByte(data.Value)); break;
                case VariableDataType.Int2: driver.SetInt2Item(obj, Convert.ToInt16(data.Value)); break;
                case VariableDataType.Int4: driver.SetInt4Item(obj, Convert.ToInt32(data.Value)); break;
                case VariableDataType.Int8: driver.SetInt8Item(obj, Convert.ToInt64(data.Value)); break;
                case VariableDataType.Float4: driver.SetFloat4Item(obj, Convert.ToSingle(data.Value)); break;
                case VariableDataType.Float8: driver.SetFloat8Item(obj, Convert.ToDouble(data.Value)); break;
            }
        }

        protected virtual void OnRequestOnline(long nMsgId, long nFromState, long nToState)
        {
            driver.GEMRspOnline(nMsgId, 0);
            Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod()}: MessageId={nMsgId},From={nFromState},To={nToState}");
        }

        protected virtual void OnRequestOffline(long nMsgId, long nFromState, long nToState)
        {
            driver.GEMRsqOffline(nMsgId, 0);
            Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod()}: MessageId={nMsgId},From={nFromState},To={nToState}");
        }

        // TODO: 20190111 - Have to implement in delivered class
        protected virtual void OnChangedEquipmentConstants(long nCount, long[] pnEcids, string[] psVals)
        {
            #region Required modification in delivered class
#if DEBUG
            Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod()}: ");

            for (int i = 0; i < nCount; i++)
                Debug.WriteLine($"Ecid:{pnEcids[i]}, Value:{psVals[i]}");
#endif
            #endregion
        }

        // TODO: 20190111 - Have to implement in delivered class
        protected virtual void OnRequestChangeEquipmentConstantValues(long nMsgId, long nCount, long[] pnEcids, string[] psVals)
        {
            #region Required modification in delivered class
#if DEBUG
            Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod()}: ");

            for (int i = 0; i < nCount; i++)
                Debug.WriteLine($"Ecid:{pnEcids[i]}, Value:{psVals[i]}");
#endif
            #endregion

            driver.GEMRspChangeECV(nMsgId, 0);
        }

        protected virtual void OnDriverErrorEvent(string sErrorName, long nErrorCode)
        {
            Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod()}: Error={sErrorName},Code={nErrorCode}");
        }

        protected virtual void OnDriverStateEvent(long nState)
        {
            switch (nState)
            {
                default:
                case -1: driverState_ = HsmsDriverStates.Unknown; break;
                case 0: driverState_ = HsmsDriverStates.Init; break;
                case 1: driverState_ = HsmsDriverStates.Idle; break;
                case 2: driverState_ = HsmsDriverStates.Setup; break;
                case 3: driverState_ = HsmsDriverStates.Ready; break;
                case 4: driverState_ = HsmsDriverStates.Execute; break;
            }

            Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod()}: DriverState={driverState_}");

            if (driverState_ == HsmsDriverStates.Execute)
            {
                SetEstablishCommunication(true);
                Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod()}: Call SetEstablishCommunication(true)");
            }
        }

        protected virtual void OnChangedCommunicationState(long nState)
        {
            previousCommunicationState_ = currentCommunicationState_;

            switch (nState)
            {
                default:
                    {
                        currentCommunicationState_ = 
                            communicationEstablishState_ == CommunicationEstablishStates.Disabled ?
                            CommunicationStates.CommDisabled :
                            CommunicationStates.None;
                    }
                    break;
                case -1: currentCommunicationState_ = CommunicationStates.None; break;
                case 1: currentCommunicationState_ = CommunicationStates.CommDisabled; break;
                case 2: currentCommunicationState_ = CommunicationStates.WaitCRFromHost; break;
                case 3: currentCommunicationState_ = CommunicationStates.WaitDelay; break;
                case 4: currentCommunicationState_ = CommunicationStates.WaitCRA; break;
                case 5: currentCommunicationState_ = CommunicationStates.Communicating; break;
            }

            Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod()}: State={currentCommunicationState_}");
        }

        protected virtual void OnChangedControlState(long nState)
        {
            previousControlState_ = currentControlState_;

            switch (nState)
            {
                case -1: currentControlState_ = ControlStates.None; break;
                case 1: currentControlState_ = ControlStates.EquipmentOffline; break;
                case 2: currentControlState_ = ControlStates.AttemptOnline; break;
                case 3: currentControlState_ = ControlStates.HostOffline; break;
                case 4: currentControlState_ = ControlStates.Local; break;
                case 5: currentControlState_ = ControlStates.Remote; break;
            }

            Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod()}: State={currentControlState_}");
        }

        // TODO: 20190111 - Have to implement in delivered class
        protected virtual void OnMultiTerminalMessage(long nTid, long nCount, string[] psMsg)
        {
        }

        // TODO: 20190111 - Have to implement in delivered class
        protected virtual void OnSingleTerminalMessage(long nTid, string sMsg)
        {
        }

        // TODO: 20190111 - Have to implement in delivered class
        // Notice: If required date time synchronize with server time,
        // you have to implement the below method.
        protected virtual void OnRequestSetDateTime(long nMsgId, string sSystemTime)
        {
            Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod()}: SystemTime={sSystemTime}");
            driver.GEMRspDateTime(nMsgId, 0);
        }

        // TODO: 20190111 - Have to implement in delivered class
        // If the host request date time of driver, you have to reply date time of it.
        protected virtual void OnRequestGetDateTime(long nMsgId)
        {
            driver.GEMRspGetDateTime(nMsgId, DateTime.Now.ToString("yyyyMMddHHmmss"));
        }

        // TODO: 20190111 - Have to implement in delivered class
        // When the host reply date time of it, you try to synchronize date time or something.
        protected virtual void OnResponseDateTime(string sSystemTime)
        {
        }

        protected virtual void OnReceivedSecsMessage(long nObjectID, long nStream, long nFunction, long nSysbyte)
        {
            switch (nStream)
            {
                case 2:
                    {
                        switch (nFunction)
                        {
                            case 37:
                                {
                                    long nItems = 0;
                                    bool[] ceed = new bool[1];
                                    ushort[] ceid = new ushort[1];
                                    driver.GetListItem(nObjectID, ref nItems);
                                    driver.GetBoolItem(nObjectID, ref ceed);
                                    driver.GetListItem(nObjectID, ref nItems);
                                    List<ushort> tempList = new List<ushort>();

                                    for (int i = 0; i < nItems; i++)
                                    {
                                        driver.GetUint2Item(nObjectID, ref ceid);
                                        tempList.Add(ceid[0]);
                                    }

                                    driver.CloseObject(nObjectID);
                                    driver.MakeObject(ref nObjectID);
                                    driver.SetListItem(nObjectID, 0);

                                    if (!ceed[0] && tempList.Count == 0)
                                        driver.GEMSetSpecificMessage(nObjectID, "DYNAMIC_EVENT_REPORT_CLEAR");
                                }
                                break;
                            case 49:
                                {
                                    long nItems = 0;
                                    uint[] uInt = new uint[1];
                                    ushort[] ushorts = new ushort[1];
                                    byte[] bytes = new byte[1];
                                    uint dataId = 0;
                                    string objectSpec = string.Empty;
                                    string stringItem = string.Empty;
                                    string commandId = string.Empty;
                                    ushort priority = 0;
                                    ushort replace = 0;
                                    string carrierId = string.Empty;
                                    string sourcePort = string.Empty;
                                    string destPort = string.Empty;
                                    byte carrierType = 0;
                                    string processId = string.Empty;
                                    string batchId = string.Empty;
                                    string batchSeq = string.Empty;
                                    string lotId = string.Empty;
                                    string carrierIdn = string.Empty;

                                    TransferCommands cmdType = TransferCommands.Transfer;

                                    driver.GetListItem(nObjectID, ref nItems);
                                    {
                                        driver.GetUint4Item(nObjectID, ref uInt);
                                        dataId = uInt[0];
                                        driver.GetStringItem(nObjectID, ref objectSpec);
                                        driver.GetStringItem(nObjectID, ref stringItem); //TRANSFER

                                        switch (stringItem.ToUpper())
                                        {
                                            case "TRANSFER": cmdType = TransferCommands.Transfer; break;
                                            case "UPDATE": cmdType = TransferCommands.Update; break;
                                            case "BATCHTRANSFER": cmdType = TransferCommands.BatchTransfer; break;
                                            case "STAGECOMMAND": cmdType = TransferCommands.StageCommand; break;
                                            default: Debug.Assert(false, $"서버로 부터 정의되지 않은 타입의 명령이 수신되었습니다. : {stringItem.ToUpper()}"); break;
                                        }

                                        driver.GetListItem(nObjectID, ref nItems);
                                        {
                                            driver.GetListItem(nObjectID, ref nItems);
                                            {
                                                driver.GetStringItem(nObjectID, ref stringItem); //COMMANDINFO
                                                driver.GetListItem(nObjectID, ref nItems);

                                                for (int i = 0; i < nItems; i++)
                                                {
                                                    long nSubItems = 0;

                                                    driver.GetListItem(nObjectID, ref nSubItems);
                                                    {
                                                        driver.GetStringItem(nObjectID, ref stringItem);

                                                        switch (stringItem.ToUpper())
                                                        {
                                                            case "COMMANDID": driver.GetStringItem(nObjectID, ref commandId); break;
                                                            case "PRIORITY":
                                                                {
                                                                    driver.GetUint2Item(nObjectID, ref ushorts);
                                                                    priority = ushorts[0];
                                                                }
                                                                break;
                                                            case "REPLACE":
                                                                {
                                                                    driver.GetUint2Item(nObjectID, ref ushorts);
                                                                    replace = ushorts[0];
                                                                }
                                                                break;
                                                            case "BATCHID":
                                                                {
                                                                    driver.GetStringItem(nObjectID, ref batchId);
                                                                }
                                                                break;
                                                            case "BATCHSEQ":
                                                                {
                                                                    driver.GetStringItem(nObjectID, ref batchSeq);
                                                                }
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                    }
                                                }
                                            }

                                            driver.GetListItem(nObjectID, ref nItems);
                                            {
                                                driver.GetStringItem(nObjectID, ref stringItem); //TRANSFERINFO
                                                driver.GetListItem(nObjectID, ref nItems);

                                                for (int i = 0; i < nItems; i++)
                                                {
                                                    long nSubItems = 0;
                                                    driver.GetListItem(nObjectID, ref nSubItems);

                                                    {
                                                        driver.GetStringItem(nObjectID, ref stringItem);
                                                        switch (stringItem.ToUpper())
                                                        {
                                                            case "CARRIERID": driver.GetStringItem(nObjectID, ref carrierId); break;
                                                            case "SOURCEPORT": driver.GetStringItem(nObjectID, ref sourcePort); break;
                                                            case "DESTPORT": driver.GetStringItem(nObjectID, ref destPort); break;
                                                        }
                                                    }
                                                }
                                            }

                                            driver.GetListItem(nObjectID, ref nItems);
                                            {
                                                driver.GetStringItem(nObjectID, ref stringItem); //CARRIERTYPE
                                                driver.GetUint1Item(nObjectID, ref bytes);
                                                carrierType = bytes[0];
                                            }
                                        }
                                    }
                                    driver.CloseObject(nObjectID);

                                    // OnRecvTransferCmd?.Invoke(this, new TransferCmdArgs()
                                    // {
                                    //     Rcmd = cmdType,
                                    // 
                                    //     CommandId = commandId,
                                    //     CommandType = "Job",
                                    //     Priority = priority,
                                    //     Replace = replace,
                                    //     BatchId = batchId,
                                    //     BatchSeq = batchSeq,
                                    //     CarrierId = carrierId,
                                    //     SrcPort = sourcePort,
                                    //     DstPort = destPort,
                                    //     CarrierType = carrierType,
                                    // 
                                    //     Stream = nStream,
                                    //     Function = nFunction,
                                    //     SysByte = nSysbyte,
                                    // });
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        protected virtual void OnReceivedRemoteCommand(long nMsgId, string sRcmd, long nCount, string[] psNames, string[] psVals)
        {
            using (RemoteCommandArgs remoteCmdArgs = new RemoteCommandArgs() { MessageId = nMsgId, Command = sRcmd })
            {
                for (int i = 0; i < nCount; i++)
                    remoteCmdArgs.Parameters.Add(psNames[i], psVals[i]);

                // OnRecvRemoteCmd?.Invoke(this, remoteCmdArgs);
            }
        }
        #endregion

        #region Public methods
        public virtual bool Load(string file = "")
        {
            AttachEventHandlers();

            if (initialized_ = (driver.Initialize(configFile_ = string.IsNullOrEmpty(file) ? CONST_HOST_DRIVER_CONFIG_FILE : file) == 0))
                return true;
            else
                throw new DriverException($"HSMS driver is not initialized.", driver.ToString(), "Please, restart MTS server.");
        }

        public virtual bool Unload()
        {
            if (initialized_)
            {
                if (driver.Close() == 0)
                    return true;
            }

            return false;
        }

        public virtual bool Start()
        {
            if (initialized_)
            {
                if (driver.Start() == 0)
                    return true;
            }
            
            return false;
        }

        public virtual bool Stop()
        {
            if (initialized_)
            {
                if (driver.Stop() == 0)
                    return true;
            }

            return false;
        }

        public virtual void SetAlarm(int alid)
        {
            driver.GEMSetAlarm(alid, 1);
        }

        public virtual void ClearAlarm(int alid)
        {
            driver.GEMSetAlarm(alid, 0);
        }

        public virtual void ReplyRemoteCommand(HcAck code, RemoteCommandArgs args)
        {
            driver.GEMRspRemoteCommand(args.MessageId, args.Command, (long)code, 0, new string[1], new long[1]);
        }

        public virtual void ReplyTransferCommand(HcAck code, TransferCommandArgs args)
        {
            ReplyCustomSecsMessage(code, args.Stream, args.Function, args.SystemByte);
        }

        public virtual void NotifyEvent(long ceid)
        {
            driver.GEMSetEvent(ceid);
        }

        public virtual void SetVariables(long[] vids, string[] values)
        {
            driver.GEMSetVariable(vids.Length, vids, values);
        }

        public virtual void SetListVariables(long vid, Variable[] data)
        {
            long objId = 0;

            driver.MakeObject(ref objId);

            foreach (var item in data)
                SetVariable(ref objId, item);

            driver.GEMSetVariables(objId, vid);
        }

        public virtual bool SetEstablishCommunication(bool state)
        {
            if (driver.GEMSetEstablish(state ? 1 : 0) == 0)
            {
                communicationEstablishState_ = (state ? CommunicationEstablishStates.Enabled : CommunicationEstablishStates.Disabled);
                return true;
            }

            return false;
        }

        public virtual void SetControlState(ControlStates toState)
        {
            switch (toState)
            {
                case ControlStates.EquipmentOffline: driver.GEMReqOffline(); break;
                case ControlStates.Local: driver.GEMReqLocal(); break;
                case ControlStates.Remote: driver.GEMReqRemote(); break;
                case ControlStates.HostOffline: driver.GEMReqHostOffline(); break;
            }

            Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod()}: ChangeTo={toState}");
        }

        public virtual bool SendTerminalMessage(string message)
        {
            if (driver.GEMSetTerminalMessage(0, message) == 0)
                return true;

            return false;
        }

        public virtual bool GetHostDateTime()
        {
            if (driver.GEMReqGetDateTime() == 0)
                return true;

            return false;
        }

        public virtual bool ReplyCustomSecsMessage(HcAck code, long nStream, long nFunction, long nSysbyte)
        {
            long nRspFunction = nFunction + 1;

            switch (nStream)
            {
                case 2:
                    {
                        switch (nFunction)
                        {
                            case 49:
                                {
                                    long objectId = 0;
                                    driver.MakeObject(ref objectId);
                                    driver.SetListItem(objectId, 2);
                                    driver.SetBinaryItem(objectId, (byte)code);

                                    switch (code)
                                    {
                                        case HcAck.ConfirmedWasExcuted:
                                        case HcAck.CmdNotExist:
                                        case HcAck.CmdNotAbleExecute:
                                        case HcAck.ConfirmedWillExcuted:
                                        case HcAck.AlreadyRequested:
                                        case HcAck.ObjNotExist:
                                            {
                                                driver.SetListItem(objectId, 0);
                                            }
                                            break;
                                        case HcAck.InvalidParam:
                                            {
                                                Debug.WriteLine("S2F50 - Invalid parameter");
                                                driver.SetListItem(objectId, invalidParameters.Count);

                                                foreach (var item in invalidParameters)
                                                {
                                                    driver.SetListItem(objectId, 2);
                                                    driver.SetStringItem(objectId, item.Key);
                                                    driver.SetBinaryItem(objectId, (byte)item.Value);
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }

                                    driver.SendSECSMessage(objectId, nStream, nRspFunction, nSysbyte);
                                }
                                break;
                        }
                    }
                    break;
            }

            return true;
        }

        public virtual void AddInvalidParameters(string[] CpName)
        {
            invalidParameters.Clear();

            for (int i = 0; i < CpName.Length; i++)
                invalidParameters.Add(CpName[i], InvalidParameterType.NoSuchObject);
        }

        public bool Save(string filename = null)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
