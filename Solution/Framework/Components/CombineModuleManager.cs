#region Imports
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using TechFloor.Object;
#endregion

#region Program
namespace TechFloor.Components
{
    public class CombineModuleManager
    {
        #region Imports
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible", Justification = "<보류 중>")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible", Justification = "<보류 중>")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible", Justification = "<보류 중>")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible", Justification = "<보류 중>")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, EntryPoint = "SendMessage")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible", Justification = "<보류 중>")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, EntryPoint = "SendMessage")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible", Justification = "<보류 중>")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible", Justification = "<보류 중>")]
        public static extern IntPtr SetParent(IntPtr hWnd, IntPtr hwp);

        [DllImport("user32.dll", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible", Justification = "<보류 중>")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible", Justification = "<보류 중>")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public int Width => Right - Left;
            public int Height => Bottom - Top;
            public Point Center => new Point(Width / 2, Height / 2);
        }

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public UInt32 cbData;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpData;
        }
        #endregion

        #region Constants
        protected const int SW_SHOWNORMAL = 1;
        protected const int SW_SHOWMINIMIZED = 2;
        protected const int SW_SHOWMAXIMIZED = 3;
        protected const int WM_QUIT = 0x0012;
        protected const int WM_CLOSE = 0x0010;
        protected const int WM_COPYDATA = 0x004A;
        protected const int HWND_TOPMOST = -1;
        protected const int HWND_NOTOPMOST = -2;
        protected const int SWP_NOMOVE = 0x0002;
        protected const int SWP_NOSIZE = 0x0001;
        protected const int SWP_NOZORDER = 0x0004;
        protected const int SWP_SHOWWINDOW = 0x0040;
        protected const int WM_SYSCOMMAND = 0x0112;
        protected const int SC_CLOSE = 0xF060;
        #endregion

        #region Fields
        protected Thread threadCombineWatcher = null;

        protected List<CombineModule> modules = new List<CombineModule>();
        #endregion

        #region Properties
        public virtual IReadOnlyList<CombineModule> Modules => modules;
        #endregion

        #region Events
        public virtual event EventHandler<Pair<bool, bool>> NotifyAllModulesCombined;
        #endregion

        #region Deligates
        #endregion

        #region Constructors
        protected CombineModuleManager() { }
        #endregion

        #region Protected methods
        protected virtual void RunCombineWatcher(CombineModule module = null)
        {
            if (threadCombineWatcher == null)
            {
                threadCombineWatcher = new Thread(new ParameterizedThreadStart(CombineWatcher));
                threadCombineWatcher.Start(module);
            }
        }

        protected virtual void StartProcess(CombineModule module, ref int result)
        {
            if (module == null)
                return;

            if (module.ProcessInfo != null)
            {
                if (CheckProcess(module.Name, true))
                    Thread.Sleep(3000);

                if (module.Start())
                    result++;
            }
        }

        protected virtual void StopProcess(IntPtr hwnd)
        {
            if (hwnd != IntPtr.Zero)
                SendMessage(hwnd, WM_SYSCOMMAND, (IntPtr)SC_CLOSE, IntPtr.Zero);
        }

        protected virtual void StopProcess(CombineModule module)
        {
            if (module != null && module.WndHandle != null)
                SendMessage(module.WndHandle, WM_SYSCOMMAND, (IntPtr)SC_CLOSE, IntPtr.Zero);
        }

        protected virtual void FitWindowToParent(CombineModule module, bool forced = false)
        {
            if (module == null)
                return;

            if (module.WndHandle.Equals(IntPtr.Zero))
                module.WndHandle = FindWindow(null, module.Caption);

            if ((!module.WndHandle.Equals(IntPtr.Zero) && !module.IsStarted) || forced)
            {
                RECT rect_;
                GetWindowRect(module.WndHandle, out rect_);

                Point pt_ = new Point(module.OwnerControl.Left + module.OwnerControl.Width / 2 - rect_.Center.X,
                    module.OwnerControl.Top + module.OwnerControl.Height / 2 - rect_.Center.Y);

                if (rect_.Center.X != module.OwnerControl.Width / 2)
                {
                    if (module.OwnerControl.InvokeRequired)
                    {
                        module.OwnerControl.BeginInvoke(new Action(() =>
                        {
                            SetParent(module.WndHandle, module.OwnerControl.Handle);

                            if (module.WindowStyle == ProcessWindowStyle.Maximized)
                            {
                                SetWindowPos(module.WndHandle, IntPtr.Zero, pt_.X, pt_.Y, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);
                                ShowWindowAsync(module.WndHandle, SW_SHOWMAXIMIZED);
                            }
                            else
                            {
                                SetWindowPos(module.WndHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);
                            }
                        }));
                    }
                    else
                    {
                        SetParent(module.WndHandle, module.OwnerControl.Handle);

                        if (module.WindowStyle == ProcessWindowStyle.Maximized)
                        {
                            SetWindowPos(module.WndHandle, IntPtr.Zero, pt_.X, pt_.Y, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);
                            ShowWindowAsync(module.WndHandle, SW_SHOWMAXIMIZED);
                        }
                        else
                        {
                            SetWindowPos(module.WndHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);
                        }
                    }
                }

                module.Combined = true;
            }
        }

        protected virtual void CombineWatcher(object arg = null)
        {
            bool stop_ = false;
            bool success_ = false;
            int cycle_ = 0;
            DateTime started_ = DateTime.Now;

            while (!stop_)
            {
                int result_ = 0;

                if (arg == null)
                {
                    foreach (CombineModule module_ in modules)
                    {
                        if (module_.OwnerControl != null)
                        {
                            FitWindowToParent(module_);

                            if (module_.IsStarted)
                                result_++;
                        }
                    }
                }
                else
                {
                    CombineModule module_ = arg as CombineModule;

                    if (module_.OwnerControl != null)
                    {
                        FitWindowToParent(module_);

                        if (module_.IsStarted)
                            result_++;
                    }
                }

                if (result_ < modules.Count)
                {
                    if ((DateTime.Now - started_).TotalSeconds <= 10)
                        Thread.Sleep(1000);
                    else
                        stop_ = true;
                }
                else if (++cycle_ >= 2)
                {
                    stop_ = success_ = true;
                    cycle_ = 0;
                }

                if (stop_)
                    FireNotifyAllModulesCombiled(success_, stop_);
            }
        }

        protected virtual void FireNotifyAllModulesCombiled(bool success, bool all = false)
        {
            NotifyAllModulesCombined?.Invoke(this, new Pair<bool, bool>(success, all));
        }
        #endregion

        #region Public methods
        public virtual void Add(XmlNode node)
        {
            if (node != null)
            {
                if (node.Name == typeof(CombineModule).Name)
                {
                    CombineModule module_ = new CombineModule(node);

                    if (modules.Find(x => x.Name == module_.Name) == null)
                        modules.Add(module_);
                }
            }
        }

        public virtual void Add(CombineModule module)
        {
            if (module != null)
            {
                if (modules.Find(x => x.Name == module.Name) == null)
                    modules.Add(module);
            }
        }

        public virtual void Add(int id, string path, string workdir, string name, string caption= null, string parent= null, string owner= null, ProcessWindowStyle windowstyle = ProcessWindowStyle.Normal)
        {
            if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(name))
            {
                if (modules.Find(x => x.Name == name) == null)
                    modules.Add(new CombineModule(id, path, workdir, name, caption, parent, owner, windowstyle));
            }
        }

        public virtual bool Create(XmlNode node)
        {
            bool result_ = false;
            
            if (node != null)
            {
                foreach (XmlNode element_ in node.ChildNodes)
                {
                    CombineModule module_ = new CombineModule(element_);

                    if (module_.IsValid)
                        modules.Add(module_);
                }

                if (modules.Count > 0)
                    result_ = true;
            }

            return result_;
        }

        public virtual XElement ToXml(string nodename= null)
        {
            XElement result_ = new XElement(string.IsNullOrEmpty(nodename) ? GetType().Name : nodename);

            foreach (CombineModule module_ in modules)
                result_.Add(module_.ToXml());

            return result_;
        }

        public virtual void Create(Control root, EventHandler<Pair<bool, bool>> handler)
        {
            try
            {
                if (root != null)
                {
                    Control parent_ = null;
                    Control owner_ = null;

                    if (handler != null)
                        NotifyAllModulesCombined += handler;

                    foreach (CombineModule module_ in modules)
                    {
                        if (!string.IsNullOrEmpty(module_.Parent) && !string.IsNullOrEmpty(module_.Owner))
                        {
                            parent_ = FindControlRecursively(root, module_.Parent);

                            if (parent_ != null)
                            {
                                owner_ = (parent_.Controls[module_.Owner] as Control);
                            }
                        }
                 
                        module_.Init(owner_);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public virtual bool CheckProcess(string name, bool kill = false)
        {
            bool result_ = false;
            Process[] procs_ = null;

            try
            {
                procs_ = Process.GetProcessesByName(name);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            if ((result_ = procs_ != null && procs_.Length > 0) && kill)
                StopProcess(procs_[0].MainWindowHandle);

            return result_;
        }

        public virtual bool CheckProcess(int id)
        {
            Process[] procs_ = null;

            try
            {
                CombineModule module_ = modules.Find(x => x.Id == id);

                if (module_ != null)
                {
                    procs_ = Process.GetProcessesByName(module_.Name);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return (procs_ != null && procs_.Length > 0);
        }

        public virtual Control FindControlRecursively(Control control, string name)
        {
            Control result_ = null;

            if (control.Name.Equals(name))
            {
                result_ = control;
            }
            else
            {
                for (int i_ = 0; i_ < control.Controls.Count; i_++)
                {
                    result_ = FindControlRecursively(control.Controls[i_], name);
                    
                    if (result_ != null)
                    {
                        break;
                    }
                }
            }

            return result_;
        }

        public virtual bool StartAll()
        {
            int result_ = 0;

            try
            {
                foreach (CombineModule module_ in modules)
                    StartProcess(module_, ref result_);

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            if (result_ == modules.Count)
            {
                RunCombineWatcher();
                return true;
            }

            return false;
        }

        public virtual void StopAll()
        {
            foreach (CombineModule module_ in modules)
            {
                if (module_.WndHandle != null)
                    SendMessage(module_.WndHandle, WM_SYSCOMMAND, (IntPtr)SC_CLOSE, IntPtr.Zero);
            }
        }

        public virtual bool Start(int id)
        {
            int result_ = 0;
            CombineModule module_ = null;

            try
            {
                module_ = modules.Find(x => x.Id == id);
                StartProcess(module_, ref result_);
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            if (result_ > 0)
                RunCombineWatcher(module_);

            return (result_ == modules.Count);
        }

        public virtual bool Start(string name)
        {
            int result_ = 0;
            CombineModule module_ = null;

            try
            {
                module_ = modules.Find(x => x.Name == name);
                StartProcess(module_, ref result_);
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            if (result_ > 0)
                RunCombineWatcher(module_);

            return (result_ == modules.Count);
        }

        public virtual void Stop(int id)
        {
            CombineModule module_ = modules.Find(x => x.Id == id);
            StopProcess(module_);
        }

        public virtual void Stop(string name)
        {
            CombineModule module_ = modules.Find(x => x.Name == name);
            StopProcess(module_);
        }

        public virtual void Destroy()
        {
            StopAll();
            NotifyAllModulesCombined -= null;
        }

        public virtual void Clear()
        {
            modules.Clear();
        }

        public virtual bool IsStarted(int id)
        {
            bool result_ = false;

            CombineModule module_ = modules.Find(x => x.Id == id);

            if (module_ != null)
                result_ = module_.IsStarted;

            return result_;
        }

        public virtual bool IsStarted(string name)
        {
            bool result_ = false;

            if (!string.IsNullOrEmpty(name))
            {
                CombineModule module_ = modules.Find(x => x.Name == name);

                if (module_ != null)
                    result_ = module_.IsStarted;
            }

            return result_;
        }

        public virtual bool IsStartedAll()
        {
            bool result_ = true;

            foreach (CombineModule module_ in modules)
            {
                if (!(result_ = module_.IsStarted))
                    break;
            }

            return result_;
        }

        public virtual void ForceCoimbineModule(int id)
        {
            CombineModule module_ = modules.Find(x_ => x_.Id == id);

            if (module_ != null && module_.OwnerControl != null)
                FitWindowToParent(module_, true);
        }

        public virtual void ForceCoimbineModule(string name)
        {
            CombineModule module_ = modules.Find(x_ => x_.Name == name);

            if (module_ != null && module_.OwnerControl != null)
                FitWindowToParent(module_, true);
        }
        #endregion
    }
}
#endregion