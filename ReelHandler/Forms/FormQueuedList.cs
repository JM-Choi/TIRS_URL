#region Imports
using TechFloor.Components;
using TechFloor.Object;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
#endregion

#region Program
#pragma warning disable CS0414
namespace TechFloor.Forms
{
    public partial class FormQueuedList : Form
    {
        #region Fields
        private int visionLightChannel1 = 0;
        #endregion

        public FormQueuedList()
        {
            InitializeComponent();
            this.Location = (App.MainForm as FormMain).Location;
            this.DialogResult = DialogResult.Cancel;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            UpdatePickingList();
        }

        private void UpdatePickingList()
        {
            try
            {
                listViewQueuedPickingList.Items.Clear();

                if (Singleton<MaterialPackageManager>.Instance.Materials.Count > 0)
                {
                    foreach (MaterialPackage pkg in Singleton<MaterialPackageManager>.Instance.Materials)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = pkg.Name;
                        lvi.SubItems.Add(pkg.Materials.Count.ToString());
                        lvi.SubItems.Add(pkg.RegisteredTime.ToString());
                        listViewQueuedPickingList.Items.Add(lvi);
                    }
                }
                else
                {
                    FormMessageExt.ShowInformation(Properties.Resources.String_FormQuedList_Notification_No_Picking_List);
                    Close();
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

        private void OnClickRemoveAll(object sender, EventArgs e)
        {
            if (Singleton<MaterialPackageManager>.Instance.RemoveAllMaterialPackages())
            {
                UpdatePickingList();
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
#endregion