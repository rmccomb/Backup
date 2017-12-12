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
    public partial class InventoryForm : Form
    {
        string downloadPath;
        public InventoryForm()
        {
            InitializeComponent();

            this.Message.Text = "Getting contents...";
            this.Shown += InventoryForm_Shown;
        }

        private async void InventoryForm_Shown(object sender, EventArgs e)
        {
            await PopulateControlsAsync();
        }

        private async Task PopulateControlsAsync()
        {
            var result = await Task.Run(() => FileManager.GetInventory());
            if (result.Result != "OK")
            {
                this.FilesList.Items.Add(new ListViewItem("No items found"));
                this.FilesList.Enabled = false;
                this.Download.Enabled = false;
                this.Message.Text = result.Result;
                return;
            }
            foreach (var archive in result.Inventory.ArchiveList)
            {
                this.FilesList.Items.Add(new ListViewItem(new string[] { archive.ArchiveDescription, archive.Size.ToString() }));
            }
            this.Message.Text = $"Inventory as at {result.Inventory.InventoryDate}";
        }

        private async void Download_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(this.downloadPath))
                this.downloadPath = FileManager.GetTempDirectory();

            this.folderBrowserDialog1.SelectedPath = this.downloadPath;
            var result = this.folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.downloadPath = this.folderBrowserDialog1.SelectedPath;
                await FileManager.DownloadS3ObjectAsync(this.downloadPath, this.FilesList.SelectedItems[0].Text);
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

        private void Refresh_Click(object sender, EventArgs e)
        {

        }
    }
}
