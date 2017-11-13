using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backup.Properties;
using Backup.Logic;
using System.Configuration;

namespace Backup
{
    class ContextMenus
    {
        public ContextMenuStrip Create()
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            var item = new ToolStripMenuItem();
            item.Text = "Discover files";
            item.Click += new EventHandler(Discover_Click);
            item.Image = Resources.Discover;
            menu.Items.Add(item);

            var backup = new ToolStripMenuItem();
            backup.Text = "Backup now";
            backup.Click += new EventHandler(Backup_Click);
            backup.Image = Resources.Backup;
            menu.Items.Add(backup);

            var configure = new ToolStripMenuItem();
            configure.Text = "Configure";
            configure.Click += new EventHandler(Configure_Click);
            menu.Items.Add(configure);

            var restore = new ToolStripMenuItem();
            restore.Text = "Restore";
            restore.Click += new EventHandler(Restore_Click);
            menu.Items.Add(restore);

            var sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            var exit = new ToolStripMenuItem();
            exit.Text = "Exit";
            exit.Click += new System.EventHandler(Exit_Click);
            menu.Items.Add(exit);

            return menu;
        }

        private void Restore_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Configure_Click(object sender, EventArgs e)
        {
            var form = new ConfigureForm();
            form.ShowDialog();
        }

        private void Backup_Click(object sender, EventArgs e)
        {
            var tempDir = ConfigurationManager.AppSettings[FileManager.TempDirKey];
            var archive = ConfigurationManager.AppSettings[FileManager.ArchiveDirKey];
            FileManager.Process(tempDir, archive);
        }

        private void Discover_Click(object sender, EventArgs e)
        {
            var tempDir = ConfigurationManager.AppSettings[FileManager.TempDirKey];
            var sources = FileManager.GetSources(tempDir);
            var lastDate = FileManager.GetLastDate(tempDir);
            FileManager.DiscoverFiles(tempDir, sources, lastDate);


        }

        private void Exit_Click(object sender, EventArgs e)
        {
            // Quit without further ado.
            Application.Exit();
        }
    }
}
