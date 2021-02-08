#region Imports
using TechFloor.Object;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
#endregion

#region Program
namespace TechFloor.Components
{
    public class AlarmManager
    {
        #region Fields
        protected string cultureCode;

        protected Dictionary<int, AlarmData> alarmList = new Dictionary<int, AlarmData>();
        #endregion

        #region Properties
        public string CultureCode => cultureCode;

        public IReadOnlyDictionary<int, AlarmData> AlarmList => alarmList;
        #endregion

        #region Constructors
        public AlarmManager(string culturecode)
        {
            this.cultureCode = culturecode;
        }
        #endregion

        #region Public methods
        public virtual void SetCulture(string culturecode)
        {
            this.cultureCode = culturecode;
        }

        public virtual bool AddAlarmData(int code, string name, SeverityLevels level, string message, bool enabled, bool report = true, string extra = null, string module = null, string description = null, string cause = null, string remedy = null)
        {
            if (!alarmList.ContainsKey(code))
            {
                alarmList.Add(code, new AlarmData(code, name, level, message, enabled, report, extra, module, description, cause, remedy));
                return true;
            }

            return false;
        }

        public virtual string GetAlarmMessage(int code)
        {
            int alarmcode_ = code;

            if (code >= 10000)
                alarmcode_ = code % 1000;

            if (alarmList.ContainsKey(alarmcode_))
                return alarmList[alarmcode_].Message;

            return "The alarm code is not defined.";
        }

        public virtual void Clear()
        {
            alarmList.Clear();
        }

        public virtual void Load(string file, string culturecode)
        {
            try
            {
                Clear();

                if (File.Exists(file))
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(file);

                    if (!string.IsNullOrEmpty(culturecode))
                        cultureCode = culturecode;

                    foreach (XmlNode element_ in xml.DocumentElement.ChildNodes)
                    {
                        switch (element_.Name.ToLower())
                        {
                            case "alarm":
                                {
                                    bool enabled_ = false;
                                    bool report_ = false;
                                    int code_ = 0;
                                    string extra_ = string.Empty;
                                    string name_ = string.Empty;
                                    string message_ = string.Empty;
                                    string remedy_ = string.Empty;
                                    SeverityLevels severity_ = SeverityLevels.Low;

                                    foreach (XmlAttribute attr_ in element_.Attributes)
                                    {
                                        switch (attr_.Name.ToLower())
                                        {
                                            case "id":
                                                code_ = Convert.ToInt32(attr_.Value);
                                                break;
                                            case "extra":
                                                extra_ = attr_.Value;
                                                break;
                                            case "name":
                                                name_ = attr_.Value;
                                                break;
                                            case "enabled":
                                                enabled_ = Convert.ToBoolean(attr_.Value);
                                                break;
                                            case "report":
                                                report_ = Convert.ToBoolean(attr_.Value);
                                                break;
                                            case "severity":
                                                severity_ = (SeverityLevels)Enum.Parse(typeof(SeverityLevels), attr_.Value);
                                                break;
                                        }
                                    }

                                    foreach (XmlNode child_ in element_.ChildNodes)
                                    {
                                        if (child_.Attributes["culture"].Value == cultureCode)
                                        {
                                            foreach (XmlNode node_ in child_.ChildNodes)
                                            {
                                                switch (node_.Name.ToLower())
                                                {
                                                    case "message":
                                                        message_ = node_.InnerText;
                                                        break;
                                                    case "remedy":
                                                        remedy_ = node_.InnerText;
                                                        break;
                                                }
                                            }

                                            AddAlarmData(code_, name_, severity_, message_, enabled_, report_, extra_, string.Empty, string.Empty, string.Empty, remedy_);
                                            break;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }
        #endregion
    }
}
#endregion