using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Motion.Comizoa
{
    /// <summary>
    /// ImportComiMotionDLL에 대한 요약 설명입니다.
    /// 수정일자 : 2011.05.03
    /// </summary>
    public unsafe class CMDLL
    {
        public delegate void CallbackFunc(IntPtr lParam);

        //====================== General FUNCTIONS ====================================================//
        // 1. cmmGnDeviceLoad
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnDeviceLoad", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl)*/)]
        public static extern unsafe int cmmGnDeviceLoad([MarshalAs(UnmanagedType.I4)] int IsResetDevice, [MarshalAs(UnmanagedType.I4)] ref int NumAxes);

        // 2. cmmGnDeviceUnload
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnDeviceUnload", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnDeviceUnload();

        // 3. cmmGnDeviceIsLoaded
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnDeviceIsLoaded", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnDeviceIsLoaded([MarshalAs(UnmanagedType.I4)] ref int IsLoaded);

        // 4. cmmGnDeviceReset
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnDeviceReset", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnDeviceReset();

        // 5. cmmGnInitFromFile
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnInitFromFile", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnInitFromFile([MarshalAs(UnmanagedType.LPStr)] string szCmeFile);

        // 6. cmmGnInitFromFile_MapOnly
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnInitFromFile_MapOnly", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnInitFromFile_MapOnly([MarshalAs(UnmanagedType.LPStr)] string szCmeFile, [MarshalAs(UnmanagedType.I4)] int MapType);

        // 7. cmmGnSetServoOn
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnSetServoOn", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnSetServoOn([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Enable);

        // 8. cmmGnGetServoOn
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnGetServoOn", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnGetServoOn([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int Enable);

        // 9. cmmGnSetAlarmRes
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnSetAlarmRes", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnSetAlarmRes([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int IsOn);

        // 10. cmmGnGetAlarmRes
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnGetAlarmRes", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnGetAlarmRes([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int IsOn);

        // 11. cmmGnPulseAlarmRes
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnPulseAlarmRes", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnPulseAlarmRes([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int IsOnPulse,
            [MarshalAs(UnmanagedType.I4)] int dwDuration, [MarshalAs(UnmanagedType.I4)] int IsWaitPulseEnd);

        // 12. cmmGnSetSimulMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnSetSimulMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnSetSimulMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int IsSimulMode);

        // 13. cmmGnGetSimulMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnGetSimulMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnGetSimulMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int IsSimulMode);

        // 14. cmmGnPutInternalSTA
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnPutInternalSTA", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnPutInternalSTA([MarshalAs(UnmanagedType.I4)] int AxesMask);

        // 15. cmmGnSetEmergency
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnSetEmergency", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnSetEmergency([MarshalAs(UnmanagedType.I4)] int IsEnable, [MarshalAs(UnmanagedType.I4)] int IsDecStop);

        // 16. cmmGnGetEmergency
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnGetEmergency", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnGetEmergency([MarshalAs(UnmanagedType.I4)] ref int IsEnabled);

        // 17. cmmGnBitShift
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmGnBitShift", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmGnBitShift([MarshalAs(UnmanagedType.I4)] int Value, [MarshalAs(UnmanagedType.I4)] int ShiftOption, [MarshalAs(UnmanagedType.I4)] ref int Result);


        //====================== Configuration FUNCTIONS ==============================================//
        // 1. cmmCfgSetMioProperty
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetMioProperty", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetMioProperty([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int PropId, [MarshalAs(UnmanagedType.I4)] int PropVal);

        // 2. cmmCfgGetMioProperty
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetMioProperty", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetMioProperty([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int PropId, [MarshalAs(UnmanagedType.I4)] ref int PropVal);

        // 3. cmmCfgSetFilter
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetFilter", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetFilter([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int IsEnable);

        // 4. cmmCfgGetFilter
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetFilter", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetFilter([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int IsEnabled);

        // 5. cmmCfgSetFilterAB
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetFilterAB", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetFilterAB([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Target, [MarshalAs(UnmanagedType.I4)] int IsEnable);

        // 6. cmmCfgGetFilterAB
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetFilterAB", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetFilterAB([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Target, [MarshalAs(UnmanagedType.I4)] ref int IsEnabled);

        // 7. cmmCfgSetInMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetInMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetInMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int InputMode,
            [MarshalAs(UnmanagedType.I4)] int IsReverse);

        // 8. cmmCfgGetInMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetInMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetInMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int InputMode,
            [MarshalAs(UnmanagedType.I4)] ref int IsReverse);

        // 9. cmmCfgSetOutMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetOutMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetOutMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int OutputMode);

        // 10. cmmCfgGetOutMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetOutMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetOutMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int OutputMode);

        // 9. cmmCfgSetCtrlMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetCtrlMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetCtrlMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int CtrlMode);

        // 10. cmmCfgGetCtrlMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetCtrlMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetCtrlMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int CtrlMode);

        // 11. cmmCfgSetInOutRatio
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetInOutRatio", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetInOutRatio([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double Ratio);

        // 12. cmmCfgGetInOutRatio
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetInOutRatio", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetInOutRatio([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] ref double Ratio);

        // 13. cmmCfgSetUnitDist
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetUnitDist", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetUnitDist([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double UnitDist);

        // 14. cmmcfgGetUnitDist
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetUnitDist", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetUnitDist([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] ref double UnitDist);

        // 15. cmmCfgSetUnitSpeed
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetUnitSpeed", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetUnitSpeed([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double UnitSpeed);

        // 16. cmmCfgGetUnitSpeed
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetUnitSpeed", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetUnitSpeed([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] ref double UnitSpeed);

        // 17. cmmCfgSetSpeedRange
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetSpeedRange", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetSpeedRange([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double MaxPPS);

        // 18. cmmCfgGetSpeedRange
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetSpeedRange", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetSpeedRange([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] ref double MinPPS,
            [MarshalAs(UnmanagedType.R8)] ref double MaxPPS);

        // 19. cmmCfgSetSpeedPattern
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetSpeedPattern", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetSpeedPattern([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double WorkSpeed, [MarshalAs(UnmanagedType.R8)] double Accel, [MarshalAs(UnmanagedType.R8)] double Decel);

        // 20. cmmCfgGetSpeedPattern
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetSpeedPattern", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetSpeedPattern([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] ref double WorkSpeed, [MarshalAs(UnmanagedType.R8)] ref double Accel, [MarshalAs(UnmanagedType.R8)] ref double Decel);

        // 19. cmmCfgSetSpeedPattern_T
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetSpeedPattern_T", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetSpeedPattern_T([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double WorkSpeed, [MarshalAs(UnmanagedType.R8)] double AccelTime, [MarshalAs(UnmanagedType.R8)] double DecelTime); // <V5.0.4.0>

        // 20. cmmCfgGetSpeedPattern_T
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetSpeedPattern_T", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetSpeedPattern_T([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] ref double WorkSpeed, [MarshalAs(UnmanagedType.R8)] ref double AccelTime, [MarshalAs(UnmanagedType.R8)] ref double DecelTime); // <V5.0.4.0>

        // 21. cmmCfgSetVelCorrRatio
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetVelCorrRatio", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetVelCorrRatio([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double CorrRatio);

        // 22. cmmCfgGetVelCorrRatio
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetVelCorrRatio", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetVelCorrRatio([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] ref double CorrRatio);

        // 23. cmmCfgSetMinCorrVel
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetMinCorrVel", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetMinCorrVel([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double MinVel); // <V5.0.4.0>

        // 24. cmmCfgGetMinCorrVel
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetMinCorrVel", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetMinCorrVel([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] ref double MinVel); // <V5.0.4.0>

        // 25. cmmCfgSetMinAccTime
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetMinAccTime", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetMinAccTime([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double MinAccT,
            [MarshalAs(UnmanagedType.R8)] double MinDecT); // <V5.0.4.0>

        // 26. cmmCfgGetMinAccTime
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetMinAccTime", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetMinAccTime([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] ref double MinAccT,
            [MarshalAs(UnmanagedType.R8)] ref double MinDecT); // <V5.0.4.0>

        // 27. cmmCfgSetActSpdCheck
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetActSpdCheck", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetActSpdCheck([MarshalAs(UnmanagedType.I4)] int IsEnable, [MarshalAs(UnmanagedType.I4)] int Interval);

        // 28. cmmCfgGetActSpdCheck
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetActSpdCheck", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetActSpdCheck([MarshalAs(UnmanagedType.I4)] ref int IsEnable, [MarshalAs(UnmanagedType.I4)] ref int Interval);

        // 29. cmmCfgSetSoftLimit
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetSoftLimit", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetSoftLimit([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int IsEnable,
            [MarshalAs(UnmanagedType.R8)] double LimitN, [MarshalAs(UnmanagedType.R8)] double LimitP);

        // 30. cmmCfgGetSoftLimit
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetSoftLimit", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetSoftLimit([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int IsEnable,
            [MarshalAs(UnmanagedType.R8)] ref double LimitN, [MarshalAs(UnmanagedType.R8)] ref double LimitP);

        // 31. cmmCfgSetRingCntr
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetRingCntr", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetRingCntr([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int TargCntr,
            [MarshalAs(UnmanagedType.I4)] int IsEnable, [MarshalAs(UnmanagedType.R8)] double CntMax);

        // 32. cmmCfgGetRingCntr
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetRingCntr", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetRingCntr([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int TargCntr,
            [MarshalAs(UnmanagedType.I4)] ref int IsEnable, [MarshalAs(UnmanagedType.R8)] ref double CntMax);

        // 33. cmmCfgSetSeqMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetSeqMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetSeqMode([MarshalAs(UnmanagedType.I4)] int SeqMode);

        // 34. cmmCfgGetSeqMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetSeqMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetSeqMode([MarshalAs(UnmanagedType.I4)] ref int SeqMode);

        // 35. cmmCfgSetManExtLimit
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgSetManExtLimit", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgSetManExtLimit([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int IsSetELP,
            [MarshalAs(UnmanagedType.I4)] int IsEnable, [MarshalAs(UnmanagedType.I4)] int ManState);

        // 36. cmmCfgGetManExtLimit
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCfgGetManExtLimit", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCfgGetManExtLimit([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int IsSetELP,
            [MarshalAs(UnmanagedType.I4)] ref int IsEnable, [MarshalAs(UnmanagedType.I4)] ref int ManState);


        //====================== HOME-RETURN FUNCTIONS ================================================//
        // 1. cmmHomeSetConfig
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeSetConfig", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeSetConfig([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int HomeMode,
            [MarshalAs(UnmanagedType.I4)] int EzCount, [MarshalAs(UnmanagedType.R8)] double EscDist, [MarshalAs(UnmanagedType.R8)] double Offset);

        // 2. cmmHomeGetConfig
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeGetConfig", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeGetConfig([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int HomeMode,
            [MarshalAs(UnmanagedType.I4)] ref int EzCount, [MarshalAs(UnmanagedType.R8)] ref double EscDist, [MarshalAs(UnmanagedType.R8)] ref double Offset);

        // 3. cmmHomeSetPosClrMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeSetPosClrMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeSetPosClrMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int PosClrMode);

        // 4. cmmHomeGetPosClrMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeGetPosClrMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeGetPosClrMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int PosClrMode);

        // 5. cmmHomeSetSpeedPattern
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeSetSpeedPattern", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeSetSpeedPattern([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double Vel, [MarshalAs(UnmanagedType.R8)] double Accel,
            [MarshalAs(UnmanagedType.R8)] double Decel, [MarshalAs(UnmanagedType.R8)] double RevVel);

        // 6. cmmHomeGetSpeedPattern
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeGetSpeedPattern", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeGetSpeedPattern([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] ref double Vel, [MarshalAs(UnmanagedType.R8)] ref double Accel,
            [MarshalAs(UnmanagedType.R8)] ref double Decel, [MarshalAs(UnmanagedType.R8)] ref double RevVel);

        // 7. cmmHomeSetSpeedPattern_T
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeSetSpeedPattern_T", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeSetSpeedPattern_T([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double Vel, [MarshalAs(UnmanagedType.R8)] double AccelTime,
            [MarshalAs(UnmanagedType.R8)] double DecelTime, [MarshalAs(UnmanagedType.R8)] double RevVel);

        // 8. cmmHomeGetSpeedPattern_T
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeGetSpeedPattern_T", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeGetSpeedPattern_T([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] ref double Vel, [MarshalAs(UnmanagedType.R8)] ref double AccelTime,
            [MarshalAs(UnmanagedType.R8)] ref double DecelTime, [MarshalAs(UnmanagedType.R8)] ref double RevVel);

        // 9. cmmHomeMoveStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeMoveStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeMoveStart([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Direction);

        // 10. cmmHomeMove
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeMove", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeMove([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Direction,
            [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 11. cmmHomeMoveAll
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeMoveAll", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeMoveAll([MarshalAs(UnmanagedType.I4)] int NumAxes, [MarshalAs(UnmanagedType.I4)] ref int AxisList,
            [MarshalAs(UnmanagedType.I4)] ref int DirList, [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 12. cmmHomeMoveAllStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeMoveAllStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeMoveAllStart([MarshalAs(UnmanagedType.I4)] int NumAxes, [MarshalAs(UnmanagedType.I4)] ref int AxisList,
            [MarshalAs(UnmanagedType.I4)] ref int DirList);

        // 13. cmmHomeGetSuccess
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeGetSuccess", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeGetSuccess([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int IsSuccess);

        // 14. cmmHomeSetSuccess
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeSetSuccess", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeSetSuccess([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int IsSuccess);

        // 15.cmmHomeIsBusy
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeIsBusy", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeIsBusy([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int IsBusy);

        // 16. cmmHomeWaitDone
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmHomeWaitDone", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmHomeWaitDone([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int IsBlocking);


        //====================== Single Axis Move FUNCTIONS ===========================================//
        // 1. cmmSxSetSpeedRatio
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxSetSpeedRatio", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxSetSpeedRatio([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double VelRatio, [MarshalAs(UnmanagedType.R8)] double AccRatio, [MarshalAs(UnmanagedType.R8)] double DecRatio);

        // 2. cmmSxGetSpeedRatio
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxGetSpeedRatio", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxGetSpeedRatio([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] ref double VelRatio, [MarshalAs(UnmanagedType.R8)] ref double AccRatio, [MarshalAs(UnmanagedType.R8)] ref double DecRatio);

        // 3. cmmSxMoveStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxMoveStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxMoveStart([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double Distance);

        // 4. cmmSxMove
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxMove", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxMove([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double Distance, [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 5. cmmSxMoveToStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxMoveToStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxMoveToStart([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double Position);

        // 6. cmmSxMoveTo
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxMoveTo", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxMoveTo([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double Position, [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 7. cmmSxVMoveStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxVMoveStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxVMoveStart([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Dir);

        // 8. cmmSxStop
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxStop", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxStop([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int IsWaitComplete, [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 9. cmmSxStopEmg
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxStopEmg", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxStopEmg([MarshalAs(UnmanagedType.I4)] int Axis);

        // 10. cmmSxIsDone
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxIsDone", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxIsDone([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int IsDone);

        // 11. cmmSxWaitDone
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxWaitDone", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxWaitDone([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 12. cmmSxGetTargetPos
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxGetTargetPos", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxGetTargetPos([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] ref double Position);

        // 13. cmmSxOptSetIniSpeed
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxOptSetIniSpeed", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxOptSetIniSpeed([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double IniSpeed);

        // 14. cmmSxOptGetIniSpeed
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxOptGetIniSpeed", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxOptGetIniSpeed([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] ref double IniSpeed);

        // 15. cmmSxSetCorrection
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxSetCorrection", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxSetCorrection([MarshalAs(UnmanagedType.I4)] int Channel, [MarshalAs(UnmanagedType.I4)] int CorrMode,
            [MarshalAs(UnmanagedType.R8)] double CorrAmount, [MarshalAs(UnmanagedType.R8)] double CorrVel, [MarshalAs(UnmanagedType.I4)] int CntrMask);

        // 16. cmmSxGetCorrection
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxGetCorrection", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxGetCorrection([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int CorrMode,
            [MarshalAs(UnmanagedType.R8)] ref double CorrAmount, [MarshalAs(UnmanagedType.R8)] ref double CorrVel, [MarshalAs(UnmanagedType.I4)] ref int CntrMask);

        // 17. cmmSxOptSetSyncMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxOptSetSyncMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxOptSetSyncMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Mode,
            [MarshalAs(UnmanagedType.I4)] int RefAxis, [MarshalAs(UnmanagedType.I4)] int Condition);

        // 18. cmmSxOptGetSyncMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxOptGetSyncMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxOptGetSyncMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int Mode,
            [MarshalAs(UnmanagedType.I4)] ref int RefAxis, [MarshalAs(UnmanagedType.I4)] ref int Condition);

        // 19. cmmSxOptSetSyncOut
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxOptSetSyncOut", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxOptSetSyncOut([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Mode,
            [MarshalAs(UnmanagedType.I4)] int DoChan_local, [MarshalAs(UnmanagedType.I4)] int DoLogic);

        // 20. cmmSxOptGetSyncOut
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxOptGetSyncOut", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxOptGetSyncOut([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int Mode,
            [MarshalAs(UnmanagedType.I4)] ref int DoChan_local, [MarshalAs(UnmanagedType.I4)] ref int DoLogic);

        // 21. cmmSxOptSetRdpOffset
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxOptSetRdpOffset", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxOptSetRdpOffset([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double OffsetDist);

        // 22. cmmSxOptGetRdpOffset
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmSxOptGetRdpOffset", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmSxOptGetRdpOffset([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] ref double OffsetDist);


        //====================== Multiple Axes Move FUNCTIONS =========================================//
        // 1. cmmMxMove
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmMxMove", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmMxMove([MarshalAs(UnmanagedType.I4)] int NumAxes, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] DistList, [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 2. cmmMxVMoveStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmMxVMoveStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmMxVMoveStart([MarshalAs(UnmanagedType.I4)] int NumAxes, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] DirList);

        // 3. cmmMxMoveStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmMxMoveStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmMxMoveStart([MarshalAs(UnmanagedType.I4)] int NumAxes, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] DistList);

        // 4. cmmMxMoveTo
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmMxMoveTo", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmMxMoveTo([MarshalAs(UnmanagedType.I4)] int NumAxes, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] PosList, [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 5. cmmMxMoveToStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmMxMoveToStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmMxMoveToStart([MarshalAs(UnmanagedType.I4)] int NumAxes, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] PosList);

        // 6. cmmMxStop 
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmMxStop", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmMxStop([MarshalAs(UnmanagedType.I4)] int NumAxes, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.I4)] int IsWaitComplete, [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 7. cmmMxStopEmg
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmMxStopEmg", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmMxStopEmg([MarshalAs(UnmanagedType.I4)] int NumAxes, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList);

        // 8. cmmMxIsDone
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmMxIsDone", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmMxIsDone([MarshalAs(UnmanagedType.I4)] int NumAxes, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] IsDone);

        // 9. cmmMxWaitDone
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmMxWaitDone", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmMxWaitDone([MarshalAs(UnmanagedType.I4)] int NumAxes, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.I4)] int IsBlocking);


        //====================== Interpolation Move FUNCTIONS =========================================//
        // 1. cmmIxMapAxes
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxMapAxes", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxMapAxes([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.I4)] int MapMask1, [MarshalAs(UnmanagedType.I4)] int MapMask2);

        // 2. cmmIxSetSpeedPattern
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxSetSpeedPattern", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxSetSpeedPattern([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.I4)] int IsVectorSpeed,
            [MarshalAs(UnmanagedType.I4)] int SpeedMode, [MarshalAs(UnmanagedType.R8)] double Vel,
            [MarshalAs(UnmanagedType.R8)] double Acc, [MarshalAs(UnmanagedType.R8)] double Dec);

        // 3. cmmIxGetSpeedPattern
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxGetSpeedPattern", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxGetSpeedPattern([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.I4)] ref int IsVectorSpeed,
            [MarshalAs(UnmanagedType.I4)] ref int SpeedMode, [MarshalAs(UnmanagedType.R8)] ref double Vel,
            [MarshalAs(UnmanagedType.R8)] ref double Acc, [MarshalAs(UnmanagedType.R8)] ref double Dec);

        // 4. cmmIxSetSpeedPattern_T
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxSetSpeedPattern_T", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxSetSpeedPattern_T([MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] int SpeedMode, [MarshalAs(UnmanagedType.R8)] double Vel,
            [MarshalAs(UnmanagedType.R8)] double AccelTime, [MarshalAs(UnmanagedType.R8)] double DecelTime); // <V5.0.4.0>

        // 5. cmmIxGetSpeedPattern_T
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxGetSpeedPattern_T", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxGetSpeedPattern_T([MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int SpeedMode, [MarshalAs(UnmanagedType.R8)] ref double Vel,
            [MarshalAs(UnmanagedType.R8)] ref double AccelTime, [MarshalAs(UnmanagedType.R8)] ref double DecelTime); // <V5.0.4.0>

        // 6. cmmIxLine
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxLine", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxLine([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.R8)] ref double DistList, [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 7. cmmIxLineStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxLineStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxLineStart([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.R8)] ref double DistList);

        // 8. cmmIxLineTo
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxLineTo", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxLineTo([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] PostList,
            [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 9. cmmIxLineToStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxLineToStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxLineToStart([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.R8)] ref double PostList);

        // 10. cmmIxArcA
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxArcA", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxArcA([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.R8)] double XCentOffset,
            [MarshalAs(UnmanagedType.R8)] double YCentOffset, [MarshalAs(UnmanagedType.R8)] double EndAngle, [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 11. cmmIxArcAStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxArcAStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxArcAStart([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.R8)] double XCentOffset,
            [MarshalAs(UnmanagedType.R8)] double YCentOffset, [MarshalAs(UnmanagedType.R8)] double EndAngle);

        // 12. cmmIxArcATo
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxArcATo", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxArcATo([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.R8)] double XCent,
            [MarshalAs(UnmanagedType.R8)] double YCent, [MarshalAs(UnmanagedType.R8)] double EndAngle, [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 13. cmmIxArcAToStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxArcAToStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxArcAToStart([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.R8)] double XCent,
            [MarshalAs(UnmanagedType.R8)] double YCent, [MarshalAs(UnmanagedType.R8)] double EndAngle);

        // 14. cmmIxArcP
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxArcP", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxArcP([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.R8)] double XCentOffset,
            [MarshalAs(UnmanagedType.R8)] double YCentOffset, [MarshalAs(UnmanagedType.R8)] double XEndPointDist,
            [MarshalAs(UnmanagedType.R8)] double YEndPointDist, [MarshalAs(UnmanagedType.I4)] int Direction,
            [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 15. cmmIxArcPStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxArcPStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxArcPStart([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.R8)] double XCentOffset,
            [MarshalAs(UnmanagedType.R8)] double YCentOffset, [MarshalAs(UnmanagedType.R8)] double XEndPointDist,
            [MarshalAs(UnmanagedType.R8)] double YEndPointDist, [MarshalAs(UnmanagedType.I4)] int Direction);

        // 16. cmmIxArcPTo
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxArcPTo", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxArcPTo([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.R8)] double XCent,
            [MarshalAs(UnmanagedType.R8)] double YCent, [MarshalAs(UnmanagedType.R8)] double XEndPos,
            [MarshalAs(UnmanagedType.R8)] double YEndPos, [MarshalAs(UnmanagedType.I4)] int Direction,
            [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 17. cmmIxArcPToStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxArcPToStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxArcPToStart([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.R8)] double XCent,
            [MarshalAs(UnmanagedType.R8)] double YCent, [MarshalAs(UnmanagedType.R8)] double XEndPos,
            [MarshalAs(UnmanagedType.R8)] double YEndPos, [MarshalAs(UnmanagedType.I4)] int Direction);

        // 18. cmmIxArc3P
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxArc3P", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxArc3P([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.R8)] double P2X,
            [MarshalAs(UnmanagedType.R8)] double P2Y, [MarshalAs(UnmanagedType.R8)] double P3X,
            [MarshalAs(UnmanagedType.R8)] double P3Y, [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 19. cmmIxArc3PStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxArc3PStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxArc3PStart([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.R8)] double P2X,
            [MarshalAs(UnmanagedType.R8)] double P2Y, [MarshalAs(UnmanagedType.R8)] double P3X,
            [MarshalAs(UnmanagedType.R8)] double P3Y, [MarshalAs(UnmanagedType.R8)] double EndAngle);

        // 20. cmmIxArc3PTo
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxArc3PTo", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxArc3PTo([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.R8)] double P2X,
            [MarshalAs(UnmanagedType.R8)] double P2Y, [MarshalAs(UnmanagedType.R8)] double P3X,
            [MarshalAs(UnmanagedType.R8)] double P3Y, [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 21. cmmIxArc3PToStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxArc3PToStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxArc3PToStart([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.R8)] double P2X,
            [MarshalAs(UnmanagedType.R8)] double P2Y, [MarshalAs(UnmanagedType.R8)] double P3X,
            [MarshalAs(UnmanagedType.R8)] double P3Y, [MarshalAs(UnmanagedType.R8)] double EndAngle);

        // 22. cmmIxIsDone
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxIsDone", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxIsDone([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.I4)] ref int IsDone);

        // 23. cmmIxWaitDone
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxWaitDone", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxWaitDone([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 24. cmmIxStop
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxStop", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxStop([MarshalAs(UnmanagedType.I4)] int MapIndex, [MarshalAs(UnmanagedType.I4)] int IsWaitComplete,
            [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 25. cmmIxStopEmg
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxStopEmg", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxStopEmg([MarshalAs(UnmanagedType.I4)] int MapIndex);

        // 26. cmmIxxHelOnceSetSpeed
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxxHelOnceSetSpeed", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxxHelOnceSetSpeed([MarshalAs(UnmanagedType.I4)] int HelId, [MarshalAs(UnmanagedType.I4)] int Master,
            [MarshalAs(UnmanagedType.I4)] int SpeedMode, [MarshalAs(UnmanagedType.R8)] double WorkSpeed,
            [MarshalAs(UnmanagedType.R8)] double Acc, [MarshalAs(UnmanagedType.R8)] double Dec);

        // 27. cmmIxxHelOnceGetSpeed
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxxHelOnceGetSpeed", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxxHelOnceGetSpeed([MarshalAs(UnmanagedType.I4)] int HelId, [MarshalAs(UnmanagedType.I4)] ref int Master,
            [MarshalAs(UnmanagedType.I4)] ref int SpeedMode, [MarshalAs(UnmanagedType.R8)] ref double WorkSpeed,
            [MarshalAs(UnmanagedType.R8)] ref double Acc, [MarshalAs(UnmanagedType.R8)] ref double Dec);

        // 28. cmmIxxHelOnce
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxxHelOnce", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxxHelOnce([MarshalAs(UnmanagedType.I4)] int HelId, [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] CoordList,
            [MarshalAs(UnmanagedType.R8)] double ArcAngle, [MarshalAs(UnmanagedType.R8)] ref double DistU,
            [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 29. cmmIxxHelOnceStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxxHelOnceStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxxHelOnceStart([MarshalAs(UnmanagedType.I4)] int NumAxes, [MarshalAs(UnmanagedType.I4)] ref int AxisList,
            [MarshalAs(UnmanagedType.R8)] ref double CoordList, [MarshalAs(UnmanagedType.R8)] double ArcAngle,
            [MarshalAs(UnmanagedType.R8)] ref double DistU);

        // 30. cmmIxxSplineBuild
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIxxSplineBuild", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIxxSplineBuild([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] InArray, [MarshalAs(UnmanagedType.I4)] int NumInArray,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] OutArray, [MarshalAs(UnmanagedType.I4)] int NumOutArray);


        //====================== External Switch Move FUNCTIONS =======================================//
        // 1. cmmExVMoveStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmExVMoveStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmExVMoveStart([MarshalAs(UnmanagedType.I4)] int Axis);

        // 2. cmmExMoveStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmExMoveStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmExMoveStart([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double Distance);

        // 3. cmmExMoveToStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmExMoveToStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmExMoveToStart([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double Position);


        //====================== Manual Pulsar FUNCTIONS ==============================================//
        // 1. cmmPlsrSetInMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmPlsrSetInMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmPlsrSetInMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int InputMode, [MarshalAs(UnmanagedType.I4)] int IsInverse);

        // 2. cmmPlsrGetInMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmPlsrGetInMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmPlsrGetInMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int InputMode, [MarshalAs(UnmanagedType.I4)] ref int IsInverse);

        // 3. cmmPlsrSetGain
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmPlsrSetGain", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmPlsrSetGain([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int GainFactor, [MarshalAs(UnmanagedType.I4)] int DivFactor);

        // 4. cmmPlsrGetGain
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmPlsrGetGain", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmPlsrGetGain([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int GainFactor, [MarshalAs(UnmanagedType.I4)] ref int DivFactor);

        // 5. cmmPlsrHomeMoveStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmPlsrHomeMoveStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmPlsrHomeMoveStart([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int HomeType);

        // 6. cmmPlsrMoveStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmPlsrMoveStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmPlsrMoveStart([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double Distance);

        // 7. cmmPlsrMove
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmPlsrMove", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmPlsrMove([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double Distance, [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 8. cmmPlsrMoveToStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmPlsrMoveToStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmPlsrMoveToStart([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double Position);

        // 9. cmmPlsrMoveTo
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmPlsrMoveTo", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmPlsrMoveTo([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double Position, [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 10. cmmPlsrVMoveStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmPlsrVMoveStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmPlsrVMoveStart([MarshalAs(UnmanagedType.I4)] int Axis);

        // 11. cmmPlsrIsActive
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmPlsrIsActive", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmPlsrIsActive([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int nIsActive);


        //====================== MASTER/SLAVE FUNCTIONS ===============================================//
        // 1. cmmMsRegisterSlave
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmMsRegisterSlave", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmMsRegisterSlave([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double MaxSpeed, [MarshalAs(UnmanagedType.I4)] int IsInverse);

        // 2. cmmMsUnregisterSlave
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmMsUnregisterSlave", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmMsUnregisterSlave([MarshalAs(UnmanagedType.I4)] int Axis);

        // 3. cmmMsCheckSlaveState
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmMsCheckSlaveState", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmMsCheckSlaveState([MarshalAs(UnmanagedType.I4)] int SlaveAxis, [MarshalAs(UnmanagedType.I4)] ref int SlaveState);

        // 4. cmmMsGetMasterAxis
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmMsGetMasterAxis", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmMsGetMasterAxis([MarshalAs(UnmanagedType.I4)] int SlaveAxis, [MarshalAs(UnmanagedType.I4)] ref int MasterAxis);


        //====================== Overriding FUNCTIONS =================================================//
        // 1. cmmOverrideSpeedSet
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmOverrideSpeedSet", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmOverrideSpeedSet([MarshalAs(UnmanagedType.I4)] int Axis);

        // 2. cmmOverrideSpeedSetAll
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmOverrideSpeedSetAll", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmOverrideSpeedSetAll([MarshalAs(UnmanagedType.I4)] int NumAxes, [MarshalAs(UnmanagedType.I4)] ref int AxisList);

        // 3. cmmOverrideMove
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmOverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmOverrideMove([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double NewDistance, [MarshalAs(UnmanagedType.I4)] ref int IsIgnored);

        // 4. cmmOverrideMoveTo
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmOverrideMoveTo", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmOverrideMoveTo([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double NewPosition, [MarshalAs(UnmanagedType.I4)] ref int IsIgnored);


        //====================== LIST-MOTION FUNCTIONS ================================================//
        // 1. cmmLmMapAxes
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmMapAxes", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmMapAxes([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] int MapMask1, [MarshalAs(UnmanagedType.I4)] int MapMask2);

        // 2. cmmLmBeginList
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmBeginList", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmBeginList([MarshalAs(UnmanagedType.I4)] int LmIndex);

        // 3. cmmLmEndList
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmEndList", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmEndList([MarshalAs(UnmanagedType.I4)] int LmIndex);

        // 4. cmmLmStartMotion
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmStartMotion", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmStartMotion([MarshalAs(UnmanagedType.I4)] int LmIndex);

        // 5. cmmLmAbortMotion
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmAbortMotion", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmAbortMotion([MarshalAs(UnmanagedType.I4)] int LmIndex);

        // 6. cmmLmAbortMotionEx
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmAbortMotionEx", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmAbortMotionEx([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.R8)] double DecelT_sec);

        // 7. cmmLmIsDone
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmIsDone", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmIsDone([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] ref int IsDone);

        // 8. cmmLmWaitDone
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmWaitDone", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmWaitDone([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] int IsBlocking);

        // 9. cmmLmCurSequence
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmCurSequence", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmCurSequence([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] ref int SeqIndex);

        // 10. cmmLmImmediacySet
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmImmediacySet", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmImmediacySet([MarshalAs(UnmanagedType.I4)] int LmIndex);

        // 11. cmmLmDoPutOne
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmDoPutOne", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmDoPutOne([MarshalAs(UnmanagedType.I4)] int LmIndex, IntPtr hDoDevice /*HANDLE*/,
            [MarshalAs(UnmanagedType.I4)] int Channel, [MarshalAs(UnmanagedType.I4)] int OutState);

        // 12. cmmLmDoPutMulti
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmDoPutMulti", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmDoPutMulti([MarshalAs(UnmanagedType.I4)] int LmIndex, IntPtr hDoDevice /*HANDLE*/,
            [MarshalAs(UnmanagedType.I4)] int ChannelGroup, [MarshalAs(UnmanagedType.I4)] int Mask,
            [MarshalAs(UnmanagedType.I4)] int OutStates);

        // 13. cmmLmDoPulseOne
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmDoPulseOne", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmDoPulseOne([MarshalAs(UnmanagedType.I4)] int LmIndex, IntPtr hDoDevice /*HANDLE*/,
            [MarshalAs(UnmanagedType.I4)] int Channel, [MarshalAs(UnmanagedType.I4)] int OutState,
            [MarshalAs(UnmanagedType.I4)] int Duration);

        // 14. cmmLmDoPulseMulti
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmDoPulseMulti", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmDoPulseMulti([MarshalAs(UnmanagedType.I4)] int LmIndex, IntPtr hDoDevice /*HANDLE*/,
            [MarshalAs(UnmanagedType.I4)] int ChannelGroup, [MarshalAs(UnmanagedType.I4)] int Mask,
            [MarshalAs(UnmanagedType.I4)] int OutStates, [MarshalAs(UnmanagedType.I4)] int Duration);


        //====================== 상태감시 FUNCTIONS ===================================================//
        // 1. cmmStSetCount
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmStSetCount", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmStSetCount([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Target, [MarshalAs(UnmanagedType.I4)] int Count);

        // 2. cmmStGetCount
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmStGetCount", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmStGetCount([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Source, [MarshalAs(UnmanagedType.I4)] ref int Count);

        // 3. cmmStSetPosition
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmStSetPosition", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmStSetPosition([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Target, [MarshalAs(UnmanagedType.R8)] double Position);

        // 4. cmmStGetPosition
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmStGetPosition", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmStGetPosition([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Source, [MarshalAs(UnmanagedType.R8)] ref double Position);

        // 5. cmmStGetSpeed
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmStGetSpeed", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmStGetSpeed([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Source, [MarshalAs(UnmanagedType.R8)] ref double Speed);

        // 6. cmmStReadMotionState
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmStReadMotionState", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmStReadMotionState([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int MotStates);

        // 7. cmmStReadMioStatuses
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmStReadMioStatuses", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmStReadMioStatuses([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int MioStates);

        // 8. cmmStGetMstString
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmStGetMstString", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmStGetMstString([MarshalAs(UnmanagedType.I4)] int MstCode, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] byte[] Buffer, [MarshalAs(UnmanagedType.I4)] int BufferLen);

        // 9. cmmMstAll_SetCfg
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmMstAll_SetCfg", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmMstAll_SetCfg([MarshalAs(UnmanagedType.I4)] int AxisMask1, [MarshalAs(UnmanagedType.I4)] int AxisMask2, [MarshalAs(UnmanagedType.I4)] int DataMask);

        // 10. cmmMstAll_GetCfg
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmMstAll_GetCfg", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmMstAll_GetCfg([MarshalAs(UnmanagedType.I4)] ref int AxisMask1, [MarshalAs(UnmanagedType.I4)] ref int AxisMask2, [MarshalAs(UnmanagedType.I4)] ref int DataMask);

        // FIXME: 아래의 11 ~ 17 까지의 상태감시 함수는 추후 정의 하기로 함.
        //		CMM_EXTERN long (WINAPI *cmmMstAll_ManScan)		(TCmMstAll *pBuf);
        // 11. cmmMstAll_ManScan
        //		CMM_EXTERN long (WINAPI *cmmMstAll_AutoStart)	(long TimerInterv);
        // 12. cmmMstAll_AutoStart
        //		CMM_EXTERN long (WINAPI *cmmMstAll_AutoStop)	();
        // 13. cmmMstAll_AutoStop
        //		CMM_EXTERN TCmMstAll* (WINAPI *cmmMstAll_AutoGetBuf) (void);
        // 14. cmmMstAll_AutoGetBuf
        //		CMM_EXTERN long (WINAPI *cmmMstAll_AutoGetData)	(TCmMstAll *pBuf, long IsFrameSync);
        // 15. cmmMstAll_AutoGetData
        //		CMM_EXTERN long (WINAPI *cmmMstAll_AutoGetInfo)	(long *ScanCount, long *ScanInerv, long *ScanConsT);
        // 16. cmmMstAll_AutoGetInfo
        //		CMM_EXTERN long (WINAPI *cmmMstAll_AutoGetInfo2) (long AxisInDev, long *ScanCount, long *ScanInerv, long *ScanConsT);
        // 17. cmmMstAll_AutoGetInfo2


        //====================== INTERRUPT FUNCTIONS ==================================================//
        // 1. cmmIntSetMask
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIntSetMask", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIntSetMask([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Mask);

        // 2. cmmIntGetMask
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIntGetMask", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIntGetMask([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int Mask);

        // 3-1. cmmIntHandlerSetup_MSG
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIntHandlerSetup", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIntHandlerSetup_MSG([MarshalAs(UnmanagedType.I4)] int HandlerType, IntPtr Handler,
            [MarshalAs(UnmanagedType.U4)] uint nMessage, IntPtr lParam);

        // 3-2. cmmIntHandlerSetup_EVT
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIntHandlerSetup", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIntHandlerSetup_EVT([MarshalAs(UnmanagedType.I4)] int HandlerType, CallbackFunc Handler,
            [MarshalAs(UnmanagedType.U4)] uint nMessage, IntPtr lParam);

        // 3-3. cmmIntHandlerSetup_CLB
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIntHandlerSetup", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIntHandlerSetup_CLB([MarshalAs(UnmanagedType.I4)] int HandlerType, CallbackFunc Handler,
            [MarshalAs(UnmanagedType.U4)] uint nMessage, IntPtr lParam);

        // 4. cmmIntHandlerEnable
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIntHandlerEnable", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIntHandlerEnable([MarshalAs(UnmanagedType.I4)] int IsEnable);

        // 5. cmmIntReadFlag
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIntReadFlag", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIntReadFlag([MarshalAs(UnmanagedType.I4)] ref int IntFlag1, [MarshalAs(UnmanagedType.I4)] ref int IntFlag2);

        // 6. cmmIntReadErrorStatus
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIntReadErrorStatus", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIntReadErrorStatus([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int ErrState);

        // 7. cmmIntReadEventStatus
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmIntReadEventStatus", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmIntReadEventStatus([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int EventState);

        //====================== LATCH FUNCTIONS =======================================================//
        // 1. cmmLtcIsLatched
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLtcIsLatched", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLtcIsLatched([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int IsLatched);

        // 2. cmmLtcReadLatched
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLtcReadLatch", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLtcReadLatch([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Counter, [MarshalAs(UnmanagedType.R8)] ref double LatchedPos);

        // 3. cmmLtcQue_SetCfg
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLtcQue_SetCfg", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLtcQue_SetCfg([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int QueSize, [MarshalAs(UnmanagedType.I4)] int LtcTargCntr);

        // 4. cmmLtcQue_GetCfg
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLtcQue_GetCfg", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLtcQue_GetCfg([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int QueSize, [MarshalAs(UnmanagedType.I4)] ref int LtcTargCntr);

        // 5. cmmLtcQue_SetEnable
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLtcQue_SetEnable", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLtcQue_SetEnable([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int IsEnabled);

        // 6. cmmLtcQue_GetEnable
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLtcQue_GetEnable", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLtcQue_GetEnable([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int IsEnabled);

        // 7. cmmLtcQue_GetItemCount
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLtcQue_GetItemCount", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLtcQue_GetItemCount([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int LtcItemCount);

        // 8. cmmLtcQue_ResetItemCount
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLtcQue_ResetItemCount", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLtcQue_ResetItemCount([MarshalAs(UnmanagedType.I4)] int Axis);

        // 9. cmmLtcQue_Deque
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLtcQue_Deque", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLtcQue_Deque([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] ref double LtcData);

        // 10. cmmLtcQue_PeekAt
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLtcQue_PeekAt", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLtcQue_PeekAt([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Index, [MarshalAs(UnmanagedType.R8)] ref double LtcData);


        //====================== Position Compare FUNCTIONS ===========================================//
        // 1. cmmCmpErrSetConfig
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpErrSetConfig", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpErrSetConfig([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double Tolerance, [MarshalAs(UnmanagedType.I4)] int IsEnable);

        // 2. cmmCmpErrGetConfig
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpErrGetConfig", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpErrGetConfig([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] ref double Tolerance, [MarshalAs(UnmanagedType.I4)] ref int IsEnabled);

        // 3. cmmCmpGenSetConfig 
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpGenSetConfig", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpGenSetConfig([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int CmpSrc,
            [MarshalAs(UnmanagedType.I4)] int CmpMethod, [MarshalAs(UnmanagedType.I4)] int CmpAction, [MarshalAs(UnmanagedType.R8)] double CmpData);

        // 4. cmmCmpGenGetConfig
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpGenGetConfig", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpGenGetConfig([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int CmpSrc,
            [MarshalAs(UnmanagedType.I4)] ref int CmpMethod, [MarshalAs(UnmanagedType.I4)] ref int CmpAction, [MarshalAs(UnmanagedType.I4)] ref int CmpData);

        // 5. cmmCmpTrgSetConfig
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpTrgSetConfig", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpTrgSetConfig([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int CmpSrc, [MarshalAs(UnmanagedType.I4)] int CmpMethod);

        // 6. cmmCmpTrgGetConfig
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpTrgGetConfig", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpTrgGetConfig([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int CmpSrc, [MarshalAs(UnmanagedType.I4)] ref int CmpMethod);

        // 7. cmmCmpTrgSetOneData
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpTrgSetOneData", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpTrgSetOneData([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double Data);

        // 8. cmmCmpTrgGetCurData
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpTrgGetCurData", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpTrgGetCurData([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] ref double Data);

        // 9. cmmCmpTrgContRegTable
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpTrgContRegTable", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpTrgContRegTable([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] Buffer, [MarshalAs(UnmanagedType.I4)] int NumData);

        // 10. cmmCmpTrgContBuildTable
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpTrgContBuildTable", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpTrgContBuildTable([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] double StartData,
            [MarshalAs(UnmanagedType.R8)] double Interval, [MarshalAs(UnmanagedType.I4)] int NumData);

        // 11. cmmCmpTrgContStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpTrgContStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpTrgContStart([MarshalAs(UnmanagedType.I4)] int Axis);

        // 12. cmmCmpTrgContStop
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpTrgContStop", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpTrgContStop([MarshalAs(UnmanagedType.I4)] int Axis);

        // 13. cmmCmpTrgContIsActive
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpTrgContIsActive", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpTrgContIsActive([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int IsActive);

        // 14. cmmCmpTrgHigh_WriteData
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpTrgHigh_WriteData", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpTrgHigh_WriteData([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int CMPH_No,
            [MarshalAs(UnmanagedType.R8)] double IniPos, [MarshalAs(UnmanagedType.R8)] double Interval);

        // 15. cmmCmpTrgHigh_ReadData
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpTrgHigh_ReadData", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpTrgHigh_ReadData([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int CMPH_No,
            [MarshalAs(UnmanagedType.R8)] ref double IniPos, [MarshalAs(UnmanagedType.R8)] ref double Interval);

        // 16. cmmCmpTrgHigh_Start
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpTrgHigh_Start", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpTrgHigh_Start([MarshalAs(UnmanagedType.I4)] int Axis);

        // 17. cmmCmpTrgHigh_Stop
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpTrgHigh_Stop", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpTrgHigh_Stop([MarshalAs(UnmanagedType.I4)] int Axis);

        // 18. cmmCmpTrgHigh_Check
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpTrgHigh_Check", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpTrgHigh_Check([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int IsActive,
            [MarshalAs(UnmanagedType.I4)] ref int OutCount);

        // 19. cmmCmpQue_SetEnable
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpQue_SetEnable", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpQue_SetEnable([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int IsEnable);

        // 20. cmmCmpQue_GetEnable
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpQue_GetEnable", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpQue_GetEnable([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int IsEnable);

        // 21. cmmCmpQue_SetQueSize
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpQue_SetQueSize", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpQue_SetQueSize([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int QueSize);

        // 22. cmmCmpQue_GetQueSize
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpQue_GetQueSize", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpQue_GetQueSize([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int QueSize);

        // 23. cmmCmpQue_Enque
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpQue_Enque", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpQue_Enque([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int CmpSrc,
            [MarshalAs(UnmanagedType.I4)] int CmpMethod, [MarshalAs(UnmanagedType.I4)] int CmpData);

        // 24. cmmCmpQue_GetEnqueCnt
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpQue_GetEnqueCnt", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpQue_GetEnqueCnt([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int EnqueCnt);

        // 25. cmmCmpQue_GetOutCnt
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpQue_GetOutCnt", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpQue_GetOutCnt([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int OutCnt);

        // 26. cmmCmpQue_SetOutCnt
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpQue_SetOutCnt", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpQue_SetOutCnt([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int OutCnt);

        // 27. cmmCmpQue_SetLtcLinkMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpQue_SetLtcLinkMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpQue_SetLtcLinkMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Enable,
            [MarshalAs(UnmanagedType.I4)] int SrcLtcCnt, [MarshalAs(UnmanagedType.I4)] int CmpSrc,
            [MarshalAs(UnmanagedType.I4)] int CmpMethod, [MarshalAs(UnmanagedType.I4)] int Offset);

        // 28. cmmCmpQue_GetLtcLinkMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmCmpQue_GetLtcLinkMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmCmpQue_GetLtcLinkMode([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int Enable,
            [MarshalAs(UnmanagedType.I4)] ref int SrcLtcCnt, [MarshalAs(UnmanagedType.I4)] ref int CmpSrc,
            [MarshalAs(UnmanagedType.I4)] ref int CmpMethod, [MarshalAs(UnmanagedType.I4)] ref int Offset);


        //====================== Digital In/Out FUNCTIONS =============================================//
        // 1. cmmDiSetInputLogic
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDiSetInputLogic", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDiSetInputLogic([MarshalAs(UnmanagedType.I4)] int Channel, [MarshalAs(UnmanagedType.I4)] int InputLogic);

        // 2. cmmDiGetInputLogic
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDiGetInputLogic", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDiGetInputLogic([MarshalAs(UnmanagedType.I4)] int Channel, [MarshalAs(UnmanagedType.I4)] ref int InputLogic);

        // 3. cmmDiGetOne
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDiGetOne", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDiGetOne([MarshalAs(UnmanagedType.I4)] int Channel, [MarshalAs(UnmanagedType.I4)] ref int InputState);

        // 4. cmmDiGetMulti
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDiGetMulti", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDiGetMulti([MarshalAs(UnmanagedType.I4)] int IniChannel, [MarshalAs(UnmanagedType.I4)] int NumChannels,
            [MarshalAs(UnmanagedType.I4)] ref int InputStates);

        // 5. cmmDiGetOneF
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDiGetOneF", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDiGetOneF([MarshalAs(UnmanagedType.I4)] int Channel, [MarshalAs(UnmanagedType.I4)] int CutoffTime_us, [MarshalAs(UnmanagedType.I4)] ref int InputState);

        // 6. cmmDiGetMultiF
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDiGetMultiF", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDiGetMultiF([MarshalAs(UnmanagedType.I4)] int IniChannel, [MarshalAs(UnmanagedType.I4)] int NumChannels,
            [MarshalAs(UnmanagedType.I4)] int CutoffTime_us, [MarshalAs(UnmanagedType.I4)] ref int InputStates);

        // 7. cmmDoSetOutputLogic
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDoSetOutputLogic", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDoSetOutputLogic([MarshalAs(UnmanagedType.I4)] int Channel, [MarshalAs(UnmanagedType.I4)] int OutputLogic);

        // 8. cmmDoGetOutputLogic
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDoGetOutputLogic", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDoGetOutputLogic([MarshalAs(UnmanagedType.I4)] int Channel, [MarshalAs(UnmanagedType.I4)] ref int OutputLogic);

        // 9. cmmDoPutOne
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDoPutOne", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDoPutOne([MarshalAs(UnmanagedType.I4)] int Channel, [MarshalAs(UnmanagedType.I4)] int OutState);

        // 10. cmmDoGetOne
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDoGetOne", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDoGetOne([MarshalAs(UnmanagedType.I4)] int Channel, [MarshalAs(UnmanagedType.I4)] ref int OutState);

        // 11. cmmDoPulseOne
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDoPulseOne", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDoPulseOne([MarshalAs(UnmanagedType.I4)] int Channel, [MarshalAs(UnmanagedType.I4)] int IsOnPulse,
            [MarshalAs(UnmanagedType.I4)] int dwDuration, [MarshalAs(UnmanagedType.I4)] int IsWaitPulseEnd);

        // 12. cmmDoPutMulti
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDoPutMulti", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDoPutMulti([MarshalAs(UnmanagedType.I4)] int IniChannel, [MarshalAs(UnmanagedType.I4)] int NumChannels,
                                [MarshalAs(UnmanagedType.I4)] int OutStates);

        // 13. cmmDoGetMulti
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDoGetMulti", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDoGetMulti([MarshalAs(UnmanagedType.I4)] int IniChannel, [MarshalAs(UnmanagedType.I4)] int NumChannels,
            [MarshalAs(UnmanagedType.I4)] ref int OutStates);

        // 14. cmmDoPulseMulti
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDoPulseMulti", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDoPulseMulti([MarshalAs(UnmanagedType.I4)] int Channel, [MarshalAs(UnmanagedType.I4)] int NumChannels,
            [MarshalAs(UnmanagedType.I4)] int OutStates, [MarshalAs(UnmanagedType.I4)] int dwDuration,
            [MarshalAs(UnmanagedType.I4)] int IsWaitPulseEnd);


        //====================== Advanced FUNCTIONS ===================================================//
        // 1. cmmAdvGetNumAvailAxes
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvGetNumAvailAxes", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvGetNumAvailAxes([MarshalAs(UnmanagedType.I4)] ref int NumAxes);

        // 2. cmmAdvGetNumDefinedAxes
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvGetNumDefinedAxes", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvGetNumDefinedAxes([MarshalAs(UnmanagedType.I4)] ref int NumAxes);

        // 3. cmmAdvGetNumAvailDioChan
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvGetNumAvailDioChan", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvGetNumAvailDioChan([MarshalAs(UnmanagedType.I4)] int IsInputChannel, [MarshalAs(UnmanagedType.I4)] ref int NumChannels);

        // 4. cmmAdvGetNumDefinedDioChan
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvGetNumDefinedDioChan", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvGetNumDefinedDioChan([MarshalAs(UnmanagedType.I4)] int IsInputChannel, [MarshalAs(UnmanagedType.I4)] ref int NumChannels);

        // 5. cmmAdvGetMotDeviceId
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvGetMotDeviceId", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvGetMotDeviceId([MarshalAs(UnmanagedType.I4)] int Channel, [MarshalAs(UnmanagedType.I4)] ref int DeviceId);

        // 6. cmmAdvGetMotDevInstance
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvGetMotDevInstance", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvGetMotDevInstance([MarshalAs(UnmanagedType.I4)] int Channel, [MarshalAs(UnmanagedType.I4)] ref int DevInstance);

        // 7. cmmAdvGetDioDeviceId
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvGetDioDeviceId", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvGetDioDeviceId([MarshalAs(UnmanagedType.I4)] int Channel,
            [MarshalAs(UnmanagedType.I4)] int IsInputChannel, [MarshalAs(UnmanagedType.I4)] ref int DeviceId);

        // 8. cmmAdvGetDioDevInstance
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvGetDioDevInstance", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvGetDioDevInstance([MarshalAs(UnmanagedType.I4)] int Channel,
            [MarshalAs(UnmanagedType.I4)] int IsInputChannel, [MarshalAs(UnmanagedType.I4)] ref int DevInstance);

        // FIXME: 아래의 9 고급 함수는 추후 정의 하기로 함.
        //		CMM_EXTERN long	(WINAPI *cmmAdvGetDeviceHandle)	(long DeviceId, long DevInstance, HANDLE *DevHandle);
        // 9. cmmAdvGetDeviceHandle

        // 10. cmmAdvWriteMainSpace
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvWriteMainSpace", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvWriteMainSpace([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Addr, [MarshalAs(UnmanagedType.I4)] int Value);

        // 11. cmmAdvReadMainSpace
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvReadMainSpace", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvReadMainSpace([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Addr, [MarshalAs(UnmanagedType.I4)] ref int Value);

        // 12. cmmAdvWriteRegister
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvWriteRegister", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvWriteRegister([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int RegisterNo, [MarshalAs(UnmanagedType.I4)] int RegVal);

        // 13. cmmAdvReadRegister
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvReadRegister", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvReadRegister([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int RegisterNo, [MarshalAs(UnmanagedType.I4)] ref int RegVal);

        // 14. cmmAdvGetMioCfg1Dword
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvGetMioCfg1Dword", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvGetMioCfg1Dword([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] ref int Mio1Dword);

        // 15. cmmAdvSetMioCfg1Dword
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvSetMioCfg1Dword", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvSetMioCfg1Dword([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int Mio1Dword);

        // 16. cmmAdvSetToolboxMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvSetToolboxMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvSetToolboxMode([MarshalAs(UnmanagedType.I4)] int EnInterrupt);

        // 17. cmmAdvGetString
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvGetString", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvGetString([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int StringID, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] byte[] szBuffer);

        // 18. cmmAdvErcOut
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvErcOut", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvErcOut([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int IsWaitOff);

        // 19. cmmAdvErcReset
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvErcReset", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvErcReset([MarshalAs(UnmanagedType.I4)] int Axis);

        // 20. cmmAdvSetExtOptions
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvSetExtOptions", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvSetExtOptions([MarshalAs(UnmanagedType.I4)] int OptionId, [MarshalAs(UnmanagedType.I4)] int lParam1,
            [MarshalAs(UnmanagedType.I4)] int lParam2, [MarshalAs(UnmanagedType.R8)] double fParam1, [MarshalAs(UnmanagedType.R8)] double fParam2);

        // FIXME: 아래의 21 ~ 26 까지의 고급 함수는 추후 정의 하기로 함.
        //		CMM_EXTERN long (WINAPI *cmmAdvEnumMotDevices)	(TMotDevEnum *EnumBuffer);
        // 21. cmmAdvEnumDioDevices

        //		CMM_EXTERN long (WINAPI *cmmAdvGetMotDevMap)	(TMotDevMap *MapBuffer);
        // 22. cmmAdvGetMotDevMap

        //		CMM_EXTERN long (WINAPI *cmmAdvEnumDioDevices)	(TDioDevEnum *EnumBuffer);
        // 23. cmmAdvEnumDioDevices

        //		CMM_EXTERN long (WINAPI *cmmAdvGetDioDevMap)	(TDioDevMap *MapBuffer);
        // 24. cmmAdvGetDioDevMap

        //		CMM_EXTERN long (WINAPI *cmmAdvInitFromCmeBuffer) (TCmeData_V2 *pCmeBuffer);
        // 25. cmmAdvInitFromCmeBuffer

        //      CMM_EXTERN long (WINAPI *cmmAdvInitFromCmeBuffer_MapOnly) (TCmeData_V2 *pCmeBuffer, int nMapType);
        // 26. cmmAdvInitFromCmeBuffer_MapOnly

        // 27. cmmAdvGetLatestCmeFile
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvGetLatestCmeFile", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvGetLatestCmeFile([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] byte[] szCmeFile);

        // 28. cmmAdvGetAxisCapability
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmAdvGetAxisCapability", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmAdvGetAxisCapability([MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.I4)] int CapId, [MarshalAs(UnmanagedType.I4)] ref int CapBuffer);


        //====================== DEBUG-LOGGING FUNCTIONS ==============================================//
        // 1. cmmDlogSetup
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDlogSetup", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDlogSetup([MarshalAs(UnmanagedType.I4)] int Level, [MarshalAs(UnmanagedType.LPStr)] byte[] szLogFile);

        // 2. cmmDlogAddComment
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDlogAddComment", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDlogAddComment([MarshalAs(UnmanagedType.LPStr)] byte[] szComment);

        // 3. cmmDlogGetCurLevel
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDlogGetCurLevel", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDlogGetCurLevel([MarshalAs(UnmanagedType.I4)] ref int CurLevel);

        // 4. cmmDlogGetCurFilePath
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDlogGetCurFilePath", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDlogGetCurFilePath([MarshalAs(UnmanagedType.LPStr)] byte[] szFilePath);

        // 5. cmmDlogEnterManMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDlogEnterManMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDlogEnterManMode([MarshalAs(UnmanagedType.I4)] int nMode);

        // 6. cmmDlogExitManMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmDlogExitManMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmDlogExitManMode();


        //====================== ERROR HANDLING FUNCTIONS =============================================//
        // 1. cmmErrGetLastCode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmErrGetLastCode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmErrGetLastCode([MarshalAs(UnmanagedType.I4)] ref int ErrorCode);

        // 2. cmmErrClearLastCode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmErrClearLastCode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmErrClearLastCode();

        // 3. cmmErrParseAxis
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmErrParseAxis", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe short cmmErrParseAxis([MarshalAs(UnmanagedType.I4)] int ErrorCode);

        // 4. cmmErrParseReason
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmErrParseReason", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe short cmmErrParseReason([MarshalAs(UnmanagedType.I4)] int ErrorCode);

        // 5. cmmErrGetString
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmErrGetString", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmErrGetString([MarshalAs(UnmanagedType.I4)] int ErrorCode, [MarshalAs(UnmanagedType.LPStr)] string Buffer, [MarshalAs(UnmanagedType.I4)] int BufferLen);

        // 6. cmmErrShowLast
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmErrShowLast", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmErrShowLast(IntPtr ParentWnd/* HWND ParentWnd*/);

        // 7. cmmErrSetSkipShowMessage
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmErrSetSkipShowMessage", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmErrSetSkipShowMessage([MarshalAs(UnmanagedType.I4)] int IsSkip);

        // 8. cmmErrGetSkipShowMessage
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmErrGetSkipShowMessage", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmErrGetSkipShowMessage([MarshalAs(UnmanagedType.I4)] ref int IsSkip);

        // 9. cmmErrSetEnableAutoMessage
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmErrSetEnableAutoMessage", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmErrSetEnableAutoMessage([MarshalAs(UnmanagedType.I4)] int Enable);

        // 10. cmmErrGetEnableAutoMessage
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmErrGetEnableAutoMessage", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmErrGetEnableAutoMessage([MarshalAs(UnmanagedType.I4)] ref int Enable);


        //====================== Utility FUNCTIONS ===================================================//
        // 1. cmmUtlProcessWndMsgS
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmUtlProcessWndMsgS", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmUtlProcessWndMsgS(IntPtr WndHandle, [MarshalAs(UnmanagedType.I4)] ref int IsEmpty);

        // 2. cmmUtlProcessWndMsgM
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmUtlProcessWndMsgM", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmUtlProcessWndMsgM(IntPtr WndHandle, [MarshalAs(UnmanagedType.I4)] int Timeout, [MarshalAs(UnmanagedType.I4)] ref int IsTimeOuted);

        // FIXME: 아래의 3 ~ 5 까지의 Utility 함수는 추후 정의 하기로 함.
        //		CMM_EXTERN long (WINAPI *cmmUtlReadUserTable)	(long nAddress, long nSize, UCHAR* pBuffer);
        // 3. cmmUtlReadUserTable

        //		CMM_EXTERN long (WINAPI *cmmUtlWriteUserTable)	(long nAddress, long nSize, UCHAR* pBuffer);
        // 4. cmmUtlWriteUserTable

        //		CMM_EXTERN long (WINAPI *cmmUtlDelayMicroSec)	(long Delay_us);
        // 5. cmmUtlDelayMicroSec


        //====================== Extended List Motion FUNCTIONS ===================================================//
        // 1. cmmLmxStart
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmxStart", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmxStart([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] int AxisMask1, [MarshalAs(UnmanagedType.I4)] int AxisMask2);

        // 2. cmmLmxPause
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmxPause", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmxPause([MarshalAs(UnmanagedType.I4)] int LmIndex);

        // 3. cmmLmxResume
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmxResume", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmxResume([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] int IsClearQue);

        // 4. cmmLmxEnd
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmxEnd", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmxEnd([MarshalAs(UnmanagedType.I4)] int LmIndex);

        // 5. cmmLmxSetSeqMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmxSetSeqMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmxSetSeqMode([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] int SeqMode);

        // 6. cmmLmxGetSeqMode
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmxGetSeqMode", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmxGetSeqMode([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] int SeqMode);

        // 7. cmmLmxSetNextItemId
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmxSetNextItemId", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmxSetNextItemId([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] int SeqId);

        // 8. cmmLmxGetNextItemId
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmxGetNextItemId", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmxGetNextItemId([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] ref int SeqId);

        // 9. cmmLmxSetNextItemParam
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmxSetNextItemParam", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmxSetNextItemParam([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] int ParamIdx, [MarshalAs(UnmanagedType.I4)] int ParamData);

        // 10. cmmLmxGetNextItemParam
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmxGetNextItemParam", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmxGetNextItemParam([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] int ParamIdx, [MarshalAs(UnmanagedType.I4)] ref int ParamData);

        // 11. cmmLmxGetRunItemParam
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmxGetRunItemParam", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmxGetRunItemParam([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] int ParamIdx, [MarshalAs(UnmanagedType.I4)] ref int ParamData);

        // 12. cmmLmxGetRunItemStaPos
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmxGetRunItemStaPos", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmxGetRunItemStaPos([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] ref double Position);

        // 13. cmmLmxGetRunItemTargPos
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmxGetRunItemTargPos", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmxGetRunItemTargPos([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] int Axis, [MarshalAs(UnmanagedType.R8)] ref double Position);

        // 14. cmmLmxGetSts
        [DllImport("Cmmsdk.dll", EntryPoint = "cmmLmxGetSts", ExactSpelling = true, CharSet = CharSet.Ansi/*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern unsafe int cmmLmxGetSts([MarshalAs(UnmanagedType.I4)] int LmIndex, [MarshalAs(UnmanagedType.I4)] int LmxStsId, [MarshalAs(UnmanagedType.I4)] ref int LmxStsVal);
    }
}

