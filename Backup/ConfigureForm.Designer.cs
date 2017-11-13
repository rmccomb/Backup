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
            this.uiSources = new System.Windows.Forms.ListView();
            this.Directory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Pattern = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.New = new System.Windows.Forms.Button();
            this.Edit = new System.Windows.Forms.Button();
            this.Delete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.Commit = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiSources
            // 
            this.uiSources.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Directory,
            this.Pattern});
            this.uiSources.Location = new System.Drawing.Point(6, 19);
            this.uiSources.MultiSelect = false;
            this.uiSources.Name = "uiSources";
            this.uiSources.Size = new System.Drawing.Size(514, 168);
            this.uiSources.TabIndex = 0;
            this.uiSources.UseCompatibleStateImageBehavior = false;
            this.uiSources.View = System.Windows.Forms.View.Details;
            this.uiSources.SelectedIndexChanged += new System.EventHandler(this.uiSources_SelectedIndexChanged);
            // 
            // Directory
            // 
            this.Directory.Text = "Directory";
            this.Directory.Width = 440;
            // 
            // Pattern
            // 
            this.Pattern.Text = "Pattern";
            // 
            // New
            // 
            this.New.Location = new System.Drawing.Point(269, 198);
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
            this.Edit.Location = new System.Drawing.Point(351, 198);
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
            this.Delete.Location = new System.Drawing.Point(433, 198);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(77, 23);
            this.Delete.TabIndex = 3;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.uiSources);
            this.groupBox1.Controls.Add(this.Delete);
            this.groupBox1.Controls.Add(this.Edit);
            this.groupBox1.Controls.Add(this.New);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(526, 236);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Included Directories and File Patterns";
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(445, 288);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Commit
            // 
            this.Commit.Location = new System.Drawing.Point(363, 288);
            this.Commit.Name = "Commit";
            this.Commit.Size = new System.Drawing.Size(75, 23);
            this.Commit.TabIndex = 6;
            this.Commit.Text = "OK";
            this.Commit.UseVisualStyleBackColor = true;
            this.Commit.Click += new System.EventHandler(this.Commit_Click);
            // 
            // ConfigureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 474);
            this.Controls.Add(this.Commit);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ConfigureForm";
            this.Text = "ConfigureForm";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView uiSources;
        private System.Windows.Forms.ColumnHeader Directory;
        private System.Windows.Forms.ColumnHeader Pattern;
        private System.Windows.Forms.Button New;
        private System.Windows.Forms.Button Edit;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Commit;
    }
}