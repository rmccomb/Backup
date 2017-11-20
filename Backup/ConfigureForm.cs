using Backup.Logic;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace Backup
{
    public partial class ConfigureForm : Form
    {
        //private List<Source> _sources;
        //private DestinationSettings _settings;
        //private string _awsSecretKey;
        FileList fileListDlg;
        DestinationForm destForm;

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

            SaveSources();
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

        private void SaveSources()
        {
            var toSave = new List<Source>();
            foreach (ListViewItem item in this.Sources.Items)
                toSave.Add(
                    new Source(item.SubItems[0].Text, item.SubItems[1].Text, item.SubItems[2].Text));

            FileManager.SaveSources(toSave);
        }

        //private void Browse_Click(object sender, EventArgs e)
        //{
        //    var result = this.folderBrowserDialog1.ShowDialog();
        //    if (result == DialogResult.OK)
        //        this.ArchivePath.Text = this.folderBrowserDialog1.SelectedPath;
        //}

        //private void IsFileSystem_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.IsFileSystem.Checked)
        //    {
        //        this.ArchivePath.Enabled = true;
        //        this.Browse.Enabled = true;
        //    }
        //    else
        //    {
        //        this.ArchivePath.Enabled = false;
        //        this.Browse.Enabled = false;
        //    }
        //}

        //private void IsS3Bucket_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.IsS3Bucket.Checked)
        //    {
        //        this.S3BucketName.Enabled = true;
        //        this.AWSProfileName.Enabled = true;
        //        this.AWSAccessKey.Enabled = true;
        //        this.AddSecret.Enabled = true;
        //    }
        //    else
        //    {
        //        this.S3BucketName.Enabled = false;
        //        this.AWSProfileName.Enabled = false;
        //        this.AWSAccessKey.Enabled = false;
        //        this.AddSecret.Enabled = false;
        //    }
        //}

        //private void AddSecret_Click(object sender, EventArgs e)
        //{
        //    var dlg = new EditSecret();
        //    var result = dlg.ShowDialog();
        //    if (result == DialogResult.OK)
        //    {
        //        this._awsSecretKey = dlg.Secret;
        //    }
        //    else
        //    {
        //        this._awsSecretKey = "";
        //    }
        //}

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

        private void BackupDestination_Click(object sender, EventArgs e)
        {
            if (this.destForm == null)
            {
                this.destForm = new DestinationForm();
                this.destForm.FormClosed += DestForm_FormClosed;
                this.destForm.ShowDialog();
            }
        }

        private void DestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.destForm = null;
        }
    }
}
