using System;
using System.Runtime.InteropServices;

namespace ComiDll
{
	/// <summary>
	/// ImportComiDasDLL에 대한 요약 설명입니다.
	/// </summary>
    
    [System.Security.SuppressUnmanagedCodeSecurity]
    internal unsafe class SafeNativeMethods
    {
        [StructLayout(LayoutKind.Sequential/*, Pack=1*/)]
        internal struct ScanData
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            internal float[] fData;
        }
        
        // COMI-DAQ Device ID
        internal enum TCmDeviceID
        {
            // CP-Seriese
	        CP101 = 0xC101, 
            CP201 = 0xC201, 
            CP301 = 0xC301, 
            CP302 = 0xC302, 
            CP401 = 0xC401, 
            CP501 = 0xC501, 
            SD101 = 0xB101,
	        // SD-Seriese
	        SD102 = 0xB102, 
            SD103 = 0xB103, 
            SD104 = 0xB104, 
            SD201 = 0xB201, 
            SD202 = 0xB202, 
            SD203 = 0xB203, 
            SD301 = 0xB301,
            SD302 = 0xB302,
	        SD401 = 0xB401, 
            SD402 = 0xB402, 
            SD403 = 0xB403, 
            SD404 = 0xB404,
            SD414 = 0xB414,
            SD424 = 0xB424, 
			SD434 = 0xB434, 
            SD501 = 0xB501, 
            SD502 = 0xB502, 
            LX101 = 0xA101,
	        // LX-Seriese
	        LX102 = 0xA102, 
            LX103 = 0xA103, 
            LX201 = 0xA201, 
            LX202 = 0xA202, 
            LX203 = 0xA203, 
            LX301 = 0xA301, 
            LX401 = 0xA401,
	        LX402 = 0xA402,
        }


        internal enum TCdAiScanTrs
        {
            cmTRS_SINGLE = 1,
            cmTRS_BLOCK = 2,
            cmTRS_BLOCK_EXT = 3
        }

        internal enum TCdVarType
        {
            VT_SHORT = 0, VT_FLOAT, VT_DOUBLE
        }

        internal struct TComiDevInfo
        {
            internal ushort wSubSysID;
            internal uint nInstance;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            internal char[] szDevName;
            internal byte bDevCaps;
            internal byte nNumAdChan, nNumDaChan, nNumDiChan, nNumDoChan, nNumCntrChan;
        }

        internal struct TComiDevList
        {
            internal ushort nNumDev;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            internal TComiDevInfo[] DevInfo;
        }

        internal struct TScanFileHead
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
            internal String szDate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            internal String szTime;

            internal int nNumChan;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            internal int[] nChanList;

            internal int dmin, dmax;


            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            internal float[] vmin;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            internal float[] vmax;

