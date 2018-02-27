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
            try
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
            catch (Exception ex)
            {
                Program.DisplayError(ex);
            }
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            try
            {
                var result = this.folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.ArchivePath.Text = this.folderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                Program.DisplayError(ex);
            }
        }

        private void PopulateControls()
        {
            try
            {
                this.settings = SettingsManager.GetSettings();

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
                    S3Region.SelectedValue = AWSHelper.GetDefaultRegionSystemName();

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
            catch (Exception ex)
            {
                Program.DisplayError(ex);
            }
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
                if (Validate(this.settings))
                {
                    this.SaveSettings();
                    this.settings = SettingsManager.GetSettings();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Program.DisplayError(ex);
            }
        }

        private bool Validate(Settings settings)
        {
            if (settings.IsFileSystemEnabled 
                && (string.IsNullOrEmpty(settings.FileSystemDirectory.Trim())
                || !System.IO.Directory.Exists(settings.FileSystemDirectory.Trim())))
            {
                MessageBox.Show("Folder must have a valid path", "Validation", MessageBoxButtons.OK);
                return false;
            }
            if (settings.IsS3BucketEnabled
                && string.IsNullOrEmpty(settings.AWSS3Bucket.Trim()))
            {
                MessageBox.Show("S3 Bucket Name must be valid", "Validation", MessageBoxButtons.OK);
                return false;
            }
            if (settings.IsGlacierEnabled
                && string.IsNullOrEmpty(settings.AWSGlacierVault.Trim()))
            {
                MessageBox.Show("Glacier Vault Name must be valid", "Validation", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private void SaveSettings()
        {
            //this.settings = SettingsBuilder.Create();

            settings.FileSystemDirectory = this.ArchivePath.Text;
            settings.IsFileSystemEnabled = this.IsFileSystem.Checked;

            settings.IsS3BucketEnabled = this.IsS3Bucket.Checked;
            settings.AWSS3Bucket = this.S3BucketName.Text;
            settings.AWSS3Region = (AWSRegionEndPoint)this.S3Region.SelectedItem;

            settings.IsGlacierEnabled = this.IsGlacier.Checked;
            settings.AWSGlacierVault = this.GlacierVaultName.Text;
            settings.AWSGlacierRegion = (AWSRegionEndPoint)this.GlacierRegion.SelectedItem;

            settings.AWSAccessKeyID = this._awsAccessKey;
            settings.AWSSecretAccessKey = this._awsSecret;

            SettingsManager.SaveSettings(this.settings);
        }

        private void ListBucketContents_Click(object sender, EventArgs e)
        {
            try
            {
                this.SaveSettings();
                var dlg = new BucketContentsForm
                {
                    Text = "S3 Bucket: " + this.settings.AWSS3Bucket
                };
                dlg.Show();
            }
            catch (Exception ex)
            {
                Program.DisplayError(ex);
            }
        }

        private void ListInventory_Click(object sender, EventArgs e)
        {
            try
            {
                this.SaveSettings();
                var dlg = new InventoryForm
                {
                    Text = "Vault: " + this.settings.AWSGlacierVault
                };
                dlg.ShowDialog();
            }
            catch (Exception ex)
            {
                Program.DisplayError(ex);
            }
        }

        private void AWSCredentials_Click(object sender, EventArgs e)
        {
            try
            {
                var dlg = new AWSCredentials(this._awsAccessKey);
                var result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this._awsAccessKey = dlg.AccessKey;
                    this._awsSecret = dlg.Secret;
                }
            }
            catch (Exception ex)
            {
                Program.DisplayError(ex);
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
