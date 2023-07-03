using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Iptc;
using SixLabors.ImageSharp.PixelFormats;

namespace PhotoOrg
{
    class MetadataReader
    {
        public Image<Rgb24> image;
        public MetadataReader(string path) 
        {
            using (var tempImage = Image.Load<Rgb24>(path).Clone())
            {
                image = tempImage.Clone();
                tempImage.Dispose();
            }

        }

        public string GetKeywords()
        {
            try
            {
                List<IptcValue> keywords = image.Metadata.IptcProfile.GetValues(IptcTag.Keywords);
                string strKeywords = "";
                foreach (var keyword in keywords)
                {
                    strKeywords += " " + keyword.Value;
                }
                return strKeywords;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return "null";
            }
        }
    }
}
