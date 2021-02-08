#region Imports
using TechFloor.Object;
using System.Threading;
#endregion

#region Program
namespace TechFloor.Gui
{
    public interface IFormMain
    {
        #region Properties
        OperationStates OperationState { get; }
        AlarmStates AlarmState { get; }
        IMainSequence MainSequence { get; }
        IDigitalIoManager DigitalIoManager { get; }
        WaitHandle ShutdownEvent { get; }
        #endregion

        #region Public methods
        void SetFocus(GuiPages page = GuiPages.MainPage);
        #endregion
    }
}
#endregion