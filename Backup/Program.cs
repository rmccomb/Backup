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

        private static void Initialise()
        {
            try
            {
                var path = ConfigurationManager.AppSettings[FileManager.SettingsKey];
                if (string.IsNullOrEmpty(path))
                {
                    path = Directory.GetParent(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                    if (Environment.OSVersion.Version.Major >= 6)
                    {
                        path = Directory.GetParent(path).ToString();
                    }

                    path = Path.Combine(path, FileManager.SettingsFolder);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                FileManager.Initialise(path);
            }
            catch (Exception ex)
            {
                throw new Exception("There was a problem with the initialisation - check settings", ex);
            }
        }
    }
}
