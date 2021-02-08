#region Imports
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TechFloor.Device.CommunicationIo;
using TechFloor.Object;
#endregion

#region Program
namespace TechFloor.Device.CommunicationIo
{
    public class ModbusMasterController : AbstractClassDisposable
    {
        #region Enumerations
        public enum Protocols
        {
            Rtu,
            Ascii,
        }

        public enum InternetProtocols
        {
            Tcp,
            Udp
        }
        #endregion

        #region Fields
        protected string name = string.Empty;
        protected string model = string.Empty;
        protected SerialPort serialPort = null;
        protected ModbusSerialMaster master = null;
        protected SerialPortSettings serialPortSetting;
        protected EthernetPortSettings ethernetSetting;
        protected IAsyncResult result = null;
        #endregion

        #region Properties
        public string Name => name;
        public string Model => model;
        public Protocols Protocol = Protocols.Rtu;
        public InterfaceTypes Interface = InterfaceTypes.Serial;
        public System.Net.Sockets.ProtocolType ProtocolType = System.Net.Sockets.ProtocolType.Tcp;
        #endregion

        #region Constuctors
        public ModbusMasterController(string name, string model, Protocols protocol, SerialPortSettings setting)
        {
            Create(name, model, protocol, setting);
        }

        public ModbusMasterController(string name, string model, Protocols protocol, System.Net.Sockets.ProtocolType protocoltype, EthernetPortSettings setting)
        {
            Create(name, model, protocol, protocoltype, setting);
        }
        #endregion

        #region Public methods
        public void Create(string name, string model, Protocols protocol, SerialPortSettings setting)
        {
            this.name = name;
            this.model = model;
            this.Interface = InterfaceTypes.Serial;
            this.Protocol = protocol;
            this.ProtocolType = System.Net.Sockets.ProtocolType.Unspecified;
            this.serialPortSetting = setting;
        }

        public void Create(string name, string model, Protocols protocol, System.Net.Sockets.ProtocolType protocoltype, EthernetPortSettings setting)
        {
            this.name = name;
            this.model = model;
            this.Interface = InterfaceTypes.Ethernet;
            this.Protocol = protocol;
            this.ProtocolType = protocoltype;
            this.ethernetSetting = setting;
        }

