﻿using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
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
        public Image<Rgb24> image;
        public DisplayWindow(string path)
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
            DispWindow.Title = path;
            DispImage.Source = bitimg;
            
        }
    }
   
}

