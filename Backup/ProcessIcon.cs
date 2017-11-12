using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backup.Properties;

namespace Backup
{
    class ProcessIcon : IDisposable
    {
        NotifyIcon ni;

        public ProcessIcon()
        {
            ni = new NotifyIcon();
        }

        public void Display()
        {
            // Put the icon in the system tray and allow it react to mouse clicks.          
            ni.MouseClick += new MouseEventHandler(ni_MouseClick);
            ni.Icon = Resources.SystemTrayIcon;
            ni.Text = "Backup running";
            ni.Visible = true;

            // Attach a context menu.
            ni.ContextMenuStrip = new ContextMenus().Create();
        }

        public void Dispose()
        {
            ni.Dispose();
        }

        void ni_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                System.Diagnostics.Process.Start("explorer", null);
            }
        }
    }
}

