#region Imports
using TechFloor.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.IO.Ports;
using System.Net;
using TechFloor.Object;
using TechFloor.Device;
using Modbus.Device;
using TechFloor.Device.CommunicationIo;
using System.CodeDom;
#endregion

#region Program
namespace TechFloor
{
    public class Config
    {
        #region Constants
        protected static readonly char[] CONST_LIGHT_VALUE_DELIMITER = { ',' };
        #endregion

        #region Fields
        protected static bool systemSimulation = false;
        protected static bool safetySensorUsage = false;
        protected static bool enableReturnReelTypeWatcher = true;
        protected static bool enableOneshotRecovery = true;
        protected static bool automaticHomeInIdleTime = true;
        protected static bool enableRejectReel = false;
        protected static bool enableVisionMarkAdjustment = true;
        protected static int reelTowerLocalServerPort = 7000;
        protected static int mobileRobotLocalServerPort = 40000;
        protected static int robotRemoteServerPort = 29999;
        protected static int robotLocalServerPort = 30004;
        protected static int reelTowerResponseTimeout = 10000;
        protected static int cartInOutCheckTimeout = 10000;
        protected static int robotCommunicationTimeout = 3000;
        protected static int robotProgramLoadTimeout = 30000;
        protected static int robotProgramPlayTimeout = 30000;
        protected static int robotActionTimeout = 30000;
        protected static int robotMoveTimeout = 30000;
        protected static int robotHomeTimeout = 60000;
        protected static int delayOfAutomaticHomeInIdleTime = 60000;
        protected static int jobSplitReelCount = 25;
        protected static string reelTowerLocalServerAddress = string.Empty;
        protected static string mobileRobotLocalServerAddress = string.Empty;
        protected static string robotRemoteServerAddress = string.Empty;
        protected static string reelTower1Name = "T0101";
        protected static string reelTower2Name = "T0102";
        protected static string reelTower3Name = "T0103";
        protected static string reelTower4Name = "T0104";
        protected static string handheldBarcodeScannerVid = string.Empty;
        protected static string handheldBarcodeScannerPid = string.Empty;
        protected static string handheldBarcodeScannerMid = string.Empty;
        protected static ReelTowerStateControlModes reelTowerStateControlMode = ReelTowerStateControlModes.QueryReelTowerStateByAtOnce;
        protected static Dictionary<int, Pair<string, string>> reelTowerIds = new Dictionary<int, Pair<string, string>>();
        protected static Dictionary<int, string> returnStageIds = new Dictionary<int, string>();
        protected static Dictionary<int, string> rejectStageIds = new Dictionary<int, string>();
        protected static Dictionary<int, string> outStageIds = new Dictionary<int, string>();
        protected static CameraObject cameraObject = null;
        protected static VisionLightNode visionNode = new VisionLightNode();
        protected static IPEndPoint remoteReportServer = null;
        protected static Dictionary<string, FiveField<string, string, TechFloor.Device.CommunicationIo.ModbusMasterController.Protocols, SerialPortSettings, EthernetPortSettings>> hmis = null;
        #endregion

