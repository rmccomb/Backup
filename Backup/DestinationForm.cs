using System;
using System.Diagnostics;
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

            this.S3RegionBindingSource.DataSource = AWSHelper.GetAllRegions();
            this.GlacierRegionBindingSource.DataSource = AWSHelper.GetAllRegions();
            // AWSHelper.GetDefaultRegionSystemName()

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

            if (settings.AWSS3Region != null)
                S3Region.SelectedValue = settings.AWSS3Region.SystemName;
            else
                GlacierRegion.SelectedValue = AWSHelper.GetDefaultRegionSystemName();

            if (S3BucketName.Text != string.Empty)
                ListBucketContents.Enabled = true;
            else
                ListBucketContents.Enabled = false;

            // Glacier
            GlacierVaultName.Text = settings.AWSGlacierVault;
            IsGlacier.Checked = settings.IsGlacierEnabled;

            if (settings.AWSGlacierRegion != null)
                GlacierRegion.SelectedValue = settings.AWSGlacierRegion.SystemName;
            else
                GlacierRegion.SelectedValue = AWSHelper.GetDefaultRegionSystemName();

            if (GlacierVaultName.Text != string.Empty)
                ListInventory.Enabled = true;
            else
                ListInventory.Enabled = false;

            _awsAccessKey = settings.AWSAccessKeyID;
            _awsSecret = settings.AWSSecretAccessKey;
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
            Debug.WriteLine(settings.AWSS3Region.SystemName != this.S3Region.SelectedValue.ToString());
            Debug.WriteLine(settings.AWSGlacierRegion.SystemName != (string)this.GlacierRegion.SelectedValue);
            var a = settings.AWSGlacierRegion.SystemName;
            var b = this.GlacierRegion.SelectedItem;

            if (settings.FileSystemDirectory != this.ArchivePath.Text ||
                settings.AWSAccessKeyID != this._awsAccessKey ||
                settings.AWSSecretAccessKey != this._awsSecret ||
                settings.IsFileSystemEnabled != this.IsFileSystem.Checked ||
                settings.IsS3BucketEnabled != this.IsS3Bucket.Checked ||
                settings.AWSS3Bucket != this.S3BucketName.Text ||
                settings.AWSS3Region.SystemName != ((AWSRegionEndPoint)this.S3Region.SelectedItem).SystemName ||
                settings.IsGlacierEnabled != this.IsGlacier.Checked ||
                settings.AWSGlacierVault != this.GlacierVaultName.Text ||
                settings.AWSGlacierRegion.SystemName != ((AWSRegionEndPoint)this.GlacierRegion.SelectedItem).SystemName)
                return true;
            else
                return false;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                this.SaveSettings();
                this.settings = FileManager.GetSettings();
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

                IsS3BucketEnabled = this.IsS3Bucket.Checked,
                AWSS3Bucket = this.S3BucketName.Text,
                AWSS3Region = (AWSRegionEndPoint)this.S3Region.SelectedItem,

                IsGlacierEnabled = this.IsGlacier.Checked,
                AWSGlacierVault = this.GlacierVaultName.Text,
                AWSGlacierRegion = (AWSRegionEndPoint)this.GlacierRegion.SelectedItem,

                AWSAccessKeyID = this._awsAccessKey,
                AWSSecretAccessKey = this._awsSecret
            };
            FileManager.SaveSettings(this.settings);
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

        private void GlacierRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("SelectedIndexChanged");
        }

        private void GlacierRegion_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Debug.WriteLine("SelectionChangeCommited");
        }
    }
}
