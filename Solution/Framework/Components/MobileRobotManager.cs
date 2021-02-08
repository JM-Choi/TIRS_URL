#region Imports
using TechFloor.Object;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TechFloor.Device;
#endregion

#region Program
namespace TechFloor.Components
{
    public partial class MobileRobotServer : AbstractClassDisposable
    {
        #region Fields
        protected System.Timers.Timer periodicStateReportTmr                    = null;

        protected CancellationTokenSource cancellationTokenSource               = new CancellationTokenSource();

        protected CancellationToken cancellationToken                           = CancellationToken.None;

        protected Dictionary<string, MobileRobotObject> robots                   = new Dictionary<string, MobileRobotObject>();

        protected Dictionary<string, OmronMobileRobotState> robotStates         = new Dictionary<string, OmronMobileRobotState>();
        #endregion

        #region Propeties
        public IReadOnlyDictionary<string, MobileRobotObject> Robots             => robots;

        public IReadOnlyDictionary<string, OmronMobileRobotState> RobotStates   => robotStates;
        #endregion

        #region Events
        public event EventHandler ChangedMobileRobotCommunicationState;

        public event EventHandler<OmronMobileRobotState> ChangedRobotStatus;

        public event EventHandler<OmronMobileRobotState> AddRobotStatus;
        #endregion

        #region Constructors
        protected MobileRobotServer(int interval = 1000)
        {
            CreateWatcher(interval);
        }
        #endregion

        #region Protrected methods
        protected virtual void CreateWatcher(int interval = 1000)
        {
            if (periodicStateReportTmr == null)
            {
                periodicStateReportTmr = new System.Timers.Timer(interval);
                periodicStateReportTmr.Elapsed += OnElapsedWatcher;
                periodicStateReportTmr.Start();
            }
        }

        protected virtual void DestroyWatcher()
        {
            if (periodicStateReportTmr == null)
            {
                periodicStateReportTmr.Stop();
                periodicStateReportTmr.Dispose();
                periodicStateReportTmr = null;
            }
        }

        protected virtual void OnElapsedWatcher(object sender, ElapsedEventArgs e)
        {
            periodicStateReportTmr.Stop();
            periodicStateReportTmr.Start();
        }

        // protected virtual async void ReportStateToRemoteHost()
        // {
        //     try
        //     {
        //         if (robotStates != null && robotStates.Count > 0)
        //         {
        //             foreach (var item in robotStates)
        //             {
        //                 if ( item.Value.Id)
        //                 {
        // 
        //                 }
        //                 await restClient.UpdateRobotStatus(item.Value, cancellationToken);
        //             }
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
        //         Logger.Debug($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}", "Alram");
        //     }
        // }

        protected virtual void OnChangedMobileRobotCommunicationState(object sender, EventArgs e)
        {
            ChangedMobileRobotCommunicationState?.Invoke(sender, e);
        }

        protected virtual void OnPeriodicStateReport(object sender, OmronMobileRobotState e)
        {
            if (sender == null || e == null)
                return;

            lock (robotStates)
            {
                if (robotStates.ContainsKey(e.Id))
                {
                    robotStates[e.Id] = e;
                    ChangedRobotStatus?.Invoke(this, e);
                }
                else
                {
                    robotStates.Add(e.Id, e);
                    AddRobotStatus?.Invoke(this, e);
                }
            }
        }

        protected virtual void OnRobotEventReport(object sender, OmronMobileRobotState e)
        {
            if (sender == null || e == null)
                return;

            lock (robotStates)
            {
                if (robotStates.ContainsKey(e.Id))
                {
                    robotStates[e.Id] = e;
                    ChangedRobotStatus?.Invoke(this, e);
                }
                else
                {
                    robotStates.Add(e.Id, e);
                    AddRobotStatus?.Invoke(this, e);
                }
            }
        }
        #endregion

        #region Public methods
        public virtual int Add(string id, string name, string addr, int port)
        {
            if (!robots.ContainsKey(id))
            {
                robots.Add(id, new MobileRobotObject(id, name, addr.ToString(), port));
                robots[id].ChangedCommunicationState    += OnChangedMobileRobotCommunicationState;
                robots[id].PeriodicStateReport          += OnPeriodicStateReport;
                robots[id].RobotEventReport             += OnRobotEventReport;
            }

            return robots.Count;
        }

        public virtual void Delete(string id)
        {
            if (robots.ContainsKey(id))
            {
                robots[id].Disconnect(true);
                robots[id].ChangedCommunicationState    -= OnChangedMobileRobotCommunicationState;
                robots[id].PeriodicStateReport          -= OnPeriodicStateReport;
                robots[id].RobotEventReport             -= OnRobotEventReport;
                robots.Remove(id);
            }
        }

        public virtual void Update(string id, string name, string address, int port, int building, int floor)
        {
            if (robots.ContainsKey(id))
                robots[id].UpdateNetwork(address, name, port, building, floor);
        }

        public virtual void Clear()
        {
            foreach (var robot in robots)
                robot.Value.Disconnect(true);

            robots.Clear();
        }

        public virtual void Terminate()
        {
            Clear();
            DestroyWatcher();
        }

        public bool LocationUpdate(string id, int floor, int building)
        {
            if (robots.ContainsKey(id))
            {
                if (building > 4 && floor > 15)
                {
                    return false;
                }
                else
                {
                    robots[id].RobotCurrentState.BuildingID = building;
                    robots[id].RobotCurrentState.Floor = floor;
                    return true;
                }
            }
            return false;
        }
        #endregion
    }

    [DataContract]
    public class OmronMobileRobotState
    {
        [DataMember(Name = "robot_id")]
        public readonly string Id;

        public readonly MobileRobotCoord Coord;

        [DataMember(Name = "floor")]
        public int Floor                        = 1;

        [DataMember(Name = "building_id")]
        public int BuildingID                   = 0;

        public int LocationX                    = 0;

        public int LocationY                    = 0;

        [DataMember(Name = "x")]
        public int OffsetX                      = 0;

        [DataMember(Name = "y")]
        public int OffsetY                      = 0;

        [DataMember(Name = "heading")]
        public double Orientation               = 0.0;

        public double Temperature               = 0.0;

        [DataMember(Name = "robot_poll_status")]
        public MobileRobotMode State            = MobileRobotMode.Idle;

        public double Charge                    = 0.0;

        [DataMember(Name = "robot_poll_status_date")]
        public string statusDateTime            = string.Empty;

        protected int statusTick                = 0;

        protected DateTime statusTime           = DateTime.MinValue;

        [DataMember(Name = "battery")]
        public double Battery                   = 0.0;

        [DataMember(Name = "item_load_yn")]
        public ItemLoadyn InnerBoxItemLoaded    = ItemLoadyn.off;

        [DataMember(Name = "lock_yn")]
        public Lockyn InnerBoxLocked            = Lockyn.off;

        [DataMember(Name = "lock_open_yn")]
        public LockerOpenyn InnerBoxOpened      = LockerOpenyn.off;

        public string realtime                  = DateTime.Now.ToString();

        public EventType RobotEvent             = EventType.None;

        public DateTime RbEvTime                = DateTime.Now;

        public OmronMobileRobotState(string id)
        {
            Id = id;
            statusDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
    }

    public class EventArgsCommandResponse : EventArgs
    {
        public string vehicleName;
        public MobileRobotCommands cmdType;
        public SubCommandTypes cmdSubType;
        public ResponseCodes response;
        public string command;
        public string message;
    }

    public class MobileRobotObject
    {
        #region Constants
        protected const int CONST_RECONNECT_INTERVAL = 1000;

        protected readonly char[] CONST_TOKEN_DELIMITERS = { '\r', '\n' }; // " "

        protected readonly char[] CONST_COMMAND_TOKEN_DELIMITERS = { ' ', '\r', '\n' };
        #endregion

        #region Fields
        protected bool autoreconnect = false;

        protected bool stopCommunication = false;

        protected bool readyToCommunicate = false;

        protected int stateReportCount = 0;

        protected int stateReportPeriod = 0;

        protected int port = 0;

        protected int ErrorCounts = 0;

        protected int ErrorMaxCnt = 2;

        protected int interruptedMessageTick = 0;

        protected double localizedX = 0.0;

        protected double localizedY = 0.0;

        protected double localizedOrientation = 0.0;

        protected double localizationScore = 0.0;

        protected double temperature = 0.0;

        protected double soc = 0.0;

        protected string address = string.Empty;

        protected string password = string.Empty;

        protected string message = string.Empty;

        protected string id = string.Empty;

        protected double intervalOfWatcher = 500;

        protected string status = string.Empty;

        protected string m_recvStr = string.Empty;

        protected string Remark = string.Empty;

        protected string name = string.Empty;

        protected ControlTypes CurrentControlType = ControlTypes.Route;
        
        protected ResponseCodes returnCode = ResponseCodes.NotAccessible;

        protected TelnetClient sock;

        protected StringBuilder keyinbuffer = new StringBuilder();

        protected System.Timers.Timer watcherTmr = null;

        protected System.Timers.Timer m_tmrRefresh = null;

        public OmronMobileRobotState RobotCurrentState { get; private set; }

        protected MobileRobotStates currentState = MobileRobotStates.None;

        protected MobileRobotCoord currentCoord = new MobileRobotCoord();

        protected CancellationToken cancellationToken = CancellationToken.None;

        protected Dictionary<string, Pair<DateTime, MobileRobotControlItemStates>> sentControlCommands = new Dictionary<string, Pair<DateTime, MobileRobotControlItemStates>>();

        protected Dictionary<string, Pair<ErrorMessageTypes, string>> errorMessages = new Dictionary<string, Pair<ErrorMessageTypes, string>>()
        {
            { "Error:",                     new Pair<ErrorMessageTypes, string>(ErrorMessageTypes.Error,        "<command> <argument>") },
            { "SetUpError:",                new Pair<ErrorMessageTypes, string>(ErrorMessageTypes.SetupError,   "<description>") },
            { "CommandError:",              new Pair<ErrorMessageTypes, string>(ErrorMessageTypes.CommandError, "<command> <argument>") },
            { "CommandErrorDescription:",   new Pair<ErrorMessageTypes, string>(ErrorMessageTypes.CommandError, "<description>") }
        };

        protected List<string> sentInstantCommands = new List<string>();

