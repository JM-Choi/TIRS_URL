#region Imports
using System;
#endregion

#region Program
namespace TechFloor.Object
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable", Justification = "<보류 중>")]
    public class PauseException<T> : Exception
    {
        #region Properties
        public T Code { get; protected set; }

        public string Module { get; protected set; }

        public string Step { get; protected set; }
        #endregion

        #region Constructors
        public PauseException() { }

        public PauseException(string message) : base(message) { }

        public PauseException(string message, Exception innerException) : base(message, innerException) { }

        public PauseException(T code, string message) : base(message)
        {
            Code = code;
        }

        public PauseException(T code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }

        public PauseException(T code, string module= null, string step= null, string message= null) : base(message)
        {
            Code = code;
            Module = module;
            Step = step;
        }

        public PauseException(T code, Exception innerException, string module= null, string step= null, string message= null) : base(message, innerException)
        {
            Code = code;
            Module = module;
            Step = step;
        }
        #endregion
    }
}
#endregion