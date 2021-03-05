using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCIRA502013
{
    public class TipChangeValidator
    {
        public Tip origin {get; set;}
        public Tip validator { get; set; }
        public String reservedWord;

        public TipChangeValidator(Tip o)
        {
            origin = o;
            validator = new Tip();
            validator = copyValues(origin, validator);
            reservedWord = origin.oznaka;
        }

        public Tip copyValues(Tip t1, Tip t2)
        {
            t2.ikonica = t1.ikonica;
            t2.ikonicaUrl = t1.ikonicaUrl;
            t2.oznaka = t1.oznaka;
            t2.ime = t1.ime;
            t2.opis = t1.opis;
            return t2;
        }
    }
}
