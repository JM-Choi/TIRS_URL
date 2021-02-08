#region Imports
using TechFloor.Gui;
using System;
using System.Threading;
#endregion

#region Program
namespace TechFloor
{
    public interface IMainFormExt : IFormMain
    {
        #region Properties
        DigitalIoManager DigitalIo { get; }
        #endregion

        #region Public methods
        #endregion
    }

    public class ReelTowerStateEventArgs : EventArgs
    {
        #region Fields
        public readonly int Id;

        public readonly ReelTowerStates State;
        #endregion

        #region Constructors
        public ReelTowerStateEventArgs(int id, ReelTowerStates state)
        {
            Id = id;
            State = state;
        }
        #endregion
    }
}
#endregion