#region Imports
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Xml;
using TechFloor.Object;
using TechFloor.Util;
#endregion

#region Program
#pragma warning disable CS0067
#pragma warning disable CS0114
namespace TechFloor.Device.CommunicationIo.EthernetIo
{
    public class NetworkStateObject // : AbstractClassDisposable
    {
        #region Constants
        public const int BufferSize = 8192; // 8 kbytes
        #endregion

        #region Fields
        public int id = 0;
        public Socket workSocket = null;
        public string connectionInfo = string.Empty;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
        #endregion
    }

    public class AsyncTcpClient : AbstractClassDisposable, IAsynchronousCommunicationEventHandler
    {
        #region Constants
        public const int DefaultBufferSize = 8192;
        public const int MaxBufferSize = 1048576;
        #endregion

        #region Fields
        private int readBufferSize = DefaultBufferSize;
        private int writeBufferSize = DefaultBufferSize;
        protected bool elementEnabled = true;
        protected bool terminate = false;
        protected bool autoReconnect = true;
        protected int elementBoard = 0;
        protected int elementId = 0;
        protected int readTimeout = 1000;
        protected int writeTimeout = 1000;
        protected int conversationTimeout = 1000;
        protected int connectionTimeout = 1000;
        protected int reconnectionInterval = 500;
        protected string elementName = string.Empty;
        protected string elementManufacturer = string.Empty;
        protected string elementModel = string.Empty;
        protected string elementDescription = string.Empty;
        protected string response = string.Empty;
        protected string endOfFile = "\r";
        protected Socket client = null;
        protected EthernetPortSettings ethernetPortSettings;
        protected AutoResetEvent connectDone = new AutoResetEvent(false);
        protected AutoResetEvent sendDone = new AutoResetEvent(false);
        protected AutoResetEvent receiveDone = new AutoResetEvent(false);
        protected NetworkStateObject stateObject = null;
        protected EventHandler<CommunicationIoEventArgs> receiveDataFunctionPtr = null;
        #endregion

        #region Events
        public virtual event EventHandler<CommunicationIoEventArgs> Connected;
        public virtual event EventHandler<CommunicationIoEventArgs> Disconnected;
        public virtual event EventHandler<CommunicationIoEventArgs> Sent;
        public virtual event EventHandler<CommunicationIoEventArgs> Received;
        public virtual event EventHandler<CommunicationIoEventArgs> FailedToConnect;
        public virtual event EventHandler<CommunicationIoEventArgs> FailedToSend;
        public virtual event EventHandler<CommunicationIoEventArgs> RemoteProcessData;
        public virtual event EventHandler<CommunicationIoEventArgs> FailedToDecode;
        public virtual event EventHandler<CommunicationIoEventArgs> ResponseTimedout;
        #endregion

        #region Properties
        public virtual bool Enabled
        {
            get => elementEnabled;
            set => elementEnabled = value;
        }

        public virtual int Board
        {
            get => elementBoard;
            set => elementBoard = value;
        }

        public virtual int Id
        {
            get => elementId;
            set => elementId = value;
        }

        public virtual string Name
        {
            get => elementName;
            set => elementName = value;
        }

        public virtual string Description
        {
            get => elementDescription;
            set => elementDescription = value;
        }

        public virtual string Manufacturer
        {
            get => elementManufacturer;
            set => elementManufacturer = value;
        }

        public virtual string Model
        {
            get => elementModel;
            set => elementModel = value;
        }

        public virtual string PartNumber { get; set; }

        public virtual string SerialNumber { get; set; }

        public virtual CommunicationIoInterface DeviceInterface => CommunicationIoInterface.Ethernet;

        public virtual EthernetPortSettings EthernetPortSettingParameters
        {
            get => ethernetPortSettings;
            set => ethernetPortSettings = value;
        }

        public virtual string IpAddress
        {
            get => ethernetPortSettings.IpAddress;
            set => ethernetPortSettings.IpAddress = value;
        }

        public virtual int Port
        {
            get => ethernetPortSettings.Port;
            set => ethernetPortSettings.Port = value;
        }

        public virtual int ReadBufferSize
        {
            get => readBufferSize;
            set
            {
                if (value < DefaultBufferSize)
                    readBufferSize = DefaultBufferSize;
                else if (value > MaxBufferSize)
                    readBufferSize = MaxBufferSize;
                else
                    readBufferSize = value;
            }
        }

        public virtual int WriteBufferSize
        {
            get => writeBufferSize;
            set
            {
                if (value < DefaultBufferSize)
                    writeBufferSize = DefaultBufferSize;
                else if (value > MaxBufferSize)
                    writeBufferSize = MaxBufferSize;
                else
                    writeBufferSize = value;
            }
        }

        public virtual int ReadTimeout
        {
            get => readTimeout;
            set => readTimeout = (value >= 0 && value <= 60000) ? value : 1000;
        }

        public virtual int WriteTimeout
        {
            get => writeTimeout;
            set => writeTimeout = (value >= 0 && value <= 60000) ? value : 1000;
        }

