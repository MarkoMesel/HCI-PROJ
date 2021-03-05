using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace HCIRA502013
{
    [Serializable]
    public class Etiketa
    {
        /*
        public event PropertyChangedEventHandler PropertyChanged;
        private string _oznaka;

        public string oznaka
        {
            get
            {
                Console.WriteLine("OZNAKA IZ GETTERA -> " + _oznaka);
                return _oznaka;
            }
            set
            {
                if (_oznaka != value)
                {
                    _oznaka = value;
                    Console.WriteLine("OVO JE VREDNOST IZ SETTERA -> " + value);
                    OnPropertyChanged("oznaka");
                }
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        */
        public string oznaka { get; set; }
        public string opis { get; set; }
        public string boja { get; set; }
    }
}
