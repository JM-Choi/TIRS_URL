#region Imports
using TechFloor.Util;
using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Windows.Forms;
#endregion

#region Program
namespace TechFloor.Gui
{
    public partial class ControlDigitalIo : UserControl
    {
        #region Enumerations
        public enum Inputs
        {
            CartInCheck1 = 0,                   // Work zone sensor 1
            CartInCheck2 = 1,                   // Work zone sensor 2
            CartAlignSensorLeftForward = 2,     // Cart align sensor left
            CartAlignSensorLeftBackward = 3,    // Cart align sensor left
            CartAlignSensorRightForward = 4,    // Cart align sensor right
            CartAlignSensorRightBackward = 5,   // Cart align sensor right
            ReelType7 = 6,
            ReelType13 = 7,
            BufferCartDetection1 = 8,           // Cart detection sensor 1
            BufferCartDetection2 = 9,           // Cart detection sensor 2
            CartAlignSensorFrontXForward = 10,  // Cart align sensor front X
            CartAlignSensorFrontXBackward = 11, // Cart align sensor front X
            CartAlignSensorFrontYForward = 12,  // Cart align sensor front Y
            CartAlignSensorFrontYBackward = 13, // Cart align sensor front Y
            MainAir = 14,

            OutputStage1Exist = 16,             // New model (Version 1.0.1.0)
            OutputStage2Exist = 17,             // New model (Version 1.0.1.0)
            OutputStage3Exist = 18,             // New model (Version 1.0.1.0)
            OutputStage4Exist = 19,             // New model (Version 1.0.1.0)
            OutputStage5Exist = 20,             // New model (Version 1.0.1.0)
            OutputStage6Exist = 21,             // New model (Version 1.0.1.0)
            RejectStageFull = 22,               // New model (Version 1.0.1.0)
        }

        public enum Outputs
        {
            LampRed = 0,
            LampYellow = 1,
            LampGreen = 2,
            Buzzer = 3,
            CartAlignCylinderLeft = 4,
            CartAlignCylinderRight = 5,
            CartAlignCylinderFrontX = 6,
            CartAlignCylinderFrontY = 7,
        }
        #endregion

        #region Fields
        protected const int MaxRow = 16;
        protected int inputCount = 0;
        protected int outputCount = 0;
        protected int inputPages = 0;
        protected int outputPages = 0;
        protected int inputPage = 0;
        protected int outputPage = 0;
        #endregion

        #region Constructos
        public ControlDigitalIo()
        {
            InitializeComponent();

            for (int i = 0; i < MaxRow; i++)
            {
                dataGridViewInput.Rows.Add();
                dataGridViewOutput.Rows.Add();
            }
        }
        #endregion
        
        #region Protected methods
        protected virtual void ChangeColor(DataGridViewCellStyle style, bool elementSignal)
        {
            if (elementSignal)
            {
                style.SelectionBackColor = style.BackColor = Color.Lime;
                style.SelectionForeColor = style.ForeColor = Color.Black;
            }
            else
            {
                style.SelectionBackColor = style.BackColor = Color.DarkGreen;
                style.SelectionForeColor = style.ForeColor = Color.White;
            }
        }

