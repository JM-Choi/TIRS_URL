#region Licenses
///////////////////////////////////////////////////////////////////////////////
/// MIT License
/// 
/// Copyright (c) 2019 Marcus Software Ltd.
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
/// SOFTWARE.
///////////////////////////////////////////////////////////////////////////////
///          Copyright Joe Coder 2004 - 2006.
/// Distributed under the Boost Software License, Version 1.0.
///    (See accompanying file LICENSE_1_0.txt or copy at
///          https://www.boost.org/LICENSE_1_0.txt)
///////////////////////////////////////////////////////////////////////////////
/// 저작권 (c) 2019 Marcus Software Ltd. (isadrastea.kor@gmail.com)
///
/// 본 라이선스의 적용을 받는 소프트웨어와 동봉된 문서(소프트웨어)를 획득하는 
/// 모든 개인이나 기관은 소프트웨어를 Marcus Software (isadrastea.kor@gmail.com)
/// 에 신고하고, 허용 의사를 서면으로 득하여, 사용, 복제, 전시, 배포, 실행 및
/// 전송할 수 있고, 소프트웨어의 파생 저작물을 생성할 수 있으며, 소프트웨어가
/// 제공된 제3자에게 그러한 행위를 허용할 수 있다. 단, 이 모든 행위는 다음과 
/// 같은 조건에 의해 제한 한다.:
///
/// 소프트웨어의 저작권 고지, 그리고 위의 라이선스 부여와 이 규정과 아래의 부인 
/// 조항을 포함한 이 글의 전문이 소프트웨어를 전체적으로나 부분적으로 복제한 
/// 모든 복제본과 소프트웨어의 모든 파생 저작물 내에 포함되어야 한다. 단, 해당 
/// 복제본이나 파생저작물이 소스 언어 프로세서에 의해 생성된, 컴퓨터로 인식 
/// 가능한 오브젝트 코드의 형식으로만 되어 있는 경우는 제외된다.
///
/// 이 소프트웨어는 상품성, 특정 목적에의 적합성, 소유권, 비침해에 대한 보증을 
/// 포함한, 이에 국한되지는 않는, 모든 종류의 명시적이거나 묵시적인 보증 없이 
///“있는 그대로의 상태”로 제공된다. 저작권자나 소프트웨어의 배포자는 어떤 
/// 경우에도 소프트웨어 자체나 소프트웨어의 취급과 관련하여 발생한 손해나 기타 
/// 책임에 대하여, 계약이나 불법행위 등에 관계 없이 어떠한 책임도 지지 않는다.
///////////////////////////////////////////////////////////////////////////////
/// project ReelTower 
/// @author Marcus (isadrastea.kor@gmail.com)
/// @namespace Marcus.Solution.TechFloor
/// @file VisionManager.cs
/// @brief
/// @details
/// @date 2019, 5, 21, 오후 1:13
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Marcus.Solution.TechFloor.Object;
using System.Xml;
using Marcus.Solution.TechFloor.Util;
using System.Runtime.ExceptionServices;
using System.Security;
#endregion

#region Program
namespace Marcus.Solution.TechFloor
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
                Trace.WriteLine($"Debug> GetValue: Exception={ex.Message}");

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
                Trace.WriteLine($"Debug> ReadingData: Exception={ex.Message}");

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