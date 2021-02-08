#region Imports
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using TechFloor.Components;
#endregion

#region Program
namespace TechFloor.Service.MYCRONIC.WebService
{
    public class ReelTowerNotificationListener
    {
        #region Enumerations
        #endregion

        #region Fields
        protected readonly ManualResetEvent shutdownEvent = new ManualResetEvent(false);

        protected readonly ManualResetEvent startedEvent = new ManualResetEvent(false);

        protected bool initialized = false;

        protected bool disconnected = false;

        protected string address = "127.0.0.1";

        protected int port = 9000;

        protected IPEndPoint endPoint = null;

        protected UdpClient server = null;

        protected CommunicationStates state = CommunicationStates.Disconnected;

        protected StringBuilder receivedMsg = new StringBuilder();

        protected Thread serverThread = null;
        #endregion

        #region Properties
        public virtual bool Initialized => initialized;

        public virtual bool Running => state == CommunicationStates.Listening;

        public virtual string Address
        {
            get => address;
            set => address = value;
        }

        public virtual int Port
        {
            get => port;
            set => port = value;
        }

        public virtual CommunicationStates State => state;
        #endregion

        #region Events
        public virtual event EventHandler<string> ReportReceivedMessage;
        #endregion

        #region Constructors
        protected ReelTowerNotificationListener() { }
        #endregion

        #region Structrues
        public struct UdpState
        {
            public UdpClient Socket;
            public IPEndPoint EndPoint;
        }
        #endregion

        #region Protected methods
        protected void FireReceivedMessage()
        {
            if (ReportReceivedMessage != null)
            {
                ReportReceivedMessage.Invoke(this, receivedMsg.ToString());
                receivedMsg.Clear();
            }
        }

        protected void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                UdpState state_ = new UdpState();
                UdpClient socket_ = ((UdpState)(ar.AsyncState)).Socket;
                IPEndPoint endPoint_ = ((UdpState)(ar.AsyncState)).EndPoint;

                byte[] buf_ = socket_.EndReceive(ar, ref endPoint_);

                if (buf_ != null && buf_.Length > 0)
                {
                    lock (receivedMsg)
                        receivedMsg.AppendLine(Encoding.ASCII.GetString(buf_, 0, buf_.Length));
                    
                    state_.EndPoint = endPoint;

                    FireReceivedMessage();

                    if (server != null)
                    {
                        state_.Socket = server;
                        state_.EndPoint = endPoint;
                        server.BeginReceive(new AsyncCallback(ReceiveCallback), state_);
                    }
                }
                else
                {
                    if (server != null)
                        server.Close();

                    state = CommunicationStates.Disconnected;
                    disconnected = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");

                if (server != null)
                    server.Close();

                state = CommunicationStates.Error;
                disconnected = true;
            }
            finally
            {
                if (disconnected)
                {
                    if (!shutdownEvent.WaitOne(100))
                    {
                        Start();
                    }
                }
            }
        }

        protected virtual void Run()
        {
            try
            {
                UdpState state_ = new UdpState();
                state_.EndPoint = endPoint;
                state_.Socket = server;

                state = CommunicationStates.Created;

                if (server != null)
                    server.BeginReceive(new AsyncCallback(ReceiveCallback), state_);

                state = CommunicationStates.Listening;
                startedEvent.Set();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
            finally
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: State={state}");
            }
        }
        #endregion

        #region Public methods
        public virtual bool Create(EventHandler<string> handler = null)
        {
            initialized = false;
            this.endPoint = new IPEndPoint(IPAddress.Any, port);
            this.server = new UdpClient(endPoint);

            if (handler != null)
                ReportReceivedMessage += handler;

            return initialized = true;
        }

        public virtual void Destroy()
        {
            Stop();
            initialized = false;
        }

        public virtual CommunicationStates Start(int timeout = 1000)
        {
            if (shutdownEvent != null)
            {
                startedEvent.Reset();
                shutdownEvent.Reset();
                serverThread = new Thread(new ThreadStart(Run));
                serverThread.Start();
                WaitForStart(timeout);
            }

            return state;
        }

        public virtual CommunicationStates Stop(int timeout = 1000)
        {
            if (shutdownEvent != null)
                shutdownEvent.Set();

            return state;
        }

        public virtual CommunicationStates WaitForStart(int timeout = 1000)
        {
            startedEvent.WaitOne(timeout);
            return state;
        }

        public virtual string GetMessage()
        {
            string result_ = string.Empty;

            if (receivedMsg.Length > 0)
            {
                lock (receivedMsg)
                {
                    result_ = receivedMsg.ToString();
                    receivedMsg.Clear();
                }
            }

            return result_;
        }
        #endregion
    }
}
#endregion