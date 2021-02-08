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
/// @file Enumerations.cs
/// @brief
/// @details
/// @date 2019, 1, 9, 오후 4:27
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using Marcus.Solution.TechFloor.Object;
#endregion

#region Program
namespace Marcus.Solution.TechFloor.Gem
{
    #region Enumerations
    // Glosaries
    // MCS (Material Control System)
    // 물류 반송 장치를 제어하고, 관리하는 물류 반송 시스템
    // AMHS (Automated Material Handling System)
    // 공장 내 물류의 저장 (Storage) 과 반송 (Transport) 을 위한 물류 반송 장치
    // Stocker-SEM (Specification for AMHS Storage Specific Equipment Model)
    // SEMI 에서 규정한 Stocker 운영을 위한 사양서
    // IB-SEM (Specification for Interbay / Intrabay AMHS SEM)
    // SEMI 에서 규정한 AGV, OHS 등의 반송 장비 운영을 위한 사양서
    // SC (Stocker Controller)
    // Stocker 내의 데이터를 관리하고 Device 를 제어하는 Controller
    // TSC (Transport System Controller)
    // 반송 장비를 제어하는 Controller, AGV Controller
    // AGV (Automated Guided Vehicle)
    // OHS (Overhead Shuttle)
    // Lift
    // 소로 다른 층간 장착된 반송에 쓰이는 반송 장비
    // RGV (Rail Guided Vehicle)
    // LP (Load Port)
    // OP (Output Port)
    // BP (Buffer Port)

    public enum CommunicationEstablishStates : int
    {
        Disabled,
        Enabled
    }

    public enum CommunicationStates : int
    {
        None = -1,
        CommDisabled = 1,
        WaitCRFromHost,
        WaitDelay,
        WaitCRA,
        Communicating,
    }

    public enum ControlStates : int
    {
        None = -1,
        EquipmentOffline = 1, // Event 송수신 불가능
        AttemptOnline,
        HostOffline,
        Local, // Event 수신만 가능, 반송은 MCP 에서 수행
        Remote // Event 송수신 가능, MCS 에서 반송 지시가 가능
    }

    public enum ControllerStates : int
    {
        Initialize, // 반송 명령을 처리하지 않음
        Paused, // 반송 명령을 수신한 경우, Queueing 하지만, Process 하지 않음
        Pausing, // 현재 진행중인 Carrier 만 반송 (Cycle Stop) 하고, 이후 반송 명령은 Queueing 하지만, Process 하지 않음
        Auto // 모든 반송 명령 수행
    }

    public enum HsmsDriverStates : int
    {
        Unknown = -1,
        Init = 0,
        Idle = 1,
        Setup = 2,
        Ready = 3,
        Execute = 4
    }

    public enum TransportStates : int
    {
        None, // Host 에서 반송 명령이 내려오고, 이것에 대해 Acknowledged 됨
        NotActive, // 물리적으로 반송명령이 실행 되지 않은 상태
        Queued, // Not Active 의 하위 상태이며, AGVC 에서 응답하여, 반송 명령만 저장한 상태
        Waiting, // Not Active 의 하위 상태이며, 반송 명령의 초기화가 이루어지는 상태
        Active, // 물리적으로 반송 명령이 실행되는 상태이며, Carrier 를 가지고 Source 에서 Destination 까지 움직이는 상태
        Transferring, // Active 의 하위 상태이며, 반송 명령이 멈추어 있는 상태
        Paused, // Active 의 하위 상태이며, 반송 명령이 멈추어 있는 상태
        Canceling, // Cancel 명령을 받아 반송 명령을 취소하는 상태
        Aborting, // 현재 실행 중인 반송 명령을 종료하는 상태
    }

    public enum TransferCommandStates : int
    {
        None,
        Ready,
        IfQueued,
        Queued,
        Fail,
        CancelFailed,
        AbortFailed,
        CancelQueued,
        AbortQueued,
        Initiated,
        Transferring,
        Paused,
        CancelInitiated,
        CancelCompleted,
        AbortInitiated,
        AbortCompleted,
        Completed,
        Resumed,
    }

