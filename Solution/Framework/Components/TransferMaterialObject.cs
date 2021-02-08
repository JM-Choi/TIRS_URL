#region Imports
using TechFloor.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Program
namespace TechFloor.Components
{
    public class TransferMaterialObject
    {
        #region Enumerations
        public enum TransferModes
        {
            PrepareToRejectReturnReel = -4,
            PrepareToRejectCartReel = -3,
            PrepareToRejectUnloadReel = -2,
            Reject = -1,
            None,
            PrepareToLoad,
            Load,
            PrepareToLoadReturn,
            LoadReturn,
            PrepareToUnload,
            Unload,
        }

        public enum TransferStates
        {
            None,
            RequestToLoadConfirm,
            WaitForLoadConfirm,
            ConfirmLoad,
            RequestToBarcodeConfirm,
            WaitForBarcodeConfirm,
            ConfirmedBarcodeOfCart,
            ConfirmedBarcodeOfReturn,
            RequestToLoadAssignment,
            WaitForLoadAssignment,
            CompleteLoad,
            VerifiedUnload,
            TakenMaterial,
            RequestToUnloadAssignment,
            WaitForUnloadAssignment,
            CompleteUnload
        }

        public enum TransferPorts
        {
            None,
            WorkSlot1OfCart7,
            WorkSlot2OfCart7,
            WorkSlot3OfCart7,
            WorkSlot4OfCart7,
            WorkSlot5OfCart7,
            WorkSlot6OfCart7,
            WorkSlot1OfCart13,
            WorkSlot2OfCart13,
            WorkSlot3OfCart13,
            WorkSlot4OfCart13,
            ReturnStageReel7,
            ReturnStageReel13,
            Tower1Port,
            Tower2Port,
            Tower3Port,
            Tower4Port,
            Output1,
            Output2,
            Output3,
            Output4,
            Output5,
            Output6,
            RejectPort,
        }
        #endregion

        #region Fields
        protected TransferModes mode = TransferModes.None;
        protected TransferStates state = TransferStates.None;
        protected TransferPorts transferSource = TransferPorts.None;
        protected TransferPorts transferDestination = TransferPorts.None;
        protected MaterialData data = new MaterialData();
        #endregion

        #region Properties
        public TransferModes Mode => mode;
        public TransferStates State => state;
        public TransferPorts TransferSource => transferSource;
        public TransferPorts TransferDestination => transferDestination;
        public MaterialData Data => data;
        #endregion

        #region Events
        public event EventHandler ChangedInformation;
        #endregion

        #region Constructors
        protected TransferMaterialObject()
        {
        }
        #endregion

        #region Public methods
        public void AttachEventHander(EventHandler func)
        {
            ChangedInformation += func;
        }

        public void DetachEventHandler(EventHandler func)
        {
            ChangedInformation -= func;
        }

        public void FireChangedInformation()
        {
            ChangedInformation?.Invoke(this, EventArgs.Empty);
        }

        // UPDATED: 20200408 (Marcus)
        // Support reject reel operation.
        public void SetMode(TransferModes val)
        {
            switch (mode = val)
            {
                case TransferModes.None:
                case TransferModes.PrepareToLoad:
                case TransferModes.PrepareToLoadReturn:
                case TransferModes.PrepareToUnload:
                    state = TransferStates.None;
                    break;
            }

            FireChangedInformation();
        }

        public void SetState(TransferStates val)
        {
            switch (state = val)
            {
                case TransferStates.ConfirmedBarcodeOfCart:
                    mode = TransferModes.Load;
                    break;
                case TransferStates.VerifiedUnload:
                    mode = TransferModes.Unload;
                    break;
            }

            FireChangedInformation();
        }

        public void SetStateWithRoute(TransferStates val, ReelDiameters reeltype, int tower)
        {
            switch (state = val)
            {
                case TransferStates.ConfirmedBarcodeOfCart:
                    mode = TransferModes.Load;
                    break;
                case TransferStates.ConfirmedBarcodeOfReturn:
                    mode = TransferModes.LoadReturn;
                    break;
            }

            switch (reeltype)
            {
                case ReelDiameters.ReelDiameter7:
                    transferSource = TransferPorts.ReturnStageReel7;
                    break;
                case ReelDiameters.ReelDiameter13:
                    transferSource = TransferPorts.ReturnStageReel13;
                    break;
            }

            switch (tower)
            {
                case 1:
                    transferDestination = TransferPorts.Tower1Port;
                    break;
                case 2:
                    transferDestination = TransferPorts.Tower2Port;
                    break;
                case 3:
                    transferDestination = TransferPorts.Tower3Port;
                    break;
                case 4:
                    transferDestination = TransferPorts.Tower4Port;
                    break;
            }

            FireChangedInformation();
        }

