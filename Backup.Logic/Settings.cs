using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Backup.Logic
{
    [Serializable]
    public class Settings
    {
        public string FileSystemDirectory { get; set; }
        public bool IsFileSystemEnabled { get; set; }
        public string AWSS3Bucket { get; set; }
        public bool IsS3BucketEnabled { get; set; }

        public bool IsGlacierEnabled { get; set; }
        public string AWSGlacierVault { get; set; }

        public string AWSAccessKeyID { get; set; }
        public string AWSSecretAccessKey { get; set; }

        public bool CreateBackupOnStart { get; set; }
        public AWSRegionEndPoint AWSS3Region { get; set; }
        public AWSRegionEndPoint AWSGlacierRegion { get; set; }

        public string SMSContact { get; set; }
        public bool IsSMSContactEnabled { get; set; }

        public DateTime InventoryUpdateRequested { get; set; }

        public bool IsBackupOnLogoff { get; set; }

        public bool IsLaunchOnLogon { get; set; }

    }

    public static class SettingsManager
    {
        private static Settings Create()
        {
            return new Settings
            {
                FileSystemDirectory = Path.Combine(FileManager.GetTempDirectory(), FileManager.ArchiveFolder)
            };
        }

        public static void SaveSettings(Settings settings)
        {
            Debug.WriteLine("SaveSettings");

            var formatter = new BinaryFormatter();
            var settingsFileName = Path.Combine(FileManager.GetTempDirectory(), FileManager.SettingsName);
            File.Delete(settingsFileName);
            var stream = new FileStream(settingsFileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, settings);
            stream.Close();
        }

        public static Settings GetSettings()
        {
            var formatter = new BinaryFormatter();
            var settingsFileName = Path.Combine(FileManager.GetTempDirectory(), FileManager.SettingsName);

            if (!File.Exists(settingsFileName))
                SaveSettings(Create());

            var stream = new FileStream(settingsFileName, FileMode.Open, FileAccess.Read, FileShare.None);
            var settings = (Settings)formatter.Deserialize(stream);
            stream.Close();
            return settings;
        }
    }
}
