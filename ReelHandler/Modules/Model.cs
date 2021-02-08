#region Imports
using TechFloor.Object;
using TechFloor.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
#endregion

#region Program
namespace TechFloor
{
    public class Model
    {
        #region Enumerations
        public enum LightCategories
        {
            Calibration,
            Check_cart_top,
            Check_cart_middle,
            Check_cart_bottom,
            Check_cart7_guide_point1_top,
            Check_cart7_guide_point1_middle,
            Check_cart7_guide_point1_bottom,
            Check_cart7_guide_point2_top,
            Check_cart7_guide_point2_middle,
            Check_cart7_guide_point2_bottom,
            Check_cart7_guide_point3_top,
            Check_cart7_guide_point3_middle,
            Check_cart7_guide_point3_bottom,
            Check_cart7_guide_point4_top,
            Check_cart7_guide_point4_middle,
            Check_cart7_guide_point4_bottom,
            Check_cart13_guide_point1_top,
            Check_cart13_guide_point1_middle,
            Check_cart13_guide_point1_bottom,
            Check_cart13_guide_point2_top,
            Check_cart13_guide_point2_middle,
            Check_cart13_guide_point2_bottom,
            Check_tower_base_point1_top,
            Check_tower_base_point1_middle,
            Check_tower_base_point1_bottom,
            Check_tower_base_point2_top,
            Check_tower_base_point2_middle,
            Check_tower_base_point2_bottom,
            Return7_top,
            Return7_middle,
            Return7_bottom,
            Return13_top,
            Return13_middle,
            Return13_bottom,
            Cart7_workslot1_top,
            Cart7_workslot1_middle,
            Cart7_workslot1_bottom,
            Cart7_workslot2_top,
            Cart7_workslot2_middle,
            Cart7_workslot2_bottom,
            Cart7_workslot3_top,
            Cart7_workslot3_middle,
            Cart7_workslot3_bottom,
            Cart7_workslot4_top,
            Cart7_workslot4_middle,
            Cart7_workslot4_bottom,
            Cart7_workslot5_top,
            Cart7_workslot5_middle,
            Cart7_workslot5_bottom,
            Cart7_workslot6_top,
            Cart7_workslot6_middle,
            Cart7_workslot6_bottom,
            Cart13_workslot1_top,
            Cart13_workslot1_middle,
            Cart13_workslot1_bottom,
            Cart13_workslot2_top,
            Cart13_workslot2_middle,
            Cart13_workslot2_bottom,
            Cart13_workslot3_top,
            Cart13_workslot3_middle,
            Cart13_workslot3_bottom,
            Cart13_workslot4_top,
            Cart13_workslot4_middle,
            Cart13_workslot4_bottom,
        }

        public enum LightLevels
        {
            Top,
            Middle,
            Bottom
        }

        public enum TaskCategories
        {
            Check_cart_type,
            Process_reel_7_on_cart,
            Align_reel_7_on_cart,
            Process_reel_13_on_cart,
            Align_reel_13_on_cart,
            Decode_barcode_on_cart,
            Process_reel_7_on_stage,
            Align_reel_7_on_stage,
            Process_reel_13_on_stage,
            Align_reel_13_on_stage,
            Decode_barcode_on_stage,
            Check_tower_base_point,
            Check_cart_guide_point,
        }

        public enum TaskSubCategories
        {
            CheckCartType,
            CheckCartGuidePoint,
            CheckTowerBasePoint,
            ProcessAtOnce,
            Alignment,
            DecodeBarcode,
        }

        public enum CalibrationSpecifications
        {
            Unknown = -1,
            TowerBasePoint,
            Cart7GuidePoint,
            Cart13GuidePoint,
        }
        #endregion

        #region Fields
        protected static int returnReelSensingDelay = 3000;
        protected static int imageProcessingDelay = 3000;
        protected static int unloadReelStateUpdateDelay = 1000;
        protected static int materialPackageUpdateDelay = 1000;
        protected static ImageProcessObject process = null;
        #endregion

        #region Properties
        public static int DelayOfReturnReelSensing
        {
            get => returnReelSensingDelay;
            set => returnReelSensingDelay = value;
        }

        public static int DelayOfImageProcessing
        {
            get => imageProcessingDelay;
            set => imageProcessingDelay = value;
        }

        public static int DelayOfUnloadReelStateUpdate
        {
            get => unloadReelStateUpdateDelay;
            set => unloadReelStateUpdateDelay = value;
        }

        public static int DelayOfMaterialPackageUpdate
        {
            get => materialPackageUpdateDelay;
            set => materialPackageUpdateDelay = value;
        }

        public static int RetryOfVisionFailure
        {
            get => process.RetryOfVisionAttempts;
            set => process.RetryOfVisionAttempts = value;
        }

        public static int RetryOfVisionAttempts
        {
            get => process.RetryOfVisionAttempts;
            set => process.RetryOfVisionAttempts = value;
        }

