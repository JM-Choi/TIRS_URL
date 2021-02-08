#region Imports
using System;
using System.Diagnostics;
#endregion

#region Program
namespace TechFloor.Object
{
    #region Enumerations
    public enum SeverityLevels
    {
       Low,
       Moderate,
       Major,
       Critical
    }

    public enum EventTypes
    {
        Alarm,
        Warning,
        Information,
        Notification,
        Log
    }

    public enum NetworkDeviceEventIds
    {
        Undefined,
        Connected,
        Disconnected,
        DisconnectedByServer,
        SucceedToSend,
        SucceedToReceive,
        FailedToConnect,
        FailedToSend,
        FailedToReceive,
        HartbeatResponseTimeout,
        CommandResponseTimeout,
        ReportAcknowledgeResponseTimeout,
        ExceedCommandResponseRetryLimit,
        ExceedCommandResponseRetryCycleLimit,
        ExceedReportAcknowledgeResponseRetryLimit,
        ExceedConnectionRetryLimit,
    }

    public enum MessageClasses
    {
        Undefined,
        HostLogin,
        HeartBeatRequest,
        HeartBeatResponse,
        RemoteCommand,
        RemoteCommandResponse,
        Ping,
        ReportState,
        ReportProductInformation,
        ReportProcessData,
    }
    #endregion

    public class EventArgsExt : EventArgs, IDisposable
    {
        #region Fields
        private bool disposedValue = false;
        #endregion

        #region Properties
        public string ClassName => GetType().ToString();
        #endregion

        #region IDisposable Support
        protected virtual void DisposeManagedObjects() { }

        protected virtual void DisposeUnmanagedObjects() { }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    DisposeManagedObjects();

                DisposeUnmanagedObjects();
                disposedValue = true;
            }
        }

        ~EventArgsExt()
        {
            Debug.Assert(disposedValue, string.Format($"The {ClassName} object was not disposed properly."));
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
#endregion