        public bool Initialize()
        {
            try
            {
                if (serialPort != null)
                    serialPort.Close();

                if (master != null)
                {
                    master.Dispose();
                    master = null;
                }

                switch (Interface)
                {
                    case InterfaceTypes.Serial:
                        {
                            serialPort = new SerialPort(serialPortSetting.PortName,
                                serialPortSetting.BaudRate,
                                serialPortSetting.Parity,
                                serialPortSetting.DataBits,
                                serialPortSetting.StopBits);
                            serialPort.Handshake = serialPortSetting.Handshake;
                            serialPort.ReadTimeout = serialPortSetting.ReadTimeout;
                            serialPort.WriteTimeout = serialPortSetting.WriteTimeout;
                            serialPort.Open();

                            switch (Protocol)
                            {
                                case Protocols.Rtu:
                                    master = ModbusSerialMaster.CreateRtu(serialPort);
                                    break;
                                case Protocols.Ascii:
                                    master = ModbusSerialMaster.CreateAscii(serialPort);
                                    break;
                            }
                        }
                        break;
                    case InterfaceTypes.Ethernet:
                        {
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return (master != null);
        }

        public bool WriteSingleCoil(byte slaveaddr, ushort startaddr, bool output)
        {
            bool result = false;

            try
            {
                if (master != null)
                {
                    master.WriteSingleCoil(slaveaddr, startaddr, output);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return result;
        }

        public Task WriteSingleCoilAsync(byte slaveaddr, ushort startaddr, bool output)
        {
            try
            {
                if (master != null)
                    return master.WriteSingleCoilAsync(slaveaddr, startaddr, output);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return null;
        }

        public bool WriteSingleRegister(byte slaveaddr, ushort startaddr, ushort output)
        {
            bool result = false;

            try
            {
                if (master != null)
                {
                    master.WriteSingleRegister(slaveaddr, startaddr, output);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return result;
        }

        public Task WriteSingleRegisterAsync(byte slaveaddr, ushort startaddr, ushort output)
        {
            try
            {
                if (master != null)
                    return master.WriteSingleRegisterAsync(slaveaddr, startaddr, output);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return null;
        }

        public bool WriteMultipleCoils(byte slaveaddr, ushort startaddr, bool[] outputs)
        {
            bool result = false;

            try
            {
                if (master != null)
                {
                    master.WriteMultipleCoils(slaveaddr, startaddr, outputs);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return result;
        }

        public Task WriteMultipleCoilsAsync(byte slaveaddr, ushort startaddr, bool[] outputs)
        {
            try
            {
                if (master != null)
                    return master.WriteMultipleCoilsAsync(slaveaddr, startaddr, outputs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return null;
        }

        public bool WriteMultipleRegisters(byte slaveaddr, ushort startaddr, ushort[] outputs)
        {
            bool result = false;

            try
            {
                if (master != null)
                {
                    master.WriteMultipleRegisters(slaveaddr, startaddr, outputs);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return result;
        }

        public Task WriteMultipleRegistersAsync(byte slaveaddr, ushort startaddr, ushort[] outputs)
        {
            try
            {
                if (master != null)
                    return master.WriteMultipleRegistersAsync(slaveaddr, startaddr, outputs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return null;
        }

        public bool[] ReadCoils(byte slaveaddr, ushort startaddr, ushort outputs)
        {
            try
            {
                if (master != null)
                    return master.ReadCoils(slaveaddr, startaddr, outputs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return null;
        }

        public Task<bool[]> ReadCoilsAsync(byte slaveaddr, ushort startaddr, ushort outputs)
        {
            try
            {
                if (master != null)
                    return master.ReadCoilsAsync(slaveaddr, startaddr, outputs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return null;
        }

        public ushort[] ReadHoldingRegisters(byte slaveaddr, ushort startaddr, ushort outputs)
        {
            try
            {
                if (master != null)
                    return master.ReadHoldingRegisters(slaveaddr, startaddr, outputs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return null;
        }

        public Task<ushort[]> ReadHoldingRegistersAsync(byte slaveaddr, ushort startaddr, ushort outputs)
        {
            try
            {
                if (master != null)
                    return master.ReadHoldingRegistersAsync(slaveaddr, startaddr, outputs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return null;
        }

        public ushort[] ReadInputRegisters(byte slaveaddr, ushort startaddr, ushort outputs)
        {
            try
            {
                if (master != null)
                    return master.ReadInputRegisters(slaveaddr, startaddr, outputs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return null;
        }

        public Task<ushort[]> ReadInputRegistersAsync(byte slaveaddr, ushort startaddr, ushort outputs)
        {
            try
            {
                if (master != null)
                    return master.ReadInputRegistersAsync(slaveaddr, startaddr, outputs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return null;
        }

        public bool[] ReadInputs(byte slaveaddr, ushort startaddr, ushort outputs)
        {
            try
            {
                if (master != null)
                    return master.ReadInputs(slaveaddr, startaddr, outputs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return null;
        }

        public Task<bool[]> ReadInputsAsync(byte slaveaddr, ushort startaddr, ushort outputs)
        {
            try
            {
                if (master != null)
                    return master.ReadInputsAsync(slaveaddr, startaddr, outputs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return null;
        }

        public ushort[] ReadWriteMultipleRegisters(byte slaveaddr, ushort startaddr, ushort outputs, ushort startwriteaddr, ushort[] data)
        {
            try
            {
                if (master != null)
                    return master.ReadWriteMultipleRegisters(slaveaddr, startaddr, outputs, startwriteaddr, data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return null;
        }

        public Task<ushort[]> ReadWriteMultipleRegistersAsync(byte slaveaddr, ushort startaddr, ushort outputs, ushort startwriteaddr, ushort[] data)
        {
            try
            {
                if (master != null)
                    return master.ReadWriteMultipleRegistersAsync(slaveaddr, startaddr, outputs, startwriteaddr, data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}");
            }

            return null;
        }
        #endregion

        #region Protected methods
        protected void Disconnect()
        {
            if (serialPort != null)
            {
                serialPort.Close();
                serialPort.Dispose();
                serialPort = null;
            }
        }

        protected override void DisposeManagedObjects()
        {
            base.DisposeManagedObjects();
            Disconnect();

            if (master != null)
            {
                master.Dispose();
                master = null;
            }
        }
        #endregion
    }
}
#endregion