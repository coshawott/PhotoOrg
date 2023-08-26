using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PhotoOrg
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DisplayWindow : Window
    {
        string path;
        public BitmapImage image;
        public DisplayWindow(string path)
        {
            InitializeComponent();
            this.path = path;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path);
            bitmap.EndInit();
            DispImage.Source = bitmap;
            //ChangeImageSize();
            Debug.WriteLine(path);
            DispWindow.Title = path;
            MetadataReader captionReader = new MetadataReader(path);
            Caption.Text = captionReader.GetCaption();
            Keywords.Text = $"Names: {GetKeywordString(captionReader)}";
            Name.Text = $"Events: {captionReader.GetNameList()[0]}";
            Categories.Text = $"Location: {captionReader.GetLocationList()[0]}";
            Date.Text = $"Date: {captionReader.GetDate()}";
            Notes.Text = $"Label: {captionReader.GetNotes()}";
        }

        private string GetKeywordString(MetadataReader reader)
        {
            string endVal = "";
            foreach (string keyword in reader.GetKeywordList())
            {
                endVal += $"\"{keyword}\" ";
            }
            return endVal;
        }

        private void Window_SizeChanged( object sender, SizeChangedEventArgs e )
        {
            //ChangeImageSize();
        }
    }
   
}

