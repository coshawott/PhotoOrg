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
        private Image<Rgb24> image;

        public DataEntryWindow(string path)
        {
            InitializeComponent();
            this.path = path;

            using (var tempImage = Image.Load<Rgb24>(path).Clone())
            {
                image = tempImage.Clone();
            }

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
                using (var fileStream = System.IO.File.OpenWrite(path))
                {
                    image.Save(fileStream, new PngEncoder());
                }
            }
            else if (path.EndsWith(".jpg") || path.EndsWith(".jpeg"))
            {
                using (var fileStream = System.IO.File.OpenWrite(path))
                {
                    image.Save(fileStream, new JpegEncoder());
                }
            }
            else if (path.EndsWith(".gif"))
            {
                using (var fileStream = System.IO.File.OpenWrite(path))
                {
                    image.Save(fileStream, new GifEncoder());
                }
            }

            MessageBox.Show("erm what the spruce");
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
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
