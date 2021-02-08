#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Program
namespace TechFloor.Object
{
    public class ComponentReelObject : AbstractClassDisposable
    {
        #region Fields
        protected int requestStageNo = 0;

        protected DateTime requestTime = DateTime.MaxValue;

        protected string name = string.Empty;

        protected string requestTowerId = string.Empty;

        protected string requestUid = string.Empty;

        protected string requestStage = string.Empty;
        #endregion

        #region Properties
        public int RequestStageNo => requestStageNo;

        public DateTime RequestTime => requestTime;

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                    name = value;
            }
        }

        public string RequestTowerId
        {
            get => requestTowerId;
            set => requestTowerId = value;
        }

        public string RequestUid
        {
            get => requestUid;
            set => requestUid = value;
        }

        public string RequestStage
        {
            get => requestStage;
            set => requestStage = value;
        }
        #endregion

        #region Constructors
        public ComponentReelObject()
        {

        }

        public ComponentReelObject(string towerid, string uid, string stage, int stageno = 0)
        {
            this.requestTowerId = towerid;
            this.requestUid = uid;
            this.requestStage = stage;
            this.requestStageNo = stageno;
            this.requestTime = DateTime.Now;
        }
        #endregion
    }
}
#endregion