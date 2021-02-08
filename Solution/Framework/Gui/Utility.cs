#region Imports
using System.Windows.Forms;
#endregion

#region Program
namespace TechFloor.Gui
{
    public class Utility
    {
        public static Control FindControl(Control parent, string name)
        {
            if (IsExistControl(parent, name))
                return parent.Controls[name];
            return null;
        }

        public static bool IsExistControl(Control parent, string name)
        {
            return parent.Controls.ContainsKey(name);
        }

        public static void SetDoubleBuffered(Control ctrl, bool enabled)
        {
            var prop_ = ctrl.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            prop_.SetValue(ctrl, enabled, null);
        }
    }
}
#endregion