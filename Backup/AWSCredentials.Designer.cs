namespace Backup
{
    partial class AWSCredentials
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AWSCredentials));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AWSAccessKey = new System.Windows.Forms.TextBox();
            this.AddAWSSecret = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Commit = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.AWSAccessKey);
            this.groupBox4.Controls.Add(this.AddAWSSecret);
            this.groupBox4.Location = new System.Drawing.Point(12, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(485, 55);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "AWS Credentials";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Access Key ID:";
            // 
            // AWSAccessKey
            // 
            this.AWSAccessKey.Location = new System.Drawing.Point(100, 19);
            this.AWSAccessKey.Name = "AWSAccessKey";
            this.AWSAccessKey.Size = new System.Drawing.Size(246, 20);
            this.AWSAccessKey.TabIndex = 12;
            // 
            // AddAWSSecret
            // 
            this.AddAWSSecret.Location = new System.Drawing.Point(352, 17);
            this.AddAWSSecret.Name = "AddAWSSecret";
            this.AddAWSSecret.Size = new System.Drawing.Size(125, 23);
            this.AddAWSSecret.TabIndex = 15;
            this.AddAWSSecret.Text = "Add Secret...";
            this.AddAWSSecret.UseVisualStyleBackColor = true;
            this.AddAWSSecret.Click += new System.EventHandler(this.AddAWSSecret_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(410, 89);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 17;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // Commit
            // 
            this.Commit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Commit.Location = new System.Drawing.Point(319, 89);
            this.Commit.Name = "Commit";
            this.Commit.Size = new System.Drawing.Size(75, 23);
            this.Commit.TabIndex = 16;
            this.Commit.Text = "OK";
            this.Commit.UseVisualStyleBackColor = true;
            this.Commit.Click += new System.EventHandler(this.Commit_Click);
            // 
            // AWSCredentials
            // 
            this.AcceptButton = this.Commit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(506, 129);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.Commit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AWSCredentials";
            this.Text = "Edit AWS Credentials";
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AWSAccessKey;
        private System.Windows.Forms.Button AddAWSSecret;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Commit;
    }
}