using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows;

namespace HCIRA502013
{
    public class CheckIfEmptyRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            //TextBox tb = value as TextBox;
            try
            {
                var s = value as string;
                if (!string.IsNullOrWhiteSpace(s))
                {
                    //tb.ToolTip = new ToolTip { Content = "" };
                    return new ValidationResult(true, null);
                }

                if(s == "Izaberi opciju...")
                    return new ValidationResult(false, "Molim vas unesite neki drugi tekst.");
                //if(s )
                //tb.ToolTip = new ToolTip { Content = "Polje ne sme ostati prazno." };
                return new ValidationResult(false, "Polje ne sme ostati prazno.");
            }
            catch
            {
                //tb.ToolTip = new ToolTip { Content = "Podatak nije dobro unet." };
                return new ValidationResult(false, null);
            }
        }
    }

    public class CheckIfUniqueRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            //TextBox tb = value as TextBox;
            try
            {
                var s = value as string;
                MainWindow mw = (MainWindow) Application.Current.MainWindow;
                IEnumerable<Spomenik> spCheck = mw.spomenici.Where(x => x.oznaka == s);
                IEnumerable<Tip> tpCheck = mw.tipovi.Where(x => x.oznaka == s);
                IEnumerable<Etiketa> etCheck = mw.etikete.Where(x => x.oznaka == s);

                if (!spCheck.Any() && !tpCheck.Any() && !etCheck.Any())
                {
                    //tb.ToolTip = new ToolTip { Content = "" };
                    return new ValidationResult(true, null);
                }
                //tb.ToolTip = new ToolTip { Content = "Oznaka mora biti unikantna." };
                return new ValidationResult(false, "Oznaka mora biti unikantna.");
            }
            catch
            {
                //tb.ToolTip = new ToolTip { Content = "Podatak nije dobro unet." };
                return new ValidationResult(false, null);
            }
        }
    }
    /*
    public class CheckIfItHasPlaceholderRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            TextBox tb = value as TextBox;
            try
            {
                var s = value as string;
                if (s != "xx/xx/xxxx")
                {
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, null);
            }
            catch
            {
                return new ValidationResult(false, null);
            }
        }
    }
     */

    public class CheckIfUniqueAndNRSORule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            //TextBox tb = value as TextBox;
            try
            {
                var s = value as string;
                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                IEnumerable<Spomenik> spCheck = mw.spomenici.Where(x => x.oznaka == s);
                IEnumerable<Tip> tpCheck = mw.tipovi.Where(x => x.oznaka == s);
                IEnumerable<Etiketa> etCheck = mw.etikete.Where(x => x.oznaka == s);

                if ((!spCheck.Any() && !tpCheck.Any() && !etCheck.Any()) || s == mw.reservedSpomenikOznaka)
                {
                    //tb.ToolTip = new ToolTip { Content = "" };
                    return new ValidationResult(true, null);
                }
                //tb.ToolTip = new ToolTip { Content = "Oznaka mora biti unikantna." };
                return new ValidationResult(false, "Oznaka mora biti unikantna.");
            }
            catch
            {
                //tb.ToolTip = new ToolTip { Content = "Podatak nije dobro unet." };
                return new ValidationResult(false, null);
            }
        }
    }

    public class CheckIfUniqueAndNRSO2Rule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            //TextBox tb = value as TextBox;
            try
            {
                var s = value as string;
                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                IEnumerable<Spomenik> spCheck = mw.spomenici.Where(x => x.oznaka == s);
                IEnumerable<Tip> tpCheck = mw.tipovi.Where(x => x.oznaka == s);
                IEnumerable<Etiketa> etCheck = mw.etikete.Where(x => x.oznaka == s);

                if ((!spCheck.Any() && !tpCheck.Any() && !etCheck.Any()) || s == mw.SCV.reservedWord)
                {
                    //tb.ToolTip = new ToolTip { Content = "" };
                    return new ValidationResult(true, null);
                }
                //tb.ToolTip = new ToolTip { Content = "Oznaka mora biti unikantna." };
                return new ValidationResult(false, "Oznaka mora biti unikantna.");
            }
            catch
            {
                //tb.ToolTip = new ToolTip { Content = "Podatak nije dobro unet." };
                return new ValidationResult(false, null);
            }
        }
    }

    public class CheckIfUniqueAndNRTORule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            //TextBox tb = value as TextBox;
            try
            {
                var s = value as string;
                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                IEnumerable<Spomenik> spCheck = mw.spomenici.Where(x => x.oznaka == s);
                IEnumerable<Tip> tpCheck = mw.tipovi.Where(x => x.oznaka == s);
                IEnumerable<Etiketa> etCheck = mw.etikete.Where(x => x.oznaka == s);

                if ((!spCheck.Any() && !tpCheck.Any() && !etCheck.Any()) || s == mw.reservedTipOznaka)
                {
                    //tb.ToolTip = new ToolTip { Content = "" };
                    return new ValidationResult(true, null);
                }
                //tb.ToolTip = new ToolTip { Content = "Oznaka mora biti unikantna." };
                return new ValidationResult(false, "Oznaka mora biti unikantna.");
            }
            catch
            {
                //tb.ToolTip = new ToolTip { Content = "Podatak nije dobro unet." };
                return new ValidationResult(false, null);
            }
        }
    }

    public class CheckIfUniqueAndNRTO2Rule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            //TextBox tb = value as TextBox;
            try
            {
                var s = value as string;
                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                IEnumerable<Spomenik> spCheck = mw.spomenici.Where(x => x.oznaka == s);
                IEnumerable<Tip> tpCheck = mw.tipovi.Where(x => x.oznaka == s);
                IEnumerable<Etiketa> etCheck = mw.etikete.Where(x => x.oznaka == s);

                if ((!spCheck.Any() && !tpCheck.Any() && !etCheck.Any()) || s == mw.TCV.reservedWord)
                {
                    //tb.ToolTip = new ToolTip { Content = "" };
                    return new ValidationResult(true, null);
                }
                //tb.ToolTip = new ToolTip { Content = "Oznaka mora biti unikantna." };
                return new ValidationResult(false, "Oznaka mora biti unikantna.");
            }
            catch
            {
                //tb.ToolTip = new ToolTip { Content = "Podatak nije dobro unet." };
                return new ValidationResult(false, null);
            }
        }
    }

    public class CheckIfUniqueAndNREORule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            //TextBox tb = value as TextBox;
            try
            {
                var s = value as string;
                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                IEnumerable<Spomenik> spCheck = mw.spomenici.Where(x => x.oznaka == s);
                IEnumerable<Tip> tpCheck = mw.tipovi.Where(x => x.oznaka == s);
                IEnumerable<Etiketa> etCheck = mw.etikete.Where(x => x.oznaka == s);

                if ((!spCheck.Any() && !tpCheck.Any() && !etCheck.Any()) || s == mw.reservedEtiketaOznaka)
                {
                    //tb.ToolTip = new ToolTip { Content = "" };
                    return new ValidationResult(true, null);
                }
                //tb.ToolTip = new ToolTip { Content = "Oznaka mora biti unikantna." };
                return new ValidationResult(false, "Oznaka mora biti unikantna.");
            }
            catch
            {
                //tb.ToolTip = new ToolTip { Content = "Podatak nije dobro unet." };
                return new ValidationResult(false, null);
            }
        }
    }

    public class CheckIfUniqueAndNREO2Rule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            //TextBox tb = value as TextBox;
            try
            {
                var s = value as string;
                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                IEnumerable<Spomenik> spCheck = mw.spomenici.Where(x => x.oznaka == s);
                IEnumerable<Tip> tpCheck = mw.tipovi.Where(x => x.oznaka == s);
                IEnumerable<Etiketa> etCheck = mw.etikete.Where(x => x.oznaka == s);

                if ((!spCheck.Any() && !tpCheck.Any() && !etCheck.Any()) || s == mw.ECV.reservedWord)
                {
                    //tb.ToolTip = new ToolTip { Content = "" };
                    return new ValidationResult(true, null);
                }
                //tb.ToolTip = new ToolTip { Content = "Oznaka mora biti unikantna." };
                return new ValidationResult(false, "Oznaka mora biti unikantna.");
            }
            catch
            {
                //tb.ToolTip = new ToolTip { Content = "Podatak nije dobro unet." };
                return new ValidationResult(false, null);
            }
        }
    }

    public class CheckIfDateFormatRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            //TextBox tb = value as TextBox;
            try
            {
                var s = value as string;
                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                Regex r1 = new Regex(@"^\d{1,2}\/\d{1,2}\/\d{1,4}$");
                Regex r2 = new Regex(@"^\d{1,2}\-\d{1,2}\-\d{1,4}$");
                if (r1.IsMatch(s) || r2.IsMatch(s))
                {
                    //tb.ToolTip = new ToolTip { Content = "" };
                    return new ValidationResult(true, null);
                }
                //tb.ToolTip = new ToolTip { Content = "Niste dobro uneli datum." };
                return new ValidationResult(false, "Niste dobro uneli datum.");
            }
            catch
            {
                //tb.ToolTip = new ToolTip { Content = "Podatak nije dobro unet." };
                return new ValidationResult(false, null);
            }
        }
    }

    public class CheckIfOnlyNumbersRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            //TextBox tb = value as TextBox;
            try
            {
                var s = value as string;
                Regex r = new Regex(@"^\d+$");
                if (r.IsMatch(s))
                {
                    //tb.ToolTip = new ToolTip { Content = "" };
                    return new ValidationResult(true, null);
                }
                //tb.ToolTip = new ToolTip { Content = "Tekst mora sadrzati samo brojeve." };
                return new ValidationResult(false, "Tekst mora sadrzati samo brojeve.");
            }
            catch
            {
                //tb.ToolTip = new ToolTip { Content = "Podatak nije dobro unet." };
                return new ValidationResult(false, null);
            }
        }
    }
}
