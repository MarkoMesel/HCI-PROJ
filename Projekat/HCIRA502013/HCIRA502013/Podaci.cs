using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace HCIRA502013
{
    [Serializable]
    public class Podaci
    {
        public ObservableCollection<SeriaSpomenik> seriaSpomenici {get; set;}
        public ObservableCollection<SeriaTip> seriaTipovi {get; set; }
        public ObservableCollection<Etiketa> etikete {get; set; }
        public byte[] mapa { get; set; }

        public Podaci()
        {
            seriaSpomenici = new ObservableCollection<SeriaSpomenik>();
            seriaTipovi = new ObservableCollection<SeriaTip>();
            etikete = new ObservableCollection<Etiketa>();
        }
    }
}
