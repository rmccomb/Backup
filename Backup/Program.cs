using Backup.Logic;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace Backup
{
    internal class Program
    {
        public const string ProgramName = "tiz.digital Backup";
        //public static CancellationTokenSource cts;
        //public static event CloseHandler Close;
        public delegate void CloseHandler();
        static Mutex _mut;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Want only one instance of the program running
            var exists = Mutex.TryOpenExisting(ProgramName, out _mut);
            if (exists)
            {
                Application.Exit();
                return;
            }
            else
                _mut = new Mutex(true, ProgramName);


            SystemEvents.SessionEnding += SystemEvents_SessionEnding;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Initialise();

            // Setup and show the system tray icon, NB event hookups are here
            var processIcon = new ProcessIcon();
            processIcon.Display();

            try
            {
                var settings = SettingsManager.GetSettings();

                // Run backup on start if configured
                if (settings.CreateBackupOnStart)
                    FileManager.InvokeBackup();

                // Update Glacier Inventory 
                Debug.WriteLine(FileManager.GetGlacierInventory(settings));
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
            }
            catch (Exception ex)
            {
                Program.DisplayError(ex);
            }

            Application.Run();
        }

        private static void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            // The user is logging off, do a backup if configured

            //if (DialogResult.Yes == MessageBox.Show(ProgramName, "Do you want to run a backup before logging off?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            if((e.Reason == SessionEndReasons.Logoff || e.Reason == SessionEndReasons.SystemShutdown)
                && SettingsManager.GetSettings().IsBackupOnLogoff)
            {
                FileManager.InvokeBackup();
                //e.Cancel = true;
            }
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

        //private static int WM_QUERYENDSESSION = 0x11;
        //private static bool systemShutdown = false;
        //protected override void WndProc(ref System.Windows.Forms.Message m)
        //{
        //    if (m.Msg == WM_QUERYENDSESSION)
        //    {
        //        MessageBox.Show("queryendsession: this is a logoff, shutdown, or reboot");
        //        systemShutdown = true;
        //    }

        //    // If this is WM_QUERYENDSESSION, the closing event should be  
        //    // raised in the base WndProc.  
        //    base.WndProc(ref m);

        //} //WndProc   

        public static void DisplayError(Exception ex)
        {
            MessageBox.Show(ex.Message, Program.ProgramName + " Error", MessageBoxButtons.OK);
        }
    }
}
