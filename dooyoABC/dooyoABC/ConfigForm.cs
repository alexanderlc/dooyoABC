using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dooyoABC
{
    public partial class ConfigForm : Form
    {
        private String mPID;

        public String PID
        {
            get { return mPID; }
            set { mPID = value; }
        }

        public ConfigForm()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            PID = this.textBoxPID.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            this.textBoxPID.Text = PID;
        }

    }
}
