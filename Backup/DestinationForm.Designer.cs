namespace Backup
{
    partial class DestinationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DestinationForm));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.AddSecret = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.AWSAccessKey = new System.Windows.Forms.TextBox();
            this.S3BucketName = new System.Windows.Forms.TextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.ArchivePath = new System.Windows.Forms.TextBox();
            this.IsS3Bucket = new System.Windows.Forms.CheckBox();
            this.IsFileSystem = new System.Windows.Forms.CheckBox();
            this.CloseForm = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.Cancel = new System.Windows.Forms.Button();
            this.MoreOptions = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.AddSecret);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.AWSAccessKey);
            this.groupBox2.Controls.Add(this.S3BucketName);
            this.groupBox2.Controls.Add(this.Browse);
            this.groupBox2.Controls.Add(this.ArchivePath);
            this.groupBox2.Controls.Add(this.IsS3Bucket);
            this.groupBox2.Controls.Add(this.IsFileSystem);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(600, 196);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Destination";
            // 
            // AddSecret
            // 
            this.AddSecret.Enabled = false;
            this.AddSecret.Location = new System.Drawing.Point(459, 133);
            this.AddSecret.Name = "AddSecret";
            this.AddSecret.Size = new System.Drawing.Size(125, 23);
            this.AddSecret.TabIndex = 15;
            this.AddSecret.Text = "Add Secret...";
            this.AddSecret.UseVisualStyleBackColor = true;
            this.AddSecret.Click += new System.EventHandler(this.AddSecret_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Access Key ID:";
            // 
            // AWSAccessKey
            // 
            this.AWSAccessKey.Enabled = false;
            this.AWSAccessKey.Location = new System.Drawing.Point(150, 135);
            this.AWSAccessKey.Name = "AWSAccessKey";
            this.AWSAccessKey.Size = new System.Drawing.Size(265, 20);
            this.AWSAccessKey.TabIndex = 12;
            // 
            // S3BucketName
            // 
            this.S3BucketName.Enabled = false;
            this.S3BucketName.Location = new System.Drawing.Point(150, 104);
            this.S3BucketName.Name = "S3BucketName";
            this.S3BucketName.Size = new System.Drawing.Size(434, 20);
            this.S3BucketName.TabIndex = 10;
            // 
            // Browse
            // 
            this.Browse.Enabled = false;
            this.Browse.Location = new System.Drawing.Point(459, 50);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(125, 23);
            this.Browse.TabIndex = 9;
            this.Browse.Text = "Browse Directory...";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // ArchivePath
            // 
            this.ArchivePath.Enabled = false;
            this.ArchivePath.Location = new System.Drawing.Point(150, 24);
            this.ArchivePath.Name = "ArchivePath";
            this.ArchivePath.Size = new System.Drawing.Size(434, 20);
            this.ArchivePath.TabIndex = 9;
            // 
            // IsS3Bucket
            // 
            this.IsS3Bucket.AutoSize = true;
            this.IsS3Bucket.Location = new System.Drawing.Point(16, 107);
            this.IsS3Bucket.Name = "IsS3Bucket";
            this.IsS3Bucket.Size = new System.Drawing.Size(120, 17);
            this.IsS3Bucket.TabIndex = 8;
            this.IsS3Bucket.Text = "Amazon S3 Bucket:";
            this.IsS3Bucket.UseVisualStyleBackColor = true;
            this.IsS3Bucket.CheckedChanged += new System.EventHandler(this.IsS3Bucket_CheckedChanged);
            // 
            // IsFileSystem
            // 
            this.IsFileSystem.AutoSize = true;
            this.IsFileSystem.Location = new System.Drawing.Point(16, 26);
            this.IsFileSystem.Name = "IsFileSystem";
            this.IsFileSystem.Size = new System.Drawing.Size(82, 17);
            this.IsFileSystem.TabIndex = 7;
            this.IsFileSystem.Text = "File System:";
            this.IsFileSystem.UseVisualStyleBackColor = true;
            this.IsFileSystem.CheckedChanged += new System.EventHandler(this.IsFileSystem_CheckedChanged);
            // 
            // CloseForm
            // 
            this.CloseForm.Location = new System.Drawing.Point(435, 226);
            this.CloseForm.Name = "CloseForm";
            this.CloseForm.Size = new System.Drawing.Size(75, 23);
            this.CloseForm.TabIndex = 10;
            this.CloseForm.Text = "Commit";
            this.CloseForm.UseVisualStyleBackColor = true;
            this.CloseForm.Click += new System.EventHandler(this.Save_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(521, 226);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 11;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // MoreOptions
            // 
            this.MoreOptions.Location = new System.Drawing.Point(28, 226);
            this.MoreOptions.Name = "MoreOptions";
            this.MoreOptions.Size = new System.Drawing.Size(120, 23);
            this.MoreOptions.TabIndex = 12;
            this.MoreOptions.Text = "More Options...";
            this.MoreOptions.UseVisualStyleBackColor = true;
            this.MoreOptions.Click += new System.EventHandler(this.MoreOptions_Click);
            // 
            // DestinationForm
            // 
            this.AcceptButton = this.CloseForm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(621, 261);
            this.Controls.Add(this.MoreOptions);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.CloseForm);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DestinationForm";
            this.Text = "Destination of Backup";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button AddSecret;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AWSAccessKey;
        private System.Windows.Forms.TextBox S3BucketName;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.TextBox ArchivePath;
        private System.Windows.Forms.CheckBox IsS3Bucket;
        private System.Windows.Forms.CheckBox IsFileSystem;
        private System.Windows.Forms.Button CloseForm;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button MoreOptions;
    }
}