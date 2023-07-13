using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// Interaction logic for AdvanceSearch.xaml
    /// </summary>
    public partial class AdvanceSearch : Window
    {
        List<ListBoxItem> items = new List<ListBoxItem>();
        List<List<List<string>>> searchData = new List<List<List<string>>>();
        List<string> lazyData = new List<string>();
        public AdvanceSearch(List<List<List<string>>> searchData, List<string> lazyData)
        {
            InitializeComponent();
            searchData = this.searchData;
            lazyData = this.lazyData;
            foreach (string item in lazyData)
            {
                new ListBoxItem { IsSelected = false, ItemText = item };
            }
            ListBoxWithCheckboxes.ItemsSource = items;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }

    public class ListBoxItem
    {
        public bool IsSelected { get; set; }
        public string ItemText { get; set; }
    }
}
