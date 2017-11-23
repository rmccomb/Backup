using Backup.Logic;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Backup
{
    public partial class FileList : Form
    {
        public FileList()
        {
            InitializeComponent();

            PopulateControls();
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
            this.Message.Text = $"{files.Count()} files have been created or changed";
        }

        private void Backup_Click(object sender, EventArgs e)
        {
            FileManager.InvokeBackup();
            this.Close();
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
            ListView.SelectedIndexCollection selectedItems = this.FilesList.SelectedIndices;
            (from int s in selectedItems orderby s descending select s)
                .ToList()
                .ForEach(i =>
                    this.FilesList.Items.RemoveAt(i));

            FileManager.SaveDiscoveredFiles(
                (from ListViewItem item in this.FilesList.Items select new FileDetail(item.Text, item.SubItems[1].Text)).ToArray());
        }
    }
}
