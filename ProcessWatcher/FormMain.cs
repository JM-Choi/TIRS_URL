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
/// project ProcessWatcher
/// @author Marcus (isadrastea.kor@gmail.com)
/// @namespace ProcessWatcher
/// @file Form1.cs
/// @brief
/// @details
/// @date 2020-3-10 오후 1:41 
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using TechFloor.Object;
using TechFloor.Util;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using System.Net.NetworkInformation;
using System.Security.Principal;
#endregion

#region Program
namespace ProcessWatcher
{
    #region Eumerations
    public enum RunModes
    {
        Simulation,
        Normal,
    }

    public enum CommunicationStates
    {
        None,
        Created,
        Listening,
        Connecting,
        Accepted,
        Connected,
        Error,
        Disconnected,
    }
    #endregion

    public partial class FormMain : Form
    {
        #region Constants
        public static readonly string CONST_SERVICE_NAME = "TechFloorService";

        public static readonly string CONST_CONFIG_FILE_PATH = "Config\\ProcessWatcher.xml";

        protected readonly string CONST_TEHFLOOR_REGISTRY = "Software\\TechFloor";
        #endregion

        #region Fields
        protected static System.Timers.Timer watchTimer = null;

        protected readonly ManualResetEvent shutdownEvent = new ManualResetEvent(false);

        protected readonly ManualResetEvent startedEvent = new ManualResetEvent(false);

        protected bool shutdown = false;

        protected bool externalSeviceConnected = false;

        protected bool initialized = false;

        protected bool reportDisconnected = false;

        protected string appPath = string.Empty;

        protected string version = string.Empty;

        protected CommunicationStates state = CommunicationStates.Disconnected;

        protected IPEndPoint endPoint = null;

        protected UdpClient server = null;

        protected StringBuilder receivedMsg = new StringBuilder();

        protected Thread serverThread = null;

        protected Dictionary<string, int> intervals = new Dictionary<string, int>();

        protected Dictionary<string, Pair<DateTime, bool>> elapsed = new Dictionary<string, Pair<DateTime, bool>>();

        private int heartbeat_ = 0;
        #endregion

        #region Properties
        public bool IsNormal => Program.ConfigObject.RunMode == RunModes.Normal;

        public bool IsPossibleReport => externalSeviceConnected;
        #endregion

        #region Structors
        public struct UdpState
        {
            public UdpClient Socket;
            public IPEndPoint EndPoint;
        }
        #endregion

        #region Constructors
        public FormMain(string[] args = null)
        {
            InitializeComponent();
        }
        #endregion

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }

