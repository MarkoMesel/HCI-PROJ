using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;

namespace HCIRA502013
{
    [Serializable]
    public class Spomenik
    {
        public BitmapImage ikonica { get; set; }
        public string ikonicaUrl { get; set; }
        public string oznaka { get; set; }
        public string ime { get; set; }
        public string opis { get; set; }
        public Tip tip { get; set; }
        public string datum { get; set; }
        public string era { get; set; }
        public string status { get; set; }
        public string arhObr { get; set; }
        public string unesco { get; set; }
        public string nasReg { get; set; }
        public string prihod { get; set; }
        public List<Etiketa> etikete { get; set; }
        public Point location { get; set; }

        public Spomenik()
        {
            location = new Point(-1, -1);
        }

        public Spomenik(SeriaSpomenik s)
        {
            this.ikonica = LoadImage(s.ikonica);
            this.ikonicaUrl = s.ikonicaUrl;
            this.oznaka = s.oznaka;
            this.ime = s.ime;
            this.opis = s.opis;
            this.tip = new Tip(s.tip);
            this.datum = s.datum;
            this.era = s.era;
            this.status = s.status;
            this.arhObr = s.arhObr;
            this.unesco = s.unesco;
            this.nasReg = s.nasReg;
            this.prihod = s.prihod;
            this.etikete = s.etikete;
            this.location = s.location;
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
