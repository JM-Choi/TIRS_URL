// ┌--------------------------------------------------------------┐
// │           Copyright (C) 2000-2015 COMIZOA Co., LTD.          │
// └--------------------------------------------------------------┘

// DATE	: 2015-01-27

unit ComiDll;

interface

uses
	Windows,
	Messages,
	SysUtils,
	Classes,
	Graphics,
	Controls,
	Forms,
	Dialogs;

const
//******************************************************************************
//* Header file for definitions of constants and data in COMIDLL library
//* - Update Data: 2015/01/27
//* - Provider: COMIZOA Co., Ltd.
//* - Phone: +82-42-936-6500~6
//* - Fax  : +82-42-936-6507
//* - URL  : http://www.comizoa.com
//*******************************************************************************

	//*******************************************************************************

	// COMI-DAQ Device ID
	// CP-Seriese
	COMI_CP101=$C101; COMI_CP201=$C201; COMI_CP301=$C301; COMI_CP302=$C302; COMI_CP401=$C401; COMI_CP501=$C501; COMI_SD101=$B101;

	// SD-Seriese
	COMI_SD102=$B102; COMI_SD103=$B103; COMI_SD104=$B104; COMI_SD201=$B201; COMI_SD202=$B202; COMI_SD203=$B203; COMI_SD301=$B301;
	COMI_SD401=$B401; COMI_SD402=$B402; COMI_SD403=$B403; COMI_SD404=$B404; COMI_SD414=$B414; COMI_SD424=$424;
	COMI_SD501=$B501; COMI_SD502=$B502; COMI_LX101=$A101;

	// LX-Seriese
	COMI_LX102=$A102; COMI_LX103=$A103; COMI_LX201=$A201; COMI_LX202=$A202; COMI_LX203=$A203; COMI_LX301=$A301; COMI_LX401=$A401;
	COMI_LX402=$A402;

	// ST-Seriese
	COMI_ST101=$D101; COMI_ST201=$D201; COMI_ST202=$D202; COMI_ST203=$D203; COMI_ST301=$D301; COMI_ST401=$D401; COMI_ST402=$D402;

	// MU-Seriese
	COMI_MU101=$E101; COMI_MU201=$E201; COMI_MU301=$E301; COMI_MU401=$E401; COMI_MU402=$E402; COMI_MU403=$E403; COMI_MU501=$E501;
	COMI_MU701=$E701;

	// MB-Seriese
	MB_DAC101=$0101;  MB_DAC201=$0201;  MB_DAC301=$0301;  MB_DAC401=$0401;  MB_DAC501=$0501;  MB_DAC601=$0601;

	// TCdAiScanTrs
    cmTRS_SINGLE = 1;
    cmTRS_BLOCK = 2;

	// TCdVarType
	VT_SHORT = 0; VT_FLOAT = 1; VT_DOUBLE = 2;

  ERR_INVALID_DEVICE_HANDLE	= -270; // Invalid device handle is passed to a function

type
	//*******************************************************************************************************
	//								API FUNCTIONS                                                           *
	//*******************************************************************************************************
	//====================== DLL LOAD/UNLOAD FUNCTIONS ============================================//
 //__________ General Functions ________________________________________________//

TCMM_COMI_LoadDevice	= function (deviceID : LongInt; instance : LongInt) : THandle stdcall; // [fixme] THandle or PLongInt
TCMM_COMI_UnloadDevice	= function (hDevice : THandle ): LongInt; stdcall; // [fixme] THandle or PLongInt, Return value type check is needed
TCMM_COMI_GetAvailDevList	= function (pDevList : PLongInt) : LongInt; stdcall; // ref TComiDevList 부분 미확인
TCMM_COMI_GetDevInfo	= function (hDevice : THandle; pDevInfo : PLongInt) : LongInt; stdcall;
TCMM_COMI_Write8402	= function (hDevice : THandle; ch : LongInt; addr : LongInt; data : LongInt) : LongInt; stdcall;
TCMM_COMI_WriteEEPR	= function (hDevice : THandle; addr : LongInt; data : LongInt) : LongInt; stdcall;
TCMM_COMI_ReadEEPR	= function (hDevice : THandle; addr : LongInt) : LongInt; stdcall;

 //__________ A/D General Functions ________________________________________________//

