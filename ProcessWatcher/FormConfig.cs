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
/// project ProcessWatcher
/// @author Marcus (isadrastea.kor@gmail.com)
/// @namespace ProcessWatcher
/// @file FormConfig.cs
/// @brief
/// @details
/// @date 2020-3-11 오후 5:07 
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using TechFloor.Object;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security;
using System.Windows.Forms;
using System.Xml;
#endregion

#region Program
namespace ProcessWatcher
{
    public partial class FormConfig : Form
    {
        #region Fields
        protected string watchProcess = string.Empty;

        protected int watchInterval = 1;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public FormConfig()
        {
            InitializeComponent();
        }
        #endregion

        #region Protected methods
        #region Form event handlers
        protected  void OnFormLoad(object sender, EventArgs e)
        {
        }

        protected  void OnFormShown(object sender, EventArgs e)
        {
            try
            {
                if (Program.ConfigObject.Load(typeof(Program).Assembly.GetName().Version.ToString()))
                {
                    foreach (var item_ in comboBoxAlarmMessageCultureValue.Items)
                    {
                        if (item_.ToString().Contains(Program.ConfigObject.CultureCode))
                        {
                            comboBoxAlarmMessageCultureValue.SelectedItem = item_;
                            break;
                        }
                    }
                }

                foreach (KeyValuePair<string, Pair<string, int>> item_ in Program.ConfigObject.Processes)
                {
                    watchProcess = item_.Key;
                    labelProcessPathValue.Text = item_.Value.first;
                    watchInterval = item_.Value.second;
                    break;
                }

                if (Program.ConfigObject.RunMode == RunModes.Simulation)
                {
                    checkBoxSimulationMode.Checked = true;
                    checkBoxSimulationMode.Visible = true;
                }

                textBoxLineCodeValue.Text = Program.ConfigObject.LineCode;
                textBoxProcessCodeValue.Text = Program.ConfigObject.ProcessCode;
                textBoxEquipmentIdValue.Text = Program.ConfigObject.EquipmentId;
                labelAlarmListValue.Text = Program.ConfigObject.AlarmList;
                textBoxListenerAddressValue.Text = Program.ConfigObject.ListenerAddress;
                numericUpDownListenerValue.Value = Program.ConfigObject.ListenerPort;

                if (Program.AlarmManagerObject.AlarmList.Count > 0)
                {
                    listViewAlarmList.Items.Clear();
                    listViewAlarmList.BeginUpdate();

                    foreach (AlarmData obj_ in Program.AlarmManagerObject.AlarmList.Values)
                    {
                        int index_ = listViewAlarmList.Items.Count;
                        listViewAlarmList.Items.Add(obj_.Code.ToString());
                        listViewAlarmList.Items[index_].SubItems.Add(obj_.Extra);
                        listViewAlarmList.Items[index_].SubItems.Add(obj_.Severity.ToString());
                        listViewAlarmList.Items[index_].SubItems.Add(obj_.Enabled.ToString());
                        listViewAlarmList.Items[index_].SubItems.Add(obj_.Message);
                    }

                    listViewAlarmList.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected  void OnFormActivated(object sender, EventArgs e)
        {   
        }
        #endregion

        #region Control event handlers
        protected void OnComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            if (sender != null && sender is ComboBox)
            {
                ComboBox ctrl_ = sender as ComboBox;

                try
                {
                    e.DrawBackground();

                    if (e.Index >= 0)
                    {
                        using (StringFormat sf_ = new StringFormat())
                        {
                            sf_.LineAlignment = StringAlignment.Center;
                            sf_.Alignment = StringAlignment.Center;
                            Brush brush_ = new SolidBrush(ctrl_.ForeColor);

                            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                                brush_ = SystemBrushes.HighlightText;

                            e.Graphics.DrawString(ctrl_.Items[e.Index].ToString(), ctrl_.Font, brush_, e.Bounds, sf_);
                            brush_.Dispose();
                            brush_ = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
            }
        }

        protected  void OnClickButtonSaveConfig(object sender, EventArgs e)
        {
            try
            {
                string str_ = string.Empty;

                switch (comboBoxAlarmMessageCultureValue.SelectedIndex)
                {
                    case 0:
                        str_ = "en-US";
                        break;
                    case 1:
                        str_ = "ko-KR";
                        break;
                }

                Program.ConfigObject.RunMode = checkBoxSimulationMode.Checked ? RunModes.Simulation : RunModes.Normal;
                Program.ConfigObject.CultureCode = str_;
                Program.ConfigObject.LineCode = textBoxLineCodeValue.Text;
                Program.ConfigObject.ProcessCode = textBoxProcessCodeValue.Text;
                Program.ConfigObject.EquipmentId = textBoxEquipmentIdValue.Text;
                Program.ConfigObject.AlarmList = labelAlarmListValue.Text;
                Program.ConfigObject.ListenerAddress = textBoxListenerAddressValue.Text;
                Program.ConfigObject.ListenerPort = Convert.ToInt32(numericUpDownListenerValue.Value);

                if (!string.IsNullOrEmpty(watchProcess))
                    Program.ConfigObject.AddWatchProcess(watchProcess, labelProcessPathValue.Text, watchInterval);

                Program.ConfigObject.Save();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected  void OnClickButtonProcessPath(object sender, EventArgs e)
        {
            OpenFileDialog filedlg_ = new OpenFileDialog()
            {
                FileName = "Select watch process file",
                Filter = "Executable (*.exe)|*.exe",
                Title = "Select a file"
            };

            if (filedlg_.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filepath_ = filedlg_.FileName;

                    if (File.Exists(filepath_))
                        labelProcessPathValue.Text = filepath_;
                }
                catch (SecurityException ex)
                {
                    Debug.WriteLine($"Security error: Exception={ex.Message}, Details={ex.StackTrace}");
                }
            }
        }

        protected  void OnClickButtonAlarmListPath(object sender, EventArgs e)
        {
            OpenFileDialog filedlg_ = new OpenFileDialog()
            {
                FileName = "Select alarm list file",
                Filter = "Extended markup (*.xml)|*.xml",
                Title = "Select a file"
            };

            if (filedlg_.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filepath_ = filedlg_.FileName;

                    if (File.Exists(filepath_))
                        labelAlarmListValue.Text = filepath_;
                }
                catch (SecurityException ex)
                {
                    Debug.WriteLine($"Security error: Exception={ex.Message}, Details={ex.StackTrace}");
                }
            }
        }
        #endregion
        #endregion
    }
}
#endregion