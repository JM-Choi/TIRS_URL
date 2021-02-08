#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Program
namespace TechFloor
{
    #region Enumerations
    public enum OperationStates
    {
        Alarm = -1,
        PowerOff,
        PowerOn,
        Initialize,
        Calibrate,
        Setup,
        Run,
        Stop,
        Pause
    }

    public enum AlarmStates
    {
        Pending, // Set
        Cleared // Reset
    }

    public enum Buttons
    {
        Ok,
        OkCancel,
        YesNo,
        RetryCancel,
        YesNoCancel,
        AbortRetryIgnore
    }

    public enum Icons
    {
        Application,
        Asterisk,
        Error,
        Exclamation,
        Hand,
        Information,
        Question,
        Warning,
        WinLogo,
        Shield
    }

    public enum UserGroup
    {
        Operator,
        Engineer,
        Manager,
        Adminstrator
    }

    public enum InitializeMode
    {
        All,
        Robot,
        Aligner,
        ReelTowerGroup,
    }

    public enum AutomaticTeachMode
    {
        All,
        Load,
        Unload,
    }

    public enum CalibrationMode
    {
        All,
        TowerBasePoints,
        CartGuidePoints,
    }

    public enum HomeModule
    {
        Robot,
        Aligner,
        ReelTowerGroup,
    }

    public enum AutomaticTeachModule
    {
        Robot,
    }

    public enum NetworkNodeModes : int
    {
        Client,
        Server,
        Router,
        Repeater,
        Mediator
    }

    public enum InterfaceTypes : int
    {
        Ethernet,
        Serial,
        Usb,
    }

    public enum SerialInterfaces : int
    {
        RS232,
        RS422,
        RS485,
        I2C,
        Hid,
    }

    public enum FieldBus : int
    {
        ModBus,
        ModBusOverTcp,
        CanBus,
        ProfiBus,
        CCLink,
        MechatroLink,
        EtherCat,
    }

    public enum IoBus : int
    {
        DigitalIo,
        AnalogIo
    }

    public enum VisionProcessItems : int
    {
        Alignment = 1,  // CONST_MODE_REEL_ALIGN
        Barcode,        // CONST_MODE_DECODE_REEL_BARCODE
        Size            // CONST_MODE_CHECK_REEL_SIZE
    }

    public enum ReelUnloadReportStates : int
    {
        Unknown = -1,
        Waiting,
        Ready,
        Run,
        Complete,
    }

    public enum ReelTowerStates
    {
        Unknown,
        Run, // Load <-> Unload (Busy)
        Load,
        Unload,
        Down, // After error
        Error, // Alert
        Wait,
        Full, // Ok (Not allowed to load)
        Idle, // Available to load/unload
    }

    public enum ProcessCategories
    {
        ImageProcess,
        MaterialStorage,
        MaterialHandler,
    }

    public enum ImageProcessTaskTypes
    {
        PatternMatching,
        DecodeBarcode,
        ImageDeviceCalibration,
        FindCenterOfReel,
        SearchCenterOfCircle,
        DecodeQrBarccodes,
        Decode1DBarcodes,
        Decode2DBarcodes,
        ReadCharacters,
        ToolCalibration,
    }

    public enum ImageProcssingResults
    {
        Unknown = -1,
        Success,
        Exception,
        Empty,
        OverScope,
    }

    public enum ImageProcessRoiTypes
    {
        WholeArea,
        Rectangle,
        Polygon,
        Circle,
        Ellipse,
        Ring,
    }

    public enum ImageProcessArithmeticOperationTypes
    {
        Plus,
        Subtract,
    }

    public enum ImageProcessConstraintRegionTypes
    {
        WithinRoi,
    }

    public enum ImageProcessConstraintOperation
    {
        Below,
        MoreThan,
        Under,
        Excess,
    }

    public enum ReelTowerStateControlModes
    {
        QueryReelTowerStateById,
        QueryReelTowerStateByAtOnce,
        NotUseReelTowerStateQuery,
    }

