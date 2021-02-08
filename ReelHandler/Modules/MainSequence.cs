#region Imports
using Cognex.VisionPro.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TechFloor.Components;
using TechFloor.Device;
using TechFloor.Forms;
using TechFloor.Object;
using TechFloor.Shared;
using TechFloor.Util;
#endregion

#region Program
namespace TechFloor
{
    #region Enumerations
    public enum ErrorCode
    {
        None,
        AbortInitialize,
        FailedInitialize,
        ReelTowerResponseTimeout,
        RobotCommunicationFailure,
        RobotMoveToHomeTimeout,
        RobotMoveTimeout,
        FailedToStopProgram,
        FailedToPauseProgram,
        FailedToLoadProgram,
        FailedToPlayProgram,
        CartPresentSensorFailure,
        ReturnReelPresentSensorFailure,
        ReturnReelTypeIsNotValid,
        RobotVisionFailure,
        RobotVisionAlignPositionFailure,
        RobotVisionDecodeBarcodeFailure,
        RobotVisionDecodedBarcodeIsNotValid,
        FailedToPickupReelFromCart,
        FailedToPickupReelFromReturn,
        FailedToCheckReelTypeOfCart,
        FailedToCheckReelTypeOfReturn,
        FailedToSetReelSizeOfCartToRobot,
        FailedToSetReelSizeOfReturnToRobot,
        FailedToSetWorkSlotOfCart,
        WorkSlotOfCartIsNotValid,
        UnloadReelTowerIdIsNotValid,
        DetectedAReelOnGripper,
        FailedToSetCartReelSize,
        FailedToSetReturnReelSize,
        RobotPositionIsNotMatched,
        RobotStateIsNotMatched,
        FailedToPickUnloadReel,
        ReelTowerIsAlarmState,
        ReelTowerIsFull,
        ReelTowerIsBusy,
        RobotEmergencyStop,
        RobotProtectiveStop,
        RobotReelDetectionSensorFailure,
        ReelBarcodeIsNotUnique,
        MobileRobotCommunicationFailure,
        FailedToPickRejectReel,
        FailedToCheckGuidePointsOfCart,
        FailedToSetGuidePointOfCartToRobot,
        FailedToCheckTowerBasePoint,
        FailedToSetTowerBasePointToRobot,
        AbortCalibration,
        FailedCalibration,
        FailedVisionMarkAdjustment,
    }

    public enum SeverityLevels
    {
        Cleared,
        Warning,
        Minor,
        Major,
        Critical,
        Indeterminate,
    }

    public enum CartDockingStates
    {
        Unknown,
        LoadStarted,
        Loading,
        LoadCompleted,
        UnloadStarted,
        Unloading,
        UnloadCompleted,
    }

    public enum ReelTowerSteps
    {
        None,
        Ready,
        CheckReelTowerState,
        StatePollingDelay,
        Done,
    }

    public enum MobileRobotSteps
    {
        None,
        Ready,
        CheckCartSensors,
        PrepareToUnloadCart,
        CheckProductionState,
        CheckCartClampState,
        RequestCartLoad,
        WaitForCartLoadStart,
        WaitForCartLoadComplete,
        WaitForReelTypeConfirm,
        CheckClampedCart,
        PrepareProduction,
        RequestCartUnload,
        WaitForCartUnloadStart,
        WaitForCartUnloadComplete,
        Done,
    }

    public enum BarcodeConfirmStates
    {
        Prepared,
        Confirmed,
        Reject
    }

    public enum ReelHandlerSteps
    {
        None,
        Ready,
        CheckProgramState,
        ConfirmRobotProgramState,

        // Load cart reel sequence
        CheckCartReelType,
        SetReelTypeOfCartToRobot,
        
        // New features
        CheckCartGuidePoint1,
        AdjustCartGuidePoint1,
        ApplyCartGuidePoint1,
        CheckCartGuidePoint2,
        AdjustCartGuidePoint2,
        ApplyCartGuidePoint2,
        CheckCartGuidePoint3,
        AdjustCartGuidePoint3,
        ApplyCartGuidePoint3,
        CheckCartGuidePoint4,
        AdjustCartGuidePoint4,
        ApplyCartGuidePoint4,
        SetCartGuideWorkSlotCenter1,
        SetCartGuideWorkSlotCenter2,
        SetCartGuideWorkSlotCenter3,
        SetCartGuideWorkSlotCenter4,
        SetCartGuideWorkSlotCenter5,
        SetCartGuideWorkSlotCenter6,

        PrepareToLoadReelFromCart,
        SetWorkSlotToRobot,
        GoToHomeBeforeReelHeightCheck, // 200
        MoveToReelHeightCheckPositionOfWorkSlot,
        MeasureReelHeightOnCart,
        CheckReelAlignmentOnCart,
        RequestToReelLoadConfirm,
        PrepareToReadBarcodeOnReel,
        ReadBarcodeOnReel,
        RequestToConfirmLoadReelBarcodeOfCart,
        AdjustPositionAndPickupReelOfCart,
        GoToHomeAfterPickUpReel, // 204
        MoveToFrontOfTower,
        PutReelIntoTower,
        RequestToConfirmLoadedReelAssign,
        CompletedToLoadReel,
        PrepareToChangeWorkSlotOfCart,
        ChangeWorkSlotOfCart,
        // Load failure case
        MoveBackToHomeByVisionAlignmentFailureToLoad,
        MoveBackToHomeByReelTowerResponseTimeoutToLoad,
        MoveBackToHomeByReelTowerRefuseToLoad,
        MoveBackToHomeByCancelBarcodeInputToLoad,
        MoveBackToHomeByReelPickupFailureToLoad,
        MoveBackToHomeByCartReelTypeCheckFailure,
        WaitForBarcodeInput,
        CheckReelAlignmentAfterBarcodeInput,

        // Load return reel sequence
        PrepareToLoadReturnReel,
        SetReelTypeOfReturnToRobot,
        MoveToFrontOfReturnStage,
        ApproachToReelHeightCheckPointAtReturnStage,
        MeasureReelHeightOnReturnStage,
        CheckReelAlignmentOnReturnStage,
        RequestToReturnReelLoadConfirm,
        // PrepareToReadBarcodeOnReel,
        // ReadBarcodeOnReel,
        RequestToConfirmLoadReelBarcodeOfReturnStage,
        AdjustPositionAndPickupReelOfReturnStage,
        // Load return failure case 
        MoveBackToFrontOfReturnStage,
        MoveBackToFrontOfReturnStageByVisionAlignmentFailureToLoadReturn,
        MoveBackToFrontOfReturnStageByResponseTimeoutToLoadReturn,
        MoveBackToFrontOfReturnStageByReelTowerRefuseToLoadReturn,
        MoveBackToFrontOfReturnStageByCancelBarcodeInputToLoadReturn,
        MoveBackToFrontOfReturnStageByReelPickupFailureToLoadReturn,

        // Unload Sequence
        PrepareToUnloadTowerReel,
        MoveToFrontOfUnloadTower,
        TakeReelFromUnloadTower,
        RequestToUnloadReelAssignment,
        MoveToFrontOfOutputStage,
        CheckUpStateOfOutputStage,
        ApproachOutputStage,
        PutReelIntoOutputStage,
        RequestToConfirmUnloadedReelAssign,
        CompletedToUnloadReelFromTower,

        // Failure sequences
        MoveBackToFrontOfTowerByReelPickupFailureToUnload,
        
        // Unload to buffer sequence
        PrepareToRejectReel,
        MoveToFrontOfRejectStage,
        CheckUpStateOfRejectStage,
        ApproachRejectStage,
        PutReelIntoRejectStage,
        CompletedToRejectReel,

        // Failure reject sequence
        MoveBackToFrontOfTowerByReelPickupFailureToReject,

        Done,
    }

    public enum SubSequenceSteps
    {
        Prepare,
        Proceed,
        Post
    }
    #endregion

    public class ReelTowerStateObject : AbstractClassDisposable
    {
        #region Fields
        public readonly int Index;
        public bool DoorOpen;
        public bool ReelExist;
        public string Id;
        public ReelTowerOperationModes Mode;
        public MaterialStorageState.StorageOperationStates State;
        #endregion

        #region Constructors
        public ReelTowerStateObject(int index, string id)
        {
            Index   = index;
            Id      = id;
            Mode    = ReelTowerOperationModes.None;
            State   = MaterialStorageState.StorageOperationStates.Unknown;
        }
        #endregion

        #region Public methods
        public void SetState(MaterialStorageState.StorageOperationStates state)
        {
            State = state;
        }
        #endregion
    }

    public class ReelTowerRobotSequence : AbstractClassDisposable, IMainSequence
    {
        #region Enumerations
        public enum InitializeSteps : int
        {
            None,
            Ready,
            CheckProgramState,
            StopProgram,
            LoadProgram,
            PlayProgram,
            CheckLoadedProgram,
            CheckHomedPosition,
            CheckReturnReelPresentSensors,
            CheckBarcodeOption,
            PrepareInitialize,
            Initializing,
            PostInitialize,
            Done,
            Unknown,
            Failed
        }

        public enum AutoTeachSteps : int
        {
            None,
            Ready,
            PreapareAutoTech,
            AutoTeching,
            PostAutoTeach,
            Done,
            Unknown,
            Failed
        }

        public enum CalibrationSteps : int
        {
            None,
            Ready,
            PrepareCalibration,
            CheckCartPresent,
            CheckTowerBasePoint1,
            AdjustTowerBasePoint1,
            SetTowerBasePoint1,
            CheckTowerBasePoint2,
            AdjustTowerBasePoint2,
            CheckDisplacements,
            PostCalibration,
            Done,
            Unknown,
            Failed
        }
        #endregion

        #region Constants
        protected const int CONST_THREAD_POLLING_INTERVAL           = 100;
        protected const int CONST_SIMULATION_DELAY                  = 300;
        public const int CONST_MAX_TOWER_COUNT                      = 4;
        public const int CONST_MAX_MOBILE_STAGE_COUNT               = 4;
#if DEBUG
        public const string CONST_WORK_PATH                         = @"C:\Temp\K4\ApplicationData\";
#else
        public const string CONST_WORK_PATH                         = @"D:\ApplicationData\";
#endif
        public const string CONST_CONFIG_PATH                       = @"Config\";
        public const string CONST_MODEL_PATH                        = @"Model\";
        public const string CONST_DATA_PATH                         = @"Data\";
        public const string CONST_PACKAGE_PATH                      = @"Packages\";
        #endregion

        #region Fields
        protected ErrorCode robotSequenceErrorCode                      = ErrorCode.None;
        protected bool cycleStop                                        = false;
        protected bool initialized                                      = false;
        protected bool systemAutoRunning                                = false;
        protected bool systemInitialized                                = false;
        protected bool stopRobotInitialize                              = false;
        protected bool finishedLoadReelFromCart                         = false;
        protected bool latestVersion                                    = false;
        protected bool outputStage1Full                                 = false;
        protected bool outputStage2Full                                 = false;
        protected bool outputStage3Full                                 = false;
        protected bool outputStage4Full                                 = false;
        protected bool outputStage5Full                                 = false;
        protected bool outputStage6Full                                 = false;
        protected bool rejectStageFull                                  = false;
        protected bool lastRejectStageFullState                         = false;
        protected bool autoTaught                                       = false;
        protected bool calibrated                                       = false;
        protected bool stopCalibration                                  = false;
        protected bool requiredVisionMarkAdjustment                     = false;
        protected int currentWorkSlotOfCart                             = 1;
        protected int threadScheduleInterval                            = 10;
        protected int intervalReelTowerStatePolling                     = 200;
        protected int timeoutReelTowerResponse                          = Math.Max(Config.TimeoutOfReelTowerResponse, 10000);
        protected int timeoutRobotCommunication                         = 10000;
        protected int timeoutOfRobotMoving                              = 30000;
        protected int timeoutOfRobotHoming                              = 36000;
        protected int timeoutMobileRobotCommunication                   = 3000;
        protected int timeOfMobileRobotClampValidation                  = 20000;
        protected int timeOfReturnReelPresentValidation                 = 10000;
        protected int timeoutOfNotification                             = 30000;
        protected int delayOfImageProcessing                            = 3000;
        protected int delayOfImageProcessingRetry                       = 1000;
        protected int delayOfGrapRetry                                  = 100;
        protected int delayOfRobotCommunicationRetry                    = 1000;
        protected int delayOfUnloadReelStateUpdate                      = 1000;
        protected int delayOfMaterialPackageUpdate                      = 3000;
        protected int reelTowerQueryRetryCount                          = 0;
        protected int robotActionRetryCount                             = 0;
        protected int robotVisionRetryCount                             = 0;
        protected int robotVisionRetryAttemptsCount                     = 0;
        protected int robotVisionRetryCycleCount                        = 0;
        protected int retryLimitOfReelTowerQuery                        = 3; // 10
        protected int retryLimitOfRobotActionCheck                      = 10;
        protected int retryLimitOfRobotVisionCheck                      = 10;
        protected int retryLimitOfRobotVisionCheckAttempts              = 3;
        protected int retryLimitOfRobotPickup                           = 10;
        protected int imageProcessTimeoutTick                           = 0;
        protected int materialUnloadMoveTick                            = 0;
        protected int materialLoadMoveTick                              = 0;
        protected int materialBarcodeTick                               = 0;
        protected int materialInfoUpdateTick                            = 0;
        protected int robotProgramStateTick                             = 0;
        protected int reelTowerTick                                     = 0;
        protected int mobileRobotTick                                   = 0;
        protected int robotTick                                         = 0;
        protected int visionTick                                        = 0;
        protected int movementTick                                      = 0;
        protected int notificationTick                                  = 0;
        protected int cartPresentSensorTick                             = 0;
        protected int returnReelPresentSensorTick                       = 0;
        protected int unloadReelReportTick                              = 0;
        protected int systemIdleStateTick                               = -1;
        protected int stateQueryId                                      = 1;
        protected int currentLoadReelTower                              = -1;
        protected int currentUnloadReelTower                            = -1;
        protected int totalLoadReelCountValue                           = 0;
        protected int totalReturnReelCountValue                         = 0;
        protected int totalUnloadReelCountValue                         = 0;
        protected int loadErrorCountValue                               = 0;
        protected int returnErrorCountValue                             = 0;
        protected int unloadErrorCountValue                             = 0;
        protected int visionAlignError                                  = 0;
        protected int visionDecodeError                                 = 0;
        protected int visionProcessResultState                          = 0;
        protected int robotProgramStateCheckRetry                       = 0;
        protected int retryLimitOfRobotProgramStateCheck                = 10;
        protected int visionProcessLockState                            = 0;
        protected int stayCountReelTower_, stayCountMobileRobot_, stayCountRobot_;
        protected double distanceXOfAlignError                          = 20.0;
        protected double distanceYOfAlignError                          = 20.0;
        protected string alignmentCoordX                                = string.Empty;
        protected string alignmentCoordY                                = string.Empty;
        protected string alignmentCoordZ                                = string.Empty;
        protected string currentLoadReelTowerId                         = string.Empty;
        protected string currentUnloadReelTowerId                       = string.Empty;
        protected string robotOperationState                            = string.Empty;
        protected string transferMaterialObjectMode                     = string.Empty;
        protected DateTime startCycleTime                               = DateTime.Now;
        protected DateTime finishCycleTime                              = DateTime.Now;
        protected TimeSpan lastCycleTime                                = TimeSpan.Zero;
        protected RobotSequenceCommands previousRobotCommand            = RobotSequenceCommands.Unknown;
        protected BarcodeKeyInData barcodeKeyInData                     = new BarcodeKeyInData();
        protected CartDockingStates currentCartDockingState             = CartDockingStates.Unknown;
        protected CartDockingStates previousCartDockingState            = CartDockingStates.Unknown;
        protected ImageProcssingResults imageProcessingResult           = ImageProcssingResults.Success;
        protected ReelDiameters currentReelTypeOfCart                   = ReelDiameters.Unknown;
        protected ReelDiameters currentReelTypeOfReturn                 = ReelDiameters.Unknown;
        protected ReelDiameters previousReelTypeOfReturn                = ReelDiameters.Unknown;
        protected ReelDiameters detectedReelTypeOfReturn                = ReelDiameters.Unknown;
        protected ReelTowerSteps previousReelTowerStep                  = ReelTowerSteps.None;
        protected ReelTowerSteps reelTowerStep                          = ReelTowerSteps.Ready;
        protected SubSequenceSteps reelTowerSubStep                     = SubSequenceSteps.Prepare;
        protected MobileRobotSteps previousMobileRobotStep              = MobileRobotSteps.None;
        protected MobileRobotSteps mobileRobotStep                      = MobileRobotSteps.Ready;
        protected SubSequenceSteps mobileRobotSubStep                   = SubSequenceSteps.Prepare;
        protected ReelHandlerSteps previousRobotStep                    = ReelHandlerSteps.None;
        protected ReelHandlerSteps robotStep                            = ReelHandlerSteps.Ready;
        protected SubSequenceSteps robotSubStep                         = SubSequenceSteps.Prepare;
        protected SubSequenceSteps robotProgramStateStep                = SubSequenceSteps.Prepare;
        protected CartPresentStates cartPresnetState                    = CartPresentStates.Unknown;
        protected OperationStates operationState                        = OperationStates.PowerOn;
        protected RobotCommunicationStates responseOfRobotSequence      = RobotCommunicationStates.Unknown;
        protected BarcodeConfirmStates barcodeConfirmState              = BarcodeConfirmStates.Prepared;
        protected List<FourField<string, string, string, ReelUnloadReportStates>> reelsOfRejectStages  = new List<FourField<string, string, string, ReelUnloadReportStates>>();
        protected Dictionary<int, MaterialPackage> reelsOfOutputStages  = new Dictionary<int, MaterialPackage>();
        protected List<int> currentProductionCountOfWorkSlot            = new List<int>()
        {
            0,0,0,0,0,0
        };
        protected Thread compositedProcess                              = null;
        protected List<Thread> processThreads                           = new List<Thread>();
        protected Dictionary<string, Thread> initializeThreads          = new Dictionary<string, Thread>();
        protected Dictionary<string, Thread> calibrationThreads         = new Dictionary<string, Thread>();
        protected List<ReelTowerStateObject> currentTowers              = new List<ReelTowerStateObject>();
        protected MaterialStorageState responseOfReelTower              = new MaterialStorageState();
        protected MaterialStorageState currentLoadReelState             = new MaterialStorageState();
        protected MaterialStorageState currentUnloadReelState           = new MaterialStorageState();
        protected MaterialStorageState currentRejectReelState           = new MaterialStorageState();
        protected RobotSequenceManager robotSequenceManager             = new RobotSequenceManager();   // UR robot connection to order/get command and response
        protected RobotController robotController                       = new RobotController();        // Dashboard
        protected MobileRobotManager mobileRobotManager                 = new MobileRobotManager();     // MRBT
        protected CompositeVisionManager barCodeVision                  = new CompositeVisionManager(); // Barcode and vision initialization
        protected WaitHandle[] sequenceEvents                           = new WaitHandle[2];
        protected MaterialData reelBarcodeContexts                      = new MaterialData();
        protected InitializeMode initializeMode                         = InitializeMode.All;
        protected InitializeSteps systemInitializeStep                  = InitializeSteps.Unknown;
        protected InitializeSteps robotInitializeStep                   = InitializeSteps.Unknown;
        protected AutoTeachSteps autoTeachStep                          = AutoTeachSteps.Unknown;
        protected CalibrationMode calibrationMode                       = CalibrationMode.TowerBasePoints;
        protected CalibrationSteps calibrationStep                      = CalibrationSteps.Unknown;
        protected SubSequenceSteps robotInitializeSubStep               = SubSequenceSteps.Prepare;
        protected SubSequenceSteps calibrationSubStep                   = SubSequenceSteps.Prepare;
        protected RobotSequenceCommands lastRobotSequenceCommand        = RobotSequenceCommands.Unknown;
        protected Dictionary<ErrorCode, Pair<string, string>> alarmList = new Dictionary<ErrorCode, Pair<string, string>>();
        protected List<Pair<int, ReelDiameters>> reelSensingStates      = new List<Pair<int, ReelDiameters>>();
        protected FormMessageExt unloadwaitNotification                 = null;
        protected VisionProcessDataObject visionProcessLastRunResult    = new VisionProcessDataObject();
        protected RobotActionOrder previousRobotActionOrder             = RobotActionOrder.None;
        protected bool verifiedTowerBasePoints                          = false;
        protected bool verifiedCartGuidePoints                          = false;
        protected int visionMarkAdjustmentStates                        = 0;
        protected Dictionary<int, List<Coord3DField<double, double, double, double, double, double>>> loopCountOfCartGuidePoints 
            = new Dictionary<int, List<Coord3DField<double, double, double, double, double, double>>>();
        protected Dictionary<int, List<Coord3DField<double, double, double, double, double, double>>> loopCountOfTowerBasePoints
            = new Dictionary<int, List<Coord3DField<double, double, double, double, double, double>>>();
        #endregion

        #region Properties
        public bool CycleStop                                                   => cycleStop;
        public bool Initialized                                                 => initialized;
        public bool AllNetworkConnected                                         => ((ReelTowerManager.IsRunning && ReelTowerManager.IsServiceNow) || robotStep == ReelHandlerSteps.RequestToConfirmLoadedReelAssign || robotStep == ReelHandlerSteps.CompletedToLoadReel | robotStep >= ReelHandlerSteps.RequestToUnloadReelAssignment) && robotController.IsConnected && robotSequenceManager.IsConnected;
        public bool IsRequiredReset                                             => operationState == OperationStates.Alarm;
        public bool IsWaitBarcodeInput                                          => robotStep == ReelHandlerSteps.WaitForBarcodeInput;
        public bool RunnableCondition                                           => (AllNetworkConnected && robotSequenceManager.IsHomed && !robotSequenceManager.IsFailure && operationState == OperationStates.Run && !outputStage1Full && !outputStage2Full && !outputStage3Full && !outputStage4Full && !outputStage5Full && !outputStage6Full && !rejectStageFull);
        public bool IsPossibleImmediateStop                                     => !robotSequenceManager.IsConnected || !robotController.IsRunnable || (!robotSequenceManager.HasAReel && robotSequenceManager.IsPossibleStop && robotSequenceManager.IsRobotAtSafeWayPoint());
        public bool IsRobotAvailableToAct(bool safeposition)                    => (robotSequenceManager.IsConnected && (safeposition || robotSequenceManager.IsAtSafePosition) && robotSequenceManager.IsHomed && !robotSequenceManager.IsMoving && !robotSequenceManager.IsFailure);
        public bool IsFinishedLoadReelFromCart                                  => finishedLoadReelFromCart;
        public int CurrentWorkSlotOfCart                                        => currentWorkSlotOfCart;
        public bool IsCartAvailable
        {
            get
            {
                switch (currentReelTypeOfCart)
                {
                    default:
                    case ReelDiameters.Unknown:         return ((App.DigitalIoManager as DigitalIoManager).IsCartHooked);
                    case ReelDiameters.ReelDiameter7:   return ((App.DigitalIoManager as DigitalIoManager).IsCartHooked && currentWorkSlotOfCart <= 6);
                    case ReelDiameters.ReelDiameter13:  return ((App.DigitalIoManager as DigitalIoManager).IsCartHooked && currentWorkSlotOfCart <= 4);
                }
            }
        }
        public bool IsReelTowerQueryRetryOver                                   => reelTowerQueryRetryCount >= retryLimitOfReelTowerQuery;
        public bool IsRobotVisionRetryOver                                      => robotVisionRetryCount >= retryLimitOfRobotVisionCheck;
        public bool IsRobotVisionRetryCycleOver                                 => robotVisionRetryCycleCount >= retryLimitOfRobotVisionCheck;
        public bool IsRobotVisionRetryAttemptsOver                              => robotVisionRetryAttemptsCount >= retryLimitOfRobotVisionCheckAttempts;
        public bool IsRobotActionRetryOver                                      => robotActionRetryCount >= retryLimitOfRobotActionCheck;
        public bool IsRobotProgramStateCheckRetryOver                           => robotProgramStateCheckRetry >= retryLimitOfRobotProgramStateCheck;
        public bool IsReelTowerStatePollingDelayOver(int delay)                 => (TimeSpan.FromMilliseconds(App.TickCount - reelTowerTick).TotalMilliseconds >= delay);
        public bool IsRobotActionDelayOver(int delay)                           => (TimeSpan.FromMilliseconds(App.TickCount - robotTick).TotalMilliseconds >= delay);
        public bool IsVisionProcessDelayOver(int delay)                         => (TimeSpan.FromMilliseconds(App.TickCount - visionTick).TotalMilliseconds >= delay);
        public bool IsReturnReelSensingDelayOver(int delay)                     => (TimeSpan.FromMilliseconds(App.TickCount - returnReelPresentSensorTick).TotalMilliseconds >= delay);
        public bool IsOverDelayTime(int delay, int tick)                        => (TimeSpan.FromMilliseconds(App.TickCount - tick).TotalMilliseconds >= delay);
        public bool IsCartPresentSensingDelayOver(int delay)                    => (TimeSpan.FromMilliseconds(App.TickCount - cartPresentSensorTick).TotalMilliseconds >= delay);
        public bool IsNotificationDelayOver(int delay)                          => (TimeSpan.FromMilliseconds(App.TickCount - notificationTick).TotalMilliseconds >= delay);
        public bool IsUnloadReelStateUpdateDelayOver(int delay)                 => (TimeSpan.FromMilliseconds(App.TickCount - unloadReelReportTick).TotalMilliseconds >= delay);
        public bool IsOverMaterialInfoUpdateDelay(int delay)                    => (TimeSpan.FromMilliseconds(App.TickCount - materialInfoUpdateTick).TotalMilliseconds >= delay);
        public int TimeoutOfRobotControllerConnection                           { get; set; }
        public int TimeoutOfMobileRobotCartInCheck                              { get; set; }
        public int TimeoutOfReturnInputReelCheck                                { get; set; }
        public int TimeoutOfReelType                                            { get; set; }
        public int RetryLimitOfVisionError                                      => retryLimitOfRobotVisionCheck;
        public int RetryLimitOfPickupFailure                                    { get; set; }
        public int InitializeStep                                               => Convert.ToInt32(robotInitializeStep);
        public int TotalLoadReels                                               => totalLoadReelCountValue;
        public int TotalReturnReels                                             => totalReturnReelCountValue;
        public int TotalUnloadReels                                             => totalUnloadReelCountValue;
        public int LoadErrors                                                   => loadErrorCountValue;
        public int ReturnErrors                                                 => returnErrorCountValue;
        public int UnloadErrors                                                 => unloadErrorCountValue;
        public int VisionAlignErrors                                            => visionAlignError;
        public int VisionDecodeErrors                                           => visionDecodeError;
        public TimeSpan LastCycleTime                                           => lastCycleTime;
        public double DistanceXOfAlignError                                     => distanceXOfAlignError;
        public double DistanceYOfAlignError                                     => distanceYOfAlignError;
        public IReadOnlyList<int> CurrentProductionCountOfWorkSlot              => currentProductionCountOfWorkSlot;
        public MaterialData ReelBarcodeContexts                                 => reelBarcodeContexts;
        public MobileRobotOperationModes CartMode                               => mobileRobotManager.CartMode;
        public ReelDiameters CurrentReelTypeOfCart                              => currentReelTypeOfCart;
        public ReelDiameters CurrentReelTypeOfReturn                            => currentReelTypeOfReturn;
        public IReadOnlyList<ReelTowerStateObject> CurrentTowerStates           => currentTowers;
        public CommunicationStates CommunicationStateOfReelTower                => ReelTowerManager.CommunicationState;
        public CommunicationStates CommunicationStateOfRobotSequence            => robotSequenceManager.CommunicationState;
        public CommunicationStates CommunicationStateOfRobotController          => robotController.CommunicationState;
        public CommunicationStates CommunicationStateOfMobileRobot              => mobileRobotManager.CommunicationState;
        public CompositeVisionManager BarCodeVision                             => barCodeVision;
        public OperationStates OperationState                                   => operationState;
        public CartPresentStates CartPresentState                               => cartPresnetState;
        public MobileRobotManager MobileRobotManager                            => mobileRobotManager;
        public RobotSequenceManager RobotSequenceManager                        => robotSequenceManager;
        public void SendReelTowerMessage(ReelTowerCommands messageName, 
            bool loadFlag,
            MaterialData barcode = null,
            string towerid = null,
            string returnCode = "0",
            string returnMessage = "done") => 
            ReelTowerManager.SendTowerMessage(messageName, barcode, towerid, true, returnCode, returnMessage);
        public void SendMobileRobotCommand(MobileRobotManagerCommands command)  => mobileRobotManager.SendCommand(command);
        public void SendMobileRobotMessage(string message)                      => mobileRobotManager.SendSocketData(message);
        public ReelHandlerSteps RobotStep                                       => robotStep;
        public MobileRobotSteps MobileRobotStep                                 => mobileRobotStep;
        public ReelTowerSteps ReelTowerStep                                     => reelTowerStep;
        public CartDockingStates GetDockingStates()                             => currentCartDockingState;
        public ReelTowerManager ReelTowerManager                                => Singleton<ReelTowerManager>.Instance;
        public bool IsLatestVersion                                             => latestVersion;
        public IReadOnlyDictionary<int, MaterialPackage> ReelOfOutputStages     => reelsOfOutputStages;
        public IReadOnlyList<FourField<string, string, string, ReelUnloadReportStates>> ReelsOfRejectStage => reelsOfRejectStages;
        public bool AutoTaught                                                  => autoTaught;
        public bool Calibrated                                                  => calibrated;
        public int AutoTechStep                                                 => Convert.ToInt32(autoTeachStep);
        public int CalibrationStep                                              => Convert.ToInt32(calibrationStep);

        public bool SameReelCheck = false;

        public AMM.AMM ReelHanlderAMM = new AMM.AMM();
        #endregion

        #region Events
        public virtual event EventHandler OperationStateChanged;
        public virtual event EventHandler OperationModeChanged;
        public virtual event EventHandler FinishedCycleStop;
        public virtual event EventHandler CycleStopOrderStateChanged;
        public virtual event EventHandler<CartPresentStates> CartPresentStateChanged;
        public virtual event EventHandler<int> ChangedReelSizeOfCart;
        public virtual event EventHandler<string> NotifyEvent;
        public virtual event EventHandler<MaterialData> NotifyToShowBarcodeInputWindow;
        public virtual event EventHandler<bool> NotifyToShowDockCartWindow;
        public virtual event EventHandler NotifyProductionInformation;
        public virtual event EventHandler<VisionProcessEventArgs> TriggerVisionControl;
        public virtual event EventHandler<string> ReportAlarmLog;
        #endregion

        #region Constructors
        public ReelTowerRobotSequence()
        {
            sequenceEvents[0] = (App.MainForm as FormMain).ShutdownEvent;
            sequenceEvents[1] = new ManualResetEvent(false);

             // Reading values from xml when prpgram starts
            totalLoadReelCountValue = ProductionRecord._TotalLoadCountInt;
            totalReturnReelCountValue = ProductionRecord._TotalReturnCountInt;
            totalUnloadReelCountValue = ProductionRecord._TotalUnloadCountInt;
            loadErrorCountValue = ProductionRecord._TotalLoadErrorCountInt;
            returnErrorCountValue = ProductionRecord._TotalReturnErrorCountInt;
            unloadErrorCountValue = ProductionRecord._TotalUnloadErrorCountInt;
            visionAlignError = ProductionRecord._VisionAlignmentErrorCountInt;
            visionDecodeError = ProductionRecord._VisionDecodeErrorCountInt;

            currentTowers.Add(new ReelTowerStateObject(1, Config.ReelTowerName1));
            currentTowers.Add(new ReelTowerStateObject(2, Config.ReelTowerName2));
            currentTowers.Add(new ReelTowerStateObject(3, Config.ReelTowerName3));
            currentTowers.Add(new ReelTowerStateObject(4, Config.ReelTowerName4));
            ReelHanlderAMM.Connect();
        }
        #endregion

        #region Protected methods
        protected override void DisposeManagedObjects()
        {
            ReelTowerManager.Dispose();
            robotSequenceManager.Dispose();
            robotController.Dispose();
            mobileRobotManager.Dispose();
            barCodeVision.Dispose();

            // if (responseOfReelTower != null)
            //     responseOfReelTower.Dispose();
            // 
            // if (currentUnloadReelState != null)
            //     currentUnloadReelState.Dispose();
            // 
            // if (currentLoadReelState != null)
            //     currentLoadReelState.Dispose();

            foreach (ReelTowerStateObject towerstate in currentTowers)
                towerstate.Dispose();

            base.DisposeManagedObjects();
        }

        protected virtual void ResetBarcodeContexts()
        {
            reelBarcodeContexts.Clear();
        }

        protected virtual void FireProductionInformation(bool reset = false)
        {
            if (reset)
                ResetBarcodeContexts();

            NotifyProductionInformation?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void FirePopupBarcodeInputWindow(MaterialData data = null)
        {
            try
            {
                lock (barcodeKeyInData)
                    barcodeKeyInData.Clear();

                NotifyToShowBarcodeInputWindow?.Invoke(this, data);
            }
            catch (Exception ex)
            {
                Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }
        
        protected virtual void FirePopupManualCartDockWindow(bool dock = true)
        {
            try
            {
                NotifyToShowDockCartWindow?.Invoke(this, dock);
            }
            catch (Exception ex)
            {
                Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected virtual void FireShowNotification(string msg)
        {
            try
            {
                NotifyEvent?.Invoke(this, msg.ToString());
            }
            catch (Exception ex)
            {
                Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public void ResetProductionCount()
        {
            totalLoadReelCountValue = 0;
            totalReturnReelCountValue = 0;
            totalUnloadReelCountValue = 0;
            loadErrorCountValue = 0;
            returnErrorCountValue = 0;
            unloadErrorCountValue = 0;
            visionAlignError = 0;
            visionDecodeError = 0;
        }

        protected void CheckSizeOfReturnReeel()
        {
            previousReelTypeOfReturn = currentReelTypeOfReturn;
            
            if ((App.DigitalIoManager as DigitalIoManager).ReturnStageReelPresentSensor13 && !(App.DigitalIoManager as DigitalIoManager).ReturnStageReelPresentSensor7)
                currentReelTypeOfReturn = ReelDiameters.ReelDiameter13;
            else if ((App.DigitalIoManager as DigitalIoManager).ReturnStageReelPresentSensor7 && !(App.DigitalIoManager as DigitalIoManager).ReturnStageReelPresentSensor13)
                currentReelTypeOfReturn = ReelDiameters.ReelDiameter7;
            else
                currentReelTypeOfReturn = ReelDiameters.Unknown;
        }

        protected bool VerifyReturnReeelSize(ReelDiameters type)
        {
            bool result_ = false;
            switch (type)
            {
                case ReelDiameters.ReelDiameter13:
                    {
                        if ((App.DigitalIoManager as DigitalIoManager).ReturnStageReelPresentSensor13 && !(App.DigitalIoManager as DigitalIoManager).ReturnStageReelPresentSensor7)
                            result_ = true;
                    }
                    break;
                case ReelDiameters.ReelDiameter7:
                    {
                        if ((App.DigitalIoManager as DigitalIoManager).ReturnStageReelPresentSensor7 && !(App.DigitalIoManager as DigitalIoManager).ReturnStageReelPresentSensor13)
                            result_ = true;
                    }
                    break;
            }
            return result_;
        }

        protected void CancelLoadProcedure()
        {
            ReelTowerManager.SetReelLoadReset();
            Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
        }

        protected void ResetLoadProcedure()
        {
            ReelTowerManager.SendReelLoadReset();
        }

        protected bool IsPossibleToSwitchTransferModeToUnload(ref bool finished, ref bool safepos)
        {
            bool cmddone_ = false;
            RobotSequenceCommands command_ = robotSequenceManager.GetRobotExecutedCommandByScenario(ref cmddone_);
            finished = robotSequenceManager.IsRobotAtWayPointByCommand(command_, ref safepos);

            if (!robotSequenceManager.HasAReel &&
                cmddone_ &&
                safepos &&
                robotSequenceManager.IsWaitForOrder)
            {
                switch (Singleton<TransferMaterialObject>.Instance.State)
                {
                    case TransferMaterialObject.TransferStates.None:
                    case TransferMaterialObject.TransferStates.RequestToLoadConfirm:
                    case TransferMaterialObject.TransferStates.WaitForLoadConfirm:
                    case TransferMaterialObject.TransferStates.ConfirmLoad:
                    case TransferMaterialObject.TransferStates.RequestToBarcodeConfirm:
                    case TransferMaterialObject.TransferStates.WaitForBarcodeConfirm:
                    case TransferMaterialObject.TransferStates.CompleteLoad:
                        {
                            switch (Singleton<TransferMaterialObject>.Instance.Mode)
                            {
                                case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                                case TransferMaterialObject.TransferModes.Reject:
                                    break;
                                default:
                                    {
                                        CancelLoadProcedure();
                                        return true;
                                    }
                            }
                        }
                        break;
                    case TransferMaterialObject.TransferStates.VerifiedUnload:
                    case TransferMaterialObject.TransferStates.TakenMaterial:
                    case TransferMaterialObject.TransferStates.RequestToUnloadAssignment:
                    case TransferMaterialObject.TransferStates.WaitForUnloadAssignment:
                    case TransferMaterialObject.TransferStates.CompleteUnload:
                        {   // Have to reset
                            if (!RobotSequenceManager.HasAReel)
                                return true;
                        }
                        break;
                }
            }

            return false;
        }

        protected void DefineRobotLoadOperation()
        {
            if (IsCartAvailable)
            {
                if (currentLoadReelTower < 0) // Load from cart
                {   // Try to load a reel from cart.
                    if (ReelTowerManager.IsPossibleLoadReel(ref currentLoadReelState))
                    {   // Assign a reel to load into tower.
                        currentLoadReelTowerId = currentLoadReelState.Name;
                        currentTowers[currentLoadReelState.Index - 1].Mode = ReelTowerOperationModes.Load;
                        currentLoadReelState.SetLoadReelType(currentReelTypeOfCart, false);
                        Interlocked.Exchange(ref currentLoadReelTower, currentLoadReelState.Index);
                        // Assign a new operation order to robot. (Load a reel from cart)
                        robotSequenceManager.RobotActionOrder = RobotActionOrder.LoadReelFromCart;
                        Logger.Trace("Assigned robot action order: To load reel from cart");
                        Debug.WriteLine($"Assigned robot action order (Cart): {currentLoadReelTowerId}");
                    }
                    else if (!ReelTowerManager.HasUnloadRequest())
                    {   // Query tower state to search which one is available to load.
                        // Reset load reel information.
                        Interlocked.Exchange(ref currentLoadReelTower, -1);
                        reelTowerStep = ReelTowerSteps.CheckReelTowerState; // When a reel available on cart, but the reel tower is not ready to load.
                    }
                }
            }
            else if ((App.DigitalIoManager as DigitalIoManager).IsReturnReelExist) // Reel Return
            {
                if (currentReelTypeOfReturn == ReelDiameters.Unknown)
                {
                    if (Config.EnableReturnReelTypeWatcher)
                        reelTowerSubStep = SubSequenceSteps.Proceed;
                    else
                    {   // If a reel is detected on return stage. But the reel size is not defined yet.
                        returnReelPresentSensorTick = App.TickCount;
                        reelTowerSubStep = SubSequenceSteps.Proceed;
                    }
                }
                else
                {
                    if (returnReelPresentSensorTick == 0 || Config.EnableReturnReelTypeWatcher)
                    {
                        if (currentLoadReelTower < 0 && VerifyReturnReeelSize(currentReelTypeOfReturn))
                        {
                            if (currentReelTypeOfReturn != ReelDiameters.Unknown)
                            {    // Try to load return reel.
                                if (ReelTowerManager.IsPossibleLoadReel(ref currentLoadReelState))
                                {   // Assign a reel to load into tower.
                                    currentLoadReelTowerId = currentLoadReelState.Name;
                                    currentTowers[currentLoadReelState.Index - 1].Mode = ReelTowerOperationModes.Load;
                                    currentLoadReelState.SetLoadReelType(currentReelTypeOfReturn, true);
                                    Interlocked.Exchange(ref currentLoadReelTower, currentLoadReelState.Index);
                                    // Assign a new operation order to robot. (Load a reel from return stage)
                                    robotSequenceManager.RobotActionOrder = RobotActionOrder.LoadReelFromReturn;
                                    Logger.Trace("Assigned robot action order: To load reel from buffer.");
                                    Debug.WriteLine($"Assigned robot action order (Return): {currentLoadReelTowerId}");
                                }
                                else if (!ReelTowerManager.HasUnloadRequest())
                                {   // Pended a new unload request from tower. Check tower state again.
                                    // Reset load reel information.
                                    Interlocked.Exchange(ref currentLoadReelTower, -1);
                                    reelTowerStep = ReelTowerSteps.CheckReelTowerState;
                                }
                            }
                        }
                    }
                    else
                    {   // Abnormal case.
                        returnReelPresentSensorTick = App.TickCount;
                        reelTowerSubStep = SubSequenceSteps.Proceed;
                    }
                }
            }
        }

        protected void DefineRobotOperation()
        {
            switch (Singleton<TransferMaterialObject>.Instance.Mode)
            {
                case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                    {
                        if (robotSequenceManager.IsWaitForOrder)
                        {
                            currentRejectReelState.CopyFrom(currentUnloadReelState);
                            currentRejectReelState.State = MaterialStorageState.StorageOperationStates.Unload;
                            
                            if (currentRejectReelState.OutputStageIndex <= 0)
                                currentRejectReelState.SetRejectStage("X0101", 1);

                            robotSequenceManager.RobotActionOrder = RobotActionOrder.UnloadReelToReject;
                            robotStep = ReelHandlerSteps.Ready;
                            Logger.Trace("Assigned robot action order: To reject reel from tower");
                        }
                    }
                    break;
                case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                    {
                        if (robotSequenceManager.IsWaitForOrder)
                        {
                            currentRejectReelState.CopyFrom(currentLoadReelState);
                            currentRejectReelState.State = MaterialStorageState.StorageOperationStates.Load;
                            currentRejectReelState.SetRejectStage("X0101", 1);
                            robotSequenceManager.RobotActionOrder = RobotActionOrder.CartReelToReject;
                            robotStep = ReelHandlerSteps.Ready;
                            Logger.Trace("Assigned robot action order: To reject reel from cart");
                        }
                    }
                    break;
                case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                    {
                        if (robotSequenceManager.IsWaitForOrder)
                        {
                            currentRejectReelState.CopyFrom(currentLoadReelState);
                            currentRejectReelState.State = MaterialStorageState.StorageOperationStates.Load;
                            currentRejectReelState.SetRejectStage("X0101", 1);
                            robotSequenceManager.RobotActionOrder = RobotActionOrder.ReturnReelToReject;
                            robotStep = ReelHandlerSteps.Ready;
                            Logger.Trace("Assigned robot action order: To reject reel from return");
                        }
                    }
                    break;
                default:
                    {
                        if (currentUnloadReelTower <= 0) // Unload
                        {
                            if (ReelTowerManager.HasUnloadRequest())
                            {
                                bool finished_ = false;
                                bool safepos_ = false;

                                if (IsPossibleToSwitchTransferModeToUnload(ref finished_, ref safepos_))
                                {   // If a reel is ready to unload at reel tower port.
                                    // The currentUnloadReelTower is referred from unload reel sequence.
                                    Interlocked.Exchange(ref currentUnloadReelTower, ReelTowerManager.GetUnloadRequest(ref currentUnloadReelState));

                                    switch (currentUnloadReelTower)
                                    {
                                        case 1:
                                        case 2:
                                        case 3:
                                        case 4:
                                            {   // Direct unload a reel from tower.
                                                currentUnloadReelTowerId = currentUnloadReelState.Name;
                                                currentTowers[currentUnloadReelTower - 1].Mode = ReelTowerOperationModes.Unload;

                                                // Assign a new operation order to robot. (Unload a reel from a tower)
                                                robotSequenceManager.RobotActionOrder = RobotActionOrder.UnloadReelFromTower;
                                                Logger.Trace("Assigned robot action order: To unload reel from cart");
                                                Debug.WriteLine($"Assigned robot action order (Unload): {currentUnloadReelTowerId}");
                                            }
                                            break;
                                        default:
                                            {   // Abnormal case.
                                                Interlocked.Exchange(ref currentUnloadReelTower, -1);
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    if (safepos_ && robotSequenceManager.IsWaitForOrder)
                                        DefineRobotLoadOperation();
                                }
                            }
                            else
                                DefineRobotLoadOperation();
                        }
                    }
                    break;
            }
        }
        
        #region Reel tower sequence
        protected void FunctionReelTower()
        {
            bool result_ = false;
            int wrongstate_ = 0;
            int idlestate_ = 0;
            string log_ = "Changed reel tower step.";

            if (reelTowerStep != previousReelTowerStep)
            {
                stayCountReelTower_ = 0;
                Logger.ProcessTrace(MethodBase.GetCurrentMethod().Name, $"{reelTowerStep}", $"From={previousReelTowerStep},To={reelTowerStep}:{log_}");
                previousReelTowerStep   = reelTowerStep;
                reelTowerSubStep        = SubSequenceSteps.Prepare;
            }

            switch (reelTowerStep)
            {
                #region Check reel tower state and define robot operation order
                case ReelTowerSteps.None:
                case ReelTowerSteps.Ready:
                    {   // After reset (or clear) the occurred alarm. The sequence of reel tower interface have to enter ready step.
                        switch (reelTowerSubStep)
                        {
                            case SubSequenceSteps.Prepare:
                                {
                                    switch (robotSequenceManager.RobotActionOrder)
                                    {
                                        case RobotActionOrder.None:
                                            {   // No operation order and stopped state of robot.
                                                if (!robotSequenceManager.HasAReel)
                                                    DefineRobotOperation();
                                            }
                                            break;
                                        case RobotActionOrder.LoadReelFromCart:
                                        case RobotActionOrder.LoadReelFromReturn:
                                            {
                                                if (currentLoadReelTower < 0 && !robotSequenceManager.HasAReel)
                                                    DefineRobotOperation();
                                            }
                                            break;
                                        case RobotActionOrder.UnloadReelFromTower:
                                            {
                                                if (currentUnloadReelTower < 0 && !robotSequenceManager.HasAReel)
                                                    DefineRobotOperation();
                                            }
                                            break;
                                        case RobotActionOrder.CartReelToReject:
                                        case RobotActionOrder.ReturnReelToReject:
                                        case RobotActionOrder.UnloadReelToReject:
                                            // Set the robot order in PrepareToRejectReel step of FunctionRobot method.
                                            break;
                                    }
                                }
                                break;
                            case SubSequenceSteps.Proceed:
                                {
                                    if (IsReturnReelSensingDelayOver(timeOfReturnReelPresentValidation))
                                    {
                                        returnReelPresentSensorTick = 0;
                                        //CheckSizeOfReturnReeel();
                                        reelTowerSubStep = SubSequenceSteps.Post;
                                    }
                                }
                                break;
                            case SubSequenceSteps.Post:
                                {
                                    switch (currentReelTypeOfReturn)
                                    {
                                        case ReelDiameters.Unknown:
                                            reelTowerStep = ReelTowerSteps.Done;
                                            break;
                                        case ReelDiameters.ReelDiameter7:
                                        case ReelDiameters.ReelDiameter13:
                                            reelTowerStep = ReelTowerSteps.CheckReelTowerState;
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    break;
                #endregion

                #region Query reel tower state to search idle tower
                case ReelTowerSteps.CheckReelTowerState:
                    {
                        switch (reelTowerSubStep)
                        {
                            case SubSequenceSteps.Prepare:
                                {
                                     switch (Config.ReelTowerStateControlMode)
                                    {
                                        case ReelTowerStateControlModes.QueryReelTowerStateById:
                                            {
                                                if (ReelTowerManager.QueryStates(stateQueryId > ReelTowerRobotSequence.CONST_MAX_TOWER_COUNT ? stateQueryId = 1 : stateQueryId, true))
                                                {
                                                    stateQueryId++;
                                                    reelTowerSubStep = SubSequenceSteps.Proceed;
                                                }
                                                else
                                                {   // Send failure or a reel wait to unload.
                                                    reelTowerStep = ReelTowerSteps.Done;
                                                }
                                            }
                                            break;
                                        case ReelTowerStateControlModes.QueryReelTowerStateByAtOnce:
                                            {
                                                if (ReelTowerManager.QueryStatesAll(true))
                                                {
                                                    reelTowerSubStep = SubSequenceSteps.Proceed;
                                                }
                                                else
                                                {   // Send failure or a reel wait to unload.
                                                    if (ReelTowerManager.IsConnected && !ReelTowerManager.HasUnloadRequest())
                                                    {
                                                        reelTowerTick = App.TickCount;
                                                        reelTowerStep = ReelTowerSteps.StatePollingDelay;
                                                    }
                                                    else
                                                        reelTowerStep = ReelTowerSteps.Done;
                                                }
                                            }
                                            break;
                                        case ReelTowerStateControlModes.NotUseReelTowerStateQuery:
                                            {
                                                reelTowerStep = ReelTowerSteps.Done;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case SubSequenceSteps.Proceed:
                                {
                                    if (ReelTowerManager.IsReceivedStateResponse(ref result_))
                                    {   // Synchronize tower state
                                        result_ = false;

                                        for (int i_ = 0; i_ < currentTowers.Count; i_++)
                                        {
                                            currentTowers[i_].SetState(ReelTowerManager.TowerStates[i_].State);

                                            switch (currentTowers[i_].State)
                                            {
                                                case MaterialStorageState.StorageOperationStates.Idle:
                                                    {
                                                        idlestate_++;
                                                    }
                                                    break;
                                                case MaterialStorageState.StorageOperationStates.Run:
                                                    {   
                                                    }
                                                    break;
                                                case MaterialStorageState.StorageOperationStates.Unknown:
                                                case MaterialStorageState.StorageOperationStates.Down:
                                                case MaterialStorageState.StorageOperationStates.Load:
                                                case MaterialStorageState.StorageOperationStates.Unload:
                                                case MaterialStorageState.StorageOperationStates.Wait:
                                                    break;
                                                case MaterialStorageState.StorageOperationStates.Full:
                                                    {
                                                        wrongstate_++;
                                                    }
                                                    break;
                                                case MaterialStorageState.StorageOperationStates.Error:
                                                    result_ = true;
                                                    break;
                                            }
                                        }

                                        // if (result_)
                                        //    throw new PauseException<ErrorCode>(ErrorCode.ReelTowerIsAlarmState);
                                        // else if (wrongstate_ == 4)
                                        if (wrongstate_ == 4)
                                            throw new PauseException<ErrorCode>(ErrorCode.ReelTowerIsFull);
                                        // else if (idlestate_ == 0)
                                        //     throw new PauseException<ErrorCode>(ErrorCode.ReelTowerIsBusy);
                                        else
                                            reelTowerSubStep = SubSequenceSteps.Post;
                                    }
                                    else if (result_)
                                    {   // Timeout
                                        reelTowerSubStep = SubSequenceSteps.Post;
                                    }
                                }
                                break;
                            case SubSequenceSteps.Post:
                                {
                                    reelTowerStep = ReelTowerSteps.Done;
                                }
                                break;
                        }
                    }
                    break;
                case ReelTowerSteps.StatePollingDelay:
                    {
                        if (ReelTowerManager.IsConnected)
                        {
                            if (IsReelTowerStatePollingDelayOver(delayOfUnloadReelStateUpdate * 10) && ReelTowerManager.IsResponseTimeout())
                            {
                                ReelTowerManager.ResetResponseTimeout();
                                reelTowerStep = ReelTowerSteps.Done;
                            }
                        }
                        else
                        {
                            reelTowerStep = ReelTowerSteps.Done;
                        }
                    }
                    break;
                case ReelTowerSteps.Done:
                    {
                        reelTowerStep = ReelTowerSteps.Ready;
                    }
                    break;
                #endregion
            }
        }
        #endregion

        #region MRBT sequence
        protected void FunctionMobileRobot()
        {
            string log_ = "Changed mobile robot step.";

            if (mobileRobotStep != previousMobileRobotStep)
            {
                stayCountMobileRobot_ = 0;
                Logger.ProcessTrace(MethodBase.GetCurrentMethod().Name, $"{mobileRobotStep}", $"From={previousMobileRobotStep},To={mobileRobotStep}:{log_}");
                previousMobileRobotStep = mobileRobotStep;
                mobileRobotSubStep = SubSequenceSteps.Prepare;
            }

            if ((App.MainForm as FormMain).UseMobileRobot)
            {
                switch (mobileRobotStep)
                {
                    #region Check cart present sensors in work zone
                    case MobileRobotSteps.None:
                    case MobileRobotSteps.Ready:
                        {
                            int bitstate_ = !(App.DigitalIoManager as DigitalIoManager).WorkZoneCartPresentSensor1 ? 0 : 1;

                            switch (mobileRobotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        cartPresentSensorTick = App.TickCount;
                                        mobileRobotSubStep = SubSequenceSteps.Proceed;
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (IsCartPresentSensingDelayOver(timeOfMobileRobotClampValidation))
                                        {
                                            mobileRobotSubStep = SubSequenceSteps.Post;
                                            cartPresentSensorTick = 0;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        if (currentCartDockingState != previousCartDockingState)
                                        {
                                            Debug.WriteLine($"Cart state (Reel type, Docking state): {currentReelTypeOfCart}, {currentCartDockingState}");
                                            previousCartDockingState = currentCartDockingState;
                                        }

                                        switch (bitstate_ += (!(App.DigitalIoManager as DigitalIoManager).WorkZoneCartPresentSensor2 ? 0 : 2))
                                        {
                                            case 0:
                                                {   // Required to check load / unload mrbt cycle by continuously
                                                    switch (currentCartDockingState)
                                                    {
                                                        case CartDockingStates.Unknown:
                                                        case CartDockingStates.UnloadCompleted:
                                                            {   // Normal case.
                                                                mobileRobotStep = MobileRobotSteps.RequestCartLoad;
                                                            }
                                                            break;
                                                        case CartDockingStates.LoadStarted:
                                                        case CartDockingStates.Loading:
                                                            {   // Normal or alarm recovery.
                                                                mobileRobotStep = MobileRobotSteps.WaitForCartLoadStart;
                                                            }
                                                            break;
                                                        case CartDockingStates.LoadCompleted:
                                                            {   // Abnormal.
                                                                mobileRobotStep = MobileRobotSteps.CheckClampedCart;
                                                            }
                                                            break;
                                                        case CartDockingStates.UnloadStarted:
                                                        case CartDockingStates.Unloading:
                                                            {   // Normal or alarm recovery.
                                                                mobileRobotStep = MobileRobotSteps.WaitForCartUnloadStart;
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                            case 3:
                                                {
                                                    switch (currentReelTypeOfCart)
                                                    {
                                                        default:
                                                        case ReelDiameters.Unknown:
                                                            {   // Initial state or Abnormal case.
                                                                // Required verification of reel type and work tray index of cart.
                                                                ResetCurrentProductionCountOfWorkSlot();
                                                                mobileRobotStep = MobileRobotSteps.CheckClampedCart;
                                                            }
                                                            break;
                                                        case ReelDiameters.ReelDiameter7:
                                                            {
                                                                if (currentWorkSlotOfCart <= 6)
                                                                {   // Processing
                                                                    if ((App.DigitalIoManager as DigitalIoManager).IsCartReleasing)
                                                                        mobileRobotStep = MobileRobotSteps.CheckClampedCart;
                                                                }
                                                                else
                                                                {   // Finish
                                                                    mobileRobotStep = MobileRobotSteps.PrepareToUnloadCart;
                                                                }
                                                            }
                                                            break;
                                                        case ReelDiameters.ReelDiameter13:
                                                            {
                                                                if (currentWorkSlotOfCart <= 4)
                                                                {   // Processing
                                                                    if ((App.DigitalIoManager as DigitalIoManager).IsCartReleasing)
                                                                        mobileRobotStep = MobileRobotSteps.CheckClampedCart;
                                                                }
                                                                else
                                                                {   // Finish
                                                                    mobileRobotStep = MobileRobotSteps.PrepareToUnloadCart;
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                            case 1:
                                            case 2:
                                                {   // Abnormal case.
                                                    // Release cart clamp
                                                    (App.DigitalIoManager as DigitalIoManager).UndockCart();

                                                    // Failure reset. Don't reset reel type.
                                                    mobileRobotTick = 0;
                                                    currentCartDockingState = CartDockingStates.Unknown;
                                                    ResetCurrentProductionCountOfWorkSlot();
                                                    Debug.WriteLine($"Cart present sensor failure.");

                                                    // Raise an error.
                                                    throw new PauseException<ErrorCode>(ErrorCode.CartPresentSensorFailure);
                                                }
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Check cart docking state
                    case MobileRobotSteps.CheckClampedCart:
                        {
                            switch (mobileRobotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone) // (App.DigitalIoManager as DigitalIoManager).IsCartReleasing
                                        {   // Clamp cart in process.
                                            (App.DigitalIoManager as DigitalIoManager).ClampCart(DigitalIoManager.CartClamps.Side);
                                            mobileRobotSubStep = SubSequenceSteps.Proceed;
                                        }
                                        else
                                        {
                                            (App.DigitalIoManager as DigitalIoManager).UndockCart();
                                            currentCartDockingState = CartDockingStates.Unknown;
                                            mobileRobotStep = MobileRobotSteps.Ready;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                                        {
                                            if ((App.DigitalIoManager as DigitalIoManager).CartAlignSensorLeftForward &&
                                                (App.DigitalIoManager as DigitalIoManager).CartAlignSensorRightForward) // (App.DigitalIoManager as DigitalIoManager).IsCartReleasing
                                            {   // Clamp cart in process.
                                                (App.DigitalIoManager as DigitalIoManager).ClampCart(DigitalIoManager.CartClamps.Hook);
                                                mobileRobotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                        else
                                        {
                                            (App.DigitalIoManager as DigitalIoManager).UndockCart();
                                            currentCartDockingState = CartDockingStates.Unknown;
                                            mobileRobotStep = MobileRobotSteps.Ready;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                                        {
                                            if ((App.DigitalIoManager as DigitalIoManager).CartAlignSensorLeftForward &&
                                                (App.DigitalIoManager as DigitalIoManager).CartAlignSensorRightForward&&
                                                (App.DigitalIoManager as DigitalIoManager).CartAlignSensorFrontXBackward)
                                            {   // Clamp cart in process.
                                                if ((App.DigitalIoManager as DigitalIoManager).CartAlignCylinderFrontY)
                                                {
                                                    Debug.WriteLine($"Clamped cart.");
                                                    mobileRobotStep = MobileRobotSteps.PrepareProduction;
                                                }
                                                else
                                                {   // (App.DigitalIoManager as DigitalIoManager).ClampCart(DigitalIoManager.CartClamps.Pull);
                                                    (App.DigitalIoManager as DigitalIoManager).DockCart();
                                                    requiredVisionMarkAdjustment = Config.EnableVisionMarkAdjustment;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            (App.DigitalIoManager as DigitalIoManager).UndockCart();
                                            currentCartDockingState = CartDockingStates.Unknown;
                                            mobileRobotStep = MobileRobotSteps.Ready;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Load a cart
                    case MobileRobotSteps.RequestCartLoad:
                        {
                            switch (mobileRobotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    { // Prepare cart docking.
                                        if ((App.DigitalIoManager as DigitalIoManager).IsCartClamping || (App.DigitalIoManager as DigitalIoManager).IsCartHooked)
                                            (App.DigitalIoManager as DigitalIoManager).UndockCart();

                                        Debug.WriteLine($"Released the cart.");
                                        mobileRobotSubStep = SubSequenceSteps.Proceed;
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        // Reset work index and reel type.
                                        currentReelTypeOfCart = ReelDiameters.Unknown;
                                        mobileRobotManager.SendCommand(MobileRobotManagerCommands.WorkZoneLoad);
                                        currentCartDockingState = CartDockingStates.LoadStarted;
                                        mobileRobotSubStep = SubSequenceSteps.Post;
                                        Debug.WriteLine($"Send load a cart message to MRBT.");
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        if (mobileRobotManager.IsResponseTimeout(timeoutMobileRobotCommunication))
                                        {
                                            // UPDATED: 20200218 (JSRHIE)
                                            // MRBT client is not reply. (Required to change alarm code)
                                            throw new PauseException<ErrorCode>(ErrorCode.MobileRobotCommunicationFailure);
                                        }
                                        else if (mobileRobotManager.IsReceived)
                                        {
                                            mobileRobotStep = MobileRobotSteps.WaitForCartLoadStart;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case MobileRobotSteps.WaitForCartLoadStart:
                        {
                            if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                            {
                                currentCartDockingState = CartDockingStates.LoadCompleted;
                                mobileRobotStep = MobileRobotSteps.Done;
                            }
                            else
                            {
                                switch (mobileRobotSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (mobileRobotManager.CartDockingSequence == CartDockingSequences.StartLoadCart)
                                            {
                                                Debug.WriteLine($"Wait for a cart load into workzone.");
                                                mobileRobotSubStep = SubSequenceSteps.Proceed;
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                                            {
                                                cartPresentSensorTick = App.TickCount;
                                                mobileRobotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {
                                            Debug.WriteLine($"A cart is loaded into workzone.");
                                            mobileRobotStep = MobileRobotSteps.WaitForCartLoadComplete;
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case MobileRobotSteps.WaitForCartLoadComplete:
                        {
                            if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                            {
                                currentCartDockingState = CartDockingStates.LoadCompleted;
                                mobileRobotStep = MobileRobotSteps.Done;
                            }
                            else
                            {
                                switch (mobileRobotSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (mobileRobotManager.CartDockingSequence == CartDockingSequences.CompleteLoadCart &&
                                                ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone &&
                                                IsCartPresentSensingDelayOver(timeOfMobileRobotClampValidation)))
                                            {
                                                cartPresentSensorTick = 0;
                                                mobileRobotSubStep = SubSequenceSteps.Proceed;
                                                (App.DigitalIoManager as DigitalIoManager).ClampCart(DigitalIoManager.CartClamps.Side);
                                                Debug.WriteLine($"Received cart load complete message from MRBT.");
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            if ((App.DigitalIoManager as DigitalIoManager).CartAlignSensorLeftForward &&
                                                (App.DigitalIoManager as DigitalIoManager).CartAlignSensorRightForward)
                                            {
                                                (App.DigitalIoManager as DigitalIoManager).ClampCart(DigitalIoManager.CartClamps.Hook);
                                                mobileRobotSubStep = SubSequenceSteps.Post;
                                                Debug.WriteLine($"Clamp the loaded cart in workzone.");
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {   // Reset reel type and production count of work tray of cart.
                                            if ((App.DigitalIoManager as DigitalIoManager).CartAlignSensorLeftForward &&
                                                (App.DigitalIoManager as DigitalIoManager).CartAlignSensorRightForward &&
                                                (App.DigitalIoManager as DigitalIoManager).CartAlignSensorFrontXForward)
                                            {
                                                if ((App.DigitalIoManager as DigitalIoManager).CartAlignSensorFrontYForward)
                                                {
                                                    currentReelTypeOfCart = ReelDiameters.Unknown;
                                                    currentWorkSlotOfCart = 1;
                                                    ResetCurrentProductionCountOfWorkSlot();
                                                    mobileRobotManager.ResetCartDockingSequence();
                                                    currentCartDockingState = CartDockingStates.LoadCompleted;
                                                    mobileRobotStep = MobileRobotSteps.CheckClampedCart;
                                                    requiredVisionMarkAdjustment = Config.EnableVisionMarkAdjustment;
                                                    Debug.WriteLine($"Cart load sequence is done.");
                                                }
                                                else
                                                    (App.DigitalIoManager as DigitalIoManager).ClampCart(DigitalIoManager.CartClamps.Pull);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Prepare to run production
                    case MobileRobotSteps.PrepareProduction:
                        {
                            switch (currentReelTypeOfCart)
                            {
                                case ReelDiameters.Unknown: // Waiting reel type verification.
                                    break;
                                case ReelDiameters.ReelDiameter7:
                                case ReelDiameters.ReelDiameter13:
                                    {
                                        mobileRobotStep = MobileRobotSteps.Done;
                                        Debug.WriteLine($"The loaded cart is {currentReelTypeOfCart}");
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Check cart unload option and state
                    case MobileRobotSteps.PrepareToUnloadCart:
                        {
                            switch (mobileRobotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if ((App.MainForm as FormMain).UseMobileRobot)
                                        {   // Normal case.
                                            FireCartPresentStateChanged(CartPresentStates.Unknown);
                                            mobileRobotStep = MobileRobotSteps.RequestCartUnload;
                                            Debug.WriteLine($"Request unload the cart in workzone.");
                                        }
                                        else
                                            mobileRobotSubStep = SubSequenceSteps.Proceed;
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {   // Manual cart unload operation.
                                        if (!(App.DigitalIoManager as DigitalIoManager).IsCartReleasing)
                                        {
                                            FireCartPresentStateChanged(CartPresentStates.Exist);

                                            // Release cart
                                            (App.DigitalIoManager as DigitalIoManager).UndockCart();
                                            FireCartPresentStateChanged(CartPresentStates.Transferring);
                                            Debug.WriteLine($"Release cart in workzone.");
                                        }
                                        else
                                        {
                                            cartPresentSensorTick = App.TickCount;
                                            mobileRobotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {   // Have to move out the cart from work zone within 20 sec.
                                        if (IsCartPresentSensingDelayOver(timeOfMobileRobotClampValidation))
                                        {
                                            cartPresentSensorTick = 0;

                                            if (!(App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                                            {   // Success to release the cart by manual.
                                                FireCartPresentStateChanged(CartPresentStates.NotExist);
                                                FireShowNotification(Properties.Resources.String_Notification_Required_Cart);
                                                mobileRobotStep = MobileRobotSteps.Ready;
                                                Debug.WriteLine($"The cart is moved out from workzone.");
                                            }
                                            else
                                            {   // Abnormal case.
                                                // The cart is still in work zone.
                                                FireCartPresentStateChanged(CartPresentStates.Exist);
                                                // Don't reset reel type and work tray index of cart to support retry.
                                                FireShowNotification(Properties.Resources.String_Notification_Required_Unload_Cart);
                                                cartPresentSensorTick = App.TickCount;
                                                // mobileRobotStep = MobileRobotSteps.Ready;
                                                Debug.WriteLine($"The cart is still remained in work zone.");
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Unload the clamped cart
                    case MobileRobotSteps.RequestCartUnload:
                        {
                            switch (mobileRobotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if ((App.DigitalIoManager as DigitalIoManager).IsCartClamping || (App.DigitalIoManager as DigitalIoManager).IsCartHooked)
                                        {   // Release cart to prepare unload.
                                            (App.DigitalIoManager as DigitalIoManager).UndockCart();
                                        }

                                        mobileRobotSubStep = SubSequenceSteps.Proceed;
                                        Debug.WriteLine($"Released the cart.");
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {   // Don't reset reel type and work tray index of cart to support retry.
                                        mobileRobotManager.SendCommand(MobileRobotManagerCommands.WorkZoneUnload);
                                        currentCartDockingState = CartDockingStates.UnloadStarted;
                                        mobileRobotSubStep = SubSequenceSteps.Post;
                                        Debug.WriteLine($"Send unload the cart message to MRBT.");
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        if (mobileRobotManager.IsResponseTimeout(timeoutMobileRobotCommunication))
                                        {   // UPDATED: 20200218 (JSRHIE)
                                            // MRBT client is not reply. (Required to change alarm code)
                                            throw new PauseException<ErrorCode>(ErrorCode.MobileRobotCommunicationFailure);
                                        }
                                        else if (mobileRobotManager.IsReceived)
                                        {
                                            mobileRobotStep = MobileRobotSteps.WaitForCartUnloadStart;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case MobileRobotSteps.WaitForCartUnloadStart:
                        {
                            if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                            {
                                switch (mobileRobotSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (mobileRobotManager.CartDockingSequence == CartDockingSequences.StartUnloadCart)
                                            {
                                                mobileRobotSubStep = SubSequenceSteps.Proceed;
                                                Debug.WriteLine($"Wait for cart unload.");
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            if (!(App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                                                cartPresentSensorTick = App.TickCount;

                                            mobileRobotSubStep = SubSequenceSteps.Post;
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {
                                            mobileRobotStep = MobileRobotSteps.WaitForCartUnloadComplete;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                currentCartDockingState = CartDockingStates.UnloadCompleted;
                                mobileRobotStep = MobileRobotSteps.Done;
                            }
                        }
                        break;
                    case MobileRobotSteps.WaitForCartUnloadComplete:
                        {
                            if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                            {
                                switch (mobileRobotSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (mobileRobotManager.CartDockingSequence == CartDockingSequences.CompleteUnloadCart &&
                                                (!(App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone &&
                                                IsCartPresentSensingDelayOver(timeOfMobileRobotClampValidation)))
                                            {
                                                cartPresentSensorTick = 0;
                                                mobileRobotSubStep = SubSequenceSteps.Proceed;
                                                Debug.WriteLine($"Received the cart unload complete message from MRBT.");
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            mobileRobotSubStep = SubSequenceSteps.Post;
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {   // Reset reel type and work tray index of cart to avoid abnormal case.
                                            currentReelTypeOfCart = ReelDiameters.Unknown;
                                            ResetCurrentProductionCountOfWorkSlot();
                                            mobileRobotManager.ResetCartDockingSequence();
                                            currentCartDockingState = CartDockingStates.UnloadCompleted;
                                            mobileRobotStep = MobileRobotSteps.Done;
                                            Debug.WriteLine($"Reset cart docking sequence.");
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                currentCartDockingState = CartDockingStates.UnloadCompleted;
                                mobileRobotStep = MobileRobotSteps.Done;
                            }
                        }
                        break;
                    case MobileRobotSteps.Done:
                        {
                            mobileRobotStep = MobileRobotSteps.Ready;
                        }
                        break;
                        #endregion
                }
            }
            else
            {
                mobileRobotStep = MobileRobotSteps.Ready;
                mobileRobotSubStep = SubSequenceSteps.Prepare;

                if (!(App.DigitalIoManager as DigitalIoManager).WorkZoneCartPresentSensor1 &
                    !(App.DigitalIoManager as DigitalIoManager).WorkZoneCartPresentSensor2)
                {
                    switch (currentReelTypeOfCart)
                    {
                        case ReelDiameters.Unknown: // Waiting reel type verification.
                            {
                                currentCartDockingState = CartDockingStates.Unknown;
                                currentWorkSlotOfCart = 1;
                            }
                            break;
                        case ReelDiameters.ReelDiameter7:
                            {
                                if (currentWorkSlotOfCart <= 6)
                                    currentCartDockingState = CartDockingStates.LoadCompleted;
                                else
                                {
                                    currentCartDockingState = CartDockingStates.Unknown;
                                    currentWorkSlotOfCart = 1;
                                }
                            }
                            break;
                        case ReelDiameters.ReelDiameter13:
                            {
                                if (currentWorkSlotOfCart <= 4)
                                    currentCartDockingState = CartDockingStates.LoadCompleted;
                                else
                                {
                                    currentCartDockingState = CartDockingStates.Unknown;
                                    currentWorkSlotOfCart = 1;
                                }
                            }
                            break;
                    }
                }
                else if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone & 
                    (App.DigitalIoManager as DigitalIoManager).IsCartHooked)
                {
                    switch (currentReelTypeOfCart)
                    {
                        case ReelDiameters.Unknown: // Waiting reel type verification.
                            currentCartDockingState = CartDockingStates.Unknown;
                            break;
                        case ReelDiameters.ReelDiameter7:
                            {
                                if (currentWorkSlotOfCart <= 6)
                                    currentCartDockingState = CartDockingStates.LoadCompleted;
                                else
                                    currentCartDockingState = CartDockingStates.Unknown;
                            }
                            break;
                        case ReelDiameters.ReelDiameter13:
                            {
                                if (currentWorkSlotOfCart <= 4)
                                    currentCartDockingState = CartDockingStates.LoadCompleted;
                                else
                                    currentCartDockingState = CartDockingStates.Unknown;
                            }
                            break;
                    }
                }
            }
        }
        #endregion

        #region Robot sequence
        protected virtual bool CheckUnloadRequestQueueAndIntercept(bool finished, bool safeposition)
        {
            if (!robotSequenceManager.HasAReel && finished &&
                (robotSequenceManager.IsAtSafePosition || safeposition))
            {
                if (ReelTowerManager.HasUnloadRequest())
                {
                    if (transferMaterialObjectMode != $"TransferMaterialObject Mode: {Singleton<TransferMaterialObject>.Instance.Mode}")
                        Logger.Trace(transferMaterialObjectMode = $"TransferMaterialObject Mode: {Singleton<TransferMaterialObject>.Instance.Mode}");

                    switch (robotSequenceManager.LastExecutedCommand)
                    {
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot5OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot6OfReel7Cart:
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot1OfReel7Cart:
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot2OfReel7Cart:
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot3OfReel7Cart:
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot4OfReel7Cart:
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot5OfReel7Cart:
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot6OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel13Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel13Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel13Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel13Cart:
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot1OfReel13Cart:
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot2OfReel13Cart:
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot3OfReel13Cart:
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot4OfReel13Cart:
                            {
                                return false;
                            }
                    }

                    switch (Singleton<TransferMaterialObject>.Instance.Mode)
                    {
                        case TransferMaterialObject.TransferModes.None:
                        case TransferMaterialObject.TransferModes.PrepareToLoad:
                        case TransferMaterialObject.TransferModes.Load:
                        case TransferMaterialObject.TransferModes.PrepareToLoadReturn:
                        case TransferMaterialObject.TransferModes.LoadReturn:
                            {
                                switch (Singleton<TransferMaterialObject>.Instance.State)
                                {
                                    case TransferMaterialObject.TransferStates.None:
                                    case TransferMaterialObject.TransferStates.RequestToLoadConfirm:
                                    case TransferMaterialObject.TransferStates.WaitForLoadConfirm:
                                    case TransferMaterialObject.TransferStates.ConfirmLoad:
                                    case TransferMaterialObject.TransferStates.RequestToBarcodeConfirm:
                                    case TransferMaterialObject.TransferStates.WaitForBarcodeConfirm:
                                    case TransferMaterialObject.TransferStates.CompleteLoad:
                                        {
                                            CancelLoadProcedure();
                                            Interlocked.Exchange(ref currentLoadReelTower, -1);
                                            return true;
                                        }
                                    case TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart:
                                    case TransferMaterialObject.TransferStates.ConfirmedBarcodeOfReturn:
                                    case TransferMaterialObject.TransferStates.RequestToLoadAssignment:
                                    case TransferMaterialObject.TransferStates.WaitForLoadAssignment:
                                    case TransferMaterialObject.TransferStates.VerifiedUnload:
                                    case TransferMaterialObject.TransferStates.TakenMaterial:
                                    case TransferMaterialObject.TransferStates.RequestToUnloadAssignment:
                                    case TransferMaterialObject.TransferStates.WaitForUnloadAssignment:
                                    case TransferMaterialObject.TransferStates.CompleteUnload:
                                        break;
                                }
                            }
                            break;
                        case TransferMaterialObject.TransferModes.Unload:
                            {   // Immediately stop to load sequence.
                                Logger.Trace($"TransferMaterialObject State: {Singleton<TransferMaterialObject>.Instance.State}");
                                CancelLoadProcedure();

                                if (!currentUnloadReelState.IsAssignedToUnload)
                                    Interlocked.Exchange(ref currentUnloadReelTower, -1);

                                Interlocked.Exchange(ref currentLoadReelTower, -1);
                                return true;
                            }
                    }
                }
            }

            return false;
        }

        protected virtual void JudgeRobotOperationToLoadReel(bool finished, bool safeposition)
        {
            if (cycleStop)
            {
                if ((robotSequenceManager.IsAtSafePosition || safeposition) && finished)
                    robotStep = ReelHandlerSteps.Done;
            }
            else
            {
                switch (robotSequenceManager.RobotActionOrder)
                {
                    case RobotActionOrder.LoadReelFromCart:
                        {
                            if (robotSequenceManager.IsAtSafePosition || safeposition)
                            {
                                if (robotSequenceManager.ReelTypeOfCart == currentReelTypeOfCart &&
                                    currentReelTypeOfCart != ReelDiameters.Unknown)
                                {
                                    reelTowerQueryRetryCount = 0;
                                    robotStep = ReelHandlerSteps.PrepareToLoadReelFromCart;
                                }
                                else if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone &&
                                    (App.DigitalIoManager as DigitalIoManager).IsCartHooked)
                                {
                                    robotTick = 0;
                                    robotVisionRetryCount = 0;
                                    robotStep = ReelHandlerSteps.CheckCartReelType;
                                }
                            }
                        }
                        break;
                    case RobotActionOrder.LoadReelFromReturn:
                        {
                            if ((robotSequenceManager.IsAtSafePosition || safeposition) &&
                                currentReelTypeOfReturn != ReelDiameters.Unknown && (App.DigitalIoManager as DigitalIoManager).IsReturnReelExist)
                            {
                                reelTowerQueryRetryCount = 0;
                                robotStep = ReelHandlerSteps.PrepareToLoadReturnReel;
                            }
                        }
                        break;
                }
            }
        }

        protected virtual void ForceSetTransferMode()
        {
            if (!robotSequenceManager.HasAReel && robotSequenceManager.IsReadyToUnloadReel)
            {
                switch (robotSequenceManager.RobotActionOrder)
                {
                    case RobotActionOrder.LoadReelFromCart:
                        {
                            if (Singleton<TransferMaterialObject>.Instance.Mode != TransferMaterialObject.TransferModes.PrepareToLoad)
                                Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.PrepareToLoad);
                        }
                        break;
                    case RobotActionOrder.LoadReelFromReturn:
                        {
                            if (Singleton<TransferMaterialObject>.Instance.Mode != TransferMaterialObject.TransferModes.PrepareToLoadReturn)
                                Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.PrepareToLoad);
                        }
                        break;
                    case RobotActionOrder.UnloadReelFromTower:
                        {
                            if (Singleton<TransferMaterialObject>.Instance.Mode != TransferMaterialObject.TransferModes.Unload &&
                                Singleton<TransferMaterialObject>.Instance.Mode != TransferMaterialObject.TransferModes.None)
                                Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.PrepareToUnload);
                        }
                        break;
                }
            }
        }

        #region Define robot operations
        protected virtual void DefineRobotOperationToUnloadReel(RobotSequenceCommands command, bool finished, bool safeposition)
        {
            if (currentUnloadReelTower > 0 && currentUnloadReelTower == currentUnloadReelState.Index)
            {   
                switch (command)
                {
                    case RobotSequenceCommands.Unknown:  // Required initialize of robot
                        break;
                    default:
                    case RobotSequenceCommands.MoveToHome: // Home
                        {   // Need to confirm currentLoadReelTower state
                            if (robotSequenceManager.HasAReel)
                            {   // Already has a reel to load.
                                // Abnormal case.
                                // Need to check transfer reel information
                            }
                            else
                            {
                                if (cycleStop)
                                {
                                    if ((robotSequenceManager.IsAtSafePosition || safeposition) && finished)
                                        robotStep = ReelHandlerSteps.Done;
                                }
                                else
                                {
                                    switch (robotSequenceManager.RobotActionOrder)
                                    {
                                        case RobotActionOrder.UnloadReelFromTower:
                                            {
                                                if (robotSequenceManager.IsAtSafePosition || safeposition)
                                                {
                                                    ForceSetTransferMode();
                                                    reelTowerQueryRetryCount = 0;
                                                    robotStep = ReelHandlerSteps.PrepareToUnloadTowerReel;
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    case RobotSequenceCommands.MoveToUnloadFrontOfTower1:
                    case RobotSequenceCommands.MoveToUnloadFrontOfTower2:
                    case RobotSequenceCommands.MoveToUnloadFrontOfTower3:
                    case RobotSequenceCommands.MoveToUnloadFrontOfTower4:
                        {
                            if (robotSequenceManager.HasAReel)
                            {   // Abnormal case
                                throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                            }
                            else
                            {
                                if (robotSequenceManager.IsWaitForOrder && Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.Unload)
                                {
                                    switch (Singleton<TransferMaterialObject>.Instance.State)
                                    {
                                    case TransferMaterialObject.TransferStates.VerifiedUnload:
                                        {
                                            reelTowerQueryRetryCount = 0;
                                            robotStep = ReelHandlerSteps.TakeReelFromUnloadTower;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    case RobotSequenceCommands.TakeReelFromTower1:
                    case RobotSequenceCommands.TakeReelFromTower2:
                    case RobotSequenceCommands.TakeReelFromTower3:
                    case RobotSequenceCommands.TakeReelFromTower4:
                        {   // UPDATED: 20200408 (Marcus)
                            // Switch robot operation to reject a reel by transfer mode of material.
                            if (robotSequenceManager.HasAReel)
                            {   
                                if (robotSequenceManager.IsWaitForOrder)
                                {
                                    switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                    {
                                        case TransferMaterialObject.TransferModes.Unload:
                                            {
                                                if (Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.TakenMaterial)
                                                {
                                                    reelTowerQueryRetryCount = 0;
                                                    robotStep = ReelHandlerSteps.MoveToFrontOfOutputStage;
                                                }
                                            }
                                            break;
                                        case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                                            {
                                                if (Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.TakenMaterial)
                                                {
                                                    reelTowerQueryRetryCount = 0;
                                                    robotStep = ReelHandlerSteps.PrepareToRejectReel;
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                            {   // Abnormal case. (Unload Reel Pick Failed)
                                switch (Singleton<TransferMaterialObject>.Instance.State)
                                {
                                    case TransferMaterialObject.TransferStates.VerifiedUnload:
                                    case TransferMaterialObject.TransferStates.TakenMaterial:
                                        {
                                            reelTowerQueryRetryCount = 0;

                                            switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                            {
                                                case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                                                case TransferMaterialObject.TransferModes.Reject:
                                                    robotStep = ReelHandlerSteps.MoveBackToFrontOfTowerByReelPickupFailureToReject;
                                                    break;
                                                case TransferMaterialObject.TransferModes.PrepareToUnload:
                                                case TransferMaterialObject.TransferModes.Unload:
                                                    robotStep = ReelHandlerSteps.MoveBackToFrontOfTowerByReelPickupFailureToUnload;
                                                    break;
                                            }
                                        }
                                        break;
                                    case TransferMaterialObject.TransferStates.None:
                                    case TransferMaterialObject.TransferStates.CompleteUnload:
                                        {
                                            if ((robotSequenceManager.IsAtSafePosition || safeposition) && robotSequenceManager.IsWaitForOrder)
                                                robotStep = ReelHandlerSteps.CompletedToUnloadReelFromTower;
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case RobotSequenceCommands.MoveBackToFrontOfUnloadTower1:
                    case RobotSequenceCommands.MoveBackToFrontOfUnloadTower2:
                    case RobotSequenceCommands.MoveBackToFrontOfUnloadTower3:
                    case RobotSequenceCommands.MoveBackToFrontOfUnloadTower4:
                        {
                            if (robotSequenceManager.IsWaitForOrder)
                            {
                                if (robotSequenceManager.HasAReel)
                                {   // Abnormal case
                                    throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                                }
                                else
                                {
                                    switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                    {
                                        case TransferMaterialObject.TransferModes.Unload:
                                            {
                                                // Abnormal state handling to recover. (Unload Reel Pick Failed)
                                                switch (Singleton<TransferMaterialObject>.Instance.State)
                                                {
                                                    case TransferMaterialObject.TransferStates.VerifiedUnload:
                                                    case TransferMaterialObject.TransferStates.TakenMaterial:
                                                        {
                                                            reelTowerQueryRetryCount = 0;
                                                            robotStep = ReelHandlerSteps.MoveBackToFrontOfTowerByReelPickupFailureToUnload;
                                                        }
                                                        break;
                                                    case TransferMaterialObject.TransferStates.None:
                                                    case TransferMaterialObject.TransferStates.CompleteUnload:
                                                        {
                                                            if ((robotSequenceManager.IsAtSafePosition || safeposition))
                                                                robotStep = ReelHandlerSteps.CompletedToUnloadReelFromTower;
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case TransferMaterialObject.TransferModes.None:
                                            {
                                                if ((robotSequenceManager.IsAtSafePosition || safeposition))
                                                {
                                                    reelTowerQueryRetryCount = 0;
                                                    robotStep = ReelHandlerSteps.PrepareToUnloadTowerReel;
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    // Put an unload reel to output stage.
                    case RobotSequenceCommands.MoveToUnloadFrontOfOutput1: // ApplyReelType for return
                    case RobotSequenceCommands.MoveToUnloadFrontOfOutput2:
                    case RobotSequenceCommands.MoveToUnloadFrontOfOutput3:
                    case RobotSequenceCommands.MoveToUnloadFrontOfOutput4:
                    case RobotSequenceCommands.MoveToUnloadFrontOfOutput5:
                    case RobotSequenceCommands.MoveToUnloadFrontOfOutput6:
                        {
                            if (robotSequenceManager.HasAReel)
                            {
                                if (robotSequenceManager.IsWaitForOrder && Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.Unload)
                                {
                                    reelTowerQueryRetryCount = 0;

                                    if (Config.OutStageIds.Count > 3 && IsLatestVersion)
                                        robotStep = ReelHandlerSteps.CheckUpStateOfOutputStage;
                                    else
                                        robotStep = ReelHandlerSteps.ApproachOutputStage;
                                }
                            }
                            else
                            {   // Abnormal case.
                                throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                            }
                        }
                        break;
                    case RobotSequenceCommands.ApproachToUnloadFrontOfOutput1:
                    case RobotSequenceCommands.ApproachToUnloadFrontOfOutput2:
                    case RobotSequenceCommands.ApproachToUnloadFrontOfOutput3:
                    case RobotSequenceCommands.ApproachToUnloadFrontOfOutput4:
                    case RobotSequenceCommands.ApproachToUnloadFrontOfOutput5:
                    case RobotSequenceCommands.ApproachToUnloadFrontOfOutput6:
                        {
                            if (robotSequenceManager.HasAReel)
                            {
                                if (robotSequenceManager.IsWaitForOrder)
                                    robotStep = ReelHandlerSteps.PutReelIntoOutputStage;
                            }
                            else
                            {   // Abnormal case.
                                throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                            }
                        }
                        break;
                    case RobotSequenceCommands.PutReelIntoOutput1:
                    case RobotSequenceCommands.PutReelIntoOutput2:
                    case RobotSequenceCommands.PutReelIntoOutput3:
                    case RobotSequenceCommands.PutReelIntoOutput4:
                    case RobotSequenceCommands.PutReelIntoOutput5:
                    case RobotSequenceCommands.PutReelIntoOutput6:
                    case RobotSequenceCommands.PutReelIntoReject:
                        {  
                            if (robotSequenceManager.HasAReel)
                            {   // Abnormal case.
                                throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                            }
                            else
                            {
                                if (robotSequenceManager.IsWaitForOrder &&
                                    Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.Unload ||
                                    Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.None)
                                {
                                    switch (Singleton<TransferMaterialObject>.Instance.State)
                                    {
                                        case TransferMaterialObject.TransferStates.None:
                                            {
                                                if ((robotSequenceManager.IsAtSafePosition || safeposition) && robotSequenceManager.IsWaitForOrder)
                                                {
                                                    reelTowerQueryRetryCount = 0;
                                                    robotStep = ReelHandlerSteps.PrepareToUnloadTowerReel;
                                                }
                                            }
                                            break;
                                        case TransferMaterialObject.TransferStates.RequestToUnloadAssignment:
                                            {
                                                reelTowerQueryRetryCount = 0;
                                                robotStep = ReelHandlerSteps.RequestToConfirmUnloadedReelAssign;
                                            }
                                            break;
                                        case TransferMaterialObject.TransferStates.CompleteUnload:
                                            {
                                                if ((robotSequenceManager.IsAtSafePosition || safeposition) && robotSequenceManager.IsWaitForOrder)
                                                    robotStep = ReelHandlerSteps.CompletedToUnloadReelFromTower;
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot5OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot6OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel13Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel13Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel13Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel13Cart:
                        {
                            if (robotSequenceManager.HasAReel)
                            {   // Abnormal case
                                throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                            }
                            else
                            {
                                robotVisionRetryCount = 0;
                                robotVisionRetryAttemptsCount = 0;
                                reelTowerQueryRetryCount = 0;
                                robotStep = ReelHandlerSteps.GoToHomeBeforeReelHeightCheck;
                            }
                        }
                        break;
                }
            }
            else
            {   // Abnormal case.
                robotStep = ReelHandlerSteps.Done;
            }
        }

        protected virtual void DefineRobotOperationToLoadReel(RobotSequenceCommands command, bool finished, bool safeposition)
        {
            if (command != previousRobotCommand)
            {
                Logger.Trace($"Robot command: Previous={previousRobotCommand},Current={command}");
                previousRobotCommand = command;
            }

            if (IsCartAvailable ||
                (App.DigitalIoManager as DigitalIoManager).IsReturnReelExist ||
                robotSequenceManager.RobotActionOrder == RobotActionOrder.LoadReelFromReturn ||
                App.DigitalIoManager.IsSimulation)
            {
                if (robotSequenceManager.RobotActionOrder == RobotActionOrder.LoadReelFromCart && currentReelTypeOfCart == ReelDiameters.Unknown && 
                    (App.DigitalIoManager as DigitalIoManager).IsCartHooked && (App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                {
                    robotTick = 0;
                    robotVisionRetryCount = 0;
                    robotStep = ReelHandlerSteps.CheckCartReelType;
                }
                else if (currentLoadReelTower > 0 && currentLoadReelTower == currentLoadReelState.Index)
                {   // Load reel from cart or return stage
                    if (robotOperationState != $"Robot operation check: {robotSequenceManager.HasAReel},{finished},{robotSequenceManager.IsAtSafePosition},{safeposition}")
                        Logger.Trace(robotOperationState = $"Robot operation check: {robotSequenceManager.HasAReel},{finished},{robotSequenceManager.IsAtSafePosition},{safeposition}");

                    if (!CheckUnloadRequestQueueAndIntercept(finished, safeposition))
                    {
                        if (!finished && !safeposition && !robotSequenceManager.HasAReel && initialized && command == RobotSequenceCommands.Unknown)
                            command = robotSequenceManager.LastExecutedCommand;
                        
                        switch (command)
                        {   // Load cart reel
                            case RobotSequenceCommands.Unknown:  // Required initialize of robot
                                break;
                            case RobotSequenceCommands.PutReelIntoOutput1:
                            case RobotSequenceCommands.PutReelIntoOutput2:
                            case RobotSequenceCommands.PutReelIntoOutput3:
                            case RobotSequenceCommands.PutReelIntoOutput4:
                            case RobotSequenceCommands.PutReelIntoOutput5:
                            case RobotSequenceCommands.PutReelIntoOutput6:
                            case RobotSequenceCommands.PutReelIntoReject:
                            case RobotSequenceCommands.MoveToHome: // Home
                                {   // Need to confirm currentLoadReelTower state
                                    if (robotSequenceManager.HasAReel)
                                    {   // Already has a reel to load.
                                        // Abnormal case.
                                        // Need to check transfer reel information
                                        // We need warning to operator!!!
                                        throw new PauseException<ErrorCode>(ErrorCode.RobotReelDetectionSensorFailure);
                                    }
                                    else
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                        {
                                            case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                            case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                            case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                                                {
                                                    if (barcodeConfirmState == BarcodeConfirmStates.Reject)
                                                        robotStep = ReelHandlerSteps.PrepareToRejectReel;
                                                }
                                                break;
                                            default:
                                                {
                                                    JudgeRobotOperationToLoadReel(finished, safeposition);
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.CheckReelTypeOfCart:
                                {
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        if (currentReelTypeOfCart == ReelDiameters.Unknown)
                                        {   // Retry after recover error or interrupted state of robot sequence.
                                            robotTick = 0;
                                            robotVisionRetryCount = 0;
                                            robotStep = ReelHandlerSteps.CheckCartReelType;
                                        }
                                        else if (currentReelTypeOfCart != ReelDiameters.Unknown &&
                                            robotSequenceManager.ReelTypeOfCart != currentReelTypeOfCart)
                                        {
                                            robotTick = App.TickCount;
                                            robotActionRetryCount = 0;
                                            robotStep = ReelHandlerSteps.SetReelTypeOfCartToRobot;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.ApplyReelTypeOfCart: // ApplyReelType
                                {
                                    if (cycleStop)
                                    {
                                        if ((robotSequenceManager.IsAtSafePosition || safeposition) && finished)
                                            robotStep = ReelHandlerSteps.Done;
                                    }
                                    else if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        reelTowerQueryRetryCount = 0;
                                        robotStep = (requiredVisionMarkAdjustment && Config.EnableVisionMarkAdjustment? ReelHandlerSteps.CheckCartGuidePoint1 : ReelHandlerSteps.PrepareToLoadReelFromCart);
                                    }
                                }
                                break;
                            case RobotSequenceCommands.AdjustCartGuidePoint1:
                            case RobotSequenceCommands.CheckCartGuidePoint1:
                                {
                                    if (cycleStop)
                                    {
                                        if ((robotSequenceManager.IsAtSafePosition || safeposition) && finished)
                                            robotStep = ReelHandlerSteps.Done;
                                    }
                                    else
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (currentReelTypeOfCart == ReelDiameters.Unknown)
                                            {   // Retry after recover error or interrupted state of robot sequence.
                                                robotTick = 0;
                                                robotVisionRetryCount = 0;
                                                robotStep = ReelHandlerSteps.CheckCartReelType;
                                            }
                                            else if (currentReelTypeOfCart != ReelDiameters.Unknown)
                                            {
                                                robotTick = App.TickCount;
                                                robotActionRetryCount = 0;
                                                robotStep = ReelHandlerSteps.CheckCartGuidePoint1;
                                            }
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.ApplyCartGuidePoint1:
                                {   // UPDATED: 20200513 (Marcus)
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        reelTowerQueryRetryCount = 0;
                                        robotStep = ReelHandlerSteps.SetCartGuideWorkSlotCenter1;
                                    }
                                }
                                break;
                            case RobotSequenceCommands.AdjustCartGuidePoint2:
                            case RobotSequenceCommands.CheckCartGuidePoint2:
                                {
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        if (currentReelTypeOfCart == ReelDiameters.Unknown)
                                        {   // Retry after recover error or interrupted state of robot sequence.
                                            robotTick = 0;
                                            robotVisionRetryCount = 0;
                                            robotStep = ReelHandlerSteps.CheckCartReelType;
                                        }
                                        else if (currentReelTypeOfCart != ReelDiameters.Unknown)
                                        {
                                            robotTick = App.TickCount;
                                            robotActionRetryCount = 0;
                                            robotStep = ReelHandlerSteps.CheckCartGuidePoint2;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.ApplyCartGuidePoint2:
                                {   // UPDATED: 20200513 (Marcus)
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        reelTowerQueryRetryCount = 0;

                                        switch (currentReelTypeOfCart)
                                        {
                                            case ReelDiameters.ReelDiameter7:
                                                robotStep = ReelHandlerSteps.SetCartGuideWorkSlotCenter5;
                                                break;
                                            case ReelDiameters.ReelDiameter13:
                                                robotStep = ReelHandlerSteps.SetCartGuideWorkSlotCenter2;
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.AdjustCartGuidePoint3:
                            case RobotSequenceCommands.CheckCartGuidePoint3:
                                {
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        if (currentReelTypeOfCart == ReelDiameters.Unknown)
                                        {   // Retry after recover error or interrupted state of robot sequence.
                                            robotTick = 0;
                                            robotVisionRetryCount = 0;
                                            robotStep = ReelHandlerSteps.CheckCartReelType;
                                        }
                                        else if (currentReelTypeOfCart != ReelDiameters.Unknown)
                                        {
                                            robotTick = App.TickCount;
                                            robotActionRetryCount = 0;
                                            robotStep = ReelHandlerSteps.CheckCartGuidePoint3;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.ApplyCartGuidePoint3:
                                {   // UPDATED: 20200513 (Marcus)
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        reelTowerQueryRetryCount = 0;
                                        robotStep = ReelHandlerSteps.SetCartGuideWorkSlotCenter2;
                                    }
                                }
                                break;
                            case RobotSequenceCommands.AdjustCartGuidePoint4:
                            case RobotSequenceCommands.CheckCartGuidePoint4:
                                {   // Not use
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        if (currentReelTypeOfCart == ReelDiameters.Unknown)
                                        {   // Retry after recover error or interrupted state of robot sequence.
                                            robotTick = 0;
                                            robotVisionRetryCount = 0;
                                            robotStep = ReelHandlerSteps.CheckCartReelType;
                                        }
                                        else if (currentReelTypeOfCart != ReelDiameters.Unknown)
                                        {
                                            robotTick = App.TickCount;
                                            robotActionRetryCount = 0;
                                            robotStep = ReelHandlerSteps.CheckCartGuidePoint4;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.ApplyCartGuidePoint4:
                                {   // UPDATED: 20200513 (Marcus)
                                    // Not use
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        reelTowerQueryRetryCount = 0;
                                        robotStep = ReelHandlerSteps.SetCartGuideWorkSlotCenter5;
                                    }
                                }
                                break;
                            case RobotSequenceCommands.SetCartGuideWorkSlotCenter1:
                                {   // UPDATED: 20200625 (Marcus)
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        reelTowerQueryRetryCount = 0;
                                        switch (currentReelTypeOfCart)
                                        {
                                            case ReelDiameters.ReelDiameter7:
                                            case ReelDiameters.ReelDiameter13:
                                                robotStep = ReelHandlerSteps.SetCartGuideWorkSlotCenter3;
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.SetCartGuideWorkSlotCenter2:
                                {   // UPDATED: 20200625 (Marcus)
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        reelTowerQueryRetryCount = 0;
                                        switch (currentReelTypeOfCart)
                                        {
                                            case ReelDiameters.ReelDiameter7:
                                            case ReelDiameters.ReelDiameter13:
                                                robotStep = ReelHandlerSteps.SetCartGuideWorkSlotCenter4;
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.SetCartGuideWorkSlotCenter3:
                                {   // UPDATED: 20200625 (Marcus)
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        reelTowerQueryRetryCount = 0;
                                        switch (currentReelTypeOfCart)
                                        {
                                            case ReelDiameters.ReelDiameter7:
                                            case ReelDiameters.ReelDiameter13:
                                                robotStep = ReelHandlerSteps.CheckCartGuidePoint2;
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.SetCartGuideWorkSlotCenter4:
                                {   // UPDATED: 20200625 (Marcus)
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        switch (currentReelTypeOfCart)
                                        {
                                            case ReelDiameters.ReelDiameter7:
                                            case ReelDiameters.ReelDiameter13:
                                                {
                                                    if (loopCountOfCartGuidePoints.Count >= 2 && (visionMarkAdjustmentStates == 0x3F || visionMarkAdjustmentStates == 0x0F))
                                                    {
                                                        reelTowerQueryRetryCount = 0;
                                                        requiredVisionMarkAdjustment = false; // Complete to adjust vision marks
                                                        robotStep = ReelHandlerSteps.PrepareToLoadReelFromCart;
                                                    }
                                                    else
                                                    {
                                                        throw new PauseException<ErrorCode>(ErrorCode.FailedVisionMarkAdjustment);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.SetCartGuideWorkSlotCenter5:
                                {   // UPDATED: 20200625 (Marcus)
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        reelTowerQueryRetryCount = 0;
                                        switch (currentReelTypeOfCart)
                                        {
                                            case ReelDiameters.ReelDiameter7:
                                                robotStep = ReelHandlerSteps.SetCartGuideWorkSlotCenter6;
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.SetCartGuideWorkSlotCenter6:
                                {   // UPDATED: 20200625 (Marcus)
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        reelTowerQueryRetryCount = 0;
                                        switch (currentReelTypeOfCart)
                                        {
                                            case ReelDiameters.ReelDiameter7:
                                                {
                                                    robotStep = ReelHandlerSteps.CheckCartGuidePoint3;
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.ApplyWorkSlot: // ApplyWorkSlot
                                {
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        robotTick = 0;
                                        robotStep = ReelHandlerSteps.GoToHomeBeforeReelHeightCheck;
                                    }
                                }
                                break;
                            case RobotSequenceCommands.ResetProcessCount: // Reset production count
                                {
                                    if (robotSequenceManager.IsAtSafePosition && robotSequenceManager.IsWaitForOrder)
                                    {
                                        robotTick = 0;
                                        robotStep = ReelHandlerSteps.GoToHomeBeforeReelHeightCheck;
                                    }
                                }
                                break;
                            case RobotSequenceCommands.GoToHomeBeforeReelHeightCheck: // Move to home
                                {
                                    if (cycleStop)
                                    {
                                        if ((robotSequenceManager.IsAtSafePosition || safeposition) && finished)
                                            robotStep = ReelHandlerSteps.Done;
                                    }
                                    else
                                    {
                                        if (robotSequenceManager.IsWaitForOrder && (robotSequenceManager.IsAtSafePosition || safeposition))
                                        {
                                            switch (robotSequenceManager.RobotActionOrder)
                                            {
                                                case RobotActionOrder.LoadReelFromCart:
                                                    {
                                                        if (requiredVisionMarkAdjustment && Config.EnableVisionMarkAdjustment)
                                                            robotStep = ReelHandlerSteps.CheckCartReelType;
                                                        else
                                                        {
                                                            robotTick = -1;
                                                            robotActionRetryCount = 0;
                                                            robotStep = ReelHandlerSteps.MoveToReelHeightCheckPositionOfWorkSlot;
                                                        }
                                                    }
                                                    break;
                                                case RobotActionOrder.LoadReelFromReturn:
                                                    {
                                                        robotStep = ReelHandlerSteps.PrepareToLoadReturnReel;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel7Cart:
                            case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel7Cart:
                            case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel7Cart:
                            case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel7Cart:
                            case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot5OfReel7Cart:
                            case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot6OfReel7Cart:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {   // Abnormal case
                                        throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                                    }
                                    else
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (ReelTowerManager.HasUnloadRequest())
                                            {
                                                robotVisionRetryCount = 0;
                                                robotVisionRetryAttemptsCount = 0;
                                                reelTowerQueryRetryCount = 0;
                                                robotStep = ReelHandlerSteps.GoToHomeBeforeReelHeightCheck;
                                            }
                                            else
                                            {
                                                Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.PrepareToLoad);
                                                robotStep = ReelHandlerSteps.MeasureReelHeightOnCart;
                                            }
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.MeasureReelHeightAtWorkSlot1OfReel7Cart:
                            case RobotSequenceCommands.MeasureReelHeightAtWorkSlot2OfReel7Cart:
                            case RobotSequenceCommands.MeasureReelHeightAtWorkSlot3OfReel7Cart:
                            case RobotSequenceCommands.MeasureReelHeightAtWorkSlot4OfReel7Cart:
                            case RobotSequenceCommands.MeasureReelHeightAtWorkSlot5OfReel7Cart:
                            case RobotSequenceCommands.MeasureReelHeightAtWorkSlot6OfReel7Cart:
                                {
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                        {
                                            default:    // Abnormal case
                                                break;
                                            case TransferMaterialObject.TransferModes.PrepareToLoad:
                                                {
                                                    switch (Singleton<TransferMaterialObject>.Instance.State)
                                                    {
                                                        case TransferMaterialObject.TransferStates.None:
                                                            {
                                                                visionTick = -1;
                                                                robotVisionRetryCount = 0;
                                                                robotVisionRetryCycleCount = 0;

                                                                if (Config.ImageProcessorType == ImageProcessorTypes.Mit)
                                                                    CompositeVisionManager.SetVisionLight(currentReelTypeOfCart);

                                                                robotStep = ReelHandlerSteps.CheckReelAlignmentOnCart;
                                                            }
                                                            break;
                                                        case TransferMaterialObject.TransferStates.RequestToLoadConfirm:
                                                        case TransferMaterialObject.TransferStates.WaitForLoadConfirm:
                                                            {
                                                                reelTowerQueryRetryCount = 0;
                                                                robotStep = ReelHandlerSteps.RequestToReelLoadConfirm;
                                                            }
                                                            break;
                                                        case TransferMaterialObject.TransferStates.ConfirmLoad:
                                                            {
                                                                robotVisionRetryCount = 0;
                                                                robotVisionRetryAttemptsCount = 0;
                                                                robotVisionRetryCycleCount = 0;
                                                                robotStep = ReelHandlerSteps.PrepareToReadBarcodeOnReel;
                                                            }
                                                            break;
                                                        case TransferMaterialObject.TransferStates.RequestToBarcodeConfirm:
                                                        case TransferMaterialObject.TransferStates.WaitForBarcodeConfirm:
                                                            {
                                                                robotStep = ReelHandlerSteps.RequestToConfirmLoadReelBarcodeOfCart;
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                            case TransferMaterialObject.TransferModes.Load:
                                                {
                                                    switch (Singleton<TransferMaterialObject>.Instance.State)
                                                    {
                                                        default:    // Abnormal case
                                                            break;
                                                        case TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart:
                                                            {
                                                                robotStep = ReelHandlerSteps.AdjustPositionAndPickupReelOfCart;
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                            case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                {
                                                    switch (Singleton<TransferMaterialObject>.Instance.State)
                                                    {
                                                        default:    // Abnormal case
                                                        case TransferMaterialObject.TransferStates.RequestToBarcodeConfirm:
                                                        case TransferMaterialObject.TransferStates.WaitForBarcodeConfirm:
                                                        case TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart:
                                                            {
                                                                robotStep = ReelHandlerSteps.AdjustPositionAndPickupReelOfCart;
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel7Cart:
                            case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel7Cart:
                            case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel7Cart:
                            case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel7Cart:
                            case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot5OfReel7Cart:
                            case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot6OfReel7Cart:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                        {
                                            case TransferMaterialObject.TransferModes.Load:
                                            case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                            case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                {
                                                    switch (Singleton<TransferMaterialObject>.Instance.State)
                                                    {
                                                        default:
                                                        case TransferMaterialObject.TransferStates.RequestToBarcodeConfirm:
                                                        case TransferMaterialObject.TransferStates.WaitForBarcodeConfirm:
                                                        case TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart:
                                                            {
                                                                robotVisionRetryCount = 0;
                                                                robotVisionRetryAttemptsCount = 0;
                                                                reelTowerQueryRetryCount = 0;
                                                                robotStep = ReelHandlerSteps.GoToHomeAfterPickUpReel;
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    else
                                    {   // Abnormal case
                                        throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                                    }
                                }
                                break;
                            case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel13Cart:
                            case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel13Cart:
                            case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel13Cart:
                            case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel13Cart:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {   // Abnormal case
                                        throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                                    }
                                    else
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (ReelTowerManager.HasUnloadRequest())
                                            {
                                                robotVisionRetryCount = 0;
                                                robotVisionRetryAttemptsCount = 0;
                                                reelTowerQueryRetryCount = 0;
                                                robotStep = ReelHandlerSteps.GoToHomeBeforeReelHeightCheck;
                                            }
                                            else
                                            {
                                                Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.PrepareToLoad);
                                                robotStep = ReelHandlerSteps.MeasureReelHeightOnCart;
                                            }
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.MeasureReelHeightAtWorkSlot1OfReel13Cart:
                            case RobotSequenceCommands.MeasureReelHeightAtWorkSlot2OfReel13Cart:
                            case RobotSequenceCommands.MeasureReelHeightAtWorkSlot3OfReel13Cart:
                            case RobotSequenceCommands.MeasureReelHeightAtWorkSlot4OfReel13Cart:
                                {
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                        {
                                            default:    // Abnormal case
                                                break;
                                            case TransferMaterialObject.TransferModes.PrepareToLoad:
                                                {
                                                    switch (Singleton<TransferMaterialObject>.Instance.State)
                                                    {
                                                        case TransferMaterialObject.TransferStates.None:
                                                            {
                                                                visionTick = -1;
                                                                robotVisionRetryCount = 0;
                                                                robotVisionRetryAttemptsCount = 0;
                                                                robotVisionRetryCycleCount = 0;

                                                                if (Config.ImageProcessorType == ImageProcessorTypes.Mit)
                                                                    CompositeVisionManager.SetVisionLight(currentReelTypeOfCart);

                                                                robotStep = ReelHandlerSteps.CheckReelAlignmentOnCart;
                                                            }
                                                            break;
                                                        case TransferMaterialObject.TransferStates.RequestToLoadConfirm:
                                                        case TransferMaterialObject.TransferStates.WaitForLoadConfirm:
                                                            {
                                                                reelTowerQueryRetryCount = 0;
                                                                robotStep = ReelHandlerSteps.RequestToReelLoadConfirm;
                                                            }
                                                            break;
                                                        case TransferMaterialObject.TransferStates.ConfirmLoad:
                                                            {
                                                                robotVisionRetryCount = 0;
                                                                robotVisionRetryAttemptsCount = 0;
                                                                robotVisionRetryCycleCount = 0;
                                                                robotStep = ReelHandlerSteps.PrepareToReadBarcodeOnReel;
                                                            }
                                                            break;
                                                        case TransferMaterialObject.TransferStates.RequestToBarcodeConfirm:
                                                        case TransferMaterialObject.TransferStates.WaitForBarcodeConfirm:
                                                            {
                                                                robotStep = ReelHandlerSteps.RequestToConfirmLoadReelBarcodeOfCart;
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                            case TransferMaterialObject.TransferModes.Load:
                                                {
                                                    switch (Singleton<TransferMaterialObject>.Instance.State)
                                                    {
                                                        default:    // Abnormal case
                                                            break;
                                                        case TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart:
                                                            {
                                                                robotStep = ReelHandlerSteps.AdjustPositionAndPickupReelOfCart;
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                            case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                {
                                                    switch (Singleton<TransferMaterialObject>.Instance.State)
                                                    {
                                                        default:    // Abnormal case
                                                        case TransferMaterialObject.TransferStates.RequestToBarcodeConfirm:
                                                        case TransferMaterialObject.TransferStates.WaitForBarcodeConfirm:
                                                        case TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart:
                                                            {
                                                                robotStep = ReelHandlerSteps.AdjustPositionAndPickupReelOfCart;
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel13Cart:
                            case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel13Cart:
                            case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel13Cart:
                            case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel13Cart:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {
                                        if (Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.Load ||
                                            Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.PrepareToRejectCartReel ||
                                            Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.PrepareToRejectReturnReel)
                                        {
                                            if (Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart ||
                                                (Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.RequestToBarcodeConfirm &&
                                                barcodeConfirmState == BarcodeConfirmStates.Reject &&
                                                Config.EnableRejectReel))
                                            {
                                                robotVisionRetryCount = 0;
                                                robotVisionRetryAttemptsCount = 0;
                                                reelTowerQueryRetryCount = 0;
                                                robotStep = ReelHandlerSteps.GoToHomeAfterPickUpReel;
                                            }
                                        }
                                    }
                                    else
                                    {   // Abnormal case
                                        throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                                    }
                                }
                                break;
                            case RobotSequenceCommands.GoToHomeAfterPickUpReel:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                        {
                                            case TransferMaterialObject.TransferModes.Load:
                                            case TransferMaterialObject.TransferModes.LoadReturn:
                                                {
                                                    if (Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart)
                                                        robotStep = ReelHandlerSteps.MoveToFrontOfTower;
                                                }
                                                break;
                                            case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                            case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                {
                                                    robotStep = ReelHandlerSteps.PrepareToRejectReel;
                                                }
                                                break;
                                        }
                                    }
                                    else
                                    {   // Abnormal case
                                        switch (robotSequenceManager.RobotActionOrder)
                                        {
                                            case RobotActionOrder.LoadReelFromCart:
                                                {
                                                    switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                                    {
                                                        case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:  // Have to check
                                                        case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                            {
                                                                if (barcodeConfirmState == BarcodeConfirmStates.Reject && robotSequenceManager.ReelGrip) // && !robotSequenceManager.ReelDetector)
                                                                {
                                                                    robotStep = ReelHandlerSteps.PrepareToRejectReel;
                                                                }
                                                                else
                                                                {
                                                                    Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                                    robotStep = ReelHandlerSteps.Done;
                                                                }
                                                            }
                                                            break;
                                                        case TransferMaterialObject.TransferModes.None:
                                                        case TransferMaterialObject.TransferModes.PrepareToLoad:        // End reel
                                                        case TransferMaterialObject.TransferModes.Load:                 // After alarm reset (We need to reset the state and mode)                                                        
                                                            {
                                                                if (robotSequenceManager.IsWaitForOrder && (robotSequenceManager.IsAtSafePosition || safeposition))
                                                                {
                                                                    if (!robotSequenceManager.HasAReel)
                                                                    {
                                                                        switch (currentReelTypeOfCart)
                                                                        {
                                                                            case ReelDiameters.Unknown:
                                                                                {
                                                                                    robotStep = ReelHandlerSteps.Done;
                                                                                }
                                                                                break;
                                                                            case ReelDiameters.ReelDiameter7:
                                                                                {
                                                                                    if (currentWorkSlotOfCart <= 6)
                                                                                        robotStep = ReelHandlerSteps.PrepareToLoadReelFromCart;
                                                                                    else
                                                                                    {   // Wait for a new cart or other event to load or unload
                                                                                        currentWorkSlotOfCart = 1;
                                                                                        robotStep = ReelHandlerSteps.Done;
                                                                                    }
                                                                                }
                                                                                break;
                                                                            case ReelDiameters.ReelDiameter13:
                                                                                {
                                                                                    if (currentWorkSlotOfCart <= 4)
                                                                                        robotStep = ReelHandlerSteps.PrepareToLoadReelFromCart;
                                                                                    else
                                                                                    {   // Wait for a new cart or other event to load or unload
                                                                                        currentWorkSlotOfCart = 1;
                                                                                        robotStep = ReelHandlerSteps.Done;
                                                                                    }
                                                                                }
                                                                                break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                            case RobotActionOrder.LoadReelFromReturn:
                                                {
                                                    JudgeRobotOperationToLoadReel(finished, safeposition);
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.MoveToLoadFrontOfTower1:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {
                                        robotStep = ReelHandlerSteps.PutReelIntoTower;
                                    }
                                    else
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                        {
                                            case TransferMaterialObject.TransferStates.RequestToLoadAssignment:
                                                {
                                                    robotStep = ReelHandlerSteps.RequestToConfirmLoadedReelAssign;
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.MoveToLoadFrontOfTower2:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {
                                        robotStep = ReelHandlerSteps.PutReelIntoTower;
                                    }
                                    else
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                        {
                                            case TransferMaterialObject.TransferStates.RequestToLoadAssignment:
                                                {
                                                    robotStep = ReelHandlerSteps.RequestToConfirmLoadedReelAssign;
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.MoveToLoadFrontOfTower3:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {
                                        robotStep = ReelHandlerSteps.PutReelIntoTower;
                                    }
                                    else
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                        {
                                            case TransferMaterialObject.TransferStates.RequestToLoadAssignment:
                                                {
                                                    robotStep = ReelHandlerSteps.RequestToConfirmLoadedReelAssign;
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.MoveToLoadFrontOfTower4:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {
                                        robotStep = ReelHandlerSteps.PutReelIntoTower;
                                    }
                                    else
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                        {
                                            case TransferMaterialObject.TransferStates.RequestToLoadAssignment:
                                                {
                                                    robotStep = ReelHandlerSteps.RequestToConfirmLoadedReelAssign;
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.PutReelIntoTower1:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {
                                        robotStep = ReelHandlerSteps.PutReelIntoTower;
                                    }
                                    else
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                        {
                                            case TransferMaterialObject.TransferStates.RequestToLoadAssignment:
                                                {
                                                    robotStep = ReelHandlerSteps.RequestToConfirmLoadedReelAssign;
                                                }
                                                break;
                                            case TransferMaterialObject.TransferStates.CompleteLoad:
                                                {
                                                    if ((robotSequenceManager.IsAtSafePosition || safeposition) &&
                                                        robotSequenceManager.IsWaitForOrder)
                                                    {
                                                        JudgeRobotOperationToLoadReel(finished, safeposition);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.PutReelIntoTower2:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {
                                        robotStep = ReelHandlerSteps.PutReelIntoTower;
                                    }
                                    else
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                        {
                                            case TransferMaterialObject.TransferStates.RequestToLoadAssignment:
                                                {
                                                    robotStep = ReelHandlerSteps.RequestToConfirmLoadedReelAssign;
                                                }
                                                break;
                                            case TransferMaterialObject.TransferStates.CompleteLoad:
                                                {
                                                    if ((robotSequenceManager.IsAtSafePosition || safeposition) &&
                                                        robotSequenceManager.IsWaitForOrder)
                                                    {
                                                        JudgeRobotOperationToLoadReel(finished, safeposition);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.PutReelIntoTower3:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {
                                        robotStep = ReelHandlerSteps.PutReelIntoTower;
                                    }
                                    else
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                        {
                                            case TransferMaterialObject.TransferStates.RequestToLoadAssignment:
                                                {
                                                    robotStep = ReelHandlerSteps.RequestToConfirmLoadedReelAssign;
                                                }
                                                break;
                                            case TransferMaterialObject.TransferStates.CompleteLoad:
                                                {
                                                    if ((robotSequenceManager.IsAtSafePosition || safeposition) &&
                                                        robotSequenceManager.IsWaitForOrder)
                                                    {
                                                        JudgeRobotOperationToLoadReel(finished, safeposition);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.PutReelIntoTower4:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {
                                        robotStep = ReelHandlerSteps.PutReelIntoTower;
                                    }
                                    else
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                        {
                                            case TransferMaterialObject.TransferStates.RequestToLoadAssignment:
                                                {
                                                    robotStep = ReelHandlerSteps.RequestToConfirmLoadedReelAssign;
                                                }
                                                break;
                                            case TransferMaterialObject.TransferStates.CompleteLoad:
                                                {
                                                    if ((robotSequenceManager.IsAtSafePosition || safeposition) &&
                                                        robotSequenceManager.IsWaitForOrder)
                                                    {
                                                        JudgeRobotOperationToLoadReel(finished, safeposition);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            // Load return reel
                            case RobotSequenceCommands.ApplyReelTypeOfReturn: // ApplyReelType for return
                                {
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        robotTick = -1;
                                        reelTowerQueryRetryCount = 0;
                                        robotStep = ReelHandlerSteps.MoveToFrontOfReturnStage;
                                    }
                                }
                                break;
                            case RobotSequenceCommands.MoveToReel7OfReturnStage:
                            case RobotSequenceCommands.MoveToReel13OfReturnStage:
                            case RobotSequenceCommands.MoveBackToFrontOfReturnStage:
                            {
                                    if (robotSequenceManager.HasAReel)
                                    {   // Abnormal case
                                        throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                                    }
                                    else
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.PrepareToRejectReturnReel)
                                            {
                                                switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                                {
                                                    case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                    case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                    case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                                                        {
                                                            robotStep = ReelHandlerSteps.PrepareToRejectReel;
                                                        }
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                if (robotSequenceManager.RobotActionOrder == RobotActionOrder.LoadReelFromReturn &&
                                                    (App.DigitalIoManager as DigitalIoManager).IsReturnReelExist)
                                                {
                                                    if (currentReelTypeOfReturn != robotSequenceManager.ReelTypeOfReturn)
                                                    {
                                                        JudgeRobotOperationToLoadReel(finished, safeposition);
                                                    }
                                                    else
                                                    {
                                                        if (cycleStop)
                                                        {
                                                            if ((robotSequenceManager.IsAtSafePosition || safeposition) && finished)
                                                                robotStep = ReelHandlerSteps.Done;
                                                        }
                                                        else
                                                        {
                                                            robotTick = -1;
                                                            robotStep = ReelHandlerSteps.ApproachToReelHeightCheckPointAtReturnStage;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (currentReelTypeOfReturn == ReelDiameters.Unknown || !(App.DigitalIoManager as DigitalIoManager).IsReturnReelExist)
                                                    {
                                                        if (robotSequenceManager.ApproachToReturnStage(currentReelTypeOfReturn, previousReelTypeOfReturn))
                                                            robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStage;
                                                    }
                                                    else
                                                    {
                                                        JudgeRobotOperationToLoadReel(finished, safeposition);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel7OfReturnStage:
                            case RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel13OfReturnStage:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {   // Abnormal case
                                        throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                                    }
                                    else
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (robotSequenceManager.RobotActionOrder == RobotActionOrder.LoadReelFromReturn)
                                            {
                                                Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.PrepareToLoadReturn);
                                                robotStep = ReelHandlerSteps.MeasureReelHeightOnReturnStage;
                                            }
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.MeasureReelHeightAtReel7OfReturnStage:
                            case RobotSequenceCommands.MeasureReelHeightAtReel13OfReturnStage:
                                {
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        if (robotSequenceManager.RobotActionOrder == RobotActionOrder.LoadReelFromReturn)
                                        {
                                            switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                            {
                                                default:    // Abnormal case
                                                    break;
                                                case TransferMaterialObject.TransferModes.PrepareToLoadReturn:
                                                    {
                                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                                        {
                                                            case TransferMaterialObject.TransferStates.None:
                                                                {
                                                                    visionTick = -1;
                                                                    robotVisionRetryCount = 0;
                                                                    robotVisionRetryCycleCount = 0;

                                                                    if (Config.ImageProcessorType == ImageProcessorTypes.Mit)
                                                                        CompositeVisionManager.SetVisionLight(currentReelTypeOfReturn);

                                                                    robotStep = ReelHandlerSteps.CheckReelAlignmentOnReturnStage;
                                                                }
                                                                break;
                                                            case TransferMaterialObject.TransferStates.RequestToLoadConfirm:
                                                            case TransferMaterialObject.TransferStates.WaitForLoadConfirm:
                                                                {
                                                                    reelTowerQueryRetryCount = 0;
                                                                    robotStep = ReelHandlerSteps.RequestToReturnReelLoadConfirm;
                                                                }
                                                                break;
                                                            case TransferMaterialObject.TransferStates.ConfirmLoad:
                                                                {
                                                                    robotVisionRetryCount = 0;
                                                                    robotVisionRetryAttemptsCount = 0;
                                                                    robotVisionRetryCycleCount = 0;
                                                                    robotStep = ReelHandlerSteps.PrepareToReadBarcodeOnReel;
                                                                }
                                                                break;
                                                            case TransferMaterialObject.TransferStates.RequestToBarcodeConfirm:
                                                            case TransferMaterialObject.TransferStates.WaitForBarcodeConfirm:
                                                                {
                                                                    robotStep = ReelHandlerSteps.RequestToConfirmLoadReelBarcodeOfReturnStage;
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case TransferMaterialObject.TransferModes.LoadReturn:
                                                    {
                                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                                        {
                                                            default:    // Abnormal case
                                                                break;
                                                            case TransferMaterialObject.TransferStates.ConfirmedBarcodeOfReturn:
                                                                {
                                                                    robotStep = ReelHandlerSteps.AdjustPositionAndPickupReelOfReturnStage;
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                    {
                                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                                        {
                                                            default:    // Abnormal case
                                                            case TransferMaterialObject.TransferStates.RequestToBarcodeConfirm:
                                                            case TransferMaterialObject.TransferStates.WaitForBarcodeConfirm:
                                                            case TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart:
                                                                {
                                                                    robotStep = ReelHandlerSteps.AdjustPositionAndPickupReelOfReturnStage;
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel7OfReturnStage:
                            case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel13OfReturnStage:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {
                                        if (robotSequenceManager.RobotActionOrder == RobotActionOrder.LoadReelFromReturn)
                                        {
                                            switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                            {
                                                case TransferMaterialObject.TransferModes.Load:
                                                case TransferMaterialObject.TransferModes.LoadReturn:
                                                    {
                                                        if (Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.ConfirmedBarcodeOfReturn)
                                                        {
                                                            robotVisionRetryCount = 0;
                                                            robotVisionRetryAttemptsCount = 0;
                                                            reelTowerQueryRetryCount = 0;
                                                            robotStep = ReelHandlerSteps.MoveToFrontOfTower;
                                                        }
                                                    }
                                                    break;
                                                case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                    {
                                                        robotStep = ReelHandlerSteps.PrepareToRejectReel;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    else
                                    {   // Abnormal case
                                        throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                                    }
                                }
                                break;
                            case RobotSequenceCommands.MoveBackToFrontOfUnloadTower1:
                            case RobotSequenceCommands.MoveBackToFrontOfUnloadTower2:
                            case RobotSequenceCommands.MoveBackToFrontOfUnloadTower3:
                            case RobotSequenceCommands.MoveBackToFrontOfUnloadTower4:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {   // Abnormal case
                                        throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);

                                    }
                                    else
                                    {   // Abnormal state handling to recover. (Unload Reel Pick Failed)
                                        if ((robotSequenceManager.IsAtSafePosition || safeposition) && robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.Unload)
                                            {
                                                switch (Singleton<TransferMaterialObject>.Instance.State)
                                                {
                                                    case TransferMaterialObject.TransferStates.None:
                                                    case TransferMaterialObject.TransferStates.CompleteUnload:
                                                        {
                                                            reelTowerQueryRetryCount = 0;
                                                            Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.None);
                                                        }
                                                        break;
                                                }
                                            }

                                            JudgeRobotOperationToLoadReel(finished, safeposition);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            else
            {   // Abnormal case.
                robotStep = ReelHandlerSteps.Done;
            }
        }

        protected virtual void DefineRobotOperationToRejectLoadReel(RobotSequenceCommands command, bool finished, bool safeposition)
        {
            if (robotSequenceManager.RobotActionOrder == RobotActionOrder.CartReelToReject ||
                robotSequenceManager.RobotActionOrder == RobotActionOrder.ReturnReelToReject ||
                App.DigitalIoManager.IsSimulation)
            {
                if (currentLoadReelTower > 0 && currentLoadReelTower == currentLoadReelState.Index &&
                    currentLoadReelState.Index == currentRejectReelState.Index && currentRejectReelState.Index > 0 && currentRejectReelState.OutputStageIndex > 0)
                {   // Load reel from cart or return stage
                    if (robotOperationState != $"Robot operation check: {robotSequenceManager.HasAReel},{finished},{robotSequenceManager.IsAtSafePosition},{safeposition}")
                        Logger.Trace(robotOperationState = $"Robot operation check: {robotSequenceManager.HasAReel},{finished},{robotSequenceManager.IsAtSafePosition},{safeposition}");

                    if (!CheckUnloadRequestQueueAndIntercept(finished, safeposition))
                    {
                        if (!finished && !safeposition && !robotSequenceManager.HasAReel && initialized && command == RobotSequenceCommands.Unknown)
                            command = robotSequenceManager.LastExecutedCommand;

                        switch (command)
                        {   // Load cart reel
                            case RobotSequenceCommands.Unknown:  // Required initialize of robot
                                break;
                            case RobotSequenceCommands.PutReelIntoOutput1:
                            case RobotSequenceCommands.PutReelIntoOutput2:
                            case RobotSequenceCommands.PutReelIntoOutput3:
                            case RobotSequenceCommands.PutReelIntoOutput4:
                            case RobotSequenceCommands.PutReelIntoOutput5:
                            case RobotSequenceCommands.PutReelIntoOutput6:
                            case RobotSequenceCommands.PutReelIntoReject:
                            case RobotSequenceCommands.MoveToHome: // Home
                            case RobotSequenceCommands.GoToHomeBeforeReelHeightCheck: // Move to home
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {   // Abnormal case.
                                        // Already has a reel to load.
                                        // Abnormal case.
                                        // Need to check transfer reel information
                                        // We need warning to operator!!!
                                        throw new PauseException<ErrorCode>(ErrorCode.RobotReelDetectionSensorFailure);
                                    }
                                    else
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (cycleStop)
                                            {
                                                if ((robotSequenceManager.IsAtSafePosition || safeposition) && finished)
                                                    robotStep = ReelHandlerSteps.Done;
                                            }
                                            else
                                            {
                                                switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                                {
                                                    case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                    case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                    case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                                                    case TransferMaterialObject.TransferModes.Reject:
                                                    case TransferMaterialObject.TransferModes.None:
                                                        {
                                                            if ((robotSequenceManager.IsAtSafePosition || safeposition) && robotSequenceManager.IsWaitForOrder)
                                                            {   // UPDATED: 20200609 (Marcus)
                                                                if (robotSequenceManager.ReelGrip) // && !robotSequenceManager.ReelDetector)
                                                                    robotStep = ReelHandlerSteps.MoveToFrontOfRejectStage;
                                                                else
                                                                    robotStep = ReelHandlerSteps.CompletedToRejectReel;
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.GoToHomeAfterPickUpReel:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                        {
                                            case TransferMaterialObject.TransferModes.Load:
                                            case TransferMaterialObject.TransferModes.LoadReturn:
                                                {
                                                    if (Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart)
                                                        robotStep = ReelHandlerSteps.MoveToFrontOfTower;
                                                }
                                                break;
                                            case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                            case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                {
                                                    robotStep = ReelHandlerSteps.MoveToFrontOfRejectStage;
                                                }
                                                break;
                                        }
                                    }
                                    else
                                    {   // Abnormal case
                                        switch (robotSequenceManager.RobotActionOrder)
                                        {
                                            case RobotActionOrder.CartReelToReject:
                                                {
                                                    switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                                    {
                                                        case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                        case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                            {
                                                                if (robotSequenceManager.ReelGrip) // && !robotSequenceManager.ReelDetector)
                                                                    robotStep = ReelHandlerSteps.MoveToFrontOfRejectStage;
                                                                else
                                                                    robotStep = ReelHandlerSteps.CompletedToRejectReel;
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                            case RobotActionOrder.LoadReelFromCart:
                                                {
                                                    switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                                    {
                                                        case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:  // Have to check
                                                        case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                            {
                                                                Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                                robotStep = ReelHandlerSteps.Done;
                                                            }
                                                            break;
                                                        case TransferMaterialObject.TransferModes.None:
                                                        case TransferMaterialObject.TransferModes.PrepareToLoad:        // End reel
                                                        case TransferMaterialObject.TransferModes.Load:                 // After alarm reset (We need to reset the state and mode)                                                        
                                                            {
                                                                if (robotSequenceManager.IsWaitForOrder && (robotSequenceManager.IsAtSafePosition || safeposition))
                                                                {
                                                                    if (!robotSequenceManager.HasAReel)
                                                                    {
                                                                        switch (currentReelTypeOfCart)
                                                                        {
                                                                            case ReelDiameters.Unknown:
                                                                                {
                                                                                    robotStep = ReelHandlerSteps.Done;
                                                                                }
                                                                                break;
                                                                            case ReelDiameters.ReelDiameter7:
                                                                                {
                                                                                    if (currentWorkSlotOfCart <= 6)
                                                                                        robotStep = ReelHandlerSteps.PrepareToLoadReelFromCart;
                                                                                    else
                                                                                    {   // Wait for a new cart or other event to load or unload
                                                                                        currentWorkSlotOfCart = 1;
                                                                                        robotStep = ReelHandlerSteps.Done;
                                                                                    }
                                                                                }
                                                                                break;
                                                                            case ReelDiameters.ReelDiameter13:
                                                                                {
                                                                                    if (currentWorkSlotOfCart <= 4)
                                                                                        robotStep = ReelHandlerSteps.PrepareToLoadReelFromCart;
                                                                                    else
                                                                                    {   // Wait for a new cart or other event to load or unload
                                                                                        currentWorkSlotOfCart = 1;
                                                                                        robotStep = ReelHandlerSteps.Done;
                                                                                    }
                                                                                }
                                                                                break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                            case RobotActionOrder.LoadReelFromReturn:
                                                {
                                                    switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                                    {
                                                        case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:  // Have to check
                                                        case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                            {
                                                                Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                                robotStep = ReelHandlerSteps.Done;
                                                            }
                                                            break;
                                                        default:
                                                            JudgeRobotOperationToLoadReel(finished, safeposition);
                                                            break;
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel7OfReturnStage:
                            case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel13OfReturnStage:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                        {
                                            case TransferMaterialObject.TransferModes.Load:
                                            case TransferMaterialObject.TransferModes.LoadReturn:
                                                {
                                                    if (Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.ConfirmedBarcodeOfReturn)
                                                    {
                                                        robotVisionRetryCount = 0;
                                                        robotVisionRetryAttemptsCount = 0;
                                                        reelTowerQueryRetryCount = 0;
                                                        robotStep = ReelHandlerSteps.MoveToFrontOfTower;
                                                    }
                                                }
                                                break;
                                            case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                            case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                {
                                                    robotStep = ReelHandlerSteps.MoveToFrontOfRejectStage;
                                                }
                                                break;
                                        }
                                    }
                                    else
                                    {   // Abnormal case
                                        throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                                    }
                                }
                                break;
                            case RobotSequenceCommands.MoveToReel7OfReturnStage:
                            case RobotSequenceCommands.MoveToReel13OfReturnStage:
                            case RobotSequenceCommands.MoveBackToFrontOfReturnStage:
                                {
                                    switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                    {
                                        case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                        case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                            {
                                                robotStep = ReelHandlerSteps.MoveToFrontOfRejectStage;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case RobotSequenceCommands.MoveToFrontOfRejectStage:
                                {
                                    // UPDATED: 20200609 (Marcus)
                                    // It will process all abnormal cases, even if a reel is not on gripper.
                                    if (robotSequenceManager.IsWaitForOrder)
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                        {
                                            case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                            case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                            case TransferMaterialObject.TransferModes.Reject:
                                                {
                                                    // UPDATED: 20200609 (Marcus)
                                                    // if (robotSequenceManager.ReelGrip)
                                                    robotStep = ReelHandlerSteps.CheckUpStateOfRejectStage;
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case RobotSequenceCommands.ApproachToFrontOfRejectStage:
                                {
                                    if (robotSequenceManager.IsWaitForOrder)
                                        robotStep = ReelHandlerSteps.PutReelIntoRejectStage;
                                }
                                break;
                            case RobotSequenceCommands.MoveBackToFrontOfUnloadTower1:
                            case RobotSequenceCommands.MoveBackToFrontOfUnloadTower2:
                            case RobotSequenceCommands.MoveBackToFrontOfUnloadTower3:
                            case RobotSequenceCommands.MoveBackToFrontOfUnloadTower4:
                                {
                                    if (robotSequenceManager.HasAReel)
                                    {   // Abnormal case
                                        throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                                    }
                                    else
                                    {   // Abnormal state handling to recover. (Unload Reel Pick Failed)
                                        if ((robotSequenceManager.IsAtSafePosition || safeposition) && robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.Unload)
                                            {
                                                switch (Singleton<TransferMaterialObject>.Instance.State)
                                                {
                                                    case TransferMaterialObject.TransferStates.None:
                                                    case TransferMaterialObject.TransferStates.CompleteUnload:
                                                        {
                                                            reelTowerQueryRetryCount = 0;
                                                            Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.None);
                                                        }
                                                        break;
                                                }
                                            }

                                            JudgeRobotOperationToLoadReel(finished, safeposition);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            else
            {   // Abnormal case.
                robotStep = ReelHandlerSteps.Done;
            }
        }

        protected virtual void DefineRobotOperationToRejectUnloadReel(RobotSequenceCommands command, bool finished, bool safeposition)
        {
            if (currentUnloadReelTower > 0 && currentUnloadReelTower == currentUnloadReelState.Index)
            {   // Proceed to reject a reel
                switch (command)
                {
                    case RobotSequenceCommands.Unknown:  // Required initialize of robot
                        break;
                    default:
                    case RobotSequenceCommands.MoveToHome: // Home
                        {   // Need to confirm currentLoadReelTower state
                            if (robotSequenceManager.HasAReel)
                            {   // Already has a reel to load.
                                // Abnormal case.
                                // Need to check transfer reel information
                                // We need warning to operator!!!
                                throw new PauseException<ErrorCode>(ErrorCode.RobotReelDetectionSensorFailure);
                            }
                            else
                            {
                                if (cycleStop)
                                {
                                    if ((robotSequenceManager.IsAtSafePosition || safeposition) && finished)
                                        robotStep = ReelHandlerSteps.Done;
                                }
                                else
                                {
                                    switch (robotSequenceManager.RobotActionOrder)
                                    {
                                        case RobotActionOrder.UnloadReelFromTower:
                                            {
                                                if (robotSequenceManager.IsAtSafePosition || safeposition)
                                                {
                                                    ForceSetTransferMode();
                                                    reelTowerQueryRetryCount = 0;
                                                    robotStep = ReelHandlerSteps.PrepareToUnloadTowerReel;
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    case RobotSequenceCommands.TakeReelFromTower1:
                    case RobotSequenceCommands.TakeReelFromTower2:
                    case RobotSequenceCommands.TakeReelFromTower3:
                    case RobotSequenceCommands.TakeReelFromTower4:
                        {   // UPDATED: 20200408 (Marcus)
                            // Switch robot operation to reject a reel by transfer mode of material.
                            if (robotSequenceManager.HasAReel)
                            {
                                if (robotSequenceManager.IsWaitForOrder)
                                {
                                    switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                    {
                                        default:
                                            {   // Abnormal case (Mismatched state)
                                                robotStep = ReelHandlerSteps.MoveBackToFrontOfTowerByReelPickupFailureToReject;
                                            }
                                            break;
                                        case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                                        case TransferMaterialObject.TransferModes.Reject:
                                            {
                                                if (Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.TakenMaterial)
                                                    robotStep = ReelHandlerSteps.MoveToFrontOfRejectStage;
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                            {   // Abnormal case. (Unload Reel Pick Failed)
                                switch (Singleton<TransferMaterialObject>.Instance.State)
                                {
                                    case TransferMaterialObject.TransferStates.VerifiedUnload:
                                    case TransferMaterialObject.TransferStates.TakenMaterial:
                                        {
                                            reelTowerQueryRetryCount = 0;

                                            switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                            {
                                                default:    // Abnormal case but you donn't need to do something.
                                                case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                                                case TransferMaterialObject.TransferModes.Reject:
                                                    robotStep = ReelHandlerSteps.MoveBackToFrontOfTowerByReelPickupFailureToReject;
                                                    break;
                                            }
                                        }
                                        break;
                                    default:
                                    case TransferMaterialObject.TransferStates.None:
                                    case TransferMaterialObject.TransferStates.CompleteUnload:
                                        {   // Abnormal case (Mismatched state)
                                            robotStep = ReelHandlerSteps.MoveBackToFrontOfTowerByReelPickupFailureToReject;
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case RobotSequenceCommands.MoveToFrontOfRejectStage:
                        {   // UPDATED: 20200609 (Marcus)
                            // It will process all abnormal cases, even if a reel is not on gripper.
                            if (robotSequenceManager.IsWaitForOrder)
                            {
                                switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                {
                                    case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                    case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                    case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                                    case TransferMaterialObject.TransferModes.Reject:
                                        {
                                            if (robotSequenceManager.ReelGrip) // && !robotSequenceManager.ReelDetector)
                                                robotStep = ReelHandlerSteps.CheckUpStateOfRejectStage;
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case RobotSequenceCommands.ApproachToFrontOfRejectStage:
                        {
                            if (robotSequenceManager.IsWaitForOrder)
                                robotStep = ReelHandlerSteps.PutReelIntoRejectStage;
                        }
                        break;
                    case RobotSequenceCommands.PutReelIntoReject:
                        {
                            if (robotSequenceManager.HasAReel)
                            {   // Abnormal case.
                                throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                            }
                            else
                            {
                                if (robotSequenceManager.IsWaitForOrder)
                                {
                                    switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                    {
                                        case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                        case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                        case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                                        case TransferMaterialObject.TransferModes.Reject:
                                        case TransferMaterialObject.TransferModes.None:
                                            {
                                                if ((robotSequenceManager.IsAtSafePosition || safeposition) && robotSequenceManager.IsWaitForOrder)
                                                    robotStep = ReelHandlerSteps.CompletedToRejectReel;
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot5OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot6OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel13Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel13Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel13Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel13Cart:
                        {   // Need confirm to avoid abnormal case.
                            if (robotSequenceManager.HasAReel)
                            {   // Abnormal case
                                throw new PauseException<String>(Properties.Resources.String_Notification_Gripper_State);
                            }
                            else
                            {
                                robotVisionRetryCount = 0;
                                robotVisionRetryAttemptsCount = 0;
                                reelTowerQueryRetryCount = 0;
                                robotStep = ReelHandlerSteps.GoToHomeBeforeReelHeightCheck;
                            }
                        }
                        break;
                }
            }
            else
            {   // Abnormal case.
                robotStep = ReelHandlerSteps.Done;
            }
        }
        #endregion

        protected void FunctionRobot()
        {
            string log_ = "Changed robot step.";
            string res_ = string.Empty;
            bool result_ = false;
            bool cmddone_ = false;
            bool finished_ = false;
            bool safeposition_ = false;

            if (robotStep != previousRobotStep)
            {
                stayCountRobot_ = 0;
                Logger.ProcessTrace(MethodBase.GetCurrentMethod().Name, $"{robotStep}", $"From={previousRobotStep},To={robotStep}:{log_}");
                previousRobotStep = robotStep;
                robotSubStep = SubSequenceSteps.Prepare;
            }

            if (robotController.IsEmergencyStop)
                throw new PauseException<ErrorCode>(ErrorCode.RobotEmergencyStop);

            if (robotController.IsProtectiveStop)
                throw new PauseException<ErrorCode>(ErrorCode.RobotProtectiveStop);

            // Check robot sequence and state
            RobotSequenceCommands command_ = robotSequenceManager.GetRobotExecutedCommandByScenario(ref cmddone_);
            finished_ = robotSequenceManager.IsRobotAtWayPointByCommand(command_, ref safeposition_);

            if (robotController.IsRunnable)
            {
                switch (robotStep)
                {
                    #region Catch robot operation order
                    case ReelHandlerSteps.None:
                    case ReelHandlerSteps.Ready:
                        {
                            switch (robotSequenceManager.RobotActionOrder)
                            {
                                #region Wait for a robot task order
                                case RobotActionOrder.None:
                                    {   // Wait for a new operation order assignment from FunctionReelTower.
                                        if (cycleStop)
                                        {
                                            if ((robotSequenceManager.IsAtSafePosition || safeposition_) && finished_)
                                                robotStep = ReelHandlerSteps.Done;
                                        }
                                    }
                                    break;
                                #endregion
                                #region Define reel load sequence order
                                case RobotActionOrder.LoadReelFromReturn:
                                case RobotActionOrder.LoadReelFromCart:
                                    DefineRobotOperationToLoadReel(command_, finished_, safeposition_);
                                    break;
                                #endregion
                                #region Define reel unload sequence order
                                case RobotActionOrder.UnloadReelFromTower:
                                    DefineRobotOperationToUnloadReel(command_, finished_, safeposition_);
                                    break;
                                #endregion
                                #region Reject reel sequence order
                                case RobotActionOrder.CartReelToReject:
                                case RobotActionOrder.ReturnReelToReject:
                                    DefineRobotOperationToRejectLoadReel(command_, finished_, safeposition_);
                                    break;
                                case RobotActionOrder.UnloadReelToReject:
                                    DefineRobotOperationToRejectUnloadReel(command_, finished_, safeposition_);
                                    break;
                                    #endregion
                            }

                            if (previousRobotActionOrder != robotSequenceManager.RobotActionOrder)
                            {
                                switch (previousRobotActionOrder = robotSequenceManager.RobotActionOrder)
                                {
                                    case RobotActionOrder.None:
                                        log_ = "Run;Completed;";
                                        break;
                                    case RobotActionOrder.LoadReelFromReturn:
                                    case RobotActionOrder.LoadReelFromCart:
                                        log_ = "Run;Loading;";
                                        break;
                                    case RobotActionOrder.UnloadReelFromTower:
                                        log_ = "Run;Unloading;";
                                        break;
                                    case RobotActionOrder.CartReelToReject:
                                    case RobotActionOrder.ReturnReelToReject:
                                    case RobotActionOrder.UnloadReelToReject:
                                        log_ = "Run;Reject;";
                                        break;
                                }

                                ReportEvent(log_);
                            }
                        }
                        break;
                    #endregion

                    #region Load a reel from cart sequence
                    #region Check reel size of cart
                    case ReelHandlerSteps.CheckCartReelType:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        finishedLoadReelFromCart = false;

                                        if (robotSequenceManager.HasAReel)
                                        {
                                            throw new PauseException<ErrorCode>(ErrorCode.DetectedAReelOnGripper);
                                        }
                                        else
                                        {
                                            if (robotSequenceManager.IsWaitForOrder)
                                            {
                                                if (robotSequenceManager.IsAtSafePosition)
                                                {   // Check home position
                                                    if (robotSequenceManager.IsRobotAtHome)
                                                    {
                                                        robotTick = 0;
                                                    }
                                                    else
                                                    {
                                                        if (robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeBeforeReelHeightCheck))
                                                            robotTick = App.TickCount;
                                                    }
                                                }
                                                else
                                                {
                                                    if (!robotSequenceManager.IsRobotAtWayPointByCommand(ref safeposition_) &&
                                                        robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.MoveToHome &&
                                                        robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.GoToHomeBeforeReelHeightCheck &&
                                                        robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.CheckReelTypeOfCart)
                                                        throw new PauseException<ErrorCode>(ErrorCode.RobotStateIsNotMatched);
                                                }

                                                if (IsRobotActionDelayOver(Convert.ToInt32(timeoutOfRobotMoving * 0.1)))
                                                {
                                                    if (IsRobotAvailableToAct(safeposition_))
                                                    {
                                                        currentReelTypeOfCart = ReelDiameters.Unknown;

                                                        switch (Config.ImageProcessorType)
                                                        {
                                                            case ImageProcessorTypes.Mit:
                                                                {
                                                                    VisionManager.LightOnForReelSizeConfirm();
                                                                }
                                                                break;
                                                        }

                                                        if (robotSequenceManager.SendCommand(RobotSequenceCommands.CheckReelTypeOfCart))
                                                        {
                                                            visionTick = App.TickCount;
                                                            Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                            robotSubStep = SubSequenceSteps.Proceed;
                                                        }
                                                    }
                                                    else if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) &&
                                                        robotSequenceManager.LastExecutedCommand == RobotSequenceCommands.CheckReelTypeOfCart)
                                                    {
                                                        visionTick = App.TickCount;
                                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                }
                                            }
                                            else if (IsRobotActionDelayOver(Convert.ToInt32(timeoutOfRobotMoving * 0.1)))
                                            {
                                                if (!robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) &&
                                                        robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.MoveToHome &&
                                                        robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.GoToHomeBeforeReelHeightCheck)
                                                    throw new PauseException<ErrorCode>(ErrorCode.RobotStateIsNotMatched);
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        switch (Config.ImageProcessorType)
                                        {
                                            case ImageProcessorTypes.Mit:
                                                {
                                                    VisionManager.LightOnForReelSizeConfirm();

                                                    if (IsVisionProcessDelayOver(delayOfImageProcessing))
                                                    {
                                                        if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) || !IsRobotVisionRetryOver)
                                                        {
                                                            imageProcessingResult = ImageProcssingResults.Unknown;
                                                            visionTick = App.TickCount;
                                                            VisionManager.TriggerForReelSizeConfirm();
                                                            robotSubStep = SubSequenceSteps.Post;
                                                        }
                                                        else if (robotSequenceManager.IsFailure)
                                                        {
                                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                                        }
                                                    }
                                                }
                                                break;
                                            case ImageProcessorTypes.TechFloor:
                                                {
                                                    if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                                    {
                                                        if (IsOverDelayTime(Model.TimeoutOfRetryTrigger, imageProcessTimeoutTick))
                                                        {
                                                            if (FireTriggerVisionControl(Model.LightLevels.Top,
                                                                Model.TaskSubCategories.CheckCartType,
                                                                currentReelTypeOfCart,
                                                                currentWorkSlotOfCart,
                                                                false,
                                                                Model.DelayOfTrigger))
                                                            {
                                                                robotSubStep = SubSequenceSteps.Post;
                                                            }
                                                            else
                                                            {
                                                                robotVisionRetryCount++;
                                                            }
                                                        }
                                                    }
                                                    else if (robotSequenceManager.IsFailure)
                                                    {
                                                        throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        switch (Config.ImageProcessorType)
                                        {
                                            case ImageProcessorTypes.Mit:
                                                {
                                                    if (IsVisionProcessDelayOver(delayOfImageProcessing))
                                                    {
                                                        switch (imageProcessingResult = CompositeVisionManager.GetReelSize())
                                                        {
                                                            case ImageProcssingResults.Success:
                                                                {
                                                                    switch (VisionManager.VisionReelSize)
                                                                    {
                                                                        case "I0":
                                                                        case "I1":
                                                                            currentReelTypeOfCart = ReelDiameters.ReelDiameter13;
                                                                            break;
                                                                        case "I2":
                                                                        case "I3":
                                                                            currentReelTypeOfCart = ReelDiameters.ReelDiameter7;
                                                                            break;
                                                                        default:
                                                                            result_ = true;
                                                                            break;
                                                                    }

                                                                    if (robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeBeforeReelHeightCheck))
                                                                    {
                                                                        if (currentReelTypeOfCart != ReelDiameters.Unknown)
                                                                        {
                                                                            robotTick = App.TickCount;
                                                                            robotActionRetryCount = 0;
                                                                            robotStep = ReelHandlerSteps.SetReelTypeOfCartToRobot;
                                                                        }
                                                                    }
                                                                }
                                                                break;
                                                            case ImageProcssingResults.Exception:
                                                            case ImageProcssingResults.Empty:
                                                            case ImageProcssingResults.OverScope:
                                                                result_ = true;
                                                                break;
                                                        }

                                                        if (imageProcessingResult > ImageProcssingResults.Unknown)
                                                            VisionManager._CILightOff();

                                                        if (result_)
                                                        {   // Abnormal case
                                                            if (IsRobotVisionRetryOver)
                                                            {   // Raise alarm.
                                                                currentReelTypeOfCart = ReelDiameters.Unknown;
                                                                robotSequenceManager.SendCommand(RobotSequenceCommands.MoveToHome);

                                                                robotStep = ReelHandlerSteps.Done;
                                                                throw new PauseException<ErrorCode>(ErrorCode.FailedToCheckReelTypeOfCart);
                                                            }
                                                            else
                                                            {   // Retry
                                                                robotVisionRetryCount++;
                                                                visionTick = App.TickCount;
                                                                robotSubStep = SubSequenceSteps.Prepare;
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            case ImageProcessorTypes.TechFloor:
                                                {
                                                    if (visionProcessResultState > 0)
                                                    {
                                                        ResetVisionProcessResultState();
                                                        object data = new object();

                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.UserData_0, ref data))
                                                        {
                                                            switch (data)
                                                            {
                                                                case 7:
                                                                case 13:
                                                                    {
                                                                        currentReelTypeOfCart = (ReelDiameters)data;

                                                                        if (robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeBeforeReelHeightCheck))
                                                                        {
                                                                            result_ = true;

                                                                            if (currentReelTypeOfCart != ReelDiameters.Unknown)
                                                                            {
                                                                                robotTick = App.TickCount;
                                                                                robotActionRetryCount = 0;
                                                                                robotStep = ReelHandlerSteps.SetReelTypeOfCartToRobot;
                                                                            }
                                                                        }
                                                                    }
                                                                    break;
                                                            }
                                                        }

                                                        if (!result_)
                                                        {   // Abnormal case
                                                            CheckImageProcessRetry(ReelHandlerSteps.CheckCartReelType, SubSequenceSteps.Prepare, ReelHandlerSteps.Done, ErrorCode.FailedToCheckReelTypeOfCart);
                                                        }
                                                    }
                                                    else if (IsOverDelayTime(Model.TimeoutOfTotalImageProcess, imageProcessTimeoutTick))
                                                    {
                                                        CheckImageProcessRetry(ReelHandlerSteps.CheckCartReelType, SubSequenceSteps.Prepare, ReelHandlerSteps.Done, ErrorCode.FailedToCheckReelTypeOfCart);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Set reel type of cart to robot
                    case ReelHandlerSteps.SetReelTypeOfCartToRobot:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                            {
                                                if (IsRobotActionRetryOver)
                                                {
                                                    robotTick = App.TickCount;
                                                    robotActionRetryCount = 0;
                                                    throw new PauseException<ErrorCode>(ErrorCode.FailedToSetReelSizeOfCartToRobot);
                                                }
                                                else
                                                {
                                                    if (SetReelTypeOfCart(currentReelTypeOfCart))
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    else
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) || cmddone_)
                                        {
                                            FireChagedReelSizeOfCart(currentReelTypeOfCart);
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        foreach (KeyValuePair<int, List<Coord3DField<double, double, double, double, double, double>>> item in loopCountOfCartGuidePoints)
                                            item.Value.Clear();

                                        visionMarkAdjustmentStates = 0;
                                        verifiedCartGuidePoints = false;
                                        robotStep = ReelHandlerSteps.Ready;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Check cart guide points
                    // UPDATED: 20200512 (Marcus)
                    case ReelHandlerSteps.CheckCartGuidePoint1:
                    case ReelHandlerSteps.CheckCartGuidePoint2:
                    case ReelHandlerSteps.CheckCartGuidePoint3:
                    case ReelHandlerSteps.CheckCartGuidePoint4:
                        {
                            int pointIndex_ = 0;
                            string arg_ = string.Empty;
                            RobotSequenceCommands cmd_ = RobotSequenceCommands.CheckCartGuidePoint1;

                            switch (pointIndex_ = (robotStep - ReelHandlerSteps.CheckCartGuidePoint1)/(ReelHandlerSteps.CheckCartGuidePoint2 - ReelHandlerSteps.CheckCartGuidePoint1) + 1)
                            {
                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                    cmd_ = (RobotSequenceCommands.CheckCartGuidePoint1 + (pointIndex_ - 1) * (ReelHandlerSteps.CheckCartGuidePoint2 - ReelHandlerSteps.CheckCartGuidePoint1));
                                    break;
                            }

                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        finishedLoadReelFromCart = false;

                                        if (robotSequenceManager.HasAReel)
                                        {
                                            throw new PauseException<ErrorCode>(ErrorCode.DetectedAReelOnGripper);
                                        }
                                        else if (currentReelTypeOfCart == ReelDiameters.Unknown)
                                        {
                                            throw new PauseException<ErrorCode>(ErrorCode.FailedToCheckReelTypeOfCart);
                                        }
                                        else if (!(App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                                        {
                                            throw new PauseException<ErrorCode>(ErrorCode.CartPresentSensorFailure);
                                        }
                                        else
                                        {
                                            if (robotSequenceManager.IsWaitForOrder)
                                            {
                                                if (robotSequenceManager.IsAtSafePosition)
                                                {   // Check home position
                                                    if (robotSequenceManager.IsRobotAtHome)
                                                    {
                                                        robotTick = 0;
                                                    }
                                                    else
                                                    {
                                                        if (robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeBeforeReelHeightCheck))
                                                            robotTick = App.TickCount;
                                                    }
                                                }
                                                else
                                                {
                                                    if (!robotSequenceManager.IsRobotAtWayPointByCommand(ref safeposition_) &&
                                                        robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.MoveToHome &&
                                                        robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.GoToHomeBeforeReelHeightCheck &&
                                                        robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.CheckCartGuidePoint1 &&
                                                        robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.CheckCartGuidePoint2 &&
                                                        robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.CheckCartGuidePoint3 &&
                                                        robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.CheckCartGuidePoint4)
                                                        throw new PauseException<ErrorCode>(ErrorCode.RobotStateIsNotMatched);
                                                }

                                                if (IsRobotActionDelayOver(Convert.ToInt32(timeoutOfRobotMoving * 0.1)))
                                                {
                                                    if (IsRobotAvailableToAct(safeposition_) ||
                                                        robotSequenceManager.LastExecutedCommand == RobotSequenceCommands.SetCartGuideWorkSlotCenter1 ||
                                                        robotSequenceManager.LastExecutedCommand == RobotSequenceCommands.SetCartGuideWorkSlotCenter2 ||
                                                        robotSequenceManager.LastExecutedCommand == RobotSequenceCommands.SetCartGuideWorkSlotCenter3 ||
                                                        robotSequenceManager.LastExecutedCommand == RobotSequenceCommands.SetCartGuideWorkSlotCenter4 ||
                                                        robotSequenceManager.LastExecutedCommand == RobotSequenceCommands.SetCartGuideWorkSlotCenter5 ||
                                                        robotSequenceManager.LastExecutedCommand == RobotSequenceCommands.SetCartGuideWorkSlotCenter6)
                                                    {
                                                        switch (Config.ImageProcessorType)
                                                        {
                                                            case ImageProcessorTypes.Mit:
                                                                {
                                                                    VisionManager.LightOnForReelSizeConfirm();
                                                                }
                                                                break;
                                                        }

                                                        switch (currentReelTypeOfCart)
                                                        {
                                                            case ReelDiameters.ReelDiameter7:
                                                                arg_ = "7";
                                                                break;
                                                            case ReelDiameters.ReelDiameter13:
                                                                arg_ = "13";
                                                                break;
                                                        }

                                                        if ((currentReelTypeOfCart == ReelDiameters.ReelDiameter7 || currentReelTypeOfCart == ReelDiameters.ReelDiameter13) &&
                                                            !string.IsNullOrEmpty(arg_))
                                                        {
                                                            if ((loopCountOfCartGuidePoints.Count > 0 & (loopCountOfCartGuidePoints.ContainsKey(pointIndex_) && loopCountOfCartGuidePoints[pointIndex_].Count > 0)) ||
                                                                robotSequenceManager.SendCommand(cmd_, arg_))
                                                            {
                                                                robotTick = App.TickCount;
                                                                visionTick = App.TickCount;
                                                                Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                                robotSubStep = SubSequenceSteps.Proceed;
                                                            }
                                                        }
                                                    }
                                                    else if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) &&
                                                        (robotSequenceManager.LastExecutedCommand == RobotSequenceCommands.CheckCartGuidePoint1 ||
                                                        robotSequenceManager.LastExecutedCommand == RobotSequenceCommands.CheckCartGuidePoint2 ||
                                                        robotSequenceManager.LastExecutedCommand == RobotSequenceCommands.CheckCartGuidePoint3 ||
                                                        robotSequenceManager.LastExecutedCommand == RobotSequenceCommands.CheckCartGuidePoint4))
                                                    {
                                                        visionTick = App.TickCount;
                                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                }
                                            }
                                            else if (IsRobotActionDelayOver(Convert.ToInt32(timeoutOfRobotMoving * 0.1)))
                                            {
                                                if (!robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) &&
                                                    robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.MoveToHome &&
                                                    robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.GoToHomeBeforeReelHeightCheck)
                                                    throw new PauseException<ErrorCode>(ErrorCode.RobotStateIsNotMatched);
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        switch (Config.ImageProcessorType)
                                        {
                                            case ImageProcessorTypes.Mit:
                                                {
                                                    VisionManager.LightOnForReelSizeConfirm();

                                                    if (IsVisionProcessDelayOver(delayOfImageProcessing))
                                                    {
                                                        if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) || !IsRobotVisionRetryOver)
                                                        {
                                                            imageProcessingResult = ImageProcssingResults.Unknown;
                                                            visionTick = App.TickCount;
                                                            VisionManager.TriggerForReelSizeConfirm();
                                                            robotSubStep = SubSequenceSteps.Post;
                                                        }
                                                        else if (robotSequenceManager.IsFailure)
                                                        {
                                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                                        }
                                                    }
                                                }
                                                break;
                                            case ImageProcessorTypes.TechFloor:
                                                {
                                                    if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) && robotSequenceManager.IsWaitForOrder)
                                                    {
                                                        if (IsOverDelayTime(Model.TimeoutOfRetryTrigger, imageProcessTimeoutTick))
                                                        {
                                                            if (FireTriggerVisionControl(Model.LightLevels.Top,
                                                            Model.TaskSubCategories.CheckCartGuidePoint,
                                                            currentReelTypeOfCart,
                                                            pointIndex_,
                                                            true,
                                                            Model.DelayOfTrigger))
                                                            {
                                                                robotSubStep = SubSequenceSteps.Post;
                                                            }
                                                            else
                                                            {
                                                                robotVisionRetryCount++;
                                                            }
                                                        }
                                                    }
                                                    else if (robotSequenceManager.IsFailure)
                                                    {
                                                        throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        switch (Config.ImageProcessorType)
                                        {
                                            case ImageProcessorTypes.Mit:
                                            case ImageProcessorTypes.TechFloor:
                                                {
                                                    if (visionProcessResultState > 0)
                                                    {
                                                        ResetVisionProcessResultState();
                                                        object data = new object();
                                                        bool verified_ = false;
                                                        double offsetx_ = 0, offsety_ = 0, offsett_ = 0;

                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.ResultCode, ref data))
                                                        {
                                                            switch ((Cognex.VisionPro.CogToolResultConstants)data)
                                                            {
                                                                case Cognex.VisionPro.CogToolResultConstants.Accept:
                                                                    {   // UPDATED: 20200512 (Marcus)
                                                                        if (currentReelTypeOfCart != ReelDiameters.Unknown)
                                                                        {
                                                                            switch (pointIndex_)
                                                                            {
                                                                                case 1:
                                                                                    {
                                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundCartGuidePointX1, ref data))
                                                                                            offsetx_ = Convert.ToDouble(data);

                                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundCartGuidePointY1, ref data))
                                                                                            offsety_ = Convert.ToDouble(data);

                                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundCartGuidePointAngle1, ref data))
                                                                                            offsett_ = Convert.ToDouble(data);
                                                                                    }
                                                                                    break;
                                                                                case 2:
                                                                                    {
                                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundCartGuidePointX2, ref data))
                                                                                            offsetx_ = Convert.ToDouble(data);

                                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundCartGuidePointY2, ref data))
                                                                                            offsety_ = Convert.ToDouble(data);

                                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundCartGuidePointAngle2, ref data))
                                                                                            offsett_ = Convert.ToDouble(data);

                                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.VerifiedGuidePoint, ref data))
                                                                                            verified_ = Convert.ToBoolean(data);
                                                                                    }
                                                                                    break;
                                                                                case 3:
                                                                                    {
                                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundCartGuidePointX3, ref data))
                                                                                            offsetx_ = Convert.ToDouble(data);

                                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundCartGuidePointY3, ref data))
                                                                                            offsety_ = Convert.ToDouble(data);

                                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundCartGuidePointAngle3, ref data))
                                                                                            offsett_ = Convert.ToDouble(data);

                                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.VerifiedGuidePoint, ref data))
                                                                                            verified_ = Convert.ToBoolean(data);
                                                                                    }
                                                                                    break;
                                                                                case 4:
                                                                                    {
                                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundCartGuidePointX4, ref data))
                                                                                            offsetx_ = Convert.ToDouble(data);

                                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundCartGuidePointY4, ref data))
                                                                                            offsety_ = Convert.ToDouble(data);

                                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundCartGuidePointAngle4, ref data))
                                                                                            offsett_ = Convert.ToDouble(data);

                                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.VerifiedGuidePoint, ref data))
                                                                                            verified_ = Convert.ToBoolean(data);
                                                                                    }
                                                                                    break;
                                                                            }

                                                                            verifiedCartGuidePoints = verified_;

                                                                            if (!loopCountOfCartGuidePoints.ContainsKey(pointIndex_))
                                                                                loopCountOfCartGuidePoints.Add(pointIndex_, new List<Coord3DField<double, double, double, double, double, double>>());

                                                                            loopCountOfCartGuidePoints[pointIndex_].Add(new Coord3DField<double, double, double, double, double, double>(offsetx_, offsety_, 0, 0, 0, offsett_));

                                                                            result_ = true;
                                                                            robotTick = App.TickCount;
                                                                            robotActionRetryCount = 0;
                                                                            robotVisionRetryCount = 0;

                                                                            if (loopCountOfCartGuidePoints[pointIndex_].Count < Model.Process.CalibrationSpecs[currentReelTypeOfCart == ReelDiameters.ReelDiameter7? Model.CalibrationSpecifications.Cart7GuidePoint : Model.CalibrationSpecifications.Cart13GuidePoint].first)
                                                                            {
                                                                                robotStep += 1;
                                                                            }
                                                                            else
                                                                            {
                                                                                switch (robotStep)
                                                                                {
                                                                                    case ReelHandlerSteps.CheckCartGuidePoint1:
                                                                                        {
                                                                                            robotStep = ReelHandlerSteps.ApplyCartGuidePoint1;
                                                                                        }
                                                                                        break;
                                                                                    case ReelHandlerSteps.CheckCartGuidePoint2:
                                                                                        {
                                                                                            robotStep = ReelHandlerSteps.ApplyCartGuidePoint2;
                                                                                        }
                                                                                        break;
                                                                                    case ReelHandlerSteps.CheckCartGuidePoint3:
                                                                                        {
                                                                                            if (currentReelTypeOfCart == ReelDiameters.ReelDiameter7)
                                                                                                robotStep = ReelHandlerSteps.ApplyCartGuidePoint3;
                                                                                        }
                                                                                        break;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    break;
                                                            }
                                                        }

                                                        if (!result_)
                                                        {   // Abnormal case
                                                            CheckImageProcessRetry(robotStep, SubSequenceSteps.Prepare, ReelHandlerSteps.Done, ErrorCode.FailedToCheckGuidePointsOfCart);
                                                        }
                                                    }
                                                    else if (IsOverDelayTime(Model.TimeoutOfTotalImageProcess, imageProcessTimeoutTick))
                                                    {
                                                        CheckImageProcessRetry(robotStep, SubSequenceSteps.Prepare, ReelHandlerSteps.Done, ErrorCode.FailedToCheckGuidePointsOfCart);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.AdjustCartGuidePoint1:
                    case ReelHandlerSteps.AdjustCartGuidePoint2:
                    case ReelHandlerSteps.AdjustCartGuidePoint3:
                    case ReelHandlerSteps.AdjustCartGuidePoint4:
                        {
                            int pointIndex_ = 0;
                            RobotSequenceCommands cmd_ = RobotSequenceCommands.AdjustCartGuidePoint1;
                            
                            switch (pointIndex_ = (robotStep - ReelHandlerSteps.AdjustCartGuidePoint1) / (ReelHandlerSteps.AdjustCartGuidePoint2 - ReelHandlerSteps.AdjustCartGuidePoint1) + 1)
                            {
                                case 1:
                                    cmd_ = RobotSequenceCommands.AdjustCartGuidePoint1;
                                    break;
                                case 2:
                                    cmd_ = RobotSequenceCommands.AdjustCartGuidePoint2;
                                    break;
                                case 3:
                                    cmd_ = RobotSequenceCommands.AdjustCartGuidePoint3;
                                    break;
                                case 4:
                                    cmd_ = RobotSequenceCommands.AdjustCartGuidePoint4;
                                    break;
                            }

                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (cycleStop)
                                        {
                                            if (robotSequenceManager.IsWaitForOrder)
                                            {
                                                if (robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeBeforeReelHeightCheck))
                                                    robotTick = App.TickCount;
                            
                                                robotSubStep = SubSequenceSteps.Proceed;
                                            }
                                        }
                                        else
                                        {
                                            if (robotSequenceManager.IsWaitForOrder)
                                            {
                                                if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                                {
                                                    if (IsRobotActionRetryOver)
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount = 0;
                                                        throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                                    }
                                                    else
                                                    {
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (cycleStop)
                                        {
                                            if (robotSequenceManager.IsWaitForOrder)
                                            {
                                                if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                                {
                                                    robotStep = ReelHandlerSteps.Done;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) || cmddone_)
                                            {
                                                robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        switch (pointIndex_)
                                        {
                                            case 1:
                                            case 2:
                                            case 3:
                                                {
                                                    if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                                    {
                                                        if (ReelTowerManager.HasUnloadRequest())
                                                        {
                                                            robotStep = ReelHandlerSteps.GoToHomeBeforeReelHeightCheck;
                                                            Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                            Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.None);
                                                        }
                                                        else
                                                            robotStep -= 1;
                                                    }
                                                    else if (robotSequenceManager.IsFailure)
                                                    {
                                                        throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Set cart guide points
                    // UPDATED: 20200512 (Marcus)
                    case ReelHandlerSteps.ApplyCartGuidePoint1:
                    case ReelHandlerSteps.ApplyCartGuidePoint2:
                    case ReelHandlerSteps.ApplyCartGuidePoint3:
                    case ReelHandlerSteps.ApplyCartGuidePoint4:
                        {
                            int pointIndex_ = 0;
                            RobotSequenceCommands cmd_ = RobotSequenceCommands.ApplyCartGuidePoint1;

                            switch (pointIndex_ = (robotStep - ReelHandlerSteps.ApplyCartGuidePoint1) / (ReelHandlerSteps.AdjustCartGuidePoint2 - ReelHandlerSteps.AdjustCartGuidePoint1) + 1)
                            {
                                case 1:
                                    cmd_ = RobotSequenceCommands.ApplyCartGuidePoint1;
                                    break;
                                case 2:
                                    cmd_ = RobotSequenceCommands.ApplyCartGuidePoint2;
                                    break;
                                case 3:
                                    cmd_ = RobotSequenceCommands.ApplyCartGuidePoint3;
                                    break;
                                case 4:
                                    cmd_ = RobotSequenceCommands.ApplyCartGuidePoint4;
                                    break;
                            }

                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                            {
                                                if (IsRobotActionRetryOver)
                                                {
                                                    robotTick = App.TickCount;
                                                    robotActionRetryCount = 0;
                                                    throw new PauseException<ErrorCode>(ErrorCode.FailedToSetGuidePointOfCartToRobot);
                                                }
                                                else
                                                {   // UPDATED: 20200512 (Marcus)
                                                    // Have to check guide point setting.
                                                    double x_ = 0, y_ = 0, z_ = 0, rz_ = 0;

                                                    if (loopCountOfCartGuidePoints[pointIndex_].Count > 0)
                                                    {
                                                        foreach (Coord3DField<double, double, double, double, double, double> item in loopCountOfCartGuidePoints[pointIndex_])
                                                        {
                                                            x_ += item.first;
                                                            y_ += item.second;
                                                            z_ += item.third;
                                                            rz_ += item.sixth;
                                                        }

                                                        // Use average value of collection
                                                        x_ /= loopCountOfCartGuidePoints[pointIndex_].Count;
                                                        y_ /= loopCountOfCartGuidePoints[pointIndex_].Count;
                                                        z_ /= loopCountOfCartGuidePoints[pointIndex_].Count;
                                                        rz_ /= loopCountOfCartGuidePoints[pointIndex_].Count;
                                                    }

                                                    if (SetCartGuidePointOffsetOfCart(cmd_, currentReelTypeOfCart, x_, y_, z_, rz_))
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                    else
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                            {
                                                if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) || cmddone_)
                                                    robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        if (ReelTowerManager.HasUnloadRequest())
                                        {
                                            robotStep = ReelHandlerSteps.GoToHomeBeforeReelHeightCheck;
                                            Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                            Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.None);
                                        }
                                        else
                                        {
                                            switch (pointIndex_)
                                            {
                                                case 1:
                                                    {
                                                        robotStep = ReelHandlerSteps.SetCartGuideWorkSlotCenter1;
                                                    }
                                                    break;
                                                case 2:
                                                    {
                                                        switch (currentReelTypeOfCart)
                                                        {
                                                            case ReelDiameters.ReelDiameter7:
                                                                robotStep = ReelHandlerSteps.SetCartGuideWorkSlotCenter5;
                                                                break;
                                                            case ReelDiameters.ReelDiameter13:
                                                                robotStep = ReelHandlerSteps.SetCartGuideWorkSlotCenter2;
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case 3:
                                                    {
                                                        robotStep = ReelHandlerSteps.SetCartGuideWorkSlotCenter2;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.SetCartGuideWorkSlotCenter1:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                            {
                                                if (IsRobotActionRetryOver)
                                                {
                                                    robotTick = App.TickCount;
                                                    robotActionRetryCount = 0;
                                                    throw new PauseException<ErrorCode>(ErrorCode.FailedToSetGuidePointOfCartToRobot);
                                                }
                                                else
                                                {   // UPDATED: 20200625 (Marcus)
                                                    if (SetCartGuidePointWorkSlotCenter(RobotSequenceCommands.SetCartGuideWorkSlotCenter1, currentReelTypeOfCart))
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount = 0;
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                    else
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                            {
                                                if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) || cmddone_)
                                                    robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        switch (currentReelTypeOfCart)
                                        {
                                            case ReelDiameters.ReelDiameter7:
                                            case ReelDiameters.ReelDiameter13:
                                                {
                                                    visionMarkAdjustmentStates |= 0x01;
                                                    robotStep = ReelHandlerSteps.SetCartGuideWorkSlotCenter3;
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.SetCartGuideWorkSlotCenter2:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                            {
                                                if (IsRobotActionRetryOver)
                                                {
                                                    robotTick = App.TickCount;
                                                    robotActionRetryCount = 0;
                                                    throw new PauseException<ErrorCode>(ErrorCode.FailedToSetGuidePointOfCartToRobot);
                                                }
                                                else
                                                {   // UPDATED: 20200625 (Marcus)
                                                    if (SetCartGuidePointWorkSlotCenter(RobotSequenceCommands.SetCartGuideWorkSlotCenter2, currentReelTypeOfCart))
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount = 0;
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                    else
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                            {
                                                if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) || cmddone_)
                                                    robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        switch (currentReelTypeOfCart)
                                        {
                                            case ReelDiameters.ReelDiameter7:
                                            case ReelDiameters.ReelDiameter13:
                                                {
                                                    visionMarkAdjustmentStates |= 0x02;
                                                    robotStep = ReelHandlerSteps.SetCartGuideWorkSlotCenter4;
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.SetCartGuideWorkSlotCenter3:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                            {
                                                if (IsRobotActionRetryOver)
                                                {
                                                    robotTick = App.TickCount;
                                                    robotActionRetryCount = 0;
                                                    throw new PauseException<ErrorCode>(ErrorCode.FailedToSetGuidePointOfCartToRobot);
                                                }
                                                else
                                                {   // UPDATED: 20200625 (Marcus)
                                                    if (SetCartGuidePointWorkSlotCenter(RobotSequenceCommands.SetCartGuideWorkSlotCenter3, currentReelTypeOfCart))
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount = 0;
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                    else
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                            {
                                                if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) || cmddone_)
                                                    robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        switch (currentReelTypeOfCart)
                                        {
                                            case ReelDiameters.ReelDiameter7:
                                            case ReelDiameters.ReelDiameter13:
                                                {
                                                    visionMarkAdjustmentStates |= 0x04;
                                                    robotStep = ReelHandlerSteps.Ready;
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.SetCartGuideWorkSlotCenter4:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                            {
                                                if (IsRobotActionRetryOver)
                                                {
                                                    robotTick = App.TickCount;
                                                    robotActionRetryCount = 0;
                                                    throw new PauseException<ErrorCode>(ErrorCode.FailedToSetGuidePointOfCartToRobot);
                                                }
                                                else
                                                {   // UPDATED: 20200625 (Marcus)
                                                    if (SetCartGuidePointWorkSlotCenter(RobotSequenceCommands.SetCartGuideWorkSlotCenter4, currentReelTypeOfCart))
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount = 0;
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                    else
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                            {
                                                if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) || cmddone_)
                                                {
                                                    robotSubStep = SubSequenceSteps.Post;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        switch (currentReelTypeOfCart)
                                        {
                                            case ReelDiameters.ReelDiameter7:
                                            case ReelDiameters.ReelDiameter13:
                                                {
                                                    visionMarkAdjustmentStates |= 0x08;
                                                    robotStep = ReelHandlerSteps.Ready;
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.SetCartGuideWorkSlotCenter5:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                            {
                                                if (IsRobotActionRetryOver)
                                                {
                                                    robotTick = App.TickCount;
                                                    robotActionRetryCount = 0;
                                                    throw new PauseException<ErrorCode>(ErrorCode.FailedToSetGuidePointOfCartToRobot);
                                                }
                                                else
                                                {   // UPDATED: 20200625 (Marcus)
                                                    if (SetCartGuidePointWorkSlotCenter(RobotSequenceCommands.SetCartGuideWorkSlotCenter5, currentReelTypeOfCart))
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount = 0;
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                    else
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                            {
                                                if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) || cmddone_)
                                                    robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        switch (currentReelTypeOfCart)
                                        {
                                            case ReelDiameters.ReelDiameter7:
                                                {
                                                    visionMarkAdjustmentStates |= 0x10;
                                                    robotStep = ReelHandlerSteps.SetCartGuideWorkSlotCenter6;
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.SetCartGuideWorkSlotCenter6:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                            {
                                                if (IsRobotActionRetryOver)
                                                {
                                                    robotTick = App.TickCount;
                                                    robotActionRetryCount = 0;
                                                    throw new PauseException<ErrorCode>(ErrorCode.FailedToSetGuidePointOfCartToRobot);
                                                }
                                                else
                                                {   // UPDATED: 20200625 (Marcus)
                                                    if (SetCartGuidePointWorkSlotCenter(RobotSequenceCommands.SetCartGuideWorkSlotCenter6, currentReelTypeOfCart))
                                                    {
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                    else
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) || cmddone_)
                                            robotSubStep = SubSequenceSteps.Post;
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        switch (currentReelTypeOfCart)
                                        {
                                            case ReelDiameters.ReelDiameter7:
                                                {
                                                    visionMarkAdjustmentStates |= 0x20;
                                                    robotStep = ReelHandlerSteps.Ready;
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Request reel load action to allow
                    case ReelHandlerSteps.PrepareToLoadReelFromCart:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        // Clear old barcode data.
                                        FireProductionInformation(true); // Reset barcode contexts
                                        robotSubStep = SubSequenceSteps.Proceed;
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        robotSubStep = SubSequenceSteps.Post;
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {   // Try to pick up the reel on return stage.
                                        robotStep = ReelHandlerSteps.SetWorkSlotToRobot;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Set work slot to robot
                    case ReelHandlerSteps.SetWorkSlotToRobot:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (!robotSequenceManager.HasAReel && currentWorkSlotOfCart >= 1 &&
                                            ((currentReelTypeOfCart == ReelDiameters.ReelDiameter7 && currentWorkSlotOfCart <= 6) ||
                                            (currentReelTypeOfCart == ReelDiameters.ReelDiameter13 && currentWorkSlotOfCart <= 4)))
                                        {
                                            if (robotSequenceManager.IsWaitForOrder)
                                            {
                                                if (robotSequenceManager.SendCommand(RobotSequenceCommands.ApplyWorkSlot, $"{currentWorkSlotOfCart}"))
                                                    robotSubStep = SubSequenceSteps.Proceed;
                                            }
                                            else
                                            {   // You need to initialize and jump to ready.
                                                // The state of load reel have to cleared during reset procedure.
                                                throw new PauseException<ErrorCode>(ErrorCode.RobotStateIsNotMatched);
                                            }
                                        }
                                        else
                                        {   // You need to initialize and jump to ready.
                                            // The state of load reel have to cleared during reset procedure.
                                            // Reset work slot
                                            robotStep = ReelHandlerSteps.Done;
                                            throw new PauseException<ErrorCode>(ErrorCode.WorkSlotOfCartIsNotValid);
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // You need to initialize and jump to ready.
                                            // The state of load reel have to cleared during reset procedure.
                                            throw new PauseException<ErrorCode>(ErrorCode.FailedToSetWorkSlotOfCart);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder && robotSequenceManager.WorkSlot == currentWorkSlotOfCart)
                                            robotSubStep = SubSequenceSteps.Post;
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotTick = 0;
                                        robotStep = ReelHandlerSteps.Ready;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Move to cart action entering position
                    case ReelHandlerSteps.GoToHomeBeforeReelHeightCheck:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeBeforeReelHeightCheck))
                                                robotSubStep = SubSequenceSteps.Proceed;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // 30 sec
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) || cmddone_)
                                        {
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.Ready;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Move to reel height check position of work slot
                    case ReelHandlerSteps.MoveToReelHeightCheckPositionOfWorkSlot:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (!robotSequenceManager.HasAReel)
                                        {
                                            if (robotSequenceManager.IsWaitForOrder)
                                            {
                                                if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                {
                                                    result_ = true;
                                                    robotSubStep = SubSequenceSteps.Proceed;
                                                }

                                                if (!result_)
                                                {
                                                    if (robotTick == -1)
                                                        robotTick = App.TickCount;
                                                    else if (IsRobotActionRetryOver)
                                                    {
                                                        throw new PauseException<ErrorCode>(ErrorCode.RobotPositionIsNotMatched);
                                                    }
                                                    else if (IsOverDelayTime(Config.TimeoutOfRobotAction, robotTick))
                                                    {
                                                        robotTick = -1;
                                                        robotActionRetryCount++;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotReelDetectionSensorFailure);
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // 30 sec
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                        {
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.Ready;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Measure reel height of cart
                    case ReelHandlerSteps.MeasureReelHeightOnCart:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsRobotAtWorkSlotOfCart())
                                        {
                                            if (result_ = robotSequenceManager.MeasureReelHeightOfWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                            {
                                                robotSubStep = SubSequenceSteps.Proceed;
                                            }
                                            else
                                            {
                                                throw new PauseException<ErrorCode>(ErrorCode.FailedToSetCartReelSize);
                                            }
                                        }
                                        else
                                        {
                                            robotTick = -1;
                                            robotActionRetryCount = 0;
                                            robotStep = ReelHandlerSteps.MoveToReelHeightCheckPositionOfWorkSlot;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // 30 sec
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsConnected && robotSequenceManager.IsReceived &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                        {   // Turn on light.
                                            visionTick = App.TickCount;
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        if (IsVisionProcessDelayOver(delayOfGrapRetry))
                                        {
                                            visionTick = -1;
                                            robotVisionRetryCount = 0;
                                            robotVisionRetryCycleCount = 0;
                                            robotStep = ReelHandlerSteps.Ready;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Check reel alignment on cart
                    case ReelHandlerSteps.CheckReelAlignmentOnCart:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        switch (Config.ImageProcessorType)
                                        {
                                            case ImageProcessorTypes.Mit:
                                                {
                                                    switch (visionTick)
                                                    {
                                                        case -1:
                                                            {   // Grab
                                                                CompositeVisionManager.SetVisionLight(currentReelTypeOfCart);

                                                                if (IsVisionProcessDelayOver(delayOfGrapRetry))
                                                                {
                                                                    visionTick = App.TickCount;
                                                                    CompositeVisionManager.TriggerOn(currentReelTypeOfCart);
                                                                }
                                                            }
                                                            break;
                                                        case 0:
                                                            {   // Image processing
                                                                visionTick = App.TickCount;
                                                                robotSubStep = SubSequenceSteps.Proceed;
                                                            }
                                                            break;
                                                        default:
                                                            {   // Turn off light
                                                                if (IsVisionProcessDelayOver(delayOfGrapRetry))
                                                                {
                                                                    visionTick = 0;
                                                                    VisionManager._CILightOff();
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                            case ImageProcessorTypes.TechFloor:
                                                {
                                                    startCycleTime = DateTime.Now;
                                                    robotSubStep = SubSequenceSteps.Proceed;
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        switch (Config.ImageProcessorType)
                                        {
                                            case ImageProcessorTypes.Mit:
                                                {
                                                    if (IsVisionProcessDelayOver(delayOfImageProcessing))
                                                    {
                                                        alignmentCoordX = string.Empty;
                                                        alignmentCoordY = string.Empty;

                                                        imageProcessingResult = CompositeVisionManager.GetAdjustmentValue(
                                                            DistanceXOfAlignError,
                                                            DistanceYOfAlignError,
                                                            ref alignmentCoordX,
                                                            ref alignmentCoordY);
                                                        robotSubStep = SubSequenceSteps.Post;
                                                    }
                                                }
                                                break;
                                            case ImageProcessorTypes.TechFloor:
                                                {
                                                    if (IsOverDelayTime(Model.TimeoutOfRetryTrigger, imageProcessTimeoutTick))
                                                    {
                                                        if (FireTriggerVisionControl(Model.LightLevels.Top,
                                                            Model.TaskSubCategories.ProcessAtOnce,
                                                            currentReelTypeOfCart,
                                                            currentWorkSlotOfCart,
                                                            true,
                                                            Model.DelayOfTrigger))
                                                        {
                                                            alignmentCoordX = string.Empty;
                                                            alignmentCoordY = string.Empty;
                                                            robotSubStep = SubSequenceSteps.Post;
                                                        }
                                                        else
                                                        {
                                                            robotVisionRetryCount++;
                                                        }
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        switch (Config.ImageProcessorType)
                                        {
                                            case ImageProcessorTypes.Mit:
                                                {
                                                    switch (imageProcessingResult)
                                                    {
                                                        case ImageProcssingResults.Success:
                                                            {   // Normal case
                                                                reelTowerQueryRetryCount = 0;
                                                                Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.RequestToLoadConfirm);
                                                                robotStep = ReelHandlerSteps.Ready;
                                                            }
                                                            break;
                                                        case ImageProcssingResults.Exception:
                                                        case ImageProcssingResults.OverScope:
                                                            {   // Vision error (Exception or over scope) 
                                                                if (robotVisionRetryCycleCount > 0)
                                                                {
                                                                    if (IsVisionProcessDelayOver(delayOfImageProcessingRetry))
                                                                    {
                                                                        visionTick = -1;
                                                                        robotVisionRetryCycleCount = 0;
                                                                        robotSubStep = SubSequenceSteps.Prepare;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (IsRobotVisionRetryOver)
                                                                    {
                                                                        // Failed to get alignment data
                                                                        robotVisionRetryCount = 0;
                                                                        VisionManager._CILightOff();

                                                                        // Set vision tick variable to retry after reset
                                                                        visionTick = -1;
                                                                        robotVisionRetryCount = 0;
                                                                        robotVisionRetryCycleCount = 0;

                                                                        // Force move to top of work slot
                                                                        // Force move to home to prevent unwanted accident.
                                                                        // Raise vision alarm 
                                                                        if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                                        {
                                                                            robotStep = ReelHandlerSteps.MoveBackToHomeByVisionAlignmentFailureToLoad;
                                                                        }
                                                                    }
                                                                    else
                                                                    {   // Turn on light to grab retry
                                                                        //BarcodeAndVision.SetVisionLight(currentReelTypeOfCart);  

                                                                        visionTick = App.TickCount;
                                                                        robotVisionRetryCount++;
                                                                        robotVisionRetryCycleCount++;
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        case ImageProcssingResults.Empty:
                                                            {
                                                                VisionManager._CILightOff();

                                                                // Move up robot to safe way point.
                                                                if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                                {
                                                                    FireShowNotification(Properties.Resources.String_Notificatiion_Empty_Slot);
                                                                    Logger.Trace($"Change work slot: Current={currentWorkSlotOfCart++}");
                                                                    robotStep = ReelHandlerSteps.PrepareToChangeWorkSlotOfCart;
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                            case ImageProcessorTypes.TechFloor:
                                                {
                                                    if (visionProcessResultState > 0)
                                                    {
                                                        object data = new object();
                                                        int foundqr = 0;
                                                        double matchscale = 0.0;
                                                        double matchscore = 0.0;
                                                        double centerx = -99999.9;
                                                        double centery = -99999.9;
                                                        double radius = -99999.9;
                                                        string foundqrdata = string.Empty;

                                                        // Check reel empty 
                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.UserData_1, ref data))
                                                        {
                                                            if ((bool)data)
                                                            {
                                                                // Move up robot to safe way point.
                                                                if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                                {
                                                                    FireShowNotification(Properties.Resources.String_Notificatiion_Empty_Slot);
                                                                    Logger.Trace($"Change work slot: Current={currentWorkSlotOfCart++}");
                                                                    robotStep = ReelHandlerSteps.PrepareToChangeWorkSlotOfCart;
                                                                    result_ = true;
                                                                }
                                                            }
                                                        }

                                                        if (!result_)
                                                        {   // Check reel x, y
                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.ResultCode, ref data))
                                                            {
                                                                switch ((Cognex.VisionPro.CogToolResultConstants)data)
                                                                {
                                                                    case Cognex.VisionPro.CogToolResultConstants.Accept:
                                                                        {
                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.MatchScale, ref data))
                                                                            {
                                                                                if (data != null)
                                                                                    matchscale = (double)data;
                                                                            }

                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.MatchScore, ref data))
                                                                            {
                                                                                if (data != null)
                                                                                    matchscore = (double)data;
                                                                            }

                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundPatternCenterX, ref data))
                                                                            {
                                                                                if (data != null)
                                                                                    centerx = (double)data;
                                                                            }

                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundPatternCenterY, ref data))
                                                                            {
                                                                                if (data != null)
                                                                                    centery = (double)data;
                                                                            }

                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundPatternRadius, ref data))
                                                                            {
                                                                                if (data != null)
                                                                                    radius = (double)data;
                                                                            }

                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.UserData_3, ref data))
                                                                            {
                                                                                if (data != null)
                                                                                    foundqr = (int)data;
                                                                            }

                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundQrCode, ref data))
                                                                            {
                                                                                if (data != null)
                                                                                    foundqrdata = data.ToString();
                                                                            }

                                                                            if (matchscore > 0.4 && Math.Abs(centerx) < DistanceXOfAlignError && Math.Abs(centery) < DistanceYOfAlignError) // Math.Round(matchscale) == 1.0 &&  radius > 100)
                                                                            {
                                                                                alignmentCoordX = centerx.ToString();
                                                                                alignmentCoordY = centery.ToString();

                                                                                result_ = true;
                                                                                reelTowerQueryRetryCount = 0;
                                                                                Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.RequestToLoadConfirm);
                                                                                robotStep = ReelHandlerSteps.Ready;

                                                                                if (foundqr <= 0 || string.IsNullOrEmpty(foundqrdata))
                                                                                {   // Required barcode read again
                                                                                    Logger.Trace($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Required barcode read again.");
                                                                                }
                                                                            }
                                                                            else
                                                                                result_ = false;
                                                                        }
                                                                        break;
                                                                    default:
                                                                        {
                                                                            result_ = false;
                                                                        }
                                                                        break;
                                                                }
                                                            }

                                                            if (!result_ || robotStep != ReelHandlerSteps.Ready)
                                                            {   // Vision error (Exception or over scope) 
                                                                ResetVisionProcessResultState();
                                                                CheckImageProcessRetry(ReelHandlerSteps.CheckReelAlignmentOnCart, SubSequenceSteps.Proceed, ReelHandlerSteps.MoveBackToHomeByVisionAlignmentFailureToLoad);
                                                            }
                                                        }
                                                    }
                                                    else if (IsOverDelayTime(Model.TimeoutOfTotalImageProcess, imageProcessTimeoutTick))
                                                    {
                                                        ResetVisionProcessResultState();
                                                        CheckImageProcessRetry(ReelHandlerSteps.CheckReelAlignmentOnCart, SubSequenceSteps.Proceed, ReelHandlerSteps.MoveBackToHomeByVisionAlignmentFailureToLoad);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Request to reel load confirm
                    case ReelHandlerSteps.RequestToReelLoadConfirm:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                        {
                                            case TransferMaterialObject.TransferStates.RequestToLoadConfirm:
                                            case TransferMaterialObject.TransferStates.WaitForLoadConfirm:
                                                {   // Send load move request to tower.
                                                    if (ReelTowerManager.SendTowerMessage(ReelTowerCommands.REQUEST_REEL_LOAD_MOVE,
                                                        reelBarcodeContexts, currentLoadReelTowerId))
                                                    {
                                                        materialLoadMoveTick = App.TickCount;
                                                        Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.WaitForLoadConfirm);
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                }
                                                break;
                                            default:    // Abnormal case
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (ReelTowerManager.IsFailure())
                                        {
                                            if (IsReelTowerQueryRetryOver)
                                            {
                                                reelTowerQueryRetryCount = 0;
                                                CancelLoadProcedure();

                                                // Force move to safe way point to prevent unwanted accident.
                                                if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                    robotStep = ReelHandlerSteps.MoveBackToHomeByReelTowerResponseTimeoutToLoad;
                                            }
                                            else
                                            {
                                                // UPDATE : 20190705
                                                if (IsOverDelayTime(ReelTowerManager.TimeoutOfTowerResponse, materialBarcodeTick))
                                                {
                                                    reelTowerQueryRetryCount++;
                                                    robotSubStep = SubSequenceSteps.Prepare;
                                                }
                                            }
                                        }
                                        else if (ReelTowerManager.IsReceivedResponse())
                                        {
                                            if (ReelTowerManager.GetReceivedResponse(ref responseOfReelTower))
                                            {
                                                if (responseOfReelTower.ReturnCode == "0")
                                                {
                                                    Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.ConfirmLoad);
                                                    robotSubStep = SubSequenceSteps.Post;
                                                }
                                                else
                                                {
                                                    CancelLoadProcedure();

                                                    // Force move to top of work slot
                                                    // Force move to home to prevent unwanted accident.
                                                    // Raise vision alarm 

                                                    //if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                    if (robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeAfterPickUpReel))
                                                        robotStep = ReelHandlerSteps.MoveBackToHomeByReelTowerRefuseToLoad;
                                                }

                                                ReelTowerManager.FireReportRuntimeLog($"Refused load a reel process by tower ({currentLoadReelTowerId}). RETURNCODE={responseOfReelTower.ReturnCode},UID={responseOfReelTower.Uid}");
                                            }
                                        }
                                        else if (IsReelTowerQueryRetryOver)
                                        {   // Throw the clamped reel to buffer stage.
                                            reelTowerQueryRetryCount = 0;

                                            // Force move to top of work slot
                                            // Force move to home to prevent unwanted accident.
                                            // Raise vision alarm 
                                            if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                robotStep = ReelHandlerSteps.MoveBackToHomeByReelTowerResponseTimeoutToLoad;
                                        }
                                        else if (IsOverDelayTime(ReelTowerManager.TimeoutOfTowerResponse, materialLoadMoveTick))
                                        {
                                            reelTowerQueryRetryCount++;
                                            robotSubStep = SubSequenceSteps.Prepare;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotVisionRetryCount = 0;
                                        robotVisionRetryCycleCount = 0;
                                        robotVisionRetryAttemptsCount = 0;
                                        robotStep = ReelHandlerSteps.Ready;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Prepare to read barcode (Light on then grab)
                    case ReelHandlerSteps.PrepareToReadBarcodeOnReel:
                        {
                            switch (Config.ImageProcessorType)
                            {
                                case ImageProcessorTypes.Mit:
                                    {
                                        switch (robotSubStep)
                                        {
                                            case SubSequenceSteps.Prepare:
                                                {
                                                    barcodeKeyInData.Clear();

                                                    switch (robotSequenceManager.RobotActionOrder)
                                                    {
                                                        case RobotActionOrder.LoadReelFromCart:
                                                            {
                                                                switch (currentReelTypeOfCart)
                                                                {
                                                                    case ReelDiameters.ReelDiameter7:
                                                                        VisionManager.LightOnForReel7Barcode();
                                                                        break;
                                                                    case ReelDiameters.ReelDiameter13:
                                                                        VisionManager.LightOnForReel13Barcode();
                                                                        break;
                                                                }
                                                            }
                                                            break;
                                                        case RobotActionOrder.LoadReelFromReturn:
                                                            {
                                                                switch (currentReelTypeOfReturn)
                                                                {
                                                                    case ReelDiameters.ReelDiameter7:
                                                                        VisionManager.LightOnForReel7Barcode();
                                                                        break;
                                                                    case ReelDiameters.ReelDiameter13:
                                                                        VisionManager.LightOnForReel13Barcode();
                                                                        break;
                                                                }
                                                            }
                                                            break;
                                                    }

                                                    visionTick = App.TickCount;
                                                    robotSubStep = SubSequenceSteps.Proceed;
                                                }
                                                break;
                                            case SubSequenceSteps.Proceed:
                                                {
                                                    if (IsVisionProcessDelayOver(delayOfImageProcessingRetry))
                                                    {
                                                        switch (robotSequenceManager.RobotActionOrder)
                                                        {
                                                            case RobotActionOrder.LoadReelFromCart:
                                                                {
                                                                    switch (currentReelTypeOfCart)
                                                                    {
                                                                        case ReelDiameters.ReelDiameter7:
                                                                            VisionManager.TriggerForReel7Barcode();
                                                                            break;
                                                                        case ReelDiameters.ReelDiameter13:
                                                                            VisionManager.TriggerForReel13Barcode();
                                                                            break;
                                                                    }
                                                                }
                                                                break;
                                                            case RobotActionOrder.LoadReelFromReturn:
                                                                {
                                                                    switch (currentReelTypeOfReturn)
                                                                    {
                                                                        case ReelDiameters.ReelDiameter7:
                                                                            VisionManager.TriggerForReel7Barcode();
                                                                            break;
                                                                        case ReelDiameters.ReelDiameter13:
                                                                            VisionManager.TriggerForReel13Barcode();
                                                                            break;
                                                                    }
                                                                }
                                                                break;
                                                        }

                                                        visionTick = App.TickCount;
                                                        robotSubStep = SubSequenceSteps.Post;
                                                    }
                                                }
                                                break;
                                            case SubSequenceSteps.Post:
                                                {
                                                    if (IsVisionProcessDelayOver(delayOfImageProcessing))
                                                    {
                                                        VisionManager._CILightOff();
                                                        robotVisionRetryCount = 0;
                                                        // robotVisionRetryCycleCount = 0;
                                                        robotStep = ReelHandlerSteps.ReadBarcodeOnReel;
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case ImageProcessorTypes.TechFloor:
                                    {
                                        if (visionProcessResultState > 0)
                                        {
                                            ResetVisionProcessResultState();
                                            ResetBarcodeContexts();
                                            object data = new object();

                                            // Check reel empty 
                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.UserData_1, ref data))
                                            {
                                                if (!(bool)data)
                                                {   // Check qr code count
                                                    if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.UserData_3, ref data))
                                                    {
                                                        if ((int)data > 0)
                                                        {
                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundQrCode, ref data))
                                                            {
                                                                string qrdata = data.ToString();

                                                                if (string.IsNullOrEmpty(qrdata) || qrdata.Substring(0, 2) == "RQ" || qrdata.Length <= 10)
                                                                {
                                                                    result_ = false;
                                                                }
                                                                else
                                                                {
                                                                    try
                                                                    {
                                                                        Logger.Trace($"Actual decoded barcode={VisionManager.VisionBarcode = qrdata}");

                                                                        if (CompositeVisionManager.GetBarcode(ref reelBarcodeContexts)) // TF solution
                                                                        {   // Validate barcode contexts.
                                                                            if (reelBarcodeContexts.IsValid)
                                                                            {
                                                                                if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.UserData_2, ref data))
                                                                                {
                                                                                    if ((int)data > 0)
                                                                                        reelBarcodeContexts.Quantity = Convert.ToInt32(data);
                                                                                }

                                                                                FireProductionInformation(); // TF solution
                                                                                Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.RequestToBarcodeConfirm);
                                                                                robotStep = ReelHandlerSteps.Ready;
                                                                                result_ = true;
                                                                            }
                                                                        }
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        result_ = false;
                                                                        Logger.Trace($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Barcode data parsing error. Exception={ex.Message}");
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (!result_)
                                            {
                                                robotVisionRetryCount = 0;
                                                robotStep = ReelHandlerSteps.ReadBarcodeOnReel;
                                            }
                                        }
                                        else
                                        {
                                            robotVisionRetryCount = 0;
                                            robotStep = ReelHandlerSteps.ReadBarcodeOnReel;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Get decoded barcode
                    case ReelHandlerSteps.ReadBarcodeOnReel:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        switch (Config.ImageProcessorType)
                                        {
                                            case ImageProcessorTypes.Mit:
                                                {
                                                    if (robotVisionRetryCycleCount > 0)
                                                    {
                                                        if (IsVisionProcessDelayOver(delayOfImageProcessingRetry))
                                                        {
                                                            robotVisionRetryCycleCount = 0;
                                                            robotStep = ReelHandlerSteps.PrepareToReadBarcodeOnReel;
                                                        }
                                                    }
                                                    else
                                                    {   // Take barcode data.
                                                        if (VisionManager.GetValue(VisionProcessItems.Barcode, ref imageProcessingResult))
                                                            robotVisionRetryCount = 0;

                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                }
                                                break;
                                            case ImageProcessorTypes.TechFloor:
                                                {
                                                    bool cart = true;

                                                    switch (robotSequenceManager.RobotActionOrder)
                                                    {
                                                        case RobotActionOrder.LoadReelFromCart:
                                                            {
                                                                cart = true;
                                                            }
                                                            break;
                                                        case RobotActionOrder.LoadReelFromReturn:
                                                            {
                                                                cart = false;
                                                            }
                                                            break;
                                                    }

                                                    if (IsOverDelayTime(Model.TimeoutOfRetryTrigger, imageProcessTimeoutTick))
                                                    {
                                                        if (FireTriggerVisionControl(Model.LightLevels.Top,
                                                            Model.TaskSubCategories.DecodeBarcode,
                                                            cart ? currentReelTypeOfCart : currentReelTypeOfReturn,
                                                            cart ? currentWorkSlotOfCart : 0,
                                                            cart,
                                                            Model.DelayOfTrigger))
                                                        {
                                                            robotSubStep = SubSequenceSteps.Proceed;
                                                        }
                                                        else
                                                        {
                                                            robotVisionRetryCount++;
                                                        }
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        switch (Config.ImageProcessorType)
                                        {
                                            case ImageProcessorTypes.Mit:
                                                {
                                                    if (imageProcessingResult == ImageProcssingResults.Success)
                                                    {
                                                        if (CompositeVisionManager.GetBarcode(ref reelBarcodeContexts)) // MIT solution
                                                        {   // Validate barcode contexts.
                                                            if (reelBarcodeContexts.IsValid)
                                                            {
                                                                FireProductionInformation(); // MIT solution
                                                                robotSubStep = SubSequenceSteps.Post;
                                                            }
                                                        }
                                                    }

                                                    if (robotSubStep == SubSequenceSteps.Proceed)
                                                    {
                                                        if (IsRobotVisionRetryOver)
                                                        {   // Retry
                                                            robotStep = ReelHandlerSteps.PrepareToReadBarcodeOnReel;
                                                        }
                                                        else
                                                        {   // Failed to read barcode.
                                                            robotVisionRetryCycleCount = 0;
                                                            VisionManager._CILightOff();
                                                            // Decode error
                                                            visionDecodeError++;
                                                            FirePopupBarcodeInputWindow(reelBarcodeContexts);
                                                            robotStep = ReelHandlerSteps.WaitForBarcodeInput;
                                                        }
                                                    }
                                                }
                                                break;
                                            case ImageProcessorTypes.TechFloor:
                                                {
                                                    if (visionProcessResultState > 0)
                                                    {
                                                        ResetVisionProcessResultState();
                                                        ResetBarcodeContexts();
                                                        object data = new object();

                                                        // Check reel empty 
                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.UserData_1, ref data))
                                                        {
                                                            if (!(bool)data)
                                                            {   // Check qr code count
                                                                if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.UserData_3, ref data))
                                                                {
                                                                    if ((int)data > 0)
                                                                    {
                                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundQrCode, ref data))
                                                                        {
                                                                            string qrdata = data.ToString();

                                                                            if (string.IsNullOrEmpty(qrdata) || qrdata.Substring(0, 2) == "RQ" || qrdata.Length <= 10)
                                                                            {
                                                                                result_ = false;
                                                                            }
                                                                            else
                                                                            {
                                                                                try
                                                                                {
                                                                                    Logger.Trace($"Actual decoded barcode={VisionManager.VisionBarcode = qrdata}");

                                                                                    if (CompositeVisionManager.GetBarcode(ref reelBarcodeContexts)) // TF solution
                                                                                    {   // Validate barcode contexts.
                                                                                        if (reelBarcodeContexts.IsValid)
                                                                                        {
                                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.UserData_2, ref data))
                                                                                            {
                                                                                                if ((int)data > 0)
                                                                                                    reelBarcodeContexts.Quantity = Convert.ToInt32(data);
                                                                                            }

                                                                                            FireProductionInformation(); // TF solution
                                                                                            Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.RequestToBarcodeConfirm);
                                                                                            robotStep = ReelHandlerSteps.Ready;
                                                                                            result_ = true;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                catch (Exception ex)
                                                                                {
                                                                                    result_ = false;
                                                                                    Logger.Trace($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Barcode data parsing error. Exception={ex.Message}");
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (!result_)
                                                        {
                                                            CheckImageProcessRetry(ReelHandlerSteps.ReadBarcodeOnReel, SubSequenceSteps.Prepare, ReelHandlerSteps.WaitForBarcodeInput);
                                                        }
                                                    }
                                                    else if (IsOverDelayTime(Model.TimeoutOfTotalImageProcess, imageProcessTimeoutTick))
                                                    {
                                                        CheckImageProcessRetry(ReelHandlerSteps.ReadBarcodeOnReel, SubSequenceSteps.Prepare, ReelHandlerSteps.WaitForBarcodeInput);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.RequestToBarcodeConfirm);
                                        robotStep = ReelHandlerSteps.Ready;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Request to confirm load reel barcode
                    case ReelHandlerSteps.RequestToConfirmLoadReelBarcodeOfCart:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                        {
                                            case TransferMaterialObject.TransferStates.CompleteLoad:
                                            case TransferMaterialObject.TransferStates.RequestToBarcodeConfirm:
                                            case TransferMaterialObject.TransferStates.WaitForBarcodeConfirm:
                                                {
                                                    reelBarcodeContexts.SetReelInformation(LoadMaterialTypes.Cart, currentReelTypeOfCart);
                                                    if (ReelTowerManager.SendTowerMessage(ReelTowerCommands.REQUEST_BARCODEINFO_CONFIRM,
                                                        reelBarcodeContexts, currentLoadReelTowerId))
                                                    {
                                                        barcodeConfirmState = BarcodeConfirmStates.Prepared;
                                                        materialBarcodeTick = App.TickCount;
                                                        Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.WaitForBarcodeConfirm);
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                }
                                                break;
                                            default:    // Abnormal case
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (ReelTowerManager.IsFailure())
                                        {
                                            if (IsReelTowerQueryRetryOver)
                                            {
                                                reelTowerQueryRetryCount = 0;
                                                CancelLoadProcedure();

                                                // Force move to safe way point to prevent unwanted accident.
                                                if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                    robotStep = ReelHandlerSteps.MoveBackToHomeByReelTowerResponseTimeoutToLoad;
                                            }
                                            else
                                            {
                                                // UPDATE : 20190705
                                                if (IsOverDelayTime(ReelTowerManager.TimeoutOfTowerResponse, materialBarcodeTick))
                                                {
                                                    reelTowerQueryRetryCount++;
                                                    robotSubStep = SubSequenceSteps.Prepare;
                                                }
                                            }
                                        }
                                        else if (ReelTowerManager.IsReceivedResponse())
                                        {
                                            if (ReelTowerManager.GetReceivedResponse(ref responseOfReelTower))
                                            {
                                                alignmentCoordZ = "0";

                                                if (responseOfReelTower.ReturnCode == "0" &&
                                                    ReelTowerManager.IsValidMaterialData(currentLoadReelState, responseOfReelTower, reelBarcodeContexts))
                                                {
                                                    if (!string.IsNullOrEmpty(responseOfReelTower.ReturnMessage))
                                                    {
                                                        switch (responseOfReelTower.ReturnMessage.ToLower())
                                                        {
                                                            case "reelthickness4":
                                                            case "reelthickness8":
                                                            case "reelthickness12":
                                                            case "reelthickness16":
                                                            case "reelthickness24":
                                                            case "reelthickness32":
                                                            case "reelthickness44":
                                                            case "reelthickness56":
                                                            case "reelthickness72":
                                                                alignmentCoordZ = responseOfReelTower.ReturnMessage.Replace("reelthickness", string.Empty);
                                                                break;
                                                        }
                                                    }

                                                    barcodeConfirmState = BarcodeConfirmStates.Confirmed;
                                                    Singleton<TransferMaterialObject>.Instance.SetStateWithRoute(TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart, currentReelTypeOfCart, currentWorkSlotOfCart, currentLoadReelTower);
                                                    robotStep = ReelHandlerSteps.Ready;
                                                }
                                                else
                                                {
                                                    if (responseOfReelTower.ReturnMessage.Contains("DUPLICATE ERROR"))
                                                    {
                                                        SameReelCheck = true;
                                                    }
                                                    barcodeConfirmState = BarcodeConfirmStates.Reject;
                                                    robotTick = App.TickCount;
                                                    robotSubStep = SubSequenceSteps.Post;
                                                }

                                                ReelTowerManager.FireReportRuntimeLog($"Confirmed to load a reel request by tower ({currentLoadReelTowerId}). RETURNCODE={responseOfReelTower.ReturnCode},UID={responseOfReelTower.Uid}");
                                            }
                                        }
                                        else
                                        {   // UPDATE : 20190705
                                            if (IsOverDelayTime(ReelTowerManager.TimeoutOfTowerResponse, materialBarcodeTick))
                                            {
                                                if (IsReelTowerQueryRetryOver)
                                                {
                                                    robotTick = App.TickCount;
                                                    robotSubStep = SubSequenceSteps.Post;
                                                    ReelTowerManager.FireReportRuntimeLog($"Failed to load a reel request confirm by tower ({currentLoadReelTowerId}). RETURNCODE={responseOfReelTower.ReturnCode},UID={responseOfReelTower.Uid}");
                                                }
                                                else
                                                {
                                                    reelTowerQueryRetryCount++;
                                                    robotSubStep = SubSequenceSteps.Prepare;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        if (IsRobotActionDelayOver(intervalReelTowerStatePolling * 2))
                                        {   // UPDATED: 20200408 (Marcus)
                                            // Set reject reel mode.
                                            if (Config.EnableRejectReel)
                                            {
                                                if (IsRobotVisionRetryOver)
                                                {
                                                    CancelLoadProcedure();
                                                    Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.PrepareToRejectCartReel);
                                                }

                                                robotVisionRetryCount++;
                                                robotStep = ReelHandlerSteps.CheckReelAlignmentAfterBarcodeInput;
                                            }
                                            else
                                            {
                                                reelTowerQueryRetryCount = 0;
                                                CancelLoadProcedure();

                                                // Force move to safe way point to prevent unwanted accident.
                                                if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                    robotStep = ReelHandlerSteps.MoveBackToHomeByReelTowerResponseTimeoutToLoad;
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Adjust reel pick up position then pickup
                    case ReelHandlerSteps.AdjustPositionAndPickupReelOfCart:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        robotSequenceErrorCode = ErrorCode.None;

                                        if (robotSequenceManager.HasAReel)
                                        {
                                            if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                            {
                                                if (cmddone_)
                                                {
                                                    switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                                    {
                                                        case TransferMaterialObject.TransferModes.Load:
                                                            {
                                                                if (Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart)
                                                                {
                                                                    robotVisionRetryCount = 0;
                                                                    robotVisionRetryAttemptsCount = 0;
                                                                    reelTowerQueryRetryCount = 0;
                                                                    robotStep = ReelHandlerSteps.Ready;
                                                                }
                                                            }
                                                            break;
                                                        case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                            {
                                                                robotVisionRetryCount = 0;
                                                                robotVisionRetryAttemptsCount = 0;
                                                                reelTowerQueryRetryCount = 0;
                                                                robotStep = ReelHandlerSteps.Ready;
                                                            }
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                            {
                                                case TransferMaterialObject.TransferModes.Load:
                                                    {
                                                        if (Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart)
                                                        {   // After success alignment and barcode reading.
                                                            // Align command.
                                                            if (robotSequenceManager.PickupReelFromCart(currentReelTypeOfCart, currentWorkSlotOfCart, alignmentCoordX, alignmentCoordY, alignmentCoordZ))
                                                            {
                                                                robotSubStep = SubSequenceSteps.Proceed;
                                                            }
                                                        }
                                                        else
                                                        {   // Failure process
                                                            robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                                            robotSubStep = SubSequenceSteps.Post;
                                                        }
                                                    }
                                                    break;
                                                case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                    {
                                                        if (robotSequenceManager.PickupReelFromCart(currentReelTypeOfCart, currentWorkSlotOfCart, alignmentCoordX, alignmentCoordY, alignmentCoordZ))
                                                        {
                                                            robotSubStep = SubSequenceSteps.Proceed;
                                                        }
                                                        else
                                                        {   // Failure process
                                                            robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                                            robotSubStep = SubSequenceSteps.Post;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {
                                            robotSequenceErrorCode = ErrorCode.RobotMoveTimeout;
                                            robotInitializeSubStep = SubSequenceSteps.Post;
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                        {
                                            if (cmddone_)
                                            {
                                                if (robotSequenceManager.HasAReel)
                                                {   // Success
                                                    robotVisionRetryCount = 0;
                                                    robotVisionRetryAttemptsCount = 0;
                                                    reelTowerQueryRetryCount = 0;
                                                    robotStep = ReelHandlerSteps.Ready;
                                                }
                                                else
                                                {   // Failure (Load reel pick up failure)
                                                    robotVisionRetryCount = 0;
                                                    robotVisionRetryAttemptsCount = 0;

                                                    if (robotSequenceManager.FailureCode != RobotActionFailures.None)
                                                    {   // Failure process
                                                        robotSequenceErrorCode = ErrorCode.FailedToPickupReelFromCart;
                                                        robotSubStep = SubSequenceSteps.Post;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        CancelLoadProcedure();

                                        // Force move to safe way point to prevent unwanted accident.
                                        if (robotSequenceManager.IsRobotAtWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart) ||
                                            robotSequenceManager.IsAtSafePosition)
                                        {
                                            robotStep = ReelHandlerSteps.MoveBackToHomeByReelPickupFailureToLoad;
                                        }
                                        else
                                        {
                                            if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                robotStep = ReelHandlerSteps.MoveBackToHomeByReelPickupFailureToLoad;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Move to front of load tower
                    case ReelHandlerSteps.GoToHomeAfterPickUpReel:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        // robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched; // Just retry
                                        robotSubStep = SubSequenceSteps.Post;

                                        if (robotSequenceManager.HasAReel && !robotSequenceManager.IsMoving &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) && cmddone_)
                                        {
                                            switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                            {
                                                case TransferMaterialObject.TransferModes.Load:
                                                case TransferMaterialObject.TransferModes.LoadReturn:
                                                case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                    {
                                                        result_ = false;

                                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                                        {
                                                            case TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart:
                                                            case TransferMaterialObject.TransferStates.ConfirmedBarcodeOfReturn:
                                                                {
                                                                    robotSequenceErrorCode = ErrorCode.None;
                                                                    result_ = true;
                                                                }
                                                                break;
                                                            default:
                                                            case TransferMaterialObject.TransferStates.RequestToBarcodeConfirm:
                                                            case TransferMaterialObject.TransferStates.WaitForBarcodeConfirm:
                                                                {
                                                                    if (barcodeConfirmState == BarcodeConfirmStates.Reject)
                                                                        result_ = true;
                                                                }
                                                                break;
                                                        }

                                                        if (result_ && robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeAfterPickUpReel))
                                                            robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                        {
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.Ready;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Move to front of load tower
                    case ReelHandlerSteps.MoveToFrontOfTower:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        robotSequenceErrorCode = ErrorCode.None;

                                        if (robotSequenceManager.HasAReel && !robotSequenceManager.IsMoving &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) && cmddone_ &&
                                            (Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.Load &&
                                            Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart) ||
                                            (Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.LoadReturn &&
                                            Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.ConfirmedBarcodeOfReturn))
                                        {
                                            if (robotSequenceManager.MoveRobotToFrontOfTower(currentLoadReelTower))
                                            {
                                                robotSubStep = SubSequenceSteps.Proceed;
                                            }
                                        }
                                        else
                                        {
                                            robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {
                                            robotSequenceErrorCode = ErrorCode.RobotMoveTimeout;
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                        {
                                            robotStep = ReelHandlerSteps.Ready;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {   // Failure process
                                        CancelLoadProcedure();
                                        throw new PauseException<ErrorCode>(robotSequenceErrorCode);
                                    }
                            }
                        }
                        break;
                    #endregion

                    #region Put a reel into tower port
                    case ReelHandlerSteps.PutReelIntoTower:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        robotSequenceErrorCode = ErrorCode.None;

                                        if (robotSequenceManager.HasAReel)
                                        {
                                            if (robotSequenceManager.IsAtSafePosition && !robotSequenceManager.IsMoving &&
                                                (Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.Load &&
                                                Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart) ||
                                                (Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.LoadReturn &&
                                                Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.ConfirmedBarcodeOfReturn))
                                            {
                                                if (robotSequenceManager.PutReelIntoTowerPort(currentLoadReelTower))
                                                {
                                                    robotSubStep = SubSequenceSteps.Proceed;
                                                }
                                            }
                                            else
                                            {
                                                robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                                robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                        else
                                        {
                                            robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {
                                            robotSequenceErrorCode = ErrorCode.RobotMoveTimeout;
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) &&
                                            !robotSequenceManager.HasAReel)
                                        {
                                            Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.RequestToLoadAssignment);
                                            robotStep = ReelHandlerSteps.Ready;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {   // Failure process
                                        CancelLoadProcedure();
                                        throw new PauseException<ErrorCode>(robotSequenceErrorCode);
                                    }
                            }
                        }
                        break;
                    #endregion

                    #region Request to confirm loaded reel assignment
                    case ReelHandlerSteps.RequestToConfirmLoadedReelAssign:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        robotSequenceErrorCode = ErrorCode.None;

                                        if (robotSequenceManager.HasAReel)
                                        {
                                            robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                        else
                                        {
                                            if (robotSequenceManager.IsAtSafePosition && !robotSequenceManager.IsMoving &&
                                                (robotSequenceManager.ActionState == RobotActionStates.LoadCompleted ||
                                                robotSequenceManager.ActionState == RobotActionStates.Stop))
                                            {
                                                switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                                {
                                                    case TransferMaterialObject.TransferModes.Load:
                                                    case TransferMaterialObject.TransferModes.LoadReturn:
                                                        {
                                                            switch (Singleton<TransferMaterialObject>.Instance.State)
                                                            {
                                                                case TransferMaterialObject.TransferStates.RequestToLoadAssignment:
                                                                    {
                                                                        if (ReelTowerManager.SendTowerMessage(ReelTowerCommands.REQUEST_REEL_LOAD_ASSIGN,
                                                                            reelBarcodeContexts, currentLoadReelState.Name))
                                                                        {
                                                                            materialBarcodeTick = 0;
                                                                            Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.WaitForLoadAssignment);
                                                                            robotSubStep = SubSequenceSteps.Proceed;
                                                                        }
                                                                    }
                                                                    break;
                                                                case TransferMaterialObject.TransferStates.WaitForLoadAssignment:
                                                                    {
                                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                                    }
                                                                    break;
                                                                default:
                                                                    {   // Abnormal case
                                                                        robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                                                        robotSubStep = SubSequenceSteps.Post;
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        {
                                                            robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                                            robotSubStep = SubSequenceSteps.Post;
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        // if (reelTower.IsFailure() || IsReelTowerQueryRetryOver)
                                        // {
                                        //     // robotSequenceErrorCode = ErrorCode.ReelTowerResponseTimeout;
                                        //     // robotSubStep = SubSequenceSteps.Post;
                                        //     reelTowerQueryRetryCount = 0;
                                        //     reelTower.IgnoreResponse();
                                        //     result_ = true;
                                        // }
                                        // else if (reelTower.IsReceivedResponse())
                                        // {
                                        //     if (reelTower.GetReceivedResponse(ref res_))
                                        //         result_ = true;
                                        // }
                                        // else
                                        // {
                                        //     // UPDATE : 20190705
                                        //     if (materialLoadMoveTick == 0)
                                        //     {
                                        //         materialLoadMoveTick = App.TickCount;
                                        //     }
                                        //     else if (IsOverDelayTime(ReelTowerManager.TimeoutOfTowerResponse, materialLoadMoveTick))
                                        //     {
                                        //         materialLoadMoveTick = 0;
                                        //         reelTowerQueryRetryCount++;
                                        //         robotSubStep = SubSequenceSteps.Prepare;
                                        //     }
                                        // }
                                        // 
                                        // if (result_)
                                        // {
                                        //     Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.CompleteLoad);
                                        // 
                                        //     switch (robotSequenceManager.RobotActionOrder)
                                        //     {
                                        //         case RobotActionOrder.LoadReelFromCart:
                                        //         case RobotActionOrder.LoadReelFromReturn:
                                        //             {
                                        //             }
                                        //             break;
                                        //         default: // Abnormal case
                                        //             break;
                                        //     }
                                        // 
                                        //     robotStep = RobotSteps.CompletedToLoadReel;
                                        // }

                                        Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.CompleteLoad);

                                        switch (robotSequenceManager.RobotActionOrder)
                                        {
                                            case RobotActionOrder.LoadReelFromCart:
                                            case RobotActionOrder.LoadReelFromReturn:
                                                {
                                                }
                                                break;
                                            default: // Abnormal case
                                                break;
                                        }

                                        robotStep = ReelHandlerSteps.CompletedToLoadReel;
                                        ReelTowerManager.FireReportRuntimeLog($"[Tower Recv] Ignore tower response check. (INDEX={currentLoadReelState.Index},TOWER={currentLoadReelState.Name},MAKER={currentLoadReelState.Maker},SID={currentLoadReelState.Sid},UID={currentLoadReelState.Uid},QTY={currentLoadReelState.Qty})");
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                        throw new PauseException<ErrorCode>(robotSequenceErrorCode);
                                    }
                            }
                        }
                        break;
                    #endregion

                    #region Complete to load reel
                    case ReelHandlerSteps.CompletedToLoadReel:
                        {
                            switch (robotSequenceManager.RobotActionOrder)
                            {
                                case RobotActionOrder.LoadReelFromReturn:
                                    {
                                        currentReelTypeOfReturn = ReelDiameters.Unknown;
                                        totalReturnReelCountValue++;
                                    }
                                    break;
                                case RobotActionOrder.LoadReelFromCart:
                                    {
                                        totalLoadReelCountValue++;
                                    }
                                    break;
                            }

                            finishCycleTime = DateTime.Now;
                            lastCycleTime = finishCycleTime - startCycleTime;
                            robotStep = ReelHandlerSteps.Done;
                        }
                        break;
                    #endregion

                    #region Change work slot
                    case ReelHandlerSteps.PrepareToChangeWorkSlotOfCart:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (currentWorkSlotOfCart > 0 &&
                                            robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart - 1))
                                        {
                                            switch (currentReelTypeOfCart)
                                            {
                                                case ReelDiameters.ReelDiameter7:
                                                    {
                                                        if (robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeAfterPickUpReel))
                                                            robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                    break;
                                                case ReelDiameters.ReelDiameter13:
                                                    {
                                                        if (robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeAfterPickUpReel))
                                                            robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                    break;
                                                default:
                                                    throw new PauseException<ErrorCode>(ErrorCode.RobotStateIsNotMatched);
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                        {
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.Done;
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.ChangeWorkSlotOfCart:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (currentWorkSlotOfCart > 0 &&
                                            robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                        {
                                            switch (currentReelTypeOfCart)
                                            {
                                                case ReelDiameters.ReelDiameter7:
                                                    {
                                                        if (currentWorkSlotOfCart <= 6)
                                                        {
                                                            if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                                robotSubStep = SubSequenceSteps.Proceed;
                                                            else
                                                                throw new PauseException<ErrorCode>(ErrorCode.FailedToSetCartReelSize);
                                                        }
                                                        else
                                                        {
                                                            robotSubStep = SubSequenceSteps.Proceed;
                                                        }
                                                    }
                                                    break;
                                                case ReelDiameters.ReelDiameter13:
                                                    {
                                                        if (currentWorkSlotOfCart <= 4)
                                                        {
                                                            if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                                robotSubStep = SubSequenceSteps.Proceed;
                                                            else
                                                                throw new PauseException<ErrorCode>(ErrorCode.FailedToSetCartReelSize);
                                                        }
                                                        else
                                                        {
                                                            robotSubStep = SubSequenceSteps.Proceed;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        robotSubStep = SubSequenceSteps.Post;
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.Done;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    //case RobotSteps.MoveBackToHomeByCartReelTypeCheckFailure:

                    #region Load failure handling
                    case ReelHandlerSteps.MoveBackToHomeByVisionAlignmentFailureToLoad:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (!robotSequenceManager.IsRobotAtWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart);

                                            robotSubStep = SubSequenceSteps.Proceed;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // 30 sec
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                        {
                                            if (robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeAfterPickUpReel)) //MoveToHome
                                                robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {   // After stop robot moving
                                        // Recover error just set robot step to ready.

                                        if (Config.ImageProcessorType == ImageProcessorTypes.Mit)
                                            VisionManager._CILightOff();

                                        if (robotSequenceManager.SendCommand(RobotSequenceCommands.ResetProcessCount))
                                        {
                                            robotVisionRetryAttemptsCount++;
                                            Console.WriteLine($"robot retry = {robotVisionRetryAttemptsCount}");
                                            Console.WriteLine("Reset Process Succeeded");
                                            robotStep = ReelHandlerSteps.Done;
                                        }
                                        if (IsRobotVisionRetryAttemptsOver)
                                        {
                                            robotVisionRetryAttemptsCount = 0;
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotVisionFailure);
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.MoveBackToHomeByReelTowerResponseTimeoutToLoad:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (!robotSequenceManager.IsRobotAtWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart);

                                            robotSubStep = SubSequenceSteps.Proceed;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {
                                            if (Config.EnableRejectReel)
                                            {

                                            }
                                            else
                                                throw new PauseException<ErrorCode>((barcodeConfirmState == BarcodeConfirmStates.Reject) ? ErrorCode.ReelBarcodeIsNotUnique : ErrorCode.ReelTowerResponseTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                        {
                                            if (robotSequenceManager.SendCommand(RobotSequenceCommands.MoveToHome))
                                                robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        if (robotSequenceManager.SendCommand(RobotSequenceCommands.ResetProcessCount))
                                        {
                                            // After stop robot moving
                                            // Recover error just set robot step to ready.
                                            robotStep = ReelHandlerSteps.Done;
                                            if (Config.EnableRejectReel)
                                            {

                                            }
                                            else
                                                throw new PauseException<ErrorCode>((barcodeConfirmState == BarcodeConfirmStates.Reject) ? ErrorCode.ReelBarcodeIsNotUnique : ErrorCode.ReelTowerResponseTimeout);
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.MoveBackToHomeByReelTowerRefuseToLoad:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (   !robotSequenceManager.IsRobotAtWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart);

                                            robotSubStep = SubSequenceSteps.Proceed;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // 30 sec
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder)
                                            // 20200804 - jm.choi
                                            // RequestToReelLoadConfirm에서 home까지 이동한 상태이며 해당부분에서 False가 되어 시퀀스가 진행되지않음
                                            //&& robotSequenceManager.IsRobotAtWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                        {
                                            if (robotSequenceManager.SendCommand(RobotSequenceCommands.MoveToHome))
                                                robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        FireShowNotification(Properties.Resources.String_Notification_Refused_Load + $"({currentLoadReelTowerId})");

                                        if (Config.ImageProcessorType == ImageProcessorTypes.Mit)
                                            VisionManager._CILightOff();

                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                        robotStep = ReelHandlerSteps.Done;
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.MoveBackToHomeByCancelBarcodeInputToLoad:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (!robotSequenceManager.IsRobotAtWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart);

                                            robotSubStep = SubSequenceSteps.Proceed;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // 30 sec
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                        {
                                            if (robotSequenceManager.SendCommand(RobotSequenceCommands.MoveToHome))
                                                robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        if (Config.ImageProcessorType == ImageProcessorTypes.Mit)
                                            VisionManager._CILightOff();

                                        if (robotSequenceManager.SendCommand(RobotSequenceCommands.ResetProcessCount))
                                            robotStep = ReelHandlerSteps.Done;
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.MoveBackToHomeByReelPickupFailureToLoad:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (!robotSequenceManager.IsRobotAtWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart);

                                            robotSubStep = SubSequenceSteps.Proceed;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // 30 sec
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                        {
                                            if (robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeAfterPickUpReel))
                                                robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        if (Config.EnableRejectReel)
                                        {   // MoveBackToHomeByReelPickupFailureToLoad
                                            CancelLoadProcedure();
                                            Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.PrepareToRejectCartReel);
                                            barcodeConfirmState = BarcodeConfirmStates.Reject;
                                            robotStep = ReelHandlerSteps.Ready;
                                        }
                                        else
                                        {
                                            if (robotSequenceManager.SendCommand(RobotSequenceCommands.ResetProcessCount))
                                                robotStep = ReelHandlerSteps.Done;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.WaitForBarcodeInput:
                        {
                            switch (barcodeKeyInData.State)
                            {
                                case BarcodeInputStates.Typed:
                                    {
                                        reelBarcodeContexts.SetValues(
                                            barcodeKeyInData.Data.Category,
                                            barcodeKeyInData.Data.Name,
                                            barcodeKeyInData.Data.LotId,
                                            barcodeKeyInData.Data.Supplier,
                                            barcodeKeyInData.Data.ManufacturedDatetime,
                                            string.Empty,
                                            string.Empty,
                                            barcodeKeyInData.Data.Quantity);

                                        visionTick = -1;
                                        robotVisionRetryCount = 0;
                                        robotStep = ReelHandlerSteps.CheckReelAlignmentAfterBarcodeInput;
                                    }
                                    break;
                                case BarcodeInputStates.Canceled:
                                    {
                                        CancelLoadProcedure();

                                        switch (robotSequenceManager.RobotActionOrder)
                                        {
                                            case RobotActionOrder.LoadReelFromCart:
                                                {   // Force move to top of work slot
                                                    // Force move to home to prevent unwanted accident.
                                                    // Raise vision alarm 
                                                    if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                        robotStep = ReelHandlerSteps.MoveBackToHomeByCancelBarcodeInputToLoad;
                                                }
                                                break;
                                            case RobotActionOrder.LoadReelFromReturn:
                                                {   // Force move to front of return stage to prevent unwanted accident.
                                                    // Raise vision alarm 
                                                    if (robotSequenceManager.MoveToFrontOfReturnStage(currentReelTypeOfReturn))
                                                        robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStageByCancelBarcodeInputToLoadReturn;
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.CheckReelAlignmentAfterBarcodeInput:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        switch (Config.ImageProcessorType)
                                        {
                                            case ImageProcessorTypes.Mit:
                                                {
                                                    switch (visionTick)
                                                    {
                                                        case -1:
                                                            {   // Grab
                                                                switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                                                {
                                                                    case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                                    case TransferMaterialObject.TransferModes.PrepareToLoad:
                                                                        CompositeVisionManager.SetVisionLight(currentReelTypeOfCart);
                                                                        break;
                                                                    case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                                    case TransferMaterialObject.TransferModes.PrepareToLoadReturn:
                                                                        CompositeVisionManager.SetVisionLight(currentReelTypeOfReturn);
                                                                        break;
                                                                }
                                                                visionTick = App.TickCount;
                                                            }
                                                            break;
                                                        case 0:
                                                            {   // Image processing
                                                                robotSubStep = SubSequenceSteps.Proceed;
                                                            }
                                                            break;
                                                        default:
                                                            {
                                                                if (visionTick > 0)
                                                                {
                                                                    if (IsVisionProcessDelayOver(delayOfImageProcessing))
                                                                    {
                                                                        switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                                                        {
                                                                            case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                                            case TransferMaterialObject.TransferModes.PrepareToLoad:
                                                                                CompositeVisionManager.TriggerOn(currentReelTypeOfCart);
                                                                                break;
                                                                            case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                                            case TransferMaterialObject.TransferModes.PrepareToLoadReturn:
                                                                                CompositeVisionManager.TriggerOn(currentReelTypeOfReturn);
                                                                                break;
                                                                        }
                                                                        visionTick = 0;
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                            case ImageProcessorTypes.TechFloor:
                                                {
                                                    bool cart = true;

                                                    switch (robotSequenceManager.RobotActionOrder)
                                                    {
                                                        case RobotActionOrder.LoadReelFromCart:
                                                            {
                                                                cart = true;
                                                            }
                                                            break;
                                                        case RobotActionOrder.LoadReelFromReturn:
                                                            {
                                                                cart = false;
                                                            }
                                                            break;
                                                    }

                                                    if (IsOverDelayTime(Model.TimeoutOfRetryTrigger, imageProcessTimeoutTick))
                                                    {
                                                        if (IsRobotVisionRetryOver)
                                                        {
                                                            alignmentCoordX = string.Empty;
                                                            alignmentCoordY = string.Empty;
                                                            visionTick = App.TickCount;
                                                            robotSubStep = SubSequenceSteps.Proceed;
                                                        }
                                                        else
                                                        {
                                                            if (FireTriggerVisionControl(Model.LightLevels.Top,
                                                                Model.TaskSubCategories.Alignment,
                                                                cart ? currentReelTypeOfCart : currentReelTypeOfReturn,
                                                                cart ? currentWorkSlotOfCart : 0,
                                                                cart,
                                                                Model.DelayOfTrigger))
                                                            {
                                                                alignmentCoordX = string.Empty;
                                                                alignmentCoordY = string.Empty;
                                                                visionTick = App.TickCount;
                                                                robotSubStep = SubSequenceSteps.Proceed;
                                                            }
                                                            else
                                                            {
                                                                robotVisionRetryCount++;
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {   // Reinitialize barcode confirm state.
                                        switch (Config.ImageProcessorType)
                                        {
                                            case ImageProcessorTypes.Mit:
                                                {
                                                    if (IsVisionProcessDelayOver(delayOfImageProcessing))
                                                    {
                                                        visionTick = 0;
                                                        VisionManager._CILightOff();
                                                        alignmentCoordX = string.Empty;
                                                        alignmentCoordY = string.Empty;

                                                        imageProcessingResult = CompositeVisionManager.GetAdjustmentValue(
                                                            DistanceXOfAlignError,
                                                            DistanceYOfAlignError,
                                                            ref alignmentCoordX,
                                                            ref alignmentCoordY);
                                                        robotSubStep = SubSequenceSteps.Post;
                                                    }
                                                }
                                                break;
                                            case ImageProcessorTypes.TechFloor:
                                                {
                                                    robotSubStep = SubSequenceSteps.Post;
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        switch (Config.ImageProcessorType)
                                        {
                                            case ImageProcessorTypes.Mit:
                                                {
                                                    switch (imageProcessingResult)
                                                    {
                                                        case ImageProcssingResults.Success:
                                                            {   // Normal case
                                                                reelTowerQueryRetryCount = 0;
                                                                Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.RequestToBarcodeConfirm);

                                                                // 20190702  Update: Additional conditions for checking return sequence
                                                                switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                                                {
                                                                    case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                                    case TransferMaterialObject.TransferModes.PrepareToLoad:
                                                                        robotStep = ReelHandlerSteps.RequestToConfirmLoadReelBarcodeOfCart;
                                                                        break;
                                                                    case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                                    case TransferMaterialObject.TransferModes.PrepareToLoadReturn:
                                                                        robotStep = ReelHandlerSteps.RequestToConfirmLoadReelBarcodeOfReturnStage;
                                                                        break;
                                                                }
                                                            }
                                                            break;
                                                        case ImageProcssingResults.Exception:
                                                        case ImageProcssingResults.OverScope:
                                                            {   // Vision error (Exception or over scope) 
                                                                if (robotVisionRetryCycleCount > 0)
                                                                {
                                                                    if (IsVisionProcessDelayOver(delayOfImageProcessingRetry))
                                                                    {
                                                                        visionTick = -1;
                                                                        robotVisionRetryCycleCount = 0;
                                                                        robotSubStep = SubSequenceSteps.Prepare;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (IsRobotVisionRetryOver)
                                                                    {   // Failed to get alignment data
                                                                        VisionManager._CILightOff();

                                                                        // Set vision tick variable to retry after reset
                                                                        visionTick = -1;
                                                                        robotVisionRetryCount = 0;
                                                                        robotVisionRetryCycleCount = 0;

                                                                        //CancelLoadProcedure();

                                                                        // Force move to top of work slot
                                                                        // Force move to home to prevent unwanted accident.
                                                                        // Raise vision alarm 
                                                                        switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                                                        {
                                                                            case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                                            case TransferMaterialObject.TransferModes.PrepareToLoad:
                                                                                {
                                                                                    if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                                                    {
                                                                                        robotStep = ReelHandlerSteps.MoveBackToHomeByVisionAlignmentFailureToLoad;
                                                                                        CancelLoadProcedure();
                                                                                    }
                                                                                }
                                                                                break;
                                                                            case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                                            case TransferMaterialObject.TransferModes.PrepareToLoadReturn:
                                                                                {
                                                                                    if (robotSequenceManager.MoveToFrontOfReturnStage(currentReelTypeOfReturn))
                                                                                    {
                                                                                        robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStageByVisionAlignmentFailureToLoadReturn;
                                                                                        CancelLoadProcedure();
                                                                                    }
                                                                                }
                                                                                break;
                                                                        }
                                                                    }
                                                                    else
                                                                    {   // Turn on light to grab retry
                                                                        visionTick = App.TickCount;
                                                                        robotVisionRetryCount++;
                                                                        robotVisionRetryCycleCount++;
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        case ImageProcssingResults.Empty:
                                                            {
                                                                VisionManager._CILightOff();

                                                                // Move up robot to safe way point.
                                                                switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                                                {
                                                                    case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                                    case TransferMaterialObject.TransferModes.PrepareToLoad:
                                                                        {
                                                                            if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                                            {
                                                                                FireShowNotification(Properties.Resources.String_Notificatiion_Empty_Slot);
                                                                                Logger.Trace($"Change work slot: Current={currentWorkSlotOfCart++}");
                                                                                robotStep = ReelHandlerSteps.PrepareToChangeWorkSlotOfCart;
                                                                            }
                                                                        }
                                                                        break;
                                                                    case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                                    case TransferMaterialObject.TransferModes.PrepareToLoadReturn:
                                                                        {
                                                                            if (robotSequenceManager.MoveToFrontOfReturnStage(currentReelTypeOfReturn))
                                                                            {
                                                                                robotTick = -1;
                                                                                FireShowNotification(Properties.Resources.String_Notification_Return_Stage_Empty);
                                                                                robotStep = ReelHandlerSteps.MoveToFrontOfReturnStage;
                                                                            }
                                                                        }
                                                                        break;
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                            case ImageProcessorTypes.TechFloor:
                                                {
                                                    if (visionProcessResultState > 0)
                                                    {
                                                        object data = new object();
                                                        double matchscale = 0.0;
                                                        double matchscore = 0.0;
                                                        double centerx = -99999.9;
                                                        double centery = -99999.9;
                                                        double radius = -99999.9;

                                                        // Check reel empty
                                                        if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.UserData_1, ref data))
                                                        {
                                                            if ((bool)data)
                                                            {
                                                                // Move up robot to safe way point.
                                                                switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                                                {
                                                                    case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                                    case TransferMaterialObject.TransferModes.PrepareToLoad:
                                                                        {
                                                                            if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                                                            {
                                                                                FireShowNotification(Properties.Resources.String_Notificatiion_Empty_Slot);
                                                                                Logger.Trace($"Change work slot: Current={currentWorkSlotOfCart++}");
                                                                                robotStep = ReelHandlerSteps.PrepareToChangeWorkSlotOfCart;
                                                                            }
                                                                        }
                                                                        break;
                                                                    case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                                    case TransferMaterialObject.TransferModes.PrepareToLoadReturn:
                                                                        {
                                                                            if (robotSequenceManager.MoveToFrontOfReturnStage(currentReelTypeOfReturn))
                                                                            {
                                                                                robotTick = -1;
                                                                                FireShowNotification(Properties.Resources.String_Notification_Return_Stage_Empty);
                                                                                robotStep = ReelHandlerSteps.MoveToFrontOfReturnStage;
                                                                            }
                                                                        }
                                                                        break;
                                                                }

                                                                result_ = true;
                                                            }
                                                        }

                                                        if (!result_)
                                                        {   // Check reel x, y
                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.ResultCode, ref data))
                                                            {
                                                                switch ((Cognex.VisionPro.CogToolResultConstants)data)
                                                                {
                                                                    case Cognex.VisionPro.CogToolResultConstants.Accept:
                                                                        {
                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.MatchScale, ref data))
                                                                            {
                                                                                if (data != null)
                                                                                    matchscale = (double)data;
                                                                            }

                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.MatchScore, ref data))
                                                                            {
                                                                                if (data != null)
                                                                                    matchscore = (double)data;
                                                                            }

                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundPatternCenterX, ref data))
                                                                            {
                                                                                if (data != null)
                                                                                    centerx = (double)data;
                                                                            }

                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundPatternCenterY, ref data))
                                                                            {
                                                                                if (data != null)
                                                                                    centery = (double)data;
                                                                            }

                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundPatternRadius, ref data))
                                                                            {
                                                                                if (data != null)
                                                                                    radius = (double)data;
                                                                            }

                                                                            if (matchscore > 0.4 && Math.Abs(centerx) < DistanceXOfAlignError && Math.Abs(centery) < DistanceYOfAlignError) // Math.Round(matchscale) == 1.0 &&  radius > 100)
                                                                            {
                                                                                alignmentCoordX = centerx.ToString();
                                                                                alignmentCoordY = centery.ToString();
                                                                                result_ = true;
                                                                                Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.RequestToBarcodeConfirm);

                                                                                // 20190702  Update: Additional conditions for checking return sequence
                                                                                switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                                                                {
                                                                                    case TransferMaterialObject.TransferModes.PrepareToLoad:
                                                                                        robotStep = ReelHandlerSteps.RequestToConfirmLoadReelBarcodeOfCart;
                                                                                        break;
                                                                                    case TransferMaterialObject.TransferModes.PrepareToLoadReturn:
                                                                                        robotStep = ReelHandlerSteps.RequestToConfirmLoadReelBarcodeOfReturnStage;
                                                                                        break;
                                                                                    case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                                                    case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                                                        {
                                                                                             barcodeConfirmState = BarcodeConfirmStates.Reject;
                                                                                            robotStep = ReelHandlerSteps.Ready;
                                                                                        }
                                                                                        break;
                                                                                }

                                                                                if (barcodeConfirmState != BarcodeConfirmStates.Reject)
                                                                                // {
                                                                                //     robotStep = ReelHandlerSteps.Ready;
                                                                                // }
                                                                                // else
                                                                                {   // Normal case
                                                                                    robotVisionRetryCount = 0;
                                                                                    reelTowerQueryRetryCount = 0;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                result_ = false;
                                                                            }
                                                                        }
                                                                        break;
                                                                    default:
                                                                        {
                                                                            result_ = false;
                                                                        }
                                                                        break;
                                                                }
                                                            }

                                                            if (!result_ || (robotStep != ReelHandlerSteps.Ready && robotStep != ReelHandlerSteps.RequestToConfirmLoadReelBarcodeOfCart && robotStep != ReelHandlerSteps.RequestToConfirmLoadReelBarcodeOfReturnStage))
                                                            {   // Vision error (Exception or over scope) 
                                                                ResetVisionProcessResultState();
                                                                CheckImageProcessRetry(ReelHandlerSteps.CheckReelAlignmentAfterBarcodeInput, SubSequenceSteps.Prepare);
                                                            }
                                                        }
                                                    }
                                                    else if (IsOverDelayTime(Model.TimeoutOfTotalImageProcess, imageProcessTimeoutTick))
                                                    {
                                                        ResetVisionProcessResultState();
                                                        CheckImageProcessRetry(ReelHandlerSteps.CheckReelAlignmentAfterBarcodeInput, SubSequenceSteps.Prepare);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion
                    #endregion

                    #region Load a reel from return stage sequence
                    #region Prepare retrun reel load process
                    case ReelHandlerSteps.PrepareToLoadReturnReel:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (IsRobotAvailableToAct(safeposition_))
                                        {   // Clear old barcode data.
                                            FireProductionInformation(true); // Reset barcode contexts
                                                                             // Turn on light.
                                            robotSubStep = SubSequenceSteps.Proceed;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        robotSubStep = SubSequenceSteps.Post;
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.SetReelTypeOfReturnToRobot;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Set reel type of return to robot
                    case ReelHandlerSteps.SetReelTypeOfReturnToRobot:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        switch (robotSequenceManager.RobotActionOrder)
                                        {
                                            case RobotActionOrder.LoadReelFromReturn:
                                                {
                                                    if (SetReelTypeOfCart(currentReelTypeOfReturn, true))
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    else
                                                        throw new PauseException<ErrorCode>(ErrorCode.FailedToSetReelSizeOfReturnToRobot);
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        switch (robotSequenceManager.RobotActionOrder)
                                        {
                                            case RobotActionOrder.LoadReelFromReturn:
                                                {
                                                    if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) || cmddone_)
                                                    {
                                                        FireChagedReelSizeOfCart(currentReelTypeOfCart);
                                                        robotSubStep = SubSequenceSteps.Post;
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.Ready;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Request reel load action to allow
                    case ReelHandlerSteps.MoveToFrontOfReturnStage:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {

                                        if ((robotSequenceManager.IsAtSafePosition || safeposition_) && !robotSequenceManager.IsMoving)
                                        {
                                            if (result_ = robotSequenceManager.MoveToFrontOfReturnStage(currentReelTypeOfReturn))
                                            {
                                                result_ = true;
                                                robotSubStep = SubSequenceSteps.Proceed;
                                            }

                                            if (!result_)
                                            {
                                                if (robotTick == -1)
                                                    robotTick = App.TickCount;
                                                else if (IsRobotActionRetryOver)
                                                {
                                                    throw new PauseException<ErrorCode>(ErrorCode.RobotPositionIsNotMatched);
                                                }
                                                else if (IsOverDelayTime(Config.TimeoutOfRobotAction, robotTick))
                                                {
                                                    robotTick = -1;
                                                    robotActionRetryCount++;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // 30 sec
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtFrontOfReturnStage(currentReelTypeOfReturn))
                                        {
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.Ready;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Approach to reel height check point at return state 
                    case ReelHandlerSteps.ApproachToReelHeightCheckPointAtReturnStage:
                        {
                            if ((App.DigitalIoManager as DigitalIoManager).IsReturnReelExist)
                            {
                                switch (robotSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if ((robotSequenceManager.IsAtSafePosition || safeposition_) && !robotSequenceManager.IsMoving)
                                            {
                                                if (robotSequenceManager.ApproachToReturnStage(currentReelTypeOfReturn, previousReelTypeOfReturn))
                                                {
                                                    result_ = true;
                                                    robotSubStep = SubSequenceSteps.Proceed;
                                                }

                                                if (!result_)
                                                {
                                                    if (robotTick == -1)
                                                        robotTick = App.TickCount;
                                                    else if (IsRobotActionRetryOver)
                                                    {
                                                        throw new PauseException<ErrorCode>(ErrorCode.RobotPositionIsNotMatched);
                                                    }
                                                    else if (IsOverDelayTime(Config.TimeoutOfRobotAction, robotTick))
                                                    {
                                                        robotTick = -1;
                                                        robotActionRetryCount++;
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            if (robotSequenceManager.IsFailure)
                                            {   // 30 sec
                                                throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                            }
                                            else if (robotSequenceManager.IsWaitForOrder &&
                                                robotSequenceManager.IsRobotAtMeasureReelHeightPositionOfReturnStage(currentReelTypeOfReturn))
                                            {
                                                robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {
                                            robotStep = ReelHandlerSteps.Ready;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                if (IsRobotActionDelayOver(1000))
                                {
                                    if ((robotSequenceManager.IsWaitForOrder &&
                                        robotSequenceManager.IsRobotAtMeasureReelHeightPositionOfReturnStage(currentReelTypeOfReturn)) ||
                                        currentReelTypeOfReturn == ReelDiameters.Unknown ||
                                        robotSubStep != SubSequenceSteps.Post)
                                    {   // Force retract robot arm to front of return stage
                                        if (robotSequenceManager.MoveToFrontOfReturnStage(ReelDiameters.ReelDiameter13))
                                        {
                                            robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStage;
                                            currentReelTypeOfReturn = ReelDiameters.Unknown;
                                            Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                        }
                                    }
                                    else
                                    {
                                        throw new PauseException<ErrorCode>(ErrorCode.RobotCommunicationFailure);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Measure reel height of return stage
                    case ReelHandlerSteps.MeasureReelHeightOnReturnStage:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsRobotAtMeasureReelHeightPositionOfReturnStage(currentReelTypeOfReturn) ||
                                            currentReelTypeOfReturn == ReelDiameters.Unknown)
                                        {
                                            if (robotSequenceManager.MeasureReelHeightOfReturnStage(currentReelTypeOfReturn))
                                                robotSubStep = SubSequenceSteps.Proceed;
                                            else
                                                throw new PauseException<ErrorCode>(ErrorCode.FailedToSetReturnReelSize);
                                        }
                                        else
                                        {
                                            // robotTick = Environment.TickCount;
                                            robotTick = -1;
                                            robotStep = ReelHandlerSteps.ApproachToReelHeightCheckPointAtReturnStage;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // 30 sec
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                        {   // Turn on light.
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        visionTick = -1;
                                        robotVisionRetryCount = 0;
                                        robotVisionRetryCycleCount = 0;
                                        robotStep = ReelHandlerSteps.Ready;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Check reel alignment on return stage
                    case ReelHandlerSteps.CheckReelAlignmentOnReturnStage:
                        {
                            if ((App.DigitalIoManager as DigitalIoManager).IsReturnReelExist)
                            {
                                switch (robotSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            switch (Config.ImageProcessorType)
                                            {
                                                case ImageProcessorTypes.Mit:
                                                    {
                                                        switch (visionTick)
                                                        {
                                                            case -1:
                                                                {
                                                                    CompositeVisionManager.SetVisionLight(currentReelTypeOfCart);

                                                                    if (IsVisionProcessDelayOver(delayOfGrapRetry))
                                                                    {
                                                                        visionTick = App.TickCount;
                                                                        CompositeVisionManager.TriggerOn(currentReelTypeOfReturn);
                                                                    }
                                                                }
                                                                break;
                                                            case 0:
                                                                {
                                                                    visionTick = App.TickCount;
                                                                    robotSubStep = SubSequenceSteps.Proceed;
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    if (IsVisionProcessDelayOver(delayOfGrapRetry))
                                                                    {
                                                                        visionTick = 0;
                                                                        VisionManager._CILightOff();
                                                                    }
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case ImageProcessorTypes.TechFloor:
                                                    {
                                                        if (IsOverDelayTime(Model.TimeoutOfRetryTrigger, imageProcessTimeoutTick))
                                                        {
                                                            if (FireTriggerVisionControl(Model.LightLevels.Top,
                                                                Model.TaskSubCategories.ProcessAtOnce,
                                                                currentReelTypeOfReturn,
                                                                0,
                                                                false,
                                                                Model.DelayOfTrigger))
                                                            {
                                                                startCycleTime = DateTime.Now;
                                                                alignmentCoordX = string.Empty;
                                                                alignmentCoordY = string.Empty;
                                                                robotSubStep = SubSequenceSteps.Proceed;
                                                            }
                                                            else
                                                            {
                                                                robotVisionRetryCount++;
                                                            }
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            switch (Config.ImageProcessorType)
                                            {
                                                case ImageProcessorTypes.Mit:
                                                    {
                                                        if (IsVisionProcessDelayOver(delayOfImageProcessing))
                                                        {
                                                            alignmentCoordX = string.Empty;
                                                            alignmentCoordY = string.Empty;

                                                            imageProcessingResult = CompositeVisionManager.GetAdjustmentValue(
                                                                DistanceXOfAlignError,
                                                                DistanceYOfAlignError,
                                                                ref alignmentCoordX,    // Adjustment X
                                                                ref alignmentCoordY);   // Adjustment Y
                                                            robotSubStep = SubSequenceSteps.Post;
                                                        }
                                                    }
                                                    break;
                                                case ImageProcessorTypes.TechFloor:
                                                    {
                                                        robotSubStep = SubSequenceSteps.Post;
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {
                                            switch (Config.ImageProcessorType)
                                            {
                                                case ImageProcessorTypes.Mit:
                                                    {
                                                        switch (imageProcessingResult)
                                                        {
                                                            case ImageProcssingResults.Success:
                                                                {   // Normal case
                                                                    reelTowerQueryRetryCount = 0;
                                                                    Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.RequestToLoadConfirm);
                                                                    robotStep = ReelHandlerSteps.Ready;
                                                                }
                                                                break;
                                                            case ImageProcssingResults.Exception:
                                                                {   // Vision error (Exception)
                                                                    if (robotVisionRetryCycleCount > 0)
                                                                    {
                                                                        if (IsVisionProcessDelayOver(delayOfImageProcessingRetry))
                                                                        {
                                                                            visionTick = -1;
                                                                            robotVisionRetryCount++;
                                                                            robotVisionRetryCycleCount = 0;
                                                                            robotSubStep = SubSequenceSteps.Prepare;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (IsRobotVisionRetryOver)
                                                                        {
                                                                            VisionManager._CILightOff();

                                                                            // Set vision tick variable to retry after reset
                                                                            visionTick = -1;
                                                                            robotVisionRetryCount = 0;
                                                                            robotVisionRetryCycleCount = 0;

                                                                            // Force move to top of work slot
                                                                            // Force move to home to prevent unwanted accident.
                                                                            // Raise vision alarm
                                                                            if (robotSequenceManager.ApproachToReturnStage(currentReelTypeOfReturn, previousReelTypeOfReturn))
                                                                                robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStageByVisionAlignmentFailureToLoadReturn;
                                                                        }
                                                                        else
                                                                        {   // Delay light turn on/off for 1 sec.
                                                                            visionTick = App.TickCount;
                                                                            robotVisionRetryCycleCount++;
                                                                        }
                                                                    }
                                                                }
                                                                break;
                                                            case ImageProcssingResults.Empty:
                                                                {
                                                                    VisionManager._CILightOff();

                                                                    // Move up robot to safe way point.
                                                                    if (robotSequenceManager.ApproachToReturnStage(currentReelTypeOfReturn, previousReelTypeOfReturn))
                                                                    {
                                                                        robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStage;
                                                                        FireShowNotification(Properties.Resources.String_Notification_Return_Stage_Empty);
                                                                    }
                                                                }
                                                                break;
                                                            case ImageProcssingResults.OverScope:
                                                                {
                                                                    if (robotVisionRetryCycleCount > 0)
                                                                    {
                                                                        if (IsRobotActionDelayOver(delayOfImageProcessingRetry))
                                                                        {
                                                                            visionTick = -1;
                                                                            robotVisionRetryCycleCount = 0;

                                                                            VisionManager._CILightOff();
                                                                            robotSubStep = SubSequenceSteps.Prepare;
                                                                        }
                                                                    }
                                                                    else
                                                                    {   // Alignment error. X and Y alignment values are greater than threshold (Specified in text file)
                                                                        if (IsRobotVisionRetryOver)
                                                                        {
                                                                            robotVisionRetryCount = 0;
                                                                            VisionManager._CILightOff();

                                                                            // Force move to top of work slot
                                                                            // Force move to home to prevent unwanted accident.
                                                                            // Raise vision alarm 
                                                                            if (robotSequenceManager.ApproachToReturnStage(currentReelTypeOfReturn, previousReelTypeOfReturn))
                                                                                robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStageByVisionAlignmentFailureToLoadReturn;
                                                                        }
                                                                        else
                                                                        {   // Delay light turn on/off for 1 sec.
                                                                            CompositeVisionManager.SetVisionLight(currentReelTypeOfReturn);
                                                                            robotTick = App.TickCount;
                                                                            robotVisionRetryCycleCount++;
                                                                        }
                                                                    }
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case ImageProcessorTypes.TechFloor:
                                                    {
                                                        if (visionProcessResultState > 0)
                                                        {
                                                            object data = new object();
                                                            int foundqr = 0;
                                                            double matchscale = 0.0;
                                                            double matchscore = 0.0;
                                                            double centerx = -99999.9;
                                                            double centery = -99999.9;
                                                            double radius = -99999.9;
                                                            string foundqrdata = string.Empty;

                                                            // Check reel empty 
                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.UserData_1, ref data))
                                                            {
                                                                if ((bool)data)
                                                                {
                                                                    result_ = true;
                                                                    visionTick = -1;
                                                                    robotVisionRetryCount = 0;
                                                                    robotVisionRetryCycleCount = 0;

                                                                    if (robotSequenceManager.ApproachToReturnStage(currentReelTypeOfReturn, previousReelTypeOfReturn))
                                                                    {
                                                                        robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStage;
                                                                        FireShowNotification(Properties.Resources.String_Notification_Return_Stage_Empty);
                                                                    }
                                                                }
                                                            }

                                                            if (!result_)
                                                            {   // Check reel x, y
                                                                if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.ResultCode, ref data))
                                                                {
                                                                    switch ((Cognex.VisionPro.CogToolResultConstants)data)
                                                                    {
                                                                        case Cognex.VisionPro.CogToolResultConstants.Accept:
                                                                            {
                                                                                if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.MatchScale, ref data))
                                                                                {
                                                                                    if (data != null)
                                                                                        matchscale = (double)data;
                                                                                }

                                                                                if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.MatchScore, ref data))
                                                                                {
                                                                                    if (data != null)
                                                                                        matchscore = (double)data;
                                                                                }

                                                                                if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundPatternCenterX, ref data))
                                                                                {
                                                                                    if (data != null)
                                                                                        centerx = (double)data;
                                                                                }

                                                                                if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundPatternCenterY, ref data))
                                                                                {
                                                                                    if (data != null)
                                                                                        centery = (double)data;
                                                                                }

                                                                                if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundPatternRadius, ref data))
                                                                                {
                                                                                    if (data != null)
                                                                                        radius = (double)data;
                                                                                }

                                                                                if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.UserData_3, ref data))
                                                                                {
                                                                                    if (data != null)
                                                                                        foundqr = (int)data;
                                                                                }

                                                                                if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundQrCode, ref data))
                                                                                {
                                                                                    if (data != null)
                                                                                        foundqrdata = data.ToString();
                                                                                }

                                                                                if (matchscore > 0.4 && Math.Abs(centerx) < DistanceXOfAlignError && Math.Abs(centery) < DistanceYOfAlignError) // Math.Round(matchscale) == 1.0 &&  radius > 100)
                                                                                {
                                                                                    alignmentCoordX = centerx.ToString();
                                                                                    alignmentCoordY = centery.ToString();

                                                                                    result_ = true;
                                                                                    reelTowerQueryRetryCount = 0;
                                                                                    Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.RequestToLoadConfirm);
                                                                                    robotStep = ReelHandlerSteps.Ready;

                                                                                    if (foundqr <= 0 || string.IsNullOrEmpty(foundqrdata))
                                                                                    {   // Required barcode read again
                                                                                        Logger.Trace($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Required barcode read again.");
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    result_ = false;
                                                                                }
                                                                            }
                                                                            break;
                                                                        default:
                                                                            result_ = false;
                                                                            break;
                                                                    }
                                                                }

                                                                if (!result_ || robotStep != ReelHandlerSteps.Ready)
                                                                {   // Vision error (Exception)
                                                                    ResetVisionProcessResultState();
                                                                    CheckImageProcessRetry(ReelHandlerSteps.CheckReelAlignmentOnReturnStage, SubSequenceSteps.Prepare, ReelHandlerSteps.MoveBackToFrontOfReturnStageByVisionAlignmentFailureToLoadReturn);
                                                                }
                                                            }
                                                        }
                                                        else if (IsOverDelayTime(Model.TimeoutOfTotalImageProcess, imageProcessTimeoutTick))
                                                        {
                                                            ResetVisionProcessResultState();
                                                            CheckImageProcessRetry(ReelHandlerSteps.CheckReelAlignmentOnReturnStage, SubSequenceSteps.Prepare, ReelHandlerSteps.MoveBackToFrontOfReturnStageByVisionAlignmentFailureToLoadReturn);
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                robotVisionRetryCount = 0;
                                robotVisionRetryAttemptsCount = 0;

                                if (Config.ImageProcessorType == ImageProcessorTypes.Mit)
                                    VisionManager._CILightOff();

                                // Force move to top of work slot
                                // Force move to home to prevent unwanted accident.
                                // Raise vision alarm 
                                if (robotSequenceManager.ApproachToReturnStage(currentReelTypeOfReturn, previousReelTypeOfReturn))
                                {
                                    robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStageByVisionAlignmentFailureToLoadReturn;
                                    currentReelTypeOfReturn = ReelDiameters.Unknown;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Request to reel load confirm
                    case ReelHandlerSteps.RequestToReturnReelLoadConfirm:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                        {
                                            case TransferMaterialObject.TransferStates.RequestToLoadConfirm:
                                            case TransferMaterialObject.TransferStates.WaitForLoadConfirm:
                                                {   // Send load move request to tower.
                                                    if (ReelTowerManager.SendTowerMessage(ReelTowerCommands.REQUEST_REEL_LOAD_MOVE,
                                                        reelBarcodeContexts, currentLoadReelTowerId))
                                                    {
                                                        materialLoadMoveTick = App.TickCount;
                                                        Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.WaitForLoadConfirm);
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                }
                                                break;
                                            default:    // Abnormal case
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (ReelTowerManager.IsFailure())
                                        {
                                            if (IsReelTowerQueryRetryOver)
                                            {
                                                reelTowerQueryRetryCount = 0;
                                                CancelLoadProcedure();

                                                // Force move to approach way point to prevent unwanted accident.
                                                if (robotSequenceManager.ApproachToReturnStage(currentReelTypeOfReturn, previousReelTypeOfReturn))
                                                    robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStageByResponseTimeoutToLoadReturn;

                                                ReelTowerManager.FireReportRuntimeLog($"Failed to load a reel request confirm by tower ({currentLoadReelTowerId}) retry over.");
                                            }
                                            else
                                            {
                                                // UPDATE : 20190705
                                                if (IsOverDelayTime(ReelTowerManager.TimeoutOfTowerResponse, materialBarcodeTick))
                                                {
                                                    reelTowerQueryRetryCount++;
                                                    robotSubStep = SubSequenceSteps.Prepare;
                                                }
                                            }
                                        }
                                        else if (ReelTowerManager.IsReceivedResponse())
                                        {
                                            if (ReelTowerManager.GetReceivedResponse(ref responseOfReelTower))
                                            {
                                                if (responseOfReelTower.ReturnCode == "0")
                                                {
                                                    Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.ConfirmLoad);
                                                    robotSubStep = SubSequenceSteps.Post;
                                                }
                                                else
                                                {
                                                    CancelLoadProcedure();

                                                    // Force move to approach way point to prevent unwanted accident.
                                                    if (robotSequenceManager.ApproachToReturnStage(currentReelTypeOfReturn, previousReelTypeOfReturn))
                                                    {
                                                        robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStageByReelTowerRefuseToLoadReturn;
                                                        ReelTowerManager.FireReportRuntimeLog($"Failed to load a reel request confirm by tower ({currentLoadReelTowerId}). RETURNCODE={responseOfReelTower.ReturnCode},UID={responseOfReelTower.Uid}");
                                                    }
                                                }
                                            }
                                        }
                                        else if (IsOverDelayTime(ReelTowerManager.TimeoutOfTowerResponse, materialLoadMoveTick))
                                        {
                                            reelTowerQueryRetryCount++;
                                            robotSubStep = SubSequenceSteps.Prepare;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotVisionRetryCount = 0;
                                        robotVisionRetryAttemptsCount = 0;
                                        robotVisionRetryCycleCount = 0;
                                        robotStep = ReelHandlerSteps.Ready;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Move back to front of return stage
                    case ReelHandlerSteps.MoveBackToFrontOfReturnStage:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {   // Need to check robot position
                                            if (robotSequenceManager.IsRobotAtMeasureReelHeightPositionOfReturnStage(currentReelTypeOfReturn) ||
                                                currentReelTypeOfReturn == ReelDiameters.Unknown)
                                            {
                                                if (robotSequenceManager.MoveToFrontOfReturnStage(currentReelTypeOfReturn))
                                                    robotSubStep = SubSequenceSteps.Proceed;
                                            }
                                            else if (robotSequenceManager.IsRobotAtFrontOfReturnStage(currentReelTypeOfReturn))
                                            {
                                                robotSubStep = SubSequenceSteps.Proceed;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // 30 sec
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtFrontOfReturnStage(currentReelTypeOfReturn))
                                        {
                                            switch (Singleton<TransferMaterialObject>.Instance.State)
                                            {
                                                case TransferMaterialObject.TransferStates.RequestToLoadConfirm:
                                                case TransferMaterialObject.TransferStates.WaitForLoadConfirm:
                                                case TransferMaterialObject.TransferStates.None:
                                                    {
                                                        robotSubStep = SubSequenceSteps.Post;
                                                    }
                                                    break;
                                                default:    // Abnormal case
                                                    {
                                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                        robotStep = ReelHandlerSteps.Done;
                                                        throw new PauseException<ErrorCode>(ErrorCode.RobotStateIsNotMatched);
                                                    }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotVisionRetryCount = 0;
                                        robotVisionRetryAttemptsCount = 0;
                                        robotVisionRetryCycleCount = 0;
                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                        robotStep = ReelHandlerSteps.Done;
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.MoveBackToFrontOfReturnStageByVisionAlignmentFailureToLoadReturn:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                        {
                                            case TransferMaterialObject.TransferStates.RequestToLoadConfirm:
                                            case TransferMaterialObject.TransferStates.WaitForLoadConfirm:
                                            case TransferMaterialObject.TransferStates.None:
                                            // 20190705 : When sequence enters this step after barcode input manually transfer state 
                                            // is request to barcode confirm. so added a check for it. Otherwise it never enters 
                                            // this condition and gives error. 
                                            case TransferMaterialObject.TransferStates.ConfirmLoad:
                                            case TransferMaterialObject.TransferStates.RequestToBarcodeConfirm:
                                                {
                                                    if (robotSequenceManager.IsRobotAtMeasureReelHeightPositionOfReturnStage(currentReelTypeOfReturn) ||
                                                        currentReelTypeOfReturn == ReelDiameters.Unknown)
                                                    {
                                                        if (robotSequenceManager.MoveToFrontOfReturnStage(currentReelTypeOfReturn))
                                                        {
                                                            robotSubStep = SubSequenceSteps.Proceed;
                                                            robotVisionRetryAttemptsCount++;
                                                        }
                                                    }
                                                    else if (robotSequenceManager.IsRobotAtFrontOfReturnStage(currentReelTypeOfReturn))
                                                    {
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                }
                                                break;
                                            default:
                                                {   // Need to check robot position
                                                    Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                    throw new PauseException<ErrorCode>(ErrorCode.RobotStateIsNotMatched);
                                                }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // 30 sec
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                                robotSequenceManager.IsRobotAtFrontOfReturnStage(currentReelTypeOfReturn))
                                        {
                                            if (IsRobotVisionRetryAttemptsOver)
                                            {
                                                robotVisionRetryAttemptsCount = 0;
                                                robotVisionRetryCount = 0;
                                                robotVisionRetryCycleCount = 0;
                                                robotStep = ReelHandlerSteps.Done;
                                                Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                throw new PauseException<ErrorCode>(ErrorCode.RobotVisionFailure);
                                            }
                                            else
                                            {
                                                robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotVisionRetryCount = 0;
                                        robotVisionRetryCycleCount = 0;
                                        robotStep = ReelHandlerSteps.Done;//Ready;
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.MoveBackToFrontOfReturnStageByResponseTimeoutToLoadReturn:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {   // Need to check robot position
                                            switch (Singleton<TransferMaterialObject>.Instance.State)
                                            {
                                                case TransferMaterialObject.TransferStates.RequestToLoadConfirm:
                                                case TransferMaterialObject.TransferStates.WaitForLoadConfirm:
                                                case TransferMaterialObject.TransferStates.None:
                                                    {
                                                        if (robotSequenceManager.IsRobotAtMeasureReelHeightPositionOfReturnStage(currentReelTypeOfReturn) ||
                                                            currentReelTypeOfReturn == ReelDiameters.Unknown)
                                                        {
                                                            if (robotSequenceManager.MoveToFrontOfReturnStage(currentReelTypeOfReturn))
                                                                robotSubStep = SubSequenceSteps.Proceed;
                                                        }
                                                    }
                                                    break;
                                                default:    // Abnormal case
                                                    {
                                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                        throw new PauseException<ErrorCode>(ErrorCode.RobotStateIsNotMatched);
                                                    }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // 30 sec
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtFrontOfReturnStage(currentReelTypeOfReturn))
                                        {
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotVisionRetryCount = 0;
                                        robotVisionRetryAttemptsCount = 0;
                                        robotVisionRetryCycleCount = 0;
                                        robotStep = ReelHandlerSteps.Done;
                                        if (Config.EnableRejectReel)
                                        {

                                        }
                                        else
                                            throw new PauseException<ErrorCode>((barcodeConfirmState == BarcodeConfirmStates.Reject) ? ErrorCode.ReelBarcodeIsNotUnique : ErrorCode.ReelTowerResponseTimeout);
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.MoveBackToFrontOfReturnStageByReelTowerRefuseToLoadReturn:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {   // Need to check robot position
                                            switch (Singleton<TransferMaterialObject>.Instance.State)
                                            {
                                                case TransferMaterialObject.TransferStates.RequestToLoadConfirm:
                                                case TransferMaterialObject.TransferStates.WaitForLoadConfirm:
                                                case TransferMaterialObject.TransferStates.None:
                                                    {
                                                        if (robotSequenceManager.IsRobotAtMeasureReelHeightPositionOfReturnStage(currentReelTypeOfReturn) ||
                                                            currentReelTypeOfReturn == ReelDiameters.Unknown)
                                                        {
                                                            if (robotSequenceManager.MoveToFrontOfReturnStage(currentReelTypeOfReturn))
                                                            {
                                                                currentReelTypeOfReturn = ReelDiameters.Unknown;
                                                                Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                                robotSubStep = SubSequenceSteps.Proceed;
                                                            }
                                                        }
                                                    }
                                                    break;
                                                default:    // Abnormal case
                                                    {
                                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                        throw new PauseException<ErrorCode>(ErrorCode.RobotStateIsNotMatched);
                                                    }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // 30 sec
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtFrontOfReturnStage(currentReelTypeOfReturn))
                                        {
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStage;
                                        FireShowNotification(Properties.Resources.String_Notification_Failed_To_Load_Return);
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.MoveBackToFrontOfReturnStageByCancelBarcodeInputToLoadReturn:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (robotSequenceManager.IsRobotAtMeasureReelHeightPositionOfReturnStage(currentReelTypeOfReturn) ||
                                                currentReelTypeOfReturn == ReelDiameters.Unknown)
                                            {
                                                if (robotSequenceManager.MoveToFrontOfReturnStage(currentReelTypeOfReturn))
                                                    robotSubStep = SubSequenceSteps.Proceed;
                                            }
                                            else if (robotSequenceManager.IsRobotAtFrontOfReturnStage(currentReelTypeOfReturn))
                                            {
                                                robotSubStep = SubSequenceSteps.Proceed;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // 30 sec
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtFrontOfReturnStage(currentReelTypeOfReturn))
                                        {
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.Done;
                                    }
                                    break;
                            }
                        }
                        break;
                    case ReelHandlerSteps.MoveBackToFrontOfReturnStageByReelPickupFailureToLoadReturn:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (robotSequenceManager.IsWaitForOrder)
                                        {
                                            if (robotSequenceManager.IsRobotAtMeasureReelHeightPositionOfReturnStage(currentReelTypeOfReturn) ||
                                                currentReelTypeOfReturn == ReelDiameters.Unknown ||
                                                robotSequenceManager.IsAtSafePosition)
                                            {
                                                if (robotSequenceManager.MoveToFrontOfReturnStage(currentReelTypeOfReturn))
                                                    robotSubStep = SubSequenceSteps.Proceed;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {   // 30 sec
                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtFrontOfReturnStage(currentReelTypeOfReturn))
                                        {
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        // Set reject reel mode.
                                        if (Config.EnableRejectReel)
                                        {   // MoveBackToHomeByReelPickupFailureToLoad
                                            CancelLoadProcedure();
                                            Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.PrepareToRejectReturnReel);
                                            barcodeConfirmState = BarcodeConfirmStates.Reject;
                                            robotStep = ReelHandlerSteps.Ready;
                                        }
                                        else
                                            robotStep = ReelHandlerSteps.Done;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Request to confirm load reel barcode
                    case ReelHandlerSteps.RequestToConfirmLoadReelBarcodeOfReturnStage:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                        {
                                            case TransferMaterialObject.TransferStates.CompleteLoad:
                                            case TransferMaterialObject.TransferStates.RequestToBarcodeConfirm:
                                            case TransferMaterialObject.TransferStates.WaitForBarcodeConfirm:
                                                {
                                                    reelBarcodeContexts.SetReelInformation(LoadMaterialTypes.Return, currentReelTypeOfReturn);
                                                    if (ReelTowerManager.SendTowerMessage(ReelTowerCommands.REQUEST_BARCODEINFO_CONFIRM,
                                                        reelBarcodeContexts, currentLoadReelTowerId))
                                                    {
                                                        barcodeConfirmState = BarcodeConfirmStates.Prepared;
                                                        materialBarcodeTick = App.TickCount;
                                                        Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.WaitForBarcodeConfirm);
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                }
                                                break;
                                            default:    // Abnormal case
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (ReelTowerManager.IsFailure())
                                        {
                                            if (IsReelTowerQueryRetryOver)
                                            {
                                                reelTowerQueryRetryCount = 0;
                                                CancelLoadProcedure();

                                                // Force move to approach way point to prevent unwanted accident.
                                                if (robotSequenceManager.ApproachToReturnStage(currentReelTypeOfReturn, previousReelTypeOfReturn))
                                                    robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStageByResponseTimeoutToLoadReturn;
                                            }
                                            else
                                            {
                                                // UPDATE : 20190705
                                                if (IsOverDelayTime(ReelTowerManager.TimeoutOfTowerResponse, materialBarcodeTick))
                                                {
                                                    reelTowerQueryRetryCount++;
                                                    robotSubStep = SubSequenceSteps.Prepare;
                                                }
                                            }
                                        }
                                        else if (ReelTowerManager.IsReceivedResponse())
                                        {
                                            if (ReelTowerManager.GetReceivedResponse(ref responseOfReelTower))
                                            {
                                                if (responseOfReelTower.ReturnCode == "0" &&
                                                    ReelTowerManager.IsValidMaterialData(currentLoadReelState, responseOfReelTower, reelBarcodeContexts))
                                                {
                                                    barcodeConfirmState = BarcodeConfirmStates.Confirmed;
                                                    Singleton<TransferMaterialObject>.Instance.SetStateWithRoute(TransferMaterialObject.TransferStates.ConfirmedBarcodeOfReturn, currentReelTypeOfReturn, currentLoadReelTower);
                                                    robotStep = ReelHandlerSteps.Ready;
                                                }
                                                else
                                                {
                                                    if (responseOfReelTower.ReturnMessage.Contains("DUPLICATE ERROR"))
                                                    {
                                                        SameReelCheck = true;
                                                    }
                                                    barcodeConfirmState = BarcodeConfirmStates.Reject;
                                                    robotTick = App.TickCount;
                                                    robotSubStep = SubSequenceSteps.Post;
                                                }

                                                ReelTowerManager.FireReportRuntimeLog($"Confirmed to load a reel request by tower ({currentLoadReelTowerId}). RETURNCODE={responseOfReelTower.ReturnCode},UID={responseOfReelTower.Uid}");
                                            }
                                        }
                                        else
                                        {
                                            if (IsOverDelayTime(ReelTowerManager.TimeoutOfTowerResponse, materialBarcodeTick))
                                            {
                                                if (IsReelTowerQueryRetryOver)
                                                {
                                                    robotTick = App.TickCount;
                                                    robotSubStep = SubSequenceSteps.Post;
                                                    ReelTowerManager.FireReportRuntimeLog($"Failed to load a reel request confirm by tower ({currentLoadReelTowerId}). RETURNCODE={responseOfReelTower.ReturnCode},UID={responseOfReelTower.Uid}");
                                                }
                                                else
                                                {
                                                    reelTowerQueryRetryCount++;
                                                    robotSubStep = SubSequenceSteps.Prepare;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        if (IsRobotActionDelayOver(intervalReelTowerStatePolling * 2))
                                        {   // UPDATED: 20200727 (Marcus)
                                            // Set reject reel mode.
                                            if (Config.EnableRejectReel)
                                            {
                                                if (IsRobotVisionRetryOver)
                                                {
                                                    CancelLoadProcedure();
                                                    Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.PrepareToRejectReturnReel);
                                                }

                                                robotVisionRetryCount++;
                                                robotStep = ReelHandlerSteps.CheckReelAlignmentAfterBarcodeInput;
                                            }
                                            else
                                            {
                                                reelTowerQueryRetryCount = 0;
                                                CancelLoadProcedure();

                                                // Force move to approach way point to prevent unwanted accident.
                                                if (robotSequenceManager.ApproachToReturnStage(currentReelTypeOfReturn, previousReelTypeOfReturn))
                                                    robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStageByResponseTimeoutToLoadReturn;
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Adjust reel pick up position then pickup
                    case ReelHandlerSteps.AdjustPositionAndPickupReelOfReturnStage:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        robotSequenceErrorCode = ErrorCode.None;

                                        if (robotSequenceManager.HasAReel)
                                        {
                                            if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                            {
                                                if (result_ &&
                                                    Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.LoadReturn &&
                                                    Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.ConfirmedBarcodeOfReturn)
                                                {
                                                    robotVisionRetryCount = 0;
                                                    robotVisionRetryAttemptsCount = 0;
                                                    reelTowerQueryRetryCount = 0;
                                                    robotStep = ReelHandlerSteps.Ready;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                            {
                                                case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                case TransferMaterialObject.TransferModes.Reject:
                                                case TransferMaterialObject.TransferModes.PrepareToLoadReturn:
                                                case TransferMaterialObject.TransferModes.None:
                                                    {
                                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                                        {
                                                            case TransferMaterialObject.TransferStates.RequestToBarcodeConfirm:
                                                            case TransferMaterialObject.TransferStates.WaitForBarcodeConfirm:
                                                            case TransferMaterialObject.TransferStates.ConfirmedBarcodeOfReturn:
                                                                {   // Reject reel
                                                                    result_ = true;
                                                                }
                                                                break;
                                                            default:
                                                                result_ = false;
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case TransferMaterialObject.TransferModes.Load:
                                                case TransferMaterialObject.TransferModes.LoadReturn:
                                                    {
                                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                                        {
                                                            case TransferMaterialObject.TransferStates.ConfirmedBarcodeOfReturn:
                                                                {   // After success alignment and barcode reading.
                                                                    // Align command.
                                                                    result_ = true;
                                                                }
                                                                break;
                                                            default:
                                                                result_ = false;
                                                                break;
                                                        }
                                                    }
                                                    break;
                                            }

                                            if (result_)
                                            {
                                                if (robotSequenceManager.PickupReelFromReturn(currentReelTypeOfReturn, alignmentCoordX, alignmentCoordY, alignmentCoordZ))
                                                    robotSubStep = SubSequenceSteps.Proceed;
                                            }
                                            else
                                            {   // Failure process
                                                robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                                robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {
                                            robotSequenceErrorCode = ErrorCode.RobotMoveTimeout;
                                            robotInitializeSubStep = SubSequenceSteps.Post;
                                        }
                                        else if (robotSequenceManager.IsWaitForOrder &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                        {
                                            if (cmddone_)
                                            {
                                                if (robotSequenceManager.HasAReel)
                                                {   // Success
                                                    robotVisionRetryCount = 0;
                                                    robotVisionRetryAttemptsCount = 0;
                                                    reelTowerQueryRetryCount = 0;
                                                    robotStep = ReelHandlerSteps.Ready;
                                                }
                                                else
                                                {   // Failure (Load reel pick up failure)
                                                    robotVisionRetryCount = 0;

                                                    if (robotSequenceManager.FailureCode != RobotActionFailures.None)
                                                    {   // Failure process
                                                        reelTowerQueryRetryCount = 0;
                                                        CancelLoadProcedure();

                                                        // Force move to approach way point to prevent unwanted accident.
                                                        if (robotSequenceManager.IsAtSafePosition)
                                                        {
                                                            robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStageByReelPickupFailureToLoadReturn;
                                                        }
                                                        else
                                                        {
                                                            if (robotSequenceManager.ApproachToReturnStage(currentReelTypeOfReturn, previousReelTypeOfReturn))
                                                                robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStageByReelPickupFailureToLoadReturn;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {   // Failure process
                                        CancelLoadProcedure();
                                        throw new PauseException<ErrorCode>(robotSequenceErrorCode);
                                    }
                            }
                        }
                        break;
                    #endregion
                    #endregion

                    #region Unload a reel from tower sequence
                    #region Prepare tower reel unload process
                    case ReelHandlerSteps.PrepareToUnloadTowerReel:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        robotSequenceErrorCode = ErrorCode.None;
                                        Logger.Trace($"robotSequenceManager.IsReadyToUnloadReel :{robotSequenceManager.IsReadyToUnloadReel } \n" +
                                            $"robotSequenceManager.IsWaitForOrder: {robotSequenceManager.IsWaitForOrder} \n" +
                                            $"robotSequenceManager.HasAReel: {robotSequenceManager.HasAReel}\n " +
                                            $"Transfer Mode : {Singleton<TransferMaterialObject>.Instance.Mode }\n ");

                                        if (robotSequenceManager.IsReadyToUnloadReel && robotSequenceManager.IsWaitForOrder && !robotSequenceManager.HasAReel &&
                                            (Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.Unload ||
                                            Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.PrepareToUnload ||
                                            Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.None))
                                        {
                                            switch (currentUnloadReelState.Index)
                                            {
                                                default:
                                                    {   // Abnormal case
                                                        throw new PauseException<ErrorCode>(ErrorCode.UnloadReelTowerIdIsNotValid);
                                                    }
                                                case 1:
                                                case 2:
                                                case 3:
                                                case 4:
                                                    {   // Clear old barcode data.
                                                        reelBarcodeContexts.SetName(currentUnloadReelState.Uid);
                                                        FireProductionInformation(); // TF solution

                                                        Singleton<MaterialPackageManager>.Instance.ExportLatestReceivedMaterialPackage();
                                                        materialInfoUpdateTick = App.TickCount;

                                                        if (ReelTowerManager.SendTowerMessage(ReelTowerCommands.REQUEST_REEL_UNLOAD_MOVE, 
                                                            reelBarcodeContexts, currentUnloadReelState.Name, true, currentUnloadReelState.ReturnCode))
                                                        {
                                                            materialBarcodeTick = 0;
                                                            robotSubStep        = SubSequenceSteps.Proceed;
                                                            Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.PrepareToUnload);
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (IsOverMaterialInfoUpdateDelay(delayOfMaterialPackageUpdate))
                                        {
                                            if (ReelTowerManager.IsFailure())
                                            {
                                                if (IsReelTowerQueryRetryOver)
                                                {   // Throw the clamped reel to buffer stage.
                                                    reelTowerQueryRetryCount = 0;
                                                    throw new PauseException<ErrorCode>(ErrorCode.ReelTowerResponseTimeout);
                                                }
                                                else
                                                {
                                                    reelTowerQueryRetryCount++;
                                                    robotSubStep = SubSequenceSteps.Prepare;
                                                }
                                            }
                                            else if (ReelTowerManager.IsReceivedResponse())
                                            {
                                                if (ReelTowerManager.GetReceivedResponse(ref res_))
                                                {   // UPDATED: 20200408 (Marcus)
                                                    // Added reject reel handling sequence.
                                                    switch (res_)
                                                    {
                                                        default:
                                                            {   // Tower is not ready.
                                                                // Maybe we need to notify.
                                                                // Endless wait
                                                                robotSubStep = SubSequenceSteps.Prepare;

                                                                if (Config.EnableRejectReel && res_ == "-2")
                                                                {
                                                                    // Reject reel
                                                                    // Move to reject stage
                                                                    barcodeConfirmState = BarcodeConfirmStates.Reject;
                                                                    robotSubStep = SubSequenceSteps.Post;
                                                                }
                                                                else
                                                                    robotStep = ReelHandlerSteps.Done;
                                                            }
                                                            break;
                                                        case "0":
                                                            {   // Try to pick up the reel from tower port.
                                                                robotSubStep = SubSequenceSteps.Post;
                                                            }
                                                            break;
                                                    }
                                                }

                                                ReelTowerManager.FireReportRuntimeLog($"[Tower Recv] State Received from Tower {currentUnloadReelState.Index} : {res_}");
                                            }
                                            else if (IsReelTowerQueryRetryOver)
                                            {   // Throw the clamped reel to buffer stage.
                                                reelTowerQueryRetryCount = 0;
                                                throw new PauseException<ErrorCode>(ErrorCode.ReelTowerResponseTimeout);
                                            }
                                            else
                                            {
                                                if (materialUnloadMoveTick == 0)
                                                {
                                                    materialUnloadMoveTick = App.TickCount;
                                                }
                                                else if (IsOverDelayTime(ReelTowerManager.TimeoutOfTowerResponse, materialUnloadMoveTick))
                                                {
                                                    materialUnloadMoveTick = 0;
                                                    reelTowerQueryRetryCount++;
                                                    robotSubStep = SubSequenceSteps.Prepare;
                                                    FireShowNotification(Properties.Resources.String_FormMain_Notification_WaitFor_Reply);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.MoveToFrontOfUnloadTower;
                                        notificationTick = 0;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Move to front of Unload Tower
                    case ReelHandlerSteps.MoveToFrontOfUnloadTower:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        robotSequenceErrorCode = ErrorCode.None;
                                        cmddone_ = robotSequenceManager.IsRobotAtWayPointByCommand(ref safeposition_);

                                        if (!robotSequenceManager.HasAReel && !robotSequenceManager.IsMoving &&
                                            cmddone_ && (robotSequenceManager.IsAtSafePosition || safeposition_) &&
                                            (Singleton<TransferMaterialObject>.Instance.Mode >= TransferMaterialObject.TransferModes.PrepareToUnload))
                                        {
                                            if (currentUnloadReelState.PendingData != null)
                                            {
                                                Singleton<MaterialPackageManager>.Instance.UpdateMaterialPackage($"{currentUnloadReelState.PendingData.Name}", ReelUnloadReportStates.Run);
                                                MaterialPackage pkg_ = Singleton<MaterialPackageManager>.Instance.GetFirstMaterialPickList();

                                                if (pkg_ != null && barcodeConfirmState != BarcodeConfirmStates.Reject && pkg_.PickState == ReelUnloadReportStates.Ready)
                                                {
                                                    pkg_.PickState = ReelUnloadReportStates.Run;
													SetPickList(pkg_);

                                                    lock (reelsOfOutputStages)
                                                    {
                                                        if (!reelsOfOutputStages.ContainsKey(pkg_.OutputPort - 1))
                                                            reelsOfOutputStages.Add(pkg_.OutputPort - 1, new MaterialPackage(pkg_));
                                                        else
                                                            reelsOfOutputStages[pkg_.OutputPort - 1].CopyFrom(pkg_);
                                                    }
                                                }
                                            }

                                            if (robotSequenceManager.MoveRobotToFrontOfUnloadTower(currentUnloadReelTower))
                                            {
                                                robotSubStep = SubSequenceSteps.Proceed;
                                                Singleton<TransferMaterialObject>.Instance.SetStateWithRoute(TransferMaterialObject.TransferStates.VerifiedUnload, currentUnloadReelState);
                                            }
                                            else
                                            {
                                                robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                                robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {
                                            robotSequenceErrorCode = ErrorCode.RobotMoveTimeout;
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                        else if (robotSequenceManager.IsConnected && robotSequenceManager.IsReceived &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                        {
                                            robotStep = ReelHandlerSteps.Ready;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                        throw new PauseException<ErrorCode>(robotSequenceErrorCode);
                                    }
                            }
                        }
                        break;
                    #endregion

                    #region Take the Reel From Unload Tower
                    case ReelHandlerSteps.TakeReelFromUnloadTower:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        robotSequenceErrorCode = ErrorCode.None;

                                        if (!robotSequenceManager.HasAReel && robotSequenceManager.IsReadyToUnloadReel &&
                                            robotSequenceManager.IsAtSafePosition && !robotSequenceManager.IsMoving &&
                                                (Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.Unload))
                                        {
                                            robotTick = App.TickCount;

                                            if (robotSequenceManager.TakeReelFromTowerPort(currentUnloadReelTower))
                                            {
                                                robotSubStep = SubSequenceSteps.Proceed;
                                            }
                                            else
                                            {
                                                robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                                robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                        else
                                        {
                                            robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {   // UPDATED: 20200408 (Marcus)
                                        // Fast check robot moving. (Delay: 100 ms)
                                        if (IsRobotActionDelayOver(Convert.ToInt32(timeoutRobotCommunication * 0.01)) && !robotSequenceManager.IsMoving)
                                        {
                                            if (robotSequenceManager.IsFailure)
                                            {
                                                robotSequenceErrorCode = ErrorCode.RobotMoveTimeout;
                                                robotSubStep = SubSequenceSteps.Post;
                                            }
                                            else if (robotSequenceManager.IsWaitForOrder &&
                                                robotSequenceManager.IsAtSafePosition &&
                                                robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                            {
                                                if (robotSequenceManager.HasAReel)
                                                {
                                                    Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.TakenMaterial);

                                                    // UPDATED: 20200408 (Marcus)
                                                    // Set reject reel mode.
                                                    if (Config.EnableRejectReel && barcodeConfirmState == BarcodeConfirmStates.Reject)
                                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel);

                                                    robotStep = ReelHandlerSteps.Ready;
                                                }
                                                else
                                                {
                                                    if (robotSequenceManager.MoveBackToFrontOfUnloadTower(currentUnloadReelTower))
                                                    {
                                                        if (Config.EnableRejectReel && barcodeConfirmState == BarcodeConfirmStates.Reject)
                                                            robotSequenceErrorCode = ErrorCode.FailedToPickRejectReel;
                                                        else
                                                            robotSequenceErrorCode = ErrorCode.FailedToPickUnloadReel;

                                                        robotSubStep = SubSequenceSteps.Post;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {   // Failure process  
                                        if (robotSequenceManager.IsWaitForOrder && robotSequenceManager.IsAtSafePosition)
                                        {
                                            if (robotSequenceErrorCode == ErrorCode.FailedToPickRejectReel)
                                                robotStep = ReelHandlerSteps.MoveBackToFrontOfTowerByReelPickupFailureToReject;
                                            else
                                                robotStep = ReelHandlerSteps.MoveBackToFrontOfTowerByReelPickupFailureToUnload;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Request to unload reel assignment
                    case ReelHandlerSteps.RequestToUnloadReelAssignment:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                        {
                                            case TransferMaterialObject.TransferStates.RequestToUnloadAssignment:
                                                {
                                                    if (ReelTowerManager.SendTowerMessage(ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN, reelBarcodeContexts, currentUnloadReelState.Name))
                                                    {
                                                        materialBarcodeTick = 0;
                                                        Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.WaitForUnloadAssignment);
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                }
                                                break;
                                            case TransferMaterialObject.TransferStates.WaitForUnloadAssignment:
                                                {
                                                    robotSubStep = SubSequenceSteps.Proceed;
                                                }
                                                break;
                                            default: // Abnormal case.
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.CompleteUnload);
                                        Singleton<MaterialPackageManager>.Instance.UpdateMaterialPackage($"{currentUnloadReelState.PendingData.Name}", ReelUnloadReportStates.Complete);

                                        switch (robotSequenceManager.RobotActionOrder)
                                        {
                                            case RobotActionOrder.UnloadReelFromTower:
                                                totalUnloadReelCountValue++;
                                                break;
                                            default: // Abnormal case
                                                break;
                                        }

                                        // A reel was loaded exactly.
                                        robotSubStep = SubSequenceSteps.Post;
                                        ReelTowerManager.FireReportRuntimeLog($"[Tower Recv] Ignore tower response check. (INDEX={currentUnloadReelState.Index},TOWER={currentUnloadReelState.Name},MAKER={currentUnloadReelState.Maker},SID={currentUnloadReelState.Sid},UID={currentUnloadReelState.Uid},QTY={currentUnloadReelState.Qty})");
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.MoveToFrontOfOutputStage;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Move to front of Output Stage
                    case ReelHandlerSteps.MoveToFrontOfOutputStage:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        robotSequenceErrorCode = ErrorCode.None;

                                        if (robotSequenceManager.HasAReel && !robotSequenceManager.IsMoving &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) && cmddone_ &&
                                            (Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.Unload))
                                        {
                                            if (robotSequenceManager.MoveRobotToFrontOfUnloadOutputStage(currentUnloadReelState.OutputStageIndex))
                                            {
                                                robotSubStep = SubSequenceSteps.Proceed;
                                            }
                                            else
                                            {
                                                robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                                robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {
                                            robotSequenceErrorCode = ErrorCode.RobotMoveTimeout;
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                        else if (robotSequenceManager.IsConnected && robotSequenceManager.IsReceived &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                        {
                                            robotStep = ReelHandlerSteps.Ready;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                        throw new PauseException<ErrorCode>(robotSequenceErrorCode);
                                    }
                            }
                        }
                        break;
                    #endregion

                    #region Check up output stage state
                    case ReelHandlerSteps.CheckUpStateOfOutputStage:
                        {   // UPDATED: 20200424 (Marcus)
                            // Not use pusher and stopper.
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        switch (currentUnloadReelState.OutputStageIndex)
                                        {
                                            case 1:
                                                {
                                                    if ((App.DigitalIoManager as DigitalIoManager).OutputStage1Exist && reelsOfOutputStages.ContainsKey(0) && reelsOfOutputStages[0].PickState == ReelUnloadReportStates.Complete)
                                                        outputStage1Full = true;
                                                    else // if (!(App.DigitalIoManager as DigitalIoManager).ForwardPusher1 && (App.DigitalIoManager as DigitalIoManager).BackwardPusher1)
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                }
                                                break;
                                            case 2:
                                                {
                                                    if ((App.DigitalIoManager as DigitalIoManager).OutputStage2Exist && reelsOfOutputStages.ContainsKey(1) && reelsOfOutputStages[1].PickState == ReelUnloadReportStates.Complete)
                                                        outputStage2Full = true;
                                                    else // if (!(App.DigitalIoManager as DigitalIoManager).ForwardPusher2 && (App.DigitalIoManager as DigitalIoManager).BackwardPusher2)
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                }
                                                break;
                                            case 3:
                                                {
                                                    if ((App.DigitalIoManager as DigitalIoManager).OutputStage3Exist && reelsOfOutputStages.ContainsKey(2) && reelsOfOutputStages[2].PickState == ReelUnloadReportStates.Complete)
                                                        outputStage3Full = true;
                                                    else // if (!(App.DigitalIoManager as DigitalIoManager).ForwardPusher3 && (App.DigitalIoManager as DigitalIoManager).BackwardPusher3)
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                }
                                                break;
                                            case 4:
                                                {
                                                    if ((App.DigitalIoManager as DigitalIoManager).OutputStage4Exist && reelsOfOutputStages.ContainsKey(3) && reelsOfOutputStages[3].PickState == ReelUnloadReportStates.Complete)
                                                        outputStage4Full = true;
                                                    else // if (!(App.DigitalIoManager as DigitalIoManager).ForwardPusher1 && (App.DigitalIoManager as DigitalIoManager).BackwardPusher1)
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                }
                                                break;
                                            case 5:
                                                {
                                                    if ((App.DigitalIoManager as DigitalIoManager).OutputStage5Exist && reelsOfOutputStages.ContainsKey(4) && reelsOfOutputStages[4].PickState == ReelUnloadReportStates.Complete)
                                                        outputStage5Full = true;
                                                    else // if (!(App.DigitalIoManager as DigitalIoManager).ForwardPusher2 && (App.DigitalIoManager as DigitalIoManager).BackwardPusher2)
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                }
                                                break;
                                            case 6:
                                                {
                                                    if ((App.DigitalIoManager as DigitalIoManager).OutputStage6Exist && reelsOfOutputStages.ContainsKey(5) && reelsOfOutputStages[5].PickState == ReelUnloadReportStates.Complete)
                                                        outputStage6Full = true;
                                                    else // if (!(App.DigitalIoManager as DigitalIoManager).ForwardPusher3 && (App.DigitalIoManager as DigitalIoManager).BackwardPusher3)
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.ApproachOutputStage;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Approach Unload Output Stage
                    case ReelHandlerSteps.ApproachOutputStage:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        robotSequenceErrorCode = ErrorCode.None;

                                        if (robotSequenceManager.HasAReel && !robotSequenceManager.IsMoving &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) && cmddone_ &&
                                            (Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.Unload))
                                        {
                                            if (robotSequenceManager.ApproachOutputStage(currentUnloadReelState.OutputStageIndex))
                                            {
                                                robotSubStep = SubSequenceSteps.Proceed;
                                            }
                                            else
                                            {
                                                robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                                robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {
                                            robotSequenceErrorCode = ErrorCode.RobotMoveTimeout;
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                        else if (robotSequenceManager.IsConnected && robotSequenceManager.IsReceived &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                        {
                                            switch (currentUnloadReelState.OutputStageIndex)
                                            {
                                                case 1:
                                                case 2:
                                                case 3:
                                                case 4:
                                                case 5:
                                                case 6:
                                                    {
                                                        if (reelsOfOutputStages.ContainsKey(currentUnloadReelState.OutputStageIndex - 1))
                                                        {
                                                            reelsOfOutputStages[currentUnloadReelState.OutputStageIndex - 1].SetMaterial(currentUnloadReelState.Uid, ReelUnloadReportStates.Complete);

                                                            if (reelsOfOutputStages[currentUnloadReelState.OutputStageIndex - 1].IsFinished())
                                                                reelsOfOutputStages[currentUnloadReelState.OutputStageIndex - 1].PickState = ReelUnloadReportStates.Complete;
                                                        }
                                                    }
                                                    break;
                                            }

                                            robotStep = ReelHandlerSteps.Ready;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                        throw new PauseException<ErrorCode>(robotSequenceErrorCode);
                                    }
                            }
                        }
                        break;
                    #endregion

                    #region Put Reel into Output Stage
                    case ReelHandlerSteps.PutReelIntoOutputStage:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        robotSequenceErrorCode = ErrorCode.None;

                                        if (robotSequenceManager.HasAReel && !robotSequenceManager.IsMoving &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) && cmddone_ &&
                                            (Singleton<TransferMaterialObject>.Instance.Mode == TransferMaterialObject.TransferModes.Unload))
                                        {
                                            if (robotSequenceManager.PutReelIntoOutputStage(currentUnloadReelState.OutputStageIndex))
                                            {
                                                robotSubStep = SubSequenceSteps.Proceed;
                                            }
                                            else
                                            {
                                                robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                                robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {
                                            robotSequenceErrorCode = ErrorCode.RobotMoveTimeout;
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                        else if (robotSequenceManager.IsConnected && robotSequenceManager.IsReceived &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                        {
                                            Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.RequestToUnloadAssignment);
                                            robotStep = ReelHandlerSteps.Ready;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                        throw new PauseException<ErrorCode>(robotSequenceErrorCode);
                                    }
                            }
                        }
                        break;
                    #endregion

                    #region Confirm unload reel assignment
                    case ReelHandlerSteps.RequestToConfirmUnloadedReelAssign:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                        {
                                            case TransferMaterialObject.TransferStates.RequestToUnloadAssignment:
                                                {
                                                    if (ReelTowerManager.SendTowerMessage(ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN, reelBarcodeContexts, currentUnloadReelState.Name))
                                                    {
                                                        materialBarcodeTick = 0;
                                                        Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.WaitForUnloadAssignment);
                                                        robotSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                }
                                                break;
                                            case TransferMaterialObject.TransferStates.WaitForUnloadAssignment:
                                                {
                                                    reelTowerQueryRetryCount = 0;
                                                    robotSubStep = SubSequenceSteps.Proceed;
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.CompleteUnload);
                                        Singleton<MaterialPackageManager>.Instance.UpdateMaterialPackage($"{currentUnloadReelState.PendingData.Name}", ReelUnloadReportStates.Complete);

                                        switch (robotSequenceManager.RobotActionOrder)
                                        {
                                            case RobotActionOrder.UnloadReelFromTower:
                                                totalUnloadReelCountValue++;
                                                break;
                                            default: // Abnormal case
                                                break;
                                        }

                                        // A reel was loaded exactly.
                                        robotSubStep = SubSequenceSteps.Post;
                                        ReelTowerManager.FireReportRuntimeLog($"[Tower Recv] Ignore tower response check. (INDEX={currentUnloadReelState.Index},TOWER={currentUnloadReelState.Name},MAKER={currentUnloadReelState.Maker},SID={currentUnloadReelState.Sid},UID={currentUnloadReelState.Uid},QTY={currentUnloadReelState.Qty})");
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.CompletedToUnloadReelFromTower;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Complete to unload reel from tower
                    case ReelHandlerSteps.CompletedToUnloadReelFromTower:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (!robotSequenceManager.HasAReel)
                                        {
                                            switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                            {
                                                case TransferMaterialObject.TransferModes.None:
                                                case TransferMaterialObject.TransferModes.Unload:
                                                    {
                                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                                        {
                                                            case TransferMaterialObject.TransferStates.None:
                                                            case TransferMaterialObject.TransferStates.VerifiedUnload:
                                                            case TransferMaterialObject.TransferStates.TakenMaterial:
                                                            case TransferMaterialObject.TransferStates.CompleteUnload:
                                                                {
                                                                    reelTowerQueryRetryCount = 0;
                                                                    Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.None);
                                                                    unloadReelReportTick = App.TickCount;
                                                                    robotSubStep = SubSequenceSteps.Proceed;
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (IsUnloadReelStateUpdateDelayOver(delayOfUnloadReelStateUpdate))
                                            robotSubStep = SubSequenceSteps.Post;
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.Done;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion
                    #endregion

                    #region Reject a reel during load and unload
                    #region Proceed unload reel pickup failure
                    case ReelHandlerSteps.MoveBackToFrontOfTowerByReelPickupFailureToUnload:
                    case ReelHandlerSteps.MoveBackToFrontOfTowerByReelPickupFailureToReject:
                        {   // UPDATED: 20200408 (Marcus)
                            // Have to show failure information.
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (ReelTowerManager.SendTowerMessage(ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN,
                                            reelBarcodeContexts, currentUnloadReelState.Name))
                                        {
                                            materialBarcodeTick = 0;
                                            robotSubStep = SubSequenceSteps.Proceed;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                        {
                                            case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                                            case TransferMaterialObject.TransferModes.Reject:
                                                {
                                                    log_ = "Failed to pick up reject reel tower";
                                                    robotSequenceErrorCode = ErrorCode.FailedToPickRejectReel;
                                                }
                                                break;
                                            default:
                                                {
                                                    log_ = "Failed to pick up unload reel tower";
                                                    robotSequenceErrorCode = ErrorCode.FailedToPickUnloadReel;
                                                }
                                                break;
                                        }

                                        robotSubStep = SubSequenceSteps.Post;
                                        ReelTowerManager.FireReportRuntimeLog($"{log_} {currentUnloadReelState.Index} : {res_}");
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {   
                                        robotStep = ReelHandlerSteps.CompletedToUnloadReelFromTower;
                                        throw new PauseException<ErrorCode>(robotSequenceErrorCode);
                                    }
                            }
                        }
                        break;
                    #endregion

                    #region Prepare to reject reel
                    case ReelHandlerSteps.PrepareToRejectReel:
                        {
                            if (!robotSequenceManager.IsMoving &&
                                robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) && cmddone_)
                            {
                                switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                {
                                    case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                    case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                        {
                                            if (currentLoadReelTower > 0 && currentLoadReelTower == currentLoadReelState.Index)
                                            {
                                                DefineRobotOperation();
                                            }
                                        }
                                        break;
                                    case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                                        {
                                            if (currentUnloadReelTower > 0 && currentUnloadReelTower == currentUnloadReelState.Index)
                                                DefineRobotOperation();
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Move to front of reject stage
                    case ReelHandlerSteps.MoveToFrontOfRejectStage:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        robotSequenceErrorCode = ErrorCode.None;

                                        if (!robotSequenceManager.IsMoving &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) && cmddone_)
                                        {
                                            switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                            {
                                                case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                                                case TransferMaterialObject.TransferModes.Reject:
                                                    {
                                                        if (robotSequenceManager.MoveRobotToFrontOfRejectStage(currentRejectReelState.OutputStageIndex))
                                                        {
                                                            Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.Reject);

                                                            if (currentRejectReelState.State == MaterialStorageState.StorageOperationStates.Unload)
                                                                Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.RequestToUnloadAssignment);

                                                            robotSequenceErrorCode = ErrorCode.None;
                                                            robotSubStep = SubSequenceSteps.Proceed;
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    {
                                                        robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                                        robotSubStep = SubSequenceSteps.Post;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {
                                            robotSequenceErrorCode = ErrorCode.RobotMoveTimeout;
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                        else if (robotSequenceManager.IsConnected && robotSequenceManager.IsReceived &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                        {
                                            if (currentRejectReelState.State == MaterialStorageState.StorageOperationStates.Unload)
                                            {
                                                ReelTowerManager.SendTowerMessage(ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN,
                                                    currentRejectReelState.PendingData, currentRejectReelState.Name);
                                            }

                                            robotStep = ReelHandlerSteps.Ready;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                        throw new PauseException<ErrorCode>(robotSequenceErrorCode);
                                    }
                            }
                        }
                        break;
                    #endregion

                    #region Check up state of reject stage
                    case ReelHandlerSteps.CheckUpStateOfRejectStage:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        robotSequenceErrorCode = ErrorCode.None;

                                        if (!robotSequenceManager.IsMoving &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) && cmddone_ &&
                                            !rejectStageFull)
                                        {
                                            robotSubStep = SubSequenceSteps.Proceed;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.ApproachRejectStage;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Approach to reject stage
                    case ReelHandlerSteps.ApproachRejectStage:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        robotSequenceErrorCode = ErrorCode.None;

                                        if (!robotSequenceManager.IsMoving &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) && cmddone_)
                                        {
                                            switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                            {
                                                case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                                                case TransferMaterialObject.TransferModes.Reject:
                                                    {
                                                        if (robotSequenceManager.ApproachRejectStage(currentRejectReelState.OutputStageIndex))
                                                        {
                                                            robotSequenceErrorCode = ErrorCode.None;
                                                            robotSubStep = SubSequenceSteps.Proceed;
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    {
                                                        robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                                        robotSubStep = SubSequenceSteps.Post;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {
                                            robotSequenceErrorCode = ErrorCode.RobotMoveTimeout;
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                        else if (robotSequenceManager.IsConnected && robotSequenceManager.IsReceived &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                        {
                                            switch (currentRejectReelState.OutputStageIndex)
                                            {
                                                case 1:
                                                case 2:
                                                case 3:
                                                case 4:
                                                case 5:
                                                case 6:
                                                    {
                                                        lock (reelsOfRejectStages)
                                                        {
                                                            reelsOfRejectStages.Add(new FourField<string, string, string, ReelUnloadReportStates>(
                                                                currentUnloadReelState.Uid,
                                                                currentUnloadReelState.PrintData(),
                                                                currentRejectReelState.State.ToString(),
                                                                ReelUnloadReportStates.Complete));
                                                        }
                                                    }
                                                    break;
                                            }

                                            robotStep = ReelHandlerSteps.Ready;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                        throw new PauseException<ErrorCode>(robotSequenceErrorCode);
                                    }
                            }
                        }
                        break;
                    #endregion

                    #region Put reel into reject stage
                    case ReelHandlerSteps.PutReelIntoRejectStage:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        robotSequenceErrorCode = ErrorCode.None;

                                        if (!robotSequenceManager.IsMoving &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) && cmddone_)
                                        {
                                            if (robotSequenceManager.PutReelIntoRejectStage(currentRejectReelState.OutputStageIndex))
                                            {
                                                robotSubStep = SubSequenceSteps.Proceed;
                                            }
                                            else
                                            {
                                                robotSequenceErrorCode = ErrorCode.RobotStateIsNotMatched;
                                                robotSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        if (robotSequenceManager.IsFailure)
                                        {
                                            robotSequenceErrorCode = ErrorCode.RobotMoveTimeout;
                                            robotSubStep = SubSequenceSteps.Post;
                                        }
                                        else if (robotSequenceManager.IsConnected && robotSequenceManager.IsReceived &&
                                            robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                        {
                                            robotStep = ReelHandlerSteps.Ready;
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                        throw new PauseException<ErrorCode>(robotSequenceErrorCode);
                                    }
                            }
                        }
                        break;
                    #endregion

                    #region Completed to reject reel
                    case ReelHandlerSteps.CompletedToRejectReel:
                        {
                            switch (robotSubStep)
                            {
                                case SubSequenceSteps.Prepare:
                                    {
                                        if (!robotSequenceManager.HasAReel)
                                        {
                                            switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                            {
                                                case TransferMaterialObject.TransferModes.None:
                                                case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                                case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                                case TransferMaterialObject.TransferModes.PrepareToRejectUnloadReel:
                                                case TransferMaterialObject.TransferModes.Reject:
                                                    {
                                                        switch (Singleton<TransferMaterialObject>.Instance.State)
                                                        {
                                                            default:
                                                            case TransferMaterialObject.TransferStates.None:
                                                            case TransferMaterialObject.TransferStates.RequestToLoadConfirm:
                                                            case TransferMaterialObject.TransferStates.WaitForLoadConfirm:
                                                            case TransferMaterialObject.TransferStates.ConfirmLoad:
                                                            case TransferMaterialObject.TransferStates.RequestToBarcodeConfirm:
                                                            case TransferMaterialObject.TransferStates.WaitForBarcodeConfirm:
                                                            case TransferMaterialObject.TransferStates.ConfirmedBarcodeOfCart:
                                                            case TransferMaterialObject.TransferStates.ConfirmedBarcodeOfReturn:
                                                            case TransferMaterialObject.TransferStates.RequestToLoadAssignment:
                                                            case TransferMaterialObject.TransferStates.WaitForLoadAssignment:
                                                            case TransferMaterialObject.TransferStates.CompleteLoad:
                                                            case TransferMaterialObject.TransferStates.VerifiedUnload:
                                                            case TransferMaterialObject.TransferStates.TakenMaterial:
                                                            case TransferMaterialObject.TransferStates.RequestToUnloadAssignment:
                                                            case TransferMaterialObject.TransferStates.WaitForUnloadAssignment:
                                                            case TransferMaterialObject.TransferStates.CompleteUnload:
                                                                {
                                                                    reelTowerQueryRetryCount = 0;
                                                                    Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.None);
                                                                    robotSubStep = SubSequenceSteps.Proceed;
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case SubSequenceSteps.Proceed:
                                    {
                                        robotSubStep = SubSequenceSteps.Post;
                                    }
                                    break;
                                case SubSequenceSteps.Post:
                                    {
                                        robotStep = ReelHandlerSteps.Done;
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion
                    #endregion

                    #region Reset or clear current pending state then turn back to ready
                    case ReelHandlerSteps.Done:
                        {
                            switch (robotSequenceManager.RobotActionOrder)
                            {
                                case RobotActionOrder.UnloadReelFromTower:
                                    {
                                        switch (robotSequenceManager.FailureCode)
                                        {
                                            case RobotActionFailures.PickupFailureToUnloadReel:
                                            case RobotActionFailures.None:
                                                {   // Remove unload reel in tower by manual
                                                    ReelTowerManager.ClearReelTowerStates(currentUnloadReelState.Name, currentUnloadReelState.Uid);
                                                    currentUnloadReelState.Clear();

                                                    switch (Singleton<TransferMaterialObject>.Instance.State)
                                                    {
                                                        case TransferMaterialObject.TransferStates.None:
                                                        case TransferMaterialObject.TransferStates.VerifiedUnload:
                                                        case TransferMaterialObject.TransferStates.TakenMaterial:
                                                        case TransferMaterialObject.TransferStates.CompleteUnload:
                                                            {   // After remove a reel by manual 
                                                                Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.None);
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                        }

                                        Interlocked.Exchange(ref currentUnloadReelTower, -1);
                                        robotSequenceManager.RobotActionOrder = RobotActionOrder.None;
                                    }
                                    break;
                                case RobotActionOrder.LoadReelFromReturn:
                                case RobotActionOrder.LoadReelFromCart:
                                    {
                                        if ((robotSequenceManager.RobotActionOrder == RobotActionOrder.LoadReelFromCart) &&
                                            ((currentReelTypeOfCart == ReelDiameters.ReelDiameter7 && currentWorkSlotOfCart > 6) ||
                                            (currentReelTypeOfCart == ReelDiameters.ReelDiameter13 && currentWorkSlotOfCart > 4)))
                                        {
                                            finishedLoadReelFromCart = true;
                                            FireShowNotification(Properties.Resources.String_Notification_Completed_Load_Reel_From_Cart);
                                            ReelTowerManager.SendTowerMessage(ReelTowerCommands.REQUEST_CART_LOAD_FINISH);
                                        }

                                        if (robotSequenceManager.FailureCode == RobotActionFailures.None)
                                        {   // Refresh load reel state by normal.
                                            currentLoadReelState.Clear();
                                        }

                                        Interlocked.Exchange(ref currentLoadReelTower, -1);
                                        robotSequenceManager.RobotActionOrder = RobotActionOrder.None;
                                        currentReelTypeOfReturn = ReelDiameters.Unknown;
                                    }
                                    break;
                                case RobotActionOrder.CartReelToReject:
                                case RobotActionOrder.ReturnReelToReject:
                                case RobotActionOrder.UnloadReelToReject:
                                    {
                                        switch (currentRejectReelState.State)
                                        {
                                            case MaterialStorageState.StorageOperationStates.Load:
                                                {
                                                    if (robotSequenceManager.FailureCode == RobotActionFailures.None)
                                                        currentLoadReelState.Clear();

                                                    Interlocked.Exchange(ref currentLoadReelTower, -1);
                                                    currentReelTypeOfReturn = ReelDiameters.Unknown;
                                                }
                                                break;
                                            case MaterialStorageState.StorageOperationStates.Unload:
                                                {
                                                    switch (robotSequenceManager.FailureCode)
                                                    {
                                                        case RobotActionFailures.PickupFailureToUnloadReel:
                                                        case RobotActionFailures.None:
                                                            {   // Remove unload reel in tower by manual
                                                                ReelTowerManager.ClearReelTowerStates(currentUnloadReelState.Name, currentUnloadReelState.Uid);
                                                                currentUnloadReelState.Clear();
                                                            }
                                                            break;
                                                    }

                                                    Interlocked.Exchange(ref currentUnloadReelTower, -1);
                                                }
                                                break;
                                        }

                                        currentRejectReelState.Clear();
                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                        Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.None);
                                        robotSequenceManager.RobotActionOrder = RobotActionOrder.None;
                                        barcodeConfirmState = BarcodeConfirmStates.Prepared;
                                        if (SameReelCheck)
                                        {
                                            Pause(ErrorCode.ReelBarcodeIsNotUnique);
                                            SameReelCheck = false;
                                        }
                                    }
                                    break;
                            }

                            // Return to define task order step.
                            robotStep = ReelHandlerSteps.Ready;

                            if (cycleStop)
                                throw new CycleStopException(Properties.Resources.String_FormMain_Information_Completed_CycleStop);
                        }
                        break;
                        #endregion
                }

                DoIdleStateProcess();

                #region Robot program state check variable reset
                // Reset robot program state check variables
                robotProgramStateCheckRetry = 0;
                robotProgramStateStep = SubSequenceSteps.Prepare;
                robotProgramStateTick = App.TickCount;
                #endregion
            }
            else
            {
                #region Robot program state check routine
                switch (robotProgramStateStep)
                {
                    case SubSequenceSteps.Prepare:
                        {
                            if (robotController.IsRunnable)
                            {
                                if (robotController.LoadedProgram != (Config.EnableVisionMarkAdjustment? RobotController.CONST_ROBOT_PROGRAM_REV3 : RobotController.CONST_ROBOT_PROGRAM_REV2))
                                    result_ = true;
                            }
                            else
                            {
                                if (robotController.LoadedProgram == (Config.EnableVisionMarkAdjustment ? RobotController.CONST_ROBOT_PROGRAM_REV3 : RobotController.CONST_ROBOT_PROGRAM_REV2))
                                    {
                                        if (IsOverDelayTime(timeoutRobotCommunication, robotProgramStateTick))
                                        {
                                            ++robotProgramStateCheckRetry;

                                            if (IsRobotProgramStateCheckRetryOver)
                                            {
                                                if (robotController.ProgramState == RobotProgramStates.Playing)
                                                    robotProgramStateStep = SubSequenceSteps.Proceed;
                                                else
                                                    robotProgramStateCheckRetry = 0;
                                            }
                                            else
                                                robotProgramStateTick = App.TickCount;
                                        }
                                        else if (Config.SafetySensorUsage)
                                            FireShowNotification(Properties.Resources.String_Notification_Robot_Program_State);
                                    }
                                    else
                                        result_ = true;
                            }

                            if (result_)
                                throw new PauseException<ErrorCode>(ErrorCode.FailedToPlayProgram);
                        }
                        break;
                    case SubSequenceSteps.Proceed:
                        {
                            if (robotSequenceManager.IsConnected && robotController.IsRunnable)
                                robotProgramStateStep = SubSequenceSteps.Post;
                            else
                                robotProgramStateStep = SubSequenceSteps.Prepare;
                        }
                        break;
                    case SubSequenceSteps.Post:
                        {
                            switch (robotController.ProgramState)
                            {
                                case RobotProgramStates.Playing:
                                    {   // Not necessary to do something. The robot just paused by some event.
                                    }
                                    break;
                                default:
                                case RobotProgramStates.Unknown:
                                    {
                                        throw new PauseException<ErrorCode>(ErrorCode.FailedToPlayProgram);
                                    }
                            }
                        }
                        break;
                }
                #endregion
            }
        }
        #endregion

        public virtual void MoveToForwardReelsInOutputStage(int outputstage)
        {   // UPDATED: 20200424 (Marcus)
            // Not use pusher and stopper.
            // switch (outputstage)
            // {
            //     case 1:
            //     case 2:
            //     case 3:
            //     case 4:
            //     case 5:
            //     case 6:
            //         {
            //             if ((App.DigitalIoManager as DigitalIoManager).PushOutputMaterial(outputstage))
            //             {   // Shift to 1, 2, 3 from 4, 5, 6.
            //                 switch (outputstage)
            //                 {
            //                     case 4:
            //                     case 5:
            //                     case 6:
            //                         {
            //                             if (reelsOfOutputStages.ContainsKey(outputstage - 1))
            //                             {
            //                                 if (reelsOfOutputStages.ContainsKey(outputstage - 4))
            //                                 {
            //                                     reelsOfOutputStages[outputstage - 4].Clear();
            //                                     reelsOfOutputStages[outputstage - 4].CopyFrom(reelsOfOutputStages[outputstage - 1]);
            //                                 }
            //                                 else
            //                                     reelsOfOutputStages.Add(outputstage - 4, new MaterialPackage(reelsOfOutputStages[outputstage - 1]));
            // 
            //                                 reelsOfOutputStages[outputstage - 1].Clear();
            //                             }
            //                         }
            //                         break;
            //                 }
            //             }
            //         }
            //         break;
            // }
        }

        public virtual void MoveToBackwardReelsInOutputStage()
        {   // UPDATED: 20200424 (Marcus)
            // Not use pusher and stopper.
            for (int i_ = 1; i_ <= Config.OutStageIds.Count; i_++)
            {
                bool pull_ = false;

                switch (i_)
                {
                    case 1:
                        {
                            // if ((App.DigitalIoManager as DigitalIoManager).ForwardPusher1)
                            //     pull_ = true;
                            // else if ((App.DigitalIoManager as DigitalIoManager).BackwardPusher1 && !(App.DigitalIoManager as DigitalIoManager).OutputStage4Exist)
                            if (!(App.DigitalIoManager as DigitalIoManager).OutputStage4Exist)
                                outputStage4Full = false;

                            // if (reelsOfOutputStages.ContainsKey(3))
                            //     reelsOfOutputStages[3].Clear();
                        }
                        break;
                    case 2:
                        {
                            // if ((App.DigitalIoManager as DigitalIoManager).ForwardPusher2)
                            //     pull_ = true;
                            // else if ((App.DigitalIoManager as DigitalIoManager).BackwardPusher2 && !(App.DigitalIoManager as DigitalIoManager).OutputStage5Exist)
                            if (!(App.DigitalIoManager as DigitalIoManager).OutputStage5Exist)
                                outputStage5Full = false;

                            // if (reelsOfOutputStages.ContainsKey(4))
                            //     reelsOfOutputStages[4].Clear();
                        }
                        break;
                    case 3:
                        {
                            // if ((App.DigitalIoManager as DigitalIoManager).ForwardPusher3)
                            //     pull_ = true;
                            // else if ((App.DigitalIoManager as DigitalIoManager).BackwardPusher3 && !(App.DigitalIoManager as DigitalIoManager).OutputStage6Exist)
                            if (!(App.DigitalIoManager as DigitalIoManager).OutputStage6Exist)
                                outputStage6Full = false;

                            // if (reelsOfOutputStages.ContainsKey(5))
                            //     reelsOfOutputStages[5].Clear();
                        }
                        break;
                }

                if (pull_)
                    (App.DigitalIoManager as DigitalIoManager).PullOutputMaterial(i_);
            }
        }

        protected virtual void OnRemovedMaterialPackage(object sender, MaterialPackage pkg)
        {
            if (pkg != null)
                MoveToForwardReelsInOutputStage(pkg.OutputPort);
        }

        protected bool SetReelTypeOfCart(ReelDiameters reeltype, bool returnreel = false)
        {
            bool result_ = false;
            string val_ = string.Empty;

            switch (reeltype)
            {
                case ReelDiameters.ReelDiameter4:
                    val_ = "4";
                    break;
                case ReelDiameters.ReelDiameter7:
                    val_ = "7";
                    break;
                case ReelDiameters.ReelDiameter13:
                    val_ = "13";
                    break;
                case ReelDiameters.ReelDiameter15:
                    val_ = "15";
                    break;
            }

            if (!string.IsNullOrEmpty(val_))
            {
                if (robotSequenceManager.SendCommand(returnreel ? RobotSequenceCommands.ApplyReelTypeOfReturn: RobotSequenceCommands.ApplyReelTypeOfCart, val_))
                    result_ = true;
            }

            return result_;
        }

        protected bool AdjustCartGuidePointOffsetOfCart(RobotSequenceCommands cmd, ReelDiameters reeltype, double x, double y, double z, double angle)
        {
            bool result_ = false;
            if (robotSequenceManager.SendCommand(cmd, $"{Convert.ToInt32(reeltype)};{x};{y};{z};{angle}"))
                result_ = true;
            return result_;
        }

        protected bool SetCartGuidePointOffsetOfCart(RobotSequenceCommands cmd, ReelDiameters reeltype, double x, double y, double z, double angle)
        {
            bool result_ = false;
            if (robotSequenceManager.SendCommand(cmd, $"{Convert.ToInt32(reeltype)};{x};{y};{z};{angle}"))
                result_ = true;
            return result_;
        }

        protected bool SetCartGuidePointWorkSlotCenter(RobotSequenceCommands cmd, ReelDiameters reeltype)
        {
            bool result_ = false;
            int index_ = -1;
            string arg_ = string.Empty;
            switch (cmd)
            {
                case RobotSequenceCommands.SetCartGuideWorkSlotCenter1:
                case RobotSequenceCommands.SetCartGuideWorkSlotCenter2:
                case RobotSequenceCommands.SetCartGuideWorkSlotCenter3:
                case RobotSequenceCommands.SetCartGuideWorkSlotCenter4:
                case RobotSequenceCommands.SetCartGuideWorkSlotCenter5:
                case RobotSequenceCommands.SetCartGuideWorkSlotCenter6:
                    {
                        index_ = cmd - RobotSequenceCommands.SetCartGuideWorkSlotCenter1 + 1;
                    }
                    break;
            }
            switch (reeltype)
            {
                case ReelDiameters.ReelDiameter7:
                    {
                        if (Model.Process.Cart7GuidePoints.ContainsKey(index_))
                            arg_ = $"{Convert.ToInt32(reeltype)};{Model.Process.Cart7GuidePoints[index_].first};{Model.Process.Cart7GuidePoints[index_].second};{Model.Process.Cart7GuidePoints[index_].third};{Model.Process.Cart7GuidePoints[index_].sixth}";
                    }
                    break;
                case ReelDiameters.ReelDiameter13:
                    {
                        if (Model.Process.Cart13GuidePoints.ContainsKey(index_))
                            arg_ = $"{Convert.ToInt32(reeltype)};{Model.Process.Cart13GuidePoints[index_].first};{Model.Process.Cart13GuidePoints[index_].second};{Model.Process.Cart13GuidePoints[index_].third};{Model.Process.Cart13GuidePoints[index_].sixth}";
                    }
                    break;
            }

            if (!string.IsNullOrEmpty(arg_))
            {
                if (robotSequenceManager.SendCommand(cmd, arg_))
                    result_ = true;
            }

            return result_;
        }

        protected bool AdjustTowerBasePointOffsetOfCart(RobotSequenceCommands cmd, int index, double x, double y, double z, double angle)
        {
            bool result_ = false;
            if (robotSequenceManager.SendCommand(cmd, $"{index};{x};{y};{z};{angle}"))
                result_ = true;
            return result_;
        }

        protected bool SetTowerBasePointOffsetOfCart(RobotSequenceCommands cmd, int index, double x, double y, double z, double angle)
        {
            bool result_ = false;
            if (robotSequenceManager.SendCommand(cmd, $"{index};{x};{y};{z};{angle}"))
                result_ = true;
            return result_;
        }

        protected bool RequestTowerBasePointOffsetOfCart(RobotSequenceCommands cmd)
        {
            bool result_ = false;
            if (robotSequenceManager.SendCommand(cmd))
                result_ = true;
            return result_;
        }

        protected int VerifyRobotResponseData(RobotSequenceCommands cmd, ref string res)
        {
            int result_ = -1;
            double xmin_ = 0.0;
            double xmax_ = 0.0;
            double ymin_ = 0.0;
            double ymax_ = 0.0;
            double rzmin_ = 0.0;
            double rzmax_ = 0.0;
            double xval_ = 0.0;
            double yval_ = 0.0;
            double rzval_ = 0.0;
            string[] data_ = null;
            string[] vals_ = null;

            if (robotSequenceManager.GetReceivedRobotData(cmd, ref data_))
            {
                if (data_ != null)
                {
                    switch (cmd)
                    {
                        case RobotSequenceCommands.VerifyTowerBasePoints:
                            {
                                vals_ = data_[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                xmin_ = Model.Process.GetTowerBasePointReference().first - 0.1;
                                xmax_ = Model.Process.GetTowerBasePointReference().first + 0.1;
                                ymin_ = Model.Process.GetTowerBasePointReference().second >= 0 ? Model.Process.GetTowerBasePointReference().second - 0.1 : Model.Process.GetTowerBasePointReference().second + 0.1;
                                ymax_ = Model.Process.GetTowerBasePointReference().second >= 0 ? Model.Process.GetTowerBasePointReference().second + 0.1 : Model.Process.GetTowerBasePointReference().second - 0.1;
                                rzmin_ = Model.Process.GetTowerBasePointReference().sixth * -1;
                                rzmax_ = Model.Process.GetTowerBasePointReference().sixth;
                                xval_ = Convert.ToDouble(vals_[0]) * 1000;
                                yval_ = Convert.ToDouble(vals_[1]) * 1000;
                                rzval_ = Convert.ToDouble(vals_[5]) * 1000;

                                res = $"X={Math.Round(xval_, 3)},Y={Math.Round(yval_, 3)}, Rz={Math.Round(rzval_, 3)}";

                                if (xval_ >= xmin_ && xval_ <= xmax_ &&
                                    yval_ >= ymin_ && yval_ <= ymax_)
                                {
                                    result_ = data_.Length;
                                }
                                else
                                {
                                    Logger.Trace($"{MethodBase.GetCurrentMethod().Name}(CompareResult): X={Math.Round(xval_, 3)}({xmin_}~{xmax_}),Y={ Math.Round(yval_, 3)}({ymin_}~{ymax_}),Rz={Math.Round(rzval_, 3)}");
                                }
                            }
                            break;
                    }
                }
            }
            return result_;
        }

        protected bool SetCartGuidePointOffsetOfCart(ReelDiameters reeltype, RobotSequenceCommands cmd, double x, double y, double angle)
        {
            bool result_ = false;
            string val_ = string.Empty;

            switch (reeltype)
            {
                case ReelDiameters.ReelDiameter4:
                    val_ = "4";
                    break;
                case ReelDiameters.ReelDiameter7:
                    val_ = "7";
                    break;
                case ReelDiameters.ReelDiameter13:
                    val_ = "13";
                    break;
                case ReelDiameters.ReelDiameter15:
                    val_ = "15";
                    break;
            }

            if (!string.IsNullOrEmpty(val_))
            {
                if (robotSequenceManager.SendCommand(cmd, $"{val_};{x};{y};{angle}"))
                    result_ = true;
            }

            return result_;
        }

        protected bool StartTasks()
        {
            if (!systemAutoRunning)
            {
                systemAutoRunning = true;
                processThreads.Add(compositedProcess = new Thread(RunCompositedProcess));

                foreach (Thread thread in processThreads)
                    thread.Start();
            }

            return systemAutoRunning;
        }

        protected bool StopTasks()
        {
            try
            {
                if (systemAutoRunning)
                {
                    systemAutoRunning = false;

                    if (processThreads.Count > 0)
                    {
                        foreach (Thread thread in processThreads)
                            thread.Join();

                        compositedProcess = null;
                    }

                    processThreads.Clear();
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{GetType()}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return !systemAutoRunning;
        }

        protected void ShowAlarm(ErrorCode code, string name, string desc)
        {
            App.DigitalIoManager.SetOutput((int)DigitalIoManager.Outputs.Buzzer, true);
        }

        protected void ResetState()
        {
            barcodeKeyInData.Clear();
        }

        protected void RecheckInterlockState()
        {
        }

        protected void CheckOperationPanelState()
        {   // Emergency stop
        }

        protected void CheckRobotState()
        {
        }

        protected void CheckAlignerState()
        {
        }

        protected void FireRunableConditionFailure(bool reset = true)
        {
            if (reset)
            {
                if (((App.DigitalIoManager as DigitalIoManager).SignalTowerRed || (App.DigitalIoManager as DigitalIoManager).Buzzer) &&
                    !IsRequiredReset && !IsWaitBarcodeInput)
                    (App.DigitalIoManager as DigitalIoManager).SetSignalTower(OperationStates.Run);
            }
            else
            {
                if (!(App.DigitalIoManager as DigitalIoManager).SignalTowerRed)
                {
                    (App.DigitalIoManager as DigitalIoManager).SignalTowerRed = true;
                    (App.DigitalIoManager as DigitalIoManager).Buzzer = true;
                }
            }
        }

        protected void RunCompositedProcess()
        {
            #region Fields
            bool state = false;
            reelTowerSubStep = SubSequenceSteps.Prepare;
            mobileRobotSubStep = SubSequenceSteps.Prepare;
            robotSubStep = SubSequenceSteps.Prepare;
            StringBuilder messages = new StringBuilder();
            #endregion

            while (systemAutoRunning)
            {   // Network connection check and operation state
                if (state = RunnableCondition)
                {
                    try
                    {
                        FunctionReelTower();
                        FunctionMobileRobot();
                        FunctionRobot();
                    }
                    catch (CycleStopException ex)
                    {
                        Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}(CycleStop)={ex.Message}");
                        PauseByCycleStop();
                    }
                    catch (PauseException<ErrorCode> ex)
                    {
                        Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}(Pause)={ex.Message}");
                        Pause(ex.Code);
                        ResetSequenceSteps();

                        if (ex.Code == ErrorCode.RobotCommunicationFailure && Config.EnableOneshotRecovery)
                        {   // Try to recover by automatically.
                            RobotSequenceManager.RestartRobotSequenceManager(Config.RobotLocalServerPort);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Trace($"{GetType()}.{MethodBase.GetCurrentMethod().Name}(Exception)={ex.Message}");
                    }
                }
                else
                {
                    if (!robotSequenceManager.IsHomed || robotSequenceManager.IsFailure)
                    {
                        if (notificationTick == 0)
                            notificationTick = App.TickCount;

                        if (IsNotificationDelayOver(timeoutOfNotification))
                        {
                            notificationTick = 0;

                            if (robotSequenceManager.IsActualHomeFailure)
                            {
                                FireShowNotification(Properties.Resources.String_Notification_Check_Robot_State);
                                ResetSequenceSteps();
                            }
                        }
                    }
                    else
                    {
                        if (notificationTick == 0)
                            notificationTick = App.TickCount;

                        if (IsNotificationDelayOver(timeoutOfNotification))
                        {
                            messages.Clear();
                            messages.AppendLine(Properties.Resources.String_Information_Check_System_Running_Environment);

                            if (!ReelTowerManager.IsRunning || !ReelTowerManager.IsServiceNow)
                                messages.AppendLine("REELTOWER IS NOT READY");

                            if (!mobileRobotManager.IsConnected)
                                messages.AppendLine("MRBT IS NOT READY");

                            if (!robotController.IsConnected || !robotSequenceManager.IsConnected)
                                messages.AppendLine("ROBOT IS NOT READY");

                            if (outputStage1Full)
                                messages.AppendLine("OUTPUT STAGE 1 IS FULL");

                            if (outputStage2Full)
                                messages.AppendLine("OUTPUT STAGE 2 IS FULL");

                            if (outputStage3Full)
                                messages.AppendLine("OUTPUT STAGE 3 IS FULL");

                            if (outputStage4Full)
                                messages.AppendLine("OUTPUT STAGE 4 IS FULL");

                            if (outputStage5Full)
                                messages.AppendLine("OUTPUT STAGE 5 IS FULL");

                            if (outputStage6Full)
                                messages.AppendLine("OUTPUT STAGE 6 IS FULL");

                            if (rejectStageFull)
                                messages.AppendLine("REJECT STAGE IS FULL");

                            FireShowNotification(messages.ToString());
                            notificationTick = 0;
                        }
                    }
                }

                FireRunableConditionFailure(state);
                (App.MainForm as FormMain).UpdateProductionDataLog();
                Thread.Sleep(threadScheduleInterval);
            }
        }

        protected virtual void RunRobotHome()
        {
            try
            {
                InitializeSteps step = InitializeSteps.Done;
                string log = "Started robot homing.";

                while (robotController.IsConnected &&
                    !systemAutoRunning &&
                    !stopRobotInitialize &&
                    robotInitializeStep < InitializeSteps.Done)
                {
                    if ((systemInitializeStep = robotInitializeStep) != step)
                    {
                        step = robotInitializeStep;
                        robotInitializeSubStep = SubSequenceSteps.Prepare;
                        Logger.ProcessTrace($"{MethodBase.GetCurrentMethod().Name}", step.ToString(), log);
                    }

                    switch (robotInitializeStep)
                    {
                        case InitializeSteps.CheckProgramState:
                            {
                                switch (robotInitializeSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (robotController.SendCommand(RobotControllerCommands.ProgramState))
                                                robotInitializeSubStep = SubSequenceSteps.Proceed;
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            if (robotController.IsFailure)
                                            {
                                                throw new PauseException<ErrorCode>(ErrorCode.RobotCommunicationFailure);
                                            }
                                            else if (robotController.IsReceived)
                                            {
                                                robotInitializeSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {
                                            switch (robotController.ProgramState)
                                            {
                                                case RobotProgramStates.Paused:
                                                case RobotProgramStates.Unknown:
                                                    {
                                                        robotInitializeStep = InitializeSteps.StopProgram;
                                                    }
                                                    break;
                                                case RobotProgramStates.Playing:
                                                    {
                                                        if (robotSequenceManager.IsConnected && robotSequenceManager.IsHomed)
                                                            robotInitializeStep = InitializeSteps.CheckLoadedProgram;
                                                        else
                                                            robotInitializeStep = InitializeSteps.StopProgram;
                                                    }
                                                    break;
                                                case RobotProgramStates.Stopped:
                                                    {
                                                        robotInitializeStep = InitializeSteps.LoadProgram;
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        case InitializeSteps.StopProgram:
                            {
                                switch (robotInitializeSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (robotController.SendCommand(RobotControllerCommands.StopProgram))
                                                robotInitializeSubStep = SubSequenceSteps.Proceed;
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            if (robotController.IsFailure)
                                            {
                                                throw new PauseException<ErrorCode>(ErrorCode.FailedToStopProgram);
                                            }
                                            else if (robotController.IsReceived)
                                            {
                                                robotInitializeSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {
                                            switch (robotController.ProgramState)
                                            {
                                                case RobotProgramStates.Paused:
                                                case RobotProgramStates.Playing:
                                                case RobotProgramStates.Unknown:
                                                    {
                                                        robotInitializeStep = InitializeSteps.StopProgram;
                                                        robotInitializeSubStep = SubSequenceSteps.Prepare;
                                                    }
                                                    break;
                                                case RobotProgramStates.Stopped:
                                                    {
                                                        if (robotSequenceManager.IsHomed)
                                                            robotInitializeStep = InitializeSteps.PrepareInitialize;
                                                        else
                                                            robotInitializeStep = InitializeSteps.LoadProgram;
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        case InitializeSteps.LoadProgram:
                            {
                                switch (robotInitializeSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (robotController.SendCommand(RobotControllerCommands.LoadProgram, (Config.EnableVisionMarkAdjustment ? RobotController.CONST_ROBOT_PROGRAM_REV3 : RobotController.CONST_ROBOT_PROGRAM_REV2)))
                                                robotInitializeSubStep = SubSequenceSteps.Proceed;
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            if (robotController.IsFailure)
                                            {
                                                throw new PauseException<ErrorCode>(ErrorCode.FailedToLoadProgram);
                                            }
                                            else if (robotController.IsReceived)
                                            {
                                                robotInitializeSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {
                                            switch (robotController.ProgramState)
                                            {
                                                case RobotProgramStates.Paused:
                                                case RobotProgramStates.Playing:
                                                case RobotProgramStates.Unknown:
                                                    {
                                                        robotInitializeStep = InitializeSteps.StopProgram;
                                                    }
                                                    break;
                                                case RobotProgramStates.Stopped:
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount = 0;
                                                        robotInitializeStep = InitializeSteps.PlayProgram;
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        case InitializeSteps.PlayProgram:
                            {
                                switch (robotInitializeSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (robotController.SendCommand(RobotControllerCommands.PlayProgram))
                                                robotInitializeSubStep = SubSequenceSteps.Proceed;
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            if (robotController.IsFailure)
                                            {
                                                throw new PauseException<ErrorCode>(ErrorCode.RobotCommunicationFailure);
                                            }
                                            else if (robotController.IsReceived)
                                            {
                                                robotInitializeSubStep = SubSequenceSteps.Post;
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {
                                            switch (robotController.ProgramState)
                                            {
                                                case RobotProgramStates.Playing:
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotInitializeStep = InitializeSteps.PrepareInitialize;
                                                    }
                                                    break;
                                                default:
                                                case RobotProgramStates.Unknown:
                                                    {
                                                        throw new PauseException<ErrorCode>(ErrorCode.FailedToPlayProgram);
                                                    }
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        case InitializeSteps.CheckLoadedProgram:
                            {
                                switch (robotInitializeSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (robotController.LoadedProgram.Contains((Config.EnableVisionMarkAdjustment ? RobotController.CONST_ROBOT_PROGRAM_REV3 : RobotController.CONST_ROBOT_PROGRAM_REV2)))
                                                robotInitializeSubStep = SubSequenceSteps.Proceed;
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            if (robotSequenceManager.IsFailure)
                                            {
                                                throw new PauseException<ErrorCode>(ErrorCode.RobotStateIsNotMatched);
                                            }
                                            else if (robotSequenceManager.IsWaitForOrder)
                                            {
                                                if (robotSequenceManager.IsAtSafePosition || robotSequenceManager.IsReadyToUnloadReel)
                                                {
                                                    robotInitializeSubStep = SubSequenceSteps.Post;
                                                }
                                                else
                                                {
                                                    throw new PauseException<ErrorCode>(ErrorCode.RobotPositionIsNotMatched);
                                                }
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {
                                            robotTick = App.TickCount;
                                            robotInitializeStep = InitializeSteps.CheckHomedPosition;
                                        }
                                        break;
                                }
                            }
                            break;
                        case InitializeSteps.CheckHomedPosition:
                            {
                                switch (robotInitializeSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (robotSequenceManager.IsHomed && robotSequenceManager.IsAtSafePosition && robotSequenceManager.IsReadyToUnloadReel)
                                            {
                                                robotSequenceManager.SendCommand(RobotSequenceCommands.MoveToHome);
                                                log = "Check homed state...";
                                                robotTick = App.TickCount;
                                                robotInitializeSubStep = SubSequenceSteps.Proceed;
                                            }
                                            else
                                                robotInitializeStep = InitializeSteps.StopProgram;
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {   // UPDATED: 20200408 (Marcus)
                                            // Fast check robot moving. (Delay: 100 ms)
                                            if (IsRobotActionDelayOver(Convert.ToInt32(timeoutRobotCommunication * 0.01)) && !robotSequenceManager.IsMoving)
                                            {
                                                if (robotSequenceManager.IsFailure)
                                                {
                                                    throw new PauseException<ErrorCode>(ErrorCode.RobotMoveToHomeTimeout);
                                                }
                                                else if (IsRobotActionRetryOver)
                                                {
                                                    throw new PauseException<ErrorCode>(ErrorCode.RobotCommunicationFailure);
                                                }
                                                else if (robotSequenceManager.IsRobotAtHome || robotSequenceManager.IsRobotAlreadyAtHome)
                                                {
                                                    robotInitializeSubStep = SubSequenceSteps.Post;
                                                }
                                                else
                                                {
                                                    robotActionRetryCount++;
                                                    robotInitializeSubStep = SubSequenceSteps.Prepare;
                                                }
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {   // Restart robot program.
                                            robotInitializeStep = InitializeSteps.StopProgram;
                                        }
                                        break;
                                }
                            }
                            break;
                        case InitializeSteps.PrepareInitialize:
                            {
                                switch (robotInitializeSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (IsRobotActionDelayOver(timeoutRobotCommunication))
                                            {
                                                if (IsRobotActionRetryOver)
                                                {
                                                    throw new PauseException<ErrorCode>(ErrorCode.RobotCommunicationFailure);
                                                }
                                                else
                                                {
                                                    robotActionRetryCount++;
                                                    robotInitializeStep = InitializeSteps.PlayProgram;
                                                }
                                            }
                                            else if (robotSequenceManager.IsConnected)
                                            {
                                                robotTick = 0;
                                                robotInitializeSubStep = SubSequenceSteps.Proceed;
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            log = "Searching home.";
                                            robotInitializeSubStep = SubSequenceSteps.Post;
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {
                                            robotActionRetryCount = 0;
                                            robotInitializeStep = InitializeSteps.Initializing;
                                        }
                                        break;
                                }
                            }
                            break;
                        case InitializeSteps.Initializing:
                            {
                                switch (robotInitializeSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            robotSequenceManager.SendCommand(RobotSequenceCommands.MoveToHome);
                                            log = "Homing.";
                                            robotTick = App.TickCount;

                                            if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                                                (App.DigitalIoManager as DigitalIoManager).ClampCart(DigitalIoManager.CartClamps.Side);
                                            else
                                                (App.DigitalIoManager as DigitalIoManager).UndockCart();

                                            robotInitializeSubStep = SubSequenceSteps.Proceed;
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {   // UPDATED: 20200408 (Marcus)
                                            // Fast check robot moving. (Delay: 100 ms)
                                            if (IsRobotActionDelayOver(Convert.ToInt32(timeoutRobotCommunication * 0.01)) && !robotSequenceManager.IsMoving)
                                            {
                                                if (robotSequenceManager.IsFailure)
                                                {
                                                    throw new PauseException<ErrorCode>(ErrorCode.RobotMoveToHomeTimeout);
                                                }
                                                else if (IsRobotActionRetryOver)
                                                {
                                                    throw new PauseException<ErrorCode>(ErrorCode.RobotCommunicationFailure);
                                                }
                                                else if (robotSequenceManager.IsRobotAtHome || robotSequenceManager.IsRobotAlreadyAtHome)
                                                {
                                                    if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                                                    {
                                                        if ((App.DigitalIoManager as DigitalIoManager).CartAlignSensorLeftForward &&
                                                            (App.DigitalIoManager as DigitalIoManager).CartAlignSensorRightForward)
                                                        {
                                                            (App.DigitalIoManager as DigitalIoManager).ClampCart(DigitalIoManager.CartClamps.Hook);
                                                            robotInitializeSubStep = SubSequenceSteps.Post;
                                                        }
                                                    }
                                                    else
                                                        robotInitializeSubStep = SubSequenceSteps.Post;
                                                }
                                                else
                                                {
                                                    robotActionRetryCount++;
                                                    robotInitializeSubStep = SubSequenceSteps.Prepare;
                                                }
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {
                                            if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                                            {
                                                if ((App.DigitalIoManager as DigitalIoManager).CartAlignSensorLeftForward &&
                                                (App.DigitalIoManager as DigitalIoManager).CartAlignSensorRightForward &&
                                                (App.DigitalIoManager as DigitalIoManager).CartAlignSensorFrontXBackward)
                                                {
                                                    (App.DigitalIoManager as DigitalIoManager).ClampCart(DigitalIoManager.CartClamps.Pull);
                                                    robotInitializeStep = InitializeSteps.PostInitialize;
                                                }
                                            }
                                            else
                                                robotInitializeStep = InitializeSteps.PostInitialize;
                                        }
                                        break;
                                }
                            }
                            break;
                        case InitializeSteps.PostInitialize:
                            {
                                if (robotSequenceManager.IsFailure)
                                {
                                    log = "Failed to search home by robot error.";
                                    systemInitializeStep = (robotInitializeStep = InitializeSteps.Failed);
                                    stopRobotInitialize = true;
                                }
                                else if (robotSequenceManager.IsConnected && robotSequenceManager.IsHomed)
                                {
                                    log = "Found home signal.";
                                    initialized = true;
                                    systemInitializeStep = (robotInitializeStep = InitializeSteps.Done);
                                }
                                else if (systemInitializeStep == InitializeSteps.Failed)
                                {
                                    stopRobotInitialize = true;
                                }
                            }
                            break;
                    }

                    Thread.Sleep(CONST_THREAD_POLLING_INTERVAL);
                }
            }
            catch (Exception ex)
            {
                systemInitializeStep = (robotInitializeStep = InitializeSteps.Unknown);
                stopRobotInitialize = true;
                Logger.Trace($"{GetType()}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }
            finally
            {
                switch (systemInitializeStep)
                {
                    default:
                        { // Aborted by operator
                            Pause(ErrorCode.AbortInitialize);
                        }
                        break;
                    case InitializeSteps.Done:
                        {
                            systemInitialized = true;
                            reelTowerStep = ReelTowerSteps.Ready;
                            mobileRobotStep = MobileRobotSteps.Ready;
                            robotStep = ReelHandlerSteps.Ready;
                            reelTowerSubStep = SubSequenceSteps.Prepare;
                            mobileRobotSubStep = SubSequenceSteps.Prepare;
                            robotSubStep = SubSequenceSteps.Prepare;
                            barcodeConfirmState = BarcodeConfirmStates.Prepared;
                            currentLoadReelState.Clear();
                            currentUnloadReelState.Clear();
                            currentRejectReelState.Clear();
                            robotSequenceManager.ResetExecuteCommandToHome();
                            Interlocked.Exchange(ref currentUnloadReelTower, -1);
                            Interlocked.Exchange(ref currentLoadReelTower, -1);
                            robotSequenceManager.RobotActionOrder = RobotActionOrder.None;
                            Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                            Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.None);
                            currentReelTypeOfReturn = ReelDiameters.Unknown;
                            Singleton<MaterialPackageManager>.Instance.RemoveAllMaterialPackages(false, false);
                            ReelTowerManager.SendTowerMessage(ReelTowerCommands.REQUEST_ALL_ALARM_RESET, null);
                            ReelTowerManager.SendTowerMessage(ReelTowerCommands.REQUEST_ALL_LOAD_RESET, null);
                            Stop();
                            ClearStates();

                            if (!(App.DigitalIoManager as DigitalIoManager).WorkZoneCartPresentSensor1 &
                                !(App.DigitalIoManager as DigitalIoManager).WorkZoneCartPresentSensor2)
                                currentCartDockingState = CartDockingStates.Unknown;
                        }
                        break;
                    case InitializeSteps.Failed:
                    case InitializeSteps.Unknown:
                        {
                            Pause(ErrorCode.FailedInitialize);
                        }
                        break;
                }
            }
        }

        // UPDATED: 20200512 (Marcus)
        protected virtual void RunCalibration()
        {
            try
            {
                string log = "Started calibration.";
                string res_ = string.Empty;
                bool result_ = false;
                bool cmddone_ = false;
                bool safeposition_ = false;
                CalibrationSteps step = CalibrationSteps.Done;
                FireOperationStateChanged();

                while (robotController.IsConnected &&
                    !systemAutoRunning &&
                    !stopCalibration &&
                    systemInitialized &&
                    calibrationStep < CalibrationSteps.Done)
                {
                    if (calibrationStep != step)
                    {
                        step = calibrationStep;
                        calibrationSubStep = SubSequenceSteps.Prepare;
                        Logger.ProcessTrace($"{MethodBase.GetCurrentMethod().Name}", step.ToString(), log);
                    }

                    switch (calibrationStep)
                    {
                        case CalibrationSteps.PrepareCalibration:
                            {
                                switch (calibrationSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (calibrationMode != CalibrationMode.TowerBasePoints)
                                            {
                                                calibrationStep = CalibrationSteps.Done;
                                            }
                                            else
                                                calibrationSubStep = SubSequenceSteps.Proceed;
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                    case SubSequenceSteps.Post:
                                        {
                                            calibrationStep = CalibrationSteps.CheckCartPresent;
                                        }
                                        break;
                                }
                            }
                            break;
                        case CalibrationSteps.CheckCartPresent:
                            {
                                if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                                {
                                    calibrationStep = CalibrationSteps.Done;
                                }
                                else
                                {
                                    foreach (KeyValuePair<int, List<Coord3DField<double, double, double, double, double, double>>> item in loopCountOfTowerBasePoints)
                                        item.Value.Clear();

                                    robotVisionRetryCount = 0;
                                    verifiedTowerBasePoints = false;
                                    calibrationStep = CalibrationSteps.CheckTowerBasePoint1;
                                }
                            }
                            break;
                        case CalibrationSteps.CheckTowerBasePoint1:
                        case CalibrationSteps.CheckTowerBasePoint2:
                            {
                                int pointIndex_ = 0;
                                RobotSequenceCommands cmd_ = RobotSequenceCommands.CheckTowerBasePoint1;

                                switch (pointIndex_ = (calibrationStep - CalibrationSteps.CheckTowerBasePoint1) / (CalibrationSteps.CheckTowerBasePoint2 - CalibrationSteps.CheckTowerBasePoint1) + 1)
                                {
                                    case 1:
                                    case 2:
                                        cmd_ = (RobotSequenceCommands.CheckTowerBasePoint1 + ((pointIndex_ - 1) * (CalibrationSteps.CheckTowerBasePoint2 - CalibrationSteps.CheckTowerBasePoint1)));
                                        break;
                                }

                                switch (calibrationSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (robotSequenceManager.HasAReel)
                                            {
                                                throw new PauseException<ErrorCode>(ErrorCode.DetectedAReelOnGripper);
                                            }
                                            else
                                            {
                                                if (robotSequenceManager.IsWaitForOrder)
                                                {
                                                    if (robotSequenceManager.IsAtSafePosition)
                                                    {   // Check home position
                                                        if (robotSequenceManager.IsRobotAtHome)
                                                        {
                                                            robotTick = 0;
                                                        }
                                                        else
                                                        {
                                                            if (robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeBeforeReelHeightCheck))
                                                                robotTick = App.TickCount;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (!robotSequenceManager.IsRobotAtWayPointByCommand(ref safeposition_) &&
                                                            robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.MoveToHome &&
                                                            robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.GoToHomeBeforeReelHeightCheck &&
                                                            robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.CheckTowerBasePoint1 &&
                                                            robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.CheckTowerBasePoint2)
                                                            throw new PauseException<ErrorCode>(ErrorCode.RobotStateIsNotMatched);
                                                    }

                                                    if (IsRobotActionDelayOver(Convert.ToInt32(timeoutOfRobotMoving * 0.1)))
                                                    {
                                                        if (IsRobotAvailableToAct(safeposition_))
                                                        {
                                                            switch (Config.ImageProcessorType)
                                                            {
                                                                case ImageProcessorTypes.Mit:
                                                                    {
                                                                        VisionManager.LightOnForReelSizeConfirm();
                                                                    }
                                                                    break;
                                                            }

                                                            if ((loopCountOfTowerBasePoints.Count > 0 & (loopCountOfTowerBasePoints.ContainsKey(pointIndex_) && loopCountOfTowerBasePoints[pointIndex_].Count > 0)) ||
                                                                robotSequenceManager.SendCommand(cmd_))
                                                            {
                                                                robotTick = App.TickCount;
                                                                visionTick = App.TickCount;
                                                                Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                                calibrationSubStep = SubSequenceSteps.Proceed;
                                                            }
                                                        }
                                                        else if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) &&
                                                            (robotSequenceManager.LastExecutedCommand == RobotSequenceCommands.CheckTowerBasePoint1 ||
                                                            robotSequenceManager.LastExecutedCommand == RobotSequenceCommands.CheckTowerBasePoint2))
                                                        {
                                                            visionTick = App.TickCount;
                                                            Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                                            calibrationSubStep = SubSequenceSteps.Proceed;
                                                        }
                                                    }
                                                }
                                                else if (IsRobotActionDelayOver(Convert.ToInt32(timeoutOfRobotMoving * 0.1)))
                                                {
                                                    if (!robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) &&
                                                        robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.MoveToHome &&
                                                        robotSequenceManager.LastExecutedCommand != RobotSequenceCommands.GoToHomeBeforeReelHeightCheck)
                                                        throw new PauseException<ErrorCode>(ErrorCode.RobotStateIsNotMatched);
                                                }
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            switch (Config.ImageProcessorType)
                                            {
                                                case ImageProcessorTypes.Mit:
                                                    {
                                                        VisionManager.LightOnForReelSizeConfirm();

                                                        if (IsVisionProcessDelayOver(delayOfImageProcessing))
                                                        {
                                                            if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) || !IsRobotVisionRetryOver)
                                                            {
                                                                imageProcessingResult = ImageProcssingResults.Unknown;
                                                                visionTick = App.TickCount;
                                                                VisionManager.TriggerForReelSizeConfirm();
                                                                calibrationSubStep = SubSequenceSteps.Post;
                                                            }
                                                            else if (robotSequenceManager.IsFailure)
                                                            {
                                                                throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                case ImageProcessorTypes.TechFloor:
                                                    {
                                                        if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) && robotSequenceManager.IsWaitForOrder)
                                                        {
                                                            if (IsOverDelayTime(Model.TimeoutOfRetryTrigger, imageProcessTimeoutTick))
                                                            {
                                                                if (FireTriggerVisionControl(Model.LightLevels.Top,
                                                                    Model.TaskSubCategories.CheckTowerBasePoint,
                                                                    ReelDiameters.Unknown,
                                                                    pointIndex_,
                                                                    false,
                                                                    Model.DelayOfTrigger))
                                                                {
                                                                    calibrationSubStep = SubSequenceSteps.Post;
                                                                }
                                                                else
                                                                {
                                                                    robotVisionRetryCount++;
                                                                }
                                                            }
                                                        }
                                                        else if (robotSequenceManager.IsFailure)
                                                        {
                                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {
                                            switch (Config.ImageProcessorType)
                                            {
                                                case ImageProcessorTypes.Mit:
                                                case ImageProcessorTypes.TechFloor:
                                                    {
                                                        if (visionProcessResultState > 0)
                                                        {
                                                            ResetVisionProcessResultState();
                                                            object data = new object();
                                                            bool verified_ = false;
                                                            double offsetx_ = 0, offsety_ = 0, offsett_ = 0;

                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.ResultCode, ref data))
                                                            {
                                                                switch ((Cognex.VisionPro.CogToolResultConstants)data)
                                                                {
                                                                    case Cognex.VisionPro.CogToolResultConstants.Accept:
                                                                        {   // UPDATED: 20200512 (Marcus)
                                                                            if (currentReelTypeOfCart == ReelDiameters.Unknown)
                                                                            {
                                                                                switch (pointIndex_)
                                                                                {
                                                                                    case 1:
                                                                                        {
                                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundTowerBasePointX1, ref data))
                                                                                                offsetx_ = Convert.ToDouble(data);

                                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundTowerBasePointY1, ref data))
                                                                                                offsety_ = Convert.ToDouble(data);

                                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundTowerBasePointAngle1, ref data))
                                                                                                offsett_ = Convert.ToDouble(data);
                                                                                        }
                                                                                        break;
                                                                                    case 2:
                                                                                        {
                                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundTowerBasePointX2, ref data))
                                                                                                offsetx_ = Convert.ToDouble(data);

                                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundTowerBasePointY2, ref data))
                                                                                                offsety_ = Convert.ToDouble(data);

                                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.FoundTowerBasePointAngle2, ref data))
                                                                                                offsett_ = Convert.ToDouble(data);

                                                                                            if (visionProcessLastRunResult.IsResultAvailable(ResultDataElements.VerifiedBasePoint, ref data))
                                                                                                verified_ = Convert.ToBoolean(data);
                                                                                        }
                                                                                        break;
                                                                                }

                                                                                verifiedTowerBasePoints = verified_;

                                                                                if (!loopCountOfTowerBasePoints.ContainsKey(pointIndex_))
                                                                                    loopCountOfTowerBasePoints.Add(pointIndex_, new List<Coord3DField<double, double, double, double, double, double>>());

                                                                                loopCountOfTowerBasePoints[pointIndex_].Add(new Coord3DField<double, double, double, double, double, double>(offsetx_, offsety_, 0, 0, 0, offsett_));

                                                                                result_ = true;
                                                                                robotTick = App.TickCount;
                                                                                robotActionRetryCount = 0;
                                                                                robotVisionRetryCount = 0;

                                                                                if (loopCountOfTowerBasePoints.Count > 0 & (loopCountOfTowerBasePoints.ContainsKey(pointIndex_) && loopCountOfTowerBasePoints[pointIndex_].Count >= Model.Process.CalibrationSpecs[Model.CalibrationSpecifications.TowerBasePoint].first))
                                                                                {
                                                                                    calibrationStep += 2;
                                                                                }
                                                                                else
                                                                                {
                                                                                    calibrationStep += 1;
                                                                                }
                                                                            }
                                                                        }
                                                                        break;
                                                                }
                                                            }

                                                            if (!result_)
                                                            {   // Abnormal case
                                                                CheckImageProcessRetry(calibrationStep, SubSequenceSteps.Prepare, CalibrationSteps.Failed, ErrorCode.FailedToSetTowerBasePointToRobot);
                                                            }
                                                        }
                                                        else if (IsOverDelayTime(Model.TimeoutOfTotalImageProcess, imageProcessTimeoutTick))
                                                        {
                                                            CheckImageProcessRetry(calibrationStep, SubSequenceSteps.Prepare, CalibrationSteps.Failed, ErrorCode.FailedToSetTowerBasePointToRobot);
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        case CalibrationSteps.AdjustTowerBasePoint1:
                        case CalibrationSteps.AdjustTowerBasePoint2:
                            {
                                int pointIndex_ = 0;
                                RobotSequenceCommands cmd_ = RobotSequenceCommands.AdjustTowerBasePoint1;

                                switch (pointIndex_ = (calibrationStep - CalibrationSteps.AdjustTowerBasePoint1) / (CalibrationSteps.AdjustTowerBasePoint2 - CalibrationSteps.AdjustTowerBasePoint1) + 1)
                                {
                                    case 1:
                                        cmd_ = RobotSequenceCommands.AdjustTowerBasePoint1;
                                        break;
                                    case 2:
                                        cmd_ = RobotSequenceCommands.AdjustTowerBasePoint2;
                                        break;
                                }

                                switch (calibrationSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (cycleStop)
                                            {
                                                if (robotSequenceManager.IsWaitForOrder)
                                                {
                                                    if (robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeBeforeReelHeightCheck))
                                                        robotTick = App.TickCount;

                                                    calibrationSubStep = SubSequenceSteps.Proceed;
                                                }
                                            }
                                            else
                                            {
                                                if (robotSequenceManager.IsWaitForOrder)
                                                {
                                                    if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                                    {
                                                        if (IsRobotActionRetryOver)
                                                        {
                                                            robotTick = App.TickCount;
                                                            robotActionRetryCount = 0;
                                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                                        }
                                                        else
                                                        {
                                                            calibrationSubStep = SubSequenceSteps.Proceed;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            if (cycleStop)
                                            {
                                                if (robotSequenceManager.IsWaitForOrder)
                                                {
                                                    if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                                    {
                                                        calibrationStep = CalibrationSteps.Done;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_) || cmddone_)
                                                {
                                                    calibrationSubStep = SubSequenceSteps.Post;
                                                }
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {
                                            switch (pointIndex_)
                                            {
                                                case 1:
                                                case 2:
                                                    {
                                                        if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                                        {
                                                            calibrationStep -= 1;
                                                        }
                                                        else if (robotSequenceManager.IsFailure)
                                                        {
                                                            throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        case CalibrationSteps.SetTowerBasePoint1:
                        case CalibrationSteps.CheckDisplacements:
                            {
                                int pointIndex_ = 0;
                                RobotSequenceCommands cmd_ = RobotSequenceCommands.ApplyTowerBasePoint1;

                                switch (pointIndex_ = (calibrationStep - CalibrationSteps.SetTowerBasePoint1) / (RobotSequenceCommands.ApplyTowerBasePoint2 - RobotSequenceCommands.ApplyTowerBasePoint1) + 1)
                                {
                                    case 1:
                                        cmd_ = RobotSequenceCommands.ApplyTowerBasePoint1;
                                        break;
                                    case 2:
                                        cmd_ = RobotSequenceCommands.ApplyTowerBasePoint2;
                                        break;
                                }

                                switch (calibrationSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (robotSequenceManager.IsWaitForOrder)
                                            {
                                                if (IsRobotActionRetryOver)
                                                {
                                                    robotTick = App.TickCount;
                                                    robotActionRetryCount = 0;
                                                    throw new PauseException<ErrorCode>(ErrorCode.FailedToSetTowerBasePointToRobot);
                                                }
                                                else
                                                {   // UPDATED: 20200512 (Marcus)
                                                    // Have to check guide point setting.
                                                    double x_ = 0, y_ = 0, z_ = 0, rz_ = 0;

                                                    if (loopCountOfTowerBasePoints[pointIndex_].Count > 0)
                                                    {
                                                        foreach (Coord3DField<double, double, double, double, double, double> item in loopCountOfTowerBasePoints[pointIndex_])
                                                        {
                                                            x_ += item.first;
                                                            y_ += item.second;
                                                            z_ += item.third;
                                                            rz_ += item.sixth;
                                                        }

                                                        // Use average value of collection
                                                        x_ /= loopCountOfTowerBasePoints[pointIndex_].Count;
                                                        y_ /= loopCountOfTowerBasePoints[pointIndex_].Count;
                                                        z_ /= loopCountOfTowerBasePoints[pointIndex_].Count;
                                                        rz_ /= loopCountOfTowerBasePoints[pointIndex_].Count;
                                                    }

                                                    if (SetTowerBasePointOffsetOfCart(cmd_, pointIndex_, x_, y_, z_, rz_))
                                                    {
                                                        calibrationSubStep = SubSequenceSteps.Proceed;
                                                    }
                                                    else
                                                    {
                                                        robotTick = App.TickCount;
                                                        robotActionRetryCount++;
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            if (robotSequenceManager.IsWaitForOrder)
                                            {
                                                if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                                {   // Check home position
                                                    if (robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeBeforeReelHeightCheck))
                                                    {
                                                        robotTick = App.TickCount;
                                                        calibrationSubStep = SubSequenceSteps.Post;
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {
                                            if (robotSequenceManager.IsWaitForOrder)
                                            {
                                                if (IsRobotActionDelayOver(delayOfRobotCommunicationRetry))
                                                {
                                                    if (robotSequenceManager.IsRobotAtWayPointByCommand(ref cmddone_))
                                                    {
                                                        switch (pointIndex_)
                                                        {
                                                            case 1:
                                                                {
                                                                    calibrationStep = CalibrationSteps.CheckTowerBasePoint2;
                                                                }
                                                                break;
                                                            case 2:
                                                                {
                                                                    calibrationStep = CalibrationSteps.PostCalibration;
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    else if (robotSequenceManager.IsFailure)
                                                    {
                                                        throw new PauseException<ErrorCode>(ErrorCode.RobotMoveTimeout);
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        case CalibrationSteps.PostCalibration:
                            {   // UPDATED: 20200722 (Marcus)
                                switch (calibrationSubStep)
                                {
                                    case SubSequenceSteps.Prepare:
                                        {
                                            if (RequestTowerBasePointOffsetOfCart(RobotSequenceCommands.VerifyTowerBasePoints))
                                            {
                                                robotTick = App.TickCount;
                                                calibrationSubStep = SubSequenceSteps.Proceed;
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Proceed:
                                        {
                                            if (robotSequenceManager.IsWaitForOrder)
                                            {
                                                if (IsRobotActionRetryOver)
                                                {
                                                    robotTick = App.TickCount;
                                                    robotActionRetryCount = 0;
                                                    throw new PauseException<ErrorCode>(ErrorCode.FailedToSetTowerBasePointToRobot);
                                                }
                                                else if (robotSequenceManager.IsFailure)
                                                {
                                                    log = "Failed to calibrate coordination by vision.";
                                                    calibrated = false;
                                                    calibrationStep = CalibrationSteps.Failed;
                                                }
                                                else
                                                {
                                                    if (VerifyRobotResponseData(RobotSequenceCommands.VerifyTowerBasePoints, ref log) > 0)
                                                    {
                                                        calibrationSubStep = SubSequenceSteps.Post;
                                                    }
                                                    else if (IsRobotActionDelayOver(Convert.ToInt32(timeoutRobotCommunication * 0.01)))
                                                    {
                                                        robotActionRetryCount++;
                                                        calibrationSubStep = SubSequenceSteps.Prepare;
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case SubSequenceSteps.Post:
                                        {
                                            FireShowNotification($"Calibrated tower base points!{System.Environment.NewLine}{log}");
                                            stopCalibration = true;
                                            calibrated = true;
                                            calibrationStep = CalibrationSteps.Done;
                                        }
                                        break;
                                }

                            }
                            break;
                    }

                    Thread.Sleep(CONST_THREAD_POLLING_INTERVAL);
                }
            }
            catch (Exception ex)
            {
                calibrationStep = CalibrationSteps.Unknown;
                stopCalibration = true;
                Logger.Trace($"{GetType()}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }
            finally
            {
                robotSequenceManager.SendCommand(RobotSequenceCommands.GoToHomeBeforeReelHeightCheck);

                switch (calibrationStep)
                {
                    default:
                        { // Aborted by operator
                            Pause(ErrorCode.AbortCalibration, true);
                        }
                        break;
                    case CalibrationSteps.Done:
                        {
                            robotSequenceManager.ResetExecuteCommandToHome();
                            robotSequenceManager.RobotActionOrder = RobotActionOrder.None;
                            Stop();
                            ClearStates();

                            if (!(App.DigitalIoManager as DigitalIoManager).WorkZoneCartPresentSensor1 &
                                !(App.DigitalIoManager as DigitalIoManager).WorkZoneCartPresentSensor2)
                                currentCartDockingState = CartDockingStates.Unknown;
                        }
                        break;
                    case CalibrationSteps.Failed:
                    case CalibrationSteps.Unknown:
                        {
                            Pause(ErrorCode.FailedCalibration, true);
                        }
                        break;
                }
            }
        }

        protected void ResetCurrentProductionCountOfWorkSlot()
        {
            currentWorkSlotOfCart = 1;

            for (int i = 0; i < currentProductionCountOfWorkSlot.Count; i++)
                currentProductionCountOfWorkSlot[i] = 0;
        }

        protected void IncreateCartProductionCount(int cart)
        {
            if (cart > 0 && cart <= currentProductionCountOfWorkSlot.Count)
                currentProductionCountOfWorkSlot[cart - 1]++;
        }

        protected void SetCartProductionCount(int cart, int count)
        {
            if (cart > 0 && cart <= currentProductionCountOfWorkSlot.Count)
                currentProductionCountOfWorkSlot[cart - 1] = count;
        }

        protected int GetCartProductionCount(int cart)
        {
            if (cart > 0 && cart <= currentProductionCountOfWorkSlot.Count)
                return currentProductionCountOfWorkSlot[cart - 1];
            return -1;
        }

        protected bool FireTriggerVisionControl(Model.LightLevels level, Model.TaskSubCategories stype, ReelDiameters rtype, int workslot, bool cart, int delay = 500)
        {
            if (visionProcessLockState <= 0)
            {
                int lightcat = -1;
                int taskcat = -1;
                int optionmode = -1;
                double optioncoordx = 0;
                double optioncoordy = 0;
                double optioncoordz = 0;
                double optionanglerx = 1;
                double optionanglery = 1;
                double optionanglerz = 1;

                if (Model.DynamicLightControl)
                {
                    switch (stype)
                    {
                        case Model.TaskSubCategories.CheckCartType:
                            {
                                switch (level)
                                {
                                    case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart_top); break;
                                    case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart_middle); break;
                                    case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart_bottom); break;
                                }

                                taskcat = Model.GetTaskCategory(Model.TaskCategories.Check_cart_type);
                            }
                            break;
                        case Model.TaskSubCategories.CheckTowerBasePoint:
                            {
                                switch (workslot)
                                {
                                    case 1:
                                        {
                                            switch (level)
                                            {
                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Check_tower_base_point1_top); break;
                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Check_tower_base_point1_middle); break;
                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Check_tower_base_point1_bottom); break;
                                            }
                                        }
                                        break;
                                    case 2:
                                        {
                                            switch (level)
                                            {
                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Check_tower_base_point2_top); break;
                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Check_tower_base_point2_middle); break;
                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Check_tower_base_point2_bottom); break;
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        case Model.TaskSubCategories.CheckCartGuidePoint:
                            {
                                if (cart)
                                {
                                    switch (workslot)
                                    {
                                        case 1:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                        {
                                                            lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point1_top);
                                                        }
                                                        break;
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart13_guide_point1_top);
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 2:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                        {
                                                            lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point2_top);
                                                        }
                                                        break;
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart13_guide_point2_top);
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 3:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point3_top);
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 4:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point1_top);
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                    }

                                    taskcat = Model.GetTaskCategory(Model.TaskCategories.Check_cart_guide_point);
                                    optionmode = workslot;

                                    switch (rtype)
                                    {
                                        case ReelDiameters.ReelDiameter7:
                                            {
                                                // optioncoordx = Model.Process.Cart7GuidePointPitch.first;
                                                // optioncoordy = Model.Process.Cart7GuidePointPitch.second;
                                                // optionanglerz = Model.Process.Cart7GuidePointPitch.third;
                                            }
                                            break;
                                        default:
                                        case ReelDiameters.ReelDiameter13:
                                            {
                                                // optioncoordx = Model.Process.Cart13GuidePointPitch.first;
                                                // optioncoordy = Model.Process.Cart13GuidePointPitch.second;
                                                // optionanglerz = Model.Process.Cart13GuidePointPitch.third;
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        default:
                            {
                                if (cart)
                                {
                                    switch (currentWorkSlotOfCart)
                                    {
                                        case 1:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                        {
                                                            lightcat = Model.GetLightCategory(1, robotSequenceManager.PoseZ, Model.LightCategories.Cart7_workslot1_top, Model.LightCategories.Cart7_workslot1_bottom);
                                                        }
                                                        break;
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            lightcat = Model.GetLightCategory(1, robotSequenceManager.PoseZ, Model.LightCategories.Cart13_workslot1_top, Model.LightCategories.Cart13_workslot1_bottom);
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 2:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                        {
                                                            lightcat = Model.GetLightCategory(1, robotSequenceManager.PoseZ, Model.LightCategories.Cart7_workslot2_top, Model.LightCategories.Cart7_workslot2_bottom);
                                                        }
                                                        break;
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            lightcat = Model.GetLightCategory(1, robotSequenceManager.PoseZ, Model.LightCategories.Cart13_workslot2_top, Model.LightCategories.Cart13_workslot2_bottom);
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 3:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                        {
                                                            lightcat = Model.GetLightCategory(1, robotSequenceManager.PoseZ, Model.LightCategories.Cart7_workslot3_top, Model.LightCategories.Cart7_workslot3_bottom);
                                                        }
                                                        break;
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            lightcat = Model.GetLightCategory(1, robotSequenceManager.PoseZ, Model.LightCategories.Cart13_workslot3_top, Model.LightCategories.Cart13_workslot3_bottom);
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 4:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                        {
                                                            lightcat = Model.GetLightCategory(1, robotSequenceManager.PoseZ, Model.LightCategories.Cart7_workslot4_top, Model.LightCategories.Cart7_workslot4_bottom);
                                                        }
                                                        break;
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            lightcat = Model.GetLightCategory(1, robotSequenceManager.PoseZ, Model.LightCategories.Cart13_workslot4_top, Model.LightCategories.Cart13_workslot4_bottom);
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 5:
                                            {
                                                lightcat = Model.GetLightCategory(1, robotSequenceManager.PoseZ, Model.LightCategories.Cart7_workslot5_top, Model.LightCategories.Cart7_workslot5_bottom);
                                            }
                                            break;
                                        case 6:
                                            {
                                                lightcat = Model.GetLightCategory(1, robotSequenceManager.PoseZ, Model.LightCategories.Cart7_workslot6_top, Model.LightCategories.Cart7_workslot6_bottom);
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (rtype)
                                    {
                                        case ReelDiameters.ReelDiameter7:
                                            {
                                                lightcat = Model.GetLightCategory(1, robotSequenceManager.PoseZ, Model.LightCategories.Return7_top, Model.LightCategories.Return7_bottom);
                                            }
                                            break;
                                        case ReelDiameters.ReelDiameter13:
                                            {
                                                lightcat = Model.GetLightCategory(1, robotSequenceManager.PoseZ, Model.LightCategories.Return13_top, Model.LightCategories.Return13_bottom);
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                    }
                }
                else
                {
                    switch (stype)
                    {
                        case Model.TaskSubCategories.CheckCartType:
                            {
                                switch (level)
                                {
                                    case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart_top); break;
                                    case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart_middle); break;
                                    case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart_bottom); break;
                                }

                                taskcat = Model.GetTaskCategory(Model.TaskCategories.Check_cart_type);
                            }
                            break;
                        case Model.TaskSubCategories.CheckTowerBasePoint:
                            {
                                switch (workslot)
                                {
                                    case 1:
                                        {
                                            switch (level)
                                            {
                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Check_tower_base_point1_top); break;
                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Check_tower_base_point1_middle); break;
                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Check_tower_base_point1_bottom); break;
                                            }
                                        }
                                        break;
                                    case 2:
                                        {
                                            switch (level)
                                            {
                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Check_tower_base_point2_top); break;
                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Check_tower_base_point2_middle); break;
                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Check_tower_base_point2_bottom); break;
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        case Model.TaskSubCategories.CheckCartGuidePoint:
                            {
                                if (cart)
                                {
                                    switch (workslot)
                                    {
                                        case 1:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                        {
                                                            switch (level)
                                                            {
                                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point1_top); break;
                                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point1_top); break;
                                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point1_top); break;
                                                            }
                                                        }
                                                        break;
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            switch (level)
                                                            {
                                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart13_guide_point1_top); break;
                                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart13_guide_point1_top); break;
                                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart13_guide_point1_top); break;
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 2:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                        {
                                                            switch (level)
                                                            {
                                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point2_top); break;
                                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point2_top); break;
                                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point2_top); break;
                                                            }
                                                        }
                                                        break;
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            switch (level)
                                                            {
                                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart13_guide_point2_top); break;
                                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart13_guide_point2_top); break;
                                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart13_guide_point2_top); break;
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 3:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            switch (level)
                                                            {
                                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point3_top); break;
                                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point3_top); break;
                                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point3_top); break;
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 4:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            switch (level)
                                                            {
                                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point4_top); break;
                                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point4_top); break;
                                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Check_cart7_guide_point4_top); break;
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                    }

                                    taskcat = Model.GetTaskCategory(Model.TaskCategories.Check_cart_guide_point);
                                    optionmode = workslot;

                                    switch (rtype)
                                    {
                                        case ReelDiameters.ReelDiameter7:
                                            {
                                                // optioncoordx = Model.Process.Cart7GuidePointPitch.first;
                                                // optioncoordy = Model.Process.Cart7GuidePointPitch.second;
                                                // optionanglerz = Model.Process.Cart7GuidePointPitch.third;
                                            }
                                            break;
                                        default:
                                        case ReelDiameters.ReelDiameter13:
                                            {
                                                // optioncoordx = Model.Process.Cart13GuidePointPitch.first;
                                                // optioncoordy = Model.Process.Cart13GuidePointPitch.second;
                                                // optionanglerz = Model.Process.Cart13GuidePointPitch.third;
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        default:
                            {
                                if (cart)
                                {
                                    switch (currentWorkSlotOfCart)
                                    {
                                        case 1:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                        {
                                                            switch (level)
                                                            {
                                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot1_top); break;
                                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot1_middle); break;
                                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot1_bottom); break;
                                                            }
                                                        }
                                                        break;
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            switch (level)
                                                            {
                                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Cart13_workslot1_top); break;
                                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Cart13_workslot1_middle); break;
                                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Cart13_workslot1_bottom); break;
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 2:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                        {
                                                            switch (level)
                                                            {
                                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot2_top); break;
                                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot2_middle); break;
                                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot2_bottom); break;
                                                            }
                                                        }
                                                        break;
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            switch (level)
                                                            {
                                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Cart13_workslot2_top); break;
                                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Cart13_workslot2_middle); break;
                                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Cart13_workslot2_bottom); break;
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 3:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                        {
                                                            switch (level)
                                                            {
                                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot3_top); break;
                                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot3_middle); break;
                                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot3_bottom); break;
                                                            }
                                                        }
                                                        break;
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            switch (level)
                                                            {
                                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Cart13_workslot3_top); break;
                                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Cart13_workslot3_middle); break;
                                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Cart13_workslot3_bottom); break;
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 4:
                                            {
                                                switch (rtype)
                                                {
                                                    case ReelDiameters.ReelDiameter7:
                                                        {
                                                            switch (level)
                                                            {
                                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot4_top); break;
                                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot4_middle); break;
                                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot4_bottom); break;
                                                            }
                                                        }
                                                        break;
                                                    case ReelDiameters.ReelDiameter13:
                                                        {
                                                            switch (level)
                                                            {
                                                                case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Cart13_workslot4_top); break;
                                                                case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Cart13_workslot4_middle); break;
                                                                case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Cart13_workslot4_bottom); break;
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case 5:
                                            {
                                                switch (level)
                                                {
                                                    case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot5_top); break;
                                                    case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot5_middle); break;
                                                    case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot5_bottom); break;
                                                }
                                            }
                                            break;
                                        case 6:
                                            {
                                                switch (level)
                                                {
                                                    case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot6_top); break;
                                                    case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot6_middle); break;
                                                    case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Cart7_workslot6_bottom); break;
                                                }
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (rtype)
                                    {
                                        case ReelDiameters.ReelDiameter7:
                                            {
                                                switch (level)
                                                {
                                                    case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Return7_top); break;
                                                    case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Return7_middle); break;
                                                    case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Return7_bottom); break;
                                                }
                                            }
                                            break;
                                        case ReelDiameters.ReelDiameter13:
                                            {
                                                switch (level)
                                                {
                                                    case Model.LightLevels.Top: lightcat = Model.GetLightCategory(Model.LightCategories.Return13_top); break;
                                                    case Model.LightLevels.Middle: lightcat = Model.GetLightCategory(Model.LightCategories.Return13_middle); break;
                                                    case Model.LightLevels.Bottom: lightcat = Model.GetLightCategory(Model.LightCategories.Return13_bottom); break;
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                    }
                }

                if (cart)
                {
                    switch (stype)
                    {
                        case Model.TaskSubCategories.ProcessAtOnce:
                            {
                                switch (rtype)
                                {
                                    case ReelDiameters.ReelDiameter7:
                                        taskcat = Model.GetTaskCategory(Model.TaskCategories.Process_reel_7_on_cart);
                                        break;
                                    case ReelDiameters.ReelDiameter13:
                                        taskcat = Model.GetTaskCategory(Model.TaskCategories.Process_reel_13_on_cart);
                                        break;
                                }
                            }
                            break;
                        case Model.TaskSubCategories.Alignment:
                            {
                                switch (rtype)
                                {
                                    case ReelDiameters.ReelDiameter7:
                                        taskcat = Model.GetTaskCategory(Model.TaskCategories.Align_reel_7_on_cart);
                                        break;
                                    default:
                                    case ReelDiameters.ReelDiameter13:
                                        taskcat = Model.GetTaskCategory(Model.TaskCategories.Align_reel_13_on_cart);
                                        break;
                                }
                            }
                            break;
                        case Model.TaskSubCategories.DecodeBarcode:
                            {
                                switch (rtype)
                                {
                                    default:
                                    case ReelDiameters.ReelDiameter7:
                                    case ReelDiameters.ReelDiameter13:
                                        taskcat = Model.GetTaskCategory(Model.TaskCategories.Decode_barcode_on_cart);
                                        break;
                                }
                            }
                            break;
                        case Model.TaskSubCategories.CheckCartGuidePoint:
                            {
                                taskcat = Model.GetTaskCategory(Model.TaskCategories.Check_cart_guide_point);
                                optionmode = workslot;

                                switch (rtype)
                                {
                                    case ReelDiameters.ReelDiameter7:
                                        {
                                            // optioncoordx = Model.Process.Cart7GuidePointPitch.first;
                                            // optioncoordy = Model.Process.Cart7GuidePointPitch.second;
                                            // optioncoordz = Model.Process.Cart7GuidePointPitch.third;
                                            // optionanglerz = Model.Process.Cart13GuidePointPitch.sixth;
                                        }
                                        break;
                                    default:
                                    case ReelDiameters.ReelDiameter13:
                                        {
                                            // optioncoordx = Model.Process.Cart13GuidePointPitch.first;
                                            // optioncoordy = Model.Process.Cart13GuidePointPitch.second;
                                            // optioncoordz = Model.Process.Cart13GuidePointPitch.third;
                                            // optionanglerz = Model.Process.Cart13GuidePointPitch.sixth;
                                        }
                                        break;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    switch (stype)
                    {
                        case Model.TaskSubCategories.ProcessAtOnce:
                            {
                                switch (rtype)
                                {
                                    case ReelDiameters.ReelDiameter7:
                                        taskcat = Model.GetTaskCategory(Model.TaskCategories.Process_reel_7_on_stage);
                                        break;
                                    default:
                                    case ReelDiameters.ReelDiameter13:
                                        taskcat = Model.GetTaskCategory(Model.TaskCategories.Process_reel_13_on_stage);
                                        break;
                                }
                            }
                            break;
                        case Model.TaskSubCategories.Alignment:
                            {
                                switch (rtype)
                                {
                                    case ReelDiameters.ReelDiameter7:
                                        taskcat = Model.GetTaskCategory(Model.TaskCategories.Align_reel_7_on_stage);
                                        break;
                                    default:
                                    case ReelDiameters.ReelDiameter13:
                                        taskcat = Model.GetTaskCategory(Model.TaskCategories.Align_reel_13_on_stage);
                                        break;
                                }
                            }
                            break;
                        case Model.TaskSubCategories.DecodeBarcode:
                            {
                                switch (rtype)
                                {
                                    default:
                                    case ReelDiameters.ReelDiameter7:
                                    case ReelDiameters.ReelDiameter13:
                                        taskcat = Model.GetTaskCategory(Model.TaskCategories.Decode_barcode_on_stage);
                                        break;
                                }
                            }
                            break;
                        case Model.TaskSubCategories.CheckTowerBasePoint:
                            {
                                taskcat = Model.GetTaskCategory(Model.TaskCategories.Check_tower_base_point);
                                optionmode = workslot;
                                optioncoordx = Model.Process.GetTowerBasePointReference().first;
                                optioncoordy = Model.Process.GetTowerBasePointReference().second;
                                optioncoordz = Model.Process.GetTowerBasePointReference().third;
                                optionanglerz = Model.Process.GetTowerBasePointReference().sixth;
                            }
                            break;
                    }
                }

                if (lightcat < 0 || taskcat <= 0)
                    return false;

                VisionProcessEventArgs arg = new VisionProcessEventArgs(
                    lightcat,
                    delay,
                    taskcat,
                    rtype,
                    DistanceXOfAlignError,
                    DistanceYOfAlignError,
                    VisionProcessDataObjectTypes.ProcessArgument);
                arg.OptionMode = optionmode;
                arg.OptionCoordX = optioncoordx;
                arg.OptionCoordY = optioncoordy;
                arg.OptionCoordZ = optioncoordz;
                arg.OptionAngleRX = optionanglerx;
                arg.OptionAngleRY = optionanglery;
                arg.OptionAngleRZ = optionanglerz;

                imageProcessTimeoutTick = App.TickCount;
                visionProcessLastRunResult.Clear();
                ResetVisionProcessResultState();
                SetVisionProcessLockState(1); // Lock
                TriggerVisionControl?.Invoke(this, arg);
                return true;
            }

            return false;
        }

        protected void ResetVisionProcessResultState()
        {
            Interlocked.Exchange(ref visionProcessResultState, -1);
        }

        protected void CheckImageProcessRetry(ReelHandlerSteps callstep, SubSequenceSteps retrystep, ReelHandlerSteps returnstep = ReelHandlerSteps.None, ErrorCode errorcode = ErrorCode.None)
        {
            switch (callstep)
            {
                case ReelHandlerSteps.CheckCartReelType:
                    {
                        if (robotSequenceManager.SendCommand(RobotSequenceCommands.MoveToHome))
                        {
                            currentReelTypeOfCart = ReelDiameters.Unknown;

                            if (ReelTowerManager.HasUnloadRequest())
                            {
                                robotStep = returnstep;
                                Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.None);
                            }
                            else if (IsRobotVisionRetryOver)
                            {   // Raise alarm.
                                robotStep = returnstep;
                                throw new PauseException<ErrorCode>(errorcode);
                            }
                            else
                            {   // Retry
                                robotSubStep = retrystep;
                                robotVisionRetryCount++;
                                visionTick = App.TickCount;
                            }
                        }
                    }
                    break;
                case ReelHandlerSteps.CheckCartGuidePoint1:
                case ReelHandlerSteps.CheckCartGuidePoint2:
                case ReelHandlerSteps.CheckCartGuidePoint3:
                case ReelHandlerSteps.CheckCartGuidePoint4:
                    {
                        if (ReelTowerManager.HasUnloadRequest())
                        {
                            if (robotSequenceManager.SendCommand(RobotSequenceCommands.MoveToHome))
                            {
                                robotStep = returnstep;
                                Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.None);
                                Singleton<TransferMaterialObject>.Instance.SetState(TransferMaterialObject.TransferStates.None);
                            }
                        }
                        else if (IsRobotVisionRetryOver)
                        {   // Raise alarm.
                            robotStep = returnstep;
                            throw new PauseException<ErrorCode>(errorcode);
                        }
                        else
                        {   // Retry
                            robotSubStep = retrystep;
                            robotVisionRetryCount++;
                            visionTick = App.TickCount;
                        }
                    }
                    break;
                case ReelHandlerSteps.CheckReelAlignmentOnCart:
                    {
                        if (IsRobotVisionRetryOver)
                        {   // Set vision tick variable to retry after reset
                            visionTick = -1;
                            robotVisionRetryCount = 0;
                            robotVisionRetryCycleCount = 0;

                            // Force move to top of work slot
                            // Force move to home to prevent unwanted accident.
                            // Raise vision alarm 
                            if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                robotStep = returnstep;
                        }
                        else
                        {   // Turn on light to grab retry
                            visionTick = -1;
                            robotVisionRetryCount++;
                            robotVisionRetryCycleCount = 0;
                            robotSubStep = retrystep;
                        }
                    }
                    break;
                case ReelHandlerSteps.ReadBarcodeOnReel:
                    {
                        if (IsRobotVisionRetryOver)
                        {   // Retry
                            robotSubStep = retrystep;
                        }
                        else
                        {   // Failed to read barcode.
                            // Decode error
                            visionDecodeError++;
                            robotVisionRetryCycleCount = 0;

                            // UPDATED: 20200408 (Marcus)
                            // Set reject reel mode.
                            if (Config.EnableRejectReel)
                            {
                                TransferMaterialObject.TransferModes mode_ = TransferMaterialObject.TransferModes.PrepareToRejectCartReel;

                                switch (Singleton<TransferMaterialObject>.Instance.Mode)
                                {
                                    case TransferMaterialObject.TransferModes.PrepareToLoad:
                                        {
                                            mode_ = TransferMaterialObject.TransferModes.PrepareToRejectCartReel;
                                        }
                                        break;
                                    case TransferMaterialObject.TransferModes.PrepareToLoadReturn:
                                        {
                                            mode_ = TransferMaterialObject.TransferModes.PrepareToRejectReturnReel;
                                        }
                                        break;
                                }

                                CancelLoadProcedure();
                                Singleton<TransferMaterialObject>.Instance.SetMode(mode_);
                                robotStep = ReelHandlerSteps.CheckReelAlignmentAfterBarcodeInput;
                            }
                            else
                            {
                                FirePopupBarcodeInputWindow(reelBarcodeContexts);
                                robotStep = returnstep;
                            }
                        }
                    }
                    break;
                case ReelHandlerSteps.CheckReelAlignmentAfterBarcodeInput:
                    {
                        if (IsRobotVisionRetryOver)
                        {   // Failed to get alignment data
                            // Set vision tick variable to retry after reset
                            visionTick = -1;
                            robotVisionRetryCount = 0;
                            robotVisionRetryCycleCount = 0;

                            // Force move to top of work slot
                            // Force move to home to prevent unwanted accident.
                            // Raise vision alarm 
                            switch (Singleton<TransferMaterialObject>.Instance.Mode)
                            {
                                case TransferMaterialObject.TransferModes.PrepareToRejectCartReel:
                                case TransferMaterialObject.TransferModes.PrepareToLoad:
                                    {
                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.PrepareToRejectCartReel);

                                        if (robotSequenceManager.MoveToWorkSlotOfCart(currentReelTypeOfCart, currentWorkSlotOfCart))
                                        {
                                            robotStep = ReelHandlerSteps.MoveBackToHomeByVisionAlignmentFailureToLoad;
                                            CancelLoadProcedure();
                                        }
                                    }
                                    break;
                                case TransferMaterialObject.TransferModes.PrepareToRejectReturnReel:
                                case TransferMaterialObject.TransferModes.PrepareToLoadReturn:
                                    {
                                        Singleton<TransferMaterialObject>.Instance.SetMode(TransferMaterialObject.TransferModes.PrepareToRejectReturnReel);

                                        if (robotSequenceManager.MoveToFrontOfReturnStage(currentReelTypeOfReturn))
                                        {
                                            robotStep = ReelHandlerSteps.MoveBackToFrontOfReturnStageByVisionAlignmentFailureToLoadReturn;
                                            CancelLoadProcedure();
                                        }
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            visionTick = -1;
                            robotVisionRetryCount++;
                            robotVisionRetryCycleCount = 0;
                            robotSubStep = retrystep;   // Have to check.
                        }
                    }
                    break;
                case ReelHandlerSteps.CheckReelAlignmentOnReturnStage:
                    {
                        if (IsRobotVisionRetryOver)
                        {   // Set vision tick variable to retry after reset
                            visionTick = -1;
                            robotVisionRetryCount = 0;
                            robotVisionRetryCycleCount = 0;

                            // Force move to top of work slot
                            // Force move to home to prevent unwanted accident.
                            // Raise vision alarm
                            if (robotSequenceManager.ApproachToReturnStage(currentReelTypeOfReturn, previousReelTypeOfReturn))
                                robotStep = returnstep;
                        }
                        else
                        {   // Delay light turn on/off for 1 sec.
                            visionTick = -1;
                            robotVisionRetryCount++;
                            robotVisionRetryCycleCount = 0;
                            robotSubStep = retrystep;
                        }
                    }
                    break;
            }
        }

        protected void CheckImageProcessRetry(CalibrationSteps callstep, SubSequenceSteps retrystep, CalibrationSteps returnstep = CalibrationSteps.Failed, ErrorCode errorcode = ErrorCode.None)
        {
            switch (callstep)
            {
                case CalibrationSteps.CheckTowerBasePoint1:
                case CalibrationSteps.CheckTowerBasePoint2:
                    {
                        if (IsRobotVisionRetryOver)
                        {   // Raise alarm.
                            calibrationStep = returnstep;
                            throw new PauseException<ErrorCode>(errorcode);
                        }
                        else
                        {   // Retry
                            calibrationSubStep = retrystep;
                            robotVisionRetryCount++;
                            visionTick = App.TickCount;
                        }
                    }
                    break;
            }
        }

        protected void DoIdleStateProcess()
        {
            if (currentLoadReelTower <= 0 &&
                currentUnloadReelTower <= 0 &&
                !robotSequenceManager.HasAReel &&
                robotSequenceManager.IsWaitForOrder &&
                robotController.IsRunnable)
            {
                if (Config.AutomaticHomeInIdleTime)
                {
                    if (systemIdleStateTick == -1)
                        systemIdleStateTick = App.TickCount;
                    else if (IsOverDelayTime(Config.DelayOfAutomaticHomeInIdleTime, systemIdleStateTick))
                    {
                        if (!robotSequenceManager.IsRobotAtHome)
                        {
                            robotSequenceManager.SendCommand(RobotSequenceCommands.MoveToHome);
                            Singleton<MaterialPackageManager>.Instance.RemoveAllMaterialPackages();
                            systemIdleStateTick = -1;
                        }
                    }
                }
                else
                    systemIdleStateTick = -1;
            }
            else
            {
                systemIdleStateTick = -1;
            }
        }

        protected void ReportEvent(string message)
        {
            if (Config.RemoteReportServer == null)
                return;

            new TaskFactory().StartNew(new Action<object>((x_) =>
            {
                try
                {
                    if (x_ != null)
                    {
                        using (UdpClient client_ = new UdpClient())
                        {
                            int sentBytes_ = 0;
                            byte[] packet_ = Encoding.ASCII.GetBytes(x_.ToString());

                            if ((sentBytes_ = client_.Send(packet_, packet_.Length, Config.RemoteReportServer)) > 0)
                                Debug.WriteLine($"Report> ({sentBytes_}):{x_.ToString()}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
            }), message);
        }
        #endregion

        #region Public methods
        public void Create()
        {
            LoadParameters();
            LoadAlarmList();
            
            robotController.Create(Config.RobotRemoteServerAddress, Config.RobotRemoteServerPort);
            robotSequenceManager.Create(Config.RobotLocalServerPort);
            mobileRobotManager.Create(Config.MobileRobotLocalServerPort);
            latestVersion = App.IsLatestVersion(1, 0, 1, 0);
        }

        public void StartElements()
        {
            if (ReelTowerManager != null)
            {
                Singleton<MaterialPackageManager>.Instance.RemovedMaterialPackage += OnRemovedMaterialPackage;
                ReelTowerManager.Init(Config.ReelTowerIds);
                ReelTowerManager.TimeoutOfTowerResponse = Config.TimeoutOfReelTowerResponse;
                ReelTowerManager.Start(Config.ReelTowerLocalServerPort);
            }
        }

        public void StopElements()
        {
            if (ReelTowerManager != null)
            {
                Singleton<MaterialPackageManager>.Instance.RemovedMaterialPackage -= OnRemovedMaterialPackage;
                ReelTowerManager.Stop();
            }
        }

        public void CheckRobotControllerProgram()
        {
            robotController.CheckProgramState();
        }

        public bool Start()
        {
            if (operationState == OperationStates.Run)
                return true;

            if (processThreads.Count > 0 || !initialized)
            {
                FormMessageExt.ShowWarning("System is not ready to run state.");
                return false;
            }

            operationState = OperationStates.Run;
            FireOperationStateChanged();
            ClearStates();

            if (StartTasks())
            {
                ReportEvent("Run;Ready;");
                return true;
            }
            else
            {
                Stop();
                return false;
            }
        }

        public bool Stop()
        {
            StopTasks();

            if (operationState == OperationStates.Stop)
                return true;

            if (cycleStop)
                TryCycleStop(!cycleStop);

            operationState = OperationStates.Stop; 
            FireOperationStateChanged();

            if (IsRequiredReset)
                Reset();
            else
                ReportEvent($"Idle;Completed;");

            return true;
        }

        public void TryCycleStop(bool state = true)
        {
            if (cycleStop != state)
            {
                cycleStop = state;
                FireCycleStopStateChanged();
            }
        }

        public void FireOperationStateChanged()
        {
            OperationStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void FireOperationModeChanged()
        {
            OperationModeChanged?.Invoke(this, EventArgs.Empty);
        }

        public void FireFinishedCycleStop()
        {
            FinishedCycleStop?.Invoke(this, EventArgs.Empty);
        }

        public void FireCycleStopStateChanged()
        {
            CycleStopOrderStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void FireCartPresentStateChanged(CartPresentStates state)
        {
            CartPresentStateChanged?.Invoke(this, cartPresnetState = state);
        }

        public void FireChagedReelSizeOfCart(ReelDiameters reeltype)
        {
            if (reeltype > ReelDiameters.Unknown)
                ChangedReelSizeOfCart?.Invoke(this, Convert.ToInt32(reeltype));
        }

        public void ClearStates()
        {
            robotController.Reset();
            robotSequenceManager.Reset();
        }

        public bool Initialize()
        {
            bool firstTry = (App.OperationState == OperationStates.PowerOn);

            if (App.OperationState == OperationStates.PowerOn || App.OperationState == OperationStates.Stop)
            {
                if (IsRequiredReset)
                {
                    FormMessageExt.ShowWarning(Properties.Resources.String_Warning_Reset_Alarm);
                    return false;
                }

                ClearStates();
                systemInitialized = false;
                operationState = OperationStates.Initialize;
                ResetSequenceSteps(true);
                FireOperationStateChanged();

                if ((new FormInitialization(InitializeMode.All).ShowDialog()) == DialogResult.OK)
                {   // Reset the steps of runnable threads
                    (App.MainForm as FormMain).SetFocus();
                    return true;
                }
                else
                {
                    if (firstTry || !App.Initialized)
                        operationState = OperationStates.PowerOn;
                }
            }
            else if (operationState == OperationStates.Initialize)
            {
                Stop();
            }

            return false;
        }

        public bool Calibrate()
        {
            if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
            {
                FormMessageExt.ShowWarning(Properties.Resources.String_Warning_Release_CartInWorkArea);
            }
            else
            {
                if ((App.OperationState == OperationStates.PowerOn || App.OperationState == OperationStates.Stop) && !robotSequenceManager.HasAReel && robotSequenceManager.IsWaitForOrder)
                {
                    if (IsRequiredReset)
                    {
                        FormMessageExt.ShowWarning(Properties.Resources.String_Warning_Reset_Alarm);
                        return false;
                    }

                    ClearStates();
                    operationState = OperationStates.Setup;
                    ResetSequenceSteps(true);
                    FireOperationStateChanged();

                    if ((new FormCalibration(CalibrationMode.TowerBasePoints).ShowDialog()) == DialogResult.OK)
                    {   // Reset the steps of runnable threads
                        (App.MainForm as FormMain).SetFocus();
                        return true;
                    }
                }

                Stop();
            }

            return false;
        }

        public bool Reset()
        {
            if (operationState == OperationStates.Run)
                return false;

            try
            {
                ResetState();
                RecheckInterlockState();

                // Turn off buzzer
                App.DigitalIoManager.SetOutput((int)DigitalIoManager.Outputs.Buzzer, false);

                if (operationState != OperationStates.Stop)
                    Stop();

                if (cycleStop)
                {
                    FormMessageExt.ShowWarning(Properties.Resources.String_Warning_Reset_Cycle_Stop);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                CheckOperationPanelState();
            }

            return false;
        }

        public void ResetSequenceSteps(bool initialize = false)
        {   // Reset process step to ready.
            // Recover reel tower sequence step
            reelTowerStep = ReelTowerSteps.Ready;

            // Recover robot sequence step
            robotSubStep = SubSequenceSteps.Prepare;
        }

        public bool Resume()
        {
            if (!systemInitialized || operationState != OperationStates.Stop || IsRequiredReset)
                return false;

            operationState = OperationStates.Run;
            FireOperationStateChanged();
            return true;
        }

        public void FinishCycleStop()
        {
            cycleStop = false;
            Stop();
        }

        public void PauseByCycleStop()
        {
            try
            {
                if (operationState == OperationStates.Alarm)
                    return;

                operationState = OperationStates.Alarm;
                new Thread(FireFinishedCycleStop).Start();
            }
            catch (Exception ex)
            {
                Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public void FireUpdateAlarmLog(string text = null)
        {
            ReportAlarmLog?.Invoke(this, text);
        }

        
        public void Pause(ErrorCode code, bool forcedpause = false)
        {
            try
            {
                if (operationState == OperationStates.Alarm)
                    return;

                string message_ = $"ALARM={code}";
                string unloadinfo_ = string.Empty;
                string alarmlog_ = string.Empty;

                switch (code)
                {
                    case ErrorCode.RobotVisionFailure:
                        {   // Important check.
                            // Actually, barcode contexts should be empty.
                            // Barcode contexts are loaded after alignment value gathering.
                            // You need check actual message sent.
                            CancelLoadProcedure();
                            visionAlignError++;
                        }
                        break;
                    case ErrorCode.RobotVisionDecodeBarcodeFailure:
                        {
                            visionDecodeError++;
                        }
                        break;
                    case ErrorCode.FailedToPickupReelFromCart:
                        {
                            loadErrorCountValue++;
                        }
                        break;
                    case ErrorCode.FailedToPickupReelFromReturn:
                        {
                            returnErrorCountValue++;
                        }
                        break;
                    case ErrorCode.FailedToPickUnloadReel:
                        {
                            unloadinfo_ = $"TOWER={currentUnloadReelTowerId}";

                            if (currentUnloadReelState.PendingData != null)
                                unloadinfo_ += $"\nUID={currentUnloadReelState.PendingData.Name}";

                            unloadErrorCountValue++;
                        }
                        break;
                }

                operationState = OperationStates.Alarm;
                FireOperationStateChanged();

                if (alarmList.ContainsKey(code))
                {
                    message_ = string.Format("ALARM={0}({1:0000})\n", alarmList[code].first, Convert.ToInt32(code));
                    message_ += string.Format("DESCRIPTION={0}", alarmList[code].second);

                    if (!string.IsNullOrEmpty(unloadinfo_))
                        message_ += unloadinfo_;

                    Logger.Alarm(alarmlog_ = $"Alarm={Convert.ToInt32(code):0000}({alarmList[code].first}),Desc={alarmList[code].second}");
                }

                ReportEvent($"Alarm;{Convert.ToInt32(code)};");

                if (Config.EnableOneshotRecovery && !forcedpause)
                    FormMessageExt.ShowAlertWithRecovery(message_);
                else
                    FormMessageExt.ShowAlert(message_);
                
                if (code == ErrorCode.FailedToPickUnloadReel)
                {
                    if (ReelHanlderAMM.Get_Twr_State_Reel("AJ54100", currentUnloadReelTowerId) == "ON")
                    {
                        bool reelcheck = true;

                        while (reelcheck)
                        {
                            FormMessageExt.ShowAlertWithRecovery(message_);
                            reelcheck = ReelHanlderAMM.Get_Twr_State_Reel("AJ54100", currentUnloadReelTowerId) == "ON";
                        }
                    }

                    reelsOfOutputStages[currentUnloadReelState.OutputStageIndex - 1].SetMaterial(currentUnloadReelState.Uid, ReelUnloadReportStates.Complete);

                    if (reelsOfOutputStages[currentUnloadReelState.OutputStageIndex - 1].IsFinished())
                        reelsOfOutputStages[currentUnloadReelState.OutputStageIndex - 1].PickState = ReelUnloadReportStates.Complete;
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public void TryRobotControllerConnect(string address, int port)
        {
            if (!robotController.IsConnected)
                robotController.TryClientConnecting(address, port);
        }

        public void Init(InitializeMode mode)
        {
            StopInit();

            initializeMode = mode;
            initialized = false;
            robotInitializeStep = InitializeSteps.CheckProgramState;
            initializeThreads.Add("RobotController", new Thread(RunRobotHome));
            initializeThreads["RobotController"].Start();
        }

        public void AutoTeach(AutomaticTeachMode mode)
        {
        }

        public void CalDevices(CalibrationMode mode)
        {
            StopCalDevices();

            calibrationMode = mode;
            calibrated = false;
            calibrationStep = CalibrationSteps.PrepareCalibration;
            calibrationThreads.Add("TowerBasePoints", new Thread(RunCalibration));
            calibrationThreads["TowerBasePoints"].Start();
        }

        public void StopInit()
        {
            if (initializeThreads.ContainsKey("RobotController"))
            {
                stopRobotInitialize = true;
                initializeThreads["RobotController"].Join();
                initializeThreads["RobotController"] = null;
                initializeThreads.Remove("RobotController");
            }

            stopRobotInitialize = false;
        }

        public void StopAutoTech()
        {

        }

        public void StopCalDevices()
        {
            if (calibrationThreads.ContainsKey("TowerBasePoints"))
            {
                stopCalibration = true;
                calibrationThreads["TowerBasePoints"].Join();
                calibrationThreads["TowerBasePoints"] = null;
                calibrationThreads.Remove("TowerBasePoints");
            }

            stopCalibration = false;
        }

        public void SetBarcodeInputData(BarcodeKeyInData src)
        {
            lock (barcodeKeyInData)
                barcodeKeyInData.CopyFrom(src);
        }

        public void SetDockState()
        {
        }

        public void SetUndockState()
        {

        }

        public void ForceSetCartState(bool dock = true)
        {
            bool forceDock = false;

            if (dock)
            {
                switch (currentReelTypeOfCart)
                {
                    case ReelDiameters.Unknown:
                        {   // Already placed in cart detection.
                            if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                            {
                                currentWorkSlotOfCart = 1;
                                currentCartDockingState = CartDockingStates.LoadCompleted;
                                mobileRobotStep = MobileRobotSteps.Ready;
                                forceDock = true;
                            }
                        }
                        break;
                    case ReelDiameters.ReelDiameter7:
                        {
                            if (currentWorkSlotOfCart <= 6 && (App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                            {
                                currentCartDockingState = CartDockingStates.LoadCompleted;
                                mobileRobotStep = MobileRobotSteps.Ready;
                                forceDock = true;
                            }
                        }
                        break;
                    case ReelDiameters.ReelDiameter13:
                        {
                            if (currentWorkSlotOfCart <= 4 && (App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                            {
                                currentCartDockingState = CartDockingStates.LoadCompleted;
                                mobileRobotStep = MobileRobotSteps.Ready;
                                forceDock = true;
                            }
                        }
                        break;
                }

                if (forceDock)
                {
                    (App.DigitalIoManager as DigitalIoManager).DockCart();
                    requiredVisionMarkAdjustment = Config.EnableVisionMarkAdjustment;
                }
            }
            else
            {
                currentReelTypeOfCart = ReelDiameters.Unknown;
                currentWorkSlotOfCart = 1;
                currentCartDockingState = CartDockingStates.UnloadCompleted;
                mobileRobotStep = MobileRobotSteps.Ready;
                (App.DigitalIoManager as DigitalIoManager).UndockCart();
            }
        }

        public virtual void LoadParameters()
        {
            timeOfReturnReelPresentValidation = Model.DelayOfReturnReelSensing;
            delayOfImageProcessing = Model.DelayOfImageProcessing;
            delayOfUnloadReelStateUpdate = Model.DelayOfUnloadReelStateUpdate;
            delayOfMaterialPackageUpdate = Model.DelayOfMaterialPackageUpdate;
            retryLimitOfRobotVisionCheckAttempts = Model.RetryOfVisionAttempts;
            retryLimitOfRobotVisionCheck = Model.RetryOfVisionFailure;
            distanceXOfAlignError = Model.AlignmentRangeLimit.X;
            distanceYOfAlignError = Model.AlignmentRangeLimit.Y;
        }

        public async void LoadAlarmList(FileExtensions ext = FileExtensions.Xml)
        {
            try
            {
                string filePath_ = string.Format(@"{0}Config\Alarmlist", App.Path);
                alarmList.Clear();

                switch (ext)
                {
                    case FileExtensions.Xml:
                        {
                            if (File.Exists(filePath_ += ".xml"))
                            {
                                int aid = 0;
                                string name = string.Empty;
                                string desc = string.Empty;

                                XmlDocument xml = new XmlDocument();
                                xml.Load(filePath_);

                                foreach (XmlNode element_ in xml.DocumentElement.ChildNodes)
                                {
                                    switch (element_.Name.ToLower())
                                    {
                                        case "alarm":
                                            {
                                                aid = int.Parse(element_.Attributes["id"].Value);
                                                name = element_.Attributes["name"].Value;

                                                foreach (XmlNode child in element_.ChildNodes)
                                                {
                                                    if (child.Attributes["culture"].Value == App.CultureInfoCode)
                                                    {
                                                        desc = child.InnerText;
                                                        break;
                                                    }
                                                }

                                                if (!alarmList.ContainsKey((ErrorCode)aid))
                                                    alarmList.Add((ErrorCode)aid, new Pair<string, string>(name, desc));
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    case FileExtensions.csv:
                        {
                            if (File.Exists(filePath_ += ".csv"))
                            {
                                using (FileStream fs = new FileStream(filePath_, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                {
                                    using (StreamReader sw = new StreamReader(fs, Encoding.UTF8))
                                    {
                                        string contents_ = await sw.ReadToEndAsync();
                                        string[] tokens_ = contents_.Split('\n');

                                        foreach (string token_ in tokens_)
                                        {
                                            int numeric_ = -1;
                                            string[] items_ = token_.Split(';');

                                            if (items_.Length >= 3)
                                            {
                                                if (int.TryParse(items_[0], out numeric_))
                                                {
                                                    ErrorCode code_ = (ErrorCode)Enum.Parse(typeof(ErrorCode), items_[0]);

                                                    if (!alarmList.ContainsKey(code_))
                                                        alarmList.Add(code_, new Pair<string, string>(items_[1], items_[2]));
                                                }
                                            }
                                        }

                                        sw.Close();
                                    }

                                    fs.Close();
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public virtual void StopRobotController()
        {
            if (robotController.IsConnected)
                robotController.SendCommand(RobotControllerCommands.StopProgram);
        }

        public virtual void ValidateOutputStageFullSignal()
        {
            MoveToBackwardReelsInOutputStage();

            if (!(App.DigitalIoManager as DigitalIoManager).OutputStage1Exist)
            {
                outputStage1Full = false;
                
                if (reelsOfOutputStages.ContainsKey(0))
                    reelsOfOutputStages[0].Clear();

                if (reelsOfOutputStages.ContainsKey(3) && reelsOfOutputStages[3].PickState == ReelUnloadReportStates.Complete && (App.DigitalIoManager as DigitalIoManager).OutputStage4Exist)
                    MoveToForwardReelsInOutputStage(4);
            }

            if (!(App.DigitalIoManager as DigitalIoManager).OutputStage2Exist)
            {
                outputStage2Full = false;

                if (reelsOfOutputStages.ContainsKey(1))
                    reelsOfOutputStages[1].Clear();

                if (reelsOfOutputStages.ContainsKey(4) && reelsOfOutputStages[4].PickState == ReelUnloadReportStates.Complete && (App.DigitalIoManager as DigitalIoManager).OutputStage5Exist)
                    MoveToForwardReelsInOutputStage(5);
            }

            if (!(App.DigitalIoManager as DigitalIoManager).OutputStage3Exist)
            {
                outputStage3Full = false;

                if (reelsOfOutputStages.ContainsKey(2))
                    reelsOfOutputStages[2].Clear();

                if (reelsOfOutputStages.ContainsKey(5) && reelsOfOutputStages[5].PickState == ReelUnloadReportStates.Complete && (App.DigitalIoManager as DigitalIoManager).OutputStage6Exist)
                    MoveToForwardReelsInOutputStage(6);
            }
        }

        int rejectAsignCheck = 0;
        public virtual void ValidateRejectStageFullSignal()
        {
            if (!rejectStageFull && (App.DigitalIoManager as DigitalIoManager).RejectStageFull)
            {
                rejectAsignCheck++;
                if (rejectAsignCheck == 100)
                {
                    rejectStageFull = (App.DigitalIoManager as DigitalIoManager).RejectStageFull;
                    lock (reelsOfRejectStages)
                        reelsOfRejectStages.Clear();

                    lastRejectStageFullState = rejectStageFull;
                    rejectAsignCheck = 0;
                }
            }
            else if (rejectStageFull && !(App.DigitalIoManager as DigitalIoManager).RejectStageFull)
            {
                rejectStageFull = (App.DigitalIoManager as DigitalIoManager).RejectStageFull;
                lock (reelsOfRejectStages)
                    reelsOfRejectStages.Clear();

                lastRejectStageFullState = rejectStageFull;
                rejectAsignCheck = 0;
            }
            else if (!rejectStageFull && !(App.DigitalIoManager as DigitalIoManager).RejectStageFull)
            {
                rejectAsignCheck = 0;
            }



            //if ((rejectStageFull = (App.DigitalIoManager as DigitalIoManager).RejectStageFull) != lastRejectStageFullState && lastRejectStageFullState)
            //{
            //    lock (reelsOfRejectStages)
            //        reelsOfRejectStages.Clear();

            //    lastRejectStageFullState = rejectStageFull;
            //}
        }

        /// <summary>
        /// Validate return reel state by periodical I/O polling.
        /// </summary>
        public virtual void ValidateReturnReelPresentSignal()
        {
            lock (reelSensingStates)
            {
                if ((App.DigitalIoManager as DigitalIoManager).ReturnStageReelPresentSensor13 && !(App.DigitalIoManager as DigitalIoManager).ReturnStageReelPresentSensor7)
                {
                    if (reelSensingStates.Count <= 0)
                    {
                        reelSensingStates.Add(new Pair<int, ReelDiameters>(App.TickCount, ReelDiameters.ReelDiameter13));
                        currentReelTypeOfReturn = ReelDiameters.Unknown;
                    }
                    else
                    {
                        if (!robotSequenceManager.HasAReel && !robotSequenceManager.IsMoving)
                        {
                            switch (reelSensingStates[0].second)
                            {
                                case ReelDiameters.ReelDiameter7:
                                    {
                                        currentReelTypeOfReturn = ReelDiameters.Unknown;
                                        reelSensingStates.Clear();
                                        // UPDATED : 20201020 (JM.Choi)
                                        // ReelSensor와 ReelDiameter를 동일한 크기로 검출하기 위해 Add 제외
                                        // ReelSensor가 13으로 감지되면 ReelDiameter는 13으로 고정됨
                                        //reelSensingStates.Add(new Pair<int, ReelDiameters>(App.TickCount, ReelDiameters.ReelDiameter7));
                                    }
                                    break;
                                case ReelDiameters.ReelDiameter13:
                                    {
                                        if (IsOverDelayTime(timeOfReturnReelPresentValidation, reelSensingStates[0].first))
                                            currentReelTypeOfReturn = ReelDiameters.ReelDiameter13;
                                    }
                                    break;
                            }
                        }
                    }
                }
                else if ((App.DigitalIoManager as DigitalIoManager).ReturnStageReelPresentSensor7 && !(App.DigitalIoManager as DigitalIoManager).ReturnStageReelPresentSensor13)
                {
                    if (reelSensingStates.Count <= 0)
                    {
                        reelSensingStates.Add(new Pair<int, ReelDiameters>(App.TickCount, ReelDiameters.ReelDiameter7));
                        currentReelTypeOfReturn = ReelDiameters.Unknown;
                    }
                    else
                    {
                        if (!robotSequenceManager.HasAReel && !robotSequenceManager.IsMoving)
                        {
                            switch (reelSensingStates[0].second)
                            {
                                case ReelDiameters.ReelDiameter7:
                                    {
                                        if (IsOverDelayTime(timeOfReturnReelPresentValidation, reelSensingStates[0].first))
                                            currentReelTypeOfReturn = ReelDiameters.ReelDiameter7;
                                    }
                                    break;
                                case ReelDiameters.ReelDiameter13:
                                    {
                                        currentReelTypeOfReturn = ReelDiameters.Unknown;
                                        reelSensingStates.Clear();
                                        // UPDATED : 20201020 (JM.Choi)
                                        // ReelSensor와 ReelDiameter를 동일한 크기로 검출하기 위해 Add 제외
                                        // ReelSensor가 7로 감지되면 ReelDiameter는 7로 고정됨
                                        //reelSensingStates.Add(new Pair<int, ReelDiameters>(App.TickCount, ReelDiameters.ReelDiameter13));
                                    }
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    if (!robotSequenceManager.HasAReel && !robotSequenceManager.IsMoving)
                        currentReelTypeOfReturn = ReelDiameters.Unknown;

                    reelSensingStates.Clear();
                }
            }
        }

        public void SetVisionProcessResult(VisionProcessDataObject result)
        {
            if (result != null)
                visionProcessLastRunResult.CopyFrom(result);

            Interlocked.Exchange(ref visionProcessResultState, 1);
        }

        public void SetVisionProcessLockState(int state)
        {
            Interlocked.Exchange(ref visionProcessLockState, state);
        }

        public void SetPickList(MaterialPackage pkg)
        {
            lock (reelsOfOutputStages)
            {
                if (!reelsOfOutputStages.ContainsKey(pkg.OutputPort - 1))
                    reelsOfOutputStages.Add(pkg.OutputPort - 1, new MaterialPackage(pkg));
                else
                    reelsOfOutputStages[pkg.OutputPort - 1].CopyFrom(pkg);
            }
        }

        public bool CleanUpRejectStage(int index = 1)
        {
            bool result_ = false;

            if (!(App.DigitalIoManager as DigitalIoManager).RejectStageFull)
            {
                reelsOfRejectStages.Clear();
                result_ = true;
            }

            return result_;
        }

        public bool CleanUpOutputStage(int index = 1)
        {
            bool result_ = false;

            switch (index)
            {
                default:
                    return result_;
                case 1:
                    {
                        if (!(App.DigitalIoManager as DigitalIoManager).OutputStage1Exist)
                            result_ = true;
                    }
                    break;
                case 2:
                    {
                        if (!(App.DigitalIoManager as DigitalIoManager).OutputStage2Exist)
                            result_ = true;
                    }
                    break;
                case 3:
                    {
                        if (!(App.DigitalIoManager as DigitalIoManager).OutputStage3Exist)
                            result_ = true;
                    }
                    break;
                case 4:
                    {
                        if (!(App.DigitalIoManager as DigitalIoManager).OutputStage4Exist)
                            result_ = true;
                    }
                    break;
                case 5:
                    {
                        if (!(App.DigitalIoManager as DigitalIoManager).OutputStage5Exist)
                            result_ = true;
                    }
                    break;
                case 6:
                    {
                        if (!(App.DigitalIoManager as DigitalIoManager).OutputStage6Exist)
                            result_ = true;
                    }
                    break;
            }

            if (result_ && reelsOfOutputStages.ContainsKey(index - 1))
            {
                lock (reelsOfOutputStages)
                    reelsOfOutputStages[index - 1].Clear();
            }

            return result_;
        }
        #endregion
    }
}
#endregion