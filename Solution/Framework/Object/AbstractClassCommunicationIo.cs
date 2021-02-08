#region Imports
using System;
using System.Xml;
using System.Net;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections.Generic;
using TechFloor.Object;
using System.IO.Ports;
using System.ComponentModel;
#endregion

#region Program
namespace TechFloor.Device.CommunicationIo
{
    #region Enumerations
    public enum AsciiControlCharacters
    {
        Esc = '\x001b',
        Eot = '\x0004',
        Stx = '\x0002',
        Etx = '\x0003',
        Enq = '\x0005',
        Ack = '\x0006',
        Nak = '\x0015',
        Lf = '\x000a',
        Cr = '\x000d',
        Sp = '\x0020'
    }

    public enum CommunicationIoInterface
    {
        Serial,
        USB,
        Ethernet,
        Parallel,
        Bluetooth
    }

    public enum SocketModels
    {
        TcpClient,
        UdpClient,
        TcpServer,
        UdpServer,
    }

    public enum NodeTypes
    {
        Mater,
        Slave
    }

    public enum EncodingTypes
    {
        UTF7,
        BigEndianUnicode,
        Unicode,
        Default,
        ASCII,
        UTF8,
        UTF32
    }
    #endregion

    public class CommunicationIoEventArgs : EventArgsExt
    {
        #region Fields
        public readonly int Id;
        public int Size = 0;
        public object Code;
        public string Text;
        public byte[] Data;
        #endregion

        #region Constructors
        public CommunicationIoEventArgs(int id = 0, string text = null)
        {
            Id = id;
            Text = text;
            Size = text.Length;
        }

        public CommunicationIoEventArgs(int id, string text, object code)
        {
            Id = id;
            Text = text;
            Code = code;
            Size = text.Length;
        }

        public CommunicationIoEventArgs(int id, byte[] data, int size)
        {
            Id = id;
            Text = string.Empty;
            Data = new byte[Size = size];
            Buffer.BlockCopy(data, 0, Data, 0, size);
        }

        public CommunicationIoEventArgs(int id, string text, byte[] data, int size)
        {
            Id = id;
            Text = text;
            Data = new byte[Size = size];
            Buffer.BlockCopy(data, 0, Data, 0, size);
        }
        #endregion
    }

    public class AsyncSocketEventArgs : EventArgs
    {
        #region Fields
        protected readonly Socket sock;
        protected readonly Exception exception;
        protected readonly int id = 0;
        protected readonly int transmitDataSize;
        protected readonly byte[] transmitData;
        #endregion

        #region Constructors
        public AsyncSocketEventArgs(int id, Socket connection)
        {
            this.id = id;
            this.sock = connection;
        }

        public AsyncSocketEventArgs(int id)
        {
            this.id = id;
        }

        public AsyncSocketEventArgs(int id, int sendBytes)
        {
            this.id = id;
            this.transmitDataSize = sendBytes;
        }

        public AsyncSocketEventArgs(int id, int receiveBytes, byte[] receiveData)
        {
            this.id = id;
            this.transmitDataSize = receiveBytes;
            this.transmitData = receiveData;
        }

        public AsyncSocketEventArgs(int id, Exception exception)
        {
            this.id = id;
            this.exception = exception;
        }
        #endregion

        #region Properties
        public Socket Connection => sock;
        public Exception Exception => exception;
        public int Id => id;
        public int Size => transmitDataSize;
        public byte[] Data => transmitData;
        #endregion
    }

    public struct SerialPortSettings
    {
        #region Properties
        [Browsable(true)]
        [DefaultValue("COM1")]
        [MonitoringDescription("ComPort")]
        public string PortName { get; set; }

        [Browsable(true)]
        [DefaultValue(9600)]
        [MonitoringDescription("BaudRate")]
        public int BaudRate { get; set; }

        [Browsable(true)]
        [DefaultValue(8)]
        [MonitoringDescription("DataBits")]
        public int DataBits { get; set; }

        [Browsable(true)]
        [DefaultValue(StopBits.One)]
        [MonitoringDescription("StopBits")]
        public StopBits StopBits { get; set; }

        [Browsable(true)]
        [DefaultValue(Parity.None)]
        [MonitoringDescription("Parity")]
        public Parity Parity { get; set; }

        [Browsable(true)]
        [DefaultValue(Handshake.None)]
        [MonitoringDescription("Handshake")]
        public Handshake Handshake { get; set; }

        [Browsable(true)]
        [DefaultValue(100)]
        [MonitoringDescription("Readtimeout")]
        public int ReadTimeout { get; set; }

        [Browsable(true)]
        [DefaultValue(100)]
        [MonitoringDescription("Writetimeout")]
        public int WriteTimeout { get; set; }
        #endregion

        #region Public methods
        public string PrintOut()
        {
            return $"{PortName};{BaudRate};{DataBits};{StopBits};{Parity};{Handshake};";
        }
        #endregion
    }

    public struct EthernetPortSettings
    {
        #region Properties
        [Browsable(true)]
        [DefaultValue("127.0.0.1")]
        [MonitoringDescription("IpAddress")]
        public string IpAddress { get; set; }