    public enum ImageProcessorTypes
    {
        Mit,
        TechFloor
    }

    public enum ImageDeviceFormats
    {
        Mono8,
        Mono16,
        Color,
    }

    public enum DisplayUnits
    {
        um,
        mm,
        cm,
        m
    }

    public enum ImageDeviceTriggerTypes
    {
        Software,
        Hardware
    };

    public enum VisionControlStates
    {
        Shutdown = -1,
        Started,
        Stopped,
        AnalyzedResult,
        Live,
        RunOnce,
        ContinueRun,
    }

    public enum FileExtensions
    {
        Bin,
        Soap,
        Xml,
        Json,
        csv,
        ini,
        inf,
    }

    public enum ComparisonOperators
    {
        EqualTo,
        NotEqualTo,
        LessThanOrEqualTo,
        LessThan,
        GreaterThanOrEqualTo,
        GreaterThan
    }

    public enum ReelTowerOperationModes
    {
        None,
        Load,
        Unload
    }

    public enum RobotActionOrder
    {
        None,
        LoadReelFromCart,
        LoadReelFromReturn,
        UnloadReelFromTower,
        CartReelToReject,
        ReturnReelToReject,
        UnloadReelToReject,
        CartAlign,
        CartWorkSlotAlign,
        CartWorkSlotCenterAlign,
        RobotOriginAlign,
    }

    public enum RobotActionStates
    {
        Unknown = -1,
        Stop,
        Load,
        Loading,
        LoadCompleted,
        Unload,
        Unloading,
        UnloadCompleted,
    }

    public enum RobotOperationOrderBy
    {
        None,
        Operator, // From return stage
        Automatic // From reel cart
    }

    public enum RobotCommunicationStates
    {
        Unknown,
        AskTask,                // Task
        AskCartCheck,           // RICV
        AskWorkSlotNo,          // CNo
        AskVisionCheck,         // VisionCheck
        AskStageNo,             // UnNo
        AskTowerNo,             // TNo
        AskReelSize,            // ReelSize
        ReportPickFail,         // PickFail
        ReportLoading,          // Loading
        ReportUnloading,        // Unloading
        ReportUnPickFail,       // UnPickFail
        Complete,               // Done
    }

    public enum RobotActionFailures
    {
        None,
        PickupFailureToLoadReel,
        PickupFailureToUnloadReel,
        VisionFailureToAdjust,
    }

    public enum CartPresentStates
    {
        Unknown,
        NotExist,
        Transferring,
        Exist,
    }

    public enum BarcodeInputStates
    {
        Wait,
        Typed,
        Canceled,
    }

    public enum MobileRobotControlItemStates
    {
        Unknown = -2,
        Interrupted = -1,
        Waiting,    //Yet
        Started,    //Started
        Process,    //In_progress
        Completed,  //Success  
        Stopped,
        Error,      //Failed
        //canceled , skipped < 추후 추가 사항>
    }

    public enum MobileRobotCommandTypes
    {
        QueuedCommand,
        InstantCommand,
    }

    public enum ErrorMessageTypes
    {
        Error,
        SetupError,
        CommandError,
    }

    public enum NotificationTypes
    {
        ChangedMap,
        ChangedConfiguration,
        ChargeBattaryRequest,
        PressedEmergency,
        ReleasedEmergentcy,
        DisabledMotors,
        AlertNotification,
        InterruptNotification,
        DockingNotification
    }

    public enum FaultMessageTypes
    {
        RobotApplicationFault,
        DrivingApplicationFault,
        OverTemperatureFault,
        LowBattaryFault,
        EncoderFault,
        GryroFault,
    }

    public enum SubCommandTypes : int
    {
        Goto_Going,
        Goto_Arrived,
        Gostraight_Going,
        Gostraight_Arrived,

    }

