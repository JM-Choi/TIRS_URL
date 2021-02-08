#region Imports
#endregion

#region Program
namespace TechFloor.Device
{
    public interface IPhysicalDeviceDriver : IDeviceElement, IChannelElement
    {
        #region Properties
        PhysicalDevices Category { get; set; }
        #endregion

        #region Public methods
        bool Load(string filename = null);

        bool Unload();

        bool Save(string filename = null);

        bool Start();

        bool Stop();
        #endregion
    }

    public interface ILogicalDeviceDriver : IDeviceElement
    {
        #region Properties
        LogicalDevices Category { get; set; }
        #endregion

        #region Public methods
        bool Load(string filename = null);

        bool Unload();

        bool Save(string filename = null);

        bool Start();

        bool Stop();
        #endregion
    }
}
#endregion