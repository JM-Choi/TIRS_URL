using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TechFloor.Device;

namespace TechFloor.Forms
{
    public partial class FormLight : Form
    {
        #region Fields
        private int visionLightChannel1 = 0;
        #endregion

        public FormLight()
        {
            InitializeComponent();
            this.Location = (App.MainForm as FormMain).Location;
        }
        
        private void OnValueChangedNumericUpDownVisionLightChannel1(object sender, EventArgs e)
        {
            trackBarVisionLightChannel1.Value = Convert.ToInt32(numericUpDownVisionLightChannel1.Value);
            visionLightChannel1 = trackBarVisionLightChannel1.Value;
            (App.MainForm as FormMain).VisionLightManager.SendCommand(VisionLightManager.ControllerCommands.Set, 1, visionLightChannel1);// string.Format("B1{0:000}#", visionLightChannel1));
        }

        private void OnValueChangedTrackBarVisionLightChannel1(object sender, EventArgs e)
        {
            if (numericUpDownVisionLightChannel1.Value != trackBarVisionLightChannel1.Value)
            {
                numericUpDownVisionLightChannel1.Value = trackBarVisionLightChannel1.Value;
            }
        }

        private void OnClickButtonVisionLightOnChannel1(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                (App.MainForm as FormMain).VisionLightManager.SendCommand(VisionLightManager.ControllerCommands.On, 1, 0);// "N1000#");
            }
        }

        private void OnClickVisionLightOffChannel1(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                (App.MainForm as FormMain).VisionLightManager.SendCommand(VisionLightManager.ControllerCommands.Off, 1, 0); // "F1000#");
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            comboBoxVisionLightChannel1.Items.Clear();

            if (Model.VisionLights != null)
            {
                foreach (VisionLightObject item in Model.VisionLights)
                {
                    comboBoxVisionLightChannel1.Items.Add($"{item.Id:000}: {item.Name}");
                }
            }
        }

        private void OnClickSave(object sender, EventArgs e)
        {
            try
            {
                if (Model.VisionLights != null)
                {
                    int index = Convert.ToInt32(comboBoxVisionLightChannel1.Text.Substring(0, 3));
                    if (Model.Process.ModifyLightValue(index, 1, trackBarVisionLightChannel1.Value))
                    {
                        Model.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }
        
        private void OnSelectedIndexChangedLight(object sender, EventArgs e)
        {
            try
            {
                if (Model.VisionLights != null)
                {
                    int index = Convert.ToInt32(comboBoxVisionLightChannel1.Text.Substring(0, 3));
                    visionLightChannel1 = Model.Process.GetLightValue(index, 1);
                    trackBarVisionLightChannel1.Value = visionLightChannel1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        private void OnFormShown(object sender, EventArgs e)
        {
            (App.MainForm as FormMain).SetDisplayMonitor(this);
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            (App.MainForm as FormMain).SetFocus();
        }
    }
}
