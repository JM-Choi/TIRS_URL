using System;
using System.Text;

namespace Shared
{
    public sealed class Defines
    {
        // Device ID definition //
        public const int COMI_SD401	                = 0xB401;
        public const int COMI_SD402                 = 0xB402;
        public const int COMI_SD403	                = 0xB403;
        public const int COMI_SD404	                = 0xB404;
        public const int COMI_SD414	                = 0xB414;
        public const int COMI_SD424	                = 0xB424;
        public const int COMI_LX501	                = 0xA501;
        public const int COMI_LX502	                = 0xA502;
        public const int COMI_LX504	                = 0xA504;
        public const int COMI_LX508	                = 0xA508;
        public const int COMI_LX534	                = 0xA534;
        public const int COMI_LX544	                = 0xA544;
        public const int COMI_LX504A                = 0xA544;
        
        // Definition of maximum number of things //
		public const int CMM_MAX_NUM_MOT_DEV		= 16; // Maximum number of Motion Devices in one PC
		public const int CMM_MAX_NUM_DIO_DEV		= 16; // Maximum number of Digital I/O Devices in one PC
		public const int CMM_MAX_NUM_AXES			= 64; // Maximum number of motion axes
		public const int CMM_MAX_DI_CH				= 512; // Maximum number of D/I channels
		public const int CMM_MAX_DO_CH				= 512; // Maximum number of D/O channels
		public const int CMM_MAX_STR_LEN_AXIS_TITLE	= 30; // Maximum string length of axis title
		public const int CMM_MAX_STR_LEN_DIST_UNIT	= 30; // Maximum string length of distance unit
		public const int CMM_MAX_STR_LEN_VEL_UNIT	= 30; // Maximum string length of velocity unit
		public const int CMM_MAX_STR_LEN_ERR		= 128;  // Maximum error string length: a buffer to receive error string must be larger than this size.

        // Error code definitions by OYS 2006/08/03
        //***********************************************************************************************
        //									ERROR CODE DEFINITIONs										*
        //***********************************************************************************************
        public const int cmERR_NONE					= 0;
        public const int cmERR_MEM_ALLOC_FAIL		= -290;	// Memory allocation fail
        public const int cmERR_GLOBAL_MEM_FAIL		= -292;	// Global memory allocation fail
        public const int cmERR_ISR_CONNEC_FAIL 		= -310;	// ISR registration fail
        public const int cmERR_DIVIDE_BY_ZERO		= -400;	// Cause divide by zero error 
        public const int cmERR_WORNG_NUM_DATA		= -500;	// Number of data is too small or too big
        public const int cmERR_VER_MISMATCH			= -600;	// Version(of file or device) mismatch

        public const int cmERR_INVALID_DEVICE_ID	= -1010; // Invalid device id => Load Device 또는 SetDeviceId()에서...
        public const int cmERR_INVALID_HANDLE		= -1020;
        public const int cmERR_UNSUPORTED_FUNC		= -1030;
        public const int cmERR_INVALID_PARAMETER	= -1101;
        public const int cmERR_INVALID_CHANNEL		= -1105;
        public const int cmERR_INVALID_INPUT_RANGE	= -1111; // Invalid range value (AI, AO)
        public const int cmERR_INVALID_FREQ_RANGE	= -1121; // Invalid input or output frequency
        public const int cmERR_FILE_CREATE_FAIL	    = -1501; // File create fail 
        public const int cmERR_FILE_OPEN_FAIL		= -1511; // File open fail
        public const int cmERR_EVENT_CREATE_FAIL	= -1550; // Event handle creation fail			
        public const int cmERR_INT_INSTANCE_FAIL	= -1560; // Interrupt event instance creation fail
        public const int cmERR_DITHREAD_CRE		    = -1570; // D/I state change monitor thread creation fail
        public const int cmERR_BUFFER_SMALL			= -1580; // Buffer size is too small

