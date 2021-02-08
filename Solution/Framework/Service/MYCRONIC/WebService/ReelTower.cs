#region Imports
using TechFloor.Object;
using RoyoTech.StSys.WebService.ComTowerApp.Contract;
using System.Net;
using System.Xml;
using System.Xml.Linq;
#endregion

#region Program
namespace TechFloor.Service.MYCRONIC.WebService
{
    public class ReelTower
    {
        #region Enumerations
        public enum ErrorCodes
        {
            Tray_16_44_not_released = 100, // Tray box is not released.
            Reel_height_exceeded_reel = 101, // The reel is too high.
            Reel_not_recognized = 102, // The reel is out of dimension.
            Tray_not_recognized = 103, // The tray box is not detected.
            Reel_diameter_too_big = 104, // The diameter of the carrier is too large.
            Reel_diameter_not_recognized = 105, // The diameter cannot be measured.
            No_scancode_during_test_run = 106, // Barcode not detected during test run.
            No_scancode_during_loading = 107, // Barcode not detected when loading carrier.
            No_scancode_during_unloading = 108, // Barcode not detected when unloading carrier.
            No_scancode_after_power_on = 109, // Barcode not detected after power-on sequence.
            Invalid_data = 116, // The storage tower receives data that cannot be processed in the current state.
            Operation_denied = 117, // Storage tower receives commands that cannot be executed in the current state.
            Code_error = 118, // Data checksum error.
            Error_arm_motor = 175, // The gripper arm motor responds with an error during calibration.
            Arm_sensor_defect = 176, // The gripper arm sensor is not detected.
            No_connection_between_arm_sensor_and_motor = 177, // The gripper arm motor does not recognize the gripper arm sensor in rest position.
            Error_arm_sensor = 178, // Error in reﬂective sensor.
            Slot_difference = 179, // The slot height measurement is more than 1.5 mm from the nominal value.
            Incorrect_ﬁrmware_scanner = 183, // The scanner PCB ﬁrmware version is not compatible with the central PCB ﬁrmware.
            Oncorrect_arm_ﬁrmware = 184, // The gripper arm PCB ﬁrmware version is not compatible with the central PCB ﬁrmware.
            Humidity_logger_not_responding = 185, // No connection to the humidity logger.
            Fault_arm_motor = 186, // Error output from the gripper arm motor.
            Error_voltage_3_3_V = 187, // The power center PCB is below <1.8 V.
            Error_voltage_60_V = 188, // The power supply for the lift motor is below 48 V.
            Rotor_motor_not_recognized = 189, // The rotor motor does not answer.
            Lift_motor_not_recognized = 190, // The lift motor does not answer.
            Invalid_reel_height = 191, // The reel or tray is too high, or out of speciﬁcation.
            Gripper_too_high = 192, // The comb of the gripper height has shifted.
            Gripper_too_low = 193, // The comb of the gripper height has shifted.
            Arm_not_recognized_while_calibrating_lift = 194, // The gripper arm is not detected during lift calibration.
            Arm_not_recognized_while_calibrating_rotor = 195, // The gripper arm is not detected during rotor calibration.
            Invalid_stack = 196, //An incorrect stack was received from STSys.
            Missing_or_invalid_ﬁrmware_in_rotor_motor = 197, // The ﬁrmware rotor motor is defective or cannot be reprogrammed.
            Missing_or_invalid_ﬁrmware_in_lift_motor = 198, // The ﬁrmware lift motor is defective or cannot be reprogrammed.
            Lift_Step_error = 201, // Time-out during installation.
            Command_not_recognized_by_lift_motor = 204, // The motor repor ts an invalid command.
            Lift_Sensor_error_encoder = 205, //Difference between the stepper motor and the encoder.
            Lift_motor_Invalid_data = 208, // The lifts motor sends the wrong value
            Lift_motor_unknown_error = 209, // Unknown error.
            Sensor_error_encoder = 215, // Difference between the stepper motor and the encoder.
            Command_not_recognized_by_rotor_motor = 217, // The motor repor ts an invalid command.
            Rotor_motor_Invalid_data = 218, // The motor reports invalid data.
            Rotor_motor_unknown_error = 219, // Unknown error.
            Error_lid_while_opening = 221, // Error opening safety hatch.
            Error_lid_Opening_top = 222, // Error opening the safety hatch.
            Error_lid_Closing_top = 224, // Error closing the safety hatch. The hatch is trying to close, but the top switch does not open.
            Error_lid_Closing_bottom = 225, // Error closing the safety hatch. The hatch is trying to close, but the top switch does not open.
            Lid_open_while_driving = 229, // Safety hatch open during movement.
            No_response_from_arm_motor = 230, // The gripper arm motor does not respond.
            Error_clamping_on_closing_1 = 231, // Difference between the stepper motor and the comb. When closing, the sensor should be blocked, but it is open (the sensor is behind the comb).
            Error_clamping_on_closing_2 = 232,
            Error_clamping_on_opening_1 = 233, // Difference between the stepper motor and the comb. When closing, the sensor should be blocked, but it is open (the sensor is behind the comb).
            Error_clamping_on_opening_2 = 234,
            Error_arm_Init_to_front = 235, // The sensor does not switch to front position during initialization.
            Error_arm_Init_to_back = 236, // The sensor does not switch to back position during initialization.
            Error_arm_blocked = 237, // Difference between the stepper motor and the encoder.
            Arm_not_in_standstill = 238, // The arm is not in the rest position when drive is off.
            Arm_board_not_recognized = 239, // The arm board does not answer.
            Scanner_board_not_recognized = 240, // The scanner board does not answer.
            Error_clamping_on_init_1 = 241, // Difference between stepper motor and sensor slot plate during initialization.
            Error_clamping_on_init_2 = 242,
            Error_clamping_while_driving_1 = 243, // Difference between stepper motor and sensor slot plate during initialization.
            Error_clamping_while_driving_2 = 244,
            Missing_or_invalid_ﬁrmware_in_arm_motor = 245, // The motor ﬁrmware cannot be programmed.
            No_tableau_on_magazine_calibration = 246, // The reﬂex sensor does not ﬁnd the magazine shelf.
            No_screening_control_rotor = 247, // The calibration hole on the magazine shelf was not found during rotor movement.
            No_screening_control_arm = 248, // The calibration hole on the magazine shelf was not found during rotor movement.
            Error_reel_sensor = 249, // The middle sensor is covered.
            Error_lid = 250, // Unknown error, safety hatch.
            Error_arm = 251, // Unknown error, gripper arm.
            Error_clamping = 252, // Unknown error, clamping.
            Error_scanner_setting = 253, // Programming of the scanner failed.
            Calibration_of_diameter_failed = 254, // Sensors were covered during initialization.
            Calibration_of_height_failed = 255, // Sensors were covered during initialization.
            Invalid_scancode = 777, // Invalid barcode
            Timeout = 999, // The storage tower has not responded after 12 seconds.
        }
        #endregion