        public void SetStateWithRoute(TransferStates val, ReelDiameters reeltype, int workslot, int tower)
        {
            switch (state = val)
            {
                case TransferStates.ConfirmedBarcodeOfCart:
                    mode = TransferModes.Load;
                    break;
                case TransferStates.ConfirmedBarcodeOfReturn:
                    mode = TransferModes.LoadReturn;
                    break;
            }

            switch (reeltype)
            {
                case ReelDiameters.ReelDiameter7:
                    {
                        switch (workslot)
                        {
                            case 1:
                                transferSource = TransferPorts.WorkSlot1OfCart7;
                                break;
                            case 2:
                                transferSource = TransferPorts.WorkSlot2OfCart7;
                                break;
                            case 3:
                                transferSource = TransferPorts.WorkSlot3OfCart7;
                                break;
                            case 4:
                                transferSource = TransferPorts.WorkSlot4OfCart7;
                                break;
                            case 5:
                                transferSource = TransferPorts.WorkSlot5OfCart7;
                                break;
                            case 6:
                                transferSource = TransferPorts.WorkSlot6OfCart7;
                                break;
                        }
                    }
                    break;
                case ReelDiameters.ReelDiameter13:
                    {
                        switch (workslot)
                        {
                            case 1:
                                transferSource = TransferPorts.WorkSlot1OfCart13;
                                break;
                            case 2:
                                transferSource = TransferPorts.WorkSlot2OfCart13;
                                break;
                            case 3:
                                transferSource = TransferPorts.WorkSlot3OfCart13;
                                break;
                            case 4:
                                transferSource = TransferPorts.WorkSlot4OfCart13;
                                break;
                        }
                    }
                    break;
            }

            switch (tower)
            {
                case 1:
                    transferDestination = TransferPorts.Tower1Port;
                    break;
                case 2:
                    transferDestination = TransferPorts.Tower2Port;
                    break;
                case 3:
                    transferDestination = TransferPorts.Tower3Port;
                    break;
                case 4:
                    transferDestination = TransferPorts.Tower4Port;
                    break;
            }

            FireChangedInformation();
        }

        public void SetStateWithRoute(TransferStates val, TransferPorts src, TransferPorts dest)
        {
            switch (state = val)
            {
                case TransferStates.ConfirmedBarcodeOfCart:
                    mode = TransferModes.Load;
                    break;
                case TransferStates.VerifiedUnload:
                    mode = TransferModes.Unload;
                    break;
            }

            transferSource = src;
            transferDestination = dest;
            FireChangedInformation();
        }

        public void SetStateWithRoute(TransferStates val, MaterialStorageState obj)
        {
            switch (state = val)
            {
                case TransferStates.VerifiedUnload:
                    {
                        mode = TransferModes.Unload;

                        switch (obj.Index)
                        {
                            case 1: transferSource = TransferPorts.Tower1Port; break;
                            case 2: transferSource = TransferPorts.Tower2Port; break;
                            case 3: transferSource = TransferPorts.Tower3Port; break;
                            case 4: transferSource = TransferPorts.Tower4Port; break;
                        }

                        // UPDATED: 20200408 (Marcus)
                        // Support max 6 output stages.
                        switch (obj.OutputStageIndex)
                        {
                            case 1: transferDestination = TransferPorts.Output1; break;
                            case 2: transferDestination = TransferPorts.Output2; break;
                            case 3: transferDestination = TransferPorts.Output3; break;
                            case 4: transferDestination = TransferPorts.Output4; break;
                            case 5: transferDestination = TransferPorts.Output5; break;
                            case 6: transferDestination = TransferPorts.Output6; break;
                        }

                        if (Data != null && obj.PendingData != null)
                        {
                            Data.Name = obj.PendingData.Name;
                            // Data.Text = obj.PendingData.Text;
                        }
                    }
                    break;
            }

            FireChangedInformation();
        }

        public void SetTransferDestination(TransferPorts dest)
        {
            transferDestination = dest;
            FireChangedInformation();
        }

        public void SetTransferRoute(TransferPorts src, TransferPorts dest)
        {
            transferSource = src;
            transferDestination = dest;
            FireChangedInformation();
        }
        #endregion
    }
}
#endregion