using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dooyoABC
{
    public partial class ManualMiaoForm : Form
    {
        public delegate void MsgEventDelegate(string msg);
        public event MsgEventDelegate MsgEvent;
        /// <summary>
        /// 信息事件
        /// </summary>
        protected void OnMsg(string msg)
        {
            if (MsgEvent != null)
            {
                MsgEvent(msg);
            }
        }
        UserManager mUserManager;
        private String mProductID;
        public ManualMiaoForm(UserManager um, String pid)
        {
            InitializeComponent();
            mUserManager = um;
            mProductID = pid;
        }


        private void ManualMiaoForm_Load(object sender, EventArgs e)
        {
            List<String> phones = mUserManager.getKeys();
            for (int i = 0; i < phones.Count; i++)
            {
                UserManager.User u = mUserManager.mMapUser[phones[i]];
                MiaoUserControl muc = new MiaoUserControl(u,mProductID,i);
                muc.MsgEvent += new MiaoUserControl.MsgEventDelegate(muc_MsgEvent);
                this.flowLayoutPanel1.Controls.Add(muc);
            }
        }

        void muc_MsgEvent(string msg)
        {
            OnMsg(msg);
        }
    }
}
