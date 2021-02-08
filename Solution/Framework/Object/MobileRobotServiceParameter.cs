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
    public class MobileRobotServiceParameter
    {
        #region Fields
        protected string timeStamp = string.Empty;
        protected MobileRobotEventTypes eventType = MobileRobotEventTypes.Added;
        [DataMember(Name = "voyage_cost")]
        public double Cost = 0.0;
        //[DataMember(Name = "voyage_etd")]
        //public string Etd = string.Empty;
        //[DataMember(Name = "voyage_eta")]
        //public string Eta = string.Empty;
        [DataMember(Name = "bound_list")]
        public List<MobileRobotBoundObject> Bounds = new List<MobileRobotBoundObject>();
        protected MobileRobotServiceStates state = MobileRobotServiceStates.Waiting;
        protected MobileRobotServiceRequestHeader requestHeader = null;
        #endregion

        #region Properties
        public string TimeStamp => timeStamp;
        public MobileRobotBoundObject GetCurrentBound() => Bounds.Find(x => x.State != MobileRobotBoundStates.Completed);
        public bool IsCompletedAllBounds => (Bounds.Find(x => x.State != MobileRobotBoundStates.Completed) == null) ? true : false;
        public MobileRobotServiceStates State
        {
            get => state;
            set
            {
                if (state != value)
                    state = value;
            }
        }
        //public DateTime EtdDateTime
        //{
        //    get => string.IsNullOrEmpty(Etd) ? DateTime.MaxValue : DateTime.Parse(Etd);
        //    set => Etd = value.ToString("yyyy-MM-dd HH:mm:ss.fff");
        //}
        //public DateTime EtaDateTime
        //{
        //    get => string.IsNullOrEmpty(Eta) ? DateTime.MaxValue : DateTime.Parse(Eta);
        //    set => Eta = value.ToString("yyyy-MM-dd HH:mm:ss.fff");
        //}
        public string DestinationGoalName // Destination goal name
        {
            get
            {
                MobileRobotBoundObject bound = GetCurrentBound();

                if (bound == null)
                {
                    if (Bounds.Count > 0)
                        return $"Goal_{Bounds.Last().FinishPosition}";
                    else
                        return string.Empty;
                }
                else
                    return $"Goal_{bound.FinishPosition}";
            }
        }
        // public int UpdateVoyageVersion(int voyage_version) => ((requestHeader != null) ? (requestHeader.VoyageVersion = voyage_version) : 0);
        // public int VoyageVersion => (requestHeader != null) ? requestHeader.VoyageVersion : 0;
        public string VoyageId => (requestHeader != null) ? requestHeader.VoyageId : string.Empty;
        public string RobotId => (requestHeader != null) ? requestHeader.RobotId : string.Empty;
        #endregion

        #region Public methods
        public virtual void UpdateTimeStamp(MobileRobotEventTypes eventtype)
        {
            eventType = eventtype;
            timeStamp = DateTime.Now.ToString("yyyymmdd-HHMMss");
        }

        /// <summary>
        /// Clear all contents to reset service parameters.
        /// </summary>
        public virtual void Clear()
        {
            // requestHeader.Clear();
            Cost = 0.0;

            if (Bounds != null)
                Bounds.Clear();

            state = MobileRobotServiceStates.Waiting;
        }

        public virtual void SetContentsIdentifier(int voyage_version, string voyage_id, string robot_id)
        {
            // if (requestHeader == null)
            //     requestHeader = new MobileRobotServiceRequestHeader(voyage_version, voyage_id, robot_id);
        }
        #endregion
    }
}
#endregion