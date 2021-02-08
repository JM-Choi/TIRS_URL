enum {TE_POSITIVE, TE_NEGATIVE};
enum {TM_INITIAL, TM_MIDDLE};
enum {CMC_DIR_N, CMC_DIR_P}; // Direction
enum {CMC_VMODE_C, CMC_VMODE_T, CMC_VMODE_S}; // Speed mode : Constant, Trapezoidal, S-curve //
typedef enum{VT_SHORT, VT_FLOAT, VT_DOUBLE}TComiVarType;
typedef unsigned short COMIDAS_DEVID;
// Interrupt Handler Callback Function Prototype //
typedef void (WINAPI *TIntHandler) (LPVOID lParam); 

typedef struct{
	unsigned short	wSubSysID;
	unsigned long	nInstance;
	char	szDevName[20];
	unsigned char	bDevCaps; // 0bit-A/D, 1bit-D/A, 2bit-Dio, 3bit-counter,
	unsigned char	nNumAdChan, nNumDaChan, nNumDiChan, nNumDoChan, nNumCntrChan;
}TComiDevInfo;

typedef struct{
	unsigned short nNumDev;
	TComiDevInfo DevInfo[16];
}TComiDevList;

typedef struct{
	char szDate[13], szTime[10];  // Save start시의 날짜와 시간에 대한 스트링
	int nNumChan; // Scan 채널 수
	int nChanList[64]; // Scan 채널 리스트
	int dmin, dmax;
	float vmin[64], vmax[64]; // Scan시의 각 채널의 A/D range
	unsigned long dwSavedScanCnt; // 저장된 총 Scan 수
}TScanFileHead;

typedef struct{
	float Ref, lim_h, lim_l;
	float Kp;
	float Td, Ti;
	int ch_ref, ch_ad, ch_da;
}TPidParams;

typedef struct _THelicalUserInfo{
	int c_map, z_axis; // Circualr interpolation axis map, Up/Down Axis
	double c_xcen, c_ycen; // Circular interpolation ceneter point
	int c_dir; // 원호보간의 회전 방향
	int c_num; // 원의 수(마자막 Arc 포함)
	double c_la; // Last angle
	double z_dist;
}THelicalUserInfo;

// Interrupt Handler Type //
typedef enum _TCmIntHandlerType{
	IHT_MESSAGE=0, IHT_EVENT, IHT_CALLBACK
}TCmIntHandlerType;

// FIlter Mode //
typedef enum _TFilterMode{
	CLOCK_12MHz=0, CLOCK_3KHz, CLOCK_760Hz, CLOCK_95Hz
}TFilterMode;

