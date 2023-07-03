using System;
using System.Windows;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Iptc;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;

namespace PhotoOrg
{
    /// <summary>
    /// Interaction logic for DataEntryWindow.xaml
    /// </summary>
    public partial class DataEntryWindow : Window, IDisposable
    {
        private string path;
        public Image<Rgb24> image;
         

        public DataEntryWindow(string path)
        {
            InitializeComponent();
            this.path = path;

            using (var tempImage = Image.Load<Rgb24>(path).Clone())
            {
                image = tempImage.Clone();
                tempImage.Dispose();
            }

            EntWindow.Title = path;
        }

        private void WriteToImage(object sender, RoutedEventArgs e)
        //rewrite this method so that instead of saving the file here, it passes image to the main window and saves it there
        {
            if (image.Metadata.IptcProfile == null)
            {
                image.Metadata.IptcProfile = new IptcProfile();
            }

            image.Metadata.IptcProfile.SetValue(IptcTag.Keywords, LastName.Text + "." + FirstName.Text);
            GLOBALS.image = image.Clone();
            GLOBALS.initialOpen = false;
            GLOBALS.path = path;
            MessageBox.Show("erm what the spruce");
            
            
        }

        public void Dispose()
        {
            image?.Dispose();
        }

        protected override void OnClosed(EventArgs e)
        {
            Dispose();
            base.OnClosed(e);
        }
    }
}
