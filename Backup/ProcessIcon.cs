using System;
using System.Windows.Forms;
using Backup.Properties;
using Backup.Logic;

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

            // Attache event handlers
            FileManager.BackupSuccess += FileManager_BackupSuccess;
            FileManager.BackupWarning += FileManager_BackupWarning;
            FileManager.BackupError += FileManager_BackupError;
        }

        private void FileManager_BackupSuccess()
        {
            CreateNotifyInfo("The backup completed successfully");
        }
        private void FileManager_BackupWarning(string warningMessage)
        {
            CreateNotifyWarning(warningMessage);
        }
        private void FileManager_BackupError(string errorMessage)
        {
            CreateNotifyError(errorMessage);
        }
        public void CreateNotifyInfo(string message)
        {
            NotifyUser("Information", message, ToolTipIcon.Info);
        }
        public void CreateNotifyWarning(string message)
        {
            NotifyUser("Warning", message, ToolTipIcon.Warning);
        }
        public void CreateNotifyError(string message)
        {
            NotifyUser("Error", message, ToolTipIcon.Error);
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

