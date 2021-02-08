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
/// @file DigitalIoManager.cs
/// @brief
/// @details
/// @date 2019, 5, 21, 오후 1:13
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using Shared;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using Marcus.Solution.TechFloor.Object;
using Marcus.Solution.TechFloor.Util;
using System.Runtime.ExceptionServices;
using System.Security;
#endregion

#region Program
namespace Marcus.Solution.TechFloor
{
    public class DigitalIoManager : SimulatableDevice, IDigitalIoManager
    {
        #region Enumerations
        public enum CartClamps
        {
            All,
            Side,
            Rear,
        }

        public enum Inputs
        {
            CartInCheck1 = 0,    // Work zone sensor 1
            CartInCheck2 = 1,   // Work zone sensor 2
            Cylinder1Forward = 2,
            Cylinder1Backward = 3,
            Cylinder2Forward = 4,
            Cylinder2Backward = 5,
            ReelType7 = 6,
            ReelType13 = 7,
            BufferCartDetection1 = 8,    // Cart detection sensor 1
            BufferCartDetection2 = 9,    // Cart detection sensor 2
            CartAlignCylinder1Forward = 10,
            CartAlignCylinder1Backward = 11,
            CartAlignCylinder2Forward = 12,
            CartAlignCylinder2Backward = 13,
            MainAir = 14,
        }
		
        public enum Outputs
        {
            LampRed = 0,
            LampYellow = 1,
            LampGreen = 2,
            Buzzer = 3,
            Cylinder1 = 4,
            Cylinder2 = 5,
            Cylinder3 = 6,
            Cylinder4 = 7,
        }
        #endregion

        #region Fields
        protected Thread innerThread = null;
        protected bool stopThread = false;
        protected bool initialized = false;

        protected bool[] inputs =
        {
            false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false,
        };

        protected bool[] outputs =
        {
            false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false,
        };
        #endregion

