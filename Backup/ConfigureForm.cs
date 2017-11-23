using Backup.Logic;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace Backup
{
    public partial class ConfigureForm : Form
    {
        FileListForm fileListForm;
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

        private void Discover_Click(object sender, EventArgs e)
        {
            if (this.fileListForm == null)
            {
                this.fileListForm = new FileListForm();
                this.fileListForm.FormClosed += FileList_FormClosed;
                this.fileListForm.ShowDialog();
            }
        }

        private void FileList_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.fileListForm = null;
            this.PopulateControls();
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
