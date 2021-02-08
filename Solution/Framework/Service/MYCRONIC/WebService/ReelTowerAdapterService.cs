#region Imports
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using RoyoTech.StSys.WebService;
using RoyoTech.StSys.WebService.SharedModel;
using RoyoTech.StSys.WebService.SharedService;
#endregion

#region Program
#pragma warning disable CS0628
namespace TechFloor.Service.MYCRONIC.WebService
{
    public class ReelTowerAdapterService : IWSAdapter
    {
        #region Fields
        protected string address = "127.0.0.1";

        protected int port = 9000;

        protected bool byThread;
        #endregion

        #region Properties
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

        public virtual bool UseThread
        {
            get => byThread;
            set => byThread = value;
        }
        #endregion

        protected sealed class PacketRouter : IDisposable
        {
            #region Fields
            protected readonly IPEndPoint endPoint;

            protected readonly string message;
            #endregion

            #region Constructors
            public PacketRouter(string message, string address, int port)
            {
                this.endPoint = new IPEndPoint(IPAddress.Parse(address), port);
                this.message = message;
            }
            #endregion

            #region Public methods
            public void Dispose()
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Disposed");
            }

            public void Run()
            {
                try
                {
                    using (UdpClient client_ = new UdpClient())
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            int sentBytes_ = 0;
                            byte[] packet_ = Encoding.ASCII.GetBytes($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")};"  + message);

                            if ((sentBytes_ = client_.Send(packet_, packet_.Length, endPoint)) > 0)
                                Debug.WriteLine($"Send> ({sentBytes_}):{message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
            }
            #endregion
        }

        #region Constructors
        public ReelTowerAdapterService()
        {
            this.byThread = false;
        }
        #endregion

        #region Public methods
        public virtual string AdapterCommand(string xmlString)
        {
            XmlResult res_ = new XmlResult();
            XmlCommand cmd_ = XmlCommandSerializer.DeserializeCommand(xmlString);
            res_.Errorcode = 0;
            res_.Message = string.Empty;
            string reply_ = XmlCommandSerializer.Serialize(res_);

            try
            {
                if (byThread)
                {
                    new Thread((obj) =>
                    {
                        if (obj != null)
                        {
                            using (PacketRouter router_ = obj as PacketRouter)
                            {
                                router_.Run();
                            }
                        }
                    }).Start(new PacketRouter($"{cmd_.Command};{cmd_.Parameter};", address, port));
                }
                else
                {
                    using (PacketRouter router_ = new PacketRouter($"{cmd_.Command};{cmd_.Parameter};", address, port))
                    {
                        router_.Run();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return reply_;
        }
        #endregion
    }
}
#endregion