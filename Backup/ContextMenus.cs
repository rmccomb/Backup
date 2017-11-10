using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backup.Properties;

namespace Backup
{
    class ContextMenus
    {
        bool isAboutLoaded = false;

        public ContextMenuStrip Create()
        {
            // Add the default menu options.
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator sep;

            // Windows Explorer.
            item = new ToolStripMenuItem();
            item.Text = "Explorer";
            item.Click += new EventHandler(Explorer_Click);
            item.Image = Resources.Explorer;
            menu.Items.Add(item);

            // About.
            item = new ToolStripMenuItem();
            item.Text = "About";
            item.Click += new EventHandler(About_Click);
            item.Image = Resources.About;
            menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Exit.
            item = new ToolStripMenuItem();
            item.Text = "Exit";
            item.Click += new System.EventHandler(Exit_Click);
            item.Image = Resources.Exit;
            menu.Items.Add(item);

            return menu;
        }

        void Explorer_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", null);
        }

        void About_Click(object sender, EventArgs e)
        {
            if (!isAboutLoaded)
            {
                isAboutLoaded = true;
                new About().ShowDialog();
                isAboutLoaded = false;
            }
        }

        void Exit_Click(object sender, EventArgs e)
        {
            // Quit without further ado.
            Application.Exit();
        }
    }
}
