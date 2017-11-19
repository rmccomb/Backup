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
        //private List<Source> _sources;
        //private DestinationSettings _settings;
        private string _awsSecretKey;
        FileList fileListDlg;

        public ConfigureForm()
        {
            InitializeComponent();
            
            PopulateControls();
        }
        
        public void PopulateControls()
        {
            Sources.BeginUpdate();
            Sources.Items.Clear();
            var sources = FileManager.GetSources();
            foreach (var source in sources)
            {
                Sources.Items.Add(new ListViewItem(new string[] { source.Directory, source.Pattern, source.LastBackupText }));
            }
            Sources.EndUpdate();

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

        private void New_Click(object sender, EventArgs e)
        {
            var editSource = new EditSource();
            var result = editSource.ShowDialog();
            if (result == DialogResult.OK)
            {
                var vals = editSource.GetValues();
                Sources.Items.Add(new ListViewItem(new string[] { vals.Item1, vals.Item2, Source.NeverText }));
            }
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            var item = this.Sources.SelectedItems[0];
            var result = new EditSource(item).ShowDialog();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Sources.SelectedItems.Count > 0)
                {
                    var confirmResult = MessageBox.Show("Are you sure to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        //this._sources.RemoveAt(this.Sources.SelectedIndices[0]);
                        //Sources.Items.RemoveAt(this.Sources.SelectedIndices[0]);
                        ListView.SelectedIndexCollection selectedItems = Sources.SelectedIndices;
                        (from int s in selectedItems orderby s descending select s)
                            .ToList()
                            .ForEach(i => Sources.Items.RemoveAt(i));

                        SaveSources();
                        PopulateControls();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An Error Occurred");
            }
        }

        private void Sources_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Sources.SelectedItems != null && Sources.SelectedItems.Count > 0)
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
                SaveSources();
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

        private void SaveSources()
        {
            var toSave = new List<Source>();
            foreach (ListViewItem item in this.Sources.Items)
                toSave.Add(
                    new Source(item.SubItems[0].Text, item.SubItems[1].Text, item.SubItems[2].Text));

            FileManager.SaveSources(toSave);
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
            if (this.fileListDlg == null)
            {
                this.fileListDlg = new FileList();
                this.fileListDlg.FormClosed += FileList_FormClosed;
                this.fileListDlg.ShowDialog();
            }
        }

        private void FileList_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.fileListDlg = null;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
