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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckPayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.listViewLog = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.backgroundWorkerUnpay = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerResult = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerExport = new System.ComponentModel.BackgroundWorker();
            this.listViewUser = new Utils.DoubleBufferListView();
            this.columnHeaderPhone = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderResult = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPWD = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderMsg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConfigToolStripMenuItem,
            this.StartToolStripMenuItem,
            this.StopToolStripMenuItem,
            this.RefreshToolStripMenuItem,
            this.CheckPayToolStripMenuItem,
            this.ResultToolStripMenuItem,
            this.ExportToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(777, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ConfigToolStripMenuItem
            // 
            this.ConfigToolStripMenuItem.Name = "ConfigToolStripMenuItem";
            this.ConfigToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.ConfigToolStripMenuItem.Text = "设置商品ID";
            this.ConfigToolStripMenuItem.Click += new System.EventHandler(this.ConfigToolStripMenuItem_Click);
            // 
            // StartToolStripMenuItem
            // 
            this.StartToolStripMenuItem.Name = "StartToolStripMenuItem";
            this.StartToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.StartToolStripMenuItem.Text = "开始秒杀";
            this.StartToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // StopToolStripMenuItem
            // 
            this.StopToolStripMenuItem.Name = "StopToolStripMenuItem";
            this.StopToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.StopToolStripMenuItem.Text = "停止";
            this.StopToolStripMenuItem.Click += new System.EventHandler(this.StopToolStripMenuItem_Click);
            // 
            // RefreshToolStripMenuItem
            // 
            this.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem";
            this.RefreshToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.RefreshToolStripMenuItem.Text = "刷新";
            this.RefreshToolStripMenuItem.Click += new System.EventHandler(this.RefreshToolStripMenuItem_Click);
            // 
            // CheckPayToolStripMenuItem
            // 
            this.CheckPayToolStripMenuItem.Enabled = false;
            this.CheckPayToolStripMenuItem.Name = "CheckPayToolStripMenuItem";
            this.CheckPayToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
            this.CheckPayToolStripMenuItem.Text = "查看支付状况";
            this.CheckPayToolStripMenuItem.Click += new System.EventHandler(this.CheckPayToolStripMenuItem_Click);
            // 
            // ResultToolStripMenuItem
            // 
            this.ResultToolStripMenuItem.Enabled = false;
            this.ResultToolStripMenuItem.Name = "ResultToolStripMenuItem";
            this.ResultToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.ResultToolStripMenuItem.Text = "查看结果";
            this.ResultToolStripMenuItem.Click += new System.EventHandler(this.ResultToolStripMenuItem_Click);
            // 
            // ExportToolStripMenuItem
            // 
            this.ExportToolStripMenuItem.Enabled = false;
            this.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            this.ExportToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.ExportToolStripMenuItem.Text = "导出兑换券";
            this.ExportToolStripMenuItem.Click += new System.EventHandler(this.ExportToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listViewUser);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitter1);
            this.splitContainer1.Panel2.Controls.Add(this.listViewLog);
            this.splitContainer1.Size = new System.Drawing.Size(777, 463);
            this.splitContainer1.SplitterDistance = 229;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitter1
            // 
            this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(777, 3);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // listViewLog
            // 
            this.listViewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewLog.Location = new System.Drawing.Point(0, 0);
            this.listViewLog.Name = "listViewLog";
            this.listViewLog.Size = new System.Drawing.Size(777, 229);
            this.listViewLog.TabIndex = 0;
            this.listViewLog.UseCompatibleStateImageBehavior = false;
            this.listViewLog.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "时间";
            this.columnHeader1.Width = 170;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "事件";
            this.columnHeader2.Width = 1200;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 465);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(777, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // backgroundWorkerUnpay
            // 
            this.backgroundWorkerUnpay.WorkerReportsProgress = true;
            this.backgroundWorkerUnpay.WorkerSupportsCancellation = true;
            this.backgroundWorkerUnpay.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerUnpay_DoWork);
            this.backgroundWorkerUnpay.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerUnpay_RunWorkerCompleted);
            // 
            // backgroundWorkerResult
            // 
            this.backgroundWorkerResult.WorkerReportsProgress = true;
            this.backgroundWorkerResult.WorkerSupportsCancellation = true;
            this.backgroundWorkerResult.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerResult_DoWork);
            this.backgroundWorkerResult.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerResult_RunWorkerCompleted);
            // 
            // backgroundWorkerExport
            // 
            this.backgroundWorkerExport.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerExport_DoWork);
            this.backgroundWorkerExport.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerExport_RunWorkerCompleted);
            // 
            // listViewUser
            // 
            this.listViewUser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderPhone,
            this.columnHeaderStatus,
            this.columnHeaderPay,
            this.columnHeaderResult,
            this.columnHeaderPWD,
            this.columnHeaderMsg});
            this.listViewUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewUser.FullRowSelect = true;
            this.listViewUser.GridLines = true;
            this.listViewUser.Location = new System.Drawing.Point(0, 0);
            this.listViewUser.MultiSelect = false;
            this.listViewUser.Name = "listViewUser";
            this.listViewUser.Size = new System.Drawing.Size(777, 229);
            this.listViewUser.TabIndex = 1;
            this.listViewUser.UseCompatibleStateImageBehavior = false;
            this.listViewUser.View = System.Windows.Forms.View.Details;
            this.listViewUser.DoubleClick += new System.EventHandler(this.listViewUser_DoubleClick);
            // 
            // columnHeaderPhone
            // 
            this.columnHeaderPhone.Text = "手机号";
            this.columnHeaderPhone.Width = 120;
            // 
            // columnHeaderStatus
            // 
            this.columnHeaderStatus.Text = "状态";
            this.columnHeaderStatus.Width = 80;
            // 
            // columnHeaderPay
            // 
            this.columnHeaderPay.Text = "未支付数";
            this.columnHeaderPay.Width = 70;
            // 
            // columnHeaderResult
            // 
            this.columnHeaderResult.Text = "结果";
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 487);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "秒杀器";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorkerLoadUsers;
        private DoubleBufferListView listViewUser;
        private System.Windows.Forms.ColumnHeader columnHeaderPhone;
        private System.Windows.Forms.ColumnHeader columnHeaderStatus;
        private System.Windows.Forms.ColumnHeader columnHeaderPWD;
        private System.Windows.Forms.ColumnHeader columnHeaderMsg;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem StartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RefreshToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ListView listViewLog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripMenuItem ConfigToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeaderPay;
        private System.ComponentModel.BackgroundWorker backgroundWorkerUnpay;
        private System.Windows.Forms.ToolStripMenuItem CheckPayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ResultToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorkerResult;
        private System.Windows.Forms.ColumnHeader columnHeaderResult;
        private System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorkerExport;
    }
}

