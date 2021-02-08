#region Imports
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TechFloor.Components;
using TechFloor.Object;
#endregion

#region Program
namespace TechFloor
{
    #region Enumerations
    public enum RobotProgramStates
    {
        Unknown,
        Stopped,
        Paused,
        Playing,
    }

    public enum RobotPowerStates
    {
        Unknown,
        PowerOff,
        PowerOn,
    }

    public enum RobotBrakeStates
    {
        Unknown,
        Released,
        Locked,
    }

    public enum RobotSafetyModes
    {
        Unknown,
        Normal,
        Reduced,
        ProtectiveStop,
        Recovery,
        SafeguardStop,
        SystemEmergencyStop,
        RobotEmergencyStop,
        Violation,
        Fault
    }

    public enum RobotModes
    {
        Unknown = -1,
        NoController,
        FreeDrive,
        Ready,
        Booting,
        SecurityStop,
        EmergecyStop,
        Fault,
        PowerOff,
        Disconnected,
        Shutdown,
        PowerOn,
        ConfirmSafety,
        Idle,
        Backdrive,
        Running
    }

    public enum RobotControllerCommands
    {
        None,
        ProgramState,
        StopProgram,
        PauseProgram,
        LoadProgram,
        PlayProgram,
        QuitProgram,
        Shutdown,
        PowerOn,
        PowerOff,
        BrakeRelease,
        SafetyMode,
        RobotMode,
        UnlockProtectiveStop,
        CloseSafetyPopup,
        LoadInstallation,
        RestartSafety,
        Popup,
        ClosePopup,
        Version,
        SetUserRole,
        AddToLog,

    }
    #endregion

    public class RobotControllerMessage
    {
        #region Fields
        public readonly int SentTick;
        public RobotControllerCommands Command = RobotControllerCommands.None;
        public string Data = string.Empty;
        #endregion

        #region Constructors
        public RobotControllerMessage(RobotControllerCommands command, string data)
        {
            Command = command;
            Data = data;
            SentTick = App.TickCount;
        }

        public RobotControllerMessage(string message)
        {
            switch (message = message.Replace("\r", string.Empty))
            {
                case "programState":
                    {
                        Command = RobotControllerCommands.ProgramState;
                    }
                    break;
                case "stop":
                    {
                        Command = RobotControllerCommands.StopProgram;
                    }
                    break;
                case "pause":
                    {
                        Command = RobotControllerCommands.PauseProgram;
                    }
                    break;
                case "play":
                    {
                        Command = RobotControllerCommands.PlayProgram;
                    }
                    break;
                case "quit":
                    {
                        Command = RobotControllerCommands.QuitProgram;
                    }
                    break;
                case "shutdown":
                    {
                        Command = RobotControllerCommands.Shutdown;
                    }
                    break;
                case "robotmode":
                    {
                        Command = RobotControllerCommands.RobotMode;
                    }
                    break;
                case "safetymode":
                    {
                        Command = RobotControllerCommands.SafetyMode;
                    }
                    break;
                case "power on":
                    {
                        Command = RobotControllerCommands.PowerOn;
                    }
                    break;
                case "power off":
                    {
                        Command = RobotControllerCommands.PowerOff;
                    }
                    break;
                case "brake release":
                    {
                        Command = RobotControllerCommands.BrakeRelease;
                    }
                    break;
                case "unlock protective stop":
                    {
                        Command = RobotControllerCommands.UnlockProtectiveStop;
                    }
                    break;
                case "close safety popup":
                    {
                        Command = RobotControllerCommands.CloseSafetyPopup;
                    }
                    break;
                case "restart safety":
                    {
                        Command = RobotControllerCommands.RestartSafety;
                    }
                    break;
                case "close popup":
                    {
                        Command = RobotControllerCommands.ClosePopup;
                    }
                    break;
                case "PolyscopeVersion":
                    {
                        Command = RobotControllerCommands.Version;
                    }
                    break;
                default:
                    {
                        if (message.Contains("load /programs/"))
                        {
                            Command = RobotControllerCommands.LoadProgram;
                        }
                        else if (message.Contains("load installation"))
                        {
                            Command = RobotControllerCommands.LoadInstallation;
                        }
                        else if (message.Contains("popup"))
                        {
                            Command = RobotControllerCommands.Popup;
                        }
                        else if (message.Contains("setUserRole"))
                        {
                            Command = RobotControllerCommands.SetUserRole;
                        }
                        else if (message.Contains("addToLog"))
                        {
                            Command = RobotControllerCommands.AddToLog;
                        }
                    }
                    break;
            }
            
            SentTick = App.TickCount;
        }
        #endregion
    }

