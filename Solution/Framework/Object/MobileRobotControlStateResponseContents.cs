#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Program
namespace TechFloor.Object
{
    [DataContract]
    public class MobileRobotControlStateResponseContents
    {
        #region Fields
        [DataMember(Name = "voyage_version")]
        public readonly string VoyageVersion;

        [DataMember(Name = "voyage_id")]
        public readonly string VoyageId = string.Empty;

        [DataMember(Name = "robot_id")]
        public readonly string RobotId = string.Empty;

        [DataMember(Name = "bound_num")]
        public readonly int BoundNumber = 0;

        [DataMember(Name = "control")]
        public MobileRobotControlItemObject ControlItem = null;
        #endregion

        #region Constructors
        public MobileRobotControlStateResponseContents(string voyage_version, string voyage_id, string robot_id, int bound_num, MobileRobotControlItemObject control)
        {
            VoyageVersion = voyage_version;
            VoyageId = voyage_id;
            RobotId = robot_id;
            BoundNumber = bound_num;
            ControlItem = new MobileRobotControlItemObject(control);
        }

        public MobileRobotControlStateResponseContents(MobileRobotControlStateResponseContents src)
        {
            VoyageVersion = src.VoyageVersion;
            VoyageId = src.VoyageId;
            RobotId = src.RobotId;
            BoundNumber = src.BoundNumber;
            ControlItem = new MobileRobotControlItemObject(src.ControlItem);
        }
        #endregion
    }
}
#endregion