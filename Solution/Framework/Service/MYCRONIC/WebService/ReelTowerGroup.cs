#region Imports
using TechFloor.Device;
using TechFloor.Object;
using TechFloor.Components;
using TechFloor.Components.Elements;
using RoyoTech.StSys.WebService.ComTowerApp.Contract;
using RoyoTech.StSys.WebService.ComTowerApp.Contract.Models;
using RoyoTech.StSys.WebService.SharedModel;
using RoyoTech.StSys.WebService.SharedService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
#endregion

#region Program
#pragma warning disable CS0628
namespace TechFloor.Service.MYCRONIC.WebService
{
    public class ReelTowerGroup
    {
        #region Enumerations
        public enum AlarmCodes
        {
            Tray_16_44_not_released                                     = 100,  // Tray 16, 44 not released
            Reel_height_exceeded                                        = 101,  // Reel height exceeded
            Reel_not_recognized                                         = 102,  // Reel not recognized
            Tray_not_recognized                                         = 103,  // Tray not recognized
            Reel_diameter_too_big                                       = 104,  // Reel diameter too big
            Reel_diameter_not_recognized                                = 105,  // Reel diameter not recognized
            No_scancode_Barcode_not_detected_during_test_run            = 106,  // No scancode, Barcode not detected during test run
            No_scancode_Barcode_not_detected_when_loading_carrier       = 107,  // No scancode, Barcode not detected when loading carrier
            No_scancode_Barcode_not_detected_when_unloading_carrier     = 108,  // No scancode, Barcode not detected when unloading carrier
            No_scancode_Barcode_not_detected_after_power_on_sequence    = 109,  // No scancode, Barcode not detected after power on sequence
            Invalid_data                                                = 116,  // Invalid data
            Operation_denied                                            = 117,  // Operation denied
            Code_error                                                  = 118,  // Code error
            Error_arm_motor                                             = 175,  // Error arm motor
            Arm_sensor_defect                                           = 176,  // Arm sensor defect
            No_connection_between_arm_sensor_and_motor                  = 177,  // No connection between arm sensor and motor
            Error_arm_sensor                                            = 178,  // Error arm sensor
            Slot_difference                                             = 179,  // Slot difference
            Incorrect_scanner_scanner                                   = 183,  // Incorrect scanner firmware
            Incorrect_arm_firmware                                      = 184,  // Incorrect arm firmware
            Humidity_logger_not_responding                              = 185,  // Humidity logger not responding
            Fault_arm_motor                                             = 186,  // Fault arm motor
            Error_voltage_3_3V                                          = 187,  // Error voltage 3.3V
            Error_voltage_60V                                           = 188,  // Error voltage 60V
            Rotor_motor_not_recognized                                  = 189,  // Rotor motor not recognized
            Lift_motor_not_recognized                                   = 190,  // Lift motor not recognized
            Invalid_reel_height                                         = 191,  // Invalid reel height
            Gripper_too_high                                            = 192,  // Gripper too high
            Gripper_too_low                                             = 193,  // Gripper too low
            Arm_not_recognized_while_calibrating_lift                   = 194,  // Arm not recognized while calibrating lift
            Arm_not_recognized_while_calibrating_rotor                  = 195,  // Arm not recognized while calibrating rotor
            Invalid_stack                                               = 196,  // Invalid stack
            Missing_or_invalid_firmware_in_rotor_motor                  = 197,  // Missing or invalid firmware in rotor motor
            Missing_or_invalid_firmware_in_lift_motor                   = 198,  // Missing or invalid firmware in lift motor
            Lift_step_error                                             = 201,  // Lift step error
            Command_not_recognized_by_lift_motor                        = 204,  // Command not recognized by lift motor
            Lift_sensor_error_encoder                                   = 205,  // Lift sensor error encoder
            Lift_motor_invalid_data                                     = 208,  // Lift motor invalid data
            Lift_motor_unknown_error                                    = 209,  // Lift motor unknown error
            Sensor_error_encoder                                        = 215,  // Sensor error encoder
            Command_recognized_by_rotor_motor                           = 217,  // Command not recognized by rotor motor
            Rotot_motor_invalid_data                                    = 218,  // Rotor motor invalid data
            Rotot_motor_unknown_error                                   = 219,  // Rotor motor unknown error
            Error_lid_while_opening                                     = 221,  // Error lid while opening
            Error_lid_opening_top                                       = 222,  // Error lid opening top
            Error_lid_closing_top                                       = 224,  // Error lid clising top
            Error_lid_closing_bottom                                    = 225,  // Error lid closing bottom
            Lid_open_while_driving                                      = 229,  // Lid open while driving
            No_response_from_arm_motor                                  = 230,  // No response from arm motor
            Error_clamping_on_closing_1                                 = 231,  // Error clamping on closing 1
            Error_clamping_on_closing_2                                 = 232,  // Error clamping on closing 2
            Error_clamping_on_opening_1                                 = 233,  // Error clamping on opening 1
            Error_clamping_on_opening_2                                 = 234,  // Error clamping on opening 2
            Error_arm_init_to_front                                     = 235,  // Error arm init to front
            Error_arm_init_to_back                                      = 236,  // Error arm init to back
            Error_arm_blocked                                           = 237,  // Error arm blocked
            Arm_not_in_stand_still                                      = 238,  // Arm not stand still
            Arm_board_not_recogninzed                                   = 239,  // Arm board not recognized
            Scanner_board_not_recogninzed                               = 240,  // Scanner board not recognized
            Error_clamping_on_init_1                                    = 241,  // Error clamping on init 1
            Error_clamping_on_init_2                                    = 242,  // Error clamping on init 2
            Error_clamping_while_driving_1                              = 243,  // Error clamping while driving 1
            Error_clamping_while_driving_2                              = 244,  // Error clamping while driving 2
            Missing_or_invalid_firmware_in_arm_motor                    = 245,  // Missing or invliad firmware in arm motor
            No_tableau_on_magazine_calibration                          = 246,  // No tableau on magazine calibration
            No_screening_control_rotor                                  = 247,  // No screening control rotor
            No_screening_control_arm                                    = 248,  // No screening control arm
            Error_reel_sensor                                           = 249,  // Error reel sensor
            Error_lid                                                   = 250,  // Error lid
            Error_arm                                                   = 251,  // Error arm
            Error_clamping                                              = 252,  // Error clamping
            Error_scanner_setting                                       = 253,  // Error scanner setting
            Calibration_of_diameter_failed                              = 254,  // Calibration of diameter failed
            Calibration_of_height_failed                                = 255,  // Calibration of height failed
            Invalid_scan_code                                           = 777,  // Invalid scan code
            Communication_timeout                                       = 999,  // Communication timeout
        }

        public enum NotificationCodes
        {
            Unknown = -1,
            WakeUpWebService,
            RequestLoad,
            RequestUnload,
            RequestCarrierData,
            RequestComponentData,
            NotifyTowerStatus,
            NotifyStSysStarted,
            NotifyCarrierLoad,
            NotifyCarrierUnload,
            NotifyCarrierNew,
            NotifyCarrierDelete,
            NotifyComponentNew,
            NotifyComponentDelete,
            NotifyJoblistStateChanged,
            NotifyTowerHumidity,
            NotifyCarrierMSLreached,
            NotifyCarrierExpired
        }

        public enum WebServiceMethods
        {
            Unknown,
            GetTowers,
            GetProdSites,
            GetArticleList,
            GetCarrierList,
            GetJobLists,
            GetTowerInformation,
            GetArticleInformation,
            GetArticleInformationByID,
            DeleteArticle,
            GetCarrierInformation,
            DeleteCarrier,
            GetJobList,
            DeleteJobList,
            CancelJobList,
            StartJobList,
            NewArticle,
            UpdateArticle,
            NewCarrier,
            UpdateCarrier,
            UnloadCarrier,
            ProvideItem,
            UpdateJobList,
            NewJobList,
            SimulateStartButton,
        }
        #endregion

        #region Constants
        protected readonly string CONST_SPECIAL_MATERIAL_DATA_FILE          = "Data\\SpecialMaterials.xml";

        protected readonly string CONST_RESERVED_JOB_DATA_FILE              = "Data\\ReservedJobs.xml";

        protected readonly string CONST_ALARM_LIST_FILE                     = "Config\\Alarmlist.xml";

        protected readonly string CONST_REELTOWERGROUP_CRYPTO_SEED          = "crypto@";
                                                                            
        protected readonly char[] CONST_REEL_INFORMATION_DELIMITER          = { ';' };

        protected const int CONST_DEFAULT_TOWER_STATE_QUERY_INTERVAL        = 3000;
        #endregion

        protected sealed class ClientChannel
        {
            #region Fields
            protected readonly object thislock                              = new object();

            protected ReelTowerWebServiceReference.WSInterfaceClient client = new ReelTowerWebServiceReference.WSInterfaceClient();
            #endregion

            #region Properties
            public ReelTowerWebServiceReference.WSInterfaceClient Client    => client;

            public EndpointAddress Address                                  => Client.Endpoint.Address;

            public string AddressToString                                   => Address.ToString();

            public bool IsConnected                                         => client.WebServiceIsConnected();
            #endregion

            #region Protected methods
            protected string CreateCommand(string cmd, object arg = null)
            {
                XmlCommand cmd_ = new XmlCommand();
                cmd_.Command    = cmd;

                if (arg != null)
                {
                    switch ((WebServiceMethods)Enum.Parse(typeof(WebServiceMethods), cmd))
                    {
                        default:
                            return string.Empty;
                        case WebServiceMethods.GetTowers:
                        case WebServiceMethods.GetProdSites:
                        case WebServiceMethods.GetArticleList:
                        case WebServiceMethods.GetCarrierList:
                        case WebServiceMethods.GetJobLists:
                            break;
                        case WebServiceMethods.GetTowerInformation:
                        case WebServiceMethods.GetArticleInformation:
                        case WebServiceMethods.GetArticleInformationByID:
                        case WebServiceMethods.DeleteArticle:
                        case WebServiceMethods.GetCarrierInformation:
                        case WebServiceMethods.DeleteCarrier:
                        case WebServiceMethods.GetJobList:
                        case WebServiceMethods.DeleteJobList:
                        case WebServiceMethods.CancelJobList:
                        case WebServiceMethods.StartJobList:
                            cmd_.Parameter = Convert.ToString(arg);
                            break;
                        case WebServiceMethods.NewArticle:
                        case WebServiceMethods.UpdateArticle:
                            cmd_.Parameter = XmlCommandSerializer.Serialize(arg as ArticleInformation);
                            break;
                        case WebServiceMethods.NewCarrier:
                        case WebServiceMethods.UpdateCarrier:
                            cmd_.Parameter = XmlCommandSerializer.Serialize(arg as CarrierInformation);
                            break;
                        case WebServiceMethods.UnloadCarrier:
                            cmd_.Parameter = XmlCommandSerializer.Serialize(arg as CarrierProvideInformation);
                            break;
                        case WebServiceMethods.ProvideItem:
                            cmd_.Parameter = XmlCommandSerializer.Serialize(arg as ProvideItem);
                            break;
                        case WebServiceMethods.UpdateJobList:
                        case WebServiceMethods.NewJobList:
                            cmd_.Parameter = XmlCommandSerializer.Serialize(arg as JobListInformation);
                            break;
                        case WebServiceMethods.SimulateStartButton:
                            cmd_.Parameter = XmlCommandSerializer.Serialize(arg as LoadMaterialInformation);
                            break;
                    }
                }

                return XmlCommandSerializer.Serialize(cmd_);
            }

