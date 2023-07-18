using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.Windows.Controls;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Windows.Media;
using System.Linq;
using System.Windows.Input;
namespace PhotoOrg
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Photo> Photos { get; set; }
        private int PageSize = 50;
        private int currentPageIndex = 0;
        private string searchText = "";
        Image<Rgb24>? image = null;
        List<List<List<string>>> searchProperties = new List<List<List<string>>>();
        List<string> autofillText = new List<string>();
        public List<string> advPhotos = new List<string>();

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
                autofillText = catalogSearchTerms(searchProperties);
                NoPics.Visibility = 0;
                InitThumbnails(photos);
                currentPageIndex = 0;
                Page_Number.Text = (currentPageIndex + 1 + "/" + ((photos.Count / PageSize) + 1));
            }
            else
            {
                Folder_Text.Text = "Please select a folder";
            }
            
            

        }

        private async void Select_Folder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Select a Folder"
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Loading.Visibility = Visibility.Visible;
                Properties.Settings.Default.FolderLocation = dialog.FileName;
                Folder_Text.Text = Properties.Settings.Default.FolderLocation;
                List<string> photos =  GetPhotos(Properties.Settings.Default.FolderLocation);
                searchProperties = catalogValues(photos);
                autofillText = catalogSearchTerms(searchProperties);
                currentPageIndex = 0;
                Page_Number.Text = (currentPageIndex + 1 + "/" + ((photos.Count / PageSize) + 1));
                InitThumbnails(photos);
                NoPics.Visibility = Visibility.Hidden;
                Loading.Visibility = Visibility.Hidden;
                if (photos.Count < 0)
                {
                    NoPics.Visibility = Visibility.Visible;
                }
                else
                {
                    NoPics.Visibility = Visibility.Hidden;
                }
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
            ComboBox cmb = (ComboBox)SearchBox;
            searchText = cmb.Text;
            Debug.WriteLine(searchText);
            string query = searchText;
            string keyword = "";
            List<string> photos = new List<string>();
            if (!query.Equals(""))
            {
                keyword = Properties.Settings.Default.FolderLocation + "\\" + query;
                Debug.WriteLine(keyword);
                for (int l1index = 0; l1index < searchProperties.Count; l1index++)
                {

                    for (int l2index = 0; l2index < searchProperties[l1index].Count; l2index++)
                    {
                        for (int l3index = 0; l3index < searchProperties[l1index][l2index].Count; l3index++)
                        {
                            if (l1index == 0 && keyword.Equals(searchProperties[l1index][l2index][l3index]))
                            {
                                Debug.WriteLine(keyword.Substring(Properties.Settings.Default.FolderLocation.Length + 1));
                                photos.Add(searchProperties[0][l2index][0]);
                                break;

                            }
                            else if ((keyword).Equals(Properties.Settings.Default.FolderLocation + "\\" + searchProperties[l1index][l2index][l3index]))
                            {
                                Debug.WriteLine("else if");
                                Debug.WriteLine(keyword);
                                Debug.WriteLine(Properties.Settings.Default.FolderLocation + "\\" + searchProperties[0][l2index][0]);
                                //foreach (string photo in searchProperties[0][l2index])
                                Debug.WriteLine(Properties.Settings.Default.FolderLocation + "\\" + searchProperties[0][l2index][0]);
                                photos.Add(searchProperties[0][l2index][0]);
                                break;
                            }
                            else
                            {
                                Debug.WriteLine(keyword);
                                Debug.WriteLine(Properties.Settings.Default.FolderLocation + "\\" + searchProperties[l1index][l2index][l3index]);
                                
                            }
                        }
                    }
                    
                }
                Debug.WriteLine("images!!!");
                currentPageIndex = 0;
                Page_Number.Text = (currentPageIndex + 1 + "/" + ((photos.Count / PageSize) + 1));
                InitThumbnails(photos);
                if (photos.Count == 0)
                {
                    NoPics.Visibility = Visibility.Visible;
                }
                else
                {
                    NoPics.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                photos = GetPhotos(Properties.Settings.Default.FolderLocation);
                if (photos.Count == 0)
                {
                    NoPics.Visibility = Visibility.Visible;
                }
                else
                {
                    NoPics.Visibility = Visibility.Hidden;
                }
                currentPageIndex = 0;
                Page_Number.Text = (currentPageIndex + 1 + "/" + ((photos.Count / PageSize) + 1));
                InitThumbnails(photos);
            }
        }

        private List<string> catalogSearchTerms(List<List<List<string>>> list)
        {
            List<string> stringList = new List<string>();
            HashSet<string> uniqueTerms = new HashSet<string>();

            foreach (List<List<string>> list1 in list)
            {
                foreach (List<string> list2 in list1)
                {
                    foreach (string term in list2)
                    {
                        if (!term.Equals("") && !uniqueTerms.Contains(term))
                        {
                            if (!term.StartsWith(Properties.Settings.Default.FolderLocation))
                            {
                                stringList.Add(term);
                                Debug.WriteLine(term);
                            }
                            else
                            {
                                string term2 = "";
                                if (term.Length > Properties.Settings.Default.FolderLocation.Length + 1)
                                {
                                    term2 = term.Substring(Properties.Settings.Default.FolderLocation.Length + 1);
                                }
                                stringList.Add(term2);
                                Debug.WriteLine(term2);
                            }
                            uniqueTerms.Add(term);
                        }
                    }
                }
            }
            return stringList;
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

        public static T GetChildOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        private void PreviewTextInput_EnhanceComboSearch(object sender, TextCompositionEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;

            cmb.IsDropDownOpen = true;

            if (!string.IsNullOrEmpty(cmb.Text))
            {
                string fullText = cmb.Text.Insert(GetChildOfType<TextBox>(cmb).CaretIndex, e.Text);
                searchText = fullText;
                cmb.ItemsSource = FilterAutofillText(fullText);
            }
            else if (!string.IsNullOrEmpty(e.Text))
            {
                cmb.ItemsSource = FilterAutofillText(e.Text);
                searchText = e.Text;
            }
            else
            {
                cmb.ItemsSource = autofillText;
            }
        }

        private void Pasting_EnhanceComboSearch(object sender, DataObjectPastingEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;

            cmb.IsDropDownOpen = true;

            string pastedText = (string)e.DataObject.GetData(typeof(string));
            string fullText = cmb.Text.Insert(GetChildOfType<TextBox>(cmb).CaretIndex, pastedText);

            if (!string.IsNullOrEmpty(fullText))
            {
                searchText = fullText;
                cmb.ItemsSource = FilterAutofillText(fullText);
            }
            else
            {
                cmb.ItemsSource = autofillText;
            }
        }

        private void PreviewKeyUp_EnhanceComboSearch(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                ComboBox cmb = (ComboBox)sender;

                cmb.IsDropDownOpen = true;

                if (!string.IsNullOrEmpty(cmb.Text))
                {
                    cmb.ItemsSource = FilterAutofillText(cmb.Text);
                }
                else
                {
                    cmb.ItemsSource = autofillText;
                }
            }
        }

        private List<string> FilterAutofillText(string searchText)
        {
            List<string> searchTerms = GetSearchTerms(searchText);

            List<string> filteredItems = new List<string>();

            foreach (string term in searchTerms)
            {
                string trimmedTerm = term.Trim('\"');
                List<string> matchedItems = autofillText
                    .Where(s => s.IndexOf(trimmedTerm, StringComparison.InvariantCultureIgnoreCase) != -1)
                    .ToList();

                filteredItems.AddRange(matchedItems);
            }

            return filteredItems.Distinct().ToList();
        }


        private List<string> GetSearchTerms(string searchText)
        {
            List<string> searchTerms = new List<string>();

            string[] terms = searchText.Split(';');

            foreach (string term in terms)
            {
                string trimmedTerm = term.Trim();

                if (trimmedTerm.StartsWith("\"") && trimmedTerm.EndsWith("\""))
                {
                    string searchTerm = trimmedTerm.Trim('\"');
                    searchTerms.Add(searchTerm);
                }
                else
                {
                    string[] subTerms = trimmedTerm.Split(' ');
                    foreach (string subTerm in subTerms)
                    {
                        searchTerms.Add(subTerm);
                    }
                }
            }

            return searchTerms;
        }

        private void AdvSearch_Menu_Click(object sender, RoutedEventArgs e)
        {
            AdvanceSearch advanceSearch = new AdvanceSearch(searchProperties);
            advanceSearch.ShowDialog();
            Debug.WriteLine("GLOBALS length:" + GLOBALS.advPhotos.Count);
            foreach (string path in GLOBALS.advPhotos)
            {
                Debug.WriteLine("Printed from MainWindow:" + path);
            }
            InitThumbnails(GLOBALS.advPhotos);

        }
    }
}
