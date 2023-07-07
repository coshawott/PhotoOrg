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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Iptc;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
using System.Linq;

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
        Image<Rgb24>? image = null;
        List<List<List<string>>> searchProperties = new List<List<List<string>>>();

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
                //TODO add loading screen
                searchProperties = catalogValues(photos);
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
                //TODO add loading screen
                searchProperties = catalogValues(photos);
                InitThumbnails(photos);
                currentPageIndex = 0;
                Page_Number.Text = (currentPageIndex + 1 + "/" + ((photos.Count / PageSize) + 1));
                //foreach(string photo in photos)
                //{
                //Debug.WriteLine(photo);
                //}
            }
        }

        private List<string> GetPhotos(string folderPath)
        {
            List<string> photoPaths = new List<string>();
            if (Directory.Exists(folderPath))
            {
                string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };
                foreach (string ext in extensions)
                {
                    string[] files = Directory.GetFiles(folderPath, "*" + ext);
                    foreach (string file in files)
                    {
                        using (FileStream fileStream = File.OpenRead(file))
                        {
                            // Perform necessary operations with the file
                            // For example, you can read the file contents or extract metadata
                            // Add the file path to the list
                            photoPaths.Add(file);
                        }
                    }
                }
            }

            return photoPaths;
        }

        private void InitThumbnails(List<string> photoPaths)
        {
            int startIndex = currentPageIndex * PageSize;
            int endIndex = Math.Min(startIndex + PageSize, photoPaths.Count);
            Photos.Clear();
            for (int i = startIndex; i < endIndex; i++)
            {
                Photos.Add(new Photo { Path = photoPaths[i] });
            }
        }


        public class Photo
        {
            public string? Path { get; set; }

            public string GetPath()
            {
                return Path;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            List<string> photos = GetPhotos(Properties.Settings.Default.FolderLocation);

            if (currentPageIndex < photos.Count / PageSize)
            {
                currentPageIndex++;
                InitThumbnails(photos);
            }
            Page_Number.Text = (currentPageIndex + 1 + "/" + ((photos.Count / PageSize) + 1));
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            List<string> photos = GetPhotos(Properties.Settings.Default.FolderLocation);
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                InitThumbnails(photos);
            }
            Page_Number.Text = (currentPageIndex + 1 + "/" + ((photos.Count / PageSize) + 1));
        }

        private void Search_Menu_Click(object sender, RoutedEventArgs e)
        {
            //temp shit
            MessageBox.Show("Erm,,, what the spruce");
            return;
            //actual code
            //used https://aaronbos.dev/posts/iptc-metadata-csharp-imagesharp for help
        }

        private List<List<List<string>>> catalogValues(List<string> photos)
        {
            List<List<List<string>>> endVals = new List<List<List<string>>>();
            List<List<string>> pathVals = new List<List<string>>();
            List<List<string>> keywordVals = new List<List<string>>();
            List<List<string>> cityVals = new List<List<string>>();
            List<List<string>> countryVals = new List<List<string>>();
            List<List<string>> nameVals = new List<List<string>>();
            foreach (string photo in photos)
            {
                List<string> photoList = new List<string>();
                photoList.Add(photo);
                pathVals.Add(photoList);
                MetadataReader tagRead = new MetadataReader(photo);
                keywordVals.Add(tagRead.GetKeywordList());
                cityVals.Add(tagRead.GetCityList());
                countryVals.Add(tagRead.GetCountryList());
                nameVals.Add(tagRead.GetNameList());

                
                tagRead.Dispose();

            }
            endVals.Add(pathVals);
            endVals.Add(keywordVals);
            endVals.Add(cityVals);
            endVals.Add(countryVals);
            endVals.Add(nameVals);
            return endVals;
        }

        private void Thumbnail_Click(object sender, RoutedEventArgs e)
        {

            Button button = sender as Button;
            string path = button.Tag.ToString();
            DisplayWindow dispWindow = new DisplayWindow(path);
            dispWindow.ShowDialog();
        }


        private void Edit_Click(object sender, RoutedEventArgs e)
        {

            MenuItem menuItem = sender as MenuItem;
            string? path = menuItem.Tag.ToString();
            string? missingPic = null;
            List<string> photos = GetPhotos(Properties.Settings.Default.FolderLocation);
            for (int index = 0; index < photos.Count(); index++)
            {
                if (photos[index] != null)
                {
                    if (photos[index] == path)
                    {
                        missingPic = photos[index];
                        photos.RemoveAt(index);
                    }
                }
            }
            InitThumbnails(photos);
            DataEntryWindow? entryWindow = new DataEntryWindow(path);
            var result = entryWindow.ShowDialog();
            //if (result == DialogResult.Equals(true))
            //{
            MessageBox.Show("Dialog = true!!!");
            photos.Add(missingPic);
            InitThumbnails(photos);
            entryWindow.Dispose();

        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            string path = menuItem.Tag.ToString();
            DisplayWindow dispWindow = new DisplayWindow(path);
            dispWindow.ShowDialog();
        }

        private void ViewMeta_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            string path = menuItem.Tag.ToString();
            MetaWindow metaWindow = new MetaWindow(path);
            metaWindow.ShowDialog();
        }

        private void SearchBox_Changed(object sender, RoutedEventArgs e)
        {
            string searchText = SearchBox.Text;
        }

        private void suggestionComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string balls = "lol";
        }

        private void textBoxInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
