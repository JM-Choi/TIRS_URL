#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Program
namespace Marcus.Solution.TechFloor.IBSEM
{
    public enum TransportUnitTypes : int
    {
        MobileRobot,
        AGV,
        OHT
    }

    public enum HsmsHostTypes : int
    {
        IBSEM,
        STKSEM,
        HSMS
    }

    public enum SemiDeviceTypes : int
    {
        Stocker,
        Equipment
    }

    public enum AlarmPendingStates : int
    {
        NoAlarms,
        Alarms
    }

    public enum StockerStates : int
    {
        None,
        Idle,
        Active
    }

    public enum PartitionCharacteristics : int
    {
        Default,
        Alternate,
        Passthrough
    }

    public enum IntraBayStates : int
    {
        None,
        NotAssigned,
        Enroute,
        Parked,
        Acquiring,
        Depositing,
        Removed
    }

    public enum TransportSystemControllerStates : int
    {
        None,
        Initialize,
        Pausing,
        Paused,
        Auto
    }

    public enum AccessMode : int
    {
        Manual,
        Auto
    }

    public enum AlternateStates : int
    {
        Alternate,
        NotAlternate
    }

    public enum CarrierStates : int
    {
        None,
        Installed, // IBSEM
        Removed,
        WaitIn,
        Transferring,
        WaitOut,
        InstallCompleted,
        RemoveCompleted,
        Resumed,
        Stored,
        StoredAlternate
    }

    public enum TransferOrderByWho : int
    {
        Host,
        Operator
    }

    public enum TransferCommandEventTypes : int
    {
        Completed,
        NotAcknowledge,
        CompoisteCommandCompleted,
        SingleCompleted,
        Aborted,
        Canceled,
        AbortFailed,
        CancelFailed
    }

    public enum ErrorEventTypes : int
    {
        StockerError,
        TransportSystemControllerError,
    }

    public enum ErrorEvetnStates : int
    {
        Clear,
        Set
    }
    
    public enum AlarmEventTypes : int
    {
        DebugError,
        PortError,
        PortEmpty,
        PortFull,
        ShelfEmpty,
        ShelfFull,
    }
}
#endregion