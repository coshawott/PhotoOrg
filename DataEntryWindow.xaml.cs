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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Iptc;
using Image = SixLabors.ImageSharp.Image;

namespace PhotoOrg
{
    /// <summary>
    /// Interaction logic for DataEntryWindow.xaml
    /// </summary>
    public partial class DataEntryWindow : Window
    {
        private string path;
        private Image image;
        public DataEntryWindow(string path)
        {
            InitializeComponent();
            this.path = path;
            image = Image.Load(path);
            MessageBox.Show(path);
        }

        private void WriteToImage(object sender, RoutedEventArgs e)
        {
        
            if (image.Metadata.IptcProfile == null)
            {
                image.Metadata.IptcProfile = new IptcProfile();
            }
            MessageBox.Show("erm what the spruce");
            Close();
            
        } 


    }
}
