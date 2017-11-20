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
            this.CloseForm = new System.Windows.Forms.Button();
            this.Discover = new System.Windows.Forms.Button();
            this.BackupDestination = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
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
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
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
            // CloseForm
            // 
            this.CloseForm.Location = new System.Drawing.Point(519, 254);
            this.CloseForm.Name = "CloseForm";
            this.CloseForm.Size = new System.Drawing.Size(75, 23);
            this.CloseForm.TabIndex = 5;
            this.CloseForm.Text = "Close";
            this.CloseForm.UseVisualStyleBackColor = true;
            this.CloseForm.Click += new System.EventHandler(this.Close_Click);
            // 
            // Discover
            // 
            this.Discover.Image = global::Backup.Properties.Resources.StartPoint_16x;
            this.Discover.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Discover.Location = new System.Drawing.Point(18, 254);
            this.Discover.Name = "Discover";
            this.Discover.Size = new System.Drawing.Size(112, 23);
            this.Discover.TabIndex = 9;
            this.Discover.Text = "Discover Files...";
            this.Discover.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Discover.UseVisualStyleBackColor = true;
            this.Discover.Click += new System.EventHandler(this.Discover_Click);
            // 
            // BackupDestination
            // 
            this.BackupDestination.Location = new System.Drawing.Point(151, 254);
            this.BackupDestination.Name = "BackupDestination";
            this.BackupDestination.Size = new System.Drawing.Size(131, 23);
            this.BackupDestination.TabIndex = 10;
            this.BackupDestination.Text = "Backup Destination...";
            this.BackupDestination.UseVisualStyleBackColor = true;
            this.BackupDestination.Click += new System.EventHandler(this.BackupDestination_Click);
            // 
            // ConfigureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 293);
            this.Controls.Add(this.BackupDestination);
            this.Controls.Add(this.Discover);
            this.Controls.Add(this.CloseForm);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfigureForm";
            this.Text = "Configure";
            this.groupBox1.ResumeLayout(false);
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
        private System.Windows.Forms.Button CloseForm;
        private System.Windows.Forms.Button Discover;
        private System.Windows.Forms.ColumnHeader lastBackupDate;
        private System.Windows.Forms.Button BackupDestination;
    }
}