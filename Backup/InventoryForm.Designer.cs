namespace Backup
{
    partial class InventoryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryForm));
            this.CloseForm = new System.Windows.Forms.Button();
            this.Message = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FilesList = new System.Windows.Forms.ListView();
            this.file = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.archiveId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Download = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CloseForm
            // 
            this.CloseForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseForm.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseForm.Location = new System.Drawing.Point(546, 325);
            this.CloseForm.Name = "CloseForm";
            this.CloseForm.Size = new System.Drawing.Size(75, 23);
            this.CloseForm.TabIndex = 10;
            this.CloseForm.Text = "Close";
            this.CloseForm.UseVisualStyleBackColor = true;
            this.CloseForm.Click += new System.EventHandler(this.CloseForm_Click);
            // 
            // Message
            // 
            this.Message.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Message.AutoSize = true;
            this.Message.Location = new System.Drawing.Point(15, 330);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(56, 13);
            this.Message.TabIndex = 11;
            this.Message.Text = "[Message]";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.FilesList);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(616, 307);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Contents";
            // 
            // FilesList
            // 
            this.FilesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.file,
            this.size,
            this.archiveId,
            this.status});
            this.FilesList.Location = new System.Drawing.Point(6, 19);
            this.FilesList.MultiSelect = false;
            this.FilesList.Name = "FilesList";
            this.FilesList.Size = new System.Drawing.Size(604, 282);
            this.FilesList.TabIndex = 0;
            this.FilesList.UseCompatibleStateImageBehavior = false;
            this.FilesList.View = System.Windows.Forms.View.Details;
            this.FilesList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.FilesList_ItemSelectionChanged);
            // 
            // file
            // 
            this.file.Text = "Archive Description";
            this.file.Width = 212;
            // 
            // size
            // 
            this.size.Text = "Size (Bytes)";
            this.size.Width = 112;
            // 
            // archiveId
            // 
            this.archiveId.Text = "Archive ID";
            this.archiveId.Width = 152;
            // 
            // status
            // 
            this.status.Text = "Status";
            this.status.Width = 121;
            // 
            // Download
            // 
            this.Download.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Download.Enabled = false;
            this.Download.Location = new System.Drawing.Point(442, 325);
            this.Download.Name = "Download";
            this.Download.Size = new System.Drawing.Size(94, 23);
            this.Download.TabIndex = 12;
            this.Download.Text = "Download";
            this.Download.UseVisualStyleBackColor = true;
            this.Download.Click += new System.EventHandler(this.Download_Click);
            // 
            // InventoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseForm;
            this.ClientSize = new System.Drawing.Size(640, 360);
            this.Controls.Add(this.Download);
            this.Controls.Add(this.CloseForm);
            this.Controls.Add(this.Message);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "InventoryForm";
            this.Text = "Glacier Inventory";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CloseForm;
        private System.Windows.Forms.Label Message;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView FilesList;
        private System.Windows.Forms.ColumnHeader file;
        private System.Windows.Forms.Button Download;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ColumnHeader size;
        private System.Windows.Forms.ColumnHeader archiveId;
        private System.Windows.Forms.ColumnHeader status;
    }
}