#region Imports
#endregion

#region Program
namespace TechFloor.Object
{
    public interface IMainSequence
    {
        #region Properties
        bool CycleStop { get; }
        bool Initialized { get; }
        bool AutoTaught { get; }
        bool Calibrated { get; }
        int InitializeStep { get; }
        int AutoTechStep { get; }
        int CalibrationStep { get; }
        OperationStates OperationState { get; }
        #endregion

        #region Public methods
        bool Reset();
        bool Start();
        bool Stop();
        void TryCycleStop(bool state = true);
        void Init(InitializeMode mode);
        void AutoTeach(AutomaticTeachMode mode);
        void CalDevices(CalibrationMode mode);
        void StopInit();
        void StopAutoTech();
        void StopCalDevices();
        #endregion
    }
}
#endregion