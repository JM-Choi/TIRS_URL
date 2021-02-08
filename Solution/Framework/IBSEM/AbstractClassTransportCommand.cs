using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marcus.Solution.TechFloor.Object;
using Marcus.Solution.TechFloor.Gem;

namespace Marcus.Solution.TechFloor.IBSEM
{
    public class AbstractClassTransportCommand : AbstractClassDisposable
    {
        #region Fields
        protected bool suspended = true;
        protected int priority = 0;
        protected string transportCommandId = string.Empty;
        protected string carrierId = string.Empty;
        protected string compositeCommandId = string.Empty;
        protected string sourceDevice = string.Empty;
        protected string sourceLocation = string.Empty;
        protected string destinationDevice = string.Empty;
        protected string destinationLocation = string.Empty;
        protected string transportSystemControllerName = string.Empty;
        protected string vehicleName = string.Empty;
        protected DateTime stateChangedTime = DateTime.MinValue;
        protected AlternateStates alternateState = AlternateStates.NotAlternate;
        protected TransferCommands transferCommand = TransferCommands.Transfer;
        protected TransferCommandStates transferCommandState = TransferCommandStates.None;
        #endregion

        #region Properties
        public bool Suspended
        {
            get => suspended;
            set
            {
                if (suspended != value)
                    suspended = value;
            }
        }

        public int Priority
        {
            get => priority;
            set
            {
                if (priority != value)
                    priority = value;
            }
        }

        public string TransportCommandId
        {
            get => transportCommandId;
            set
            {
                if (transportCommandId != value)
                    transportCommandId = value;
            }
        }

        public string CarrierId
        {
            get => carrierId;
            set
            {
                if (carrierId != value)
                    carrierId = value;
            }
        }

        public string CompositeCommandId
        {
            get => compositeCommandId;
            set
            {
                if (compositeCommandId != value)
                    compositeCommandId = value;
            }
        }

        public string SourceDevice
        {
            get => sourceDevice;
            set
            {
                if (sourceDevice != value)
                    sourceDevice = value;
            }
        }

        public string SourceLocation
        {
            get => sourceLocation;
            set
            {
                if (sourceLocation != value)
                    sourceLocation = value;
            }
        }

        public string DestinationDevice
        {
            get => destinationDevice;
            set
            {
                if (destinationDevice != value)
                    destinationDevice = value;
            }
        }

        public string DestinationLocation
        {
            get => destinationLocation;
            set
            {
                if (destinationLocation != value)
                    destinationLocation = value;
            }
        }

        public string TransportSystemControllerName
        {
            get => transportSystemControllerName;
            set
            {
                if (transportSystemControllerName != value)
                    transportSystemControllerName = value;
            }
        }

        public string VehicleName
        {
            get => vehicleName;
            set
            {
                if (vehicleName != value)
                    vehicleName = value;
            }
        }

        public DateTime StateChangedTime => stateChangedTime;

        public AlternateStates AlternateState
        {
            get => alternateState;
            set
            {
                if (alternateState != value)
                    alternateState = value;
            }
        }

        public TransferCommands TransferCommand
        {
            get => transferCommand;
            set
            {
                if (transferCommand != value)
                    transferCommand = value;
            }
        }

        public TransferCommandStates TransferComtrandState
        {
            get => transferCommandState;
            set
            {
                if (transferCommandState != value)
                    transferCommandState = value;
            }
        }
        #endregion
    }
}
