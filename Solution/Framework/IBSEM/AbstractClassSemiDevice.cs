using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Marcus.Solution.TechFloor.Object;

namespace Marcus.Solution.TechFloor.IBSEM
{
    public class AbstractClassSemiDevice : AbstractClassDisposable
    {
        #region Fields
        protected bool enabled = false;
        protected double angle = 0;
        protected string name = string.Empty;
        protected string transportSystemControllerName = string.Empty;
        protected PointF location = new PointF(0, 0);
        protected SizeF size = new SizeF(0, 0);
        protected SemiDeviceTypes type = SemiDeviceTypes.Equipment;
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

        public string Name
        {
            get => name;
            set
            {   // Have to update vehicle information and restart.
                if (name != value)
                    name = value;
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

        public double Angle
        {
            get => angle;
            set
            {
                if (angle != value)
                    angle = value;
            }
        }

        public PointF Location
        {
            get => location;
            set
            {
                if (location != value)
                    location = value;
            }
        }

        public SizeF Size
        {
            get => size;
            set
            {
                if (size != value)
                    size = value;
            }
        }

        public SemiDeviceTypes Type
        {
            get => type;
            set
            {
                if (type != value)
                    type = value;
            }
        }
        #endregion
    }
}
