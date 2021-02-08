#region Imports
using System;
#endregion

#region Program
namespace TechFloor.Object
{
    public class CycleStopException : Exception
    {
        #region Properties
        public string Module { get; protected set; }

        public string Step { get; protected set; }
        #endregion

        #region Constructors
        public CycleStopException() { }

        public CycleStopException(string message) : base(message) { }

        public CycleStopException(string message, Exception innerException) : base(message, innerException) { }

        public CycleStopException(string module, string step, string message = null) : base(message)
        {
            Module = module;
            Step = step;
        }

        public CycleStopException(string module, string step, string message, Exception innerException) : base(message, innerException)
        {
            Module = module;
            Step = step;
        }
        #endregion
    }
}
#endregion