// Interrupt Mode //
typedef enum _TInterruptMode{
	INT_EDGE_BI=0, INT_EDGE_RISING, INT_EDGE_FALLING, INT_PATTERN_LOW, INT_PATTERN_HIGH
}TInterruptMode;
 
 int COMI_LoadDll(void);
 void COMI_UnloadDll(void);
  HANDLE 	(COMI_LoadDevice) (unsigned short deviceID, unsigned long instance);
  void	(COMI_UnloadDevice) (HANDLE hDevice);
  int	(COMI_GetAvailDevList) (TComiDevList *pDevList);
  int	(COMI_GetDevInfo) (HANDLE hDevice, TComiDevInfo *pDevInfo);
  void	(COMI_Write8402) (HANDLE hDevice, int ch, int addr, int data);
  void	(COMI_WriteEEPR) (HANDLE hDevice, int addr, int data);
  int		(COMI_ReadEEPR) (HANDLE hDevice, int addr);
 //__________ A/D General Functions ________________________________________________//
  int	(COMI_AD_SetRange) (HANDLE hDevice, int ch, float vmin, float vmax);
  short	(COMI_AD_GetDigit) (HANDLE hDevice, int ch);
  float	(COMI_AD_GetVolt) (HANDLE hDevice, int ch);
 //__________ A/D Unlimited Scan Functions _________________________________//
  long	(COMI_US_Start) (HANDLE hDevice, int numCh, int chanList, unsigned int scanFreq,
			unsigned int msb, int trsMethod);
  long	(COMI_US_StartEx) (HANDLE hDevice, unsigned int dwScanFreq, unsigned int nFrameSize, unsigned int nBufSizeGain);
  int	(COMI_US_Stop) (HANDLE hDevice, int bReleaseBuf);
  int	(COMI_US_SetPauseAtFull) (HANDLE hDevice, int bPauseAtFull);
  int	(COMI_US_Resume) (HANDLE hDevice);
  long	(COMI_US_ChangeScanFreq) (HANDLE hDevice, unsigned int dwScanFreq);
  void	(COMI_US_ResetCount) (HANDLE hDevice);
  void	(COMI_US_ChangeSampleFreq) (HANDLE hDevice, unsigned long dwSampleFreq);
  unsigned long	(COMI_US_CurCount) (HANDLE hDevice);
  unsigned int	(COMI_US_SBPos) (HANDLE hDevice, int chOrder, unsigned long scanCount);
  short	(COMI_US_GetBufPtr) (HANDLE hDevice);
  int	(COMI_US_ReleaseBuf) (HANDLE hDevice);
  short	(COMI_US_RetrvOne) (HANDLE hDevice, int chOrder, unsigned long scanCount);
  unsigned long	(COMI_US_RetrvChannel) (HANDLE hDevice, int chOrder, unsigned long startCount, 
			int maxNumData, void pDestBuf, TComiVarType VarType);
  unsigned int	(COMI_US_RetrvBlock) (HANDLE hDevice, unsigned int startCount, int maxNumScan, 
			void pDestBuf, TComiVarType VarType);
  int	(COMI_US_FileSaveFirst) (HANDLE hDevice, char *szFilePath, int bIsFromStart);
  unsigned long	(COMI_US_FileSaveNext) (HANDLE hDevice);
  int	(COMI_US_FileSaveStop) (HANDLE hDevice);
  void	(COMI_US_FileConvert) (char *szBinFilePath, char *szTextFilePath, unsigned long nMaxDataRow);
  float	(COMI_US_CheckFileConvert) (void);
  void	(COMI_US_CancelFileConvert) (void);
 //___________ PID Functions _______________________________________________//
  int	(COMI_PID_Enable) (HANDLE hDevice); 
  int	(COMI_PID_SetParams) (HANDLE hDevice, int nNumCtrls, TPidParams *pPidParams); 
  int	(COMI_PID_Disable) (HANDLE hDevice); 
 //___________ DIO Common __________________________________________________//
  void	(COMI_DIO_SetUsage) (HANDLE hDevice, int usage);
  int		(COMI_DIO_GetUsage) (HANDLE hDevice);
 //__________ D/I Functions ________________________________________________//
  int		(COMI_DI_GetOne) (HANDLE hDevice, int ch);
  unsigned long	(COMI_DI_GetAll) (HANDLE hDevice);
  unsigned long	(COMI_DI_GetAllEx) (HANDLE hDevice, int nGroupIdx);
 //__________ D/O Functions ________________________________________________//
  int	(COMI_DO_PutOne) (HANDLE hDevice, int ch, int status);
  int	(COMI_DO_PutAll) (HANDLE hDevice, unsigned long dwStatuses);
  void	(COMI_DO_PutAllEx) (HANDLE hDevice, int nGroupIdx, unsigned long dwStatuses);
  int		(COMI_DO_GetOne) (HANDLE hDevice, int ch);
  unsigned long	(COMI_DO_GetAll) (HANDLE hDevice);
  unsigned long	(COMI_DO_GetAllEx) (HANDLE hDevice, int nGroupIdx);
 //__________ D/A Functions ________________________________________________//
  int	(COMI_DA_Out) (HANDLE hDevice, int ch, float volt);
  void	(COMI_DA_SetRange) (HANDLE hDevice, int ch, int VMin, int VMax);
  long	(COMI_WFM_Start) (HANDLE hDevice, int ch, float *pDataBuffer, 
						unsigned int nNumData, unsigned int nPPS, int nMaxLoops);
  long	(COMI_WFM_RateChange) (HANDLE hDevice, int ch, unsigned long nPPS);
  long	(COMI_WFM_GetCurPos) (HANDLE hDevice, int ch);
  long	(COMI_WFM_GetCurLoops) (HANDLE hDevice, int ch);
  void	(COMI_WFM_Stop) (HANDLE hDevice, int ch);
 //__________ Counter Functions ____________________________________________//
  void	(COMI_SetCounter) (HANDLE hDevice, int ch, int rw, int op, int bcd_bin, unsigned short load_value);
  void	(COMI_LoadCount) (HANDLE hDevice, int ch, unsigned short load_value);
  unsigned short	(COMI_ReadCount) (HANDLE hDevice, int ch);
  unsigned long 	(COMI_ReadCounter32) (HANDLE hDevice, int ch);
  void 	(COMI_ClearCounter32) (HANDLE hDevice, int ch);
  void	(COMI_FC_SelectGate) (HANDLE hDevice, int ch, int nGateIndex);
  unsigned long	(COMI_FC_GateTime) (HANDLE hDevice, int ch);
  unsigned long	(COMI_FC_ReadCount) (HANDLE hDevice, int ch);
  unsigned long	(COMI_FC_ReadFreq) (HANDLE hDevice, int ch);
 
  void	(COMI_ENC_Config) (HANDLE hDevice, int ch, int mode, int bResetByZ);
  void	(COMI_ENC_Reset) (HANDLE hDevice, int ch);
  void	(COMI_ENC_Load) (HANDLE hDevice, int ch, long Count);
  long	(COMI_ENC_Read) (HANDLE hDevice, int ch);
  void	(COMI_ENC_ResetZ) (HANDLE hDevice, int ch);
  void	(COMI_ENC_LoadZ) (HANDLE hDevice, int ch, short count);
  short	(COMI_ENC_ReadZ) (HANDLE hDevice, int ch);
  double	(COMI_PG_Start) (HANDLE hDevice, int ch, double freq, unsigned int num_pulses);
  double	(COMI_PG_ChangeFreq) (HANDLE hDevice, int ch, double freq);
  int	(COMI_PG_IsActive) (HANDLE hDevice, int ch);
  void	(COMI_PG_Stop) (HANDLE hDevice, int ch);

  void	(COMI_SD502_SetCounter) (HANDLE hDevice, int ch, int nMode, unsigned int nClkSource);
  unsigned long	(COMI_SD502_ReadNowCount) (HANDLE hDevice, int ch);
  unsigned long	(COMI_SD502_ReadOldCount) (HANDLE hDevice, int ch);
  int	(COMI_SD502_GetGateState) (HANDLE hDevice, int ch);
  double	(COMI_SD502_GetClkFreq) (int nClkSrcIdx);
  void	(COMI_SD502_Clear) (HANDLE hDevice, int ch);
  void	(COMI_SD502_ClearMulti) (HANDLE hDevice, unsigned long dwCtrlBits);


 //__________ Utility Functions ____________________________________________//
  float	(COMI_DigitToVolt) (short digit, float vmin, float vmax);
  float	(COMI_Digit14ToVolt) (short digit, float vmin, float vmax);
  float	(COMI_Digit16ToVolt) (short digit, float vmin, float vmax);
  int		(COMI_LastError) (void);
  const char (COMI_ErrorString) (int nErrCode);
  void	(COMI_GetResources) (HANDLE hDevice, unsigned long *pdwIntVect, unsigned long *pdwIoPorts,
			 int nNumPorts, unsigned long *pdwMemPorts, int nNumMemPorts);
  void	(COMI_WriteIoPortDW) (HANDLE hDevice, unsigned long dwPortBase, unsigned long nOffset, unsigned long dwOutVal);
  unsigned long	(COMI_ReadIoPortDW) (HANDLE hDevice, unsigned long dwPortBase, unsigned long nOffset);
  void	(COMI_WriteMemPortDW) (HANDLE hDevice, unsigned long dwPortBase, unsigned long nOffset, unsigned long dwOutVal);
  unsigned long	(COMI_ReadMemPortDW) (HANDLE hDevice, unsigned long dwPortBase, unsigned long nOffset);
  void	(COMI_GotoURL) (const char *szUrl, int bMaximize);


 //---------------SD434 Functions ------------------------------//
  long	(COMI_GetTerminal) (HANDLE hDevice, int TmNum);
  long	(COMI_INT_Start) (HANDLE hDevice);
  long	(COMI_INT_Stop) (HANDLE hDevice);
  long	(COMI_INT_Clear) (HANDLE hDevice);
  long	(COMI_INT_SetIntChan) (HANDLE hDevice, int NumCh, int nState, int nMode);
  long	(COMI_INT_GetIntChan) (HANDLE hDevice, int NumCh, long *nState, long *nMode);
  long	(COMI_INT_GetIntState) (HANDLE hDevice);
  long    (COMI_INT_SetHandler) (HANDLE hDevice, long HandlerType, HANDLE Handler, unsigned int nMessage, LPVOID lParam);
  long	(COMI_SetFilterMode) (HANDLE hDevice, int nMode);
  long	(COMI_GetFilterMode) (HANDLE hDevice);
