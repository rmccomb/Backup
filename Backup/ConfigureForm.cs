using Backup.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Backup
{
    public partial class ConfigureForm : Form
    {
        private List<Source> sources;

        public ConfigureForm()
        {
            InitializeComponent();
            var tempDir = ConfigurationManager.AppSettings[FileManager.TempDirKey];
            this.sources = FileManager.GetSources(tempDir);
            this.PopulateControls();
        }

        public void PopulateControls()
        {
            this.uiSources.BeginUpdate();
            foreach (var source in this.sources)
            {
                this.uiSources.Items.Add(new ListViewItem(new string[] { source.Directory, source.Pattern }));
            }
            this.uiSources.EndUpdate();
        }

        private void New_Click(object sender, EventArgs e)
        {
            var editSource = new EditSource();
            var result = editSource.ShowDialog();
            if (result == DialogResult.OK)
            {
                var vals = editSource.GetValues();
                this.uiSources.Items.Add(new ListViewItem(new string[] { vals.Item1, vals.Item2 }));
            }
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            var item = this.uiSources.SelectedItems[0];
            var result = new EditSource(item).ShowDialog();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                
            }
        }

        private void uiSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uiSources.SelectedItems != null)
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

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Commit_Click(object sender, EventArgs e)
        {

        }
    }
}
