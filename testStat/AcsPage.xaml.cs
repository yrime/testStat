using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices.Sensors;
using System.Diagnostics;
using System.Threading;
using System.IO.IsolatedStorage;
using System.Windows.Input;
using System.Xml.Serialization;

namespace testStat
{
    public partial class AcsPage : PhoneApplicationPage
    {
        ReaderWriterLockSlim locker;
        ManualResetEventSlim slim;
        bool end = false;
        bool window = true;

        int StatIteration = 0;
        const int namberOfIteration = 10;

        List<buffer> bufferList = new List<buffer>();

        Accelerometer myAcs;
        bufAcs bA;
        bufScr bC;
        Thread stData;

        IsolatedStorageFile file;
        IsolatedStorageFileStream fileStream;

        System.IO.StreamWriter sw;
        XmlSerializer seriXML = new XmlSerializer(typeof(buffer));

        bool sttoper = false;

        public AcsPage()
        {
            InitializeComponent();

            file = IsolatedStorageFile.GetUserStoreForApplication();
            
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            messageDialog.IsOpen = false;
            this.sttoper = true;
        }

        private void messageDialog_Closed(object sender, EventArgs e)
        {
            
        }

        private void LayoutRoot_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            if ((myAcs != null) && this.sttoper)
            {
                myAcs.Stop();
                myAcs.Dispose();
                myAcs = null;
                messageDialog.IsOpen = true;
                end = true;
                sw.Close();
                if (StatIteration > namberOfIteration)
                {
                    this.sttoper = false;
                    StatIteration = 0;
                    
                }
            }
        }

        private void LayoutRoot_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            if (this.sttoper)
            {
                bC = new bufScr((int)e.ManipulationOrigin.X, (int)e.ManipulationOrigin.Y);
            }
        }

        private void LayoutRoot_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            if (this.sttoper)
            {
                bC = new bufScr((int)e.ManipulationOrigin.X, (int)e.ManipulationOrigin.Y);
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

                end = false;

                stData = new Thread(new ThreadStart(ThreadTask));
                stData.Start();
            }
        }
        private void ThreadTask()
        {
            buffer b;
            int sleep = 50;
            int time = 0;
    /*        Dispatcher.BeginInvoke(() =>
            {
                meTBox.Text = " hgjh";
            });
    */      while (!end)
            {
                b = new buffer();
                b.time = time;
                b.acs = bA;
                b.scr = bC;
                seriXML.Serialize(sw, b);
                Thread.Sleep(sleep);
                time += sleep;
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
                bA = new bufAcs((float)e.X, (float)e.Y, (float)e.Z);
            }
        }

        private void meTBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (window)
            {
                string outData = NavigationContext.QueryString["UserString"].ToString();
                if (!file.FileExists(outData+".txt"))
                {
                    fileStream = file.CreateFile(outData + ".txt");
                }
                else
                {
                    fileStream = file.OpenFile(outData + ".txt", System.IO.FileMode.Open);
                }
                sw = new System.IO.StreamWriter(fileStream);
                window = false;
            }
            
        }
    }
    public struct bufAcs
    {
        public float x;
        public float y;
        public float z;
        public bufAcs(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    public struct bufScr
    {
        public int x;
        public int y;
        public bufScr(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public struct buffer
    {
        public double time;
        public bufAcs acs;
        public bufScr scr;
    }
}