TCMM_COMI_AD_SetRange	= function (hDevice : THandle; ch : LongInt; vmin : Single; vmax : Single) : LongInt; stdcall;
TCMM_COMI_AD_GetDigit	= function (hDevice : THandle; ch : LongInt) : LongInt; stdcall;
TCMM_COMI_AD_GetVolt	= function (hDevice : THandle; ch : LongInt) : LongInt; stdcall;

 //__________ A/D Unlimited Scan Functions _________________________________//

TCMM_COMI_US_Start	= function (hDevice : THandle; numCh : LongInt; chanList : PLongInt; scanFreq : LongInt; msb : LongInt; trsMethod : LongInt) : LongInt; stdcall;
TCMM_COMI_US_StartEx	= function (hDevice : THandle; dwScanFreq : LongInt; nFrameSize : LongInt; nBufSizeGain : LongInt) : LongInt; stdcall;
TCMM_COMI_US_Stop	= function (hDevice : THandle; bReleaseBuf : LongInt) : LongInt; stdcall;
TCMM_COMI_US_SetPauseAtFull	= function (hDevice : THandle; bPauseAtFull : LongInt) : LongInt; stdcall;
TCMM_COMI_US_Resume	= function (hDevice : THandle) : LongInt; stdcall;
TCMM_COMI_US_ChangeScanFreq	= function (hDevice : THandle; dwScanFreq : LongInt) : LongInt; stdcall;
TCMM_COMI_US_ResetCount	= function (hDevice : THandle) : LongInt; stdcall;
TCMM_COMI_US_ChangeSampleFreq	= function (hDevice : THandle; dwSampleFreq : LongInt ) : LongInt; stdcall;
TCMM_COMI_US_CurCount	= function (hDevice : THandle ) : LongInt; stdcall;
TCMM_COMI_US_SBPos	= function (hDevice : THandle; chOrder : LongInt; scanCount : LongInt ) : LongInt; stdcall;
TCMM_COMI_US_GetBufPtr	= function (hDevice : THandle ) : LongInt; stdcall; //short형 Word로 사용해야는지 의문.
TCMM_COMI_US_ReleaseBuf	= function (hDevice : THandle ) : LongInt; stdcall;
TCMM_COMI_US_RetrvOne	= function (hDevice : THandle; chOrder : LongInt; scanCount : LongInt) : LongInt; stdcall;

 //___________ PID Functions _______________________________________________//
TCMM_COMI_PID_Enable	= function (hDevice : THandle ) : LongInt; stdcall;

 // TPidParams pPidParams
TCMM_COMI_PID_SetParams	= function (hDevice : THandle; nNumCtrls : LongInt; pPidParams : PDouble ) : LongInt; stdcall;
TCMM_COMI_PID_Disable	= function (hDevice : THandle ) : LongInt; stdcall;

 //___________ DIO Common __________________________________________________//
TCMM_COMI_DIO_SetUsage	= function (hDevice : THandle; usage : LongInt ) : LongInt; stdcall;
TCMM_COMI_DIO_GetUsage	= function (hDevice : THandle ) : LongInt; stdcall;

 //__________ D/I Functions ________________________________________________//
TCMM_COMI_DI_GetOne	= function (hDevice : THandle; ch : LongInt ) : LongInt; stdcall;
TCMM_COMI_DI_GetAll	= function (hDevice : THandle ) : LongInt; stdcall;
TCMM_COMI_DI_GetAllEx	= function (hDevice : THandle; nGroupIdx : LongInt ) : LongInt; stdcall;

 //__________ D/O Functions ________________________________________________//
