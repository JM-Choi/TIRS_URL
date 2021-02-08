#region Imports
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Runtime.ExceptionServices;
using System.Security;
using Shared;
using TechFloor.Object;
using TechFloor.Util;
using System.Threading.Tasks;
#endregion

#region Program
namespace TechFloor
{
    public class DigitalIoManager : SimulatableDevice, IDigitalIoManager
    {
        #region Enumerations
        public enum CartClamps
        {
            All,
            Side,
            Hook,
            Pull,
        }

        public enum Inputs
        {
            CartInCheck1 = 0,                   // Work zone sensor 1
            CartInCheck2 = 1,                   // Work zone sensor 2
            CartAlignSensorLeftForward = 2,     // Cart align sensor left
            CartAlignSensorLeftBackward = 3,    // Cart align sensor left
            CartAlignSensorRightForward = 4,    // Cart align sensor right
            CartAlignSensorRightBackward = 5,   // Cart align sensor right
            ReelType7 = 6,
            ReelType13 = 7,
            BufferCartDetection1 = 8,           // Cart detection sensor 1
            BufferCartDetection2 = 9,           // Cart detection sensor 2
            CartAlignSensorFrontXForward = 10,  // Cart align sensor front X
            CartAlignSensorFrontXBackward = 11, // Cart align sensor front X
            CartAlignSensorFrontYForward = 12,  // Cart align sensor front Y
            CartAlignSensorFrontYBackward = 13, // Cart align sensor front Y
            MainAir = 14,

            OutputStage1Exist = 16,             // New model (Version 1.0.1.0)
            OutputStage2Exist = 17,             // New model (Version 1.0.1.0)
            OutputStage3Exist = 18,             // New model (Version 1.0.1.0)
            OutputStage4Exist = 19,             // New model (Version 1.0.1.0)
            OutputStage5Exist = 20,             // New model (Version 1.0.1.0)
            OutputStage6Exist = 21,             // New model (Version 1.0.1.0)
            RejectStageFull = 22,               // New model (Version 1.0.1.0)
        }
		
        public enum Outputs
        {
            LampRed = 0,
            LampYellow = 1,
            LampGreen = 2,
            Buzzer = 3,
            CartAlignCylinderLeft = 4,
            CartAlignCylinderRight = 5,
            CartAlignCylinderFrontX = 6,
            CartAlignCylinderFrontY = 7,
        }
        #endregion

        #region Fields
        protected Thread innerThread = null;
        protected bool stopThread = false;
        protected bool initialized = false;
        protected int cartDockStep = 0;

        protected bool[] inputs =
        {
            false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false,
        };

        protected bool[] outputs =
        {
            false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false,
        };
        #endregion