        protected Dictionary<string, Pair<int, List<string>>> commandTable = new Dictionary<string, Pair<int, List<string>>>()
        {
            { "analogInputList",            new Pair<int, List<string>>(0, new List<string>()       { "AnalogInputList:" }) },
            { "analogInputQueryRaw",        new Pair<int, List<string>>(1, new List<string>()       { "AnalogInputRaw:" }) },
            { "analogInputQueryVoltage",    new Pair<int, List<string>>(2, new List<string>()       { "AnalogInputVoltage:" }) },
            { "applicationFaultClear",      new Pair<int, List<string>>(3, new List<string>()       { "FaultCleared::", "ApplicationFaultClear" }) },
            { "applicationFaultQuery",      new Pair<int, List<string>>(4, new List<string>()       { "ApplicationFaultQuery:", "ApplicationFaultQuery" }) },
            { "applicationFaultSet",        new Pair<int, List<string>>(5, new List<string>()       { "applicationFaultSet", "Fault" }) },
            { "arclSendText",               new Pair<int, List<string>>(6, new List<string>()       { }) }, // sent message
            { "configAdd",                  new Pair<int, List<string>>(7, new List<string>()       { "Added" }) },
            { "configParse",                new Pair<int, List<string>>(8, new List<string>()       { "Will", "Config", "parsed" }) },
            { "configStart",                new Pair<int, List<string>>(9, new List<string>()       { "New", "config", "starting" }) },
            { "connectOutgoing",            new Pair<int, List<string>>(10, new List<string>()      { "connectOutgoing" }) },
            { "createInfo ",                new Pair<int, List<string>>(11, new List<string>()      { "Created", "info" }) },
            { "dock",                       new Pair<int, List<string>>(12, new List<string>()      { "DockingState:", "ForcedState:", "ChargeState:" }) },
            { "doTask setheading",          new Pair<int, List<string>>(13, new List<string>()      { "Will", "Doing", "Completed" }) },
            { "doTaskInstant",              new Pair<int, List<string>>(14, new List<string>()      { "completed", "doing", "instant", "task" }) },
            { "echo",                       new Pair<int, List<string>>(15, new List<string>()      { "Echo" }) },
            { "enableMotors",               new Pair<int, List<string>>(16, new List<string>()      { "Motors", "Estop", "motors" }) },
            { "executeMacro",               new Pair<int, List<string>>(17, new List<string>()      { "Executing", "WaitState:", "Completed" }) },
            { "extIOAdd",                   new Pair<int, List<string>>(18, new List<string>()      { "extIOAdd:" }) },
            { "extIODump",                  new Pair<int, List<string>>(19, new List<string>()      { "ExtIODump:", "EndExtIODump" }) },
            { "extIODumpLocal",             new Pair<int, List<string>>(20, new List<string>()      { "ExtIODumpLocal:", "EndExtIODumpLocal" }) },
            { "extIOInputUpdate",           new Pair<int, List<string>>(21, new List<string>()      { "extIOInputUpdate:" }) },
            { "extIOInputUpdateBit",        new Pair<int, List<string>>(22, new List<string>()      { "extIOInputUpdateBit:" }) },
            { "extIOInputUpdateByte",       new Pair<int, List<string>>(23, new List<string>()      { "extIOInputUpdateByte:" }) },
            { "extIOOutputUpdate",          new Pair<int, List<string>>(24, new List<string>()      { "extIOOutputUpdate:" }) },
            { "extIOOutputUpdateBit",       new Pair<int, List<string>>(25, new List<string>()      { "extIOOutputUpdateBit:" }) },
            { "extIOOutputUpdateByte",      new Pair<int, List<string>>(26, new List<string>()      { "extIOOutputUpdateByte:" }) },
            { "extIORemove",                new Pair<int, List<string>>(27, new List<string>()      { "extIORemove:" }) },
            { "faultsGet",                  new Pair<int, List<string>>(28, new List<string>()      { "FaultList:", "FaultList" }) },
            { "getConfigSectionInfo",       new Pair<int, List<string>>(29, new List<string>()      { "GetConfigSectionInfo:", "EndOfGetConfigSectionInfo", "GetConfigSectionInfo" }) },
            { "getConfigSectionList",       new Pair<int, List<string>>(30, new List<string>()      { "GetConfigSectionList:", "EndOfGetConfigSectionList", "GetConfigSectionList" }) },
            { "getConfigSectionValues",     new Pair<int, List<string>>(31, new List<string>()      { "GetConfigSectionValues:", "EndOfGetConfigSectionValues", "GetConfigSectionValues" }) },
            { "getDataStoreFieldInfo",      new Pair<int, List<string>>(32, new List<string>()      { "GetDataStoreFieldInfo:", "EndOfGetDataStoreFieldInfo", "GetDataStoreFieldInfo" }) },
            { "getDataStoreFieldList",      new Pair<int, List<string>>(33, new List<string>()      { "GetDataStoreFieldList:", "EndOfGetDataStoreFieldList", "GetDataStoreFieldList" }) },
            { "getDataStoreFieldValues",    new Pair<int, List<string>>(34, new List<string>()      { "GetDataStoreFieldValues:", "EndOfGetDataStoreFieldValues", "GetDataStoreFieldValues" }) },
            { "getDataStoreGroupInfo",      new Pair<int, List<string>>(35, new List<string>()      { "GetDataStoreGroupInfo:", "EndOfGetDataStoreGroupInfo", "GetDataStoreGroupInfo" }) },
            { "getDataStoreGroupList",      new Pair<int, List<string>>(36, new List<string>()      { "GetDataStoreGroupList:", "EndOfGetDataStoreGroupList", "GetDataStoreGroupList" }) },
            { "getDataStoreGroupValues",    new Pair<int, List<string>>(37, new List<string>()      { "GetDataStoreGroupValues:", "EndOfGetDataStoreGroupValues", "GetDataStoreGroupValues" }) },
            { "getDataStoreTripGroupList",  new Pair<int, List<string>>(38, new List<string>()      { "GetDataStoreTripGroupList:", "EndOfGetDataStoreTripGroupList", "GetDataStoreTripGroupList" }) },
            { "getDateTime",                new Pair<int, List<string>>(39, new List<string>()      { "DateTime:" }) },
            { "getGoals",                   new Pair<int, List<string>>(40, new List<string>()      { "Goals:", "goals" }) },
            { "getInfoList",                new Pair<int, List<string>>(41, new List<string>()      { "Info:" }) },
            { "getInfo",                    new Pair<int, List<string>>(42, new List<string>()      { "InfoList:", "info", "list" }) },
            { "getMacros",                  new Pair<int, List<string>>(43, new List<string>()      { "macros" }) },
            { "getRoutes",                  new Pair<int, List<string>>(44, new List<string>()      { "Routes", "Route:", "routes" }) },
            { "goto",                       new Pair<int, List<string>>(45, new List<string>()      { "Going", "Arrived", "Error:" }) },
            { "inputList",                  new Pair<int, List<string>>(46, new List<string>()      { "Input:", "InputList" }) },
            { "inputQuery",                 new Pair<int, List<string>>(47, new List<string>()      { "Input:" }) },
            { "log",                        new Pair<int, List<string>>(48, new List<string>()      { "Logging" }) },
            { "mapObjectInfo",              new Pair<int, List<string>>(49, new List<string>()      { "MapObjectInfo:", "MapObjectInfoParams:", "MapObjectInfo" }) },
            { "mapObjectList",              new Pair<int, List<string>>(50, new List<string>()      { "MapObjectList:", "MapObjectList" }) },
            { "mapObjectTypeInfo",          new Pair<int, List<string>>(51, new List<string>()      { "MapObjectTypeList:", "MapObjectTypeInfoArgument:", "MapObjectTypeInfo" }) },
            { "mapObjectTypeList",          new Pair<int, List<string>>(52, new List<string>()      { "MapObjectTypeList:", "MapObjectTypeList" }) },
            { "newConfigParam",             new Pair<int, List<string>>(53, new List<string>()      { "Will", "param", "section" }) },
            { "newConfigSectionComment",    new Pair<int, List<string>>(54, new List<string>()      { "Will", "comment", "section" }) },
            { "odometer",                   new Pair<int, List<string>>(55, new List<string>()      { "Odometer:", "comment", "section" }) },
            { "odometerReset",              new Pair<int, List<string>>(56, new List<string>()      { "Reset", "Odometer" }) },
            { "oneLineStatus",              new Pair<int, List<string>>(57, new List<string>()      { "Status:", "BatteryVoltage:", "Location:", "LocalizationScore:", "Temperature:" }) },
            { "outputList",                 new Pair<int, List<string>>(58, new List<string>()      { "Output:", "OutputList" }) },
            { "outputOff",                  new Pair<int, List<string>>(59, new List<string>()      { "Output:" }) },
            { "outputOn",                   new Pair<int, List<string>>(60, new List<string>()      { "Output:" }) },
            { "outputQuery",                new Pair<int, List<string>>(61, new List<string>()      { "Output:" }) },
            { "patrol",                     new Pair<int, List<string>>(62, new List<string>()      { "Patrolling", "route" }) },
            { "patrolOnce",                 new Pair<int, List<string>>(63, new List<string>()      { "Patrolling", "route", "Finished", "patrolling" }) },
            { "patrolResume",               new Pair<int, List<string>>(64, new List<string>()      { "Patrolling", "route", "Finished", "patrolling" }) },
            { "payloadQuery",               new Pair<int, List<string>>(65, new List<string>()      { "PayloadQuery:" }) },
            { "payloadQueryLocal",          new Pair<int, List<string>>(66, new List<string>()      { "PayloadQuery:", "EndPayloadQuery" }) },
            { "payloadRemove",              new Pair<int, List<string>>(67, new List<string>()      { "payloadremove", "PayloadUpdate:" }) },
            { "payloadSet",                 new Pair<int, List<string>>(68, new List<string>()      { "payloadset", "PayloadUpdate:" }) },
            { "payloadSlotCount",           new Pair<int, List<string>>(69, new List<string>()      { "PayloadSlotCount:", "EndPayloadSlotCount" }) },
            { "payloadSlotCountLocal",      new Pair<int, List<string>>(70, new List<string>()      { "PayloadSlotCount:", "EndPayloadSlotCount" }) },
            { "play",                       new Pair<int, List<string>>(71, new List<string>()      { "Playing" }) },
            { "pupupSimple",                new Pair<int, List<string>>(72, new List<string>()      { "Creating", "simple", "popup" }) },
            { "queryDockStatus",            new Pair<int, List<string>>(73, new List<string>()      { "DockingState:", "ForcedState:", "ChargeState:" }) },
            { "queryFaults",                new Pair<int, List<string>>(74, new List<string>()      { "RobotFaultQuery:", "EndQueryFaults" }) },
            { "queryMotors",                new Pair<int, List<string>>(75, new List<string>()      { "Motors", "Estop", "motors" }) },
            { "queueCancel",                new Pair<int, List<string>>(76, new List<string>()      { "QueueUpdate:" }) },
            { "queueCancelLocal",           new Pair<int, List<string>>(77, new List<string>()      { "QueueUpdate:" }) },
            { "queueDropoff",               new Pair<int, List<string>>(78, new List<string>()      { "QueueUpdate:" }) },
            { "queueModify",                new Pair<int, List<string>>(79, new List<string>()      { "QueueUpdate:" }) },
            { "queueModifyLocal",           new Pair<int, List<string>>(80, new List<string>()      { "QueueUpdate:" }) },
            { "queueMulti",                 new Pair<int, List<string>>(81, new List<string>()      { "QueueMulti:" }) },
            { "queuePickup",                new Pair<int, List<string>>(82, new List<string>()      { "QueueUpdate:" }) },
            { "queuePickupDropoff",         new Pair<int, List<string>>(83, new List<string>()      { "QueueUpdate:" }) },
            { "queueQuery",                 new Pair<int, List<string>>(84, new List<string>()      { "QueueQuery:" }) },
            { "queueQueryLocal",            new Pair<int, List<string>>(85, new List<string>()      { "QueueQuery:" }) },
            { "queueShow",                  new Pair<int, List<string>>(86, new List<string>()      { "QueueRobot:", "QueueShow:" }) },
            { "queueShowCompleted",         new Pair<int, List<string>>(87, new List<string>()      { "QueueShow:", "EndQueueShowCompleted" }) },
            { "queueShowRobot",             new Pair<int, List<string>>(88, new List<string>()      { "QueueRobot:", "EndQueueShowRobot" }) },
            { "queueShowRobotLocal",        new Pair<int, List<string>>(89, new List<string>()      { "QueueRobot:", "EndQueueShowRobot" }) },
            { "quit",                       new Pair<int, List<string>>(90, new List<string>()      { "Closing", "connection" }) },
            { "say",                        new Pair<int, List<string>>(91, new List<string>()      { "Saying" }) },
            { "shutDownServer",             new Pair<int, List<string>>(92, new List<string>()      {  }) },
            { "status",                     new Pair<int, List<string>>(93, new List<string>()      { "Status:", "StateOfCharge:", "BatteryVoltage:", "Location:", "LocalizationScore:", "Temperature:" }) },
            { "stop",                       new Pair<int, List<string>>(94, new List<string>()      { "Stopping", "Stopped" }) },
            { "tripReset",                  new Pair<int, List<string>>(95, new List<string>()      { "tripReset:", "EndOfTripReset" }) },
            { "undock",                     new Pair<int, List<string>>(96, new List<string>()      { "DockingState:", "ForcedState:", "ChargeState:" }) },
            { "updateInfo",                 new Pair<int, List<string>>(97, new List<string>()      { "UpdateInfo", "info" }) },
            { "waitTaskCancel",             new Pair<int, List<string>>(98, new List<string>()      { "WaitState:" }) },
            { "waitTaskState",              new Pair<int, List<string>>(99, new List<string>()      { "WaitState:" }) },
            { "localizeatgoal",             new Pair<int, List<string>>(100, new List<string>()     { }) },
            { "localizetopoint",            new Pair<int, List<string>>(101, new List<string>()     { }) },
            { "dotask gotostraight",        new Pair<int, List<string>>(102 , new List<string>()    { "Doing", "Completed", "Will" }) },
        };

        protected Dictionary<string, NotificationTypes> stateMessages = new Dictionary<string, NotificationTypes>()
        {
            { "Map changed",                    NotificationTypes.ChangedMap},
            { "Configuration changed",          NotificationTypes.ChangedConfiguration },
            { "TextRequestChargeVoltage",       NotificationTypes.ChargeBattaryRequest },
            { "Estop pressed",                  NotificationTypes.PressedEmergency },
            { "Estop relieved",                 NotificationTypes.ReleasedEmergentcy },
            { "Motors disabled",                NotificationTypes.DisabledMotors },
            { "Error:",                         NotificationTypes.AlertNotification },
            { "Interrupted:",                   NotificationTypes.InterruptNotification },
            { "DockingState",                   NotificationTypes.DockingNotification }
        };