TCMM_COMI_DO_PutOne	= function (hDevice : THandle; ch : LongInt; status : LongInt) : LongInt; stdcall;
TCMM_COMI_DO_PutAll	= function (hDevice : THandle; dwStatuses : LongInt ) : LongInt; stdcall;
TCMM_COMI_DO_PutAllEx	= function (hDevice : THandle; nGroupIdx : LongInt; dwStatuses : LongInt) : LongInt; stdcall;
TCMM_COMI_DO_GetOne	= function (hDevice : THandle; ch : LongInt ) : LongInt; stdcall;
TCMM_COMI_DO_GetAll	= function (hDevice : THandle) : LongInt; stdcall;
TCMM_COMI_DO_GetAllEx	= function (hDevice : THandle; nGroupIdx : LongInt ) : LongInt; stdcall;

 //__________ D/A Functions ________________________________________________//
TCMM_COMI_DA_Out	= function (hDevice : THandle; ch : LongInt; volt : Single ) : LongInt; stdcall;
TCMM_COMI_DA_SetRanget	= function (hDevice : THandle; ch : LongInt; VMin : LongInt; VMax : LongInt ) : LongInt; stdcall;
TCMM_COMI_WFM_Start	= function (hDevice : THandle; ch : LongInt; Buffer : PDouble; nNumData : LongInt; nPPS : LongInt; nMaxLoops : LongInt ) : LongInt; stdcall;
TCMM_COMI_WFM_RateChange	= function (hDevice : THandle; ch : LongInt; nPPS : LongInt ) : LongInt; stdcall;
TCMM_COMI_WFM_GetCurPos	= function (hDevice : THandle; ch : LongInt ) : LongInt; stdcall;
TCMM_COMI_WFM_GetCurLoops	= function (hDevice : THandle; ch : LongInt ) : LongInt; stdcall;
TCMM_COMI_WFM_Stop	= function (hDevice : THandle; ch : LongInt ) : LongInt; stdcall;

 //__________ Counter Functions ____________________________________________//
TCMM_COMI_SetCounter		 = function(hDevice : THandle; ch : LongInt; rw : LongInt; op : LongInt; bcd_bin : LongInt; load_value : LongInt) : LongInt; stdcall;
TCMM_COMI_LoadCount 		 = function(hDevice : THandle; ch : LongInt; load_value : LongInt) : LongInt; stdcall;
TCMM_COMI_ReadCount		     = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;
TCMM_COMI_ReadCounter32 	 = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;
TCMM_COMI_ClearCounter32 	 = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;
TCMM_COMI_FC_SelectGate 	 = function(hDevice : THandle; ch : LongInt; nGateIndex : LongInt) : LongInt; stdcall;
TCMM_COMI_FC_GateTime 		 = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;
TCMM_COMI_FC_ReadCount 		 = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;
TCMM_COMI_FC_ReadFreq 		 = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;

TCMM_COMI_ENC_Config 		 = function(hDevice : THandle; ch : LongInt; mode : LongInt; bResetByZ : LongInt) : LongInt; stdcall;
TCMM_COMI_ENC_Reset 		 = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;
TCMM_COMI_ENC_Load 			 = function(hDevice : THandle; ch : LongInt; count : LongInt) : LongInt; stdcall;
TCMM_COMI_ENC_Read 			 = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;
TCMM_COMI_ENC_ResetZ 		 = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;
TCMM_COMI_ENC_LoadZ 		 = function(hDevice : THandle; ch : LongInt; count : LongInt) : LongInt; stdcall;
TCMM_COMI_ENC_ReadZ 		 = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;
TCMM_COMI_PG_Start		     = function(hDevice : THandle; ch : LongInt; freq : Double; ch : num_pulses) : Double; stdcall;
TCMM_COMI_PG_ChangeFreq 	 = function(hDevice : THandle; ch : LongInt; freq : Double) : Double; stdcall;
TCMM_COMI_PG_IsActive 		 = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;
TCMM_COMI_PG_Stop	         = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;

