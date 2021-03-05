using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;

namespace HCIRA502013
{
    [Serializable]
    public class Tip
    {
        public BitmapImage ikonica { get; set; }
        public string ikonicaUrl { get; set; }
        public string oznaka { get; set; }
        public string ime { get; set; }
        public string opis { get; set; }

        public Tip()
        {
        }

        public Tip(SeriaTip t)
        {
            this.ikonica = LoadImage(t.ikonica);
            this.ikonicaUrl = t.ikonicaUrl;
            this.oznaka = t.oznaka;
            this.ime = t.ime;
            this.opis = t.opis;
        }

        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
