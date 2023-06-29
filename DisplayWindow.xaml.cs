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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DisplayWindow : Window
    {
        string path;
        public DisplayWindow(string path)
        {
            InitializeComponent();
            this.path = path;
            BitmapImage bitimg = new BitmapImage();
            bitimg.BeginInit();
            bitimg.UriSource = new Uri(path);
            bitimg.EndInit();
            DispWindow.Title = path;
            DispImage.Source = bitimg;
            //TODO: Dynamically resize the image based on the window dimensions
            
        }
    }
}
