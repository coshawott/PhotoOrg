using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Linq;

namespace PhotoOrg
{
    public partial class AdvanceSearch : Window
    {
        List<ListBoxItem> keyItems = new List<ListBoxItem>();
        List<ListBoxItem> locationItems = new List<ListBoxItem>();
        List<ListBoxItem> nameItems = new List<ListBoxItem>();
        List<ListBoxItem> keyItems2 = new List<ListBoxItem>();
        List<ListBoxItem> locationItems2 = new List<ListBoxItem>();
        List<ListBoxItem> nameItems2 = new List<ListBoxItem>();
        List<ListBoxItem> keyItems3 = new List<ListBoxItem>();
        List<ListBoxItem> locationItems3 = new List<ListBoxItem>();
        List<ListBoxItem> nameItems3 = new List<ListBoxItem>();
        private List<List<List<string>>> searchData = new List<List<List<string>>>();
        List<string> keyData = new List<string>();
        List<string> locationData = new List<string>();
        List<string> nameData = new List<string>();
        int timeRan = 0;
        
        public AdvanceSearch(List<List<List<string>>> oldSearchData)
        {
            InitializeComponent();
            for (int l1index = 0; l1index < oldSearchData.Count; l1index++)
            {
                List<List<string>> l1Copy = new List<List<string>>();

                for (int l2index = 0; l2index < oldSearchData[l1index].Count; l2index++)
                {
                    List<string> l2Copy = new List<string>();

                    for (int l3index = 0; l3index < oldSearchData[l1index][l2index].Count; l3index++)
                    {
                        string l3Copy = oldSearchData[l1index][l2index][l3index];
                        l2Copy.Add(l3Copy);
                    }

                    l1Copy.Add(l2Copy);
                }

                searchData.Add(l1Copy);
            }

            keyData = catalogKeywords(searchData);
            foreach (string item in keyData)
            {
                Debug.WriteLine(item);
                ListBoxItem lbI = new ListBoxItem { IsSelected = false, ItemText = item };
                ListBoxItem lbI2 = new ListBoxItem { IsSelected = false, ItemText = item };
                ListBoxItem lbI3 = new ListBoxItem { IsSelected = false, ItemText = item };
                keyItems.Add(lbI);
                keyItems2.Add(lbI2);
                keyItems3.Add(lbI3);
            }
            ListBoxWithCheckboxes.ItemsSource = keyItems;
            ListBoxWithCheckboxes2.ItemsSource = keyItems2;
            ListBoxWithCheckboxes3.ItemsSource = keyItems3;

            locationData = catalogLocations(searchData);
            foreach (string item in locationData)
            {
                Debug.WriteLine(item);
                ListBoxItem lbI = new ListBoxItem { IsSelected = false, ItemText = item };
                ListBoxItem lbI2 = new ListBoxItem { IsSelected = false, ItemText = item };
                ListBoxItem lbI3 = new ListBoxItem { IsSelected = false, ItemText = item };
                locationItems.Add(lbI);
                locationItems2.Add(lbI2);
                locationItems3.Add(lbI3);
            }
            Locations.ItemsSource = locationItems;
            Locations2.ItemsSource = locationItems2;
            Locations3.ItemsSource = locationItems3;

            nameData = catalogNames(searchData);
            foreach (string item in nameData)
            {
                Debug.WriteLine(item);
                ListBoxItem lbI = new ListBoxItem { IsSelected = false, ItemText = item };
                ListBoxItem lbI2 = new ListBoxItem { IsSelected = false, ItemText = item };
                ListBoxItem lbI3 = new ListBoxItem { IsSelected = false, ItemText = item };
                nameItems.Add(lbI);
                nameItems2.Add(lbI2);
                nameItems3.Add(lbI3);
            }
            Names.ItemsSource = nameItems;
            Names2.ItemsSource = nameItems2;
            Names3.ItemsSource = nameItems3;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            List<string> keywords = new List<string>();
            List<string> names = new List<string>();
            List<string> locations = new List<string>();
            List<string> keywords2 = new List<string>();
            List<string> names2 = new List<string>();
            List<string> locations2 = new List<string>();
            List<string> keywords3 = new List<string>();
            List<string> names3 = new List<string>();
            List<string> locations3 = new List<string>();
            foreach (ListBoxItem lbI in ListBoxWithCheckboxes.ItemsSource)
            {
                if (lbI.IsSelected)
                {
                    keywords.Add(lbI.ItemText);
                }
            }
            foreach (ListBoxItem lbI in Locations.ItemsSource)
            {
                if (lbI.IsSelected)
                {
                    locations.Add(lbI.ItemText);
                }
            }
            foreach (ListBoxItem lbI in Names.ItemsSource)
            {
                if (lbI.IsSelected)
                {
                    Debug.WriteLine($"Object Name: {lbI.ItemText}");
                    names.Add(lbI.ItemText);
                }
            }
            foreach (ListBoxItem lbI in ListBoxWithCheckboxes2.ItemsSource)
            {
                if (lbI.IsSelected)
                {
                    Debug.WriteLine($"Object Name for keywords 2: {lbI.ItemText}");
                    keywords2.Add(lbI.ItemText);
                }
            }
            foreach (ListBoxItem lbI in Locations2.ItemsSource)
            {
                if (lbI.IsSelected)
                {
                    locations2.Add(lbI.ItemText);
                }
            }
            foreach (ListBoxItem lbI in Names2.ItemsSource)
            {
                if (lbI.IsSelected)
                {
                    
                    names2.Add(lbI.ItemText);
                }
            }
            foreach (ListBoxItem lbI in ListBoxWithCheckboxes3.ItemsSource)
            {
                if (lbI.IsSelected)
                {
                    keywords3.Add(lbI.ItemText);
                }
            }
            foreach (ListBoxItem lbI in Locations3.ItemsSource)
            {
                if (lbI.IsSelected)
                {
                    locations3.Add(lbI.ItemText);
                }
            }
            foreach (ListBoxItem lbI in Names3.ItemsSource)
            {
                if (lbI.IsSelected)
                {
                    Debug.WriteLine($"Object Name: {lbI.ItemText}");
                    names3.Add(lbI.ItemText);
                }
            }



            //AND Search
            if (names.Count > 0 || keywords.Count > 0 || locations.Count > 0)
            {
                if (keywords.Count > 0 && names.Count > 0 && locations.Count > 0)
                {
                    List<string> keyPaths = SearchKeywords(keywords);
                    List<string> namePaths = SearchNames(names);
                    List<string> locationsPaths = SearchLocations(locations);
                    GLOBALS.advPhotos = keyPaths.Intersect(namePaths).ToList().Intersect(locationsPaths).ToList();
                    
                }
                else if (keywords.Count > 0 && names.Count > 0)
                {
                    List<string> keyPaths = SearchKeywords(keywords);
                    List<string> namePaths = SearchNames(names);
                    GLOBALS.advPhotos = keyPaths.Intersect(namePaths).ToList();
                }
                else if (keywords.Count > 0 && locations.Count >0)
                {
                    List<string> keyPaths = SearchKeywords(keywords);
                    List<string> locationPaths = SearchLocations(locations);
                    GLOBALS.advPhotos = keyPaths.Intersect(locationPaths).ToList();
                }
                else if (locations.Count > 0 && names.Count > 0)
                {
                    List<string> namePaths = SearchNames(names);
                    List<string> locationPaths = SearchLocations(locations);
                    GLOBALS.advPhotos = namePaths.Intersect(locationPaths).ToList();
                }
                else if (locations.Count > 0)
                {
                    List<string> locationPaths = SearchLocations(locations);
                    GLOBALS.advPhotos = locationPaths;
                }
                else if (keywords.Count > 0)
                {
                    List<string> keyPaths = SearchKeywords(keywords);
                    GLOBALS.advPhotos = keyPaths;
                }
                else
                {
                    List<string> namePaths = SearchNames(names);
                    GLOBALS.advPhotos = namePaths;
                }
               
            }
            //OR Search
            Debug.WriteLine($"or keywords length {keywords2.Count}");
            if (names2.Count > 0 || keywords2.Count > 0 || locations2.Count > 0)
            {
                List<string> keyPaths =  OrSearchKeywords(keywords2);
                List<string> namePaths = OrSearchNames(names2);
                List<string> locationPaths = OrSearchLocations(locations2);
                Debug.WriteLine($"keypaths length {keyPaths.Count}");
                List<string> erm = GLOBALS.advPhotos.Union(keyPaths.Union(namePaths).ToList().Union(locationPaths).ToList()).ToList();
                Debug.WriteLine($"unionized length {erm.Count}");
                GLOBALS.advPhotos = erm;
                
            }
            //NOT Search
            if (names3.Count > 0 || keywords3.Count > 0 || locations3.Count > 0)
            {
                List<string> keyPaths = OrSearchKeywords(keywords3);
                List<string> namePaths = OrSearchNames(names3);
                List<string> locationPaths = OrSearchLocations(locations3);
                List<string> removeThese = keyPaths.Union(namePaths).ToList().Union(locationPaths).ToList();
                if (GLOBALS.advPhotos.Count > 0)
                {
                    foreach (string item in removeThese)
                    {
                        GLOBALS.advPhotos.RemoveAll(item => removeThese.Contains(item));
                    }
                }
                else if(names.Count == 0 && keywords.Count == 0 && locations.Count == 0 && names2.Count == 0 && keywords2.Count == 0 && locations2.Count == 0)
                {
                    List<string> allPaths = new List<string>();
                    foreach (List<string> paths in searchData[0])
                    {
                        allPaths.Add(paths[0]);
                    }
                    foreach (string item in removeThese)
                    {
                        allPaths.RemoveAll(item => removeThese.Contains(item));
                    }
                    GLOBALS.advPhotos = allPaths;
                }
                
                
               
            }
            if (names.Count == 0 && keywords.Count == 0 && locations.Count == 0 && names2.Count == 0 && keywords2.Count == 0 && locations2.Count == 0 && names3.Count == 0 && keywords3.Count == 0 && locations3.Count == 0) 
            {
                List<string> allPaths = new List<string>();
                foreach (List<string> paths in searchData[0])
                {
                    allPaths.Add(paths[0]);
                }
                GLOBALS.advPhotos = allPaths;

            }
            Close();
        }

