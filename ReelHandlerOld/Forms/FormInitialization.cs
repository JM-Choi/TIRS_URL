#region Imports
using TechFloor;
using TechFloor.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;
#endregion

#region Program
namespace TechFloor
{
    public partial class FormInitialization : Form
    {
        #region Fields
        protected object lockObject = new object();
        protected InitializeMode initializeMode = InitializeMode.All;
        protected HomeModule initializeModule = HomeModule.Robot;
        protected int countOfRobot = 0;
        protected int countOfAligner = 0;
        protected List<int> robotHomeIndex = new List<int>();
        protected List<int> robotHomeCount = new List<int>();
        protected List<int> alignerHomeIndex = new List<int>();
        protected List<int> alignerHomeCount = new List<int>();
        protected Dictionary<int, int> robotHomeTimer = new Dictionary<int, int>();
        protected Dictionary<int, int> alignerHomeTimer = new Dictionary<int, int>();
        protected Dictionary<int, int> robotRows = new Dictionary<int, int>();
        protected Dictionary<int, int> alignerRows = new Dictionary<int, int>();
        protected System.Windows.Forms.Timer stateUpdateTimer = new System.Windows.Forms.Timer();
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
        protected virtual void OnFormLoad(object sender, EventArgs e)
        {
            try
            {
                int index = 0;
                SetDisplayLanguage();

                switch (initializeMode)
                {
                    case InitializeMode.All:
                    case InitializeMode.Robot:
                        {
                            if (robotHomeIndex.Contains(0))
                            {
                                index = dataGridView.Rows.Add("Universal Robot");
                                robotRows.Add(0, index);
                            }
                        }
                        break;
                }

                switch (initializeMode)
                {
                    case InitializeMode.All:
                    case InitializeMode.Robot:
                    case InitializeMode.Aligner:
                        {
                            robotHomeCount.Add(0);
                            dataGridView.Rows[robotRows[0]].Cells[1].Value = "Initialize robot.";

                            foreach (int item in robotHomeCount)
                                robotHomeTimer[0] = 0;

                            App.MainSequence.Init(initializeMode);
                        }
                        break;
                }

                GridResize(dataGridView, dataGridView.RowCount);
                stateUpdateTimer.Start();
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

        protected virtual void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            (App.MainForm as FormMain).SetFocus();
        }

        protected virtual void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            App.MainSequence.StopInit();
        }

        protected virtual void OnFormShown(object sender, EventArgs e)
        {
            (App.MainForm as FormMain).SetDisplayMonitor(this);
        }

        protected virtual void CloseForm(bool state = false)
        {
            DialogResult = (state ? DialogResult.OK : DialogResult.Cancel);

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => {
                    stateUpdateTimer.Stop();
                    Close();
                }));
            }
            else
            {
                stateUpdateTimer.Stop();
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
                        {
                            robotHomeIndex.Add(0);
                            robotHomeTimer[0] = 0;
                            countOfRobot++;
                  
                            if (countOfAligner > 0)
                                initializeModule = HomeModule.Aligner;
                            else
                                initializeModule = HomeModule.Robot;
                        }
                        break;
                    case InitializeMode.Robot:
                        {
                            robotHomeIndex.Add(0);
                            robotHomeTimer[0] = 0;
                            countOfRobot++;

                            if (countOfRobot > 0)
                                initializeModule = HomeModule.Robot;
                            else
                                this.Close();
                        }
                        break;
                }

                this.stateUpdateTimer.Interval = 1000;
                this.stateUpdateTimer.Tick    += OnTimerTick;
            }
            catch (Exception ex)
            {
                result = false;
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result;
        }

        protected virtual void MoveAfterRobotHome(object id)
        {
            int index = Convert.ToInt32(id);
            
            dataGridView.Rows[robotRows[index]].Cells[1].Value = "Universal Robot";
            robotHomeCount.Remove(index);
            dataGridView.Rows[robotRows[index]].Cells[1].Value = "Completed";
            HomeDone();
        }

        protected virtual void MoveAfterAlignerHome(object id)
        {
            int index = Convert.ToInt32(id);

            dataGridView.Rows[alignerRows[index]].Cells[1].Value = "Aligner";
            alignerHomeCount.Remove(index);
            dataGridView.Rows[alignerRows[index]].Cells[1].Value = "Completed";
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

                if (robotHomeTimer[0] != 0 &&
                    robotHomeTimer[0] < tick)
                {
                    robotHomeTimer[0] = 0;
                }
            }

            switch ((ReelTowerRobotSequence.InitializeSteps)App.MainSequence.InitializeStep)
            {
                case ReelTowerRobotSequence.InitializeSteps.None:
                case ReelTowerRobotSequence.InitializeSteps.Ready:
                case ReelTowerRobotSequence.InitializeSteps.CheckProgramState:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Check program state";
                    }
                    break;
                case ReelTowerRobotSequence.InitializeSteps.StopProgram:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Stop program.";
                    }
                    break;
                case ReelTowerRobotSequence.InitializeSteps.LoadProgram:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Load robot program.";
                    }
                    break;
                case ReelTowerRobotSequence.InitializeSteps.PlayProgram:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Play loaded program.";
                    }
                    break;
                case ReelTowerRobotSequence.InitializeSteps.CheckReturnReelPresentSensors:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Check return reel present state.";
                    }
                    break;
                case ReelTowerRobotSequence.InitializeSteps.CheckBarcodeOption:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Check barcode option.";
                    }
                    break;
                case ReelTowerRobotSequence.InitializeSteps.PrepareInitialize:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Prepare initialization.";
                    }
                    break;
                case ReelTowerRobotSequence.InitializeSteps.Initializing:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Initializing...";
                    }
                    break;
                case ReelTowerRobotSequence.InitializeSteps.PostInitialize:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Check initialization state.";
                    }
                    break;
                case ReelTowerRobotSequence.InitializeSteps.Done:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Complete initialization.";
                    }
                    break;
                case ReelTowerRobotSequence.InitializeSteps.Unknown:
                    break;
                case ReelTowerRobotSequence.InitializeSteps.Failed:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Failed to home.";
                        failure_ = true;
                    }
                    break;
            }

            if (App.Initialized || App.OperationState == OperationStates.Alarm || failure_)
            {
                stateUpdateTimer.Stop();
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

        #region Aligner state event handlers
        protected virtual void OnChangedAlignerHomeStatus(object sender, EventArgs args)
        {
        }
        #endregion

        #region Set display language
        protected virtual void SetDisplayLanguage()
        {
            buttonAbort.Text = Properties.Resources.String_FormIntialization_buttonAbort;
            labelTitle.Text = Properties.Resources.String_Initializing;
        }
        #endregion

        #endregion
    }
}
#endregion