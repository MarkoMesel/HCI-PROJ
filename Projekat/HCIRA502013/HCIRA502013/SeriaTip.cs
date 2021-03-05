using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;

namespace HCIRA502013
{
    [Serializable]
    public class SeriaTip
    {
        public byte[] ikonica { get; set; }
        public string ikonicaUrl { get; set; }
        public string oznaka { get; set; }
        public string ime { get; set; }
        public string opis { get; set; }

        public SeriaTip(Tip t)
        {
            this.ikonica = getJPGFromImageControl(t.ikonica);
            this.ikonicaUrl = t.ikonicaUrl;
            this.oznaka = t.oznaka;
            this.ime = t.ime;
            this.opis = t.opis;
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
