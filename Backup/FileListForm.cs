using Backup.Logic;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Backup
{
    public partial class FileListForm : Form
    {
        delegate void BoolArgReturningVoidDelegate(bool val);

        public FileListForm()
        {
            InitializeComponent();

            // Attach event handlers
            FileManager.BackupCompleted += FileManager_BackupCompleted;

            PopulateControls();
        }

        private void FileManager_BackupCompleted()
        {
            try
            {
                // InvokeRequired required compares the thread ID of the  
                // calling thread to the thread ID of the creating thread.  
                // If these threads are different, it returns true.  
                if (this.Backup.InvokeRequired)
                {
                    BoolArgReturningVoidDelegate d = new BoolArgReturningVoidDelegate(EnableBackup);
                    this.Invoke(d, true);
                }
                else
                {
                    this.Backup.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void EnableBackup(bool val)
        {
            this.Backup.Enabled = val;
        }

        private void PopulateControls()
        {
            this.FilesList.BeginUpdate();

            var sources = FileManager.GetSources();
            var files = FileManager.DiscoverFiles(sources);
            FileManager.SaveDiscoveredFiles(files);
            if (files == null || files.Count() == 0)
            {
                this.Message.Text = "";
                this.FilesList.Items.Add(new ListViewItem("No new or changed files"));
                this.FilesList.Enabled = false;
                this.Backup.Enabled = false;
                return;
            }
            foreach (var file in files)
            {
                this.FilesList.Items.Add(new ListViewItem(new string[] {file.FilePath, file.SubPath}));
            }

            this.FilesList.EndUpdate();
            this.Message.Text = $"{files.Count()} files have been created or changed since the last backup";
        }

        private async void Backup_Click(object sender, EventArgs e)
        {
            this.Backup.Enabled = false;
            await Task.Run(() => FileManager.InvokeBackupFromUser());
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FilesList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show((ListView)sender, e.X, e.Y);
            }
        }

        private void RemoveFromDiscoveredFiles_Click(object sender, EventArgs e)
        {
            DoRemove();
        }

        private void DoRemove()
        {
            ListView.SelectedIndexCollection selectedItems = this.FilesList.SelectedIndices;
            (from int s in selectedItems orderby s descending select s)
                .ToList()
                .ForEach(i =>
                    this.FilesList.Items.RemoveAt(i));

            FileManager.SaveDiscoveredFiles(
                (from ListViewItem item in FilesList.Items select new FileDetail(item.Text, item.SubItems[1].Text)).ToArray());
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            DoRemove();
        }

        private void FilesList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (this.FilesList.SelectedIndices.Count > 0)
                this.Remove.Enabled = true;
            else
                this.Remove.Enabled = false;
        }
    }
}
