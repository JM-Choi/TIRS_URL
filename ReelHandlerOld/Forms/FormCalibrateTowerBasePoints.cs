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
    public partial class FormCalibrateTowerBasePoints : Form
    {
        #region Fields
        protected object lockObject = new object();
        protected AutomaticTeachMode automaticTeachMode = AutomaticTeachMode.All;
        protected AutomaticTeachModule automaticTeachModule = AutomaticTeachModule.Robot;
        protected int countOfRobot = 0;
        protected List<int> robotAutomaticTeachIndex = new List<int>();
        protected List<int> robotAutomaticTeachCount = new List<int>();
        protected Dictionary<int, int> robotAutomaticTeachTimer = new Dictionary<int, int>();
        protected Dictionary<int, int> robotRows = new Dictionary<int, int>();
        protected System.Windows.Forms.Timer stateUpdateTimer = new System.Windows.Forms.Timer();
        #endregion

        #region Constructors
        public FormCalibrateTowerBasePoints()
        {
            InitializeComponent();
            this.Location = (App.MainForm as FormMain).Location;
        }

        public FormCalibrateTowerBasePoints(AutomaticTeachMode mode) : this()
        {
            this.automaticTeachMode = mode;
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

                switch (automaticTeachMode)
                {
                    case AutomaticTeachMode.All:
                    case AutomaticTeachMode.Load:
                        {
                            if (robotAutomaticTeachIndex.Contains(0))
                            {
                                index = dataGridView.Rows.Add("Universal Robot");
                                robotRows.Add(0, index);
                            }
                        }
                        break;
                }

                switch (automaticTeachMode)
                {
                    case AutomaticTeachMode.All:
                    case AutomaticTeachMode.Load:
                    case AutomaticTeachMode.Unload:
                        {
                            robotAutomaticTeachCount.Add(0);
                            dataGridView.Rows[robotRows[0]].Cells[1].Value = "Initialize robot.";

                            foreach (int item in robotAutomaticTeachCount)
                                robotAutomaticTeachTimer[0] = 0;

                            App.MainSequence.AutomaticTeachRobot(automaticTeachMode);
                        }
                        break;
                }

                GridResize(dataGridView, dataGridView.RowCount);
                stateUpdateTimer.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                AutomaticTeachDone(false);
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
                BeginInvoke(new Action(() => { Close(); }));
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
                switch (automaticTeachMode)
                {
                    case AutomaticTeachMode.All:
                    case AutomaticTeachMode.Load:
                    case AutomaticTeachMode.Unload:
                        {
                            robotAutomaticTeachIndex.Add(0);
                            robotAutomaticTeachTimer[0] = 0;
                            
                            if (++countOfRobot > 0)
                                automaticTeachModule = AutomaticTeachModule.Robot;
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
            robotAutomaticTeachCount.Remove(index);
            dataGridView.Rows[robotRows[index]].Cells[1].Value = "Completed";
            AutomaticTeachDone();
        }

        protected virtual void AutomaticTeachDone(bool result = true)
        {
            CloseForm(result);
        }

        protected virtual void OnTimerTick(object sender, EventArgs args)
        {
            bool failure_ = false;

            lock (lockObject)
            {
                int tick = App.TickCount;

                if (robotAutomaticTeachTimer[0] != 0 &&
                    robotAutomaticTeachTimer[0] < tick)
                {
                    robotAutomaticTeachTimer[0] = 0;
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
                AutomaticTeachDone();
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

        #endregion

        private void OnFormShown(object sender, EventArgs e)
        {
            (App.MainForm as FormMain).SetDisplayMonitor(this);
        }
    }
}
#endregion