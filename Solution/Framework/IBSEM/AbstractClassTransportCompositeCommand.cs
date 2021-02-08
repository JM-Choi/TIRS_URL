using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marcus.Solution.TechFloor.Object;

namespace Marcus.Solution.TechFloor.IBSEM
{
    public class AbstractClassTransportCompositeCommand : AbstractClassDisposable
    {
        #region Fields
        protected string compositeCommandId = string.Empty;
        protected string compositeId = string.Empty;
        #endregion

        #region Properties
        public string CompositeCommandId
        {
            get => compositeCommandId;
            set
            {
                if (compositeCommandId != value)
                    compositeCommandId = value;
            }
        }

        public string CompositeId
        {
            get => compositeId;
            set
            {
                if (compositeId != value)
                    compositeId = value;
            }
        }
        #endregion
    }
}