        protected Dictionary<string, FaultMessageTypes> faultMessages = new Dictionary<string, FaultMessageTypes>()
        {
            { "Fault_Application",              FaultMessageTypes.RobotApplicationFault},
            { "Driving_Application_Fault",      FaultMessageTypes.DrivingApplicationFault },
            { "Critical OverTemperatureAnalog", FaultMessageTypes.OverTemperatureFault },
            { "Critical UnderVoltage",          FaultMessageTypes.LowBattaryFault },
            { "EncoderDegraded",                FaultMessageTypes.EncoderFault },
            { "Critical GyroFault",             FaultMessageTypes.GryroFault },
        };

        protected bool stopServiceBroker = false;

        protected AutoResetEvent shutdown = new AutoResetEvent(false);

        protected ManualResetEvent stop = new ManualResetEvent(false);

        protected Thread serviceBroker = null;

        protected ServiceBrokerSubSteps subStep = ServiceBrokerSubSteps.Prepare;

        protected bool IsFirstTimeConnect = true;

        public MobileRobotCommands GetCommandType(string command) => (commandTable.ContainsKey(command) ? (MobileRobotCommands)commandTable[command].first : MobileRobotCommands.CommandType_none);

        public MobileRobotServiceParameter ServiceParameter = new MobileRobotServiceParameter();

        protected List<MobileRobotControlStateResponseContents> controlStateQueue = new List<MobileRobotControlStateResponseContents>();
        #endregion

        #region Events
        public event EventHandler<OmronMobileRobotState> PeriodicStateReport;

        public event EventHandler<OmronMobileRobotState> RobotEventReport;

        public event EventHandler<EventArgsCommandResponse> ReceivedHostCommand;

        public event EventHandler ChangedCommunicationState;

        public event EventHandler ChangeControlItemState;
        #endregion

        #region Properties

        public bool IsExecuteCommand => (sentControlCommands.Count > 0 || serviceBroker != null);

        protected bool IsOverDelayOfIntteruptMessage(int delay) => (TimeSpan.FromMilliseconds(Environment.TickCount - interruptedMessageTick).TotalMilliseconds >= delay);

        public bool IsTerminated => (sock == null && stopCommunication);

        public bool IsConnected => (sock != null) ? sock.IsConnected : false;

        public bool IsReadyToCommunication => readyToCommunicate;

        public string Id => id;

        public string Name => name;

        public string Address => address;

        public string Message => message;

        public int Port => port;

        public ResponseCodes ReturnCode => returnCode;

        public MobileRobotStates State => currentState;
        
        public MobileRobotCoord Coord => currentCoord;

        public ServiceBrokerSubSteps ServiceBrokerSubStep => subStep;

        public bool IsfirstTimeConnect => IsFirstTimeConnect;
        #endregion

        #region Constructors
        public MobileRobotObject(string id, string name, string address, int port, bool connect = false, string password = "adept")
        {
            RobotCurrentState = new OmronMobileRobotState(id);
            Create(id, name, address, port, password, connect);
        }

        protected Dictionary<string, Pair<DateTime, MobileRobotControlItemStates>> ErrorsentControlCommands = new Dictionary<string, Pair<DateTime, MobileRobotControlItemStates>>();

        public void StartCommand(MobileRobotControlItemObject control)
        {
            StartControl(new HostControlCommand(MobileRobotCommandTypes.QueuedCommand, control.ControlNumber.ToString()));
        }
        #endregion

        #region Protected methods
        protected virtual bool ImmediateHalt()
        {
            stopServiceBroker = true;

            if (serviceBroker != null)
            {
                serviceBroker.Join();
                serviceBroker = null;
            }

            // if (CurrentControlPlan != null)
            // {
            //     var errCmdList = CurrentControlPlan.Where(x => x.State == MobileRobotControlItemStates.Error).ToList();
            //     bool isNeedUpdate = false;
            //     foreach (var item in errCmdList)
            //     {
            //         item.State = MobileRobotControlItemStates.Waiting;
            //         isNeedUpdate = true;
            //     }
            //     if (isNeedUpdate)
            //     {
            //         ChangeControlItemState?.Invoke(this, null);
            //     }
            // }

            return serviceBroker == null;
        }

        protected virtual void EnqueueControlStateResponse(MobileRobotServiceParameter service, MobileRobotBoundObject bound, MobileRobotControlItemObject control)
        {
            // lock (controlStateQueue)
            //     controlStateQueue.Add(new MobileRobotControlStateResponseContents(
            //         service.VoyageVersion,
            //         service.VoyageId,
            //         service.RobotId,
            //         bound.BoundNumber,
            //         control));
        }

        protected virtual void PeekControlStateResponse(ref MobileRobotControlStateResponseContents state)
        {
            state = new MobileRobotControlStateResponseContents(controlStateQueue.First());
        }

        public virtual int DequeueControlStateResponse()
        {
            lock (controlStateQueue)
            {
                if (controlStateQueue.Count > 0)
                    controlStateQueue.RemoveAt(0);
            }
            return controlStateQueue.Count;
        }

        public void SendChkStatus()
        {
            SockSend("status");
        }

        public void SendChkItem()
        {
            int index = 2;
            if (index <= 4)
            {
                SockSend($"inputquery i{index}");
                index++;
            }
        }

