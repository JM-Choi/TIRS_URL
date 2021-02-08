#region Imports
using TechFloor.Device.CommunicationIo;
using TechFloor.Device.CommunicationIo.EthernetIo;
using TechFloor.Object;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
#endregion

#region Program
namespace TechFloor.Device
{
    public interface IScriptableCommunicator
    {
        #region Properties
        TimeSpan ResponseTimeout { get; set; }

        Encoding Encoding { get; set; }

        string LineTerminator { get; set; }
        #endregion

        #region Events
        event EventHandler<string> Received;

        event EventHandler<Exception> CatchedException;
        #endregion

        #region Public methods
        bool Connect(string address);

        void WriteLine(string data, params object[] args);

        void Write(string data, params object[] args);

        void Close();
        #endregion
    }

    public abstract class ScriptableCommunicator : AbstractClassDisposable, IScriptableCommunicator
    {
        #region Fields
        private Encoding encoding_;

        private string lineterminator_;
        #endregion

        #region Properties
        public virtual TimeSpan ResponseTimeout { get; set; }

        public virtual Encoding Encoding
        {
            get => encoding_;
            set
            {
                if (value == null)
                    throw (new InvalidOperationException("The value of Encoding must not be null"));

                encoding_ = value;
                return;
            }
        }

        public virtual string LineTerminator
        {
            get => lineterminator_;
            set
            {
                if (value == null)
                    throw (new InvalidOperationException("The value of LineTerminator must not be null"));

                lineterminator_ = value;
            }
        }

        protected virtual System.Timers.Timer tmr { get; set; }
        #endregion

        #region Events
        public virtual event EventHandler Connected;

        public virtual event EventHandler Disconnected;

        public virtual event EventHandler<string> Received;

        public virtual event EventHandler<Exception> CatchedException;
        #endregion

        #region Protected methods
        protected ScriptableCommunicator(TimeSpan timeout, Encoding encoding, string terminator)
        {
            ResponseTimeout = timeout;
            Encoding = encoding;
            LineTerminator = terminator;
            tmr = null;
            return;
        }
        #endregion

        #region Public methods
        public abstract bool Connect(string address);

        public abstract void Write(string data, params object[] args);

        public abstract void Close();

        public virtual void WriteLine(string fmt, params object[] args)
        {
            Write(fmt + lineterminator_, args);
        }

        protected virtual void FireConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void FireDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void FireReceived(string data)
        {
            if (tmr != null)
                tmr.Stop();

            Received?.Invoke(this, data);

            if (tmr != null)
                tmr.Start();
        }

        protected virtual void FireCatchedException(Exception ex)
        {
            CatchedException?.Invoke(this, ex);
        }

        protected override void DisposeManagedObjects()
        {
            Close();
            base.DisposeManagedObjects();
        }
        #endregion
    }

    public class TelnetClient : ScriptableCommunicator
    {
        #region Fields
        private int port_               = 23;
        private string address_         = string.Empty;
        private string user_            = string.Empty;
        private string password_        = "adept"; // Login password
        private AsyncTcpClient socket_  = null;
        #endregion

        #region Properties
        public bool IsConnected => (socket_ != null ? socket_.IsOpen : false);
        #endregion

        #region Constructor
        public TelnetClient(string address, int port, string user = null, string password = null)
            : base(new TimeSpan(0, 1, 0), Encoding.ASCII, "\r")
        {
            address_    = address;
            port_       = port;
            user_       = user;
            password_   = password;
        }
        #endregion

        #region Public Methods
        public override bool Connect(string address)
        {
            if (address == null)
                throw (new ArgumentNullException("Host", "Host must not be null"));

            string[] parts = address.Split(new char[] { ':' }, 2);
            int port = 23;

            if ((parts.Length > 1) && !int.TryParse(parts[1], out port))
                throw (new ArgumentException("Unable to parse the port", "Host"));

            return DoConnect(parts[0], port);
        }

        public virtual bool Connect(string address, int port)
        {
            if (address == null)
                throw (new ArgumentNullException("Host", "Host must not be null"));

            return DoConnect(address, port);
        }

        public override void Write(string fmt, params object[] args)
        {
            if (IsConnected)
            {
                try
                {
                    if ((args != null) && (args.Length > 0))
                        fmt = string.Format(fmt, args);

                    byte[] data = Encoding.GetBytes(fmt);
                    socket_.Send(data);
                }
                catch (Exception err)
                {
                    FireCatchedException(err);
                    //throw;
                }
            }
            else
            {
                throw (new InvalidOperationException("The socket appears to be closed"));
            }
        }

