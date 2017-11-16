namespace Backup
{
    partial class EditSecret
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
            this.Commit = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SecretID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Commit
            // 
            this.Commit.Location = new System.Drawing.Point(255, 52);
            this.Commit.Name = "Commit";
            this.Commit.Size = new System.Drawing.Size(75, 23);
            this.Commit.TabIndex = 8;
            this.Commit.Text = "OK";
            this.Commit.UseVisualStyleBackColor = true;
            this.Commit.Click += new System.EventHandler(this.Commit_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(337, 52);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 7;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Secret ID:";
            // 
            // SecretID
            // 
            this.SecretID.Location = new System.Drawing.Point(88, 17);
            this.SecretID.Name = "SecretID";
            this.SecretID.Size = new System.Drawing.Size(324, 20);
            this.SecretID.TabIndex = 10;
            // 
            // EditSecret
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 93);
            this.Controls.Add(this.SecretID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Commit);
            this.Controls.Add(this.Cancel);
            this.Name = "EditSecret";
            this.Text = "Add Secret";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Commit;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SecretID;
    }
}