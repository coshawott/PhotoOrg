using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
namespace PhotoOrg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Photo> Photos { get; set; }
        private int PageSize = 50;
        private int currentPageIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Photos = new ObservableCollection<Photo>();
            Console.WriteLine(Properties.Settings.Default.FolderLocation);
            if (Properties.Settings.Default.FolderLocation != "")
            {
                Folder_Text.Text = Properties.Settings.Default.FolderLocation;
                Photos = new ObservableCollection<Photo>();
                List<string> photos = GetPhotos(Properties.Settings.Default.FolderLocation);
            }
            else
            {
                Folder_Text.Text = "Please select a folder";
            }

        }

        private void Select_Folder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Select a Folder"
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Properties.Settings.Default.FolderLocation = dialog.FileName;
                Folder_Text.Text = Properties.Settings.Default.FolderLocation;
                List<string> photos = GetPhotos(Properties.Settings.Default.FolderLocation);
                InitThumbnails(photos);
                currentPageIndex = 0;
                Page_Number.Text = (currentPageIndex + 1 + "/" + ((photos.Count / PageSize) + 1));
                //foreach(string photo in photos)
                //{
                //Debug.WriteLine(photo);
                //}
            }
        }

        private List<string> GetPhotos(string FolderPath)
        {
            List<string> PhotoPath = new List<string>();
            if (Directory.Exists(FolderPath))
            {
                string[] extension = { ".jpg", ".jpeg", ".png", ".gif" };
                foreach (string ext in extension)
                {
                    string[] files = Directory.GetFiles(FolderPath, "*" + ext);
                    foreach (string file in files)
                    {
                        PhotoPath.Add(file);
                    }
                }
            }
            
            return PhotoPath;
        }

        private void InitThumbnails(List<string> PhotoPaths)
        {
            int startIndex = currentPageIndex * PageSize;
            int endIndex = Math.Min(startIndex + PageSize, PhotoPaths.Count);
            Photos.Clear();
            for (int i = startIndex; i < endIndex; i++)
            {
                Photos.Add(new Photo { Path = PhotoPaths[i] });
            }
        }
        

        public class Photo
        {
            public string Path { get; set; }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            List<string> photos = GetPhotos(Properties.Settings.Default.FolderLocation);
            
            if (currentPageIndex < photos.Count/PageSize)
            {
                currentPageIndex++;
                InitThumbnails(photos);
            }
            Page_Number.Text = (currentPageIndex + 1  + "/" + ((photos.Count / PageSize) + 1) );
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            List<string> photos = GetPhotos(Properties.Settings.Default.FolderLocation);
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                InitThumbnails(photos);
            }
            Page_Number.Text = (currentPageIndex + 1 + "/" + ((photos.Count / PageSize) + 1) );
        }

        private void Search_Menu_Click(object sender, RoutedEventArgs e)
        {
            DataEntryWindow dataEntryWindow = new DataEntryWindow();
            dataEntryWindow.ShowDialog();
        }
    }
}
