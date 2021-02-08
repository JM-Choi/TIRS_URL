#region Licenses
///////////////////////////////////////////////////////////////////////////////
/// MIT License
/// 
/// Copyright (c) 2019 Marcus Software Ltd.
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
/// SOFTWARE.
///////////////////////////////////////////////////////////////////////////////
///          Copyright Joe Coder 2004 - 2006.
/// Distributed under the Boost Software License, Version 1.0.
///    (See accompanying file LICENSE_1_0.txt or copy at
///          https://www.boost.org/LICENSE_1_0.txt)
///////////////////////////////////////////////////////////////////////////////
/// 저작권 (c) 2019 Marcus Software Ltd. (isadrastea.kor@gmail.com)
///
/// 본 라이선스의 적용을 받는 소프트웨어와 동봉된 문서(소프트웨어)를 획득하는 
/// 모든 개인이나 기관은 소프트웨어를 Marcus Software (isadrastea.kor@gmail.com)
/// 에 신고하고, 허용 의사를 서면으로 득하여, 사용, 복제, 전시, 배포, 실행 및
/// 전송할 수 있고, 소프트웨어의 파생 저작물을 생성할 수 있으며, 소프트웨어가
/// 제공된 제3자에게 그러한 행위를 허용할 수 있다. 단, 이 모든 행위는 다음과 
/// 같은 조건에 의해 제한 한다.:
///
/// 소프트웨어의 저작권 고지, 그리고 위의 라이선스 부여와 이 규정과 아래의 부인 
/// 조항을 포함한 이 글의 전문이 소프트웨어를 전체적으로나 부분적으로 복제한 
/// 모든 복제본과 소프트웨어의 모든 파생 저작물 내에 포함되어야 한다. 단, 해당 
/// 복제본이나 파생저작물이 소스 언어 프로세서에 의해 생성된, 컴퓨터로 인식 
/// 가능한 오브젝트 코드의 형식으로만 되어 있는 경우는 제외된다.
///
/// 이 소프트웨어는 상품성, 특정 목적에의 적합성, 소유권, 비침해에 대한 보증을 
/// 포함한, 이에 국한되지는 않는, 모든 종류의 명시적이거나 묵시적인 보증 없이 
///“있는 그대로의 상태”로 제공된다. 저작권자나 소프트웨어의 배포자는 어떤 
/// 경우에도 소프트웨어 자체나 소프트웨어의 취급과 관련하여 발생한 손해나 기타 
/// 책임에 대하여, 계약이나 불법행위 등에 관계 없이 어떠한 책임도 지지 않는다.
///////////////////////////////////////////////////////////////////////////////
/// project ReelTower 
/// @author Marcus (isadrastea.kor@gmail.com)
/// @namespace Marcus.Solution.TechFloor
/// @file AsyncSocket.cs
/// @brief
/// @details
/// @date 2019, 5, 21, 오후 1:13
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using Marcus.Solution.TechFloor.Object;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Threading;
#endregion

#region Program
namespace Marcus.Solution.TechFloor
{
    #region Enumerations
    public enum CommunicationStates
    {
        None,
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
        public int ID => id;
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
        public int ID => this.id;
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
        public int ID => id;
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
        public int ID => id;
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
        protected int Id;
        #endregion

        #region Properties
        public int ID => Id;
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
            Id = -1;
        }

        public AsyncSocketClass(int id)
        {
            Id = id;
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
        protected Socket sock = null;
        protected Exception sockException = null;
        protected ManualResetEvent timeoutObject = new ManualResetEvent(false);
        #endregion

        #region Properties
        public Socket Sock
        {
            get => sock;
            set => sock = value;
        }

        public bool IsOpen => sock != null ? sock.Connected : false;
        #endregion

        #region Constructors
        public AsyncSocketClient(int id)
        {
            Id = id;
        }

        public AsyncSocketClient(int id,
            Socket conn,
            AsyncSocketConnectedEventHandler handlerconnected = null,
            AsyncSocketSentEventHandler handlersent = null,
            AsyncSocketReceivedEventHandler handlerreceive = null)
        {
            Id = id;
            sock = conn;

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
                    connected = false;
                    Socket client = (Socket)ar.AsyncState;

                    if (client != null)
                    {
                        client.EndConnect(ar);
                        sock = client;
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
                sockException = null;
                timeoutObject.Reset();

                IPAddress[] ips = Dns.GetHostAddresses(address);
                IPEndPoint remoteEP = new IPEndPoint(ips[0], port);
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.BeginConnect(remoteEP, new AsyncCallback(OnConnectCallback), client);

                if (timeoutObject.WaitOne(timeout, false))
                {
                    if (!(result_ = connected))
                        throw sockException;
                }
                else
                {
                    client.Close();
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
                StateObject so = new StateObject(sock);
                so.Worker.BeginReceive(so.Buffer, 0, so.BufferSize, 0, new AsyncCallback(OnReceiveCallback), so);
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
                if (sock != null && sock.Connected)
                {
                    sock.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(OnSendCallback), sock);
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
                if (sock != null && sock.Connected)
                {
                    sock.Shutdown(SocketShutdown.Both);
                    sock.BeginDisconnect(false, new AsyncCallback(OnDisconnectCallback), sock);
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
        protected Socket sock;
        #endregion

        #region Properties
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
                sock.BeginAccept(new AsyncCallback(OnAcceptCallback), sock);
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

                if (autoreconnect)
                {
                    Stop();
                    Start();
                }
            }
        }
        #endregion

        #region Public methods

        public void Start()
        {
            if (sock != null)
                return;

            try
            {
                autoreconnect = true;
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.Bind(new IPEndPoint(IPAddress.Any, port));
                sock.Listen(backLog);
                StartAccept();
                running = true;
                FireServiceStartEvent();
            }
            catch (Exception ex)
            {
                FireErrorEvent(new AsyncSocketErrorEventArgs(Id, ex));
            }
        }

        public void Stop(bool forced = false)
        {
            try
            {
                if (sock != null)
                {
                    if (sock.IsBound)
                        sock.Close(100);
                }

                sock = null;
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