        protected virtual void ReportStateToRemoteHost()
        {
            try
            {
                if (++stateReportCount >= 3)
                {
                    FireReportState();
                    stateReportCount = 0;
                }

                // if (controlStateQueue.Count > 0)
                // {
                //     ControlStateResponseContents state = null;
                //     PeekControlStateResponse(ref state);
                // 
                //     if (state != null)
                //         FireReportControlState(state);
                // }
            }
            catch (Exception ex)
            {
                stateReportCount = 0;
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void RunQueuedCommand(MobileRobotBoundObject bound, MobileRobotControlItemObject control, OmronMobileRobotState state)
        {
            while (!stopServiceBroker)
            {
                try
                {
                    MobileRobotControlItemStates previousState = control.State;
                    if (IsConnected)
                    {
                        if (previousState == MobileRobotControlItemStates.Waiting)
                            control.State = MobileRobotControlItemStates.Started;

                        if (shutdown.WaitOne(10))
                            break;
                        else
                        {
                            CurrentControlType = (ControlTypes)control.ControlType;

                            if (control.State < MobileRobotControlItemStates.Completed)
                            {
                                switch ((ControlTypes)control.ControlType)
                                {
                                    case ControlTypes.Get_Off_Elevator:
                                    case ControlTypes.Take_Elevator:
                                        {
                                            switch (subStep)
                                            {
                                                case ServiceBrokerSubSteps.Prepare:
                                                    {
                                                        //TaskGotostraight($"Goal{control.PositionId}");
                                                        TaskGotostraight(Convert.ToInt32(control.PositionId));
                                                        EnqueueControlStateResponse(ServiceParameter, bound, control);
                                                        subStep = ServiceBrokerSubSteps.Process;
                                                        state_ = MobileRobotStates.Going_To;
                                                        Debug.WriteLine("> Sent TaskGotostraight command");
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Process:
                                                    {
                                                        // 명령을 수행
                                                        switch (sentControlCommands.First().Value.second)
                                                        {
                                                            case MobileRobotControlItemStates.Interrupted:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Process;
                                                                    interruptedMessageTick = 0;
                                                                    subStep = ServiceBrokerSubSteps.ProcessInterrupt;
                                                                    Debug.WriteLine("> Received Interrupted state");
                                                                    ChangeControlItemState?.Invoke(this, null);
                                                                }
                                                                break;
                                                            case MobileRobotControlItemStates.Started:
                                                            case MobileRobotControlItemStates.Process:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Process;
                                                                    Debug.WriteLine("> Received running state");
                                                                    //subStep = ServiceBrokerSubSteps.Post;
                                                                    ChangeControlItemState?.Invoke(this, null);
                                                                }
                                                                break;
                                                            case MobileRobotControlItemStates.Error:
                                                                {
                                                                    PlaySoundFile($"{PlayType.Interrupted.ToString()}");

                                                                    control.State = MobileRobotControlItemStates.Error;
                                                                    subStep = ServiceBrokerSubSteps.Post;
                                                                    Debug.WriteLine("> Received Error state");
                                                                    {
                                                                        if (!RetryCommand((ControlTypes)control.ControlType))
                                                                        {
                                                                            control.State = MobileRobotControlItemStates.Error;
                                                                            Debug.WriteLine("> Received Error state");
                                                                            //EnqueueControlStateResponse(ServiceParameter, bound, control);
                                                                        }
                                                                        //else
                                                                        //{
                                                                        //    EnqueueControlStateResponse(ServiceParameter, bound, control);
                                                                        //}

                                                                        ChangeControlItemState?.Invoke(this, null);
                                                                    }
                                                                    break;
                                                                }
                                                            case MobileRobotControlItemStates.Completed:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Completed;
                                                                    subStep = ServiceBrokerSubSteps.Post;
                                                                    Debug.WriteLine("> Received Completed state");
                                                                    ChangeControlItemState?.Invoke(this, null);
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.ProcessInterrupt:
                                                    {
                                                        // if (IsOverDelayOfIntteruptMessage(Singleton<Config>.Instance.DelayOfTextToSpeech))
                                                        // {
                                                        //     interruptedMessageTick = Environment.TickCount;
                                                        // }
                                                        // else if (sentControlCommands.First().Value.second != MobileRobotControlItemStates.Interrupted)
                                                        // {
                                                        //     subStep = ServiceBrokerSubSteps.Process;
                                                        //     Debug.WriteLine("> Processed Interrupted state");
                                                        // }
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Post:
                                                    {
                                                        Debug.WriteLine("> Completed state");
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case ControlTypes.Route:
                                        {
                                            switch (subStep)
                                            {
                                                case ServiceBrokerSubSteps.Prepare:
                                                    {
                                                        Goto(Convert.ToInt32(control.PositionId), control.Heading);
                                                        state_ = MobileRobotStates.Going_To;
                                                        subStep = ServiceBrokerSubSteps.Process;
                                                        Debug.WriteLine($"> Sent {control.ControlType} command");
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Process:
                                                    {
                                                        switch (sentControlCommands.First().Value.second)
                                                        {
                                                            case MobileRobotControlItemStates.Started:
                                                                control.State = MobileRobotControlItemStates.Process;
                                                                EnqueueControlStateResponse(ServiceParameter, bound, control);
                                                                break;
                                                            case MobileRobotControlItemStates.Process:
                                                                EnqueueControlStateResponse(ServiceParameter, bound, control);
                                                                control.State = MobileRobotControlItemStates.Process;
                                                                //Debug.WriteLine("> Received running state");
                                                                ChangeControlItemState?.Invoke(this, null);
                                                                break;
                                                            case MobileRobotControlItemStates.Interrupted:
                                                            case MobileRobotControlItemStates.Error:
                                                                {

                                                                    if (!RetryCommand((ControlTypes)control.ControlType))
                                                                    {
                                                                        if (!IsInsideGoalBoundary(control, state))
                                                                        {
                                                                            control.State = MobileRobotControlItemStates.Error;
                                                                            Debug.WriteLine("> Received Error state");
                                                                        }
                                                                        else
                                                                        {
                                                                            control.State = MobileRobotControlItemStates.Completed;
                                                                            Stop();
                                                                        }
                                                                        //EnqueueControlStateResponse(ServiceParameter, bound, control);
                                                                    }
                                                                    else
                                                                    {
                                                                        PlaySoundFile($"{PlayType.Interrupted.ToString()}");
                                                                    }
                                                                    ChangeControlItemState?.Invoke(this, null);
                                                                }
                                                                break;
                                                            case MobileRobotControlItemStates.Completed:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Completed;
                                                                    subStep = ServiceBrokerSubSteps.Post;
                                                                    Debug.WriteLine("> Received Completed state");
                                                                }
                                                                ChangeControlItemState?.Invoke(this, null);
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.ProcessInterrupt:
                                                    {
                                                        // if (IsOverDelayOfIntteruptMessage(Singleton<Config>.Instance.DelayOfTextToSpeech))
                                                        // {
                                                        //     interruptedMessageTick = Environment.TickCount;
                                                        // }
                                                        // else if (sentControlCommands.First().Value.second != MobileRobotControlItemStates.Interrupted)
                                                        // {
                                                        //     subStep = ServiceBrokerSubSteps.Process;
                                                        //     Debug.WriteLine("> Processed Interrupted state");
                                                        // }
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Post:
                                                    {
                                                        Debug.WriteLine("> Completed state");
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case ControlTypes.Rotation:
                                        {
                                            switch (subStep)
                                            {
                                                case ServiceBrokerSubSteps.Prepare:
                                                    {
                                                        // 명령을 수행하기 위한 제한 조건 및 상태 점검
                                                        DoTaskSetHeading(control);
                                                        subStep = ServiceBrokerSubSteps.Process;
                                                        Debug.WriteLine("> Sent DoTaskDeltaHeading command");
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Process:
                                                    {
                                                        // 명령을 수행
                                                        switch (sentControlCommands.First().Value.second)
                                                        {
                                                            case MobileRobotControlItemStates.Interrupted:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Process;
                                                                    interruptedMessageTick = 0;
                                                                    subStep = ServiceBrokerSubSteps.ProcessInterrupt;
                                                                    Debug.WriteLine("> Received Interrupted state");
                                                                }
                                                                break;
                                                            case MobileRobotControlItemStates.Started:
                                                            case MobileRobotControlItemStates.Process:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Process;
                                                                    Debug.WriteLine("> Received running state");
                                                                    subStep = ServiceBrokerSubSteps.Post;
                                                                }
                                                                break;
                                                            case MobileRobotControlItemStates.Error:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Error;
                                                                    subStep = ServiceBrokerSubSteps.Post;
                                                                    Debug.WriteLine("> Received Error state");
                                                                }
                                                                break;
                                                            case MobileRobotControlItemStates.Completed:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Completed;
                                                                    subStep = ServiceBrokerSubSteps.Post;
                                                                    Debug.WriteLine("> Received Completed state");
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.ProcessInterrupt:
                                                    {
                                                        // TTS 전송 주기는 사용자 설정 (10 sec)
                                                        if (IsOverDelayOfIntteruptMessage(10000))
                                                        {
                                                            interruptedMessageTick = Environment.TickCount;
                                                        }
                                                        else if (sentControlCommands.First().Value.second != MobileRobotControlItemStates.Interrupted)
                                                        {
                                                            subStep = ServiceBrokerSubSteps.Process;
                                                            Debug.WriteLine("> Processed Interrupted state");
                                                        }
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Post:
                                                    {
                                                        Debug.WriteLine("> Completed state");
                                                        // 명령 수행 결과 대기 및 결과 보고
                                                        //stopServiceBroker = true;
                                                    }
                                                    break;
                                            }
                                        }
                                        break;

                                    case ControlTypes.Tts:
                                        {
                                            switch (subStep)
                                            {
                                                case ServiceBrokerSubSteps.Prepare:
                                                    {
                                                        subStep = ServiceBrokerSubSteps.Process;
                                                        Debug.WriteLine($"> Sent {control.ControlType} command");
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Process:
                                                    {
                                                        SayMessage(control.Message);
                                                        control.State = MobileRobotControlItemStates.Completed;
                                                        subStep = ServiceBrokerSubSteps.Post;
                                                        Debug.WriteLine("> Received running state");
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Post:
                                                    {
                                                        control.State = MobileRobotControlItemStates.Completed;
                                                        ChangeControlItemState?.Invoke(this, null);
                                                        Debug.WriteLine("> Completed state");
                                                    }
                                                    break;
                                            }
                                        }
                                        break;

                                    case ControlTypes.Lock:
                                    case ControlTypes.Unlock:
                                        {
                                            switch (subStep)
                                            {
                                                case ServiceBrokerSubSteps.Prepare:
                                                    {
                                                        if ((ControlTypes)control.ControlType == ControlTypes.Lock)
                                                        {
                                                            SetOutIoBit(7, true);
                                                            control.State = MobileRobotControlItemStates.Completed;
                                                        }
                                                        else
                                                        {
                                                            SetOutIoBit(7, false);
                                                            control.State = MobileRobotControlItemStates.Completed;
                                                        }

                                                        subStep = ServiceBrokerSubSteps.Process;
                                                        Debug.WriteLine($"> Sent {control.ControlType} command");
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Process:
                                                    {
                                                        switch (sentControlCommands.First().Value.second)
                                                        {
                                                            case MobileRobotControlItemStates.Waiting:
                                                            case MobileRobotControlItemStates.Completed:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Completed;
                                                                    subStep = ServiceBrokerSubSteps.Post;
                                                                    Debug.WriteLine("> Received Completed state");
                                                                }
                                                                ChangeControlItemState?.Invoke(this, null);
                                                                break;
                                                        }

                                                        //control.State = ControlItemStates.Process;
                                                        //subStep = ServiceBrokerSubSteps.Post;
                                                        //Debug.WriteLine("> Received running state");
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Post:
                                                    {
                                                        //control.State = ControlItemStates.Completed;
                                                        Debug.WriteLine("> Completed state");
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case ControlTypes.Charge:
                                        break;
                                    case ControlTypes.Change_Floor:
                                    case ControlTypes.Localization:
                                        {
                                            switch (subStep)
                                            {
                                                case ServiceBrokerSubSteps.Prepare:
                                                    {
                                                        // 명령을 수행하기 위한 제한 조건 및 상태 점검
                                                        //LocalizeAtGoal($"Goal{control.PositionId}");
                                                        DoTaskSetHeading(control);
                                                        EnqueueControlStateResponse(ServiceParameter, bound, control);
                                                        //로봇의 층을 구분하는건 100번대 숫자. 지하는 - 값을 갖는다.
                                                        //RobotCurrentState.Floor = (int)(Convert.ToInt32(control.PositionId) / 100);
                                                        subStep = ServiceBrokerSubSteps.Process;
                                                        Debug.WriteLine("> Sent DoTaskDeltaHeading command");
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Process:
                                                    {
                                                        // 명령을 수행
                                                        switch (sentControlCommands.First().Value.second)
                                                        {
                                                            case MobileRobotControlItemStates.Interrupted:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Process;
                                                                    interruptedMessageTick = 0;
                                                                    subStep = ServiceBrokerSubSteps.ProcessInterrupt;
                                                                    Debug.WriteLine("> Received Interrupted state");
                                                                }
                                                                break;
                                                            case MobileRobotControlItemStates.Started:
                                                            case MobileRobotControlItemStates.Process:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Process;
                                                                    Debug.WriteLine("> Received running state");
                                                                }
                                                                break;
                                                            case MobileRobotControlItemStates.Error:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Error;
                                                                    subStep = ServiceBrokerSubSteps.Post;
                                                                    Debug.WriteLine("> Received Error state");
                                                                }
                                                                break;
                                                            case MobileRobotControlItemStates.Completed:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Completed;
                                                                    subStep = ServiceBrokerSubSteps.Post;
                                                                    Debug.WriteLine("> Received Completed state");
                                                                    ChangeControlItemState?.Invoke(this, null);
                                                                    LocalizeToPoint(Convert.ToInt32(control.PositionId), control);
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.ProcessInterrupt:
                                                    {
                                                        // TTS 전송 주기는 사용자 설정 (10 sec)
                                                        if (IsOverDelayOfIntteruptMessage(10000))
                                                        {
                                                            interruptedMessageTick = Environment.TickCount;
                                                        }
                                                        else if (sentControlCommands.First().Value.second != MobileRobotControlItemStates.Interrupted)
                                                        {
                                                            subStep = ServiceBrokerSubSteps.Process;
                                                            Debug.WriteLine("> Processed Interrupted state");
                                                        }
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Post:
                                                    {
                                                        Debug.WriteLine("> Completed state");
                                                        EnqueueControlStateResponse(ServiceParameter, bound, control);
                                                        // 명령 수행 결과 대기 및 결과 보고
                                                        //stopServiceBroker = true;
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case ControlTypes.Play:
                                        {
                                            switch (subStep)
                                            {
                                                case ServiceBrokerSubSteps.Prepare:
                                                    {
                                                        // PlaySoundFile($"{((PlayType)control.playtype).ToString()}");
                                                        subStep = ServiceBrokerSubSteps.Process;
                                                        Debug.WriteLine($"> Sent {control.ControlType} command");
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Process:
                                                    {
                                                        switch (sentControlCommands.First().Value.second)
                                                        {
                                                            case MobileRobotControlItemStates.Interrupted:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Process;
                                                                    interruptedMessageTick = 0;
                                                                    ErrorCounts++;
                                                                    subStep = ServiceBrokerSubSteps.ProcessInterrupt;
                                                                    Debug.WriteLine("> Received Interrupted state");
                                                                }
                                                                break;
                                                            case MobileRobotControlItemStates.Started:
                                                            case MobileRobotControlItemStates.Process:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Process;
                                                                    Debug.WriteLine("> Received running state");
                                                                }
                                                                break;
                                                            case MobileRobotControlItemStates.Error:
                                                                {
                                                                    if (!RetryCommand((ControlTypes)control.ControlType))
                                                                    {
                                                                        control.State = MobileRobotControlItemStates.Error;
                                                                        Debug.WriteLine("> Received Error state");
                                                                    }
                                                                }
                                                                break;
                                                            case MobileRobotControlItemStates.Completed:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Completed;
                                                                    subStep = ServiceBrokerSubSteps.Post;
                                                                    Debug.WriteLine("> Received Completed state");
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.ProcessInterrupt:
                                                    {
                                                        // if (IsOverDelayOfIntteruptMessage(Singleton<Config>.Instance.DelayOfTextToSpeech))
                                                        // {
                                                        //     interruptedMessageTick = Environment.TickCount;
                                                        // }
                                                        // else if (sentControlCommands.First().Value.second != MobileRobotControlItemStates.Interrupted)
                                                        // {
                                                        //     subStep = ServiceBrokerSubSteps.Process;
                                                        //     Debug.WriteLine("> Processed Interrupted state");
                                                        // }
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Post:
                                                    {
                                                        Debug.WriteLine("> Completed state");
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                ErrorCounts = 0;
                                switch (control.State)
                                {
                                    case MobileRobotControlItemStates.Error:
                                        {
                                            stopServiceBroker = true;
                                            sentControlCommands.Clear();
                                            control.Remark = Remark;
                                            //control.State = ControlItemStates.Waiting;
                                            EnqueueControlStateResponse(ServiceParameter, bound, control);

                                            // if (bound.IsCompletedAllControlPlan)
                                            //     bound.State = MobileRobotBoundStates.Completed;
                                        }
                                        break;
                                    case MobileRobotControlItemStates.Completed:
                                        {
                                            stopServiceBroker = true;

                                            sentControlCommands.Remove(sentControlCommands.First().Key);
                                            EnqueueControlStateResponse(ServiceParameter, bound, control);

                                            // if (bound.IsCompletedAllControlPlan)
                                            // {
                                            //     bound.State = MobileRobotBoundStates.Completed;
                                            //     EnqueueControlStateResponse(ServiceParameter, bound, control);
                                            // }
                                        }
                                        break;
                                }
                                ChangeControlItemState?.Invoke(this, null);
                            }

                            // 2. Proceed tasks of control item 
                            // 3. Report control item updated information
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);

                        if (IsConnected)
                        {
                            if (control.State < MobileRobotControlItemStates.Completed)
                            {
                                subStep = ServiceBrokerSubSteps.Prepare;
                                Debug.WriteLine("> Reconnected. Recover last execute command step.");
                            }
                        }
                        else
                        {
                            sentControlCommands.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private bool RetryCommand(ControlTypes controlTypes)
        {
            if (ErrorCounts >= ErrorMaxCnt)
            {
                return false;
            }
            else
            {
                subStep = ServiceBrokerSubSteps.Prepare;
                var temp = sentControlCommands.FirstOrDefault();

                switch (controlTypes)
                {
                    case ControlTypes.Route:
                    case ControlTypes.Rotation:
                    case ControlTypes.Tts:
                    case ControlTypes.Lock:
                    case ControlTypes.Unlock:
                    case ControlTypes.Change_Floor:
                    case ControlTypes.Take_Elevator:
                    case ControlTypes.Get_Off_Elevator:
                    case ControlTypes.Charge:
                    case ControlTypes.Stop:
                    case ControlTypes.Localization:
                    case ControlTypes.Play:
                        sentControlCommands.Remove(temp.Key);
                        break;
                }
                ErrorCounts++;
                Debug.WriteLine($"> Received Error state [Retry({ErrorCounts})]");
                return true;
            }
        }

        protected void RunInstantCommand(MobileRobotBoundObject bound, MobileRobotControlItemObject control, HostControlCommand cmd)
        {
            while (!stopServiceBroker)
            {
                try
                {
                    if (IsConnected)
                    {
                        if (shutdown.WaitOne(10))
                        {
                            break;
                        }
                        else
                        {
                            if (control.State < MobileRobotControlItemStates.Completed)
                            {
                                switch ((ControlTypes)control.ControlType)
                                {
                                    case ControlTypes.Status:
                                        {
                                            QueryState();
                                        }
                                        break;
                                    case ControlTypes.Tts:
                                        {
                                            SayMessage(control.Message);
                                        }
                                        break;
                                    case ControlTypes.Stop:
                                        {
                                            switch (subStep)
                                            {
                                                case ServiceBrokerSubSteps.Prepare:
                                                    {
                                                        Stop();
                                                        EnqueueControlStateResponse(ServiceParameter, bound, control);
                                                        subStep = ServiceBrokerSubSteps.Process;
                                                        Debug.WriteLine($"> Sent {control.ControlType} command");
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Process:
                                                    {
                                                        switch (sentControlCommands.First().Value.second)
                                                        {
                                                            case MobileRobotControlItemStates.Interrupted:
                                                                {
                                                                    EnqueueControlStateResponse(ServiceParameter, bound, control);
                                                                    interruptedMessageTick = 0;
                                                                    subStep = ServiceBrokerSubSteps.ProcessInterrupt;
                                                                    Debug.WriteLine("> Received Interrupted state");
                                                                }
                                                                break;
                                                            case MobileRobotControlItemStates.Started:
                                                            case MobileRobotControlItemStates.Process:
                                                                {
                                                                    EnqueueControlStateResponse(ServiceParameter, bound, control);
                                                                    Debug.WriteLine("> Received running state");
                                                                }
                                                                break;
                                                            case MobileRobotControlItemStates.Error:
                                                                {
                                                                    EnqueueControlStateResponse(ServiceParameter, bound, control);
                                                                    subStep = ServiceBrokerSubSteps.Post;
                                                                    Debug.WriteLine("> Received Error state");
                                                                }
                                                                break;
                                                            case MobileRobotControlItemStates.Completed:
                                                                {
                                                                    control.State = MobileRobotControlItemStates.Waiting;
                                                                    EnqueueControlStateResponse(ServiceParameter, bound, control);
                                                                    subStep = ServiceBrokerSubSteps.Post;

                                                                    Debug.WriteLine("> Received Completed state");
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.ProcessInterrupt:
                                                    {
                                                        // if (IsOverDelayOfIntteruptMessage(Singleton<Config>.Instance.DelayOfTextToSpeech))
                                                        // {
                                                        //     interruptedMessageTick = Environment.TickCount;
                                                        // }
                                                        // else if (sentControlCommands.First().Value.second != MobileRobotControlItemStates.Interrupted)
                                                        // {
                                                        //     subStep = ServiceBrokerSubSteps.Process;
                                                        //     Debug.WriteLine("> Processed Interrupted state");
                                                        // }
                                                    }
                                                    break;
                                                case ServiceBrokerSubSteps.Post:
                                                    {
                                                        Debug.WriteLine("> Completed state");
                                                    }
                                                    break;
                                            }
                                        }

                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                switch (control.State)
                                {
                                    case MobileRobotControlItemStates.Error:
                                    case MobileRobotControlItemStates.Completed:
                                        {
                                            stopServiceBroker = true;
                                            sentControlCommands.Clear();

                                            if (control.State == MobileRobotControlItemStates.Error)
                                                EnqueueControlStateResponse(ServiceParameter, bound, control);

                                            // if (bound.IsCompletedAllControlPlan)
                                            //     bound.State = MobileRobotBoundStates.Completed;
                                        }
                                        break;
                                }
                            }

                            // 2. Proceed tasks of control item 
                            // 3. Report control item updated information
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);

                        if (IsConnected)
                        {
                            if (control.State < MobileRobotControlItemStates.Completed)
                            {
                                subStep = ServiceBrokerSubSteps.Prepare;
                                Debug.WriteLine("> Reconnected. Recover last execute command step.");
                            }
                        }
                        else
                        {
                            sentControlCommands.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        protected void Run(object data)
        {   // 1. Get current control item
            if (data != null)
            {
                // MobileRobotControlItemObject ctrl_ = null;
                HostControlCommand cmd_ = (HostControlCommand)data;
                OmronMobileRobotState state_ = RobotCurrentState;

                stopServiceBroker = false;
                subStep = ServiceBrokerSubSteps.Prepare;

                switch (cmd_.CommandType)
                {
                    case MobileRobotCommandTypes.QueuedCommand:
                        {
                            // RunQueuedCommand(bound, ctrl_, state);
                        }
                        break;
                    case MobileRobotCommandTypes.InstantCommand:
                        {
                            // RunInstantCommand(bound, ctrl_, cmd);
                        }
                        break;
                }
            }
            serviceBroker = null;
        }

        public virtual void CreateWatcher(int x = 0)
        {
            if (watcherTmr == null)
            {
                double elapsed = 0;

                // if (x == 0)
                // {
                //     elapsed = Singleton<Config>.Instance.IntervalOfRobotStateReport / 3.0f;
                // }
                // else
                // {
                //     elapsed = x * 1000 / 3.0f;
                // }
                // 
                watcherTmr = new System.Timers.Timer(elapsed);
                watcherTmr.Elapsed += OnElapsedWatcher;
                watcherTmr.Start();
            }
        }

        protected virtual void CreatetmrRefresh()
        {
            if (m_tmrRefresh == null)
            {
                m_tmrRefresh = new System.Timers.Timer(1000);
                m_tmrRefresh.Elapsed += RefrashElapsedWatcher;
                m_tmrRefresh.Start();
            }
        }


        public virtual void DestroyWatcher()
        {
            if (watcherTmr != null)
            {
                watcherTmr.Stop();
                watcherTmr.Dispose();
                watcherTmr = null;
            }

            readyToCommunicate = false;
        }

        protected virtual void DestroymrRefresh()
        {
            if (m_tmrRefresh != null)
            {
                m_tmrRefresh.Stop();
                m_tmrRefresh.Dispose();
                m_tmrRefresh = null;
            }
        }

        protected virtual void OnConnected(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                ChangedCommunicationState?.Invoke(this, EventArgs.Empty);
                CreatetmrRefresh();
            }
            if (IsFirstTimeConnect)
            {
                SendForcedStop();
                IsFirstTimeConnect = false;
            }
        }

        protected virtual void OnDisconnected(object sender, EventArgs e)
        {
            DestroyWatcher();
            DestroymrRefresh();
            ChangedCommunicationState?.Invoke(this, EventArgs.Empty);
            RobotCurrentState.State = MobileRobotMode.Disconnected;
            FireReportState();
        }


        protected virtual void OnReceived(object sender, string data)
        {
            string[] tokens_ = null;

            m_recvStr += data;
            string[] splitRcvStr = m_recvStr.Split('\n');
            if (splitRcvStr.Length == 0)
            {
                // 완성된 메세지를 받지 못함.
                return;
            }
            m_recvStr = m_recvStr.Substring(m_recvStr.LastIndexOf('\n') + 1);

            try
            {
                foreach (var item in splitRcvStr)
                {
                    if (readyToCommunicate)
                        tokens_ = item.Split(CONST_COMMAND_TOKEN_DELIMITERS, StringSplitOptions.RemoveEmptyEntries);
                    else
                        tokens_ = item.Split(CONST_TOKEN_DELIMITERS, StringSplitOptions.RemoveEmptyEntries);

                    if (tokens_.Length <= 0 || tokens_[0] == "Enter" || tokens_.Contains("password"))
                        return;
                    if (errorMessages.ContainsKey(tokens_[0]))
                    {   // Process error message
                        ProcessFailure(tokens_);
                        Remark = item;
                    }
                    else
                    {
                        if (StatusMessagePars(item))
                        {
                        }
                        else
                        {
                            // Process response or notification from mobile robot
                            ProcessResponse(tokens_);
                            Remark = string.Empty;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Logger.Debug($"")
                Debug.WriteLine($"OnReceived : Exception : {e.ToString()}");
            }
        }
        protected virtual void OnCatchedException(object sender, Exception ex)
        {
            ChangedCommunicationState?.Invoke(this, EventArgs.Empty);
            Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
        }
        protected virtual bool ProcessFailure(string[] data)
        {
            switch (errorMessages[data[0]].first)
            {
                case ErrorMessageTypes.Error:
                    {
                        switch (GetCommandType(sentControlCommands.First().Key.Split(' ').First()))
                        {
                            case MobileRobotCommands.CommandType_goto:
                                {
                                    Debug.WriteLine($"{data[1]}에 대한 이벤트를 수행할 수 없습니다.]");
                                    sentControlCommands.First().Value.second = MobileRobotControlItemStates.Error;
                                }
                                break;
                            case MobileRobotCommands.CommandType_doTask_gotostraight:
                                {
                                    Debug.WriteLine($"{data[1]}에 대한 이벤트를 수행할 수 없습니다.]");
                                    sentControlCommands.First().Value.second = MobileRobotControlItemStates.Error;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case ErrorMessageTypes.SetupError:
                    {

                    }
                    break;
                case ErrorMessageTypes.CommandError:
                    {
                        if (commandTable.ContainsKey(data[1]))
                        {
                            if (sentControlCommands.Count > 0 && sentControlCommands.First().Key.Contains(data[1]))
                            {
                                switch (GetCommandType(data[1]))
                                {
                                    case MobileRobotCommands.CommandType_goto:
                                        {
                                            Debug.WriteLine($"Goto에 대한 이벤트를 수행할 수 없습니다.[goto {data[2]}]");
                                            sentControlCommands.First().Value.second = MobileRobotControlItemStates.Error;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    break;
            }

            return true;
        }

        protected virtual bool ProcessResponse(string[] data)
        {
            Debug.WriteLine($"Rx> {data[0]}");
            Debug.WriteLine($"Rx> {data}");

            foreach (string item in data)
            {
                var currentData = item;
                if (sentControlCommands.Count > 0)
                {
                    var splitDatas = currentData.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var foundCmd = commandTable.First();
                    var foundMsg = sentControlCommands.First();
                    bool isFound = false;

                    var findCmds = commandTable.Where(p => p.Value.second.Where(x => x == splitDatas[0]).Any());

                    foreach (var subItem in sentControlCommands)
                    {
                        var tempKey = subItem.Key.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).First();
                        if (tempKey.ToLower() == "dotask")
                        {
                            var temp = subItem.Key.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            tempKey = $"{temp[0]} {temp[1]}";
                        }

                        var findList = findCmds.Where(p => p.Key.ToLower() == tempKey.ToLower());
                        if (findList.Any())
                        {
                            foundCmd = findList.First();
                            foundMsg = subItem;
                            isFound = true;
                            break;
                        }
                    }

                    if (isFound)
                    {
                        switch ((MobileRobotCommands)foundCmd.Value.first)
                        {
                            case MobileRobotCommands.CommandType_goto:
                                {
                                    status = splitDatas[0];
                                    switch (foundCmd.Value.second.FindIndex(x => x == splitDatas[0]))
                                    {
                                        case 0: // Going
                                            {
                                                Debug.WriteLine($"Goto에 대한 진행중 이벤트를 받았습니다.[{splitDatas[0]}]");
                                                //ControlItemObject obj = ControlItems[Convert.ToInt32(data)];
                                                //obj.State = ControlItemStates.Start;
                                                // 1. 현재 실행중인 command를 나타내는 flag
                                                // 2. 현재 실행중인 command 의 상태를 나타내는 flag 를 업데이트 한다.
                                                sentControlCommands[foundMsg.Key].second = MobileRobotControlItemStates.Process;
                                                ReceivedHostCommand?.Invoke(this, new EventArgsCommandResponse() { vehicleName = name, cmdType = MobileRobotCommands.CommandType_goto, cmdSubType = SubCommandTypes.Goto_Going });
                                            }
                                            break;
                                        case 1: // Arrived
                                            {
                                                Debug.WriteLine($"Goto에 대한 완료 이벤트를 받았습니다.[{splitDatas[0]}]");
                                                sentControlCommands[foundMsg.Key].second = MobileRobotControlItemStates.Completed;
                                                ReceivedHostCommand?.Invoke(this, new EventArgsCommandResponse() { vehicleName = name, cmdType = MobileRobotCommands.CommandType_goto, cmdSubType = SubCommandTypes.Goto_Arrived });
                                            }
                                            break;
                                        case 2: // Error:
                                            {
                                                Debug.WriteLine($"Goto에 대한 이벤트를 수행할 수 없습니다.[{splitDatas[0]}]");
                                                sentControlCommands[foundMsg.Key].second = MobileRobotControlItemStates.Error;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case MobileRobotCommands.CommandType_doTask_gotostraight:
                                {
                                    status = splitDatas[0];
                                    switch (foundCmd.Value.second.FindIndex(x => x == splitDatas[0]))
                                    {
                                        case 0: // doing
                                        case 2: // will:
                                            {
                                                Debug.WriteLine($"Gostraight에 대한 진행중 이벤트를 받았습니다.[{splitDatas[0]}]");
                                                // 1. 현재 실행중인 command를 나타내는 flag
                                                // 2. 현재 실행중인 command 의 상태를 나타내는 flag 를 업데이트 한다.
                                                sentControlCommands[foundMsg.Key].second = MobileRobotControlItemStates.Process;
                                                ReceivedHostCommand?.Invoke(this, new EventArgsCommandResponse() { vehicleName = name, cmdType = MobileRobotCommands.CommandType_doTask_gotostraight, cmdSubType = SubCommandTypes.Gostraight_Going });
                                            }
                                            break;
                                        case 1: // complete
                                            {
                                                Debug.WriteLine($"Gostraight에 대한 완료 이벤트를 받았습니다.[{splitDatas[0]}]");
                                                sentControlCommands[foundMsg.Key].second = MobileRobotControlItemStates.Completed;
                                                ReceivedHostCommand?.Invoke(this, new EventArgsCommandResponse() { vehicleName = name, cmdType = MobileRobotCommands.CommandType_doTask_gotostraight, cmdSubType = SubCommandTypes.Gostraight_Arrived });
                                            }
                                            break;
                                    }
                                }
                                break;
                            case MobileRobotCommands.CommandType_doTask_setheading:
                                {
                                    status = splitDatas[0];
                                    switch (foundCmd.Value.second.FindIndex(x => x == splitDatas[0]))
                                    {
                                        case 1: // doing
                                        case 0: // will:
                                            {
                                                Debug.WriteLine($"Gostraight에 대한 진행중 이벤트를 받았습니다.[{splitDatas[0]}]");
                                                // 1. 현재 실행중인 command를 나타내는 flag
                                                // 2. 현재 실행중인 command 의 상태를 나타내는 flag 를 업데이트 한다.
                                                sentControlCommands[foundMsg.Key].second = MobileRobotControlItemStates.Process;
                                                ReceivedHostCommand?.Invoke(this, new EventArgsCommandResponse() { vehicleName = name, cmdType = MobileRobotCommands.CommandType_doTask_setheading, cmdSubType = SubCommandTypes.Gostraight_Going });
                                            }
                                            break;
                                        case 2: // complete
                                            {
                                                Debug.WriteLine($"Gostraight에 대한 완료 이벤트를 받았습니다.[{splitDatas[0]}]");
                                                sentControlCommands[foundMsg.Key].second = MobileRobotControlItemStates.Completed;
                                                ReceivedHostCommand?.Invoke(this, new EventArgsCommandResponse() { vehicleName = name, cmdType = MobileRobotCommands.CommandType_doTask_setheading, cmdSubType = SubCommandTypes.Gostraight_Arrived });
                                            }
                                            break;
                                    }
                                }
                                break;

                            case MobileRobotCommands.CommandType_stop:
                                {
                                    status = splitDatas[0];
                                    switch (foundCmd.Value.second.FindIndex(x => x == splitDatas[0]))
                                    {
                                        case 0: // stopping
                                            {
                                                Debug.WriteLine($"stop에 대한 응답을 받았습니다.[{splitDatas[0]}]");
                                                sentControlCommands[foundMsg.Key].second = MobileRobotControlItemStates.Process;
                                            }
                                            break;
                                        case 1: // stoped
                                            {
                                                Debug.WriteLine($"stop에 대한 완료 응답을 받았습니다.[{splitDatas[0]}]");
                                                sentControlCommands[foundMsg.Key].second = MobileRobotControlItemStates.Completed;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case MobileRobotCommands.CommandType_say:
                                {
                                    status = splitDatas[0];
                                    switch (foundCmd.Value.second.FindIndex(x => x == splitDatas[0]))
                                    {
                                        case 0: // saying
                                            {
                                                Debug.WriteLine($"Say에 대한 응답을 받았습니다.[{splitDatas[0]}]");
                                                sentControlCommands[foundMsg.Key].second = MobileRobotControlItemStates.Completed;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case MobileRobotCommands.CommandType_outputOn:
                            case MobileRobotCommands.CommandType_outputOff:
                                {
                                    status = splitDatas[0];
                                    switch (foundCmd.Value.second.FindIndex(x => x == splitDatas[0]))
                                    {
                                        case 0: // output
                                            {
                                                Debug.WriteLine($"Output에 대한 응답을 받았습니다.[{splitDatas[0]}]");
                                                sentControlCommands[foundMsg.Key].second = MobileRobotControlItemStates.Completed;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case MobileRobotCommands.CommandType_inputQuery:
                                {
                                    status = splitDatas[0];
                                    switch (foundCmd.Value.second.FindIndex(x => x == splitDatas[0]))
                                    {
                                        case 0: // inputquery
                                            {
                                                Debug.WriteLine($"InputQuery에 대한 응답을 받았습니다.[{splitDatas[0]}]");
                                                sentControlCommands[foundMsg.Key].second = MobileRobotControlItemStates.Completed;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case MobileRobotCommands.CommandType_play:
                                {
                                    status = splitDatas[0];
                                    switch (foundCmd.Value.second.FindIndex(x => x == splitDatas[0]))
                                    {
                                        case 0: // playing
                                            {
                                                Debug.WriteLine($"play에 대한 응답을 받았습니다.[{splitDatas[0]}]");
                                                sentControlCommands[foundMsg.Key].second = MobileRobotControlItemStates.Completed;
                                            }
                                            break;
                                    }
                                }
                                break;
                        }

                        if (sentControlCommands.ContainsKey(foundMsg.Key))
                        {
                            if (sentControlCommands[foundMsg.Key].second == MobileRobotControlItemStates.Completed && sentInstantCommands.Where(x => x == foundMsg.Key).Any())
                            {
                                sentControlCommands.Remove(foundMsg.Key);
                                sentInstantCommands.Remove(foundMsg.Key);
                            }
                        }
                        else
                        {
                            Debug.Assert(false, $"#Debug : 해당 key값[{foundMsg.Key}]이 존재하지 않습니다.");
                        }
                    }
                    else
                    {
                        if (data.Contains("End of commands") && !readyToCommunicate)
                        {
                            readyToCommunicate = true;
                            EchoOff();
                        }
                    }
                }
                {
                    switch (item.ToLower())
                    {
                        case "enter password:":
                            Login();
                            break;
                        case "end of commands":
                            {
                                readyToCommunicate = true;
                                EchoOff();
                            }
                            break;
                        default:
                            ProcessNotification(data);
                            break;
                    }

                }
            }

            return true;
        }

        protected virtual bool ProcessNotification(string[] data)
        {
            return true;
        }

        protected virtual void OnElapsedWatcher(object sender, ElapsedEventArgs e)
        {
            watcherTmr.Stop();
            ReportStateToRemoteHost();
            CreatetmrRefresh();
            watcherTmr.Start();
        }

        protected virtual void RefrashElapsedWatcher(object sender, ElapsedEventArgs e)
        {
            if (IsConnected)
            {
                SendChkStatus();
                SendChkItem();
            }

        }
        #endregion

        #region Public methods
        public virtual bool Create(string id, string name, string address, int port, string password = "adept", bool connect = false)
        {
            try
            {
                this.id         = id;
                this.name       = name;
                this.address    = address;
                this.port       = port;
                this.password   = password;

                if (connect)
                    return Connect(address, port, true, password);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}={ex.Message}");
            }

            return false;
        }

        public virtual bool Connect()
        {
            stopCommunication = false;
            return Connect(address, port);
        }

        public virtual bool Connect(string address, int port, bool autoreconnect = true, string password = "adept")
        {
            try
            {
                Disconnect();

                if (sock != null)
                {
                    sock = new TelnetClient(address, port, password);
                    sock.Connected += OnConnected;
                    sock.Disconnected += OnDisconnected;
                    sock.Received += OnReceived;
                    sock.CatchedException += OnCatchedException;
                    sock.Connect(address, port);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return IsConnected;
        }

        public virtual void Disconnect(bool forced = false)
        {
            if (sock != null)
            {
                sock.Close();
                sock.Dispose();
                sock = null;
            }

            stopCommunication = forced;
        }

        public virtual void Login()
        {
            readyToCommunicate = false;
            sock.Login();
            CreateWatcher();
        }

        public virtual void EchoOff()
        {
            if (sock != null)
                SockSend("echo off");
        }

        public virtual void QueryState()
        {
            if (sock != null)
            {
                SockSend("status");
                sentControlCommands.Add("status", new Pair<DateTime, MobileRobotControlItemStates>(DateTime.Now, MobileRobotControlItemStates.Waiting));
            }
        }

        public virtual bool Goto(string goalname, string heading = null)
        {
            if (sock != null)
            {
                var sent = sentControlCommands.Where(x => x.Key.Split()[0] == "goto").ToList();

                if (sent.Count > 0)
                    sentControlCommands.Remove(sent.FirstOrDefault().Key);

                var cmd_ = CreateCmd(MobileRobotCommands.CommandType_goto, goalname, heading);
                SockSend(cmd_);
                sentControlCommands.Add(cmd_, new Pair<DateTime, MobileRobotControlItemStates>(DateTime.Now, MobileRobotControlItemStates.Waiting));
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool Goto(int positionId, string heading = null)
        {
            var obj = GetGoalObjFromPosId(positionId, out int buildId, out int floorId);
            var head = (Convert.ToInt32(heading) - 180) * -1;
            if (obj != null)
            {
                RobotCurrentState.BuildingID = buildId;
                RobotCurrentState.Floor = floorId;
                return Goto(obj.goalName, head.ToString());
            }
            else
            {
                return false;
            }
        }

        public MobileRobotGoalObject GetGoalObjFromPosId(int positionId, out int buildId, out int floorId)
        {
            buildId = 0;
            floorId = 0;

            // foreach (var item in Singleton<Config>.Instance.MapParameter.Buildings)
            // {
            //     foreach (var sub in item.FloorList)
            //     {
            //         foreach (var subsub in sub.GoalList)
            //         {
            //             if (subsub.goalId == positionId.ToString())
            //             {
            //                 buildId = Convert.ToInt32(item.BuildingId);
            //                 floorId = Convert.ToInt32(sub.floorId);
            //                 return subsub;
            //             }
            //         }
            //     }
            // }

            return null;
        }

        public virtual bool TaskGotostraight(string goalname)
        {

            if (sock != null)
            {
                var sent = sentControlCommands.Where(x => x.Key.Split()[0] == "dotask gotostraight").ToList();

                if (sent.Count > 0)
                    sentControlCommands.Remove(sent.FirstOrDefault().Key);

                var cmd_ = CreateCmd(MobileRobotCommands.CommandType_doTask_gotostraight, goalname);
                SockSend(cmd_);
                sentControlCommands.Add(cmd_, new Pair<DateTime, MobileRobotControlItemStates>(DateTime.Now, MobileRobotControlItemStates.Waiting));
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool TaskGotostraight(int positionId)
        {
            var obj = GetGoalObjFromPosId(positionId, out int buildId, out int floorId);

            if (obj != null)
            {
                RobotCurrentState.BuildingID = buildId;
                RobotCurrentState.Floor = floorId;
                return TaskGotostraight(obj.goalName);
            }
            else
            {
                return false;
            }
        }

        public virtual bool LocalizeToPoint(string goalname, string goalx, string goaly, MobileRobotControlItemObject control)
        {
            if (sock != null)
            {
                var cmd_ = CreateCmd(MobileRobotCommands.CommandType_localizetopoint, goalx, goaly, control.Heading);
                SockSend(cmd_);
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool LocalizeToPoint(int positionId, MobileRobotControlItemObject control)
        {
            var obj = GetGoalObjFromPosId(positionId, out int buildId, out int floorId);

            if (obj != null)
            {
                RobotCurrentState.BuildingID = buildId;
                RobotCurrentState.Floor = floorId;

                return LocalizeToPoint(obj.goalName, obj.goalX, obj.goalY, control);
            }
            else
            {
                return false;
            }
        }

        public virtual bool PlaySoundFile(string PlayTaget, InstantMode isInstant = InstantMode.QueuedCmd)
        {
            if (sock != null)
            {
                if (sentControlCommands.Where(x => x.Key.Split()[0] == "Play").Any())
                {
                    Debug.WriteLine($"Play[{PlayTaget}] command is running or not able to add.");
                    return false;
                }

                var cmd_ = CreateCmd(MobileRobotCommands.CommandType_play, $"{PlayTaget}.mp3");

                if (isInstant == InstantMode.InstantCmd)
                    sentInstantCommands.Add(cmd_);

                SockSend(cmd_);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SendForcedStop()
        {
            var cmd_ = CreateCmd(MobileRobotCommands.CommandType_stop);
            SockSend(cmd_);
        }

        public virtual bool Stop(InstantMode isInstant = InstantMode.QueuedCmd)
        {
            if (sock != null)
            {
                sentControlCommands.Clear();
                sentInstantCommands.Clear();
                var cmd_ = CreateCmd(MobileRobotCommands.CommandType_stop);

                if (isInstant == InstantMode.InstantCmd)
                {
                    sentInstantCommands.Add(cmd_);
                    sentControlCommands.Add(cmd_, new Pair<DateTime, MobileRobotControlItemStates>(DateTime.Now, MobileRobotControlItemStates.Waiting));
                    currentState = MobileRobotStates.Stopped;
                }
                if (SockSend(cmd_))
                {
                    currentState = MobileRobotStates.Stopped;
                    return true;
                }
            }
            return false;
        }

        public virtual void ItemLoad(int index = 2, InstantMode isInstant = InstantMode.InstantCmd) //고정 index 추가 예정
        {
            var cmd_ = string.Empty;
            var ioName = $"i{index}";

            if (sock != null)
            {
                cmd_ = CreateCmd(MobileRobotCommands.CommandType_inputQuery, index);

                if (index <= 4)
                {
                    SockSend(cmd_);
                    sentControlCommands.Add(cmd_, new Pair<DateTime, MobileRobotControlItemStates>(DateTime.Now, MobileRobotControlItemStates.Waiting));
                    index++;
                }
            }

            if (isInstant == InstantMode.InstantCmd)
                sentInstantCommands.Add(cmd_);
        }

        public virtual void SetOutIoBit(int index, bool isOn, InstantMode isInstant = InstantMode.QueuedCmd)
        {
            var cmd_ = string.Empty;
            var ioName = $"o{index}";

            if (isOn)
                cmd_ = CreateCmd(MobileRobotCommands.CommandType_outputOn, ioName);
            else
                cmd_ = CreateCmd(MobileRobotCommands.CommandType_outputOff, ioName);

            SockSend(cmd_);
            sentControlCommands[cmd_] = new Pair<DateTime, MobileRobotControlItemStates>(DateTime.Now, MobileRobotControlItemStates.Waiting);
            
            if (isInstant == InstantMode.InstantCmd)
                sentInstantCommands.Add(cmd_);
        }

        public bool SockSend(string cmd)
        {
            bool retVal = false;

            if (sock != null)
            {
                if (IsConnected)
                {
                    sock.WriteLine(cmd);
                    retVal = true;
                }
                else
                {
                    Debug.WriteLine($"socket is disconnected. [{address}]");
                    retVal = false;
                }
            }
            else
            {
                Debug.WriteLine($"socket information is not available. [{address}]");
                retVal = false;
            }
            return retVal;
        }

        public virtual void DoTaskSetHeading(MobileRobotControlItemObject control)
        {
            if (sock != null)
            {
                var head = (Convert.ToInt32(control.Heading) - 180) * -1;
                var cmd_ = CreateCmd(MobileRobotCommands.CommandType_doTask_setheading, head);
                SockSend(cmd_);
                sentControlCommands.Add(cmd_, new Pair<DateTime, MobileRobotControlItemStates>(DateTime.Now, MobileRobotControlItemStates.Waiting));
            }
        }

        public virtual void SayMessage(string message, InstantMode isInstant = InstantMode.QueuedCmd)
        {
            if (sock != null)
            {
                if (sentControlCommands.Where(x => x.Key.Split()[0] == "say").Any())
                {
                    Debug.WriteLine($"Say [{message}] is running or not able to add.");
                    return;
                }

                var cmd_ = CreateCmd(MobileRobotCommands.CommandType_say, message);
                SockSend(cmd_);
                sentControlCommands.Add(cmd_, new Pair<DateTime, MobileRobotControlItemStates>(DateTime.Now, MobileRobotControlItemStates.Waiting));
                
                if (isInstant == InstantMode.InstantCmd)
                    sentInstantCommands.Add(cmd_);

                return;
            }
        }

        public bool SendInstantCommand(MobileRobotControlItemObject control)
        {
            var ctrlType = (ControlTypes)control.ControlType;

            switch (ctrlType)
            {
                case ControlTypes.Route:
                    Goto(Convert.ToInt32(control.PositionId), control.Heading);
                    break;
                case ControlTypes.Rotation:  //heading 
                    {
                        if (ImmediateHalt())
                        {
                            if (Stop(InstantMode.InstantCmd))
                            {
                                DoTaskSetHeading(control);
                                return true;
                            }
                        }
                    }
                    break;
                case ControlTypes.Tts:
                    SayMessage(control.Message, InstantMode.InstantCmd);
                    return true;
                case ControlTypes.Lock:
                    SetOutIoBit(7, true, InstantMode.InstantCmd);
                    return true;
                case ControlTypes.Unlock:
                    SetOutIoBit(7, false, InstantMode.InstantCmd);
                    return true;
                case ControlTypes.Change_Floor:
                    break;
                case ControlTypes.Take_Elevator:
                    break;
                case ControlTypes.Get_Off_Elevator:
                    break;
                case ControlTypes.Charge:
                    break;
                case ControlTypes.Stop:
                    {
                        if (ImmediateHalt())
                            return Stop(InstantMode.InstantCmd);
                    }
                    break;
                case ControlTypes.Localization:
                    LocalizeToPoint(Convert.ToInt32(control.PositionId), control);
                    break;
                case ControlTypes.Play:
                    //PlaySoundFile(((PlayType)control.playtype).ToString(), InstantMode.InstantCmd);
                    break;
                default:
                    break;
                case ControlTypes.Status:
                    QueryState();
                    break;
                case ControlTypes.ItemLoad:
                    ItemLoad(2, InstantMode.InstantCmd);
                    break;
            }
            return false;
        }

        public string CreateCmd(MobileRobotCommands cmdtype, params object[] arguments)
        {
            var str = cmdtype.ToString().Replace("CommandType_", string.Empty).Replace("_", " ");
            foreach (var item in arguments)
                str += $" {item}";
            return str;
        }

        private MobileRobotStates state_ = MobileRobotStates.None;

        public virtual void FireReportState()
        {
            #region sample of offset code by cch
            ////sample code by cch
            //var offsetXOverGround = new int[] { 0, 100, 200, 300, 400 };
            //var offsetXUnderGround = new int[] { 0, 100, 200, 300, 400 };

            //var offsetYOverGround = new int[] { 0, 100, 200, 300, 400 };
            //var offsetYUnderGround = new int[] { 0, 100, 200, 300, 400 };

            //var offsetX = 0;
            //var offsetY = 0;
            //if (M_RobotState.Floor < 0)
            //{
            //    offsetX = offsetXUnderGround[-M_RobotState.Floor];
            //    offsetY = offsetYUnderGround[-M_RobotState.Floor];
            //}
            //else
            //{
            //    offsetX = offsetXOverGround[M_RobotState.Floor];
            //    offsetY = offsetYOverGround[M_RobotState.Floor];
            //}

            //M_RobotState.LocationX -= offsetX;
            //M_RobotState.LocationY -= offsetY;
            #endregion

            var bdId = RobotCurrentState.BuildingID;
            var flId = RobotCurrentState.Floor;
            var xOri = RobotCurrentState.LocationX;
            var yOri = RobotCurrentState.LocationY;

            // var ori = Singleton<Config>.Instance.MapParameter.Buildings.Where(p => p.BuildingId == bdId.ToString()).FirstOrDefault()?.FloorList.Where(i => i.floorId == flId.ToString()).FirstOrDefault()?.origin;
            // if (ori != null)
            // {
            //     var orisplit = ori.Split('/');
            //     if (orisplit.Count() == 2)
            //     {
            //         var x = (int)Convert.ToDouble(orisplit[0]);
            //         var y = (int)Convert.ToDouble(orisplit[1]);
            // 
            //         RobotCurrentState.OffsetX = RobotCurrentState.LocationX - x;
            //         RobotCurrentState.OffsetY = RobotCurrentState.LocationY - y;
            //     }
            // }
            // else
            // {
            //     RobotCurrentState.OffsetX = 0;
            //     RobotCurrentState.OffsetY = 0;
            // }

            PeriodicStateReport?.Invoke(this, RobotCurrentState);
        }

        public virtual void FireRobotEventState()
        {
            RobotEventReport?.Invoke(this, RobotCurrentState);
        }

        // public virtual void FireReportControlState(ControlStateResponseContents state)
        // {
        //     ControlStateReport?.Invoke(this, state);
        // }

        protected virtual bool StartControl(HostControlCommand cmd)
        {
            if (serviceBroker != null && cmd != null)
                return false;

            serviceBroker = new Thread(new ParameterizedThreadStart(Run));
            serviceBroker.Start(cmd);
            return true;
        }

        public virtual void UpdateNetwork(string address, string name, int port, int building, int floor)
        {
            if (this.address != address || this.port != port)
            {
                Disconnect(true);
            }
            this.address = address;
            this.name = name;
            this.port = port;
            this.RobotCurrentState.BuildingID = building;
            this.RobotCurrentState.Floor = floor;
        }

        public virtual void DequeResponse()
        {
            DequeueControlStateResponse();
        }
        #endregion
        private void RefreshStatus(string src)
        {
            //###############sample###############
            //Status: Stopped
            //StateOfCharge: 11.7
            //Location: -16871 - 573 - 90
            //LocalizationScore: 0.271967
            //Temperature: 31
            string[] words = src.Split('\n');

            foreach (var item in words)
            {
                string[] splitStr = item.Split(':');

                if (splitStr[0] == "EXTENDEDSTATUSFORHUMANS")
                {
                    //EXTENDEDSTATUSFORHUMANS: FAILED DOING TASK MOVE -1000 50 10 10 10\r

                    if (0 <= splitStr[1].IndexOf("DRIVING INTO DOCK"))
                    {
                        state_ = MobileRobotStates.DockingState_Driving_Into_Dock;
                    }
                    else if (0 <= splitStr[1].IndexOf("UNDOCKING"))
                    {
                        state_ = MobileRobotStates.DockingState_Undocking;
                    }
                    else if (0 <= splitStr[1].IndexOf("GOING TO DOCK AT LD/LYNX"))
                    {
                        state_ = MobileRobotStates.DockingState_Going_To;
                    }
                    else if (0 <= splitStr[1].IndexOf("CANNOT DRIVE TO DOCK SINCE NONE AVAILABLE"))
                    {
                        state_ = MobileRobotStates.DockingState_Cannot_Drive_To_Dock;
                    }
                    else if (0 <= splitStr[1].IndexOf("DOCKED"))
                    {
                        state_ = MobileRobotStates.DockingState_Overcharge;
                    }
                    else
                    {
                        CheckMotorsEnable(splitStr[1]);
                    }
                }
                else if (splitStr[0] == "STATEOFCHARGE")
                {
                    RobotCurrentState.Battery = Convert.ToDouble(splitStr[1]);
                }
                else if (splitStr[0] == "LOCATION")
                {
                    var data = splitStr[1].Split(' ');
                    RobotCurrentState.LocationX = Convert.ToInt32(data[1]);
                    RobotCurrentState.LocationY = Convert.ToInt32(data[2]);
                    RobotCurrentState.Orientation = Convert.ToSingle(data[3]);
                }
                else if (splitStr[1] == " DOCKINGSTATE")
                {
                    //DockingState: Undocked ForcedState: Unforced ChargeState: Not
                    //DockingState: Docking ForcedState: Unforced ChargeState: Not
                    //DockingState: Docking ForcedState: Unforced ChargeState: Bulk
                    //DockingState: Docked ForcedState: Unforced ChargeState: Bulk
                    //DockingState: Docked ForcedState: Unforced ChargeState: Overcharge
                    var regex = new Regex(@"\r\n?|\n|\t", RegexOptions.Compiled); // 공백문자 전부 지우기...
                    string result = regex.Replace(item, String.Empty);
                    result = result.Replace(" ", "");
                    var split = result.Split(':');

                    if (split[2] == "DOCKEDFORCEDSTATE" && split[3] == "UNFORCEDCHARGESTATE")
                    {
                        var status = state_;
                        if ("NOT" == split[4]) status = MobileRobotStates.DockingState_Not;
                        else if ("BULK" == split[4]) status = MobileRobotStates.DockingState_Bulk;
                        else if ("OVERCHARGE" == split[4]) status = MobileRobotStates.DockingState_Overcharge;
                    }
                }
            }
        }

        private void CheckMotorsEnable(string status)
        {
            if (status.IndexOf("MOTORS DISABLED") >= 0 || status.IndexOf("MOTORS STILL DISABLED") >= 0)
            {
                EnableMotors();
            }
        }

        public void EnableMotors()
        {
            SockSend("enablemotors");
        }

        protected bool[] IsDetectedI = new bool[10];

        protected bool InputQueryPars(string data)
        {
            string item = data;
            string msg = item.ToUpper();
            string[] words = data.Split('\n');
            string[] splitStr = item.Split(':');
            const int itemDtc1 = 1;
            const int itemDtc2 = 2;
            const int itemDtc3 = 3;
            const int boxOpened = 0;
            foreach (var load in words)
            {
                {
                    if (msg.IndexOf("INPUT") >= 0)
                    {
                        var iosplit = splitStr[1].Trim().Split(' ').First().Trim();
                        var ioOnOff = splitStr[1].Trim().Split(' ').Last().Trim();
                        var ioIndex = Convert.ToInt32(iosplit.Remove(0, 1)) - 1;

                        var isOn = (ioOnOff.ToUpper() == "ON") ? true : false;

                        if (IsDetectedI[ioIndex] != isOn)
                        {
                            FireReportState();
                            FireRobotEventState();
                            IsDetectedI[ioIndex] = isOn;

                            // IO에 따른 상태변경
                            if (IsDetectedI[itemDtc1] || IsDetectedI[itemDtc2] || IsDetectedI[itemDtc3])
                            {
                                RobotCurrentState.InnerBoxItemLoaded = ItemLoadyn.on;
                            }
                            else
                            {
                                RobotCurrentState.InnerBoxItemLoaded = ItemLoadyn.off;
                            }

                            if (IsDetectedI[boxOpened])
                            {
                                RobotCurrentState.InnerBoxOpened = LockerOpenyn.on;
                            }
                            else
                            {
                                RobotCurrentState.InnerBoxOpened = LockerOpenyn.off;
                            }
                        }

                        return true;
                    }
                }
            }
            return false;
        }

        protected bool[] IsActed = new bool[10];

        protected bool OutputQueryPars(string data)
        {
            string item = data;
            string msg = item.ToUpper();
            string[] words = data.Split('\n');
            string[] splitStr = item.Split(':');
            const int BoxLock = 7;
            foreach (var load in words)
            {
                {
                    if (msg.IndexOf("OUTPUT") >= 0)
                    {
                        var iosplit = splitStr[1].Trim().Split(' ').First().Trim();
                        var ioOnOff = splitStr[1].Trim().Split(' ').Last().Trim();
                        var ioIndex = Convert.ToInt32(iosplit.Remove(0, 1)) - 1;

                        IsActed[ioIndex] = (ioOnOff.ToUpper() == "ON") ? true : false;

                        if (IsActed[BoxLock])
                        {
                            RobotCurrentState.InnerBoxLocked = Lockyn.on;
                        }
                        else
                        {
                            RobotCurrentState.InnerBoxLocked = Lockyn.off;
                        }
                        FireReportState();
                        FireRobotEventState();
                        return true;
                    }
                }

            }

            return false;
        }

        protected bool StatusMessagePars(string data)
        {
            string item = data;
            string msg = item.ToUpper();

            if (msg.IndexOf("STATEOFCHARGE:") >= 0 ||
                msg.IndexOf("LOCATION:") >= 0 ||
                msg.IndexOf("LOCALIZATIONSCORE:") >= 0 ||
                msg.IndexOf("TEMPERATURE:") >= 0 ||
                msg.IndexOf("DOCKINGSTATE:") >= 0 || // "DockingState:"
                msg.IndexOf("EXTENDEDSTATUSFORHUMANS:") >= 0)
            {
                RefreshStatus(msg);
                return true;
            }

            if (msg.IndexOf("INPUT:") >= 0)
            {
                InputQueryPars(data);
                return true;
            }

            if (msg.IndexOf("OUTPUT:") >= 0)
            {
                OutputQueryPars(data);
                return true;
            }
            else if (msg.IndexOf("STATUS:") >= 0 || msg.IndexOf("ERROR:") >= 0)
            {
                string strDest = string.Empty;
                var status = GetRobotStatus(msg, ref strDest);
                state_ = status;

                switch (status)
                {
                    case MobileRobotStates.Failed_To_Get_To:
                    case MobileRobotStates.Failed_Going_To_Goal:
                        {
                            if (stopServiceBroker) // control이 진행중 일때는 Robot 상태를 Error로 바꾸지않음
                                RobotCurrentState.State = MobileRobotMode.Error;
                        }
                        break;
                }

                switch (status)
                {
                    case MobileRobotStates.Stopped:
                    case MobileRobotStates.Arrived_At:
                    case MobileRobotStates.Arrived_At_Point:
                        RobotCurrentState.State = MobileRobotMode.Idle;
                        break;
                    case MobileRobotStates.Going_To:
                    case MobileRobotStates.Going_To_Point:
                        {
                            if (CurrentControlType == ControlTypes.Take_Elevator)
                                RobotCurrentState.State = MobileRobotMode.Takeing_elevator;
                            else if (CurrentControlType == ControlTypes.Get_Off_Elevator)
                                RobotCurrentState.State = MobileRobotMode.Getting_off_elevator;
                            else
                                RobotCurrentState.State = MobileRobotMode.Run;
                        }
                        break;
                    case MobileRobotStates.Estop_Pressed:
                    case MobileRobotStates.Estop_Relieved_But_Moters_Still_Disabled:
                    case MobileRobotStates.Failed_Going_To_Goal:
                    case MobileRobotStates.Failed_To_Get_To:
                        RobotCurrentState.State = MobileRobotMode.Error;
                        break;
                    default:
                        break;
                }

                return true;
            }

            switch (state_)
            {
                case MobileRobotStates.None:
                case MobileRobotStates.Stopping:
                case MobileRobotStates.Stopped:
                case MobileRobotStates.Arrived_At:
                case MobileRobotStates.Arrived_At_Point:
                case MobileRobotStates.Completed_Doing_Task_DeltaHeading:
                    RobotCurrentState.State = MobileRobotMode.Idle;
                    break;
                case MobileRobotStates.Going_To:
                case MobileRobotStates.Going_To_Point:
                case MobileRobotStates.Doing_Task_DeltaHeading:
                case MobileRobotStates.DockingState_Going_To:
                case MobileRobotStates.DockingState_Undocking:                  // Dock 상태 추가필요
                    {
                        if (CurrentControlType == ControlTypes.Take_Elevator)
                            RobotCurrentState.State = MobileRobotMode.Takeing_elevator;
                        else if (CurrentControlType == ControlTypes.Get_Off_Elevator)
                            RobotCurrentState.State = MobileRobotMode.Getting_off_elevator;
                        else
                            RobotCurrentState.State = MobileRobotMode.Run;
                    }
                    break;
                case MobileRobotStates.Failed_Going_To_Goal:
                case MobileRobotStates.Failed_To_Get_To:
                case MobileRobotStates.Estop_Pressed:
                case MobileRobotStates.Estop_Relieved_But_Moters_Still_Disabled:
                case MobileRobotStates.No_GoalName:
                case MobileRobotStates.Can_Not_Find_Path:
                case MobileRobotStates.DockingState_Cannot_Drive_To_Dock:
                case MobileRobotStates.DockingState_Not:
                    RobotCurrentState.State = MobileRobotMode.Error;
                    break;
                case MobileRobotStates.DockingState_Bulk:
                case MobileRobotStates.DockingState_Overcharge:
                case MobileRobotStates.DockingState_Driving_Into_Dock:
                    RobotCurrentState.State = MobileRobotMode.Dock;
                    break;
            }

            return false;
        }

        public void ClearServiceParam()
        {
            // Bounds.Clear();
            ChangeControlItemState?.Invoke(this, null);
        }

        private MobileRobotStates GetRobotStatus(string msg, ref string dest)
        {
            MobileRobotStates rtn = MobileRobotStates.None;
            string[] words = msg.Split(' ');
            string strStateMsg = string.Empty, strbuff = string.Empty, strdest = string.Empty;

            if (0 <= msg.IndexOf("COMPLETED DOING TASK DELTAHEADING"))
            {
                rtn = MobileRobotStates.Completed_Doing_Task_DeltaHeading;
            }
            else if (0 <= msg.IndexOf("FAILED")
                    || 0 <= msg.IndexOf("FAILED GOING TO")
                    || 0 <= msg.IndexOf("ERROR: MANAGEDMOTION:"))
            {
                rtn = MobileRobotStates.Failed_Going_To_Goal;
            }
            else if (0 <= msg.IndexOf("CANNOT FIND PATH"))
            {
                rtn = MobileRobotStates.Can_Not_Find_Path;
            }
            else if (0 <= msg.ToString().IndexOf("GOING TO"))
            {
                if (0 <= msg.ToString().IndexOf("POINT"))
                {
                    rtn = MobileRobotStates.Going_To_Point;
                }
                else
                {
                    rtn = MobileRobotStates.Going_To;
                }
                strdest = words[words.Count() - 1];
            }
            else if (0 <= msg.ToString().IndexOf("ARRIVED AT"))
            {
                if (0 <= msg.ToString().IndexOf("POINT"))
                {
                    rtn = MobileRobotStates.Arrived_At_Point;
                }
                else
                {
                    rtn = MobileRobotStates.Arrived_At;
                }
                strdest = words[words.Count() - 1];
            }
            else if (0 <= msg.ToString().IndexOf("DOING TASK DELTAHEADING"))
            {
                rtn = MobileRobotStates.Doing_Task_DeltaHeading;
            }
            else if (0 <= msg.ToString().IndexOf("COMPLETED DOING TASK DELTAHEADING"))
            {
                rtn = MobileRobotStates.Completed_Doing_Task_DeltaHeading;
            }
            else if (0 <= msg.ToString().IndexOf("STOPPING"))
            {
                rtn = MobileRobotStates.Stopping;
            }
            else if (0 <= msg.ToString().IndexOf("STOPPED"))
            {
                rtn = MobileRobotStates.Stopped;
            }
            else if (0 <= msg.ToString().IndexOf("FAILED TO GET TO"))
            {
                rtn = MobileRobotStates.Failed_To_Get_To;
            }
            else if (0 <= msg.ToString().IndexOf("ESTOP PRESSED"))
            {
                rtn = MobileRobotStates.Estop_Pressed;
            }
            else if (0 <= msg.ToString().IndexOf("ESTOP RELIEVED BUT MOTORS STILL DISABLED"))
            {
                rtn = MobileRobotStates.Estop_Relieved_But_Moters_Still_Disabled;
            }
            else if (0 <= msg.ToString().IndexOf("NO GOALNAME"))
            {
                rtn = MobileRobotStates.No_GoalName;
            }

            strdest = strdest.Replace("\r", "");
            dest    = strdest;
            return rtn;
        }

        public bool IsInsideGoalBoundary(MobileRobotControlItemObject control, OmronMobileRobotState state)
        {
            var obj = GetGoalObjFromPosId(Convert.ToInt32(control.PositionId), out int buildId, out int floorId);
            var distancepoint = Math.Sqrt(Math.Pow(Convert.ToInt32(obj.goalX) - state.LocationX, 2) + Math.Pow(Convert.ToInt32(obj.goalY) - state.LocationY, 2));

            // if (distancepoint >= Convert.ToDouble(control.Boundary))
            // {
            //     Debug.WriteLine($"**********************************************************************현재 {state.Id} 로봇과 골과의 거리는 {distancepoint} {control.Boundary}내 이므로 실패!");
            //     return false;
            // }
            // else
            // {
            //     Debug.WriteLine($"**********************************************************************현재 {state.Id} 로봇과 골과의 거리는 {distancepoint} {control.Boundary}내 이므로 성공!");
            //     return true;
            // }

            return true;
        }
    }
}
#endregion
