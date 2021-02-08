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
/// project Sec 
/// @author Marcus (isadrastea.kor@gmail.com)
/// @namespace Marcus.Solution.TechFloor.Gem.Sec
/// @file SecHsmsDriverLinkgenesis.cs
/// @brief
/// @details
/// @date 2019, 1, 10, 오후 2:17
///////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Marcus.Solution.TechFloor.Gem;
using Marcus.Solution.TechFloor.Object;

namespace Marcus.Solution.TechFloor.Gem.Sec
{
    public class SecHsmsDriverLinkgenesis : AbstractClassHsmsDriverLinkgenesis
    {
        #region Fields
        // Host service definition table
        private string ecids_ =
        "Equipment_Initiated_Connected,     1,          true\n" +
        "EstablishCommunicationsTimeout,    2,          true\n" +
        "MaxSpoolTransmit,                  3,          true\n" +
        "OverWriteSpool,                    4,          true\n" +
        "EnableSpooling,                    5,          true\n" +
        "TimeFormat,                        6,          true\n" +
        "Maker,                             7,          true\n" +
        "T3Timeout,                         8,          true\n" +
        "T5Timeout,                         9,          true\n" +
        "T6Timeout,                         10,         true\n" +
        "T7Timeout,                         11,         true\n" +
        "T8Timeout,                         12,         true\n" +
        "InitControlState,                  13,         true\n" +
        "OffLineSubState,                   14,         true\n" +
        "OnLineFailState,                   15,         true\n" +
        "OnLineSubState,                    16,         true\n" +
        "MaxSpoolMsg,                       17,         true\n" +
        "DeviceID,                          18,         true\n" +
        "IPAddress,                         19,         true\n" +
        "PortNumber,                        20,         true\n" +
        "ActiveMode,                        21,         true\n" +
        "LinkTestInterval,                  22,         true\n" +
        "RetryLimit,                        23,         true\n";

        private string ceids_ =
        "OperatorCommand,                   1,          false\n" +
        "EquipmentConstantChanged,          2,          false\n" +
        "ProcessProgramChanged,             3,          false\n" +
        "ProcessRecipeSelected,             4,          false\n" +
        "SpoolActivated,                    5,          false\n" +
        "SpoolDeactivated,                  6,          false\n" +
        "SpoolTransmitFailure,              7,          false\n" +
        "MessageRecognition,                8,          false\n" +
        "Offline,                           9,          false\n" +
        "Local,                             10,         false\n" +
        "Remote,                            11,         false\n" +
        "ControlStatusChange,               1001,       true \n" +
        "AGVCAutoComplete,                  2001,       true \n" +
        "AGVCAutoInitiated,                 2002,       true \n" +
        "AGVCPauseInitiated,                2003,       true \n" +
        "AGVCPauseCompleted,                2004,       true \n" +
        "AGVCPaused,                        2005,       true \n" +
        "AlarmSet,                          2006,       true \n" +
        "AlarmClear,                        2007,       true \n" +
        "UnitAlarmSet,                      2008,       false\n" +
        "UnitAlarmClear,                    2009,       false\n" +
        "TransferAbortInitiated,            3001,       true \n" +
        "TransferAbortCompleted,            3002,       true \n" +
        "TransferAbortFailed,               3003,       true \n" +
        "TransferCancelInitiated,           3004,       true \n" +
        "TransferCancelCompleted,           3005,       true \n" +
        "TransferCancelFailed,              3006,       true \n" +
        "TransferInitiated,                 3007,       true \n" +
        "TransferCompleted,                 3008,       true \n" +
        "TransferPaused,                    3009,       true \n" +
        "TransferResumed,                   3010,       true \n" +
        "Transferring,                      3011,       true \n" +
        "VehicleArrived,                    4001,       true \n" +
        "VehicleAcquireStarted,             4002,       true \n" +
        "VehicleAcquireCompleted,           4003,       true \n" +
        "VehicleAssigned,                   4004,       true \n" +
        "VehicleDeparted,                   4005,       true \n" +
        "VehicleDepositStarted,             4006,       true \n" +
        "VehicleDepositCompleted,           4007,       true \n" +
        "VehicleInstalled,                  4008,       true \n" +
        "VehicleRemoved,                    4009,       true \n" +
        "VehicleUnassigned,                 4010,       true \n" +
        "VehicleStateChanged,               4011,       true \n" +
        "VehicleTransferring,               4012,       true \n" +
        "MaterialReceived,                  5001,       true \n" +
        "MaterialRemoved,                   5002,       true \n" +
        "PortInfoChange,                    6001,       false\n" +
        "ConvUnitStateChange,               7003,       false\n" +
        "ConvUnitStatusChange,              7004,       false\n";