        public virtual int ConversationTimeout
        {
            get => conversationTimeout;
            set => conversationTimeout = (value >= 0 && value <= 60000) ? value : 10000;
        }

        public virtual int ConnectionTimeout
        {
            get => connectionTimeout;
            set => connectionTimeout = (value >= 0 && value <= 60000) ? value : 1000;
        }

        public virtual int ReconnectionInterval
        {
            get => reconnectionInterval;
            set => reconnectionInterval = (value >= 0 && connectionTimeout > value) ? value : (int)(connectionTimeout * 0.5);
        }

        public virtual Encoding Encoding { get; set; }

        public virtual IReadOnlyList<string> Commands => null;

        public virtual string LastCommand { get; set; }

        public virtual string LastResponse
        {
            get => response;
            set => response = value;
        }

        public virtual bool IsOpen => (client != null) ? client.Connected : false;

        public virtual string RemoteEndPointInfo
        {
            get
            {
                if (client != null && client.Connected)
                    return client.RemoteEndPoint.ToString();

                return string.Empty;
            }
        }

        public virtual Socket Sock => client;
        #endregion

        #region Constructors
        public AsyncTcpClient(string address, EventHandler<CommunicationIoEventArgs> function, int timeout = 1000, bool autoreconnect = true)
        {
            string[] param = address.Split(':');

            if ((receiveDataFunctionPtr = function) != null)
                Received += receiveDataFunctionPtr;

            if (param.Length >= 2 && StringLogicalComparer.IsDigit(param[1]))
            {
                ethernetPortSettings.IpAddress = param[0];
                ethernetPortSettings.Port = int.Parse(param[1]);
                connectionTimeout = timeout;
                autoReconnect = autoreconnect;
            }
        }

        public AsyncTcpClient(string address = "127.0.0.1:50000", int timeout = 1000, bool autoreconnect = true)
        {
            string[] param = address.Split(':');

            if (param.Length >= 2)
            {
                ethernetPortSettings.IpAddress = param[0];
                ethernetPortSettings.Port = int.Parse(param[1]);
                connectionTimeout = timeout;
                autoReconnect = autoreconnect;
            }
        }

        public AsyncTcpClient(string address, int port, int timeout = 1000, bool autoreconnect = true)
        {
            ethernetPortSettings.IpAddress = address;
            ethernetPortSettings.Port = port;
            connectionTimeout = timeout;
            autoReconnect = autoreconnect;
        }

        public AsyncTcpClient(int id, Socket connection, EventHandler<CommunicationIoEventArgs> function = null, bool autoreconnect = true)
        {
            if (connection != null)
            {
                elementId = id;
                IPEndPoint endpoint = (client = connection).RemoteEndPoint as IPEndPoint;
                ethernetPortSettings.IpAddress = endpoint.Address.ToString();
                ethernetPortSettings.Port = endpoint.Port;
                autoReconnect = autoreconnect;
                SetSocketOptions(connection, true);

                if (function != null)
                {
                    Connected += function;

                    using (CommunicationIoEventArgs args = new CommunicationIoEventArgs(elementId))
                        OnConnected(args);
                }
            }
        }
        #endregion

        #region Protected methods
        protected override void DisposeManagedObjects()
        {
            base.DisposeManagedObjects();

            if (receiveDataFunctionPtr != null)
                Received -= receiveDataFunctionPtr;

            if (IsOpen)
                Disconnect();
        }

        // [HandleProcessCorruptedStateExceptions, SecurityCritical]
        protected virtual void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket sock = (Socket)ar.AsyncState;

