using Backup.Logic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Backup
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Initialise();

            // Show the system tray icon.
            using (ProcessIcon pi = new ProcessIcon())
            {
                pi.Display();
                Application.Run();
            }
        }

        /// <summary>
        /// Create the folder for temp files
        /// </summary>
        private static void Initialise()
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
