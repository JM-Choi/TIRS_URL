#region Imports
using System;
using System.Drawing;
#endregion

#region Program
namespace TechFloor.Object
{
    public class MobileRobotCoord
    {
        #region Fields
        public double Orientation   = 0; // Angle

        public string Name          = string.Empty;

        public PointF Location      = new PointF(0.0f, 0.0f); // X, Y
        #endregion

        #region Constructors
        public MobileRobotCoord(double x = 0, double y = 0, double t = 0)
        {
            Location.X  = Convert.ToSingle(x);
            Location.Y  = Convert.ToSingle(y);
            Orientation = t;
        }

        public MobileRobotCoord(Point pt, double t = 0)
        {
            Location    = new PointF(pt.X, pt.Y);
            Orientation = t;
        }

        public MobileRobotCoord(MobileRobotCoord src)
        {
            Location    = new PointF(src.Location.X, src.Location.Y);
            Orientation = src.Orientation;
        }
        #endregion

        #region Public methods
        public void GetValues(ref double x, ref double y, ref double heading)
        {
            x       = Location.X;
            y       = Location.Y;
            heading = Orientation;
        }
        #endregion
    }

    public class MobileRobotGoal
    {
        #region Fields
        public string Name              = string.Empty;

        public int Floor                = 0;

        public MobileRobotCoord Coord   = new MobileRobotCoord();
        #endregion

        #region Constructors
        public MobileRobotGoal(string name= null, int floor = 0, double x = 0, double y = 0, double t = 0)
        {
            SetData(name, floor, x, y, t);
        }
        #endregion

        #region Public methods
        public void SetData(string name= null, int floor = 0, double x = 0, double y = 0, double t = 0)
        {
            this.Name   = name;
            this.Coord  = new MobileRobotCoord(x, y, t);
            this.Floor  = floor;
        }

        public void SetData(MobileRobotGoal src)
        {
            this.Name   = src.Name;
            this.Coord  = new MobileRobotCoord(src.Coord);
            this.Floor  = src.Floor;
        }
        #endregion
    }
}
#endregion