        public static int DelayOfTrigger
        {
            get => process == null? 500 : process.DelayOfTrigger;
            set
            {
                if (process == null)
                    process.DelayOfTrigger = value;
            }
        }

        public static int TimeoutOfImageProcessing
        {
            get => process == null ? 10000 : process.TimeoutOfImageProcessing;
            set
            {
                if (process == null)
                    process.TimeoutOfImageProcessing = value;
            }
        }

        public static int TimeoutOfTotalImageProcess
        {
            get => process == null ? 11000 : process.TimeoutOfTotalImageProcess;
        }

        public static int TimeoutOfRetryTrigger
        {
            get => process == null ?1000 : process.TimeoutOfRetryTrigger;
        }

        public static bool DynamicLightControl => process != null ? process.DynamicLightControl : false;

        public static PointF AlignmentRangeLimit
        {
            get => process.AlignmentRangeLimit;
            set => process.AlignmentRangeLimit = value;
        }

        public static ImageProcessObject Process => process;

        public static IReadOnlyDictionary<int, Pair<string, ImageProcessTaskTypes>> VisionTasks => (process != null ? process.VisionTasks : null);

        public static IReadOnlyList<VisionLightObject> VisionLights => (process != null ? process.VisionLights : null);
        #endregion

        #region Constructors
        public Model()
        {
            Load();
        }
        #endregion