                if (sock != null && sock.Handle != null)
                {
                    sock.EndConnect(ar);
                    connectDone.Set();
                    ReportConnection(sock);
                }
            }
            catch (Exception ex)
            {
                if (!terminate)
                    Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");

                Reconnect();

                using (CommunicationIoEventArgs args = new CommunicationIoEventArgs(elementId, ex.Message))
                    OnFailedToSend(args);
            }
        }

        // [HandleProcessCorruptedStateExceptions, SecurityCritical]
        protected virtual bool Receive(Socket client)
        {
            try
            {
                if (IsOpen)
                {
                    if (stateObject == null)
                        stateObject = new NetworkStateObject();

                    stateObject.workSocket = client;
                    client.BeginReceive(stateObject.buffer, 0,
                        NetworkStateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), stateObject);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                Reconnect();

                using (CommunicationIoEventArgs args = new CommunicationIoEventArgs(elementId, ex.Message))
                    OnDisconnected(args);
            }

            return false;
        }

        // [HandleProcessCorruptedStateExceptions, SecurityCritical]
        protected virtual void ReceiveCallback(IAsyncResult ar)
        {
            int bytesRead = 0;

            try
            {
                NetworkStateObject state = (NetworkStateObject)ar.AsyncState;
                Socket sock = state.workSocket;

                bytesRead = sock.EndReceive(ar);

                if (bytesRead > 0)
                {
                    byte[] buffer = new byte[bytesRead];
                    Buffer.BlockCopy(state.buffer, 0, buffer, 0, bytesRead);
                    response = Encoding.UTF8.GetString(state.buffer, 0, bytesRead);
                    CallResponseProcessTask(buffer, bytesRead);
                    receiveDone.Set();

                    using (CommunicationIoEventArgs args = new CommunicationIoEventArgs(elementId, response, buffer, bytesRead))
                        OnReceived(args);

                    client.BeginReceive(state.buffer, 0,
                        NetworkStateObject.BufferSize,
                        0, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    if (bytesRead > 0)
                    {
                        byte[] buffer = new byte[bytesRead];
                        Buffer.BlockCopy(state.buffer, 0, buffer, 0, bytesRead);
                        CallResponseProcessTask(buffer, bytesRead);
                        receiveDone.Set();

                        using (CommunicationIoEventArgs args = new CommunicationIoEventArgs(elementId, response, buffer, bytesRead))
                            OnReceived(args);
                    }
                    else
                    {
                        Reconnect();

                        using (CommunicationIoEventArgs args = new CommunicationIoEventArgs(elementId, "Disconnected by server socket close"))
                            OnDisconnected(args);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!terminate)
                    Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");

                Reconnect();

                using (CommunicationIoEventArgs args = new CommunicationIoEventArgs(elementId, ex.Message))
                    OnDisconnected(args);
            }
        }

        // [HandleProcessCorruptedStateExceptions, SecurityCritical]
        protected virtual void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket sock = (Socket)ar.AsyncState;

                int bytesSent = sock.EndSend(ar);
                Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Sent {bytesSent} bytes to server.");
                sendDone.Set();

                using (CommunicationIoEventArgs args = new CommunicationIoEventArgs(elementId, $"Sent {bytesSent} bytes to server."))
                    OnSent(args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");

                using (CommunicationIoEventArgs args = new CommunicationIoEventArgs(elementId, ex.Message))
                {
                    OnFailedToSend(args);
                    OnDisconnected(args);
                }

                Reconnect();
            }
        }

        protected virtual void CallResponseProcessTask(byte[] data, int size)
        {
            if (size <= 0)
                return;

            Debug.WriteLine(string.Format($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Received data {size} bytes."));

            ThreadStart internal_ = delegate
            {
                if (RemoteProcessData != null)
                {
                    byte[] rbuf = new byte[size];
                    data.CopyTo(rbuf, 0);

                    using (CommunicationIoEventArgs args = new CommunicationIoEventArgs(elementId, rbuf, size))
                        RemoteProcessData?.Invoke(this, args);
                }
                else
                    OnProcessResponse(data, size);
            };
            new Thread(internal_).Start();
        }

        // [HandleProcessCorruptedStateExceptions, SecurityCritical]
        protected virtual void OnProcessResponse(byte[] data, int bytes)
        {
            try
            {
                string buf = Encoding.ASCII.GetString(data);

                if (string.IsNullOrEmpty(buf))
                    Debug.WriteLine(string.Format($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Data={buf}"));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected virtual bool SetSocketOptions(Socket sock, bool connected = false)
        {
            if (connectDone != null && client != null && client.Handle != null)
            {
                SocketExtensions.SetKeepAlive(client, 5000, 1000);

                if (connected)
                {
                    ReportConnection(sock);
                }
                else
                {
                    IPAddress ipAddress = IPAddress.Parse(ethernetPortSettings.IpAddress);
                    IPEndPoint remoteEP = new IPEndPoint(ipAddress, ethernetPortSettings.Port);
                    var result = client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);

                    if (connectDone.WaitOne(connectionTimeout, true))
                        return true;
                    else
                        throw new Exception("Connection failed...");
                }
            }

            return false;
        }

        protected virtual void ReportConnection(Socket sock)
        {
            using (CommunicationIoEventArgs args = new CommunicationIoEventArgs(elementId, sock.RemoteEndPoint.ToString()))
                OnConnected(args);

            Receive(sock);
        }
        #endregion

        #region Public methods
        // [HandleProcessCorruptedStateExceptions, SecurityCritical]
        public virtual bool Create(XmlNode node)
        {
            try
            {
                foreach (XmlAttribute attr in node.Attributes)
                {
                    switch (attr.Name.ToLower())
                    {
                        case "id": elementId = int.Parse(attr.Value); break;
                        case "name": elementName = attr.Value; break;
                        case "ip": ethernetPortSettings.IpAddress = attr.Value; break;
                        case "port": ethernetPortSettings.Port = int.Parse(attr.Value); break;
                        case "timeout": connectionTimeout = int.Parse(attr.Value); break;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                return false;
            }
        }

        public virtual void Destroy(bool forced = false)
        {
            if (forced)
                Terminate();
            else
                Disconnect();
            Dispose();
        }

        // [HandleProcessCorruptedStateExceptions, SecurityCritical]
        public virtual bool Connect()
        {
            if (client != null)
            {
                if (client.Connected)
                    return true;
                else
                    Disconnect();
            }

            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                return SetSocketOptions(client);
            }
            catch (Exception ex)
            {
                Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}(Exception)={ex.Message}");
            }

            return false;
        }

        public virtual void Terminate()
        {
            terminate = true;
            Disconnect();
        }

        public virtual void Disconnect()
        {
            if (client != null)
            {
                if (client.Connected)
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }

                client.Dispose();
                client = null;
            }

            if (stateObject != null)
            {
                // stateObject.Dispose();
                stateObject = null;
            }
        }

        public virtual int Send(string data)
        {
            if (!IsOpen)
                return 0;

            byte[] byteData = Encoding.ASCII.GetBytes(data);
            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
            return byteData.Length;
        }

        public virtual int Send(byte[] data)
        {
            if (!IsOpen)
                return 0;

            client.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), client);
            return data.Length;
        }

        public virtual void Reconnect()
        {
            Disconnect();

            if (!terminate && autoReconnect)
            {
                Thread.Sleep(reconnectionInterval);
                Connect();
            }
        }

        public virtual void OnConnected(CommunicationIoEventArgs args)
        {
            Connected?.Invoke(this, args);
        }

        public virtual void OnDisconnected(CommunicationIoEventArgs args)
        {
            Disconnected?.Invoke(this, args);
        }

        public virtual void OnSent(CommunicationIoEventArgs args)
        {
            Sent?.Invoke(this, args);
        }

        public virtual void OnReceived(CommunicationIoEventArgs args)
        {
            Received?.Invoke(this, args);
        }

        public virtual void OnFailedToConnect(CommunicationIoEventArgs args)
        {
            FailedToConnect?.Invoke(this, args);
        }

        public virtual void OnFailedToSend(CommunicationIoEventArgs args)
        {
            FailedToSend?.Invoke(this, args);
        }

        public void FireConnected(CommunicationIoEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void FireDisconnected(CommunicationIoEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void FireSent(CommunicationIoEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void FireReceived(CommunicationIoEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void FireFailedToConnect(CommunicationIoEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void FireFailedToSend(CommunicationIoEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void FireFailedToDecode(CommunicationIoEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void FireResponseTimeout(CommunicationIoEventArgs args)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    public class AsyncTcpServer : AbstractClassDisposable
    {
        #region Constants
        public const int DefaultBufferSize = 8192;
        public const int MaxBufferSize = 1048576;
        #endregion

        #region Fields
        protected bool elementEnabled = true;
        protected bool terminate = false;
        protected bool running = false;
        protected int count = -1;
        protected int elementBoard = 0;
        protected int elementId = 0;
        protected string elementName = string.Empty;
        protected string elementManufacturer = string.Empty;
        protected string elementModel = string.Empty;
        protected string elementDescription = string.Empty;
        protected string response = string.Empty;
        protected string endOfFile = "\r";
        protected Socket server = null;
        protected EncodingTypes encodingType = EncodingTypes.Default;
        protected EthernetPortSettings ethernetPortSettings;
        #endregion

        #region Events
        public virtual event EventHandler<AsyncSocketEventArgs> Sent;
        public virtual event EventHandler<AsyncSocketEventArgs> Accepted;
        public virtual event EventHandler<AsyncSocketEventArgs> Failure;
        public virtual event EventHandler ServiceStarted;
        public virtual event EventHandler ServiceStopped;
        #endregion

        #region Properties
        public virtual bool Enabled
        {
            get => elementEnabled;
            set => elementEnabled = value;
        }

        public virtual int Board
        {
            get => elementBoard;
            set => elementBoard = value;
        }

        public virtual int Id
        {
            get => elementId;
            set => elementId = value;
        }

        public virtual string Name
        {
            get => elementName;
            set => elementName = value;
        }

        public virtual string Description
        {
            get => elementDescription;
            set => elementDescription = value;
        }

        public virtual string Manufacturer
        {
            get => elementManufacturer;
            set => elementManufacturer = value;
        }

        public virtual string Model
        {
            get => elementModel;
            set => elementModel = value;
        }

        public virtual string PartNumber { get; set; }

        public virtual string SerialNumber { get; set; }

        public virtual CommunicationIoInterface DeviceInterface => CommunicationIoInterface.Ethernet;

        public virtual EthernetPortSettings EthernetPortSettingParameters
        {
            get => ethernetPortSettings;
            set => ethernetPortSettings = value;
        }

        public virtual string IpAddress
        {
            get => ethernetPortSettings.IpAddress;
            set => ethernetPortSettings.IpAddress = value;
        }

        public virtual int Port
        {
            get => ethernetPortSettings.Port;
            set => ethernetPortSettings.Port = value;
        }

        public virtual Encoding Encoding { get; set; }

        public virtual bool IsRunning => running;
        #endregion

        #region Constructors
        public AsyncTcpServer(string address = "127.0.0.1:50000", EncodingTypes encoding = EncodingTypes.Default)
        {
            string[] param = address.Split(':');
            if (param.Length >= 2)
            {
                ethernetPortSettings.IpAddress = param[0];
                ethernetPortSettings.Port = int.Parse(param[1]);
            }

            switch (encoding)
            {
                case EncodingTypes.UTF7:
                    Encoding = Encoding.UTF7;
                    break;
                case EncodingTypes.BigEndianUnicode:
                    Encoding = Encoding.BigEndianUnicode;
                    break;
                case EncodingTypes.Unicode:
                    Encoding = Encoding.Unicode;
                    break;
                case EncodingTypes.Default:
                    Encoding = Encoding.Default;
                    break;
                case EncodingTypes.ASCII:
                    Encoding = Encoding.ASCII;
                    break;
                case EncodingTypes.UTF8:
                    Encoding = Encoding.UTF8;
                    break;
                case EncodingTypes.UTF32:
                    Encoding = Encoding.UTF32;
                    break;
            }
        }

        public AsyncTcpServer(string address, int port, EncodingTypes encoding = EncodingTypes.Default)
        {
            ethernetPortSettings.IpAddress = address;
            ethernetPortSettings.Port = port;

            switch (encoding)
            {
                case EncodingTypes.UTF7:
                    Encoding = Encoding.UTF7;
                    break;
                case EncodingTypes.BigEndianUnicode:
                    Encoding = Encoding.BigEndianUnicode;
                    break;
                case EncodingTypes.Unicode:
                    Encoding = Encoding.Unicode;
                    break;
                case EncodingTypes.Default:
                    Encoding = Encoding.Default;
                    break;
                case EncodingTypes.ASCII:
                    Encoding = Encoding.ASCII;
                    break;
                case EncodingTypes.UTF8:
                    Encoding = Encoding.UTF8;
                    break;
                case EncodingTypes.UTF32:
                    Encoding = Encoding.UTF32;
                    break;
            }
        }
        #endregion

        #region Protected methods
        protected override void DisposeManagedObjects()
        {
            base.DisposeManagedObjects();
            Terminate();
        }

        protected virtual void FireErrorEvent(AsyncSocketEventArgs e)
        {
            Failure?.Invoke(this, e);
        }

        protected virtual void FireSentEvent(AsyncSocketEventArgs e)
        {
            Sent?.Invoke(this, e);
        }

        protected virtual void FireAcceptedEvent(AsyncSocketEventArgs e)
        {
            Accepted?.Invoke(this, e);
        }

        protected virtual void FireServiceStartEvent()
        {
            ServiceStarted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void FireServiceStopEvent()
        {
            ServiceStopped?.Invoke(this, EventArgs.Empty);
        }

        // [HandleProcessCorruptedStateExceptions, SecurityCritical]
        protected virtual void StartAccept()
        {
            try
            {
                server.BeginAccept(new AsyncCallback(OnAcceptCallback), server);
            }
            catch (Exception e)
            {
                FireErrorEvent(new AsyncSocketEventArgs(Id, e));
            }
        }

        // [HandleProcessCorruptedStateExceptions, SecurityCritical]
        protected virtual void OnAcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket server = (Socket)ar.AsyncState;
                Socket client = server.EndAccept(ar);
                FireAcceptedEvent(new AsyncSocketEventArgs(++count, client));
                StartAccept();
            }
            catch (Exception ex)
            {
                FireErrorEvent(new AsyncSocketEventArgs(count, ex));

                if (!terminate)
                {
                    Stop();
                    Start();
                }
            }
        }
        #endregion

        #region Public methods
        // [HandleProcessCorruptedStateExceptions, SecurityCritical]
        public virtual bool Create(XmlNode node)
        {
            try
            {
                foreach (XmlAttribute attr in node.Attributes)
                {
                    switch (attr.Name.ToLower())
                    {
                        case "id": elementId = int.Parse(attr.Value); break;
                        case "name": elementName = attr.Value; break;
                        case "ip": ethernetPortSettings.IpAddress = attr.Value; break;
                        case "port": ethernetPortSettings.Port = int.Parse(attr.Value); break;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                return false;
            }
        }

        public virtual void Destroy()
        {
            Terminate();
            Dispose();
        }

        public virtual void Start()
        {
            Start(ethernetPortSettings.IpAddress, ethernetPortSettings.Port);
        }

        // [HandleProcessCorruptedStateExceptions, SecurityCritical]
        public virtual void Start(string address, int port, int backlog = 100)
        {
            if (server != null)
                return;

            try
            {
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(IPAddress.Parse(address), port));
                server.Listen(backlog);
                StartAccept();
                running = true;
                FireServiceStartEvent();
            }
            catch (Exception e)
            {
                FireErrorEvent(new AsyncSocketEventArgs(Id, e));
            }
        }

        // [HandleProcessCorruptedStateExceptions, SecurityCritical]
        public virtual void Stop(bool forced = false)
        {
            try
            {
                if (server != null)
                {
                    if (server.IsBound)
                        server.Close(100);
                }

                server = null;
                running = false;
                terminate = !forced;
                FireServiceStopEvent();
            }
            catch (Exception e)
            {
                FireErrorEvent(new AsyncSocketEventArgs(Id, e));
            }
        }

        public virtual void Terminate()
        {
            terminate = true;
        }
        #endregion
    }

    public abstract class AbstractClassEthernetIo : AbstractClassCommunicationIo, IEthernetIo, IAsynchronousCommunicationEventHandler
    {
        #region Fields
        protected AsyncTcpClient asyncTcpSocket = null;
        protected EthernetPortSettings ethernetPortSettings;
        #endregion

        #region Properties
        public override DeviceStates DeviceState => deviceState;
        public override bool IsOpen => (asyncTcpSocket == null ? false : asyncTcpSocket.IsOpen);
        public EthernetPortSettings EthernetPortSettingParameters
        {
            get => ethernetPortSettings;
            set
            {
                if (!IsOpen)
                    ethernetPortSettings = value;
            }
        }
        #endregion

        #region Events
        public virtual event EventHandler<CommunicationIoEventArgs> Connected;
        public virtual event EventHandler<CommunicationIoEventArgs> Disconnected;
        public virtual event EventHandler<CommunicationIoEventArgs> Sent;
        public virtual event EventHandler<CommunicationIoEventArgs> Received;
        public virtual event EventHandler<CommunicationIoEventArgs> FailedToConnect;
        public virtual event EventHandler<CommunicationIoEventArgs> FailedToSend;
        public virtual event EventHandler<CommunicationIoEventArgs> RemoteProcessData;
        public virtual event EventHandler<CommunicationIoEventArgs> FailedToDecode;
        public virtual event EventHandler<CommunicationIoEventArgs> ResponseTimedout;
        #endregion

        #region Constructors
        public AbstractClassEthernetIo(string ip, int port, int timeout = 1000)
        {
            Create(ip, port, timeout);
        }
        #endregion

        #region Destructor
        ~AbstractClassEthernetIo()
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

            if (asyncTcpSocket != null)
            {
                asyncTcpSocket.Connected -= OnConnected;
                asyncTcpSocket.Disconnected -= OnDisconnected;
                asyncTcpSocket.Sent -= OnSent;
                asyncTcpSocket.Received -= OnReceived;
                asyncTcpSocket.FailedToConnect -= OnFailedToConnect;
                asyncTcpSocket.FailedToSend -= OnFailedToSend;
                asyncTcpSocket.RemoteProcessData -= OnRemoteProcessData;
                asyncTcpSocket.Terminate();
                asyncTcpSocket.Dispose();
                asyncTcpSocket = null;
            }
        }

        protected virtual void OnRemoteProcessData(object sender, CommunicationIoEventArgs e)
        {
            if (RemoteProcessData != null)
                RemoteProcessData?.Invoke(sender, e);
        }

        protected virtual void OnConnected(object sender, CommunicationIoEventArgs e)
        {
            FireConnected(e);
        }

        protected virtual void OnDisconnected(object sender, CommunicationIoEventArgs e)
        {
            FireDisconnected(e);
        }

        protected virtual void OnSent(object sender, CommunicationIoEventArgs e)
        {
            FireSent(e);
        }

        protected virtual void OnReceived(object sender, CommunicationIoEventArgs e)
        {
            FireReceived(e);
        }

        protected virtual void OnFailedToConnect(object sender, CommunicationIoEventArgs e)
        {
            FireFailedToConnect(e);
        }

        protected virtual void OnFailedToSend(object sender, CommunicationIoEventArgs e)
        {
            FireFailedToSend(e);
        }

        protected virtual void OnFailedToDecode(object sender, CommunicationIoEventArgs e)
        {
            FireFailedToDecode(e);
        }

        protected virtual void OnResponseTimeout(object sender, CommunicationIoEventArgs e)
        {
            FireResponseTimeout(e);
        }
        #endregion

        #region Public methods
        // [HandleProcessCorruptedStateExceptions, SecurityCritical]
        public virtual void Create(string ip, int port, int timeout = 1000)
        {
            try
            {
                if ((asyncTcpSocket = new AsyncTcpClient(ip, port, timeout)) != null)
                {
                    asyncTcpSocket.Connected += OnConnected;
                    asyncTcpSocket.Disconnected += OnDisconnected;
                    asyncTcpSocket.Sent += OnSent;
                    asyncTcpSocket.Received += OnReceived;
                    asyncTcpSocket.FailedToConnect += OnFailedToConnect;
                    asyncTcpSocket.FailedToSend += OnFailedToSend;
                    asyncTcpSocket.RemoteProcessData += OnRemoteProcessData;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public void Connect(string ip, int port)
        {
            asyncTcpSocket.IpAddress = ip;
            asyncTcpSocket.Port = port;
            Connect();
        }

        // [HandleProcessCorruptedStateExceptions, SecurityCritical]
        public override void Connect()
        {
            Disconnect();

            try
            {
                if (asyncTcpSocket != null && !asyncTcpSocket.IsOpen)
                    asyncTcpSocket.Connect();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                FireFailedToConnect(null);
            }
        }

        public override void Disconnect()
        {
            receivedData = string.Empty;
            Decoded = false;
            queryTimer.Stop();
            queryTimer.Reset();

            if (IsOpen)
            {
                if (asyncTcpSocket != null)
                    asyncTcpSocket.Disconnect();
            }
        }

        protected override bool Send(string str, bool attachCr = true, int timeout = 0)
        {
            if (!IsOpen || asyncTcpSocket == null)
                return false;

            return (asyncTcpSocket.Send(str) > 0) ? true : false;
        }

        public void Terminate()
        {
            asyncTcpSocket.Terminate();
        }

        public virtual void FireConnected(CommunicationIoEventArgs args)
        {
            deviceState = DeviceStates.Idle;

            if (Connected != null)
                Connected?.Invoke(this, args);
        }

        public virtual void FireDisconnected(CommunicationIoEventArgs args)
        {
            deviceState = DeviceStates.Disconnected;

            if (Disconnected != null)
                Disconnected?.Invoke(this, args);
        }

        public virtual void FireSent(CommunicationIoEventArgs args)
        {
            deviceState = DeviceStates.SentCommand;

            if (Sent != null)
                Sent?.Invoke(this, args);
        }

        public virtual void FireReceived(CommunicationIoEventArgs args)
        {
            deviceState = DeviceStates.ReceivedData;
            ResetResponseCheckTimer();

            if (Received != null)
                Received?.Invoke(this, args);
        }

        public virtual void FireFailedToConnect(CommunicationIoEventArgs args)
        {
            deviceState = DeviceStates.ConnectionFailure;

            if (FailedToConnect != null)
                FailedToConnect?.Invoke(this, args);
        }

        public virtual void FireFailedToSend(CommunicationIoEventArgs args)
        {
            deviceState = DeviceStates.SentFailure;

            if (FailedToSend != null)
                FailedToSend?.Invoke(this, args);
        }

        public virtual void FireFailedToDecode(CommunicationIoEventArgs args)
        {
            deviceState = DeviceStates.ReceivedWrongData;

            if (FailedToDecode != null)
                FailedToDecode?.Invoke(this, args);
        }

        public void FireResponseTimeout(CommunicationIoEventArgs e)
        {
            deviceState = DeviceStates.ResponseTimeout;
            ResetResponseCheckTimer();

            if (ResponseTimedout != null)
                ResponseTimedout?.Invoke(this, e);
        }
        #endregion
    }

    public abstract class AbstractClassEthernetIoServer : SimulatableDevice
    {
        #region Enumerations
        public enum DeviceStates
        {
            Stopped,
            Started,
            Listen,
            Accepted,
            Broadcast,
            Failure,
        }

        public enum CommunicationStates
        {
            Disconnected,
            Connected,
            Failure,
        }
        #endregion

        #region Fields
        protected AsyncTcpServer asyncTcpSocket = null;
        protected DeviceStates deviceState = DeviceStates.Stopped;
        protected CommunicationStates communicationState = CommunicationStates.Disconnected;
        protected Dictionary<int, AsyncTcpClient> clients = new Dictionary<int, AsyncTcpClient>();
        #endregion

        #region Events
        public event EventHandler ChangedServerState;
        public event EventHandler ChangedCommunicationState;
        #endregion

        #region Properties
        public virtual DeviceStates DeviceState => deviceState;
        public virtual CommunicationStates CommunicationState => communicationState;
        public virtual bool IsRunning => (asyncTcpSocket == null ? false : asyncTcpSocket.IsRunning);

        public virtual EthernetPortSettings EthernetPortSettingParameters
        {
            get => asyncTcpSocket.EthernetPortSettingParameters;
            set => asyncTcpSocket.EthernetPortSettingParameters = value;
        }

        public virtual Encoding Encoding => asyncTcpSocket.Encoding;
        #endregion

        #region Constructors
        public AbstractClassEthernetIoServer(string ip, int port, EncodingTypes encoding = EncodingTypes.Default, bool singleconnection = true)
        {
            Create(ip, port);
        }
        #endregion

        #region Destructor
        ~AbstractClassEthernetIoServer()
        {
            Terminate();
            Dispose(false);
        }
        #endregion

        #region Protected methods
        protected override void DisposeManagedObjects()
        {
            base.DisposeManagedObjects();

            if (asyncTcpSocket != null)
            {
                asyncTcpSocket.Sent -= OnSent;
                asyncTcpSocket.Accepted -= OnAccepted;
                asyncTcpSocket.Failure -= OnFailure;
                asyncTcpSocket.ServiceStarted -= OnServiceStarted;
                asyncTcpSocket.ServiceStopped -= OnServiceStopped;
                asyncTcpSocket.Terminate();
                asyncTcpSocket.Dispose();
                asyncTcpSocket = null;
            }
        }

        protected virtual void FireChangedServerState(DeviceStates state)
        {
            if (deviceState != state)
            {
                deviceState = state;
                ChangedServerState?.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void FireChangedCommunicationState(CommunicationStates state)
        {
            if (communicationState != state)
            {
                communicationState = state;
                ChangedCommunicationState?.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnAccepted(object sender, AsyncSocketEventArgs e)
        {
            foreach (var client in clients)
            {
                IPEndPoint s1 = client.Value.Sock.RemoteEndPoint as IPEndPoint;
                IPEndPoint s2 = e.Connection.RemoteEndPoint as IPEndPoint;

                if (s1.Address.ToString().Contains(s2.Address.ToString()))
                    client.Value.Terminate();
            }

            FireChangedServerState(DeviceStates.Accepted);

            Thread.Sleep(100);
            AsyncTcpClient newClient = new AsyncTcpClient(e.Id, e.Connection, OnClientConnected, false);
            newClient.Disconnected += OnClientDisconnected;
            newClient.Sent += OnClientSent;
            newClient.Received += OnClientReceived;
            newClient.FailedToConnect += OnClientFailToConnect;
            newClient.FailedToDecode += OnClientFailToDecode;
            newClient.FailedToSend += OnClientFailToSend;
            newClient.ResponseTimedout += OnResponseTimeout;

            lock (clients)
                clients.Add(e.Id, newClient);
        }

        protected virtual void OnSent(object sender, AsyncSocketEventArgs e)
        {
            FireChangedServerState(DeviceStates.Broadcast);
        }

        protected virtual void OnFailure(object sender, AsyncSocketEventArgs e)
        {
            lock (clients)
            {
                if (clients.ContainsKey(e.Id))
                {
                    clients[e.Id].Destroy(true);
                    clients.Remove(e.Id);
                }
            }

            FireChangedServerState(DeviceStates.Failure);
            Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: ({e.Id}) Exception={e.Exception}");
        }

        protected virtual void OnServiceStarted(object sender, EventArgs e)
        {
            FireChangedServerState(DeviceStates.Started);
            Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Server started");
            FireChangedServerState(DeviceStates.Listen);
        }

        protected virtual void OnServiceStopped(object sender, EventArgs e)
        {
            lock (clients)
            {
                foreach (var obj in clients)
                    obj.Value.Destroy(true);

                clients.Clear();
            }

            FireChangedServerState(DeviceStates.Stopped);
            Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Server stopped");
        }

        protected virtual void OnClientConnected(object sender, CommunicationIoEventArgs e)
        {
            FireChangedCommunicationState(CommunicationStates.Connected);
            Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Connected a client ({e.Id})");
        }

        protected virtual void OnClientDisconnected(object sender, CommunicationIoEventArgs e)
        {
            lock (clients)
            {
                if (clients.ContainsKey(e.Id))
                {
                    clients[e.Id].Destroy(true);
                    clients.Remove(e.Id);
                }
            }

            FireChangedCommunicationState(CommunicationStates.Disconnected);
            Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Disconnected a client ({e.Id})");
        }

        protected virtual void OnClientSent(object sender, CommunicationIoEventArgs e)
        {
            Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Tx({e.Id})={e.Text}");
        }

        protected virtual void OnClientReceived(object sender, CommunicationIoEventArgs e)
        {
            Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Rx({e.Id})={e.Text}");
        }

        protected virtual void OnClientFailToConnect(object sender, CommunicationIoEventArgs e)
        {
            FireChangedCommunicationState(CommunicationStates.Failure);
        }

        protected virtual void OnClientFailToDecode(object sender, CommunicationIoEventArgs e)
        {

        }

        protected virtual void OnClientFailToSend(object sender, CommunicationIoEventArgs e)
        {

        }

        protected virtual void OnResponseTimeout(object sender, CommunicationIoEventArgs e)
        {

        }
        #endregion

        #region Public methods
        // [HandleProcessCorruptedStateExceptions, SecurityCritical]
        public virtual void Create(string ip, int port, EncodingTypes encoding = EncodingTypes.Default)
        {
            try
            {
                if ((asyncTcpSocket = new AsyncTcpServer(ip, port, encoding)) != null)
                {
                    asyncTcpSocket.Sent += OnSent;
                    asyncTcpSocket.Accepted += OnAccepted;
                    asyncTcpSocket.Failure += OnFailure;
                    asyncTcpSocket.ServiceStarted += OnServiceStarted;
                    asyncTcpSocket.ServiceStopped += OnServiceStopped;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType()}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public virtual void Start()
        {
            if (asyncTcpSocket != null)
                asyncTcpSocket.Start();
        }

        public virtual void Stop()
        {
            if (asyncTcpSocket != null)
                asyncTcpSocket.Stop();
        }

        public void Terminate()
        {
            if (asyncTcpSocket != null)
            {
                asyncTcpSocket.Terminate();
                asyncTcpSocket.Dispose();
            }
        }

        public virtual int Send(string data)
        {
            int result_ = 0;

            foreach (var client in clients)
            {
                if (client.Value.IsOpen)
                    result_ = client.Value.Send(data);
            }

            return result_;
        }

        public virtual int Send(byte[] data)
        {
            int result_ = 0;

            foreach (var client in clients)
            {
                if (client.Value.IsOpen)
                    result_ = client.Value.Send(data);
            }

            return result_;
        }
        #endregion
    }
}
#endregion