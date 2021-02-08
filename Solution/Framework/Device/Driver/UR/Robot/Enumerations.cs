#region Imports
#endregion

#region Program
namespace Marcus.Solution.TechFloor.Device.Driver.UR.Robot
{
    #region Enumerations
    public enum ProgramStates
    {
        Stopped,
        Playing,
        Paused
    };

    public enum RobotModes
    {
        NoController = -1,
        Disconnected,
        ConfirmSafety,
        Booting,
        PowerOff,
        PowerOn,
        Idle,
        Backdrive,
        Running,
        UpdatingFirmware
    };

    public enum SafetyModes
    {
        Normal = 1,
        Reduced,
        ProtectiveStop,
        Recovery,
        SafeguardStop,
        SystemEmergencyStop,
        RobotEmergencyStop,
        Violation,
        Fault,
        ValidatedJointId,
        UndefinedSafetyMode
    };

    public enum JointModes
    {
        Reset = 235,
        ShuttingDown,
        PartDCalibrationMode,
        Backdrive,
        PowerOff,
        ReadyForPowerOff,
        NotResponding = 245,
        MotorInitialisation,
        Booting,
        PartDCalibrationErrorMode,
        Bootloader,
        CalibrationMode,
        Violation,
        Fault = 252,
        Running = 253,
        Idle = 255,
    }

    public enum ToolModes
    {
        Reset = 235,
        ShuttingDown,
        PowerOff = 239,
        NotResponding = 245,
        Booting = 247,
        Bootloader = 249,
        Fault = 252,
        Running = 253,
        Idle = 255,
    }

    public enum ReportLevels
    {
        Debug,
        Information,
        Warning,
        Violation,
        Fault,
        DevelopmentDebug = 128,
        DevelopmentInformation,
        DevelopmentWarning,
        DevelopmentViolation,
        DevelopmentFault
    }

    public enum RequestedTypes
    {
        Boolean,
        Integer,
        Float,
        String,
        Pose,
        JointVector,
        Waypoint,
        Expression,
        None
    }

    public enum ValueTypes
    {
        NoneValue, // No conntent (Has not been initialized yet)
        ConstStringValue = 3, // (int16_t=lenght, char[]=value)
        StringValue = ConstStringValue,
        VariableStringValue = 4, // (int16_t=length, char[]=value)
        ListValueOld = 4, // (int16_t=length, (uint8_t=type, data value)...)
        ListValue = 5,
        PoseValueOld = 9, // (float=x, float=y, float=z, float=rx, float=ry, float=rz)
        PoseValue = 10,
        BooleanValueOld = 11, // (bool=value)
        BooleanValue = 12,
        NumberValueOld = 12, // (float=value)
        NumberValue = 13,
        IntegerValueOld = 13, // (int32_t=value)
        IntegerValue = 14,
        FloatValueOld = 14, // (float=value)
        FloatValue = 15
    }

    public enum MessageSources
    {
        Gui = -5,
        SimulatedRobot = -4,
        RtMachine = -3,
        RobotInterface = -2,
        Undefined = -1,
        Base = 0,
        Shoulder,
        Elbow,
        Wrist1,
        Wrist2,
        Wrist3,
        Tool,
        Controller = 7,
        Rtde = 8,
        ProcessorUa = 20,
        ProcessorUb = 30,
        ScbFpga = 40,
        TeachPendant1 = 65,
        TeachPendant2 = 66,
        Euromap1 = 67,
        Euromap2 = 68,
        Joint0Fpga = 100,
        Joint1Fpga = 101,
        Joint2Fpga = 102,
        Joint3Fpga = 103,
        Joint4Fpga = 104,
        Joint5Fpga = 105,
        ToolFpga = 106,
        EuromapFpga = 107,
        TeachPendantA = 108,
        Joint0A = 110,
        Joint1A = 111,
        Joint2A = 112,
        Joint3A = 113,
        Joint4A = 114,
        Joint5A = 115,
        ToolA = 116,
        EuromapA = 117,
        TeachPendantB = 118,
        Joint0B = 120,
        Joint1B = 121,
        Joint2B = 122,
        Joint3B = 123,
        Joint4B = 124,
        Joint5B = 125,
        ToolB = 126,
        EuromapB = 127,
    }

    public enum MessageTypes
    {
        Disconnect = -1,
        ModbusInfomationMessage = 5,
        RobotStateMessage = 16,
        RobotMessage = 20,
        HmcMessage = 22,
        SafetySetupBroadcastMessage = 23,
        SafetyComplianceTolerancesMessgae = 24,
        ProgramStateMessage = 25
    }

    public enum ErrorCode
    {
        None,
        ResponseTimeout,
    }
    #endregion
}
#endregion