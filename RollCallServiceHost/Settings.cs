using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RollCallServiceHost
{
    public partial class Settings : Form
    {
        public string Port
        {
            get { return this.textBoxPort.Text; }
            set { this.textBoxPort.Text = value; }
        }

        public Settings()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.textBoxPort.Text.Trim() != "")
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("必须设置端口号！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
