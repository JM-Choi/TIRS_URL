using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marcus.Solution.TechFloor.Object;

namespace Marcus.Solution.TechFloor.IBSEM
{
    [Serializable]
    public class AbstractClassCarrier : AbstractClassDisposable
    {
        #region Fields
        protected bool empty = false;
        protected bool transferOrdered = false;
        protected int priority = 0;
        protected int timeout = 0;
        protected string carrierId = string.Empty;
        protected string carrierLocation = string.Empty;
        protected string usage = string.Empty;
        protected string attribute = string.Empty;
        protected string content = string.Empty;
        protected string compositeCommandId = string.Empty;
        protected string compositeId = string.Empty;
        protected string deviceName = string.Empty;
        protected DateTime transferOrderTime = DateTime.MinValue;
        protected TransferOrderByWho transferOrderBy = TransferOrderByWho.Host;
        protected AlternateStates alternateState = AlternateStates.NotAlternate;
        protected CarrierStates state = CarrierStates.None;
        #endregion

        #region Properties
        public bool Empty
        {
            get => empty;
            set
            {
                if (empty != value)
                    empty = value;
            }
        }

        public bool TransferOrdered
        {
            get => transferOrdered;
            set
            {
                if (transferOrdered != value)
                    transferOrdered = value;
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

        public int Timeout
        {
            get => timeout;
            set
            {
                if (timeout != value)
                    timeout = value;
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

        public string CarrierLocation
        {
            get => carrierLocation;
            set
            {
                if (carrierLocation != value)
                    carrierLocation = value;
            }
        }

        public string Usage
        {
            get => usage;
            set
            {
                if (usage != value)
                    usage = value;
            }
        }

        public string Attribute
        {
            get => attribute;
            set
            {
                if (attribute != value)
                    attribute = value;
            }
        }

        public string Content
        {
            get => content;
            set
            {
                if (content != value)
                    content = value;
            }
        }

        public DateTime TranderOrderTime
        {
            get => transferOrderTime;
            set
            {
                if (transferOrderTime != value)
                    transferOrderTime = value;
            }
        }

        public TransferOrderByWho TransferOrderBy
        {
            get => transferOrderBy;
            set
            {
                if (transferOrderBy != value)
                    transferOrderBy = value;
            }
        }

        public AlternateStates AlternateState
        {
            get => alternateState;
            set
            {
                if (alternateState != value)
                    alternateState = value;
            }
        }

        public CarrierStates State
        {
            get => state;
            set
            {
                if (state != value)
                    state = value;
            }
        }
        #endregion
    }
}
