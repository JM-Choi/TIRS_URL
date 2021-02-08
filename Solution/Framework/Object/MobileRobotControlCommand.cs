#region Imports
using System.Collections.Generic;
#endregion

#region Program
namespace TechFloor.Object
{
    public class MobileRobotControlCommand
    {
        #region Fields
        public readonly string Name;

        public readonly List<string> Arguments;
        #endregion

        #region Constructors
        public MobileRobotControlCommand(string command, List<string> args = null)
        {
            Name        = command;
            Arguments   = args;
        }

        public MobileRobotControlCommand(MobileRobotControlCommand src)
        {
            Name        = src.Name;
            Arguments   = src.Arguments;
        }
        #endregion
    }
}
#endregion