using Backup.Logic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;

namespace Backup
{
    internal class Program
    {
        static CancellationTokenSource cts;
        public static event CloseHandler Close;
        public delegate void CloseHandler();

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

            // Setup and show the system tray icon, NB event hookups are here
            var processIcon = new ProcessIcon();
            processIcon.Display();

            var settings = FileManager.GetSettings();

            // Run backup on start if configured
            if (settings.CreateBackupOnStart)
                FileManager.InvokeBackup();

            // Update Glacier Inventory 
            Debug.WriteLine(FileManager.GetGlacierInventory(settings.IsGlacierEnabled));
            if (settings.IsGlacierEnabled)
            {
                try
                {
                    ProcessArchiveModelAsync();
                }
                catch (Exception ex)
                {
                    processIcon.CreateNotifyError("An error occurred processing a Glacier job. " + ex.Message);
                }
            }

            Application.Run();
        }

        private static Task ProcessArchiveModelAsync()
        {
            return Task.Run(() =>
            {
                // Complete any requested get archive jobs
                FileManager.ProcessArchiveModel();
                Thread.Sleep(1000 * 60 * 60);
            });
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
