using Backup.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Backup
{
    public partial class BucketContentsForm : Form
    {
        string downloadPath;
        public BucketContentsForm()
        {
            InitializeComponent();

            Message.Text = "Getting contents...";
            Shown += ArchiveContentsForm_Shown;
        }

        private async void ArchiveContentsForm_Shown(object sender, EventArgs e)
        {
            await PopulateControlsAsync();
        }

        private async Task PopulateControlsAsync()
        {
            try
            {
                //var files = await Task.Run(() => FileManager.GetBucketContents());
                var files = await FileManager.GetBucketContentsAsync();
                if (files == null || files.Count() == 0)
                {
                    Message.Text = "";
                    FilesList.Items.Add(new ListViewItem("No new or changed files"));
                    FilesList.Enabled = false;
                    Download.Enabled = false;
                    return;
                }
                foreach (var file in files)
                {
                    FilesList.Items.Add(new ListViewItem(new string[] { file }));
                }

                Message.Text = $"{files.Count()} files have been found";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private async void Download_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(this.downloadPath))
                    this.downloadPath = FileManager.GetTempDirectory();

                this.folderBrowserDialog1.SelectedPath = this.downloadPath;
                var result = this.folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.downloadPath = this.folderBrowserDialog1.SelectedPath;
                    await FileManager.DownloadS3ObjectAsync(this.downloadPath, this.FilesList.SelectedItems[0].Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void CloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FilesList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (this.FilesList.SelectedItems.Count > 0)
                this.Download.Enabled = true;
            else
                this.Download.Enabled = false;
        }

        private void FilesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("FilesList_SelectedIndexChanged");
        }
    }
}
