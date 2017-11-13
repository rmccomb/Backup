using Backup.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Backup
{
    public partial class EditSource : Form
    {
        public EditSource()
        {
            InitializeComponent();
        }

        public EditSource(ListViewItem item) : this()
        {
            this.Directory.Text = item.SubItems[0].Text;
            this.Pattern.Text = item.SubItems[1].Text;
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            var result = this.folderBrowserDialog1.ShowDialog();
            if(result == DialogResult.OK)
                this.Directory.Text = this.folderBrowserDialog1.SelectedPath;
        }

        private void Commit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public (string, string) GetValues()
        {
            return (d: Directory.Text, p: Pattern.Text);
        }

    }
}
