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
                    SaveSettings(true);
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

            ArchivePath.Text = settings.FileSystemDirectory;
            S3BucketName.Text = settings.S3Bucket;
            AWSAccessKey.Text = settings.AWSAccessKeyID;
            _awsSecretKey = settings.AWSSecretAccessKey;
            IsFileSystem.Checked = settings.IsFileSystemEnabled;
            IsS3Bucket.Checked = settings.IsS3BucketEnabled;

            CreateOnStart.Checked = settings.CreateBackupOnStart;
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
                this.AWSAccessKey.Enabled = true;
                this.AddSecret.Enabled = true;
            }
            else
            {
                this.S3BucketName.Enabled = false;
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
        }

        private bool HasChanges()
        {
            if (settings.FileSystemDirectory != this.ArchivePath.Text ||
                settings.AWSAccessKeyID != this.AWSAccessKey.Text ||
                settings.AWSSecretAccessKey != this._awsSecretKey ||
                settings.S3Bucket != this.S3BucketName.Text ||
                settings.IsFileSystemEnabled != this.IsFileSystem.Checked ||
                settings.IsS3BucketEnabled != this.IsS3Bucket.Checked ||
                settings.CreateBackupOnStart != this.CreateOnStart.Checked)
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

        private void SaveSettings(bool isClosing = false)
        {
            Debug.WriteLine("SaveSettings, closing:" + isClosing);
            this.settings = new DestinationSettings
            {
                FileSystemDirectory = this.ArchivePath.Text,
                IsFileSystemEnabled = this.IsFileSystem.Checked,
                AWSAccessKeyID = this.AWSAccessKey.Text,
                AWSSecretAccessKey = this._awsSecretKey,
                S3Bucket = this.S3BucketName.Text,
                IsS3BucketEnabled = this.IsS3Bucket.Checked,
                CreateBackupOnStart = this.CreateOnStart.Checked
            };
            FileManager.SaveSettings(this.settings);
            PopulateControls();
            this.Close();
        }
    }
}
