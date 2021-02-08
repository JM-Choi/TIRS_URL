using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechFloor.Object
{
    public interface IDigitalIoManager
    {
        #region Properties
        bool Initialized { get; }

        bool SignalTowerRed { get; set; }

        bool SignalTowerYellow { get; set; }

        bool SignalTowerGreen { get; set; }

        bool SignalTowerBlue { get; set; }

        bool SignalTowerWhite { get; set; }

        bool Buzzer { get; set; }

        bool IsSimulation { get; }
        #endregion

        #region Events
        event EventHandler<int> InputSignalChanged;
        event EventHandler<int> OutputSignalChanged;
        #endregion

        #region Public methods
        bool GetInput(int channel);

        bool GetOutput(int channel);

        bool SetInput(int channel, bool state); // Only simulation

        bool SetOutput(int channel, bool state);

        void SetSignalTower(OperationStates operationstate, bool buzzeron = false);

        void UpdateInputs();

        void UpdateOutputs();
        #endregion
    }
}
