using System;
using System.Windows.Forms;
using Backup.Properties;
using Backup.Logic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

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

        public ContextMenuStrip Create(NotifyIcon icon)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            this.icon = icon;

            //Program.cts.Token.Register(() => FileManager.Cancel());
            //Program.cts.Token.Register(() => Thread.Sleep(20_000));

            var exit = new ToolStripMenuItem();
            exit.Name = "Exit";
            exit.Text = "Exit";
            exit.Click += new EventHandler(Exit_Click);
            menu.Items.Add(exit);

            var sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            var configure = new ToolStripMenuItem();
            configure.Name = "Open";
            configure.Text = "Open...";
            configure.Click += new EventHandler(Configure_Click);
            menu.Items.Add(configure);

            var discover = new ToolStripMenuItem();
            discover.Name = "Discover";
            discover.Text = "Discover files...";
            discover.Click += new EventHandler(Discover_Click);
            discover.Image = Resources.StartPoint_16x;
            menu.Items.Add(discover);

            var backup = new ToolStripMenuItem();
            backup.Name = "Backup";
            backup.Text = "Backup now";
            backup.Click += new EventHandler(Backup_Click);
            backup.Image = Resources.Open_16x;
            menu.Items.Add(backup);
                        
            return menu;
        }

        private void Archive_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.destinationForm == null)
                {
                    this.destinationForm = new DestinationForm();
                    this.destinationForm.FormClosed += Destination_FormClosed;
                    this.destinationForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Program.DisplayError(ex);
            }
        }

        private void Destination_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.destinationForm = null;
        }

        private void Configure_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.configForm == null)
                {
                    this.configForm = new ConfigureForm();
                    this.configForm.FormClosed += Config_FormClosed;
                    this.configForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Program.DisplayError(ex);
            }
        }

        private void Config_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.configForm = null;
        }

        private async void Backup_Click(object sender, EventArgs e)
        {
            try
            {
                await Task.Run(() => FileManager.InvokeBackup());
            }
            catch (Exception ex)
            {
                Program.DisplayError(ex);
            }
        }

        private void Discover_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.fileListForm == null)
                {
                    this.fileListForm = new FileListForm();
                    this.fileListForm.FormClosed += FileList_FormClosed;
                    this.fileListForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Program.DisplayError(ex);
            }
        }

        private void FileList_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.fileListForm = null;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.icon.Visible = false;
            Application.Exit();
        }
    }
}
