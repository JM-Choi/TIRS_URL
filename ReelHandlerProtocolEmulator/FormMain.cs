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
/// project ReelHandlerProtocolEmulator
/// @author Marcus (isadrastea.kor@gmail.com)
/// @namespace ReelHandlerProtocolEmulator
/// @file Form1.cs
/// @brief
/// @details
/// @date 2020-2-13 오전 11:57 
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using Marcus.Solution.TechFloor;
using Marcus.Solution.TechFloor.Components;
using Marcus.Solution.TechFloor.Device;
using Marcus.Solution.TechFloor.Gui;
using Marcus.Solution.TechFloor.Object;
using Marcus.Solution.TechFloor.Util;
using System;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Windows.Forms;
#endregion

#region Program
namespace ReelHandlerProtocolEmulator
{
    public partial class FormMain : Form, IFormMain
    {
        protected IPEndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.0.20"), 7000);
        protected ReelHandlerManager reelHandler => Singleton<ReelHandlerManager>.Instance;
        protected App appInstance = null;
        protected ManualResetEvent shutdownEvent = new ManualResetEvent(false);
        
        public OperationStates OperationState => throw new NotImplementedException();

        public AlarmStates AlarmState => throw new NotImplementedException();

        public IMainSequence MainSequence => throw new NotImplementedException();

        public IDigitalIoManager DigitalIoManager => throw new NotImplementedException();

        public WaitHandle ShutdownEvent => shutdownEvent;

        public FormMain(App app = null)
        {
            InitializeComponent();
            appInstance = app;
            Logger.Create();
            reelHandler.CommunicationStateChanged += OnChangedCommunicationState;
            reelHandler.RequestCommandReceived += OnReceivedRequestCommand;
            reelHandler.ReceivedMaterialPackage += OnReceivedMaterialPackage;
            reelHandler.ReportRuntimeLog += OnLog;
            reelHandler.ReportException += OnLog;
        }

        protected void OnChangedCommunicationState(object sender, CommunicationStates state)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateCommunicationState(state); }));
            else
                UpdateCommunicationState(state);
        }

        protected void UpdateCommunicationState(CommunicationStates state)
        { 
            switch (state)
            {
                case Marcus.Solution.TechFloor.CommunicationStates.Connected:
                    {
                        button1.BackColor = Color.Lime;
                        textBoxReelTowerGroupAddress.Enabled = false;
                        numericUpDownReelTowerGroupPort.Enabled = false;
                    }
                    break;
                default:
                    {
                        button1.BackColor = SystemColors.Control;
                        textBoxReelTowerGroupAddress.Enabled = true;
                        numericUpDownReelTowerGroupPort.Enabled = true;
                    }
                    break;
            }

            button1.Text = button1.BackColor == Color.Lime ? "STOP" : "START";
        }

        protected void OnReceivedRequestCommand(object sender, Pair<ReelTowerCommands, MaterialStorageMessage> arg)
        {
            switch (arg.first)
            {
                case ReelTowerCommands.REQUEST_TOWER_STATE:
                    {
                        if (!string.IsNullOrEmpty(arg.second.TowerId))
                            reelHandler.SendMessage(ReelTowerCommands.REPLY_TOWER_STATE, null, arg.second.TowerId, "IDLE;DOWN;DOWN;DOWN");
                    }
                    break;
            }
        }

        protected void OnReceivedMaterialPackage(object sender, MaterialPackage arg)
        {

        }

        protected void OnLog(object sender, string log)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateLog(log); }));
            else
                UpdateLog(log);
        }

        protected void UpdateLog(string log)
        {
            listView1.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            listView1.Items[listView1.Items.Count - 1].SubItems.Add(log);
        }

        protected void ChangeReelHandlerCommunication()
        {
            if (reelHandler.CommunicationState != CommunicationStates.Connected)
            {
                if (!string.IsNullOrEmpty(textBoxReelTowerGroupAddress.Text))
                {
                    ep.Address = IPAddress.Parse(textBoxReelTowerGroupAddress.Text);
                    ep.Port = Convert.ToInt32(numericUpDownReelTowerGroupPort.Value);
                    reelHandler.Connect(ep.Address, ep.Port);
                }
            }
            else
                reelHandler.Disconnect();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {   
        }

        private void OnFormShown(object sender, EventArgs e)
        {
            textBoxReelTowerGroupAddress.Text = ep.Address.ToString();
            numericUpDownReelTowerGroupPort.Value = ep.Port;
            reelHandler.Create(ep);
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            reelHandler.Destroy();
            reelHandler.Dispose();
            Logger.Destroy();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {   
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangeReelHandlerCommunication();
        }
    }
}
#endregion