#region Imports
using TechFloor.Object;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using TechFloor.Device;
#endregion

#region Program
namespace TechFloor.Components
{
    public class CompositeVisionManager : AbstractClassDisposable
    {
        public CompositeVisionManager()
        {
        }

        public static void SetVisionLight(ReelDiameters reeltype)
        {
            switch (reeltype)
            {
                case ReelDiameters.ReelDiameter7:
                    VisionManager.LightOnForReel7Aligenment();
                    break;
                case ReelDiameters.ReelDiameter13:
                    VisionManager.LightOnForReel13Aligenment();
                    break;
            }
        }

        public static void TriggerOn(ReelDiameters reeltype)
        {
            switch (reeltype)
            {
                case ReelDiameters.ReelDiameter7:
                    VisionManager.TriggerForReel7Alignment();
                    break;
                case ReelDiameters.ReelDiameter13:
                    VisionManager.TriggerForReel13Alignment();
                    break;
            }
        }

        public static ImageProcssingResults GetAdjustmentValue(double xerr, double yerr, ref string xpos, ref string ypos)
        {
            double xpos_ = 0;
            double ypos_ = 0;
            ImageProcssingResults rc_ = ImageProcssingResults.Exception;

            try
            {
                if (VisionManager.GetValue(VisionProcessItems.Alignment, ref rc_))
                {
                    xpos = (xpos_ = VisionManager.VisionAlignCoordnationX /= 1000).ToString();
                    ypos = (ypos_ = VisionManager.VisionAlignCoordnationY /= 1000).ToString();

                    if (xpos_ < 0)
                        xpos_ *=  -1;

                    if (ypos_ < 0)
                        ypos_ *= -1;

                    if (xpos_ > xerr || ypos_ > yerr)
                        rc_ = ImageProcssingResults.OverScope;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug> GetAdjustmentValue: Exception={ex.Message}");
            }

            return rc_;
        }

        public static void TryReadBarcode(ReelDiameters reeltype)
        {
            switch (reeltype)
            {
                case ReelDiameters.ReelDiameter7:
                    {
                        VisionManager.LightOnForReel7Barcode();
                        Thread.Sleep(800);
                        VisionManager.TriggerForReel7Barcode();
                    }
                    break;
                case ReelDiameters.ReelDiameter13:
                    {
                        VisionManager.LightOnForReel13Barcode();
                        Thread.Sleep(800);
                        VisionManager.TriggerForReel13Barcode();
                    }
                    break;
            }
        }

        public static bool GetBarcode(ref MaterialData data)
        {
            try
            {
                if (ParseBarcodeQR(VisionManager.VisionBarcode, ref data))
                {
                    if (string.IsNullOrEmpty(data.ManufacturedDatetime))
                        data.ManufacturedDatetime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    
                    if (string.IsNullOrEmpty(data.Name))
                        data.Name = DateTime.Now.ToString("yyyyMMddHHmmss");
                    
                    if (data.Supplier.ToUpper().Contains("SAMSUNG"))
                    {
                        string year_= data.ManufacturedDatetime.Substring(0, 2);
                        string week_= data.ManufacturedDatetime.Substring(2, 2);
                        int month_  = GetMonth(Int32.Parse(year_), Int32.Parse(week_));

                        if (month_ < 10)
                            data.ManufacturedDatetime = "20" + year_ + "0" + month_ + "01";
                        else
                            data.ManufacturedDatetime = "20" + year_ + month_ + "01";
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug> GetBarcode: Exception={ex.Message}");
            }

            return false;
        }
        
        public static bool GetBarcode(string contents, ref MaterialData data)
        {
            try
            {
                if (ParseBarcodeQR(contents, ref data))
                {
                    if (string.IsNullOrEmpty(data.ManufacturedDatetime))
                        data.ManufacturedDatetime = DateTime.Now.ToString("yyyyMMddHHmmss");

                    if (string.IsNullOrEmpty(data.Name))
                        data.Name = DateTime.Now.ToString("yyyyMMddHHmmss");

                    // Code for handling Samsung date exception
                    if (data.Supplier.ToUpper().Contains("SAMSUNG"))
                    {
                        string year_ = data.ManufacturedDatetime.Substring(0, 2);
                        string week_ = data.ManufacturedDatetime.Substring(2, 2);
                        int month_ = GetMonth(Int32.Parse(year_), Int32.Parse(week_));

                        if (month_ < 10)
                            data.ManufacturedDatetime = "20" + year_ + "0" + month_ + "01";
                        else
                            data.ManufacturedDatetime = "20" + year_ + month_ + "01";
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug> GetBarcode: Exception={ex.Message}");
            }

            return false;
        }

        // QR Code Parsing
        public static bool ParseBarcodeQR(string src, ref MaterialData data)
        {
            if (string.IsNullOrEmpty(src))
                return false;

            bool flag_      = false;
            bool digit_     = false;
            int count_      = 0;
            int change_     = 0;
            string num_     = string.Empty;
            string[] t1_    = src.Split(';');
            string[] t2_    = src.Split(',');
            string[] t3_;

            data.Clear();

            try
            {
                if (t2_.Length >= 3)
                {
                    for (int i = 0; i < t2_.Length; i++)
                    {
                        flag_   = t2_[i].Contains(";");
                        t3_     = t2_[i].Split(';');

                        if (flag_)
                        {
                            if (t3_.Length > 2)
                            {
                                if (string.IsNullOrEmpty(t3_[0]))
                                    return false;

                                change_ = int.Parse(t3_[0]);
                                data.Category = change_.ToString();
                                data.LotId = t3_[1];
                            }
                        }
                        else
                        {
                            digit_ = int.TryParse(t2_[i], out count_);

                            if (digit_)
                            {
                                if (t2_[i].Length > 3 && t2_[i].Length < 6)
                                    data.Quantity = int.Parse(t2_[i]);
                            }
                        }
                    }

                    return true;
                }
                else if (t1_.Length >= 3)
                {
                    if (string.IsNullOrEmpty(t1_[0]))
                        return false;

                    if (string.IsNullOrEmpty(t1_[1]))
                        return false;

                    // QR Code의경우 첫번째, 두번째는 무조건 SidNo와 LotNo임
                    data.Category = t1_[0];
                    data.LotId = t1_[1];

                    if (string.IsNullOrEmpty(t1_[2]))
                        return false;

                    if (t1_[0] == t1_[2] && t1_[1] == t1_[3])
                    {   // Amkor 의 새로운 공통 규격의 바코드 Rule인 경우 해당
                        // 엑셀 파일의 두번째 Sheet 참조
                        data.Supplier = t1_[4];
                        data.Quantity = int.Parse(t1_[5]);
                        data.ManufacturedDatetime = t1_[6];
                        return true;
                    }

                    // QR Code의 세번째 문자열이 숫자만 포함하는경우는 QTY이며 문자를 포함하는 경우는 Supplier 임
                    // 20201112 - jm.choi
                    // #4, #5에서 마지막에 N/A가 들어있는 Barcode 사용
                    // N/A가 들어있는 Barcode의 세번째 문자열은 Part_No으로 QTY가 아님
                    // Part_No은 사용하지 않도록 수정
                    if (t1_[5] != "N/A")  
                        num_ = Regex.Replace(t1_[2], @"\D", ""); // 숫자만 가져옴 (문자열 제거 (""으로 대체))

                    if (num_ == t1_[2])
                        data.Quantity = int.Parse(t1_[2]);
                    else
                    {   // QR Code의 세번째 문자열이 문자를 포함하는 경우는 Supplier 임
                        if (t1_[5] != "N/A")
                            data.Supplier = t1_[2];
                        data.Quantity = int.Parse(t1_[3]);
                        data.Name = t1_[4];
                        data.ManufacturedDatetime = t1_[5];

                        if (data.ManufacturedDatetime == "N/A")
                        {
                            if (data.Name.Substring(0, 4) == DateTime.Now.Year.ToString() || data.Name.Substring(0, 4) == (DateTime.Now.Year - 1).ToString())
                            {
                                if (App.IsDigit(data.Name.Substring(0, 8)))
                                    data.ManufacturedDatetime = data.Name.Substring(0, 8);
                            }
                            else
                                data.ManufacturedDatetime = DateTime.Now.ToString("yyyyMMdd");
                        }

                        if (t1_.Length > 6)
                        {
                            if (!string.IsNullOrEmpty(t1_[6]))
                            {
                                if (t1_[6].Length > 2)
                                {
                                    if (t1_[6].Substring(0, 2) == "RQ")
                                        data.Quantity = int.Parse(t1_[6].Substring(2, t1_[6].Length - 2));
                                }
                            }
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug> ParseBarcodeQR: Exception={ex.Message}");
            }

            return false;
        }

        public static ImageProcssingResults GetReelSize()
        {
            ImageProcssingResults rc_ = ImageProcssingResults.Success;

            if (VisionManager.GetValue(VisionProcessItems.Size, ref rc_))
                return rc_;
            else if (VisionManager.GetValue(VisionProcessItems.Alignment, ref rc_))
            {
                switch (rc_)
                {
                    case ImageProcssingResults.Exception:
                    case ImageProcssingResults.Empty:
                        return rc_;
                }
            }

            return rc_;
        }

        public static int QRcodeParsing(string QRcodeString, ref string SidNo, ref string LotNo, ref string Supplier,  ref string Qty, ref string RID, ref string MFG)
        {
            SidNo = LotNo = Supplier = Qty = RID = MFG= null;

            string[] QRcodeParts_Type1;
            string[] QRcodeParts_Type2;
            string[] QRcodeParts_Type2_1;
            string num = string.Empty;

            if (QRcodeString == string.Empty)
                return 1;

            QRcodeParts_Type1 = QRcodeString.Split(';');
            QRcodeParts_Type2 = QRcodeString.Split(',');

            bool bChkStr = false;
            bool bisnum = false;
            int chknum = 0;
            int nchange = 0;

            if (QRcodeParts_Type2.Length >= 3)
            {
                for (int j = 0; j < QRcodeParts_Type2.Length; j++)
                {
                    bChkStr = QRcodeParts_Type2[j].Contains(";");
                    QRcodeParts_Type2_1 = QRcodeParts_Type2[j].Split(';');

                    if (bChkStr == true)
                    {
                        if (QRcodeParts_Type2_1.Length > 2)
                        {
                            if (QRcodeParts_Type2_1[0] == "")
                            {
                                return 1;
                            }

                            nchange = int.Parse(QRcodeParts_Type2_1[0]);
                            SidNo = nchange.ToString();

                            LotNo = QRcodeParts_Type2_1[1];
                        }
                    }
                    else
                    {
                        bisnum = int.TryParse(QRcodeParts_Type2[j], out chknum);
                        if (bisnum == true)
                        {
                            if (QRcodeParts_Type2[j].Length > 3 && QRcodeParts_Type2[j].Length < 6)
                            {
                                Qty = QRcodeParts_Type2[j];
                            }
                        }
                    }
                }
                return 0;
            }
            else if (QRcodeParts_Type1.Length >= 3)
            {
                if (string.IsNullOrEmpty(QRcodeParts_Type1[0]))
                { return 1; }
                if (string.IsNullOrEmpty(QRcodeParts_Type1[1]))
                { return 1; }

                // QR Code의경우 첫번째, 두번째는 무조건 SidNo와 LotNo임
                SidNo = QRcodeParts_Type1[0];
                LotNo = QRcodeParts_Type1[1];

                if (QRcodeParts_Type1[2] == "") return 1;

                if (QRcodeParts_Type1[0] == QRcodeParts_Type1[2] && QRcodeParts_Type1[1] == QRcodeParts_Type1[3])
                {
                    // Amkor 의 새로운 공통 규격의 바코드 Rule인 경우 해당
                    // 엑셀 파일의 두번째 Sheet 참조
                    Supplier = QRcodeParts_Type1[4];
                    Qty = QRcodeParts_Type1[5];
                    MFG = QRcodeParts_Type1[6];
                    return 0;
                }

                // QR Code의 세번째 문자열이 숫자만 포함하는경우는 QTY이며 문자를 포함하는 경우는 Supplier 임
                num = Regex.Replace(QRcodeParts_Type1[2], @"\D", ""); // 숫자만 가져옴 (문자열 제거 (""으로 대체))

                if (num == QRcodeParts_Type1[2])
                {
                    Qty = QRcodeParts_Type1[2];
                    return 0;
                }
                else
                {   // QR Code의 세번째 문자열이 문자를 포함하는 경우는 Supplier 임
                    Supplier = QRcodeParts_Type1[2];
                    Qty = QRcodeParts_Type1[3];
                    RID = QRcodeParts_Type1[4];
                    MFG = QRcodeParts_Type1[5];

                    if (QRcodeParts_Type1.Length > 6)
                    {
                        if (QRcodeParts_Type1[6] != "")
                        {
                            if (QRcodeParts_Type1[6].Length > 2)
                            {
                                if (QRcodeParts_Type1[6].Substring(0, 2) == "RQ")
                                    Qty = QRcodeParts_Type1[6].Substring(2, QRcodeParts_Type1[6].Length - 2);
                            }
                        }
                    }
                    return 0;
                }
            }              
            return 1;
        }

        public static void SandVisionGetData_InchChk()
        {
            VisionManager._CILightOn(VisionManager.CONST_REEL_SIZE_CONFIRM);
            Thread.Sleep(1000);
            VisionManager._CITrigger(VisionManager.CONST_REEL_SIZE_CONFIRM);
        }

        public static int GetVisionData_InchChk()
        {
            int result_ = 0;

            if (VisionManager.ReadReelType() != 0)
            {
                switch (result_ = VisionManager.ReadAlignmentResult())
                {
                    default:
                        result_ = 0;
                        break;
                    case 1:
                    case 2:
                        break;
                }
            }

            return result_;
        }

        // Function for handling Samsung date exception. Converts weeks to months
        public static int GetMonth(int year, int week)
        {
            DateTime dt = new DateTime(year, 1, 1);
            dt.AddDays((week - 1) * 7);

            for (int i = 0; i <= 365; ++i)
            {
                int Week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                if (Week == week) return dt.Month;
                dt = dt.AddDays(1);
            }

            return 0;
        }
    }
}
#endregion