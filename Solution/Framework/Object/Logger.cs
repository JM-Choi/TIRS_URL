#region Imports
using TechFloor.Object;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
#endregion

#region Program
namespace TechFloor.Util
{
    public class Logger : AbstractClassDisposable
    {
        #region Enumerations
        public enum Priority
        {
            Fatal,
            Critical,
            Alarm,
            Error = Alarm,
            Warning,
            Notice,
            Event,
            Record,
            Information,
            Debug,
            Trace
        };
        #endregion

        #region Event Handlers
        public delegate void EventHandler(DateTime time, Priority priority, string category, string log);
        #endregion

        #region Events
        public static event EventHandler WriteLog;
        #endregion

        #region Fields
        private static LogFile file_;
        private static Dictionary<string, LogFile> files_ = new Dictionary<string, LogFile>();
        #endregion

        #region Properties
        public static string RootPath { get; set; }
        #endregion

        #region Constructors
        static Logger()
        {
            file_ = new LogFile("Log");
        }

        private Logger()
        {
        }
        #endregion

        #region Public methods
        public static void AddCategory(string category)
        {
            if (!files_.ContainsKey(category))
                files_.Add(category, new LogFile(category));
        }

        public static void Create(string rootPath= null)
        {
            RootPath = (rootPath == "") ? Directory.GetCurrentDirectory() : rootPath;
        }

        public static void Destroy()
        {
            foreach (LogFile logFile in files_.Values)
            {
                logFile.Dispose();
            }

            file_.Dispose();
        }

        public static bool IsEmpty()
        {
            return file_.IsEmpty;
        }

        public static bool IsRun()
        {
            return file_.IsRun;
        }

        public static void Write(string log, string category = null, Priority priority = Priority.Information, [CallerFilePath] string filePath= null, [CallerLineNumber] int line = 0)
        {
            DateTime dt = DateTime.Now;
            int tid = Thread.CurrentThread.ManagedThreadId;

            try
            {
                if (!string.IsNullOrEmpty(category) && files_.ContainsKey(category) && priority <= Priority.Information)
                    files_[category].Write(dt, 0, log, string.Empty, priority, string.Empty, 0);
                else
                    file_.Write(dt, tid, log, category, priority, filePath, line);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Logger.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            WriteLog?.Invoke(dt, priority, category, log);
        }

        public static void Fatal(string log, string category = "Fatal", [CallerFilePath] string filePath= null, [CallerLineNumber] int line = 0)
        {
            Write(log, category, Priority.Fatal, filePath, line);
        }

        public static void Critical(string log, string category = "Critical", [CallerFilePath] string filePath= null, [CallerLineNumber] int line = 0)
        {
            Write(log, category, Priority.Critical, filePath, line);
        }

        public static void Alarm(string log, string category = "Alarm", [CallerFilePath] string filePath= null, [CallerLineNumber] int line = 0)
        {
            Write(log, category, Priority.Error, filePath, line);
        }

        public static void Error(string log, string category = "Error", [CallerFilePath] string filePath= null, [CallerLineNumber] int line = 0)
        {
            Write(log, category, Priority.Error, filePath, line);
        }

        public static void Warning(string log, string category = "Warning", [CallerFilePath] string filePath= null, [CallerLineNumber] int line = 0)
        {
            Write(log, category, Priority.Warning, filePath, line);
        }

        public static void Notice(string log, string category = "Notice", [CallerFilePath] string filePath= null, [CallerLineNumber] int line = 0)
        {
            Write(log, category, Priority.Notice, filePath, line);
        }

        public static void Event(string log, string category = "Event", [CallerFilePath] string filePath= null, [CallerLineNumber] int line = 0)
        {
            Write(log, category, Priority.Event, filePath, line);
        }

        public static void Information(string log, string category = "Information", [CallerFilePath] string filePath= null, [CallerLineNumber] int line = 0)
        {
            Write(log, category, Priority.Information, filePath, line);
        }

        public static void Debug(string log, string category = "Debug", [CallerFilePath] string filePath= null, [CallerLineNumber] int line = 0)
        {
            Write(log, category, Priority.Debug, filePath, line);
        }

        public static void Trace(string log, string category = "Trace", [CallerFilePath] string filePath= null, [CallerLineNumber] int line = 0)
        {
            Write(log, category, Priority.Trace, filePath, line);
        }

        public static void Record(string log, string category = "Record", [CallerFilePath] string filePath= null, [CallerLineNumber] int line = 0)
        {
            Write(log, category, Priority.Record, filePath, line);
        }

        public static void TraceKeyAndMouseEvent(Control src, EventArgs e, string log= null, string category= null, string filePath= null, int line = 0)
        {
            if (e is MouseEventArgs)
                Write(string.Format("Control={0},{1},{2}", src.Name, (e as MouseEventArgs).Button.ToString(), ((e as MouseEventArgs).Clicks < 2) ? "Clicked" : "Double clicked", log), category, Priority.Trace, filePath, line);
            else if (e is KeyEventArgs)
                Write(string.Format("Control={0},{1},{2}", src.Name, (e as KeyEventArgs).KeyCode.ToString(), "Pressed", log), category, Priority.Trace, filePath, line);

        }

        public static void ProcessTrace(string module, string step, string log, string category = "Process", [CallerFilePath] string filePath = null, int line = 0)
        {
            Write(string.Format("Module={0},Step={1},{2}", module, step, log), category, Priority.Trace, filePath, line);
        }
        #endregion
    }
}
#endregion