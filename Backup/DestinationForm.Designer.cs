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
            this.label1 = new System.Windows.Forms.Label();
            this.AWSAccessKey = new System.Windows.Forms.TextBox();
            this.AWSProfileName = new System.Windows.Forms.TextBox();
            this.S3BucketName = new System.Windows.Forms.TextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.ArchivePath = new System.Windows.Forms.TextBox();
            this.IsS3Bucket = new System.Windows.Forms.CheckBox();
            this.IsFileSystem = new System.Windows.Forms.CheckBox();
            this.CloseForm = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.AddSecret);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.AWSAccessKey);
            this.groupBox2.Controls.Add(this.AWSProfileName);
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
            this.groupBox2.Text = "Backup Destination";
            // 
            // AddSecret
            // 
            this.AddSecret.Enabled = false;
            this.AddSecret.Location = new System.Drawing.Point(459, 160);
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
            this.label2.Location = new System.Drawing.Point(350, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Access Key:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "AWS Profile Name:";
            // 
            // AWSAccessKey
            // 
            this.AWSAccessKey.Enabled = false;
            this.AWSAccessKey.Location = new System.Drawing.Point(423, 134);
            this.AWSAccessKey.Name = "AWSAccessKey";
            this.AWSAccessKey.Size = new System.Drawing.Size(161, 20);
            this.AWSAccessKey.TabIndex = 12;
            // 
            // AWSProfileName
            // 
            this.AWSProfileName.Enabled = false;
            this.AWSProfileName.Location = new System.Drawing.Point(150, 134);
            this.AWSProfileName.Name = "AWSProfileName";
            this.AWSProfileName.Size = new System.Drawing.Size(161, 20);
            this.AWSProfileName.TabIndex = 11;
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
            this.IsS3Bucket.Size = new System.Drawing.Size(117, 17);
            this.IsS3Bucket.TabIndex = 8;
            this.IsS3Bucket.Text = "Amazon S3 Bucket";
            this.IsS3Bucket.UseVisualStyleBackColor = true;
            this.IsS3Bucket.CheckedChanged += new System.EventHandler(this.IsS3Bucket_CheckedChanged);
            // 
            // IsFileSystem
            // 
            this.IsFileSystem.AutoSize = true;
            this.IsFileSystem.Location = new System.Drawing.Point(16, 26);
            this.IsFileSystem.Name = "IsFileSystem";
            this.IsFileSystem.Size = new System.Drawing.Size(79, 17);
            this.IsFileSystem.TabIndex = 7;
            this.IsFileSystem.Text = "File System";
            this.IsFileSystem.UseVisualStyleBackColor = true;
            this.IsFileSystem.CheckedChanged += new System.EventHandler(this.IsFileSystem_CheckedChanged);
            // 
            // CloseForm
            // 
            this.CloseForm.Location = new System.Drawing.Point(521, 222);
            this.CloseForm.Name = "CloseForm";
            this.CloseForm.Size = new System.Drawing.Size(75, 23);
            this.CloseForm.TabIndex = 10;
            this.CloseForm.Text = "Close";
            this.CloseForm.UseVisualStyleBackColor = true;
            this.CloseForm.Click += new System.EventHandler(this.Save_Click);
            // 
            // DestinationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 261);
            this.Controls.Add(this.CloseForm);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DestinationForm";
            this.Text = "Backup Destination";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button AddSecret;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox AWSAccessKey;
        private System.Windows.Forms.TextBox AWSProfileName;
        private System.Windows.Forms.TextBox S3BucketName;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.TextBox ArchivePath;
        private System.Windows.Forms.CheckBox IsS3Bucket;
        private System.Windows.Forms.CheckBox IsFileSystem;
        private System.Windows.Forms.Button CloseForm;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}