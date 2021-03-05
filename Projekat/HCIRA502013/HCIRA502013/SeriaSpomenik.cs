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
    public class SeriaSpomenik
    {
        public byte[] ikonica { get; set; }
        public string ikonicaUrl { get; set; }
        public string oznaka { get; set; }
        public string ime { get; set; }
        public string opis { get; set; }
        public SeriaTip tip { get; set; }
        public string datum { get; set; }
        public string era { get; set; }
        public string status { get; set; }
        public string arhObr { get; set; }
        public string unesco { get; set; }
        public string nasReg { get; set; }
        public string prihod { get; set; }
        public List<Etiketa> etikete { get; set; }
        public Point location { get; set; }

        public SeriaSpomenik(Spomenik s)
        {
            this.ikonica = getJPGFromImageControl(s.ikonica);
            this.ikonicaUrl = s.ikonicaUrl;
            this.oznaka = s.oznaka;
            this.ime = s.ime;
            this.opis = s.opis;
            this.tip = new SeriaTip(s.tip);
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

        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
    }
}
