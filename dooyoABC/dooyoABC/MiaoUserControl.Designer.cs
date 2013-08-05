namespace dooyoABC
{
    partial class MiaoUserControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxUser = new System.Windows.Forms.GroupBox();
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.buttonMiao = new System.Windows.Forms.Button();
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.pictureBoxCode = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorkerGetCode = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerMiao = new System.ComponentModel.BackgroundWorker();
            this.groupBoxUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCode)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxUser
            // 
            this.groupBoxUser.Controls.Add(this.textBoxMsg);
            this.groupBoxUser.Controls.Add(this.buttonMiao);
            this.groupBoxUser.Controls.Add(this.textBoxCode);
            this.groupBoxUser.Controls.Add(this.pictureBoxCode);
            this.groupBoxUser.Controls.Add(this.label1);
            this.groupBoxUser.Location = new System.Drawing.Point(3, 3);
            this.groupBoxUser.Name = "groupBoxUser";
            this.groupBoxUser.Size = new System.Drawing.Size(142, 163);
            this.groupBoxUser.TabIndex = 0;
            this.groupBoxUser.TabStop = false;
            this.groupBoxUser.Text = "Phone";
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.BackColor = System.Drawing.Color.Yellow;
            this.textBoxMsg.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxMsg.ForeColor = System.Drawing.Color.Red;
            this.textBoxMsg.Location = new System.Drawing.Point(6, 38);
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.ReadOnly = true;
            this.textBoxMsg.Size = new System.Drawing.Size(130, 21);
            this.textBoxMsg.TabIndex = 9999;
            this.textBoxMsg.TabStop = false;
            // 
            // buttonMiao
            // 
            this.buttonMiao.Enabled = false;
            this.buttonMiao.Location = new System.Drawing.Point(6, 15);
            this.buttonMiao.Name = "buttonMiao";
            this.buttonMiao.Size = new System.Drawing.Size(130, 27);
            this.buttonMiao.TabIndex = 3;
            this.buttonMiao.Text = "提交";
            this.buttonMiao.UseVisualStyleBackColor = true;
            this.buttonMiao.Visible = false;
            this.buttonMiao.Click += new System.EventHandler(this.buttonMiao_Click);
            // 
            // textBoxCode
            // 
            this.textBoxCode.BackColor = System.Drawing.Color.LightGray;
            this.textBoxCode.Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxCode.Location = new System.Drawing.Point(6, 132);
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(130, 27);
            this.textBoxCode.TabIndex = 2;
            this.textBoxCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxCode.TextChanged += new System.EventHandler(this.textBoxCode_TextChanged);
            this.textBoxCode.Enter += new System.EventHandler(this.textBoxCode_Enter);
            // 
            // pictureBoxCode
            // 
            this.pictureBoxCode.BackColor = System.Drawing.Color.White;
            this.pictureBoxCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxCode.Location = new System.Drawing.Point(6, 65);
            this.pictureBoxCode.Name = "pictureBoxCode";
            this.pictureBoxCode.Size = new System.Drawing.Size(130, 61);
            this.pictureBoxCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCode.TabIndex = 1;
            this.pictureBoxCode.TabStop = false;
            this.pictureBoxCode.Click += new System.EventHandler(this.pictureBoxCode_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "次数：0/0";
            // 
            // backgroundWorkerGetCode
            // 
            this.backgroundWorkerGetCode.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerGetCode_DoWork);
            this.backgroundWorkerGetCode.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerGetCode_RunWorkerCompleted);
            // 
            // backgroundWorkerMiao
            // 
            this.backgroundWorkerMiao.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerMiao_DoWork);
            this.backgroundWorkerMiao.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerMiao_RunWorkerCompleted);
            // 
            // MiaoUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxUser);
            this.Name = "MiaoUserControl";
            this.Size = new System.Drawing.Size(148, 173);
            this.Load += new System.EventHandler(this.MiaoUserControl_Load);
            this.groupBoxUser.ResumeLayout(false);
            this.groupBoxUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxUser;
        private System.Windows.Forms.Button buttonMiao;
        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.PictureBox pictureBoxCode;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorkerGetCode;
        private System.ComponentModel.BackgroundWorker backgroundWorkerMiao;
        private System.Windows.Forms.TextBox textBoxMsg;
    }
}
