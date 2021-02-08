#region Imports
using System.Windows.Forms;
#endregion

#region Program
namespace TechFloor.Object
{
    public interface IFormMessage
    {
        #region Properties
        #endregion

        #region Public methods
        DialogResult ShowDialog(string message, string caption, Buttons buttons, Icons icon, bool autoclose, int autoclosedelay, bool buzzer);
        void Show(string message, string caption, Buttons buttons, Icons icon, bool autoclose, int autoclosedelay, bool buzzer);
        #endregion
    }
}
#endregion