        private string vids_ =
        "CommState,                         100,        StatusVariable\n" +
        "PrevioousCommState,                101,        StatusVariable\n" +
        "MDLN,                              102,        StatusVariable\n" +
        "SOFTREV,                           103,        StatusVariable\n" +
        "ChangedECID,                       104,        DataVariable\n" +
        "EventLimit,                        105,        DataVariable\n" +
        "LimitVariable,                     106,        DataVariable\n" +
        "TransitionType,                    107,        DataVariable\n" +
        "PPChangeName,                      108,        DataVariable\n" +
        "PPChangeStatus,                    109,        StatusVariable\n" +
        "PPError,                           110,        DataVariable\n" +
        "PreviousControlState,              111,        StatusVariable\n" +
        "OperatorCommand,                   112,        DataVariable\n" +
        "SpoolStatus,                       113,        StatusVariable\n" +
        "SpoolStartTime,                    114,        StatusVariable\n" +
        "SpoolFullTime,                     115,        StatusVariable\n" +
        "SpoolCountTotal,                   116,        StatusVariable\n" +
        "SpoolCountActual,                  117,        StatusVariable\n" +
        "SpoolFull,                         118,        StatusVariable\n" +
        "TscName,                           119,        StatusVariable\n" +
        "TscState,                          120,        StatusVariable\n" +
        "ActiveAlarm,                       121,        StatusVariable\n" +
        "ActiveUnitAlarm,                   122,        StatusVariable\n" +
        "AlarmInfo,                         123,        StatusVariable\n" +
        "UnifAlarmInfo,                     124,        StatusVariable\n" +
        "BatchId,                           125,        DataVariable\n" +
        "BatchSequence,                     126,        DataVariable\n" +
        "BatchTransferCommand,              127,        DataVariable\n" +
        "BatchCommandInfo,                  128,        DataVariable\n" +
        "CarrierType,                       129,        DataVariable\n" +
        "ReOrder,                           130,        DataVariable\n" +
        "ProcessState,                      131,        StatusVariable\n" +
        "PreviousProcessState,              132,        StatusVariable\n" +
        "EventsEnabled,                     133,        StatusVariable\n" +
        "AlarmsEnabled,                     134,        StatusVariable\n" +
        "ALCD,                              135,        DataVariable\n" +
        "AlarmSet,                          136,        StatusVariable\n" +
        "Clock,                             1001,       StatusVariable\n" +
        "CommandId,                         1002,       DataVariable\n" +
        "Priority,                          1003,       StatusVariable\n" +
        "ControlState,                      1004,       StatusVariable\n" +
        "ActiveCarriers,                    1005,       StatusVariable\n" +
        "ActiveTransfers,                   1006,       StatusVariable\n" +
        "ActiveVehicles,                    1007,       StatusVariable\n" +
        "AlarmID,                           1008,       DataVariable\n" +
        "ErrorId,                           1009,       DataVariable\n" +
        "Carrierid,                         2001,       DataVariable\n" +
        "CarrierLoc,                        2002,       DataVariable\n" +
        "SourcePort,                        2003,       StatusVariable\n" +
        "DestPort,                          2004,       StatusVariable\n" +
        "CarrierInfo,                       2005,       StatusVariable\n" +
        "LotId,                             2030,       DataVariable\n" +
        "HandoffType,                       2200,       StatusVariable\n" +
        "CommandName,                       4001,       DataVariable\n" +
        "CommandType,                       4002,       DataVariable\n" +
        "InstallTime,                       4003,       StatusVariable\n" +
        "Replace,                           4004,       DataVariable\n" +
        "ResultCode,                        4005,       DataVariable\n" +
        "TransferPort,                      4006,       DataVariable\n" +
        "TransferState,                     4007,       DataVariable\n" +
        "AGVCState,                         4008,       StatusVariable\n" +
        "CommandInfo,                       4009,       StatusVariable\n" +
        "TransferCommand,                   4010,       StatusVariable\n" +
        "TransferInfo,                      4011,       StatusVariable\n" +
        "TransferPortList,                  4012,       DataVariable\n" +
        "AGVCName,                          5001,       DataVariable\n" +
        "VehicleId,                         5002,       DataVariable\n" +
        "VehicleState,                      5003,       DataVariable\n" +
        "VehicleInfo,                       5009,       StatusVariable\n" +
        "RecoveryOption,                    5010,       DataVariable\n" +
        "WayPoint,                          5011,       StatusVariable\n" +
        "ConveyorName,                      6001,       StatusVariable\n" +
        "PortID,                            6002,       DataVariable\n" +
        "OperatorId,                        6003,       DataVariable\n" +
        "PortStatus,                        6004,       DataVariable\n" +
        "WaitLotCount,                      6005,       StatusVariable\n" +
        "ConveyorUnitState,                 6006,       StatusVariable\n" +
        "ConveyorUnitStatus,                6007,       StatusVariable\n";
        #endregion