TCMM_COMI_SD502_SetCounter 	 = function(hDevice : THandle; ch : LongInt; nMode : LongInt; nClkSource : LongInt) : LongInt; stdcall;
TCMM_COMI_SD502_ReadNowCount = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;
TCMM_COMI_SD502_ReadOldCount = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;
TCMM_COMI_SD502_GetGateState = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;
TCMM_COMI_SD502_GetClkFreq 	 = function(nClkSrcIdx : LongInt) : Double; stdcall;
TCMM_COMI_SD502_Clear	     = function(hDevice : THandle; ch : LongInt) : LongInt; stdcall;
TCMM_COMI_SD502_ClearMulti 	 = function(hDevice : THandle; dwCtrlBits : LongInt) : LongInt; stdcall;

 //__________ Utility Functions ____________________________________________//
TCMM_COMI_DigitToVolt	= function (digit : LongInt; vmin : Double; vmax : Double ) : Double; stdcall;
TCMM_COMI_Digit14ToVolt	= function (digit : LongInt; vmin : Double; vmax : Double ) : Double; stdcall;
TCMM_COMI_Digit16ToVolt	= function (digit : LongInt; vmin : Double; vmax : Double ) : Double; stdcall;
TCMM_COMI_LastError	= function () : LongInt; stdcall;
TCMM_COMI_ErrorString	= function (nErrCode : LongInt ) : LongInt; stdcall; //String 처리 의문.
TCMM_COMI_WriteIoPortDW	= function (hDevice : THandle; dwPortBase : LongInt; nOffset : LongInt; dwOutVal : LongInt ) : LongInt; stdcall;
TCMM_COMI_ReadIoPortDW	= function (hDevice : THandle; dwPortBase : LongInt; nOffset : LongInt ) : LongInt; stdcall;
TCMM_COMI_ReadMemPortDW	= function (hDevice : THandle; dwPortBase : LongInt; nOffset : LongInt ) : LongInt; stdcall;








var

//====================== General FUNCTIONS ====================================================//

