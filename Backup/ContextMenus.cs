using System;
using System.Windows.Forms;
using Backup.Properties;
using Backup.Logic;
using System.Configuration;
using System.Threading;

namespace Backup
{
    /// <summary>
    /// Encapsulates menu functionality include dialog management
    /// </summary>
    public class ContextMenus
    {
        ConfigureForm configForm;
        DestinationForm destinationForm;
        FileListForm fileListForm;
        NotifyIcon icon;
        CancellationTokenSource cts;

        public ContextMenuStrip Create(NotifyIcon icon)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            this.icon = icon;
            cts = new CancellationTokenSource();
            cts.Token.Register(() => FileManager.Cancel());

            var exit = new ToolStripMenuItem();
            exit.Text = "Exit";
            exit.Click += new EventHandler(Exit_Click);
            menu.Items.Add(exit);

            var sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            var configure = new ToolStripMenuItem();
            configure.Text = "Configure...";
            configure.Click += new EventHandler(Configure_Click);
            menu.Items.Add(configure);

            var item = new ToolStripMenuItem();
            item.Text = "Discover files...";
            item.Click += new EventHandler(Discover_Click);
            item.Image = Resources.StartPoint_16x;
            menu.Items.Add(item);

            var backup = new ToolStripMenuItem();
            backup.Text = "Backup now";
            backup.Click += new EventHandler(Backup_Click);
            backup.Image = Resources.Open_16x;
            menu.Items.Add(backup);
            
            return menu;
        }

        private void Archive_Click(object sender, EventArgs e)
        {
            if (this.destinationForm == null)
            {
                this.destinationForm = new DestinationForm();
                this.destinationForm.FormClosed += Destination_FormClosed;
                this.destinationForm.ShowDialog();
            }
        }

        private void Destination_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.destinationForm = null;
        }

        private void Configure_Click(object sender, EventArgs e)
        {
            if (this.configForm == null)
            {
                this.configForm = new ConfigureForm();
                this.configForm.FormClosed += Config_FormClosed;
                this.configForm.ShowDialog();
            }
        }

        private void Config_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.configForm = null;
        }

        private void Backup_Click(object sender, EventArgs e)
        {
            try
            {
                FileManager.InvokeBackup();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An Error Occurred");
            }
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
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.cts.Cancel();
            this.cts.Dispose();
            this.icon.Visible = false;
            Application.Exit();
        }
    }
}
