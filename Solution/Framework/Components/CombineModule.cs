#region Imports
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
#endregion

#region Program
namespace TechFloor.Components
{
    public class CombineModule
    {
        #region Fields
        protected bool started = false;

        protected bool combined = false;

        protected int id = 0;

        protected string path = string.Empty;

        protected string name = string.Empty;

        protected string caption = string.Empty;

        protected string parent = string.Empty;

        protected string owner = string.Empty;

        protected string workdir = string.Empty;

        protected ProcessWindowStyle windowstyle = ProcessWindowStyle.Normal;

        protected IntPtr wndHandle = IntPtr.Zero;

        protected Control ctrl = null;

        protected ProcessStartInfo processInfo = null;

        protected Thread threadCombineWatcher = null;
        #endregion

        #region Properties
        public bool IsValid => !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(path);

        public int Id => id;

        public string Path => path;

        public string WorkDirectory => workdir;

        public string Name => name;

        public string Caption => caption;

        public string Parent => parent;

        public string Owner => owner;

        public ProcessWindowStyle WindowStyle => windowstyle;

        public IntPtr WndHandle
        {
            get => wndHandle;
            set => wndHandle = value;
        }

        public Control OwnerControl
        {
            get => ctrl;
            set => ctrl = value;
        }

        public bool Combined
        {
            get => combined;
            set => combined = value;
        }

        public ProcessStartInfo ProcessInfo => processInfo;

        public bool IsStarted => started && ((ctrl != null)? combined : false);
        #endregion

        #region Constructors
        public CombineModule(XmlNode node = null)
        {
            Create(node);
        }

        public CombineModule(int id, string path, string workdir, string name, string caption= null, string parent= null, string owner= null, ProcessWindowStyle windowstyle = ProcessWindowStyle.Normal)
        {
            this.id = id;
            this.path = path;
            this.workdir = workdir;
            this.name = name;
            this.caption = caption;
            this.parent = parent;
            this.owner = owner;
            this.windowstyle = windowstyle;
        }
        #endregion

        #region Protected methods        
        #endregion

        #region Public methods
        public virtual bool Create(XmlNode node)
        {
            bool result_ = false;

            try
            {
                if (node != null)
                {
                    if (node.Name == GetType().Name)
                    {
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
                                case "path":
                                    {
                                        if (!string.IsNullOrEmpty(attr_.Value))
                                            path = attr_.Value;
                                    }
                                    break;
                                case "workdir":
                                    {
                                        if (!string.IsNullOrEmpty(attr_.Value))
                                            workdir = attr_.Value;
                                    }
                                    break;
                                case "caption":
                                    {
                                        if (!string.IsNullOrEmpty(attr_.Value))
                                            caption = attr_.Value;
                                    }
                                    break;
                                case "parent":
                                    {
                                        if (!string.IsNullOrEmpty(attr_.Value))
                                            parent = attr_.Value;
                                    }
                                    break;
                                case "owner":
                                    {
                                        if (!string.IsNullOrEmpty(attr_.Value))
                                            owner = attr_.Value;
                                    }
                                    break;
                                case "name":
                                    {
                                        if (!string.IsNullOrEmpty(attr_.Value))
                                            name = attr_.Value;
                                    }
                                    break;
                                case "windowstyle":
                                    {
                                        if (!string.IsNullOrEmpty(attr_.Value))
                                            windowstyle = (ProcessWindowStyle)Enum.Parse(typeof(ProcessWindowStyle), attr_.Value);
                                    }
                                    break;
                            }
                        }

                        result_ = !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(path);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                result_ = false;
            }

            return result_;
        }

        public virtual XElement ToXml()
        {
            XElement result_ = null;

            try
            {
                result_ = new XElement(GetType().Name,
                    new XAttribute("id", id),
                    new XAttribute("name", name),
                    new XAttribute("path", path),
                    new XAttribute("windowstyle", windowstyle));

                if (!string.IsNullOrEmpty(workdir))
                    result_.Add(new XAttribute("workdir", workdir));

                if (!string.IsNullOrEmpty(caption))
                {
                    if (workdir.Contains("AMM"))
                    {
                        string ammPath = @"D:\AMM\Versioninfo.ini";
                        if (File.Exists(ammPath))
                        {
                            using (FileStream fs = new FileStream(ammPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                using (StreamReader sw = new StreamReader(fs, Encoding.UTF8))
                                {
                                    string contents_ = sw.ReadToEnd();
                                    caption = caption.Split(':')[0] + ":" + contents_;
                                }
                            }
                        }
                    }

                    result_.Add(new XAttribute("caption", caption));
                }
                

                if (!string.IsNullOrEmpty(parent))
                    result_.Add(new XAttribute("parent", parent));

                if (!string.IsNullOrEmpty(owner))
                    result_.Add(new XAttribute("owner", owner));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public virtual void Init(Control ctrl = null)
        {
            if (processInfo == null)
            {
                if (File.Exists(path))
                {
                    processInfo = new ProcessStartInfo();
                    processInfo.FileName = path;
                    processInfo.Arguments = string.Empty;
                    processInfo.UseShellExecute = true;
                    processInfo.WindowStyle = windowstyle;
                    processInfo.RedirectStandardInput = false;
                    processInfo.RedirectStandardOutput = false;
                    processInfo.RedirectStandardError = false;

                    if (!string.IsNullOrEmpty(workdir) && Directory.Exists(workdir))
                        processInfo.WorkingDirectory = workdir;

                    // processInfo.Verb = "open";
                    // processInfo.CreateNoWindow = true;

                    if (ctrl != null)
                        this.ctrl = ctrl;
                }
            }
        }

        public virtual bool Start()
        {
            if (processInfo != null)
            {
                Process proc_ = Process.Start(processInfo);
                started = proc_.WaitForInputIdle();
            }

            return started;
        }
        #endregion
    }
}
#endregion