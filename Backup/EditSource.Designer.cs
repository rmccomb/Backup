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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Directory:";
            // 
            // Directory
            // 
            this.Directory.Location = new System.Drawing.Point(85, 20);
            this.Directory.Name = "Directory";
            this.Directory.Size = new System.Drawing.Size(517, 20);
            this.Directory.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Pattern:";
            // 
            // Pattern
            // 
            this.Pattern.Location = new System.Drawing.Point(85, 49);
            this.Pattern.Name = "Pattern";
            this.Pattern.Size = new System.Drawing.Size(104, 20);
            this.Pattern.TabIndex = 3;
            this.Pattern.Text = "*.*";
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(85, 87);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(125, 23);
            this.Browse.TabIndex = 4;
            this.Browse.Text = "Browse Directory...";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // Commit
            // 
            this.Commit.Location = new System.Drawing.Point(446, 87);
            this.Commit.Name = "Commit";
            this.Commit.Size = new System.Drawing.Size(75, 23);
            this.Commit.TabIndex = 5;
            this.Commit.Text = "OK";
            this.Commit.UseVisualStyleBackColor = true;
            this.Commit.Click += new System.EventHandler(this.Commit_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(527, 87);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 6;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // EditSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 128);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Commit);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.Pattern);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Directory);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditSource";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "EditSource";
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
    }
}