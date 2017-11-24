using Backup.Logic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace Backup
{
    internal class Program
    {
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

            // Show the system tray icon.
            var processIcon = new ProcessIcon();
            processIcon.Display();

            // Run backup on start if configured
            if (FileManager.GetSettings().CreateBackupOnStart)
                FileManager.InvokeBackup();

            Application.Run();
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
    }
}
