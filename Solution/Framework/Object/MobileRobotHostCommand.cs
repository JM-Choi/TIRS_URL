#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Program
namespace TechFloor.Object
{
    public class HostControlCommand
    {
        #region Fields
        public readonly MobileRobotCommandTypes CommandType;

        public readonly MobileRobotControlCommand Command;
        #endregion

        #region Constructors
        public HostControlCommand(MobileRobotCommandTypes type, string command, List<string> args = null)
        {
            CommandType     = type;
            Command         = new MobileRobotControlCommand(command, args);
        }

        public HostControlCommand(HostControlCommand src)
        {
            CommandType     = src.CommandType;
            Command         = new MobileRobotControlCommand(src.Command);
        }
        #endregion
    }
}
#endregion