#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Program
namespace TechFloor.Components
{
    public class BarcodeKeyInData
    {
        #region Fields
        public BarcodeInputStates State = BarcodeInputStates.Wait;

        public MaterialData Data = new MaterialData();
        #endregion

        #region Constructors
        public BarcodeKeyInData(BarcodeInputStates state = BarcodeInputStates.Wait)
        {
            State = state;
        }
        #endregion

        #region Public methods
        public void Clear()
        {
            State = BarcodeInputStates.Wait;
            Data.Clear();
        }

        public void CopyFrom(BarcodeKeyInData src)
        {
            State = src.State;
            Data.CopyFrom(src.Data);
        }

        public void CopyMaterialData(MaterialData data)
        {
            Data.CopyFrom(data);
        }
        #endregion
    }
}
#endregion