        private List<string> catalogKeywords(List<List<List<string>>> list)
        {
            List<string> stringList = new List<string>();
            if (list.Count > 0) 
            {
                foreach (List<string> list2 in list[1])
                {
                    foreach (string term in list2)
                    {
                        if (!term.Equals(""))
                        {
                            if (!term.StartsWith(Properties.Settings.Default.FolderLocation) && !IsStringInList(term, stringList))
                            {
                                stringList.Add(term);
                                Debug.WriteLine(term);
                            }
                        }
                    }
                }
            }
            stringList.Sort();
            return stringList;
        }

        private List<string> catalogNames(List<List<List<string>>> list)
        {
            List<string> stringList = new List<string>();
            if (list.Count > 0)
            {
                foreach (List<string> list2 in list[4])
                {
                    foreach (string term in list2)
                    {
                        if (!term.Equals(""))
                        {
                            if (!term.StartsWith(Properties.Settings.Default.FolderLocation) && !IsStringInList(term, stringList))
                            {
                                stringList.Add(term);
                                Debug.WriteLine(term);
                            }
                        }
                    }
                }
            }
            stringList.Sort();
            return stringList;
        }
        private List<string> catalogLocations(List<List<List<string>>> list)
        {
            List<string> stringList = new List<string>();
            if (list.Count > 0)
            {
                foreach (List<string> list2 in list[5])
                {
                    foreach (string term in list2)
                    {
                        if (!term.Equals(""))
                        {
                            if (!term.StartsWith(Properties.Settings.Default.FolderLocation) && !IsStringInList(term, stringList))
                            {
                                stringList.Add(term);
                                Debug.WriteLine(term);
                            }
                        }
                    }
                }
            }
            stringList.Sort();
            return stringList;
        }

