namespace Backup
{
    partial class FileListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileListForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FilesList = new System.Windows.Forms.ListView();
            this.file = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.subpath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Backup = new System.Windows.Forms.Button();
            this.CloseForm = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeFromDiscoveredFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Message = new System.Windows.Forms.Label();
            this.Remove = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.FilesList);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(875, 511);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Files to include in backup";
            // 
            // FilesList
            // 
            this.FilesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.file,
            this.subpath});
            this.FilesList.Location = new System.Drawing.Point(6, 19);
            this.FilesList.Name = "FilesList";
            this.FilesList.Size = new System.Drawing.Size(863, 486);
            this.FilesList.TabIndex = 0;
            this.FilesList.UseCompatibleStateImageBehavior = false;
            this.FilesList.View = System.Windows.Forms.View.Details;
            this.FilesList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.FilesList_ItemSelectionChanged);
            this.FilesList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FilesList_MouseDown);
            // 
            // file
            // 
            this.file.Text = "File";
            this.file.Width = 509;
            // 
            // subpath
            // 
            this.subpath.Text = "Subpath";
            this.subpath.Width = 347;
            // 
            // Backup
            // 
            this.Backup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Backup.Image = global::Backup.Properties.Resources.Open_16x;
            this.Backup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Backup.Location = new System.Drawing.Point(712, 536);
            this.Backup.Name = "Backup";
            this.Backup.Size = new System.Drawing.Size(75, 23);
            this.Backup.TabIndex = 0;
            this.Backup.Text = "Backup";
            this.Backup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Backup.UseVisualStyleBackColor = true;
            this.Backup.Click += new System.EventHandler(this.Backup_Click);
            // 
            // CloseForm
            // 
            this.CloseForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseForm.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseForm.Location = new System.Drawing.Point(806, 536);
            this.CloseForm.Name = "CloseForm";
            this.CloseForm.Size = new System.Drawing.Size(75, 23);
            this.CloseForm.TabIndex = 7;
            this.CloseForm.Text = "Close";
            this.CloseForm.UseVisualStyleBackColor = true;
            this.CloseForm.Click += new System.EventHandler(this.Close_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeFromDiscoveredFilesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(231, 26);
            // 
            // removeFromDiscoveredFilesToolStripMenuItem
            // 
            this.removeFromDiscoveredFilesToolStripMenuItem.Name = "removeFromDiscoveredFilesToolStripMenuItem";
            this.removeFromDiscoveredFilesToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.removeFromDiscoveredFilesToolStripMenuItem.Text = "Remove from discovered files";
            this.removeFromDiscoveredFilesToolStripMenuItem.Click += new System.EventHandler(this.RemoveFromDiscoveredFiles_Click);
            // 
            // Message
            // 
            this.Message.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Message.AutoSize = true;
            this.Message.Location = new System.Drawing.Point(15, 539);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(56, 13);
            this.Message.TabIndex = 8;
            this.Message.Text = "[Message]";
            // 
            // Remove
            // 
            this.Remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Remove.Enabled = false;
            this.Remove.Location = new System.Drawing.Point(615, 536);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(75, 23);
            this.Remove.TabIndex = 9;
            this.Remove.Text = "Remove";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // FileListForm
            // 
            this.AcceptButton = this.Backup;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseForm;
            this.ClientSize = new System.Drawing.Size(899, 573);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.Backup);
            this.Controls.Add(this.CloseForm);
            this.Controls.Add(this.Message);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileListForm";
            this.Text = "Backup - Discovered Files";
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView FilesList;
        private System.Windows.Forms.Button Backup;
        private System.Windows.Forms.Button CloseForm;
        private System.Windows.Forms.ColumnHeader file;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem removeFromDiscoveredFilesToolStripMenuItem;
        private System.Windows.Forms.Label Message;
        private System.Windows.Forms.ColumnHeader subpath;
        private System.Windows.Forms.Button Remove;
    }
}