        public const int cmERR_ON_MOTION			= -5001;
        public const int cmERR_STOP_BY_SLP		    = -5002; // Abnormally stopped by positive soft limit
        public const int cmERR_STOP_BY_SLN		    = -5003; // Abnormally stopped by negative soft limit
        public const int cmERR_STOP_BY_CMP3		    = -5004; // Abnormally stopped by comparator3
        public const int cmERR_STOP_BY_CMP4		    = -5005; // Abnormally stopped by comparator4
        public const int cmERR_STOP_BY_CMP5		    = -5006; // Abnormally stopped by comparator5
        public const int cmERR_STOP_BY_ELP		    = -5007; // Abnormally stopped by (+) external limit
        public const int cmERR_STOP_BY_ELN		    = -5008; // Abnormally stopped by (-) external limit
        public const int cmERR_STOP_BY_ALM		    = -5009; // Abnormally stopped by alarm input signal
        public const int cmERR_STOP_BY_CSTP		    = -5010; // Abnormally stopped by CSTP input signal
        public const int cmERR_STOP_BY_CEMG		    = -5011; // Abnormally stopped by CEMG input signal
        public const int cmERR_STOP_BY_SD		    = -5012; // Abnormally stopped by SD input signal
        public const int cmERR_STOP_BY_DERROR	    = -5013; // Abnormally stopped by operation data error
        public const int cmERR_STOP_BY_IP		    = -5014; // Abnormally stopped by other axis error during interpolation
        public const int cmERR_STOP_BY_PO		    = -5015; // An overflow occurred in the PA/PB input buffer
        public const int cmERR_STOP_BY_AO		    = -5016; // Out of range position counter during interpolation
        public const int cmERR_STOP_BY_EE		    = -5017; // An EA/EB input error occurred (does not stop)
        public const int cmERR_STOP_BY_PE		    = -5018; // An PA/PB input error occurred (does not stop)
        public const int cmERR_STOP_BY_SLVERR	    = -5019; // Abnormally stopped because slave axis has been stopped
        public const int cmERR_STOP_BY_SEMG		    = -5020; // Abnormally stopped by software emergency setting

        public const int cmERR_MOT_MAOMODE		    = -5110; // Master output mode is not CW/CCW mode // Master/Slave 동작시에 Master output모드가 CW/CCW모드가 아니다.
        public const int cmERR_MOT_SLAVE_SET		= -5120; // Slave start fail (Motion state가 "Wait for Pulsar Input"으로 변하지 않는다.
        public const int cmERR_SPEED_RANGE_OVER	    = -5130;
        public const int cmERR_INVALID_SPEED_SET	= -5140; // Speed setting value is not valid
        public const int cmERR_INVALID_IXMAP		= -5150; // Invalid interpolation map index
        public const int cmERR_INVALID_LMMAP		= -5160; // Invalid List-Motion Map 
        public const int cmERR_MOT_SEQ_SKIPPED		= -5170; // Motion command is skipped because the axis is already running.  
        public const int cmERR_CMPIX_INVALID_MAP	= -5180; // Interpolated position compare output map is not valid
        public const int cmERR_INVALID_ARC_POS		= -5190; // Position data for circular interpolation is invalid
        public const int cmERR_LMX_ADD_ITEM_FAIL	= -5200; // failed to add an job item to "extend list motion"
        public const int cmERR_LMX_IS_NOT_ACTIVE    = -5300; // Extended ListMotion' is not active.
        public const int cmERR_UNKNOWN              = -9999;

        // Motion Chip Main-space Address //
        public enum TMCWAddr { COMW, OTPW, BUF0, BUF1 };
        public enum TMCRAddr { MSTSW, SSTSW };

        // Motion Chip Registers //
        public enum TMCRegister {
            PRMV, PRFL, PRFH, PRUR, PRDR, PRMG, PRDP, PRMD, PRIP, PRUS, PRDS, PRCP5, PRCI, 
	        RMV=16, RFL, RFH, RUR, RDR, RMG, RDP, RMD, RIP, RUS, RDS, RFA, RENV1, RENV2, RENV3, RENV4, RENV5, RENV6, RENV7, 
	        RCUN1=35, RCUN2, RCUN3, RCUN4, RCMP1, RCMP2, RCMP3, RCMP4, RCMP5, RIRQ, RLTC1, RLTC2, RLTC3, RLTC4, 
	        RSTS=49, REST, RIST, RPLS, RSPD, PSDC, 
	        RCI=60, RCIC, 
	        RIPS=63
        };
        
        // 0. Common
        public enum MASKBIT { LOWLIMIT = 31, HIGHLIMIT = 63 }; // cmmIxMapAxes 의 MapMask 구분(하위32비트 and 상위 32비트)
        public enum PGTYPE { POSITION, VELOCITY }; // MotionStatus 창에서 Vertical Progress 의 타입.

