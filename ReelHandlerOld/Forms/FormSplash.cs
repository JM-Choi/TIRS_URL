using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechFloor.Forms
{
    public partial class FormSplash : Form
    {
        private delegate void ProgressDelegate(int progress);
        private ProgressDelegate del;

        public int Progress
        {
            get => progressBar.Value;
            set => progressBar.Value = value;
        }

        public FormSplash()
        {
            InitializeComponent();
            this.progressBar.Maximum = 100;
            del = this.UpdateProgressInternal;
        }

        private void UpdateProgressInternal(int progress)
        {
            if (this.Handle == null)
                return;

            this.progressBar.Value = progress;
        }

        public void UpdateProgress(int progress)
        {
            this.Invoke(del, progress);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void OnFormLoad(object sender, EventArgs e)
        {
        }

        private void OnFormShown(object sender, EventArgs e)
        {
            // (App.MainForm as FormMain).SetDisplayMonitor(this);
            CenterToParent();
        }
    }

    public class Hardworker
    {
        public event EventHandler<HardWorkerEventArgs> ProgressChanged;
        public event EventHandler HardWorkDone;
        public void SetProgress(int progress)
        {
            if (progress < 100)
                this.OnProgressChanged(progress);
            else
                this.OnHardWorkDone();
        }

        public void DoHardWork()
        {
            for (int i = 1; i <= 100; i++)
            {
                for (int j = 1; j <= 500000; j++)
                {
                    Math.Pow(i, j);
                }
                this.OnProgressChanged(i);
            }
            this.OnHardWorkDone();
        }

        private void OnProgressChanged(int progress)
        {
            var handler = this.ProgressChanged;
            if (handler != null)
            {
                handler(this, new HardWorkerEventArgs(progress));
            }
        }

        private void OnHardWorkDone()
        {
            var handler = this.HardWorkDone;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }

    public class HardWorkerEventArgs : EventArgs
    {
        public HardWorkerEventArgs(int progress)
        {
            this.Progress = progress;
        }

        public int Progress
        {
            get;
            private set;
        }
    }
}
