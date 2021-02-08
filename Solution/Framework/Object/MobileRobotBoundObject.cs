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
    public class MobileRobotBoundObject
    {
        #region Fields
        [DataMember(Name = "bound")]
        public MobileRobotBoundTypes BoundType = MobileRobotBoundTypes.Pickup;

        [DataMember(Name = "bound_num")]
        public int Key = 0;

        [DataMember(Name = "bound_cost")]
        public double Cost = 0;

        [DataMember(Name = "departure_position_id")]
        public int StartPosition = 0;

        [DataMember(Name = "destination_position_id")] // 필요성있으며, control plan 에 반드시 최종 목적지여야 한다.
        public int FinishPosition = 0;

        [DataMember(Name = "bound_etd")]
        public string Etd = string.Empty;

        [DataMember(Name = "bound_eta")]
        public string Eta = string.Empty;

        [DataMember(Name = "control_plan")]
        public List<MobileRobotControlItemObject> ControlPlan = new List<MobileRobotControlItemObject>();

        protected MobileRobotBoundStates state = MobileRobotBoundStates.Waiting;
        protected MobileRobotGoal goal = new MobileRobotGoal();

        private static object syncRoot = new object();
        private static MobileRobotBoundObject instance = null;
        #endregion

        #region Properties
        // public MobileRobotControlItemObject GetCurrentControlItem() => ControlPlan.Find(x => x.State != MobileRobotControlItemsStates.Completed);

        // public bool IsCompletedAllControlPlan => (ControlPlan.Find(x => x.State != MobileRobotControlItemsStates.Completed) == null) ? true : false;

        public int key { get; set; } = 0;
        public double cost { get; set; }
        public int startPosition { get; set; }
        public int finishPosition { get; set; }
        public MobileRobotBoundStates State
        {
            get => state;
            set
            {
                if (state != value) state = value;
            }
        }

        public static MobileRobotBoundObject Inst
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MobileRobotBoundObject();
                    }
                }
                return instance;
            }
        }


        public DateTime BoundEtdDateTime
        {
            get
            {
                return string.IsNullOrEmpty(Etd) ? DateTime.MaxValue : DateTime.Parse(Etd);
            }
            set
            {
                Etd = value.ToString("yyyy-mm-dd HH:MM:ss.fff");
            }
        }

        public DateTime BoundEtaDateTime
        {
            get
            {
                return string.IsNullOrEmpty(Eta) ? DateTime.MaxValue : DateTime.Parse(Eta);
            }
            set
            {
                Eta = value.ToString("yyyy-mm-dd HH:MM:ss.fff");
            }
        }

        public string GoalName => $"Goal_{FinishPosition}";

        public MobileRobotGoal Goal
        {
            get
            {
                goal.SetData(GoalName);
                return goal;
            }
            set
            {
                goal.SetData(value);
            }
        }
        #endregion

        #region Public methods
        public virtual void Clear()
        {
            if (ControlPlan != null)
                ControlPlan.Clear();
        }

        public virtual bool IsCompletedControlItem(int jobnumber)
        {
            MobileRobotControlItemObject item = ControlPlan.Find(x => x.ControlNumber == jobnumber);

            if (item == null)
                return false;

            return (item.State == MobileRobotControlItemStates.Completed);
        }
        #endregion
    }
}
#endregion