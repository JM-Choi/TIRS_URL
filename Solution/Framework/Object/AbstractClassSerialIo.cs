#region Imports
using System;
using System.Xml;
using System.Net;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security;
#endregion

#region Program
namespace TechFloor.Device.CommunicationIo.SerialIo
{
    public abstract class AbstractClassSerialIo : AbstractClassCommunicationIo, ISerialIo, IGenericCommunicationIoEventHandler
    {
        #region Fields
        protected bool connected = false;
        protected SerialPort serialPort = null;
        protected SerialPortSettings serialPortSettingParameters;
        #endregion

        #region Properties
        public virtual SerialPort Comm
        {
            get => serialPort;
            protected set
            {
                try
                {
                    if (serialPort == null)
                        serialPort = value;
                    else
                    {
                        Disconnect();
                        serialPort = value;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
            }
        }

        public override DeviceStates DeviceState => deviceState;

        public override bool IsOpen => (serialPort == null ? false : serialPort.IsOpen);

        public virtual bool DtrEnable
        {
            get => dtrEnable;
            set => dtrEnable = value;
        }

        public virtual bool RtsEnable
        {
            get => rtsEnable;
            set => rtsEnable = value;
        }

        public virtual Handshake Handshake
        {
            get => handShake;
            set => handShake = value;
        }

        public SerialPortSettings SerialPortSettingParameters
        {
            get => serialPortSettingParameters;
            set
            {
                if (!IsOpen)
                    serialPortSettingParameters = value;
            }
        }
        #endregion

        #region Events
        public virtual event EventHandler Connected;
        public virtual event EventHandler Disconnected;
        public virtual event EventHandler Sent;
        public virtual event EventHandler Received;
        public virtual event EventHandler FailedToConnect;
        public virtual event EventHandler FailedToDecode;
        public virtual event EventHandler FailedToSend;
        public virtual event EventHandler ResponseTimedout;
        #endregion

        #region Constructors
        public AbstractClassSerialIo()
        {

        }

        public AbstractClassSerialIo(int port, int baudRate, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, bool periodicTask = false)
        {
            Create(port, baudRate, parity, dataBits, stopBits, Handshake.None, false, false, periodicTask);
        }

        public AbstractClassSerialIo(int port, int baudRate, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, Handshake handshake = Handshake.None, bool dtrenable = false, bool rtsenable = false, bool periodicTask = false)
        {
            Create(port, baudRate, parity, dataBits, stopBits, handshake, dtrenable, rtsenable, periodicTask);
        }
        #endregion

        #region Destructor
        ~AbstractClassSerialIo()
        {
            queryTimer.Stop();
            pollingTimer.Stop();
            Disconnect();
            DestroyPeriodicTask();
            Dispose(false);
        }
        #endregion

        #region Protected methods
        protected override void DisposeManagedObjects()
        {
            base.DisposeManagedObjects();

            if (serialPort != null)
            {
                serialPort.Dispose();
                serialPort = null;
            }
        }

        protected override void OnReceived(string data)
        {
            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    ResetQueryTimer();
                    ResetResponseCheckTimer();
                    receivedData = data;
                    Decoded = true;
                    FireReceived();
                }
            }
            catch (Exception ex)
            {
                Decoded = false;
                receivedData = string.Empty;
                Debug.WriteLine(string.Format($"{GetType()}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}"));
                FireFailedToDecode();
            }
        }

        protected virtual void OnReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (connected)
                    OnReceived(serialPort.ReadLine());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected override void DoCheckResponseTimeout()
        {
            if (responseCheckTimer.IsRunning)
            {
                responseCheckTimer.Stop();

                if (responseCheckTimer.ElapsedMilliseconds > responseTimeout)
                {
                    blockDelayedResponse = true;
                    FireResponseTimeout();
                }
                else
                    responseCheckTimer.Start();
            }
        }

