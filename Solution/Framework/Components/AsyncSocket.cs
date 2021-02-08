#region Imports
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
#endregion

#region Program
namespace TechFloor.Components
{
    #region Enumerations
    public enum CommunicationStates
    {
        None,
        Created,
        Listening,
        Connecting,
        Accepted,
        Connected,
        Error,
        Disconnected,
    }
    #endregion

    public class StateObject
    {
        #region Constants
        protected const int BUFFER_SIZE = Int16.MaxValue + 1;
        #endregion

        #region Fields
        protected Socket worker = null;

        protected byte[] buffer = new byte[BUFFER_SIZE];
        #endregion

        #region Properties
        public Socket Worker
        {
            get => worker;
            set => worker = value;
        }

        public byte[] Buffer
        {
            get => buffer;
            set => buffer = value;
        }

        public int BufferSize => BUFFER_SIZE;
        #endregion

        #region Constructors
        public StateObject(Socket worker)
        {
            this.worker = worker;
            Array.Clear(buffer, 0, BUFFER_SIZE);
        }
        #endregion
    }

    public class AsyncSocketErrorEventArgs : EventArgs
    {
        #region Constants
        protected readonly Exception exception;

        protected readonly int id = 0;
        #endregion

        #region Properties
        public Exception AsyncSocketException => exception;

        public int Id => id;
        #endregion

        #region Constructors
        public AsyncSocketErrorEventArgs(int id, Exception exception)
        {
            this.id = id;
            this.exception = exception;
        }
        #endregion
    }

    public class AsyncSocketConnectionEventArgs : EventArgs
    {
        #region Constants
        protected readonly int id = 0;
        #endregion

        #region Properties
        public int Id => this.id;
        #endregion

        #region Constructors
        public AsyncSocketConnectionEventArgs(int id)
        {
            this.id = id;
        }
        #endregion
    }

    public class AsyncSocketSendEventArgs : EventArgs
    {
        #region Constants
        protected readonly int id = 0;

        protected readonly int sendBytes;
        #endregion

        #region Properties
        public int SendBytes => sendBytes;

        public int Id => id;
        #endregion

        #region Constructors
        public AsyncSocketSendEventArgs(int id, int sentbytes)
        {
            this.id = id;
            this.sendBytes = sentbytes;
        }
        #endregion
    }

    public class AsyncSocketReceiveEventArgs : EventArgs
    {
        #region Constants
        protected readonly int id = 0;

        protected readonly int receiveBytes = 0;

        protected readonly byte[] receiveData = null;
        #endregion

        #region Properties
        public int ReceiveBytes => receiveBytes;

        public byte[] ReceiveData => receiveData;

        public int Id => id;
        #endregion

        #region Constructors
        public AsyncSocketReceiveEventArgs(int id, int receivebytes, byte[] receivedata)
        {
            this.id = id;

            if ((this.receiveBytes = receivebytes) > 0)
            {
                this.receiveData = new byte[receivebytes];
                Buffer.BlockCopy(receivedata, 0, receiveData, 0, receivebytes);
            }
        }
        #endregion
    }

    public class AsyncSocketAcceptEventArgs : EventArgs
    {
        #region Constants
        protected readonly Socket conn = null;
        #endregion

        #region Constructors
        public AsyncSocketAcceptEventArgs(Socket conn)
        {
            this.conn = conn;
        }
        #endregion

        #region Properties
        public Socket Worker => conn;
        #endregion
    }

    public delegate void AsyncSocketErrorEventHandler(object sender, AsyncSocketErrorEventArgs e);

    public delegate void AsyncSocketConnectedEventHandler(object sender, AsyncSocketConnectionEventArgs e);

    public delegate void AsyncSocketDisconnectedEventHandler(object sender, AsyncSocketConnectionEventArgs e);

    public delegate void AsyncSocketSentEventHandler(object sender, AsyncSocketSendEventArgs e);

    public delegate void AsyncSocketReceivedEventHandler(object sender, AsyncSocketReceiveEventArgs e);

    public delegate void AsyncSocketAcceptedEventHandler(object sender, AsyncSocketAcceptEventArgs e);

    public class AsyncSocketClass
    {
        #region Fields
        protected int id;

        protected Socket socket = null;
        #endregion

        #region Properties
        public int Id => id;

        public Socket Socket
        {
            get => socket;
            set => socket = value;
        }
        #endregion

        #region Events
        public event AsyncSocketErrorEventHandler OnError;

        public event AsyncSocketConnectedEventHandler OnConnected;

        public event AsyncSocketDisconnectedEventHandler OnDisconnected;

        public event AsyncSocketSentEventHandler OnSent;

        public event AsyncSocketReceivedEventHandler OnReceived;

        public event AsyncSocketAcceptedEventHandler OnAccepted;
        #endregion

        #region Constructors
        public AsyncSocketClass()
        {
            this.id = -1;
        }

