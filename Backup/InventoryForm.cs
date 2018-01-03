using Backup.Logic;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Backup
{
    public partial class InventoryForm : Form
    {
        string downloadDirectory;
        public InventoryForm()
        {
            InitializeComponent();

            this.Message.Text = "";
            this.Shown += InventoryForm_Shown;
        }

        private void InventoryForm_Shown(object sender, EventArgs e)
        {
            PopulateControls();
        }

        private async void Download_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.downloadDirectory))
                this.downloadDirectory = FileManager.GetTempDirectory();

            this.folderBrowserDialog1.SelectedPath = this.downloadDirectory;
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var description = FilesList.SelectedItems[0].SubItems[0].Text;
                var archiveId = FilesList.SelectedItems[0].SubItems[2].Text;
                this.downloadDirectory = folderBrowserDialog1.SelectedPath;
                
                // Make the request
                var result = await Task.Run(() 
                    => FileManager.RequestGlacierArchive(archiveId, this.downloadDirectory, description));
                
                PopulateControls();
            }
        }

        private async void PopulateControls()
        {
            this.FilesList.Items.Clear();
            var inventory = await Task.Run(() => FileManager.GetArchiveModel());
            if (inventory == null)
            {
                this.FilesList.Items.Add(new ListViewItem("No items found"));
                this.FilesList.Enabled = false;
                this.Download.Enabled = false;
                this.Message.Text = "No Inventory file found";
                return;
            }
            foreach (var item in inventory)
            {
                this.FilesList.Items.Add(
                    new ListViewItem(new string[] {
                        item.Description,
                        item.Size.ToString(),
                        item.ArchiveId,
                        CreateMessage(item.GlacierJobStatus)
                    }));
            }
            this.Message.Text = $"Inventory as at {inventory.InventoryDate}";
            this.Download.Enabled = false;
        }

        private string CreateMessage(GlacierResult result)
        {
            switch (result)
            {
                case GlacierResult.Completed:
                    return "Completed";
                case GlacierResult.Error:
                case GlacierResult.JobFailed:
                    return "Failed";
                case GlacierResult.JobRequested:
                case GlacierResult.Incomplete:
                    return "Requested";
                default:
                    return "Archived";
            }
        }

        private void CloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FilesList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (this.FilesList.SelectedItems.Count > 0)
                if (this.FilesList.SelectedItems[0].SubItems[3].Text == "Archived")
                    this.Download.Enabled = true;
            else
                this.Download.Enabled = false;
        }

    }
}
