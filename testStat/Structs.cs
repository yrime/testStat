using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testStat
{
 
        /*
 * 
 * Структуры для хранения данных
 * Акселерометра
 * Координатов касания пальца
 * Структура для хранения структур выше
 * 
 */ 
        
        

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
