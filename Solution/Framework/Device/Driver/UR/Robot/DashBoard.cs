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
/// project UniversalRobot 
/// @author Marcus (isadrastea.kor@gmail.com)
/// @namespace Marcus.Solution.TechFloor.Device.Driver.UR.Robot
/// @file DashBoard.cs
/// @brief
/// @details
/// @date 2019, 1, 24, 오후 1:34
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using Marcus.Solution.TechFloor.Object;
using Marcus.Solution.TechFloor.Util;
using Marcus.Solution.TechFloor.Device.CommunicationIo.EthernetIo;
using Marcus.Solution.TechFloor.Device.CommunicationIo;
#endregion

#region Program
namespace Marcus.Solution.TechFloor.Device.Driver.UR.Robot
{
    public class DashBoardState : AbstractClassDisposable
    {
        #region Enumerations
        public enum DashBoardConversationStates : int
        {
            Error = -1,
            None = 0,
            Prepare,
            Proceed,
            Post
        }
        #endregion

        #region Fields
        protected bool busyFlag;
        protected bool errorFlag;
        protected DashBoardConversationStates conversationState = DashBoardConversationStates.None;
        protected SafetyModes safetyMode = SafetyModes.UndefinedSafetyMode;
        protected ProgramStates programState = ProgramStates.Stopped;
        protected ErrorCode errorCode = ErrorCode.None;

        public bool Running; // Running or Stop
        public bool Pause; // Paused state
        public bool Acknowledge;
        public bool Received;
        public int RobotId;
        public string Address;
        public string Message;
        public string ProgramName;
        #endregion

        #region Properties
        public DashBoardConversationStates ConversationState
        {
            get => conversationState;
            set
            {
                if (conversationState != value)
                {
                    conversationState = value;
                    FireConversationStateChanged();
                }
            }
        }

        public bool Busy
        {
            get => busyFlag;
            set
            {
                if (busyFlag != value)
                {
                    busyFlag = value;
                    FireBusyStateChanged();
                }
            }
        }

        public bool Error
        {
            get => errorFlag;
            set
            {
                if (errorFlag != value)
                {
                    errorFlag = value;
                    FireAlarmStateChanged(errorCode);
                }
            }
        }

        public ErrorCode Code
        {
            get => errorCode;
            set
            {
                if (errorCode != value)
                    errorCode = value;
            }
        }

        public ProgramStates ProgramState
        {
            get => programState;
            set
            {
                if (programState != value)
                {
                    programState = value;
                    FireProgramStateChanged();
                }
            }
        }

        public SafetyModes SafetyMode
        {
            get => safetyMode;
            set
            {
                if (safetyMode != value)
                {
                    safetyMode = value;
                    FireSafetyModeChanged();
                }
            }
        }
        #endregion

        #region Events
        public event EventHandler BusyStateChanged;
        public event EventHandler ConversationStateChanged;
        public event EventHandler StateChanged;
        public event EventHandler ProgramStateChanged;
        public event EventHandler SafetyModeChanged;
        public event EventHandler<ErrorCode> AlarmStateChanged;
        #endregion

        #region Public methods

