#region Imports
using TechFloor.Device;
using TechFloor.Device.CommunicationIo;
using TechFloor.Object;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
#endregion

#region Program
namespace TechFloor.Components.Elements
{
    public class NetworkNode
    {
        #region Fields
        protected InterfaceTypes interfaceType;

        protected ProtocolType protocol;

        protected NetworkNodeModes mode;

        protected IPEndPoint endpoint;

        protected string name;

        protected string uri;

        protected string desc;
        #endregion

        #region Properties
        public InterfaceTypes InterfaceType => interfaceType;

        public ProtocolType Protocol => protocol;

        public NetworkNodeModes Mode => mode;

        public IPEndPoint Endpoint => endpoint;

        public string Name => name;

        public string Uri => uri;

        public string Desc => desc;

        public bool IsValid => !string.IsNullOrEmpty(name) && (endpoint != null || !string.IsNullOrEmpty(uri));
        #endregion

        #region Constructors
        public NetworkNode(XmlNode node)
        {
            Create(node);
        }
        #endregion

        #region Public methods
        public virtual bool Create(XmlNode node)
        {
            bool result_ = false;
            int port_ = 0;
            string address_ = string.Empty;

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
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        name = attr_.Value;
                                }
                                break;
                            case "interfacetype":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        interfaceType = (InterfaceTypes)Enum.Parse(typeof(InterfaceTypes), attr_.Value);
                                }
                                break;
                            case "protocol":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        protocol = (ProtocolType)Enum.Parse(typeof(ProtocolType), attr_.Value);
                                }
                                break;
                            case "mode":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        mode = (NetworkNodeModes)Enum.Parse(typeof(NetworkNodeModes), attr_.Value);
                                }
                                break;
                            case "uri":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        uri = attr_.Value;
                                }
                                break;
                            case "address":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        address_ = attr_.Value;
                                }
                                break;
                            case "port":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        port_ = int.Parse(attr_.Value);
                                }
                                break;
                            case "desc":
                                {
                                    desc = attr_.Value;
                                }
                                break;
                        }
                    }

                    if (!string.IsNullOrEmpty(address_))
                    {
                        if (StringLogicalComparer.IsIpAddress(address_))
                            endpoint = new IPEndPoint(IPAddress.Parse(address_), port_);
                        else if (StringLogicalComparer.IsHostAddress(address_))
                            endpoint = new IPEndPoint(StringLogicalComparer.ResolveHostAddress(address_), port_);
                    }

                    if (!string.IsNullOrEmpty(name) && (endpoint != null || !string.IsNullOrEmpty(uri)))
                        result_ = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                result_ = false;
            }

            return result_;
        }

        public virtual XElement ToXml(string nodename= null)
        {
            XElement result_ = new XElement(string.IsNullOrEmpty(nodename)? GetType().Name : nodename,
                new XAttribute("name", name),
                new XAttribute("interfacetype", interfaceType),
                new XAttribute("protocol", protocol),
                new XAttribute("mode", mode));

            if (endpoint != null)
                result_.Add(new XAttribute("address", endpoint.Address.ToString()),
                    new XAttribute("port", endpoint.Port));

            if (!string.IsNullOrEmpty(uri))
                result_.Add(new XAttribute("uri", uri));

            if (!string.IsNullOrEmpty(desc))
                result_.Add(new XAttribute("desc", desc));

            return result_;
        }
        #endregion
    }

    public class DatabaseNode
    {
        #region Fields
        protected DatabaseManager.DatabaseOleDrivers oleDriver;

        protected DatabaseManager.DatabaseTypes dbType;

        protected int dbPort = -1;

        protected string name;

        protected string dbServer;

        protected string dbName;

        protected string dbFile;

        protected string dbUser;

        protected string dbPassword;
        #endregion

        #region Properties
        public DatabaseManager.DatabaseOleDrivers OleDriver => oleDriver;

        public DatabaseManager.DatabaseTypes DbType => dbType;

        public int DbPort => dbPort;

        public string Name => name;

        public string DbServer => dbServer;

        public string DbName => dbName;

        public string DbUser => dbUser;

        public string DbPassword => dbPassword;

        public bool IsValid => !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(dbServer);
        #endregion

        #region Constructors
        public DatabaseNode(XmlNode node)
        {
            Create(node);
        }
        #endregion

        #region Public methods
        public virtual bool Create(XmlNode node)
        {
            bool result_ = false;
            string address_ = string.Empty;

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
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        name = attr_.Value;
                                }
                                break;
                            case "driver":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        oleDriver = (DatabaseManager.DatabaseOleDrivers)Enum.Parse(typeof(DatabaseManager.DatabaseOleDrivers), attr_.Value);
                                }
                                break;
                            case "dbtype":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        dbType = (DatabaseManager.DatabaseTypes)Enum.Parse(typeof(DatabaseManager.DatabaseTypes), attr_.Value);
                                }
                                break;
                            case "user":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        dbUser = attr_.Value;
                                }
                                break;
                            case "password":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        dbPassword = attr_.Value;
                                }
                                break;
                            case "server":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        dbServer = attr_.Value;
                                }
                                break;
                            case "dbname":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        dbName = attr_.Value;
                                }
                                break;
                            case "dbfile":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        dbFile = attr_.Value;
                                }
                                break;
                            case "port":
                                {
                                    if (!string.IsNullOrEmpty(attr_.Value))
                                        dbPort = int.Parse(attr_.Value);
                                }
                                break;
                        }
                    }

                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(dbServer))
                        result_ = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                result_ = false;
            }

            return result_;
        }

        public virtual XElement ToXml(string nodename= null)
        {
            XElement result_ = new XElement(string.IsNullOrEmpty(nodename) ? GetType().Name : nodename,
                new XAttribute("name", name),
                new XAttribute("server", dbServer),
                new XAttribute("dbname", dbName));

            if (!string.IsNullOrEmpty(dbFile))
                result_.Add(new XAttribute("dbfile", dbFile));

            if (!string.IsNullOrEmpty(dbUser) && !string.IsNullOrEmpty(dbPassword))
                result_.Add(new XAttribute("user", dbUser),
                    new XAttribute("password", dbPassword));

            result_.Add(new XAttribute("driver", oleDriver),
                new XAttribute("dbtype", dbType));

            return result_;
        }
        #endregion
    }

    public class DeviceNode
    {
        #region Fields
        protected bool useHid;

        protected string name;

        protected string model;

        protected string hardwareId;

        protected string desc;

        protected string vid;

        protected string pid;

        protected string mid;

        protected PhysicalDevices category;

        protected DeviceManufacturer maker;

        protected InterfaceTypes interfaceType;

        protected SerialPortSettings comPortSetting;

        protected EthernetPortSettings ethernetPortSetting;
        #endregion

        #region Properties
        public bool HidUsage => useHid;

        public bool IsValid => !string.IsNullOrEmpty(name) && (!string.IsNullOrEmpty(hardwareId) & useHid) || (!string.IsNullOrEmpty(comPortSetting.PortName) || !string.IsNullOrEmpty(ethernetPortSetting.IpAddress));

        public string Name => name;

        public string Model => model;

        public string HardwardId => hardwareId;

        public string Desc => desc;

        public string Vid => vid;

        public string Pid => pid;

        public string Mid => mid;

        public PhysicalDevices Category => category;

        public DeviceManufacturer Maker => maker;

        public InterfaceTypes InterfaceType => interfaceType;

        public SerialPortSettings ComPortSetting => comPortSetting;

        public EthernetPortSettings EthernetPortSetting => ethernetPortSetting;
        #endregion

        #region Constructors
        public DeviceNode(XmlNode node)
        {
            Create(node);
        }
        #endregion

        #region Public methods
        public virtual bool Create(XmlNode node)
        {
            bool result_ = false;
            
            if (node != null)
            {
                foreach (XmlAttribute attr_ in node.Attributes)
                {
                    switch (attr_.Name.ToLower())
                    {
                        case "name":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                    name = attr_.Value;
                            }
                            break;
                        case "interfacetype":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                    interfaceType = (InterfaceTypes)Enum.Parse(typeof(InterfaceTypes), attr_.Value);
                            }
                            break;
                        case "category":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                    category = (PhysicalDevices)Enum.Parse(typeof(PhysicalDevices), attr_.Value);
                            }
                            break;
                        case "maker":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                    maker = (DeviceManufacturer)Enum.Parse(typeof(DeviceManufacturer), attr_.Value);
                            }
                            break;
                        case "hid":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                    useHid = bool.Parse(attr_.Value);
                            }
                            break;
                        case "model":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                    model = attr_.Value;
                            }
                            break;
                        case "hardwareid":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                {
                                    hardwareId = Regex.Replace(attr_.Value, "%26", "&");
                                    string[] ids_ = hardwareId.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                                    switch (ids_.Length)
                                    {
                                        case 2:
                                            {
                                                vid = ids_[0];
                                                pid = ids_[1];
                                            }
                                            break;
                                        case 3:
                                            {
                                                vid = ids_[0];
                                                pid = ids_[1];
                                                mid = ids_[2];
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        case "desc":
                            {
                                desc = attr_.Value;
                            }
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(name))
                    result_ = true;
            }

            return result_;
        }

        public virtual XElement ToXml(string nodename= null)
        {
            XElement result_ = new XElement(string.IsNullOrEmpty(nodename) ? GetType().Name : nodename,
                new XAttribute("name", name),
                new XAttribute("category", category),
                new XAttribute("interfacetype", interfaceType),
                new XAttribute("maker", maker),
                new XAttribute("model", model),
                new XAttribute("hid", useHid));

            if (!string.IsNullOrEmpty(hardwareId))
                result_.Add(new XAttribute("hardwareid", Regex.Replace(hardwareId, "&", "%26")));

            if (!string.IsNullOrEmpty(desc))
                result_.Add(new XAttribute("desc", desc));

            return result_;
        }
        #endregion
    }

    public class PropertyNode
    {
        #region Fields
        protected bool enabled;

        protected string name;

        protected object value;

        protected TypeCode valueType;
        #endregion

        #region Properties
        public bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }

        public string Name => name;

        public object Value
        {
            get => this.value;
            set => this.value = value;
        }

        public TypeCode ValueType => valueType;

        public bool IsValid => !string.IsNullOrEmpty(name) && valueType != TypeCode.Empty && name != null;
        #endregion

        #region Constructors
        public PropertyNode(XmlNode node)
        {
            Create(node);
        }
        #endregion

        #region Public methods
        public bool Create(XmlNode node)
        {
            bool result_ = false;

            if (node != null)
            {
                foreach (XmlAttribute attr_ in node.Attributes)
                {
                    switch (attr_.Name.ToLower())
                    {
                        case "name":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                    name = attr_.Value;
                            }
                            break;
                        case "enabled":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                    enabled = bool.Parse(attr_.Value);
                            }
                            break;
                        case "type":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                    valueType = (TypeCode)Enum.Parse(typeof(TypeCode), attr_.Value);
                            }
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(node.InnerText))
                    value = node.InnerText;
            }

            return result_;
        }

        public virtual XElement ToXml(string nodename= null)
        {
            return new XElement(string.IsNullOrEmpty(nodename) ? GetType().Name : nodename,
                new XAttribute("name", name),
                new XAttribute("enabled", enabled),
                new XAttribute("type", valueType),
                value);
        }

        public virtual object ChangeType()
        {
            return Convert.ChangeType(value, valueType);
        }
        #endregion
    }

    public class ProcessNode
    {
        #region Fields
        protected ProcessCategories category;

        protected bool enabled = false;

        protected string model = string.Empty;

        protected List<PropertyNode> properties = new List<PropertyNode>();
        #endregion

        #region Properties
        public ProcessCategories Category => category;

        public bool Enabled => enabled;

        public string Model => model;

        public bool IsValid => !string.IsNullOrEmpty(model);

        public List<PropertyNode> Properties => properties;
        #endregion

        #region Constructors
        public ProcessNode(XmlNode node)
        {
            Create(node);
        }
        #endregion

        #region Public methods
        public virtual bool Create(XmlNode node)
        {
            bool result_ = false;

            if (node != null)
            {
                if (node.Attributes["category"] != null && node.Attributes["model"] != null)
                {
                    category = (ProcessCategories)Enum.Parse(typeof(ProcessCategories), node.Attributes["category"].Value);
                    model = node.Attributes["model"].Value;

                    foreach (XmlNode element_ in node.ChildNodes)
                    {
                        switch (element_.Name.ToLower())
                        {
                            case "properties":
                                {
                                    foreach (XmlNode child_ in element_.ChildNodes)
                                    {
                                        PropertyNode property_ = new PropertyNode(child_);

                                        if (property_.IsValid)
                                            properties.Add(property_);
                                    }
                                }
                                break;
                        }
                    }

                    result_ = true;
                }
            }

            return result_;
        }

        public virtual XElement ToXml(string nodename= null)
        {
            XElement result_ = new XElement(string.IsNullOrEmpty(nodename)? GetType().Name : nodename,
                new XAttribute("category", category),
                new XAttribute("model", model),
                new XAttribute("enabled", enabled));

            foreach (PropertyNode node_ in properties)
                result_.Add(node_.ToXml("Property"));

            return result_;
        }
        #endregion
    }
}
#endregion