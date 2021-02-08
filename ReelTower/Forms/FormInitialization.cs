#region Imports
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using TechFloor.Object;
#endregion

#region Program
namespace TechFloor
{
    public partial class FormInitialization : Form
    {
        #region Fields
        protected int countOfRobot = 0;

        protected int countOfAligner = 0;

        protected object lockObject = new object();

        protected InitializeMode initializeMode = InitializeMode.All;

        protected HomeModule initializeModule = HomeModule.Robot;

        protected List<int> reelTowerGroupIndex = new List<int>();

        protected List<int> reelTowerHomeCount = new List<int>();

        protected Dictionary<int, int> reelTowerGroupHomeTimer = new Dictionary<int, int>();

        protected Dictionary<int, int> reelTowerGroupRows = new Dictionary<int, int>();

        protected System.Windows.Forms.Timer statusUpdateTimer = new System.Windows.Forms.Timer();
        #endregion

        #region Constructors
        public FormInitialization()
        {
            InitializeComponent();
            this.Location = (App.MainForm as FormMain).Location;
        }

        public FormInitialization(InitializeMode mode) : this()
        {
            this.initializeMode = mode;
            Create();
        }
        #endregion

        #region Protected methods
        #region Form event handlers
        protected virtual void OnFormShown(object sender, EventArgs e)
        {
            (App.MainForm as FormMain).SetDisplayMonitor(this);
        }

        protected virtual void OnFormLoad(object sender, EventArgs e)
        {
            try
            {
                int index = 0;
                SetDisplayLanguage();

                switch (initializeMode)
                {
                    case InitializeMode.All:
                    case InitializeMode.ReelTowerGroup:
                        {
                            if (reelTowerGroupIndex.Contains(0))
                            {
                                index = dataGridView.Rows.Add(Properties.Resources.String_FormInitialization_ReelTowerGroup);
                                reelTowerGroupRows.Add(0, index);
                            }

                            reelTowerHomeCount.Add(0);
                            dataGridView.Rows[reelTowerGroupRows[0]].Cells[1].Value = Properties.Resources.String_FormInitialization_InitializeReelTowerGroup;

                            foreach (int item in reelTowerHomeCount)
                                reelTowerGroupHomeTimer[0] = 0;

                            App.MainSequence.Init(initializeMode);
                        }
                        break;
                }

                GridResize(dataGridView, dataGridView.RowCount);
                statusUpdateTimer.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                HomeDone(false);
            }
        }

        protected virtual void OnFormResize(object sender, EventArgs args)
        {
            GridResize(dataGridView, dataGridView.RowCount);
        }

        protected virtual void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            App.MainSequence.StopInit();
        }
        
