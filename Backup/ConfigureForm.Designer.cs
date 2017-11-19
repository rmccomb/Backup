namespace Backup
{
    partial class ConfigureForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureForm));
            this.Sources = new System.Windows.Forms.ListView();
            this.Directory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Pattern = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastBackupDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.New = new System.Windows.Forms.Button();
            this.Edit = new System.Windows.Forms.Button();
            this.Delete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.Commit = new System.Windows.Forms.Button();
            this.IsFileSystem = new System.Windows.Forms.CheckBox();
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.Discover = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Sources
            // 
            this.Sources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Sources.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Directory,
            this.Pattern,
            this.lastBackupDate});
            this.Sources.FullRowSelect = true;
            this.Sources.Location = new System.Drawing.Point(6, 19);
            this.Sources.MultiSelect = false;
            this.Sources.Name = "Sources";
            this.Sources.Size = new System.Drawing.Size(588, 169);
            this.Sources.TabIndex = 0;
            this.Sources.UseCompatibleStateImageBehavior = false;
            this.Sources.View = System.Windows.Forms.View.Details;
            this.Sources.SelectedIndexChanged += new System.EventHandler(this.Sources_SelectedIndexChanged);
            // 
            // Directory
            // 
            this.Directory.Text = "Directory";
            this.Directory.Width = 382;
            // 
            // Pattern
            // 
            this.Pattern.Text = "Pattern";
            this.Pattern.Width = 50;
            // 
            // lastBackupDate
            // 
            this.lastBackupDate.Text = "Last Backup";
            this.lastBackupDate.Width = 143;
            // 
            // New
            // 
            this.New.Location = new System.Drawing.Point(339, 194);
            this.New.Name = "New";
            this.New.Size = new System.Drawing.Size(77, 23);
            this.New.TabIndex = 1;
            this.New.Text = "New...";
            this.New.UseVisualStyleBackColor = true;
            this.New.Click += new System.EventHandler(this.New_Click);
            // 
            // Edit
            // 
            this.Edit.Enabled = false;
            this.Edit.Location = new System.Drawing.Point(422, 194);
            this.Edit.Name = "Edit";
            this.Edit.Size = new System.Drawing.Size(77, 23);
            this.Edit.TabIndex = 2;
            this.Edit.Text = "Edit...";
            this.Edit.UseVisualStyleBackColor = true;
            this.Edit.Click += new System.EventHandler(this.Edit_Click);
            // 
            // Delete
            // 
            this.Delete.Enabled = false;
            this.Delete.Location = new System.Drawing.Point(507, 194);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(77, 23);
            this.Delete.TabIndex = 3;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Remove_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.Sources);
            this.groupBox1.Controls.Add(this.Delete);
            this.groupBox1.Controls.Add(this.Edit);
            this.groupBox1.Controls.Add(this.New);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(600, 228);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Included Directories and File Patterns";
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(519, 483);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "Close";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Close_Click);
            // 
            // Commit
            // 
            this.Commit.Location = new System.Drawing.Point(434, 483);
            this.Commit.Name = "Commit";
            this.Commit.Size = new System.Drawing.Size(75, 23);
            this.Commit.TabIndex = 6;
            this.Commit.Text = "Save";
            this.Commit.UseVisualStyleBackColor = true;
            this.Commit.Click += new System.EventHandler(this.Commit_Click);
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
            this.groupBox2.Location = new System.Drawing.Point(12, 255);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(600, 195);
            this.groupBox2.TabIndex = 8;
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
            // Discover
            // 
            this.Discover.Image = global::Backup.Properties.Resources.StartPoint_16x;
            this.Discover.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Discover.Location = new System.Drawing.Point(28, 483);
            this.Discover.Name = "Discover";
            this.Discover.Size = new System.Drawing.Size(112, 23);
            this.Discover.TabIndex = 9;
            this.Discover.Text = "Discover Files...";
            this.Discover.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Discover.UseVisualStyleBackColor = true;
            this.Discover.Click += new System.EventHandler(this.Discover_Click);
            // 
            // ConfigureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 525);
            this.Controls.Add(this.Discover);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Commit);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfigureForm";
            this.Text = "Configure";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView Sources;
        private System.Windows.Forms.ColumnHeader Directory;
        private System.Windows.Forms.ColumnHeader Pattern;
        private System.Windows.Forms.Button New;
        private System.Windows.Forms.Button Edit;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Commit;
        private System.Windows.Forms.CheckBox IsFileSystem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox ArchivePath;
        private System.Windows.Forms.CheckBox IsS3Bucket;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox AWSAccessKey;
        private System.Windows.Forms.TextBox AWSProfileName;
        private System.Windows.Forms.TextBox S3BucketName;
        private System.Windows.Forms.Button AddSecret;
        private System.Windows.Forms.Button Discover;
        private System.Windows.Forms.ColumnHeader lastBackupDate;
    }
}