using Backup.Logic;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
                {
                    SaveSettings();
                    this.Close();
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

            ArchivePath.Text = settings.FileSystemDirectory;
            IsFileSystem.Checked = settings.IsFileSystemEnabled;

            S3BucketName.Text = settings.AWSS3Bucket;
            IsS3Bucket.Checked = settings.IsS3BucketEnabled;
            S3Region.SelectedValue = settings.AWSS3Region == null ? settings.AWSS3Region.SystemName : AWSHelper.GetDefaultRegionSystemName();

            GlacierVaultName.Text = settings.AWSGlacierVault;
            IsGlacier.Checked = settings.IsGlacierEnabled;
            GlacierRegion.SelectedValue = settings.AWSGlacierRegion == null ?settings.AWSGlacierRegion.SystemName : AWSHelper.GetDefaultRegionSystemName();

            AWSAccessKey.Text = settings.AWSAccessKeyID;
            _awsSecretKey = settings.AWSSecretAccessKey;
            SMSContact.Text = settings.SMSContact;
            IsSMSContact.Checked = settings.IsSMSContactEnabled;

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
                this.S3Region.Enabled = true;
                this.AWSAccessKey.Enabled = true;
                this.AddAWSSecret.Enabled = true;
            }
            else
            {
                this.S3BucketName.Enabled = false;
                this.S3Region.Enabled = false;
            }

            if (!this.IsS3Bucket.Checked && !this.IsGlacier.Checked)
            {
                this.AWSAccessKey.Enabled = false;
                this.AddAWSSecret.Enabled = false;
            }
        }

        private void IsGlacier_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsGlacier.Checked)
            {
                this.GlacierVaultName.Enabled = true;
                this.GlacierRegion.Enabled = true;
                this.AWSAccessKey.Enabled = true;
                this.AddAWSSecret.Enabled = true;
            }
            else
            {
                this.GlacierVaultName.Enabled = false;
                this.GlacierRegion.Enabled = false;
            }
            
            if(!this.IsGlacier.Checked && !this.IsS3Bucket.Checked)
            {
                this.AWSAccessKey.Enabled = false;
                this.AddAWSSecret.Enabled = false;
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
                settings.AWSS3Bucket != this.S3BucketName.Text ||
                settings.IsFileSystemEnabled != this.IsFileSystem.Checked ||
                settings.IsS3BucketEnabled != this.IsS3Bucket.Checked ||
                settings.CreateBackupOnStart != this.CreateOnStart.Checked ||
                settings.IsGlacierEnabled != this.IsGlacier.Checked ||
                settings.AWSGlacierVault != this.GlacierVaultName.Text ||
                settings.AWSS3Region != (AWSRegionEndPoint)this.S3Region.SelectedItem ||
                settings.AWSGlacierRegion != (AWSRegionEndPoint)this.GlacierRegion.SelectedItem ||
                settings.SMSContact != this.SMSContact.Text ||
                settings.IsSMSContactEnabled != this.IsSMSContact.Checked)
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
            this.settings = new DestinationSettings
            {
                FileSystemDirectory = this.ArchivePath.Text,
                IsFileSystemEnabled = this.IsFileSystem.Checked,
                AWSAccessKeyID = this.AWSAccessKey.Text,
                AWSSecretAccessKey = this._awsSecretKey,
                AWSS3Bucket = this.S3BucketName.Text,
                IsS3BucketEnabled = this.IsS3Bucket.Checked,
                AWSS3Region = (AWSRegionEndPoint)this.S3Region.SelectedItem,
                CreateBackupOnStart = this.CreateOnStart.Checked,
                IsGlacierEnabled = this.IsGlacier.Checked,
                AWSGlacierVault = this.GlacierVaultName.Text,
                AWSGlacierRegion = (AWSRegionEndPoint)this.GlacierRegion.SelectedItem,
                SMSContact = this.SMSContact.Text,
                IsSMSContactEnabled = this.IsSMSContact.Checked
            };
            FileManager.SaveSettings(this.settings);
            PopulateControls();
        }

        private void ListBucketContents_Click(object sender, EventArgs e)
        {
            this.SaveSettings();
            var dlg = new FileListForm
            {
                Text = this.settings.AWSS3Bucket
            };
            dlg.Show();
        }

        private void ListInventory_Click(object sender, EventArgs e)
        {
            this.SaveSettings();
            var dlg = new InventoryForm
            {
                Text = this.settings.AWSGlacierVault
            };
            dlg.ShowDialog();
        }
    }
}
