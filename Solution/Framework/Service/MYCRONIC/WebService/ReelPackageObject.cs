using System;

namespace TechFloor
{
    public class ReelPackageObject
    {
        #region Enumerations
        public enum TransferModes
        {
            Unknown,
            PrepareToLoad,
            Load,
            PrepareToLoadReturn,
            LoadReturn,
            PrepareToUnload,
            Unload
        }

        public enum TransferStates
        {
            Unknown,
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
            CompleteUnload
        }

        public enum TransferPorts
        {
            Unknown,
            WorkSlo1OfCart7,
            WorkSlo2OfCart7,
            WorkSlo3OfCart7,
            WorkSlo4OfCart7,
            WorkSlo5OfCart7,
            WorkSlo6OfCart7,
            WorkSlo1OfCart13,
            WorkSlo2OfCart13,
            WorkSlo3OfCart13,
            WorkSlo4OfCart13,
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
            Reject1
        }
        #endregion

        #region Fields
        protected TransferModes mode = TransferModes.Unknown;
        protected TransferStates state = TransferStates.Unknown;
        protected TransferPorts transferSource = TransferPorts.Unknown;
        protected TransferPorts transferDestination = TransferPorts.Unknown;
        protected MaterialData data = new MaterialData();
        #endregion

        #region Properties
        public virtual TransferModes Mode => mode;
        public virtual TransferStates State => state;
        public virtual TransferPorts TransferSource => transferSource;
        public virtual TransferPorts TransferDestination => transferDestination;
        public virtual MaterialData Data => data;
        #endregion

        #region Events
        public virtual event EventHandler ChangedInformation;
        #endregion

        #region Constructors
        protected ReelPackageObject()
        {
        }
        #endregion

        #region Public methods
        public virtual void AttachEventHander(EventHandler func)
        {
            ChangedInformation += func;
        }

        public virtual void DetachEventHandler(EventHandler func)
        {
            ChangedInformation -= func;
        }

        public virtual void FireChangedInformation()
        {
            ChangedInformation?.Invoke(this, EventArgs.Empty);
        }

        public virtual void SetMode(TransferModes val)
        {
            switch (mode = val)
            {
                case TransferModes.Unknown:
                case TransferModes.PrepareToLoad:
                case TransferModes.PrepareToLoadReturn:
                case TransferModes.PrepareToUnload:
                    state = TransferStates.Unknown;
                    break;
            }

            FireChangedInformation();
        }

        public virtual void SetState(TransferStates val)
        {
            switch (state = val)
            {
                case TransferStates.ConfirmedBarcodeOfCart:
                    mode = TransferModes.Load;
                    break;
            }
            FireChangedInformation();
        }

        public virtual void SetStateWithRoute(TransferStates val, ReelDiameters reeltype, int tower)
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

        public virtual void SetStateWithRoute(TransferStates val, ReelDiameters reeltype, int workslot, int tower)
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
                                transferSource = TransferPorts.WorkSlo1OfCart7;
                                break;
                            case 2:
                                transferSource = TransferPorts.WorkSlo2OfCart7;
                                break;
                            case 3:
                                transferSource = TransferPorts.WorkSlo3OfCart7;
                                break;
                            case 4:
                                transferSource = TransferPorts.WorkSlo4OfCart7;
                                break;
                            case 5:
                                transferSource = TransferPorts.WorkSlo5OfCart7;
                                break;
                            case 6:
                                transferSource = TransferPorts.WorkSlo6OfCart7;
                                break;
                        }
                    }
                    break;
                case ReelDiameters.ReelDiameter13:
                    {
                        switch (workslot)
                        {
                            case 1:
                                transferSource = TransferPorts.WorkSlo1OfCart13;
                                break;
                            case 2:
                                transferSource = TransferPorts.WorkSlo2OfCart13;
                                break;
                            case 3:
                                transferSource = TransferPorts.WorkSlo3OfCart13;
                                break;
                            case 4:
                                transferSource = TransferPorts.WorkSlo4OfCart13;
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

        public virtual void SetStateWithRoute(TransferStates val, TransferPorts src, TransferPorts dest)
        {
            switch (state = val)
            {
                case TransferStates.ConfirmedBarcodeOfCart:
                    mode = TransferModes.Load;
                    break;
            }

            transferSource = src;
            transferDestination = dest;
            FireChangedInformation();
        }

        public virtual void SetTransferDestination(TransferPorts dest)
        {
            transferDestination = dest;
            FireChangedInformation();
        }

        public virtual void SetTransferRoute(TransferPorts src, TransferPorts dest)
        {
            transferSource = src;
            transferDestination = dest;
            FireChangedInformation();
        }
        #endregion
    }
}
