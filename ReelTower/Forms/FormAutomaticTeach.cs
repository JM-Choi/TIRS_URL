#region Licenses
///////////////////////////////////////////////////////////////////////////////
/// MIT License
/// 
/// Copyright (c) 2019 Marcus Software Ltd.
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
/// SOFTWARE.
///////////////////////////////////////////////////////////////////////////////
///          Copyright Joe Coder 2004 - 2006.
/// Distributed under the Boost Software License, Version 1.0.
///    (See accompanying file LICENSE_1_0.txt or copy at
///          https://www.boost.org/LICENSE_1_0.txt)
///////////////////////////////////////////////////////////////////////////////
/// 저작권 (c) 2019 Marcus Software Ltd. (isadrastea.kor@gmail.com)
///
/// 본 라이선스의 적용을 받는 소프트웨어와 동봉된 문서(소프트웨어)를 획득하는 
/// 모든 개인이나 기관은 소프트웨어를 Marcus Software (isadrastea.kor@gmail.com)
/// 에 신고하고, 허용 의사를 서면으로 득하여, 사용, 복제, 전시, 배포, 실행 및
/// 전송할 수 있고, 소프트웨어의 파생 저작물을 생성할 수 있으며, 소프트웨어가
/// 제공된 제3자에게 그러한 행위를 허용할 수 있다. 단, 이 모든 행위는 다음과 
/// 같은 조건에 의해 제한 한다.:
///
/// 소프트웨어의 저작권 고지, 그리고 위의 라이선스 부여와 이 규정과 아래의 부인 
/// 조항을 포함한 이 글의 전문이 소프트웨어를 전체적으로나 부분적으로 복제한 
/// 모든 복제본과 소프트웨어의 모든 파생 저작물 내에 포함되어야 한다. 단, 해당 
/// 복제본이나 파생저작물이 소스 언어 프로세서에 의해 생성된, 컴퓨터로 인식 
/// 가능한 오브젝트 코드의 형식으로만 되어 있는 경우는 제외된다.
///
/// 이 소프트웨어는 상품성, 특정 목적에의 적합성, 소유권, 비침해에 대한 보증을 
/// 포함한, 이에 국한되지는 않는, 모든 종류의 명시적이거나 묵시적인 보증 없이 
///“있는 그대로의 상태”로 제공된다. 저작권자나 소프트웨어의 배포자는 어떤 
/// 경우에도 소프트웨어 자체나 소프트웨어의 취급과 관련하여 발생한 손해나 기타 
/// 책임에 대하여, 계약이나 불법행위 등에 관계 없이 어떠한 책임도 지지 않는다.
///////////////////////////////////////////////////////////////////////////////
/// project ReelTower 
/// @author Marcus (isadrastea.kor@gmail.com)
/// @namespace Marcus.Solution.TechFloor
/// @file FormAutomaticTeach.cs
/// @brief
/// @details
/// @date 2019, 5, 21, 오후 1:13
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using Marcus.Solution.TechFloor;
using Marcus.Solution.TechFloor.Object;
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
namespace Marcus.Solution.TechFloor
{
    public partial class FormAutomaticTeach : Form
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
        public FormAutomaticTeach()
        {
            InitializeComponent();
            this.Location = (App.MainForm as FormMain).Location;
        }

        public FormAutomaticTeach(AutomaticTeachMode mode) : this()
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
                Trace.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                AutomaticTeachDone(false);
            }
        }

        protected virtual void OnFormResize(object sender, EventArgs args)
        {
            GridResize(dataGridView, dataGridView.RowCount);
        }

        protected virtual void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            App.MainSequence.StopRobotHome();
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
                Trace.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
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

            switch ((ReelTowerGroupSequence.InitializeSteps)App.MainSequence.InitializeStep)
            {
                case ReelTowerGroupSequence.InitializeSteps.None:
                case ReelTowerGroupSequence.InitializeSteps.Ready:
                case ReelTowerGroupSequence.InitializeSteps.CheckProgramState:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Check program state";
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.StopProgram:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Stop program.";
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.LoadProgram:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Load robot program.";
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.PlayProgram:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Play loaded program.";
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.CheckReturnReelPresentSensors:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Check return reel present state.";
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.CheckBarcodeOption:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Check barcode option.";
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.PrepareInitialize:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Prepare initialization.";
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.Initializing:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Initializing...";
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.PostInitialize:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Check initialization state.";
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.Done:
                    {
                        dataGridView.Rows[robotRows[0]].Cells[1].Value = "Complete initialization.";
                    }
                    break;
                case ReelTowerGroupSequence.InitializeSteps.Unknown:
                    break;
                case ReelTowerGroupSequence.InitializeSteps.Failed:
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