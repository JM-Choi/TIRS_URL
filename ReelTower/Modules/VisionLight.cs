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
/// @namespace Marcus.Solution.TechFloor.Components
/// @file VisionLight.cs
/// @brief
/// @details
/// @date 2019, 5, 21, 오후 1:13
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using Marcus.Solution.TechFloor.Device.CommunicationIo.SerialIo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Reflection;
using System.Text;
#endregion

#region Program
namespace Marcus.Solution.TechFloor.Components
{
    public class VisionLight : AbstractClassSerialIo
    {
        #region Fields
        protected List<int> valuesOfChannel1 = new List<int>();
        #endregion

        #region Properties
        public List<int> ValuesOfChannel1 => valuesOfChannel1;
        #endregion

        #region Constructors
        public VisionLight(int port = 3, int baudRate = 57600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, Handshake handshake = Handshake.None, bool dtrenable = true, bool rtsenable = true, bool periodicTask = false)
            : base(port, baudRate, parity, dataBits, stopBits, handshake, dtrenable, rtsenable, periodicTask)
        {
        }

        public VisionLight(int port, int baudRate, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, bool periodicTask = false)
            : base(port, baudRate, parity, dataBits, stopBits, Handshake.None, false, false, periodicTask)
        {
        }
        #endregion

        #region Protected methods
        protected override void OnReceived(string data)
        {
            Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: {data}");
        }
        #endregion

        #region Public methods
        public override int Send(string data)
        {
            return Send(Encoding.ASCII.GetBytes(data), 0)? 0 : -1;
        }
        #endregion
    }

    public class VisionProcessEventArgs : EventArgs
    {
        public int ProcessType = 0;
        public int TriggerDelay = 1000;
        public int LightCategory = 0;
        public double CenterXOffsetLimit = 10;
        public double CenterYOffsetLimit = 10;
        public ReelTypes ReelType = ReelTypes.Unknown;
        //public readonly VisionProcessDataObjectTypes ProcessDataType = VisionProcessDataObjectTypes.ProcessArgument;

        // public VisionProcessEventArgs(int model, int delay, int ptype, ReelTypes rtype, VisionProcessDataObjectTypes dtype = VisionProcessDataObjectTypes.ProcessArgument)
        // {
        //     ProcessType = ptype;
        //     TriggerDelay = delay;
        //     LightCategory = model;
        //     ReelType = rtype;
        //     ProcessDataType = dtype;
        // }

        // public VisionProcessEventArgs(int model, int delay, int ptype, ReelTypes rtype, double xlimit, double ylimit, VisionProcessDataObjectTypes dtype = VisionProcessDataObjectTypes.ProcessArgument)
        // {
        //     ProcessType = ptype;
        //     TriggerDelay = delay;
        //     LightCategory = model;
        //     ReelType = rtype;
        //     ProcessDataType = dtype;
        //     CenterXOffsetLimit = xlimit;
        //     CenterYOffsetLimit = ylimit;
        // }
    }
}
#endregion