        #region Properties
        public static bool SystemSimulation => systemSimulation;
        public static bool SafetySensorUsage => safetySensorUsage;
        public static bool ImageProcessorSimulation
        {
            get;
            protected set;
        }
        public static bool EnableReturnReelTypeWatcher => enableReturnReelTypeWatcher;
        public static bool EnableOneshotRecovery => enableOneshotRecovery;
        public static bool AutomaticHomeInIdleTime => automaticHomeInIdleTime; 
        public static bool EnableRejectReel => enableRejectReel;
        public static bool EnableVisionMarkAdjustment => enableVisionMarkAdjustment;
        public static int ReelTowerLocalServerPort => reelTowerLocalServerPort;
        public static int MobileRobotLocalServerPort => mobileRobotLocalServerPort;
        public static int RobotRemoteServerPort => robotRemoteServerPort;
        public static int RobotLocalServerPort => robotLocalServerPort;
        public static int TimeoutOfReelTowerResponse => reelTowerResponseTimeout;
        public static int TimeoutOfCartInOutCheck => cartInOutCheckTimeout;
        public static int TimeoutOfRobotCommunication => robotCommunicationTimeout;
        public static int TimeoutOfRobotProgramLoad => robotProgramLoadTimeout;
        public static int TimeoutOfRobotProgramPlay => robotProgramPlayTimeout;
        public static int TimeoutOfRobotAction => robotActionTimeout;
        public static int TimeoutOfRobotMove => robotMoveTimeout;
        public static int TimeoutOfRobotHome => robotHomeTimeout;
        public static int DelayOfAutomaticHomeInIdleTime
        {
            get => delayOfAutomaticHomeInIdleTime;
            set => delayOfAutomaticHomeInIdleTime = value;
        }
        public static int JobSplitReelCount => jobSplitReelCount;
        public static string ReelTowerLocalServerAddress => reelTowerLocalServerAddress;
        public static string MobilRobotLocalServerAddress => mobileRobotLocalServerAddress;
        public static string RobotRemoteServerAddress => robotRemoteServerAddress;
        public static string ReelTowerName1 => reelTower1Name;
        public static string ReelTowerName2 => reelTower2Name;
        public static string ReelTowerName3 => reelTower3Name;
        public static string ReelTowerName4 => reelTower4Name;
        public static string BarcodeScannerVid => handheldBarcodeScannerVid;
        public static string BarcodeScannerPid => handheldBarcodeScannerPid;
        public static string BarcodeScannerMid => handheldBarcodeScannerMid;
        public static ReelTowerStateControlModes ReelTowerStateControlMode => reelTowerStateControlMode;
        public static IReadOnlyDictionary<int, Pair<string, string>> ReelTowerIds => reelTowerIds;
        public static IReadOnlyDictionary<int, string> ReturnStageIds => returnStageIds;
        public static IReadOnlyDictionary<int, string> RejectStageIds => rejectStageIds;
        public static IReadOnlyDictionary<int, string> OutStageIds => outStageIds;
        public static ProcessCategories ProcessCategory
        {
            get;
            set;
        }
        public static ImageProcessorTypes ImageProcessorType
        {
            get;
            set;
        }
        public static CameraObject ImageDevice => cameraObject;
		public static IPEndPoint RemoteReportServer => remoteReportServer;
        public static VisionLightNode VisionLight => visionNode;
        public static IReadOnlyDictionary<string, FiveField<string, string, TechFloor.Device.CommunicationIo.ModbusMasterController.Protocols, SerialPortSettings, EthernetPortSettings>> Hmis => hmis;
        #endregion

        #region Constructors
        public Config()
        {
            Load();
        }
        #endregion

