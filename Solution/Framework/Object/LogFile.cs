#region Imports
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TechFloor.Object;
#endregion

#region Program
namespace TechFloor.Util
{
    public class LogFile : AbstractClassDisposable
    {
        #region Fields
        protected LogIndexComparerByFileInfo comparer = new LogIndexComparerByFileInfo();
        protected System.Timers.Timer tmr = null;
        protected bool running = false;
        protected int queueId = 0;
        protected int maxSize = 1048576;
        protected int interval = 1000;
        protected string[] date = new string[3];
        protected string prefix = string.Empty;
        protected string filePath = string.Empty;
        protected string filePattern = string.Empty;
        protected ConcurrentQueue<string> queuePrimary = null;
        protected ConcurrentQueue<string> queueSecondary = null;
        #endregion

        #region Properties
        public bool IsEmpty { get { return (queuePrimary.Count <= 0 && queueSecondary.Count <= 0); } }
        public bool IsRun { get { return (tmr != null); } }
        #endregion

        #region Protected methods
        protected virtual void WriteToFile()
        {
            string str = string.Empty;
            string path = string.Empty;
            string log = string.Empty;
            ConcurrentQueue<string> queue = null;

            if (queueId == 0) queue = queueSecondary;
            else queue = queuePrimary;

            do
            {
                if (queue.TryDequeue(out str))
                {
                    if (filePath != (path = SearchLogFile(str.Substring(0, 10).Split('-'))))
                    {
                        if (string.IsNullOrEmpty(filePath))
                        {
                            log = string.Concat(log, string.Format("{0}\r\n", str.Substring(10)));
                            filePath = path;
                            continue;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(log))
                                log = string.Concat(log, string.Format("{0}\r\n", str.Substring(10)));
                            else
                            {
                                Write(log);
                                log = string.Empty;
                            }

                            filePath = path;
                        }

                        break;
                    }
                    else
                        log = string.Concat(log, string.Format("{0}\r\n", str.Substring(10)));
                }
            } while (queue.Count > 0);

            if (!string.IsNullOrEmpty(log))
                Write(log);

            Interlocked.Exchange(ref queueId, queueId == 0 ? 1 : 0);
        }

        protected virtual void Destroy()
        {
            try
            {
                if (tmr != null)
                {
                    running = false;
                    tmr.Dispose();
                    tmr = null;
                }
            }
            finally
            {
                WriteToFile();
                WriteToFile();
            }
        }

        protected virtual char PriorityToString(Logger.Priority priority)
        {
            switch (priority)
            {
                case Logger.Priority.Fatal: return 'F';
                case Logger.Priority.Critical: return 'C';
                case Logger.Priority.Error: return 'E';
                case Logger.Priority.Warning: return 'W';
                case Logger.Priority.Notice: return 'N';
                case Logger.Priority.Information: return 'I';
                case Logger.Priority.Debug: return 'D';
                case Logger.Priority.Trace: return 'T';
                default: return ' ';
            }
        }

        protected virtual void OnFlush(object sender, System.Timers.ElapsedEventArgs e)
        {
            tmr.Enabled = false;

            try
            {
                if (running)
                    WriteToFile();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
            finally
            {
                if (tmr != null)
                    tmr.Enabled = running;
            }
        }

        protected virtual string SearchLogFile(string[] date)
        {
            string path = string.Empty;

            if (this.date != date)
            {
                path = string.Format("{0}\\Log\\{1}\\{2}", (string.IsNullOrEmpty(Logger.RootPath) ? Directory.GetCurrentDirectory() : Logger.RootPath), date[0], date[1]);
                
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                filePattern = string.Empty;
            }

            if (string.IsNullOrEmpty(filePattern))
                filePattern = string.Format("{0}{1}_{2}_*.log", prefix, Process.GetCurrentProcess().ProcessName, date[2]);

            uint id = 1;
            FileInfo[] files = new DirectoryInfo(path).GetFiles(filePattern);

            if (files.Length > 0)
            {
                Array.Sort(files, comparer);
                FileInfo latestfile = files.Last();
                id = UInt32.Parse(latestfile.FullName.Remove(0, latestfile.FullName.LastIndexOf("_") + 1).Replace(".log", string.Empty));

                if (latestfile.Length < maxSize)
                    return latestfile.FullName;
                else
                    id++;
            }

            this.date = date;
            return string.Format("{0}\\{1}{2}_{3}_{4}.log", path, prefix, Process.GetCurrentProcess().ProcessName, date[2], id);
        }

        protected virtual void Write(string content)
        {
            if (string.IsNullOrEmpty(content))
                return;

            using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(content);
                    sw.Flush();
                }
            }
        }
        #endregion

        #region Constructors
        public LogFile(string prefix= null, int interval = 1, int maxsize = 1048576)
        {
            if (prefix != string.Empty)
                this.prefix = string.Format("[{0}]", prefix);

            maxSize = maxsize;
            queuePrimary = new ConcurrentQueue<string>();
            queueSecondary = new ConcurrentQueue<string>();
            tmr = new System.Timers.Timer(this.interval = Math.Max(interval, 1000));
            tmr.Elapsed += OnFlush;
            tmr.Enabled = true;
            tmr.AutoReset = false;
            running = true;
        }
        #endregion

        #region IDisposable Support
        protected override void DisposeUnmanagedObjects()
        {
            Destroy();
        }
        #endregion

        #region Public methods
        public virtual void Write(DateTime dt, int tid, string content, string category, Logger.Priority priority, string path, int line)
        {
            string log = string.Empty;

            if (tid == 0) log = string.Format("{0}{1}{2} ", dt.ToString("yyyy-MM-dd"), PriorityToString(priority), dt.ToString("HH:mm:ss.fff"));
            else log = string.Format("{0}{1}{2} ({3,3}) ", dt.ToString("yyyy-MM-dd"), PriorityToString(priority), dt.ToString("HH:mm:ss.fff"), tid);

            if (!string.IsNullOrEmpty(path) && path.Length > 0)
            {
                if (path.Length > 26) log = string.Concat(log, string.Format("...{0}:{1,-4} ", path.Substring(path.Length - 23), line));
                else log = string.Concat(log, string.Format("{0,-26}:{1,-4} ", path, line));
            }

            if (!string.IsNullOrEmpty(category))
            {
                if (category.Length > 12) log = string.Concat(log, string.Format("[{0}] ", category.Substring(0, 12)));
                else log = string.Concat(log, string.Format("[{0,-12}] ", category));
            }

            if (queueId == 0) queuePrimary.Enqueue(string.Concat(log, content));
            else queueSecondary.Enqueue(string.Concat(log, content));
        }
        #endregion
    }
}
#endregion