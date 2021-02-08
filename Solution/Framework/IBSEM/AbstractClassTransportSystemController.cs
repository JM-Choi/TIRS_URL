using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marcus.Solution.TechFloor.Gem;
using Marcus.Solution.TechFloor.Object;

namespace Marcus.Solution.TechFloor.IBSEM
{
    public class AbstractClassTransportSystemController : AbstractClassDisposable
    {
        #region Fields
        protected bool enabled = false;
        protected int workQueues = 0;
        protected double utilizationRate = 0;
        protected string name = string.Empty;
        protected DateTime controlStateChangedTime = DateTime.MinValue;
        protected ControlStates controlState = ControlStates.None; // HSMS driver state
        protected AlarmPendingStates alarmPendingState = AlarmPendingStates.NoAlarms;
        protected TransportUnitTypes type = TransportUnitTypes.MobileRobot;
        protected HsmsHostTypes hostType = HsmsHostTypes.HSMS;
        protected TransportSystemControllerStates state = TransportSystemControllerStates.None;
        protected List<AbstractClassVehicle> vehicles = new List<AbstractClassVehicle>();
        #endregion

        #region Properties
        public bool Enabled
        {
            get => enabled;
            set
            {   // Have to update / or check vehicle usage state.
                if (enabled != value)
                    enabled = value;
            }
        }

        public int WorkQueues => workQueues;

        public double UtilizationRate => utilizationRate;

        public string Name
        {
            get => name;
            set
            {   // Have to update of TSC name in vehicle information.
                if (name != value)
                    name = value;
            }
        }

        public DateTime ControlStateChangedTime => controlStateChangedTime;

        public ControlStates ControlState => controlState;

        public AlarmPendingStates AlarmPendingState => alarmPendingState;

        public TransportUnitTypes Type
        {
            get => type;
            set
            {   // Have to update of vehicle information list.
                if (type != value)
                    type = value;
            }
        }

        public HsmsHostTypes HostType
        {
            get => hostType;
            set
            {   // Requires host interface update and restart.
                if (hostType != value)
                    hostType = value;
            }
        }

        public TransportSystemControllerStates State
        {
            get => state;
            set
            {   // Requires update of contents
                if (state != value)
                    state = value;
            }
        }

        public IReadOnlyList<AbstractClassVehicle> Vehicles => vehicles;
        #endregion
    }
}
