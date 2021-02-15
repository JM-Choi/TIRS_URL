#region Imports
using TechFloor.Components;
using TechFloor.Components.Elements;
using TechFloor.Object;
using TechFloor.Service.MYCRONIC.WebService;
using TechFloor.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Linq;
#endregion

#region Program
namespace TechFloor
{
    public class Config
    {
        #region Constants
        protected const int CONST_DEFAULT_COMMUNIVATION_RESPONSE_TIMEOUT = 1000;

        protected const int CONST_DEFAULT_CONVERSATION_RETRY_LIMIT = 3;
        #endregion

        #region Fields        
        protected static ReelTowerStateControlModes reelTowerStateControlMode = ReelTowerStateControlModes.QueryReelTowerStateByAtOnce;

        protected static Dictionary<int, string> reelTowerIds = new Dictionary<int, string>();

        protected static Dictionary<int, string> outStageIds = new Dictionary<int, string>();

        protected static Dictionary<int, string> returnStageIds = new Dictionary<int, string>();

        protected static Dictionary<int, string> rejectStageIds = new Dictionary<int, string>();

        protected static Dictionary<string, Pair<string, bool>> processes = new Dictionary<string, Pair<string, bool>>();

        protected static Dictionary<string, PropertyNode> properties = new Dictionary<string, PropertyNode>();

        protected static List<NetworkNode> networks = new List<NetworkNode>();

        protected static List<DeviceNode> devices = new List<DeviceNode>();

        #endregion

        #region Properties
        public static bool SystemSimulation
        {
            get
            {
                if (properties.ContainsKey("SystemSimulation"))
                    return Convert.ToBoolean(properties["SystemSimulation"].Value);
                return false;
            }
            set
            {
                if (properties.ContainsKey("SystemSimulation"))
                    properties["SystemSimulation"].Value = value;
            }
        }

        public static bool OneshotRecovery
        {
            get
            {
                if (properties.ContainsKey("OneshotRecovery"))
                    return Convert.ToBoolean(properties["OneshotRecovery"].Value);
                return false;
            }
            set
            {
                if (properties.ContainsKey("OneshotRecovery"))
                    properties["OneshotRecovery"].Value = value;
            }
        }

        public static bool HomeAtIdleTime
        {
            get
            {
                if (properties.ContainsKey("HomeAtIdleTime"))
                    return Convert.ToBoolean(properties["HomeAtIdleTime"].Value);
                return false;
            }
            set
            {
                if (properties.ContainsKey("HomeAtIdleTime"))
                    properties["HomeAtIdleTime"].Value = value;
            }
        }

        public static bool ReelHandlerUsage
        {
            get
            {
                if (properties.ContainsKey("ReelHandlerUsage"))
                    return Convert.ToBoolean(properties["ReelHandlerUsage"].Value);
                return false;
            }
            set
            {
                if (properties.ContainsKey("ReelHandlerUsage"))
                    properties["ReelHandlerUsage"].Value = value;
            }
        }

        public static bool AMMUsage
        {
            get
            {
                if (properties.ContainsKey("AMMUsage"))
                    return Convert.ToBoolean(properties["AMMUsage"].Value);
                return false;
            }
            set
            {
                if (properties.ContainsKey("AMMUsage"))
                    properties["AMMUsage"].Value = value;
            }
        }

        public static bool RejectAutoUsage
        {
            get
            {
                if (properties.ContainsKey("RejectAutoUsage"))
                    return Convert.ToBoolean(properties["RejectAutoUsage"].Value);
                return false;
            }
            set
            {
                if (properties.ContainsKey("RejectAutoUsage"))
                    properties["RejectAutoUsage"].Value = value;
            }
        }

