using Backup.Logic;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Backup
{
    public partial class DestinationForm : Form
    {
        private DestinationSettings settings;
        private string _awsSecretKey;

        public DestinationForm()
        {
            InitializeComponent();
            PopulateControls();
            this.FormClosing += DestinationForm_FormClosing;
        }

        private void DestinationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.HasChanges())
            {
                var dlg = new MessageForm("Commit changes?", "Confirm Close");
                var result = dlg.ShowDialog(this);
                if (result == DialogResult.Yes)
                    SaveSettings();
            }
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            var result = this.folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.ArchivePath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void PopulateControls()
        {
            this.settings = FileManager.GetSettings();

            ArchivePath.Text = settings.ArchiveDirectory;
            //AWSProfileName.Text = _settings.AWSProfileName;
            S3BucketName.Text = settings.S3Bucket;
            AWSAccessKey.Text = settings.AWSAccessKeyID;
            _awsSecretKey = settings.AWSSecretAccessKey;

            if (!String.IsNullOrEmpty(ArchivePath.Text))
                IsFileSystem.Checked = true;

            if (!String.IsNullOrEmpty(S3BucketName.Text))
                IsS3Bucket.Checked = true;
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
                this.ArchivePath.Text = String.Empty;
                this.ArchivePath.Enabled = false;
                this.Browse.Enabled = false;
            }
        }

        private void IsS3Bucket_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsS3Bucket.Checked)
            {
                this.S3BucketName.Enabled = true;
                //this.AWSProfileName.Enabled = true;
                this.AWSAccessKey.Enabled = true;
                this.AddSecret.Enabled = true;
            }
            else
            {
                this.S3BucketName.Text = String.Empty;
                this.S3BucketName.Enabled = false;
                //this.AWSProfileName.Enabled = false;
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

        private bool HasChanges()
        {
            if (settings.ArchiveDirectory != this.ArchivePath.Text ||
                settings.AWSAccessKeyID != this.AWSAccessKey.Text ||
                settings.AWSSecretAccessKey != this._awsSecretKey ||
                settings.S3Bucket != this.S3BucketName.Text)
                return true;
            else
                return false;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An Error Occurred");
            }
        }

        private void SaveSettings()
        {
            Debug.WriteLine("SaveSettings");
            this.settings = new DestinationSettings
            {
                ArchiveDirectory = this.IsFileSystem.Checked ? this.ArchivePath.Text : "",
                AWSAccessKeyID = this.IsS3Bucket.Checked ? this.AWSAccessKey.Text : "",
                AWSSecretAccessKey = this.IsS3Bucket.Checked ? this._awsSecretKey : "",
                S3Bucket = this.IsS3Bucket.Checked ? this.S3BucketName.Text : ""
            };
            FileManager.SaveSettings(this.settings);
            PopulateControls();
            this.Close();
        }

        private void MoreOptions_Click(object sender, EventArgs e)
        {

        }
    }
}
