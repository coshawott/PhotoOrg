using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace PhotoOrg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        public ObservableCollection<Photo> Photos { get; set; }
        private int PageSize = 50;
        public int currentPageIndex = 0;
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose of any managed resources here
                    Photos.Clear();
                }

                // Dispose of any unmanaged resources here

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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

        private void InitThumbnails(List<string> photoPaths)
        {
            int startIndex = currentPageIndex * PageSize;
            int endIndex = Math.Min(startIndex + PageSize, photoPaths.Count);
            Photos.Clear();

            for (int i = startIndex; i < endIndex; i++)
            {
                string path = photoPaths[i];

                // Load the image using System.Drawing.Image
                using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
                {
                    // Generate a thumbnail of size 100x100 pixels
                    System.Drawing.Image thumbnail = image.GetThumbnailImage(100, 100, null, IntPtr.Zero);

                    // Create a WPF Image control to display the thumbnail
                    System.Windows.Controls.Image wpfImage = new System.Windows.Controls.Image();
                    wpfImage.Source = ConvertToBitmapImage(thumbnail);

                    // Add the photo to your collection
                    Photos.Add(new Photo(path) { Thumbnail = wpfImage });
                }
            }
        }


        private BitmapImage ConvertToBitmapImage(System.Drawing.Image image)
        {
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(memoryStream.ToArray());
            bitmapImage.EndInit();
            memoryStream.Dispose();

            return bitmapImage;
        }


        public class Photo : IDisposable
        {
            public string Path { get; set; }
            public System.Windows.Controls.Image Thumbnail { get; set; }

            public Photo(string path)
            {
                Path = path;

                using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
                {
                    // Generate a thumbnail of size 100x100 pixels
                    System.Drawing.Image thumbnail = image.GetThumbnailImage(100, 100, null, IntPtr.Zero);

                    // Create a WPF Image control to display the thumbnail
                    Thumbnail = new System.Windows.Controls.Image();
                    Thumbnail.Source = ConvertToBitmapImage(thumbnail);
                }
            }

            private BitmapImage ConvertToBitmapImage(System.Drawing.Image image)
            {
                MemoryStream memoryStream = new MemoryStream();
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmapImage.StreamSource = new MemoryStream(memoryStream.ToArray());
                bitmapImage.EndInit();
                memoryStream.Dispose();

                return bitmapImage;
            }

            public void Dispose()
            {
                Thumbnail?.SetCurrentValue(System.Windows.Controls.Image.SourceProperty, null);
            }

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
            //temp shit
            MessageBox.Show("Erm,,, what the spruce");
            return;
            //actual code
            //used https://aaronbos.dev/posts/iptc-metadata-csharp-imagesharp for help
        }

        private void Thumbnail_Click(object sender, RoutedEventArgs e)
        {
            
            Button button = sender as Button;
            string path = button.Tag.ToString();
            DisplayWindow dispWindow = new DisplayWindow(path);
            dispWindow.Show();
        }


        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            string path = menuItem.Tag.ToString();
            DataEntryWindow entryWindow = new DataEntryWindow(path);
            entryWindow.ShowDialog();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            string path = menuItem.Tag.ToString();
            DisplayWindow dispWindow = new DisplayWindow(path);
            dispWindow.Show();
        }
    }
}
