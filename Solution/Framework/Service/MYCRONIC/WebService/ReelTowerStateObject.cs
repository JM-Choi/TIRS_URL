#region Imports
using TechFloor.Object;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
#endregion

#region Program
namespace TechFloor.Service.MYCRONIC.WebService
{
    public class ReelTowerState : MaterialStorageState
    {
        #region Fields
        protected bool onlineState                                  = false;

        protected int statusCode                                    = 0;

        protected int lastOccurredAlarmCode                         = 0;

        protected int currentOccurredAlarmCode                      = 0;

        protected int lastIdleReportCount                           = 0;

        protected int totalOccurredAlarmCounts                      = 0;

        protected int totalPendingAlarmCounts                       = 0;

        protected DateTime lastAlarmOccurredTime                    = DateTime.MinValue;

        protected DateTime alarmOccurredTime                        = DateTime.MinValue;

        protected TimeSpan alarmTimeSpan                            = TimeSpan.Zero;

        protected MaterialHandlingDestination materialDestination   = MaterialHandlingDestination.None;

        protected DateTime lastIdleReportTime                       = DateTime.MinValue;
        #endregion

        #region Properties
        public virtual MaterialHandlingDestination MaterialDestination => materialDestination;

        public virtual bool RequestToLoad                           => materialDestination == MaterialHandlingDestination.LoadToStorage;

        public virtual bool RequestToUnload                         => materialDestination == MaterialHandlingDestination.UnloadToOutStage;

        public virtual bool RequestToReject                         => materialDestination == MaterialHandlingDestination.UnloadToReject;

        public virtual bool OnlineState
        {
            get => onlineState;
            set
            {
                if (!(onlineState = value))
                    State = MaterialStorageState.StorageOperationStates.Down;
            }
        }

        public override StorageOperationStates State
        {
            get => operationState;
            set
            {
                if (operationState != value)
                {
                    switch (operationState = value)
                    {
                        case StorageOperationStates.RequestedToLoad:
                        case StorageOperationStates.PrepareToLoad:
                            materialDestination = MaterialHandlingDestination.LoadToStorage;
                            break;
                        case StorageOperationStates.RequestedToUnload:
                        case StorageOperationStates.PrepareToUnload:
                            materialDestination = MaterialHandlingDestination.UnloadToOutStage;
                            break;
                        case StorageOperationStates.Unknown:
                        case StorageOperationStates.Idle:
                        case StorageOperationStates.Run:
                        case StorageOperationStates.Down:
                        case StorageOperationStates.Load:
                        case StorageOperationStates.Wait:
                        case StorageOperationStates.Full:
                        case StorageOperationStates.Error:
                            assignedToUnload = false;
                            break;
                        case StorageOperationStates.Unload:
                            break;
                    }

                    FireStorageOperationStateChanged($"Tower={name},State=({operationState} -> {value})");
                }
            }
        }

