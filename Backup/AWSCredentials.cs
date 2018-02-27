using System;
using System.Windows.Forms;
using Backup.Logic;

namespace Backup
{
    public partial class AWSCredentials : Form
    {
        public string AccessKey { get; set; }
        public string Secret { get; set; }

        public AWSCredentials(string awsAccessKey)
        {
            InitializeComponent();
            this.AWSAccessKey.Text = awsAccessKey;
        }

        private void AddAWSSecret_Click(object sender, EventArgs e)
        {
            try
            {
                var dlg = new EditSecret();
                var result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.Secret = dlg.Secret;
                }
            }
            catch (Exception ex)
            {
                Program.DisplayError(ex);
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
