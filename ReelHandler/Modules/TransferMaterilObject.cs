using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marcus.Solution.TechFloor
{
    public class TransferMaterilObject
    {
        #region Enumerations
        public enum TransferModes
        {
            Unknown,
            Load,
            LoadReturn,
            Unload
        }

        public enum TransferStates
        {
            None,
            ConfirmLoad,
            ConfirmedBarcode,
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
            ReturnStage,
            Tower1Port,
            Tower2Port,
            Tower3Port,
            Tower4Port,
            Output1,
            Output2,
            Output3
        }
        #endregion

        #region Fields
        private static TransferMaterilObject instance_ = null;
        protected TransferModes mode = TransferModes.Unknown;
        protected TransferStates state = TransferStates.None;
        protected TransferPorts transferSource = TransferPorts.Unknown;
        protected TransferPorts trasferDestination = TransferPorts.Unknown;
        protected MaterialData data = new MaterialData();
        #endregion

        #region Properties
        public TransferModes Mode => mode;
        public TransferStates State => state;
        public TransferPorts TransferSource => transferSource;
        public TransferPorts TrasferDestination => trasferDestination;
        public MaterialData Data => data;

        public static TransferMaterilObject Instance
        {
            get
            {
                if (instance_ == null)
                    instance_ = new TransferMaterilObject();
                return instance_;
            }
        }
        #endregion

        #region Events
        public event EventHandler ChangedInformation;
        #endregion

        #region Constructors
        protected TransferMaterilObject()
        {
        }
        #endregion

        public void AttachEventHander(EventHandler func)
        {
            ChangedInformation += func;
        }

        public void DetachEventHandler(EventHandler func)
        {
            ChangedInformation -= func;
        }

        public void SetMode(TransferModes val)
        {
            mode = val;
            ChangedInformation?.Invoke(instance_, EventArgs.Empty);
        }

        public void SetState(TransferStates val)
        {
            state = val;
            ChangedInformation?.Invoke(instance_, EventArgs.Empty);
        }
    }
}