    // TransportState flow
    // None -> Queued: Host 에서 반송 명령이 전송되고, 이를 Acknowledged 함
    // Queued -> Waiting: 반송 명령이 AGVC 에 의해 초기화 됨 (TransferInitated)
    // Waiting -> Transferring: 반송 명령 시작 (Transferring)
    // NotActive -> Canceling: 반송 명령에 대해 Cancel 내림 (TransferCancelInitiated)
    // Canceling -> Previous State: 반송 시스템이 반송명령에 대해 Cancel 불가 (TransferCancelFailed)
    // Transferring -> Paused: 비정상적인 상태에 대해 AGVC 가 Paused 를 실행 (TransferPaused)
    // Paused -> Transferring: 비정상적인 상태가 해소되어 AGVC 가 Resume 실행 (TransferResumed)
    // Active -> Aorting: Host 가 반송명령에 대해 Abort (TransferAbortInitiated)
    // Aborting -> None: AGVC 가 반송명령의 Abort 를 완료 (TransferAbortCompleted)
    // Active -> None: 반송 명령 완료, 반송 완료 (TransferCompleted 와 ResultCode 를 보낸다.)
    // Aborting -> Previous State: 물리적으로 Abort 불가 (TransferAbortFailed)
    // Canceling -> Transferring: 반송이 현재 진행 중 이므로, 해당 반송 명령에 관하여 Cancel 불가 (Transferring)

    public enum VehicleStates : int
    {
        Arrived,
        Installed, // AGVC 안에 들어와서 관리되는 상태
        Removed, // AGVC 에서 벗어난 상태
        Assigned, // 반송 명령의 수행이 이루어지는 단계
        NotAssigned, // 반송 명령 수행이 이루어지지 않은 상태
        Enroute, // 반송을 위해 이동 중이며, 이동 경로가 설정이 되어 있는 상태
        Parked, // Active 의 하위 상태이며, Vehicle 도착 완료 후 움직임을 취하기 전 상태, Vehicle 출발 전, 움직임을 취하기 전 상태
        AcquireStarted,
        Acquiring, // Assigned 의 하위 상태이며, 현재 Carrier 를 Vehicle 에 싣고 있는 상태
        AcquireCompleted,
        Departed,
        DepositStarted,
        Depositing, // Assigned 의 하위 상태이며, 현재 Carrier 를 내리고 있는 상태
        DepositCompleted,
    }

    // VehicleState flow
    // Enroute -> Parked: 실행중인 반송 명령에 대해 Vehicle Arrived (VehicleArrived)
    // Parked -> Enroute: 실행중인 반송 명령에 대해 Vehicle Departed (VehicleDeparted)
    // Parked -> Acquiring: 도착 한 Transfer unit 과 Carrier Acquire I/O 통신 (VehicleAcquireStart)
    // Acquiring -> Parked: 도착 한 Transfer unit 과 Carrier Acquire I/O 통신 (VehicleAcquireCompleted)
    // Parked -> Depositing: 도착 한 Transfer unit 과 Carrier Dsposit I/O 통신 (VehicleDepositStart)
    // Depositing -> Parked: 도착 한 Transfer unit 과 Carrier Deposit I/O 통신 (VehicleDepositCompleted)
    // Acquiring -> Depositing: 도착 한 Transfer unit 과 Carrier Acquired 완료 I/O 통신 (VehicleDepositStart)
    // Depositing -> Acquiring: 도착 한 Transfer unit 과 Carrier Deposited 완료 I/O 통신 (VehicleAcquireStart)
    // Assigned -> NotAssigned: Vehicle 반송 명령 완료 (VehicleUnassigned)
    // NotAssigned -> Assigned: 반송 명령 수행을 위해 Vehicle Assign (VehicleAssigned)
    // Installed -> Removed: Vehicle 이 Remove 됨 (VehicleRemoved)
    // Removed -> Installed: 새로운 Vehicle 이 Install 됨 (VehicleInstalled)

    public enum CarrierStates : int
    {
        None, // Carrier 가 취합되지 않은 상태
        Installed, // AGV vehicle 에 Carrier 가 들어 옴 (AGVC DB 에 입력 됨)
        Removed
    }

    public enum EquipmentConstantIds : int
    {
        Equipment_Initiated_Connected,
        EstablishCommunicationsTimeout,
        MaxSpoolTransmit,
        OverWriteSpool,
        EnableSpooling,
        TimeFormat,
        Maker,
        T3Timeout,
        T5Timeout,
        T6Timeout,
        T7Timeout,
        T8Timeout,
        InitControlState,
        OffLineSubState,
        OnLineFailState,
        OnLineSubState,
        MaxSpoolMsg,
        DeviceID,
        IPAddress,
        PortNumber,
        ActiveMode,
        LinkTestInterval,
        RetryLimit
    }

