#region Imports
using TechFloor.Components.Elements;
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
#endregion

#region Program
namespace TechFloor
{
    public class Model
    {
        #region Enumerations
        #endregion

        #region Fields
        protected static List<ProcessNode> processes = new List<ProcessNode>();

        protected static List<PropertyNode> delays = new List<PropertyNode>();

        protected static List<PropertyNode> timeouts = new List<PropertyNode>();
        #endregion

        #region Properties
        public static IReadOnlyList<ProcessNode> Processes => processes;

        public static IReadOnlyList<PropertyNode> Delays => delays;

        public static IReadOnlyList<PropertyNode> Timeouts => timeouts;

        public static object GetProperty(ProcessCategories process, string model, string name)
        {
            ProcessNode node_ = processes.Find(x => x.Category == process && x.Model.ToLower() == model.ToLower());

            if (node_ != null)
            {
                PropertyNode item_ = node_.Properties.Find(x => x.Name == name);

                if (item_ != null)
                    return item_.Value;
            }

            return null;
        }

        public static void SetProperty(ProcessCategories process, string model, string name, object value)
        {
            ProcessNode node_ = processes.Find(x => x.Category == process && x.Model.ToLower() == model.ToLower());

            if (node_ != null)
            {
                PropertyNode item_ = node_.Properties.Find(x => x.Name == name);

                if (item_ != null)
                    item_.Value = value;
            }
        }

        public static bool IsPropertyEnabled(ProcessCategories process, string model, string name)
        {
            ProcessNode node_ = processes.Find(x => x.Category == process && x.Model.ToLower() == model.ToLower());

            if (node_ != null)
            {
                PropertyNode item_ = node_.Properties.Find(x => x.Name == name);

                if (item_ != null)
                    return item_.Enabled;
            }

            return false;
        }

        public static int GetTimeout(string name)
        {
            PropertyNode node_ = timeouts.Find(x => x.Name == name);

            if (node_ != null)
                return Convert.ToInt32(node_.Value);

            return 0;
        }

        public static void SetTimeout(string name, int value)
        {
            PropertyNode node_ = timeouts.Find(x => x.Name == name);

            if (node_ != null)
                node_.Value = value;
        }

        public static bool IsTimeoutEnabled(string name)
        {
            PropertyNode node_ = timeouts.Find(x => x.Name == name);

            if (node_ != null)
                return node_.Enabled;

            return false;
        }

        public static int GetDelay(string name)
        {
            PropertyNode node_ = delays.Find(x => x.Name == name);

            if (node_ != null)
                return Convert.ToInt32(node_.Value);

            return 0;
        }

        public static void SetDelay(string name, int value)
        {
            PropertyNode node_ = delays.Find(x => x.Name == name);

            if (node_ != null)
                node_.Value = value;
        }

        public static bool IsDelayEnabled(string name)
        {
            PropertyNode node_ = delays.Find(x => x.Name == name);

            if (node_ != null)
                return node_.Enabled;

            return false;
        }
        #endregion

        #region Constructors
        public Model()
        {
            Load();
        }
        #endregion

        #region Static methods
        public static void Save(string filename = "Default.xml")
        {  
            try
            {
                string filePath_ = $@"{App.Path}Model\{(string.IsNullOrEmpty(filename) ? "Default.xml" : filename)}";

                XElement result_ = new XElement("Model", new XAttribute("version", App.Version));                
                XElement nodes_ = null;

                if (processes.Count > 0)
                {
                    nodes_ = new XElement("Processes");

                    foreach (ProcessNode node_ in processes)
                        nodes_.Add(node_.ToXml("Process"));

                    if (nodes_.HasElements)
                        result_.Add(nodes_);
                }

                if (delays.Count > 0)
                {
                    nodes_ = new XElement("Delays");

                    foreach (PropertyNode node_ in delays)
                        nodes_.Add(node_.ToXml("Delay"));

                    if (nodes_.HasElements)
                        result_.Add(nodes_);
                }

                if (timeouts.Count > 0)
                {
                    nodes_ = new XElement("Timeouts");

                    foreach (PropertyNode node_ in timeouts)
                        nodes_.Add(node_.ToXml("Timeout"));

                    if (nodes_.HasElements)
                        result_.Add(nodes_);
                }

                result_.Save(filePath_);
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }
        }

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
                            case "processes":
                            case "delays":
                            case "timeouts":
                                {
                                    foreach (XmlNode child_ in node_.ChildNodes)
                                    {
                                        switch (child_.Name.ToLower())
                                        {
                                            case "process":
                                                {
                                                    ProcessNode item_ = new ProcessNode(child_);

                                                    if (item_.IsValid)
                                                        processes.Add(item_);
                                                }
                                                break;
                                            case "delay":
                                                {
                                                    PropertyNode property_ = new PropertyNode(child_);

                                                    if (property_.IsValid)
                                                        delays.Add(property_);
                                                }
                                                break;
                                            case "timeout":
                                                {
                                                    PropertyNode property_ = new PropertyNode(child_);

                                                    if (property_.IsValid)
                                                        timeouts.Add(property_);
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
        #endregion
    }
}
#endregion
