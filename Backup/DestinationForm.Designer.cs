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
            this.CreateOnStart = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.Browse);
            this.groupBox2.Controls.Add(this.ArchivePath);
            this.groupBox2.Controls.Add(this.IsFileSystem);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(600, 70);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File System";
            // 
            // AddSecret
            // 
            this.AddSecret.Enabled = false;
            this.AddSecret.Location = new System.Drawing.Point(458, 43);
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
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Access Key ID:";
            // 
            // AWSAccessKey
            // 
            this.AWSAccessKey.Enabled = false;
            this.AWSAccessKey.Location = new System.Drawing.Point(98, 45);
            this.AWSAccessKey.Name = "AWSAccessKey";
            this.AWSAccessKey.Size = new System.Drawing.Size(351, 20);
            this.AWSAccessKey.TabIndex = 12;
            // 
            // S3BucketName
            // 
            this.S3BucketName.Enabled = false;
            this.S3BucketName.Location = new System.Drawing.Point(98, 19);
            this.S3BucketName.Name = "S3BucketName";
            this.S3BucketName.Size = new System.Drawing.Size(351, 20);
            this.S3BucketName.TabIndex = 10;
            // 
            // Browse
            // 
            this.Browse.Enabled = false;
            this.Browse.Location = new System.Drawing.Point(459, 17);
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
            this.ArchivePath.Location = new System.Drawing.Point(99, 19);
            this.ArchivePath.Name = "ArchivePath";
            this.ArchivePath.Size = new System.Drawing.Size(351, 20);
            this.ArchivePath.TabIndex = 9;
            // 
            // IsS3Bucket
            // 
            this.IsS3Bucket.AutoSize = true;
            this.IsS3Bucket.Location = new System.Drawing.Point(98, 71);
            this.IsS3Bucket.Name = "IsS3Bucket";
            this.IsS3Bucket.Size = new System.Drawing.Size(15, 14);
            this.IsS3Bucket.TabIndex = 8;
            this.IsS3Bucket.UseVisualStyleBackColor = true;
            this.IsS3Bucket.CheckedChanged += new System.EventHandler(this.IsS3Bucket_CheckedChanged);
            // 
            // IsFileSystem
            // 
            this.IsFileSystem.AutoSize = true;
            this.IsFileSystem.Location = new System.Drawing.Point(99, 44);
            this.IsFileSystem.Name = "IsFileSystem";
            this.IsFileSystem.Size = new System.Drawing.Size(15, 14);
            this.IsFileSystem.TabIndex = 7;
            this.IsFileSystem.UseVisualStyleBackColor = true;
            this.IsFileSystem.CheckedChanged += new System.EventHandler(this.IsFileSystem_CheckedChanged);
            // 
            // CloseForm
            // 
            this.CloseForm.Location = new System.Drawing.Point(435, 202);
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
            this.Cancel.Location = new System.Drawing.Point(521, 202);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 11;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // CreateOnStart
            // 
            this.CreateOnStart.AutoSize = true;
            this.CreateOnStart.Location = new System.Drawing.Point(12, 203);
            this.CreateOnStart.Name = "CreateOnStart";
            this.CreateOnStart.Size = new System.Drawing.Size(221, 17);
            this.CreateOnStart.TabIndex = 11;
            this.CreateOnStart.Text = "Create a backup when the program starts";
            this.CreateOnStart.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.S3BucketName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.AddSecret);
            this.groupBox1.Controls.Add(this.IsS3Bucket);
            this.groupBox1.Controls.Add(this.AWSAccessKey);
            this.groupBox1.Location = new System.Drawing.Point(13, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(596, 99);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Amazon S3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Bucket Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Folder:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Enable:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Enable:";
            // 
            // DestinationForm
            // 
            this.AcceptButton = this.CloseForm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(621, 238);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CreateOnStart);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.CheckBox CreateOnStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}