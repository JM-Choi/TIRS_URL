#region Imports
using System;
using System.Windows.Forms;
#endregion

#region Program
namespace TechFloor.Forms
{
    public partial class FormSplash : Form
    {
        #region Events
        private delegate void ProgressDelegate(int progress);

        private ProgressDelegate delegateFunction_;
        #endregion

        #region Properties
        public int Progress
        {
            get => progressBar.Value;
            set => progressBar.Value = value;
        }
        #endregion

        #region Constructors
        public FormSplash()
        {
            InitializeComponent();
            this.TopLevel = true;
            this.progressBar.Maximum = 100;
            delegateFunction_ = this.UpdateProgressInternal;            
        }
        #endregion

        #region Protected methods
        protected virtual void OnFormClosing(object sender, FormClosingEventArgs e) { }

        protected virtual void OnFormClosed(object sender, FormClosedEventArgs e) { }

        protected virtual void OnFormLoad(object sender, EventArgs e) { }

        protected virtual void OnFormShown(object sender, EventArgs e)
        {
            // (App.MainForm as FormMain).SetDisplayMonitor(this);
            CenterToParent();
        }

        protected virtual void UpdateProgressInternal(int progress)
        {
            if (this.Handle == null)
                return;

            this.progressBar.Value = progress;
        }
        #endregion

        #region Public methods
        public virtual void UpdateProgress(int progress)
        {
            this.Invoke(delegateFunction_, progress);
        }
        #endregion
    }

    public class SpalshWorker
    {
        #region Events
        public event EventHandler WorkCompleted;

        public event EventHandler<SplashWorkerEventArgs> ProgressChanged;
        #endregion

        #region Protected methods
        protected virtual void FireProgressChanged(int progress)
        {
            ProgressChanged?.Invoke(this, new SplashWorkerEventArgs(progress));
        }

        protected virtual void FireWorkCompleted()
        {
            WorkCompleted?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Public methods
        public virtual void SetProgress(int progress)
        {
            if (progress < 100)
                FireProgressChanged(progress);
            else
                FireWorkCompleted();
        }

        public virtual void Run()
        {
            for (int i = 1; i <= 100; i++)
            {
                for (int j = 1; j <= 500000; j++)
                    Math.Pow(i, j);

                FireProgressChanged(i);
            }

            FireWorkCompleted();
        }
        #endregion
    }

    public class SplashWorkerEventArgs : EventArgs
    {
        #region Properties
        public int Progress
        {
            get;
            private set;
        }
        #endregion

        #region Constructors
        public SplashWorkerEventArgs(int progress)
        {
            Progress = progress;
        }
        #endregion
    }
}
#endregion