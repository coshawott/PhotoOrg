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
    /// Interaction logic for DataEntryWindow.xaml
    /// </summary>
    public partial class DataEntryWindow : Window
    {
        String path;
        public DataEntryWindow(String path)
        {
            InitializeComponent();
            this.path = path;
            MessageBox.Show(path);
        }


    }
}
