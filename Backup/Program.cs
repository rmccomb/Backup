using Backup.Logic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace Backup
{
    internal class Program : IDisposable
    {
        static ProcessIcon processIcon;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Initialise();

            FileManager.BackupSuccess += FileManager_BackupSuccess;
            FileManager.BackupWarning += FileManager_BackupWarning;
            FileManager.BackupError += FileManager_BackupError;

            // Show the system tray icon.
            processIcon = new ProcessIcon();
            processIcon.Display();
            Application.Run();
        }

        ~Program()
        {
            Dispose();
        }

        private static void FileManager_BackupSuccess()
        {
            CreateNotifyInfo("The backup completed successfully");
        }
        private static void FileManager_BackupWarning(string warningMessage)
        {
            CreateNotifyWarning(warningMessage);
        }
        private static void FileManager_BackupError(string errorMessage)
        {
            CreateNotifyError(errorMessage);
        }
        static public void CreateNotifyInfo(string message)
        {
            processIcon.NotifyUser("Information", message, ToolTipIcon.Info);
        }
        static public void CreateNotifyWarning(string message)
        {
            processIcon.NotifyUser("Warning", message, ToolTipIcon.Warning);
        }
        static public void CreateNotifyError(string message)
        {
            processIcon.NotifyUser("Error", message, ToolTipIcon.Error);
        }

        /// <summary>
        /// Create the folder for temp files
        /// </summary>
        static void Initialise()
        {
            try
            {
                var tempDir = ConfigurationManager.AppSettings[FileManager.TempDirKey];
                if (string.IsNullOrEmpty(tempDir))
                {
                    tempDir = Directory.GetParent(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                    if (Environment.OSVersion.Version.Major >= 6)
                    {
                        tempDir = Directory.GetParent(tempDir).ToString();
                    }

                    tempDir = Path.Combine(tempDir, FileManager.SettingsFolder);
                    if (!Directory.Exists(tempDir))
                        Directory.CreateDirectory(tempDir);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("There was a problem with the initialisation - check settings", ex);
            }
        }

        public void Dispose()
        {
            if (processIcon != null)
                processIcon.Dispose();
        }
    }
}
