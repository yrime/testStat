using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;
using System.Windows.Input;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace testStat
{
    public partial class MainPage : PhoneApplicationPage
    {
        //

        private string placeholder = "Ваше имя";

        public string Placeholder
        {
            get
            {
                return this.placeholder;
            }
            set
            {
                this.placeholder = value;
                tbMain.Text = this.placeholder;
            }
        }

        //vars
        //List<String> myList = new List<String>();
        XmlSerializer seriXML = new XmlSerializer(typeof(buffer));
        List<buffer> bufferList = new List<buffer>();

        Accelerometer myAcs;
        Thread stData;

        String outData = "";
        bool userNameEntered = false;
        int StatIteration = 0;

        bufAcs bA = new bufAcs();
        bufScr bS = new bufScr();

        const int namberOfIteration = 10;

        TextBox uName;
        IsolatedStorageFile file;
        IsolatedStorageFileStream fileStream;

        System.IO.StreamWriter sw;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            file = IsolatedStorageFile.GetUserStoreForApplication();

        }

        //sreen tap does complete
        private void LayoutRoot_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            
            //stData.Abort();
            bufferList.Add(new buffer());
            if (myAcs != null)
            {
                myAcs.Stop();
                myAcs.Dispose();
                myAcs = null;
                ++StatIteration;

                if (StatIteration > namberOfIteration)
                {
                    StatIteration = 0;
                    userNameEntered = false;
                }
            }
        }


        //return hold screen coordinates
        private void LayoutRoot_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            if (userNameEntered)
            {
                Point rp = ((UIElement)e.OriginalSource).TransformToVisual(LayoutRoot).Transform(new Point(0, 0));
                outData = (rp.X + e.ManipulationOrigin.X).ToString() + " " + (rp.Y + e.ManipulationOrigin.Y).ToString();
                bS.x = (int)(rp.X + e.ManipulationOrigin.X);
                bS.y = (int)(rp.Y + e.ManipulationOrigin.Y);
            }
        }
        //return start 9screen tap cordinates
        /**
         * 
         * create new process for acselerometr?
         * NO
         * 
         */
        private void LayoutRoot_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            if (userNameEntered)
            {
                Point rp = ((UIElement)e.OriginalSource).TransformToVisual(LayoutRoot).Transform(new Point(0, 0));
                // outData = (rp.X + e.ManipulationOrigin.X).ToString() + " " + (rp.Y + e.ManipulationOrigin.Y).ToString();
                bS.x = (int)(rp.X + e.ManipulationOrigin.X);
                bS.y = (int)(rp.Y + e.ManipulationOrigin.Y);

                if (myAcs == null)
                {
                    myAcs = new Accelerometer();
                    myAcs.ReadingChanged +=
                        new EventHandler<AccelerometerReadingEventArgs>(myAcs_readingChanged);
                    myAcs.Start();
                }
                else
                {
                    myAcs.Stop();
                    myAcs.Dispose();
                    myAcs = null;
                }
                stData = new Thread(new ThreadStart(ThreadTask));
                stData.Start();
            }
        }

        private void ThreadTask()
        { 
            Stopwatch st = new Stopwatch();
            buffer b;
            st.Start();
            while (true)
            {
                b = new buffer();
                b.time = st.Elapsed.Milliseconds;
                b.acs = new bufAcs(bA);
                b.scr = new bufScr(bS);
                seriXML.Serialize(sw, b);
            }
        }

        private void myAcs_readingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => my_readingChanged(e));
        }
        /*
         * 
         * Чтение и запись значений акселерометра в файл
         * 
         */
        private void my_readingChanged(AccelerometerReadingEventArgs e)
        {
            if (myAcs != null)
            {
               
                bA.x = (float)e.X;
                bA.y = (float)e.Y;
                bA.z = (float)e.Z;               
            }
        }

        /*
         * 
         * Обработка ввода по нажатию enter
         * 
         */

        private void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                userNameEntered = true;
                this.Focus();

                outData = "" + uName.Text.ToString();
                outData = outData.Substring(0, outData.Length - 2);
                outData += ".txt";

                if (!file.FileExists(outData))
                {
                    fileStream = file.CreateFile(outData);
                }else
                {
                    fileStream = file.OpenFile(outData, System.IO.FileMode.Open);
                }

                sw = new System.IO.StreamWriter(fileStream);
            }
        }


    /*    private void stoper_Click(object sender, RoutedEventArgs e)
        {
            sw.Close();
            fileStream.Close();
            stData.Abort();
            if (myAcs != null)
            {
                myAcs.Stop();
                myAcs.Dispose();
                myAcs = null;
            }

            Application.Current.Terminate();
        }
        */

        private void btn_name_enter(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new Uri("/AcsPage.xaml", UriKind.Relative));
        }

        private void tbMain_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbMain.Text.Trim() == this.placeholder)
            {
                tbMain.Text = "";
            }
            bClear.Visibility = Visibility.Visible;
        }

        private void tbMain_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbMain.Text.Trim() == "")
            {
                tbMain.Text = this.placeholder;
            }
            bClear.Visibility = Visibility.Collapsed;
        }

        private void bClear_Click(object sender, RoutedEventArgs e)
        {
            tbMain.Text = string.Empty;
            tbMain.Focus();
        }

    }

    public struct bufAcs
    {
        public float x;
        public float y;
        public float z;
        public bufAcs(bufAcs a)
        {
            x = a.x;
            y = a.y;
            z = a.z;
        }
    }

    public struct bufScr
    {
        public int x;
        public int y;
        public bufScr(bufScr a)
        {
            x = a.x;
            y = a.y;
        }
    }
    public struct buffer
    {
        public double time;
        public bufAcs acs;
        public bufScr scr;
    }
}