        protected virtual void CloseForm(bool state = false)
        {
            DialogResult = (state ? DialogResult.OK : DialogResult.Cancel);

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    statusUpdateTimer.Stop();
                    Close();
                }));
            }
            else
            {
                statusUpdateTimer.Stop();
                Close();
            }
        }
        #endregion

        #region Initialize items methods
        protected virtual void GridResize(DataGridView gridView, int count)
        {
            if (count == 0) return;
            int height = Math.Max((gridView.Height - 1) / count, (gridView.Height - 1) / 12);
            int remain = Math.Max((gridView.Height - 1) % count, (gridView.Height - 1) % 12);

            if (gridView.Rows.Count > 0)
            {
                for (int i = 0; i < count; i++)
                    gridView.Rows[i].Height = height;

                for (int i = count - 1; i >= 0; i--)
                {
                    if (remain == 0) break;
                    gridView.Rows[i].Height++;
                    remain--;
                }
            }
        }
        #endregion

        #region Initialize event handlers
        protected virtual bool Create()
        {
            bool result = true;

            try
            {
                switch (initializeMode)
                {
                    case InitializeMode.All:
                    case InitializeMode.ReelTowerGroup:
                        {
                            reelTowerGroupIndex.Add(0);
                            reelTowerGroupHomeTimer[0] = 0;
                            countOfRobot++;
                  
                            if (countOfAligner > 0)
                                initializeModule = HomeModule.Aligner;
                            else
                                initializeModule = HomeModule.Robot;
                        }
                        break;
                }

                this.statusUpdateTimer.Interval = 100;
                this.statusUpdateTimer.Tick    += OnTimerTick;
            }
            catch (Exception ex)
            {
                result = false;
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result;
        }

        protected virtual void MoveAfterReelTowerGroupHome(object id)
        {
            int index = Convert.ToInt32(id);
            
            dataGridView.Rows[reelTowerGroupRows[index]].Cells[1].Value = Properties.Resources.String_FormInitialization_ReelTowerGroup;
            reelTowerHomeCount.Remove(index);
            dataGridView.Rows[reelTowerGroupRows[index]].Cells[1].Value = Properties.Resources.String_Completed;
            HomeDone();
        }

        protected virtual void HomeDone(bool result = true)
        {
            CloseForm(result);
        }

        protected virtual void OnTimerTick(object sender, EventArgs args)
        {
            bool failure_ = false;

            lock (lockObject)
            {
                int tick = App.TickCount;

                if (reelTowerGroupHomeTimer[0] != 0 &&
                    reelTowerGroupHomeTimer[0] < tick)
                {
                    reelTowerGroupHomeTimer[0] = 0;
                }
            }

            switch ((ReelTowerGroupSequence.InitializeSteps)App.MainSequence.InitializeStep)
            {
                case ReelTowerGroupSequence.InitializeSteps.None:
                case ReelTowerGroupSequence.InitializeSteps.Ready:
                case ReelTowerGroupSequence.InitializeSteps.CheckTowerInformation:
                    {
                        dataGridView.Rows[reelTowerGroupRows[0]].Cells[1].Value = Properties.Resources.String_FormInitialization_CheckTtowerInformation;
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.CheckNotificationListener:
                    {
                        dataGridView.Rows[reelTowerGroupRows[0]].Cells[1].Value = Properties.Resources.String_FormInitialization_CheckNotificationListener;
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.CheckReelHandler:
                    {
                        dataGridView.Rows[reelTowerGroupRows[0]].Cells[1].Value = Properties.Resources.String_FormInitialization_CheckReelHandler;
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.PrepareInitialize:
                    {
                        dataGridView.Rows[reelTowerGroupRows[0]].Cells[1].Value = Properties.Resources.String_FormInitialization_PrepareInitialization;
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.Initializing:
                    {
                        dataGridView.Rows[reelTowerGroupRows[0]].Cells[1].Value = Properties.Resources.String_FormInitialization_PrepareInitialization;
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.PostInitialize:
                    {
                        dataGridView.Rows[reelTowerGroupRows[0]].Cells[1].Value = Properties.Resources.String_FormInitialization_CheckInitializationState;
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.Done:
                    {
                        dataGridView.Rows[reelTowerGroupRows[0]].Cells[1].Value = Properties.Resources.String_FormInitialization_CompleteInitialization;
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.Unknown:
                    break;
                case ReelTowerGroupSequence.InitializeSteps.Failed:
                    {
                        dataGridView.Rows[reelTowerGroupRows[0]].Cells[1].Value = Properties.Resources.String_FormInitialization_FailedInitialization;
                        failure_ = true;
                    }
                    break;
            }

            if (App.Initialized || App.OperationState == OperationStates.Alarm || App.OperationState == OperationStates.Stop || failure_)
            {
                statusUpdateTimer.Stop();
                HomeDone();
            }
        }

        protected virtual void CancelByInterlock(object sender, EventArgs args)
        {
            CloseForm();
        }
        #endregion

        #region Robot state event handlers
        protected virtual void OnChangedRobotHomeStatus(object sender, EventArgs args)
        {
        }
        #endregion

        #region Set display language
        protected virtual void SetDisplayLanguage()
        {
            buttonAbort.Text = Properties.Resources.String_Abort;
            labelTitle.Text = Properties.Resources.String_Initializing;
        }
        #endregion

        #endregion
    }
}
#endregion