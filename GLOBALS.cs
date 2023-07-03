using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Iptc;
using SixLabors.ImageSharp.PixelFormats;

namespace PhotoOrg
{
    static class GLOBALS
    {
        public static Image<Rgb24> image = null;
        public static string path = null;
        public static Boolean initialOpen = true;
    }
}
