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
    public partial class FormCalibration : Form
    {
        #region Fields
        protected object lockObject = new object();
        protected CalibrationMode calibrationMode = CalibrationMode.TowerBasePoints;
        protected int countOfPoints = 2;
        protected List<int> towerBasePontsIndex = new List<int>();
        protected List<int> towerBasePointsCount = new List<int>();
        protected Dictionary<int, int> towerBasePointsTimer = new Dictionary<int, int>();
        protected Dictionary<int, int> pointsRows = new Dictionary<int, int>();
        protected System.Windows.Forms.Timer stateUpdateTimer = new System.Windows.Forms.Timer();
        #endregion

        #region Constructors
        public FormCalibration()
        {
            InitializeComponent();
            this.Location = (App.MainForm as FormMain).Location;
        }

        public FormCalibration(CalibrationMode mode) : this()
        {
            this.calibrationMode = mode;
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

                switch (calibrationMode)
                {
                    case CalibrationMode.All:
                    case CalibrationMode.TowerBasePoints:
                        {
                            if (towerBasePontsIndex.Contains(0))
                            {
                                index = dataGridView.Rows.Add("Tower base points");
                                pointsRows.Add(0, index);
                            }
                        }
                        break;
                }

                switch (calibrationMode)
                {
                    case CalibrationMode.All:
                    case CalibrationMode.TowerBasePoints:
                        {
                            towerBasePointsCount.Add(0);
                            dataGridView.Rows[pointsRows[0]].Cells[1].Value = "Prepare calibration.";

                            foreach (int item in towerBasePointsCount)
                                towerBasePointsTimer[0] = 0;

                            App.MainSequence.CalDevices(calibrationMode);
                        }
                        break;
                }

                GridResize(dataGridView, dataGridView.RowCount);
                stateUpdateTimer.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                CalibrateDevicesDone(false);
            }
        }

        protected virtual void OnFormResize(object sender, EventArgs args)
        {
            GridResize(dataGridView, dataGridView.RowCount);
        }

        protected virtual void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            App.MainSequence.StopCalDevices();
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

        #region Calibration items methods
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

        #region Calibration event handlers
        protected virtual bool Create()
        {
            bool result = true;

            try
            {
                switch (calibrationMode)
                {
                    case CalibrationMode.All:
                    case CalibrationMode.TowerBasePoints:
                        {
                            towerBasePontsIndex.Add(0);
                            towerBasePointsTimer[0] = 0;
                            
                            if (++countOfPoints > 0)
                                calibrationMode = CalibrationMode.TowerBasePoints;
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
            
            dataGridView.Rows[pointsRows[index]].Cells[1].Value = "Tower base points";
            towerBasePointsCount.Remove(index);
            dataGridView.Rows[pointsRows[index]].Cells[1].Value = "Completed";
            CalibrateDevicesDone();
        }

        protected virtual void CalibrateDevicesDone(bool result = true)
        {
            CloseForm(result);
        }

        protected virtual void OnTimerTick(object sender, EventArgs args)
        {
            bool failure_ = false;

            lock (lockObject)
            {
                int tick = App.TickCount;

                if (towerBasePointsTimer[0] != 0 &&
                    towerBasePointsTimer[0] < tick)
                {
                    towerBasePointsTimer[0] = 0;
                }
            }

            switch ((ReelTowerRobotSequence.CalibrationSteps)App.MainSequence.CalibrationStep)
            {
                case ReelTowerRobotSequence.CalibrationSteps.None:
                case ReelTowerRobotSequence.CalibrationSteps.Ready:
                case ReelTowerRobotSequence.CalibrationSteps.PrepareCalibration:
                    {
                        dataGridView.Rows[pointsRows[0]].Cells[1].Value = "Prepare calibration.";
                    }
                    break;
                case ReelTowerRobotSequence.CalibrationSteps.CheckCartPresent:
                    {
                        dataGridView.Rows[pointsRows[0]].Cells[1].Value = "Check cart present.";
                    }
                    break;
                case ReelTowerRobotSequence.CalibrationSteps.CheckTowerBasePoint1:
                    {
                        dataGridView.Rows[pointsRows[0]].Cells[1].Value = "Check tower base point1.";
                    }
                    break;
                case ReelTowerRobotSequence.CalibrationSteps.SetTowerBasePoint1:
                    {
                        dataGridView.Rows[pointsRows[0]].Cells[1].Value = "Set tower base point1.";
                    }
                    break;
                case ReelTowerRobotSequence.CalibrationSteps.CheckTowerBasePoint2:
                    {
                        dataGridView.Rows[pointsRows[0]].Cells[1].Value = "Check tower base point2.";
                    }
                    break;
                case ReelTowerRobotSequence.CalibrationSteps.CheckDisplacements:
                    {
                        dataGridView.Rows[pointsRows[0]].Cells[1].Value = "Check displacements.";
                    }
                    break;
                case ReelTowerRobotSequence.CalibrationSteps.PostCalibration:
                    {
                        dataGridView.Rows[pointsRows[0]].Cells[1].Value = "Post calibration.";
                    }
                    break;
                case ReelTowerRobotSequence.CalibrationSteps.Done:
                    {
                        dataGridView.Rows[pointsRows[0]].Cells[1].Value = "Complete calibration.";
                    }
                    break;
                case ReelTowerRobotSequence.CalibrationSteps.Unknown:
                    break;
                case ReelTowerRobotSequence.CalibrationSteps.Failed:
                    {
                        dataGridView.Rows[pointsRows[0]].Cells[1].Value = "Failed to calibrate.";
                        failure_ = true;
                    }
                    break;
            }

            if ((App.MainSequence as ReelTowerRobotSequence).Calibrated || App.OperationState == OperationStates.Alarm || failure_)
            {
                stateUpdateTimer.Stop();
                CalibrateDevicesDone();
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