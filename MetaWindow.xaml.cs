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
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;

namespace PhotoOrg
{
    public partial class MetaWindow : Window
    {
        String path;
        public Image<Rgb24> image;
        public MetaWindow(string path)
        {           
            InitializeComponent();
            this.path = path;
            using (var tempImage = SixLabors.ImageSharp.Image.Load<Rgb24>(path).Clone())
            {
                image = tempImage.Clone();
                tempImage.Dispose();
            }
            BitmapImage bitimg = new BitmapImage();
            bitimg.BeginInit();
            bitimg.UriSource = new Uri(path);
            bitimg.EndInit();
            PreviewImage.Source = bitimg;
            MetadataReader metadataReader = new MetadataReader(path);
            City.Text = metadataReader.GetCity();
            Country.Text = metadataReader.GetCountry();
            Keywords.Text = metadataReader.GetKeywords();
            Name.Text = metadataReader.GetName();
            Location.Text = metadataReader.GetLocation();
            State.Text = metadataReader.GetState();
            

        }
    }
}
