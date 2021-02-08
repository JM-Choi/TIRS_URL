using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marcus.Solution.TechFloor.Object;
using Marcus.Solution.TechFloor.Gem;

namespace Marcus.Solution.TechFloor.IBSEM
{
    public class AbstractClassVehicle : AbstractClassDisposable
    {
        #region Fields
        protected bool enabled = false;
        protected int loadingSucessCount = 0;
        protected int loadingFailureCount = 0;
        protected int unloadinSuccessCount = 0;
        protected int unloadinFailureCount = 0;
        protected string name = string.Empty;
        protected string controller = string.Empty;
        protected DateTime stateChangedTime = DateTime.MinValue;
        protected DateTime alarmSetTime = DateTime.MinValue;
        protected TransportUnitTypes type = TransportUnitTypes.MobileRobot;
        protected AlarmPendingStates alarmPendingState = AlarmPendingStates.NoAlarms;
        protected VehicleStates state = VehicleStates.NotAssigned;
        protected List<int> pendingAlarms = new List<int>();
        protected List<AbstractClassCarrier> carriers = new List<AbstractClassCarrier>();
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

        public int LoadingSuccessCount => loadingSucessCount;

        public int UnloadingSuccessCount => unloadinSuccessCount;

        public int LoadingFailureCount => loadingFailureCount;

        public int UnloadingFailureCount => unloadinFailureCount;

        public string Name
        {
            get => name;
            set
            {   // Have to update vehicle information and restart.
                if (name != value)
                    name = value;
            }
        }

        public DateTime StateChangedTime => stateChangedTime;

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

        public VehicleStates State
        {
            get => state;
            set
            {   // Requires update of contents
                if (state != value)
                    state = value;
            }
        }

        public IReadOnlyList<int> PendingAlarms => pendingAlarms;

        public IReadOnlyList<AbstractClassCarrier> Carriers => carriers;
        #endregion
    }
}
