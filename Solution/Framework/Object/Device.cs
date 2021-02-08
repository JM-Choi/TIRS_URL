#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Program
namespace TechFloor.Device
{
    #region Enumerations
    public enum CompositeDeviceModel
    {
        AnalogIo,
        DigitalIo,
        Counter,
        CommunicationIo
    }

    public enum StateCollectionTask
    {
        Call,
        Polling,
        EventDriven
    }

    public enum DeviceManufacturer
    {
        Custom,
        Cognex,
        Comizoa,
        Contec,
        Epson,
        Harmon,
        Rsa,
        Ajinextek,
        Adlink,
        Ni,
        Ti,
        Sehan,
        Dongdo,
        UR,
        HandheldProducts,
        Mycronic,
        Keyence,
        KpVision,
        Omron,
        Motorola,
        Zebra,
        Fastech,
        Panasonic,
        Banner,
        Datalogic,
        MicroScan,
        ComfilePi,
    }

    public enum Customers
    {
        Amkor,
        SamsungElectronics,
        SamsungElectronMeachanics,
        LGElectronics,
    }

    public enum PhysicalDevices
    {
        DigitalIo,
        AnalogIo,
        Motor,
        Robot,
        Camera,
        Light,
        Cylinder,
        Vacuum,
        Pump,
        SwitchLamp,
        SignalTower,
        BarcodeScanner,
        BarcodeRepeater,
        MicroMeter,
        ScrewDriver,
        Feeder,
        Plc
    }

    public enum LogicalDevices
    {
        Handler,
        Station,
        Conveyor
    }
    #endregion

    public interface IDeviceDescriptor
    {
        #region Properties
        bool Enabled { get; set; }

        string Name { get; set; }

        string Description { get; set; }
        #endregion
    }
    public interface IDeviceManufacturer
    {
        #region Properties
        string Manufacturer { get; set; }

        string Model { get; set; }

        string PartNumber { get; set; }

        string SerialNumber { get; set; }
        #endregion
    }

    public interface IChannelElement : IDeviceDescriptor
    {
        #region Properties
        int Channel { get; set; }

        string Tag { get; set; }
        #endregion
    }

    public interface IIoPolarityElement
    {
        #region Properties
        bool Invert { get; set; }
        #endregion
    }

    public interface IDeviceElement : IDeviceManufacturer, IDeviceDescriptor
    {
        #region Properties
        int Board { get; set; }

        int Id { get; set; }
        #endregion
    }
}
#endregion