        public AsyncSocketClass(int id)
        {
            this.id = id;
        }
        #endregion

        #region Protected methods
        protected virtual void FireErrorEvent(AsyncSocketErrorEventArgs e)
        {
            OnError?.Invoke(this, e);
        }

        protected virtual void FireConnectedEvent(AsyncSocketConnectionEventArgs e)
        {
            OnConnected?.Invoke(this, e);
        }

        protected virtual void FireDisconnectedEvent(AsyncSocketConnectionEventArgs e)
        {
            OnDisconnected?.Invoke(this, e);
        }

        protected virtual void FireSentEvent(AsyncSocketSendEventArgs e)
        {
            OnSent?.Invoke(this, e);
        }

        protected virtual void FireReceivedEvent(AsyncSocketReceiveEventArgs e)
        {
            OnReceived?.Invoke(this, e);
        }

        protected virtual void FireAcceptedEvent(AsyncSocketAcceptEventArgs e)
        {
            OnAccepted?.Invoke(this, e);
        }
        #endregion
    }

    public class AsyncSocketClient : AsyncSocketClass
    {
        #region Fields
        protected bool connected = false;

        protected Exception sockException = null;

        protected ManualResetEvent timeoutObject = new ManualResetEvent(false);
        #endregion

        #region Properties
        public bool IsConnected => socket != null ? socket.Connected : false;
        #endregion

        #region Constructors
        public AsyncSocketClient(int id)
        {
            this.id = id;
        }

        public AsyncSocketClient(int id,
            Socket sock,
            AsyncSocketConnectedEventHandler handlerconnected = null,
            AsyncSocketSentEventHandler handlersent = null,
            AsyncSocketReceivedEventHandler handlerreceive = null)
        {
            this.id = id;
            this.socket = sock;

            if (handlersent != null)
                OnSent += handlersent;

            if (handlerreceive != null)
                OnReceived += handlerreceive;

            if (handlerconnected != null)
            {
                OnConnected += handlerconnected;
                FireConnectionEvent();
            }
        }
        #endregion

        #region Protected methods
        protected void FireConnectionEvent(bool async = true)
        {
            if (async)
                Receive();

            FireConnectedEvent(new AsyncSocketConnectionEventArgs(Id));
        }

        protected void OnConnectCallback(IAsyncResult ar)
        {
            try
            {
                try
                {
                    // connected = false;
                    Socket client_ = (Socket)ar.AsyncState;

                    if (client_ != null)
                    {
                        client_.EndConnect(ar);
                        socket = client_;
                        connected = true;
                        FireConnectionEvent();
                    }
                }
                catch (Exception ex)
                {
                    connected = false;
                    sockException = ex;
                }
                finally
                {
                    timeoutObject.Set();
                }

            }
            catch (Exception ex)
            {
                FireErrorEvent(new AsyncSocketErrorEventArgs(Id, ex));
            }
        }

        protected void OnReceiveCallback(IAsyncResult ar)
        {
            int readbytes_ = 0;

            try
            {
                StateObject so = (StateObject)ar.AsyncState;

                if (so != null && so.Worker != null && so.Worker.Connected)
                {
                    if ((readbytes_ = so.Worker.EndReceive(ar)) > 0)
                    {
                        FireReceivedEvent(new AsyncSocketReceiveEventArgs(Id, readbytes_, so.Buffer));
                        Receive();
                    }
                    else
                        Disconnect();
                }
            }
            catch (Exception ex)
            {
                FireErrorEvent(new AsyncSocketErrorEventArgs(Id, ex));
            }
        }

        protected void OnSendCallback(IAsyncResult ar)
        {
            int writebytes_ = 0;

            try
            {
                Socket client = (Socket)ar.AsyncState;
                writebytes_ = client.EndSend(ar);
                FireSentEvent(new AsyncSocketSendEventArgs(Id, writebytes_));
            }
            catch (Exception ex)
            {
                FireErrorEvent(new AsyncSocketErrorEventArgs(Id, ex));
            }
        }

        protected void OnDisconnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndDisconnect(ar);

                if (!client.Connected)
                    client.Close();