COMI_LoadDevice			:	TCMM_COMI_LoadDevice;
COMI_UnloadDevice		:	TCMM_COMI_UnloadDevice;
COMI_GetAvailDevList	:	TCMM_COMI_GetAvailDevList;
COMI_GetDevInfo			:	TCMM_COMI_GetDevInfo;
COMI_Write8402			:	TCMM_COMI_Write8402;
COMI_WriteEEPR			:	TCMM_COMI_WriteEEPR;
COMI_ReadEEPR			:	TCMM_COMI_ReadEEPR;
COMI_AD_SetRange		:	TCMM_COMI_AD_SetRange;
COMI_AD_GetDigit		:	TCMM_COMI_AD_GetDigit;
COMI_AD_GetVolt			:	TCMM_COMI_AD_GetVolt;
COMI_US_Start			:	TCMM_COMI_US_Start;
COMI_US_StartEx			:	TCMM_COMI_US_StartEx;
COMI_US_Stop			:	TCMM_COMI_US_Stop;
COMI_US_SetPauseAtFull	:	TCMM_COMI_US_SetPauseAtFull;
COMI_US_Resume			:	TCMM_COMI_US_Resume;
COMI_US_ChangeScanFreq	:	TCMM_COMI_US_ChangeScanFreq;
COMI_US_ResetCount		:	TCMM_COMI_US_ResetCount;
COMI_US_ChangeSampleFreq:	TCMM_COMI_US_ChangeSampleFreq;
COMI_US_CurCount		:	TCMM_COMI_US_CurCount;
COMI_US_SBPos			:	TCMM_COMI_US_SBPos;
COMI_US_GetBufPtr		:	TCMM_COMI_US_GetBufPtr;
COMI_US_ReleaseBuf		:	TCMM_COMI_US_ReleaseBuf;
COMI_US_RetrvOne		:	TCMM_COMI_US_RetrvOne;
COMI_PID_Enable			:	TCMM_COMI_PID_Enable;
COMI_PID_SetParams		:	TCMM_COMI_PID_SetParams;
COMI_PID_Disable		:	TCMM_COMI_PID_Disable;
COMI_DIO_SetUsage		:	TCMM_COMI_DIO_SetUsage;
COMI_DIO_GetUsage		:	TCMM_COMI_DIO_GetUsage;
COMI_DI_GetOne			:	TCMM_COMI_DI_GetOne;
COMI_DI_GetAll			:	TCMM_COMI_DI_GetAll;
COMI_DI_GetAllEx		:	TCMM_COMI_DI_GetAllEx;
COMI_DO_PutOne			:	TCMM_COMI_DO_PutOne;
COMI_DO_PutAll			:	TCMM_COMI_DO_PutAll;
COMI_DO_PutAllEx		:	TCMM_COMI_DO_PutAllEx;
COMI_DO_GetOne			:	TCMM_COMI_DO_GetOne;
COMI_DO_GetAll			:	TCMM_COMI_DO_GetAll;
COMI_DO_GetAllEx		:	TCMM_COMI_DO_GetAllEx;
COMI_DA_Out				:	TCMM_COMI_DA_Out;
COMI_DA_SetRange		:	TCMM_COMI_DA_SetRanget;
COMI_WFM_Start			:	TCMM_COMI_WFM_Start;
COMI_WFM_RateChange		:	TCMM_COMI_WFM_RateChange;
COMI_WFM_GetCurPos		:	TCMM_COMI_WFM_GetCurPos;
COMI_WFM_GetCurLoops	:	TCMM_COMI_WFM_GetCurLoops;
COMI_WFM_Stop			:	TCMM_COMI_WFM_Stop;
COMI_SetCounter			:	TCMM_COMI_SetCounter;
COMI_LoadCount			:	TCMM_COMI_LoadCount;
COMI_ReadCount			:	TCMM_COMI_ReadCount;
COMI_ReadCounter32		:	TCMM_COMI_ReadCounter32;
COMI_ClearCounter32		:	TCMM_COMI_ClearCounter32;
COMI_FC_SelectGate		:	TCMM_COMI_FC_SelectGate;
COMI_FC_GateTime		:	TCMM_COMI_FC_GateTime;
COMI_FC_ReadCount		:	TCMM_COMI_FC_ReadCount;
COMI_FC_ReadFreq		:	TCMM_COMI_FC_ReadFreq;
COMI_ENC_Config			:	TCMM_COMI_ENC_Config;
COMI_ENC_Reset			:	TCMM_COMI_ENC_Reset;
COMI_ENC_Load			:	TCMM_COMI_ENC_Load;
COMI_ENC_Read			:	TCMM_COMI_ENC_Read;
COMI_ENC_ResetZ			:	TCMM_COMI_ENC_ResetZ;
COMI_ENC_LoadZ			:	TCMM_COMI_ENC_LoadZ;
COMI_ENC_ReadZ			:	TCMM_COMI_ENC_ReadZ;
COMI_PG_Start			:	TCMM_COMI_PG_Start;
COMI_PG_ChangeFreq		:	TCMM_COMI_PG_ChangeFreq;
COMI_PG_IsActive		:	TCMM_COMI_PG_IsActive;
COMI_PG_Stop			:	TCMM_COMI_PG_Stop;
COMI_SD502_SetCounter	:	TCMM_COMI_SD502_SetCounter;
COMI_SD502_ReadNowCount	:	TCMM_COMI_SD502_ReadNowCount;
COMI_SD502_ReadOldCount	:	TCMM_COMI_SD502_ReadOldCount;
COMI_SD502_GetGateState	:	TCMM_COMI_SD502_GetGateState;
COMI_SD502_GetClkFreq	:	TCMM_COMI_SD502_GetClkFreq;
COMI_SD502_Clear		:	TCMM_COMI_SD502_Clear;
COMI_SD502_ClearMulti	:	TCMM_COMI_SD502_ClearMulti;
COMI_DigitToVolt		:	TCMM_COMI_DigitToVolt;
COMI_Digit14ToVolt		:	TCMM_COMI_Digit14ToVolt;
COMI_Digit16ToVolt		:	TCMM_COMI_Digit16ToVolt;
COMI_LastError			:	TCMM_COMI_LastError;
COMI_ErrorString		:	TCMM_COMI_ErrorString;
COMI_WriteIoPortDW		:	TCMM_COMI_WriteIoPortDW;
COMI_ReadIoPortDW		:	TCMM_COMI_ReadIoPortDW;
COMI_WriteMemPortDW		:	TCMM_COMI_WriteIoPortDW;
COMI_ReadMemPortDW		:	TCMM_COMI_ReadMemPortDW;

	FDLLInstance : THandle;

