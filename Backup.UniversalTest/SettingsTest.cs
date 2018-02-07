
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.Storage;

namespace Backup.UniversalTest
{
    [TestClass]
    public class SettingsTest
    {
        public const string SettingsName = "backup.settings";
        public Settings _settings = new Settings
            {
                AWSAccessKeyID = "123",
                AWSGlacierVault = "test",
                AWSS3Bucket = "bucket",
                AWSSecretAccessKey = "abc",
                IsFileSystemEnabled = true,
                IsGlacierEnabled = false,
                IsS3BucketEnabled = true,
            };

        [TestMethod]
        public void SaveSettings()
        {
            _SaveSettings(_settings);
        }

        [TestMethod]
        public void GetSettings()
        {
            var saved = _GetSettings().Result;
            Assert.AreEqual(_settings.AWSSecretAccessKey, saved.AWSSecretAccessKey);
        }

        async void _SaveSettings(Settings settings)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            Debug.WriteLine(localFolder.Path);
            var formatter = new DataContractSerializer(typeof(Settings));
            StorageFile file = await localFolder.CreateFileAsync(SettingsName, CreationCollisionOption.ReplaceExisting);
            MemoryStream stream = new MemoryStream();
            formatter.WriteObject(stream, settings);
            await FileIO.WriteBytesAsync(file, stream.ToArray());
        }

        async Task<Settings> _GetSettings()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            Debug.WriteLine(localFolder.Path);
            var settingsFilePath = Path.Combine(localFolder.Path, SettingsName);
            if (!File.Exists(settingsFilePath))
                _SaveSettings(_settings);

            var formatter = new DataContractSerializer(typeof(Settings));
            StorageFile file = await localFolder.GetFileAsync(SettingsName);
            var stream = new FileStream(settingsFilePath, FileMode.Open, FileAccess.Read, FileShare.None);
            var settings = (Settings)formatter.ReadObject(stream);
            return settings;
        }

    }
}
