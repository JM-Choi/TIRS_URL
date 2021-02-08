#region Imports
using TechFloor.Object;
#endregion

#region Program
namespace TechFloor.Components
{
    public class AlarmData
    {
        #region Fields
        protected bool enabled;

        protected bool requiredReport;

        protected int code;

        protected SeverityLevels severity;

        protected string name;

        protected string extra;

        protected string message;

        protected string module;

        protected string description;

        protected string cause;

        protected string remedy;
        #endregion

        #region Properties
        public bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }

        public bool RequiredReport
        {
            get => requiredReport;
            set => requiredReport = value;
        }

        public int Code => code;

        public string Extra => extra;

        public string Name => name;

        public SeverityLevels Severity => severity;

        public string Message
        {
            get => message;
            set => message = value;
        }

        public string Module
        {
            get => module;
            set => module = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public string Cause
        {
            get => cause;
            set => cause = value;
        }

        public string Remedy
        {
            get => remedy;
            set => remedy = value;
        }
        #endregion

        #region Constructors
        public AlarmData(int code, string name, SeverityLevels level = SeverityLevels.Low, string message = null, bool enabled = true, bool report = true, string extra = null, string module= null, string description= null, string cause= null, string remedy= null)
        {
            this.enabled = enabled;
            this.requiredReport = report;
            this.severity = level;
            this.code = code;
            this.name = name;
            this.extra = extra;
            this.message = message;
            this.module = module;
            this.description = description;
            this.cause = cause;
            this.remedy = remedy;
        }
        #endregion
    }
}
#endregion