    public class RobotController : SimulatableDevice
    {
        #region Constants
        protected readonly char[] CONST_TOKEN_DELIMITERS        = { ' ', '\r', '\n', '\0' };
        public const string CONST_ROBOT_PROGRAM_REV2            = "REELROBOT_REV2.urp";
        public const string CONST_ROBOT_PROGRAM_REV3            = "REELROBOT_REV3.urp";
        #endregion

        #region Fields
        protected bool failure                                  = false;
        protected bool waitResponseFlag                         = false;
        protected int serverPort                                = 29999;
        protected int receivedFlag                              = 0;
        protected int timeoutOfResponse                         = 3000;
        protected int timeoutOfProgramLoad                      = 30000;
        protected int timeoutOfProgramPlay                      = 30000;
        protected int timeoutOfAction                           = 30000;
        protected int timeoutOfMove                             = 30000;
        protected int timeoutOfHome                             = 60000;
        protected string serverAddress                          = "192.168.100.100";
        protected string loadedProgram                          = string.Empty;
        protected string version                                = string.Empty;
        protected string role                                   = string.Empty;
        protected RobotControllerCommands stateQueury           = RobotControllerCommands.ProgramState;
        protected RobotControllerCommands sentCommand           = RobotControllerCommands.None;
        protected RobotProgramStates programState               = RobotProgramStates.Unknown;
        protected RobotProgramStates previousProgramState       = RobotProgramStates.Unknown;
        protected RobotPowerStates powerState                   = RobotPowerStates.Unknown;
        protected RobotBrakeStates brakeState                   = RobotBrakeStates.Locked;
        protected RobotSafetyModes safetyMode                   = RobotSafetyModes.Unknown;
        protected RobotSafetyModes previousSafetyMode           = RobotSafetyModes.Unknown;
        protected RobotModes robotMode                          = RobotModes.Unknown;
        protected RobotModes previousRobotMode                  = RobotModes.Unknown;
        protected CommunicationStates communicationState        = CommunicationStates.None;
        protected Queue<RobotControllerMessage> messageQueue    = new Queue<RobotControllerMessage>();
        protected AsyncSocketClient client                      = null;
        protected System.Timers.Timer ConnectionWatcher         = null;
        #endregion

        #region Properties
        public bool IsReceived                                  => receivedFlag == 1;
        public bool IsRunnable                                  => safetyMode == RobotSafetyModes.Normal && robotMode == RobotModes.Running && programState == RobotProgramStates.Playing; 
        public bool IsEmergencyStop                             => safetyMode == RobotSafetyModes.SystemEmergencyStop || safetyMode == RobotSafetyModes.RobotEmergencyStop || robotMode == RobotModes.EmergecyStop;
        public bool IsProtectiveStop                            => safetyMode == RobotSafetyModes.ProtectiveStop || safetyMode == RobotSafetyModes.Violation || safetyMode == RobotSafetyModes.Fault || robotMode == RobotModes.SecurityStop;
        public bool IsFailure                                   => failure;
        public bool IsConnected                                 => (client != null) ? client.IsConnected : false;
        public string Version                                   => version;
        public string Role                                      => role;
        public string LoadedProgram                             => loadedProgram;
        public RobotControllerCommands Command                  => sentCommand;
        public RobotProgramStates ProgramState                  => programState;
        public RobotPowerStates PowerState                      => powerState;
        public RobotBrakeStates BreakState                      => brakeState;
        public RobotSafetyModes SafetyMode                      => safetyMode;
        public RobotModes RobotMode                             => robotMode;
        public CommunicationStates CommunicationState           => communicationState;
        public Queue<RobotControllerMessage> MessageQueue       => messageQueue;
        #endregion

        #region Events
        public event EventHandler<CommunicationStates> CommunicationStateChanged;
        public event EventHandler<string> ReportRuntimeLog;
        #endregion

