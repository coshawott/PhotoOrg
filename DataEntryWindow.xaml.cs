using System.Windows;
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
            EntWindow.Title = path;
        }

        private void WriteToImage(object sender, RoutedEventArgs e)
        {
        
            if (image.Metadata.IptcProfile == null)
            {
                image.Metadata.IptcProfile = new IptcProfile();
            }
            image.Metadata.IptcProfile.SetValue(IptcTag.Keywords, LastName.Text + "." + FirstName.Text);
            if (path.EndsWith(".png"))
            {
                image.SaveAsPng(path);
            }
            else if (path.EndsWith(".jpg") || path.EndsWith(".jpeg"))
            {
                image.SaveAsJpeg(path);
            }
            else if (path.EndsWith(".gif"))
            {
                image.SaveAsGif(path);
            }
            MessageBox.Show("erm what the spruce");
            Close();
            
        } 


    }
}