    public enum MobileRobotCommands : int
    {
        CommandType_none = -1,
        CommandType_analogInputList,
        CommandType_analogInputQueryRaw,
        CommandType_analogInputQueryVoltage,
        CommandType_applicationFaultClear,
        CommandType_applicationFaultQuery,
        CommandType_applicationFaultSet,
        CommandType_arclSendText,
        CommandType_configAdd,
        CommandType_configParse,
        CommandType_configStart,
        CommandType_connectOutgoing,
        CommandType_createInfo,
        CommandType_dock,
        CommandType_doTask_setheading,
        CommandType_doTaskInstant,
        CommandType_echo,
        CommandType_enableMotors,
        CommandType_executeMacro,
        CommandType_extIOAdd,
        CommandType_extIODump,
        CommandType_extIODumpLocal,
        CommandType_extIOInputUpdate,
        CommandType_extIOInputUpdateBit,
        CommandType_extIOInputUpdateByte,
        CommandType_extIOOutputUpdate,
        CommandType_extIOOutputUpdateBit,
        CommandType_extIOOutputUpdateByte,
        CommandType_extIORemove,
        CommandType_faultsGet,
        CommandType_getConfigSectionInfo,
        CommandType_getConfigSectionList,
        CommandType_getConfigSectionValues,
        CommandType_getDataStoreFieldInfo,
        CommandType_getDataStoreFieldList,
        CommandType_getDataStoreFieldValues,
        CommandType_getDataStoreGroupInfo,
        CommandType_getDataStoreGroupList,
        CommandType_getDataStoreGroupValues,
        CommandType_getDataStoreTripGroupList,
        CommandType_getDateTime,
        CommandType_getGoals,
        CommandType_getInfoList,
        CommandType_getInfo,
        CommandType_getMacros,
        CommandType_getRoutes,
        CommandType_goto,
        CommandType_inputList,
        CommandType_inputQuery,
        CommandType_log,
        CommandType_mapObjectInfo,
        CommandType_mapObjectList,
        CommandType_mapObjectTypeInfo,
        CommandType_mapObjectTypeList,
        CommandType_newConfigParam,
        CommandType_newConfigSectionComment,
        CommandType_odometer,
        CommandType_odometerReset,
        CommandType_oneLineStatus,
        CommandType_outputList,
        CommandType_outputOff,
        CommandType_outputOn,
        CommandType_outputQuery,
        CommandType_patrol,
        CommandType_patrolOnce,
        CommandType_patrolResume,
        CommandType_payloadQuery,
        CommandType_payloadQueryLocal,
        CommandType_payloadRemove,
        CommandType_payloadSet,
        CommandType_payloadSlotCount,
        CommandType_payloadSlotCountLocal,
        CommandType_play,
        CommandType_pupupSimple,
        CommandType_queryDockStatus,
        CommandType_queryFaults,
        CommandType_queryMotors,
        CommandType_queueCancel,
        CommandType_queueCancelLocal,
        CommandType_queueDropoff,
        CommandType_queueModify,
        CommandType_queueModifyLocal,
        CommandType_queueMulti,
        CommandType_queuePickup,
        CommandType_queuePickupDropoff,
        CommandType_queueQuery,
        CommandType_queueQueryLocal,
        CommandType_queueShow,
        CommandType_queueShowCompleted,
        CommandType_queueShowRobot,
        CommandType_queueShowRobotLocal,
        CommandType_quit,
        CommandType_say,
        CommandType_shutDownServer,
        CommandType_status,
        CommandType_stop,
        CommandType_tripReset,
        CommandType_undock,
        CommandType_updateInfo,
        CommandType_waitTaskCancel,
        CommandType_waitTaskState,
        CommandType_localizeatgoal,
        CommandType_localizetopoint,
        CommandType_doTask_gotostraight,
    }

    public enum InstantMode
    {
        QueuedCmd,
        InstantCmd,
    }

    public enum EventType
    {
        None = -1,
        Locker_close,
        Locker_opne,
        Item_unload,
        Item_load,
        Unlock,
        Lock,
    }

    public enum MobileRobotMode
    {
        Error,
        Idle,
        Run,
        Takeing_elevator,
        Getting_off_elevator,
        Dock,
        Disconnected,
    }