        #region Static methods
        public static void Save(string filename = "Config.xml")
        {
            // try
            // {
            //     string filePath_ = $@"{App.Path}Config\{(string.IsNullOrEmpty(filename) ? "Config.xml" : filename)}";
            // 
            //     XElement result_ = new XElement("Config",
            //         new XAttribute("version", App.Version),
            //         new XAttribute("culture", App.CultureInfoCode));
            // 
            //     XElement nodes_ = null;
            // 
            //     if (properties.Count > 0)
            //     {
            //         nodes_ = new XElement("Properties");
            // 
            //         foreach (KeyValuePair<string, PropertyNode> node_ in properties)
            //             nodes_.Add(node_.Value.ToXml("Property"));
            // 
            //         result_.Add(nodes_);
            //     }
            // 
            //     if (processes.Count > 0)
            //     {
            //         nodes_ = new XElement("Processes");
            // 
            //         foreach (KeyValuePair<string, Pair<string, bool>> node_ in processes)
            //         {
            //             nodes_.Add(new XElement("Process",
            //                 new XAttribute("name", node_.Key),
            //                 new XAttribute("enabled", node_.Value.second),
            //                 new XAttribute("type", node_.Value.first)));
            //         }
            // 
            //         result_.Add(nodes_);
            //     }
            // 
            //     if (networks.Count > 0)
            //     {
            //         nodes_ = new XElement("Networks");
            // 
            //         foreach (NetworkNode node_ in networks)
            //             nodes_.Add(node_.ToXml("Node"));
            // 
            //         result_.Add(nodes_);
            //     }
            // 
            //     if (devices.Count > 0)
            //     {
            //         nodes_ = new XElement("Devices");
            // 
            //         foreach (DeviceNode node_ in devices)
            //             nodes_.Add(node_.ToXml("Device"));
            // 
            //         result_.Add(nodes_);
            //     }
            // 
            //     if (returnStageIds.Count > 0)
            //     {
            //         nodes_ = new XElement("ReturnStages");
            // 
            //         foreach (KeyValuePair<int, string> node_ in returnStageIds)
            //             nodes_.Add(new XElement("Stage",
            //                 new XAttribute("id", node_.Key),
            //                 new XAttribute("name", node_.Value)));
            // 
            //         result_.Add(nodes_);
            //     }
            // 
            //     if (rejectStageIds.Count > 0)
            //     {
            //         nodes_ = new XElement("RejectStages");
            // 
            //         foreach (KeyValuePair<int, string> node_ in rejectStageIds)
            //             nodes_.Add(new XElement("Stage",
            //                 new XAttribute("id", node_.Key),
            //                 new XAttribute("name", node_.Value)));
            // 
            //         result_.Add(nodes_);
            //     }
            // 
            //     if (outStageIds.Count > 0)
            //     {
            //         nodes_ = new XElement("OutStages");
            // 
            //         foreach (KeyValuePair<int, string> node_ in outStageIds)
            //             nodes_.Add(new XElement("Stage",
            //                 new XAttribute("id", node_.Key),
            //                 new XAttribute("name", node_.Value)));
            // 
            //         result_.Add(nodes_);
            //     }
            // 
            //     if (Singleton<ReelTowerGroup>.Instance != null)
            //         result_.Add(Singleton<ReelTowerGroup>.Instance.ToXml());
            // 
            //     if (Singleton<CombineModuleManager>.Instance != null)
            //         result_.Add(Singleton<CombineModuleManager>.Instance.ToXml());
            // 
            //     result_.Save(filePath_);
            // }
            // catch (Exception ex)
            // {
            //     Debug.WriteLine($"Config.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            // }
        }

