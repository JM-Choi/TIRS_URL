#region Imports
using System;
#endregion

#region Program
namespace TechFloor.Object
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable", Justification = "<보류 중>")]
    public class DriverException : Exception
    {
        #region Properties
        public string Driver { get; protected set; }
        public string Description { get; protected set; }
        #endregion

        #region Public methods
        public DriverException() { }
        public DriverException(string message) : base(message) { }
        public DriverException(string message, Exception innerException) : base(message, innerException) { }
        public DriverException(string message, string driver, string description= null) : base(message) { Driver = driver; Description = description; }
        public DriverException(string message, Exception innerException, string driver, string description) : base(message, innerException) { Driver = driver; Description = description; }
        #endregion
    }
}
#endregion