        protected virtual void UpdateInputPage()
        {
            int id = inputPage * MaxRow;
            int max = id + MaxRow;
            string str = string.Empty;

            if (App.DigitalIoManager != null)
            {
                for (int i_ = id, j_ = 0; i_ < max; i_++, j_++, id++)
                {
                    if (Enum.IsDefined(typeof(Inputs), i_))
                    {
                        switch ((Inputs)i_)
                        {
                            case Inputs.CartInCheck1:
                                str = "CART IN CHECK 1";
                                break;
                            case Inputs.CartInCheck2:
                                str = "CART IN CHECK 2";
                                break;
                            case Inputs.CartAlignSensorLeftForward:
                                str = "CART ALIGN L FWD";
                                break;
                            case Inputs.CartAlignSensorLeftBackward:
                                str = "CART ALIGN L BWD";
                                break;
                            case Inputs.CartAlignSensorRightForward:
                                str = "CART ALIGN R FWD";
                                break;
                            case Inputs.CartAlignSensorRightBackward:
                                str = "CART ALIGN R BWD";
                                break;
                            case Inputs.ReelType7:
                                str = "REEL 7 INCH SENSOR ";
                                break;
                            case Inputs.ReelType13:
                                str = "REEL 13 INCH SENSOR ";
                                break;
                            case Inputs.BufferCartDetection1:
                                str = "BUFFER SENSOR 1";
                                break;
                            case Inputs.BufferCartDetection2:
                                str = "BUFFER SENSOR 2";
                                break;
                            case Inputs.CartAlignSensorFrontXForward:
                                str = "CART ALIGN X FWD";
                                break;
                            case Inputs.CartAlignSensorFrontXBackward:
                                str = "CART ALIGN X BWD";
                                break;
                            case Inputs.CartAlignSensorFrontYForward:
                                str = "CART ALIGN Y FWD";
                                break;
                            case Inputs.CartAlignSensorFrontYBackward:
                                str = "CART ALIGN Y BWD";
                                break;
                            case Inputs.MainAir:
                                str = "MAIN AIR SUPPLY";
                                break;                            
                            case Inputs.OutputStage1Exist:
                                str = "OUTPUT STAGE 1 EXIST";
                                break;
                            case Inputs.OutputStage2Exist:
                                str = "OUTPUT STAGE 2 EXIST";
                                break;
                            case Inputs.OutputStage3Exist:
                                str = "OUTPUT STAGE 3 EXIST";
                                break;
                            case Inputs.OutputStage4Exist:
                                str = "OUTPUT STAGE 4 EXIST";
                                break;
                            case Inputs.OutputStage5Exist:
                                str = "OUTPUT STAGE 5 EXIST";
                                break;
                            case Inputs.OutputStage6Exist:
                                str = "OUTPUT STAGE 6 EXIST";
                                break;
                            case Inputs.RejectStageFull:
                                str = "REJECT STAGE FULL";
                                break;
                        }
                    }
                    else
                        str = "NONE";

                    dataGridViewInput.Rows[j_].Cells[0].Value = id;
                    dataGridViewInput.Rows[j_].Cells[1].Value = str;
                    ChangeColor(dataGridViewInput.Rows[j_].Cells[0].Style, App.DigitalIoManager.GetInput(id));
                }

                btnInputPrev.Enabled = inputPage > 0;
                btnInputNext.Enabled = inputPage < inputPages - 1;
            }
        }

        protected virtual void UpdateOutputPage()
        {
            int id = outputPage * MaxRow;
            int max = id + MaxRow;
            string str = string.Empty;

            if (App.DigitalIoManager != null)
            {
                for (int i_ = id, j_ = 0; i_ < max; i_++, j_++, id++)
                {
                    if (Enum.IsDefined(typeof(Outputs), i_))
                    {
                        switch ((Outputs)i_)
                        {
                            case Outputs.LampRed:
                                str = "SIGNAL TOWER RED";
                                break;
                            case Outputs.LampYellow:
                                str = "SIGNAL TOWER YELLOW";
                                break;
                            case Outputs.LampGreen:
                                str = "SIGNAL TOWER GREEN";
                                break;
                            case Outputs.Buzzer:
                                str = "SIGNAL TOWER BUZZER";
                                break;
                            case Outputs.CartAlignCylinderLeft:
                                str = "CYLINDER L";
                                break;
                            case Outputs.CartAlignCylinderRight:
                                str = "CYLINDER R";
                                break;
                            case Outputs.CartAlignCylinderFrontX:
                                str = "CYLINDER X";
                                break;
                            case Outputs.CartAlignCylinderFrontY:
                                str = "CYLINDER Y";
                                break;
                        }
                    }
                    else
                        str = "NONE";

                    dataGridViewOutput.Rows[j_].Cells[0].Value = id;
                    dataGridViewOutput.Rows[j_].Cells[1].Value = str;
                    ChangeColor(dataGridViewOutput.Rows[j_].Cells[0].Style, App.DigitalIoManager.GetOutput(id));
                }

                btnOutputPrev.Enabled = outputPage > 0;
                btnOutputNext.Enabled = outputPage < outputPages - 1;
            }
        }

        protected virtual void OnChangedInputSignal(object sender, int channel)
        {
            ChangeColor(dataGridViewInput.Rows[channel % MaxRow].Cells[0].Style, App.DigitalIoManager.GetInput(channel));
        }

        protected virtual void OnChangedOutputSignal(object sender, int channel)
        {
            ChangeColor(dataGridViewOutput.Rows[channel % MaxRow].Cells[0].Style, App.DigitalIoManager.GetOutput(channel));
        }

        protected virtual void OnChangedSize(object sender, EventArgs args)
        {
            if (dataGridViewInput.RowCount == 0)
                return;

            int height = dataGridViewInput.Height / (MaxRow + 1);
            int remain = dataGridViewInput.Height % (MaxRow + 1);

            dataGridViewInput.ColumnHeadersHeight = height + remain;
            dataGridViewOutput.ColumnHeadersHeight = height + remain;

            for (int i = 0; i < MaxRow; i++)
            {
                dataGridViewInput.Rows[i].Height = height;
                dataGridViewOutput.Rows[i].Height = height;
            }
        }