        [Browsable(true)]
        [DefaultValue(50000)]
        [MonitoringDescription("Port")]
        public int Port { get; set; }

        [Browsable(true)]
        [DefaultValue(ProtocolFamily.InterNetwork)]
        [MonitoringDescription("ProtocolFamily")]
        public ProtocolFamily ProtocolFamily { get; set; }

        [Browsable(true)]
        [DefaultValue(ProtocolFamily.InterNetwork)]
        [MonitoringDescription("ProtocolType")]
        public ProtocolType ProtocolType { get; set; }

        [Browsable(true)]
        [DefaultValue(1000)]
        [MonitoringDescription("ReadTimeout")]
        public int ReadTimeout { get; set; }

        [Browsable(true)]
        [DefaultValue(1000)]
        [MonitoringDescription("WriteTimeout")]
        public int WriteTimeout { get; set; }
        #endregion

        #region Public methods
        public string PrintOut()
        {
            return $"{IpAddress};{Port};";
        }
        #endregion
    }

    public interface IGenericCommunicationIoEventHandler
    {
        #region Events
        event EventHandler Connected;
        event EventHandler Disconnected;
        event EventHandler Sent;
        event EventHandler Received;
        event EventHandler FailedToConnect;
        event EventHandler FailedToDecode;
        event EventHandler FailedToSend;
        event EventHandler ResponseTimedout;
        #endregion

        #region Event handlers
        void FireConnected();
        void FireDisonnected();
        void FireReceived();
        void FireSent();
        void FireFailedToConnect();
        void FireFailedToDecode();
        void FireFailedToSend();
        void FireResponseTimeout();
        #endregion
    }

    public interface IAsynchronousCommunicationEventHandler
    {
        #region Events
        event EventHandler<CommunicationIoEventArgs> Connected;
        event EventHandler<CommunicationIoEventArgs> Disconnected;
        event EventHandler<CommunicationIoEventArgs> Sent;
        event EventHandler<CommunicationIoEventArgs> Received;
        event EventHandler<CommunicationIoEventArgs> FailedToConnect;
        event EventHandler<CommunicationIoEventArgs> FailedToDecode;
        event EventHandler<CommunicationIoEventArgs> FailedToSend;
        event EventHandler<CommunicationIoEventArgs> ResponseTimedout;
        event EventHandler<CommunicationIoEventArgs> RemoteProcessData;
        #endregion

        #region Event handlers
        void FireConnected(CommunicationIoEventArgs args);
        void FireDisconnected(CommunicationIoEventArgs args);
        void FireSent(CommunicationIoEventArgs args);
        void FireReceived(CommunicationIoEventArgs args);
        void FireFailedToConnect(CommunicationIoEventArgs args);
        void FireFailedToDecode(CommunicationIoEventArgs args);
        void FireFailedToSend(CommunicationIoEventArgs args);
        void FireResponseTimeout(CommunicationIoEventArgs args);
        #endregion
    }

    public interface ICommunicationIo : IDeviceElement
    {
        #region Properties
        [Browsable(true)]
        [DefaultValue(4096)]
        [MonitoringDescription("ReadBufferSize")]
        int ReadBufferSize { get; set; }

        [Browsable(true)]
        [DefaultValue(4096)]
        [MonitoringDescription("WriteBufferSize")]
        int WriteBufferSize { get; set; }

        [Browsable(true)]
        [DefaultValue(-1)]
        [MonitoringDescription("ReadTimeout")]
        int ReadTimeout { get; set; }

        [Browsable(true)]
        [DefaultValue(-1)]
        [MonitoringDescription("WriteTimeout")]
        int WriteTimeout { get; set; }

        [Browsable(true)]
        [DefaultValue(-1)]
        [MonitoringDescription("ConnectionTimeout")]
        int ConnectionTimeout { get; set; }

        [Browsable(true)]
        [DefaultValue(1000)]
        [MonitoringDescription("ConversationTimeout")]
        int ConversationTimeout { get; set; }

        [Browsable(true)]
        [DefaultValue(1000)]
        [MonitoringDescription("ReconnectionTimeout")]
        int ReconnectionInterval { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("Encoding")]
        Encoding Encoding { get; set; }

        CommunicationIoInterface DeviceInterface { get; }
        IReadOnlyList<string> Commands { get; }
        string LastCommand { get; set; }
        string LastResponse { get; set; }
        bool IsOpen { get; }
        #endregion

        #region Public methods
        bool Create(XmlNode node);
        void Destroy();
        void Connect();
        void Disconnect();
        void Reconnect();
        int Send(string data);
        // void OnConnected(CommunicationIoEventArgs args);
        // void OnDisconnected(CommunicationIoEventArgs args);
        // void OnSent(CommunicationIoEventArgs args);
        // void OnReceived(CommunicationIoEventArgs args);
        // void OnFailedToConnect(CommunicationIoEventArgs args);
        // void OnFailedToDecode(CommunicationIoEventArgs args);
        // void OnFailedToSend(CommunicationIoEventArgs args);
        // void OnResponseTimeout(CommunicationIoEventArgs args);
        #endregion
    }

