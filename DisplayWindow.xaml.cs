using System;
using System.Diagnostics;
using System.Text;
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
            StringBuilder sb = new StringBuilder(Keywords.Text);
            if (Keywords.Text.Length / 150 >= 1)
            {
                for (int i = 1; i <= Keywords.Text.Length/150;i++)
                {
                    sb.Insert(150 * i, "\n");
                    Keywords.Text = sb.ToString();
                }
            }
            Name.Text = $"Events: {captionReader.GetNameList()[0]}";
            Categories.Text = $"Location: {captionReader.GetLocationList()[0]}";
            if (captionReader.GetDate().Equals("99999"))
            {
                Date.Text = "Date:";
            }
            else
            {
                Date.Text = $"Date: {captionReader.GetDate()}";
            }
            Notes.Text = $"Label: {captionReader.GetNotes()}";
            Filename.Text = $"Filename: {path}";
            int indexOfDelimiter = path.LastIndexOf('\\');
            if (indexOfDelimiter >= 0)
            {
                Filename.Text = $"Filename: {path.Substring(indexOfDelimiter + 1)}";
            }
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

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
   
}