        public static bool AMMWebserviceUsage
        {
            get
            {
                if (properties.ContainsKey("AMMWebserviceUsage"))
                    return Convert.ToBoolean(properties["AMMWebserviceUsage"].Value);
                return false;
            }
            set
            {
                if (properties.ContainsKey("AMMWebserviceUsage"))
                    properties["AMMWebserviceUsage"].Value = value;
            }
        }
        public static int LoadDelayTimeByManual
        {
            get
            {
                if (properties.ContainsKey("LoadDelayTimeByManual"))
                    return Convert.ToInt32(properties["LoadDelayTimeByManual"].Value);
                return 10;
            }
            set
            {
                if (properties.ContainsKey("LoadDelayTimeByManual"))
                    properties["LoadDelayTimeByManual"].Value = value;
            }
        }

        public static int IntervalOfReelHandlerPing
        {
            get
            {
                if (properties.ContainsKey("IntervalOfReelHandlerPing"))
                    return Convert.ToInt32(properties["IntervalOfReelHandlerPing"].Value);
                return CONST_DEFAULT_COMMUNIVATION_RESPONSE_TIMEOUT * 10;
            }
            set
            {
                if (properties.ContainsKey("IntervalOfReelHandlerPing"))
                    properties["IntervalOfReelHandlerPing"].Value = value;
            }
        }

        public static int TimeoutOfReelHandlerResponse
        {
            get
            {
                if (properties.ContainsKey("TimeoutOfReelHandlerResponse"))
                    return Convert.ToInt32(properties["TimeoutOfReelHandlerResponse"].Value);
                return CONST_DEFAULT_COMMUNIVATION_RESPONSE_TIMEOUT;
            }
            set
            {
                if (properties.ContainsKey("TimeoutOfReelHandlerResponse"))
                    properties["TimeoutOfReelHandlerResponse"].Value = value;
            }
        }

        public static int LimitOfRetry
        {
            get
            {
                if (properties.ContainsKey("LimitOfRetry"))
                    return Convert.ToInt32(properties["LimitOfRetry"].Value);
                return CONST_DEFAULT_CONVERSATION_RETRY_LIMIT;
            }
            set
            {
                if (properties.ContainsKey("LimitOfRetry"))
                    properties["LimitOfRetry"].Value = value;
            }
        }

        public static int JobSplitReelCount
        {
            get
            {
                if (properties.ContainsKey("JobSplitReelCount"))
                    return Convert.ToInt32(properties["JobSplitReelCount"].Value);
                return 20;
            }
            set
            {
                if (properties.ContainsKey("JobSplitReelCount"))
                    properties["JobSplitReelCount"].Value = value;
            }
        }

        public static int TimeoutOfReject
        {
            get
            {
                if (properties.ContainsKey("TimeoutOfReject"))
                    return Convert.ToInt32(properties["TimeoutOfReject"].Value);
                return 60;
            }
            set
            {
                if (properties.ContainsKey("TimeoutOfReject"))
                    properties["TimeoutOfReject"].Value = value;
            }
        }

        public static bool RemapCreateTimeByMFG
        {
            get
            {
                if (properties.ContainsKey("RemapCreateTimeByMFG"))
                    return Convert.ToBoolean(properties["RemapCreateTimeByMFG"].Value);
                return true;
            }
            set
            {
                if (properties.ContainsKey("RemapCreateTimeByMFG"))
                    properties["RemapCreateTimeByMFG"].Value = value;
            }
        }
    
        public static int AssignedRejectPort
        {
            get
            {
                if (properties.ContainsKey("AssignedRejectPort"))
                    return Convert.ToInt32(properties["AssignedRejectPort"].Value);
                return 7;
            }
            set
            {
                if (properties.ContainsKey("AssignedRejectPort"))
                    properties["AssignedRejectPort"].Value = value;
            }
        }

        public static string ProvideModeByString
        {
            get
            {
                if (properties.ContainsKey("ProvideMode"))
                    return Convert.ToString(properties["ProvideMode"].Value);
                return ProvideModes.ByCreateTime.ToString();
            }
            set
            {
                if (properties.ContainsKey("ProvideMode"))
                    properties["ProvideMode"].Value = value;
            }
        }