        private List<string> SearchKeywords(List<string> oldsearchterms)
        {
            List<string> searchterms = new List<string>(oldsearchterms);
            List<string> filepaths = new List<string>();
            
            for (int l2index = 0; l2index < searchData[1].Count; l2index++)
            {
                bool allTermsFound = true;
                foreach (string term in searchterms)
                {
                    bool termFound = false;
                    for (int l3index = 0; l3index < searchData[1][l2index].Count; l3index++)
                    {
                        if (term.Equals(searchData[1][l2index][l3index]))
                        {
                            termFound = true;
                            break;
                        }
                    }
                    if (!termFound)
                    {
                        allTermsFound = false;
                        break;
                    }
                }
                if (allTermsFound && !IsStringInList(searchData[0][l2index][0], filepaths))
                {
                    filepaths.Add(searchData[0][l2index][0]);
                    Debug.WriteLine($"All search terms found at {searchData[0][l2index][0]}");
                }
            }

            return filepaths;
        }

        private List<string> SearchLocations(List<string> oldsearchterms)
        {
            List<string> searchterms = new List<string>(oldsearchterms);
            List<string> filepaths = new List<string>();

            for (int l2index = 0; l2index < searchData[5].Count; l2index++)
            {
                bool allTermsFound = true;
                foreach (string term in searchterms)
                {
                    bool termFound = false;
                    for (int l3index = 0; l3index < searchData[5][l2index].Count; l3index++)
                    {
                        if (term.Equals(searchData[5][l2index][l3index]))
                        {
                            termFound = true;
                            break;
                        }
                    }
                    if (!termFound)
                    {            
                        allTermsFound = false;
                        break;
                    }
                }
                if (allTermsFound && !IsStringInList(searchData[0][l2index][0], filepaths))
                {
                    filepaths.Add(searchData[0][l2index][0]);
                    Debug.WriteLine($"All search terms found at {searchData[0][l2index][0]}");
                }
            }

            return filepaths;
        }

