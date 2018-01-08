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
            this.modifiedOnly = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastBackupDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AddDirectory = new System.Windows.Forms.Button();
            this.Edit = new System.Windows.Forms.Button();
            this.Delete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Discover = new System.Windows.Forms.Button();
            this.CloseForm = new System.Windows.Forms.Button();
            this.BackupDestination = new System.Windows.Forms.Button();
            this.IsBackupOnLogoff = new System.Windows.Forms.CheckBox();
            this.CreateOnStart = new System.Windows.Forms.CheckBox();
            this.LaunchOnLogon = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
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
            this.modifiedOnly,
            this.lastBackupDate});
            this.Sources.FullRowSelect = true;
            this.Sources.Location = new System.Drawing.Point(6, 19);
            this.Sources.MultiSelect = false;
            this.Sources.Name = "Sources";
            this.Sources.Size = new System.Drawing.Size(661, 169);
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
            this.Pattern.Width = 53;
            // 
            // modifiedOnly
            // 
            this.modifiedOnly.Text = "Modified Only";
            this.modifiedOnly.Width = 83;
            // 
            // lastBackupDate
            // 
            this.lastBackupDate.Text = "Last Backup";
            this.lastBackupDate.Width = 138;
            // 
            // AddDirectory
            // 
            this.AddDirectory.Location = new System.Drawing.Point(409, 195);
            this.AddDirectory.Name = "AddDirectory";
            this.AddDirectory.Size = new System.Drawing.Size(77, 23);
            this.AddDirectory.TabIndex = 1;
            this.AddDirectory.Text = "Add...";
            this.AddDirectory.UseVisualStyleBackColor = true;
            this.AddDirectory.Click += new System.EventHandler(this.AddDirectory_Click);
            // 
            // Edit
            // 
            this.Edit.Enabled = false;
            this.Edit.Location = new System.Drawing.Point(494, 195);
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
            this.Delete.Location = new System.Drawing.Point(579, 195);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(77, 23);
            this.Delete.TabIndex = 3;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Remove_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.Sources);
            this.groupBox1.Controls.Add(this.Delete);
            this.groupBox1.Controls.Add(this.Edit);
            this.groupBox1.Controls.Add(this.Discover);
            this.groupBox1.Controls.Add(this.AddDirectory);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(673, 228);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Included Directories and File Patterns";
            // 
            // Discover
            // 
            this.Discover.Image = global::Backup.Properties.Resources.StartPoint_16x;
            this.Discover.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Discover.Location = new System.Drawing.Point(20, 195);
            this.Discover.Name = "Discover";
            this.Discover.Size = new System.Drawing.Size(112, 23);
            this.Discover.TabIndex = 9;
            this.Discover.Text = "Discover Files...";
            this.Discover.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Discover.UseVisualStyleBackColor = true;
            this.Discover.Click += new System.EventHandler(this.Discover_Click);
            // 
            // CloseForm
            // 
            this.CloseForm.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseForm.Location = new System.Drawing.Point(591, 320);
            this.CloseForm.Name = "CloseForm";
            this.CloseForm.Size = new System.Drawing.Size(75, 23);
            this.CloseForm.TabIndex = 5;
            this.CloseForm.Text = "Close";
            this.CloseForm.UseVisualStyleBackColor = true;
            this.CloseForm.Click += new System.EventHandler(this.Close_Click);
            // 
            // BackupDestination
            // 
            this.BackupDestination.Image = global::Backup.Properties.Resources.Cloud_16x;
            this.BackupDestination.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BackupDestination.Location = new System.Drawing.Point(514, 15);
            this.BackupDestination.Name = "BackupDestination";
            this.BackupDestination.Size = new System.Drawing.Size(140, 23);
            this.BackupDestination.TabIndex = 10;
            this.BackupDestination.Text = "Backup Destination...";
            this.BackupDestination.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BackupDestination.UseVisualStyleBackColor = true;
            this.BackupDestination.Click += new System.EventHandler(this.BackupDestination_Click);
            // 
            // IsBackupOnLogoff
            // 
            this.IsBackupOnLogoff.AutoSize = true;
            this.IsBackupOnLogoff.Location = new System.Drawing.Point(20, 42);
            this.IsBackupOnLogoff.Name = "IsBackupOnLogoff";
            this.IsBackupOnLogoff.Size = new System.Drawing.Size(177, 17);
            this.IsBackupOnLogoff.TabIndex = 11;
            this.IsBackupOnLogoff.Text = "Create an archive on user logoff";
            this.IsBackupOnLogoff.UseVisualStyleBackColor = true;
            // 
            // CreateOnStart
            // 
            this.CreateOnStart.AutoSize = true;
            this.CreateOnStart.Location = new System.Drawing.Point(20, 19);
            this.CreateOnStart.Name = "CreateOnStart";
            this.CreateOnStart.Size = new System.Drawing.Size(266, 17);
            this.CreateOnStart.TabIndex = 12;
            this.CreateOnStart.Text = "Create an archive whenever this program launches";
            this.CreateOnStart.UseVisualStyleBackColor = true;
            // 
            // LaunchOnLogon
            // 
            this.LaunchOnLogon.AutoSize = true;
            this.LaunchOnLogon.Location = new System.Drawing.Point(32, 324);
            this.LaunchOnLogon.Name = "LaunchOnLogon";
            this.LaunchOnLogon.Size = new System.Drawing.Size(189, 17);
            this.LaunchOnLogon.TabIndex = 13;
            this.LaunchOnLogon.Text = "Launch this program on user logon";
            this.LaunchOnLogon.UseVisualStyleBackColor = true;
            this.LaunchOnLogon.Click += new System.EventHandler(this.LaunchOnLogon_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.IsBackupOnLogoff);
            this.groupBox2.Controls.Add(this.CreateOnStart);
            this.groupBox2.Controls.Add(this.BackupDestination);
            this.groupBox2.Location = new System.Drawing.Point(12, 246);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(672, 68);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Backup Options";
            // 
            // ConfigureForm
            // 
            this.AcceptButton = this.CloseForm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseForm;
            this.ClientSize = new System.Drawing.Size(697, 355);
            this.Controls.Add(this.LaunchOnLogon);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.CloseForm);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigureForm";
            this.Text = "Backup - Configure";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView Sources;
        private System.Windows.Forms.ColumnHeader Directory;
        private System.Windows.Forms.ColumnHeader Pattern;
        private System.Windows.Forms.Button AddDirectory;
        private System.Windows.Forms.Button Edit;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button CloseForm;
        private System.Windows.Forms.Button Discover;
        private System.Windows.Forms.ColumnHeader lastBackupDate;
        private System.Windows.Forms.Button BackupDestination;
        private System.Windows.Forms.ColumnHeader modifiedOnly;
        private System.Windows.Forms.CheckBox IsBackupOnLogoff;
        private System.Windows.Forms.CheckBox CreateOnStart;
        private System.Windows.Forms.CheckBox LaunchOnLogon;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}