#region Imports
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Threading;
using System.Windows.Forms;
using TechFloor.Gui;
using TechFloor.Object;
#endregion

#region Program
#pragma warning disable CS0414
namespace TechFloor
{
    public class App
    {
        #region Fields
        private readonly string appGuid_ = "{36CCC42E-2F24-4B35-935B-B2AE2089931E}";
        private readonly string appKey_ = string.Empty;
        private readonly Mutex appLock_ = null;
        private static bool appReady = false;

        private static IFormMain formMain = null;

        public static int DataLoad = 0;
        #endregion

        #region Properties
        public static string Name
        {
            get;
            protected set;
        }

        public static string Version
        {
            get;
            protected set;
        }

        public static string CultureInfoCode
        {
            get;
            protected set;
        }

        public static int TickCount => Environment.TickCount & int.MaxValue;

        public static bool Initialized
        {
            get
            {
                try
                {
                    return (formMain != null ? formMain.MainSequence.Initialized : false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Debug> TechFloor.App: Exception={ex.Message}");
                    return false;
                }
            }
        }

        public static bool CycleStop
        {
            get
            {
                try
                {
                    return (formMain != null ? formMain.MainSequence.CycleStop : false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Debug> TechFloor.App: Exception={ex.Message}");
                    return false;
                }
            }
        }

        public static string Path => GetApplicationBaseDirectory();

        public static string LogPath { get; set; }

        public static IFormMain MainForm => formMain;

        public static IDigitalIoManager DigitalIoManager
        {
            get
            {
                try
                {
                    return (formMain != null ? formMain.DigitalIoManager : null);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Debug> TechFloor.App: Exception={ex.Message}");
                    return null;
                }
            }
        }

        public static IMainSequence MainSequence
        {
            get
            {
                try
                {
                    return (formMain != null ? formMain.MainSequence : null);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Debug> TechFloor.App: Exception={ex.Message}");
                    return null;
                }
            }
        }

        public static OperationStates OperationState
        {
            get
            {
                try
                {
                    return (formMain != null ? formMain.OperationState : OperationStates.Alarm);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Debug> TechFloor.App: Exception={ex.Message}");
                    return OperationStates.Alarm;
                }
            }
        }

        public static WaitHandle ShutdownEvent
        {
            get
            {
                try
                {
                    return (formMain != null ? formMain.ShutdownEvent : null);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Debug> TechFloor.App: Exception={ex.Message}");
                    return null;
                }
            }
        }

        public static bool Start()
        {
            try
            {
                return (formMain != null ? formMain.MainSequence.Start() : false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug> TechFloor.App: Exception={ex.Message}");
                return false;
            }
        }

        public static bool Stop()
        {
            try
            {
                return (formMain != null ? formMain.MainSequence.Stop() : false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug> TechFloor.App: Exception={ex.Message}");
                return false;
            }
        }

        public static void TryCycleStop(bool state)
        {
            try
            {
                formMain.MainSequence.TryCycleStop(state);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug> TechFloor.App: Exception={ex.Message}");
            }
        }
        #endregion

        #region Constructors
        public App(Assembly module = null, bool simulation = false)
        {
            if (simulation)
            {
                appKey_ = string.Format("{0}@TECHFLOOR@SIMULATION@{1}", GetApplicationTag(module), appGuid_);
                appLock_ = new Mutex(true, appKey_, out appReady);
            }
            else
            {
                appKey_ = string.Format("{0}@TECHFLOOR@{1}", GetApplicationTag(module), appGuid_);
                appLock_ = new Mutex(true, appKey_, out appReady);
            }
        }
        #endregion

        #region Protected methods
        protected virtual void AppRun(IFormMain obj)
        {
            if (typeof(IFormMain).IsAssignableFrom(obj.GetType()))
                Application.Run((Form)(formMain = obj));
            else
                MessageBox.Show("Main form is not properly declared.");
        }
        #endregion

        #region Public methods
        public static string GetApplicationTag(Assembly module)
        {
            Name = GetApplicationName(module);
            Version = GetApplicationVersion(module);
            return string.Format($"{Name}@{Version}");
        }

        public static string GetApplicationName(Assembly module)
        {
            if (module != null)
                return module.GetName().Name;
            return Assembly.GetExecutingAssembly().GetName().Name;
        }

        public static string GetApplicationVersion(Assembly module)
        {
            if (module != null)
                return module.GetName().Version.ToString();
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static string GetApplicationFullName()
        {
            return Assembly.GetExecutingAssembly().GetName().FullName;
        }

        public static string GetApplicationBaseDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static bool IsNumeric(string str)
        {
            return str.All(char.IsNumber);
        }

        public static bool IsDigit(string str)
        {
            foreach (char c_ in str)
            {
                if (!char.IsDigit(c_) && c_ != '.')
                    return false;
            }

            return true;
        }

        public static bool IsLatestVersion(int major, int minor, int build, int revision)
        {
            System.Version currentversion_ = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            if (currentversion_.Major >= major)
            {
                if (currentversion_.Minor >= minor)
                {
                    if (currentversion_.Build >= build)
                        return currentversion_.Revision >= revision;
                }
            }
            
            return false;
        }

        public virtual void Run(IFormMain obj)
        {
            if (appReady)
                AppRun(obj);
            else
            {
                MessageBox.Show("The other process is running.", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public virtual void SetCultureCode(string culturecode, bool forced = true)
        {
            if (string.IsNullOrEmpty(culturecode))
                return;

            try
            {
                CultureInfoCode = culturecode;

                if (forced)
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(CultureInfoCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }
        #endregion
    }
}
#endregion