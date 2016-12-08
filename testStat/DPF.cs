using System;
using System.Collections.Generic;
using System.Linq;

namespace testStat
{
    /*
     * 
     * Преобразование фурье
     * статический класс
     * 
     */ 
    class DPF
    {
        /*
         * 
         * Возращает связный список комплексных чисел(результат преобразования Фурье)
         * На вход получает связный список значений акселерометра
         * 
         */ 
        public static List<Complex> sumOfSeq(List<float> al)
        {
            int k;
            int n;
            int size = al.Count();
            double buf;
            Complex com = new Complex(0, 0);
            List<Complex> ou = new List<Complex>();

            for (k = 0; k < size; ++k)
            {
                for (n = 0; n < size; ++n)
                {
                    buf = (-1) * 2 * Math.PI * k * n / size;
                    com.sum(new Complex(al[n] * Math.Cos(buf),
                            al[n] * Math.Sin(buf)));
                }
                ou.Add(com);
                com = new Complex(0, 0);
            }
            return ou;
        }
    }
}
