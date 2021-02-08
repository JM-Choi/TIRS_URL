#region Imports
#endregion

#region Program
namespace TechFloor.Object
{
    public interface ISimulatable
    {
        #region Properties
        bool Simulation { get; set; }
        #endregion
    }

    public abstract class Simulatable : AbstractClassDisposable, ISimulatable
    {
        #region Fields
        protected bool simulation = false;
        #endregion

        #region Properties
        public virtual bool Simulation
        {
            get => simulation;
            set => simulation = value;
        }
        #endregion
    }

    public abstract class SimulatableDevice : Simulatable
    {
    }
}
#endregion