        #region Fields
        protected int index;

        protected string id;

        protected string name;

        protected IPEndPoint endpoint;

        protected TowerDetailInformation info = null;
        #endregion

        #region Properties
        public virtual bool IsValid => !string.IsNullOrEmpty(name) && endpoint != null;

        public virtual int Index => index;

        public virtual string Id => id;

        public virtual string Name => name;

        public virtual string Usage { get; set; }

        public virtual IPEndPoint EndPoint => endpoint;

        public virtual TowerDetailInformation Info => info;
        #endregion

        #region Constructors
        protected ReelTower() { }

        public ReelTower(int index, TowerDetailInformation info = null)
        {
            this.index = index;

            if (info != null)
                Create(index, info);
        }

        public ReelTower(int index, string id, string name, string address, int port)
        {
            this.index = index;
            this.id = id;
            this.name = name;
            
            if (!string.IsNullOrEmpty(address) && StringLogicalComparer.IsIpAddress(address))
                this.endpoint = new IPEndPoint(IPAddress.Parse(address), port); 
        }

        public ReelTower(XmlNode node)
        {
            Create(node);
        }
        #endregion

        #region Protected methods
        #endregion

        #region Public methods
        public virtual void Init(TowerDetailInformation info)
        {
            this.info = info;
        }

        public virtual bool Create(int index, TowerDetailInformation info)
        {
            bool result_ = false;
            this.index = index;
            this.id = info.TowerId;
            this.name = info.TowerName;
            this.info = info;
            return result_;
        }

        public virtual bool Create(XmlNode node)
        {
            bool result_ = false;
            string address_ = string.Empty;
            int port_ = 0;

            if (node != null)
            {
                foreach (XmlAttribute attr_ in node.Attributes)
                {
                    switch (attr_.Name.ToLower())
                    {
                        case "index":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                    index = int.Parse(attr_.Value);
                            }
                            break;
                        case "id":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                    id = attr_.Value;
                            }
                            break;
                        case "name":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                    name = attr_.Value;
                            }
                            break;
                        case "address":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                    address_ = attr_.Value;
                            }
                            break;
                        case "port":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                    port_ = int.Parse(attr_.Value);
                            }
                            break;
                        case "usage":
                            {
                                if (!string.IsNullOrEmpty(attr_.Value))
                                    Usage = attr_.Value;
                            }
                            break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(address_) && StringLogicalComparer.IsIpAddress(address_))
                endpoint = new IPEndPoint(IPAddress.Parse(address_), port_);

            if (!string.IsNullOrEmpty(name) && endpoint != null)
                result_ = true;

            return result_;
        }

        public virtual XElement ToXml(string nodename= null)
        {
            return new XElement(string.IsNullOrEmpty(nodename) ? GetType().Name : nodename,
                new XAttribute("index", index),
                new XAttribute("id", id),
                new XAttribute("name", name),
                new XAttribute("address", endpoint.Address),
                new XAttribute("port", endpoint.Port),
                new XAttribute("usage", Usage));
        }

        public virtual void UpdateInfo(TowerDetailInformation info)
        {
            this.info = info;
        }
        #endregion
    }
}
#endregion