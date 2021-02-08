#region Imports
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TechFloor.Components;
using TechFloor.Device.CommunicationIo;
using TechFloor.Gui;
using TechFloor.Object;
using TechFloor.Util;
#endregion

#region Program
namespace TechFloor.Forms
{
    public partial class FormMonitorStages : FormExt
    {
        protected bool shutdown = false;
        protected int previousRejects = 0;
        protected DateTime elapsedTime = DateTime.MinValue;
        protected List<Pair<ReelUnloadReportStates, int>> previousOutputs = new List<Pair<ReelUnloadReportStates, int>> {
            new Pair<ReelUnloadReportStates, int>(ReelUnloadReportStates.Unknown, 0),
            new Pair<ReelUnloadReportStates, int>(ReelUnloadReportStates.Unknown, 0),
            new Pair<ReelUnloadReportStates, int>(ReelUnloadReportStates.Unknown, 0),
            new Pair<ReelUnloadReportStates, int>(ReelUnloadReportStates.Unknown, 0),
            new Pair<ReelUnloadReportStates, int>(ReelUnloadReportStates.Unknown, 0),
            new Pair<ReelUnloadReportStates, int>(ReelUnloadReportStates.Unknown, 0)
        };
        protected RobotActionStates previousReelHandlerActionState = RobotActionStates.Unknown;
        protected CommunicationStates previousReelTowerState = CommunicationStates.None;
        protected Dictionary<string, ModbusMasterController> hmis = new Dictionary<string, ModbusMasterController>();

        protected ModbusMasterController hmi1 => hmis.ContainsKey("hmi1") ? hmis["hmi1"] : null;
        protected ModbusMasterController hmi2 => hmis.ContainsKey("hmi2") ? hmis["hmi2"] : null;
        protected ModbusMasterController hmi3 => hmis.ContainsKey("hmi3") ? hmis["hmi3"] : null;

        public FormMonitorStages()
        {
            InitializeComponent();
        }

        protected virtual void OnFormLoad(object sender, EventArgs e)
        {
            ShowOnScreen(1);
            SetDisplayLanguage();

            Utility.SetDoubleBuffered(listBoxOutputStage1, true);
            Utility.SetDoubleBuffered(listBoxOutputStage2, true);
            Utility.SetDoubleBuffered(listBoxOutputStage3, true);

            if (backgroundWorker.IsBusy != true)
                backgroundWorker.RunWorkerAsync();

            if (Config.Hmis != null)
            {
                foreach (KeyValuePair<string, FiveField<string, string, TechFloor.Device.CommunicationIo.ModbusMasterController.Protocols, SerialPortSettings, EthernetPortSettings>> item in Config.Hmis)
                {
                    hmis.Add(item.Key, new ModbusMasterController(item.Value.first, item.Value.second, item.Value.third, item.Value.fourth));
                    hmis.Last().Value.Initialize();
                }
            }
        }

        protected virtual void ShowOnScreen(int screenNumber)
        {
            Screen[] screens = Screen.AllScreens;

            if (screenNumber >= 0 && screenNumber < screens.Length)
            {
                bool maximised = false;
                if (WindowState == FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Normal;
                    maximised = true;
                }

                Location = screens[screenNumber].WorkingArea.Location;

                if (maximised)
                {
                    WindowState = FormWindowState.Maximized;
                }
            }
        }

