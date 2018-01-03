using System;
using System.Windows.Forms;
using Backup.Logic;

namespace Backup
{
    public partial class AWSCredentials : Form
    {
        public string AccessKey { get; set; }
        public string Secret { get; set; }

        public AWSCredentials()
        {
            InitializeComponent();
            this.AWSAccessKey.Text = this.AccessKey;
        }

        private void AddAWSSecret_Click(object sender, EventArgs e)
        {
            var dlg = new EditSecret();
            var result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.Secret = dlg.Secret;
            }
        }

        private void Commit_Click(object sender, EventArgs e)
        {
            this.AccessKey = this.AWSAccessKey.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
