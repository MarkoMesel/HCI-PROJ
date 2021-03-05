using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCIRA502013
{
    public class SpomenikChangeValidator
    {
        public Spomenik origin {get; set;}
        public Spomenik validator { get; set; }
        public String reservedWord { get; set; } 

        public SpomenikChangeValidator(Spomenik o)
        {
            origin = o;
            validator = new Spomenik();
            validator = copyValues(origin, validator);
            reservedWord = origin.oznaka;
        }

        public Spomenik copyValues(Spomenik s1, Spomenik s2)
        {
            s2.ikonica = s1.ikonica;
            s2.ikonicaUrl = s1.ikonicaUrl;
            s2.oznaka = s1.oznaka;
            s2.ime = s1.ime;
            s2.opis = s1.opis;
            s2.tip = s1.tip;
            s2.datum = s1.datum;
            s2.era = s1.era;
            s2.status = s1.status;
            s2.arhObr = s1.arhObr;
            s2.unesco = s1.unesco;
            s2.nasReg = s1.nasReg;
            s2.prihod = s1.prihod;
            s2.etikete = s1.etikete;
            return s2;
        }
    }
}
