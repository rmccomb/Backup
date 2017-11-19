using Backup.Logic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using System.Linq;

namespace Backup
{
    public partial class ConfigureForm : Form
    {
        private List<Source> _sources;
        private DestinationSettings _settings;
        private string _awsSecretKey;
        FileList fileList;

        public ConfigureForm()
        {
            InitializeComponent();
            this._sources = FileManager.GetSources();
            this._settings = FileManager.GetSettings();
            this.PopulateControls();
        }
        
        public void PopulateControls()
        {
            this.Sources.BeginUpdate();
            this.Sources.Items.Clear();
            foreach (var source in this._sources)
            {
                this.Sources.Items.Add(new ListViewItem(new string[] { source.Directory, source.Pattern }));
            }
            this.Sources.EndUpdate();

            this.ArchivePath.Text = this._settings.ArchiveDirectory;
            this.AWSProfileName.Text = this._settings.AWSProfileName;
            this.S3BucketName.Text = this._settings.S3Bucket;
            this.AWSAccessKey.Text = this._settings.AWSAccessKeyID;
            this._awsSecretKey = this._settings.AWSSecretAccessKey;

            if (!String.IsNullOrEmpty(this.ArchivePath.Text))
                this.IsFileSystem.Checked = true;

            if (!String.IsNullOrEmpty(this.S3BucketName.Text))
                this.IsS3Bucket.Checked = true;
        }

        private void New_Click(object sender, EventArgs e)
        {
            var editSource = new EditSource();
            var result = editSource.ShowDialog();
            if (result == DialogResult.OK)
            {
                var vals = editSource.GetValues();
                this.Sources.Items.Add(new ListViewItem(new string[] { vals.Item1, vals.Item2 }));
            }
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            var item = this.Sources.SelectedItems[0];
            var result = new EditSource(item).ShowDialog();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (this.Sources.SelectedItems.Count > 0)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    this._sources.RemoveAt(this.Sources.SelectedIndices[0]);
                    PopulateControls();
                }
            }
        }

        private void Sources_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Sources.SelectedItems != null)
            {
                this.Edit.Enabled = true;
                this.Delete.Enabled = true;
            }
            else
            {
                this.Edit.Enabled = false;
                this.Delete.Enabled = false;
            }
        }

        private void Commit_Click(object sender, EventArgs e)
        {
            try
            {
                var toSave = new List<Source>();
                foreach (ListViewItem item in this.Sources.Items)
                    toSave.Add(
                        new Source(item.SubItems[0].Text, item.SubItems[1].Text)); 
                
                FileManager.SaveSources(toSave);
                FileManager.SaveSettings(new DestinationSettings
                {
                    ArchiveDirectory = this.IsFileSystem.Checked ? this.ArchivePath.Text : "",
                    AWSAccessKeyID = this.IsS3Bucket.Checked ? this.AWSAccessKey.Text : "",
                    AWSProfileName = this.IsS3Bucket.Checked ? this.AWSProfileName.Text : "",
                    AWSSecretAccessKey = this.IsS3Bucket.Checked ? this._awsSecretKey : "",
                    S3Bucket = this.IsS3Bucket.Checked ? this.S3BucketName.Text : ""
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An Error Occurred");
            }
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            var result = this.folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                this.ArchivePath.Text = this.folderBrowserDialog1.SelectedPath;
        }

        private void IsFileSystem_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsFileSystem.Checked)
            {
                this.ArchivePath.Enabled = true;
                this.Browse.Enabled = true;
            }
            else
            {
                this.ArchivePath.Enabled = false;
                this.Browse.Enabled = false;
            }
        }

        private void IsS3Bucket_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsS3Bucket.Checked)
            {
                this.S3BucketName.Enabled = true;
                this.AWSProfileName.Enabled = true;
                this.AWSAccessKey.Enabled = true;
                this.AddSecret.Enabled = true;
            }
            else
            {
                this.S3BucketName.Enabled = false;
                this.AWSProfileName.Enabled = false;
                this.AWSAccessKey.Enabled = false;
                this.AddSecret.Enabled = false;
            }
        }

        private void AddSecret_Click(object sender, EventArgs e)
        {
            var dlg = new EditSecret();
            var result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this._awsSecretKey = dlg.Secret;
            }
            else
            {
                this._awsSecretKey = "";
            }
        }

        private void Discover_Click(object sender, EventArgs e)
        {
            if (this.fileList == null)
            {
                this.fileList = new FileList();
                this.fileList.FormClosed += FileList_FormClosed;
                this.fileList.ShowDialog();
            }
        }

        private void FileList_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.fileList = null;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