procedure LoadDll;
procedure UnloadDll;

implementation

{ TComiDaq }

Const
LIBRARY_FILENAME : AnsiString = 'ComiDll.dll';

procedure LoadDll;
var
	LibraryFilename : AnsiString;
	// FDLLPath : string;

begin
	LibraryFilename := LIBRARY_FILENAME;
	FDLLInstance := LoadLibrary(PChar(LibraryFileName));

	if ( FDLLInstance <> 0 ) then
	begin

//====================== General FUNCTIONS ====================================================//
@COMI_LoadDevice			:=	GetProcAddress(FDLLInstance, 'COMI_LoadDevice');
@COMI_UnloadDevice			:=	GetProcAddress(FDLLInstance, 'COMI_UnloadDevice');
@COMI_GetAvailDevList		:=	GetProcAddress(FDLLInstance, 'COMI_GetAvailDevList');
@COMI_GetDevInfo			:=	GetProcAddress(FDLLInstance, 'COMI_GetDevInfo');
@COMI_Write8402				:=	GetProcAddress(FDLLInstance, 'COMI_Write8402');
@COMI_WriteEEPR				:=	GetProcAddress(FDLLInstance, 'COMI_WriteEEPR');
@COMI_ReadEEPR				:=	GetProcAddress(FDLLInstance, 'COMI_ReadEEPR');
@COMI_AD_SetRange			:=	GetProcAddress(FDLLInstance, 'COMI_AD_SetRange');
@COMI_AD_GetDigit			:=	GetProcAddress(FDLLInstance, 'COMI_AD_GetDigit');
@COMI_AD_GetVolt			:=	GetProcAddress(FDLLInstance, 'COMI_AD_GetVolt');
@COMI_US_Start				:=	GetProcAddress(FDLLInstance, 'COMI_US_Start');
@COMI_US_StartEx			:=	GetProcAddress(FDLLInstance, 'COMI_US_StartEx');
@COMI_US_Stop				:=	GetProcAddress(FDLLInstance, 'COMI_US_Stop');
@COMI_US_SetPauseAtFull		:=	GetProcAddress(FDLLInstance, 'COMI_US_SetPauseAtFull');
@COMI_US_Resume				:=	GetProcAddress(FDLLInstance, 'COMI_US_Resume');
@COMI_US_ChangeScanFreq		:=	GetProcAddress(FDLLInstance, 'COMI_US_ChangeScanFreq');
@COMI_US_ResetCount			:=	GetProcAddress(FDLLInstance, 'COMI_US_ResetCount');
@COMI_US_ChangeSampleFreq	:=	GetProcAddress(FDLLInstance, 'COMI_US_ChangeSampleFreq');
@COMI_US_CurCount			:=	GetProcAddress(FDLLInstance, 'COMI_US_CurCount');
@COMI_US_SBPos				:=	GetProcAddress(FDLLInstance, 'COMI_US_SBPos');
@COMI_US_GetBufPtr			:=	GetProcAddress(FDLLInstance, 'COMI_US_GetBufPtr');
@COMI_US_ReleaseBuf			:=	GetProcAddress(FDLLInstance, 'COMI_US_ReleaseBuf');
@COMI_US_RetrvOne			:=	GetProcAddress(FDLLInstance, 'COMI_US_RetrvOne');
@COMI_PID_Enable			:=	GetProcAddress(FDLLInstance, 'COMI_PID_Enable');
@COMI_PID_SetParams			:=	GetProcAddress(FDLLInstance, 'COMI_PID_SetParams');
@COMI_PID_Disable			:=	GetProcAddress(FDLLInstance, 'COMI_PID_Disable');
@COMI_DIO_SetUsage			:=	GetProcAddress(FDLLInstance, 'COMI_DIO_SetUsage');
@COMI_DIO_GetUsage			:=	GetProcAddress(FDLLInstance, 'COMI_DIO_GetUsage');
@COMI_DI_GetOne				:=	GetProcAddress(FDLLInstance, 'COMI_DI_GetOne');
@COMI_DI_GetAll				:=	GetProcAddress(FDLLInstance, 'COMI_DI_GetAll');
@COMI_DI_GetAllEx			:=	GetProcAddress(FDLLInstance, 'COMI_DI_GetAllEx');
@COMI_DO_PutOne				:=	GetProcAddress(FDLLInstance, 'COMI_DO_PutOne');
@COMI_DO_PutAll				:=	GetProcAddress(FDLLInstance, 'COMI_DO_PutAll');
@COMI_DO_PutAllEx			:=	GetProcAddress(FDLLInstance, 'COMI_DO_PutAllEx');
@COMI_DO_GetOne				:=	GetProcAddress(FDLLInstance, 'COMI_DO_GetOne');
@COMI_DO_GetAll				:=	GetProcAddress(FDLLInstance, 'COMI_DO_GetAll');
@COMI_DO_GetAllEx			:=	GetProcAddress(FDLLInstance, 'COMI_DO_GetAllEx');
@COMI_DA_Out				:=	GetProcAddress(FDLLInstance, 'COMI_DA_Out');
@COMI_DA_SetRange			:=	GetProcAddress(FDLLInstance, 'COMI_DA_SetRange');
@COMI_WFM_Start				:=	GetProcAddress(FDLLInstance, 'COMI_WFM_Start');
@COMI_WFM_RateChange		:=	GetProcAddress(FDLLInstance, 'COMI_WFM_RateChange');
@COMI_WFM_GetCurPos			:=	GetProcAddress(FDLLInstance, 'COMI_WFM_GetCurPos');
@COMI_WFM_GetCurLoops		:=	GetProcAddress(FDLLInstance, 'COMI_WFM_GetCurLoops');
@COMI_WFM_Stop				:=	GetProcAddress(FDLLInstance, 'COMI_WFM_Stop');
@COMI_SetCounter			:=	GetProcAddress(FDLLInstance, 'COMI_SetCounter');
@COMI_LoadCount				:=	GetProcAddress(FDLLInstance, 'COMI_LoadCount');
@COMI_ReadCount				:=	GetProcAddress(FDLLInstance, 'COMI_ReadCount');
@COMI_ReadCounter32			:=	GetProcAddress(FDLLInstance, 'COMI_ReadCounter32');
@COMI_ClearCounter32		:=	GetProcAddress(FDLLInstance, 'COMI_ClearCounter32');
@COMI_FC_SelectGate			:=	GetProcAddress(FDLLInstance, 'COMI_FC_SelectGate');
@COMI_FC_GateTime			:=	GetProcAddress(FDLLInstance, 'COMI_FC_GateTime');
@COMI_FC_ReadCount			:=	GetProcAddress(FDLLInstance, 'COMI_FC_ReadCount');
@COMI_FC_ReadFreq			:=	GetProcAddress(FDLLInstance, 'COMI_FC_ReadFreq');
@COMI_ENC_Config			:=	GetProcAddress(FDLLInstance, 'COMI_ENC_Config');
@COMI_ENC_Reset				:=	GetProcAddress(FDLLInstance, 'COMI_ENC_Reset');
@COMI_ENC_Load				:=	GetProcAddress(FDLLInstance, 'COMI_ENC_Load');
@COMI_ENC_Read				:=	GetProcAddress(FDLLInstance, 'COMI_ENC_Read');
@COMI_ENC_ResetZ			:=	GetProcAddress(FDLLInstance, 'COMI_ENC_ResetZ');
@COMI_ENC_LoadZ				:=	GetProcAddress(FDLLInstance, 'COMI_ENC_LoadZ');
@COMI_ENC_ReadZ				:=	GetProcAddress(FDLLInstance, 'COMI_ENC_ReadZ');
@COMI_PG_Start				:=	GetProcAddress(FDLLInstance, 'COMI_PG_Start');
@COMI_PG_ChangeFreq			:=	GetProcAddress(FDLLInstance, 'COMI_PG_ChangeFreq');
@COMI_PG_IsActive			:=	GetProcAddress(FDLLInstance, 'COMI_PG_IsActive');
@COMI_PG_Stop				:=	GetProcAddress(FDLLInstance, 'COMI_PG_Stop');
@COMI_SD502_SetCounter		:=	GetProcAddress(FDLLInstance, 'COMI_SD502_SetCounter');
@COMI_SD502_ReadNowCount	:=	GetProcAddress(FDLLInstance, 'COMI_SD502_ReadNowCount');
@COMI_SD502_ReadOldCount	:=	GetProcAddress(FDLLInstance, 'COMI_SD502_ReadOldCount');
@COMI_SD502_GetGateState	:=	GetProcAddress(FDLLInstance, 'COMI_SD502_GetGateState');
@COMI_SD502_GetClkFreq		:=	GetProcAddress(FDLLInstance, 'COMI_SD502_GetClkFreq');
@COMI_SD502_Clear			:=	GetProcAddress(FDLLInstance, 'COMI_SD502_Clear');
@COMI_SD502_ClearMulti		:=	GetProcAddress(FDLLInstance, 'COMI_SD502_ClearMulti');
@COMI_DigitToVolt			:=	GetProcAddress(FDLLInstance, 'COMI_DigitToVolt');
@COMI_Digit14ToVolt			:=	GetProcAddress(FDLLInstance, 'COMI_Digit14ToVolt');
@COMI_Digit16ToVolt			:=	GetProcAddress(FDLLInstance, 'COMI_Digit16ToVolt');
@COMI_LastError				:=	GetProcAddress(FDLLInstance, 'COMI_LastError');
@COMI_ErrorString			:=	GetProcAddress(FDLLInstance, 'COMI_ErrorString');
@COMI_WriteIoPortDW			:=	GetProcAddress(FDLLInstance, 'COMI_WriteIoPortDW');
@COMI_ReadIoPortDW			:=	GetProcAddress(FDLLInstance, 'COMI_ReadIoPortDW');
@COMI_WriteMemPortDW		:=	GetProcAddress(FDLLInstance, 'COMI_WriteMemPortDW');
@COMI_ReadMemPortDW			:=	GetProcAddress(FDLLInstance, 'COMI_ReadMemPortDW');

	end
end;

procedure UnloadDll;
begin
	if not FreeLibrary(FDLLInstance) then exit;
end;

Initialization

begin
	LoadDll;

end;

Finalization
	UnloadDll;
end.
