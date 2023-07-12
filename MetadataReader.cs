using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.AccessControl;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Iptc;
using SixLabors.ImageSharp.PixelFormats;

namespace PhotoOrg
{
    class MetadataReader : IDisposable
    {
        private Image<Rgba32> image;
        private string path;
        private bool disposed = false;

        public MetadataReader(string path)
        {
            this.image = Image.Load<Rgba32>(path);
            this.path = path;
        }

        public string GetKeywords()
        {
            try
            {
                List<IptcValue> keywords = image.Metadata.IptcProfile.GetValues(IptcTag.Keywords);
                string strKeywords = "";
                foreach (var keyword in keywords)
                {
                    strKeywords += " \"" + keyword.Value + "\"";
                }
                return strKeywords;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex);
                return "";
            }
        }

        public List<string> GetKeywordList()
        {
            try
            {
                List<IptcValue> keywords = image.Metadata.IptcProfile.GetValues(IptcTag.Keywords);
                List<string> strKeywords = new List<string>();
                //string actualStrKeywords = "";
                foreach (var keyword in keywords)
                {
                    strKeywords.Add(keyword.Value);
                }
                //strKeywords.Add(actualStrKeywords);
                return strKeywords;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex);
                List<string> strKeywords = new List<string>();
                strKeywords.Add("");
                return strKeywords;
            }
        }

        public string GetCity()
        {
            try
            {
                List<IptcValue> keywords = image.Metadata.IptcProfile.GetValues(IptcTag.City);
                string strKeywords = "";
                foreach (var keyword in keywords)
                {
                    strKeywords += " \"" + keyword.Value + "\"";
                }
                return strKeywords;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex);
                return "";
            }
        }

        public List<string> GetCityList()
        {
            try
            {
                List<IptcValue> keywords = image.Metadata.IptcProfile.GetValues(IptcTag.City);
                List<string> strKeywords = new List<string>();
                foreach (var keyword in keywords)
                {
                    strKeywords.Add(keyword.Value);
                }
                return strKeywords;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex);
                List<string> strKeywords = new List<string>();
                strKeywords.Add("");
                return strKeywords;
            }
        }

        public string GetCountry()
        {
            try
            {
                List<IptcValue> keywords = image.Metadata.IptcProfile.GetValues(IptcTag.Country);
                string strKeywords = "";
                foreach (var keyword in keywords)
                {
                    strKeywords += " \"" + keyword.Value + "\"";
                }
                return strKeywords;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex);
                return "";
            }
        }

        public List<string> GetCountryList()
        {
            try
            {
                List<IptcValue> keywords = image.Metadata.IptcProfile.GetValues(IptcTag.Country);
                List<string> strKeywords = new List<string>();
                foreach (var keyword in keywords)
                {
                    strKeywords.Add(keyword.Value);
                }
                return strKeywords;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex);
                List<string> strKeywords = new List<string>();
                strKeywords.Add("");
                return strKeywords;
            }
        }

        public string GetName()
        {
            try
            {
                List<IptcValue> keywords = image.Metadata.IptcProfile.GetValues(IptcTag.Name);
                string strKeywords = "";
                foreach (var keyword in keywords)
                {
                    strKeywords += " \"" + keyword.Value + "\"";
                }
                return strKeywords;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex);
                return "";
            }
        }

        public List<string> GetNameList()
        {
            try
            {
                List<IptcValue> keywords = image.Metadata.IptcProfile.GetValues(IptcTag.Name);
                List<string> strKeywords = new List<string>();
                foreach (var keyword in keywords)
                {
                    strKeywords.Add(keyword.Value);
                }
                return strKeywords;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex);
                List<string> strKeywords = new List<string>();
                strKeywords.Add("");
                return strKeywords;
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