        public virtual int StatusCode
        {
            get => statusCode;
            set
            {
                switch (statusCode = value)
                {
                    case 26: // Ready, Idle
                        {
                            if (!string.IsNullOrEmpty(StatusText) && StatusText.ToLower() == "ready" && !HasAlarm && OnlineState)
                            {
                                if (materialDestination == MaterialHandlingDestination.None)
                                    State = MaterialStorageState.StorageOperationStates.Idle;

                                ClearAlarm();
                            }
                            else if (HasAlarm)
                                State = MaterialStorageState.StorageOperationStates.Error;
                        }
                        break;
                    case 1030:
                        {   // Only occur in load sequence.
                            materialDestination = MaterialHandlingDestination.None;
                        }
                        break;
                    case 1031: // Locate material
                        {
                            switch (State)
                            {
                                case MaterialStorageState.StorageOperationStates.Idle:
                                case MaterialStorageState.StorageOperationStates.RequestedToLoad:
                                case MaterialStorageState.StorageOperationStates.PrepareToLoad:
                                    State = MaterialStorageState.StorageOperationStates.Load;   // After reel assignment
                                    break;
                                case MaterialStorageState.StorageOperationStates.Load:
                                    materialDestination = MaterialHandlingDestination.None;
                                    break;
                            }
                        }
                        break;
                    case 1032: // Material put in terminal from outside 
                        {
                            switch (State)
                            {
                                case MaterialStorageState.StorageOperationStates.RequestedToLoad:   // After barcode confirm
                                    {
                                        State = MaterialStorageState.StorageOperationStates.PrepareToLoad;  // When a reel placed on terminal
                                        materialDestination = MaterialHandlingDestination.LoadToStorage;
                                    }
                                    break;
                                case MaterialStorageState.StorageOperationStates.RequestedToUnload:
                                    {
                                        State = MaterialStorageState.StorageOperationStates.PrepareToUnload;
                                        materialDestination = MaterialHandlingDestination.UnloadToOutStage;
                                    }
                                    break;
                            }
                        }
                        break;
                    case 1040: // Unloading material 
                        {
                            State = MaterialStorageState.StorageOperationStates.RequestedToUnload;    // After start provide job
                            materialDestination = MaterialHandlingDestination.UnloadToOutStage;
                        }
                        break;
                    case 1041: // Unloaded material from tower 
                        {
                            State = MaterialStorageState.StorageOperationStates.Unload; // When a removed from terminal
                            materialDestination = MaterialHandlingDestination.None;
                        }
                        break;
                    default:
                        {
                            if (value <= 18 || !onlineState)
                                State = MaterialStorageState.StorageOperationStates.Down;
                            else if (value > 3000)
                                State = MaterialStorageState.StorageOperationStates.Error;
                            else
                                State = MaterialStorageState.StorageOperationStates.Unknown;

                            if (HasAlarm)
                            {
                                if (StatusCode >= 10000)
                                    SetAlarm(StatusCode % 10000);
                                else if (StatusCode > 3000)
                                    SetAlarm(StatusCode - 3000);
                            }
                        }
                        break;
                }
            }
        }

        public virtual string StatusText
        {
            get;
            set;
        }

        public virtual bool HasAlarm
        {
            get
            {
                if (StatusCode >= 3000)
                {
                    if (StatusCode >= 10000)
                        return true;
                    else
                        return (StatusCode - 3000 >= 100);
                }

                return false;
            }
        }

        public virtual int LastAlarmCode => lastOccurredAlarmCode;

        public virtual int AlarmCode => currentOccurredAlarmCode;

        public virtual int TotalDailyOccurredAlarms => totalOccurredAlarmCounts;

        public virtual int TotalPendingAlarms => totalPendingAlarmCounts;

        public virtual DateTime LastOccurredAlarmDateTime => lastAlarmOccurredTime;

        public virtual TimeSpan TotalOccurredAlarmTime => alarmTimeSpan;
        #endregion

        #region Constructors
        public ReelTowerState(int index, string id, string name= null, EventHandler<string> handler = null)
            : base(index, id, name) 
        {
            base.StorageOperationStateChanged += handler;
        }
        #endregion

        #region Protected methods
        protected virtual int ClearAlarm()
        {
            try
            {
                if (alarmOccurredTime != DateTime.MinValue && currentOccurredAlarmCode != 0)
                {
                    alarmTimeSpan.Add(DateTime.Now - alarmOccurredTime);
                    alarmOccurredTime = DateTime.MinValue;
                    lastOccurredAlarmCode = currentOccurredAlarmCode;
                    currentOccurredAlarmCode = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return totalPendingAlarmCounts;
        }

        protected virtual int SetAlarm(int alarmcode)
        {
            try
            {
                if (lastAlarmOccurredTime.Date != DateTime.Now.Date && totalOccurredAlarmCounts != 0)
                    totalOccurredAlarmCounts = 0;

                if (currentOccurredAlarmCode != alarmcode)
                {
                    currentOccurredAlarmCode = alarmcode;
                    alarmOccurredTime = DateTime.Now;
                    totalPendingAlarmCounts++;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return totalPendingAlarmCounts;
        }
        #endregion

        #region Public methods
        public virtual void SetState(StorageOperationStates state)
        {
            this.operationState = state;
        }

        public virtual void SetMaterialDestination(MaterialHandlingDestination dest)
        {
            materialDestination = dest;
        }
        #endregion
    }
}
#endregion