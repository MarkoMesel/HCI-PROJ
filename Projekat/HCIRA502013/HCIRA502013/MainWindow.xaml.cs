using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;
using System.Windows.Resources;
using Xceed.Wpf.Toolkit;
using System.Windows.Threading;

namespace HCIRA502013
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //public event PropertyChangedEventHandler PropertyChanged;
        //private Boolean _SpIsValid = true;
        //private Boolean _TpIsValid = true;
        //private Boolean _EtIsValid = true;

        public Podaci podaci;

        public Image draggedImage;
        public Point mousePosition;
        public Spomenik tempSp;

        public Point startPoint = new Point();

        public string tipImagePath { get; set; }
        public string spomenikImagePath { get; set; }

        public string reservedSpomenikOznaka { get; set; }
        public string reservedTipOznaka { get; set; }
        public string reservedEtiketaOznaka { get; set; }

        public Dictionary<string, Boolean> validators = new Dictionary<string,Boolean>();

        public Spomenik spomenikIzmeneValidation;
        public Tip tipIzmeneValidation;
        public Etiketa etiketaIzmeneValidation;

        public Spomenik spomenikValidation;
        public Tip tipValidation;
        public Etiketa etiketaValidation { get; set; }

        public ObservableCollection<Spomenik> spomenici = new ObservableCollection<Spomenik>();
        public ObservableCollection<Tip> tipovi = new ObservableCollection<Tip>();
        public ObservableCollection<Etiketa> etikete = new ObservableCollection<Etiketa>();

        public Dictionary<String, TabItem> taboviZaElemente = new Dictionary<String, TabItem>();

        public SpomenikChangeValidator SCV;
        public TipChangeValidator TCV;
        public EtiketaChangeValidator ECV;
        
        private Boolean labelClicked = false;

        public Boolean dragCollisionEnabled = false;
        public Boolean koristiIkonicuSpomenika = true;
        public Cursor currentCursor = Cursors.SizeAll;

        private BitmapImage mapa {get; set;}

        public Boolean izlistajSpomenikeWasPressed = false;
        public Boolean izlistajTipoveWasPressed = false;
        public Boolean izlistajEtiketeWasPressed = false;

        public ObservableCollection<Xceed.Wpf.Toolkit.ColorItem> bojeZaFilter = new ObservableCollection<Xceed.Wpf.Toolkit.ColorItem>();

        public DemoMod demo { get; set; }

        public static RoutedCommand kreiraj = new RoutedCommand();
        public static RoutedCommand nadji = new RoutedCommand();
        public static RoutedCommand izmeni = new RoutedCommand();
        public static RoutedCommand obrisi = new RoutedCommand();

        public MainWindow()
        {
            InitializeComponent();
            //currentCursor = ((Image)this.Resources["eraserCursor"]).Cursor;

            kreiraj.InputGestures.Add(new KeyGesture(Key.F1, ModifierKeys.None));
            nadji.InputGestures.Add(new KeyGesture(Key.F2, ModifierKeys.None));
            izmeni.InputGestures.Add(new KeyGesture(Key.F3, ModifierKeys.None));
            obrisi.InputGestures.Add(new KeyGesture(Key.F4, ModifierKeys.None));

            demo = new DemoMod();

            loadPodatke();

            /*
            for (int i = 0; i < 19; i++)
            {
                Button newBtn = new Button();

                newBtn.Content = i.ToString();
                newBtn.Name = "Button" + i.ToString();

                stackPanel2.Children.Add(newBtn);
            }
             */
            tempSp = new Spomenik();

            //validators.Add("spomenik",true);
            //validators.Add("tip", true);
            //validators.Add("entitet", true);

            validators["spomenik"] = true;
            validators["tip"] = true;
            validators["etiketa"] = true;

            spomenikIzmeneValidation = new Spomenik();
            tipIzmeneValidation = new Tip();
            etiketaIzmeneValidation = new Etiketa();

            spomenikValidation = new Spomenik();
            tipValidation = new Tip();
            etiketaValidation = new Etiketa();

            grid6.DataContext = spomenikIzmeneValidation;
            grid7.DataContext = tipIzmeneValidation;
            grid8.DataContext = etiketaIzmeneValidation;

            spomenikTab.DataContext = spomenikValidation;
            tipTab.DataContext = tipValidation;
            etiketaTab.DataContext = etiketaValidation;

            //dataGrid3.ItemsSource = etikete;

            tabControl1.Items.Remove(spomenikTab);
            tabControl1.Items.Remove(tipTab);
            tabControl1.Items.Remove(etiketaTab);
            tabControl1.Items.Remove(helpTab);

            etiketaValidation.oznaka = "TEST!!!";

            selectLabel(mapaLabel);

            foreach (UIElement e in grid5.Children)
            {
                ComboBox cb = e as ComboBox;
                if (cb != null)
                {
                    cb.IsEditable = true;
                    cb.IsReadOnly = false;
                    cb.IsEnabled = true;
                    cb.Visibility = Visibility.Hidden;
                }
            }

            foreach (UIElement e in gridFiltriranjeTip.Children)
            {
                ComboBox cb = e as ComboBox;
                if (cb != null)
                {
                    cb.IsEditable = true;
                    cb.IsReadOnly = false;
                    cb.IsEnabled = true;
                    cb.Visibility = Visibility.Hidden;
                }
            }

            foreach (UIElement e in gridFiltriranjeEtiketa.Children)
            {
                ComboBox cb = e as ComboBox;
                if (cb != null)
                {
                    cb.IsEditable = true;
                    cb.IsReadOnly = false;
                    cb.IsEnabled = true;
                    cb.Visibility = Visibility.Hidden;
                }
            }

            refreshSpomenikFilters();
            refreshTipFilters();
            refreshEtiketaFilters();

            moveGridZaBrisanjeIDugmeRight(true);
        }

        private void validate(DependencyObject element, string s)
        {
            if (validators[s])
            {
                validators[s] = !Validation.GetHasError(element);
            }
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (labelClicked == true)
            {
                Label l = sender as Label;
                /*
                if (currentLabel != selectedLabel)
                {
                    deselectLabel(selectedLabel);
                    selectLabel(currentLabel);
                    showPanel(currentLabel.Content.ToString());
                }
                 */
                selectLabelAndPanel(l);
                /*
                deselectAllLabels(l.Parent as Panel);
                selectLabel(l);
                showPanel(l.Content.ToString());
                 */
            }
        }

        private void selectLabel(Label l)
        {
            removeHighlightFromLabel(l);
            l.FontWeight = FontWeights.Bold;
            l.Foreground = Brushes.Black;
            //selectedLabel = l;
            updateBlueLabel(l.Content.ToString());
        }

        private void updateBlueLabel(String s)
        {
            label1.Content = "Glavna stranica - " + s;
            switch (s)
            {
                case "Tipovi":
                    updateBlueLabelButtons("Tip");
                    break;
                case "Etikete":
                    updateBlueLabelButtons("Etiketu");
                    break;
                case "Mapa":
                    button5.Content = "Ocisti Mapu";
                    button6.Visibility=Visibility.Hidden;
                    button7.Visibility=Visibility.Hidden;
                    button8.Visibility = Visibility.Hidden;
                    button10.Visibility = Visibility.Hidden;
                    break;
                default:
                    updateBlueLabelButtons("Spomenik");
                    break;
            }
        }

        private void updateBlueLabelButtons(String s)
        {
            button5.Content = "Kreiraj " + s;
            button6.Visibility = Visibility.Visible;
            button6.Content = "Izmeni " + s;
            button7.Visibility = Visibility.Visible;
            button7.Content = "Izbrisi " + s;
            button8.Visibility = Visibility.Visible;
            button8.Content = "Nadji " + s;
            button10.Visibility = Visibility.Visible;
        }

        private void deselectAllLabels(Panel parent)
        {
            if (parent == null) return;
            var elements = parent.Children.OfType<Label>().Where(x => x.FontWeight.Equals(FontWeights.Bold));
            //var elements = parent.Children.OfType<Label>();
            if (elements != null)
            {
                foreach (Label el in elements)
                {
                    el.FontWeight = FontWeights.Regular;
                    el.Foreground = Brushes.Black;
                }
            }
        }

        private void highlightLabel(Label l)
        {
            l.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            //l.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
        }

        private void removeHighlightFromLabel(Label l)
        {
            //l.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            l.Background = rectBorderGlavnaStranica.Fill;
        }

        private void showPanel(string s)
        {
            switch (s)
            {
                case "Spomenici":
                    bringToFront(spomenikPanel);
                    break;
                case "Tipovi":
                    bringToFront(tipPanel);
                    break;
                case "Etikete":
                    bringToFront(etiketaPanel);
                    break;
                case "Mapa":
                    bringToFront(mapaPanel);
                    break;
                default:
                    break;
            }
        }

        public static void bringToFront(Grid element)
        {
            if (element == null) return;

            Panel parent = element.Parent as Panel;
            if (parent == null) return;
            /*
            var maxZ = parent.Children.OfType<UIElement>()
              .Where(x => x != element)
              .Select(x => Panel.GetZIndex(x))
              .Max();
             */
            var elements = parent.Children.OfType<UIElement>().Where(x => x != element);
            foreach (UIElement el in elements)
            {
                Panel.SetZIndex(el, 0);
            }
            Panel.SetZIndex(element, 1);
        }

        private void label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            highlightLabel(label);
        }

        private void label_MouseLeave(object sender, MouseEventArgs e)
        {
            labelClicked = false;
            Label label = sender as Label;
            removeHighlightFromLabel(label);
        }

        private void expander1_Expanded(object sender, RoutedEventArgs e)
        {

        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showSpomenikIzmenePanels();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void scrollViewer1_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

        }

        private void label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            labelClicked = true;
        }

        private void scrollViewer1_ScrollChanged_1(object sender, ScrollChangedEventArgs e)
        {

        }

        private void tabControl1_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            /*
            TabItem ti = tabControl1.SelectedItem as TabItem;
            if (ti != null)
            {
                switch (ti.Header.ToString())
                {
                    case "Spomenik":
                        this.DataContext = dataForValidation["spomenik"];
                        break;
                    case "Tip":
                        this.DataContext = dataForValidation["tip"];
                        break;
                    case "Etiketa":
                        this.DataContext = dataForValidation["etiketa"];
                        break;
                    default:
                        break;

                }
            }
             */

        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showTipIzmenePanels();
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBox1.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void createSpomenik_Click(object sender, RoutedEventArgs e)
        {
            if (spomenikValidated())
                createNewSpomenik();
        }

        private void findSpomenik_Click(object sender, RoutedEventArgs e)
        {
            if (spomenikValidated())
            {
                Spomenik s = spomenikFound();
                if (s != null)
                {
                    findExistingSpomenik(s);
                }
            }
        }

        public void findExistingSpomenik(Spomenik s)
        {
            switchBackToHomeTab(spomenikTab);
            dataGrid1.UnselectAll();
            dataGrid1.SelectedItem = s;
            dataGrid1.CurrentCell = dataGrid1.SelectedCells.ElementAt(0);
        }

        public void findExistingTip(Tip t)
        {
            switchBackToHomeTab(tipTab);
            dataGrid2.UnselectAll();
            dataGrid2.SelectedItem = t;
            dataGrid2.CurrentCell = dataGrid2.SelectedCells.ElementAt(0);
        }

        public void findExistingEtiketa(Etiketa e)
        {
            switchBackToHomeTab(etiketaTab);
            dataGrid3.UnselectAll();
            dataGrid3.SelectedItem = e;
            dataGrid3.CurrentCell = dataGrid3.SelectedCells.ElementAt(0);
        }
        /*
        public static bool spomeniciSuJednaki(Spomenik s1, Spomenik s2)
        {
            PropertyInfo[] p1 = typeof(Spomenik).GetProperties();
            foreach (PropertyInfo property in p1)
            {
                property.SetValue(record, value);
            }
        }
         */

        public bool reflectiveEquals(object first, object second)
        {
            if (first == null && second == null)
            {
                return true;
            }
            if (first == null || second == null)
            {
                return false;
            }
            Type firstType = first.GetType();
            if (second.GetType() != firstType)
            {
                return false; // Or throw an exception
            }
            // This will only use public properties. Is that enough?
            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    object firstValue = propertyInfo.GetValue(first, null);
                    object secondValue = propertyInfo.GetValue(second, null);
                    String ignore = propertyInfo.Name;
                    if (ignore != "location" && ignore != "ikonica" && ignore != "ikonicaUrl")
                    {
                        if (ignore == "tip")
                        {
                            Tip t1 = (Tip)firstValue;
                            Tip t2 = (Tip)secondValue;
                            if(t1. oznaka != t2.oznaka)
                            {
                                return false;
                            }
                        }
                        else if (ignore == "etikete")
                        {
                            List<Etiketa> e1 = (List<Etiketa>)firstValue;
                            List<Etiketa> e2 = (List<Etiketa>)secondValue;
                            foreach (Etiketa e in e1)
                            {
                                if (!e2.Contains(e))
                                    return false;
                            }
                        }
                        else
                        {
                            if (!object.Equals(firstValue, secondValue))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public Spomenik spomenikFound()
        {
            getAllSpomenikRelatedValues();
            foreach (Spomenik s in spomenici)
            {
                if (reflectiveEquals(spomenikValidation, s))
                {
                    //if (imagesAreEqual(spomenikValidation.ikonica, s.ikonica))
                    //{
                        return s;
                    //}
                }
            }
            return null;
        }

        public Tip tipFound()
        {
            getAllTipRelatedValues();
            foreach (Tip t in tipovi)
            {
                if (reflectiveEquals(tipValidation, t))
                {
                    //if (imagesAreEqual(spomenikValidation.ikonica, s.ikonica))
                    //{
                    return t;
                    //}
                }
            }
            return null;
        }

        public Etiketa etiketaFound()
        {
            etiketaValidation.boja = colorPicker1.SelectedColor.ToString();
            foreach (Etiketa e in etikete)
            {
                if (reflectiveEquals(etiketaValidation, e))
                {
                    //if (imagesAreEqual(spomenikValidation.ikonica, s.ikonica))
                    //{
                    return e;
                    //}
                }
            }
            return null;
        }

        public Boolean imagesAreEqual(BitmapImage image1, BitmapImage image2)
        {
            if (image1 == null || image2 == null)
            {
                return false;
            }
            return ToBytes(image1).SequenceEqual(ToBytes(image2));
        }

        public static byte[] ToBytes(BitmapImage image)
        {
            byte[] data = new byte[] { };
            if (image != null)
            {
                try
                {
                    var encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        data = ms.ToArray();
                    }
                    return data;
                }
                catch (Exception ex)
                {
                }
            }
            return data;
        }

        private void findTip_Click(object sender, RoutedEventArgs e)
        {
            if (tipValidated("ignore image"))
            {
                Tip t = tipFound();
                if (t != null)
                {
                    findExistingTip(t);
                }
            }
        }

        private void findEtiketa_Click(object sender, RoutedEventArgs e)
        {
            if (etiketaValidated())
            {
                Etiketa et = etiketaFound();
                if (et != null)
                {
                    findExistingEtiketa(et);
                }
            }
        }

        private Boolean spomenikValidated()
        {
            validators["spomenik"] = true;
            validateSpomenik();
            if (validators["spomenik"])
                return true;
            return false;
        }

        private void changeSpomenik_Click(object sender, RoutedEventArgs e)
        {
            if (spomenikValidated())
                changeSpomenik();
        }

        public void createNewSpomenik()
        {
            getAllSpomenikRelatedValues();
            spomenikValidation.location = new Point(-1, -1);

            spomenici.Add(spomenikValidation);

            switchBackToHomeTab(spomenikTab);
            selectLabelAndPanel(spomenikLabel);
            refreshSpomenikRelatedContainers();
        }

        private void changeSpomenik()
        {
            getAllSpomenikRelatedValues();

            SCV.origin = SCV.copyValues(SCV.validator, SCV.origin);

            switchBackToHomeTab(spomenikTab);
            selectLabelAndPanel(spomenikLabel);
            refreshSpomenikRelatedContainers();
        }

        public void getAllSpomenikRelatedValues()
        {
            spomenikValidation.tip = (Tip) comboBox1.SelectedItem;
            spomenikValidation.era = comboBox2.Text;
            spomenikValidation.status = comboBox3.Text;

            spomenikValidation.arhObr = getStringFromCheckBox(checkBox1);
            spomenikValidation.unesco = getStringFromCheckBox(checkBox2);
            spomenikValidation.nasReg = getStringFromCheckBox(checkBox3);

            spomenikValidation.etikete = new List<Etiketa>();

            foreach (Etiketa et in listBox1.SelectedItems)
            {
                spomenikValidation.etikete.Add(et);
                Console.WriteLine("OVO JE OZNAKA TRENUTNOG ENTITETA -> " + et.oznaka);
            }

            if (image1.Source == null)
            {
                spomenikValidation.ikonicaUrl = spomenikValidation.tip.ikonicaUrl;
                spomenikValidation.ikonica = spomenikValidation.tip.ikonica as BitmapImage;
            }
            else
            {
                spomenikValidation.ikonicaUrl = spomenikImagePath;
                spomenikValidation.ikonica = image1.Source as BitmapImage;
            }
            spomenikImagePath = null;
        }

        public void getAllTipRelatedValues()
        {
            tipValidation.ikonicaUrl = tipImagePath;
            tipValidation.ikonica = image2.Source as BitmapImage;
            tipImagePath = null;
        }

        public void refreshSpomenikRelatedContainers()
        {
            dataGrid1.ItemsSource = spomenici;
            dataGrid1.Items.Refresh();

            listBox3.ItemsSource = spomenici;
            listBox3.Items.Refresh();

            showSpomenikIzmenePanels();

            foreach (Spomenik s in spomenici)
            {
                refreshImageOnMap(s.oznaka);
            }

            setAllImagesOnMap();

            refreshSpomenikFilters();
            if (izlistajSpomenikeWasPressed)
            {
                List<Spomenik> filtriraniSpomenici = findFilteredSpomenike();
                izlistajSpomenike(filtriraniSpomenici);
            }
        }

        public void refreshTipRelatedContainers()
        {
            dataGrid2.ItemsSource = tipovi;
            dataGrid2.Items.Refresh();

            int selectedIndex = comboBox1.SelectedIndex;
            comboBox1.SelectedIndex = -1;
            comboBox1.ItemsSource = tipovi;
            comboBox1.Items.Refresh();
            comboBox1.SelectedIndex = selectedIndex;

            selectedIndex = comboBox4.SelectedIndex;
            comboBox4.SelectedIndex = -1;
            comboBox4.ItemsSource = tipovi;
            comboBox4.Items.Refresh();
            comboBox4.SelectedIndex = selectedIndex;

            showTipIzmenePanels();

            refreshTipFilters();
            if (izlistajTipoveWasPressed)
            {
                List<Tip> filtriraniTipovi = findFilteredTipove();
                izlistajTipove(filtriraniTipovi);
            }
        }

        public void refreshImageOnMap(string s)
        {
            foreach (UIElement e in canvas1.Children)
            {
                Image i0 = (Image)e;
                Spomenik sp = (Spomenik)i0.Tag;
                string s1 = sp.oznaka;

                if (s1 == s)
                {
                    i0.Source = sp.ikonica;
                    break;
                }
            }
        }

        public void refreshEtiketaRelatedContainers()
        {
            dataGrid3.ItemsSource = etikete;
            dataGrid3.Items.Refresh();

            listBox1.ItemsSource = etikete;
            listBox1.Items.Refresh();

            listBox2.ItemsSource = etikete;
            listBox2.Items.Refresh();

            showEtiketaIzmenePanels();

            refreshEtiketaFilters();
            if (izlistajEtiketeWasPressed)
            {
                List<Etiketa> filtriraneEtikete = findFilteredEtikete();
                izlistajEtikete(filtriraneEtikete);
            }
        }

        private string getStringFromCheckBox(CheckBox c)
        {
            if (c.IsChecked == true)
                return "Da";
            return "Ne";
                
        }

        private void validateSpomenik()
        {
            string s = "spomenik";
            var elements = grid1.Children.OfType<TextBox>();
            foreach (TextBox t in elements)
            {
                t.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                validate(t, s);
            }

            if (tipovi.Count <= 0)
            {
                validators["spomenik"] = false;
                tipNeMozeDaSeBira();
            }
            else
            {
                tipMozeDaSeBira();
            }
        }

        private void tipMozeDaSeBira()
        {
            //label20.Visibility = Visibility.Visible;
            label7.Visibility = Visibility.Hidden;
            label7.FontWeight = FontWeights.Regular;
            label7.Foreground = Brushes.Black;
            if (comboBox1.SelectedIndex == -1)
                comboBox1.SelectedIndex = 0;
            comboBox1.IsEnabled = true;
            comboBox1.ItemsSource = tipovi;
            comboBox1.Items.Refresh();
        }

        private void tipNeMozeDaSeBira()
        {
            //label20.Visibility = Visibility.Hidden;
            label7.Visibility = Visibility.Visible;
            label7.FontWeight = FontWeights.Bold;
            label7.Foreground = Brushes.Red;
            comboBox1.IsEnabled = false;
            comboBox1.SelectedIndex = -1;
            comboBox1.Text = "";
        }

        private void createEtiketa_Click(object sender, RoutedEventArgs e)
        {
            if (etiketaValidated())
                createNewEtiketa();
        }

        private void changeEtiketa_Click(object sender, RoutedEventArgs e)
        {
            if (etiketaValidated())
                changeEtiketa();
        }

        public Boolean etiketaValidated()
        {
            validators["etiketa"] = true;
            validateEtiketa();
            if (validators["etiketa"])
                return true;
            return false;
        }

        private void validateEtiketa()
        {
            string s = "etiketa";
            var elements = grid3.Children.OfType<TextBox>();
            foreach (TextBox t in elements)
            {
                t.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                validate(t, s);
            }
        }

        private void createNewEtiketa()
        {
            etiketaValidation.boja = colorPicker1.SelectedColor.ToString();
            
            etikete.Add(etiketaValidation);

            switchBackToHomeTab(etiketaTab);
            selectLabelAndPanel(etiketeLabel);
            refreshEtiketaRelatedContainers();
        }

        private void changeEtiketa()
        {
            etiketaValidation.boja = colorPicker1.SelectedColor.ToString();

            ECV.origin = ECV.copyValues(ECV.validator, ECV.origin);

            switchBackToHomeTab(etiketaTab);
            selectLabelAndPanel(etiketeLabel);
            refreshEtiketaRelatedContainers();
            refreshSpomenikRelatedContainers();
        }

        private void switchBackToHomeTab(TabItem t)
        {
            tabControl1.Items.Remove(t);
            tabControl1.SelectedItem = homeTab;
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            String s = (sender as Button).Content.ToString();
            Console.WriteLine("OVO JE TAJ STRING -> " + s);
            switch (s)
            {
                case "Kreiraj Spomenik":
                    setUpCreateTab(spomenikTab);
                    break;
                case "Kreiraj Tip":
                    setUpCreateTab(tipTab);
                    break;
                case "Kreiraj Etiketu":
                    setUpCreateTab(etiketaTab);
                    break;
                case "Ocisti Mapu":
                    ocistiMapu();
                    break;
                default:
                    break;
            }
        }

        public void ocistiMapu()
        {
            canvas1.Children.Clear();
            foreach (Spomenik s in spomenici)
                s.location = new Point(-1, -1);
        }

        private void selectTab(TabItem t)
        {
            t.Visibility = Visibility.Visible;
            tabControl1.SelectedItem = t;
        }

        private void setUpCreateTab(TabItem t)
        {
            try
            {
                tabControl1.Items.Add(t);
            }
            catch
            {
                //
            }
            setTabDefaultValues(t.Header.ToString());
            selectTab(t);
        }

        private void setUpNadjiTab(TabItem t)
        {
            try
            {
                tabControl1.Items.Add(t);
            }
            catch
            {
                //
            }
            setTabNadjiValues(t.Header.ToString());
            selectTab(t);
        }

        private void setTabDefaultValues(string s)
        {
            switch (s)
            {
                case "Tip":
                    tipValidation = new Tip();
                    tipTab.DataContext = tipValidation;
                    image2.Source = null;

                    labelNoviTip.Content = "Kreiraj novi tip";
                    button3.Content = "Kreiraj novi tip";

                    removeClickEvent(button3, "t");

                    button3.Click += createTip_Click;

                    switchValidationRules(textBoxTipOznaka, new CheckIfUniqueRule());

                    break;
                case "Etiketa":
                    etiketaValidation = new Etiketa();
                    etiketaTab.DataContext = etiketaValidation;
                    colorPicker1.SelectedColor = Colors.White;

                    labelNovaEtiketa.Content = "Kreiraj novu etiketu";
                    button4.Content = "Kreiraj novu etiketu";

                    removeClickEvent(button4, "e");

                    button4.Click += createEtiketa_Click;

                    switchValidationRules(textBoxEtiketaOznaka, new CheckIfUniqueRule());

                    break;
                case "Spomenik":
                    spomenikValidation = new Spomenik();
                    spomenikTab.DataContext = spomenikValidation;
                    comboBox2.SelectedIndex = 0;
                    comboBox3.SelectedIndex = 0;
                    checkBox1.IsChecked = false;
                    checkBox2.IsChecked = false;
                    checkBox3.IsChecked = false;
                    comboBox1.SelectedIndex = 0;
                    comboBox1.ItemsSource = tipovi;
                    comboBox1.Items.Refresh();
                    listBox1.ItemsSource = etikete;
                    listBox1.Items.Refresh();
                    listBox1.SelectedItems.Clear();
                    image1.Source = null;

                    labelNoviSpomenik.Content = "Kreiraj novi spomenik";
                    button2.Content = "Kreiraj novi spomenik";

                    removeClickEvent(button2, "s");

                    button2.Click += createSpomenik_Click;

                    switchValidationRules(textBox1, new CheckIfUniqueRule());

                    break;
                default:
                    break;
            }
        }

        private void setTabNadjiValues(string s)
        {
            switch (s)
            {
                case "Tip":
                    tipValidation = new Tip();
                    tipTab.DataContext = tipValidation;
                    image2.Source = null;

                    labelNoviTip.Content = "Nadji tip";
                    button3.Content = "Nadji tip";

                    removeClickEvent(button3, "t");

                    button3.Click += findTip_Click;

                    switchValidationRules(textBoxTipOznaka, null);

                    break;
                case "Etiketa":
                    etiketaValidation = new Etiketa();
                    etiketaTab.DataContext = etiketaValidation;
                    colorPicker1.SelectedColor = Colors.White;

                    labelNovaEtiketa.Content = "Nadji etiketu";
                    button4.Content = "Nadji etiketu";

                    removeClickEvent(button4, "e");

                    button4.Click += findEtiketa_Click;

                    switchValidationRules(textBoxEtiketaOznaka, null);

                    break;
                case "Spomenik":
                    spomenikValidation = new Spomenik();
                    spomenikTab.DataContext = spomenikValidation;
                    comboBox2.SelectedIndex = 0;
                    comboBox3.SelectedIndex = 0;
                    checkBox1.IsChecked = false;
                    checkBox2.IsChecked = false;
                    checkBox3.IsChecked = false;
                    comboBox1.SelectedIndex = 0;
                    comboBox1.ItemsSource = tipovi;
                    comboBox1.Items.Refresh();
                    listBox1.ItemsSource = etikete;
                    listBox1.Items.Refresh();
                    listBox1.SelectedItems.Clear();
                    image1.Source = null;

                    labelNoviSpomenik.Content = "Nadji spomenik";
                    button2.Content = "Nadji spomenik";

                    removeClickEvent(button2, "s");

                    button2.Click += findSpomenik_Click;

                    switchValidationRules(textBox1, null);

                    break;
                default:
                    break;
            }
        }

        private void removeClickEvent(Button b, string s)
        {
            switch (s)
            {
                case "s":
                    b.Click -= createSpomenik_Click;
                    b.Click -= changeSpomenik_Click;
                    b.Click -= findSpomenik_Click;
                    break;
                case "t":
                    b.Click -= createTip_Click;
                    b.Click -= changeTip_Click;
                    b.Click -= findTip_Click;
                    break;
                case "e":
                    b.Click -= createEtiketa_Click;
                    b.Click -= changeEtiketa_Click;
                    b.Click -= findEtiketa_Click;
                    break;
                default:
                    break;
            }
        }

        private void textBoxEtiketaOznaka_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxEtiketaOznaka.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            //etiketaValidation.oznaka = textBoxEtiketaOznaka.Text;
            //Console.WriteLine("OVO JE OZNAKA KADA SE PROMENIO TEXT -> " + etiketaValidation.oznaka);
        }

        private void createTip_Click(object sender, RoutedEventArgs e)
        {
            if (tipValidated(""))
                createNewTip();
        }

        public Boolean tipValidated(string ignore)
        {
            validators["tip"] = true;
            validateTip(ignore);
            if (validators["tip"])
                return true;
            return false;
        }

        private void changeTip_Click(object sender, RoutedEventArgs e)
        {
            if (tipValidated(""))
                changeTip();
        }

        private void validateTip(string ignore)
        {
            string s = "tip";
            var elements = grid2.Children.OfType<TextBox>();
            foreach (TextBox t in elements)
            {
                t.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                validate(t, s);
            }
            if (ignore != "ignore image")
            {
                if (image2.Source == null)
                {
                    validators["tip"] = false;
                    labelTipIkonica2.FontWeight = FontWeights.Bold;
                    labelTipIkonica2.Foreground = Brushes.Red;
                    labelTipIkonica2.Visibility = Visibility.Visible;
                }
                else
                {
                    labelTipIkonica2.FontWeight = FontWeights.Regular;
                    labelTipIkonica2.Foreground = Brushes.Black;
                    labelTipIkonica2.Visibility = Visibility.Hidden;
                }
            }
        }

        private void createNewTip()
        {
            getAllTipRelatedValues();

            tipovi.Add(tipValidation);
            switchBackToHomeTab(tipTab);
            selectLabelAndPanel(tipoviLabel);
            dataGrid2.ItemsSource = tipovi;
            dataGrid2.Items.Refresh();
            tipMozeDaSeBira();
            refreshSpomenikRelatedContainers();
            refreshTipRelatedContainers();
        }

        private void changeTip()
        {
            getAllTipRelatedValues();

            TCV.origin = TCV.copyValues(TCV.validator, TCV.origin);
            switchBackToHomeTab(tipTab);
            selectLabelAndPanel(tipoviLabel);
            dataGrid2.ItemsSource = tipovi;
            dataGrid2.Items.Refresh();
            refreshSpomenikRelatedContainers();
            refreshTipRelatedContainers();
            tipMozeDaSeBira();
        }

        private void selectLabelAndPanel(Label l)
        {
            deselectAllLabels(l.Parent as Panel);
            selectLabel(l);
            izbrisiGrid.Visibility = Visibility.Hidden;
            moveGridZaBrisanjeIDugmeRight(false);
            izmeniGrid.Visibility = Visibility.Hidden;
            showPanel(l.Content.ToString());
        }

        private void textBlock1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.bmp|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                tipImagePath = op.FileName;
                image2.Source = new BitmapImage(new Uri(op.FileName));
                labelTipIkonica2.FontWeight = FontWeights.Regular;
                labelTipIkonica2.Foreground = Brushes.Black;
                labelTipIkonica2.Visibility = Visibility.Hidden;
            }
        }

        private void spomenikIkonicaLink_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.bmp|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                spomenikImagePath = op.FileName;
                image1.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            String s = (sender as Button).Content.ToString();
            switch (s)
            {
                case "Izmeni Spomenik":
                    if (dataGrid1.SelectedIndex != -1)
                        setUpChangeTab(spomenikTab);
                    else
                        prikaziEkranZaIzmenu("s");
                    break;
                case "Izmeni Tip":
                    if (dataGrid2.SelectedIndex != -1)
                        setUpChangeTab(tipTab);
                    else
                        prikaziEkranZaIzmenu("t");
                    break;
                case "Izmeni Etiketu":
                    if (dataGrid3.SelectedIndex != -1)
                        setUpChangeTab(etiketaTab);
                    else
                        prikaziEkranZaIzmenu("e");
                    break;
                case "Ocisti Mapu":
                    break;
                default:
                    break;
            }
        }

        public void setUpChangeTab(TabItem t)
        {
            try
            {
                tabControl1.Items.Add(t);
                //setTabDefaultValues(t.Header.ToString());
            }
            catch
            {
                //
            }
            setTabSelectedValues(t.Header.ToString());
            selectTab(t);
        }

        public void setTabSelectedValues(string s)
        {
            switch (s)
            {
                case "Tip":
                    Tip originT = (Tip)dataGrid2.SelectedItem;
                    TCV = new TipChangeValidator(originT);
                    tipValidation = TCV.validator;
                    tipTab.DataContext = tipValidation;
                    setTipTabInterface(tipValidation);

                    removeClickEvent(button3, "t");
                    button3.Click += changeTip_Click;

                    switchValidationRules(textBoxTipOznaka, new CheckIfUniqueAndNRTO2Rule());

                    BindingExpression bt = textBoxTipOznaka.GetBindingExpression(TextBox.TextProperty);
                    bt.UpdateSource();

                    break;
                case "Etiketa":
                    Etiketa originE = (Etiketa)dataGrid3.SelectedItem;
                    ECV = new EtiketaChangeValidator(originE);
                    etiketaValidation = ECV.validator;
                    etiketaTab.DataContext = etiketaValidation;
                    setEtiketaTabInterface(etiketaValidation);

                    removeClickEvent(button4, "e");
                    button4.Click += changeEtiketa_Click;

                    switchValidationRules(textBoxEtiketaOznaka, new CheckIfUniqueAndNREO2Rule());

                    BindingExpression be = textBoxEtiketaOznaka.GetBindingExpression(TextBox.TextProperty);
                    be.UpdateSource();

                    break;
                case "Spomenik":
                    Spomenik originS = (Spomenik)dataGrid1.SelectedItem;
                    SCV = new SpomenikChangeValidator(originS);
                    spomenikValidation = SCV.validator;
                    spomenikTab.DataContext = spomenikValidation;
                    setSpomenikTabInterface(spomenikValidation);
                    
                    removeClickEvent(button2, "s");
                    button2.Click += changeSpomenik_Click;
                    
                    switchValidationRules(textBox1, new CheckIfUniqueAndNRSO2Rule());

                    BindingExpression bs = textBox1.GetBindingExpression(TextBox.TextProperty);
                    bs.UpdateSource();

                    break;
                default:
                    break;
            }

        }

        public void setEtiketaTabInterface(Etiketa e)
        {
            //ECV.reservedWord = e.oznaka;

            textBoxEtiketaOpis.Text = e.opis;
            textBoxEtiketaOznaka.Text = e.oznaka;
            var color = (Color)ColorConverter.ConvertFromString(e.boja);
            colorPicker1.SelectedColor = color;

            button4.Content = "Izmeni etiketu";

            labelNovaEtiketa.Content = "Izmeni etiketu - " + e.oznaka;
        }

        public void setTipTabInterface(Tip t)
        {
            //TCV.reservedWord = t.oznaka;

            textBoxTipOpis.Text = t.opis;
            textBoxTipOznaka.Text = t.oznaka;
            textBoxTipIme.Text = t.ime;
            image2.Source = t.ikonica;

            button3.Content = "Izmeni tip";

            labelNoviTip.Content = "Izmeni tip - " + t.oznaka;
        }

        public void switchValidationRules(TextBox t, ValidationRule v)
        {
            Binding binding = BindingOperations.GetBinding(t, TextBox.TextProperty);
            binding.ValidationRules.Clear();
            binding.ValidationRules.Add(new CheckIfEmptyRule());
            if (v != null)
            {
                binding.ValidationRules.Add(v);
            }
        }

        public void setSpomenikTabInterface(Spomenik s)
        {
            //SCV.reservedWord = s.oznaka;

            comboBox1.ItemsSource = tipovi;
            comboBox1.Items.Refresh();
            comboBox1.SelectedItem = s.tip;
            comboBox2.SelectedValue = s.era;
            comboBox3.SelectedValue = s.status;

            if (s.arhObr == "Da")
                checkBox1.IsChecked = true;
            else
                checkBox1.IsChecked = false;

            if (s.unesco == "Da")
                checkBox2.IsChecked = true;
            else
                checkBox2.IsChecked = false;

            if (s.nasReg == "Da")
                checkBox3.IsChecked = true;
            else
                checkBox3.IsChecked = false;

            listBox1.ItemsSource = etikete;
            listBox1.Items.Refresh();
            listBox1.SelectedItems.Clear();
            foreach (Etiketa et in s.etikete)
                listBox1.SelectedItems.Add(et);


            image1.Source = s.ikonica;

            button2.Content = "Izmeni spomenik";

            labelNoviSpomenik.Content = "Izmeni spomenik - " + s.oznaka;
        }

        private void dataGrid1_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            hideGridZaBrisanje();
            showSpomenikIzmenePanels();
        }

        private void dataGrid1_CurrentCellChanged(object sender, EventArgs e)
        {
            showSpomenikIzmenePanels();
        }

        public void showEtiketaIzmenePanels()
        {
            if (dataGrid3.SelectedIndex != -1)
            {
                Etiketa e = (Etiketa)dataGrid3.SelectedItem;
                etiketaIzmeneValidation = e;
                setEtiketaSingleIzmenaInterface(e);
                if (dataGrid3.CurrentCell.Column != null)
                {
                    int index = dataGrid3.CurrentCell.Column.DisplayIndex;
                    switch (index)
                    {
                        case 0:
                            bringToFront(etiketaBojaGrid);
                            break;
                        case 1:
                            bringToFront(etiketaOznakaGrid);
                            break;
                        default:
                            bringToFront(etiketaOpisGrid);
                            break;
                    }
                    labelIzmenaEtiketa.Tag = index;
                }
            }
            else
            {
                bringToFront(nijedanEGrid);
                nijedanELabel.Content = "Nijedna etiketa nije selektovana.";
                labelIzmenaEtiketa.Content = "Izmena podataka o etiketi";
            }
        }

        public void setEtiketaSingleIzmenaInterface(Etiketa e)
        {
            labelIzmenaEtiketa.Content = "Izmeni etiketu - " + e.oznaka;
            reservedEtiketaOznaka = e.oznaka;

            textBox14.Text = e.opis;
            textBox15.Text = e.oznaka;
            var color = (Color)ColorConverter.ConvertFromString(e.boja);
            colorPicker2.SelectedColor = color;
        }

        public void showTipIzmenePanels()
        {
            if (dataGrid2.SelectedIndex != -1)
            {
                Tip t = (Tip)dataGrid2.SelectedItem;
                tipIzmeneValidation = t;
                setTipSingleIzmenaInterface(t);
                if (dataGrid2.CurrentCell.Column != null)
                {
                    int index = dataGrid2.CurrentCell.Column.DisplayIndex;
                    switch (index)
                    {
                        case 0:
                            bringToFront(tipImageGrid);
                            break;
                        case 1:
                            bringToFront(tipOznakaGrid);
                            break;
                        case 2:
                            bringToFront(tipImeGrid);
                            break;
                        default:
                            bringToFront(tipOpisGrid);
                            break;
                    }
                    labelIzmenaTip.Tag = index;
                }
            }
            else
            {
                bringToFront(nijedanTGrid);
                nijedanTLabel.Content = "Nijedan tip nije selektovan.";
                labelIzmenaTip.Content = "Izmena podataka o tipu";
            }
        }

        public void setTipSingleIzmenaInterface(Tip t)
        {
            labelIzmenaTip.Content = "Izmeni tip - " + t.oznaka;
                    reservedTipOznaka = t.oznaka;

                    textBox11.Text = t.opis;
                    textBox12.Text = t.oznaka;
                    textBox13.Text = t.ime;
                    image4.Source = t.ikonica;
        }

        public void showSpomenikIzmenePanels()
        {
            if (dataGrid1.SelectedIndex != -1)
            {
                Spomenik s = (Spomenik)dataGrid1.SelectedItem;
                spomenikIzmeneValidation = s;
                setSpomenikSingleIzmenaInterface(s);
                if (dataGrid1.CurrentCell.Column != null)
                {
                    int index = dataGrid1.CurrentCell.Column.DisplayIndex;
                    switch (index)
                    {
                        case 0:
                            bringToFront(spomenikImageGrid);
                            break;
                        case 1:
                            bringToFront(spomenikOznakaGrid);
                            break;
                        case 2:
                            bringToFront(spomenikImeGrid);
                            break;
                        case 3:
                            bringToFront(spomenikTipGrid);
                            break;
                        case 4:
                            bringToFront(spomenikDatumGrid);
                            break;
                        case 5:
                            bringToFront(spomenikEraGrid);
                            break;
                        case 6:
                            bringToFront(spomenikStatusGrid);
                            break;
                        case 7:
                            bringToFront(spomenikObradjenGrid);
                            break;
                        case 8:
                            bringToFront(spomenikUnescoGrid);
                            break;
                        case 9:
                            bringToFront(spomenikRegionGrid);
                            break;
                        case 10:
                            bringToFront(spomenikPrihodGrid);
                            break;
                        case 11:
                            bringToFront(spomenikEtiketeGrid);
                            break;
                        default:
                            bringToFront(spomenikOpisGrid);
                            break;
                    }
                    label25.Tag = index;
                }
            }
            else
            {
                bringToFront(nijedanSGrid);
                nijedanSLabel.Content = "Nijedan spomenik nije selektovan.";
                label25.Content = "Izmena podataka o spomeniku";
            }
        }

        public void setSpomenikSingleIzmenaInterface(Spomenik s)
        {
            label25.Content = "Izmeni spomenik - " + s.oznaka;
            reservedSpomenikOznaka = s.oznaka;

            textBox6.Text = s.opis;
            textBox7.Text = s.oznaka;
            textBox8.Text = s.ime;
            textBox9.Text = s.datum;
            textBox10.Text = s.prihod;

            comboBox4.ItemsSource = tipovi;
            comboBox4.Items.Refresh();
            comboBox4.SelectedItem = s.tip;
            comboBox5.SelectedValue = s.era;
            comboBox6.SelectedValue = s.status;

            if (s.arhObr == "Da")
                checkBox4.IsChecked = true;
            else
                checkBox4.IsChecked = false;

            if (s.unesco == "Da")
                checkBox5.IsChecked = true;
            else
                checkBox5.IsChecked = false;

            if (s.nasReg == "Da")
                checkBox6.IsChecked = true;
            else
                checkBox6.IsChecked = false;

            listBox2.ItemsSource = etikete;
            listBox2.Items.Refresh();
            listBox2.SelectedItems.Clear();
            foreach (Etiketa et in s.etikete)
                listBox2.SelectedItems.Add(et);


            image3.Source = s.ikonica;
        }

        private void potvrdiOpisBtn_Click(object sender, RoutedEventArgs e)
        {
            Boolean tacno = !Validation.GetHasError(textBox6);
            if (tacno)
            {
                spomenikIzmeneValidation.opis = textBox6.Text;
                refreshSpomenikRelatedContainers();
            }
        }

        private void potvrdiOznakaBtn_Click(object sender, RoutedEventArgs e)
        {
            Boolean tacno = !Validation.GetHasError(textBox7);
            if (tacno)
            {
                spomenikIzmeneValidation.oznaka = textBox7.Text;
                refreshSpomenikRelatedContainers();
            }
        }

        private void potvrdiImeBtn_Click(object sender, RoutedEventArgs e)
        {
            Boolean tacno = !Validation.GetHasError(textBox8);
            if (tacno)
            {
                spomenikIzmeneValidation.ime = textBox8.Text;
                refreshSpomenikRelatedContainers();
            }
        }

        private void potvrdiDatumBtn_Click(object sender, RoutedEventArgs e)
        {
            Boolean tacno = !Validation.GetHasError(textBox9);
            if (tacno)
            {
                spomenikIzmeneValidation.datum = textBox9.Text;
                refreshSpomenikRelatedContainers();
            }
        }

        private void potvrdiPrihodBtn_Click(object sender, RoutedEventArgs e)
        {
            Boolean tacno = !Validation.GetHasError(textBox10);
            if (tacno)
            {
                spomenikIzmeneValidation.prihod = textBox10.Text;
                refreshSpomenikRelatedContainers();
            }
        }

        private void potvrdiTipBtn_Click(object sender, RoutedEventArgs e)
        {
            spomenikIzmeneValidation.tip = (Tip)comboBox4.SelectedItem;
            refreshSpomenikRelatedContainers();
        }

        private void potvrdiEraBtn_Click(object sender, RoutedEventArgs e)
        {
            spomenikIzmeneValidation.era = comboBox5.Text;
            refreshSpomenikRelatedContainers();
        }

        private void potvrdiStatusBtn_Click(object sender, RoutedEventArgs e)
        {
            spomenikIzmeneValidation.status = comboBox6.Text;
            refreshSpomenikRelatedContainers();
        }

        private void potvrdiObradjenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox4.IsChecked ?? false)
                spomenikIzmeneValidation.arhObr = "Da";
            else
                spomenikIzmeneValidation.arhObr = "Ne";
            refreshSpomenikRelatedContainers();
        }

        private void potvrdiUnescoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox5.IsChecked ?? false)
                spomenikIzmeneValidation.unesco = "Da";
            else
                spomenikIzmeneValidation.unesco = "Ne";
            refreshSpomenikRelatedContainers();
        }

        private void potvrdiRegionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox6.IsChecked ?? false)
                spomenikIzmeneValidation.nasReg = "Da";
            else
                spomenikIzmeneValidation.nasReg = "Ne";
            refreshSpomenikRelatedContainers();
        }

        private void potvrdiEtiketeBtn_Click(object sender, RoutedEventArgs e)
        {
            spomenikIzmeneValidation.etikete = new List<Etiketa>();

            foreach (Etiketa et in listBox2.SelectedItems)
            {
                spomenikIzmeneValidation.etikete.Add(et);
            }
            refreshSpomenikRelatedContainers();
        }

        private void potvrdiImageBtn_Click(object sender, RoutedEventArgs e)
        {
            spomenikIzmeneValidation.ikonica = image3.Source as BitmapImage;
            refreshSpomenikRelatedContainers();
        }

        private void textBlock2_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.bmp|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                spomenikIzmeneValidation.ikonicaUrl = op.FileName;
                image3.Source = new BitmapImage(new Uri(op.FileName));
                spomenikIzmeneValidation.ikonica = image3.Source as BitmapImage;
                refreshSpomenikRelatedContainers();
            }
        }

        private void potvrdiOpisTipBtn_Click(object sender, RoutedEventArgs e)
        {
            Boolean tacno = !Validation.GetHasError(textBox11);
            if (tacno)
            {
                tipIzmeneValidation.opis = textBox11.Text;
                refreshTipRelatedContainers();
                refreshSpomenikRelatedContainers();
            }
        }

        private void potvrdiOznakaTipBtn_Click(object sender, RoutedEventArgs e)
        {
            Boolean tacno = !Validation.GetHasError(textBox12);
            if (tacno)
            {
                tipIzmeneValidation.oznaka = textBox12.Text;
                refreshTipRelatedContainers();
                refreshSpomenikRelatedContainers();
            }
        }

        private void potvrdiImeTipBtn_Click(object sender, RoutedEventArgs e)
        {
            Boolean tacno = !Validation.GetHasError(textBox13);
            if (tacno)
            {
                tipIzmeneValidation.ime = textBox13.Text;
                refreshTipRelatedContainers();
                refreshSpomenikRelatedContainers();
            }
        }

        private void textBlock3_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.bmp|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                tipIzmeneValidation.ikonicaUrl = op.FileName;
                image4.Source = new BitmapImage(new Uri(op.FileName));
                tipIzmeneValidation.ikonica = image4.Source as BitmapImage;
                refreshTipRelatedContainers();
                refreshSpomenikRelatedContainers();
            }
        }

        private void potvrdiImageTipBtn_Click(object sender, RoutedEventArgs e)
        {
            tipIzmeneValidation.ikonica = image4.Source as BitmapImage;
            refreshTipRelatedContainers();
            refreshSpomenikRelatedContainers();
        }

        private void dataGrid2_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            hideGridZaBrisanje();
            showTipIzmenePanels();
        }

        private void dataGrid2_CurrentCellChanged(object sender, EventArgs e)
        {
            showTipIzmenePanels();
        }

        private void potvrdiOpisEtiketaBtn_Click(object sender, RoutedEventArgs e)
        {
            Boolean tacno = !Validation.GetHasError(textBox14);
            if (tacno)
            {
                etiketaIzmeneValidation.opis = textBox14.Text;
                refreshEtiketaRelatedContainers();
                refreshSpomenikRelatedContainers();
            }
        }

        private void potvrdiOznakaEtiketaBtn_Click(object sender, RoutedEventArgs e)
        {
            Boolean tacno = !Validation.GetHasError(textBox15);
            if (tacno)
            {
                etiketaIzmeneValidation.oznaka = textBox15.Text;
                refreshEtiketaRelatedContainers();
                refreshSpomenikRelatedContainers();
            }
        }

        private void potvrdiBojaEtiketaBtn_Click(object sender, RoutedEventArgs e)
        {
            etiketaIzmeneValidation.boja = colorPicker2.SelectedColor.ToString();
            refreshEtiketaRelatedContainers();
            refreshSpomenikRelatedContainers();
        }

        private void dataGrid3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showEtiketaIzmenePanels();
        }

        private void dataGrid3_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            hideGridZaBrisanje();
            showEtiketaIzmenePanels();
        }

        private void dataGrid3_CurrentCellChanged(object sender, EventArgs e)
        {
            showEtiketaIzmenePanels();
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            button7.Visibility = Visibility.Hidden;
            izbrisiGrid.Visibility = Visibility.Visible;
            Button b = sender as Button;
            string s = b.Content as string;
            switch(s)
            {
                case "Izbrisi Spomenik":
                    prikaziEkranZaBrisanje("s");
                    break;
                case "Izbrisi Tip":
                    prikaziEkranZaBrisanje("t");
                    break;
                case "Izbrisi Etiketu":
                    prikaziEkranZaBrisanje("e");
                    break;
                default:
                    break;
            }
            //homeTab.Opacity = 0.6;
            //izbrisiMainGrid.Visibility = Visibility.Visible;
        }

        public void prikaziEkranZaBrisanje(string s)
        {
            button7.Visibility = Visibility.Hidden;
            izbrisiGrid.Visibility = Visibility.Visible;
            switch(s)
            {
                case "s":
                    labelNaslovZaBrisanje.Content = "Izbrisi Spomenik";
                    if (dataGrid1.SelectedIndex == -1)
                    {
                        labelZaBrisanje.Content = "Nijedan spomenik nije selektovan.";
                        SelektovanoZaBrisanje(false);
                    }
                    else
                    {
                        labelZaBrisanje.Content = "Da li si siguran da zelis da obrises spomenik?";
                        SelektovanoZaBrisanje(true);
                    }
                    break;
                case "t":
                    labelNaslovZaBrisanje.Content = "Izbrisi Tip";
                    if (dataGrid2.SelectedIndex == -1)
                    {
                        labelZaBrisanje.Content = "Nijedan tip nije selektovan.";
                        SelektovanoZaBrisanje(false);
                    }
                    else
                    {
                        labelZaBrisanje.Content = "Da li si siguran da zelis da obrises tip?";
                        SelektovanoZaBrisanje(true);

                        Tip t = (Tip)dataGrid2.SelectedItem;
                        foreach (Spomenik sp in spomenici)
                        {
                            if (sp.tip.Equals(t))
                            {
                                labelZaBrisanje.Content = "Postoji spomenik koji sadrzi ovaj tip.";
                                SelektovanoZaBrisanje(false);
                                break;
                            }
                        }
                    }
                    break;
                case "e":
                    labelNaslovZaBrisanje.Content = "Izbrisi Etiketu";
                    if (dataGrid3.SelectedIndex == -1)
                    {
                        labelZaBrisanje.Content = "Nijedna etiketa nije selektovana.";
                        SelektovanoZaBrisanje(false);
                    }
                    else
                    {
                        labelZaBrisanje.Content = "Da li si siguran da zelis da obrises etiketu?";
                        SelektovanoZaBrisanje(true);
                    }
                    break;
                default:
                    break;

            }

        }

        public void prikaziEkranZaIzmenu(string s)
        {
            button6.Visibility = Visibility.Hidden;
            izmeniGrid.Visibility = Visibility.Visible;
            moveGridZaBrisanjeIDugmeRight(true);
            switch (s)
            {
                case "s":
                    labelNaslovZaIzmenu.Content = "Izmeni Spomenik";
                    labelZaIzmenu.Content = "Nijedan spomenik nije selektovan.";
                    break;
                case "t":
                    labelNaslovZaIzmenu.Content = "Izmeni Tip";
                    labelZaIzmenu.Content = "Nijedan tip nije selektovan.";                    
                    break;
                case "e":
                    labelNaslovZaIzmenu.Content = "Izmeni Etiketu";
                    labelZaIzmenu.Content = "Nijedna etiketa nije selektovana.";  
                    break;
                default:
                    break;

            }

        }

        public void moveGridZaBrisanjeIDugmeRight(Boolean b)
        {
            if (b)
            {
                button7.Margin = new Thickness(1200, 39, 0, 0);
                izbrisiGrid.Margin = new Thickness(1202, 39, 0, 0);
            }
            else
            {
                button7.Margin = new Thickness(741, 39, 0, 0);
                izbrisiGrid.Margin = new Thickness(743, 39, 0, 0);
            }
        }

        public void SelektovanoZaBrisanje(Boolean b)
        {
            if (b)
            {
                odustaniOdBrisanja.Visibility = Visibility.Visible;
                potvrdiBrisanje.Content = "Da";
            }
            else
            {
                odustaniOdBrisanja.Visibility = Visibility.Hidden;
                potvrdiBrisanje.Content = "Ok";
            }
        }

        private void button11_Click(object sender, RoutedEventArgs e)
        {
            //izbrisiGrid.Visibility = Visibility.Hidden;
            //izbrisiMainGrid.Visibility = Visibility.Hidden;
        }

        private void izbrisiGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            
        }

        private void border4_LostFocus(object sender, RoutedEventArgs e)
        {
            //izbrisiGrid.Visibility = Visibility.Hidden;
            //izbrisiMainGrid.Visibility = Visibility.Hidden;
        }

        private void izbrisiMainGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //izbrisiMainGrid.Visibility = Visibility.Hidden;
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            hideGridZaBrisanje();
        }

        public void hideGridZaBrisanje()
        {
            button7.Visibility = Visibility.Visible;
            izbrisiGrid.Visibility = Visibility.Hidden;
        }

        public void hideGridZaIzmenu()
        {
            button6.Visibility = Visibility.Visible;
            izmeniGrid.Visibility = Visibility.Hidden;
        }

        private void nijeSelektovan_Click(object sender, RoutedEventArgs e)
        {
            hideGridZaIzmenu();
            moveGridZaBrisanjeIDugmeRight(false);
        }

        private void potvrdiBrisanje_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string s = b.Content as string;
            if (s == "Ok")
            {
                hideGridZaBrisanje();
            }
            else
            {
                string btnText = button7.Content as string;
                switch (btnText)
                {
                    case "Izbrisi Spomenik":
                        izbrisiSpomenik();
                        break;
                    case "Izbrisi Tip":
                        izbrisiTip();
                        break;
                    case "Izbrisi Etiketu":
                        izbrisiEtiketu();
                        break;
                    default:
                        break;
                }
            }
        }

        public void izbrisiSpomenik()
        {
            hideGridZaBrisanje();
            Spomenik s = (Spomenik)dataGrid1.SelectedItem;
            spomenici.Remove(s);
            removeFromMap(s.oznaka);
            dataGrid1.UnselectAll();
            refreshSpomenikRelatedContainers();
        }

        public void removeFromMap(string s)
        {
            foreach (UIElement e in canvas1.Children)
            {
                Image i0 = (Image)e;
                Spomenik sp = (Spomenik)i0.Tag;
                string s1 = sp.oznaka;

                if (s1 == s)
                {
                    canvas1.Children.Remove(i0);
                    break;
                }
            }
        }

        public void izbrisiTip()
        {
            hideGridZaBrisanje();
            Tip t = (Tip)dataGrid2.SelectedItem;
            tipovi.Remove(t);
            refreshTipRelatedContainers();
            if (tipovi.Count <= 0)
                tipNeMozeDaSeBira();
            dataGrid2.UnselectAll();
        }

        public void izbrisiEtiketu()
        {
            hideGridZaBrisanje();
            Etiketa e = (Etiketa)dataGrid3.SelectedItem;
            foreach (Spomenik s in spomenici)
            {
                s.etikete.Remove(e);
            }
            etikete.Remove(e);
            refreshEtiketaRelatedContainers();
            refreshSpomenikRelatedContainers();
            dataGrid3.UnselectAll();
        }

        public void savePodatke()
        {
            podaci = new Podaci();
            ObservableCollection<SeriaSpomenik> seriaSpomenici = obicneUSeriaS(spomenici);
            ObservableCollection<SeriaTip> seriaTipovi = obicneUSeriaT(tipovi);
            //podaci.spomenici = spomenici;
            //podaci.tipovi = tipovi;

            podaci.seriaSpomenici = seriaSpomenici;
            podaci.seriaTipovi = seriaTipovi;
            podaci.etikete = etikete;
            podaci.mapa = getJPGFromImageControl(mapa);

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("podaci.bin",
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, podaci);
            stream.Close();
        }

        public ObservableCollection<SeriaSpomenik> obicneUSeriaS(ObservableCollection<Spomenik> s)
        {
            ObservableCollection<SeriaSpomenik> ss = new ObservableCollection<SeriaSpomenik>();
            if (s != null)
            {
                foreach (Spomenik s0 in s)
                {
                    SeriaSpomenik s1 = new SeriaSpomenik(s0);
                    ss.Add(s1);
                }
            }
            return ss;
        }

        public ObservableCollection<SeriaTip> obicneUSeriaT(ObservableCollection<Tip> t)
        {
            ObservableCollection<SeriaTip> tt = new ObservableCollection<SeriaTip>();
            foreach (Tip t0 in t)
            {
                SeriaTip t1 = new SeriaTip(t0);
                tt.Add(t1);
            }
            return tt;
        }

        public ObservableCollection<Spomenik> seriaUObicneS(ObservableCollection<SeriaSpomenik> s)
        {
            ObservableCollection<Spomenik> ss = new ObservableCollection<Spomenik>();
            if (s != null)
            {
                foreach (SeriaSpomenik s0 in s)
                {
                    Spomenik s1 = new Spomenik(s0);
                    ss.Add(s1);
                }
            }
            return ss;
        }

        public ObservableCollection<Tip> seriaUObicneT(ObservableCollection<SeriaTip> t)
        {
            ObservableCollection<Tip> tt = new ObservableCollection<Tip>();
            if (t != null)
            {
                foreach (SeriaTip t0 in t)
                {
                    Tip t1 = new Tip(t0);
                    tt.Add(t1);
                }
            }
            return tt;
        }

        public void loadPodatke()
        {
            podaci = new Podaci();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("podaci.bin",
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.Read);
            if (stream.Length != 0)
                podaci = (Podaci)formatter.Deserialize(stream);
            stream.Close();

            spomenici = seriaUObicneS(podaci.seriaSpomenici);
            tipovi = seriaUObicneT(podaci.seriaTipovi);
            etikete = podaci.etikete;

            foreach (Tip t in tipovi)
            {
                foreach (Spomenik s in spomenici)
                {
                    if (t.oznaka == s.tip.oznaka)
                    {
                        s.tip = t;
                    }
                }
            }

            if (tipovi.Count > 0)
                tipMozeDaSeBira();
            else
                tipNeMozeDaSeBira();

            List<Etiketa> etiketeTemp = new List<Etiketa>();
            foreach (Spomenik s in spomenici)
            {
                etiketeTemp = new List<Etiketa>();
                foreach (Etiketa e0 in etikete)
                {
                    foreach (Etiketa e1 in s.etikete)
                    {
                        if (e0.oznaka == e1.oznaka)
                        {
                            etiketeTemp.Add(e0);
                            break;
                        }
                    }
                }
                s.etikete = etiketeTemp;
            }

            refreshSpomenikRelatedContainers();
            refreshTipRelatedContainers();
            refreshEtiketaRelatedContainers();

            //mapa = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "earth.jpg"));

            loadCanvas();
        }

        public void loadCanvas()
        {
            mapa = LoadImage(podaci.mapa);
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = mapa;
            canvas1.Background = ib;

            foreach (Spomenik s in spomenici)
            {
                if (s.location.X != -1)
                {
                    Image i = new Image();

                    i.Source = s.ikonica;
                    i.Width = 32;
                    i.Height = 32;
                    i.Stretch = Stretch.None;
                    i.Tag = s;
                    canvas1.Children.Add(i);
                    i.Cursor = currentCursor;

                    Canvas.SetLeft(i, s.location.X - 16);
                    Canvas.SetTop(i, s.location.Y - 16);
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            savePodatke();
            System.Environment.Exit(1);
        }

        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
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

        private void textBox4_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = sender as TextBox;
            if (t.Text == "xx/xx/xxxx")
                t.Text = "";
            t.Foreground = Brushes.Black;
            t.FontWeight = FontWeights.Regular;
        }

        private void textBox4_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = sender as TextBox;
            if (t.Text == "")
            {
                addPlaceholder(t);
            }
        }
        
        public void addPlaceholder(TextBox t)
        {
            t.Foreground = Brushes.Silver;
            t.Text = "xx/xx/xxxx";
            t.FontWeight = FontWeights.Bold;
        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void textBoxTipOznaka_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxTipOznaka.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listBox3_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void listBox3_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListBox listView = sender as ListBox;
                ListBoxItem listBoxItem =
                    FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);
                if (listBoxItem != null)
                {
                    // Find the data behind the ListViewItem
                    Spomenik spomenik = (Spomenik)listView.ItemContainerGenerator.
                        ItemFromContainer(listBoxItem);

                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject("myFormat", spomenik);
                    DragDrop.DoDragDrop(listBoxItem, dragData, DragDropEffects.Move);
                }
            } 
        }

        private void canvas1_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void canvas1_Drop(object sender, DragEventArgs e)
        {
            Boolean mozeDaSeStavi = true;
            Point p = e.GetPosition(canvas1);
            mozeDaSeStavi = checkCollision(p, "");
                if (mozeDaSeStavi)
                {
                    if (e.Data.GetDataPresent("myFormat")  && p.X < canvas1.Width - 16 && p.Y < canvas1.Height - 16 && p.X > 16 && p.Y > 16)
                    {
                        Spomenik s = e.Data.GetData("myFormat") as Spomenik;
                        //if (String.Equals(s.locationX, "-") && String.Equals(s.locationY, "-"))
                        //{
                        foreach (UIElement child in canvas1.Children)
                        {
                            Image i0 = (Image)child;
                            Spomenik s0 = (Spomenik)i0.Tag;
                            if (s0.oznaka == s.oznaka)
                            {
                                canvas1.Children.Remove(i0);
                                break;
                            }
                        }

                        Image i = new Image();

                        i.Width = 32;
                        i.Height = 32;
                        i.Stretch = Stretch.None;
                        i.Tag = s;
                        setImageOnMap(i);
                        canvas1.Children.Add(i);
                        i.Cursor = currentCursor;

                        Canvas.SetLeft(i, p.X - 16);
                        Canvas.SetTop(i, p.Y - 16);
                        //Console.WriteLine(s.Oznaka);
                        //s.locationX = p.X.ToString();
                        //s.locationY = p.Y.ToString();
                        //}

                        s.location = p;
                    }
                }
        }

        public Boolean checkCollision(Point p, string s)
        {
            foreach (UIElement child in canvas1.Children)
            {
                Image i0 = (Image)child;
                Spomenik s0 = (Spomenik)i0.Tag;
                Point p0 = s0.location;
                if (s == "exclude p")
                {
                    if (p == p0)
                    {
                        continue;
                    }
                }
                if (p0.X > p.X - 32 && p0.X < p.X + 32 && p0.Y > p.Y - 32 && p0.Y < p.Y + 32)
                {
                    return false;
                }

            }

            return true;
        }

        public Point getCollidingPoint(Point p)
        {
            foreach (UIElement child in canvas1.Children)
            {
                Image i0 = (Image)child;
                Spomenik s0 = (Spomenik)i0.Tag;
                Point p0 = s0.location;
                if (p == p0)
                {
                    continue;
                }
                if (p0.X > p.X - 32 && p0.X < p.X + 32 && p0.Y > p.Y - 32 && p0.Y < p.Y + 32)
                {
                    return p0;
                }

            }

            return new Point(-1, -1);
        }

        private void canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = e.Source as Image;
            if (image != null)
            {
                if (currentCursor == Cursors.SizeAll)
                {
                    if (canvas1.CaptureMouse())
                    {
                        mousePosition = e.GetPosition(canvas1);
                        draggedImage = image;

                        string imageloc = (Canvas.GetLeft(draggedImage) + 32).ToString() + (Canvas.GetTop(draggedImage) + 32).ToString();
                        foreach (Spomenik s in spomenici)
                        {
                            if (String.Equals(imageloc, s.location.X + s.location.Y))
                            {
                                tempSp = s;
                                break;
                            }
                        }

                        Panel.SetZIndex(draggedImage, 1); // in case of multiple images
                    }
                }
                else
                {
                    Spomenik s = (Spomenik) image.Tag;
                    s.location = new Point(-1, -1);
                    canvas1.Children.Remove(image);
                }
            }
        }

        private void canvas1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (draggedImage != null)
            {
                Point imagePosition = ((Spomenik)draggedImage.Tag).location;

                Boolean mozeDaSeStavi = checkCollision(imagePosition, "exclude p");
                //int i = 0;
                int j = 1;
                while (!mozeDaSeStavi)
                {
                    Point collidingPoint = getCollidingPoint(imagePosition);
                    /*
                    var j = collidingPoint.X - p.X;
                    var k = collidingPoint.Y - p.Y;
                    Canvas.SetLeft(draggedImage, Canvas.GetLeft(draggedImage) - j);
                    Canvas.SetTop(draggedImage, Canvas.GetTop(draggedImage) - k);
                    //mozeDaSeStavi = checkCollision(p, "exclude p");
                     */
                    if (imagePosition.X < collidingPoint.X)
                    {
                        Canvas.SetLeft(draggedImage, Canvas.GetLeft(draggedImage) - j);
                    }
                    else if (imagePosition.X > collidingPoint.X)
                    {
                        Canvas.SetLeft(draggedImage, Canvas.GetLeft(draggedImage) + j);
                    }

                    if (imagePosition.Y < collidingPoint.Y)
                    {
                        Canvas.SetTop(draggedImage, Canvas.GetTop(draggedImage) - j);
                    }
                    else if (imagePosition.Y > collidingPoint.Y)
                    {
                        Canvas.SetTop(draggedImage, Canvas.GetTop(draggedImage) + j);
                    }

                    Spomenik s = (Spomenik)draggedImage.Tag;
                    s.location = new Point(Canvas.GetLeft(draggedImage) + 16, Canvas.GetTop(draggedImage) + 16);

                   imagePosition = ((Spomenik)draggedImage.Tag).location;

                    mozeDaSeStavi = checkCollision(imagePosition, "exclude p");

                    j++;

                    /*
                    i++;
                    if (i >= 1000)
                    {
                        i = 0;
                        j = -j;
                    }
                     */

                }

                if (!(imagePosition.X < canvas1.Width - 16 && imagePosition.Y < canvas1.Height - 16 && imagePosition.X > 16 && imagePosition.Y > 16))
                {
                    Spomenik s0 = (Spomenik)draggedImage.Tag;
                    s0.location = new Point(-1, -1);

                    canvas1.Children.Remove(draggedImage);
                }

                canvas1.ReleaseMouseCapture();
                Panel.SetZIndex(draggedImage, 0);
                draggedImage = null;

            }
        }

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggedImage != null)
            {
                var position = e.GetPosition(canvas1);
                var offset = position - mousePosition;
                mousePosition = position;
                Point p = new Point(Canvas.GetLeft(draggedImage) + offset.X, Canvas.GetTop(draggedImage) + offset.Y);

                Point imagePosition = ((Spomenik)draggedImage.Tag).location;
                if (dragCollisionEnabled)
                {
                    Boolean mozeDaSeStavi = checkCollision(imagePosition, "exclude p");
                    if (mozeDaSeStavi)
                    {
                        if (p.X < canvas1.Width - 32 && p.Y < canvas1.Height - 32 && p.X > 0 && p.Y > 0)
                        {
                            Canvas.SetLeft(draggedImage, Canvas.GetLeft(draggedImage) + offset.X);
                            Canvas.SetTop(draggedImage, Canvas.GetTop(draggedImage) + offset.Y);
                        }
                    }
                    else
                    {
                        int j = 1;
                        Point collidingPoint = getCollidingPoint(imagePosition);

                        if (imagePosition.X < collidingPoint.X)
                        {
                            Canvas.SetLeft(draggedImage, Canvas.GetLeft(draggedImage) - j);
                        }
                        else if (imagePosition.X > collidingPoint.X)
                        {
                            Canvas.SetLeft(draggedImage, Canvas.GetLeft(draggedImage) + j);
                        }

                        if (imagePosition.Y < collidingPoint.Y)
                        {
                            Canvas.SetTop(draggedImage, Canvas.GetTop(draggedImage) - j);
                        }
                        else if (imagePosition.Y > collidingPoint.Y)
                        {
                            Canvas.SetTop(draggedImage, Canvas.GetTop(draggedImage) + j);
                        }

                        /*
                            var j = -offset.X;
                            var k = -offset.Y;
                            Point collidingPoint = getCollidingPoint(spomenikPosition);

                            if (spomenikPosition.X < collidingPoint.X)
                            {
                                Canvas.SetLeft(draggedImage, Canvas.GetLeft(draggedImage) + j);
                            }
                            else if (spomenikPosition.X > collidingPoint.X)
                            {
                                Canvas.SetLeft(draggedImage, Canvas.GetLeft(draggedImage) + j);
                            }

                            if (spomenikPosition.Y < collidingPoint.Y)
                            {
                                Canvas.SetTop(draggedImage, Canvas.GetTop(draggedImage) + k);
                            }
                            else if (spomenikPosition.Y > collidingPoint.Y)
                            {
                                Canvas.SetTop(draggedImage, Canvas.GetTop(draggedImage) + k);
                            }
                         */

                        //Point collidingPoint = getCollidingPoint(imagePosition);

                        //Canvas.SetLeft(draggedImage, Canvas.GetLeft(draggedImage));
                        //Canvas.SetTop(draggedImage, Canvas.GetTop(draggedImage));
                    }
                }
                else
                {
                    if (p.X < canvas1.Width - 32 && p.Y < canvas1.Height - 32 && p.X > 0 && p.Y > 0)
                    {
                        Canvas.SetLeft(draggedImage, Canvas.GetLeft(draggedImage) + offset.X);
                        Canvas.SetTop(draggedImage, Canvas.GetTop(draggedImage) + offset.Y);
                    }
                }

                tempSp.location = new Point(Canvas.GetLeft(draggedImage) + 16, Canvas.GetTop(draggedImage) + 16);
                Spomenik s = (Spomenik) draggedImage.Tag;
                s.location = new Point(Canvas.GetLeft(draggedImage) + 16, Canvas.GetTop(draggedImage) + 16);
            }
        }

        private void canvas1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void button9_Click_1(object sender, RoutedEventArgs e)
        {
            string text = button9.Content as string;
            const string yes = "Da";
            const string no = "Ne";
            switch (text)
            {
                case no:
                    button9.Content = yes;
                    dragCollisionEnabled = true;
                    break;
                case yes:
                    button9.Content = no;
                    dragCollisionEnabled = false;
                    break;
                default:
                    break;
            }
        }

        private void listBox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        /*
        private void button10_Click(object sender, RoutedEventArgs e)
        {
            string text = button10.Content as string;
            switch (text)
            {
                case "spomenika":
                    button10.Content = "tipa";
                    koristiIkonicuSpomenika = false;
                    break;
                case "tipa":
                    button10.Content = "spomenika";
                    koristiIkonicuSpomenika = true;
                    break;
                default:
                    break;
            }
            setAllImagesOnMap();
        }
         */

        public void setAllImagesOnMap()
        {
            foreach (UIElement child in canvas1.Children)
            {
                Image i0 = (Image)child;
                Spomenik s0 = (Spomenik)i0.Tag;
                if (koristiIkonicuSpomenika)
                    i0.Source = s0.ikonica;
                else
                    i0.Source = s0.tip.ikonica;
            }
        }

        public void setImageOnMap(Image i)
        {
            Spomenik s0 = (Spomenik)i.Tag;
            if (koristiIkonicuSpomenika)
                i.Source = s0.ikonica;
            else
                i.Source = s0.tip.ikonica;
        }

        private void button11_Click_1(object sender, RoutedEventArgs e)
        {
            string text = button11.Content as string;
            switch (text)
            {
                case "Pomeranje":
                    button11.Content = "Brisanje";
                    currentCursor = ((Image)this.Resources["eraserCursor"]).Cursor;
                    break;
                case "Brisanje":
                    button11.Content = "Pomeranje";
                    currentCursor = Cursors.SizeAll;
                    break;
                default:
                    break;
            }

            setCursorForAllImages();
        }

        public void setCursorForAllImages()
        {
            foreach (UIElement child in canvas1.Children)
            {
                Image i0 = (Image)child;
                i0.Cursor = currentCursor;
            }
        }

        private void checkBox18_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox18);
        }

        public void setVisible(UIElement e)
        {
            /*
            if (e.IsEnabled == false)
                e.IsEnabled = true;
            else
                e.IsEnabled = false;
             */
            if (e.Visibility == Visibility.Hidden)
                e.Visibility = Visibility.Visible;
            else
                e.Visibility = Visibility.Hidden;
        }

        private void comboBox18_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void checkBox7_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox7);
        }

        private void checkBox17_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox17);
        }

        private void checkBox9_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox9);
        }

        private void checkBox15_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox15);
        }

        private void checkBox16_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox16);
        }

        private void checkBox10_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox10);
        }

        private void checkBox11_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox11);
        }

        private void checkBox12_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox12);
        }

        private void checkBox13_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox13);
        }

        private void checkBox14_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox14);
        }

        private void checkBox8_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox8);
        }

        public void refreshSpomenikFilters()
        {
            String selectSomething = "Izaberi opciju...";
            foreach (UIElement e in grid5.Children)
            {
                ComboBox cb = e as ComboBox;
                if (cb != null)
                {
                    String selectedText = cb.Text;
                    cb.Items.Clear();
                    if (cb.Tag != null)
                        refreshSFilter(cb, cb.Tag.ToString());
                    if (cb.Items.Contains(selectedText))
                        cb.Text = selectedText;
                    else
                        cb.Text = selectSomething;

                }
            }
            //refreshSFilter(comboBox18, "oznaka");
            //refreshSFilter(comboBox7, "ime");
        }

        public void refreshTipFilters()
        {
            String selectSomething = "Izaberi opciju...";
            foreach (UIElement e in gridFiltriranjeTip.Children)
            {
                ComboBox cb = e as ComboBox;
                if (cb != null)
                {
                    String selectedText = cb.Text;
                    cb.Items.Clear();
                    if (cb.Tag != null)
                        refreshTFilter(cb, cb.Tag.ToString());
                    if (cb.Items.Contains(selectedText))
                        cb.Text = selectedText;
                    else
                        cb.Text = selectSomething;

                }
            }
            //refreshSFilter(comboBox18, "oznaka");
            //refreshSFilter(comboBox7, "ime");
        }

        public void refreshEtiketaFilters()
        {
            String selectSomething = "Izaberi opciju...";
            foreach (UIElement e in gridFiltriranjeEtiketa.Children)
            {
                ComboBox cb = e as ComboBox;
                if (cb != null)
                {
                    String selectedText = cb.Text;
                    cb.Items.Clear();
                    if (cb.Tag != null)
                        refreshEFilter(cb, cb.Tag.ToString());
                    if (cb.Items.Contains(selectedText))
                        cb.Text = selectedText;
                    else
                        cb.Text = selectSomething;

                }
            }
            bojeZaFilter = new ObservableCollection<Xceed.Wpf.Toolkit.ColorItem>();
            skupiBojeZaFilter();
            //refreshSFilter(comboBox18, "oznaka");
            //refreshSFilter(comboBox7, "ime");
        }

        public void skupiBojeZaFilter()
        {
            foreach(Etiketa e in etikete)
            {
                ColorItem c = new ColorItem((Color)ColorConverter.ConvertFromString(e.boja), e.boja);
                if (!bojeZaFilter.Contains(c))
                    bojeZaFilter.Add(c);
            }
            colorPicker3.StandardColors = bojeZaFilter;
            colorPicker3.AvailableColors = bojeZaFilter;
        }

        public void refreshSFilter(ComboBox cb, string s)
        {
            switch (s)
            {
                case "tipOzn":
                    foreach (Spomenik s0 in spomenici)
                    {
                        Tip t = s0.tip;
                        addIfUnique(cb, t.GetType().GetProperty("oznaka").GetValue(t, null).ToString());
                    }
                    break;
                case "etOzn":
                    List<string> etOznake = new List<string>();
                    foreach (Spomenik s0 in spomenici)
                    {
                        List<Etiketa> e = s0.etikete;
                        foreach (Etiketa e0 in e)
                        {
                            addIfUnique(comboBox8, e0.oznaka);
                        }
                    }
                    break;
                default:
                    foreach (Spomenik s0 in spomenici)
                    {
                        addIfUnique(cb, s0.GetType().GetProperty(s).GetValue(s0,null).ToString());
                    }
                    break;
            }
        }

        public void refreshTFilter(ComboBox cb, string s)
        {
            foreach (Tip t0 in tipovi)
            {
                addIfUnique(cb, t0.GetType().GetProperty(s).GetValue(t0, null).ToString());
            }
        }

        public void refreshEFilter(ComboBox cb, string s)
        {
            foreach (Etiketa e0 in etikete)
            {
                addIfUnique(cb, e0.GetType().GetProperty(s).GetValue(e0, null).ToString());
            }
        }

        public void addIfUnique(ComboBox cb, String s)
        {
            if (!cb.Items.Contains(s))
                cb.Items.Add(s);
        }
        /*
        public void addIfUnique(List<Spomenik> l, Spomenik s)
        {
            int index = l.FindIndex(item => item.oznaka == s.oznaka);
            if (index == -1)
            {
                l.Add(s);
            }
        }
         */

        private void button12_Click(object sender, RoutedEventArgs e)
        {
            izlistajSpomenikeWasPressed = true;
            List<Spomenik> filtriraniSpomenici = findFilteredSpomenike();
            izlistajSpomenike(filtriraniSpomenici);
        }

        public void izlistajSpomenike(List<Spomenik> list)
        {
            stackPanel2.Children.Clear();
            foreach (Spomenik s in list)
            {
                Button b = new Button();
                b.Content = "Selektuj - " + s.oznaka;
                b.Background = Brushes.White;
                b.Tag = s;
                b.Click += new RoutedEventHandler(selektujSpomenik);
                stackPanel2.Children.Add(b);
            }
        }

        public void izlistajTipove(List<Tip> list)
        {
            stackPanel3.Children.Clear();
            foreach (Tip t in list)
            {
                Button b = new Button();
                b.Content = "Selektuj - " + t.oznaka;
                b.Background = Brushes.White;
                b.Tag = t;
                b.Click += new RoutedEventHandler(selektujTip);
                stackPanel3.Children.Add(b);
            }
        }

        public void izlistajEtikete(List<Etiketa> list)
        {
            stackPanel4.Children.Clear();
            foreach (Etiketa e in list)
            {
                Button b = new Button();
                b.Content = "Selektuj - " + e.oznaka;
                b.Background = Brushes.White;
                b.Tag = e;
                b.Click += new RoutedEventHandler(selektujEtiketu);
                stackPanel4.Children.Add(b);
            }
        }

        private void selektujSpomenik(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            dataGrid1.UnselectAll();
            dataGrid1.SelectedItem = (Spomenik)b.Tag;
            dataGrid1.CurrentCell = dataGrid1.SelectedCells.ElementAt(0);
        }

        private void selektujTip(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            dataGrid2.UnselectAll();
            dataGrid2.SelectedItem = (Tip)b.Tag;
            dataGrid2.CurrentCell = dataGrid2.SelectedCells.ElementAt(0);
        }

        private void selektujEtiketu(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            dataGrid3.UnselectAll();
            dataGrid3.SelectedItem = (Etiketa)b.Tag;
            dataGrid3.CurrentCell = dataGrid3.SelectedCells.ElementAt(0);
        }

        public List<Spomenik> findFilteredSpomenike()
        {
            List<Spomenik> list = new List<Spomenik>();
            foreach (Spomenik s in spomenici)
            {
                List<ComboBox> kriterijumi = popuniMapuKriterijuma(grid5);
                if (spomenikZadovoljavaKriterijume(s, kriterijumi))
                    list.Add(s);
            }
            return list;
        }

        public List<Tip> findFilteredTipove()
        {
            List<Tip> list = new List<Tip>();
            foreach (Tip t in tipovi)
            {
                List<ComboBox> kriterijumi = popuniMapuKriterijuma(gridFiltriranjeTip);
                if (tipZadovoljavaKriterijume(t, kriterijumi))
                    list.Add(t);
            }
            return list;
        }

        public List<Etiketa> findFilteredEtikete()
        {
            List<Etiketa> list = new List<Etiketa>();
            foreach (Etiketa e in etikete)
            {
                List<ComboBox> comboBoxKriterijumi = popuniMapuKriterijuma(gridFiltriranjeEtiketa);
                if (etiketaZadovoljavaKriterijume(e, comboBoxKriterijumi))
                {
                    if (colorPicker3.Visibility == Visibility.Visible)
                    {
                        if(e.boja == colorPicker3.SelectedColor.ToString())
                            list.Add(e);
                    }
                    else
                        list.Add(e);
                }
            }
            return list;
        }

        public Boolean spomenikZadovoljavaKriterijume(Spomenik s0, List<ComboBox> kriterijumi)
        {
            foreach (ComboBox cb in kriterijumi)
            {
                Boolean test = false;
                //ComboBox cb = entry.Key;
                String s = cb.Tag.ToString();
                String parameter = ""; 
                switch (s)
                {
                    case "tipOzn":
                        Tip t = s0.tip;
                        parameter = t.oznaka;
                        if (parameter == cb.Text)
                            test = true;
                        break;
                    case "etOzn":
                        List<string> etOznake = new List<string>();
                        List<Etiketa> e = s0.etikete;
                        foreach (Etiketa e0 in e)
                        {
                            parameter = e0.oznaka;
                            if (parameter == cb.Text)
                                test = true;
                        }
                        break;
                    default:
                            parameter = s0.GetType().GetProperty(s).GetValue(s0, null).ToString();
                            if (parameter == cb.Text)
                                test = true;
                        break;
                }
                if (!test)
                    return false;
            }
            return true;
        }

        public Boolean tipZadovoljavaKriterijume(Tip t0, List<ComboBox> kriterijumi)
        {
            foreach (ComboBox cb in kriterijumi)
            {
                Boolean test = false;
                //ComboBox cb = entry.Key;
                String s = cb.Tag.ToString();
                String parameter = "";
                parameter = t0.GetType().GetProperty(s).GetValue(t0, null).ToString();
                if (parameter == cb.Text)
                    test = true;
                if (!test)
                    return false;
            }
            return true;
        }

        public Boolean etiketaZadovoljavaKriterijume(Etiketa e0, List<ComboBox> kriterijumi)
        {
            foreach (ComboBox cb in kriterijumi)
            {
                Boolean test = false;
                //ComboBox cb = entry.Key;
                String s = cb.Tag.ToString();
                String parameter = "";
                parameter = e0.GetType().GetProperty(s).GetValue(e0, null).ToString();
                if (parameter == cb.Text)
                    test = true;
                if (!test)
                    return false;
            }
            return true;
        }

        public List<ComboBox> popuniMapuKriterijuma(Grid g)
        {
            List<ComboBox> k = new List<ComboBox>();
            foreach (UIElement e in g.Children)
            {
                ComboBox cb = e as ComboBox;
                if (cb != null)
                {
                    /*
                    if (cb.IsEnabled == true)
                        k.Add(cb);
                     */

                    if (cb.Visibility == Visibility.Visible)
                        k.Add(cb);
                }
            }
            return k;
        }

        private void checkBox23_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox23);
        }

        private void checkBox19_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox19);
        }

        private void checkBox29_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox29);
        }

        private void button13_Click(object sender, RoutedEventArgs e)
        {
            izlistajTipoveWasPressed = true;
            List<Tip> filtriraniTipovi = findFilteredTipove();
            izlistajTipove(filtriraniTipovi);
        }

        private void checkBox21_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox21);
        }

        private void checkBox20_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(colorPicker3);
        }

        private void checkBox22_CheckChanged(object sender, RoutedEventArgs e)
        {
            setVisible(comboBox22);
        }

        private void button14_Click(object sender, RoutedEventArgs e)
        {
            izlistajEtiketeWasPressed = true;
            List<Etiketa> filtriraneEtikete = findFilteredEtikete();
            izlistajEtikete(filtriraneEtikete);
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button8_Click_1(object sender, RoutedEventArgs e)
        {
            String s = (sender as Button).Content.ToString();
            switch (s)
            {
                case "Nadji Spomenik":
                    setUpNadjiTab(spomenikTab);
                    break;
                case "Nadji Tip":
                    setUpNadjiTab(tipTab);
                    break;
                case "Nadji Etiketu":
                    setUpNadjiTab(etiketaTab);
                    break;
                default:
                    break;
            }
        }

        private void button15_Click(object sender, RoutedEventArgs e)
        {
            dataGrid1.UnselectAll();
        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {
            String s = label1.Content.ToString();

            switch (s)
            {
                case "Glavna stranica - Spomenici":
                    if (demo.aTimer.Enabled == false)
                    {
                        demo = new DemoMod();
                        demo.beginSpomenikDemoMode();
                    }
                    else
                    {
                        demo.aTimer.Enabled = false;
                        demo.aTimer.Stop();
                        demo.state = 0;
                    }
                    break;
                case "Glavna stranica - Tipovi":
                    if (demo.aTimer.Enabled == false)
                    {
                        demo = new DemoMod();
                        demo.beginTipDemoMode();
                    }
                    else
                    {
                        demo.aTimer.Enabled = false;
                        demo.aTimer.Stop();
                        demo.state = 0;
                    }
                    break;
                case "Glavna stranica - Etikete":
                    if (demo.aTimer.Enabled == false)
                    {
                        demo = new DemoMod();
                        demo.beginEtiketaDemoMode();
                    }
                    else
                    {
                        demo.aTimer.Enabled = false;
                        demo.aTimer.Stop();
                        demo.state = 0;
                    }
                    break;
                default:
                    break;
            }
        }

        private void button18_Click(object sender, RoutedEventArgs e)
        {
            refreshujFilterIRezultatSpomenik();
        }

        private void refreshujFilterIRezultatSpomenik()
        {
            foreach (UIElement uie in grid5.Children)
            {
                CheckBox cb = uie as CheckBox;
                if (cb != null)
                    cb.IsChecked = false;
                stackPanel2.Children.Clear();
                izlistajSpomenikeWasPressed = false;
            }
        }

        private void button17_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedIndex != -1)
            {
                //if (dataGrid1.CurrentCell.Column != null)
                //{
                    if (label25.Tag != null)
                    {
                        int i = (int)label25.Tag;
                        switch (i)
                        {
                            case 12:
                                dataGrid1.CurrentCell = dataGrid1.SelectedCells.ElementAt(0);
                                break;
                            default:
                                i++;
                                dataGrid1.CurrentCell = dataGrid1.SelectedCells.ElementAt(i);
                                break;
                        }
                    }
                    showSpomenikIzmenePanels();
                //}
            }
        }

        private void button16_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedIndex != -1)
            {
                //if (dataGrid1.CurrentCell.Column != null)
                //{
                if (label25.Tag != null)
                {
                    int i = (int)label25.Tag;
                    switch (i)
                    {
                        case 0:
                            dataGrid1.CurrentCell = dataGrid1.SelectedCells.ElementAt(12);
                            break;
                        default:
                            i--;
                            dataGrid1.CurrentCell = dataGrid1.SelectedCells.ElementAt(i);
                            break;
                    }
                }
                showSpomenikIzmenePanels();
                //}
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            ugasiDemo();
        }

        public void ugasiDemo()
        {
            switch (demo.trenutniDemoMod)
            {
                case "s":
                    ugasiTimerIDemoModZaSpomenik();
                    break;
                case "t":
                    ugasiTimerIDemoModZaTip();
                    break;
                case "e":
                    ugasiTimerIDemoModZaEtiketu();
                    break;
                default:
                    ugasiTimerIDemoModZaSpomenik();
                    ugasiTimerIDemoModZaTip();
                    ugasiTimerIDemoModZaEtiketu();
                    break;
            }
        }

        public void ugasiTimerIDemoModZaSpomenik()
        {
            if (demo != null)
            {
                if (demo.aTimer.Enabled == true)
                {
                    switchBackToHomeTab(spomenikTab);
                    Spomenik s = null;
                    foreach (Spomenik s0 in spomenici)
                    {
                        if (s0.oznaka == "demoSpomenik")
                        {
                            s = s0;
                            break;
                        }
                    }
                    if (s != null)
                    {
                        spomenici.Remove(s);
                        removeFromMap(s.oznaka);
                        dataGrid1.UnselectAll();
                        refreshSpomenikRelatedContainers();
                        refreshujFilterIRezultatSpomenik();

                    }
                }
            }
            demo.aTimer.Stop();
            demo.state = 0;
        }

        public void ugasiTimerIDemoModZaTip()
        {
            if (demo != null)
            {
                if (demo.aTimer.Enabled == true)
                {
                    switchBackToHomeTab(tipTab);
                    Tip t = null;
                    foreach (Tip t0 in tipovi)
                    {
                        if (t0.oznaka == "demoTip")
                        {
                            t = t0;
                            break;
                        }
                    }
                    if (t != null)
                    {
                        tipovi.Remove(t);
                        refreshTipRelatedContainers();
                        if (tipovi.Count <= 0)
                            tipNeMozeDaSeBira();
                        dataGrid2.UnselectAll();
                        refreshujFilterIRezultatTip();
                    }
                }
            }
            demo.aTimer.Stop();
            demo.state = 0;
        }

        public void ugasiTimerIDemoModZaEtiketu()
        {
            if (demo != null)
            {
                if (demo.aTimer.Enabled == true)
                {
                    switchBackToHomeTab(etiketaTab);
                    Etiketa e = null;
                    foreach (Etiketa e0 in etikete)
                    {
                        if (e0.oznaka == "demoEtiketa")
                        {
                            e = e0;
                            break;
                        }
                    }
                    if (e != null)
                    {
                        foreach (Spomenik s in spomenici)
                        {
                            s.etikete.Remove(e);
                        }
                        etikete.Remove(e);
                        refreshEtiketaRelatedContainers();
                        refreshSpomenikRelatedContainers();
                        dataGrid3.UnselectAll();
                        refreshujFilterIRezultatEtiketa();
                    }
                }
            }
            demo.aTimer.Stop();
            demo.state = 0;
        }

        private void button19_Click(object sender, RoutedEventArgs e)
        {
            izlistajSpomenikeWasPressed = false;
        }

        private void button21_Click(object sender, RoutedEventArgs e)
        {
            dataGrid2.UnselectAll();
        }

        private void button22_Click(object sender, RoutedEventArgs e)
        {
            refreshujFilterIRezultatTip();
        }

        private void refreshujFilterIRezultatTip()
        {
            foreach (UIElement uie in gridFiltriranjeTip.Children)
            {
                CheckBox cb = uie as CheckBox;
                if (cb != null)
                    cb.IsChecked = false;
                stackPanel3.Children.Clear();
                izlistajTipoveWasPressed = false;
            }
        }

        private void refreshujFilterIRezultatEtiketa()
        {
            foreach (UIElement uie in gridFiltriranjeEtiketa.Children)
            {
                CheckBox cb = uie as CheckBox;
                if (cb != null)
                    cb.IsChecked = false;
                stackPanel4.Children.Clear();
                izlistajEtiketeWasPressed = false;
            }
        }

        private void button19_Click_1(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedIndex != -1)
            {
                //if (dataGrid1.CurrentCell.Column != null)
                //{
                if (labelIzmenaTip.Tag != null)
                {
                    int i = (int)labelIzmenaTip.Tag;
                    switch (i)
                    {
                        case 0:
                            dataGrid2.CurrentCell = dataGrid2.SelectedCells.ElementAt(3);
                            break;
                        default:
                            i--;
                            dataGrid2.CurrentCell = dataGrid2.SelectedCells.ElementAt(i);
                            break;
                    }
                }
                showTipIzmenePanels();
                //}
            }
        }

        private void button20_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedIndex != -1)
            {
                //if (dataGrid1.CurrentCell.Column != null)
                //{
                if (labelIzmenaTip.Tag != null)
                {
                    int i = (int)labelIzmenaTip.Tag;
                    switch (i)
                    {
                        case 3:
                            dataGrid2.CurrentCell = dataGrid2.SelectedCells.ElementAt(0);
                            break;
                        default:
                            i++;
                            dataGrid2.CurrentCell = dataGrid2.SelectedCells.ElementAt(i);
                            break;
                    }
                }
                showTipIzmenePanels();
                //}
            }
        }

        private void button25_Click(object sender, RoutedEventArgs e)
        {
            dataGrid3.UnselectAll();
        }

        private void button24_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid3.SelectedIndex != -1)
            {
                //if (dataGrid1.CurrentCell.Column != null)
                //{
                if (labelIzmenaEtiketa.Tag != null)
                {
                    int i = (int)labelIzmenaEtiketa.Tag;
                    switch (i)
                    {
                        case 2:
                            dataGrid3.CurrentCell = dataGrid3.SelectedCells.ElementAt(0);
                            break;
                        default:
                            i++;
                            dataGrid3.CurrentCell = dataGrid3.SelectedCells.ElementAt(i);
                            break;
                    }
                }
                showEtiketaIzmenePanels();
                //}
            }
        }

        private void button23_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid3.SelectedIndex != -1)
            {
                //if (dataGrid1.CurrentCell.Column != null)
                //{
                if (labelIzmenaEtiketa.Tag != null)
                {
                    int i = (int)labelIzmenaEtiketa.Tag;
                    switch (i)
                    {
                        case 0:
                            dataGrid3.CurrentCell = dataGrid3.SelectedCells.ElementAt(2);
                            break;
                        default:
                            i--;
                            dataGrid3.CurrentCell = dataGrid3.SelectedCells.ElementAt(i);
                            break;
                    }
                }
                showEtiketaIzmenePanels();
                //}
            }
        }

        private void button26_Click(object sender, RoutedEventArgs e)
        {
            refreshujFilterIRezultatEtiketa();
        }

        private void tabControl2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem t = tabControl2.SelectedItem as TabItem;
            label19.Content = "Help - " + t.Header.ToString();
        }

        private void idiNaGlavnaStranaSpomenici_Click(object sender, RoutedEventArgs e)
        {
            selectTab(homeTab);
            selectLabelAndPanel(spomenikLabel);
        }

        private void idiNaGlavnaStranaTipovi_Click(object sender, RoutedEventArgs e)
        {
            selectTab(homeTab);
            selectLabelAndPanel(tipoviLabel);
        }

        private void idiNaGlavnaStranaEtikete_Click(object sender, RoutedEventArgs e)
        {
            selectTab(homeTab);
            selectLabelAndPanel(etiketeLabel);
        }

        private void idiNaGlavnaStranaMapa_Click(object sender, RoutedEventArgs e)
        {
            selectTab(homeTab);
            selectLabelAndPanel(mapaLabel);
        }

        private void idiNaSpomenikTab_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectTab(spomenikTab);
            }
            catch (Exception)
            {
                
            }
        }

        private void idiNaTipTab_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectTab(tipTab);
            }
            catch (Exception)
            {
                
            }
        }

        private void idiNaEtiketaTab_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectTab(etiketaTab);
            }
            catch (Exception)
            {

            }
        }

        private void idiNaHelpTab_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tabControl1.Items.Add(helpTab);
            }
            catch (Exception)
            {
                
            }
            try
            {
                
                selectTab(helpTab);
            }
            catch (Exception)
            {

            }
        }

        private void idiNaGlavniTab_Click(object sender, RoutedEventArgs e)
        {
            selectTab(homeTab);
        }

        private void idiNaGlavniTabiRemoveHelpTab_Click(object sender, RoutedEventArgs e)
        {
            tabControl1.Items.Remove(helpTab);
            selectTab(homeTab);
        }

        private void button62_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tabControl1.Items.Remove(spomenikTab);
            }
            catch (Exception)
            {
                
            }
            try
            {
                tabControl1.Items.Remove(tipTab);
            }
            catch (Exception)
            {
                
            }
            try
            {
                tabControl1.Items.Remove(etiketaTab);
            }
            catch (Exception)
            {
                
            }
            try
            {
                tabControl1.Items.Remove(helpTab);
            }
            catch (Exception)
            {
                
            }
        }

        private void button63_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchWord = textBox16.Text.ToString();

                Boolean skipTheOthers = false;

                if (searchWord != null && searchWord != "")
                {
                    foreach (Spomenik s in spomenici)
                    {
                        Type sType = s.GetType();
                        foreach (PropertyInfo propertyInfo in sType.GetProperties())
                        {
                            if (propertyInfo.CanRead)
                            {
                                object value = propertyInfo.GetValue(s, null);
                                if (value is string)
                                {
                                    string v = (string)value;
                                    if (v.Contains(searchWord))
                                    {
                                        selectTab(homeTab);
                                        selectLabelAndPanel(spomenikLabel);
                                        dataGrid1.SelectedItem = s;
                                        dataGrid1.CurrentCell = dataGrid1.SelectedCells.ElementAt(0);
                                        skipTheOthers = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (skipTheOthers)
                            break;
                    }

                    if (!skipTheOthers)
                    {
                        foreach (Tip t in tipovi)
                        {
                            Type tType = t.GetType();
                            foreach (PropertyInfo propertyInfo in tType.GetProperties())
                            {
                                if (propertyInfo.CanRead)
                                {
                                    object value = propertyInfo.GetValue(t, null);
                                    if (value is string)
                                    {
                                        string v = value.ToString();
                                        if (v.Contains(searchWord))
                                        {
                                            selectTab(homeTab);
                                            selectLabelAndPanel(tipoviLabel);
                                            dataGrid2.SelectedItem = t;
                                            dataGrid2.CurrentCell = dataGrid1.SelectedCells.ElementAt(0);
                                            skipTheOthers = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (skipTheOthers)
                                break;
                        }
                    }

                    if (!skipTheOthers)
                    {
                        foreach (Etiketa et in etikete)
                        {
                            Type tType = et.GetType();
                            foreach (PropertyInfo propertyInfo in tType.GetProperties())
                            {
                                if (propertyInfo.CanRead)
                                {
                                    object value = propertyInfo.GetValue(et, null);
                                    if (value is string)
                                    {
                                        string v = value.ToString();
                                        if (v.Contains(searchWord))
                                        {
                                            selectTab(homeTab);
                                            selectLabelAndPanel(etiketeLabel);
                                            dataGrid3.SelectedItem = et;
                                            dataGrid3.CurrentCell = dataGrid1.SelectedCells.ElementAt(0);
                                            skipTheOthers = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (skipTheOthers)
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}