        public static ProvideModes ProvideMode
        {
            get
            {
                if (properties.ContainsKey("ProvideMode"))
                    return (ProvideModes)Enum.Parse(typeof(ProvideModes), properties["ProvideMode"].Value.ToString());
                return ProvideModes.ByCreateTime;
            }
            set
            {
                if (properties.ContainsKey("ProvideMode"))
                    properties["ProvideMode"].Value = value;
            }
        }

        public static int ArticleNameLength
        {
            get
            {
                if (properties.ContainsKey("ArticleNameLength"))
                    return Convert.ToInt32(properties["ArticleNameLength"].Value);
                return 9;
            }
            set
            {
                if (properties.ContainsKey("ArticleNameLength"))
                    properties["ArticleNameLength"].Value = value;
            }
        }

        public static int ReelHandlerPort
        {
            get
            {
                NetworkNode node_ = networks.Find(x => x.Name == "ReelHandlerServer");

                if (node_ == null)
                    return 0;

                return node_.Endpoint.Port;
            }
        }

        public static IPEndPoint ReelHandlerEndPoint
        {
            get
            {
                NetworkNode node_ = networks.Find(x => x.Name == "ReelHandlerServer");

                if (node_ == null)
                    return null;

                return node_.Endpoint;
            }
        }

        public static string ReelHandlerAddress
        {
            get
            {
                NetworkNode node_ = networks.Find(x => x.Name == "ReelHandlerServer");

                if (node_ == null)
                    return string.Empty;

                return node_.Endpoint.Address.ToString();
            }
        }

        public static string Bcr1Vid
        {
            get
            {
                DeviceNode node_ = devices.Find(x => x.Name == "Bcr1" && x.Category == Device.PhysicalDevices.BarcodeScanner);;

                if (node_ != null)
                    return node_.Vid;
                return string.Empty;
            }
        }
        
        public static string Bcr1Pid
        {
            get
            {
                DeviceNode node_ = devices.Find(x => x.Name == "Bcr1" && x.Category == Device.PhysicalDevices.BarcodeScanner);

                if (node_ != null)
                    return node_.Pid;
                return string.Empty;
            }
        }

        public static string Bcr1Mid
        {
            get
            {
                DeviceNode node_ = devices.Find(x => x.Name == "Bcr1" && x.Category == Device.PhysicalDevices.BarcodeScanner);

                if (node_ != null)
                    return node_.Mid;
                return string.Empty;
            }
        }

        public static bool IsAvailableProcess(string name)                  => processes[name] == null ? false : processes[name].second;

        public static string GetProcessModel(string name)                   => processes[name] == null ? string.Empty : processes[name].first;

        public static ReelTowerStateControlModes ReelTowerStateControlMode  => reelTowerStateControlMode;
        
        public static IReadOnlyDictionary<int, string> ReelTowerIds         => reelTowerIds;
        
        public static IReadOnlyDictionary<int, string> OutStageIds          => outStageIds;
        
        public static IReadOnlyDictionary<int, string> ReturnStageIds       => returnStageIds;
        
        public static IReadOnlyDictionary<int, string> RejectStageIds       => rejectStageIds;

        public static IReadOnlyList<NetworkNode> Networks                   => networks;

        public static IReadOnlyList<DeviceNode> Devices                     => devices;

        public static IReadOnlyDictionary<string, PropertyNode> Properties  => properties;
        #endregion

        #region Constructors
        public Config()
        {
            Load();
        }
        #endregion

        #region Static methods
        public static void Clear()
        {
            returnStageIds.Clear();
            rejectStageIds.Clear();
            outStageIds.Clear();
            processes.Clear();
            networks.Clear();
            devices.Clear();
        }

