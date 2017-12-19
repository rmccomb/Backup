using Backup.Logic;
using System;
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

            this.Message.Text = "";
            this.Shown += InventoryForm_Shown;
        }

        private void InventoryForm_Shown(object sender, EventArgs e)
        {
            PopulateControls();
        }

        private async void Download_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.downloadPath))
                this.downloadPath = FileManager.GetTempDirectory();

            this.folderBrowserDialog1.SelectedPath = this.downloadPath;
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var archiveId = FilesList.SelectedItems[0].SubItems[2].Text;
                this.downloadPath = folderBrowserDialog1.SelectedPath;
                
                // Make the request
                var result = await Task.Run(() 
                    => FileManager.RequestGlacierArchive(archiveId, this.downloadPath));
                
                //CreateMessage(result);
                PopulateControls();
            }
        }

        private async void PopulateControls()
        {
            //var result = await Task.Run(() => FileManager.GetGlacierInventory());
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
                        item.GlacierJobStatus.ToString()}));
            }
            this.Message.Text = $"Inventory as at {inventory.InventoryDate}";
        }

        private void CreateMessage(GlacierResult result)
        {
            switch (result)
            {
                case GlacierResult.Completed:
                    this.Message.Text = "Glacier job completed";
                    break;
                case GlacierResult.Error:
                case GlacierResult.JobFailed:
                    this.Message.Text = "An error occurred";
                    break;
                case GlacierResult.DownloadRequested:
                    this.Message.Text = "A Glacier download job was requested";
                    break;
                case GlacierResult.InventoryRequested:
                    this.Message.Text = "A Glacier inventory job was requested";
                    break;
                case GlacierResult.Incomplete:
                    this.Message.Text = "Waiting for Glacier update";
                    break;
                default:
                    this.Message.Text = "";
                    break;
            }
        }

        private void CloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FilesList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // TODO check status of any existing job
            // 
            if (this.FilesList.SelectedItems.Count > 0)
                this.Download.Enabled = true;
            else
                this.Download.Enabled = false;
        }

    }
}
