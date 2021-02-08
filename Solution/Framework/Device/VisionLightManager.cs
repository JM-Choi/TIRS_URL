#region Imports
using TechFloor.Device.CommunicationIo.SerialIo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TechFloor.Object;
using TechFloor.Shared;
#endregion

#region Program
namespace TechFloor.Device
{
    public class VisionLightManager : AbstractClassSerialIo
    {
        #region Enumerations
        public enum ControllerCommands
        {
            On,
            Off,
            Set,
            SetAll,
            Get,
            GetAll,
            Version,
            Save,
        }
        #endregion

        #region Fields
        protected DeviceManufacturer maker;

        protected List<int> valuesOfChannel1 = new List<int>();
        #endregion

        #region Properties
        public DeviceManufacturer Maker => maker;

        public List<int> ValuesOfChannel1 => valuesOfChannel1;
        #endregion

        #region Constructors
        public VisionLightManager(VisionLightNode info)
            : base(info.Port, info.Setting.BaudRate, info.Setting.Parity, info.Setting.DataBits, info.Setting.StopBits, info.Setting.Handshake, info.DtrEnable, info.RtsEnable, info.PeriodicTask)
        {
            this.maker = info.Maker;
        }

        public VisionLightManager(int port = 3, int baudRate = 57600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, Handshake handshake = Handshake.None, bool dtrenable = true, bool rtsenable = true, bool periodicTask = false)
            : base(port, baudRate, parity, dataBits, stopBits, handshake, dtrenable, rtsenable, periodicTask)
        {
        }

        public VisionLightManager(int port, int baudRate, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, bool periodicTask = false)
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
        public int SendCommand(ControllerCommands cmd, int channel, int value)
        {
            switch (this.Maker)
            {
                case DeviceManufacturer.KpVision:
                    {
                        switch (cmd)
                        {
                            case ControllerCommands.On:
                                return Send($"#A{channel}001&");
                            case ControllerCommands.Off:
                                return Send($"#A{channel}{value:000}&");
                            case ControllerCommands.Set:
                                return Send($"#A{channel}{value:000}&");
                            case ControllerCommands.SetAll:
                                return Send($"#Aa{value:000}&");
                            case ControllerCommands.Get:
                                return Send($"#?{channel}&");
                            case ControllerCommands.Save:
                                return Send($"#S&");
                            case ControllerCommands.Version:
                                return Send($"#V&");
                            default:
                                return 0;
                        }
                    }
                default:
                    {
                        switch (cmd)
                        {
                            case ControllerCommands.On:
                                return Send($"N{channel}{value:000}#");
                            case ControllerCommands.Off:
                                return Send($"F{channel}{value:000}#");
                            case ControllerCommands.Set:
                                return Send($"B{channel}{value:000}#");
                            case ControllerCommands.Get:
                            default:
                                return 0;
                        }
                    }
            }
        }

        public override int Send(string data)
        {
            return Send(Encoding.ASCII.GetBytes(data), 0) ? 0 : -1;
        }
        #endregion
    }

    public class VisionProcessEventArgs : EventArgs
    {
        public int ProcessType = 0;
        public int OptionMode = 0;
        public int TriggerDelay = 1000;
        public int LightCategory = 0;
        public double CenterXOffsetLimit = 10;
        public double CenterYOffsetLimit = 10;
        public double OptionCoordX = 0;
        public double OptionCoordY = 0;
        public double OptionCoordZ = 0;
        public double OptionAngleRX = 0;
        public double OptionAngleRY = 0;
        public double OptionAngleRZ = 0;
        public ReelDiameters ReelType = ReelDiameters.Unknown;
        public readonly VisionProcessDataObjectTypes ProcessDataType = VisionProcessDataObjectTypes.ProcessArgument;

        public VisionProcessEventArgs(int model, int delay, int ptype, ReelDiameters rtype, VisionProcessDataObjectTypes dtype = VisionProcessDataObjectTypes.ProcessArgument)
        {
            ProcessType = ptype;
            TriggerDelay = delay;
            LightCategory = model;
            ReelType = rtype;
            ProcessDataType = dtype;
        }

        public VisionProcessEventArgs(int model, int delay, int ptype, ReelDiameters rtype, double xlimit, double ylimit, VisionProcessDataObjectTypes dtype = VisionProcessDataObjectTypes.ProcessArgument)
        {
            ProcessType = ptype;
            TriggerDelay = delay;
            LightCategory = model;
            ReelType = rtype;
            ProcessDataType = dtype;
            CenterXOffsetLimit = xlimit;
            CenterYOffsetLimit = ylimit;
        }
    }
}
#endregion