        protected override bool Send(string str, bool attachCr = true, int timeout = 0)
        {
            try
            {
                Decoded = false;

                if (IsOpen)
                {
                    serialPort.DiscardOutBuffer();
                    serialPort.WriteLine(attachCr ? string.Concat(str, (char)AsciiControlCharacters.Cr) : str);
                    queryTimer.Restart();
                    SetResponseCheckTimer(timeout);
                    FireSent();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format($"{GetType()}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}"));
                FireFailedToSend();
            }

            return false;
        }

        protected override bool Send(byte[] data, int timeout = 1000)
        {
            try
            {
                Decoded = false;

                if (IsOpen && data != null)
                {
                    serialPort.DiscardOutBuffer();
                    serialPort.Write(data, 0, data.Length);
                    SetResponseCheckTimer(timeout);
                    queryTimer.Restart();
                    FireSent();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format($"{GetType()}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}"));
                FireFailedToSend();
            }
            return false;
        }
        #endregion

        #region Public methods
        public virtual void Create(int port, int baudRate, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, Handshake handshake = Handshake.None, bool dtrenable = false, bool rtsenable = false, bool periodicTask = false)
        {
            try
            {
                handShake = handshake;
                dtrEnable = dtrenable;
                rtsEnable = rtsenable;

                if ((Comm = new SerialPort(string.Format("COM{0}", port), baudRate, parity, dataBits, stopBits)) != null)
                {
                    if (periodicTask)
                        CreatePeriodicTask();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format($"{GetType()}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}"));
            }
        }

        public override void Connect()
        {
            Disconnect();

            try
            {
                if (serialPort != null && !serialPort.IsOpen)
                {
                    serialPort.DataReceived += OnReceived;
                    serialPort.Open();

                    if (connected = serialPort.IsOpen)
                    {

                        serialPort.DtrEnable = dtrEnable;
                        serialPort.RtsEnable = rtsEnable;
                        serialPort.Handshake = Handshake;
                        serialPort.DiscardInBuffer();
                        serialPort.DiscardOutBuffer();
                        FireConnected();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                FireFailedToConnect();
            }
        }

        public override void Disconnect()
        {
            receivedData = string.Empty;
            connected = false;
            Decoded = false;
            queryTimer.Stop();
            queryTimer.Reset();

            if (IsOpen)
            {
                if (serialPort != null)
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    serialPort.DataReceived -= OnReceived;
                    serialPort.Close();
                }

                FireDisonnected();
            }
        }

        public virtual void FireConnected()
        {
            deviceState = DeviceStates.Idle;

            if (Connected != null)
                Connected.Invoke(this, EventArgs.Empty);
        }

        public virtual void FireDisonnected()
        {
            deviceState = DeviceStates.ConnectionFailure;

            if (Disconnected != null)
                Disconnected.Invoke(this, EventArgs.Empty);
        }

        public virtual void FireReceived()
        {
            deviceState = DeviceStates.ReceivedData;
            ResetResponseCheckTimer();

            if (Received != null)
                Received.Invoke(this, EventArgs.Empty);
        }

        public virtual void FireSent()
        {
            deviceState = DeviceStates.SentCommand;

            if (Sent != null)
                Sent.Invoke(this, EventArgs.Empty);
        }

        public virtual void FireFailedToConnect()
        {
            deviceState = DeviceStates.ConnectionFailure;

            if (FailedToConnect != null)
                FailedToConnect.Invoke(this, EventArgs.Empty);
        }

        public virtual void FireFailedToDecode()
        {
            deviceState = DeviceStates.ReceivedWrongData;

            if (FailedToDecode != null)
                FailedToDecode.Invoke(this, EventArgs.Empty);
        }

        public virtual void FireFailedToSend()
        {
            deviceState = DeviceStates.SentFailure;

            if (FailedToSend != null)
                FailedToSend.Invoke(this, EventArgs.Empty);
        }

        public virtual void FireResponseTimeout()
        {
            deviceState = DeviceStates.ResponseTimeout;

            ResetResponseCheckTimer();

            if (ResponseTimedout != null)
                ResponseTimedout.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
#endregion