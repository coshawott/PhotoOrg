using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhotoOrg
{
    public partial class AdvanceSearch : Window
    {
        List<ListBoxItem> items = new List<ListBoxItem>();
        private List<List<List<string>>> searchData = new List<List<List<string>>>();
        List<string> lazyData = new List<string>();
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

            lazyData = catalogSearchTerms(searchData);
            foreach (string item in lazyData)
            {
                Debug.WriteLine(item);
                ListBoxItem lbI = new ListBoxItem { IsSelected = false, ItemText = item };
                items.Add(lbI);
            }
            ListBoxWithCheckboxes.ItemsSource = items;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            List<string> searchTerms = new List<string>();
            foreach (ListBoxItem lbI in ListBoxWithCheckboxes.ItemsSource)
            {
                if (lbI.IsSelected)
                {
                    searchTerms.Add(lbI.ItemText);
                }
            }
            Debug.WriteLine(Search(searchTerms));
            GLOBALS.advPhotos = Search(searchTerms);
            Debug.WriteLine("Printed from AdvanceSearch:" + GLOBALS.advPhotos.Count);
            Close();
        }

        private List<string> catalogSearchTerms(List<List<List<string>>> list)
        {
            List<string> stringList = new List<string>();
            for (int i = 1; i < list.Count; i++)
            {
                foreach (List<string> list2 in list[i])
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
            return stringList;
        }

        private List<string> Search(List<string> oldsearchterms)
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


    }



    public class ListBoxItem
    {
        public bool IsSelected { get; set; }
        public string ItemText { get; set; }
    }
}
