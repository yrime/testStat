using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace testStat
{
    class ConverterDataAcs
    {
        private int numberOfIter;
        private bool complited = false;
        private System.Xml.Serialization.XmlSerializer reader =
            new System.Xml.Serialization.XmlSerializer(typeof(List<buffer>));
        private StreamReader rf;
        private List<buffer> bufList = new List<buffer>();
        private List<Complex> bufFurieList = new List<Complex>();
        IsolatedStorageFile file;
        IsolatedStorageFileStream fileStream;

        XmlSerializer seriXML = new XmlSerializer(typeof(List<double>));
        private StreamWriter sw;

        public ConverterDataAcs(int num)
        {
            /////clear isolated Storage
            //file = IsolatedStorageFile.GetUserStoreForApplication();
            //file.Remove();
            
            this.numberOfIter = num;
            file = IsolatedStorageFile.GetUserStoreForApplication();
            try
            {
                this.complited = this.start();
            }catch(FileNotFoundException err)
            {

            }
        }

        private bool start()
        {
            for(int i = 0; i < numberOfIter; ++i)
            {
                if (file.FileExists(@"buFile" + i + ".txt"))
                {
                    fileStream = file.OpenFile(@"buFile" + i + ".txt", System.IO.FileMode.Open);
                }else
                {
                    throw new FileNotFoundException();
                }
                rf = new StreamReader(fileStream);
                bufList = (List<buffer>)reader.Deserialize(rf);
                // (new Thread(delegate() { func(bufList, i); })).Start();
                func(bufList, i);
            }
            return true;
        }

        private IsolatedStorageFileStream openFile(string name)
        {
            IsolatedStorageFileStream fileStream;

            if (!file.FileExists("ACSX" + name + ".txt"))
            {
                fileStream = file.CreateFile("ACS" + name + ".txt");
            }
            else
            {
                fileStream = file.OpenFile("ACS" + name + ".txt", System.IO.FileMode.Open);
            }
            return fileStream;
        }

        private void modDPF(string name, List<float> l)
        {
            List<double> modAcs = new List<double>();
            List<Complex> dpf;
            //IsolatedStorageFileStream fileStream = this.openFile(name);

            sw = new System.IO.StreamWriter(this.openFile(name));
            dpf = DPF.sumOfSeq(l);
            for (int i = 0; i < dpf.Count(); ++i)
            {
                modAcs.Add(dpf[i].mod());
            }
                
            seriXML.Serialize(sw, modAcs);
            sw.Close();
            fileStream.Close();
        }
        //хуйня
        private void func(List<buffer> obj, int num)
        {
            int i;
            List<buffer> myLystBuf = (List<buffer>)obj;
            List<float> acsListX = new List<float>();
            List<float> acsListY = new List<float>();
            List<float> acsListZ = new List<float>();
            for (i = 0; i < myLystBuf.Count(); ++i)
            {
                acsListX.Add(myLystBuf[i].acs.x);
                acsListY.Add(myLystBuf[i].acs.y);
                acsListZ.Add(myLystBuf[i].acs.z);
            }
            this.modDPF("X" + num, acsListX);
            this.modDPF("Y" + num, acsListY);
            this.modDPF("Z" + num, acsListZ);
        }

        public bool state()
        {
            return this.complited;
        }
        
    }
}