        #region Events
        public override event EventHandler<string> RuntimeTrace;
        public override event EventHandler ChangedControlState;
        #endregion

        #region Constructors
        public SecHsmsDriverLinkgenesis() : base()
        {
        }

        #endregion

        #region Protected methods
        protected override void DefineIdentifiers()
        {
            // Collection event
            string[] elements = ecids_.Split('\n');

            foreach (string element in elements)
            {
                string[] items = element.Split(',');

                if (items.Length >= 3)
                {
                    equipmentConstantTable.Add((int)Enum.Parse(typeof(EquipmentConstantIds), items[0]),
                        new Pair<int, bool>(Convert.ToInt32(items[1]), Convert.ToBoolean(items[2])));
                }
            }

            elements = ceids_.Split('\n');

            foreach (string element in elements)
            {
                string[] items = element.Split(',');

                if (items.Length >= 3)
                {
                    collectionEventTable.Add((int)Enum.Parse(typeof(CollectionEventIds), items[0]),
                        new Pair<int, bool>(Convert.ToInt32(items[1]), Convert.ToBoolean(items[2])));
                }
            }

            elements = vids_.Split('\n');

            foreach (string element in elements)
            {
                string[] items = element.Split(',');

                if (items.Length >= 3)
                {
                    variableTable.Add((int)Enum.Parse(typeof(VariableIds), items[0]),
                        new Pair<int, VariableTypes>(Convert.ToInt32(items[1]), (VariableTypes)Enum.Parse(typeof(VariableTypes), items[2])));
                }
            }

            // Alarm table
        }

        protected override void OnDriverErrorEvent(string sErrorName, long nErrorCode)
        {
            RuntimeTrace?.Invoke(this, $"{ClassName}.{MethodBase.GetCurrentMethod()}: Error={sErrorName},Code={nErrorCode}");
        }

        protected override void OnDriverStateEvent(long nState)
        {
            base.OnDriverStateEvent(nState);
            RuntimeTrace?.Invoke(this, $"{ClassName}.{MethodBase.GetCurrentMethod()}: DriverState={DriverState}");
        }

        protected override void OnChangedCommunicationState(long nState)
        {
            base.OnChangedCommunicationState(nState);
            RuntimeTrace?.Invoke(this, $"{ClassName}.{MethodBase.GetCurrentMethod()}: State={CurrentCommunicationState}");
        }

        protected override void OnChangedControlState(long nState)
        {
            try
            {
                base.OnChangedControlState(nState);

                long[] vids = { variableTable[(int)VariableIds.ControlState].first };
                string[] values = { nState.ToString() };

                SetVariables(vids, values);
                NotifyEvent(CollectionEventIds.ControlStatusChange);
                ChangedControlState?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }
        }
        #endregion

        #region Public methods
        public virtual void NotifyEvent(CollectionEventIds ceid)
        {
            try
            {
                NotifyEvent(collectionEventTable[(int)ceid].first);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
