using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Input;
using System.Windows;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace HCIRA502013
{
    public class DemoMod
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public System.Timers.Timer aTimer;
        public int state;
        public MainWindow mw;

        public Boolean pretragaSpomenika = false;
        public Boolean pretragaTipa = false;
        public Boolean pretragaEtikete = false;

        public string trenutniDemoMod; 

        public DemoMod()
        {
            aTimer = new System.Timers.Timer();
            state = 0;
            mw = (MainWindow)System.Windows.Application.Current.MainWindow;
            pretragaSpomenika = false;
            pretragaTipa = false;
        }

        public void beginSpomenikDemoMode()
        {
            trenutniDemoMod = "s";
            aTimer = new System.Timers.Timer();
            state = 0;
            aTimer.Elapsed += new ElapsedEventHandler(spomenikDemoEvents);
            aTimer.Interval = 500;
            aTimer.Enabled = true;

            aTimer.Start();

        }

        public void beginTipDemoMode()
        {
            trenutniDemoMod = "t";

            aTimer = new System.Timers.Timer();
            state = 0;
            aTimer.Elapsed += new ElapsedEventHandler(tipDemoEvents);
            aTimer.Interval = 500;
            aTimer.Enabled = true;

            aTimer.Start();
        }

        public void beginEtiketaDemoMode()
        {
            trenutniDemoMod = "e";

            aTimer = new System.Timers.Timer();
            state = 0;
            aTimer.Elapsed += new ElapsedEventHandler(etiketaDemoEvents);
            aTimer.Interval = 500;
            aTimer.Enabled = true;

            aTimer.Start();
        }

        private void spomenikDemoEvents(object source, ElapsedEventArgs e)
        {
            switch (state)
            {
                case 0:
                    misPrekoElementa(mw.button5);
                    break;
                case 1:
                    klikniNaElement(mw.button5);
                    break;
                case 2:
                    misPrekoElementa(mw.textBox1);
                    break;
                case 3:
                    klikniNaElement(mw.textBox1);
                    break;
                case 4:
                    ispuniElement(mw.textBox1, "demoSpomenik");
                    break;
                case 5:
                    misPrekoElementa(mw.textBox2);
                    break;
                case 6:
                    klikniNaElement(mw.textBox2);
                    break;
                case 7:
                    ispuniElement(mw.textBox2, "abc");
                    break;
                case 8:
                    misPrekoElementa(mw.textBox4);
                    break;
                case 9:
                    klikniNaElement(mw.textBox4);
                    break;
                case 10:
                    ispuniElement(mw.textBox4, "11/11/1111");
                    break;
                case 11:
                    misPrekoElementa(mw.textBox5);
                    break;
                case 12:
                    klikniNaElement(mw.textBox5);
                    break;
                case 13:
                    ispuniElement(mw.textBox5, "123");
                    break;
                case 14:
                    misPrekoElementa(mw.comboBox1);
                    break;
                case 15:
                    klikniNaElement(mw.comboBox1);
                    break;
                case 16:
                    misNaOpciju(mw.comboBox1, 25, 25);
                    break;
                case 17:
                    klikniNaOpciju(mw.comboBox1, 25, 25);
                    break;
                case 18:
                    misPrekoElementa(mw.comboBox2);
                    break;
                case 19:
                    klikniNaElement(mw.comboBox2);
                    break;
                case 20:
                    misNaOpciju(mw.comboBox2, 25, 25);
                    break;
                case 21:
                    klikniNaOpciju(mw.comboBox2, 25, 25);
                    break;
                case 22:
                    misPrekoElementa(mw.comboBox3);
                    break;
                case 23:
                    klikniNaElement(mw.comboBox3);
                    break;
                case 24:
                    misNaOpciju(mw.comboBox3, 25, 25);
                    break;
                case 25:
                    klikniNaOpciju(mw.comboBox3, 25, 25);
                    break;
                case 26:
                    misPrekoElementa(mw.checkBox1);
                    break;
                case 27:
                    klikniNaElement(mw.checkBox1);
                    break;
                case 28:
                    misPrekoElementa(mw.checkBox2);
                    break;
                case 29:
                    klikniNaElement(mw.checkBox2);
                    break;
                case 30:
                    misPrekoElementa(mw.checkBox3);
                    break;
                case 31:
                    klikniNaElement(mw.checkBox3);
                    break;
                case 32:
                    misPrekoElementa(mw.textBox3);
                    break;
                case 33:
                    klikniNaElement(mw.textBox3);
                    break;
                case 34:
                    ispuniElement(mw.textBox3, "def");
                    break;
                case 35:
                    misPrekoElementa(mw.listBox1);
                    break;
                case 36:
                    klikniNaElement(mw.listBox1);
                    break;
                case 37:
                    misPrekoElementa(mw.spomenikIkonicaLink);
                    break;
                case 38:
                    staviIkonicu(mw.image1);
                    break;
                case 39:
                    misPrekoElementa(mw.button2);
                    break;
                case 40:
                    klikniNaElement(mw.button2);
                    if (pretragaSpomenika)
                    {
                        state = 42;
                        pretragaSpomenika = false;
                    }
                    break;
                case 41:
                    misPrekoElementa(mw.button8);
                    break;
                case 42:
                    klikniNaElement(mw.button8);
                    state = 1;
                    pretragaSpomenika = true;
                    break;
                case 43:
                    misPrekoElementa(mw.button15);
                    break;
                case 44:
                    klikniNaElement(mw.button15);
                    break;
                case 45:
                    misPrekoElementa(mw.checkBox18);
                    break;
                case 46:
                    klikniNaElement(mw.checkBox18);
                    break;
                case 47:
                    misPrekoElementa(mw.comboBox18);
                    break;
                case 48:
                    klikniNaElement(mw.comboBox18);
                    break;
                case 49:
                    ispuniElement(mw.comboBox18, "demoSpomenik");
                    break;
                case 50:
                    misPrekoElementa(mw.button12);
                    break;
                case 51:
                    klikniNaElement(mw.button12);
                    break;
                case 52:
                    misNaOpciju(mw.stackPanel2, 25, 25);
                    break;
                case 53:
                    klikniNaOpciju(mw.stackPanel2, 25, 25);
                    break;
                case 54:
                    misNaOpciju(mw.button18, 25, 25);
                    break;
                case 55:
                    klikniNaOpciju(mw.button18, 25, 25);
                    break;
                case 56:
                    misPrekoElementa(mw.button17);
                    break;
                case 57:
                    klikniNaElement(mw.button17);
                    break;
                case 58:
                    klikniNaElement(mw.button17);
                    break;
                case 59:
                    klikniNaElement(mw.button17);
                    break;
                case 60:
                    misPrekoElementa(mw.button16);
                    break;
                case 61:
                    klikniNaElement(mw.button16);
                    break;
                case 62:
                    misPrekoElementa(mw.textBox8);
                    break;
                case 63:
                    klikniNaElement(mw.textBox8);
                    break;
                case 64:
                    ispuniElement(mw.textBox8, "xyz");
                    break;
                case 65:
                    misPrekoElementa(mw.potvrdiImeBtn);
                    break;
                case 66:
                    klikniNaElement(mw.potvrdiImeBtn);
                    break;
                case 67:
                    misPrekoElementa(mw.button6);
                    break;
                case 68:
                    klikniNaElement(mw.button6);
                    break;
                case 69:
                    misPrekoElementa(mw.textBox2);
                    break;
                case 70:
                    klikniNaElement(mw.textBox2);
                    break;
                case 71:
                    ispuniElement(mw.textBox2, "abc");
                    break;
                case 72:
                    misPrekoElementa(mw.button2);
                    break;
                case 73:
                    klikniNaElement(mw.button2);
                    break;
                case 74:
                    misPrekoElementa(mw.button7);
                    break;
                case 75:
                    klikniNaElement(mw.button7);
                    break;
                case 76:
                    misPrekoElementa(mw.potvrdiBrisanje);
                    break;
                case 77:
                    klikniNaElement(mw.potvrdiBrisanje);
                    state = -1;
                    break;
            }
            state++;
        }

        private void tipDemoEvents(object source, ElapsedEventArgs e)
        {
            switch (state)
            {
                case 0:
                    misPrekoElementa(mw.button5);
                    break;
                case 1:
                    klikniNaElement(mw.button5);
                    break;
                case 2:
                    misPrekoElementa(mw.textBoxTipOznaka);
                    break;
                case 3:
                    klikniNaElement(mw.textBoxTipOznaka);
                    break;
                case 4:
                    ispuniElement(mw.textBoxTipOznaka, "demoTip");
                    break;
                case 5:
                    misPrekoElementa(mw.textBoxTipIme);
                    break;
                case 6:
                    klikniNaElement(mw.textBoxTipIme);
                    break;
                case 7:
                    ispuniElement(mw.textBoxTipIme, "abc");
                    break;
                case 8:
                    misPrekoElementa(mw.textBoxTipOpis);
                    break;
                case 9:
                    klikniNaElement(mw.textBoxTipOpis);
                    break;
                case 10:
                    ispuniElement(mw.textBoxTipOpis, "def");
                    break;
                case 11:
                    misPrekoElementa(mw.textBlock1);
                    break;
                case 12:
                    staviIkonicu(mw.image2);
                    break;
                case 13:
                    misPrekoElementa(mw.button3);
                    break;
                case 14:
                    klikniNaElement(mw.button3);
                    if (pretragaTipa)
                    {
                        state = 16;
                        pretragaTipa = false;
                    }
                    break;
                case 15:
                    misPrekoElementa(mw.button8);
                    break;
                case 16:
                    klikniNaElement(mw.button8);
                    state = 1;
                    pretragaTipa = true;
                    break;
                case 17:
                    misPrekoElementa(mw.button21);
                    break;
                case 18:
                    klikniNaElement(mw.button21);
                    break;
                case 19:
                    misPrekoElementa(mw.checkBox23);
                    break;
                case 20:
                    klikniNaElement(mw.checkBox23);
                    break;
                case 21:
                    misPrekoElementa(mw.comboBox23);
                    break;
                case 22:
                    klikniNaElement(mw.comboBox23);
                    break;
                case 23:
                    ispuniElement(mw.comboBox23, "demoTip");
                    break;
                case 24:
                    misPrekoElementa(mw.button13);
                    break;
                case 25:
                    klikniNaElement(mw.button13);
                    break;
                case 26:
                    misNaOpciju(mw.stackPanel3, 25, 25);
                    break;
                case 27:
                    klikniNaOpciju(mw.stackPanel3, 25, 25);
                    break;
                case 28:
                    misNaOpciju(mw.button22, 25, 25);
                    break;
                case 29:
                    klikniNaOpciju(mw.button22, 25, 25);
                    break;
                case 30:
                    misPrekoElementa(mw.button20);
                    break;
                case 31:
                    klikniNaElement(mw.button20);
                    break;
                case 32:
                    klikniNaElement(mw.button20);
                    break;
                case 33:
                    klikniNaElement(mw.button20);
                    break;
                case 34:
                    misPrekoElementa(mw.button19);
                    break;
                case 35:
                    klikniNaElement(mw.button19);
                    break;
                case 36:
                    misPrekoElementa(mw.textBox13);
                    break;
                case 37:
                    klikniNaElement(mw.textBox13);
                    break;
                case 38:
                    ispuniElement(mw.textBox13, "xyz");
                    break;
                case 39:
                    misPrekoElementa(mw.potvrdiImeTipBtn);
                    break;
                case 40:
                    klikniNaElement(mw.potvrdiImeTipBtn);
                    break;
                case 41:
                    misPrekoElementa(mw.button6);
                    break;
                case 42:
                    klikniNaElement(mw.button6);
                    break;
                case 43:
                    misPrekoElementa(mw.textBoxTipIme);
                    break;
                case 44:
                    klikniNaElement(mw.textBoxTipIme);
                    break;
                case 45:
                    ispuniElement(mw.textBoxTipIme, "abc");
                    break;
                case 46:
                    misPrekoElementa(mw.button3);
                    break;
                case 47:
                    klikniNaElement(mw.button3);
                    break;
                case 48:
                    misPrekoElementa(mw.button7);
                    break;
                case 49:
                    klikniNaElement(mw.button7);
                    break;
                case 50:
                    misPrekoElementa(mw.potvrdiBrisanje);
                    break;
                case 51:
                    klikniNaElement(mw.potvrdiBrisanje);
                    state = -1;
                    break;
            }
            state++;
        }

        private void etiketaDemoEvents(object source, ElapsedEventArgs e)
        {
            switch (state)
            {
                case 0:
                    misPrekoElementa(mw.button5);
                    break;
                case 1:
                    klikniNaElement(mw.button5);
                    break;
                case 2:
                    misPrekoElementa(mw.textBoxEtiketaOznaka);
                    break;
                case 3:
                    klikniNaElement(mw.textBoxEtiketaOznaka);
                    break;
                case 4:
                    ispuniElement(mw.textBoxEtiketaOznaka, "demoEtiketa");
                    break;
                case 5:
                    misPrekoElementa(mw.colorPicker1);
                    break;
                case 6:
                    klikniNaElement(mw.colorPicker1);
                    break;
                case 7:
                    misNaOpciju(mw.colorPicker1, 50, 70);
                    break;
                case 8:
                    klikniNaOpciju(mw.colorPicker1, 50, 70);
                    break;
                case 9:
                    misPrekoElementa(mw.textBoxEtiketaOpis);
                    break;
                case 10:
                    klikniNaElement(mw.textBoxEtiketaOpis);
                    break;
                case 11:
                    ispuniElement(mw.textBoxEtiketaOpis, "abc");
                    break;
                case 12:
                    misPrekoElementa(mw.button4);
                    break;
                case 13:
                    klikniNaElement(mw.button4);
                    if (pretragaEtikete)
                    {
                        state = 15;
                        pretragaEtikete = false;
                    }
                    break;
                case 14:
                    misPrekoElementa(mw.button8);
                    break;
                case 15:
                    klikniNaElement(mw.button8);
                    state = 1;
                    pretragaEtikete = true;
                    break;
                case 16:
                    misPrekoElementa(mw.button25);
                    break;
                case 17:
                    klikniNaElement(mw.button25);
                    break;
                case 18:
                    misPrekoElementa(mw.checkBox21);
                    break;
                case 19:
                    klikniNaElement(mw.checkBox21);
                    break;
                case 20:
                    misPrekoElementa(mw.comboBox21);
                    break;
                case 21:
                    klikniNaElement(mw.comboBox21);
                    break;
                case 22:
                    ispuniElement(mw.comboBox21, "demoEtiketa");
                    break;
                case 23:
                    misPrekoElementa(mw.button14);
                    break;
                case 24:
                    klikniNaElement(mw.button14);
                    break;
                case 25:
                    misNaOpciju(mw.stackPanel4, 25, 25);
                    break;
                case 26:
                    klikniNaOpciju(mw.stackPanel4, 25, 25);
                    break;
                case 27:
                    misNaOpciju(mw.button26, 25, 25);
                    break;
                case 28:
                    klikniNaOpciju(mw.button26, 25, 25);
                    break;
                case 29:
                    misPrekoElementa(mw.button24);
                    break;
                case 30:
                    klikniNaElement(mw.button24);
                    break;
                case 31:
                    klikniNaElement(mw.button24);
                    break;
                case 32:
                    klikniNaElement(mw.button24);
                    break;
                case 33:
                    misPrekoElementa(mw.button23);
                    break;
                case 34:
                    klikniNaElement(mw.button23);
                    break;
                case 35:
                    misPrekoElementa(mw.textBox14);
                    break;
                case 36:
                    klikniNaElement(mw.textBox14);
                    break;
                case 37:
                    ispuniElement(mw.textBox14, "xyz");
                    break;
                case 38:
                    misPrekoElementa(mw.potvrdiOpisEtiketaBtn);
                    break;
                case 39:
                    klikniNaElement(mw.potvrdiOpisEtiketaBtn);
                    break;
                case 40:
                    misPrekoElementa(mw.button6);
                    break;
                case 41:
                    klikniNaElement(mw.button6);
                    break;
                case 42:
                    misPrekoElementa(mw.textBoxEtiketaOpis);
                    break;
                case 43:
                    klikniNaElement(mw.textBoxEtiketaOpis);
                    break;
                case 44:
                    ispuniElement(mw.textBoxEtiketaOpis, "abc");
                    break;
                case 45:
                    misPrekoElementa(mw.button4);
                    break;
                case 46:
                    klikniNaElement(mw.button4);
                    break;
                case 47:
                    misPrekoElementa(mw.button7);
                    break;
                case 48:
                    klikniNaElement(mw.button7);
                    break;
                case 49:
                    misPrekoElementa(mw.potvrdiBrisanje);
                    break;
                case 50:
                    klikniNaElement(mw.potvrdiBrisanje);
                    state = -1;
                    break;
            }
            state++;
        }


        public void goToPoint(Point p)
        {
            try
            {
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)p.X, (int)p.Y);
            }
            catch (Exception)
            {
                mw.ugasiDemo();
            }
        }

        public void doMouseClick(Point p)
        {
            try
            {
                //Call the imported function with the cursor's current position
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)p.X, (uint)p.Y, 0, 0);
            }
            catch (Exception)
            {
                mw.ugasiDemo();
            }
        }

        public void misPrekoElementa(UIElement e)
        {
            this.mw.Dispatcher.Invoke(new Action(() => {
                try
                {
                    Point position = e.PointToScreen(new Point(5, 5));
                    goToPoint(position);
                }
                catch (Exception)
                {
                    mw.ugasiDemo();
                }
            }), DispatcherPriority.ContextIdle);
        }

        public void klikniNaElement(UIElement e)
        {
            this.mw.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    Point position = e.PointToScreen(new Point(5, 5));
                    doMouseClick(position);
                }
                catch (Exception)
                {
                    mw.ugasiDemo();
                }
            }), DispatcherPriority.ContextIdle);
        }

        public void ispuniElement(UIElement e, string s)
        {
            this.mw.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    if (e is System.Windows.Controls.TextBox)
                        ((System.Windows.Controls.TextBox)e).Text = s;
                    else if (e is System.Windows.Controls.ComboBox)
                        ((System.Windows.Controls.ComboBox)e).Text = s;
                }
                catch (Exception)
                {
                    mw.ugasiDemo();
                }
            }), DispatcherPriority.ContextIdle);
        }

        public void misNaOpciju(UIElement e, int x, int y)
        {
            this.mw.Dispatcher.Invoke(new Action(() =>
            {
                //if (e is System.Windows.Controls.ComboBox)
                //{
                try
                {
                    Point position = e.PointToScreen(new Point(5 + x, 5 + y));
                    goToPoint(position);
                }
                catch (Exception)
                {
                    mw.ugasiDemo();
                }
                //}
            }), DispatcherPriority.ContextIdle);
        }

        public void klikniNaOpciju(UIElement e, int x, int y)
        {
            this.mw.Dispatcher.Invoke(new Action(() =>
            {
                //if (e is System.Windows.Controls.ComboBox)
                //{
                try
                {
                    Point position = e.PointToScreen(new Point(5 + x, 5 + y));
                    doMouseClick(position);
                }
                catch (Exception)
                {
                    mw.ugasiDemo();
                }
                //}
            }), DispatcherPriority.ContextIdle);
        }

        public void staviIkonicu(System.Windows.Controls.Image i)
        {
            this.mw.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    mw.spomenikImagePath = @"/HCIRA502013Lib;Resources/Back.png";
                    i.Source = new BitmapImage(((BitmapImage)mw.Resources["demoIcon"]).UriSource);
                }
                catch (Exception)
                {
                    mw.ugasiDemo();
                }
            }), DispatcherPriority.ContextIdle);
        }
    }
}