        #region Properties
        public bool Initialized                     => initialized;
        public bool IsRunningThread                 => !stopThread;
        public bool WorkZoneCartPresentSensor1      => inputs[0];
        public bool WorkZoneCartPresentSensor2      => inputs[1];
        public bool CartAlignSensorLeftForward      => inputs[2];
        public bool CartAlignSensorLeftBackward     => inputs[3];
        public bool CartAlignSensorRightForward     => inputs[4];
        public bool CartAlignSensorRightBackward    => inputs[5];
        public bool ReturnStageReelPresentSensor7   => inputs[6];
        public bool ReturnStageReelPresentSensor13  => inputs[7];
        public bool BufferZoneCartPresentSensor1    => inputs[8];
        public bool BufferZoneCartPresentSensor2    => inputs[9];
        public bool CartAlignSensorFrontXForward    => inputs[10];
        public bool CartAlignSensorFrontXBackward   => inputs[11];
        public bool CartAlignSensorFrontYForward    => inputs[12];
        public bool CartAlignSensorFrontYBackward   => inputs[13];
        public bool MainAirSupply                   => inputs[14];
        public bool OutputStage1Exist               => inputs[16];         // New model (Version 1.0.1.0)
        public bool OutputStage2Exist               => inputs[17];         // New model (Version 1.0.1.0)
        public bool OutputStage3Exist               => inputs[18];         // New model (Version 1.0.1.0)
        public bool OutputStage4Exist               => inputs[19];         // New model (Version 1.0.1.0)
        public bool OutputStage5Exist               => inputs[20];         // New model (Version 1.0.1.0)
        public bool OutputStage6Exist               => inputs[21];         // New model (Version 1.0.1.0)
        public bool RejectStageFull                 => inputs[22];         // New model (Version 1.0.1.0)
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
        public bool CartAlignCylinderLeft
        {
            get => outputs[4];
            set
            {
                SetOutput(4, value ? 1 : 0);
            }
        }
        public bool CartAlignCylinderRight
        {
            get => outputs[5];
            set
            {
                SetOutput(5, value ? 1 : 0);
            }
        }
        public bool CartAlignCylinderFrontX
        {
            get => outputs[6];
            set
            {
                SetOutput(6, value ? 1 : 0);
            }
        }
        public bool CartAlignCylinderFrontY
        {
            get => outputs[7];
            set
            {
                SetOutput(7, value ? 1 : 0);
            }
        }
        public bool IsCartInWorkZone    => WorkZoneCartPresentSensor1 && WorkZoneCartPresentSensor2;
        public bool IsCartInBufferZone  => BufferZoneCartPresentSensor1 && BufferZoneCartPresentSensor2;
        public bool IsCartClamped       => (CartAlignSensorLeftForward && CartAlignSensorRightForward) & (CartAlignSensorFrontXBackward && CartAlignSensorFrontYForward && MainAirSupply);
        public bool IsCartClamping      => (CartAlignSensorLeftForward || CartAlignSensorRightForward) & (CartAlignSensorFrontXBackward && CartAlignSensorFrontYForward && MainAirSupply);
        public bool IsCartReleased      => (CartAlignSensorLeftBackward && CartAlignSensorRightBackward) & (CartAlignSensorFrontXForward && CartAlignSensorFrontYBackward && MainAirSupply);
        public bool IsCartReleasing     => (CartAlignSensorLeftBackward || CartAlignSensorRightBackward) & (CartAlignSensorFrontXForward && CartAlignSensorFrontYBackward && MainAirSupply);
        public bool IsReturnReelExist   => ReturnStageReelPresentSensor7 || ReturnStageReelPresentSensor13;
        public bool SignalTowerBlue     { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool SignalTowerWhite    { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsSimulation        => simulation;

        public bool IsCartHooked        => !CartAlignSensorFrontYForward && !CartAlignSensorFrontYBackward && !CartAlignSensorFrontXForward && CartAlignSensorFrontXBackward && CartAlignCylinderFrontY && !CartAlignCylinderFrontX;
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
                    if (!Simulation && Initialized)
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
                Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
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
                    Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
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
            int modules = inputs.Length / 16;

            if (modules * 16 <= nChannel)
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
                    Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
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
            int modules = inputs.Length / 16;

            if (modules * 16 <= nChannel)
                return false;

            if (simulation)
            {
                result = outputs[nChannel];
            }
            else
            {
                try
                {
                    int nState = (int)Defines._TCmBool.cmFALSE;

                    if (CMDLL.cmmDoGetOne(nChannel, ref nState) == Defines.cmERR_NONE)
                        result = true;

                    if (nState == (int)Defines._TCmBool.cmFALSE)
                        result = false;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
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
            int modules = inputs.Length / 16;

            if (modules * 16 <= nChannel)
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
            int modules = outputs.Length / 16;

            if (modules * 16 <= nChannel)
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
                    Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
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
                int modules = inputs.Length / 16;

                for (int i_ = 0; i_ < modules; i_++)
                {
                    int ch = i_ * 16;
                    CMDLL.cmmDiGetMulti(ch, ch + 16, ref nState);

                    for (int j_ = 0; j_ < 16; j_++)
                    {
                        bool state = (((nState >> j_) & 1) == 1) ? true : false;
                        
                        if (inputs[ch + j_] != state)
                        {
                            inputs[ch + j_] = state;
                            InputSignalChanged?.Invoke(this, ch + j_);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public void UpdateOutputs()
        {
            try
            {
                int nState = 0;
                int modules = outputs.Length / 16;

                for (int i_ = 0; i_ < modules; i_++)
                {
                    int ch = i_ * 16;
                    CMDLL.cmmDoGetMulti(ch, ch + 16, ref nState);

                    for (int j_ = 0; j_ < 16; j_++)
                    {
                        bool state = (((nState >> j_) & 1) == 1) ? true : false;

                        if (outputs[ch + j_] != state)
                        {
                            outputs[ch + j_] = state;
                            OutputSignalChanged?.Invoke(this, ch + j_);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
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
                        CartAlignCylinderLeft = true;
                        CartAlignCylinderRight = true;
                        CartAlignCylinderFrontX = true;
                        CartAlignCylinderFrontY = true;
                    }
                    break;
                case CartClamps.Side:
                    {
                        CartAlignCylinderLeft = true;
                        CartAlignCylinderRight = true;
                    }
                    break;
                case CartClamps.Hook:
                    {
                        CartAlignCylinderFrontX = false;
                    }
                    break;
                case CartClamps.Pull:
                    {
                        CartAlignCylinderFrontY = true;
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
                        CartAlignCylinderFrontY = false;
                        CartAlignCylinderLeft = false;
                        CartAlignCylinderRight = false;
                        CartAlignCylinderFrontX = false;
                    }
                    break;
                case CartClamps.Side:
                    {
                        CartAlignCylinderLeft = false;
                        CartAlignCylinderRight = false;
                    }
                    break;
                case CartClamps.Hook:
                    {
                        CartAlignCylinderFrontX = true;
                    }
                    break;
                case CartClamps.Pull:
                    {
                        CartAlignCylinderFrontX = false;
                    }
                    break;
            }
        }

        public bool PushOutputMaterial(int stageindex = 0)
        {   // UPDATED: 20200424 (Marcus)
            // Not use pusher and stopper.
            bool result_ = false;

            // switch (stageindex)
            // {
            //     case 0:
            //         {
            //             OutputStage1Pusher = !OutputStage1Exist;
            //             OutputStage2Pusher = !OutputStage2Exist;
            //             OutputStage3Pusher = !OutputStage3Exist;
            //         }
            //         break;
            //     case 1:
            //     case 4:
            //         {
            //             OutputStage1Pusher = !OutputStage1Exist;
            //         }
            //         break;
            //     case 2:
            //     case 5:
            //         {
            //             OutputStage2Pusher = !OutputStage2Exist;
            //         }
            //         break;
            //     case 3:
            //     case 6:
            //         {
            //             OutputStage3Pusher = !OutputStage3Exist;
            //         }
            //         break;
            // }

            return result_;
        }

        public bool PullOutputMaterial(int stageindex = 0)
        {   // UPDATED: 20200424 (Marcus)
            // Not use pusher and stopper.
            bool result_ = false;

            // switch (stageindex)
            // {
            //     case 0:
            //         {
            //             OutputStage1Pusher = false;
            //             OutputStage2Pusher = false;
            //             OutputStage3Pusher = false;
            //         }
            //         break;
            //     case 1:
            //         {
            //             OutputStage1Pusher = false;
            //         }
            //         break;
            //     case 2:
            //         {
            //             OutputStage2Pusher = false;
            //         }
            //         break;
            //     case 3:
            //         {
            //             OutputStage3Pusher = false;
            //         }
            //         break;
            // }

            return result_;
        }

        public void DockCart()
        {
            new TaskFactory().StartNew(new Action<object>((x_) =>
            {
                try
                {
                    bool stop_ = false;
                    int to_ = 5000;
                    DateTime sdt_ = DateTime.Now;

                    while (!stop_ && (DateTime.Now - sdt_).TotalMilliseconds <= to_)
                    {
                        if (!CartAlignSensorLeftForward && CartAlignSensorLeftBackward)
                            CartAlignCylinderLeft = true;

                        if (!CartAlignSensorRightForward && CartAlignSensorRightBackward)
                            CartAlignCylinderRight = true;

                        if (CartAlignSensorLeftForward && CartAlignSensorRightForward)
                        {
                            CartAlignCylinderFrontX = false;
                            stop_ = true;
                        }
                        else
                            Thread.Sleep(1);
                    }

                    stop_ = false;
                    sdt_ = DateTime.Now;

                    if (x_ != null)
                        to_ = Convert.ToInt32(x_);

                    while (!stop_ && (DateTime.Now - sdt_).TotalMilliseconds <= to_)
                    {
                        if (CartAlignSensorLeftForward && CartAlignSensorRightForward && CartAlignSensorFrontXBackward)
                        {
                            CartAlignCylinderFrontY = true;
                            stop_ = true;
                        }
                        else
                            Thread.Sleep(1);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
            }), 5000);
        }

        public void UndockCart()
        {
            new TaskFactory().StartNew(new Action<object>((x_) =>
            {
                try
                {
                    bool stop_ = false;
                    int to_ = 5000;
                    DateTime sdt_ = DateTime.Now;

                    while (!stop_ && (DateTime.Now - sdt_).TotalMilliseconds <= to_)
                    {
                        if (CartAlignSensorFrontYForward || !CartAlignSensorFrontYBackward)
                            CartAlignCylinderFrontY = false;

                        if (!CartAlignSensorFrontYForward && CartAlignSensorFrontYBackward)
                        {
                            CartAlignCylinderFrontX = true;
                            stop_ = true;
                        }
                        else
                            Thread.Sleep(1);
                    }

                    if (CartAlignSensorLeftForward && !CartAlignSensorLeftBackward)
                        CartAlignCylinderLeft = false;

                    if (CartAlignSensorRightForward && !CartAlignSensorRightBackward)
                        CartAlignCylinderRight = false;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
            }), 5000);
        }
        #endregion
    }
}
#endregion