        private List<string> SearchNames(List<string> oldsearchterms)
        {
            List<string> searchterms = new List<string>(oldsearchterms);
            List<string> filepaths = new List<string>();

            for (int l2index = 0; l2index < searchData[4].Count; l2index++)
            {
                bool allTermsFound = true;
                foreach (string term in searchterms)
                {
                    bool termFound = false;
                    for (int l3index = 0; l3index < searchData[4][l2index].Count; l3index++)
                    {
                        if (term.Equals(searchData[4][l2index][l3index]))
                        {
                            termFound = true;
                            break;
                        }
                    }
                    if (!termFound)
                    {
                        allTermsFound = false;
                        break;
                    }
                }
                if (allTermsFound && !IsStringInList(searchData[0][l2index][0], filepaths))
                {
                    filepaths.Add(searchData[0][l2index][0]);
                    Debug.WriteLine($"All search terms found at {searchData[0][l2index][0]}");
                }
            }

            return filepaths;
        }

        private List<string> OrSearchKeywords(List<string> oldsearchterms)
        {
            List<string> searchterms = new List<string>(oldsearchterms);
            List<string> filepaths = new List<string>();

            for (int l2index = 0; l2index < searchData[1].Count; l2index++)
            {
                foreach (string term in searchterms)
                {
                    for (int l3index = 0; l3index < searchData[1][l2index].Count; l3index++)
                    {
                        if (term.Equals(searchData[1][l2index][l3index]))
                        {
                            filepaths.Add(searchData[0][l2index][0]);
                        }
                    }
                }
            }

            return filepaths;
        }