        //public const int CMD_THREAD_INTERVAL1 = 1;
        public const int CMD_THREAD_INTERVAL0 = 0;
        public const int CMD_THREAD_INTERVAL100 = 100;

        // 1. Single Axis Move 관련
        // Interval 변수
        public const int MIN_INT = 0;
        public const int INIT_INT = 1;

        public const int MIN_CHANNEL = 0;
        public const int INIT_CHANNEL = 0;

        // Distance numeric 변수
        public const int MIN_DIST = -99999999;
        public const int MAX_DIST = 99999999;
        public const int INIT_DIST0 = 0;
        public const int INIT_DIST10000 = 10000;

        public const int MIN_WORKSPD = 0;
        public const int INIT_WORKSPD10000 = 10000;
        public const int MAX_WORKSPD = 99999999;

        // MAP
        public enum MAP { MAP0, MAP1, MAP2, MAP3, MAP4, MAP5, MAP6, MAP7 };

        public enum SPDMODE { MODE_CONST, MODE_TRPZDL, MODE_SCURVE };
        public enum OPMODE { MODE_VEL, MODE_RELPOS, MODE_ABSPOS };
        public enum DIR { MODE_CW, MODE_CCW };
        public enum MOTIONTYPE
        {
            SXMOVE, MXMOVE, IXMOVE, HOMERETURN, MANUALPULSAR, EXTERNALSWITCH,
            POSITIONCOMPARE, LISTEDMOTION, NONE
        };

        public enum SERVO { NOTDEFINED=-100, SERVOFAULT = -1, SERVOOFF, SERVOON };

        // Motion Status
        public enum RESET_AXIS { RESETDONE = -1, FIRSTCH, SECONDCH, THIRDCH, FORTHCH, RESETALL = 99 };

        // Master/Slave
        public enum MP_OPMODE { HOME_MOVE, VEL_MOVE, REL_POS, ABS_POS };
        public enum HOME_TYPE { COMMAND, ORG };
        public enum SLAVE_STATE { ERROR = -1, UNREGISTERED, REGISTERED };

        public enum EXSW_OPMODE { VEL_MOVE, REL_POS };

        // position compare
        public enum CMP_PNTMODE { ONE_PNT, REGULAR_PNT, VOLUNTARY_PNT };

        // 2. Multi Axis Move 관련

        // 3. Operation Form 관련
        public enum OPGROUPBARITEM
        {
            GBISXMOVE, GBIMXMOVE, GBIHOMERETURN, GBIIXMOVE, GBIMANPUL,
            GBIEXTSW, GBILSTMOTION, GBIPOSCOMP
        };

        // 4. Listed Motion
        public enum LM_OPIDX
        {
            CAT_SX, SX_SETSPEEDPTN, SX_MOVE, SX_MOVETO = 3,
            CAT_IX, IX_MAPAXES, IX_SETSPEEDPTN, IX_SETSPEEDPTNV, IX_LINE, IX_LINETO, IX_ARCA, IX_ARCP, IX_ARCATO, IX_ARCPTO = 13,
            CAT_ONLY4MACROMODE, DO_PUTONE, DO_PUTALL, DELAY = 17
        };
            
        public const int MAX_CMDLIST = 999;


		//----- Definitions for Motion -------------------------------------------//

		public enum _Direction {CMC_DIR_N, CMC_DIR_P}; // Direction

		public enum _TMioProperty 
		{
			MioALM_Logic, MioALM_Mode, MioCMP_Logic, MioDR_Logic, 
			MioEL_Logic, MioEL_Mode, MioERC_Logic, MioERC_OutAtHome, MioEZ_Logic, 
			MioINP_Enable, MioINP_Logic, MioLTC_Logic, MioLTC_Ltc2Src, MioORG_Logic, 
			MioSD_Enable, MioSD_Logic, MioSD_Latch, MioSD_Mode, MioSTA_Mode, MioSTA_TrgType, 
			MioSTP_Mode, /* End of 1bit values */ 
			MioCLR_CntrSel, MioCLR_SigType, MioCMP_PulseWidth, MioERC_OnTime
		};