        protected void SetDisplayLanguage()
        {
            labelOutputStage1User.Text = Properties.Resources.String_Label_User;
            labelOutputStage2User.Text = Properties.Resources.String_Label_User;
            labelOutputStage3User.Text = Properties.Resources.String_Label_User;
            buttonOutputStage1.Text = Properties.Resources.String_Button_OutputStage1;
            buttonOutputStage2.Text = Properties.Resources.String_Button_OutputStage2;
            buttonOutputStage3.Text = Properties.Resources.String_Button_OutputStage3;
        }
        protected void UpdateReelHandlerStatus()
        {
            string reelsize_ = string.Empty;

            if ((App.MainSequence as ReelTowerRobotSequence).RobotSequenceManager.IsFailure)
            {
                if (labelReelHandlerStatus.BackColor != Color.Red)
                {
                    pictureBoxReelHandler.BackgroundImage = Properties.Resources.robot_2_red;
                    labelReelHandlerStatus.BackColor = Color.Red;
                    labelReelHandlerStatus.ForeColor = Color.Yellow;
                    labelReelHandlerStatus.Text = Properties.Resources.String_FormMain_Robot_Action_State_Failed;
                }
            }
            else
            {
                if ((App.MainSequence as ReelTowerRobotSequence).RobotSequenceManager.ActionState != previousReelHandlerActionState)
                {
                    labelReelHandlerStatus.BackColor = Color.Lime;
                    labelReelHandlerStatus.ForeColor = SystemColors.ControlText;

                    switch ((App.MainSequence as ReelTowerRobotSequence).RobotSequenceManager.ActionState)
                    {
                        case RobotActionStates.Stop:
                            pictureBoxReelHandler.BackgroundImage = Properties.Resources.robot_2_black;
                            labelReelHandlerStatus.Text = Properties.Resources.String_FormMain_Robot_Action_State_Stopped;
                            break;
                        case RobotActionStates.Loading:
                            pictureBoxReelHandler.BackgroundImage = Properties.Resources.robot_2_green;
                            labelReelHandlerStatus.Text = Properties.Resources.String_FormMain_Robot_Action_State_Do_Loading;
                            break;
                        case RobotActionStates.LoadCompleted:
                            pictureBoxReelHandler.BackgroundImage = Properties.Resources.robot_2_blue;
                            labelReelHandlerStatus.Text = Properties.Resources.String_FormMain_Robot_Action_State_Completed_Load;
                            break;
                        case RobotActionStates.Unloading:
                            pictureBoxReelHandler.BackgroundImage = Properties.Resources.robot_2_green;
                            labelReelHandlerStatus.Text = Properties.Resources.String_FormMain_Robot_Action_State_Do_Unloading;
                            break;
                        case RobotActionStates.UnloadCompleted:
                            pictureBoxReelHandler.BackgroundImage = Properties.Resources.robot_2_blue;
                            labelReelHandlerStatus.Text = Properties.Resources.String_FormMain_Robot_Action_State_Completed_Unload;
                            break;
                        default:
                            {
                                if ((App.MainSequence as ReelTowerRobotSequence).RobotSequenceManager.ActionState == RobotActionStates.Unknown)
                                {
                                    pictureBoxReelHandler.BackgroundImage = Properties.Resources.robot_2_black;
                                    labelReelHandlerStatus.BackColor = SystemColors.ControlDarkDark;
                                    labelReelHandlerStatus.ForeColor = SystemColors.Window;
                                }

                                labelReelHandlerStatus.Text = (App.MainSequence as ReelTowerRobotSequence).RobotSequenceManager.ActionState.ToString().ToUpper();
                            }
                            break;
                    }
                }

                previousReelHandlerActionState = (App.MainSequence as ReelTowerRobotSequence).RobotSequenceManager.ActionState;
            }
        }