            protected bool ExecuteCommand(string cmd, string xml, ref object data, ref AlarmData alarm)
            {
                bool result_ = false;

                try
                {
                    if (IsConnected)
                    {
                        lock (thislock)
                        {
                            XElement ret_ = client.XmlAction(xml);
                            XmlResult res_ = XmlCommandSerializer.DeserializeResult(ret_.ToString());

                            switch (res_.Errorcode)
                            {
                                case 0:
                                    {
                                        switch ((WebServiceMethods)Enum.Parse(typeof(WebServiceMethods), cmd))
                                        {
                                            default:
                                                break;
                                            case WebServiceMethods.GetTowers:
                                                data = XmlCommandSerializer.DeserializeTowerBaseInformationList(res_.Data);
                                                break;
                                            case WebServiceMethods.GetTowerInformation:
                                                data = XmlCommandSerializer.DeserializeTowerDetailInformation(res_.Data);
                                                break;
                                            case WebServiceMethods.GetProdSites:
                                                data = XmlCommandSerializer.DeserializeDepotInformationList(res_.Data);
                                                break;
                                            case WebServiceMethods.GetArticleList:
                                                data = XmlCommandSerializer.DeserializeArticleInformationList(res_.Data);
                                                break;
                                            case WebServiceMethods.GetArticleInformation:
                                            case WebServiceMethods.GetArticleInformationByID:
                                                data = XmlCommandSerializer.DeserializeArticleInformation(res_.Data);
                                                break;
                                            case WebServiceMethods.GetCarrierList:
                                                data = XmlCommandSerializer.DeserializeCarrierInformationList(res_.Data);
                                                break;
                                            case WebServiceMethods.GetCarrierInformation:
                                                data = XmlCommandSerializer.DeserializeCarrierInformation(res_.Data);
                                                break;
                                            case WebServiceMethods.GetJobLists:
                                                data = XmlCommandSerializer.DeserializeJobLists(res_.Data);
                                                break;
                                            case WebServiceMethods.GetJobList:
                                                data = XmlCommandSerializer.DeserializeJobList(res_.Data);
                                                break;
                                            case WebServiceMethods.NewArticle:
                                            case WebServiceMethods.UpdateArticle:
                                            case WebServiceMethods.DeleteArticle:
                                            case WebServiceMethods.NewCarrier:
                                            case WebServiceMethods.UpdateCarrier:
                                            case WebServiceMethods.DeleteCarrier:
                                            case WebServiceMethods.NewJobList:
                                            case WebServiceMethods.UpdateJobList:
                                            case WebServiceMethods.DeleteJobList:
                                            case WebServiceMethods.CancelJobList:
                                            case WebServiceMethods.UnloadCarrier:
                                            case WebServiceMethods.ProvideItem:
                                            case WebServiceMethods.StartJobList:
                                            case WebServiceMethods.SimulateStartButton:
                                                break;
                                        }

                                        result_ = true;
                                    }
                                    break;
                                default:
                                    {
                                        alarm = new AlarmData(res_.Errorcode, res_.Message);
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        alarm = new AlarmData(999, "WebService is not connected.");
                    }
                }
                catch (Exception ex)
                {
                    alarm = new AlarmData(-1, ex.Message);
                }

                return result_;
            }
            #endregion

            #region Public methods
            public object DoAction(string cmd, object arg, ref int error, ref string msg)
            {
                object obj_ = null;

                try
                {
                    AlarmData alarmData_    = null;
                    string xml_             = CreateCommand(cmd, arg);

                    if (!string.IsNullOrEmpty(xml_))
                        ExecuteCommand(cmd, xml_, ref obj_, ref alarmData_);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }

                return obj_;
            }
            #endregion
        }

        #region Fields
        protected bool initialized                                                           = false;

        protected bool autoRun                                                              = false;

        protected bool manualOperation                                                      = false;

        protected bool ignorePastReservedJobs                                               = false;

        protected int pollingTowerStatusIndex                                               = 0;

        protected int id;

        protected int reservedProvideJobCount                                               = 1;

        protected int processedProvideJobCount                                              = 1;

        protected int lastTowerStateQueryTick                                               = 0;

        protected string currentProvidingJobname                                            = string.Empty;

        protected string site;

        protected string linecode;

        protected string groupname;

        protected string accountName                                                        = string.Empty;

        protected string currentOperator                                                    = string.Empty;

        protected string lastReservedProvideJob                                             = string.Empty;

        protected string reservedJobFile                                                    = string.Empty;

        protected string cultureCode                                                        = "en-US";

        protected int totalOccurredAlarmCounts                                              = 0;

        protected int totalPendingAlarmCounts                                               = 0;

        protected DateTime lastAlarmOccurredTime                                            = DateTime.MinValue;

        protected TimeSpan alarmTimeSpan                                                    = TimeSpan.Zero;

        protected UserGroup accountGid                                                      = UserGroup.Operator;

        protected DeviceManufacturer maker;

        protected Customers customer;

        protected DateTime lastReservedProvideJobTimestamp                                  = DateTime.MinValue;

        protected AlarmManager alarmManager                                                 = new AlarmManager(App.CultureInfoCode);

        protected Dictionary<string, int> towerIds                                          = new Dictionary<string, int>();

        protected Dictionary<int, ReelTower> towers                                         = new Dictionary<int, ReelTower>();

        protected Dictionary<int, ReelTowerState> states                                    = new Dictionary<int, ReelTowerState>();

        protected Dictionary<string, DatabaseManager> databaseManagers                      = new Dictionary<string, DatabaseManager>();

        protected Dictionary<string, Pair<int, int>> articles                               = new Dictionary<string, Pair<int, int>>();

        protected Dictionary<string, Pair<int, ProvideMaterialData>> carriers               = new Dictionary<string, Pair<int, ProvideMaterialData>>();

        protected Dictionary<string, Pair<string, ProvideMaterialData>> pendingUnloadReels  = new Dictionary<string, Pair<string, ProvideMaterialData>>();

        protected Dictionary<string, ThreeField<string, MaterialData, bool>> loadReels                      = new Dictionary<string, ThreeField<string, MaterialData, bool>>();

        protected Dictionary<string, FourField<string, string, int, ProvideMaterialData>> unloadReels       = new Dictionary<string, FourField<string, string, int, ProvideMaterialData>>();

        protected List<Pair<string, ProvideJobListData>> provideJobs                        = new List<Pair<string, ProvideJobListData>>();

        protected Dictionary<string, ProvideJobListData> reservedJobs                       = new Dictionary<string, ProvideJobListData>();

        protected Dictionary<string, List<ThreeField<int, string, DateTime>>> alarmRecords  = new Dictionary<string, List<ThreeField<int, string, DateTime>>>();

        protected Dictionary<string, string> specialMaterials                                  = new Dictionary<string, string>();

        protected DataTable ammpickingid                                                    = null;
        #endregion

        #region Properties
        public virtual bool IsInitialized => initialized;

        public virtual bool AutoRun
        {
            get => autoRun;
            set => autoRun = value;
        }

        public virtual bool ManualOperation
        {
            get => manualOperation;
            set => manualOperation = value;
        }

        public virtual bool IsAllTowerOffline
        {
            get
            {
                if (TowerStates.Count <= 0)
                    return true;

                var val_ = TowerStates.Select(x_ => x_.Value).Where(y_ => y_.OnlineState).ToList();

                if (val_.Count > 0)
                    return false;

                return true;
            }
        }

        public virtual bool IsWaitToProvide => provideJobs.Count > 0;

        public virtual bool IsProviding
        {
            get
            {
                if (provideJobs.Count <= 0)
                {
                    currentProvidingJobname = string.Empty;
                    return false;
                }

                var job_ = provideJobs.Find(x_ => x_.second.State >= ProvideJobListData.States.Providing);
                if (job_ != null)
                {
                    currentProvidingJobname = job_.first;
                    return true;
                }

                return false;
            }
        }

        public virtual int TotalDailyOccurredAlarms                                                         => totalOccurredAlarmCounts;

        public virtual int TotalPendingAlarms                                                               => totalPendingAlarmCounts;

        public virtual int Id                                                                               => id;

        public virtual string Site                                                                          => site;

        public virtual string AccountName                                                                   => accountName;

        public virtual string CurrentOperator                                                               => currentOperator;

        public virtual string LastReservedProvideJob                                                        => lastReservedProvideJob;

        public virtual DateTime LastOccurredAlarmDateTime                                                   => lastAlarmOccurredTime;

        public virtual TimeSpan TotalOccurredAlarmTime                                                      => alarmTimeSpan;

        public virtual UserGroup AccountGid                                                                 => accountGid;

        public virtual DeviceManufacturer Maker                                                             => maker;

        public virtual Customers Customer                                                                   => customer;

        public virtual IReadOnlyDictionary<string, int> TowerIds                                            => towerIds;

        public virtual IReadOnlyDictionary<int, ReelTower> Towers                                                    => towers;

        public virtual IReadOnlyDictionary<int, ReelTowerState> TowerStates                                 => states;

        public virtual IReadOnlyDictionary<string, Pair<string, ProvideMaterialData>> PendingUnloadReels    => pendingUnloadReels;

        public virtual IReadOnlyDictionary<string, ThreeField<string, MaterialData, bool>> LoadReels        => loadReels;

        public virtual IReadOnlyList<Pair<string, ProvideJobListData>> ProvideJobs                          => provideJobs;

        public virtual IReadOnlyDictionary<string, ProvideJobListData> ReservedJobs                         => reservedJobs;

        public virtual DatabaseManager ComponentDB
        {
            get
            {
                if (databaseManagers != null && databaseManagers.ContainsKey("ComponentDatabase"))
                    return databaseManagers["ComponentDatabase"];
                else
                    throw new NotImplementedException("Component database is not exist");
            }
        }

        public virtual DatabaseManager AccountDB
        {
            get
            {
                if (databaseManagers != null && databaseManagers.ContainsKey("AccountDatabase"))
                    return databaseManagers["AccountDatabase"];
                else
                    throw new NotImplementedException("Account database is not exist");
            }
        }

        public virtual string LineCode                              => linecode;

        public virtual string GroupName                             => groupname;

        public virtual bool CancelJob(string jobname)               => CancelJobList(jobname);

        public virtual bool RemoveJob(string jobname, bool flush = true)               => DeleteJobList(jobname, flush);

        public virtual bool RemoveArticle(string articlename)       => DeleteArticle(articlename);

        public virtual bool RemoveCarrier(string carriername)       => DeleteCarrier(carriername);

        public virtual string CreateProvideJobName(string prefix)   => $"{prefix}{reservedProvideJobCount++:00000000}";

        public virtual string AllTowerNameList
        {
            get
            {
                string namelist_ = string.Empty;

                foreach (string name_ in towerIds.Keys)
                    namelist_ += $"{name_};";

                return string.IsNullOrEmpty(namelist_)? string.Empty : namelist_.Remove(namelist_.Length - 1);
            }
        }

        public virtual bool IsOverDelayTime(int delay, int tick)    => (TimeSpan.FromMilliseconds(App.TickCount - tick).TotalMilliseconds >= delay);

        public virtual bool IsManualOperationMode                   => manualOperation;

        public virtual Dictionary<string, string> SpecialMaterials     => specialMaterials;

        public AMM.AMM ReelTowerAMM                         = new AMM.AMM();

        public DataTable AMMPickingID                                  => ammpickingid;

        public MaterialStorageState.StorageOperationStates paststate;

        public Dictionary<string, MaterialStorageState.StorageOperationStates> paststate_ = new Dictionary<string, MaterialStorageState.StorageOperationStates>();

        public bool process_state = false;

        public string CurrentProvidingJobname => currentProvidingJobname;
        #endregion

        #region Events
        public virtual event EventHandler<ReelTowerState> ReelTowerStateChanged;

        public virtual event EventHandler<MaterialEventArgs> MaterialEventRaised;

        public virtual event EventHandler<ProvideJobListData> ProvideJobStateChanged;

        public virtual event EventHandler<ProvideMaterialData> ProvideMaterialStateChanged;

        public virtual event EventHandler<string> ReportRuntimeLog;

        public virtual event EventHandler<string> ReportException;

        public virtual event EventHandler<FiveField<bool, string, string, int, string>> ReportAlarmLog;
        #endregion

        #region Constructors
        protected ReelTowerGroup() { }
        #endregion

        #region Protected methods
        protected virtual bool ClearAlarm(string towerid, int alarmcode)
        {
            bool result_ = false;

            try
            {
                if (alarmRecords.ContainsKey(towerid))
                {
                    lock (alarmRecords)
                    {
                        ThreeField<int, string, DateTime> item_ = alarmRecords[towerid].Find(x_ => x_.first == alarmcode);

                        if (item_ != null)
                        {
                            alarmTimeSpan.Add(DateTime.Now - item_.third);
                            alarmRecords[towerid].Remove(item_);
                            result_ = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        protected virtual bool SetAlarm(string towerid, int alarmcode, string alarmmessage = null)
        {
            bool result_ = false;

            try
            {
                if (alarmRecords.ContainsKey(towerid) && !alarmRecords[towerid].Where(x_ => x_.first == alarmcode).Any())
                {
                    if (lastAlarmOccurredTime.Date != DateTime.Now.Date)
                        totalOccurredAlarmCounts = 0;

                    lock (alarmRecords)
                    {
                        ThreeField<int, string, DateTime> item_ = alarmRecords[towerid].Find(x_ => x_.first == alarmcode);

                        if (item_ == null)
                        {
                            alarmRecords[towerid].Add(new ThreeField<int, string, DateTime>(alarmcode, alarmmessage, DateTime.Now));
                            totalPendingAlarmCounts++;
                            result_ = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        protected virtual void AsyncUpdateTowerState(string towerid, bool ammusage = false)
        {
            new TaskFactory().StartNew(new Action<object>((x_) => {
                TowerDetailInformation info_ = null;
                try
                {
                    if (x_ != null && GetTowerInformation(Convert.ToString(x_), ref info_))
                    {
                        ReelTowerState obj_ = null;
                        UpdateTowerStatus(info_, ref obj_);

                        if (!paststate_.ContainsKey(obj_.Name))
                            paststate_.Add(obj_.Name, MaterialStorageState.StorageOperationStates.Unknown);

                        if (paststate_.ContainsKey("null"))
                            paststate_.Remove("null");

                        if (obj_.HasAlarm)
                        {
                            obj_.State = MaterialStorageState.StorageOperationStates.Error;

                            if (ammusage && obj_.State != paststate_[obj_.Name])
                            {
                                //SetEqStatus("ALARM");
                                //SetEqEvent();
                                paststate_[obj_.Name] = obj_.State;
                            }
                        }
                        else
                        {
                            string towername_ = GetTowerIdByName(obj_.Id);

                            if (ammusage && obj_.State != paststate_[obj_.Name])
                            {
                                paststate_[obj_.Name] = obj_.State;

                                switch (obj_.State)
                                {
                                    case MaterialStorageState.StorageOperationStates.Idle:
                                        {
                                            bool idle_check = true;
                                            foreach (var stat in paststate_)
                                            {
                                                if (stat.Value.ToString() != "Idle")
                                                    idle_check = false;
                                            }
                                            if (idle_check)
                                            {
                                                if (process_state)
                                                    SetEqStatus("READY");
                                                else
                                                    SetEqStatus("IDLE");
                                            }
                                        }
                                        break;
                                }

                            }
                            if (obj_ != null && obj_.StatusText.ToLower() == "ready" && obj_.OnlineState)
                            {
                                switch (obj_.State)
                                {
                                    case MaterialStorageState.StorageOperationStates.Unknown:
                                    case MaterialStorageState.StorageOperationStates.Idle:
                                    case MaterialStorageState.StorageOperationStates.Run:
                                    case MaterialStorageState.StorageOperationStates.Down:
                                    case MaterialStorageState.StorageOperationStates.Error:
                                    case MaterialStorageState.StorageOperationStates.Wait:
                                    case MaterialStorageState.StorageOperationStates.Full:
                                        break;
                                    case MaterialStorageState.StorageOperationStates.RequestedToLoad:
                                    case MaterialStorageState.StorageOperationStates.PrepareToLoad:
                                    case MaterialStorageState.StorageOperationStates.Load:
                                        {
                                            switch (obj_.MaterialDestination)
                                            {
                                                case MaterialStorageState.MaterialHandlingDestination.LoadToStorage:
                                                case MaterialStorageState.MaterialHandlingDestination.UnloadToOutStage:
                                                case MaterialStorageState.MaterialHandlingDestination.UnloadToReject:
                                                    break;
                                                case MaterialStorageState.MaterialHandlingDestination.None:
                                                    obj_.State = MaterialStorageState.StorageOperationStates.Idle;
                                                    break;
                                            }
                                            paststate_[obj_.Name] = MaterialStorageState.StorageOperationStates.Load;
                                        }
                                        break;
                                    case MaterialStorageState.StorageOperationStates.RequestedToUnload:
                                    case MaterialStorageState.StorageOperationStates.PrepareToUnload:
                                    case MaterialStorageState.StorageOperationStates.Unload:
                                        {
                                            switch (obj_.MaterialDestination)
                                            {
                                                case MaterialStorageState.MaterialHandlingDestination.LoadToStorage:
                                                case MaterialStorageState.MaterialHandlingDestination.UnloadToOutStage:
                                                case MaterialStorageState.MaterialHandlingDestination.UnloadToReject:
                                                    break;
                                                case MaterialStorageState.MaterialHandlingDestination.None:
                                                    obj_.State = MaterialStorageState.StorageOperationStates.Idle;
                                                    break;
                                            }
                                            paststate_[obj_.Name] = MaterialStorageState.StorageOperationStates.Unload;
                                        }
                                        break;
                                    case MaterialStorageState.StorageOperationStates.Abort:
                                        {
                                            obj_.State = MaterialStorageState.StorageOperationStates.Idle;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
            }), towerid);
        }

        protected virtual void CheckCarrierInformation(string carriername, bool asynchronous = true)
        {
            if (!carriers.ContainsKey(carriername))
            {
                if (asynchronous)
                {
                    new Thread((obj) =>
                    {
                        if (obj != null)
                        {
                            CarrierInformation result_ = null;
                            GetCarrierInformation(obj.ToString(), ref result_);
                        }
                    }).Start(carriername);
                }
                else
                {
                    CarrierInformation result_ = null;
                    GetCarrierInformation(carriername, ref result_);
                }
            }
        }

        protected virtual bool GetTowers(ref List<TowerBaseInformation> data)
        {
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            object obj_             = client_.DoAction(MethodBase.GetCurrentMethod().Name, null, ref error_, ref msg_);

            if (obj_ != null)
                data = obj_ as List<TowerBaseInformation>;

            return obj_ != null;
        }

        protected virtual bool GetTowerInformation(string towerid, ref TowerDetailInformation data)
        {
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            object obj_             = client_.DoAction(MethodBase.GetCurrentMethod().Name, towerid, ref error_, ref msg_);

            if (obj_ != null)
                data = obj_ as TowerDetailInformation;

            return obj_ != null;
        }

        protected virtual bool GetProdSites(ref DepotInformation data)
        {
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            object obj_             = client_.DoAction(MethodBase.GetCurrentMethod().Name, null, ref error_, ref msg_);

            if (obj_ != null)
                data = obj_ as DepotInformation;

            return obj_ != null;
        }

        protected virtual bool GetArticleList(ref List<ArticleInformation> data)
        {
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            object obj_             = client_.DoAction(MethodBase.GetCurrentMethod().Name, null, ref error_, ref msg_);

            if (obj_ != null)
                data = obj_ as List<ArticleInformation>;

            return obj_ != null;
        }

        protected virtual bool GetArticleInformation(string articlename, ref ArticleInformation data)
        {
            bool result_ = false;
            int error_  = 0;
            string msg_ = string.Empty;

            try
            {
                ClientChannel client_ = new ClientChannel();
                object obj_ = client_.DoAction(MethodBase.GetCurrentMethod().Name, articlename, ref error_, ref msg_);

                if (obj_ != null)
                {
                    data = obj_ as ArticleInformation;

                    if (articles.ContainsKey(data.Article))
                        UpdateArticleInformation(data.Article, data.ID, data.ReelCount);
                    else if (error_ == 0)
                        AddNewArticleInformation(data.Article, data.ID, data.ReelCount);

                    result_ = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        protected virtual bool GetArticleInformationByID(string articleid, ref ArticleInformation data)
        {
            bool result_ = false;
            int error_  = 0;
            string msg_ = string.Empty;

            try
            {
                ClientChannel client_ = new ClientChannel();
                object obj_ = client_.DoAction(MethodBase.GetCurrentMethod().Name, articleid, ref error_, ref msg_);

                if (obj_ != null)
                {
                    data = obj_ as ArticleInformation;

                    if (articles.ContainsKey(data.Article))
                        UpdateArticleInformation(data.Article, data.ID, data.ReelCount);
                    else if (error_ == 0)
                        AddNewArticleInformation(data.Article, data.ID, data.ReelCount);

                    result_ = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        protected virtual bool DeleteArticle(string articlename)
        {
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            client_.DoAction(MethodBase.GetCurrentMethod().Name, articlename, ref error_, ref msg_);
            return (error_ == 0);
        }

        protected virtual bool NewArticle(ArticleInformation article)
        {
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            client_.DoAction(MethodBase.GetCurrentMethod().Name, article, ref error_, ref msg_);
            return (error_ == 0);
        }

        protected virtual bool UpdateArticle(ArticleInformation article)
        {
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            client_.DoAction(MethodBase.GetCurrentMethod().Name, article, ref error_, ref msg_);
            return (error_ == 0);
        }

        protected virtual bool GetCarrierList(ref List<CarrierInformation> data)
        {
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            object obj_          = client_.DoAction(MethodBase.GetCurrentMethod().Name, null, ref error_, ref msg_);

            if (obj_ != null)
                data = obj_ as List<CarrierInformation>;

            return obj_ != null;
        }

        protected virtual bool GetCarrierInformation(string carriername, ref CarrierInformation data)
        {
            bool result_ = false;
            int error_  = 0;
            string msg_ = string.Empty;

            try
            {
                ClientChannel client_ = new ClientChannel();
                object obj_ = client_.DoAction(MethodBase.GetCurrentMethod().Name, carriername, ref error_, ref msg_);

                if (obj_ != null)
                {
                    data = obj_ as CarrierInformation;
                    

                    if (carriers.ContainsKey(data.Carrier))
                        UpdateCarrierInformation(data);
                    else if (error_ == 0)
                        AddNewCarrierInformation(data);
                    
                    if (string.IsNullOrEmpty(data.Depot) || data.Depot == null || data.Depot[0] != 'T' || data.Depot == "0")
                        result_ = false;
                    else
                        result_ = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        protected virtual bool DeleteCarrier(string carriername)
        {
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            client_.DoAction(MethodBase.GetCurrentMethod().Name, carriername, ref error_, ref msg_);
            return (error_ == 0);
        }

        protected virtual bool NewCarrier(CarrierInformation carrier)
        {
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            client_.DoAction(MethodBase.GetCurrentMethod().Name, carrier, ref error_, ref msg_);
            return (error_ == 0);
        }

        protected virtual bool UpdateCarrier(CarrierInformation carrier)
        {
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            client_.DoAction(MethodBase.GetCurrentMethod().Name, carrier, ref error_, ref msg_);
            return (error_ == 0);
        }

        protected virtual void ImportComponentRecords()
        {
            int index_ = 0;
            string querydata = "SELECT Carrier.Carrier As UID, Article.Article As SID, Carrier.Article As ID, Article.ReelCount As REELCOUNT, Carrier.Depot As LOCATION, Carrier.Stock As PARTS, Carrier.Manufactur As SUPPLIER, Carrier.Custom1 As LOTID, Carrier.Custom2 As MFG FROM Article LEFT JOIN Carrier ON Article.ID = Carrier.Article ";
            string wherephase_ = string.Empty;
            string lastsid_ = string.Empty;
            string lastuid_ = string.Empty;
            DataSet Queryresult_ = null;
            DataSet result_ = new DataSet();
            Dictionary<string, Pair<bool, int>> components_ = new Dictionary<string, Pair<bool, int>>();

            wherephase_ = $"WHERE Depot Is Not Null And Depot <> 'Unknown' And Depot <> 'Unnown' And Depot Like 'Tower%'";
            Queryresult_ = QueryCarrier(querydata + wherephase_);
            Queryresult_.Tables[0].DefaultView.Sort = "SID DESC";
            result_.Tables.Add(Queryresult_.Tables[0].DefaultView.ToTable());

            foreach (DataRow record_ in result_.Tables[0].Rows)
            {
                if (lastsid_ != record_["SID"].ToString())
                {
                    lastsid_ = record_["SID"].ToString();
                    index_ = 0;
                }
                else
                    index_ = 1;

                if (lastuid_ != record_["UID"].ToString())
                    lastuid_ = record_["UID"].ToString();

                switch (index_)
                {
                    case 0:
                        {   // Article and carrier confirm
                            if (articles.ContainsKey(lastsid_))
                            {   // Just get the reel count
                                if (!components_.ContainsKey(lastsid_))
                                    components_.Add(lastsid_, new Pair<bool, int>(false, articles[lastsid_].second));
                            }
                            else
                            {   // Add a new article information
                                articles.Add(lastsid_, new Pair<int, int>(Convert.ToInt32(record_["ID"]), Convert.ToInt32(record_["REELCOUNT"])));

                                if (!components_.ContainsKey(lastsid_))
                                    components_.Add(lastsid_, new Pair<bool, int>(true, 1));
                            }

                            if (!carriers.ContainsKey(lastuid_))
                            {
                                carriers.Add(record_["UID"].ToString(), new Pair<int, ProvideMaterialData>(Convert.ToInt32(record_["ID"]), new ProvideMaterialData(record_["LOCATION"].ToString().Substring(1, 6), record_["SID"].ToString(), record_["UID"].ToString(), record_["SUPPLIER"].ToString(), record_["LOTID"].ToString(), Convert.ToInt32(record_["PARTS"]), record_["LOCATION"].ToString(), record_["MFG"].ToString(), ProvideMaterialData.States.Stored)));
                            }
                        }
                        break;
                    case 1:
                        {
                            if (articles.ContainsKey(lastsid_))
                            {   // Increase the reel count of new added article
                                if (components_.ContainsKey(lastsid_) && components_[lastsid_].first)
                                    components_[lastsid_].second++;
                            }

                            if (!carriers.ContainsKey(lastuid_))
                            {
                                carriers.Add(record_["UID"].ToString(), new Pair<int, ProvideMaterialData>(Convert.ToInt32(record_["ID"]), new ProvideMaterialData(record_["LOCATION"].ToString().Substring(1, 6), record_["SID"].ToString(), record_["UID"].ToString(), record_["SUPPLIER"].ToString(), record_["LOTID"].ToString(), Convert.ToInt32(record_["PARTS"]), record_["LOCATION"].ToString(), record_["MFG"].ToString(), ProvideMaterialData.States.Stored)));
                            }
                        }
                        break;
                }
            }

            foreach (string articleName_ in articles.Keys)
            {
                if (articles[articleName_].second != components_[articleName_].second)
                    articles[articleName_].second = components_[articleName_].second;
            }
        }

        protected virtual bool ProvideItem(ProvideItem carrier)
        {
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            client_.DoAction(MethodBase.GetCurrentMethod().Name, carrier, ref error_, ref msg_);
            return (error_ == 0);
        }

        protected virtual bool GetJobLists(ref List<JobListInformation> data)
        {
            bool result_ = false;
            int error_  = 0;
            string msg_ = string.Empty;
            
            try
            {
                ClientChannel client_ = new ClientChannel();
                object obj_ = client_.DoAction(MethodBase.GetCurrentMethod().Name, null, ref error_, ref msg_);

                if (obj_ != null)
                {
                    data = obj_ as List<JobListInformation>;

                    foreach (JobListInformation job_ in data)
                    {
                        List<ProvideMaterialData> items_ = new List<ProvideMaterialData>();

                        foreach (JobListItem item_ in job_.List)
                        {
                            ProvideMaterialData.States state_ = ProvideMaterialData.States.Ready;

                            switch (item_.State)
                            {
                                case 3:
                                    state_ = ProvideMaterialData.States.Providing;
                                    break;
                                default:
                                    {
                                        if (item_.State > 3)
                                            state_ = ProvideMaterialData.States.Completed;
                                    }
                                    break;
                            }

                            items_.Add(new ProvideMaterialData(new string[] { item_.Article, item_.Carrier, string.Empty, item_.Reels.ToString(), item_.Comment, state_.ToString() }));
                        }

                        if (provideJobs.Find(x_ => x_.first == job_.Name) != null)
                            UpdateProvideJobInformation(job_.Name, items_);
                        else if (error_ == 0)
                            AddNewProvideJobInformation(job_.Name, items_); // Restore the job informations
                    }

                    result_ = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        // Not use
        protected virtual bool GetJobList(string jobname, ref JobListInformation data)
        {
            bool result_ = false;
            int error_  = 0;
            string msg_ = string.Empty;
            
            try
            {
                ClientChannel client_ = new ClientChannel();
                object obj_ = client_.DoAction(MethodBase.GetCurrentMethod().Name, jobname, ref error_, ref msg_);

                if (obj_ != null)
                {
                    data = obj_ as JobListInformation;
                    List<ProvideMaterialData> items_ = new List<ProvideMaterialData>();

                    foreach (JobListItem item_ in data.List)
                    {
                        ProvideMaterialData.States state_ = ProvideMaterialData.States.Ready;

                        switch (item_.State)
                        {
                            case 3:
                                state_ = ProvideMaterialData.States.Providing;
                                break;
                            default:
                                {
                                    if (item_.State > 3)
                                        state_ = ProvideMaterialData.States.Completed;
                                }
                                break;
                        }

                        items_.Add(new ProvideMaterialData(new string[] { item_.Article, item_.Carrier, string.Empty, item_.Reels.ToString(), item_.Comment, state_.ToString() }));
                    }

                    if (provideJobs.Find(x_ => x_.first == jobname) != null)
                        UpdateProvideJobInformation(data.Name, items_);
                    else if (error_ == 0)
                        AddNewProvideJobInformation(data.Name, items_); // Not use

                    result_ = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        protected virtual bool DeleteJobList(string jobname, bool flush = true)
        {
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            client_.DoAction(MethodBase.GetCurrentMethod().Name, jobname, ref error_, ref msg_);

            if (error_ == 0)
            {
                if (flush)
                    RemoveProvideJobInformation(jobname);

                return true;
            }

            return false;
        }

        // Not use
        protected virtual bool NewJobList(JobListInformation job)
        {
            bool result_            = false;
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            
            try
            {
                Pair<string, ProvideJobListData> provideJob_ = null;
                client_.DoAction(MethodBase.GetCurrentMethod().Name, job, ref error_, ref msg_);

                if (error_ == 0)
                {
                    List<ProvideMaterialData> items_ = new List<ProvideMaterialData>();

                    foreach (JobListItem item_ in job.List)
                    {
                        ProvideMaterialData.States state_ = ProvideMaterialData.States.Ready;

                        switch (item_.State)
                        {
                            case 3:
                                state_ = ProvideMaterialData.States.Providing;
                                break;
                            default:
                                {
                                    if (item_.State > 3)
                                        state_ = ProvideMaterialData.States.Completed;
                                }
                                break;
                        }

                        items_.Add(new ProvideMaterialData(new string[] { item_.Article, item_.Carrier, string.Empty, item_.Reels.ToString(), item_.Comment, state_.ToString() }));
                    }

                    if ((provideJob_ = provideJobs.Find(x_ => x_.first == job.Name)) == null)
                        AddNewProvideJobInformation(job.Name, items_);  // Not use

                    if ((provideJob_ = provideJobs.Find(x_ => x_.first == job.Name)) != null)
                        FireProvideJobStateChanged(provideJob_.second);

                    result_ = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        protected virtual bool NewJobList(JobListInformation job, string user, int outport)
        {
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            List<ProvideMaterialData> items_ = new List<ProvideMaterialData>();

            try
            {
                foreach (JobListItem item_ in job.List)
                {
                    if (carriers.ContainsKey(item_.Carrier))
                    {
                        items_.Add(new ProvideMaterialData(carriers[item_.Carrier].second));
                    }
                }

                if (job.List.Count == items_.Count)
                {
                    Pair<string, ProvideJobListData> provideJob_ = null;
                    client_.DoAction(MethodBase.GetCurrentMethod().Name, job, ref error_, ref msg_);

                    if (error_ == 0)
                    {
                        if ((provideJob_ = provideJobs.Find(x_ => x_.first == job.Name)) == null)
                            AddNewProvideJobInformation(job.Name, user, outport, items_);   // Added by remote server query

                        if ((provideJob_ = provideJobs.Find(x_ => x_.first == job.Name)) != null)
                            FireProvideJobStateChanged(provideJob_.second);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return false;
        }

        protected virtual bool UpdateJobList(JobListInformation job)
        {
            bool result_            = false;
            int error_              = 0;
            string msg_             = string.Empty;

            try
            {
                ClientChannel client_ = new ClientChannel();
                Pair<string, ProvideJobListData> provideJob_ = null;
                client_.DoAction(MethodBase.GetCurrentMethod().Name, job, ref error_, ref msg_);

                if (error_ == 0)
                {
                    if ((provideJob_ = provideJobs.Find(x_ => x_.first == job.Name)) != null)
                        FireProvideJobStateChanged(provideJob_.second);

                    result_ = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        protected virtual void DeleteCompletedJobList(List<string> jobnames)
        {
            foreach (string jobname_ in jobnames)
                DeleteJobList(jobname_);
        }

        protected virtual bool StartJobList(string jobname)
        {
            bool result_            = false;
            int error_              = 0;
            string msg_             = string.Empty;

            try
            {
                ClientChannel client_ = new ClientChannel();
                Pair<string, ProvideJobListData> provideJob_ = null;
                client_.DoAction(MethodBase.GetCurrentMethod().Name, jobname, ref error_, ref msg_);

                if (result_ = (error_ == 0))
                {
                    if (lastReservedProvideJobTimestamp != DateTime.Now.Date)
                    {
                        lastReservedProvideJobTimestamp = DateTime.Now.Date;
                        reservedProvideJobCount = 0;
                        processedProvideJobCount = 0;
                    }
                    else
                        processedProvideJobCount++;

                    lastReservedProvideJob = jobname;

                    if ((provideJob_ = provideJobs.Find(x_ => x_.first == jobname)) != null)
                    {
                        provideJob_.second.State = ProvideJobListData.States.Providing;
                        FireProvideJobStateChanged(provideJob_.second);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        protected virtual bool CancelJobList(string jobname)
        {
            int error_              = 0;
            string msg_             = string.Empty;

            try
            {
                ClientChannel client_ = new ClientChannel();
                Pair<string, ProvideJobListData> provideJob_ = null;
                client_.DoAction(MethodBase.GetCurrentMethod().Name, jobname, ref error_, ref msg_);

                if (error_ == 0)
                {
                    if ((provideJob_ = provideJobs.Find(x_ => x_.first == jobname)) != null)
                    {
                        provideJob_.second.State = ProvideJobListData.States.Providing;
                        FireProvideJobStateChanged(provideJob_.second);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return false;
        }

        protected virtual bool SimulateStartButton(LoadMaterialInformation material)
        {
            int error_              = 0;
            string msg_             = string.Empty;
            ClientChannel client_   = new ClientChannel();
            client_.DoAction(MethodBase.GetCurrentMethod().Name, material, ref error_, ref msg_);
            return (error_ == 0);
        }

        protected virtual void OnReelTowerStateChangedEvent(object sender, string str)
        {
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    string[] tokens_ = str.Split(new char[] { '=', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (tokens_.Length > 2 && tokens_[0].ToLower() == "tower")
                    {
                        var obj_ = TowerStates.Where(x => x.Value.Name == tokens_[1]).ToArray();

                        if (obj_ != null && obj_.Length > 0)
                            FireReelTowerStateChanaged(obj_[0].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected virtual void FireReelTowerStateChanaged(ReelTowerState obj)
        {
            ReelTowerStateChanged?.Invoke(this, obj);
        }

        public virtual void FireMaterialEventRaised(MaterialEventArgs obj)
        {
            MaterialEventRaised?.Invoke(this, obj);
        }

        protected virtual void FireProvideJobStateChanged(ProvideJobListData data)
        {
            if (data != null)
                ProvideJobStateChanged?.Invoke(this, new ProvideJobListData(data));
        }

        protected virtual void FireProvideMaterialStateChanged(ProvideMaterialData data)
        {
            if (data != null)
                ProvideMaterialStateChanged?.Invoke(this, new ProvideMaterialData(data));
        }

        protected virtual void FireReportRuntimeLog(string log)
        {
            if (!string.IsNullOrEmpty(log))
                ReportRuntimeLog?.Invoke(this, log);
        }

        protected virtual void FireReportException(string log)
        {
            if (!string.IsNullOrEmpty(log))
                ReportException?.Invoke(this, log);
        }

        protected virtual void FireReportAlarmLog(bool state, string towername, string towerid, int alarmcode, string alarmmessage = null)
        {
            ReportAlarmLog?.Invoke(this, new FiveField<bool, string, string, int, string>(state, towername, towerid, alarmcode, string.IsNullOrEmpty(alarmmessage)? GetAlarmMessage(alarmcode) : alarmmessage));
        }

        protected virtual void AddNewArticleInformation(string articlename, int articleid = -1, int reels = -1)
        {
            if (string.IsNullOrEmpty(articlename))
                return;

            try
            {
                lock (articles)
                    articles.Add(articlename, new Pair<int, int>(articleid, reels));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            FireReportRuntimeLog($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Article={articlename},Id={articleid},Reels={reels}");
        }

        protected virtual void RemoveArticleInformation(string articlename)
        {
            if (string.IsNullOrEmpty(articlename))
                return;

            try
            {
                lock (articles)
                    articles.Remove(articlename);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            FireReportRuntimeLog($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Article={articlename}");
        }

        protected virtual void UpdateArticleInformation(string articlename, int articleid, int reels)
        {
            if (string.IsNullOrEmpty(articlename))
                return;

            try
            {
                lock (articles)
                {
                    articles[articlename].first     = articleid;
                    articles[articlename].second    = reels;
                }

                FireReportRuntimeLog($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Article={articlename},Id={articleid},Reels={reels}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected virtual void AddNewCarrierInformation(CarrierInformation data)
        {
            if (data == null)
                return;

            string towerid_ = string.Empty;
            string articlename_ = string.Empty;
            string carriername_ = string.Empty;

            try
            {
                lock (carriers)
                {
                    if (!string.IsNullOrEmpty(data.Depot) && data.Depot[0] == 'T')
                        towerid_ = data.Depot.Substring(1, 6);
                    else if (!string.IsNullOrEmpty(data.DepotOld) && data.DepotOld[0] == 'T')
                        towerid_ = data.DepotOld.Substring(1, 6);

                    articlename_ = GetArticleNameById(data.Article);

                    if (string.IsNullOrEmpty(articlename_))
                        articlename_ = data.ArticleName;

                    carriers.Add(carriername_ = data.Carrier, new Pair<int, ProvideMaterialData>(data.Article, new ProvideMaterialData(towerid_, articlename_, data.Carrier, data.Manufactur, data.Custom1, data.Stock, data.Depot, data.Custom2, ProvideMaterialData.States.Stored)));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            FireReportRuntimeLog($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: TowerId={towerid_},Article={articlename_},Carrier={carriername_}");
        }

        protected virtual void RemoveCarrierInformation(string carriername)
        {
            bool result = false;

            if (string.IsNullOrEmpty(carriername))
                return;

            try
            {
                lock (carriers)
                {
                    if (carriers.ContainsKey(carriername))
                    {
                        carriers.Remove(carriername);
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            if (result)
                FireReportRuntimeLog($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Carrier={carriername}");
        }

        protected virtual void UpdateCarrierInformation(CarrierInformation data)
        {
            if (string.IsNullOrEmpty(data.Carrier))
                return;

            try
            {
                string towerid_     = string.Empty;
                string articlename_ = data.ArticleName;
                string carriername_ = string.Empty;

                lock (carriers)
                {
                    if (!string.IsNullOrEmpty(data.Depot) && data.Depot[0] == 'T' && data.Depot.Length >= 7)
                        towerid_ = data.Depot.Substring(1, 6);

                    if (string.IsNullOrEmpty(articlename_))
                    {
                        articlename_ = GetArticleNameById(data.Article);

                        if (string.IsNullOrEmpty(articlename_))
                            articlename_ = data.ArticleName;
                    }

                    carriers[data.Carrier].first    = data.Article;
                    carriers[data.Carrier].second   = new ProvideMaterialData(towerid_, articlename_, carriername_ = data.Carrier, data.Manufactur, data.Custom1, data.Stock, data.Depot, data.Custom2, ProvideMaterialData.States.Stored);
                }

                FireReportRuntimeLog($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: TowerId={towerid_},Article={articlename_},Carrier={carriername_}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected virtual bool AddNewProvideJobInformation(string jobname, List<ProvideMaterialData> items)
        {
            bool result_        = false;
            string materials_   = string.Empty;

            if (string.IsNullOrEmpty(jobname))
                return false;

            try
            {
                lock (provideJobs)
                {
                    if (provideJobs.Find(x_ => x_.first == jobname) == null && reservedJobs.ContainsKey(jobname))
                    {
                        reservedJobs[jobname].State = ProvideJobListData.States.Created;
                        Pair<string, ProvideJobListData> job_ = new Pair<string, ProvideJobListData>(jobname, new ProvideJobListData(reservedJobs[jobname]));
                        provideJobs.Add(job_);

                        foreach (ProvideMaterialData item_ in items)
                        {
                            if (!string.IsNullOrEmpty(item_.Name) && !pendingUnloadReels.ContainsKey(item_.Name))
                            {
                                materials_ += $"{item_.Category};{item_.Name}|";
                                if (carriers.ContainsKey(item_.Name))
                                    pendingUnloadReels.Add(item_.Name, new Pair<string, ProvideMaterialData>(jobname, new ProvideMaterialData(carriers[item_.Name].second)));
                                else
                                    pendingUnloadReels.Add(item_.Name, new Pair<string, ProvideMaterialData>(jobname, new ProvideMaterialData(item_)));
                                result_ = true;
                            }
                        }

                        provideJobs.Last().second.State = ProvideJobListData.States.Ready;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}-Imported: Exception={ex.Message}");
            }

            FireReportRuntimeLog($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}-Imported: Jobname={jobname},Materials={materials_}");
            return result_;
        }

        protected virtual void AddNewProvideJobInformation(string jobname, string user, int outport, List<ProvideMaterialData> items)
        {
            if (string.IsNullOrEmpty(jobname))
                return;

            string materials_ = string.Empty;

            try
            {
                lock (provideJobs)
                {
                    if (provideJobs.Find(x_ => x_.first == jobname) == null && !reservedJobs.ContainsKey(jobname))
                    {
                        provideJobs.Add(new Pair<string, ProvideJobListData>(
                            jobname, new ProvideJobListData(
                                jobname,
                                user,
                                outport,
                                items.Count,
                                ProvideJobListData.States.Created,
                                items)));

                        foreach (ProvideMaterialData item_ in items)
                        {
                            Debug.WriteLine($">>> Job = {jobname}, Reserved item = {item_.Name}");
                            if (!string.IsNullOrEmpty(item_.Name) && !pendingUnloadReels.ContainsKey(item_.Name))
                            {
                                materials_ += $"{item_.Category};{item_.Name}|";
                                if (carriers.ContainsKey(item_.Name))
                                    pendingUnloadReels.Add(item_.Name, new Pair<string, ProvideMaterialData>(jobname, new ProvideMaterialData(carriers[item_.Name].second)));
                                else
                                    pendingUnloadReels.Add(item_.Name, new Pair<string, ProvideMaterialData>(jobname, new ProvideMaterialData(item_)));
                            }
                        }

                        provideJobs.Last().second.State = ProvideJobListData.States.Ready;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}-Remote: Exception={ex.Message}");
            }

            FireReportRuntimeLog($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}-Remote: Jobname={jobname},Materials={materials_}");
        }

        protected virtual void RemoveProvideJobInformation(string jobname)
        {
            if (string.IsNullOrEmpty(jobname))
                return;

            string materials_ = string.Empty;

            try
            {   
                Pair<string, ProvideJobListData> provideJob_ = null;

                // lock (provideJobs)
                // {
                    if ((provideJob_ = provideJobs.Find(x_ => x_.first == jobname)) != null)
                    {
                        if (provideJob_.second.State == ProvideJobListData.States.Providing)
                        {
                            provideJob_.second.State = ProvideJobListData.States.Completed;
                            FireProvideJobStateChanged(provideJob_.second);
                        }

                        new TaskFactory().StartNew(new Action<object>((x_) =>
                        {
                            if (x_ != null)
                            {
                                Pair<string, ProvideJobListData> job_ = x_ as Pair<string, ProvideJobListData>;
                                job_.second.State = ProvideJobListData.States.Deleted;

                                lock (provideJobs)
                                {
                                    provideJobs.Remove(job_);
                                }

                                reservedJobs.Remove(job_.first);
                                List<string> items_ = pendingUnloadReels.Where(x => x.Value.first == jobname).Select(y_ => y_.Key).ToList();

                                foreach (string item_ in items_)
                                {
                                    materials_ += $"{item_}|";
                                    pendingUnloadReels.Remove(item_);
                                }

                                FireProvideJobStateChanged(job_.second);
                                // Reset providing job name
                                currentProvidingJobname = string.Empty;
                            }
                        }), provideJob_);
                    }
                // }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            FireReportRuntimeLog($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Jobname={jobname},Materials={materials_}");
        }

        protected virtual void RemoveProvideItem(string jobname, string articlename, string carriername)
        {
            if (string.IsNullOrEmpty(jobname))
                return;

            try
            {
                Pair<string, ProvideJobListData> provideJob_ = null;

                lock (provideJobs)
                {
                    if ((provideJob_ = provideJobs.Find(x_ => x_.first == jobname)) != null)
                    {
                        ProvideMaterialData item_ = provideJob_.second.GetMaterialData(articlename, carriername);
                        var carriers_ = pendingUnloadReels.Where(x_ => x_.Key == carriername && x_.Value.first == jobname && x_.Value.second.Category == articlename);

                        if (item_ != null)
                        {
                            provideJob_.second.RemoveMaterialData(item_);
                        }

                        foreach (KeyValuePair<string, Pair<string, ProvideMaterialData>> carrier_ in carriers_)
                        {
                            if (!string.IsNullOrEmpty(carrier_.Key))
                                pendingUnloadReels.Remove(carriername);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            FireReportRuntimeLog($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Jobname={jobname},Article={articlename},Carrier={carriername}");
        }

        protected virtual void RemoveProvideItem(string jobname, string item)
        {
            if (string.IsNullOrEmpty(jobname))
                return;

            try
            {
                string[] tokens_ = item.Split(CONST_REEL_INFORMATION_DELIMITER, StringSplitOptions.RemoveEmptyEntries);

                if (tokens_.Length >= 2)
                {
                    string articlename_ = string.Empty;
                    string carriername_ = string.Empty;
                    Pair<string, ProvideJobListData> provideJob_ = null;

                    lock (provideJobs)
                    {
                        if ((provideJob_ = provideJobs.Find(x_ => x_.first == jobname)) != null)
                        {
                            ProvideMaterialData item_ = provideJob_.second.GetMaterialData(tokens_[0], tokens_[1]);
                            var carriers_ = pendingUnloadReels.Where(x_ => x_.Key == tokens_[1] && x_.Value.first == jobname && x_.Value.second.Category == tokens_[0]);

                            if (item_ != null)
                            {
                                articlename_ = item_.Category;
                                carriername_ = item_.Name;
                                provideJob_.second.RemoveMaterialData(item_);
                            }

                            foreach (KeyValuePair<string, Pair<string, ProvideMaterialData>> carrier_ in carriers_)
                            {
                                if (!string.IsNullOrEmpty(carrier_.Key))
                                    pendingUnloadReels.Remove(tokens_[1]);
                            }
                        }
                    }

                    FireReportRuntimeLog($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Jobname={jobname},Article={articlename_},Carrier={carriername_}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected virtual int GetProvidedCounts(string jobname)
        {
            int result_ = 0;

            try
            {
                Pair<string, ProvideJobListData> provideJob_ = null;

                lock (provideJobs)
                {
                    if ((provideJob_ = provideJobs.Find(x_ => x_.first == jobname)) != null)
                    {
                        foreach (ProvideMaterialData item_ in provideJob_.second.Materials)
                        {
                            if (item_.State == ProvideMaterialData.States.Completed)
                                result_++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        protected virtual bool UpdateProvideJobInformation(string jobname, List<ProvideMaterialData> items)
        {
            bool result_ = false;

            try
            {
                string materials_ = string.Empty;
                Pair<string, ProvideJobListData> provideJob_ = null;

                lock (provideJobs)
                {
                    if ((provideJob_ = provideJobs.Find(x_ => x_.first == jobname)) != null)
                    {
                        provideJob_.second.UpdateMaterials(items);
                        result_ = true;
                    }

                    if (pendingUnloadReels.Where(x_ => x_.Value.first == jobname).Any())
                    {
                        var item_ = pendingUnloadReels.Where(x_ => x_.Value.first == jobname).First();

                        if (!string.IsNullOrEmpty(item_.Key))
                        {
                            materials_ += $"{item_.Value.second.Category};{item_.Value.second.Name}|";
                            pendingUnloadReels.Remove(item_.Key);
                        }
                    }

                    foreach (ProvideMaterialData item_ in items)
                    {
                        if (!string.IsNullOrEmpty(item_.Name) && !pendingUnloadReels.ContainsKey(item_.Name))
                        {
                            if (carriers.ContainsKey(item_.Name))
                                pendingUnloadReels.Add(item_.Name, new Pair<string, ProvideMaterialData>(jobname, new ProvideMaterialData(carriers[item_.Name].second)));
                            else
                                pendingUnloadReels.Add(item_.Name, new Pair<string, ProvideMaterialData>(jobname, new ProvideMaterialData(item_)));
                        }
                    }
                }

                FireReportRuntimeLog($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Jobname={jobname},Maerials={materials_}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        protected virtual bool ImportAlarmData(string file = null, string culturecode = "en-US")
        {
            bool enabled_ = false;
            int alarmCode_ = 0;
            string alarmName_ = string.Empty;
            string alarmMessage_ = string.Empty;
            SeverityLevels level_ = SeverityLevels.Major;

            try
            {
                alarmManager.Clear();

                if (string.IsNullOrEmpty(file))
                {
                    foreach (AlarmCodes code_ in Enum.GetValues(typeof(AlarmCodes)))
                    {
                        switch (code_)
                        {
                            case AlarmCodes.Tray_16_44_not_released: alarmCode_ = 100; alarmMessage_ = "Tray 16, 44 not released."; break;
                            case AlarmCodes.Reel_height_exceeded: alarmCode_ = 101; alarmMessage_ = "Reel height exceeded."; break;
                            case AlarmCodes.Reel_not_recognized: alarmCode_ = 102; alarmMessage_ = "Reel not recognized."; break;
                            case AlarmCodes.Tray_not_recognized: alarmCode_ = 103; alarmMessage_ = "Tray not recognized."; break;
                            case AlarmCodes.Reel_diameter_too_big: alarmCode_ = 104; alarmMessage_ = "Reel diameter too big."; break;
                            case AlarmCodes.Reel_diameter_not_recognized: alarmCode_ = 105; alarmMessage_ = "Reel diameter not recognized."; break;
                            case AlarmCodes.No_scancode_Barcode_not_detected_during_test_run: alarmCode_ = 106; alarmMessage_ = "No scancode, Barcode not detected during test run."; break;
                            case AlarmCodes.No_scancode_Barcode_not_detected_when_loading_carrier: alarmCode_ = 107; alarmMessage_ = "No scancode, Barcode not detected when loading carrier."; break;
                            case AlarmCodes.No_scancode_Barcode_not_detected_when_unloading_carrier: alarmCode_ = 108; alarmMessage_ = "No scancode, Barcode not detected when unloading carrier."; break;
                            case AlarmCodes.No_scancode_Barcode_not_detected_after_power_on_sequence: alarmCode_ = 109; alarmMessage_ = "No scancode, Barcode not detected after power on sequence."; break;
                            case AlarmCodes.Invalid_data: alarmCode_ = 116; alarmMessage_ = "Invalid data."; break;
                            case AlarmCodes.Operation_denied: alarmCode_ = 117; alarmMessage_ = "Operation denied."; break;
                            case AlarmCodes.Code_error: alarmCode_ = 118; alarmMessage_ = "Code error."; break;
                            case AlarmCodes.Error_arm_motor: alarmCode_ = 175; alarmMessage_ = "Error arm motor."; break;
                            case AlarmCodes.Arm_sensor_defect: alarmCode_ = 176; alarmMessage_ = "Arm sensor defect."; break;
                            case AlarmCodes.No_connection_between_arm_sensor_and_motor: alarmCode_ = 177; alarmMessage_ = "No connection between arm sensor and motor."; break;
                            case AlarmCodes.Error_arm_sensor: alarmCode_ = 178; alarmMessage_ = "Error arm sensor."; break;
                            case AlarmCodes.Slot_difference: alarmCode_ = 179; alarmMessage_ = "Slot difference."; break;
                            case AlarmCodes.Incorrect_scanner_scanner: alarmCode_ = 183; alarmMessage_ = "Incorrect scanner firmware."; break;
                            case AlarmCodes.Incorrect_arm_firmware: alarmCode_ = 184; alarmMessage_ = "Incorrect arm firmware."; break;
                            case AlarmCodes.Humidity_logger_not_responding: alarmCode_ = 185; alarmMessage_ = "Humidity logger not responding."; break;
                            case AlarmCodes.Fault_arm_motor: alarmCode_ = 186; alarmMessage_ = "Fault arm motor."; break;
                            case AlarmCodes.Error_voltage_3_3V: alarmCode_ = 187; alarmMessage_ = "Error voltage 3.3V."; break;
                            case AlarmCodes.Error_voltage_60V: alarmCode_ = 188; alarmMessage_ = "Error voltage 60V."; break;
                            case AlarmCodes.Rotor_motor_not_recognized: alarmCode_ = 189; alarmMessage_ = "Rotor motor not recognized."; break;
                            case AlarmCodes.Lift_motor_not_recognized: alarmCode_ = 190; alarmMessage_ = "Lift motor not recognized."; break;
                            case AlarmCodes.Invalid_reel_height: alarmCode_ = 191; alarmMessage_ = "Invalid reel height."; break;
                            case AlarmCodes.Gripper_too_high: alarmCode_ = 192; alarmMessage_ = "Gripper too high."; break;
                            case AlarmCodes.Gripper_too_low: alarmCode_ = 193; alarmMessage_ = "Gripper too low."; break;
                            case AlarmCodes.Arm_not_recognized_while_calibrating_lift: alarmCode_ = 194; alarmMessage_ = "Arm not recognized while calibrating lift."; break;
                            case AlarmCodes.Arm_not_recognized_while_calibrating_rotor: alarmCode_ = 195; alarmMessage_ = "Arm not recognized while calibrating rotor."; break;
                            case AlarmCodes.Invalid_stack: alarmCode_ = 196; alarmMessage_ = "Invalid stack."; break;
                            case AlarmCodes.Missing_or_invalid_firmware_in_rotor_motor: alarmCode_ = 197; alarmMessage_ = "Missing or invalid firmware in rotor motor."; break;
                            case AlarmCodes.Missing_or_invalid_firmware_in_lift_motor: alarmCode_ = 198; alarmMessage_ = "Missing or invalid firmware in lift motor."; break;
                            case AlarmCodes.Lift_step_error: alarmCode_ = 201; alarmMessage_ = "Lift step error."; break;
                            case AlarmCodes.Command_not_recognized_by_lift_motor: alarmCode_ = 204; alarmMessage_ = "Command not recognized by lift motor."; break;
                            case AlarmCodes.Lift_sensor_error_encoder: alarmCode_ = 205; alarmMessage_ = "Lift sensor error encoder."; break;
                            case AlarmCodes.Lift_motor_invalid_data: alarmCode_ = 208; alarmMessage_ = "Lift motor invalid data."; break;
                            case AlarmCodes.Lift_motor_unknown_error: alarmCode_ = 209; alarmMessage_ = "Lift motor unknown error."; break;
                            case AlarmCodes.Sensor_error_encoder: alarmCode_ = 215; alarmMessage_ = "Sensor error encoder."; break;
                            case AlarmCodes.Command_recognized_by_rotor_motor: alarmCode_ = 217; alarmMessage_ = "Command not recognized by rotor motor."; break;
                            case AlarmCodes.Rotot_motor_invalid_data: alarmCode_ = 218; alarmMessage_ = "Rotor motor invalid data."; break;
                            case AlarmCodes.Rotot_motor_unknown_error: alarmCode_ = 219; alarmMessage_ = "Rotor motor unknown error."; break;
                            case AlarmCodes.Error_lid_while_opening: alarmCode_ = 221; alarmMessage_ = "Error lid while opening."; break;
                            case AlarmCodes.Error_lid_opening_top: alarmCode_ = 222; alarmMessage_ = "Error lid opening top."; break;
                            case AlarmCodes.Error_lid_closing_top: alarmCode_ = 224; alarmMessage_ = "Error lid clising top."; break;
                            case AlarmCodes.Error_lid_closing_bottom: alarmCode_ = 225; alarmMessage_ = "Error lid closing bottom."; break;
                            case AlarmCodes.Lid_open_while_driving: alarmCode_ = 229; alarmMessage_ = "Lid open while driving."; break;
                            case AlarmCodes.No_response_from_arm_motor: alarmCode_ = 230; alarmMessage_ = "No response from arm motor."; break;
                            case AlarmCodes.Error_clamping_on_closing_1: alarmCode_ = 231; alarmMessage_ = "Error clamping on closing 1."; break;
                            case AlarmCodes.Error_clamping_on_closing_2: alarmCode_ = 232; alarmMessage_ = "Error clamping on closing 2."; break;
                            case AlarmCodes.Error_clamping_on_opening_1: alarmCode_ = 233; alarmMessage_ = "Error clamping on opening 1."; break;
                            case AlarmCodes.Error_clamping_on_opening_2: alarmCode_ = 234; alarmMessage_ = "Error clamping on opening 2."; break;
                            case AlarmCodes.Error_arm_init_to_front: alarmCode_ = 235; alarmMessage_ = "Error arm init to front."; break;
                            case AlarmCodes.Error_arm_init_to_back: alarmCode_ = 236; alarmMessage_ = "Error arm init to back."; break;
                            case AlarmCodes.Error_arm_blocked: alarmCode_ = 237; alarmMessage_ = "Error arm blocked."; break;
                            case AlarmCodes.Arm_not_in_stand_still: alarmCode_ = 238; alarmMessage_ = "Arm not stand still."; break;
                            case AlarmCodes.Arm_board_not_recogninzed: alarmCode_ = 239; alarmMessage_ = "Arm board not recognized."; break;
                            case AlarmCodes.Scanner_board_not_recogninzed: alarmCode_ = 240; alarmMessage_ = "Scanner board not recognized."; break;
                            case AlarmCodes.Error_clamping_on_init_1: alarmCode_ = 241; alarmMessage_ = "Error clamping on init 1."; break;
                            case AlarmCodes.Error_clamping_on_init_2: alarmCode_ = 242; alarmMessage_ = "Error clamping on init 2."; break;
                            case AlarmCodes.Error_clamping_while_driving_1: alarmCode_ = 243; alarmMessage_ = "Error clamping while driving 1."; break;
                            case AlarmCodes.Error_clamping_while_driving_2: alarmCode_ = 244; alarmMessage_ = "Error clamping while driving 2."; break;
                            case AlarmCodes.Missing_or_invalid_firmware_in_arm_motor: alarmCode_ = 245; alarmMessage_ = "Missing or invliad firmware in arm motor."; break;
                            case AlarmCodes.No_tableau_on_magazine_calibration: alarmCode_ = 246; alarmMessage_ = "No tableau on magazine calibration."; break;
                            case AlarmCodes.No_screening_control_rotor: alarmCode_ = 247; alarmMessage_ = "No screening control rotor."; break;
                            case AlarmCodes.No_screening_control_arm: alarmCode_ = 248; alarmMessage_ = "No screening control arm."; break;
                            case AlarmCodes.Error_reel_sensor: alarmCode_ = 249; alarmMessage_ = "Error reel sensor."; break;
                            case AlarmCodes.Error_lid: alarmCode_ = 250; alarmMessage_ = "Error lid."; break;
                            case AlarmCodes.Error_arm: alarmCode_ = 251; alarmMessage_ = "Error arm."; break;
                            case AlarmCodes.Error_clamping: alarmCode_ = 252; alarmMessage_ = "Error clamping."; break;
                            case AlarmCodes.Error_scanner_setting: alarmCode_ = 253; alarmMessage_ = "Error scanner setting."; break;
                            case AlarmCodes.Calibration_of_diameter_failed: alarmCode_ = 254; alarmMessage_ = "Calibration of diameter failed."; break;
                            case AlarmCodes.Calibration_of_height_failed: alarmCode_ = 255; alarmMessage_ = "Calibration of height failed."; break;
                            case AlarmCodes.Invalid_scan_code: alarmCode_ = 777; alarmMessage_ = "Invalid scan code."; break;
                            case AlarmCodes.Communication_timeout: alarmCode_ = 999; alarmMessage_ = "Communication timeout."; break;
                        }

                        alarmManager.AddAlarmData(alarmCode_, code_.ToString(), SeverityLevels.Major, alarmMessage_, true);
                    }
                }
                else
                {
                    if (File.Exists(file))
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.Load(file);

                        XmlNode node_ = xml.DocumentElement;
                        if (node_.Name == "Alarms")
                        {
                            alarmManager.SetCulture(culturecode);

                            foreach (XmlNode element_ in node_.ChildNodes)
                            {
                                switch (element_.Name.ToLower())
                                {
                                    case "alarm":
                                        {
                                            foreach (XmlAttribute attr_ in element_.Attributes)
                                            {
                                                switch (attr_.Name.ToString().ToLower())
                                                {
                                                    case "id":
                                                        alarmCode_ = Convert.ToInt32(attr_.Value);
                                                        break;
                                                    case "name":
                                                        alarmName_ = attr_.Value; 
                                                        break;
                                                    case "enabled":
                                                        enabled_ = Convert.ToBoolean(attr_.Value);
                                                        break;
                                                    case "severity":
                                                        level_ = (SeverityLevels)Enum.Parse(typeof(SeverityLevels), attr_.Value);
                                                        break;
                                                }
                                            }

                                            foreach (XmlNode child_ in element_.ChildNodes)
                                            {
                                                switch (child_.Name.ToLower().ToString())
                                                {
                                                    case "description":
                                                        {
                                                            if (child_.Attributes["culture"].Value == cultureCode)
                                                            {
                                                                alarmMessage_ = child_.InnerText;
                                                                alarmManager.AddAlarmData(alarmCode_, alarmName_, level_, alarmMessage_, enabled_);
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return alarmManager.AlarmList.Count > 0;
        }

        protected virtual void ExportSpecialData()
        {
            try
            {
                if (specialMaterials.Count > 0)
                {
                    XElement result_ = new XElement("Materials");

                    foreach (KeyValuePair<string, string> material_ in specialMaterials)
                    {
                        result_.Add(new XElement("Material",
                            new XAttribute("name", material_.Key),
                            new XAttribute("type", material_.Value)));
                    }

                    result_.Save(CONST_SPECIAL_MATERIAL_DATA_FILE);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected virtual void ImportSpecialData()
        {
            try
            {
                if (File.Exists(CONST_SPECIAL_MATERIAL_DATA_FILE))
                {
                    specialMaterials.Clear();

                    XmlDocument xml = new XmlDocument();
                    xml.Load(CONST_SPECIAL_MATERIAL_DATA_FILE);

                    XmlNode node_ = xml.DocumentElement;
                    if (node_.Name == "Materials")
                    {
                        foreach (XmlNode element_ in node_.ChildNodes)
                        {
                            string name_ = string.Empty;
                            string type_ = string.Empty;

                            switch (element_.Name.ToLower())
                            {
                                case "material":
                                    {
                                        foreach (XmlAttribute attr_ in element_.Attributes)
                                        {
                                            switch (attr_.Name.ToString().ToLower())
                                            {
                                                case "name":
                                                    name_ = attr_.Value;
                                                    break;
                                                case "type":
                                                    type_ = attr_.Value;
                                                    break;
                                            }
                                        }

                                        if (!specialMaterials.ContainsKey(name_))
                                            specialMaterials.Add(name_, type_);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected virtual void ImportJobData(string file= null)
        {
            try
            {
                if (!string.IsNullOrEmpty(file))
                    reservedJobFile = file;

                reservedJobs.Clear();

                if (string.IsNullOrEmpty(reservedJobFile))
                    reservedJobFile = CONST_RESERVED_JOB_DATA_FILE;

                if (File.Exists(reservedJobFile))
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(reservedJobFile);

                    XmlNode node_ = xml.DocumentElement;
                    switch (node_.Name.ToLower())
                    {
                        case "reservedjobs":
                            {
                                foreach (XmlAttribute attr_ in node_.Attributes)
                                {
                                    switch (attr_.Name.ToLower())
                                    {
                                        case "timestamp":
                                            lastReservedProvideJobTimestamp = Convert.ToDateTime(attr_.Value).Date;
                                            break;
                                        case "last":
                                            lastReservedProvideJob = attr_.Value;
                                            break;
                                        case "index":
                                            reservedProvideJobCount = (processedProvideJobCount = int.Parse(attr_.Value));
                                            break;
                                    }
                                }

                                foreach (XmlNode element_ in node_.ChildNodes)
                                {
                                    string name_ = string.Empty;
                                    string user_ = string.Empty;
                                    int outport_ = 0;
                                    int reels_ = 0;
                                    ProvideJobListData.States state_ = ProvideJobListData.States.Unknown;

                                    switch (element_.Name.ToLower())
                                    {
                                        case "job":
                                            {
                                                foreach (XmlAttribute attr_ in element_.Attributes)
                                                {
                                                    switch (attr_.Name.ToLower())
                                                    {
                                                        case "name": name_ = attr_.Value; break;
                                                        case "user": user_ = attr_.Value; break;
                                                        case "outport": outport_ = int.Parse(attr_.Value); break;
                                                        case "reels": reels_ = int.Parse(attr_.Value); break;
                                                        case "state": state_ = (ProvideJobListData.States)Enum.Parse(typeof(ProvideJobListData.States), attr_.Value); break;
                                                    }
                                                }

                                                List<ProvideMaterialData> materials_ = new List<ProvideMaterialData>();

                                                foreach (XmlNode child_ in element_.ChildNodes)
                                                {
                                                    ProvideMaterialData material_ = new ProvideMaterialData();

                                                    foreach (XmlAttribute attr_ in child_.Attributes)
                                                    {
                                                        switch (attr_.Name.ToLower())
                                                        {
                                                            case "name": material_.Name = attr_.Value; break;
                                                            case "article": material_.Category = attr_.Value; break;
                                                            case "state": material_.State = (ProvideMaterialData.States)Enum.Parse(typeof(ProvideMaterialData.States), attr_.Value); break;
                                                            case "count": material_.Quantity = int.Parse(attr_.Value); break;
                                                        }
                                                    }

                                                    materials_.Add(material_);
                                                }

                                                ProvideJobListData providejob_ = new ProvideJobListData(name_, user_, outport_, reels_, state_, materials_);

                                                if (!reservedJobs.ContainsKey(name_))
                                                    reservedJobs.Add(name_, providejob_);
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                    }

                    if (lastReservedProvideJobTimestamp == DateTime.MinValue)
                        lastReservedProvideJobTimestamp = DateTime.Now.Date;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected virtual bool ImportData(string joblist= null, string alarmlist= null)
        {
            try
            {
                List<ArticleInformation> articles_ = null;
                if (GetArticleList(ref articles_))
                {
                    articles.Clear();

                    foreach (ArticleInformation article_ in articles_)
                        AddNewArticleInformation(article_.Article, article_.ID, article_.ReelCount);
                }

                List<CarrierInformation> carriers_ = null;
                if (GetCarrierList(ref carriers_))
                {
                    carriers.Clear();

                    foreach (CarrierInformation carrier_ in carriers_)
                        AddNewCarrierInformation(carrier_);
                }

                ImportComponentRecords();

                ImportSpecialData();

                if (!initialized && !ignorePastReservedJobs)
                {
                    ImportJobData(joblist);

                    foreach (string jobname_ in reservedJobs.Keys)
                    {
                        if (provideJobs.Find(x_ => x_.first == jobname_) == null)
                        {
                            reservedJobs[jobname_].State = ProvideJobListData.States.Ready;
                            Pair<string, ProvideJobListData> job_ = new Pair<string, ProvideJobListData>(jobname_, new ProvideJobListData(reservedJobs[jobname_]));
                            provideJobs.Add(job_);
                            string materials_ = string.Empty;

                            foreach (ProvideMaterialData item_ in job_.second.Materials)
                            {
                                if (!string.IsNullOrEmpty(item_.Name) && !pendingUnloadReels.ContainsKey(item_.Name))
                                {
                                    materials_ += $"{item_.Category};{item_.Name}|";
                                    if (carriers.ContainsKey(item_.Name))
                                        pendingUnloadReels.Add(item_.Name, new Pair<string, ProvideMaterialData>(job_.first, new ProvideMaterialData(carriers[item_.Name].second)));
                                    else
                                    {   // UPDATED: 20200307 (Marcus)
                                        job_.second.RemoveMaterialData(item_);
                                        job_.second.Reels--;
                                        reservedJobs[jobname_].RemoveMaterialData(item_);
                                        reservedJobs[jobname_].Reels--;
                                    }
                                }
                            }

                            // UPDATED: 20200307 (Marcus)
                            if (job_.second.Materials.Count <= 0)
                            {
                                provideJobs.Remove(job_);
                                reservedJobs.Remove(jobname_);
                            }
                        }
                    }
                }
                // else
                //     provideJobs.Clear();

                List<JobListInformation> jobs_ = null;
                if (GetJobLists(ref jobs_))
                {
                    if (initialized && jobs_.Count <= 0)
                    {
                        reservedJobs.Clear();
                        provideJobs.Clear();
                    }
                    else
                    {
                        foreach (JobListInformation job_ in jobs_)
                        {
                            if (job_.State <= 4)
                            {
                                bool flush_ = true;
                                int completedmaterials_ = 0;
                                List<ProvideMaterialData> items_ = new List<ProvideMaterialData>();

                                foreach (JobListItem item_ in job_.List)
                                {
                                    ProvideMaterialData.States state_ = ProvideMaterialData.States.Ready;

                                    switch (item_.State)
                                    {
                                        case 3:
                                            {
                                                if (carriers.ContainsKey(item_.Carrier))
                                                {
                                                    if (!string.IsNullOrEmpty(carriers[item_.Carrier].second.Depot) &&
                                                        (carriers[item_.Carrier].second.Depot.Contains("Tower") || carriers[item_.Carrier].second.Depot[0] == 'T'))
                                                        state_ = ProvideMaterialData.States.Providing;
                                                    else
                                                    {
                                                        completedmaterials_++;
                                                        state_ = ProvideMaterialData.States.Completed;
                                                    }
                                                }
                                                else
                                                {
                                                    completedmaterials_++;
                                                    state_ = ProvideMaterialData.States.Completed;
                                                }
                                            }
                                            break;
                                        default:
                                            {
                                                if (item_.State > 3)
                                                    state_ = ProvideMaterialData.States.Completed;
                                                else
                                                    state_ = ProvideMaterialData.States.Unknown;
                                            }
                                            break;
                                    }

                                    items_.Add(new ProvideMaterialData(new string[] { item_.Article, item_.Carrier, string.Empty, item_.Reels.ToString(), item_.Comment, state_.ToString() }));
                                }

                                if (completedmaterials_ != job_.List.Count || completedmaterials_ != GetProvidedCounts(job_.Name))
                                {
                                    if (UpdateProvideJobInformation(job_.Name, items_))
                                    {
                                        foreach (Pair<string, ProvideJobListData> pendedjob_ in provideJobs)
                                        {
                                            if (pendedjob_.first == job_.Name)
                                            {
                                                switch (job_.ProdState)
                                                {
                                                    case 0:
                                                    case 1:
                                                        pendedjob_.second.State = ProvideJobListData.States.Ready;
                                                        flush_ = false;
                                                        break;
                                                    case 2:
                                                        pendedjob_.second.State = ProvideJobListData.States.Providing;
                                                        flush_ = false;
                                                        break;
                                                    case 3:
                                                    case 4:
                                                        pendedjob_.second.State = ProvideJobListData.States.Completed;
                                                        break;
                                                    default:
                                                        {   // Re-check the completed item count
                                                            if (completedmaterials_ == job_.List.Count)
                                                                pendedjob_.second.State = ProvideJobListData.States.Completed;
                                                            else
                                                                flush_ = false;
                                                        }
                                                        break;
                                                }

                                                break;
                                            }
                                        }
                                    }
                                }

                                if (flush_)
                                {
                                    items_.Clear();
                                    DeleteJobList(job_.Name);
                                }
                            }
                        }

                        if (!initialized)
                        {
                            string providingjob_ = string.Empty;

                            foreach (JobListInformation job_ in jobs_)
                                DeleteJobList(job_.Name, false);

                            foreach (KeyValuePair<string, ProvideJobListData> job_ in reservedJobs)
                            {
                                if (job_.Value.State == ProvideJobListData.States.Providing)
                                    providingjob_ = job_.Key;

                                AddProvideJob(job_.Key, job_.Value.User, job_.Value.Outport, job_.Value.Materials, true);
                            }

                            if (!string.IsNullOrEmpty(providingjob_))
                                ProvideJob(providingjob_);
                        }

                        initialized = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return ImportAlarmData(string.IsNullOrEmpty(alarmlist)? CONST_ALARM_LIST_FILE : alarmlist, cultureCode);
        }

        protected virtual void SetTowerState(ReelTowerState state)
        {
            try
            {
                if (state.OnlineState && !state.HasAlarm && state.StatusCode != 1032 && (state.StatusText.ToLower() == "ready" || state.State == MaterialStorageState.StorageOperationStates.Idle))
                {
                    state.State = MaterialStorageState.StorageOperationStates.Idle;

                    // UPDATED: 20200410 (Marcus)
                    // Notice! It changed the material handling direction immediately during load a reel process.
                    state.SetMaterialDestination(MaterialStorageState.MaterialHandlingDestination.None);

                    if (loadReels.ContainsKey(state.Id))
                    {
                        lock (loadReels)
                            loadReels.Remove(state.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }
        #endregion

        #region Public methods
        public virtual void IgnorePastReservedJob(bool state)
        {
            ignorePastReservedJobs = state;
        }

        public virtual bool HasReservedJob(string reservedjobfile = null)
        {
            try
            {
                if (string.IsNullOrEmpty(reservedjobfile))
                    reservedjobfile = CONST_RESERVED_JOB_DATA_FILE;

                if (File.Exists(reservedjobfile))
                {
                    XmlDocument xml_ = new XmlDocument();
                    xml_.Load(reservedjobfile);

                    if (File.Exists(reservedjobfile))
                    {
                        XmlNodeList elements_ = xml_.GetElementsByTagName("Job");
                        return elements_.Count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return false;
        }

        public virtual void ResetTowerState(ReelTowerState state)
        {
            SetTowerState(state);
        }

        public virtual void SetTowerState(string towername, MaterialStorageState.StorageOperationStates state)
        {
            ReelTowerState state_ = GetTowerStateByName(towername);

            if (state_ != null)
                state_.State = state;
        }

        public virtual void ResetTowerStateByName(string towername)
        {
            try
            {
                if (string.IsNullOrEmpty(towername))
                {
                    foreach (KeyValuePair<int, ReelTowerState> state_ in states)
                        SetTowerState(state_.Value);
                }
                else
                {
                    if (towers.Where(x_ => x_.Value.Name == towername).Any())
                    {
                        foreach (KeyValuePair<int, ReelTowerState> state_ in states)
                        {
                            if (state_.Value.Name == towername)
                                SetTowerState(state_.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public virtual void ForceDelteCompleteedJobList(List<string> jobnames, string towerid= null)
        {
            DeleteCompletedJobList(jobnames);
            
            if (!string.IsNullOrEmpty(towerid))
                AsyncUpdateTowerState(towerid);
        }

        public virtual void FireProvideMaterialStateChangedByTowerName(string towername, string carriername)
        {
            FireProvideMaterialStateChanged(GetTowerIdByName(towername), carriername);
        }

        public virtual void FireProvideMaterialStateChanged(string towerid, string carriername)
        {
            try
            {
                bool providing_ = false;

                if (!string.IsNullOrEmpty(towerid))
                {
                    ReelTowerState state_ = TowerStates[towerIds[towerid]];

                    switch (state_.State)
                    {
                        case MaterialStorageState.StorageOperationStates.Idle:
                        case MaterialStorageState.StorageOperationStates.Error:
                        case MaterialStorageState.StorageOperationStates.RequestedToUnload:
                        case MaterialStorageState.StorageOperationStates.PrepareToUnload:
                        case MaterialStorageState.StorageOperationStates.Unload:
                            {
                                providing_ = true;
                            }
                            break;
                    }

                    if (carriers.ContainsKey(carriername))
                    {
                        if (providing_)
                            carriers[carriername].second.State = ProvideMaterialData.States.Completed;

                        FireProvideMaterialStateChanged(carriers[carriername].second);
                    }
                    else if (pendingUnloadReels.ContainsKey(carriername))
                    {
                        if (providing_)
                            pendingUnloadReels[carriername].second.State = ProvideMaterialData.States.Completed;

                        FireProvideMaterialStateChanged(pendingUnloadReels[carriername].second);
                    }

                    if (providing_ && pendingUnloadReels.Count > 0 && provideJobs.Count > 0 && pendingUnloadReels.ContainsKey(carriername))
                    {
                        string jobname_ = pendingUnloadReels[carriername].first;
                        string articlename_ = pendingUnloadReels[carriername].second.Category;
                        List<ProvideJobListData> providejobs_ = provideJobs.Where(x_ => x_.first == jobname_).Select(y_ => y_.second).ToList();
                        List<string> completedjobs_ = new List<string>();

                        foreach (ProvideJobListData job_ in providejobs_)
                        {
                            List<ProvideMaterialData> data_ = job_.Materials.Where(x_ => x_.Name == carriername).ToList();

                            foreach (ProvideMaterialData item_ in data_)
                                item_.State = ProvideMaterialData.States.Completed;

                            if (!job_.Materials.Where(x_ => x_.State < ProvideMaterialData.States.Completed).Any())
                                completedjobs_.Add(jobname_);
                        }

                        if (!carriername.Contains("reject_"))
                            RemoveCarrier(carriername);

                        RemoveCarrierInformation(carriername);
                        pendingUnloadReels.Remove(carriername);

                        if (unloadReels.ContainsKey(towerid))
                            unloadReels.Remove(towerid);

                        ForceDelteCompleteedJobList(completedjobs_, towerid);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public virtual Pair<string, ThreeField<string, MaterialData, bool>> GetLoadReelInformation(string towername)
        {
            try
            {
                if (towers.Where(x_ => x_.Value.Name == towername).Any())
                {
                    var item_ = towers.First(x_ => x_.Value.Name == towername);

                    if (loadReels.ContainsKey(item_.Value.Id))
                        return new Pair<string, ThreeField<string, MaterialData, bool>>(item_.Value.Id, loadReels[item_.Value.Id]);
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return null;
        }

        public virtual ProvideMaterialData GetTransferMaterialName(string towerid)
        {
            try
            {
                if (loadReels.ContainsKey(towerid))
                {
                    return new ProvideMaterialData(loadReels[towerid].second);
                }
                else if (unloadReels.ContainsKey(towerid))
                {
                    return unloadReels[towerid].fourth;
                }            
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return null;
        }

        public virtual MaterialData GetLoadMaterialName(string towerid)
        {
            try
            {
                if (loadReels.ContainsKey(towerid))
                {
                    MaterialData result_ = loadReels[towerid].second;
                    return result_;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return null;
        }

        public virtual ProvideMaterialData GetUnloadMaterialName(string towerid)
        {
            if (unloadReels.ContainsKey(towerid))
            {
                ProvideMaterialData result_ = unloadReels[towerid].fourth;
                return result_;
            }

            return null;
        }

        public virtual bool CancelRemoveLoadMaterialName(string towerid)
        {
            bool result_ = false;

            try
            {
                if (loadReels.ContainsKey(towerid))
                {
                    loadReels.Remove(towerid);
                    result_ = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public virtual bool GetProvideMaterialData(string carriername, string towerid, ProvideMaterialData.States state, ref ProvideMaterialData data)
        {
            bool result_ = false;

            try
            {
                if (pendingUnloadReels.Count > 0 && pendingUnloadReels.ContainsKey(carriername))
                {
                    Pair<string, ProvideJobListData> provideJob_ = null;
                    Pair<string, ProvideMaterialData> item_ = pendingUnloadReels[carriername];

                    if ((provideJob_ = provideJobs.Find(x_ => x_.first == item_.first)) != null && towerIds.ContainsKey(towerid))
                    {
                        item_.second.State = state;
                        provideJob_.second.SetMaterialState(item_.second.Name, item_.second.State);
                        data = item_.second;
                        data.TowerId = towerid;
                        data.TowerName = towers[towerIds[towerid]].Name;
                        data.Text = $"Output_No_{provideJob_.second.Outport}";
                        result_ = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public virtual ReelTowerState GetTowerStateByName(string towername)
        {
            try
            {
                if (towers.Where(x_ => x_.Value.Name == towername).Any())
                {
                    var item_ = towers.First(x_ => x_.Value.Name == towername);
                    return TowerStates[item_.Key];
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return null;
        }

        public virtual ReelTowerState GetTowerStateById(string towerid)
        {
            try
            {
                if (!towerIds.ContainsKey(towerid))
                    return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return states[towerIds[towerid]];
        }

        public virtual string GetTowerNameById(string towerid)
        {
            try
            {
                if (!towerIds.ContainsKey(towerid))
                    return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return towers[towerIds[towerid]].Name;
        }

        public virtual string GetTowerIdByName(string towername)
        {
            try
            {
                if (towers.Where(x_ => x_.Value.Name == towername).Any())
                {
                    var item_ = towers.First(x_ => x_.Value.Name == towername);
                    return towerIds.First(x_ => x_.Value == item_.Key).Key;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return null;
        }

        public virtual int GetArticleIdByName(string articlename, bool subfix = false)
        {
            try
            {
                var item_ = articles.Where(x_ => subfix ? x_.Key.Remove(0, x_.Key.Length - articlename.Length) == articlename : x_.Key == articlename);

                if (item_ != null && item_.Any())
                    return item_.First().Value.first;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return -1;
        }

        public virtual string GetArticleNameById(int articleid)
        {
            try
            {
                var item_ = articles.Where(x_ => x_.Value.first == articleid);

                if (item_ != null && item_.Any())
                    return item_.First().Key;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return string.Empty;
        }

        public virtual DataSet QueryArticle(string query)
        {
            try
            {
                if (ComponentDB.IsExistDB(false))
                    return ComponentDB.Select(query);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return null;
        }

        public virtual DataSet QueryCarrier(string query)
        {
            try
            {
                if (ComponentDB.IsExistDB(false))
                    return ComponentDB.Select(query);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return null;
        }

        public virtual bool IsRecordExistArticle(string articlename)
        {
            ArticleInformation result_ = null;
            GetArticleInformation(articlename, ref result_);
            return (result_ != null);
        }

        public virtual bool IsRecordExistCarrier(string carriername)
        {
            CarrierInformation result_ = null;
            bool a = GetCarrierInformation(carriername, ref result_);
            return a;
            //return (result_ != null);
        }

        public virtual bool IsStoredCarrier(string carriername, ref string depot)
        {
            bool result_                = false;
            CarrierInformation info_    = null;

            if (GetCarrierInformation(carriername, ref info_))
            {
                KeyValuePair<string, Pair<int, int>> item_ = articles.Where(x => x.Value.first == info_.Article).First();

                if (!string.IsNullOrEmpty(item_.Key))
                {
                    if (!string.IsNullOrEmpty(info_.Depot) && info_.Depot.Length > 5 && info_.Depot[0] == 'T')
                    {
                        depot = info_.Depot;
                        result_ = true;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(info_.Depot) && info_.Depot.Length > 5 && info_.Depot[0] == 'T')
                    {
                        depot = $"Mismatched article=empty,depot={info_.Depot}";
                        result_ = true;
                    }
                    else
                        depot = $"Article information is not proper ({item_.Key})";
                }
            }

            return result_;
        }

        public virtual bool IsStoredCarrier(string carriername, string articlename, ref string depot)
        {
            bool result_                = false;
            CarrierInformation info_    = null;

            if (GetCarrierInformation(carriername, ref info_) && info_ != null)
            {
                KeyValuePair<string, Pair<int, int>> item_ = articles.Where(x => x.Value.first == info_.Article).First();

                if (!string.IsNullOrEmpty(item_.Key) && item_.Key == articlename)
                {
                    if (!string.IsNullOrEmpty(info_.Depot) && info_.Depot.Length > 5 && info_.Depot[0] == 'T')
                    {
                        depot = info_.Depot;
                        result_ = true;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(info_.Depot) && info_.Depot.Length > 5 && info_.Depot[0] == 'T')
                    {
                        depot = $"Mismatched article={item_.Key},depot={info_.Depot}";
                        result_ = true;
                    }
                    else
                        depot = $"Article information is not proper ({item_.Key})";
                }
            }

            return result_;
        }

        public virtual bool IsStoredMaterial(string articlename, string carriername, ref string depot)
        {
            return IsStoredCarrier(carriername, articlename, ref depot);
        }

        // Not use
        public virtual bool IsExistJob(string jobname)
        {
            JobListInformation result_ = null;
            GetJobList(jobname, ref result_);
            return (result_ != null);
        }

        public virtual bool IsFinishedJob(string jobname)
        {
            bool result_ = false;
            int error_ = 0;
            string msg_ = string.Empty;

            try
            {
                ClientChannel client_ = new ClientChannel();
                object obj_ = client_.DoAction("GetJobList", jobname, ref error_, ref msg_);

                if (obj_ != null)
                {
                    JobListInformation data = obj_ as JobListInformation;
                    // List<ProvideMaterialData> items_ = new List<ProvideMaterialData>();

                    if (data.State >= 3 && data.ProdState > 2)
                        result_ = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public virtual bool LoadReel(string towerid, string towername, ref ThreeField<string, string, string> data)
        {
            bool result_ = false;

            try
            {
                if (loadReels.ContainsKey(towerid))
                {
                    LoadMaterialInformation material_ = new LoadMaterialInformation();
                    data.first = material_.TowerId = towerid;
                    data.second = material_.Article = (loadReels[towerid].second != null ? loadReels[towerid].second.Category : loadReels[towerid].first);
                    data.third = material_.Barcode = loadReels[towerid].first;

                    for (int i_ = 0; i_ < 10; i_++)
                    {
                        if (SimulateStartButton(material_))
                        {
                            Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Called SimulateStartButton! ({towerid};{towername};{material_.Article};{material_.Barcode})");
                            result_ = true;
                            break;
                        }
                        else
                        {
                            Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Failed to call SimulateStartButton. ({towerid};{towername};{material_.Article};{material_.Barcode})");
                        }
                    }
                }
                else
                    Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Load reel information is not available. ({towerid};{towername})");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message} ({towerid};{towername})");
            }

            return result_;
        }

        public virtual bool UpdateCarrier(string towername, string articlename, string carriername, string lotid, string supplier, string mfg, string data, int qty, ReelDiameters diameter, ReelThicknesses thickness, LoadMaterialTypes loadtype = LoadMaterialTypes.Cart, bool bymfg = false)
        {
            bool result_ = false;

            try
            {
                if (!string.IsNullOrEmpty(articlename) && !string.IsNullOrEmpty(carriername))
                {
                    if (!(result_ = IsRecordExistArticle(articlename)))
                    {
                        ArticleInformation article_ = new ArticleInformation();
                        article_.Article = articlename;

                        if (result_ = NewArticle(article_))
                            GetArticleInformation(articlename, ref article_);
                    }

                    if (result_ && articles.ContainsKey(articlename))
                    {
                        CarrierInformation carrier_ = new CarrierInformation();

                        if (bymfg)
                        {
                            if (mfg.Length > 8)
                                mfg = mfg.Substring(0, 8);
                            else if (mfg.Length < 8)
                                return false;

                            carrier_.CreateDate = DateTime.ParseExact(mfg, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        }
                        else
                            carrier_.CreateDate = DateTime.Now;

                        carrier_.ArticleName = articlename;
                        carrier_.Article = articles[articlename].first;
                        carrier_.Carrier = carriername;
                        carrier_.Diameter = Convert.ToInt32(diameter);
                        carrier_.Height = thickness <= ReelThicknesses.Unknown ? 0 : Convert.ToInt32(thickness.ToString().Remove(0, "ReelThickness".Length));
                        carrier_.Stock = qty;
                        carrier_.Manufactur = supplier;
                        carrier_.Custom1 = lotid;
                        carrier_.Custom2 = mfg;
                        result_ = UpdateCarrier(carrier_);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public virtual bool CreateCarrier(string towername, string articlename, string carriername, string lotid, string supplier, string mfg, string data, int qty, ReelDiameters diameter, ReelThicknesses thickness, LoadMaterialTypes loadtype = LoadMaterialTypes.Cart, bool bymfg = false)
        {
            bool result_ = false;

            try
            {
                if (!string.IsNullOrEmpty(towername) && !string.IsNullOrEmpty(articlename) && !string.IsNullOrEmpty(carriername))
                {
                    if (!(result_ = IsRecordExistArticle(articlename)))
                    {
                        ArticleInformation article_ = new ArticleInformation();
                        article_.Article = articlename;

                        if (result_ = NewArticle(article_))
                            GetArticleInformation(articlename, ref article_);
                    }

                    if (result_ && articles.ContainsKey(articlename))
                    {
                        CarrierInformation carrier_ = new CarrierInformation();

                        if (bymfg)
                        {
                            if (mfg.Length > 8)
                                mfg = mfg.Substring(0, 8);
                            else if (mfg.Length < 8)
                                return false;

                            carrier_.CreateDate = DateTime.ParseExact(mfg, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        }
                        else
                            carrier_.CreateDate = DateTime.Now;

                        carrier_.ArticleName = articlename;
                        carrier_.Article = articles[articlename].first;
                        carrier_.Carrier = carriername;
                        carrier_.Diameter = Convert.ToInt32(diameter);
                        carrier_.Height = thickness <= ReelThicknesses.Unknown? 0 : Convert.ToInt32(thickness.ToString().Remove(0, "ReelThickness".Length));
                        carrier_.Stock = qty;
                        carrier_.Manufactur = supplier;
                        carrier_.Custom1 = lotid;
                        carrier_.Custom2 = mfg;
                        result_ = NewCarrier(carrier_);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public virtual bool AddProvideJob(string jobname, string user, int outport, IReadOnlyList<ProvideMaterialData> items, bool restore = false)
        {
            bool result_                                    = false;
            Pair<string, ProvideJobListData> provideJob_    = null;

            try
            {
                if (((provideJob_ = provideJobs.Find(x_ => x_.first == jobname)) != null && !restore) || IsAllTowerOffline)
                    return false;

                JobListInformation job_     = new JobListInformation();            
                job_.Name                   = jobname;
                job_.CreateUser             = user;
                job_.CreateDate             = DateTime.Now;
                job_.BaseMount              = false;
                job_.AutoDelete             = true;
                job_.LosSize                = 1;
                job_.State                  = 0;
                job_.ProdSite               = $"{site}-G{id}";
                job_.ProdDate               = DateTime.Now.AddHours(1.0);
                job_.ProdState              = 2;
                job_.List                   = new List<JobListItem>();

                foreach (ProvideMaterialData item_ in items)
                {
                    if (!carriers.ContainsKey(item_.Name))
                    {   // Synchronize carrier information.
                        CarrierInformation temp_ = null;
                        GetCarrierInformation(item_.Name, ref temp_);
                    }
                
                    // Update supplier information
                    item_.Supplier = carriers[item_.Name].second.Supplier;

                    JobListItem reel_ = new JobListItem();
                    reel_.Article = item_.Category;
                    reel_.Carrier = item_.Name;
                    reel_.Reels = 1;
                    job_.List.Add(reel_);
                }

                if (result_ = NewJobList(job_, user, outport) && !restore)
                {
                    if (!reservedJobs.ContainsKey(job_.Name))
                    {
                        reservedJobs.Add(job_.Name, new ProvideJobListData(jobname, user, outport, items.Count, ProvideJobListData.States.Created, items));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public virtual bool Init(string path)
        {
            if (towerIds.Count <= 0 || (towers.Count != towerIds.Count))
                return false;

            int result_                         = 0;
            List<TowerBaseInformation> towers_  = null;

            try
            { 
                if (GetTowers(ref towers_))
                {
                    if (towers_ != null)
                    {
                        foreach (TowerBaseInformation tower_ in towers_)
                        {
                            if (towerIds.ContainsKey(tower_.TowerId))
                            {
                                TowerDetailInformation info_ = null;

                                if (GetTowerInformation(tower_.TowerId, ref info_))
                                {
                                    if (towers.ContainsKey(towerIds[tower_.TowerId]))
                                    {
                                        towers[towerIds[tower_.TowerId]].Init(info_);
                                        states[towerIds[tower_.TowerId]].OnlineState    = info_.OnlineStatus.ToLower() == "online";
                                        states[towerIds[tower_.TowerId]].StatusText     = info_.StatusText;
                                        states[towerIds[tower_.TowerId]].StatusCode     = info_.StatusCode;
                                        result_++;
                                    }
                                }
                            }
                        }

                        foreach (KeyValuePair<string, DatabaseManager> db_ in databaseManagers)
                        {
                            switch (db_.Value.Name.ToLower())
                            {
                                case "componentdatabase":
                                    ComponentDB.Init(string.Empty, "Article");
                                    break;
                                case "accountdatabase":
                                    AccountDB.Init(App.Path + path, "Account");
                                    break;
                            }
                        }

                        ImportData();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return towers.Count == result_;
        }

        public virtual bool Create(XmlNode node, string culturecode = "en-US")
        {
            bool result_ = false;
            cultureCode = culturecode;

            try
            { 
                if (node != null)
                {
                    Clear();

                    foreach (XmlAttribute attr_ in node.Attributes)
                    {
                        switch (attr_.Name.ToLower())
                        {
                            case "id":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        id = int.Parse(attr_.Value);
                                }
                                break;
                            case "site":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        site = attr_.Value;
                                }
                                break;
                            case "linecode":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        linecode = attr_.Value;
                                }
                                break;
                            case "groupname":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        groupname = attr_.Value;
                                }
                                break;
                            case "customer":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        customer = (Customers)Enum.Parse(typeof(Customers), attr_.Value);
                                }
                                break;
                            case "maker":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        maker = (DeviceManufacturer)Enum.Parse(typeof(DeviceManufacturer), attr_.Value);
                                }
                                break;
                            case "path":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        reservedJobFile = attr_.Value;
                                }
                                break;
                        }
                    }

                    foreach (XmlNode element_ in node.ChildNodes)
                    {
                        switch (element_.Name.ToLower())
                        {
                            case "reeltower":
                                {
                                    ReelTower tower_ = new ReelTower(element_);

                                    if (tower_.IsValid && !towers.ContainsKey(tower_.Index))
                                    {
                                        towerIds.Add(tower_.Id, tower_.Index);
                                        towers.Add(tower_.Index, tower_);
                                        states.Add(tower_.Index, new ReelTowerState(tower_.Index, tower_.Id, tower_.Name, OnReelTowerStateChangedEvent));
                                    }
                                }
                                break;
                            case "databases":
                                {
                                    foreach (XmlNode child_ in element_.ChildNodes)
                                    {
                                        DatabaseNode item_ = new DatabaseNode(child_);

                                        if (item_.IsValid)
                                            databaseManagers.Add(item_.Name, new DatabaseManager(item_.Name, item_.DbServer, item_.DbName, item_.DbPort, item_.DbUser, item_.DbPassword, item_.DbType, item_.OleDriver, item_.DbServer.Contains("localdb") ? true : false));
                                    }
                                }
                                break;
                        }
                    }

                    if (towerIds.Count > 0 && alarmRecords.Count <= 0)
                    {
                        foreach (string id_ in towerIds.Keys)
                            alarmRecords.Add(id_, new List<ThreeField<int, string, DateTime>>());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public virtual XElement ToXml(string nodename = null)
        {
            try
            {
                XElement result_ = new XElement(string.IsNullOrEmpty(nodename) ? GetType().Name : nodename,
                    new XAttribute("id", id),
                    new XAttribute("maker", maker),
                    new XAttribute("customer", customer),
                    new XAttribute("site", site),
                    new XAttribute("linecode", linecode),
                    new XAttribute("groupname", groupname));
                        


                if (!string.IsNullOrEmpty(reservedJobFile))
                    result_.Add(new XAttribute("path", reservedJobFile));

                foreach (KeyValuePair<int, ReelTower> tower_ in towers)
                    result_.Add(tower_.Value.ToXml());

                if (databaseManagers.Count > 0)
                {
                    XElement nodes_ = new XElement("Databases");

                    foreach (KeyValuePair<string, DatabaseManager> node_ in databaseManagers)
                        nodes_.Add(node_.Value.ToXml("Database"));

                    result_.Add(nodes_);
                }

                return result_;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                return null;
            }
        }

        public virtual void SaveSpecialMaterialData()
        {
            ExportSpecialData();
        }

        public virtual void SaveReservedJobData()
        {
            try
            { 
                XElement result_ = new XElement("ReservedJobs",
                    new XAttribute("timestamp", lastReservedProvideJobTimestamp),
                    new XAttribute("last", lastReservedProvideJob),
                    new XAttribute("index", processedProvideJobCount));

                foreach (KeyValuePair<string, ProvideJobListData> job_ in reservedJobs)
                {
                    Pair<string, ProvideJobListData> providejob_ = provideJobs.Find(x_ => x_.first == job_.Key);

                    if (providejob_ != null && providejob_.second.State <= ProvideJobListData.States.Providing)
                    {
                        XElement item_ = new XElement("Job",
                            new XAttribute("name", job_.Key),
                            new XAttribute("state", providejob_.second.State != job_.Value.State ? providejob_.second.State : job_.Value.State),
                            new XAttribute("user", job_.Value.User),
                            new XAttribute("outport", job_.Value.Outport),
                            new XAttribute("reels", job_.Value.Reels));

                        foreach (ProvideMaterialData material_ in providejob_.second.Materials)
                            item_.Add(new XElement("Carrier",
                                new XAttribute("name", material_.Name),
                                new XAttribute("article", material_.Category),
                                new XAttribute("state", material_.State),
                                new XAttribute("count", material_.Quantity)));

                        result_.Add(item_);
                    }
                }

                result_.Save(reservedJobFile);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public virtual void Clear()
        {
            towerIds.Clear();
            towers.Clear();
            states.Clear();
        }

        public virtual void Destroy()
        {
            SaveReservedJobData();
            SaveSpecialMaterialData();
            Clear();
        }

        public virtual List<ThreeField<string, string, int>> GetProvideJobReel(List<Pair<string, string>> unloaditems)
        {
            List<ThreeField<string, string, int>> result_ = new List<ThreeField<string, string, int>>();

            try
            {
                foreach (Pair<string, string> item_ in unloaditems)
                {
                    var job_ = pendingUnloadReels.Where(x_ => x_.Value.second.Category == item_.first && x_.Value.second.Supplier == item_.second).ToList();

                    if (job_.Count > 0)
                        result_.Add(new ThreeField<string, string, int>(job_[0].Value.second.Category, job_[0].Value.second.Supplier, job_.Count));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_.Count <= 0 ? null : result_;
        }

        public virtual List<FourField<string, string, string, int>> GetProvideJobReel(List<ThreeField<string, string, string>> unloaditems)
        {
            List<FourField<string, string, string, int>> result_ = new List<FourField<string, string, string, int>>();

            try
            {
                foreach (ThreeField<string, string, string> item_ in unloaditems)
                {
                    var job_ = pendingUnloadReels.Where(x_ => x_.Value.second.Category == item_.first && x_.Value.second.Name == item_.second && x_.Value.second.Supplier == item_.third).ToList();

                    if (job_.Count > 0)
                        result_.Add(new FourField<string, string, string, int>(job_[0].Value.second.Category, job_[0].Value.second.Name, job_[0].Value.second.Supplier, job_.Count));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_.Count <= 0 ? null : result_;
        }

        public virtual List<Pair<string, string>> HasProvideJobReel(List<FourField<string, string, string, int>> unloaditems)
        {
            List<Pair<string, string>> result_ = new List<Pair<string, string>>();

            try
            {
                foreach (FourField<string, string, string, int> item_ in unloaditems)
                {
                    var job_ = pendingUnloadReels.Where(x_ => x_.Value.second.Category == item_.first && x_.Value.second.Name == item_.second).ToList();

                    if (job_.Count > 0)
                        result_.Add(new Pair<string, string>(job_[0].Value.second.Category, job_[0].Value.second.Name));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_.Count <= 0 ? null : result_;
        }

        public virtual List<Pair<string, string>> HasProvideJobReel(List<FiveField<string, string, string, int, string>> unloaditems)
        {
            List<Pair<string, string>> result_ = new List<Pair<string, string>>();

            try
            {
                foreach (FiveField<string, string, string, int, string> item_ in unloaditems)
                {
                    var job_ = pendingUnloadReels.Where(x_ => x_.Value.second.Category == item_.first && x_.Value.second.Name == item_.second).ToList();

                    if (job_.Count > 0)
                        result_.Add(new Pair<string, string>(job_[0].Value.second.Category, job_[0].Value.second.Name));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_.Count <= 0 ? null : result_;
        }

        public virtual bool AddLoadReel(string towername, string articlename, string carriername, string lotid, string supplier, string mfg, int size, string data, string comment, int qty, ReelDiameters diameter, ReelThicknesses thickness, LoadMaterialTypes loadtype = LoadMaterialTypes.Cart)
        {
            bool result_ = false;

            try
            {
                lock (loadReels)
                {
                    ReelTower tower_ = towers.First(x => x.Value.Name == towername).Value;
                    ReelTowerState towerstate_ = null;

                    if (tower_ != null && (towerstate_ = GetTowerStateByName(towername)) != null)
                    {
                        if (loadReels.ContainsKey(tower_.Id))
                        {
                            switch (towerstate_.State)
                            {
                                case MaterialStorageState.StorageOperationStates.RequestedToLoad:
                                case MaterialStorageState.StorageOperationStates.PrepareToLoad:
                                case MaterialStorageState.StorageOperationStates.Load:
                                    return false;
                                default:
                                    loadReels.Remove(tower_.Id);
                                    break;
                            }
                        }

                        if (!autoRun || ManualOperation)
                            towerstate_.State = MaterialStorageState.StorageOperationStates.RequestedToLoad;

                        loadReels.Add(tower_.Id, new ThreeField<string, MaterialData, bool>(carriername, new MaterialData(towername, articlename, carriername, lotid, supplier, mfg, size, data, comment, qty, diameter, thickness, loadtype), false));
                        result_ = true;
                    }
                    else
                    {
                        Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Abnormal case load reel tower state is not corret.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public virtual void AddUnloadReel(string jobid, string user, int outport, List<string> reels)
        {
        }

        protected virtual int UpdateTowerStatus(TowerDetailInformation data, ref ReelTowerState state)
        {
            try
            {
                if (data == null)
                    return 0;

                lock (towers)
                {
                    if (towerIds.ContainsKey(data.TowerId))
                    {
                        int fullslot_       = 0;
                        int idx_            = towerIds[data.TowerId];
                        ReelTower tower_    = towers[idx_];
                        state               = TowerStates[idx_];
                        tower_.UpdateInfo(data);
                        
                        state.OnlineState    = (data.OnlineStatus.ToLower() == "online");
                        
                        foreach (Slot slot_ in data.Slots)
                        {
                            if (slot_.Slots_free == 0)
                                fullslot_++;
                        }

                        if (fullslot_ == data.Slots.Count)
                        {
                            state.State = MaterialStorageState.StorageOperationStates.Full;
                            state.StatusCode = 0;
                            state.StatusText = "Full";
                        }
                        else
                        {
                            state.StatusCode = data.StatusCode;
                            state.StatusText = data.StatusText;
                        }

                        switch (data.StatusCode)
                        {
                            case 26:
                                {
                                    if (!state.HasAlarm)
                                    {
                                        if (ClearAlarm(data.TowerId, state.LastAlarmCode))
                                        {
                                             if (state.MaterialDestination != MaterialStorageState.MaterialHandlingDestination.None)
                                                state.State = MaterialStorageState.StorageOperationStates.Idle;

                                            FireReportAlarmLog(false, data.TowerName, data.TowerId, state.LastAlarmCode);
                                        }
                                    }
                                }
                                break;
                            case 1032:
                                {   // Manual run
                                    switch (state.State)
                                    {
                                        case MaterialStorageState.StorageOperationStates.RequestedToLoad:
                                        case MaterialStorageState.StorageOperationStates.PrepareToLoad:
                                            {
                                                if ((!autoRun || ManualOperation) && loadReels.ContainsKey(tower_.Id) && !loadReels[tower_.Id].third)
                                                {
                                                    object arg_ = new MaterialEventArgs(tower_.Name, tower_.Id, loadReels[tower_.Id].first, loadReels[tower_.Id].first, MaterialEventArgs.MaterialActions.PreapareLoad, loadReels[tower_.Id].second);
                                                    new TaskFactory().StartNew(new Action<object>((x_) => {
                                                        FireMaterialEventRaised(x_ as MaterialEventArgs);
                                                    }), arg_);
                                                    loadReels[tower_.Id].third = true;
                                                }
                                            }
                                            break;
                                    }
                                }
                                break;
                            default:
                                {
                                    if (state.HasAlarm)
                                    {
                                        if (SetAlarm(data.TowerId, state.AlarmCode))
                                        {
                                            FireReportAlarmLog(true, data.TowerName, data.TowerId, state.AlarmCode);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return data.StatusCode;
        }

        public virtual int UpdateTowerStatus(string state, ref ReelTowerState obj)
        {
            return UpdateTowerStatus(XmlCommandSerializer.DeserializeTowerDetailInformation(state), ref obj);
        }

        public virtual void UpdateArticleStatus(string notification, string state)
        {
            try
            {
                Dictionary<string, string> fields_;
                fields_ = XmlCommandSerializer.XmlToDictionary(state);
                string articlename_ = $"{fields_["COMPONENT"]}";

                switch (notification)
                {
                    case "NotifyComponentNew":
                        {
                            if (!articles.ContainsKey(articlename_))
                                AddNewArticleInformation(articlename_);
                        }
                        break;
                    case "NotifyComponentDelete":
                        {
                            if (articles.ContainsKey(articlename_))
                                RemoveArticleInformation(articlename_);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public virtual bool UpdateCarrierStatus(string notification, string state, ref string carriername, ref string towerid, ref ProvideMaterialData data)
        {
            bool result_ = false;

            try
            {
                Dictionary<string, string> fields_;
                fields_             = XmlCommandSerializer.XmlToDictionary(state);
                carriername         = $"{fields_["CARRIER"]}";

                switch (notification)
                {
                    case "NotifyCarrierNew":
                        {
                            CheckCarrierInformation(carriername);
                            result_ = true;
                        }
                        break;
                    case "NotifyCarrierDelete":
                        {
                            if (carriers.ContainsKey(carriername))
                            {
                                towerid = carriers[carriername].second.TowerId;

                                if (towerIds.ContainsKey(towerid) && unloadReels.ContainsKey(towerid))
                                {   // Normal case.
                                    RemoveCarrierInformation(carriername);
                                }

                                result_ = true;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public virtual void UpdateRequestToLoadState(string state, ref string towerid, ref string carriername)
        {   // Delayed store process...
            try
            {
                Dictionary<string, string> fields_;
                fields_             = XmlCommandSerializer.XmlToDictionary(state);
                string temp_ = $"{fields_["CARRIER"]}";
                carriername = temp_;

                List<KeyValuePair<string, ThreeField<string, MaterialData, bool>>> items_ = loadReels.Where(x_ => x_.Value.first == temp_).ToList();

                if (items_.Count > 0)
                {
                    int idx_ = towerIds[towerid = items_[0].Key];
                    ReelTower tower_ = towers[idx_];
                    states[idx_].State = MaterialStorageState.StorageOperationStates.Load;
                    CheckCarrierInformation(carriername, false);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public virtual void UpdateRequestToUnloadState(string state)
        {
            try
            {
                Dictionary<string, string> fields_;
                fields_             = XmlCommandSerializer.XmlToDictionary(state);
                string carriername_ = $"{fields_["CARRIER"]}";
                string towerid_     = $"{fields_["TOWER"]}";
                string jobname_     = string.Empty;

                if (towerIds.ContainsKey(towerid_))
                {
                    int index_                              = towerIds[towerid_];
                    ReelTower tower_                        = towers[index_];
                    Pair<string, ProvideJobListData> job_   = null;
                    ProvideMaterialData material_           = null;

                    switch (states[index_].State)
                    {
                        case MaterialStorageState.StorageOperationStates.Idle:
                        case MaterialStorageState.StorageOperationStates.Load: // Maybe or not
                        case MaterialStorageState.StorageOperationStates.Error: // Maybe or not
                        case MaterialStorageState.StorageOperationStates.RequestedToUnload:
                        case MaterialStorageState.StorageOperationStates.PrepareToUnload:
                        case MaterialStorageState.StorageOperationStates.Unload:
                            {   // A reel was unloaded and placed on terminal.
                                if (unloadReels.ContainsKey(tower_.Id))
                                    unloadReels.Remove(tower_.Id);

                                if (pendingUnloadReels.ContainsKey(carriername_))
                                {
                                    jobname_ = pendingUnloadReels[carriername_].first;
                                    material_ = pendingUnloadReels[carriername_].second;

                                    if ((job_ = provideJobs.Find(x_ => x_.first == jobname_)) != null)
                                    {
                                        unloadReels.Add(tower_.Id,
                                            new FourField<string, string, int, ProvideMaterialData>(
                                                jobname_,
                                                carriername_,
                                                job_.second.Outport,
                                                new ProvideMaterialData(material_)));

                                        RemoveCarrier(carriername_);
                                    }

                                    states[index_].State = MaterialStorageState.StorageOperationStates.RequestedToUnload;
                                }
                                else
                                    Debug.WriteLine($">>> Wrong State! {carriername_} is not pended");
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public virtual void UpdateTowerStatusByIdAfterCarrierLoad(string state, ref string towerid_)
        {
            try
            {
                Dictionary<string, string> fields_;
                fields_             = XmlCommandSerializer.XmlToDictionary(state);
                string carriername_ = $"{fields_["CARRIER"]}";
                towerid_     = $"{fields_["TOWER"]}";

                if (towerIds.ContainsKey(towerid_))
                {
                    int index_ = towerIds[towerid_];
                    ReelTower tower_ = towers[index_];

                    switch (states[index_].State)
                    {
                        case MaterialStorageState.StorageOperationStates.RequestedToLoad:
                        case MaterialStorageState.StorageOperationStates.PrepareToLoad:
                        case MaterialStorageState.StorageOperationStates.Load:
                            {
                                if (loadReels.ContainsKey(towerid_))
                                {
                                    lock (loadReels)
                                        loadReels.Remove(towerid_);
                                }

                                states[index_].State = MaterialStorageState.StorageOperationStates.Idle;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public virtual bool UpdatePrepareToUnloadState(string state, ref string towerid, ref string carriername, ref ProvideMaterialData data)
        {
            bool result_ = false;

            try
            {
                Dictionary<string, string> fields_;
                fields_     = XmlCommandSerializer.XmlToDictionary(state);
                carriername = $"{fields_["CARRIER"]}";
                towerid     = $"{fields_["TOWER"]}";

                if (towerIds.ContainsKey(towerid))
                {
                    int index_          = towerIds[towerid];
                    ReelTower tower_    = towers[index_];

                    switch (TowerStates[index_].State)
                    {
                        default:
                        case MaterialStorageState.StorageOperationStates.RequestedToUnload:
                        case MaterialStorageState.StorageOperationStates.PrepareToUnload:
                        case MaterialStorageState.StorageOperationStates.Unload:
                            {
                                Debug.WriteLine($">>> TowerId = {towerid}, CarrierName = {carriername}");
                                // if (unloadReels.ContainsKey(towerid) && unloadReels[towerid].second == carriername)
                                if (unloadReels.ContainsKey(towerid))
                                {
                                    if (unloadReels[towerid].second == carriername)
                                    {
                                        result_ = GetProvideMaterialData(carriername, towerid, ProvideMaterialData.States.Providing, ref data);

                                        if (data != null)
                                        {
                                            data.TowerName = tower_.Name;
                                            data.TowerId = towerid;
                                            FireProvideMaterialStateChanged(data);
                                        }

                                        Debug.WriteLine($">>> GetProvideMaterialData result = {result_}");
                                    }
                                    else
                                        Debug.WriteLine($">>> Contained information = {unloadReels[towerid].second}");
                                }
                                else
                                    Debug.WriteLine($">>> Wrong state!");
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public virtual bool UpdateProvideJobState(string state, ref string jobname, ref string productionstate, ref string jobstate)
        {
            bool result_    = false;
            string jobname_ = string.Empty;

            try
            {
                Dictionary<string, string> fields_;
                bool fault_     = false;
                fields_         = XmlCommandSerializer.XmlToDictionary(state);
                jobname         = $"{fields_["JOBNAME"]}";
                productionstate = $"{fields_["ProductionState"]}";
                jobstate        = $"{fields_["JobState"]}";
                jobname_        = jobname;
                Pair<string, ProvideJobListData> provideJob_ = null;

                if (state.Contains("JOBSTATE") || state.Contains("FAIL"))
                    fault_ = true;

                lock (provideJobs)
                {
                    if ((provideJob_ = provideJobs.Find(x_ => x_.first == jobname_)) != null)
                    {
                        if (productionstate.ToLower() == "issued" && jobstate.ToLower() == "ok")
                        {
                            provideJob_.second.State = (fault_ ? ProvideJobListData.States.Failed : ProvideJobListData.States.Completed);
                            FireProvideJobStateChanged(provideJob_.second);
                            result_ = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public virtual string GetAlarmMessage(int errorcode)
        {
            return alarmManager.GetAlarmMessage(errorcode);
        }

        public virtual bool LoginAccountManager(string user, string password)
        {
            DataSet result_ = null;

            try
            { 
                if (user == "Administrator")
                    result_ = AccountDB.DecryptSelect($"SELECT GroupId, Name, CONVERT(NVARCHAR(4000), DecryptByPassPhrase(", $", Password)), Fullname, Remark FROM ACCOUNT WHERE Name='{user}'");
                else
                    result_ = AccountDB.Select($"SELECT GroupId, Name, CONVERT(NVARCHAR(4000), DecryptByPassPhrase(N'{CONST_REELTOWERGROUP_CRYPTO_SEED}@{site}@tf0778', Password)), Fullname, Remark FROM ACCOUNT WHERE Name='{user}'");

                if (result_ != null && result_.Tables.Count > 0 && result_.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(result_.Tables[0].Rows[0].ItemArray[2].ToString()) && result_.Tables[0].Rows[0].ItemArray[2].ToString() == password)
                    {
                        accountName = result_.Tables[0].Rows[0].ItemArray[1].ToString();
                        accountGid  = (UserGroup)Enum.Parse(typeof(UserGroup), result_.Tables[0].Rows[0].ItemArray[0].ToString());
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return false;
        }

        public virtual void LogoutAccountManager()
        {
            accountName = string.Empty;
            accountGid  = UserGroup.Operator;
        }

        public virtual bool LoginUser(string user)
        {
            try
            {
                currentOperator = string.Empty;
                string username = User_check(user);
                if (username != "NO_INFO")
                {
                    currentOperator = username;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return false;
        }

        //public virtual bool LoginUser(string user)
        //{
        //    try
        //    {
        //        DataSet result_ = AccountDB.Select($"SELECT GroupId, Name, CONVERT(NVARCHAR(4000), DecryptByPassPhrase(N'{CONST_REELTOWERGROUP_CRYPTO_SEED}@{site}@tf0778', Password)), Fullname, Remark FROM ACCOUNT WHERE Name='{user}'");
        //        currentOperator = string.Empty;

        //        if (result_ != null && result_.Tables.Count > 0 && result_.Tables[0].Rows.Count > 0)
        //        {
        //            currentOperator = result_.Tables[0].Rows[0].ItemArray[1].ToString();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
        //    }

        //    return false;
        //}

        public virtual void LogoutUser()
        {
            currentOperator = string.Empty;
        }

        public virtual bool AddNewAccount(UserGroup gid, string user, string password, string fullname, string remark)
        {
            if (accountGid < UserGroup.Manager)
                return false;

            string query_ = $"INSERT INTO ACCOUNT (GroupId, Name, Password, Fullname, Remark) VALUES ({Convert.ToInt32(gid)}, '{user}', EncryptByPassPhrase(N'{CONST_REELTOWERGROUP_CRYPTO_SEED}@{site}@tf0778', N'{password}'), '{fullname}', '{remark}')";
            return AccountDB.Insert(query_);
        }

        public virtual bool DeleteAccount(string user)
        {
            if (accountGid < UserGroup.Manager)
                return false;

            string query_ = $"DELETE FROM ACCOUNT WHERE Name='{user}'";
            return AccountDB.Delete(query_);
        }

        public virtual bool UpdateAccount(UserGroup gid, string user, string password, string fullname, string remark)
        {
            if (accountGid < UserGroup.Manager)
                return false;

            string query_ = $"UPDATE ACCOUNT SET GroupId={gid}, Name='{user}', Password=EncryptByPassPhrase(N'{CONST_REELTOWERGROUP_CRYPTO_SEED}@{site}@tf0778', N'{password}'), Fullname='{fullname}', Remark='{remark}' WHERE Name='{user}'";
            return AccountDB.Update(query_);
        }

        public virtual DataSet QueryAccount(string user)
        {
            if (accountGid < UserGroup.Manager)
                return null;

            string query_ = $"SELECT GroupId, Name, CONVERT(NVARCHAR(4000), DecryptByPassPhrase(N'{CONST_REELTOWERGROUP_CRYPTO_SEED}@{site}@tf0778', Password)), Fullname, Remark FROM ACCOUNT WHERE Name='{user}'";
            return AccountDB.Select(query_);
        }

        public virtual ProvideJobListData.States GetProvideJobState(string jobname)
        {
            Pair<string, ProvideJobListData> provideJob_ = null;
            
            if ((provideJob_ = provideJobs.Find(x_ => x_.first == jobname)) != null)
                return provideJob_.second.State;

            return ProvideJobListData.States.Unknown;
        }

        // Not use
        public virtual DataSet RefreshJob()
        {
            int provided_                   = 0;
            DataSet result_                 = null;
            List<JobListInformation> jobs_  = null;

            try
            { 
                if (GetJobLists(ref jobs_))
                {
                    foreach (JobListInformation job_ in jobs_)
                    {
                        List<FiveField<string, string, string, int, int>> items_ = new List<FiveField<string, string, string, int, int>>();
                        Pair<string, ProvideJobListData> provideJob_ = null;

                        if (job_.State <= 3)
                        {
                            foreach (JobListItem item_ in job_.List)
                            {
                                items_.Add(new FiveField<string, string, string, int, int>(item_.Article, item_.Carrier, item_.Comment, item_.Reels, item_.State));

                                if (item_.State == 3)
                                    provided_++;
                            }
                        }

                        if ((provideJob_ = provideJobs.Find(x_ => x_.first == job_.Name)) != null)
                        {
                            if (provided_ == items_.Count)
                                provideJob_.second.State = ProvideJobListData.States.Completed;
                            else
                            {
                                if (provided_ > 0)
                                    provideJob_.second.State = ProvideJobListData.States.Providing;
                            }

                            if (result_ == null)
                            {
                                result_ = new DataSet();
                                result_.Tables.Add(new DataTable());
                                result_.Tables[0].Columns.Add("Jobname", typeof(string));
                                result_.Tables[0].Columns.Add("User", typeof(string));
                                result_.Tables[0].Columns.Add("Outport", typeof(string));
                                result_.Tables[0].Columns.Add("Reels", typeof(int));
                                result_.Tables[0].Columns.Add("State", typeof(string));
                            }

                            result_.Tables[0].Rows.Add(job_.Name, provideJob_.second.User, provideJob_.second.Outport, provideJob_.second.Reels, provideJob_.second.State);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public virtual bool IsPossibleToProvideJob()
        {
            int count_ = 0;

            try
            { 
                if (provideJobs.Count > 0)
                {
                    if (IsOverDelayTime(CONST_DEFAULT_TOWER_STATE_QUERY_INTERVAL, lastTowerStateQueryTick))
                    {
                        foreach (string towerid_ in towerIds.Keys)
                        {
                            TowerDetailInformation info_ = null;

                            if (GetTowerInformation(towerid_, ref info_))
                            {
                                lastTowerStateQueryTick = App.TickCount;

                                switch (states[towerIds[info_.TowerId]].State)
                                {
                                    default:
                                    case MaterialStorageState.StorageOperationStates.Unknown:
                                    case MaterialStorageState.StorageOperationStates.Down:
                                    case MaterialStorageState.StorageOperationStates.Error:
                                    case MaterialStorageState.StorageOperationStates.Wait:
                                    case MaterialStorageState.StorageOperationStates.Full:
                                        {
                                            ReelTowerState obj_ = null;
                                            UpdateTowerStatus(info_, ref obj_);
                                            count_++;
                                        }
                                        break;
                                    case MaterialStorageState.StorageOperationStates.Idle:
                                    case MaterialStorageState.StorageOperationStates.Run:
                                    case MaterialStorageState.StorageOperationStates.RequestedToLoad:
                                    case MaterialStorageState.StorageOperationStates.PrepareToLoad:
                                    case MaterialStorageState.StorageOperationStates.Load:
                                    case MaterialStorageState.StorageOperationStates.RequestedToUnload:
                                    case MaterialStorageState.StorageOperationStates.PrepareToUnload:
                                    case MaterialStorageState.StorageOperationStates.Unload:
                                        {
                                            if (info_.OnlineStatus.ToLower() == "offline" || info_.StatusCode != 26 || info_.StatusText.ToLower() == "component in terminal")
                                                count_++;
                                        }
                                        break;
                                }
                            }
                        }

                        return count_ != towerIds.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return false;
        }

        public virtual bool StartProvideJob(ref Pair<string, ProvideJobListData> job, bool bymfg = false)
        {
            try
            {
                if (!IsProviding && string.IsNullOrEmpty(currentProvidingJobname))
                {
                    job = provideJobs.Find(x_ => x_.second.State == ProvideJobListData.States.Ready);

                    if (job != null)
                    {
                        for (int i_ = 0; i_ < job.second.Materials.Count; i_++)
                        {
                            ProvideMaterialData item_   = job.second.Materials[i_];
                            CarrierInformation carrier_ = null;

                            if (GetCarrierInformation(item_.Name, ref carrier_))
                            {
                                item_.Category  = carrier_.ArticleName;
                                item_.Name      = carrier_.Carrier;
                                item_.Supplier  = carrier_.Manufactur;
                                item_.LotId     = carrier_.Custom1;
                                item_.Quantity  = carrier_.Stock;
                                item_.Depot     = carrier_.Depot;

                                if (bymfg)
                                    item_.ManufacturedDatetime = carrier_.CreateDate.ToString("yyyymmdd");
                                else
                                    item_.ManufacturedDatetime = carrier_.Custom2;

                                // item_.Comment   = carrier_.Custom6;
                                item_.State     = ProvideMaterialData.States.Ready;
                                item_.Text      = $"Output_No_{job.second.Outport}";
                            }
                        }
                        
                        return ProvideJob(job.first);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return false;
        }

        public virtual bool RemoveCurrentProvidedCarriers(ref string jobname)
        {
            Pair<string, ProvideJobListData> provideJob_ = provideJobs.Find(x_ => x_.second.State >= ProvideJobListData.States.Providing);

            if (provideJob_ != null)
                jobname = (currentProvidingJobname = provideJob_.first);

            lock (provideJobs)
            {
                foreach (ProvideMaterialData item_ in provideJob_.second.Materials)
                {
                    if (item_.State >= ProvideMaterialData.States.Providing)
                        RemoveCarrier(item_.Name);
                }
            }

            if (provideJob_.second.Materials.Count <= 0)
            {
                FireProvideJobStateChanged(provideJob_.second);
                return true;
            }

            return false;
        }

        public virtual void RemoveProvidedCarriers(string jobname)
        {
            Pair<string, ProvideJobListData> provideJob_ = null;

            lock (provideJobs)
            {
                if ((provideJob_ = provideJobs.Find(x_ => x_.first == jobname)) != null)
                {
                    foreach (ProvideMaterialData item_ in provideJob_.second.Materials)
                        RemoveCarrier(item_.Name);
                }
            }
        }

        public virtual bool ProvideJob(string jobname)
        {
            bool result_ = false;
            Pair<string, ProvideJobListData> provideJob_ = null;

            try
            {
                if ((provideJob_ = provideJobs.Find(x_ => x_.first == jobname)) != null)
                {
                    if ((DateTime.Now - provideJob_.second.RegisteredDateTime).TotalMinutes >= 60.0)
                    {
                        JobListInformation job_ = new JobListInformation();
                        job_.Name = jobname;
                        job_.CreateUser = provideJob_.second.User;
                        job_.CreateDate = DateTime.Now;
                        job_.BaseMount = false;
                        job_.AutoDelete = true;
                        job_.LosSize = 1;
                        job_.State = 0;
                        job_.ProdSite = $"{site}-G{id}";
                        job_.ProdDate = DateTime.Now.AddHours(1.0);
                        job_.ProdState = 2;
                        job_.List = new List<JobListItem>();

                        foreach (ProvideMaterialData item_ in provideJob_.second.Materials)
                        {
                            if (!carriers.ContainsKey(item_.Name))
                            {   // Synchronize carrier information.
                                CarrierInformation temp_ = null;
                                GetCarrierInformation(item_.Name, ref temp_);
                            }

                            JobListItem reel_ = new JobListItem();
                            reel_.Article = item_.Category;
                            reel_.Carrier = item_.Name;
                            reel_.Reels = 1;
                            job_.List.Add(reel_);
                        }

                        result_ = UpdateJobList(job_);
                    }
                    else
                        result_ = true;

                    if (result_)
                        result_ = StartJobList(jobname);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public virtual void PeriodicUpdateTowerStatus(bool ammusage)
        {
            // if (++pollingTowerStatusIndex > towerIds.Count)
            //     pollingTowerStatusIndex = 1;
            // 
            // if (towerIds.ContainsValue(pollingTowerStatusIndex))
            //     AsyncUpdateTowerState(towerIds.First(x_ => x_.Value == pollingTowerStatusIndex).Key, ammusage);

            foreach (string id_ in towerIds.Keys)
                AsyncUpdateTowerState(id_, ammusage);
        }

        public virtual bool CleanUpMaterials()
        {
            if (unloadReels.Count <= 0)
                return false;

            try
            { 
                foreach (ReelTowerState state_ in states.Values)
                {
                    if (state_.MaterialDestination == MaterialStorageState.MaterialHandlingDestination.UnloadToOutStage)
                    {
                        string towerid_ = state_.Id;

                        if (!unloadReels.ContainsKey(towerid_))
                            return false;
                    }
                }

                // Remove carrier in storage
                if (unloadReels.Count > 0)
                {
                    foreach (KeyValuePair< string, FourField<string, string, int, ProvideMaterialData>> item_ in unloadReels)
                    {
                        RemoveCarrier(item_.Value.second);
                    }
                }

                List<string> carriers_ = new List<string>();

                lock (provideJobs)
                {
                    foreach (Pair<string, ProvideJobListData> job_ in provideJobs)
                    {
                        job_.second.CleanUpMaterials(ref carriers_, unloadReels);
                        job_.second.Reels = job_.second.Materials.Count;

                        if (reservedJobs.ContainsKey(job_.first))
                        {
                            reservedJobs[job_.first].State = job_.second.State;
                            reservedJobs[job_.first].RemoveMaterialData(carriers_);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return true;
        }

        public virtual void FlushProvideJobs()
        {
            // Cancel provide jobs
            foreach (Pair<string, ProvideJobListData> job_ in provideJobs)
            {
                if (CancelJob(job_.first))
                    DeleteJobList(job_.first, false);
            }
        }

        public virtual void RemoveUnloadReelInformation(string towerid, string carriername)
        {
            if (unloadReels.ContainsKey(towerid))
            {
                Debug.WriteLine($"Unload reel information: TowerId={towerid},Carrier={carriername},Actual={unloadReels[towerid].second}");
                unloadReels.Remove(towerid);
            }
        }

        public virtual void RemoveLoadReelInformation(string towerid)
        {
            if (loadReels.ContainsKey(towerid))
            {
                Debug.WriteLine($"Load reel information: TowerId={towerid},Carrier={loadReels[towerid]}");
                loadReels.Remove(towerid);
            }
        }

        public virtual MaterialStorageState.MaterialHandlingDestination GetStorageHandlingDestination()
        {
            if (IsProviding)
            {
                foreach (ReelTowerState state_ in states.Values)
                {
                    switch (state_.MaterialDestination)
                    {
                        case MaterialStorageState.MaterialHandlingDestination.LoadToStorage:
                            return MaterialStorageState.MaterialHandlingDestination.LoadToStorage;
                        case MaterialStorageState.MaterialHandlingDestination.None:
                        case MaterialStorageState.MaterialHandlingDestination.UnloadToReject:
                        case MaterialStorageState.MaterialHandlingDestination.UnloadToOutStage:
                            return MaterialStorageState.MaterialHandlingDestination.UnloadToOutStage;
                    }
                }
            }
            else
            {
                foreach (ReelTowerState state_ in states.Values)
                {
                    switch (state_.MaterialDestination)
                    {
                        case MaterialStorageState.MaterialHandlingDestination.None:
                        case MaterialStorageState.MaterialHandlingDestination.UnloadToReject:
                            break;
                        case MaterialStorageState.MaterialHandlingDestination.LoadToStorage:
                            return MaterialStorageState.MaterialHandlingDestination.LoadToStorage;
                        case MaterialStorageState.MaterialHandlingDestination.UnloadToOutStage:
                            return MaterialStorageState.MaterialHandlingDestination.UnloadToOutStage;
                    }
                }
            }

            return MaterialStorageState.MaterialHandlingDestination.None;
        }

        public virtual bool IsSpecialMaterialData(string name, ref string val)
        {
            bool result_ = false;

            if (specialMaterials.ContainsKey(name))
            {
                val = specialMaterials[name];
                result_ = true;
            }

            return result_;
        }

        public virtual string GetSpecialMaterialData(string name)
        {
            if (specialMaterials.ContainsKey(name))
                return specialMaterials[name];
            return string.Empty;
        }

        public virtual void AddSpecialMaterialData(string name, string type, bool export = false)
        {
            if (!specialMaterials.ContainsKey(name))
            {
                lock (specialMaterials)
                    specialMaterials.Add(name, type);
            }
            else
                specialMaterials[name] = type;

            if (export)
                ExportSpecialData();
        }

        public virtual void RemoveSpecialMaterialData(string name, bool export = false)
        {
            if (specialMaterials.ContainsKey(name))
            {
                lock (specialMaterials)
                    specialMaterials.Remove(name);
            }

            if (export)
                ExportSpecialData();
        }

        public virtual string Connect()
        {
            return ReelTowerAMM.Connect();
        }

        public virtual DataTable GetPickingID()
        {
            ammpickingid = ReelTowerAMM.GetPickingID(linecode, groupname);

            return ammpickingid;
        }

        public virtual string SetEq()
        {
            string result_ = ReelTowerAMM.SetEqStart(linecode, groupname);

            return result_;
        }

        public virtual string SetEqEnd()
        {
            string result_ = string.Empty;
            if (ReelTowerAMM.MSSql != null)
                result_ = ReelTowerAMM.SetEqEnd(linecode, groupname);

            return result_;
        }

        public virtual int SetEqAlive(int nAlive)
        {
            var val = ReelTowerAMM.SetEqAlive(linecode, groupname, nAlive);
            return ReelTowerAMM.SetEqAlive(linecode, groupname, nAlive);
        }

        public virtual string GetReelqty(MaterialData data_)
        {
            int count = 0;
            string result_ = string.Empty;
            //for (count = 0; count < 3; count++)
            //{
                result_ = ReelTowerAMM.GetReelQty(linecode, groupname, data_.Category, data_.Quantity.ToString());

            //    if (result_.ToUpper() == data_.Quantity.ToString())
            //        break;
            //}

            return result_;
        }

        public virtual string SetLoadComplete(MaterialData data_, bool bWebService)
        {
            int count = 0;
            string result_ = string.Empty;
            string reelinfo = string.Empty;
            string time = string.Empty;

            time = data_.ManufacturedDatetime.Substring(0, 4) + "-" + data_.ManufacturedDatetime.Substring(4, 2) + "-" + data_.ManufacturedDatetime.Substring(6, 2);
            reelinfo = data_.TowerId + ";" + data_.Name + ";" + data_.Category + ";" + data_.LotId + ";" + data_.Quantity + ";" + data_.Supplier + ";" + time + ";" + data_.Size + ";" + data_.LoadType.ToString().ToUpper();

            for (count = 0; count < 3; count++)
            {
                try
                {
                    result_ = ReelTowerAMM.SetLoadComplete(linecode, groupname, reelinfo, bWebService);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
            
                if (result_.ToUpper() == "OK")
                    break;
            }

            return result_;
        }

        public virtual string SetUnloadStart(string pickingid)
        {
            return ReelTowerAMM.SetUnloadStart(linecode, groupname, pickingid);
        }

        public virtual string SetUnloadOut(string uid, bool bWebService)
        {
            return ReelTowerAMM.SetUnloadOut(linecode, groupname, uid, bWebService);
        }

        public virtual string SetUnloadOut_Manual(string uid, bool bWebService)
        {
            return ReelTowerAMM.SetUnloadOut_Manual(linecode, groupname, uid, bWebService);
        }

        public virtual string SetUnloadEnd(string pickingid)
        {
            return ReelTowerAMM.SetUnloadEnd(linecode, groupname, pickingid);
        }

        public virtual string SetEqStatus(string state, string substate = "", string firstid = "", string secondid = "")
        {
            string result_ = string.Empty;
            if (!string.IsNullOrEmpty(firstid))
            {
                result_ = ReelTowerAMM.SetEqStatus(linecode, groupname, state, substate, firstid, secondid);
            }
            else
            {
                result_ = ReelTowerAMM.SetEqStatus(linecode, groupname, state, substate);
            }
            return result_;
        }

        public virtual string SetEqEvent()
        {
            return ReelTowerAMM.SetEqEvent(linecode, groupname, "Error_Code", "Error_type", "Error_Name", "Error_Descript", "Error_Action");
        }

        public virtual string User_check(string usernum)
        {
            return ReelTowerAMM.User_check(usernum).Trim();
        }

        // UPDATED: 20200728 (jm.choi)
        public virtual string SetPickingList_Cancel(string pickingid)
        {
            return ReelTowerAMM.SetPickingList_Cancel(linecode, groupname, pickingid);
        }

        public virtual DataTable GetMTLInfo(int index, string data)
        {
            DataTable result_ = null;

            switch(index)
            {
                case 0:
                    result_ = ReelTowerAMM.GetMTLInfo(linecode, groupname, data);
                    break;
                case 1:
                    result_ = ReelTowerAMM.GetMTLInfo_SID(linecode, groupname, data);
                    break;
                case 2:
                    result_ = ReelTowerAMM.GetMTLInfo_UID(linecode, groupname, data);
                    break;
                case 3:
                    result_ = ReelTowerAMM.GetMTLInfo(linecode, groupname);
                    break;
            }
            return result_;
        }

        public virtual string Set_Twr_State(string TowerID, string ReelOnOff, string TowerJobState)
        {
            return ReelTowerAMM.Set_Twr_State(linecode, TowerID, ReelOnOff, TowerJobState);
        }

        public virtual string Get_Twr_State(string TowerID)
        {
            return ReelTowerAMM.Get_Twr_State(linecode, TowerID);
        }

        public virtual string Get_Twr_State_Job(string TowerID)
        {
            return ReelTowerAMM.Get_Twr_State_Job(linecode, TowerID);
        }

        public virtual string Get_Twr_State_Reel(string TowerID)
        {
            return ReelTowerAMM.Get_Twr_State_Reel(linecode, TowerID);
        }
    }
    #endregion
}
#endregion