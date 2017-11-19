using System;
using System.Windows.Forms;
using Backup.Properties;
using Backup.Logic;
using System.Configuration;

namespace Backup
{
    /// <summary>
    /// Encapsulates menu functionality include dialog management
    /// </summary>
    public class ContextMenus
    {
        ConfigureForm config;
        FileList fileList;

        public ContextMenuStrip Create()
        {
            ContextMenuStrip menu = new ContextMenuStrip();

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

            var configure = new ToolStripMenuItem();
            configure.Text = "Configure...";
            configure.Click += new EventHandler(Configure_Click);
            configure.Image = Resources.save_16xMD;
            menu.Items.Add(configure);

            var sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            var exit = new ToolStripMenuItem();
            exit.Text = "Exit";
            exit.Click += new EventHandler(Exit_Click);
            menu.Items.Add(exit);

            return menu;
        }

        private void Configure_Click(object sender, EventArgs e)
        {
            if (this.config == null)
            {
                this.config = new ConfigureForm();
                this.config.FormClosed += Config_FormClosed;
                this.config.ShowDialog();
            }
        }

        private void Config_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.config = null;
        }

        private void Backup_Click(object sender, EventArgs e)
        {
            try
            {
                FileManager.ProcessBackup();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An Error Occurred");
            }
        }

        private void Discover_Click(object sender, EventArgs e)
        {
            if (this.fileList == null)
            {
                this.fileList = new FileList();
                this.fileList.FormClosed += FileList_FormClosed;
                this.fileList.ShowDialog();
            }
        }

        private void FileList_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.fileList = null;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
