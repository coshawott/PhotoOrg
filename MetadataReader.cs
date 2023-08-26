using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.AccessControl;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Iptc;
using SixLabors.ImageSharp.PixelFormats;
using MetadataExtractor;
using MetadataExtractor.Formats.Iptc;
using MetadataExtractor.Formats.Xmp;
using System.Linq;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace PhotoOrg
{
    class MetadataReader : IDisposable
    {
        private IReadOnlyList<Directory> imageTif;
        private SixLabors.ImageSharp.Image image;
        private string path;
        private bool disposed = false;

        public MetadataReader(string path)
        {
            if (path.EndsWith(".tif"))
            {
                imageTif = ImageMetadataReader.ReadMetadata(path);
            }
            else
            {
                try
                {
                    this.image = SixLabors.ImageSharp.Image.Load(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Exception:{ex}\n\n\nthrown at {path}");
                }
                
            }
            this.path = path;
        }

        public List<string> GetKeywordList()
        {
            if (path.EndsWith(".tif"))
            {
                List<string> keywordsList = new List<string>();

                try
                {
                    var directories = ImageMetadataReader.ReadMetadata(path);

                    foreach (var directory in directories)
                    {
                        if (directory is IptcDirectory iptcDirectory)
                        {
                            if (iptcDirectory.ContainsTag(IptcDirectory.TagKeywords))
                            {
                                var keywords = iptcDirectory.GetStringArray(IptcDirectory.TagKeywords);

                                if (keywords != null && keywords.Length > 0)
                                {
                                    keywordsList.AddRange(keywords);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading metadata: {ex.Message}");
                }

                return keywordsList;

            }
            else
            {
                try
                {
                    List<IptcValue> keywords = image.Metadata.IptcProfile.GetValues(IptcTag.Keywords);
                    List<string> strKeywords = new List<string>();
                    foreach (var keyword in keywords)
                    {
                        strKeywords.Add(keyword.Value);
                    }
                    return strKeywords;
                }
                catch (Exception ex)
                {
                    List<string> strKeywords = new List<string>();
                    strKeywords.Add("");
                    return strKeywords;
                }
            }
            
        }

        public List<string> GetNameList()
        {
            if (path.EndsWith(".tif"))
            {
                List<string> keywordsList = new List<string>();

                try
                {
                    var directories = ImageMetadataReader.ReadMetadata(path);

                    foreach (var directory in directories)
                    {
                        if (directory is IptcDirectory iptcDirectory)
                        {
                            if (iptcDirectory.ContainsTag(IptcDirectory.TagSpecialInstructions))
                            {
                                var keywords = iptcDirectory.GetStringArray(IptcDirectory.TagSpecialInstructions);

                                if (keywords != null && keywords.Length > 0)
                                {
                                    keywordsList.AddRange(keywords);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading metadata: {ex.Message}");
                    keywordsList.Add("");
                }
                if (keywordsList.Count == 0)
                {
                    keywordsList.Add("");
                }

                return keywordsList;

            }
            else
            {
                try
                {
                    List<IptcValue> keywords = image.Metadata.IptcProfile.GetValues(IptcTag.SpecialInstructions);
                    List<string> strKeywords = new List<string>();
                    foreach (var keyword in keywords)
                    {
                        strKeywords.Add(keyword.Value);
                    }
                    if (strKeywords.Count == 0)
                    {
                        strKeywords.Add("");
                    }
                    return strKeywords;
                }
                catch (Exception ex)
                {
                    List<string> strKeywords = new List<string>();
                    strKeywords.Add("");
                    if (strKeywords.Count == 0)
                    {
                        strKeywords.Add("");
                    }
                    return strKeywords;
                }
            }
        }


        public List<string> GetLocationList()
        {
            if (path.EndsWith(".tif"))
            {
                List<string> keywordsList = new List<string>();

                try
                {
                    var directories = ImageMetadataReader.ReadMetadata(path);

                    foreach (var directory in directories)
                    {
                        if (directory is IptcDirectory iptcDirectory)
                        {
                            if (iptcDirectory.ContainsTag(IptcDirectory.TagCaption))
                            {
                                var keywords = iptcDirectory.GetStringArray(IptcDirectory.TagCaption);

                                if (keywords != null && keywords.Length > 0)
                                {
                                    keywordsList.AddRange(keywords);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    keywordsList.Add("");
                }
                if (keywordsList.Count == 0)
                {
                    keywordsList.Add("");
                }
                return keywordsList;

            }
            else
            {
                try
                {
                    List<IptcValue> keywords = image.Metadata.IptcProfile.GetValues(IptcTag.Caption);
                    List<string> strKeywords = new List<string>();
                    foreach (var keyword in keywords)
                    {
                        strKeywords.Add(keyword.Value);
                        //strKeywords.Add(keyword.Value);
                    }
                    if (strKeywords.Count == 0)
                    {
                        strKeywords.Add("");
                    }
                    return strKeywords;
                }
                catch (Exception ex)
                {
                    List<string> strKeywords = new List<string>();
                    strKeywords.Add("");
                    if (strKeywords.Count == 0)
                    {
                        strKeywords.Add("");
                    }
                    return strKeywords;
                }
            }
        }


        public List<string> GetCaptionList()
        {
            try
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
                {
                    PropertyItem[] propertyItems = image.PropertyItems;

                    List<string> coolList = new List<string>();
                    foreach (PropertyItem propertyItem in propertyItems)
                    {
                        string metadata = Encoding.UTF7.GetString(propertyItem.Value);
                        Debug.WriteLine($"Property ID: 0x{propertyItem.Id:X} - Type: {propertyItem.Type} - Value: {metadata}");
                        coolList.Add($"Property ID: 0x{propertyItem.Id:X} - Type: {propertyItem.Type} - Value: {metadata}");
                    }

                    return coolList;
                }
            }
            catch (Exception)
            {
                List<string> coolList = new List<string>();
                return coolList;
            }
        }

        public string GetCaption()
        {
            if (path.EndsWith(".tif"))
            {
                List<string> keywordsList = new List<string>();

                try
                {
                    var directories = ImageMetadataReader.ReadMetadata(path);

                    foreach (var directory in directories)
                    {
                        if (directory is IptcDirectory iptcDirectory)
                        {
                            if (iptcDirectory.ContainsTag(IptcDirectory.TagObjectName))
                            {
                                var keywords = iptcDirectory.GetStringArray(IptcDirectory.TagObjectName);

                                if (keywords != null && keywords.Length > 0)
                                {
                                    keywordsList.AddRange(keywords);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    keywordsList.Add("");
                }
                if (keywordsList.Count == 0)
                {
                    keywordsList.Add("");
                }
                return keywordsList[0];

            }
            else
            {
                try
                {
                    List<IptcValue> keywords = image.Metadata.IptcProfile.GetValues(IptcTag.Name);
                    List<string> strKeywords = new List<string>();
                    foreach (var keyword in keywords)
                    {
                        strKeywords.Add(keyword.Value);
                        //strKeywords.Add(keyword.Value);
                    }
                    if (strKeywords.Count == 0)
                    {
                        strKeywords.Add("");
                    }
                    return strKeywords[0];
                }
                catch (Exception ex)
                {
                    List<string> strKeywords = new List<string>();
                    strKeywords.Add("");
                    if (strKeywords.Count == 0)
                    {
                        strKeywords.Add("");
                    }
                    return strKeywords[0];
                }
            }

        }

        public string GetDate()
        {
            if (path.EndsWith(".tif"))
            {
                List<string> keywordsList = new List<string>();

                try
                {
                    var directories = ImageMetadataReader.ReadMetadata(path);

                    foreach (var directory in directories)
                    {
                        if (directory is IptcDirectory iptcDirectory)
                        {
                            if (iptcDirectory.ContainsTag(IptcDirectory.TagFixtureId))
                            {
                                var keywords = iptcDirectory.GetStringArray(IptcDirectory.TagFixtureId);

                                if (keywords != null && keywords.Length > 0)
                                {
                                    keywordsList.AddRange(keywords);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    keywordsList.Add("");
                }
                if (keywordsList.Count == 0)
                {
                    keywordsList.Add("");
                }
                return keywordsList[0];

            }
            else
            {
                try
                {
                    List<IptcValue> keywords = image.Metadata.IptcProfile.GetValues(IptcTag.FixtureIdentifier);
                    List<string> strKeywords = new List<string>();
                    foreach (var keyword in keywords)
                    {
                        strKeywords.Add(keyword.Value);
                        //strKeywords.Add(keyword.Value);
                    }
                    if (strKeywords.Count == 0)
                    {
                        strKeywords.Add("");
                    }
                    return strKeywords[0];
                }
                catch (Exception ex)
                {
                    List<string> strKeywords = new List<string>();
                    strKeywords.Add("");
                    if (strKeywords.Count == 0)
                    {
                        strKeywords.Add("");
                    }
                    return strKeywords[0];
                }
            }

        }

        public string GetNotes()
        {
            if (path.EndsWith(".tif"))
            {
                List<string> keywordsList = new List<string>();

                try
                {
                    var directories = ImageMetadataReader.ReadMetadata(path);

                    foreach (var directory in directories)
                    {
                        if (directory is IptcDirectory iptcDirectory)
                        {
                            if (iptcDirectory.ContainsTag(IptcDirectory.TagOriginalTransmissionReference))
                            {
                                var keywords = iptcDirectory.GetStringArray(IptcDirectory.TagOriginalTransmissionReference);

                                if (keywords != null && keywords.Length > 0)
                                {
                                    keywordsList.AddRange(keywords);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    keywordsList.Add("");
                }
                if (keywordsList.Count == 0)
                {
                    keywordsList.Add("");
                }
                return keywordsList[0];

            }
            else
            {
                try
                {
                    List<IptcValue> keywords = image.Metadata.IptcProfile.GetValues(IptcTag.OriginalTransmissionReference);
                    List<string> strKeywords = new List<string>();
                    foreach (var keyword in keywords)
                    {
                        strKeywords.Add(keyword.Value);
                        //strKeywords.Add(keyword.Value);
                    }
                    if (strKeywords.Count == 0)
                    {
                        strKeywords.Add("");
                    }
                    return strKeywords[0];
                }
                catch (Exception ex)
                {
                    List<string> strKeywords = new List<string>();
                    strKeywords.Add("");
                    if (strKeywords.Count == 0)
                    {
                        strKeywords.Add("");
                    }
                    return strKeywords[0];
                }
            }

        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
                image?.Dispose();

                disposed = true;
            }
        }

        ~MetadataReader()
        {
            Dispose(false);
        }
    }
}
