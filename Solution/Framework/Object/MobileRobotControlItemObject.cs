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
    public class MobileRobotControlItemObject
    {
        #region Fields
        [DataMember(Name = "control_num")]
        public int ControlNumber = 0;

        [DataMember(Name = "control_type")]
        public int ControlType = 0;

        [DataMember(Name = "command")]
        public string Command = string.Empty;

        [DataMember(Name = "control_type_name")]
        public string ControlName = string.Empty;

        [DataMember(Name = "control_target")]
        public int ControlTarget = 0;

        [DataMember(Name = "position_id")]
        public string PositionId = string.Empty;

        [DataMember(Name = "destination_position")]
        public string DestinationPositionId = string.Empty;

        [DataMember(Name = "message")]
        public string Message = string.Empty;

        [DataMember(Name = "floor")]
        public int Floor = 0;

        [DataMember(Name = "destination_floor")]
        public int DestinationFloor = 0;

        [DataMember(Name = "x")]
        public double CoordX = 0.0;

        [DataMember(Name = "y")]
        public double CoordY = 0.0;

        [DataMember(Name = "heading")]
        public string Heading = string.Empty;

        [DataMember(Name = "control_cost")]
        public double ControlCost = 0;

        [DataMember(Name = "position_type")]
        public double PositionType = 0;

        [DataMember(Name = "control_accumulated_cost")]
        public double AccumulatedCost = 0;

        [DataMember(Name = "control_etd")]
        public string ControlEtd = string.Empty;

        [DataMember(Name = "control_eta")]
        public string ControlEta = string.Empty;

        [DataMember(Name = "control_status")]
        protected MobileRobotControlItemStates state = MobileRobotControlItemStates.Waiting;

        [DataMember(Name = "control_status_date")]
        protected string stateChangeDateTime = string.Empty;

        [DataMember(Name = "remark")]
        protected string remark = string.Empty;

        protected int startedTick = 0;

        protected DateTime completedTime = DateTime.MinValue;
        #endregion

        #region Properties
        public MobileRobotControlItemStates State
        {
            get => state;
            set
            {
                if (state != value)
                {
                    switch (state = value)
                    {
                        case MobileRobotControlItemStates.Started:
                            startedTick = Environment.TickCount;
                            break;
                        case MobileRobotControlItemStates.Completed:
                            break;
                    }

                    stateChangeDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
            }
        }

        public string StateChangeDateTime => stateChangeDateTime;

        public string Remark
        {
            get => remark;
            set => remark = value;
        }

        public DateTime SectionEtdDateTime
        {
            get
            {
                return string.IsNullOrEmpty(ControlEtd) ? DateTime.MaxValue : DateTime.Parse(ControlEtd);
            }
            set
            {
                ControlEtd = value.ToString("yyyy-mm-dd HH:MM:ss.fff");
            }
        }

        public DateTime SectionEtaDateTime
        {
            get
            {
                return string.IsNullOrEmpty(ControlEta) ? DateTime.MaxValue : DateTime.Parse(ControlEta);
            }
            set
            {
                ControlEta = value.ToString("yyyy-mm-dd HH:MM:ss.fff");
            }
        }

        //public MobileRobotGoal Goal
        //{
        //    get
        //    {
        //        goal.SetData(PositionName, Floor, CoordX, CoordY, Heading);
        //        return goal;
        //    }
        //    set
        //    {
        //        goal.SetData(value);
        //        goal.CopyData(ref PositionName, ref Floor, ref CoordX, ref CoordY, ref Heading);
        //    }
        //}
        public int controlNumber => ControlNumber;
        public int controlType => ControlType;
        public string command => Command;
        public string controlName => ControlName;
        public int controlTarget => ControlTarget;
        public string positionId => PositionId;
        public string destinationPositionId => DestinationPositionId;
        public string message => Message;
        public int floor => Floor;
        public double coordX => CoordX;
        public double coordY => CoordY;
        public string heading => Heading;
        public double controlCost => ControlCost;
        public double positionType => PositionType;
        public double accumulatedCost => AccumulatedCost;
        public string controlEtd => ControlEtd;
        public string controlEta => ControlEta;
        public int StartedTick => startedTick;
        public DateTime CompletedTime => completedTime;
        #endregion

        #region Constructors
        public MobileRobotControlItemObject(MobileRobotControlItemObject src)
        {
            ControlNumber = src.ControlNumber;
            ControlType = src.ControlType;
            Command = src.Command;
            ControlName = src.ControlName;
            ControlTarget = src.ControlTarget;
            PositionId = src.PositionId;
            DestinationPositionId = src.DestinationPositionId;
            Message = string.Empty;
            Floor = src.Floor;
            DestinationFloor = src.DestinationFloor;
            CoordX = src.CoordX;
            CoordY = src.CoordY;
            Heading = src.Heading;
            ControlCost = src.ControlCost;
            PositionType = src.PositionType;
            AccumulatedCost = src.AccumulatedCost;
            ControlEtd = src.ControlEtd;
            ControlEta = src.ControlEta;
            state = src.state;
            stateChangeDateTime = src.stateChangeDateTime;
            remark = src.remark;
            startedTick = src.startedTick;
        }
        #endregion
    }
}
#endregion