        public static bool Load(string filename = "Config.xml")
        {
            bool result_    = false;
            int idex_       = 0;
            string id_      = string.Empty;
            string name_    = string.Empty;

            try
            {
                string filePath_ = $@"{App.Path}Config\{(string.IsNullOrEmpty(filename)? "Config.xml" : filename)}";

                if (!File.Exists(filePath_))
                    return false;

                reelTowerIds.Clear();
                returnStageIds.Clear();
                rejectStageIds.Clear();
                outStageIds.Clear();

                XmlDocument xml = new XmlDocument();
                xml.Load(filePath_);

                if (App.Version == xml.DocumentElement.GetAttribute("version"))
                {
                    foreach (XmlNode element_ in xml.DocumentElement.ChildNodes)
                    {
                        switch (element_.Name.ToLower())
                        {
                            case "systemsimulation":
                                {
                                    systemSimulation = bool.Parse(element_.Attributes["usage"].Value);
                                }
                                break;
                            case "safetysensor":
                                {
                                    safetySensorUsage = bool.Parse(element_.Attributes["usage"].Value);
                                }
                                break;
                            case "enablereturnreeltypewatcher":
                                {
                                    enableReturnReelTypeWatcher = bool.Parse(element_.Attributes["usage"].Value);
                                }
                                break;
                            case "enableoneshotrecovery":
                                {
                                    enableOneshotRecovery = bool.Parse(element_.Attributes["usage"].Value);
                                }
                                break;
                            case "automatichomeinidletime":
                                {
                                    automaticHomeInIdleTime = bool.Parse(element_.Attributes["usage"].Value);
                                }
                                break;
                            case "jobsplitreelcount":
                                {
                                    jobSplitReelCount = int.Parse(element_.InnerText);
                                }
                                break;
                            case "process":
                                {
                                    ProcessCategory = (ProcessCategories)Enum.Parse(typeof(ProcessCategories), element_.Attributes["category"].Value);
                                    ImageProcessorType = (ImageProcessorTypes)Enum.Parse(typeof(ImageProcessorTypes), element_.Attributes["type"].Value);
                                    ImageProcessorSimulation = bool.Parse(element_.Attributes["simulation"].Value);
                                }
                                break;
                            case "materialtransferports":
                                {
                                    foreach (XmlNode node_ in element_.ChildNodes)
                                    {
                                        switch (node_.Name.ToLower())
                                        {
                                            case "reeltowers":
                                                {
                                                    foreach (XmlNode child_ in node_.ChildNodes)
                                                    {
                                                        if (child_.Name.ToLower() == "reeltower")
                                                        {
                                                            id_ = child_.Attributes["id"].Value;
                                                            switch (idex_ = Convert.ToInt32(child_.Attributes["index"].Value))
                                                            {
                                                                case 1:
                                                                    reelTower1Name = child_.InnerText;
                                                                    break;
                                                                case 2:
                                                                    reelTower2Name = child_.InnerText;
                                                                    break;
                                                                case 3:
                                                                    reelTower3Name = child_.InnerText;
                                                                    break;
                                                                case 4:
                                                                    reelTower4Name = child_.InnerText;
                                                                    break;
                                                            }

                                                            if (idex_ >= 1 && idex_ <= 4)
                                                            {
                                                                if (reelTowerIds.ContainsKey(idex_))
                                                                    return result_;
                                                                else
                                                                    reelTowerIds.Add(idex_, new Pair<string, string>(id_, child_.InnerText));
                                                            }
                                                            else
                                                                return result_;
                                                        }
                                                    }
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
                                                                idex_ = Convert.ToInt32(child_.Attributes["id"].Value);
                                                                name_ = child_.Attributes["name"].Value;

                                                                if (returnStageIds.ContainsKey(idex_))
                                                                    return result_;
                                                                else
                                                                    returnStageIds.Add(idex_, name_);
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
                                                                    idex_ = Convert.ToInt32(child_.Attributes["id"].Value);
                                                                    name_ = child_.Attributes["name"].Value;

                                                                    if (rejectStageIds.ContainsKey(idex_))
                                                                        return result_;
                                                                    else
                                                                        rejectStageIds.Add(idex_, name_);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            case "outstages":
                                                {
                                                    foreach (XmlNode child_ in node_.ChildNodes)
                                                    {
                                                        if (child_.Name.ToLower() == "stage")
                                                        {
                                                            switch (idex_ = Convert.ToInt32(child_.Attributes["id"].Value))
                                                            {
                                                                case 1:
                                                                case 2:
                                                                case 3:
                                                                case 4:
                                                                case 5:
                                                                case 6:
                                                                    {
                                                                        if (outStageIds.ContainsKey(idex_))
                                                                            return result_;
                                                                        else
                                                                            outStageIds.Add(idex_, child_.InnerText);
                                                                    }
                                                                    break;
                                                                default:
                                                                    return result_;
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case "network":
                                {
                                    foreach (XmlNode node_ in element_.ChildNodes)
                                    {
                                        switch (node_.Name.ToLower())
                                        {
                                            case "node":
                                                {
                                                    switch (node_.Attributes["name"].Value.ToLower())
                                                    {
                                                        case "reeltowerlocalserver":
                                                            {
                                                                reelTowerLocalServerAddress = node_.Attributes["address"].Value;
                                                                reelTowerLocalServerPort = Convert.ToInt32(node_.Attributes["port"].Value);
                                                            }
                                                            break;
                                                        case "mobilerobotlocalserver":
                                                            {
                                                                mobileRobotLocalServerAddress = node_.Attributes["address"].Value;
                                                                mobileRobotLocalServerPort = Convert.ToInt32(node_.Attributes["port"].Value);
                                                            }
                                                            break;
                                                        case "robotlocalserver":
                                                            {
                                                                robotLocalServerPort = Convert.ToInt32(node_.Attributes["port"].Value);

                                                                foreach (XmlNode child_ in node_.ChildNodes)
                                                                {
                                                                    if (child_.Name.ToLower() == "module")
                                                                    {
                                                                        switch (idex_ = Convert.ToInt32(child_.Attributes["id"].Value))
                                                                        {
                                                                            case 1:
                                                                                {
                                                                                    robotRemoteServerAddress = child_.Attributes["address"].Value;
                                                                                    robotRemoteServerPort = Convert.ToInt32(child_.Attributes["port"].Value);
                                                                                }
                                                                                break;
                                                                            default:
                                                                                return result_;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        case "remotereportserver":
                                                            {
                                                                string address_ = node_.Attributes["address"].Value;
                                                                int port_ = Convert.ToInt32(node_.Attributes["port"].Value);
                                                                remoteReportServer = new IPEndPoint(IPAddress.Parse(address_), port_);
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case "devices":
                                {
                                    foreach (XmlNode node_ in element_.ChildNodes)
                                    {
                                        switch (node_.Name.ToLower())
                                        {
                                            case "device":
                                                {
                                                    switch (node_.Attributes["category"].Value.ToLower())
                                                    {
                                                        case "barcodescanner":
                                                            {
                                                                handheldBarcodeScannerVid = node_.Attributes["vid"].Value;
                                                                handheldBarcodeScannerPid = node_.Attributes["pid"].Value;
                                                                handheldBarcodeScannerMid = node_.Attributes["mid"].Value;
                                                            }
                                                            break;
                                                        case "light":
                                                            {
                                                                foreach (XmlAttribute attr_ in node_.Attributes)
                                                                {
                                                                    switch (attr_.Name.ToLower())
                                                                    {
                                                                        case "maker": visionNode.Maker = (DeviceManufacturer)Enum.Parse(typeof(DeviceManufacturer), attr_.Value); break;
                                                                        case "setting":
                                                                            {
                                                                                int indx_ = 0;
                                                                                string[] tokens_ = attr_.Value.Split(CONST_LIGHT_VALUE_DELIMITER, StringSplitOptions.RemoveEmptyEntries);
                                                                                
                                                                                foreach (string token_ in tokens_)
                                                                                {
                                                                                    switch (indx_++)
                                                                                    {
                                                                                        case 0:
                                                                                            {
                                                                                                if (token_.ToLower().Contains("com"))
                                                                                                    visionNode.Setting.PortName = token_;
                                                                                            }
                                                                                            break;
                                                                                        case 1:
                                                                                            {
                                                                                                visionNode.Setting.BaudRate = Convert.ToInt32(token_);
                                                                                            }
                                                                                            break;
                                                                                        case 2:
                                                                                            {
                                                                                                visionNode.Setting.Parity = (Parity)Enum.Parse(typeof(Parity), token_);
                                                                                            }
                                                                                            break;
                                                                                        case 3:
                                                                                            {
                                                                                                visionNode.Setting.DataBits = Convert.ToInt32(token_);
                                                                                            }
                                                                                            break;
                                                                                        case 4:
                                                                                            {
                                                                                                visionNode.Setting.StopBits = (StopBits)Enum.Parse(typeof(StopBits), token_);
                                                                                            }
                                                                                            break;
                                                                                        case 5:
                                                                                            {
                                                                                                visionNode.Setting.Handshake = (Handshake)Enum.Parse(typeof(Handshake), token_);
                                                                                            }
                                                                                            break;
                                                                                    }
                                                                                }
                                                                                
                                                                            }
                                                                            break;
                                                                        case "channel": visionNode.Channel= Convert.ToInt32(attr_.Value); break;
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        case "camera":
                                                            {
                                                                if (cameraObject == null)
                                                                    cameraObject = new CameraObject(node_);
                                                            }
                                                            break;
                                                        case "display":
                                                            {
                                                                int slaveaddr = 1;
                                                                string name = string.Empty;
                                                                string model = string.Empty;
                                                                InterfaceTypes inf = InterfaceTypes.Serial;
                                                                TechFloor.Device.CommunicationIo.ModbusMasterController.Protocols protocol = TechFloor.Device.CommunicationIo.ModbusMasterController.Protocols.Rtu;
                                                                DeviceManufacturer maker = DeviceManufacturer.ComfilePi;
                                                                SerialPortSettings serialSetting = new SerialPortSettings();
                                                                EthernetPortSettings ethernetSetting = new EthernetPortSettings();

                                                                if (hmis == null)
                                                                    hmis = new Dictionary<string, FiveField<string, string, TechFloor.Device.CommunicationIo.ModbusMasterController.Protocols, SerialPortSettings, EthernetPortSettings>>();

                                                                foreach (XmlAttribute attr_ in node_.Attributes)
                                                                {
                                                                    switch (attr_.Name.ToLower())
                                                                    {
                                                                        case "name": name = attr_.Value; break;
                                                                        case "model": model = attr_.Value; break;
                                                                        case "maker": maker = (DeviceManufacturer)Enum.Parse(typeof(DeviceManufacturer), attr_.Value); break;
                                                                        case "interface": inf = (InterfaceTypes)Enum.Parse(typeof(InterfaceTypes), attr_.Value); break;
                                                                        case "setting":
                                                                            {
                                                                                int indx_ = 0;
                                                                                string[] tokens_ = attr_.Value.Split(CONST_LIGHT_VALUE_DELIMITER, StringSplitOptions.RemoveEmptyEntries);

                                                                                foreach (string token_ in tokens_)
                                                                                {
                                                                                    switch (indx_++)
                                                                                    {
                                                                                        case 0:
                                                                                            {
                                                                                                if (token_.ToLower().Contains("com"))
                                                                                                    serialSetting.PortName = token_;
                                                                                            }
                                                                                            break;
                                                                                        case 1:
                                                                                            {
                                                                                                serialSetting.BaudRate = Convert.ToInt32(token_);
                                                                                            }
                                                                                            break;
                                                                                        case 2:
                                                                                            {
                                                                                                serialSetting.Parity = (Parity)Enum.Parse(typeof(Parity), token_);
                                                                                            }
                                                                                            break;
                                                                                        case 3:
                                                                                            {
                                                                                                serialSetting.DataBits = Convert.ToInt32(token_);
                                                                                            }
                                                                                            break;
                                                                                        case 4:
                                                                                            {
                                                                                                serialSetting.StopBits = (StopBits)Enum.Parse(typeof(StopBits), token_);
                                                                                            }
                                                                                            break;
                                                                                        case 5:
                                                                                            {
                                                                                                serialSetting.Handshake = (Handshake)Enum.Parse(typeof(Handshake), token_);
                                                                                            }
                                                                                            break;
                                                                                    }
                                                                                }

                                                                                serialSetting.ReadTimeout = 100;
                                                                                serialSetting.WriteTimeout = 100;
                                                                            }
                                                                            break;
                                                                        case "slaveaddress": slaveaddr = Convert.ToInt32(attr_.Value); break;
                                                                        case "protocol":
                                                                            protocol = (TechFloor.Device.CommunicationIo.ModbusMasterController.Protocols)Enum.Parse(typeof(TechFloor.Device.CommunicationIo.ModbusMasterController.Protocols), attr_.Value);
                                                                            break;
                                                                    }
                                                                }

                                                                if (!string.IsNullOrEmpty(name) && !hmis.ContainsKey(name))
                                                                {
                                                                    hmis.Add(name, new FiveField<string, string, TechFloor.Device.CommunicationIo.ModbusMasterController.Protocols, SerialPortSettings, EthernetPortSettings>(name, model, protocol, serialSetting, ethernetSetting));
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case "properties":
                                {
                                    foreach (XmlNode node_ in element_.ChildNodes)
                                    {
                                        switch (node_.Name.ToLower())
                                        {
                                            case "timeout":
                                                {
                                                    foreach (XmlNode child_ in node_.ChildNodes)
                                                    {
                                                        switch (child_.Name.ToLower())
                                                        {
                                                            case "reeltowerresponsetimeout":
                                                                {
                                                                    reelTowerResponseTimeout = Convert.ToInt32(child_.InnerText);
                                                                }
                                                                break;
                                                            case "cartinoutchecktimeout":
                                                                {
                                                                    reelTowerResponseTimeout = Convert.ToInt32(child_.InnerText);
                                                                }
                                                                break;
                                                            case "robotcommunicationtimeout":
                                                                {
                                                                    robotCommunicationTimeout = Convert.ToInt32(child_.InnerText);
                                                                }
                                                                break;
                                                            case "robotprogramloadtimeout":
                                                                {
                                                                    robotProgramLoadTimeout = Convert.ToInt32(child_.InnerText);
                                                                }
                                                                break;
                                                            case "robotprogramplaytimeout":
                                                                {
                                                                    robotProgramPlayTimeout = Convert.ToInt32(child_.InnerText);
                                                                }
                                                                break;
                                                            case "robotactiontimeout":
                                                                {
                                                                    robotActionTimeout = Convert.ToInt32(child_.InnerText);
                                                                }
                                                                break;
                                                            case "robotmovetimeout":
                                                                {
                                                                    robotMoveTimeout = Convert.ToInt32(child_.InnerText);
                                                                }
                                                                break;
                                                            case "robothometimeout":
                                                                {
                                                                    robotHomeTimeout = Convert.ToInt32(child_.InnerText);
                                                                }
                                                                break;
                                                            case "delayofautomatichomeinidletime":
                                                                {
                                                                    delayOfAutomaticHomeInIdleTime = Convert.ToInt32(child_.InnerText);
                                                                }
                                                                break;
                                                        }
                                                    }
                                                }
                                                break;
                                            case "options":
                                                {
                                                    foreach (XmlNode child_ in node_.ChildNodes)
                                                    {
                                                        switch (child_.Name.ToLower())
                                                        {
                                                            case "reeltowerstatecontrolmode":
                                                                {
                                                                    reelTowerStateControlMode = (ReelTowerStateControlModes)Enum.Parse(typeof(ReelTowerStateControlModes), child_.InnerText);
                                                                }
                                                                break;
                                                            case "enablerejectreel":
                                                                {
                                                                    enableRejectReel = bool.Parse(child_.Attributes["usage"].Value);
                                                                }
                                                                break;
                                                            case "enablevisionmarkadjustment":
                                                                {
                                                                    enableVisionMarkAdjustment = bool.Parse(child_.Attributes["usage"].Value);
                                                                }
                                                                break;
                                                        }
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                        }
                    }

                    // Set display language
                    (App.MainForm as FormMain).SetCulture(xml.DocumentElement.GetAttribute("culture"));
                    result_ = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }

            return result_;
        }
        #endregion
    }

    public class CameraObject
    {
        #region Fields
        protected Point fov = new Point(0, 0);
        #endregion

        #region Properties
        public string Name
        {
            get;
            protected set;
        }

        public string SerialNumber
        {
            get;
            protected set;
        }

        public string Address
        {
            get;
            protected set;
        }

        public string Maker
        {
            get;
            protected set;
        }

        public string Model
        {
            get;
            protected set;
        }

        public ImageDeviceFormats Format
        {
            get;
            protected set;
        }

        public double PixelSize
        {
            get;
            protected set;
        }

        public Point Fov
        {
            get => fov;
            set => fov = value;
        }

        public DisplayUnits Unit
        {
            get;
            protected set;
        }
        #endregion

        #region Constructors
        public CameraObject(XmlNode node)
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
                    Name = node.Attributes["name"].Value;
                    Address = node.Attributes["address"].Value;
                    Maker = node.Attributes["maker"].Value;
                    Model = node.Attributes["model"].Value;
                    SerialNumber = node.Attributes["serialnumber"].Value;
                    Format = (ImageDeviceFormats)Enum.Parse(typeof(ImageDeviceFormats), node.Attributes["format"].Value);

                    foreach (XmlNode child_ in node.ChildNodes)
                    {
                        switch (child_.Name.ToLower())
                        {
                            case "fov":
                                {
                                    foreach (XmlNode element_ in child_.ChildNodes)
                                    {
                                        switch (element_.Name.ToLower())
                                        {
                                            case "width":
                                                {
                                                    fov.X = int.Parse(element_.InnerText);
                                                }
                                                break;
                                            case "height":
                                                {
                                                    fov.Y = int.Parse(element_.InnerText);
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                            case "pixelsize":
                                {
                                    Unit = (DisplayUnits)Enum.Parse(typeof(DisplayUnits), child_.Attributes["unit"].Value);
                                    PixelSize = double.Parse(child_.InnerText);
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
        #endregion
    }
}
#endregion