        protected virtual void OnCellClickOutputDataGridView(object sender, DataGridViewCellEventArgs e)
        {   
            if (e.ColumnIndex == 0 && App.DigitalIoManager!= null)
            {
                int id = outputPage * MaxRow + dataGridViewOutput.CurrentRow.Index;
                bool state = App.DigitalIoManager.GetOutput(id);
                App.DigitalIoManager.SetOutput(id, !state);
            }
        }

        protected virtual void OnCellMouseEnterOutputDataGridView(object sender, DataGridViewCellEventArgs e)
        {
            Cursor = (e.ColumnIndex == 0) ? Cursors.Hand : Cursors.Default;
        }

        protected virtual void OnCellMouseLeaveOutputDataGridView(object sender, DataGridViewCellEventArgs e)
        {
            Cursor = Cursors.Default;
        }

        protected virtual void OnCellClickInputDataGridView(object sender, DataGridViewCellEventArgs e)
        {
            if (App.DigitalIoManager != null)
            {
                if (App.DigitalIoManager.IsSimulation && e.ColumnIndex == 0)
                {
                    int id = inputPage * MaxRow + dataGridViewInput.CurrentRow.Index;
                    bool state = App.DigitalIoManager.GetInput(id);
                    App.DigitalIoManager.SetInput(id, !state);
                }
            }
        }

        protected virtual void OnCellMouseEnterInputDataGridView(object sender, DataGridViewCellEventArgs e)
        {
            if (App.DigitalIoManager != null && !DesignMode)
            {
                if (App.DigitalIoManager.IsSimulation)
                    Cursor = (e.ColumnIndex == 0) ? Cursors.Hand : Cursors.Default;
            }
        }

        protected virtual void OnCellMouseLeaveInputDataGridView(object sender, DataGridViewCellEventArgs e)
        {
            if (App.DigitalIoManager != null && !DesignMode)
            {
                if (App.DigitalIoManager.IsSimulation)
                    Cursor = Cursors.Default;
            }
        }

        protected virtual void OnClickInputPrev(object sender, EventArgs args)
        {
            if (inputPage > 0)
            {
                inputPage--;
                UpdateInputPage();
            }
        }

        protected virtual void OnClickInputNext(object sender, EventArgs args)
        {
            if (inputPage < inputPages - 1)
            {
                inputPage++;
                UpdateInputPage();
            }
        }

        protected virtual void OnClickOutputPrev(object sender, EventArgs args)
        {
            if (outputPage > 0)
            {
                outputPage--;
                UpdateOutputPage();
            }
        }

        protected virtual void OnClickOutputNext(object sender, EventArgs args)
        {
            if (outputPage < outputPages - 1)
            {
                outputPage++;
                UpdateOutputPage();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            DetachEventHandler();
            base.Dispose(disposing);
        }

        protected virtual void AttachEventHandler()
        {
            if (App.DigitalIoManager != null)
            {
                App.DigitalIoManager.InputSignalChanged += OnChangedInputSignal;
                App.DigitalIoManager.OutputSignalChanged += OnChangedOutputSignal;
            }
        }

        protected virtual void DetachEventHandler()
        {
            if (App.DigitalIoManager != null)
            {
                App.DigitalIoManager.InputSignalChanged -= OnChangedInputSignal;
                App.DigitalIoManager.OutputSignalChanged -= OnChangedOutputSignal;
            }
        }
        #endregion

        #region Public methods
        public virtual bool Create()
        {
            bool result = true;

            try
            {
                Logger.Write($"{this.GetType().ToString()}.{MethodBase.GetCurrentMethod().Name}: called", "GUI");

                inputCount = Enum.GetNames(typeof(Inputs)).Length;
                outputCount = Enum.GetNames(typeof(Outputs)).Length;
                AttachEventHandler();

                inputPages = inputCount / MaxRow;

                if (inputCount % MaxRow > 0)
                    inputPages++;

                UpdateInputPage();

                outputPages = outputCount / MaxRow;

                if (outputCount % MaxRow > 0)
                    outputPages++;

                UpdateOutputPage();
                Logger.Write($"{this.GetType().ToString()}.{MethodBase.GetCurrentMethod().Name}: completed", "GUI");
            }
            catch (Exception ex)
            {
                result = false;
                Logger.Write($"{this.GetType().ToString()}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}", "GUI");
            }

            return result;
        }

        public void Destroy()
        {
            Logger.Write($"{this.GetType().ToString()}.{MethodBase.GetCurrentMethod().Name}(: called", "GUI");
            DetachEventHandler();
            Logger.Write($"{this.GetType().ToString()}.{MethodBase.GetCurrentMethod().Name}(: completed", "GUI");
        }

        public void RefreshInputs()
        {
            UpdateInputPage();
        }

        public void RefreshOutputs()
        {
            UpdateOutputPage();
        }
        #endregion
    }
}
#endregion