    public enum MobileRobotStates : int
    {
        None,
        //Moters_Enabled,
        Stopping,
        Stopped,
        Arrived_At,
        Arrived_At_Point,
        Completed_Doing_Task_DeltaHeading,
        Going_To,
        Going_To_Point,
        //Teleop_Driving,
        //Teleop_Driving_UnSafely,
        Doing_Task_DeltaHeading,
        Failed_Going_To_Goal,
        Failed_To_Get_To,
        Estop_Pressed,
        Estop_Relieved_But_Moters_Still_Disabled,
        No_GoalName,
        Can_Not_Find_Path,
        DockingState_Not,
        DockingState_Bulk,
        DockingState_Overcharge,  //충전중
        DockingState_Going_To,
        DockingState_Driving_Into_Dock,
        DockingState_Undocking, //충전 완료 후 나오는 상태
        DockingState_Cannot_Drive_To_Dock,  //충전실패
                                            //Unknown = -3,
                                            //Alert = -2,
                                            //Paused = -1,
                                            //Stopped = 0,
                                            //Run,
    }

    public enum CommandExecuteStates
    {
        Interrupted = -1,
        Wait,       // Yet
        Started,    // In_progress
        Completed,  // Success
        Error,      // Failed
    }

    public enum ServiceBrokerSubSteps
    {
        Prepare,
        Process,
        ProcessInterrupt,
        Post
    }

    public enum ControlCommandStates
    {
        Start,
        Running,
        Finish
    }

    public enum ItemLoadyn
    {
        on,
        off,
    }

    public enum LockerOpenyn
    {
        on,
        off,
    }

    public enum Lockyn
    {
        on,
        off,
    }

    public enum ResponseCodes : int
    {
        NotAcknowledge,
        Acknowledge,
        ServiceIsNotAvailable = -1,
        MobileRobotIsNotAvailable = -2,
        MobileRobotIsPaused = -3,
        MobileRobotIsBusy = -4,
        NotAccessible = -5,
        ProcessDelay = -6,
        FailedToExecute = -7,
    }

    public enum MobileRobotTaskStates : int
    {
        None,
        GoToPickup,
        WaitForPlaceCompleteSignal,
        GoToService,
        WaitForServiceCompleteSignal,
        GoToStandBy,
        WaitForWackupSignal,
        GoToCharge,
        WaitForChargeCompleteSignal,
    }

    public enum PlayType : int
    {
        PickupPlaceArrived,
        Locker,
        Interrupted,
        DeliveryArrived,
        DockStarted,
    }

    public enum ControlTypes
    {
        Route,
        Rotation,
        Tts,
        Lock,
        Unlock,
        Change_Floor,
        Take_Elevator,
        Get_Off_Elevator,
        Charge,
        Stop,
        Localization,
        Play,
        Locker_open,
        Locker_close,
        Item_load,
        Item_unload,
        Status,     // TF만사용
        ItemLoad,   // TF만사용
    }

    public enum MobileRobotServiceStates
    {
        Waiting,
        Start,
        Process,
        Error,
        Stop,
        Completed
    }

    public enum MobileRobotBoundTypes
    {
        Pickup,     //
        OnDelivery,
        Homing,   //Ible 상태
        Move,     //Ible > goto 특정 좌표 이동 ,  controlplan 전부 complet 상태 or controlplan 없는 상태  
        Charge      //로봇 대기장소에서만 charge 가능
    }

    public enum MobileRobotBoundStates
    {
        Waiting,
        Start,
        Process,
        Error,
        Stop,
        Completed
    }

    public enum MobileRobotEventTypes
    {
        Added,
        Modified,
        Completed
    }

    public enum ProvideModes
    {
        ByCreateTime,
        ByOrderSequence,
    }

    public enum GuiPages
    {
        MainPage,
        OperationPage,
        MaintenancePage,
        ConfigPage,
        ModelPage,
        RecipePage,
        AlarmPage,
        LogPage,
        RecordPage,
        DataPage,
        TitleBar,
        MenuBar,
        CommandBar,
        StatusBar,
    }
    #endregion
}
#endregion