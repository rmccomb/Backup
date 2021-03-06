﻿namespace Backup
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DestinationForm));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BrowseDirectory = new System.Windows.Forms.Button();
            this.ArchivePath = new System.Windows.Forms.TextBox();
            this.IsFileSystem = new System.Windows.Forms.CheckBox();
            this.S3BucketName = new System.Windows.Forms.TextBox();
            this.IsS3Bucket = new System.Windows.Forms.CheckBox();
            this.CloseForm = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.Cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.S3Region = new System.Windows.Forms.ComboBox();
            this.S3RegionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.ListBucketContents = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ListInventory = new System.Windows.Forms.Button();
            this.GlacierRegion = new System.Windows.Forms.ComboBox();
            this.GlacierRegionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.IsGlacierLabel = new System.Windows.Forms.Label();
            this.IsGlacier = new System.Windows.Forms.CheckBox();
            this.GlacierVaultName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.AWSCredentials = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.S3RegionBindingSource)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GlacierRegionBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.BrowseDirectory);
            this.groupBox2.Controls.Add(this.ArchivePath);
            this.groupBox2.Controls.Add(this.IsFileSystem);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(566, 70);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File System";
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Folder:";
            // 
            // BrowseDirectory
            // 
            this.BrowseDirectory.Enabled = false;
            this.BrowseDirectory.Location = new System.Drawing.Point(430, 17);
            this.BrowseDirectory.Name = "BrowseDirectory";
            this.BrowseDirectory.Size = new System.Drawing.Size(123, 23);
            this.BrowseDirectory.TabIndex = 9;
            this.BrowseDirectory.Text = "Select Directory...";
            this.BrowseDirectory.UseVisualStyleBackColor = true;
            this.BrowseDirectory.Click += new System.EventHandler(this.Browse_Click);
            // 
            // ArchivePath
            // 
            this.ArchivePath.Enabled = false;
            this.ArchivePath.Location = new System.Drawing.Point(99, 19);
            this.ArchivePath.Name = "ArchivePath";
            this.ArchivePath.Size = new System.Drawing.Size(323, 20);
            this.ArchivePath.TabIndex = 9;
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
            // S3BucketName
            // 
            this.S3BucketName.Enabled = false;
            this.S3BucketName.Location = new System.Drawing.Point(98, 19);
            this.S3BucketName.Name = "S3BucketName";
            this.S3BucketName.Size = new System.Drawing.Size(323, 20);
            this.S3BucketName.TabIndex = 10;
            this.S3BucketName.TextChanged += new System.EventHandler(this.S3BucketName_TextChanged);
            // 
            // IsS3Bucket
            // 
            this.IsS3Bucket.AutoSize = true;
            this.IsS3Bucket.Location = new System.Drawing.Point(98, 47);
            this.IsS3Bucket.Name = "IsS3Bucket";
            this.IsS3Bucket.Size = new System.Drawing.Size(15, 14);
            this.IsS3Bucket.TabIndex = 8;
            this.IsS3Bucket.UseVisualStyleBackColor = true;
            this.IsS3Bucket.CheckedChanged += new System.EventHandler(this.IsS3Bucket_CheckedChanged);
            // 
            // CloseForm
            // 
            this.CloseForm.Location = new System.Drawing.Point(402, 250);
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
            this.Cancel.Location = new System.Drawing.Point(488, 250);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 11;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.S3Region);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.ListBucketContents);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.S3BucketName);
            this.groupBox1.Controls.Add(this.IsS3Bucket);
            this.groupBox1.Location = new System.Drawing.Point(13, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(565, 72);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Amazon S3";
            // 
            // S3Region
            // 
            this.S3Region.DataSource = this.S3RegionBindingSource;
            this.S3Region.DisplayMember = "DisplayName";
            this.S3Region.Enabled = false;
            this.S3Region.FormattingEnabled = true;
            this.S3Region.Location = new System.Drawing.Point(251, 44);
            this.S3Region.Name = "S3Region";
            this.S3Region.Size = new System.Drawing.Size(170, 21);
            this.S3Region.TabIndex = 19;
            this.S3Region.ValueMember = "SystemName";
            // 
            // S3RegionBindingSource
            // 
            this.S3RegionBindingSource.DataSource = typeof(Backup.Logic.AWSRegionEndPoint);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(201, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Region:";
            // 
            // ListBucketContents
            // 
            this.ListBucketContents.Enabled = false;
            this.ListBucketContents.Location = new System.Drawing.Point(429, 17);
            this.ListBucketContents.Name = "ListBucketContents";
            this.ListBucketContents.Size = new System.Drawing.Size(124, 23);
            this.ListBucketContents.TabIndex = 17;
            this.ListBucketContents.Text = "List Contents...";
            this.ListBucketContents.UseVisualStyleBackColor = true;
            this.ListBucketContents.Click += new System.EventHandler(this.ListBucketContents_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Enable:";
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ListInventory);
            this.groupBox3.Controls.Add(this.GlacierRegion);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.IsGlacierLabel);
            this.groupBox3.Controls.Add(this.IsGlacier);
            this.groupBox3.Controls.Add(this.GlacierVaultName);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(13, 167);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(565, 70);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Amazon Glacier";
            // 
            // ListInventory
            // 
            this.ListInventory.Enabled = false;
            this.ListInventory.Location = new System.Drawing.Point(429, 15);
            this.ListInventory.Name = "ListInventory";
            this.ListInventory.Size = new System.Drawing.Size(123, 23);
            this.ListInventory.TabIndex = 20;
            this.ListInventory.Text = "List Inventory...";
            this.ListInventory.UseVisualStyleBackColor = true;
            this.ListInventory.Click += new System.EventHandler(this.ListInventory_Click);
            // 
            // GlacierRegion
            // 
            this.GlacierRegion.DataSource = this.GlacierRegionBindingSource;
            this.GlacierRegion.DisplayMember = "DisplayName";
            this.GlacierRegion.Enabled = false;
            this.GlacierRegion.FormattingEnabled = true;
            this.GlacierRegion.Location = new System.Drawing.Point(251, 42);
            this.GlacierRegion.Name = "GlacierRegion";
            this.GlacierRegion.Size = new System.Drawing.Size(170, 21);
            this.GlacierRegion.TabIndex = 19;
            this.GlacierRegion.ValueMember = "SystemName";
            this.GlacierRegion.SelectedIndexChanged += new System.EventHandler(this.GlacierRegion_SelectedIndexChanged);
            this.GlacierRegion.SelectionChangeCommitted += new System.EventHandler(this.GlacierRegion_SelectionChangeCommitted);
            // 
            // GlacierRegionBindingSource
            // 
            this.GlacierRegionBindingSource.DataSource = typeof(Backup.Logic.AWSRegionEndPoint);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(201, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Region:";
            // 
            // IsGlacierLabel
            // 
            this.IsGlacierLabel.AutoSize = true;
            this.IsGlacierLabel.Location = new System.Drawing.Point(13, 45);
            this.IsGlacierLabel.Name = "IsGlacierLabel";
            this.IsGlacierLabel.Size = new System.Drawing.Size(43, 13);
            this.IsGlacierLabel.TabIndex = 13;
            this.IsGlacierLabel.Text = "Enable:";
            // 
            // IsGlacier
            // 
            this.IsGlacier.AutoSize = true;
            this.IsGlacier.Location = new System.Drawing.Point(99, 45);
            this.IsGlacier.Name = "IsGlacier";
            this.IsGlacier.Size = new System.Drawing.Size(15, 14);
            this.IsGlacier.TabIndex = 12;
            this.IsGlacier.UseVisualStyleBackColor = true;
            this.IsGlacier.CheckedChanged += new System.EventHandler(this.IsGlacier_CheckedChanged);
            // 
            // GlacierVaultName
            // 
            this.GlacierVaultName.Enabled = false;
            this.GlacierVaultName.Location = new System.Drawing.Point(99, 17);
            this.GlacierVaultName.Name = "GlacierVaultName";
            this.GlacierVaultName.Size = new System.Drawing.Size(323, 20);
            this.GlacierVaultName.TabIndex = 1;
            this.GlacierVaultName.TextChanged += new System.EventHandler(this.GlacierVaultName_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Vault Name:";
            // 
            // AWSCredentials
            // 
            this.AWSCredentials.Location = new System.Drawing.Point(28, 250);
            this.AWSCredentials.Name = "AWSCredentials";
            this.AWSCredentials.Size = new System.Drawing.Size(121, 23);
            this.AWSCredentials.TabIndex = 16;
            this.AWSCredentials.Text = "AWS Credentials...";
            this.AWSCredentials.UseVisualStyleBackColor = true;
            this.AWSCredentials.Click += new System.EventHandler(this.AWSCredentials_Click);
            // 
            // DestinationForm
            // 
            this.AcceptButton = this.CloseForm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(587, 289);
            this.Controls.Add(this.AWSCredentials);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.CloseForm);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DestinationForm";
            this.Text = "Backup - Destination of Archive";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.S3RegionBindingSource)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GlacierRegionBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox S3BucketName;
        private System.Windows.Forms.Button BrowseDirectory;
        private System.Windows.Forms.TextBox ArchivePath;
        private System.Windows.Forms.CheckBox IsS3Bucket;
        private System.Windows.Forms.CheckBox IsFileSystem;
        private System.Windows.Forms.Button CloseForm;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox GlacierVaultName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label IsGlacierLabel;
        private System.Windows.Forms.CheckBox IsGlacier;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ListBucketContents;
        private System.Windows.Forms.ComboBox S3Region;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox GlacierRegion;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.BindingSource S3RegionBindingSource;
        private System.Windows.Forms.Button ListInventory;
        private System.Windows.Forms.Button AWSCredentials;
        private System.Windows.Forms.BindingSource GlacierRegionBindingSource;
    }
}