    public enum CollectionEventIds : int
    {   // Controller Status Events
        OperatorCommand, // 1
        EquipmentConstantChanged, // 2
        ProcessProgramChanged, // 3
        ProcessRecipeSelected, // 4
        SpoolActivated, // 5
        SpoolDeactivated, // 6
        SpoolTransmitFailure, // 7
        MessageRecognition, // 8
        Offline, // 9 (Customer: Amkor)
        Local, // 10 (Customer: Amkor)
        Remote, // 11 (Customer: Amkor)

        ControlStatusChange, // 1001 (States: NONE -> ONLINE-LOCAL, ONLINE-REMOTE, OFFLINE)
        
        // AGVC State Transition Event
        AGVCAutoComplete, // 2001 (States: Paused, Pausing -> Auto)
        AGVCAutoInitiated, // 2002 (States: NONE -> AGVC INIT)
        AGVCPauseInitiated, // 2003 (States: NONE -> AGVC INIT)
        AGVCPauseCompleted, // 2004 (States: Pausing -> Paused)
        AGVCPaused, // 2005 (States: AGVC INIT -> Paused)
        AlarmSet, // 2006 (States: No alarms, Alarms -> Alarms)
        AlarmClear, // 2007 (States: Alarms -> No alarms)

        UnitAlarmSet, // 2008
        UnitAlarmClear, // 2009

        // Transfer Command Transaction Event
        TransferAbortInitiated, // 3001 (States: ACTIVE -> ABORTING)
        TransferAbortCompleted, // 3002 (States: ABORTING -> NONE)
        TransferAbortFailed, // 3003 (States: ABORTING -> ACTIVE)
        TransferCancelInitiated, // 3004 (States: QUEUED -> CANCELLING)
        TransferCancelCompleted, // 3005 (States: CANCELLING -> NONE)
        TransferCancelFailed, // 3006 (States: CANCELLING -> QUEUED)
        TransferInitiated, // 3007 (States: QUEUED -> TRANSFERRING)
        TransferCompleted, // 3008 (States: ACTIVE -> NONE)
        TransferPaused, // 3009 (States: TRANSFERRING -> PAUSED)
        TransferResumed, // 3010 (States: PAUSED -> TRANSFERRING)
        Transferring, // 3011 (States: Waiting, Canceling -> Transferring)

        //Vehicle State Transition Event
        VehicleArrived, // 4001 (States: Enroute -> Parked)
        VehicleAcquireStarted, // 4002 (States: Parked, Depositing -> Acquired)
        VehicleAcquireCompleted, // 4003 (States: Acquiring -> Parked)
        VehicleAssigned, // 4003 (States: NotAssigned -> Assigned)
        VehicleDeparted, // 4005 (States: Parked -> Enroute)
        VehicleDepositStarted, // 4006 (States: Parked, Acquiring -> Depositing)
        VehicleDepositCompleted, // 4007 (States: Depositing -> Parked)
        VehicleInstalled, // 4008 (States: Removed -> Installed)
        VehicleRemoved, // 4009 (States: Installed -> Removed)
        VehicleUnassigned, // 4010 (States: Assigned -> NotAssigned)
        VehicleStateChanged, // 4011
        VehicleTransferring, // 4012

        // Carrier State Transition Event
        MaterialReceived, // 5001 (States: None -> Installed)
        MaterialRemoved, // 5002 (States: None -> Installed)

        // Non-Transition Event
        PortInfoChange, // 6001 (States: None -> None)
        ConvUnitStateChange, // 7003 (States: None -> None)
        ConvUnitStatusChange, // 7004 (States: None -> None)
    }