        #region Static methods
        public static bool Load(string filename = "Default.xml")
        {
            bool result_ = false;

            try
            {
                string filePath_ = $@"{App.Path}Model\{(string.IsNullOrEmpty(filename) ? "Default.xml" : filename)}";

                if (!File.Exists(filePath_))
                    return false;

                XmlDocument xml = new XmlDocument();
                xml.Load(filePath_);

                if (App.Version == xml.DocumentElement.GetAttribute("version"))
                {
                    XmlNode element_ = xml.DocumentElement;

                    foreach (XmlNode node_ in element_.ChildNodes)
                    {
                        switch (node_.Name.ToLower())
                        {
                            case "process":
                                {
                                    process = new ImageProcessObject(node_);
                                }
                                break;
                            case "delay":
                                {
                                    foreach (XmlNode child_ in node_.ChildNodes)
                                    {
                                        switch (child_.Name.ToLower())
                                        {
                                            case "returnreelsensing":
                                                {
                                                    returnReelSensingDelay = Convert.ToInt32(child_.InnerText);
                                                }
                                                break;
                                            case "imageprocessing":
                                                {
                                                    imageProcessingDelay = Convert.ToInt32(child_.InnerText);
                                                }
                                                break;
                                            case "unloadreelstateupdatedelay":
                                                {
                                                    unloadReelStateUpdateDelay= Convert.ToInt32(child_.InnerText);
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                        }
                    }

                    result_ = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }

            return result_;
        }

        public static bool Save(string filename= null)
        {
            bool result_ = false;

            try
            {
                string filePath_ = $@"{App.Path}Model\{(string.IsNullOrEmpty(filename) ? "Default.xml" : filename)}";

                if (!File.Exists(filePath_))
                    return false;

                XElement delay = new XElement("Delay");
                delay.Add(new XElement("ReturnReelSensing", Model.DelayOfReturnReelSensing));
                delay.Add(new XElement("ImageProcessing", Model.DelayOfImageProcessing));
                delay.Add(new XElement("UnloadReelStateUpdate", Model.DelayOfUnloadReelStateUpdate));

                XElement xml = new XElement("Model", new XAttribute("version", App.Version));

                if (process != null)
                    process.Save(ref xml);

                xml.Add(delay);
                xml.Save(filePath_);
                result_ = true;
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }

            return result_;
        }

        public static int GetLightCategory(int channel, int height, LightCategories topcat, LightCategories bottomcat)
        {
            int result = -1;
            VisionLightObject light = null;

            if (process != null)
            {
                for (LightCategories s = topcat; s <= bottomcat; s++)
                {
                    if ((light = process.GetLightCategoryByName(s.ToString().Replace("_", " "))) != null)
                    {
                        if (height >= light.GetHeight(channel))
                        {
                            result = light.Id;
                            break;
                        }
                    }
                }

                if (result < 0)
                {
                    if ((light = process.GetLightCategoryByName(topcat.ToString().Replace("_", " "))) != null)
                    {
                        result = light.Id;
                    }
                }
            }

            return result;
        }

        public static int GetLightCategory(LightCategories cat)
        {
            int result = -1;
            VisionLightObject light = null;

            if (process != null)
            {
                if ((light = process.GetLightCategoryByName(cat.ToString().Replace("_", " "))) != null)
                    result = light.Id;
            }

            return result;
        }

        public static int GetTaskCategory(TaskCategories cat)
        {
            int result = -1;
            
            if (process != null)
            {
                if (process.VisionTasks != null)
                    return process.GetTaskCategoryByName(cat.ToString().Replace("_", " "));
            }

            return result;
        }
        #endregion
    }

    public class VisionLightObject
    {
        public class LightValue
        {
            public int height;
            public int value;

            public LightValue(int h, int v)
            {
                height = h;
                value = v;
            }
        }

        #region Fields
        protected int objectId = -1;
        protected string objectName = string.Empty;
        protected List<Pair<int, LightValue>> lights = new List<Pair<int, LightValue>>();
        #endregion

        #region Properties
        public int Id => objectId;
        public string Name => objectName;
        public IReadOnlyList<Pair<int, LightValue>> Lights => lights;
        public bool IsAvailable(int channel) => lights.Find(x => x.first == channel) != null ? true : false;
        #endregion

        #region Constructors
        public VisionLightObject(int id = -1, string name= null)
        {
            objectId = id;
            objectName = name;
        }
        #endregion

        #region Public methods
        public void ModifyValue(int channel, int value)
        {
            Pair<int, LightValue> light = lights.Find(x => x.first == channel);
            if (light != null)
            {
                light.second.value = value;
            }
        }

        public void SetValue(int channel, int value, int heihgt = 0)
        {
            Pair<int, LightValue> light = lights.Find(x => x.first == channel);
            if (light != null)
            {
                light.second.height = heihgt;
                light.second.value = value;
            }
            else
                lights.Add(new Pair<int, LightValue>(channel, new LightValue(heihgt, value)));
        }

        public int GetValue(int channel)
        {
            Pair<int, LightValue> light = lights.Find(x => x.first == channel);
            if (light != null)
                return light.second.value;

            return -1;
        }

        public int GetHeight(int channel)
        {
            Pair<int, LightValue> light = lights.Find(x => x.first == channel);
            if (light != null)
                return light.second.height;

            return 0;
        }
        #endregion
    }

    public class ImageProcessObject
    {
        #region Fields
        protected bool dynamicLightControl = false;
        protected int visionFailureRetry = 10;
        protected int visionRetryAttempts = 3;
        protected int timeoutOfRetryTrigger = 2000;
        protected int ImageProcessingTimeout = 10000;
        protected int delayOfTrigger = 500;
        protected PointF alignmentRangeLimit = new PointF(20.0f, 20.0f);
        protected Dictionary<Model.CalibrationSpecifications, Pair<int, Coord3DField<double, double, double, double, double, double>>> calibrationspecs = new Dictionary<Model.CalibrationSpecifications, Pair<int, Coord3DField<double, double, double, double, double, double>>>();
        protected Dictionary<int, Coord3DField<double, double, double, double, double, double>> towerBasePoints = new Dictionary<int, Coord3DField<double, double, double, double, double, double>>();
        protected Dictionary<int, Coord3DField<double, double, double, double, double, double>> cart7GuidePoints = new Dictionary<int, Coord3DField<double, double, double, double, double, double>>();
        protected Dictionary<int, Coord3DField<double, double, double, double, double, double>> cart13GuidePoints = new Dictionary<int, Coord3DField<double, double, double, double, double, double>>();
        protected List<VisionLightObject> visionLights = new List<VisionLightObject>();
        protected Dictionary<int, Pair<string, ImageProcessTaskTypes>> visionTasks = new Dictionary<int, Pair<string, ImageProcessTaskTypes>>();
        #endregion

        #region Properties
        public bool DynamicLightControl
        {
            get => dynamicLightControl;
            set => dynamicLightControl = value;
        }

        public int RetryOfVisionFailure
        {
            get => visionFailureRetry;
            set => visionFailureRetry = value;
        }

        public int RetryOfVisionAttempts
        {
            get => visionRetryAttempts;
            set => visionRetryAttempts = value;
        }

        public int DelayOfTrigger
        {
            get => delayOfTrigger;
            set => delayOfTrigger = value;
        }

        public int TimeoutOfImageProcessing
        {
            get => ImageProcessingTimeout;
            set => ImageProcessingTimeout = value;
        }

        public int TimeoutOfTotalImageProcess
        {
            get => delayOfTrigger + ImageProcessingTimeout;
        }

        public int TimeoutOfRetryTrigger
        {
            get => timeoutOfRetryTrigger;
        }

        public PointF AlignmentRangeLimit
        {
            get => alignmentRangeLimit;
            set => alignmentRangeLimit = value;
        }

        public ProcessCategories ProcessCategory
        {
            get;
            set;
        }

        public IReadOnlyDictionary<int, Pair<string, ImageProcessTaskTypes>> VisionTasks => visionTasks;

        public IReadOnlyList<VisionLightObject> VisionLights => visionLights;

        public VisionLightObject GetLightCategoryByName(string name) => visionLights.Find(x => x.Name == name);

        public int GetTaskCategoryByName(string name)
        {
            foreach (KeyValuePair<int, Pair<string, ImageProcessTaskTypes>> item in visionTasks)
            {
                if (item.Value.first == name)
                    return item.Key;
            }

            return -1;
        }

        public IReadOnlyDictionary<Model.CalibrationSpecifications, Pair<int, Coord3DField<double, double, double, double, double, double>>> CalibrationSpecs => calibrationspecs;

        public IReadOnlyDictionary<int, Coord3DField<double, double, double, double, double, double>> TowerBasePoints => towerBasePoints;

        public IReadOnlyDictionary<int, Coord3DField<double, double, double, double, double, double>> Cart7GuidePoints => cart7GuidePoints;

        public IReadOnlyDictionary<int, Coord3DField<double, double, double, double, double, double>> Cart13GuidePoints => cart13GuidePoints;

        public Coord3DField<double, double, double, double, double, double> GetTowerBasePointReference(int id = 1) => towerBasePoints?[id];
        #endregion

        #region Constructors
        public ImageProcessObject(XmlNode node)
        {
            Load(node);
        }
        #endregion

        #region Protected methods
        #endregion

        #region Public methods
        public bool IsAvailable(int index) => visionLights.Find(x => x.Id == index) != null ? true : false;

        public VisionLightObject GetLight(int index) => visionLights.Find(x => x.Id == index);

        public bool ModifyLightValue(int index, int channel, int value)
        {
            VisionLightObject light = GetLight(index);
            if (light != null)
            {
                light.ModifyValue(channel, value);
                return true;
            }

            return false;
        }

        public bool SetLightValue(int index, int channel, int value, int height = 0)
        {
            VisionLightObject light = GetLight(index);
            if (light != null)
            {
                light.SetValue(channel, value, height);
                return true;
            }

            return false;
        }

        public int GetLightValue(int index, int channel)
        {
            VisionLightObject light = GetLight(index);
            if (light != null)
                return light.GetValue(channel);
            return -1;
        }

        protected bool Load(XmlNode node)
        {
            bool result_ = false;

            try
            {
                if (node != null)
                {
                    ProcessCategory = (ProcessCategories)Enum.Parse(typeof(ProcessCategories), node.Attributes["category"].Value);
                    visionLights.Clear();

                    foreach (XmlNode child_ in node.ChildNodes)
                    {
                        switch (child_.Name.ToLower())
                        {
                            case "tasks":
                                {
                                    foreach (XmlNode element_ in child_.ChildNodes)
                                    {
                                        visionTasks.Add(Convert.ToInt32(element_.Attributes["id"].Value),
                                            new Pair<string, ImageProcessTaskTypes>(element_.Attributes["name"].Value, 
                                            (ImageProcessTaskTypes)Enum.Parse(typeof(ImageProcessTaskTypes), element_.Attributes["type"].Value)));
                                    }
                                }
                                break;
                            case "lights":
                                {
                                    try
                                    {
                                        dynamicLightControl = bool.Parse(child_.Attributes["dynamic"].Value);
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                                        dynamicLightControl = false;
                                    }
                                    finally
                                    {
                                        foreach (XmlNode element_ in child_.ChildNodes)
                                        {
                                            switch (element_.Name.ToLower())
                                            {
                                                case "category":
                                                    {
                                                        if (element_.HasChildNodes)
                                                        {
                                                            VisionLightObject light = new VisionLightObject(Convert.ToInt32(element_.Attributes["id"].Value), element_.Attributes["name"].Value);

                                                            foreach (XmlNode item_ in element_.ChildNodes)
                                                            {
                                                                light.SetValue(Convert.ToInt32(item_.Attributes["channel"].Value), Convert.ToInt32(item_.Attributes["value"].Value), Convert.ToInt32(item_.Attributes["height"].Value));
                                                            }

                                                            if (light.Lights.Count > 0)
                                                                visionLights.Add(light);
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                }
                                break;
                            case "alignmentrangelimit":
                                {
                                    foreach (XmlNode item_ in child_.ChildNodes)
                                    {
                                        switch (item_.Name.ToLower())
                                        {
                                            case "x":
                                                {
                                                    alignmentRangeLimit.X = int.Parse(item_.InnerText);
                                                }
                                                break;
                                            case "y":
                                                {
                                                    alignmentRangeLimit.Y = int.Parse(item_.InnerText);
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case "calibration":
                                {
                                    foreach (XmlNode item_ in child_.ChildNodes)
                                    {
                                        Model.CalibrationSpecifications spec_ = Model.CalibrationSpecifications.Unknown;
                                        Dictionary<int, Coord3DField<double, double, double, double, double, double>> vl_ = null;
                                        int id_ = -1, count_ = 1;
                                        double x_ = 0, y_ = 0, z_ = 0, rz_ = 0;
                                        string name_ = string.Empty;

                                        switch (item_.Name.ToLower())
                                        {
                                            case "specification":
                                                {
                                                    name_ = item_.Attributes["name"].Value.ToLower();

                                                    switch (name_)
                                                    {
                                                        case "towerbasepoint":
                                                            {
                                                                spec_ = Model.CalibrationSpecifications.TowerBasePoint;
                                                            }
                                                            break;
                                                        case "cart7guidepoint":
                                                            {
                                                                spec_ = Model.CalibrationSpecifications.Cart7GuidePoint;
                                                            }
                                                            break;
                                                        case "cart13guidepoint":
                                                            {
                                                                spec_ = Model.CalibrationSpecifications.Cart13GuidePoint;
                                                            }
                                                            break;
                                                    }

                                                    foreach (XmlAttribute attr_ in item_.Attributes)
                                                    {
                                                        switch (attr_.Name.ToLower())
                                                        {
                                                            case "count":
                                                                {
                                                                    count_ = (count_ = int.Parse(attr_.Value)) <= 0 ? 1 : count_; 
                                                                }
                                                                break;
                                                            case "x":
                                                                {
                                                                    x_ = double.Parse(attr_.Value);
                                                                }
                                                                break;
                                                            case "y":
                                                                {
                                                                    y_ = double.Parse(attr_.Value);
                                                                }
                                                                break;
                                                            case "z":
                                                                {
                                                                    z_ = double.Parse(attr_.Value);
                                                                }
                                                                break;
                                                            case "rz":
                                                                {
                                                                    rz_ = double.Parse(attr_.Value);
                                                                }
                                                                break;
                                                        }
                                                    }

                                                    if (spec_ != Model.CalibrationSpecifications.Unknown)
                                                    {
                                                        if (!calibrationspecs.ContainsKey(spec_))
                                                            calibrationspecs.Add(spec_, new Pair<int, Coord3DField<double, double, double, double, double, double>>(count_, new Coord3DField<double, double, double, double, double, double>(x_, y_, z_, 0, 0, rz_)));
                                                    }
                                                }
                                                break;
                                            case "parameter":
                                                {
                                                    id_ = Convert.ToInt32(item_.Attributes["id"].Value);
                                                    name_ = item_.Attributes["name"].Value.ToLower();

                                                    switch (name_)
                                                    {
                                                        case "towerbasepoint":
                                                            {
                                                                towerBasePoints.Add(id_, new Coord3DField<double, double, double, double, double, double>(0, 0, 0, 0, 0, 0));
                                                                vl_ = towerBasePoints;
                                                            }
                                                            break;
                                                        case "cart7guidepoint":
                                                            {
                                                                cart7GuidePoints.Add(id_, new Coord3DField<double, double, double, double, double, double>(0, 0, 0, 0, 0, 0));
                                                                vl_ = cart7GuidePoints;
                                                            }
                                                            break;
                                                        case "cart13guidepoint":
                                                            {
                                                                cart13GuidePoints.Add(id_, new Coord3DField<double, double, double, double, double, double>(0, 0, 0, 0, 0, 0));
                                                                vl_ = cart13GuidePoints;
                                                            }
                                                            break;
                                                    }

                                                    foreach (XmlAttribute attr_ in item_.Attributes)
                                                    {
                                                        switch (attr_.Name.ToLower())
                                                        {
                                                            case "x":
                                                                {
                                                                    x_ = double.Parse(attr_.Value);
                                                                }
                                                                break;
                                                            case "y":
                                                                {
                                                                    y_ = double.Parse(attr_.Value);
                                                                }
                                                                break;
                                                            case "z":
                                                                {
                                                                    z_ = double.Parse(attr_.Value);
                                                                }
                                                                break;
                                                            case "rz":
                                                                {
                                                                    rz_ = double.Parse(attr_.Value);
                                                                }
                                                                break;
                                                        }
                                                    }

                                                    if (vl_ != null)
                                                    {
                                                        vl_[id_].first = x_;
                                                        vl_[id_].second = y_;
                                                        vl_[id_].third = z_;
                                                        vl_[id_].sixth = rz_;
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case "failureretry":
                                {
                                    visionFailureRetry = int.Parse(child_.InnerText);
                                }
                                break;
                            case "visionattempts":
                                {
                                    visionRetryAttempts = int.Parse(child_.InnerText);
                                }
                                break;
                            case "imageprocessingtimeout":
                                {
                                    ImageProcessingTimeout = int.Parse(child_.InnerText);
                                }
                                break;
                            case "delayoftrigger":
                                {
                                    delayOfTrigger = int.Parse(child_.InnerText);
                                }
                                break;
                        }
                    }

                    result_ = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }

            return result_;
        }

        public void Save(ref XElement root)
        {
            if (visionTasks.Count > 0)
            {
                XElement imageProcess = new XElement("Process", 
                    new XAttribute("category", ProcessCategory));

                if (visionLights.Count > 0)
                {
                    XElement lights = new XElement("Lights", new XAttribute("dynamic", dynamicLightControl));

                    foreach (VisionLightObject item in visionLights)
                    {
                        if (item.Lights.Count > 0)
                        {
                            XElement cat = new XElement("Category",
                                new XAttribute("id", item.Id),
                                new XAttribute("name", item.Name));

                            foreach (Pair<int, VisionLightObject.LightValue> light in item.Lights)
                            {
                                cat.Add(new XElement("Light",
                                    new XAttribute("channel", light.first),
                                    new XAttribute("value", light.second.value),
                                    new XAttribute("height", light.second.height)));
                            }

                            lights.Add(cat);
                        }
                    }

                    imageProcess.Add(lights);
                }

                if (visionTasks.Count > 0)
                {
                    XElement items = new XElement("Tasks");

                    foreach (KeyValuePair<int, Pair<string, ImageProcessTaskTypes>> item in visionTasks)
                    {
                        XElement cat = new XElement("Task",
                            new XAttribute("id", item.Key),
                            new XAttribute("name", item.Value.first),
                            new XAttribute("type", item.Value.second));

                        items.Add(cat);
                    }

                    imageProcess.Add(items);
                }

                imageProcess.Add(new XElement("AlignmentRangeLimit",
                        new XElement("X", Model.AlignmentRangeLimit.X),
                        new XElement("Y", Model.AlignmentRangeLimit.Y)));

                if (calibrationspecs.Count > 0 || towerBasePoints.Count > 0 || cart7GuidePoints.Count > 0 || cart13GuidePoints.Count > 0)
                {
                    XElement cal_ = new XElement("Calibration");

                    foreach (var item_ in calibrationspecs)
                    {
                        cal_.Add(new XElement("Specification",
                            new XAttribute("name", item_.Key),
                            new XAttribute("count", item_.Value.first),
                            new XAttribute("x", item_.Value.second.first),
                            new XAttribute("y", item_.Value.second.second),
                            new XAttribute("z", item_.Value.second.third),
                            new XAttribute("rz", item_.Value.second.sixth)));
                    }

                    foreach (var item_ in towerBasePoints)
                    {
                        cal_.Add(new XElement("Parameter",
                            new XAttribute("name", "TowerBasePoint"),
                            new XAttribute("id", item_.Key),
                            new XAttribute("x", item_.Value.first),
                            new XAttribute("y", item_.Value.second),
                            new XAttribute("z", item_.Value.third),
                            new XAttribute("rz", item_.Value.sixth)));
                    }

                    foreach (var item_ in cart7GuidePoints)
                    {
                        cal_.Add(new XElement("Parameter",
                            new XAttribute("name", "Cart7GuidePoint"),
                            new XAttribute("id", item_.Key),
                            new XAttribute("x", item_.Value.first),
                            new XAttribute("y", item_.Value.second),
                            new XAttribute("z", item_.Value.third),
                            new XAttribute("rz", item_.Value.sixth)));
                    }

                    foreach (var item_ in cart13GuidePoints)
                    {
                        cal_.Add(new XElement("Parameter",
                            new XAttribute("name", "Cart13GuidePoint"),
                            new XAttribute("id", item_.Key),
                            new XAttribute("x", item_.Value.first),
                            new XAttribute("y", item_.Value.second),
                            new XAttribute("z", item_.Value.third),
                            new XAttribute("rz", item_.Value.sixth)));
                    }

                    imageProcess.Add(cal_);
                }

                imageProcess.Add(new XElement("FailureRetry", Model.RetryOfVisionFailure));
                imageProcess.Add(new XElement("VisionAttempts", Model.RetryOfVisionAttempts));
                imageProcess.Add(new XElement("ImageProcessingTimeout", Model.TimeoutOfImageProcessing));
                imageProcess.Add(new XElement("DelayOfTrigger", Model.DelayOfTrigger));

                root.Add(imageProcess);
            }
        }
        #endregion
    }

    public class ImageProcessTaskObject
    {
        #region Fields
        public ImageProcessTaskRoi Roi = null;

        public ImageProcessTaskConstraint Constraint = null;

        public List<Pair<string, ImageProcessTaskMask>> Masks = new List<Pair<string, ImageProcessTaskMask>>();

        public List<Pair<string, ImageProcessTaskPattern>> Patterns = new List<Pair<string, ImageProcessTaskPattern>>();

        public List<Pair<int, int>> Lights = new List<Pair<int, int>>();
        #endregion

        #region Properties
        public bool Correction
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public double MinimumScale
        {
            get;
            set;
        }


        public double MaximumScale
        {
            get;
            set;
        }

        public double MinimumTheta
        {
            get;
            set;
        }

        public double MaximumTheta
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public ImageProcessTaskTypes TaskType
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public ImageProcessTaskObject(XmlNode node)
        {
            Load(node);
        }
        #endregion

        #region Protected methods
        #endregion

        #region Public methods
        public void Load(XmlNode node)
        {
            try
            {
                Lights.Clear();

                foreach (XmlAttribute attr_ in node.Attributes)
                {
                    switch (attr_.Name.ToLower())
                    {
                        case "id":
                            {
                                Id = int.Parse(attr_.Value);
                            }
                            break;
                        case "minscale":
                            {
                                MinimumScale = double.Parse(attr_.Value);
                            }
                            break;
                        case "maxscale":
                            {
                                MaximumScale = double.Parse(attr_.Value);
                            }
                            break;
                        case "mintheta":
                            {
                                MinimumTheta = double.Parse(attr_.Value);
                            }
                            break;
                        case "maxtheta":
                            {
                                MaximumTheta = double.Parse(attr_.Value);
                            }
                            break;
                        case "correction":
                            {
                                Correction = bool.Parse(attr_.Value);
                            }
                            break;
                    }
                }
                
                foreach (XmlNode child_ in node.ChildNodes)
            
                {
                        switch (child_.Name.ToLower())
                        {
                        case "roi":
                            {
                                Roi = new ImageProcessTaskRoi(child_);
                            }
                            break;
                        case "mask":
                            {
                                foreach (XmlNode element_ in  child_.ChildNodes)
                                    Masks.Add(new Pair<string, ImageProcessTaskMask>(element_.Attributes["name"].Value, new ImageProcessTaskMask(element_)));
                            }
                            break;
                        case "patterns":
                            {
                                foreach (XmlNode element_ in child_.ChildNodes)
                                    Patterns.Add(new Pair<string, ImageProcessTaskPattern>(element_.Attributes["name"].Value, new ImageProcessTaskPattern(element_)));
                            }
                            break;
                        case "lights":
                            {
                                foreach (XmlNode element_ in child_.ChildNodes)
                                    Lights.Add(new Pair<int, int>(int.Parse(element_.Attributes["channel"].Value), int.Parse(element_.Attributes["value"].Value)));
                            }
                            break;
                        case "constraints":
                            {
                                Constraint = new ImageProcessTaskConstraint(child_);
                            }
                            break;
                }
                  }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }
        }

        public void Save(ref XElement root)
        {
            XElement xml = new XElement("Task",
                new XAttribute("id", Id),
                new XAttribute("type", TaskType),
                new XAttribute("minscale", MinimumScale),
                new XAttribute("maxscale", MaximumScale),
                new XAttribute("mintheta", MinimumTheta),
                new XAttribute("maxtheta", MaximumTheta),
                new XAttribute("correction", Correction));

            if (Roi != null)
                Roi.Save(ref xml);

            if (Masks.Count > 0)
            {
                XElement element = new XElement("Masks");

                foreach (var item in Masks)
                    item.second.Save(ref element);

                xml.Add(element);
            }

            if (Patterns.Count > 0)
            {
                XElement element = new XElement("Patterns");

                foreach (var item in Patterns)
                    item.second.Save(ref element);

                xml.Add(element);
            }

            if (Lights.Count > 0)
            {
                XElement element = new XElement("Lights");

                foreach (var item in Lights)
                {
                    element.Add(new XElement("Light",
                        new XAttribute("channel", item.first),
                        new XAttribute("value", item.second)));
                }

                xml.Add(element);
            }

            if (Constraint != null)
                Constraint.Save(ref xml);

            root.Add(xml);
        }
        #endregion
    }

    public class ImageProcessTaskRoi
    {
        #region Fields
        public List<List<Pair<string, int>>> Regions = new List<List<Pair<string, int>>>();
        #endregion

        #region Properties
        public int Id
        {
            get;
            set;
        }

        public int Channel
        {
            get;
            set;
        }

        public string DeviceName
        {
            get;
            set;
        }

        public ImageProcessRoiTypes RoiType
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public ImageProcessTaskRoi(XmlNode node)
        {
            Load(node);
        }
        #endregion

        #region Public methods
        public void Load(XmlNode node)
        {
            try
            {
                RoiType = ImageProcessRoiTypes.WholeArea;
                Regions.Clear();

                if (node != null)
                {
                    foreach (XmlAttribute attr_ in node.Attributes)
                    {
                        switch (attr_.Name.ToLower())
                        {
                            case "device":
                                {
                                    DeviceName = attr_.Value;
                                }
                                break;
                            case "channel":
                                {
                                    Channel = int.Parse(attr_.Value);
                                }
                                break;
                            case "id":
                                {
                                    Id = int.Parse(attr_.Value);
                                }
                                break;
                            case "type":
                                {
                                    RoiType = (ImageProcessRoiTypes)Enum.Parse(typeof(ImageProcessRoiTypes), attr_.Value);
                                }
                                break;
                        }
                    }

                    if (node.HasChildNodes)
                    {
                        foreach (XmlNode child_ in node.ChildNodes)
                        {
                            switch (child_.Name.ToLower())
                            {
                                case "regions":
                                    {
                                        foreach (XmlNode element_ in child_.ChildNodes)
                                        {
                                            if (element_.Name.ToLower() == "region")
                                            {
                                                Regions.Add(new List<Pair<string, int>>());

                                                foreach (XmlNode point_ in element_.ChildNodes)
                                                {
                                                    if (point_.Attributes.GetNamedItem("name") != null)
                                                    {
                                                        Regions[Regions.Count - 1].Add(
                                                            new Pair<string, int>(
                                                                point_.Attributes["name"].Value,
                                                                int.Parse(point_.InnerText)));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }
        }

        public void Save(ref XElement root)
        {
            int index = 1;

            XElement xml = new XElement("Roi", 
                new XAttribute("device", DeviceName),
                new XAttribute("channel", Channel),
                new XAttribute("id", Id),
                new XAttribute("type", RoiType));

            if (Regions.Count > 0)
            {
                XElement element = new XElement("Regions");

                foreach (var region in Regions)
                {
                    XElement child = new XElement("Region",
                        new XAttribute("id", index++));

                    foreach (var point in region)
                    {
                        child.Add(new XElement("Point",
                            new XAttribute("name", point.first),
                            point.second));
                    }

                    element.Add(child);
                }

                xml.Add(element);
            }

            root.Add(xml);
        }
        #endregion
    }

    public class ImageProcessTaskMask
    {
        #region Properties
        public string Name
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public ImageProcessArithmeticOperationTypes ArithmeticOperation
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public ImageProcessTaskMask(XmlNode node)
        {
            Load(node);
        }
        #endregion

        #region Public methods
        public void Load(XmlNode node)
        {
            try
            {
                if (node != null)
                {
                    foreach (XmlAttribute attr_ in node.Attributes)
                    {
                        switch (attr_.Name.ToLower())
                        {
                            case "name":
                                {
                                    Name = attr_.Value;
                                }
                                break;
                            case "path":
                                {
                                    Path = attr_.Value;
                                }
                                break;
                            case "operation":
                                {
                                    ArithmeticOperation = (ImageProcessArithmeticOperationTypes)Enum.Parse(typeof(ImageProcessArithmeticOperationTypes), attr_.Value);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }
        }

        public void Save(ref XElement root)
        {
            XElement xml = new XElement("Mask",
                new XAttribute("name", Name),
                new XAttribute("path", Path),
                new XAttribute("operation", ArithmeticOperation));

            root.Add(xml);
        }
        #endregion
    }

    public class ImageProcessTaskPattern
    {
        #region Properties
        public string Name
        {
            get;
            set;
        }

        public double Theta
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public ImageProcessTaskPattern(XmlNode node)
        {
            Load(node);
        }
        #endregion

        #region Public methods
        public void Load(XmlNode node)
        {
            try
            {
                if (node != null)
                {
                    foreach (XmlAttribute attr_ in node.Attributes)
                    {
                        switch (attr_.Name.ToLower())
                        {
                            case "name":
                                {
                                    Name = attr_.Value;
                                }
                                break;
                            case "path":
                                {
                                    Path = attr_.Value;
                                }
                                break;
                            case "theta":
                                {
                                    Theta = double.Parse(attr_.Value);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }
        }

        public void Save(ref XElement root)
        {
            XElement xml = new XElement("Pattern",
                new XAttribute("name", Name),
                new XAttribute("path", Path),
                new XAttribute("theta", Theta));

            root.Add(xml);
        }
        #endregion
    }

    public class ImageProcessTaskConstraint
    {
        #region Properties
        public int Diameter
        {
            get;
            set;
        }

        public int NumberOfItem
        {
            get;
            set;
        }

        public ImageProcessConstraintRegionTypes ConstraintRegionType
        {
            get;
            set;
        }

        public ImageProcessConstraintOperation Operation
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public ImageProcessTaskConstraint(XmlNode node)
        {
            Load(node);
        }
        #endregion

        #region Public methods
        public void Load(XmlNode node)
        {
            try
            {
                if (node != null)
                {
                    foreach (XmlAttribute attr_ in node.Attributes)
                    {
                        switch (attr_.Name.ToLower())
                        {
                            case "diameter":
                                {
                                    Diameter = int.Parse(attr_.Value);
                                }
                                break;
                            case "numberofitem":
                                {
                                    NumberOfItem = int.Parse(attr_.Value);
                                }
                                break;
                            case "region":
                                {
                                    ConstraintRegionType = (ImageProcessConstraintRegionTypes)Enum.Parse(typeof(ImageProcessConstraintRegionTypes), attr_.Value);
                                }
                                break;
                            case "operation":
                                {
                                    Operation = (ImageProcessConstraintOperation)Enum.Parse(typeof(ImageProcessConstraintOperation), attr_.Value);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }
        }

        public void Save(ref XElement root)
        {
            XElement xml = new XElement("Constraint",
                new XAttribute("region", ConstraintRegionType),
                new XAttribute("diameter", Diameter),
                new XAttribute("numberofitem", NumberOfItem),
                new XAttribute("operation", Operation));

            root.Add(xml);
        }
        #endregion
    }
}
#endregion
