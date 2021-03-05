using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCIRA502013
{
    public class EtiketaChangeValidator
    {
        public Etiketa origin { get; set; }
        public Etiketa validator { get; set; }
        public String reservedWord;

        public EtiketaChangeValidator(Etiketa o)
        {
            origin = o;
            validator = new Etiketa();
            validator = copyValues(origin, validator);
            reservedWord = origin.oznaka;
        }

        public Etiketa copyValues(Etiketa e1, Etiketa e2)
        {
            e2.oznaka = e1.oznaka;
            e2.opis = e1.opis;
            e2.boja = e1.boja;
            return e2;
        }
    }
}