        #region Protected methods
        protected override void DisposeManagedObjects()
        {
            base.DisposeManagedObjects();

            if (ConnectionWatcher != null)
                ConnectionWatcher.Stop();
        }

        protected void FireCommunicationStateChanged(CommunicationStates state)
        {
            CommunicationStateChanged?.Invoke(this, communicationState = state);
        }

        protected void ConnectClose()
        {
            client.Disconnect();
        }

        protected void OnTickWatcher(object sender, EventArgs e)
        {
            ConnectionWatcher.Stop();
            if (IsConnected)
            {
                if (messageQueue.Count > 0)
                {
                    int timeout_ = timeoutOfResponse;
                    RobotControllerMessage sentMessage_ = messageQueue.Peek();

                    switch (sentMessage_.Command)
                    {
                        case RobotControllerCommands.None:
                        case RobotControllerCommands.ProgramState:
                        case RobotControllerCommands.SafetyMode:
                        case RobotControllerCommands.RobotMode:
                        case RobotControllerCommands.Popup:
                        case RobotControllerCommands.ClosePopup:
                        case RobotControllerCommands.Version:
                        case RobotControllerCommands.SetUserRole:
                        case RobotControllerCommands.AddToLog:
                        case RobotControllerCommands.CloseSafetyPopup:
                            break;
                        case RobotControllerCommands.StopProgram:
                        case RobotControllerCommands.PauseProgram:
                        case RobotControllerCommands.QuitProgram:
                        case RobotControllerCommands.Shutdown:
                        case RobotControllerCommands.PowerOn:
                        case RobotControllerCommands.PowerOff:
                        case RobotControllerCommands.BrakeRelease:
                        case RobotControllerCommands.UnlockProtectiveStop:
                        case RobotControllerCommands.RestartSafety:
                            timeout_ = timeoutOfAction;
                            break;
                        case RobotControllerCommands.LoadProgram:
                        case RobotControllerCommands.LoadInstallation:
                            timeout_ = timeoutOfProgramLoad;
                            break;
                        case RobotControllerCommands.PlayProgram:
                            timeout_ = timeoutOfProgramPlay;
                            break;
                    }

                    IsResponseTimeout(timeout_);
                }
                else
                {
                    if (SendCommand(stateQueury))
                    {
                        switch (stateQueury)
                        {
                            case RobotControllerCommands.ProgramState:
                                stateQueury = RobotControllerCommands.SafetyMode;
                                break;
                            case RobotControllerCommands.SafetyMode:
                                stateQueury = RobotControllerCommands.RobotMode;
                                break;
                            case RobotControllerCommands.RobotMode:
                                stateQueury = RobotControllerCommands.ProgramState;
                                break;
                        }
                    }
                }
            }
            else
            {
                TryClientConnecting(serverAddress, serverPort);
            }

            ConnectionWatcher.Start();
        }

        protected void OnClientSent(object sender, AsyncSocketSendEventArgs e)
        {
        }

        protected void OnClientConneted(object sender, AsyncSocketConnectionEventArgs e)
        {
            FireCommunicationStateChanged(CommunicationStates.Connected);
            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.Id}");
        }