        protected void UpdateReelTowerStatus()
        {
            try
            {
                if (previousReelTowerState != (App.MainSequence as ReelTowerRobotSequence).ReelTowerManager.CommunicationState)
                {
                    labelReelTowerStatus.ForeColor = SystemColors.ControlText;

                    switch (previousReelTowerState = (App.MainSequence as ReelTowerRobotSequence).ReelTowerManager.CommunicationState)
                    {
                        case CommunicationStates.None:
                            {
                                pictureBoxReelTower.BackgroundImage = Properties.Resources.transfer_black;
                                labelReelTowerStatus.BackColor = SystemColors.Window;
                                labelReelTowerStatus.Text = string.Empty;
                            }
                            break;
                        case CommunicationStates.Listening:
                            {
                                pictureBoxReelTower.BackgroundImage = Properties.Resources.transfer_blue;
                                labelReelTowerStatus.BackColor = SystemColors.Info;
                                labelReelTowerStatus.Text = Properties.Resources.String_FormMain_Communication_State_Listen;
                            }
                            break;
                        case CommunicationStates.Connecting:
                            {
                                pictureBoxReelTower.BackgroundImage = Properties.Resources.transfer_yellow;
                                labelReelTowerStatus.BackColor = Color.LightBlue;
                                labelReelTowerStatus.Text = Properties.Resources.String_FormMain_Communication_State_Connecting;
                            }
                            break;
                        case CommunicationStates.Accepted:
                            {
                                pictureBoxReelTower.BackgroundImage = Properties.Resources.transfer_yellow;
                                labelReelTowerStatus.BackColor = Color.LightGreen;
                                labelReelTowerStatus.Text = Properties.Resources.String_FormMain_Communication_State_Accepted;
                            }
                            break;
                        case CommunicationStates.Connected:
                            {
                                pictureBoxReelTower.BackgroundImage = Properties.Resources.transfer_green;
                                labelReelTowerStatus.BackColor = Color.Lime;
                                labelReelTowerStatus.Text = Properties.Resources.String_FormMain_Communication_State_Connected;
                            }
                            break;
                        case CommunicationStates.Disconnected:
                            {
                                pictureBoxReelTower.BackgroundImage = Properties.Resources.transfer_black;
                                labelReelTowerStatus.BackColor = SystemColors.ControlDarkDark;
                                labelReelTowerStatus.ForeColor = SystemColors.Window;
                                labelReelTowerStatus.Text = Properties.Resources.String_FormMain_Communication_State_Disconnected;
                            }
                            break;
                        case CommunicationStates.Error:
                            {
                                pictureBoxReelTower.BackgroundImage = Properties.Resources.transfer_red;
                                labelReelTowerStatus.BackColor = Color.Red;
                                labelReelTowerStatus.ForeColor = Color.Yellow;
                                labelReelTowerStatus.Text = Properties.Resources.String_FormMain_Communication_State_Socket_Error;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void UpdateRejectStageStatus()
        {
            if ((App.MainSequence as ReelTowerRobotSequence).ReelsOfRejectStage.Count <= 0)
            {
                labelRejectStatgeStatus.Text = string.Empty;
            }
            else if ((App.MainSequence as ReelTowerRobotSequence).ReelsOfRejectStage.Count != previousRejects)
            { 
                FourField<string, string, string, ReelUnloadReportStates> item_ = (App.MainSequence as ReelTowerRobotSequence).ReelsOfRejectStage.Last();
                labelRejectStatgeStatus.Text = $"{(App.MainSequence as ReelTowerRobotSequence).ReelsOfRejectStage.Count}";
                previousRejects = (App.MainSequence as ReelTowerRobotSequence).ReelsOfRejectStage.Count;
            }
        }

        protected void UpdateOutputStageStatus()
        {
            int index_ = 0;
            int dpos_ = 0;
            int completed_ = 0;
            string val_ = string.Empty;
            string material_ = string.Empty;
            Label name_ = labelOutputStage1User;
            Label pid_ = labelOutputStage1PID;
            Label state_ = labelOutputStage1Status;
            Label count_ = labelOutputStage1Count;
            Label elapsed_ = labelOutputStage1Elapsed;
            Label prediction_ = labelOutputStage1Prediction;
            ListBox items_ = listBoxOutputStage1;

            foreach (KeyValuePair<int, MaterialPackage> item_ in (App.MainSequence as ReelTowerRobotSequence).ReelOfOutputStages)
            {
                switch (index_ = item_.Key)
                {
                    default:
                        continue;
                    case 0:
                        {
                            name_ = labelOutputStage1User;
                            pid_ = labelOutputStage1PID;
                            state_ = labelOutputStage1Status;
                            count_ = labelOutputStage1Count;
                            elapsed_ = labelOutputStage1Elapsed;
                            prediction_ = labelOutputStage1Prediction;
                            items_ = listBoxOutputStage1;
                        }
                        break;
                    case 1:
                        {
                            name_ = labelOutputStage2User;
                            pid_ = labelOutputStage2PID;
                            state_ = labelOutputStage2Status;
                            count_ = labelOutputStage2Count;
                            elapsed_ = labelOutputStage2Elapsed;
                            prediction_ = labelOutputStage2Prediction;
                            items_ = listBoxOutputStage2;
                        }
                        break;
                    case 2:
                        {
                            name_ = labelOutputStage3User;
                            pid_ = labelOutputStage3PID;
                            state_ = labelOutputStage3Status;
                            count_ = labelOutputStage3Count;
                            elapsed_ = labelOutputStage3Elapsed;
                            prediction_ = labelOutputStage3Prediction;
                            items_ = listBoxOutputStage3;
                        }
                        break;
                    //case 3:
                    //    {
                    //        name_ = labelOutputStage4User;
                    //        pid_ = labelOutputStage4PID;
                    //        state_ = labelOutputStage4Status;
                    //        count_ = labelOutputStage4Count;
                    //        elapsed_ = labelOutputStage4Elapsed;
                    //        prediction_ = labelOutputStage4Prediction;
                    //        items_ = listBoxOutputStage4;
                    //    }
                    //    break;
                    //case 4:
                    //    {
                    //        name_ = labelOutputStage5User;
                    //        pid_ = labelOutputStage5PID;
                    //        state_ = labelOutputStage5Status;
                    //        count_ = labelOutputStage5Count;
                    //        elapsed_ = labelOutputStage5Elapsed;
                    //        prediction_ = labelOutputStage5Prediction;
                    //        items_ = listBoxOutputStage5;
                    //    }
                    //    break;
                    //case 5:
                    //    {
                    //        name_ = labelOutputStage6User;
                    //        pid_ = labelOutputStage6PID;
                    //        state_ = labelOutputStage6Status;
                    //        count_ = labelOutputStage6Count;
                    //        elapsed_ = labelOutputStage6Elapsed;
                    //        prediction_ = labelOutputStage6Prediction;
                    //        items_ = listBoxOutputStage6;
                    //    }
                    //    break;
                }

                if (item_.Value.IsEmpty)
                {
                    if (item_.Value.PickState != previousOutputs[index_].first)
                    {
                        state_.ForeColor = SystemColors.WindowText;
                        state_.BackColor = SystemColors.Info;
                        name_.Text = string.Empty;
                        pid_.Text = string.Empty;
                        state_.Text = "NONE";
                        count_.Text = "0/0";
                        elapsed_.Text = "00:00:00";
                        prediction_.Text = "00:00:00";
                        items_.Items.Clear();
                        previousOutputs[index_].second = 0;
                    }
                }
                else
                {
                    dpos_ = item_.Value.Name.IndexOf(";", 0);
                    state_.Text = item_.Value.PickState.ToString().ToUpper();
                    prediction_.Text = $"{TimeSpan.FromSeconds(item_.Value.MaterialCount * 18).ToString(@"hh\:mm\:ss")}";

                    if (dpos_ > 0)
                    {
                        pid_.Text = item_.Value.Name.Substring(0, dpos_);
                        name_.Text = item_.Value.Name.Substring(dpos_ + 1, item_.Value.Name.Length - dpos_ - 1);
                    }
                    else
                        pid_.Text = item_.Value.Name;

                    switch (item_.Value.PickState)
                    {
                        case ReelUnloadReportStates.Unknown:
                        case ReelUnloadReportStates.Waiting:
                            {
                                state_.ForeColor = SystemColors.WindowText;
                                state_.BackColor = SystemColors.Info;
                                count_.Text = $"0/{item_.Value.MaterialCount}";
                                elapsedTime = DateTime.MinValue;
                                elapsed_.Text = "00:00:00";
                            }
                            break;
                        case ReelUnloadReportStates.Ready:
                            {
                                if (state_.BackColor != Color.DarkOrange)
                                {
                                    state_.ForeColor = SystemColors.Window;
                                    state_.BackColor = Color.DarkOrange;
                                    count_.Text = $"0/{item_.Value.MaterialCount}";
                                    elapsedTime = DateTime.MinValue;
                                    elapsed_.Text = "00:00:00";
                                    WriteOperatorNameToHmi(index_ + 1, name_.Text);
                                    WriteReelsToHmi(index_ + 1, count_.Text);
                                }

                                if (items_.Items.Count != item_.Value.Materials.Count)
                                    items_.Items.Clear();
                            }
                            break;
                        case ReelUnloadReportStates.Run:
                            {
                                if (elapsedTime == DateTime.MinValue)
                                    elapsedTime = DateTime.Now;

                                if (state_.BackColor != Color.Lime)
                                {
                                    state_.ForeColor = SystemColors.WindowText;
                                    state_.BackColor = Color.Lime;
                                }

                                if (elapsedTime != DateTime.MinValue)
                                    elapsed_.Text = (DateTime.Now - elapsedTime).ToString(@"hh\:mm\:ss");
                            }
                            break;
                        case ReelUnloadReportStates.Complete:
                            {
                                if (state_.BackColor != SystemColors.Info)
                                {
                                    state_.ForeColor = Color.Blue;
                                    state_.BackColor = SystemColors.Info;
                                }
                            }
                            break;
                    }

                    if (items_.Items.Count <= 0 || items_.Items.Count != item_.Value.Materials.Count)
                    {
                        for (int i_ = 0; i_ < item_.Value.Materials.Count; i_++)
                        {
                            if (item_.Value.Materials[i_].first.Length > 10)
                            {
                                dpos_ = item_.Value.Materials[i_].first.IndexOf(";", 10);
                                material_ = item_.Value.Materials[i_].first.Substring(0, dpos_);
                            }
                            else
                                material_ = item_.Value.Materials[i_].first;

                            string[] split_material = item_.Value.Materials[i_].first.Split(';');
                            val_ = $"{split_material[1]} ▶▶▶ {item_.Value.Materials[i_].second}";
                            items_.Items.Add(val_);
                        }
                    }
                    else
                    {
                        if (previousOutputs[index_].first != ReelUnloadReportStates.Complete)
                        {
                            items_.BeginUpdate();

                            if (item_.Value.CompletedCount != previousOutputs[index_].second)
                            {
                                for (int i_ = 0; i_ < item_.Value.Materials.Count; i_++)
                                {
                                    if (item_.Value.Materials[i_].first.Length > 10)
                                    {
                                        dpos_ = item_.Value.Materials[i_].first.IndexOf(";", 10);
                                        material_ = item_.Value.Materials[i_].first.Substring(0, dpos_);
                                    }
                                    else
                                        material_ = item_.Value.Materials[i_].first;

                                    string[] split_material = item_.Value.Materials[i_].first.Split(';');
                                    val_ = $"{split_material[1]} ▶▶▶ {item_.Value.Materials[i_].second}";

                                    if (items_.Items[i_].ToString() != val_)
                                        items_.Items[i_] = val_;

                                    if (item_.Value.Materials[i_].second == ReelUnloadReportStates.Complete)
                                        completed_++;
                                }

                                if (count_.Text != $"{completed_}/{item_.Value.MaterialCount}")
                                {
                                    count_.Text = $"{completed_}/{item_.Value.MaterialCount}";
                                    WriteReelsToHmi(index_ + 1, count_.Text);
                                }

                                previousOutputs[index_].second = completed_;
                            }

                            items_.EndUpdate();

                            if (item_.Value.PickState == ReelUnloadReportStates.Complete)
                            {
                                if (elapsedTime != DateTime.MinValue)
                                    elapsed_.Text = (DateTime.Now - elapsedTime).ToString(@"hh\:mm\:ss");

                                elapsedTime = DateTime.MinValue;
                                previousOutputs[index_].second = 0;
                            }
                        }
                        else
                        {
                            if (count_.Text != $"{item_.Value.MaterialCount}/{item_.Value.MaterialCount}")
                            {
                                count_.Text = $"{item_.Value.MaterialCount}/{item_.Value.MaterialCount}";
                                WriteReelsToHmi(index_ + 1, count_.Text);
                            }
                        }
                    }
                }

                previousOutputs[index_].first = item_.Value.PickState;
            }
        }

        protected void UpdateDisplay()
        {
            UpdateReelHandlerStatus();
            UpdateReelTowerStatus();
            UpdateRejectStageStatus();
            UpdateOutputStageStatus();
        }

        protected void OnDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (!(App.MainForm as FormMain).IsShutdown)
                {
                    if (!App.ShutdownEvent.WaitOne(100) && App.MainSequence != null)
                    {
                        if (InvokeRequired)
                        {
                            BeginInvoke(new Action(() =>
                            {
                                UpdateDisplay();
                            }));
                        }
                        else
                        {
                            UpdateDisplay();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Canceled");
            }
            else if (e.Error != null)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Error={e.Error.Message}");
            }
            else
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Done");
            }
        }

        private void OnClick(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                int index_ = 0;
                string context_ = string.Empty;
                Label name_ = labelOutputStage1User;
                Label pid_ = labelOutputStage1PID;
                Label state_ = labelOutputStage1Status;
                Label count_ = labelOutputStage1Count;
                Label elapsed_ = labelOutputStage1Elapsed;
                Label prediction_ = labelOutputStage1Prediction;
                ListBox items_ = listBoxOutputStage1;

                switch ((sender as Button).Name.ToLower())
                {
                    case "buttonrejectstage":
                        {
                            context_ = string.Format(Properties.Resources.String_Question_ClearMonitorStageContext, "reject");
                        }
                        break;
                    case "buttonoutputstage1":
                    case "buttonoutputstage2":
                    case "buttonoutputstage3":
                    case "buttonoutputstage4":
                    case "buttonoutputstage5":
                    case "buttonoutputstage6":
                        {
                            switch (index_ = Convert.ToInt32((sender as Button).Name.Last().ToString()))
                            {
                                default:
                                    return;
                                case 1:
                                    {
                                        name_ = labelOutputStage1User;
                                        pid_ = labelOutputStage1PID;
                                        state_ = labelOutputStage1Status;
                                        count_ = labelOutputStage1Count;
                                        elapsed_ = labelOutputStage1Elapsed;
                                        prediction_ = labelOutputStage1Prediction;
                                        items_ = listBoxOutputStage1;
                                    }
                                    break;
                                case 2:
                                    {
                                        name_ = labelOutputStage2User;
                                        pid_ = labelOutputStage2PID;
                                        state_ = labelOutputStage2Status;
                                        count_ = labelOutputStage2Count;
                                        elapsed_ = labelOutputStage2Elapsed;
                                        prediction_ = labelOutputStage2Prediction;
                                        items_ = listBoxOutputStage2;
                                    }
                                    break;
                                case 3:
                                    {
                                        name_ = labelOutputStage3User;
                                        pid_ = labelOutputStage3PID;
                                        state_ = labelOutputStage3Status;
                                        count_ = labelOutputStage3Count;
                                        elapsed_ = labelOutputStage3Elapsed;
                                        prediction_ = labelOutputStage1Prediction;
                                        items_ = listBoxOutputStage3;
                                    }
                                    break;
                                //case 4:
                                //    {
                                //        name_ = labelOutputStage4User;
                                //        pid_ = labelOutputStage4PID;
                                //        state_ = labelOutputStage4Status;
                                //        count_ = labelOutputStage4Count;
                                //        elapsed_ = labelOutputStage4Elapsed;
                                //        prediction_ = labelOutputStage4Prediction;
                                //        items_ = listBoxOutputStage4;
                                //    }
                                //    break;
                                //case 5:
                                //    {
                                //        name_ = labelOutputStage5User;
                                //        pid_ = labelOutputStage5PID;
                                //        state_ = labelOutputStage5Status;
                                //        count_ = labelOutputStage5Count;
                                //        elapsed_ = labelOutputStage5Elapsed;
                                //        prediction_ = labelOutputStage5Prediction;
                                //        items_ = listBoxOutputStage5;
                                //    }
                                //    break;
                                //case 6:
                                //    {
                                //        name_ = labelOutputStage6User;
                                //        pid_ = labelOutputStage6PID;
                                //        state_ = labelOutputStage6Status;
                                //        count_ = labelOutputStage6Count;
                                //        elapsed_ = labelOutputStage6Elapsed;
                                //        prediction_ = labelOutputStage6Prediction;
                                //        items_ = listBoxOutputStage6;
                                //    }
                                //    break;
                            }

                            context_ = string.Format(Properties.Resources.String_Question_ClearMonitorStageContext, $"{Properties.Resources.String_Button_OutputStage1.Split(' ')[0]} {index_}");
                        }
                        break;
                }

                if (FormMessageExt.ShowQuestion(context_) == DialogResult.Yes)
                {
                    if (index_ <= 0)
                    {
                        if ((App.MainSequence as ReelTowerRobotSequence).CleanUpRejectStage(index_))
                            labelRejectStatgeStatus.Text = string.Empty;
                    }
                    else
                    {
                        if ((App.MainSequence as ReelTowerRobotSequence).CleanUpOutputStage(index_))
                        {
                            name_.Text = string.Empty;
                            pid_.Text = string.Empty;
                            state_.Text = "NONE";
                            count_.Text = "0/0";
                            prediction_.Text = "00:00:00";
                            elapsed_.Text = "00:00:00";
                            items_.Items.Clear();
                        }
                    }
                }
            }

           (App.MainForm as FormMain).SetFocus();
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
           (App.MainForm as FormMain).SetFocus();
        }

        protected void WriteOperatorNameToHmi(int index, string data)
        {
            string text = string.Empty;
            ModbusMasterController ctl = null;

            switch (index)
            {
                case 1: ctl = hmi1; break;
                case 2: ctl = hmi2; break;
                case 3: ctl = hmi3; break;
            }

            if (ctl != null)
            {
                int i = 0;
                char[] values = data.ToCharArray();
                ushort[] result = new ushort[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

                foreach (char letter in values)
                {
                    if (i < 10)
                        result[i++] = Convert.ToUInt16(letter);
                    else
                        break;
                }

                ctl.WriteMultipleRegistersAsync(1, 0, result.ToArray());
            }
        }

        protected void WriteReelsToHmi(int index, string data)
        {
            ModbusMasterController ctl = null;

            switch (index)
            {
                case 1: ctl = hmi1; break;
                case 2: ctl = hmi2; break;
                case 3: ctl = hmi3; break;
            }

            if (ctl != null)
            {
                int i = 0;
                char[] values = data.ToCharArray();
                ushort[] result = new ushort[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

                foreach (char letter in values)
                {
                    if (i < 10)
                        result[i++] = Convert.ToUInt16(letter);
                    else
                        break;
                }

                ctl.WriteMultipleRegistersAsync(1, 10, result.ToArray());
            }
        }

        protected void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (KeyValuePair<string, ModbusMasterController> item in hmis)
            {
                item.Value.Dispose();
            }
        }
    }
}
#endregion