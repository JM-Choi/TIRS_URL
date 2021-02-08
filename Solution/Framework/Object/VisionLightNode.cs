#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechFloor.Device;
using TechFloor.Device.CommunicationIo;
#endregion

#region Program
namespace TechFloor.Object
{
    public class VisionLightNode
    {
        public SerialPortSettings Setting;

        public DeviceManufacturer Maker;

        public int Channel;

        public bool DtrEnable = true;

        public bool RtsEnable = true;

        public bool PeriodicTask = false;

        public int Port => string.IsNullOrEmpty(Setting.PortName) ? 0 : Convert.ToInt32(Setting.PortName.Remove(0, 3));
    }
}
#endregion