    public enum VariableIds : int
    {
        CommState,
        PrevioousCommState,
        MDLN,
        SOFTREV,
        ChangedECID,
        EventLimit,
        LimitVariable,
        TransitionType,
        PPChangeName,
        PPChangeStatus,
        PPError,
        PreviousControlState,
        OperatorCommand,
        SpoolStatus,
        SpoolStartTime,
        SpoolFullTime,
        SpoolCountTotal,
        SpoolCountActual,
        SpoolFull,
        TscName,
        TscState,
        ActiveAlarm,
        ActiveUnitAlarm,
        AlarmInfo,
        UnifAlarmInfo,
        BatchId,
        BatchSequence,
        BatchTransferCommand,
        BatchCommandInfo,
        CarrierType,
        ReOrder,
        ProcessState,
        PreviousProcessState,
        EventsEnabled,
        AlarmsEnabled,
        ALCD,
        AlarmSet,

        // Common
        Clock, // 1001 (EventTime)
        CommandId, // 1002
        Priority, // 1003
        ControlState, // 1004
        ActiveCarriers, // 1005 (List)
        ActiveTransfers, // 1006 (List)
        ActiveVehicles, // 1007 (List)
        AlarmID, // 1008
        ErrorId, // 1009

        // SEM Carrier
        Carrierid, // 2001
        CarrierLoc, // 2002
        SourcePort, // 2003
        DestPort, // 2004
        CarrierInfo, // 2005 (List)
        LotId, // 2030
        HandoffType, // 2200

        // SEM command
        CommandName, // 4001
        CommandType, // 4002
        InstallTime, // 4003
        Replace, // 4004
        ResultCode, // 4005
        TransferPort, // 4006
        TransferState, // 4007
        AGVCState, // 4008
        CommandInfo, // 4009 (List)
        TransferCommand, // 4010
        TransferInfo, // 4011
        TransferPortList, // 4012

        // SEM AGV
        AGVCName, // 5001 (AGV Controller Name)
        VehicleId, // 5002 (AGV Name)
        VehicleState, // 5003
        VehicleInfo, // 5009 (List)
        RecoveryOption, // 5010 (Retry, Abort)
        WayPoint, // 5011

        // SEM Unit
        ConveyorName, // 6001 (Unique id of the conveyor)
        PortID, // 6002 (Lifter cover tray port or target equipment port)
        OperatorId, // 6003
        PortStatus, // 6004 (LOAD/UNLOAD)
        WaitLotCount, // 6005 (MCS 에서는 0 으로 처리, 0 = 0 Lot/Carrier wait)
        ConveyorUnitState, // 6006 (0 = Normal, 1 = PM, 2 = Abnormal)
        ConveyorUnitStatus, // 6007 (0 = Empty, 1 = Full)
    }

    public enum AlarmIds : int
    {

    }

    public enum VariableTypes
    {
        StatusVariable,
        DataVariable
    }

    public enum RemoteCommands : int
    {
        Abort,
        Cancel,
        Pause,
        Resume,
        Transfer,
        Move,
        Update,
    }

    public enum RemoveCommandParameters : int
    {
        Commandid,
        CommandInfo,
        TransferInfo,
        SourcePort,
        SourceZoneId, // Zone 간 AVG 이동
        DestPort,
        DestZoneId,
    }

    public enum AlarmStates : int
    {
        Cleared,
        Set,
    }

    public enum VariableDataType : int
    {
        List,
        Ascii,
        Bin,
        Bool,
        Uint1,
        Uint2,
        Uint4,
        UInt8,
        Int1,
        Int2,
        Int4,
        Int8,
        Float4,
        Float8,
    }

    public enum InvalidParameterType : int
    {
        HasBeen,
        NotExist,
        CannotPerformNow,
        AtLeastOneParameterIsInvalid,
        CommandWillBePerformedWithCompletionSignaledLaterByAnEvent,
        AlreadyInDesired,
        NoSuchObject,
    }

    public enum HcAck : int
    {
        ConfirmedWasExcuted,
        CmdNotExist,
        CmdNotAbleExecute,
        InvalidParam,
        ConfirmedWillExcuted,
        AlreadyRequested,
        ObjNotExist,
    }

    public enum VariableDataListType : int
    {
        EnhancedCarriers,
        EnhancedTransfers,
        EnhancedVehicles,
        EnhancedUnitAlarms,
        VehicleInfo,
        TransferCompleteInfo,
        CommandInfo,
    }

    public enum TransferCommands : int
    {
        Transfer,
        Cancel,
        Abort,
        Update,
        Move,
        BatchTransfer,
        StageCommand,
    }

    public enum Manufacturer : int
    {
        Linkgenesis,
    }
    #endregion
}
#endregion