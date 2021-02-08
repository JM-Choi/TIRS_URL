#region Imports
using System;
#endregion

#region Program
namespace TechFloor.Components
{
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