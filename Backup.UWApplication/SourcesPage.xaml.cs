using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Backup.UWApplication
{
    partial class SourcesPage : Page
    {
        private MainPage rootPage;

        public SourcesPage()
        {
            this.InitializeComponent();
            this.ViewModel = new ViewModels.SourcesViewModel();
            Add.Click += new RoutedEventHandler(Add_ClickAsync);
        }

        public ViewModels.SourcesViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            rootPage = MainPage.Current;

            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                // If we have pages in our in-app backstack and have opted in to showing back, do so
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if there are no pages in our in-app back stack
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        private async void Add_ClickAsync(object sender, RoutedEventArgs e)
        {
            // Clear previous returned folder name, if it exists, between iterations of this scenario
            rootPage.StatusText = "";

            var folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            //folderPicker.FileTypeFilter.Add(".docx");
            //folderPicker.FileTypeFilter.Add(".xlsx");
            //folderPicker.FileTypeFilter.Add(".pptx");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                // Application now has read/write access to all contents in the picked folder (including other sub-folder contents)
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                rootPage.StatusText = "Picked folder: " + folder.Path;
            }
            else
            {
                rootPage.StatusText = "Operation cancelled.";
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Delete");
        }
    }
}
