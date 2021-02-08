using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marcus.Solution.TechFloor.Object;

namespace Marcus.Solution.TechFloor.IBSEM
{
    public class AbstractClassPartition : AbstractClassDisposable
    {
        #region Fields
        protected bool enabled = false;
        protected string name = string.Empty;
        protected string owner = string.Empty;
        protected PartitionCharacteristics characteristic = PartitionCharacteristics.Alternate;
        protected List<string> carrierLocations = new List<string>();
        #endregion

        #region Properties
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled != value)
                    enabled = value;
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                    name = value;
            }
        }

        public string Owner
        {
            get => owner;
            set
            {
                if (owner != value)
                    owner = value;
            }
        }

        public PartitionCharacteristics Characteristic
        {
            get => characteristic;
            set
            {
                if (characteristic != value)
                    characteristic = value;
            }
        }

        public IReadOnlyList<string> CarrierLocations => carrierLocations;
        #endregion
    }
}
