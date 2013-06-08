using Utils;
namespace dooyoABC
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.backgroundWorkerLoadUsers = new System.ComponentModel.BackgroundWorker();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.StartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewUser = new Utils.DoubleBufferListView();
            this.columnHeaderPhone = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPWD = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderMsg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorkerLoadUsers
            // 
            this.backgroundWorkerLoadUsers.WorkerReportsProgress = true;
            this.backgroundWorkerLoadUsers.WorkerSupportsCancellation = true;
            this.backgroundWorkerLoadUsers.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerLoadUsers_DoWork);
            this.backgroundWorkerLoadUsers.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerLoadUsers_ProgressChanged);
            this.backgroundWorkerLoadUsers.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerLoadUsers_RunWorkerCompleted);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(22, 57);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(219, 117);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StartToolStripMenuItem,
            this.StopToolStripMenuItem,
            this.RefreshToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(777, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // StartToolStripMenuItem
            // 
            this.StartToolStripMenuItem.Name = "StartToolStripMenuItem";
            this.StartToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.StartToolStripMenuItem.Text = "开始秒杀";
            this.StartToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // StopToolStripMenuItem
            // 
            this.StopToolStripMenuItem.Name = "StopToolStripMenuItem";
            this.StopToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.StopToolStripMenuItem.Text = "停止";
            this.StopToolStripMenuItem.Click += new System.EventHandler(this.StopToolStripMenuItem_Click);
            // 
            // listViewUser
            // 
            this.listViewUser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderPhone,
            this.columnHeaderStatus,
            this.columnHeaderPWD,
            this.columnHeaderMsg});
            this.listViewUser.FullRowSelect = true;
            this.listViewUser.GridLines = true;
            this.listViewUser.Location = new System.Drawing.Point(0, 123);
            this.listViewUser.MultiSelect = false;
            this.listViewUser.Name = "listViewUser";
            this.listViewUser.Size = new System.Drawing.Size(753, 182);
            this.listViewUser.TabIndex = 1;
            this.listViewUser.UseCompatibleStateImageBehavior = false;
            this.listViewUser.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderPhone
            // 
            this.columnHeaderPhone.Text = "手机号";
            this.columnHeaderPhone.Width = 150;
            // 
            // columnHeaderStatus
            // 
            this.columnHeaderStatus.Text = "状态";
            this.columnHeaderStatus.Width = 80;
            // 
            // columnHeaderPWD
            // 
            this.columnHeaderPWD.Text = "加密密码";
            this.columnHeaderPWD.Width = 200;
            // 
            // columnHeaderMsg
            // 
            this.columnHeaderMsg.Text = "消息";
            this.columnHeaderMsg.Width = 300;
            // 
            // RefreshToolStripMenuItem
            // 
            this.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem";
            this.RefreshToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.RefreshToolStripMenuItem.Text = "刷新";
            this.RefreshToolStripMenuItem.Click += new System.EventHandler(this.RefreshToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 487);
            this.Controls.Add(this.listViewUser);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorkerLoadUsers;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private DoubleBufferListView listViewUser;
        private System.Windows.Forms.ColumnHeader columnHeaderPhone;
        private System.Windows.Forms.ColumnHeader columnHeaderStatus;
        private System.Windows.Forms.ColumnHeader columnHeaderPWD;
        private System.Windows.Forms.ColumnHeader columnHeaderMsg;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem StartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RefreshToolStripMenuItem;
    }
}