        protected void OnClientDisconnected(object sender, AsyncSocketConnectionEventArgs e)
        {
            FlushMessageQueue();
            FireCommunicationStateChanged(CommunicationStates.Disconnected);
            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.Id}");
        }

        protected void OnClientFailure(object sender, AsyncSocketErrorEventArgs e)
        {
            if (client != null)
                client.Disconnect();

            FlushMessageQueue();
            FireCommunicationStateChanged(CommunicationStates.Disconnected);
            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.AsyncSocketException.Message}");
        }

        protected void OnClientReceived(object sender, AsyncSocketReceiveEventArgs e)
        {
            bool record_                        = true;
            string message_                     = string.Empty;
            string[] tokens_                    = null;
            RobotControllerMessage sentMessage_ = null;

            if (string.IsNullOrEmpty(message_ = (new string(Encoding.Default.GetChars(e.ReceiveData))).Replace("\0", string.Empty)))
                return;

            if (GetSentMessage(ref sentMessage_))
            {
                tokens_ = message_.Split(CONST_TOKEN_DELIMITERS, StringSplitOptions.RemoveEmptyEntries);

                switch (sentMessage_.Command)
                {
                    case RobotControllerCommands.ProgramState:
                        {
                            record_ = false;

                            switch (tokens_[0].ToLower())
                            {
                                default:
                                    {
                                        programState = RobotProgramStates.Unknown;
                                    }
                                    break;
                                case "stopped":
                                    {
                                        programState = RobotProgramStates.Stopped;
                                    }
                                    break;
                                case "paused":
                                    {
                                        programState = RobotProgramStates.Paused;
                                    }
                                    break;
                                case "playing":
                                    {
                                        programState = RobotProgramStates.Playing;
                                    }
                                    break;
                            }

                            if (tokens_.Length >= 2)
                                loadedProgram = tokens_[1];

                            if (previousProgramState != programState)
                            {
                                record_ = true;
                                previousProgramState = programState;
                            }
                        }
                        break;
                    case RobotControllerCommands.StopProgram:
                        {
                            if (message_.ToLower().Contains("stopped"))
                            {
                                programState = RobotProgramStates.Stopped;
                            }
                            else
                            {   // "failed to execute: stop":
                                programState = RobotProgramStates.Unknown;
                                failure = true;
                            }
                        }
                        break;
                    case RobotControllerCommands.PauseProgram:
                        {
                            if (message_.ToLower().Contains("pausing program"))
                            {
                                programState = RobotProgramStates.Paused;
                            }
                            else
                            {   // "failed to execute: pause":
                                programState = RobotProgramStates.Unknown;
                                failure = true;
                            }
                        }
                        break;
                    case RobotControllerCommands.LoadProgram:
                        {
                            switch (tokens_[0])
                            {
                                default:
                                    {
                                        if (message_.Contains("Loading program:"))
                                        {
                                            loadedProgram = message_.Substring(message_.IndexOf(":") + 1, message_.Length - message_.IndexOf(":") - 1).Trim();
                                        }
                                        else if (message_.ToUpper().Contains("FILE NOT FOUND:") ||
                                            message_.ToUpper().Contains("ERROR WHILE LOADING PROGRAM:"))
                                        {
                                            programState = RobotProgramStates.Unknown;
                                            failure = true;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case RobotControllerCommands.PlayProgram:
                        {
                            if (message_.ToLower().Contains("starting program"))
                            {
                                programState = RobotProgramStates.Playing;
                            }
                            else
                            {   // "failed to execute: play":
                                programState = RobotProgramStates.Unknown;
                                failure = true;
                            }
                        }
                        break;
                    case RobotControllerCommands.QuitProgram:
                        {
                            switch (tokens_[0].ToLower())
                            {
                                case "disconnected":
                                    {
                                        programState = RobotProgramStates.Stopped;
                                    }
                                    break;
                                default:
                                    {
                                        programState = RobotProgramStates.Unknown;
                                        failure = true;
                                    }
                                    break;
                            }
                        }
                        break;
                    case RobotControllerCommands.Shutdown:
                        {
                            if (message_.ToLower().Contains("shutting down"))
                            {
                                programState = RobotProgramStates.Stopped;
                            }
                            else
                            {
                                programState = RobotProgramStates.Unknown;
                                failure = true;
                            }
                        }
                        break;
                    case RobotControllerCommands.PowerOn:
                        {
                            if (message_.ToLower().Contains("powering on"))
                            {
                                powerState = RobotPowerStates.PowerOn;
                            }
                            else
                            {
                                powerState = RobotPowerStates.Unknown;
                                failure = true;
                            }
                        }
                        break;
                    case RobotControllerCommands.PowerOff:
                        {
                            if (message_.ToLower().Contains("powering off"))
                            {
                                powerState = RobotPowerStates.PowerOff;
                            }
                            else
                            {
                                powerState = RobotPowerStates.Unknown;
                                failure = true;
                            }
                        }
                        break;
                    case RobotControllerCommands.BrakeRelease:
                        {
                            if (message_.ToLower().Contains("brake releasing"))
                            {
                                brakeState = RobotBrakeStates.Released;
                            }
                            else
                            { 
                                brakeState = RobotBrakeStates.Locked;
                            }
                        }
                        break;
                    case RobotControllerCommands.SafetyMode:
                        {
                            record_ = false;
                            message_ = message_.Replace("Safetymode: ", string.Empty);

                            switch (tokens_[1].ToLower())
                            {
                                case "normal":
                                    {
                                        safetyMode = RobotSafetyModes.Normal;
                                    }
                                    break;
                                case "reduced":
                                    {
                                        safetyMode = RobotSafetyModes.Reduced;
                                    }
                                    break;
                                case "protective_stop":
                                    {
                                        safetyMode = RobotSafetyModes.ProtectiveStop;
                                    }
                                    break;
                                case "recovery":
                                    {
                                        safetyMode = RobotSafetyModes.Recovery;
                                    }
                                    break;
                                case "safeguard_stop":
                                    {
                                        safetyMode = RobotSafetyModes.SafeguardStop;
                                    }
                                    break;
                                case "system_emergency_stop":
                                    {
                                        safetyMode = RobotSafetyModes.SystemEmergencyStop;
                                    }
                                    break;
                                case "robot_emergency_stop":
                                    {
                                        safetyMode = RobotSafetyModes.RobotEmergencyStop;
                                    }
                                    break;
                                case "violation":
                                    {
                                        safetyMode = RobotSafetyModes.Violation;
                                    }
                                    break;
                                case "fault":
                                    {
                                        safetyMode = RobotSafetyModes.Fault;
                                    }
                                    break;
                                default:
                                    {
                                        safetyMode = RobotSafetyModes.Unknown;
                                        failure = true;
                                    }
                                    break;
                            }

                            if (previousSafetyMode != safetyMode)
                            {
                                record_ = true;
                                previousSafetyMode = safetyMode;
                            }
                        }
                        break;
                    case RobotControllerCommands.RobotMode:
                        {
                            record_ = false;
                            
                            if (message_.Contains("Robotmode: "))
                            {
                                message_ = message_.Replace("Robotmode: ", string.Empty);

                                switch (tokens_[1].ToLower())
                                {
                                    case "no_controller":
                                        {
                                            robotMode = RobotModes.NoController;
                                        }
                                        break;
                                    case "disconnected":
                                        {
                                            robotMode = RobotModes.Disconnected;
                                        }
                                        break;
                                    case "confirm_safety":
                                        {
                                            robotMode = RobotModes.ConfirmSafety;
                                        }
                                        break;
                                    case "booting":
                                        {
                                            robotMode = RobotModes.Booting;
                                        }
                                        break;
                                    case "power_off":
                                        {
                                            robotMode = RobotModes.PowerOff;
                                        }
                                        break;
                                    case "power_on":
                                        {
                                            robotMode = RobotModes.PowerOn;
                                        }
                                        break;
                                    case "idle":
                                        {
                                            robotMode = RobotModes.Idle;
                                        }
                                        break;
                                    case "backdrive":
                                        {
                                            robotMode = RobotModes.Backdrive;
                                        }
                                        break;
                                    case "running":
                                        {
                                            robotMode = RobotModes.Running;
                                        }
                                        break;
                                    default:
                                        {
                                            robotMode = RobotModes.Unknown;
                                            failure = true;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                switch (tokens_[0])
                                {
                                    case "-1":
                                        {
                                            robotMode = RobotModes.NoController;
                                        }
                                        break;
                                    case "0":
                                        {
                                            robotMode = RobotModes.Running;
                                        }
                                        break;
                                    case "1":
                                        {
                                            robotMode = RobotModes.FreeDrive;
                                        }
                                        break;
                                    case "2":
                                        {
                                            robotMode = RobotModes.Ready;
                                        }
                                        break;
                                    case "3":
                                        {
                                            robotMode = RobotModes.Booting;
                                        }
                                        break;
                                    case "4":
                                        {
                                            robotMode = RobotModes.SecurityStop;
                                        }
                                        break;
                                    case "5":
                                        {
                                            robotMode = RobotModes.EmergecyStop;
                                        }
                                        break;
                                    case "6":
                                        {
                                            robotMode = RobotModes.Fault;
                                        }
                                        break;
                                    case "7":
                                        {
                                            robotMode = RobotModes.PowerOff;
                                        }
                                        break;
                                    case "8":
                                        {
                                            robotMode = RobotModes.Disconnected;
                                        }
                                        break;
                                    case "9":
                                        {
                                            robotMode = RobotModes.Shutdown;
                                        }
                                        break;
                                    default:
                                        {
                                            robotMode = RobotModes.Unknown;
                                            failure = true;
                                        }
                                        break;
                                }
                            }

                            if (previousRobotMode != robotMode)
                            {
                                record_ = true;
                                previousRobotMode = robotMode;
                            }
                        }
                        break;
                    case RobotControllerCommands.UnlockProtectiveStop:
                        {
                            if (!message_.ToLower().Contains("protective stop releasing"))
                            {
                                failure = true;
                            }
                        }
                        break;
                    case RobotControllerCommands.CloseSafetyPopup:
                        {
                            if (message_.ToLower().Contains("closing safety popup"))
                            { 
                                failure = true;
                            }
                        }
                        break;
                    case RobotControllerCommands.LoadInstallation:
                        {
                            if (message_.Contains("File not found:") ||
                                message_.Contains("Failed to load installation:"))
                            {
                                failure = true;
                            }
                        }
                        break;
                    case RobotControllerCommands.RestartSafety:
                        {   // Nothing
                        }
                        break;
                    case RobotControllerCommands.Popup:
                    case RobotControllerCommands.ClosePopup:
                        {
                            if (message_.ToLower().Contains("showing popup") ||
                                message_.ToLower().Contains("closing popup"))
                            {
                                failure = true;
                            }
                        }
                        break;
                    case RobotControllerCommands.Version:
                        {
                            version = message_;
                        }
                        break;
                    case RobotControllerCommands.SetUserRole:
                        {
                            if (message_.Contains("Setting user role:"))
                            {
                                role = message_.Substring(message_.IndexOf(":") + 1, message_.Length - message_.IndexOf(":") - 1).Trim();
                            }
                            else if (message_.Contains("Failed setting user role:") ||
                                message_.Contains("Restricted") ||
                                string.IsNullOrEmpty(message_))
                            {
                                failure = true;
                            }
                        }
                        break;
                    case RobotControllerCommands.AddToLog:
                        {
                            if (message_.Contains("No log message to add"))
                            {
                                failure = true;
                            }
                        }
                        break;
                }

                SetConversationFlag();

                if (record_)
                    FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={message_}");
            }
        }

        protected void FlushMessageQueue()
        {
            RobotControllerMessage message_ = null;

            if (messageQueue.Count > 0)
            {
                lock (messageQueue)
                {
                    while (messageQueue.Count > 0)
                        message_ = messageQueue.Dequeue();
                }
            }
        }

        protected void LoadProperties()
        {
            timeoutOfResponse = Config.TimeoutOfRobotCommunication;
            timeoutOfProgramLoad = Config.TimeoutOfRobotProgramLoad;
            timeoutOfProgramPlay = Config.TimeoutOfRobotProgramPlay;
            timeoutOfAction = Config.TimeoutOfRobotAction;
            timeoutOfMove = Config.TimeoutOfRobotMove;
            timeoutOfHome = Config.TimeoutOfRobotHome;
        }
        #endregion

        #region Public methods
        public void FireCommunicationStateChanged()
        {
            CommunicationStateChanged?.Invoke(this, communicationState);
        }

        public void FireReportRuntimeLog(string text = null)
        {
            ReportRuntimeLog?.Invoke(this, text);
        }

        public void Create(string address, int port)
        {
            LoadProperties();
            serverAddress               = address;
            serverPort                  = port;
            client                      = new AsyncSocketClient(0);
            client.OnConnected          += new AsyncSocketConnectedEventHandler(OnClientConneted);
            client.OnDisconnected       += new AsyncSocketDisconnectedEventHandler(OnClientDisconnected);
            client.OnSent               += new AsyncSocketSentEventHandler(OnClientSent);
            client.OnReceived           += new AsyncSocketReceivedEventHandler(OnClientReceived);
            client.OnError              += new AsyncSocketErrorEventHandler(OnClientFailure);
            CommunicationStateChanged   += (App.MainForm as FormMain).OnChangedRobotControllerCommunicationState;
            ReportRuntimeLog            += (App.MainForm as FormMain).OnReportRobotRuntimeLog;
            ConnectionWatcher           = new System.Timers.Timer();
            ConnectionWatcher.Interval  = 1000;
            ConnectionWatcher.Elapsed   += OnTickWatcher;
            ConnectionWatcher.Start();
        }

        public void TryClientConnecting(string address, int port)
        {
            if (IsConnected)
                return;

            client.Connect(address, port);
            FireCommunicationStateChanged(CommunicationStates.Connecting);
            FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}");
            serverPort      = port;
            serverAddress   = address;
        }

        public void CheckProgramState()
        {
            if (App.MainSequence.OperationState != OperationStates.Initialize)
                SendCommand(RobotControllerCommands.ProgramState);
        }

        public bool GetSentMessage(ref RobotControllerMessage message)
        {
            lock (messageQueue)
            {
                if (messageQueue.Count > 0)
                {
                    message = messageQueue.Dequeue();
                    return true;
                }
            }

            return false;
        }

        public bool IsResponseTimeout(int timeout = 1000)
        {
            if (messageQueue.Count > 0)
            {
                RobotControllerMessage sentMessage_ = messageQueue.Peek();

                if (TimeSpan.FromMilliseconds(App.TickCount - sentMessage_.SentTick).TotalMilliseconds > timeout)
                {   // Remove message from queue.
                    GetSentMessage(ref sentMessage_);
                    failure = true;
                    FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Response Timeout ({sentMessage_})");
                }
            }

            return failure;
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

        public bool SendCommand(string message, string delimiter = "\n", bool record = true)
        {
            bool result = false;

            if (messageQueue.Count <= 0)
            {
                Reset();

                if (client.IsConnected && client.Send(Encoding.Default.GetBytes($"{message}{delimiter}")))
                {
                    RobotControllerMessage sendMessage_ = new RobotControllerMessage(message);
                    sentCommand = sendMessage_.Command;
                    messageQueue.Enqueue(sendMessage_);

                    if (record)
                        FireReportRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={message}");

                    result = true;
                }
            }

            return result;
        }

        public bool SendCommand(RobotControllerCommands command, string arg = null)
        {
            bool wait_ = true;
            bool record = true;
            string commandText_ = string.Empty;

            switch (command)
            {
                case RobotControllerCommands.None:
                    return false;
                case RobotControllerCommands.ProgramState:
                    commandText_ = "programState";
                    record = false;
                    break;
                case RobotControllerCommands.StopProgram:
                    commandText_ = "stop";
                    break;
                case RobotControllerCommands.PauseProgram:
                    commandText_ = "pause";
                    break;
                case RobotControllerCommands.LoadProgram:
                    commandText_ = $"load /programs/{arg}";
                    break;
                case RobotControllerCommands.PlayProgram:
                    commandText_ = "play";
                    break;
                case RobotControllerCommands.QuitProgram:
                    commandText_ = "quit";
                    break;
                case RobotControllerCommands.Shutdown:
                    commandText_ = "shutdown";
                    break;
                case RobotControllerCommands.PowerOn:
                    commandText_ = "power on";
                    break;
                case RobotControllerCommands.PowerOff:
                    commandText_ = "power off";
                    break;
                case RobotControllerCommands.BrakeRelease:
                    commandText_ = "brake release";
                    break;
                case RobotControllerCommands.SafetyMode:
                    commandText_ = "safetymode";
                    record = false;
                    break;
                case RobotControllerCommands.RobotMode:
                    commandText_ = "robotmode";
                    record = false;
                    break;
                case RobotControllerCommands.UnlockProtectiveStop:
                    commandText_ = "unlock protective stop";
                    break;
                case RobotControllerCommands.CloseSafetyPopup:
                    commandText_ = "close safety popup";
                    break;
                case RobotControllerCommands.LoadInstallation:
                    commandText_ = $"load installation {arg}";
                    break;
                case RobotControllerCommands.RestartSafety:
                    commandText_ = "restart safety";
                    break;
                case RobotControllerCommands.Popup:
                    commandText_ = $"popup {arg}";
                    break;
                case RobotControllerCommands.ClosePopup:
                    commandText_ = "close popup";
                    break;
                case RobotControllerCommands.Version:
                    commandText_ = "PolyscopeVersion";
                    break;
                case RobotControllerCommands.SetUserRole:
                    commandText_ = $"setUserRole {arg}";
                    break;
                case RobotControllerCommands.AddToLog:
                    commandText_ = $"addToLog {arg}";
                    break;
            }

            waitResponseFlag = wait_;
            return SendCommand(commandText_, "\n", record);
        }
        #endregion
    }
}
#endregion