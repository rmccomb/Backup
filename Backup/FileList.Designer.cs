namespace Backup
{
    partial class FileList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileList));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FilesList = new System.Windows.Forms.ListView();
            this.file = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Backup = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeFromDiscoveredFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Message = new System.Windows.Forms.Label();
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
            this.groupBox1.Size = new System.Drawing.Size(730, 443);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Discovered Files";
            // 
            // FilesList
            // 
            this.FilesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.file});
            this.FilesList.Location = new System.Drawing.Point(6, 19);
            this.FilesList.Name = "FilesList";
            this.FilesList.Size = new System.Drawing.Size(718, 418);
            this.FilesList.TabIndex = 0;
            this.FilesList.UseCompatibleStateImageBehavior = false;
            this.FilesList.View = System.Windows.Forms.View.Details;
            this.FilesList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FilesList_MouseDown);
            // 
            // file
            // 
            this.file.Text = "File";
            this.file.Width = 706;
            // 
            // Backup
            // 
            this.Backup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Backup.Image = global::Backup.Properties.Resources.Open_16x;
            this.Backup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Backup.Location = new System.Drawing.Point(565, 477);
            this.Backup.Name = "Backup";
            this.Backup.Size = new System.Drawing.Size(75, 23);
            this.Backup.TabIndex = 0;
            this.Backup.Text = "Backup";
            this.Backup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Backup.UseVisualStyleBackColor = true;
            this.Backup.Click += new System.EventHandler(this.Backup_Click);
            // 
            // Close
            // 
            this.Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Close.Location = new System.Drawing.Point(649, 477);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 23);
            this.Close.TabIndex = 7;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.Close_Click);
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
            this.removeFromDiscoveredFilesToolStripMenuItem.Click += new System.EventHandler(this.removeFromDiscoveredFilesToolStripMenuItem_Click);
            // 
            // Message
            // 
            this.Message.AutoSize = true;
            this.Message.Location = new System.Drawing.Point(18, 477);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(56, 13);
            this.Message.TabIndex = 8;
            this.Message.Text = "[Message]";
            // 
            // FileList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 522);
            this.Controls.Add(this.Message);
            this.Controls.Add(this.Backup);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FileList";
            this.Text = "File List";
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView FilesList;
        private System.Windows.Forms.Button Backup;
        private System.Windows.Forms.Button Close;
        private System.Windows.Forms.ColumnHeader file;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem removeFromDiscoveredFilesToolStripMenuItem;
        private System.Windows.Forms.Label Message;
    }
}