        private List<string> OrSearchNames(List<string> oldsearchterms)
        {
            List<string> searchterms = new List<string>(oldsearchterms);
            List<string> filepaths = new List<string>();

            for (int l2index = 0; l2index < searchData[4].Count; l2index++)
            {
                foreach (string term in searchterms)
                {
                    for (int l3index = 0; l3index < searchData[4][l2index].Count; l3index++)
                    {
                        if (term.Equals(searchData[4][l2index][l3index]))
                        {
                            filepaths.Add(searchData[0][l2index][0]);
                        }
                    }
                }
            }

            return filepaths;
        }

        private List<string> OrSearchLocations(List<string> oldsearchterms)
        {
            List<string> searchterms = new List<string>(oldsearchterms);
            List<string> filepaths = new List<string>();

            for (int l2index = 0; l2index < searchData[5].Count; l2index++)
            {
                foreach (string term in searchterms)
                {
                    for (int l3index = 0; l3index < searchData[5][l2index].Count; l3index++)
                    {
                        if (term.Equals(searchData[5][l2index][l3index]))
                        {
                            filepaths.Add(searchData[0][l2index][0]);
                        }
                    }
                }
            }

            return filepaths;
        }

        private Boolean IsStringInList(string term, List<string> strings)
        {
            foreach (string entry in strings)
            {
                if (term.Equals(entry))
                {
                    return true;
                }
            }
            return false;
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (NameSearchBar.Text.Length > 0)
            {
                List<ListBoxItem> newKeyItems = new List<ListBoxItem>();
                foreach (ListBoxItem item in keyItems)
                {
                    if (item.ItemText.ToLower().Contains(NameSearchBar.Text.ToLower()))
                    {
                        ListBoxItem lbI = new ListBoxItem { IsSelected = false, ItemText = item.ItemText };
                        newKeyItems.Add(lbI);
                    }
                }
                ListBoxWithCheckboxes.ItemsSource = newKeyItems;
            }
            else
            {
                ListBoxWithCheckboxes.ItemsSource = keyItems;
            }
        }

        private void TextBox_TextChanged2(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (NameSearchBar2.Text.Length > 0)
            {
                List<ListBoxItem> newKeyItems = new List<ListBoxItem>();
                foreach (ListBoxItem item in keyItems)
                {
                    if (item.ItemText.ToLower().Contains(NameSearchBar2.Text.ToLower()))
                    {
                        ListBoxItem lbI = new ListBoxItem { IsSelected = false, ItemText = item.ItemText };
                        newKeyItems.Add(lbI);
                    }
                }
                ListBoxWithCheckboxes2.ItemsSource = newKeyItems;
            }
            else
            {
                ListBoxWithCheckboxes2.ItemsSource = keyItems2;
            }
        }

        private void TextBox_TextChanged3(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (NameSearchBar3.Text.Length > 0)
            {
                List<ListBoxItem> newKeyItems = new List<ListBoxItem>();
                foreach (ListBoxItem item in keyItems)
                {
                    if (item.ItemText.ToLower().Contains(NameSearchBar3.Text.ToLower()))
                    {
                        ListBoxItem lbI = new ListBoxItem { IsSelected = false, ItemText = item.ItemText };
                        newKeyItems.Add(lbI);
                    }
                }
                ListBoxWithCheckboxes3.ItemsSource = newKeyItems;
            }
            else
            {
                ListBoxWithCheckboxes3.ItemsSource = keyItems3;
            }
        }

    }



    public class ListBoxItem
    {
        public bool IsSelected { get; set; }
        public string ItemText { get; set; }
    }
}
