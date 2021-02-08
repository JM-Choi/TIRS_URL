#region Imports
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
#endregion

#region Program
namespace TechFloor.Gui
{
    public enum MenuCategories : int
    {
        Operation,
        Maintenance,
        Recipe,
        Config,
        Log,
        Trend
    }

    public partial class FormExt : Form
    {
        #region Fields
        protected bool dragging = false;
        protected Point startPoint;
        #endregion

        #region Properties
        #endregion

        #region Protected methods
        protected virtual void FireChangedThemeColor()
        {
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            dragging = true;
            startPoint = new Point(e.X, e.Y);
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - startPoint.X, p.Y - startPoint.Y);
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            dragging = false;
            base.OnMouseUp(e);
        }
        #endregion

        #region Static methods
        public static void SetDoubleBuffered(Control control, bool state)
        {   // set instance non-public property with name "DoubleBuffered" to true
            typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, control, new object[] { state });
        }
        #endregion
    }
}
#endregion