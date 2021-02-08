#region Imports
using System;
using System.ComponentModel;
using System.Diagnostics;
#endregion

#region Program
namespace TechFloor.Object
{
    public class AbstractClassEventArgs : EventArgs, IDisposable
    {
        #region Fields
        protected bool disposedValue = false;
        #endregion

        #region Properties
        [Browsable(false)]
        public virtual string ClassName
        {
            get { return GetType().Name; }
        }
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

        ~AbstractClassEventArgs()
        {
            Debug.Assert(disposedValue, string.Format("The {0} EventArgs was not disposed properly.", ClassName));
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