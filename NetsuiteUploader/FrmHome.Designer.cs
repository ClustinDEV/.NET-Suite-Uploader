namespace NetsuiteUploader
{
    partial class frmUploader
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUploader));
            this.tmrTimeout = new System.Windows.Forms.Timer(this.components);
            this.lstTasks = new System.Windows.Forms.ListBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.chkWatchChanges = new System.Windows.Forms.CheckBox();
            this.statusStripUploader = new System.Windows.Forms.StatusStrip();
            this.lblToolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlsUploader = new System.Windows.Forms.ToolStrip();
            this.ddbAccount = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ddbTasks = new System.Windows.Forms.ToolStripDropDownButton();
            this.mniTasksOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mniTasksOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.picDone = new System.Windows.Forms.PictureBox();
            this.picError = new System.Windows.Forms.PictureBox();
            this.statusStripUploader.SuspendLayout();
            this.tlsUploader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picError)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrTimeout
            // 
            this.tmrTimeout.Enabled = true;
            this.tmrTimeout.Interval = 600000;
            this.tmrTimeout.Tick += new System.EventHandler(this.tmrTimeout_Tick);
            // 
            // lstTasks
            // 
            this.lstTasks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstTasks.FormattingEnabled = true;
            this.lstTasks.Location = new System.Drawing.Point(16, 35);
            this.lstTasks.Name = "lstTasks";
            this.lstTasks.Size = new System.Drawing.Size(680, 132);
            this.lstTasks.TabIndex = 1;
            // 
            // btnUpload
            // 
            this.btnUpload.BackColor = System.Drawing.SystemColors.Control;
            this.btnUpload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpload.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUpload.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.Location = new System.Drawing.Point(16, 175);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(255, 55);
            this.btnUpload.TabIndex = 2;
            this.btnUpload.Text = "UPLOAD";
            this.btnUpload.UseVisualStyleBackColor = false;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(16, 236);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(680, 141);
            this.txtLog.TabIndex = 3;
            // 
            // chkWatchChanges
            // 
            this.chkWatchChanges.AutoSize = true;
            this.chkWatchChanges.Location = new System.Drawing.Point(547, 176);
            this.chkWatchChanges.Name = "chkWatchChanges";
            this.chkWatchChanges.Size = new System.Drawing.Size(149, 17);
            this.chkWatchChanges.TabIndex = 4;
            this.chkWatchChanges.Text = "Start watch file changes";
            this.chkWatchChanges.UseVisualStyleBackColor = true;
            this.chkWatchChanges.CheckedChanged += new System.EventHandler(this.chkWatchChanges_CheckedChanged);
            // 
            // statusStripUploader
            // 
            this.statusStripUploader.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblToolStripStatus});
            this.statusStripUploader.Location = new System.Drawing.Point(0, 392);
            this.statusStripUploader.Name = "statusStripUploader";
            this.statusStripUploader.Size = new System.Drawing.Size(712, 22);
            this.statusStripUploader.SizingGrip = false;
            this.statusStripUploader.TabIndex = 5;
            // 
            // lblToolStripStatus
            // 
            this.lblToolStripStatus.Name = "lblToolStripStatus";
            this.lblToolStripStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // tlsUploader
            // 
            this.tlsUploader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlsUploader.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ddbAccount,
            this.toolStripSeparator1,
            this.ddbTasks});
            this.tlsUploader.Location = new System.Drawing.Point(0, 0);
            this.tlsUploader.Name = "tlsUploader";
            this.tlsUploader.Padding = new System.Windows.Forms.Padding(0);
            this.tlsUploader.ShowItemToolTips = false;
            this.tlsUploader.Size = new System.Drawing.Size(712, 25);
            this.tlsUploader.Stretch = true;
            this.tlsUploader.TabIndex = 6;
            // 
            // ddbAccount
            // 
            this.ddbAccount.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ddbAccount.Image = ((System.Drawing.Image)(resources.GetObject("ddbAccount.Image")));
            this.ddbAccount.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddbAccount.Name = "ddbAccount";
            this.ddbAccount.Size = new System.Drawing.Size(109, 22);
            this.ddbAccount.Text = "Change Account";
            this.ddbAccount.ToolTipText = "Change Account";
            this.ddbAccount.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ddbAccount_DropDownItemClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ddbTasks
            // 
            this.ddbTasks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ddbTasks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniTasksOpenFolder,
            this.mniTasksOpenFile});
            this.ddbTasks.Image = ((System.Drawing.Image)(resources.GetObject("ddbTasks.Image")));
            this.ddbTasks.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddbTasks.Name = "ddbTasks";
            this.ddbTasks.Size = new System.Drawing.Size(49, 22);
            this.ddbTasks.Text = "Tasks";
            // 
            // mniTasksOpenFolder
            // 
            this.mniTasksOpenFolder.Name = "mniTasksOpenFolder";
            this.mniTasksOpenFolder.Size = new System.Drawing.Size(166, 22);
            this.mniTasksOpenFolder.Text = "Open Task Folder";
            this.mniTasksOpenFolder.Click += new System.EventHandler(this.mniTasksOpenFolder_Click);
            // 
            // mniTasksOpenFile
            // 
            this.mniTasksOpenFile.Name = "mniTasksOpenFile";
            this.mniTasksOpenFile.Size = new System.Drawing.Size(166, 22);
            this.mniTasksOpenFile.Text = "Open Task File";
            this.mniTasksOpenFile.Click += new System.EventHandler(this.mniTasksOpenFile_Click);
            // 
            // picDone
            // 
            this.picDone.Location = new System.Drawing.Point(274, 175);
            this.picDone.Margin = new System.Windows.Forms.Padding(0);
            this.picDone.Name = "picDone";
            this.picDone.Size = new System.Drawing.Size(54, 54);
            this.picDone.TabIndex = 8;
            this.picDone.TabStop = false;
            this.picDone.Visible = false;
            // 
            // picError
            // 
            this.picError.Location = new System.Drawing.Point(274, 176);
            this.picError.Margin = new System.Windows.Forms.Padding(0);
            this.picError.Name = "picError";
            this.picError.Size = new System.Drawing.Size(54, 54);
            this.picError.TabIndex = 9;
            this.picError.TabStop = false;
            this.picError.Visible = false;
            // 
            // frmUploader
            // 
            this.AcceptButton = this.btnUpload;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(712, 414);
            this.Controls.Add(this.picError);
            this.Controls.Add(this.picDone);
            this.Controls.Add(this.tlsUploader);
            this.Controls.Add(this.statusStripUploader);
            this.Controls.Add(this.chkWatchChanges);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.lstTasks);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmUploader";
            this.Text = "Netsuite Uploader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUploader_Closing);
            this.Load += new System.EventHandler(this.frmUploader_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmUploader_KeyDown);
            this.statusStripUploader.ResumeLayout(false);
            this.statusStripUploader.PerformLayout();
            this.tlsUploader.ResumeLayout(false);
            this.tlsUploader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picError)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrTimeout;
        private System.Windows.Forms.ListBox lstTasks;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.CheckBox chkWatchChanges;
        private System.Windows.Forms.StatusStrip statusStripUploader;
        private System.Windows.Forms.ToolStripStatusLabel lblToolStripStatus;
        private System.Windows.Forms.ToolStrip tlsUploader;
        private System.Windows.Forms.ToolStripDropDownButton ddbAccount;
        private System.Windows.Forms.ToolStripDropDownButton ddbTasks;
        private System.Windows.Forms.ToolStripMenuItem mniTasksOpenFolder;
        private System.Windows.Forms.ToolStripMenuItem mniTasksOpenFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.PictureBox picDone;
        private System.Windows.Forms.PictureBox picError;
    }
}

