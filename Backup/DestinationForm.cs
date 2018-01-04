using System;
using System.Windows.Forms;
using Backup.Logic;

namespace Backup
{
    public partial class DestinationForm : Form
    {
        private Settings settings;
        private string _awsAccessKey;
        private string _awsSecret;

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
                {
                    SaveSettings();
                    Close();
                }
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
            this.aWSRegionEndPointBindingSource.DataSource = AWSHelper.GetAllRegions();

            this.settings = FileManager.GetSettings();

            // File System
            ArchivePath.Text = settings.FileSystemDirectory;
            IsFileSystem.Checked = settings.IsFileSystemEnabled;
            if (settings.IsFileSystemEnabled)
                BrowseDirectory.Enabled = true;
            else
                BrowseDirectory.Enabled = false;

            // S3 Bucket
            S3BucketName.Text = settings.AWSS3Bucket;
            IsS3Bucket.Checked = settings.IsS3BucketEnabled;
            S3Region.SelectedValue = settings.AWSS3Region == null ? settings.AWSS3Region.SystemName : AWSHelper.GetDefaultRegionSystemName();
            if (S3BucketName.Text != string.Empty)
                ListBucketContents.Enabled = true;
            else
                ListBucketContents.Enabled = false;

            // Glacier
            GlacierVaultName.Text = settings.AWSGlacierVault;
            IsGlacier.Checked = settings.IsGlacierEnabled;
            GlacierRegion.SelectedValue = settings.AWSGlacierRegion == null ?settings.AWSGlacierRegion.SystemName : AWSHelper.GetDefaultRegionSystemName();
            if (GlacierVaultName.Text != string.Empty)
                ListInventory.Enabled = true;
            else
                ListInventory.Enabled = false;

            _awsAccessKey = settings.AWSAccessKeyID;
            _awsSecret = settings.AWSSecretAccessKey;

            CreateOnStart.Checked = settings.CreateBackupOnStart;
        }

        private void IsFileSystem_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsFileSystem.Checked)
            {
                this.ArchivePath.Enabled = true;
                this.BrowseDirectory.Enabled = true;
            }
            else
            {
                this.ArchivePath.Enabled = false;
                this.BrowseDirectory.Enabled = false;
            }
        }

        private void IsS3Bucket_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsS3Bucket.Checked)
            {
                this.S3BucketName.Enabled = true;
                this.S3Region.Enabled = true;
                if (this.S3BucketName.Text != string.Empty)
                    this.ListBucketContents.Enabled = true;
            }
            else
            {
                this.S3BucketName.Enabled = false;
                this.S3Region.Enabled = false;
            }
        }

        private void IsGlacier_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsGlacier.Checked)
            {
                this.GlacierVaultName.Enabled = true;
                this.GlacierRegion.Enabled = true;
                if (this.GlacierVaultName.Text != string.Empty)
                    this.ListInventory.Enabled = true;
            }
            else
            {
                this.GlacierVaultName.Enabled = false;
                this.GlacierRegion.Enabled = false;
            }
        }

        private bool HasChanges()
        {
            if (settings.FileSystemDirectory != this.ArchivePath.Text ||
                settings.AWSAccessKeyID != this._awsAccessKey ||
                settings.AWSSecretAccessKey != this._awsSecret ||
                settings.AWSS3Bucket != this.S3BucketName.Text ||
                settings.IsFileSystemEnabled != this.IsFileSystem.Checked ||
                settings.IsS3BucketEnabled != this.IsS3Bucket.Checked ||
                settings.CreateBackupOnStart != this.CreateOnStart.Checked ||
                settings.IsGlacierEnabled != this.IsGlacier.Checked ||
                settings.AWSGlacierVault != this.GlacierVaultName.Text ||
                settings.AWSS3Region != (AWSRegionEndPoint)this.S3Region.SelectedItem ||
                settings.AWSGlacierRegion != (AWSRegionEndPoint)this.GlacierRegion.SelectedItem)
                return true;
            else
                return false;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                this.SaveSettings();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An Error Occurred");
            }
        }

        private void SaveSettings()
        {
            this.settings = new Settings
            {
                FileSystemDirectory = this.ArchivePath.Text,
                IsFileSystemEnabled = this.IsFileSystem.Checked,
                AWSAccessKeyID = this._awsAccessKey,
                AWSSecretAccessKey = this._awsSecret,
                AWSS3Bucket = this.S3BucketName.Text,
                IsS3BucketEnabled = this.IsS3Bucket.Checked,
                AWSS3Region = (AWSRegionEndPoint)this.S3Region.SelectedItem,
                CreateBackupOnStart = this.CreateOnStart.Checked,
                IsGlacierEnabled = this.IsGlacier.Checked,
                AWSGlacierVault = this.GlacierVaultName.Text,
                AWSGlacierRegion = (AWSRegionEndPoint)this.GlacierRegion.SelectedItem,
            };
            FileManager.SaveSettings(this.settings);
            PopulateControls();
        }

        private void ListBucketContents_Click(object sender, EventArgs e)
        {
            this.SaveSettings();
            var dlg = new BucketContentsForm
            {
                Text = "S3 Bucket: " + this.settings.AWSS3Bucket
            };
            dlg.Show();
        }

        private void ListInventory_Click(object sender, EventArgs e)
        {
            this.SaveSettings();
            var dlg = new InventoryForm
            {
                Text = "Vault: " + this.settings.AWSGlacierVault
            };
            dlg.ShowDialog();
        }

        private void AWSCredentials_Click(object sender, EventArgs e)
        {
            var dlg = new AWSCredentials(this._awsAccessKey);
            var result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this._awsAccessKey = dlg.AccessKey;
                this._awsSecret = dlg.Secret;
            }
        }

        private void S3BucketName_TextChanged(object sender, EventArgs e)
        {
            if (this.S3BucketName.Text != string.Empty)
                this.ListBucketContents.Enabled = true;
            else
                this.ListBucketContents.Enabled = false;
        }

        private void GlacierVaultName_TextChanged(object sender, EventArgs e)
        {
            if (this.GlacierVaultName.Text != string.Empty)
                this.ListInventory.Enabled = true;
            else
                this.ListInventory.Enabled = false;
        }
    }
}
