using Backup.Logic;
using System;
using System.Windows.Forms;

namespace Backup
{
    public partial class DestinationForm : Form
    {
        private string _awsSecretKey;

        public DestinationForm()
        {
            InitializeComponent();
            PopulateControls();
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
            var _settings = FileManager.GetSettings();

            ArchivePath.Text = _settings.ArchiveDirectory;
            AWSProfileName.Text = _settings.AWSProfileName;
            S3BucketName.Text = _settings.S3Bucket;
            AWSAccessKey.Text = _settings.AWSAccessKeyID;
            _awsSecretKey = _settings.AWSSecretAccessKey;

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

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                FileManager.SaveSettings(new DestinationSettings
                {
                    ArchiveDirectory = this.IsFileSystem.Checked ? this.ArchivePath.Text : "",
                    AWSAccessKeyID = this.IsS3Bucket.Checked ? this.AWSAccessKey.Text : "",
                    AWSProfileName = this.IsS3Bucket.Checked ? this.AWSProfileName.Text : "",
                    AWSSecretAccessKey = this.IsS3Bucket.Checked ? this._awsSecretKey : "",
                    S3Bucket = this.IsS3Bucket.Checked ? this.S3BucketName.Text : ""
                });
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An Error Occurred");
            }
        }
    }
}