		public enum _TCmMioPropId 
		{
			cmALM_LOGIC, cmALM_MODE, cmCMP_LOGIC, cmDR_LOGIC, cmEL_LOGIC, cmEL_MODE, 
			cmERC_LOGIC, cmERC_OUT, cmEZ_LOGIC, cmINP_EN, cmINP_LOGIC, cmLTC_LOGIC, 
			cmLTC_LTC2SRC, cmORG_LOGIC, cmSD_EN, cmSD_LOGIC, cmSD_LATCH, cmSD_MODE, cmSTA_MODE,
			cmSTA_TRG, cmSTP_MODE, cmCLR_CNTR, cmCLR_SIGTYPE, cmCMP_PWIDTH, cmERC_ONTIME
		};

        public enum _TCmMioPropIdEx
        {
            cmMPID_ALM_LOGIC, cmMPID_ALM_MODE, cmMPID_CMP_LOGIC, cmMPID_DR_LOGIC, cmMPID_EL_LOGIC, cmMPID_EL_MODE,
            cmMPID_ERC_LOGIC, cmMPID_ERC_OUT, cmMPID_EZ_LOGIC, cmMPID_INP_EN, cmMPID_INP_LOGIC, cmMPID_LTC_LOGIC,
            cmMPID_LTC_LTC2SRC, cmMPID_ORG_LOGIC, cmMPID_SD_EN, cmMPID_SD_LOGIC, cmMPID_SD_LATCH, cmMPID_SD_MODE,
            cmMPID_STA_MODE, cmMPID_STA_TRG, cmMPID_STP_MODE, cmMPID_CLR_CNTR, cmMPID_CLR_SIGTYPE, cmMPID_CMP_PWIDTH,
            cmMPID_ERC_ONTIME, cmMPID_SVON_LOGIC, cmMPID_ERC_OUT_EL, cmMPID_CNT_D_SRC, cmMPID_CNT_G_SRC, cmMPID_HOME_ESC_DIS,
            cmMPID_LTC_TRGSRC, cmMPID_EN_ARDP
        };

		public enum _TCmMioState
		{
			cmIOST_RDY, cmIOST_ALM, cmIOST_ELP, cmIOST_ELN, cmIOST_ORG, 
			cmIOST_DIR, cmIOST_RSV1, cmIOST_PCS, cmIOST_ERC, cmIOST_EZ, 
			cmIOST_CLR, cmIOST_LTC,	cmIOST_SD, cmIOST_INP, cmIOST_DRP,
			cmIOST_DRN, cmIOST_STA, cmIOST_STP, cmIOST_RSV2 = 18, cmIOST_RSV3
		};

		public enum _TCmSigLogic{ cmLOGIC_A=0, cmLOGIC_B=1 };
		public enum _TCmBool { cmFALSE, cmTRUE };
		public enum _TCmAxis{ cmX1, cmY1, cmZ1, cmU1, cmX2, cmY2, cmZ2, cmU2 };
		public enum _TCmAxisMask
		{
            cmLY_MASK = 0x1, cmLZ_MASK = 0x2, cmLPX_MASK = 0x4, cmLPY_MASK = 0x8,
            cmLPZ_MASK = 0x10, cmLTX_MASK = 0x20, cmLTY_MASK = 0x40, cmUY_MASK = 0x80,
            cmUZ_MASK = 0x100, cmUPX_MASK = 0x200, cmUPZ_MASK = 0x400, cmUTX_MASK = 0x800,
            cmUTY_MASK = 0x1000, cmDX_MASK = 0x2000, cmDY_MASK = 0x4000, cmDZ_MASK = 0x8000
        };
		public enum _TCmInMode{ cmIMODE_AB1X, cmIMODE_AB2X, cmIMODE_AB4X, cmIMODE_CWCCW};
		public enum _TCmOutMode
		{
			cmOMODE_PDIR0, cmOMODE_PDIR1, cmOMODE_PDIR2, cmOMODE_PDIR3,
			cmOMODE_CWCCW0, cmOMODE_CWCCW1};
		public enum _TCmSpeedMode{ cmSMODE_KEEP=-1/* Keep previous setting*/, cmSMODE_C=0, cmSMODE_T, cmSMODE_S };
		public enum _TCmDir{ cmDIR_N, cmDIR_P};
		public enum _TCmArcDir{ cmARC_CW, cmARC_CCW};
		public enum _TCmCntr 
		{
			cmCNT_COMM/*Command*/, cmCNT_FEED/*Feedback*/, cmCNT_DEV/*Deviation*/, 
			cmCNT_GEN/*General*/, cmCNT_REM/*Remained*/};