                FireDisconnectedEvent(new AsyncSocketConnectionEventArgs(Id));
            }
            catch (Exception ex)
            {
                FireErrorEvent(new AsyncSocketErrorEventArgs(Id, ex));
            }
        }
        #endregion

        #region Public methods
        public bool Connect(string address, int port, int timeout = 1000)
        {
            bool result_ = true;

            try
            {
                connected = false;
                sockException = null;
                timeoutObject.Reset();

                IPAddress[] addrs_ = Dns.GetHostAddresses(address);
                IPEndPoint endpoint_ = new IPEndPoint(addrs_[0], port);
                Socket client_ = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client_.BeginConnect(endpoint_, new AsyncCallback(OnConnectCallback), client_);

                if (timeoutObject.WaitOne(timeout, false))
                {
                    if (!(result_ = connected))
                        throw sockException;
                }
                else
                {
                    client_.Close();
                    result_ = false;
                    throw new TimeoutException($"Connection timeout ({timeout} ms). {address}:{port}");
                }
            }
            catch (Exception ex)
            {
                FireErrorEvent(new AsyncSocketErrorEventArgs(Id, ex));
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public bool Connect(IPAddress address, int port, int timeout = 1000)
        {
            bool result_ = true;

            try
            {
                sockException = null;
                timeoutObject.Reset();

                IPEndPoint endpoint_ = new IPEndPoint(address, port);
                Socket client_ = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client_.BeginConnect(endpoint_, new AsyncCallback(OnConnectCallback), client_);

                if (timeoutObject.WaitOne(timeout, false))
                {
                    if (!(result_ = connected))
                        throw sockException;
                }
                else
                {
                    client_.Close();
                    result_ = false;
                    throw new TimeoutException($"Connection timeout ({timeout} ms). {address}:{port}");
                }
            }
            catch (Exception ex)
            {
                FireErrorEvent(new AsyncSocketErrorEventArgs(Id, ex));
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public void Receive()
        {
            try
            {
                StateObject so_ = new StateObject(socket);
                so_.Worker.BeginReceive(so_.Buffer, 0, so_.BufferSize, 0, new AsyncCallback(OnReceiveCallback), so_);
            }
            catch (Exception ex)
            {
                FireErrorEvent(new AsyncSocketErrorEventArgs(Id, ex));
            }
        }


        public bool Send(byte[] buffer)
        {
            bool result_ = false;

            try
            {
                if (socket != null && socket.Connected)
                {
                    socket.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(OnSendCallback), socket);
                    result_ = true;
                }
            }
            catch (Exception ex)
            {
                FireErrorEvent(new AsyncSocketErrorEventArgs(Id, ex));
            }

            return result_;
        }


        public void Disconnect()
        {
            try
            {
                if (socket != null && socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.BeginDisconnect(false, new AsyncCallback(OnDisconnectCallback), socket);
                }
            }
            catch (Exception ex)
            {
                FireErrorEvent(new AsyncSocketErrorEventArgs(Id, ex));
            }
        }
        #endregion
    }

    public class AsyncSocketServer : AsyncSocketClass
    {
        #region Constants
        protected const int backLog = 100;
        #endregion

        #region Fields
        protected bool autoreconnect = true;

        protected bool running = false;

        protected int port;
        #endregion

        #region Properties
        public bool AutoReconnect => autoreconnect;

        public int Port => port;

        public bool IsRunning => running;
        #endregion

        #region Events
        public event EventHandler OnServiceStarted;

        public event EventHandler OnServiceStopped;
        #endregion

        #region Constructors
        public AsyncSocketServer(int port)
        {
            this.port = port;
        }
        #endregion

        #region Protected methods
        protected void FireServiceStartEvent()
        {
            OnServiceStarted?.Invoke(this, EventArgs.Empty);
        }

        protected void FireServiceStopEvent()
        {
            OnServiceStopped?.Invoke(this, EventArgs.Empty);
        }

        protected void StartAccept()
        {
            try
            {
                socket.BeginAccept(new AsyncCallback(OnAcceptCallback), socket);
            }
            catch (Exception ex)
            {
                FireErrorEvent(new AsyncSocketErrorEventArgs(Id, ex));
            }
        }

        protected void OnAcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket server = (Socket)ar.AsyncState;
                Socket client = server.EndAccept(ar);
                FireAcceptedEvent(new AsyncSocketAcceptEventArgs(client));
                StartAccept();
            }
            catch (Exception ex)
            {
                FireErrorEvent(new AsyncSocketErrorEventArgs(Id, ex));

                // if (autoreconnect)
                // {
                //     Stop();
                //     Start();
                // }
            }
        }
        #endregion

        #region Public methods
        public bool Start()
        {
            bool result = false;

            if (socket == null)
            {
                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Bind(new IPEndPoint(IPAddress.Any, port));
                    socket.Listen(backLog);
                    StartAccept();
                    running = true;
                    FireServiceStartEvent();
                    result = true;
                }
                catch (Exception ex)
                {
                    FireErrorEvent(new AsyncSocketErrorEventArgs(Id, ex));
                }
            }

            return result;
        }

        public void Stop(bool forced = false)
        {
            try
            {
                if (socket != null)
                {
                    // if (socket.IsBound)
                    socket.Close(100);
                }

                socket = null;
                running = false;

                autoreconnect = !forced;

                FireServiceStopEvent();
            }
            catch (Exception ex)
            {
                FireErrorEvent(new AsyncSocketErrorEventArgs(Id, ex));
            }
        }
        #endregion
    }
}
#endregion