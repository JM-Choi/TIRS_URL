#region Imports
using TechFloor.Object;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
#endregion

#region Program
namespace TechFloor.Device
{
    public class VisionManager : SimulatableDevice
    {
        #region Imports
        [DllImport("GrabInterfaceDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void _CIInitDll();

        [DllImport("GrabInterfaceDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void _CILightOn(long nInspMode);

        [DllImport("GrabInterfaceDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void _CILightOff();

        [DllImport("GrabInterfaceDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void _CITrigger(long nInspMode);

        [DllImport("GrabInterfaceDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern string _CIResult();

        [DllImport("GrabInterfaceDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr _CIReturnResult();

        [DllImport("GrabInterfaceDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr _CIMCStatusResult();

        [DllImport("GrabInterfaceDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr _CIMCStatusResultReset();
        #endregion

        #region Constants
        public const int CONST_MODE_REEL_ALIGN              =1;
        public const int CONST_MODE_DECODE_REEL_BARCODE     = 2;
        public const int CONST_MODE_CHECK_REEL_SIZE         = 3;
        public const int CONST_REEL_SIZE_13_BARCODE         = 0;
        public const int CONST_REEL_SIZE_13_ALIGN           = 1;
        public const int CONST_REEL_SIZE_7_BARCODE          = 2;
        public const int CONST_REEL_SIZE_7_ALIGN            = 3;
        public const int CONST_REEL_SIZE_CONFIRM            = 4;
        public const int CONST_REEL_BARCODE_TYPE_1D         = 1;
        public const int CONST_REEL_BARCODE_TYPE_2D         = 2;
        public const string CONST_TOKEN_DELIMITER           = "/";
        public const string CONST_ALIGN_DELIMITER           = ",";
        public const string CONST_BARCODE_DELIMITER         = ":";
        #endregion

        #region Fields
        public static int VisionPositionOfToken         = 0;
        public static double VisionAlignCoordnationX    = 0.0;
        public static double VisionAlignCoordnationY    = 0.0;
        public static string VisionAllContextsOfReel    = string.Empty;
        public static string VisionTargetContexts       = string.Empty;
        public static string VisionAlign                = string.Empty;
        public static string VisionBarcode              = string.Empty;
        public static string VisionReelSize             = string.Empty;
        #endregion

        #region Read barcode on reel
        public static bool GetValue(VisionProcessItems item, ref ImageProcssingResults rc)
        {
            bool result_            = true;
            int countOfText_        = 0;
            int count_              = 0;
            string token_           = string.Empty;
            VisionAlignCoordnationX = 0.0;
            VisionAlignCoordnationY = 0.0;
            VisionAllContextsOfReel = string.Empty;
            VisionReelSize          = string.Empty;

            try
            {
                IntPtr ptr_             = VisionManager._CIReturnResult();
                VisionAllContextsOfReel = Marshal.PtrToStringAnsi(ptr_);
                VisionTargetContexts    = VisionAllContextsOfReel.Remove(0, 15); // Remove 15 characters.
                VisionPositionOfToken   = VisionTargetContexts.IndexOf(CONST_TOKEN_DELIMITER);

                switch (item)
                {
                    case VisionProcessItems.Alignment:
                        {
                            switch ((token_ = VisionTargetContexts.Substring(0, VisionPositionOfToken)).ToLower())
                            {
                                case "end":
                                    {
                                        rc = ImageProcssingResults.Empty;
                                        result_ = false;
                                    }
                                    break;
                                default:
                                    {
                                        countOfText_ = token_.Length;
                                        count_ = token_.IndexOf(CONST_ALIGN_DELIMITER);
                                        VisionAlignCoordnationX = double.Parse(token_.Substring(0, count_));
                                        VisionAlignCoordnationY = double.Parse(token_.Substring(count_ + 1));
                                        rc = ImageProcssingResults.Success;
                                    }
                                    break;
                            }
                        }
                        break;
                    case VisionProcessItems.Barcode:
                        {
                            if (VisionPositionOfToken <= 0)
                            {
                                rc = ImageProcssingResults.Empty;
                                result_ = false;
                            }
                            else
                            {
                                VisionBarcode = VisionTargetContexts.Substring(0, VisionPositionOfToken);
                                rc = ImageProcssingResults.Success;
                            }
                        }
                        break;
                    case VisionProcessItems.Size:
                        {
                            if (VisionPositionOfToken <= 0)
                            {
                                rc = ImageProcssingResults.Empty;
                                result_ = false;
                            }
                            else
                            {
                                VisionReelSize = VisionTargetContexts.Substring(0, VisionPositionOfToken);
                                rc = ImageProcssingResults.Success;
                            }
                        }
                        break;
                }
            }
            catch (SystemException ex)
            {
                Debug.WriteLine($"Debug> GetValue: Exception={ex.Message}");

                switch (item)
                {
                    case VisionProcessItems.Alignment:
                        {
                            VisionAlignCoordnationX = 0.0;
                            VisionAlignCoordnationY = 0.0;
                        }
                        break;
                    case VisionProcessItems.Barcode:
                        {
                            VisionBarcode = string.Empty;
                        }
                        break;
                }

                rc = ImageProcssingResults.Exception;
                result_ = false;
            }

            return result_;
        }
        
        public static int ReadingData(int querytype)
        {
            int countOfText_        = 0;
            int count_              = 0;
            string token_           = string.Empty;
            VisionAlignCoordnationX = 0.0;
            VisionAlignCoordnationY = 0.0;
            VisionAllContextsOfReel = string.Empty;
            VisionReelSize          = string.Empty;
            
            try
            {
                IntPtr ptr_         = VisionManager._CIReturnResult();
                VisionAllContextsOfReel   = Marshal.PtrToStringAnsi(ptr_);
                VisionTargetContexts      = VisionAllContextsOfReel.Remove(0, 15); // Remove 15 characters.
                VisionPositionOfToken     = VisionTargetContexts.IndexOf(CONST_TOKEN_DELIMITER);

                if (querytype == CONST_MODE_REEL_ALIGN)
                { 
                    token_ = VisionTargetContexts.Substring(0, VisionPositionOfToken);

                    if (token_ == "END")
                        return 2;
                    else
                    {
                        countOfText_            = token_.Length;
                        count_                  = token_.IndexOf(CONST_ALIGN_DELIMITER);
                        VisionAlignCoordnationX = double.Parse(token_.Substring(0, count_));
                        VisionAlignCoordnationY = double.Parse(token_.Substring(count_ + 1));
                    }
                }
                else if (querytype == CONST_MODE_DECODE_REEL_BARCODE)
                {
                    if (VisionPositionOfToken == 0)
                        return 3;

                    VisionBarcode = VisionTargetContexts.Substring(0, VisionPositionOfToken);
                }
                else if (querytype == CONST_MODE_CHECK_REEL_SIZE)
                {
                    VisionReelSize = VisionTargetContexts.Substring(0, VisionPositionOfToken);
                }
            }
            catch (SystemException ex)
            {
                Debug.WriteLine($"Debug> ReadingData: Exception={ex.Message}");

                if (querytype == CONST_MODE_REEL_ALIGN)
                {
                    VisionAlignCoordnationX = 0.0;
                    VisionAlignCoordnationY = 0.0;
                }
                else if (querytype == CONST_MODE_DECODE_REEL_BARCODE)
                {
                    VisionBarcode = string.Empty;
                }

                return 1;
            } 

            return 0;
        }

        public static void ReadingDataInit()
        {
            _CIInitDll();
        }

        public static void LightOnForReelSizeConfirm()  => VisionManager._CILightOn(CONST_REEL_SIZE_CONFIRM);
        public static void TriggerForReelSizeConfirm()  => VisionManager._CITrigger(CONST_REEL_SIZE_CONFIRM);

        public static void LightOnForReel7Aligenment()  => VisionManager._CILightOn(CONST_REEL_SIZE_7_ALIGN);
        public static void TriggerForReel7Alignment()   => VisionManager._CITrigger(CONST_REEL_SIZE_7_ALIGN);
        public static void LightOnForReel7Barcode()     => VisionManager._CILightOn(CONST_REEL_SIZE_7_BARCODE);
        public static void TriggerForReel7Barcode()     => VisionManager._CITrigger(CONST_REEL_SIZE_7_BARCODE);

        public static void LightOnForReel13Aligenment() => VisionManager._CILightOn(CONST_REEL_SIZE_13_ALIGN);
        public static void TriggerForReel13Alignment()  => VisionManager._CITrigger(CONST_REEL_SIZE_13_ALIGN);
        public static void LightOnForReel13Barcode()    => VisionManager._CILightOn(CONST_REEL_SIZE_13_BARCODE);
        public static void TriggerForReel13Barcode()    => VisionManager._CITrigger(CONST_REEL_SIZE_13_BARCODE);

        public static int ReadReelType()                => VisionManager.ReadingData(CONST_MODE_CHECK_REEL_SIZE);
        public static int ReadAlignmentResult()         => VisionManager.ReadingData(CONST_MODE_REEL_ALIGN);
        #endregion
    }
}
#endregion