		public enum _TCmMotionState
		{ 
			cmMST_STOP, cmMST_WAIT_DR, cmMST_WAIT_STA, cmMST_WAIT_INSYNC,
			cmMST_WAIT_OTHER, cmMST_WAIT_ERC, cmMST_WAIT_DIR, cmMST_RESERVED1, cmMST_WAIT_PLSR,
			cmMST_IN_RVSSPD, cmMST_IN_INISPD, cmMST_IN_ACC, cmMST_IN_WORKSPD, cmMST_IN_DEC,
			cmMST_WAIT_INP, cmMST_SPARE0
		};

		//------------------------------------------------------------------------//
		// Compare Method //
		public enum _TCmCmpMethod
		{
			cmDISABLE, cmEQ_BIDIR, cmEQ_PDIR, cmEQ_NDIR, cmLESS/*Cnt<Data*/, cmGREATER/*Cnt>Data*/
		};

		// Action when general comparator met the condition //
		public enum _TCmCmpAction
		{
			cmEVNT_ONLY, cmEVNT_IS, cmEVNT_DS, cmEVNT_SPDCHG
		};

		// Backlash/Slip correction mode //
		public enum _TCmCorrMode
		{
			cmCORR_DIS, // Disable correction 
			cmCORR_BACK, // Backlash correction mode 
			cmCORR_SLIP // Slip correction mode
		};

		public enum _TCmExtOptionId
		{
			cmEXOPT_SET_USE_PREREG
		};

		// Interrupt Handler Type //
		public enum _TCmIntHandlerType
		{
			cmIHT_MESSAGE, cmIHT_EVENT, cmIHT_CALLBACK
		};

		// Interrupt Handler Type //
		public enum _TCmStringID
		{
			cmSTR_AXIS_NAME, cmSTR_DIST_UNIT, cmSTR_SPEED_UNIT
		};
        
		public enum _TComiVarType{VT_SHORT, VT_FLOAT, VT_DOUBLE};

		// Interrupt Handler Type //
		public enum TCmIntHandlerType
		{
			cmIHT_MESSAGE, cmIHT_EVENT, cmIHT_CALLBACK
		};

        // SetFilterAB의 대상 //
        public enum _TCmABFilter { cmAB_ENC, cmAB_PULSAR };

        // TCmLmxStsId: cmmLmxGetSts() 함수를 통해서 읽어올 status ID 값의 정의
        public enum _TCmLmxStsId
        {
            cmLMX_STARTED, // Lmx 기능이 활성화 되었는지를 나타내는 status 
            cmLMX_BUSY, // Lmx가 현재 실제로 이송을 진행 중인지를 나타내는 status
            cmLMX_FREE_SPACE, // Lmx 버퍼의 여유 공간. 반환되는 값은 바이트 단위가 아니라 등록할 수 있는 아이템 수이다.
            cmLMX_RUN_ITEM_NO, // 현재 이송되고 있거나 마지막 이송된 Item의 번호.
            cmLMX_RUN_ITEM_ID, // 현재 이송되고 있거나 마지막 이송된 Item의 아이디값(아이디는 cmmLmxSetNextItemId() 함수를 통해서 설정한다)
            cmLMX_LAST_SET_ITEM_ID, // 마지막으로 예약된 Item의 아이디값.
            //cmLMX_LAST_SET_ITEM_NO  // 마지막으로 예약된 Item의 아이디값[향후에 추가하자].
        };

        ////////////////////////////////////////////////////////////////////////////////////////////
        // TCmLmxSeqMode: 이송명령을 예약하려 하는데 LMX Buffer가 이미 꽉 차있는 경우에 어떻게 처리
        // 할지에 대한 모드의 아이디.
        // -. cmLMX_SEQM_SKIP_RUN: 'cmERR_LMX_ADD_ITEM_FAIL'에러를 발생하고 함수는 바로 반환된다.
        // -. cmLMX_SEQM_WAIT_RUN: LMX 버퍼에 free space가 생길 때까지 대기하고 있다가 free space가
        //    생기면 예약하고 함수가 반환된다.
        public enum _TCmLmxSeqMode
        {
            cmLMX_SEQM_SKIP_RUN,
            cmLMX_SEQM_WAIT_RUN
        };
    }
}
