#region Imports
using System;
#endregion

#region Program
namespace TechFloor.Service.MYCRONIC.WebService
{
    public class MaterialEventArgs : EventArgs
    {
        #region Enumerations
        public enum MaterialEvents
        {
            Unknown,
            Created,
            Arrived,
            Removed,
            Blocked,
        }

        public enum MaterialActions
        {
            NotDefined,
            PreapareLoad,
            Load,
            CompleteLoad,
            Processing,
            PreapareUnload,
            Unload,
            CompleteUnload,
        }
        #endregion

        #region Fields
        public readonly string Equipment;

        public readonly string Port;

        public readonly string MaterialId;

        public readonly string MaterialName;

        public readonly MaterialEvents EventType;

        public readonly MaterialActions Action;

        public readonly object Data;
        #endregion

        #region Constructors
        public MaterialEventArgs(string equipment, string port, string id, string name, MaterialActions action, object data = null)
        {
            this.Equipment = equipment;
            this.Port = port;
            this.MaterialId = id;
            this.MaterialName = name;
            this.EventType = MaterialEvents.Unknown;
            this.Action = action;
            this.Data = data;
        }

        public MaterialEventArgs(string equipment, string port, string id, string name, MaterialEvents type, object data = null)
        {
            this.Equipment = equipment;
            this.Port = port;
            this.MaterialId = id;
            this.MaterialName = name;
            this.EventType = type;
            this.Action = MaterialActions.NotDefined;
            this.Data = data;
        }

        public MaterialEventArgs(string equipment, string port, string id, string name, MaterialActions action, MaterialEvents type, object data = null)
        {
            this.Equipment = equipment;
            this.Port = port;
            this.MaterialId = id;
            this.MaterialName = name;
            this.EventType = type;
            this.Action = action;
            this.Data = data;
        }
        #endregion
    }
}
#endregion