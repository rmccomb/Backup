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
    public partial class EditSecret : Form
    {
        public string Secret { get; set; }
        public EditSecret()
        {
            InitializeComponent();
        }

        private void Commit_Click(object sender, EventArgs e)
        {
            this.Secret = this.SecretID.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
