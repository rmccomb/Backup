using System;
using System.Windows.Forms;
using Backup.Properties;
using Backup.Logic;
using System.Diagnostics;

namespace Backup
{
    /// <summary>
    /// Encapsulates the System Tray Icon functionality
    /// </summary>
    internal class ProcessIcon : IDisposable
    {
        NotifyIcon notifyIcon;
        bool isNotifying;
        bool backupStarted;

        public ProcessIcon()
        {
            notifyIcon = new NotifyIcon();
        }

        ~ProcessIcon()
        {
            this.Dispose();
        }
        
        public void Display()
        {
            notifyIcon.Icon = Resources.Save;
            notifyIcon.Text = Program.ProgramName;
            notifyIcon.Visible = true;

            // Attach a context menu.
            notifyIcon.ContextMenuStrip = new ContextMenus().Create(notifyIcon);
            notifyIcon.Click += NotifyIcon_Click;

            // Attach event handlers
            FileManager.BackupSuccess += FileManager_BackupSuccess;
            FileManager.BackupWarning += FileManager_BackupWarning;
            FileManager.BackupError += FileManager_BackupError;
            FileManager.DownloadSuccess += FileManager_DownloadSuccess;
            FileManager.DownloadWarning += FileManager_DownloadWarning;
            FileManager.DownloadError += FileManager_DownloadError;
            FileManager.BackupStarted += FileManager_BackupStarted;
            FileManager.BackupCompleted += FileManager_BackupCompleted;
        }

        private void FileManager_BackupCompleted()
        {
            backupStarted = false;
        }

        private void FileManager_BackupStarted()
        {
            backupStarted = true;
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            var backup = notifyIcon.ContextMenuStrip.Items.Find("Backup", false)[0];
            backup.Enabled = !backupStarted;
            var exit = notifyIcon.ContextMenuStrip.Items.Find("Exit", false)[0];
            exit.Enabled = !backupStarted;
        }

        private void FileManager_DownloadWarning(string warningMessage)
        {
            CreateNotifyWarning(warningMessage);
        }
        private void FileManager_DownloadSuccess(string successMessage)
        {
            CreateNotifyInfo(successMessage);
        }
        private void FileManager_DownloadError(string errorMessage)
        {
            CreateNotifyError(errorMessage);
        }
        private void FileManager_BackupSuccess(string successMessage)
        {
            CreateNotifyInfo(successMessage);
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
            Debug.WriteLine("ProcessIcon.Dispose()");
            while (isNotifying) ; // Wait for notifications to finish
            notifyIcon.Dispose();
            notifyIcon = null;
        }

        internal void NotifyUser(string title, string message, ToolTipIcon icon)
        {
            try
            {
                isNotifying = true; // May fix issue with notifications loosing parent application identification in certain situations (logoff?)

                // WriteEventLog(message, icon); // TODO requires Admin privileges and installutil (cmd line) for Installers

                // Window Notification
                notifyIcon.ShowBalloonTip(3000, title, message, icon);

                isNotifying = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Program.DisplayError(ex);
            }
        }

        private static void WriteEventLog(string message, ToolTipIcon icon)
        {
            // Log to Event Log
            // Create the source, if it does not already exist.
            if (!EventLog.SourceExists(Program.ProgramName))
            {
                // TODO create source in EventLogInstaller
                EventLog.CreateEventSource(Program.ProgramName, null);
            }
            switch (icon)
            {
                case ToolTipIcon.Error:
                    EventLog.WriteEntry(Program.ProgramName, message, EventLogEntryType.Error);
                    break;
                case ToolTipIcon.Info:
                    EventLog.WriteEntry(Program.ProgramName, message, EventLogEntryType.Information);
                    break;
                case ToolTipIcon.Warning:
                default:
                    EventLog.WriteEntry(Program.ProgramName, message, EventLogEntryType.Warning);
                    break;
            }
        }
    }
}

