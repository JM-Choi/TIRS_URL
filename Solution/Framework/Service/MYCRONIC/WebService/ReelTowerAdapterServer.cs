#region Imports
using System;
using System.Diagnostics;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using RoyoTech.StSys.WebService;
#endregion

#region Program
namespace TechFloor.Service.MYCRONIC.WebService
{
    public class ReelTowerAdapterServer
    {
        #region Fields
        protected readonly ManualResetEvent shutdownEvent = new ManualResetEvent(false);

        protected readonly ManualResetEvent startedEvent = new ManualResetEvent(false);

        protected bool initialized = false;

        protected bool started = false;

        protected string serviceUri;

        protected CommunicationState state = CommunicationState.Closed;

        protected Thread serverThread = null;
        #endregion

        #region Properties
        public virtual bool Initialized => initialized;

        public virtual bool Running => started;

        public virtual ManualResetEvent ShutdownEvent => shutdownEvent;

        public virtual CommunicationState State => state;
        #endregion

        #region Constructors
        protected ReelTowerAdapterServer() { }
        #endregion

        #region Protected methods
        protected virtual void Run()
        {
            try
            {
                Uri uri_ = new Uri(serviceUri);

                using (ServiceHost serviceHost = new ServiceHost(typeof(ReelTowerAdapterService), uri_))
                {
                    try
                    {
                        serviceHost.AddServiceEndpoint(typeof(IWSAdapter), new BasicHttpBinding(), "");
                        ServiceMetadataBehavior mexBehavior = new ServiceMetadataBehavior();
                        mexBehavior.HttpGetEnabled = true;
                        serviceHost.Description.Behaviors.Add(mexBehavior);
                        serviceHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
                        serviceHost.Open();
                        state = serviceHost.State;
                        started = true;
                        startedEvent.Set();
                        shutdownEvent.WaitOne();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                        state = serviceHost.State;
                        serviceHost.Abort();
                        state = serviceHost.State;
                    }
                    finally
                    {
                        serviceHost.Close();
                        state = serviceHost.State;
                        startedEvent.Reset();
                        started = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
            finally
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Terminated");
            }
        }
        #endregion

        #region Public methods
        public virtual bool Create(string uri = "http://localhost:8686/RtTower.AdapterService/")
        {
            initialized = false;
            this.serviceUri = uri;
            return initialized = true;
        }

        public virtual void Destroy()
        {
            Stop();
            initialized = false;
        }

        public virtual CommunicationState Start(int timeout = 1000)
        {
            if (shutdownEvent != null)
            {
                shutdownEvent.Reset();
                serverThread = new Thread(new ThreadStart(Run));
                serverThread.Start();
                WaitForStart(timeout);
            }

            return state;
        }

        public virtual CommunicationState Stop(int timeout = 1000)
        {
            if (shutdownEvent != null)
                shutdownEvent.Set();

            if (serverThread != null)
            {
                serverThread.Join(timeout);
                serverThread = null;
            }

            return state;
        }

        public virtual CommunicationState WaitForStart(int timeout = 1000)
        {
            startedEvent.WaitOne(timeout);
            return state;
        }
        #endregion
    }
}
#endregion