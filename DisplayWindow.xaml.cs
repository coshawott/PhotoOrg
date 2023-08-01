using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
            Caption.Text = Caption.Text.Replace("Caption                         :", "");
            Keywords.Text = $"People: {GetKeywordString(captionReader)}";
            Name.Text = $"Label: {captionReader.GetNameList()[0]}";
            Categories.Text = $"Location: {captionReader.GetLocationList()[0]}";
            Date.Text = $"Date: ";
            Notes.Text = $"Notes: {captionReader.GetNotes()}";
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

