using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;
using System.Threading;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace testStat
{
    public partial class AcsPage : PhoneApplicationPage
    {
        bool endThread = false;
        bool window = true;
        bool sttoper = false;

        int StatIteration = -1;
        const int namberOfIteration = 10;
        string peopleName;

        public List<buffer> bufferList = new List<buffer>();

        Accelerometer myAcs;
        bufAcs bA;
        bufScr bC;
        Thread stData;

        IsolatedStorageFile file;
        IsolatedStorageFileStream fileStream;

        System.IO.StreamWriter sw;
        XmlSerializer seriXML = new XmlSerializer(typeof(List<buffer>));

        public AcsPage()
        {
            InitializeComponent();

            file = IsolatedStorageFile.GetUserStoreForApplication();
        }
        private void creatOrOpenFile(int nam)
        {
            if (!file.FileExists("buFile" + nam + ".txt"))
            {
                fileStream = file.CreateFile("buFile" + nam + ".txt");
            }
            else
            {
                fileStream = file.OpenFile("buFile" + nam + ".txt", System.IO.FileMode.Open);
            }
            sw = new System.IO.StreamWriter(fileStream);
        }
        private void closeFile()
        {
            sw.Close();
            fileStream.Close();
        }
        /*
         * закрытие диалогового окна
         */ 
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            messageDialog.IsOpen = false;
            this.sttoper = true;
        }

        private void messageDialog_Closed(object sender, EventArgs e)
        {
            
        }
        /**
         * 
         * обработчик события
         * событие происходит, когда отпускают экран(tap)
         * пока не закрыто диалоговое окно, событие не обрабатывается(
         *  реализованно через логическую переменную stopper, возможно можно как то исправить через 
         *  try-catch)
         * останавливает работу акселерометра, закрывает файл
         * меняет переменную итерации StatOfIteration
         * Здесь следует запускать обработчик преобразования фурье(NO)
         * 
         */ 
        private void LayoutRoot_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            ++StatIteration;
            if ((myAcs != null) && this.sttoper)
            {
                meTBox.Text = " \n\n\nhgjh" + StatIteration.ToString();
                myAcs.Stop();
                myAcs.Dispose();
                myAcs = null;
                messageDialog.IsOpen = true;
                endThread = true; // thread copleted

                this.creatOrOpenFile(StatIteration);
                seriXML.Serialize(sw, bufferList);
                bufferList.Clear();
                this.closeFile();


                if (StatIteration == namberOfIteration - 1)
                {
                    this.sttoper = false;
                    StatIteration = 0;

                    ConverterDataAcs nn = new ConverterDataAcs(namberOfIteration);
                    if (nn.state())
                    {
                        messageDialog.IsOpen = true;
                        Application.Current.Terminate();
                        
                    }
                }
            }
        }
        /*
         * 
         * Обработчик события
         * происходит когда экран зажат и палец передвигается
         * 
         */ 
        private void LayoutRoot_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            if (this.sttoper)
            {
                bC = new bufScr((int)e.ManipulationOrigin.X, (int)e.ManipulationOrigin.Y);
            }
        }
        /*
         * 
         * Обработчик события
         * происходит когда палец касается экрана
         * запускает датчик акселерометра
         * запускает поток для снятия данных
         * 
         */ 
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

                endThread = false;
                stData = new Thread(new ThreadStart(ThreadTask));
                stData.Start();
            }
        }
        /*
         * 
         * исполняемый код потока снятия данных
         * пишет данные в файл
         * 
         */ 
        private void ThreadTask()
        {
            buffer b;
            int sleep = 50;
            int time = 0;
            // вроде как не успевает создаться streamwritter
            Thread.Sleep(5);
    //        Dispatcher.BeginInvoke(() =>
    //        {
    //            meTBox.Text = " hgjh";
    //        });
            while (!endThread)
            {
                b = new buffer();
                b.time = time;
                b.acs = bA;
                b.scr = bC;
                bufferList.Add(b);
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
         * Чтение и запись значений акселерометра в переменную
         * 
         */
        private void my_readingChanged(AccelerometerReadingEventArgs e)
        {
            if (myAcs != null)
            {
                bA = new bufAcs((float)e.X, (float)e.Y, (float)e.Z);
            }
        }
        /*
         * 
         * Обработчик события
         * происходит при открытие фрейма, получает параметры(имя) с предыдущего фрейма
         * создает файл с этим именем
         * 
         * error почемуто возникает при каждом открытии элемента popup
         * 
         */ 
        private void meTBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (window)
            {
                this.peopleName = NavigationContext.QueryString["UserString"].ToString();
                window = false;
            }
            
        }
    }

}