    public interface ISerialIo : ICommunicationIo
    {
        #region Properties
        SerialPortSettings SerialPortSettingParameters { get; set; }
        #endregion
    }

    public interface IEthernetIo : ICommunicationIo
    {
        #region Properties
        EthernetPortSettings EthernetPortSettingParameters { get; set; }
        #endregion
    }

    public abstract class AbstractClassCommunicationIo : SimulatableDevice
    {
        #region Enumerations
        public enum DeviceStates
        {
            Idle,
            WaitResponse,
            SentFailure,
            SentCommand,
            ReceivedWrongData,
            ReceivedData,
            ResponseTimeout,
            ConnectionFailure,
            Disconnected,
        }
        #endregion

        #region Fields
        protected bool blockDelayedResponse = false;
        protected bool dtrEnable = false;
        protected bool rtsEnable = false;
        protected int pollingInterval = 100;
        protected int queryInterval = 200;
        protected int responseTimeout = 1000;
        protected string receivedData = string.Empty;
        protected Handshake handShake = Handshake.None;
        protected DeviceStates deviceState = DeviceStates.Idle;
        protected System.Timers.Timer timer = null;
        protected System.Timers.ElapsedEventHandler periodicTaskFunc = null;
        protected Queue<string> packets = new Queue<string>();
        protected Stopwatch pollingTimer = new Stopwatch();
        protected Stopwatch queryTimer = new Stopwatch();
        protected Stopwatch responseCheckTimer = new Stopwatch();
        #endregion

        #region Properties
        public virtual DeviceStates DeviceState => deviceState;
        public virtual bool IsOpen { get; }
        public virtual bool Decoded { get; protected set; }
        public virtual int PollingInterval
        {
            get => pollingInterval;
            set => pollingInterval = Math.Max(100, value);
        }
        public virtual int QueryInterval
        {
            get => queryInterval;
            set => queryInterval = Math.Min(1000, pollingInterval * 5);
        }
        public virtual int ResponseTimeout => responseTimeout;
        public virtual string ReceivedData => receivedData;
        public int ReadBufferSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int WriteBufferSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ReadTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int WriteTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ConnectionTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ConversationTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ReconnectionInterval { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Encoding Encoding { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public CommunicationIoInterface DeviceInterface => throw new NotImplementedException();
        public IReadOnlyList<string> Commands => throw new NotImplementedException();
        public string LastCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string LastResponse { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Board { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Manufacturer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Model { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string PartNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SerialNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion

        #region Protected methods
        protected virtual void ResetPollingTimer()
        {
            pollingTimer.Stop();
            pollingTimer.Reset();
        }

        protected virtual void ResetQueryTimer()
        {
            queryTimer.Stop();
            queryTimer.Reset();
        }

        protected virtual void SetResponseCheckTimer(int timeout)
        {
            if (timeout > 0)
            {
                Interlocked.Exchange(ref responseTimeout, Math.Max(timeout, queryInterval));
                responseCheckTimer.Restart();
            }
        }

        protected virtual void ResetResponseCheckTimer()
        {
            Interlocked.Exchange(ref responseTimeout, 0);
            responseCheckTimer.Stop();
            responseCheckTimer.Reset();
        }

        protected virtual void OnPeriodicQuery(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Simulation)
                return;
        }

        protected virtual void CreatePeriodicTask(System.Timers.ElapsedEventHandler func = null)
        {
            DestroyPeriodicTask();

            if (timer == null)
            {
                timer = new System.Timers.Timer(PollingInterval = 500);
                timer.Elapsed += ((func == null) ? periodicTaskFunc = OnPeriodicQuery : periodicTaskFunc = func);
                timer.AutoReset = true;
                timer.Start();
            }
        }

        protected virtual void DestroyPeriodicTask()
        {
            if (timer != null)
            {
                if (periodicTaskFunc != null)
                    timer.Elapsed -= periodicTaskFunc;

                timer.Stop();
                timer.Dispose();
                timer = null;
            }

            ResetPollingTimer();
            ResetQueryTimer();
            ResetResponseCheckTimer();
        }

        protected virtual void DoCheckResponseTimeout()
        {
            throw new NotImplementedException();
        }

        protected virtual int EnqueResponse(char delimiter = (char)AsciiControlCharacters.Cr)
        {
            throw new NotImplementedException();
        }

        protected virtual void ProcessDequeueResponse(bool multiplePacket = true)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnReceived(string data)
        {
            throw new NotImplementedException();
        }

        protected virtual bool Send(string str, bool attachCr = true, int timeout = 0)
        {
            throw new NotImplementedException();
        }

        protected virtual bool Send(byte[] data, int timeout = 1000)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Public methods
        public virtual void Connect()
        {
            throw new NotImplementedException();
        }

        public virtual void Disconnect()
        {
            throw new NotImplementedException();
        }

        public virtual bool Create(XmlNode node)
        {
            throw new NotImplementedException();
        }

        public virtual void Destroy()
        {
            throw new NotImplementedException();
        }

        public virtual void Reconnect()
        {
            throw new NotImplementedException();
        }

        public virtual int Send(string data)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Protected methods
        #endregion
    }
}
#endregion