        #region Properties
        public bool Initialized                     => initialized;
        public bool IsRunningThread                 => !stopThread;
        public bool WorkZoneCartPresentSensor1      => inputs[0];
        public bool WorkZoneCartPresentSensor2      => inputs[1];
        public bool Cylinder1Forward                => inputs[2];
        public bool Cylinder1Backward               => inputs[3];
        public bool Cylinder2Forward                => inputs[4];
        public bool Cylinder2Backward               => inputs[5];
        public bool ReturnStageReelPresentSensor7   => inputs[6];
        public bool ReturnStageReelPresentSensor13  => inputs[7];
        public bool BufferZoneCartPresentSensor1    => inputs[8];
        public bool BufferZoneCartPresentSensor2    => inputs[9];
        public bool Cylinder3Forward                => inputs[10];
        public bool Cylinder3Backward               => inputs[11];
        public bool Cylinder4Forward                => inputs[12];
        public bool Cylinder4Backward               => inputs[13];
        public bool MainAirSupply                   => inputs[14];
        public bool SignalTowerRed
        {
            get => outputs[0];
            set
            {
                SetOutput(0, value ? 1 : 0);
            }
        }
        public bool SignalTowerYellow
        {
            get => outputs[1];
            set
            {
                SetOutput(1, value ? 1 : 0);
            }
        }
        public bool SignalTowerGreen
        {
            get => outputs[2];
            set
            {
                SetOutput(2, value ? 1 : 0);
            }
        }
        public bool Buzzer
        {
            get => outputs[3];
            set
            {
                SetOutput(3, value ? 1 : 0);
            }
        }
        public bool Cylinder1
        {
            get => outputs[4];
            set
            {
                SetOutput(4, value ? 1 : 0);
            }
        }
        public bool Cylinder2
        {
            get => outputs[5];
            set
            {
                SetOutput(5, value ? 1 : 0);
            }
        }
        public bool Cylinder3
        {
            get => outputs[4];
            set
            {
                SetOutput(6, value ? 1 : 0);
            }
        }
        public bool Cylinder4
        {
            get => outputs[5];
            set
            {
                SetOutput(7, value ? 1 : 0);
            }
        }
        public bool IsCartInWorkZone    => WorkZoneCartPresentSensor1 && WorkZoneCartPresentSensor2;
        public bool IsCartInBufferZone  => BufferZoneCartPresentSensor1 && BufferZoneCartPresentSensor2;
        public bool IsCartClamped => (Cylinder1Forward && Cylinder2Forward) & (Cylinder3Forward && Cylinder4Forward && MainAirSupply);
        //public bool IsCartClamped => Cylinder1Forward && Cylinder2Forward;
        public bool IsCartClamping => (Cylinder1Forward || Cylinder2Forward) & (Cylinder3Forward && Cylinder4Forward && MainAirSupply);
        //public bool IsCartClamping => (Cylinder1Forward || Cylinder2Forward);
        public bool IsCartReleased => (Cylinder1Backward && Cylinder2Backward) & (Cylinder3Backward && Cylinder4Backward && MainAirSupply);
        //public bool IsCartReleased => Cylinder1Backward && Cylinder2Backward;
        public bool IsCartReleasing => (Cylinder1Backward || Cylinder2Backward) & (Cylinder4Backward && Cylinder4Backward && MainAirSupply);
        //public bool IsCartReleasing => (Cylinder1Backward || Cylinder2Backward);
        public bool IsReturnReelExist   => ReturnStageReelPresentSensor7 || ReturnStageReelPresentSensor13;
        public bool SignalTowerBlue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool SignalTowerWhite { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsSimulation        => simulation;
        #endregion

        #region Events
        public virtual event EventHandler<int> InputSignalChanged;
        public virtual event EventHandler<int> OutputSignalChanged;
        #endregion

        #region Protected methods
        protected void Run()
        {
            Logger.Trace($"Started: Thread={MethodBase.GetCurrentMethod().Name}");

            while (!stopThread)
            {
                if (App.ShutdownEvent.WaitOne(10, false))
                {
                    Logger.Trace($"Stopped: Thread={MethodBase.GetCurrentMethod().Name}");
                    stopThread = true;
                }
                else
                {
                    if (!Simulation)
                    {
                        UpdateInputs();
                        UpdateOutputs();
                    }
                }
            }
        }
        #endregion

        #region Public methods
        
        public bool Open()
        {
            int nNumAxes = 0;
            initialized = false;

            try
            {
                if (CMDLL.cmmGnDeviceLoad((int)Defines._TCmBool.cmFALSE, ref nNumAxes) == Defines.cmERR_NONE)
                    initialized = true;
            }
            catch (Exception ex)
            {
                simulation = true;
                Trace.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            initialized |= simulation;
            return initialized;
        }

        
        public bool Close()
        {
            if (!simulation)
            {
                try
                {
                    CMDLL.cmmGnDeviceUnload();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
            }

            return (initialized = false);
        }

        public bool GetInput(Inputs inChannel)
        {
            return GetInput((int)inChannel);
        }

        
        public bool GetInput(int nChannel)
        {
            bool result = false;

            if (15 <= nChannel)
                return false;

            if (simulation)
            {
                result = inputs[nChannel];
            }
            else
            {
                try
                {
                    int nState = (int)Defines._TCmBool.cmFALSE;

                    if (CMDLL.cmmDiGetOne(nChannel, ref nState) == Defines.cmERR_NONE)
                        result = true;

                    if (nState == (int)Defines._TCmBool.cmFALSE)
                        result = false;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
            }

            return result;
        }

        public bool GetOutput(Outputs outChannel)
        {
            return GetOutput((int)outChannel);
        }

        
        public bool GetOutput(int nChannel)
        {
            bool result = false;

            if (15 <= nChannel)
                return false;

            if (simulation)
            {
                result = outputs[nChannel];
            }
            else
            {
                try
                {
                    if (15 <= nChannel) return false;
                    int nState = (int)Defines._TCmBool.cmFALSE;

                    if (CMDLL.cmmDoGetOne(nChannel, ref nState) == Defines.cmERR_NONE)
                        result = true;

                    if (nState == (int)Defines._TCmBool.cmFALSE)
                        result = false;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
            }

            return result;
        }

        public bool SetInput(Inputs outChannel, int nState)
        {
            return SetInput((int)outChannel, nState);
        }

        public bool SetInput(int nChannel, int nState)
        {
            bool result = false;

            if (15 <= nChannel)
                return false;

            if (simulation)
            {
                result = inputs[nChannel] = (nState == 1 ? true : false);
            }

            return result;
        }

        public bool SetInput(int channel, bool state)
        {
            return SetInput(channel, state ? 1 : 0);
        }

        public bool SetOutput(Outputs outChannel, int nState)
        {
            return SetOutput((int)outChannel, nState);
        }

        
        public bool SetOutput(int nChannel, int nState)
        {
            bool result = false;

            if (15 <= nChannel)
                return false;

            if (simulation)
            {
                result = outputs[nChannel] = (nState == 1 ? true : false);
            }
            else
            {
                try
                {
                    if (CMDLL.cmmDoPutOne(nChannel, nState) == Defines.cmERR_NONE)
                        result = true;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
            }

            return result;
        }

        public bool SetOutput(int channel, bool state)
        {
            return SetOutput(channel, state ? 1 : 0);
        }

        public void UpdateInputs()
        {
            try
            {
                int nState = 0;
                CMDLL.cmmDiGetMulti(0, 16, ref nState);

                for (int i = 0; i < 16; i++)
                {
                    bool state = (((nState >> i) & 1) == 1) ? true : false;

                    if (inputs[i] != state)
                    {
                        inputs[i] = state;
                        InputSignalChanged?.Invoke(this, i);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        
        public void UpdateOutputs()
        {
            try
            {
                int nState = 0;
                CMDLL.cmmDoGetMulti(0, 16, ref nState);

                for (int i = 0; i < 16; i++)
                {
                    bool state = (((nState >> i) & 1) == 1) ? true : false;

                    if (outputs[i] != state)
                    {
                        outputs[i] = state;
                        OutputSignalChanged?.Invoke(this, i);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public DigitalIoManager(bool runthread = false)
        {
            if (runthread)
                (innerThread = new Thread(new ThreadStart(Run)))?.Start();
        }

        public void SetSignalTower(OperationStates operationstate, bool buzzeron = false)
        {
            if (simulation)
                return;

            switch (operationstate)
            {
                case OperationStates.Alarm:
                    {
                        SignalTowerRed = true;
                        SignalTowerYellow = false;
                        SignalTowerGreen = false;
                        Buzzer = buzzeron;
                    }
                    break;
                case OperationStates.PowerOn:
                    {
                        SignalTowerRed = true;
                        SignalTowerYellow = true;
                        SignalTowerGreen = true;
                        Buzzer = false;
                    }
                    break;
                case OperationStates.PowerOff:
                    {
                        SignalTowerRed = false;
                        SignalTowerYellow = false;
                        SignalTowerGreen = false;
                        Buzzer = false;
                    }
                    break;
                case OperationStates.Initialize:
                    {
                        if (SignalTowerRed == SignalTowerYellow &&
                            SignalTowerYellow == SignalTowerGreen)
                        {
                            SignalTowerRed = !SignalTowerRed;
                            SignalTowerYellow = !SignalTowerYellow;
                            SignalTowerGreen = !SignalTowerGreen;
                        }
                        else
                        {
                            SignalTowerRed = true;
                            SignalTowerYellow = true;
                            SignalTowerGreen = true;
                        }
                    }
                    break;
                case OperationStates.Run:
                    {
                        SignalTowerRed = false;
                        SignalTowerYellow = false;
                        SignalTowerGreen = true;
                        Buzzer = false;
                    }
                    break;
                case OperationStates.Stop:
                    {
                        SignalTowerRed = true;
                        SignalTowerYellow = false;
                        SignalTowerGreen = false;
                        Buzzer = false;
                    }
                    break;
                case OperationStates.Pause:
                    {
                        SignalTowerRed = false;
                        SignalTowerYellow = true;
                        SignalTowerGreen = false;
                        Buzzer = buzzeron;
                    }
                    break;
            }
        }

        public void ClampCart(CartClamps type = CartClamps.All)
        {
            switch (type)
            {
                case CartClamps.All:
                    {
                        Cylinder1 = true;
                        Cylinder2 = true;
                        Cylinder3 = true;
                        Cylinder4 = true;
                    }
                    break;
                case CartClamps.Side:
                    {
                        Cylinder1 = true;
                        Cylinder2 = true;
                    }
                    break;
                case CartClamps.Rear:
                    {
                        Cylinder3 = true;
                        Cylinder4 = true;
                    }
                    break;
            }
        }

        public void ReleaseCart(CartClamps type = CartClamps.All)
        {
            switch (type)
            {
                case CartClamps.All:
                    {
                        Cylinder1 = false;
                        Cylinder2 = false;
                        Cylinder3 = false;
                        Cylinder4 = false;
                    }
                    break;
                case CartClamps.Side:
                    {
                        Cylinder1 = false;
                        Cylinder2 = false;
                    }
                    break;
                case CartClamps.Rear:
                    {
                        Cylinder3 = false;
                        Cylinder4 = false;
                    }
                    break;
            }
        }
        #endregion
    }
}
#endregion