        protected void OnFormLoad(object sender, EventArgs e)
        {
            try
            {
                if (!IsRunningAsAdministrator())
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(Assembly.GetEntryAssembly().CodeBase);
                    {
                        var withBlock = processStartInfo;
                        withBlock.UseShellExecute = true;
                        withBlock.Verb = "runas";
                        shutdown = true;
                        Close();
                        Process.Start(processStartInfo);
                    }
                }
                else
                {
                    this.appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    this.appPath = System.IO.Path.GetDirectoryName(appPath) + "\\";
                    this.version = typeof(Program).Assembly.GetName().Version.ToString();
                    Logger.Create(appPath);

                    if (Program.ConfigObject.Load(version))
                    {
                        if (IsNormal)
                            this.Text += " (Administrator)";
                        else
                            this.Text += "-Simulation (Administrator)";

                        foreach (KeyValuePair<string, Pair<string, int>> item_ in Program.ConfigObject.Processes)
                        {
                            intervals.Add(item_.Key, item_.Value.second);
                            elapsed.Add(item_.Key, new Pair<DateTime, bool>(DateTime.MinValue, false));
                        }

                        if (IsNormal)
                            shutdown = !IsServiceRunning(CONST_SERVICE_NAME);
                    }
                    else
                        shutdown = true;

                    if (shutdown)
                        Close();
                    else
                    {
                        Logger.Trace($"Started ProcessWatcher.");
                        SaveWindowHandle(this.Handle.ToInt32());
                        ConnectToExternalService();
                        watchTimer = new System.Timers.Timer(1000);
                        watchTimer.Elapsed += OnElapsed;
                        watchTimer.AutoReset = false;
                        watchTimer.Start();
                        WindowState = FormWindowState.Minimized;
                        Hide();
                        labelHostAddressValue.Text = Program.ConfigObject.ExternalServiceAddress;
                        labelEquipmentStatusValue.Text = string.Empty;
                        labelAlarmCodeValue.Text = string.Empty;
                        labelAlarmMessageValue.Text = string.Empty;
                        Create();

                        if (!backgroundWorker1.IsBusy)
                            backgroundWorker1.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                shutdown = true;
                Close();
            }
        }

        protected void OnFormShown(object sender, EventArgs e)
        {
            Hide();
        }

        protected void OnFormResize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                Hide();

                if (notifyIcon.BalloonTipTitle == string.Empty)
                    notifyIcon.BalloonTipTitle = "TechFloor Process Watcher";

                if (notifyIcon.BalloonTipText == string.Empty)
                    notifyIcon.BalloonTipText = "The process watcher is running in minimized state.";

                notifyIcon.ShowBalloonTip(3000);
            }
        }

        protected void OnFormHide(object sender, EventArgs e)
        {
            if (FormWindowState.Normal == WindowState)
                Hide();
        }

        protected void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!shutdown)
            {
                e.Cancel = true;
                WindowState = FormWindowState.Minimized;
                Hide();
            }
        }

        protected void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            SaveWindowHandle();
            Destroy();

            if (watchTimer != null)
            {
                watchTimer.Stop();
                watchTimer.Dispose();
            }

            Logger.Destroy();
        }

        protected bool LoadRegistry()
        {
            bool result_ = false;

            try
            {
                RegistryExt obj_ = new RegistryExt(CONST_TEHFLOOR_REGISTRY);

                if (obj_.IsExistSubKey(CONST_TEHFLOOR_REGISTRY))
                    result_ = !string.IsNullOrEmpty(obj_.Read("ConfigFile"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry($"{GetType().Name}", $"{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        protected void SaveWindowHandle(int handle = 0)
        {   
            try
            {
                if (string.IsNullOrEmpty(appPath))
                {
                    appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    appPath = System.IO.Path.GetDirectoryName(appPath) + "\\";
                }

                string file_ = appPath + "handle.lock";

                try
                {                
                    if (File.Exists(file_))
                        File.Delete(file_);

                    if (handle != 0)
                    {
                        using (StreamWriter sw = File.CreateText(file_))
                            sw.WriteLine(handle.ToString());
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry($"{GetType().Name}", $"{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry($"{GetType().Name}", $"{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected bool LoadWindowHandle(ref int handle)
        {
            bool result_ = false;

            try
            {
                string file_ = appPath + "handle.inf";

                try
                {
                    if (File.Exists(file_))
                    {
                        using (StreamReader sw = File.OpenText(file_))
                        {
                            string data_ = sw.ReadLine();
                            handle = Convert.ToInt32(data_);
                            result_ = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry($"{GetType().Name}", $"{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry($"{GetType().Name}", $"{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        protected bool IsRunningAsAdministrator()
        {
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(windowsIdentity);
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static bool IsServiceRunning(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);

            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    return true;
                case ServiceControllerStatus.Stopped:
                case ServiceControllerStatus.Paused:
                case ServiceControllerStatus.StopPending:
                case ServiceControllerStatus.StartPending:
                default:
                    return false;
            }
        }

        public static bool GetServiceStatus(string serviceName, ref string status)
        {
            ServiceController sc = new ServiceController(serviceName);

            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    {
                        status = "Running";
                        return true;
                    }
                case ServiceControllerStatus.Stopped:
                    status = "Stopped";
                    break;
                case ServiceControllerStatus.Paused:
                    status = "Paused";
                    break;
                case ServiceControllerStatus.StopPending:
                    status = "Stopping";
                    break;
                case ServiceControllerStatus.StartPending:
                    status = "Starting";
                    break;
                default:
                    status = "Status Changing";
                    break;
            }

            return false;
        }

        public static ServiceControllerStatus GetServiceStatus(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);
            return sc.Status;
        }

        public static void StartService(string serviceName, int timeoutMilliseconds = 30000)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                
                if (watchTimer != null && !watchTimer.Enabled)
                    watchTimer.Start();
                
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ProcessWatcher.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public static void StopService(string serviceName, int timeoutMilliseconds = 30000)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                if (watchTimer != null)
                    watchTimer.Stop();

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ProcessWatcher.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public static void RestartService(string serviceName, int timeoutMilliseconds = 30000)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                if (watchTimer != null)
                    watchTimer.Stop();

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                if (watchTimer != null && !watchTimer.Enabled)
                    watchTimer.Start();

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ProcessWatcher.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void UpdateEventReportServerConnectionStatus()
        {
            if (checkBoxHostConnectionValue.Checked = externalSeviceConnected)
            {
                checkBoxHostConnectionValue.Text = "CONNECTED";
                checkBoxHostConnectionValue.BackColor = Color.Lime;
                Logger.Trace($"Connected to remote report server. {Program.ConfigObject.ExternalServiceAddress}");
            }
            else
            {
                checkBoxHostConnectionValue.Text = "DISCONNECTED";
                checkBoxHostConnectionValue.BackColor = SystemColors.Control;
                Logger.Trace($"Disconnected from remote report server.");
            }
        }

        protected void ConnectToExternalService()
        {
            try
            {
                if (!externalSeviceConnected)
                {   // UPDATED: 20200902 (Marcus)
                    // Skip ping test.
                    // if (PingHost(Program.ConfigObject.ExternalServiceAddress))
                        externalSeviceConnected = Program.SkynetService.Skynet_Connect(Program.ConfigObject.ExternalServiceAddress);
                }

                if (InvokeRequired)
                    BeginInvoke(new Action(() => { UpdateEventReportServerConnectionStatus(); }));
                else
                    UpdateEventReportServerConnectionStatus();
            }
            catch (Exception ex)
            {
                externalSeviceConnected = false;
                Logger.Trace($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void OnCheckedChangedCheckBoxHostConnectionValue(object sender, EventArgs e)
        {
            ConnectToExternalService();
        }

        protected void OnElapsed(object sender, ElapsedEventArgs e)
        {
            watchTimer.Stop();

#if DEBUG
            Logger.Trace($"Entered process watch timer.");
#endif
            try
            {
                if (!IsPossibleReport)
                    ConnectToExternalService();

                foreach (KeyValuePair<string, int> item_ in intervals)
                {
                    if (elapsed.ContainsKey(item_.Key))
                    {
                        if ((DateTime.Now - elapsed[item_.Key].first).TotalSeconds >= item_.Value)
                        {
                            if (Program.IsProcessRunning(item_.Key))
                            {
                                if (!elapsed[item_.Key].second && (IsPossibleReport || !IsNormal))
                                {
                                    Program.SkynetService.Skynet_PM_Start(
                                        Program.ConfigObject.LineCode,
                                        Program.ConfigObject.ProcessCode,
                                        Program.ConfigObject.EquipmentId);

                                    Logger.Trace($"Started:{Program.ConfigObject.LineCode},{Program.ConfigObject.ProcessCode},{Program.ConfigObject.EquipmentId}");
                                    Start();
                                    elapsed[item_.Key].second = true;
                                }
                            }
                            else
                            {
                                if (elapsed[item_.Key].second && (IsPossibleReport || !IsNormal))
                                {
                                    Program.SkynetService.Skynet_PM_End(
                                        Program.ConfigObject.LineCode,
                                        Program.ConfigObject.ProcessCode,
                                        Program.ConfigObject.EquipmentId);
                                    Logger.Trace($"Stoped:{Program.ConfigObject.LineCode},{Program.ConfigObject.ProcessCode},{Program.ConfigObject.EquipmentId}");
                                    Stop();
                                    elapsed[item_.Key].second = false;
                                }
                            }

                            elapsed[item_.Key].first = DateTime.Now;
                        }
                    }
                }

#if DEBUG
            Logger.Trace($"Exit process watch timer.");
#endif
                watchTimer.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                shutdown = true;
                Close();
            }
        }

        #region Protected methods
        protected void FireReceivedMessage()
        {
            string str_ = receivedMsg.ToString();//.Replace(Environment.NewLine, string.Empty);

            new TaskFactory().StartNew(new Action<object>((x_) =>
            {
                int result_ = 0;

                if (x_ != null && (IsPossibleReport || !IsNormal))
                {
                    string state_ = string.Empty;
                    string alarmcode_ = string.Empty;
                    string alarmmsg_ = string.Empty;
                    string[] tokens_ = x_.ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    if (tokens_.Length > 0)
                    {
                        switch ((state_ = tokens_[0]).ToLower())
                        {
                            case "idle":
                                {
                                    result_ = Program.SkynetService.Skynet_SM_Send_Idle(
                                        Program.ConfigObject.LineCode,
                                        Program.ConfigObject.ProcessCode,
                                        Program.ConfigObject.EquipmentId,
                                        tokens_.Length > 1 ? tokens_[1] : "Complete");
                                }
                                break;
                            case "run":
                                {
                                    result_ = Program.SkynetService.Skynet_SM_Send_Run(
                                        Program.ConfigObject.LineCode,
                                        Program.ConfigObject.ProcessCode,
                                        Program.ConfigObject.EquipmentId,
                                        tokens_.Length > 1 ? tokens_[1] : "Complete");
                                }
                                break;
                            case "move":
                                {
                                    if (tokens_.Length > 2)
                                    {
                                        result_ = Program.SkynetService.Skynet_SM_Send_Run(
                                        Program.ConfigObject.LineCode,
                                        Program.ConfigObject.ProcessCode,
                                        Program.ConfigObject.EquipmentId,
                                        tokens_.Length > 1 ? tokens_[1] : "Complete",
                                        tokens_[1],     // Departure
                                        tokens_[2]);    // Arrival
                                    }
                                }
                                break;
                            case "alarm":
                                {
                                    if (tokens_.Length > 1)
                                    {
                                        AlarmData data_ = Program.AlarmManagerObject.GetAlarmData(Convert.ToInt32(tokens_[1]));

                                        if (data_ != null && data_.RequiredReport)
                                        {
                                            if ((result_ = Program.SkynetService.Skynet_EM_DataSend(
                                                Program.ConfigObject.LineCode,
                                                Program.ConfigObject.ProcessCode,
                                                Program.ConfigObject.EquipmentId,
                                                data_.Extra,
                                                data_.Severity.ToString(),
                                                data_.Name,
                                                data_.Message,
                                                data_.Remedy)) == 1)
                                            {
                                                Program.SkynetService.Skynet_SM_Send_Alarm(
                                                        Program.ConfigObject.LineCode,
                                                        Program.ConfigObject.ProcessCode,
                                                        Program.ConfigObject.EquipmentId,
                                                        tokens_.Length > 1 ? tokens_[1] : "Complete");
                                            }
                                            alarmcode_ = data_.Extra;
                                            alarmmsg_ = data_.Message;
                                        }
                                    }
                                }
                                break;
                        }

                        if (InvokeRequired)
                        {
                            BeginInvoke(new Action(() => {
                                labelEquipmentStatusValue.Text = state_;
                                labelAlarmCodeValue.Text = alarmcode_;
                                labelAlarmMessageValue.Text = alarmmsg_;
                            }));
                        }
                    }
                }

                Logger.Trace(result_ == 1 ? $"Reported> {str_}" : $"Failed Report> {str_}");
            }), str_);
            receivedMsg.Clear();            
        }

        protected void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                string message_ = string.Empty;
                UdpState state_ = new UdpState();
                UdpClient socket_ = ((UdpState)(ar.AsyncState)).Socket;
                IPEndPoint endPoint_ = ((UdpState)(ar.AsyncState)).EndPoint;

                byte[] buf_ = socket_.EndReceive(ar, ref endPoint_);

                if (buf_ != null && buf_.Length > 0)
                {
                    lock (receivedMsg)
                        receivedMsg.Append(message_ = Encoding.ASCII.GetString(buf_, 0, buf_.Length));

                    state_.EndPoint = endPoint;

                    if (Program.ConfigObject.RunMode == RunModes.Simulation)
                        Logger.Trace($"Received from equipment> {message_}");

                    FireReceivedMessage();

                    if (server != null)
                    {
                        state_.Socket = server;
                        state_.EndPoint = endPoint;
                        server.BeginReceive(new AsyncCallback(ReceiveCallback), state_);
                    }
                }
                else
                {
                    if (server != null)
                        server.Close();

                    state = CommunicationStates.Disconnected;
                    reportDisconnected = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");

                if (server != null)
                    server.Close();

                state = CommunicationStates.Error;
                reportDisconnected = true;
            }
            finally
            {
                if (reportDisconnected)
                {
                    if (!shutdownEvent.WaitOne(100))
                    {
                        Start();
                    }
                }
            }
        }

        protected virtual void Run()
        {
            try
            {
                UdpState state_ = new UdpState();
                state_.EndPoint = endPoint;
                state_.Socket = server;

                state = CommunicationStates.Created;

                if (server != null)
                {
                    server.BeginReceive(new AsyncCallback(ReceiveCallback), state_);
#if DEBUG
                    Logger.Trace($"Wait for a message from equipment!");
#endif
                }

                state = CommunicationStates.Listening;
                startedEvent.Set();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
            finally
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: State={state}");
            }
        }

        protected void OnDoubleClicked(object sender, EventArgs e)
        {
            CenterToScreen();
            Show();
            WindowState = FormWindowState.Normal;
        }

        protected void OnHide(object sender, EventArgs e)
        {
            if (FormWindowState.Normal == WindowState)
                Hide();
        }

        protected void OnExitApplication(object sender, EventArgs e)
        {
            if (!shutdown)
            {
                if (MessageBox.Show("Do you want to exit the program ?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            shutdown = true;
            Close();
        }

        protected void OnConfig(object sender, EventArgs e)
        {
            new FormConfig().ShowDialog(this);
        }

        protected bool PingHost(string address)
        {
            bool pingable_ = false;
            Ping pinger_ = null;

            try
            {
                pinger_ = new Ping();
                PingReply reply_ = pinger_.Send(address);
                pingable_ = reply_.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger_ != null)
                {
                    pinger_.Dispose();
                }
            }

            return pingable_;
        }
#endregion

#region Public methods
        public virtual bool Create()
        {
            initialized = false;

            if (server == null)
            {
                endPoint = new IPEndPoint(
                Program.ConfigObject.ListenerAddress == "127.0.0.1" ? IPAddress.Any : IPAddress.Parse(Program.ConfigObject.ListenerAddress),
                Program.ConfigObject.ListenerPort);
                server = new UdpClient(endPoint);
                Logger.Trace($"Started listener: Address={Program.ConfigObject.ListenerAddress},Port={Program.ConfigObject.ListenerPort}");
            }

            return initialized = true;
        }

        public virtual void Destroy()
        {
            if (server != null)
            {
                server.Dispose();
                server = null;
                Logger.Trace($"Stop listener and terminate.");
            }

            Stop();
            StopService(CONST_SERVICE_NAME);
            initialized = false;
        }

        public virtual CommunicationStates Start(int timeout = 1000)
        {
            if (shutdownEvent != null)
            {
                startedEvent.Reset();
                shutdownEvent.Reset();
                serverThread = new Thread(new ThreadStart(Run));
                serverThread.Start();
                WaitForStart(timeout);
            }

            return state;
        }

        public virtual CommunicationStates Stop(int timeout = 1000)
        {
            if (shutdownEvent != null)
                shutdownEvent.Set();

            return state;
        }

        public virtual CommunicationStates WaitForStart(int timeout = 1000)
        {
            startedEvent.WaitOne(timeout);
            return state;
        }

        public virtual string GetMessage()
        {
            string result_ = string.Empty;

            if (receivedMsg.Length > 0)
            {
                lock (receivedMsg)
                {
                    result_ = receivedMsg.ToString();
                    receivedMsg.Clear();
                }
            }

            return result_;
        }
        #endregion

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        if (checkBoxHostConnectionValue.Text != "DISCONNECTED")
                                Program.SkynetService.Skynet_SM_Alive(
                                                    Program.ConfigObject.LineCode,
                                                    Program.ConfigObject.ProcessCode,
                                                    Program.ConfigObject.EquipmentId,
                                                    ++heartbeat_ % 2
                                                    );
                            

                        
                    }));
                }
                else
                {
                    //Send heart beat
                    
                        if (checkBoxHostConnectionValue.Text != "DISCONNECTED")
                        {
                            Program.SkynetService.Skynet_SM_Alive(
                                                Program.ConfigObject.LineCode,
                                                Program.ConfigObject.ProcessCode,
                                                Program.ConfigObject.EquipmentId,
                                                ++heartbeat_ % 2
                                                );
                        }
                    
                }
                Thread.Sleep(30000);
            }
        }
    }
}
#endregion