            internal uint dwSavedScanCnt;
        }

        internal struct TPidParams
        {
            internal float Ref, lim_h, lim_l;
            internal float Kp;
            internal float Td, Ti;
            internal int ch_ref, ch_ad, ch_da;
        }

        internal struct THelicalUserInfo
        {
            internal int c_map, z_axis;
            internal double c_xcen, c_ycen;
            internal int c_dir;
            internal int c_num;
            internal double c_la;
            internal double z_dist;
        }

        internal delegate void EventFunc(IntPtr lParam);

    

 //__________ General Functions ________________________________________________//
 
 //COMIDAS_DEVID deviceID
        [DllImport("Comidll.dll", EntryPoint = "COMI_LoadDevice", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe IntPtr	COMI_LoadDevice([MarshalAs(UnmanagedType.I4)] int deviceID, [MarshalAs(UnmanagedType.I4)] int instance);

        [DllImport("Comidll.dll", EntryPoint = "COMI_UnloadDevice", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe void	COMI_UnloadDevice(IntPtr hDevice /*HANDLE*/);
        
// TComiDevList
		[DllImport("Comidll.dll", EntryPoint = "COMI_GetAvailDevList", CallingConvention=CallingConvention.Cdecl)]
        internal static extern unsafe bool COMI_GetAvailDevList(ref TComiDevList pDevList);
        
// TComiDevInfo
        [DllImport("Comidll.dll", EntryPoint = "COMI_GetDevInfo", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe bool COMI_GetDevInfo(IntPtr hDevice /*HANDLE*/, ref TComiDevInfo pDevInfo);

        [DllImport("Comidll.dll", EntryPoint = "COMI_Write8402", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe void	COMI_Write8402(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch, [MarshalAs(UnmanagedType.I4)] int  addr, [MarshalAs(UnmanagedType.I4)] int  data);

        [DllImport("Comidll.dll", EntryPoint = "COMI_WriteEEPR", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe void	COMI_WriteEEPR(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  addr, [MarshalAs(UnmanagedType.I4)] int  data);

        [DllImport("Comidll.dll", EntryPoint = "COMI_ReadEEPR", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int		COMI_ReadEEPR(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  addr);
 
 //__________ A/D General Functions ________________________________________________//

        [DllImport("Comidll.dll", EntryPoint = "COMI_AD_SetRange", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe bool	COMI_AD_SetRange(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch, [MarshalAs(UnmanagedType.R4)] float vmin, [MarshalAs(UnmanagedType.R4)] float vmax);

        [DllImport("Comidll.dll", EntryPoint = "COMI_AD_GetDigit", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe short	COMI_AD_GetDigit(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);

        [DllImport("Comidll.dll", EntryPoint = "COMI_AD_GetVolt", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe float	COMI_AD_GetVolt(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
 
 //__________ A/D Unlimited Scan Functions _________________________________//
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_Start")]
		internal static extern unsafe int	COMI_US_Start(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  numCh, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] chanList, [MarshalAs(UnmanagedType.U4)] uint scanFreq, [MarshalAs(UnmanagedType.U4)] uint msb, [MarshalAs(UnmanagedType.I4)] int  trsMethod);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_StartEx")]
		internal static extern unsafe int	COMI_US_StartEx(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.U4)] uint dwScanFreq, [MarshalAs(UnmanagedType.U4)] uint nFrameSize, [MarshalAs(UnmanagedType.U4)] uint nBufSizeGain);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_Stop")]
		internal static extern unsafe bool	COMI_US_Stop(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.Bool)] bool bReleaseBuf);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_SetPauseAtFull")]
		internal static extern unsafe bool	COMI_US_SetPauseAtFull(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.Bool)] bool bPauseAtFull);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_Resume")]
		internal static extern unsafe bool	COMI_US_Resume(IntPtr hDevice /*HANDLE*/);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_ChangeScanFreq")]
		internal static extern unsafe int	COMI_US_ChangeScanFreq(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.U4)] uint dwScanFreq);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_ResetCount")]
		internal static extern unsafe void	COMI_US_ResetCount(IntPtr hDevice /*HANDLE*/);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_ChangeSampleFreq")]
		internal static extern unsafe void	COMI_US_ChangeSampleFreq(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.U4)] uint dwSampleFreq);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_CurCount")]
		internal static extern unsafe uint	COMI_US_CurCount(IntPtr hDevice /*HANDLE*/);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_SBPos")]
		internal static extern unsafe uint	COMI_US_SBPos(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  chOrder, [MarshalAs(UnmanagedType.U4)] uint scanCount);

// short*        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_GetBufPtr")]
		internal static extern unsafe short[]	COMI_US_GetBufPtr(IntPtr hDevice /*HANDLE*/);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_ReleaseBuf")]
		internal static extern unsafe bool	COMI_US_ReleaseBuf(IntPtr hDevice /*HANDLE*/);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_RetrvOne")]
		internal static extern unsafe short	COMI_US_RetrvOne(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  chOrder, [MarshalAs(UnmanagedType.U4)] uint scanCount);

//TCOmiVarType VarType        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_RetrvChannel")]
        internal static extern unsafe uint COMI_US_RetrvChannel(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int chOrder, [MarshalAs(UnmanagedType.U4)] uint startCount, [MarshalAs(UnmanagedType.I4)] int maxNumData, IntPtr Buffer, [MarshalAs(UnmanagedType.I4)] int VarType);

//TCOmiVarType VarType  
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_RetrvBlock")]
        internal static extern unsafe uint COMI_US_RetrvBlock(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int startCount, [MarshalAs(UnmanagedType.I4)] int maxNumScan, [MarshalAs(UnmanagedType.I4)] IntPtr Buffer, [MarshalAs(UnmanagedType.I4)] int VarType);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_FileSaveFirst")]
		internal static extern unsafe bool	COMI_US_FileSaveFirst(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.LPWStr)] string szFilePath, [MarshalAs(UnmanagedType.Bool)] bool bIsFromStart);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_FileSaveNext")]
		internal static extern unsafe uint	COMI_US_FileSaveNext(IntPtr hDevice /*HANDLE*/);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_FileSaveStop")]
		internal static extern unsafe bool	COMI_US_FileSaveStop(IntPtr hDevice /*HANDLE*/);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_FileConvert")]
		internal static extern unsafe void	COMI_US_FileConvert([MarshalAs(UnmanagedType.LPWStr)] string szBinFilePath, [MarshalAs(UnmanagedType.LPWStr)] string szTextFilePath, [MarshalAs(UnmanagedType.U4)] uint nMaxDataRow);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_CheckFileConvert")]
		internal static extern unsafe double	COMI_US_CheckFileConvert();
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_US_CancelFileConvert")]
		internal static extern unsafe void	COMI_US_CancelFileConvert();
 
 //___________ PID Functions _______________________________________________//
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_PID_Enable")]
		internal static extern unsafe bool	COMI_PID_Enable(IntPtr hDevice /*HANDLE*/); 

// TPidParams pPidParams       
        //[DllImport("Comidll.dll", EntryPoint = "COMI_PID_SetParams")]
        //internal static extern unsafe bool	COMI_PID_SetParams(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  nNumCtrls, ref TPidParams pPidParams); 
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_PID_Disable")]
		internal static extern unsafe bool	COMI_PID_Disable(IntPtr hDevice /*HANDLE*/); 
 
 //___________ DIO Common __________________________________________________//

        [DllImport("Comidll.dll", EntryPoint = "COMI_DIO_SetUsage", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe void	COMI_DIO_SetUsage(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  usage);

        [DllImport("Comidll.dll", EntryPoint = "COMI_DIO_GetUsage", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int		COMI_DIO_GetUsage(IntPtr hDevice /*HANDLE*/);
 
 //__________ D/I Functions ________________________________________________//

        [DllImport("Comidll.dll", EntryPoint = "COMI_DI_GetOne", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int		COMI_DI_GetOne(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);

        [DllImport("Comidll.dll", EntryPoint = "COMI_DI_GetAll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int	COMI_DI_GetAll(IntPtr hDevice /*HANDLE*/);

        [DllImport("Comidll.dll", EntryPoint = "COMI_DI_GetAllEx", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int	COMI_DI_GetAllEx(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  nGroupIdx);
 
 //__________ D/O Functions ________________________________________________//

        [DllImport("Comidll.dll", EntryPoint = "COMI_DO_PutOne", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe bool	COMI_DO_PutOne(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch, [MarshalAs(UnmanagedType.I4)] int  status);

        [DllImport("Comidll.dll", EntryPoint = "COMI_DO_PutAll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe bool	COMI_DO_PutAll(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.U4)] uint dwStatuses);

        [DllImport("Comidll.dll", EntryPoint = "COMI_DO_PutAllEx", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe void	COMI_DO_PutAllEx(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  nGroupIdx, [MarshalAs(UnmanagedType.I4)] int dwStatuses);

        [DllImport("Comidll.dll", EntryPoint = "COMI_DO_GetOne", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int COMI_DO_GetOne(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_DO_GetAll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int	COMI_DO_GetAll(IntPtr hDevice /*HANDLE*/);

        [DllImport("Comidll.dll", EntryPoint = "COMI_DO_GetAllEx", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int	COMI_DO_GetAllEx(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  nGroupIdx);
 
 //__________ D/A Functions ________________________________________________//

        [DllImport("Comidll.dll", EntryPoint = "COMI_DA_Out", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe bool	COMI_DA_Out(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch, [MarshalAs(UnmanagedType.R4)] float volt);

        [DllImport("Comidll.dll", EntryPoint = "COMI_DA_SetRange", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe void	COMI_DA_SetRange(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch, [MarshalAs(UnmanagedType.I4)] int  VMin, [MarshalAs(UnmanagedType.I4)] int  VMax);

        [DllImport("Comidll.dll", EntryPoint = "COMI_WFM_Start", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int	COMI_WFM_Start(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] ref double Buffer, [MarshalAs(UnmanagedType.U4)] uint nNumData, [MarshalAs(UnmanagedType.U4)] uint nPPS, [MarshalAs(UnmanagedType.I4)] int  nMaxLoops);

        [DllImport("Comidll.dll", EntryPoint = "COMI_WFM_RateChange", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int	COMI_WFM_RateChange(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch, [MarshalAs(UnmanagedType.I4)] int nPPS);

        [DllImport("Comidll.dll", EntryPoint = "COMI_WFM_GetCurPos", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int	COMI_WFM_GetCurPos(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_WFM_GetCurLoops")]
		internal static extern unsafe int	COMI_WFM_GetCurLoops(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_WFM_Stop")]
		internal static extern unsafe void	COMI_WFM_Stop(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
 
 //__________ Counter Functions ____________________________________________//

        [DllImport("Comidll.dll", EntryPoint = "COMI_SetCounter", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe void	COMI_SetCounter(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch, [MarshalAs(UnmanagedType.I4)] int  rw, [MarshalAs(UnmanagedType.I4)] int  op, [MarshalAs(UnmanagedType.I4)] int  bcd_bin, [MarshalAs(UnmanagedType.U4)] uint load_value);

        [DllImport("Comidll.dll", EntryPoint = "COMI_LoadCount", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe void	COMI_LoadCount(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch, [MarshalAs(UnmanagedType.U4)] uint load_value);

        [DllImport("Comidll.dll", EntryPoint = "COMI_ReadCount", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe ushort	COMI_ReadCount(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);

        [DllImport("Comidll.dll", EntryPoint = "COMI_ReadCounter32", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe uint 	COMI_ReadCounter32(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);

        [DllImport("Comidll.dll", EntryPoint = "COMI_ClearCounter32", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe void 	COMI_ClearCounter32(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);

        [DllImport("Comidll.dll", EntryPoint = "COMI_FC_SelectGate", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe void	COMI_FC_SelectGate(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch, [MarshalAs(UnmanagedType.I4)] int  nGateIndex);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_FC_GateTime")]
		internal static extern unsafe uint	COMI_FC_GateTime(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_FC_ReadCount")]
		internal static extern unsafe uint	COMI_FC_ReadCount(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_FC_ReadFreq")]
		internal static extern unsafe uint	COMI_FC_ReadFreq(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
 
 
		[DllImport("Comidll.dll", EntryPoint = "COMI_ENC_Config")]
		internal static extern unsafe void	COMI_ENC_Config(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch, [MarshalAs(UnmanagedType.I4)] int  mode, [MarshalAs(UnmanagedType.Bool)] bool  bResetByZ);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_ENC_Reset")]
		internal static extern unsafe void	COMI_ENC_Reset(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_ENC_Load")]
		internal static extern unsafe void	COMI_ENC_Load(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch, [MarshalAs(UnmanagedType.I4)] int Count);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_ENC_Read")]
		internal static extern unsafe int	COMI_ENC_Read(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_ENC_ResetZ")]
		internal static extern unsafe void	COMI_ENC_ResetZ(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_ENC_LoadZ")]
		internal static extern unsafe void	COMI_ENC_LoadZ(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch,  [MarshalAs(UnmanagedType.I4)] int  count);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_ENC_ReadZ")]
		internal static extern unsafe short	COMI_ENC_ReadZ(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_PG_Start")]
		internal static extern unsafe double	COMI_PG_Start(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch, [MarshalAs(UnmanagedType.R8)] double freq, [MarshalAs(UnmanagedType.U4)] uint num_pulses);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_PG_ChangeFreq")]
		internal static extern unsafe double	COMI_PG_ChangeFreq(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch, [MarshalAs(UnmanagedType.R8)] double freq);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_PG_IsActive")]
		internal static extern unsafe bool	COMI_PG_IsActive(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_PG_Stop")]
		internal static extern unsafe void	COMI_PG_Stop(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);

        
		[DllImport("Comidll.dll", EntryPoint = "COMI_SD502_SetCounter")]
		internal static extern unsafe void	COMI_SD502_SetCounter(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch, [MarshalAs(UnmanagedType.I4)] int  nMode, [MarshalAs(UnmanagedType.U4)] uint nClkSource);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_SD502_ReadNowCount")]
		internal static extern unsafe uint	COMI_SD502_ReadNowCount(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_SD502_ReadOldCount")]
		internal static extern unsafe uint	COMI_SD502_ReadOldCount(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_SD502_GetGateState")]
		internal static extern unsafe bool	COMI_SD502_GetGateState(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_SD502_GetClkFreq")]
		internal static extern unsafe double	COMI_SD502_GetClkFreq([MarshalAs(UnmanagedType.I4)] int nClkSrcIdx);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_SD502_Clear")]
		internal static extern unsafe void	COMI_SD502_Clear(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int  ch);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_SD502_ClearMulti")]
		internal static extern unsafe void	COMI_SD502_ClearMulti(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int dwCtrlBits);

 //__________ Utility Functions ____________________________________________//
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_DigitToVolt")]
		internal static extern unsafe double	COMI_DigitToVolt([MarshalAs(UnmanagedType.I4)] int digit, [MarshalAs(UnmanagedType.R8)] double vmin, [MarshalAs(UnmanagedType.R8)] double vmax);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_Digit14ToVolt")]
		internal static extern unsafe double	COMI_Digit14ToVolt([MarshalAs(UnmanagedType.I4)] int digit, [MarshalAs(UnmanagedType.R8)] double vmin, [MarshalAs(UnmanagedType.R8)] double vmax);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_Digit16ToVolt")]
		internal static extern unsafe double	COMI_Digit16ToVolt([MarshalAs(UnmanagedType.I4)] int digit, [MarshalAs(UnmanagedType.R8)] double vmin, [MarshalAs(UnmanagedType.R8)] double vmax);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_LastError")]
		internal static extern unsafe int		COMI_LastError();
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_ErrorString")]
		internal static extern unsafe string COMI_ErrorString([MarshalAs(UnmanagedType.I4)] int nErrCode);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_GetResources")]
		internal static extern unsafe void	COMI_GetResources(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[]  pdwIntVect, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] pdwIoPorts, [MarshalAs(UnmanagedType.I4)] int  nNumPorts, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] pdwMemPorts, [MarshalAs(UnmanagedType.I4)] int  nNumMemPorts);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_WriteIoPortDW")]
		internal static extern unsafe void	COMI_WriteIoPortDW(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.U4)] uint dwPortBase, [MarshalAs(UnmanagedType.U4)] uint nOffset, [MarshalAs(UnmanagedType.I4)] int dwOutVal);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_ReadIoPortDW")]
		internal static extern unsafe int	COMI_ReadIoPortDW(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.U4)] uint dwPortBase, [MarshalAs(UnmanagedType.U4)] uint nOffset);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_WriteMemPortDW")]
		internal static extern unsafe void	COMI_WriteMemPortDW(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.U4)] uint int dwPortBase, [MarshalAs(UnmanagedType.U4)] uint nOffset, [MarshalAs(UnmanagedType.I4)] int dwOutVal);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_ReadMemPortDW")]
		internal static extern unsafe int	COMI_ReadMemPortDW(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.U4)] uint dwPortBase, [MarshalAs(UnmanagedType.U4)] uint nOffset);
        
		[DllImport("Comidll.dll", EntryPoint = "COMI_GotoURL")]
		internal static extern unsafe void	COMI_GotoURL([MarshalAs(UnmanagedType.LPWStr)] string szUrl, [MarshalAs(UnmanagedType.I4)] int bMaximize);
		
	//___________________________________________ SD434 Functions ____________________________________________//	
		[DllImport("Comidll.dll", EntryPoint = "COMI_GetTerminal")]
		internal static extern unsafe int	COMI_GetTerminal(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int TmNum);
		
		[DllImport("Comidll.dll", EntryPoint = "COMI_INT_Start")]
		internal static extern unsafe int	COMI_INT_Start(IntPtr hDevice /*HANDLE*/);
		
		[DllImport("Comidll.dll", EntryPoint = "COMI_INT_Stop")]
		internal static extern unsafe int	COMI_INT_Stop(IntPtr hDevice /*HANDLE*/);
		
		[DllImport("Comidll.dll", EntryPoint = "COMI_INT_Clear")]
		internal static extern unsafe int	COMI_INT_Clear(IntPtr hDevice /*HANDLE*/);
		
		[DllImport("Comidll.dll", EntryPoint = "COMI_INT_SetIntChan")]
		internal static extern unsafe int	COMI_INT_SetIntChan(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int NumCh, [MarshalAs(UnmanagedType.I4)] int nState, [MarshalAs(UnmanagedType.I4)] int nMode);
		
		[DllImport("Comidll.dll", EntryPoint = "COMI_INT_GetIntChan")]
		internal static extern unsafe int	COMI_INT_GetIntChan(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int NumCh, [MarshalAs(UnmanagedType.I4)] ref int nState, [MarshalAs(UnmanagedType.I4)] ref int nMode);
		
		[DllImport("Comidll.dll", EntryPoint = "COMI_INT_GetIntState")]
		internal static extern unsafe int	COMI_INT_GetIntState(IntPtr hDevice /*HANDLE*/);
		
		[DllImport("Comidll.dll", EntryPoint = "COMI_INT_SetHandler")]
		internal static extern unsafe int	COMI_INT_SetHandler(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int bMaximize, IntPtr Handler /*HANDLE*/, [MarshalAs(UnmanagedType.U4)] uint nMessage, IntPtr lParam);
		
		[DllImport("Comidll.dll", EntryPoint = "COMI_SetFilterMode")]
		internal static extern unsafe int	COMI_SetFilterMode(IntPtr hDevice /*HANDLE*/, [MarshalAs(UnmanagedType.I4)] int nMode);
		
		[DllImport("Comidll.dll", EntryPoint = "COMI_GetFilterMode")]
		internal static extern unsafe int	COMI_GetFilterMode(IntPtr hDevice /*HANDLE*/);
    }
}

