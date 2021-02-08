#region Imports
using System.Collections.Generic;
using System.Runtime.Serialization;
#endregion

#region Program
namespace TechFloor.Object
{
    [DataContract(Name = "response")]
    public class MobileRobotServiceResponse
    {
        [DataMember(Name = "code")]
        public ResponseCodes Code;

        [DataMember(Name = "command")]
        public string Command;

        [DataMember(Name = "message")]
        public string Message;

        [DataMember(Name = "data")]
        public object Data;
    }

    [DataContract(Name = "header")]
    public class MobileRobotServiceRequestHeader
    {
        [DataMember(Name = "siteid")]
        public string SiteId;

        [DataMember(Name = "voyageid")]
        public string VoyageId;

        [DataMember(Name = "robotid")]
        public string RobotId;
    }

    [DataContract(Name = "orderinfo")]
    public class MobileRobotServiceRequestOrderInformation
    {
        [DataMember(Name = "orderid")]
        public int OrdererId;

        [DataMember(Name = "host")]
        public string Host;

        [DataMember(Name = "destination")]
        public string DestinationName;

        [DataMember(Name = "item")]
        public List<string> Items;
    }

    [DataContract(Name = "abortinfo")]
    public class MobileRobotServiceRequestAbortInformation
    {
        [DataMember(Name = "orderinfo")]
        public MobileRobotServiceRequestOrderInformation Order;

        [DataMember(Name = "abortid")]
        public int AbortId;

        [DataMember(Name = "reason")]
        public string Reason;
    }

    [DataContract(Name = "commandinfo")]
    public class MobileRobotServiceRequestCommandInformation
    {
        [DataMember(Name = "command")]
        public string Command;

        [DataMember(Name = "parameters")]
        public List<string> Parameters;
    }

    [DataContract(Name = "jobinfo")]
    public class MobileRobotServiceRequestJobInformation
    {
        [DataMember(Name = "jobid")]
        public string JobId;
    }

    #region Extension classes
    [DataContract(Name = "request")]
    public class MobileRobotServiceRequestCreate
    {
        [DataMember(Name = "orderinfo")]
        public MobileRobotServiceRequestOrderInformation OrderInfo;
    }

    [DataContract(Name = "request")]
    public class MobileRobotServiceRequestAbort
    {
        [DataMember(Name = "abortinfo")]
        public MobileRobotServiceRequestAbortInformation AbortInfo;
    }

    [DataContract(Name = "request")]
    public class MobileRobotServiceRequestCommand
    {
        [DataMember(Name = "commandinfo")]
        public MobileRobotServiceRequestCommandInformation CommandInfo;
    }

    [DataContract(Name = "request")]
    public class ServiceRequestJobState
    {
        [DataMember(Name = "jobinfo")]
        public MobileRobotServiceRequestJobInformation JobInfo;
    }

    [DataContract(Name = "request")]
    public class MobileRobotServiceRequestRobotState
    {
        [DataMember(Name = "jobinfo")]
        public MobileRobotServiceRequestJobInformation JobInfo;
    }
    #endregion
}
#endregion