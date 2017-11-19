using System;
using System.Windows.Forms;
using Backup.Properties;

namespace Backup
{
    /// <summary>
    /// Encapsulates the System Tray Icon functionality
    /// </summary>
    internal class ProcessIcon : IDisposable
    {
        NotifyIcon notifyIcon;

        public ProcessIcon()
        {
            notifyIcon = new NotifyIcon();
        }

        public void Display()
        {
            // Put the icon in the system tray and allow it react to mouse clicks.          
            notifyIcon.MouseClick += new MouseEventHandler(NotifyIcon_MouseClick);
            notifyIcon.Icon = Resources.Save;
            notifyIcon.Text = "Backup Utility";
            notifyIcon.Visible = true;

            // Attach a context menu.
            notifyIcon.ContextMenuStrip = new ContextMenus().Create();
        }

        public void Dispose()
        {
            notifyIcon.Dispose();
        }

        internal void NotifyUser(string title, string message, ToolTipIcon icon)
        {
            notifyIcon.ShowBalloonTip(5000, title, message, icon);
        }

        void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    System.Diagnostics.Process.Start("explorer", null);
            //}
        }
    }
}

