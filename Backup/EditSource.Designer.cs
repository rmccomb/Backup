namespace Backup
{
    partial class EditSource
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditSource));
            this.label1 = new System.Windows.Forms.Label();
            this.Directory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Pattern = new System.Windows.Forms.TextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.Commit = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.IsModifiedOnly = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Directory:";
            // 
            // Directory
            // 
            this.Directory.Location = new System.Drawing.Point(120, 20);
            this.Directory.Name = "Directory";
            this.Directory.Size = new System.Drawing.Size(482, 20);
            this.Directory.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Pattern:";
            // 
            // Pattern
            // 
            this.Pattern.Location = new System.Drawing.Point(120, 49);
            this.Pattern.Name = "Pattern";
            this.Pattern.Size = new System.Drawing.Size(104, 20);
            this.Pattern.TabIndex = 3;
            this.Pattern.Text = "*.*";
            // 
            // Browse
            // 
            this.Browse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Browse.Location = new System.Drawing.Point(120, 113);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(125, 23);
            this.Browse.TabIndex = 4;
            this.Browse.Text = "Select Directory...";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // Commit
            // 
            this.Commit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Commit.Location = new System.Drawing.Point(441, 113);
            this.Commit.Name = "Commit";
            this.Commit.Size = new System.Drawing.Size(75, 23);
            this.Commit.TabIndex = 5;
            this.Commit.Text = "OK";
            this.Commit.UseVisualStyleBackColor = true;
            this.Commit.Click += new System.EventHandler(this.Commit_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(524, 113);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 6;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // IsModifiedOnly
            // 
            this.IsModifiedOnly.AutoSize = true;
            this.IsModifiedOnly.Location = new System.Drawing.Point(126, 82);
            this.IsModifiedOnly.Name = "IsModifiedOnly";
            this.IsModifiedOnly.Size = new System.Drawing.Size(15, 14);
            this.IsModifiedOnly.TabIndex = 7;
            this.IsModifiedOnly.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Modified Files Only:";
            // 
            // EditSource
            // 
            this.AcceptButton = this.Commit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(618, 151);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.IsModifiedOnly);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Commit);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.Pattern);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Directory);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditSource";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Source Directory";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Directory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Pattern;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.Button Commit;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox IsModifiedOnly;
        private System.Windows.Forms.Label label3;
    }
}