        public override void Close()
        {
            if (socket_ != null)
            {
                socket_.Terminate();
                socket_.Dispose();
                socket_ = null;
            }
        }

        public virtual void Login(int delay = 0)
        {
            if (!string.IsNullOrEmpty(user_))
                WriteLine(user_);

            if (delay > 0)
                Thread.Sleep(delay);

            if (!string.IsNullOrEmpty(password_))
                WriteLine(password_);
        }
        #endregion

        #region Protected methods
        protected virtual bool DoConnect(string address, int port)
        {
            if (socket_ != null)
                Close();

            socket_ = new AsyncTcpClient(address, port, 1000, true);
            socket_.Connected += OnConnected;
            socket_.Disconnected += OnDisconnected;
            socket_.FailedToConnect += OnFailedToConnect;
            socket_.FailedToDecode += OnFailedToDecode;
            socket_.FailedToSend += OnFailedToSend;
            socket_.Received += OnReceived;
            socket_.Sent += OnSent;
            socket_.ResponseTimedout += OnResponseTimedout;
            return socket_.Connect();
        }

        protected virtual void OnConnected(object sender, CommunicationIoEventArgs e)
        {
            FireConnected();
        }

        protected virtual void OnDisconnected(object sender, CommunicationIoEventArgs e)
        {
            FireDisconnected();
        }

        protected virtual void OnFailedToConnect(object sender, CommunicationIoEventArgs e)
        {
        }

        protected virtual void OnFailedToDecode(object sender, CommunicationIoEventArgs e)
        {
        }

        protected virtual void OnFailedToSend(object sender, CommunicationIoEventArgs e)
        {
        }

        protected virtual void OnReceived(object sender, CommunicationIoEventArgs e)
        {
            FireReceived(e.Text);
        }

        protected virtual void OnSent(object sender, CommunicationIoEventArgs args)
        {

        }

        protected virtual void OnResponseTimedout(object sender, CommunicationIoEventArgs args)
        {

        }
        #endregion

        #region Negotiator class
        protected class Negotiator : AbstractClassDisposable
        {
            #region Structures
            private struct TelnetByte
            {
                public const byte GA    = 249;
                public const byte WILL  = 251;
                public const byte WONT  = 252;
                public const byte DO    = 253;
                public const byte DONT  = 254;
                public const byte IAC   = 255;
                public const byte ECHO  = 1;
                public const byte SUPP  = 3;
            }
            #endregion

            #region Fields
            private NetworkStream stream_;
            #endregion

            #region Constructors
            public Negotiator(NetworkStream stream)
            {
                stream_ = stream;
            }
            #endregion

            #region Protected methods
            protected override void DisposeManagedObjects()
            {
                if (stream_ != null)
                    stream_ = null;

                base.DisposeManagedObjects();
            }
            #endregion

            #region Public methods
            public int Negotiate(byte[] buf, int count)
            {
                int resplen = 0;
                int index = 0;

                while (index < count)
                {
                    if (buf[index] == TelnetByte.IAC)
                    {
                        try
                        {
                            switch (buf[index + 1])
                            {
                                case TelnetByte.IAC:
                                    {
                                        buf[resplen++] = buf[index];
                                        index += 2;
                                    }
                                    break;

                                case TelnetByte.GA:
                                    {
                                        index += 2;
                                    }
                                    break;

                                case TelnetByte.DO:
                                case TelnetByte.DONT:
                                    {
                                        buf[index + 1] = TelnetByte.WONT;

                                        lock (stream_)
                                        {
                                            stream_.Write(buf, index, 3);
                                        }

                                        index += 3;
                                    }
                                    break;

                                case TelnetByte.WONT:
                                    {
                                        buf[index + 1] = TelnetByte.DONT;

                                        lock (stream_)
                                        {
                                            stream_.Write(buf, index, 3);
                                        }

                                        index += 3;
                                    }
                                    break;

                                case TelnetByte.WILL:
                                    {
                                        byte action = TelnetByte.DONT;

                                        if (buf[index + 2] == TelnetByte.ECHO)
                                        {
                                            action = TelnetByte.DO;
                                        }
                                        else if (buf[index + 2] == TelnetByte.SUPP)
                                        {
                                            action = TelnetByte.DO;
                                        }

                                        buf[index + 1] = action;

                                        lock (stream_)
                                        {
                                            stream_.Write(buf, index, 3);
                                        }

                                        index += 3;
                                    }
                                    break;
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            index = count;
                        }
                    }
                    else
                    {
                        if (buf[index] != 0)
                            buf[resplen++] = buf[index];

                        index++;
                    }
                }

                return (resplen);
            }
            #endregion
        }
        #endregion
    }
}
#endregion