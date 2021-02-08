#region Imports
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
#endregion

#region Program
namespace TechFloor.Gui
{
    public partial class ControlStatusLabel : UserControl
    {
        #region Properties
        [Browsable(true)]
        [Category("Components")]
        public TableLayoutPanel Frame => tableLayoutPanelFrame;
        [Browsable(true)]
        [Category("Components")]
        public Label Status => labelStatus;
        [Browsable(true)]
        [Category("Components")]
        public Label Value => labelValue;
        public Color StatusBackColor => labelStatus.BackColor;
        public Color StatusForeColor => labelStatus.ForeColor;
        public Color ValueBackColor => labelValue.BackColor;
        public Color ValueForeColor => labelValue.ForeColor;
        [Browsable(true)]
        [Category("Display Data")]
        public string StatusText
        {
            get => labelStatus.Text;
            set => labelStatus.Text = value;
        }
        [Browsable(true)]
        [Category("Display Data")]
        public string ValueText
        {
            get => labelValue.Text;
            set => labelValue.Text = value;
        }
        public override Font Font
        {
            get => base.Font;
            set
            {
                labelStatus.Font = base.Font = value;
            }
        }
        public Font StatusFont
        {
            get => labelStatus.Font;
            set => labelStatus.Font = value;
        }
        public Font ValueFont
        {
            get => labelValue.Font;
            set => labelValue.Font = value;
        }
        #endregion

        #region Fields
        [Browsable(true)]
        [Category("Components Events")]
        public event EventHandler ChangedStatus;
        #endregion

        #region Constructors
        public ControlStatusLabel()
        {
            InitializeComponent();
        }
        #endregion

        #region Public methods
        public virtual void SetStatusColor(Color statusbackcolor, Color statusforecolor)
        {
            labelStatus.BackColor = statusbackcolor;
            labelStatus.ForeColor = statusforecolor;
        }

        public virtual void SetValueColor(Color valuebackcolor, Color valueforecolor)
        {
            labelValue.BackColor = valuebackcolor;
            labelValue.ForeColor = valueforecolor;
        }

        public virtual void SetStatusValue(string text, Color statusbackcolor, Color statusforecolor, Color valuebackcolor, Color valueforecolor)
        {
            labelStatus.BackColor = statusbackcolor;
            labelStatus.ForeColor = statusforecolor;
            labelValue.BackColor = valuebackcolor;
            labelValue.ForeColor = valueforecolor;
            labelValue.Text = text;
            ChangedStatus?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
#endregion