        public static void Save(string filename = "Config.xml")
        {
            try
            {
                string filePath_ = $@"{App.Path}Config\{(string.IsNullOrEmpty(filename) ? "Config.xml" : filename)}";

                XElement result_ = new XElement("Config",
                    new XAttribute("version", App.Version),
                    new XAttribute("culture", App.CultureInfoCode));

                XElement nodes_ = null;

                if (properties.Count > 0)
                {
                    nodes_ = new XElement("Properties");

                    foreach (KeyValuePair<string, PropertyNode> node_ in properties)
                        nodes_.Add(node_.Value.ToXml("Property"));

                    result_.Add(nodes_);
                }

                if (processes.Count > 0)
                {
                    nodes_ = new XElement("Processes");

                    foreach (KeyValuePair<string, Pair<string, bool>> node_ in processes)
                    {
                        nodes_.Add(new XElement("Process",
                            new XAttribute("name", node_.Key),
                            new XAttribute("enabled", node_.Value.second),
                            new XAttribute("type", node_.Value.first)));
                    }

                    result_.Add(nodes_);
                }

                if (networks.Count > 0)
                {
                    nodes_ = new XElement("Networks");

                    foreach (NetworkNode node_ in networks)
                        nodes_.Add(node_.ToXml("Node"));

                    result_.Add(nodes_);
                }

                if (devices.Count > 0)
                {
                    nodes_ = new XElement("Devices");

                    foreach (DeviceNode node_ in devices)
                        nodes_.Add(node_.ToXml("Device"));

                    result_.Add(nodes_);
                }

                if (returnStageIds.Count > 0)
                {
                    nodes_ = new XElement("ReturnStages");

                    foreach (KeyValuePair<int, string> node_ in returnStageIds)
                        nodes_.Add(new XElement("Stage",
                            new XAttribute("id", node_.Key),
                            new XAttribute("name", node_.Value)));

                    result_.Add(nodes_);
                }

                if (rejectStageIds.Count > 0)
                {
                    nodes_ = new XElement("RejectStages");

                    foreach (KeyValuePair<int, string> node_ in rejectStageIds)
                        nodes_.Add(new XElement("Stage",
                            new XAttribute("id", node_.Key),
                            new XAttribute("name", node_.Value)));

                    result_.Add(nodes_);
                }

                if (outStageIds.Count > 0)
                {
                    nodes_ = new XElement("OutStages");

                    foreach (KeyValuePair<int, string> node_ in outStageIds)
                        nodes_.Add(new XElement("Stage",
                            new XAttribute("id", node_.Key),
                            new XAttribute("name", node_.Value)));

                    result_.Add(nodes_);
                }

                if (Singleton<ReelTowerGroup>.Instance != null)
                    result_.Add(Singleton<ReelTowerGroup>.Instance.ToXml());

                if (Singleton<CombineModuleManager>.Instance != null)
                    result_.Add(Singleton<CombineModuleManager>.Instance.ToXml());

                result_.Save(filePath_);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Config.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }
        
        public static bool Load(string filename = "Config.xml")
        {
            int id_;
            bool result_ = false, usage_;            
            string name_, category_, model_, culture_;

            try
            {
                string filePath_ = $@"{App.Path}Config\{(string.IsNullOrEmpty(filename)? "Config.xml" : filename)}";

                if (!File.Exists(filePath_))
                    return false;

                Clear();

                XmlDocument xml = new XmlDocument();
                xml.Load(filePath_);

                if (App.Version == xml.DocumentElement.GetAttribute("version"))
                {
                    culture_ = xml.DocumentElement.GetAttribute("culture");

                    foreach (XmlNode element_ in xml.DocumentElement.ChildNodes)
                    {
                        switch (element_.Name.ToLower())
                        {
                            case "properties":
                                {
                                    foreach (XmlNode child_ in element_.ChildNodes)
                                    {
                                        PropertyNode property_ = new PropertyNode(child_);

                                        if (property_.IsValid)
                                            properties.Add(property_.Name, property_);
                                    }
                                }
                                break;
                            case "processes":
                                {
                                    foreach (XmlElement child_ in element_.ChildNodes)
                                    {
                                        category_ = string.Empty;
                                        model_ = string.Empty;
                                        usage_ = false;

                                        foreach (XmlAttribute attr_ in child_.Attributes)
                                        {
                                            switch (attr_.Name.ToLower())
                                            {
                                                case "category":
                                                    category_ = attr_.Value;
                                                    break;
                                                case "model":
                                                    model_ = attr_.Value;
                                                    break;
                                                case "usage":
                                                    usage_ = bool.Parse(attr_.Value);
                                                    break;
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(category_) && !processes.ContainsKey(category_))
                                            processes.Add(category_, new Pair<string, bool>(model_, usage_));
                                    }
                                }
                                break;
                            case "reeltowergroup":
                                {
                                    Singleton<ReelTowerGroup>.Instance.Create(element_, culture_);
                                }
                                break;
                            case "combinemodulemanager":
                                {
                                    Singleton<CombineModuleManager>.Instance.Create(element_);
                                }
                                break;
                            case "returnstages":
                                {
                                    foreach (XmlNode child_ in element_.ChildNodes)
                                    {
                                        if (child_.Name.ToLower() == "stage")
                                        {
                                            if (!string.IsNullOrEmpty(child_.Attributes["id"].Value))
                                            {
                                                id_ = Convert.ToInt32(child_.Attributes["id"].Value);
                                                name_ = child_.Attributes["name"].Value;

                                                if (returnStageIds.ContainsKey(id_))
                                                    return result_;
                                                else
                                                    returnStageIds.Add(id_, name_);
                                            }
                                        }
                                    }
                                }
                                break;
                            case "rejectstages":
                                {
                                    foreach (XmlNode child_ in element_.ChildNodes)
                                    {
                                        if (child_.Name.ToLower() == "stage")
                                        {
                                            if (!string.IsNullOrEmpty(child_.Attributes["id"].Value))
                                            {
                                                if (!string.IsNullOrEmpty(child_.Attributes["id"].Value))
                                                {
                                                    id_ = Convert.ToInt32(child_.Attributes["id"].Value);
                                                    name_ = child_.Attributes["name"].Value;

                                                    if (rejectStageIds.ContainsKey(id_))
                                                        return result_;
                                                    else
                                                        rejectStageIds.Add(id_, name_);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case "outstages":
                                {
                                    foreach (XmlNode child_ in element_.ChildNodes)
                                    {
                                        if (child_.Name.ToLower() == "stage")
                                        {
                                            if (!string.IsNullOrEmpty(child_.Attributes["id"].Value))
                                            {
                                                if (!string.IsNullOrEmpty(child_.Attributes["id"].Value))
                                                {
                                                    id_ = Convert.ToInt32(child_.Attributes["id"].Value);
                                                    name_ = child_.Attributes["name"].Value;

                                                    if (outStageIds.ContainsKey(id_))
                                                        return result_;
                                                    else
                                                        outStageIds.Add(id_, name_);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case "networks":
                                {
                                    foreach (XmlNode child_ in element_.ChildNodes)
                                    {
                                        NetworkNode item_ = new NetworkNode(child_);
                                        
                                        if (item_.IsValid)
                                            networks.Add(item_);
                                    }
                                }
                                break;
                            case "devices":
                                {
                                    foreach (XmlNode child_ in element_.ChildNodes)
                                    {
                                        DeviceNode item_ = new DeviceNode(child_);

                                        if (item_.IsValid)
                                            devices.Add(item_);
                                    }
                                }
                                break;
                        }
                    }

                    (App.MainForm as FormMain).SetCulture(culture_);
                    result_ = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }

            return result_;
        }

        public static void SaveDisplayLanguage(string culturecode)
        {
            int begin_ = culturecode.LastIndexOf('(');
            int end_ = culturecode.LastIndexOf(')');
            string culture_ = culturecode.Substring(begin_ + 1, end_ - begin_);

            switch (culture_)
            {
                case "en-US":
                case "ko-KR":
                    (App.MainForm as FormMain).SetCulture(culture_);
                    break;
            }
        }

        public static void SelectedIndexChange(string filename = "Config.xml")
        {
            string filePath_ = $@"{App.Path}Config\{(string.IsNullOrEmpty(filename) ? "Config.xml" : filename)}";

            XElement result_ = new XElement("Config",
                new XAttribute("version", App.Version),
                new XAttribute("culture", App.CultureInfoCode));

            XElement nodes_ = null;
        }
        #endregion
    }
}
#endregion