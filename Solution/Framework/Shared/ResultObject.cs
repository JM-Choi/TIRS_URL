#region Imports
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Program
namespace TechFloor.Shared
{
    #region Enumerations
    public enum VisionProcessDataObjectTypes : int
    {
        ProcessArgument,
        ResultData,
    }

    public enum ProcessArguments : int
    {
        ProcessType,
        CenterXOffsetLimit,
        CenterYOffsetLimit,
        MatchPatternIndex,
        MatchScoreLimit,
        MatchAcceptScore,
        MatchScaleLimitLow,
        MatchScaleLimitUpper,
        MatchAngleMin,
        MatchAngleMax,
        UseCenterPattern,
        UseFoundPatternVerify,
        RMSErrorLimit,
        SearchMaskRadius_0, // Reel 7 inch
        SearchMaskRadius_1, // Reel 13 inch
        SearchMaskRadial_0,
        SearchMaskRadial_1,
        VerifyMaskRadius_0, // Reel 7 inch
        VerifyMaskRadius_1, // Reel 13 inch
        VerifyMaskRadial_0,
        VerifyMaskRadial_1,
        TargetPointRadius_0,
        MaxFindQrCount,
        MaxFind2DMatrixCount,
        MaxFind1DBarcodeCount,
        MaxFindBarcodeTimeout, // 0 is Not use
        TowerBasePointMode,
        TowerBasePointReferenceX,
        TowerBasePointReferenceY,
        TowerBasePointReferenceZ,
        CartGuidePointMode,
        CartGuidePointPitchX,
        CartGuidePointPitchY,
    }

    public enum ResultDataElements : int
    {
        ResultCode,
        RunStatus,
        ProcessIndex,
        MatchScore,
        MatchScale,
        RMSError,
        FoundPatternIndex,
        FoundPatternCenterX,
        FoundPatternCenterY,
        FoundPatternAngle,
        FoundPatternRadius,
        FoundQrCode,
        Found2DMatrix,
        Found1DBarcode,
        TotalElapsed,
        EndTime, // The date time of end vision process 
        UserData_0, // Cart type
        UserData_1, // Reel empty
        UserData_2, // 2nd QR data
        UserData_3, // Qr count
        VerifiedBasePoint,
        VerifiedGuidePoint,
        FoundCartGuidePointX1,
        FoundCartGuidePointY1,
        FoundCartGuidePointAngle1,
        FoundCartGuidePointX2,
        FoundCartGuidePointY2,
        FoundCartGuidePointAngle2,
        FoundCartGuidePointX3,
        FoundCartGuidePointY3,
        FoundCartGuidePointAngle3,
        FoundCartGuidePointX4,
        FoundCartGuidePointY4,
        FoundCartGuidePointAngle4,
        FoundTowerBasePointX1,
        FoundTowerBasePointY1,
        FoundTowerBasePointAngle1,
        FoundTowerBasePointX2,
        FoundTowerBasePointY2,
        FoundTowerBasePointAngle2,
        FoundTowerBasePointDisplacementX,
        FoundTowerBasePointDisplacementY,
        FoundTowerBasePointDisplacementAngle,
    }
    #endregion

    [Serializable]
    public class VisionProcessDataObject
    {
        #region Fields
        protected VisionProcessDataObjectTypes dataObjectType = VisionProcessDataObjectTypes.ResultData;
        protected Dictionary<int, object> items = new Dictionary<int, object>();
        #endregion

        #region Properties
        public VisionProcessDataObjectTypes DataObjectType => dataObjectType;
        public IReadOnlyDictionary<int, object> Items => items;
        #endregion

        #region Constructors
        public VisionProcessDataObject(VisionProcessDataObjectTypes dataobjecttype = VisionProcessDataObjectTypes.ResultData, Dictionary<int, object> items = null)
        {
            this.dataObjectType = dataobjecttype;

            if (items != null)
                this.items = items;
        }
        #endregion

        #region Public methods
        public void Clear()
        {
            items.Clear();
        }

        public void CopyFrom(VisionProcessDataObject src)
        {
            dataObjectType = src.dataObjectType;
            items = src.items;            
        }

        protected virtual string ToStringAllProcessArgument()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<int, object> item in items)
                sb.AppendFormat($"{((ProcessArguments)item.Key).ToString()}={item.Value}/");
            return sb.ToString();
        }

        protected virtual string ToStringAllResultData()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<int, object> item in items)
                sb.AppendFormat($"{((ResultDataElements)item.Key).ToString()}={item.Value}/");
            return sb.ToString();
        }

        public virtual string ToStringAll()
        {
            
            switch (dataObjectType)
            {
                case VisionProcessDataObjectTypes.ProcessArgument:
                    return ToStringAllProcessArgument();
                case VisionProcessDataObjectTypes.ResultData:
                    return ToStringAllResultData();
                default:
                    return string.Empty;
            }
        }

        public virtual object GetValue(ProcessArguments name)
        {
            switch (dataObjectType)
            {
                case VisionProcessDataObjectTypes.ProcessArgument:
                    {
                        if (items.ContainsKey((int)name))
                        {
                            return items[(int)name];
                        }
                    }
                    break;
            }

            return null;
        }

        public virtual object GetValue(ResultDataElements name)
        {
            switch (dataObjectType)
            {
                case VisionProcessDataObjectTypes.ResultData:
                    {
                        if (items.ContainsKey((int)name))
                        {
                            return items[(int)name];
                        }
                    }
                    break;
            }

            return null;
        }

        public virtual bool SetValue(ProcessArguments name, object value)
        {
            switch (dataObjectType)
            {
                case VisionProcessDataObjectTypes.ProcessArgument:
                    {
                        if (items.ContainsKey((int)name))
                        {
                            items[(int)name] = value;
                        }
                        else
                        {
                            items.Add((int)name, value);
                        }

                        return true;
                    }
            }

            return false;
        }

        public virtual bool SetValue(ResultDataElements name, object value)
        {
            switch (dataObjectType)
            {
                case VisionProcessDataObjectTypes.ResultData:
                    {
                        if (items.ContainsKey((int)name))
                        {
                            items[(int)name] = value;
                        }
                        else
                        {
                            items.Add((int)name, value);
                        }

                        return true;
                    }
            }

            return false;
        }

        public virtual bool IsArgumnnetAvailable(ProcessArguments name, ref object value)
        {
            switch (dataObjectType)
            {
                case VisionProcessDataObjectTypes.ProcessArgument:
                    {
                        if (items.ContainsKey((int)name))
                        {
                            value = items[(int)name];
                            return true;
                        }
                    }
                    break;
            }

            return false;
        }

        public virtual bool IsResultAvailable(ResultDataElements name, ref object value)
        {
            switch (dataObjectType)
            {
                case VisionProcessDataObjectTypes.ResultData:
                    {
                        if (items.ContainsKey((int)name))
                        {
                            value = items[(int)name];
                            return true;
                        }
                    }
                    break;
            }

            return false;
        }
        #endregion
    }
}
#endregion