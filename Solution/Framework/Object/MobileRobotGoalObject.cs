#region Imports
using System.Xml.Serialization;
#endregion

#region Program
namespace TechFloor.Object
{
    public class MobileRobotGoalObject
    {
        #region Properties
        [XmlAttribute("id")]
        public string goalId    { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string goalName  { get; set; } = string.Empty;

        [XmlAttribute("x")]
        public string goalX     { get; set; } = string.Empty;

        [XmlAttribute("y")]
        public string goalY     { get; set; } = string.Empty;

        [XmlAttribute("desc")]
        public string goalDesc  { get; set; } = string.Empty;
        #endregion
    }
}
#endregion