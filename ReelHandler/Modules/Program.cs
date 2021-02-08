#region Imports
using System;
using System.Reflection;
using System.Windows.Forms;
using TechFloor.Util;
using Cognex.VisionPro;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Security;
using System.Diagnostics;
#endregion

#region Program
namespace TechFloor
{
    static class Program
    {
        private static App app_ = new App(Assembly.GetEntryAssembly());

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandler);

            app_.Run(new FormMain(app_));

            CogFrameGrabbers frameGrabbers = new CogFrameGrabbers();
            for (int i = 0; i < frameGrabbers.Count; i++)
            {
                frameGrabbers[i].Disconnect(false);
            }
        }

        [HandleProcessCorruptedStateExceptions, SecurityCritical]
        static void ThreadException(object sender, ThreadExceptionEventArgs args)
        {
            Debug.WriteLine($"ExceptionHandler caught : {args.Exception.Message}", "System");
            MessageBox.Show($"ExceptionHandler caught : {args.Exception.Message}, Runtime terminated.");
        }

        [HandleProcessCorruptedStateExceptions, SecurityCritical]
        static void ExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;

            Debug.WriteLine($"ExceptionHandler caught : {e.Message}", "System");
            Debug.WriteLine($"Runtime terminating: {args.IsTerminating}", "System");
            MessageBox.Show($"ExceptionHandler caught : {e.Message}, Runtime terminating: {args.IsTerminating}");
        }
    }
}
#endregion