        #region Event handling methods
        public void FireBusyStateChanged()
        {
            BusyStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void FireConversationStateChanged()
        {
            ConversationStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void FireStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void FireProgramStateChanged()
        {
            ProgramStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void FireSafetyModeChanged()
        {
            SafetyModeChanged?.Invoke(this, EventArgs.Empty);
        }

        public void FireAlarmStateChanged(ErrorCode code)
        {
            AlarmStateChanged?.Invoke(this, code);
        }
        #endregion

        #region Copy methods
        public void CopyFrom(DashBoardState src)
        {
            RobotId = src.RobotId;
            ProgramState = src.ProgramState;
            SafetyMode = src.SafetyMode;
            ConversationState = src.ConversationState;
            Busy = src.Busy;
            Error = src.Error;
            Message = src.Message;
            Code = src.Code;
            Running = src.Running;
            Pause = src.Pause;
            Acknowledge = src.Acknowledge;
            Received = src.Received;
        }
        #endregion

        #endregion
    }

    public class DashBoard : AbstractClassEthernetIo
    {
        #region Constants
        protected const int CONST_SIMULATION_DELAY = 300;
        protected const int CONST_MINIMUM_DATA_LENGTH = 16;
        protected const string delimiter = "\n";
        #endregion

        #region Enumerations
        public enum DashBoardCommands
        {
            Error = -1,
            None = 0,
            GetPolyscopeVersion,
            GetProgramState,
            GetRobotMode,
            GetSafetyMode,
            GetLoadedProgram,
            IsProgramSaved,
            IsRunning,
            AddLog,
            BreakRelease,
            ClosePopup,
            CloseSafetyPopup,
            LoadProgram,
            LoadInstallation,
            Pause,
            Play,
            Popup,
            PowerOff,
            PowerOn,
            Shutdown,
            Stop,
            Quit,
            UnlockProtectiveStop,
            Emergency = 99
        }

        public enum CommunicationProcessStep
        {
            Failure = -1,
            Ready = 0,
            Send = 1,
            Received = 2,
            Completed = 3,
            Timeout = 4
        }
        #endregion

        #region Fields
        protected bool stopThread = false;
        protected bool stateChanged = false;
        protected int homeTimeout = 60000;
        protected int queryTimout = 30000;
        protected int lastPollingTick = 0;
        protected string lastCommand = string.Empty;
        protected object thisLock = new object();
        protected CommunicationProcessStep dashBoardStep = CommunicationProcessStep.Ready;
        protected DateTime lastQueryTime = DateTime.MinValue;
        protected Thread thread = null;
        protected DashBoardState dashBoardState = new DashBoardState();
        protected List<Pair<DashBoardCommands, string>> pendingCommands = new List<Pair<DashBoardCommands, string>>();
        protected Dictionary<DashBoardCommands, string> dashBoardCommandTable = new Dictionary<DashBoardCommands, string>()
        {
            { DashBoardCommands.GetPolyscopeVersion,  "PolyscopeVersion" }, //commandGetPolyscopeVersion },
            { DashBoardCommands.GetProgramState,      "programState" }, //commandGetProgramState },
            { DashBoardCommands.GetRobotMode,         "robotmode" }, //commandGetRobotMode },
            { DashBoardCommands.GetSafetyMode,        "safetymode" },
            { DashBoardCommands.GetLoadedProgram,     "get loaded program" },
            { DashBoardCommands.IsProgramSaved,       "isProgramSaved" },
            { DashBoardCommands.IsRunning,            "running" },
            { DashBoardCommands.AddLog,               "addToLog" },
            { DashBoardCommands.BreakRelease,         "break release" },
            { DashBoardCommands.ClosePopup,           "close popup" },
            { DashBoardCommands.CloseSafetyPopup,     "close safety popup" },
            { DashBoardCommands.LoadProgram,          "load" },
            { DashBoardCommands.LoadInstallation,     "load installation" },
            { DashBoardCommands.Pause,                "pause" },
            { DashBoardCommands.Play,                 "play" },
            { DashBoardCommands.Popup,                "popup" },
            { DashBoardCommands.PowerOff,             "power off" },
            { DashBoardCommands.PowerOn,              "power on" },
            { DashBoardCommands.Shutdown,             "shutdown" },
            { DashBoardCommands.Stop,                 "stop" },
            { DashBoardCommands.Quit,                 "quit" },
            { DashBoardCommands.UnlockProtectiveStop, "unlock protective stop" }
        };
        #endregion

        #region Properties
        public virtual bool IsArrived => dashBoardState.Acknowledge;
        public virtual bool IsReceived => dashBoardState.Received;
        public virtual bool IsBusy => dashBoardState.Busy;
        public virtual bool IsFailure => dashBoardState.Error;
        public virtual bool IsAlive => !stopThread;
        public virtual ProgramStates ProgramState => dashBoardState.ProgramState;
        public virtual SafetyModes SafetyMode => dashBoardState.SafetyMode;
        #endregion

        #region Constructors
        public DashBoard(string ip, int port = 29999, int timeout = 1000) : base(ip, port, timeout)
        {
            CreatePeriodicTask();
        }
        #endregion

        #region Public methods
        public bool GetVersion()
        {
            if (pendingCommands.Count > 0)
                return false;

            pendingCommands.Add(new Pair<DashBoardCommands, string>(
                SetStateFlag(DashBoardCommands.GetPolyscopeVersion),
                string.Empty));
            return true;
        }
        #endregion

        #region Protected methods
        protected override void DisposeManagedObjects()
        {
            Terminate();
            DestroyPeriodicTask();
            base.DisposeManagedObjects();

            if (dashBoardState != null)
                dashBoardState.Dispose();
        }

        protected override void CreatePeriodicTask(System.Timers.ElapsedEventHandler func = null)
        {
            DestroyPeriodicTask();

            if (thread == null)
            {
                stopThread = false;
                thread = new Thread(DashBoardProcess);
                thread.Start();
            }
        }

        protected override void DestroyPeriodicTask()
        {
            if (thread != null)
            {
                stopThread = true;
                thread.Join();
                thread = null;
            }
        }

        protected virtual void DashBoardProcess()
        {
            queryInterval *= 3;

            while (!stopThread)
            {
                try
                {
                    if (!simulation)
                    {
                        if (!IsOpen)
                        {
                            // Wait automatic reconnect
                            Thread.Sleep(queryInterval);
                            continue;
                        }
                        else
                        {
                            lock (thisLock)
                            {
                                if (pendingCommands.Count <= 0)
                                {   // Status polling is not necessary.
                                    // if (IsArrived == IsReceived && (Environment.TickCount - lastPollingTick) >= queryInterval)
                                    //     pendingCommands.Add(new Pair<DashBoardCommands, string>(DashBoardCommands.Status, commandStatus));
                                }
                                else
                                {
                                    switch (dashBoardStep)
                                    {
                                        case CommunicationProcessStep.Ready:
                                        case CommunicationProcessStep.Send:
                                            {
                                                switch (SetStateFlag(pendingCommands[0].first))
                                                {
                                                    case DashBoardCommands.None:
                                                        break;
                                                    case DashBoardCommands.BreakRelease:
                                                    case DashBoardCommands.ClosePopup:
                                                    case DashBoardCommands.CloseSafetyPopup:
                                                    case DashBoardCommands.Pause:
                                                    case DashBoardCommands.Play:
                                                    case DashBoardCommands.PowerOff:
                                                    case DashBoardCommands.PowerOn:
                                                    case DashBoardCommands.Shutdown:
                                                    case DashBoardCommands.Stop:
                                                    case DashBoardCommands.Quit:
                                                    case DashBoardCommands.UnlockProtectiveStop:
                                                    case DashBoardCommands.GetPolyscopeVersion:
                                                    case DashBoardCommands.GetLoadedProgram:
                                                    case DashBoardCommands.GetProgramState:
                                                    case DashBoardCommands.GetRobotMode:
                                                    case DashBoardCommands.GetSafetyMode:
                                                    case DashBoardCommands.IsProgramSaved:
                                                    case DashBoardCommands.IsRunning:
                                                    case DashBoardCommands.AddLog:
                                                    case DashBoardCommands.LoadProgram:
                                                    case DashBoardCommands.LoadInstallation:
                                                        {
                                                            dashBoardState.Busy = true;

                                                            if (SendCommand(pendingCommands[0]))
                                                                dashBoardStep = CommunicationProcessStep.Completed;
                                                        }
                                                        break;
                                                    case DashBoardCommands.Emergency:
                                                        break;
                                                }

                                                lastQueryTime = DateTime.Now;
                                            }
                                            break;
                                        case CommunicationProcessStep.Received:
                                        case CommunicationProcessStep.Completed:
                                            {
                                                if (dashBoardState.Error)
                                                {   // Have to clean up by manually
                                                    dashBoardStep = CommunicationProcessStep.Failure;
                                                }
                                                else
                                                {
                                                    if (((TimeSpan)(DateTime.Now - lastQueryTime)).TotalMilliseconds > queryTimout)
                                                    {
                                                        stateChanged = true;
                                                        dashBoardState.Error = true;
                                                        dashBoardState.Code = ErrorCode.ResponseTimeout;
                                                        pendingCommands.Clear();
                                                        dashBoardStep = CommunicationProcessStep.Timeout;
                                                    }
                                                    else
                                                    {
                                                        if (dashBoardState.Acknowledge || dashBoardState.Received)
                                                        {
                                                            if (pendingCommands.Count > 0)
                                                            {
                                                                switch (SetStateFlag(pendingCommands[0].first))
                                                                {
                                                                    case DashBoardCommands.GetProgramState:
                                                                        {
                                                                            // Noitify
                                                                        }
                                                                        break;
                                                                    case DashBoardCommands.GetSafetyMode:
                                                                        {
                                                                            // Notify
                                                                        }
                                                                        break;
                                                                }

                                                                pendingCommands.RemoveAt(0);
                                                            }

                                                            lastPollingTick = Environment.TickCount;
                                                            dashBoardStep = CommunicationProcessStep.Ready;
                                                        }
                                                    }
                                                }

                                                if (stateChanged)
                                                    dashBoardState.FireStateChanged();
                                            }
                                            break;
                                        case CommunicationProcessStep.Timeout:
                                        case CommunicationProcessStep.Failure:
                                            {
                                                lastQueryTime = DateTime.MinValue;
                                                SetStateFlag(DashBoardCommands.Error);
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
                    Debug.WriteLine(string.Format($"{GetType()}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}"));
                }

                Thread.Sleep(pollingInterval);
            }
        }

        protected virtual bool SendCommand(Pair<DashBoardCommands, string> command, bool attachCr = true, int retry = -1)
        {
            bool result = false;

            try
            {
                Decoded = false;
                dashBoardState.Acknowledge = false;
                dashBoardState.Received = false;

                if (IsOpen)
                {
                    using (CommunicationIoEventArgs arg = new CommunicationIoEventArgs(Convert.ToInt32(command.first), command.second))
                    {
                        if (string.IsNullOrEmpty(command.second))
                            lastCommand = attachCr ? string.Concat(dashBoardCommandTable[command.first], (char)AsciiControlCharacters.Lf) : dashBoardCommandTable[command.first];
                        else
                            lastCommand = attachCr ? string.Format($"{dashBoardCommandTable[command.first]} {command.second}{(char)AsciiControlCharacters.Lf}") :
                                string.Format($"{dashBoardCommandTable[command.first]} {command.second}");

                        result = Send(lastCommand, attachCr);
                        Debug.WriteLine(string.Format("{0} Tx: {1}/{2}> {3}", GetType().Name, DateTime.Now.ToString("HH:mm:ss.fff"), ethernetPortSettings.Port, command.second)); 
                        FireSent(arg);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format($"{GetType()}.{MethodBase.GetCurrentMethod()}: ({command.first},{command.second}) Exception={ex.Message}"));
                
                if (retry != 0 && !stopThread)
                {
                    Thread.Sleep(pollingInterval);
                    return SendCommand(command, attachCr, retry--);
                }
            }

            return result;
        }

        protected virtual DashBoardCommands SetStateFlag(DashBoardCommands command)
        {
            switch (command)
            {
                default:
                case DashBoardCommands.Error:
                    {
                        dashBoardState.ConversationState = DashBoardState.DashBoardConversationStates.Error;
                    }
                    break;
                case DashBoardCommands.None:
                    break;
                case DashBoardCommands.BreakRelease:
                case DashBoardCommands.ClosePopup:
                case DashBoardCommands.CloseSafetyPopup:
                case DashBoardCommands.Pause:
                case DashBoardCommands.Play:
                case DashBoardCommands.PowerOff:
                case DashBoardCommands.PowerOn:
                case DashBoardCommands.Shutdown:
                case DashBoardCommands.Stop:
                case DashBoardCommands.Quit:
                case DashBoardCommands.UnlockProtectiveStop:
                case DashBoardCommands.GetPolyscopeVersion:
                case DashBoardCommands.GetLoadedProgram:
                case DashBoardCommands.GetProgramState:
                case DashBoardCommands.GetRobotMode:
                case DashBoardCommands.GetSafetyMode:
                case DashBoardCommands.IsProgramSaved:
                case DashBoardCommands.IsRunning:
                    {
                        switch (dashBoardState.ConversationState)
                        {
                            default: dashBoardState.ConversationState = DashBoardState.DashBoardConversationStates.Prepare; break;
                            case DashBoardState.DashBoardConversationStates.Prepare: dashBoardState.ConversationState = DashBoardState.DashBoardConversationStates.Proceed; break;
                            case DashBoardState.DashBoardConversationStates.Proceed: dashBoardState.ConversationState = DashBoardState.DashBoardConversationStates.Post; break;
                        }
                    }
                    break;
                case DashBoardCommands.Emergency:
                    break;
            }

            return command;
        }

        protected virtual bool ParseData(string data)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} Rx: {1}/{2}> {3}", GetType().Name, DateTime.Now.ToString("HH:mm:ss.fff"), ethernetPortSettings.Port, data));

                dashBoardState.Acknowledge = true;
                dashBoardState.Received = true;
                return dashBoardState.Acknowledge;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format($"{GetType()}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}"));
                return false;
            }
        }

        protected override void OnReceived(object sender, CommunicationIoEventArgs e)
        {
            try
            {
                Decoded = true;
                ParseData(e.Text);
                // string temp = string.Empty;
                // string[] dump = null;
                // string[] fragments = null;
                // 
                // temp = serialPort.ReadExisting();
                // temp = receivedData + temp;
                // receivedData = string.Empty;
                // ResetQueryTimer();
                // 
                // if (temp.Contains("\r") && temp.Contains("\n"))
                // {
                //     dump = temp.Split(("\n").ToArray(), StringSplitOptions.RemoveEmptyEntries);
                // 
                //     if (!temp.EndsWith("\n"))
                //     {
                //         receivedData = dump[dump.Length - 1];
                //         dump[dump.Length - 1] = string.Empty;
                //     }
                // 
                //     foreach (string str in dump)
                //     {
                //         if (!string.IsNullOrEmpty(str))
                //         {
                //             fragments = str.Split(("\r").ToArray(), StringSplitOptions.RemoveEmptyEntries);
                // 
                //             if (!str.EndsWith("\r"))
                //             {   // CR 이 없는 Data는 쓰레기로 판단하여 버린다.
                //                 fragments[fragments.Length - 1] = string.Empty;
                //             }
                // 
                //             foreach (string fragment in fragments)
                //             {
                //                 if (!string.IsNullOrEmpty(fragment))
                //                 {
                //                     Decoded = true;
                //                     ParseData(fragment.Trim());
                //                 }
                //             }
                //         }
                //     }
                // }
                // else
                // {
                //     if (!temp.Contains("\n"))
                //         receivedData = temp;
                // }
            }
            catch (Exception ex)
            {
                Decoded = false;
                receivedData = string.Empty;
                Debug.WriteLine(string.Format($"{GetType()}.OnReceived: Exception={ex.Message}"));
                // FireFailedToDecode();
            }
        }
        #endregion
    }
}
#endregion