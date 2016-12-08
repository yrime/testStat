using System;

namespace testStat
{
    /*
     * 
     * Реализация комплексных чисел
     * 
     */ 
    class Complex
    {
        public double x;
        public double y;

        public Complex(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        /*
         * 
         * сумма комплесных чисел
         * с1 = а1 + b1 * i
         * c2 = a2 + b2 * i
         * 
         * c1 + c2 = (a1 + a2) + (b1 + b2) * i
         * 
         */ 
        public void sum(Complex c)
        {
            this.x += c.x;
            this.y += c.y;
        }
        /*
         * 
         * Модуль комплексного числа
         * sqrt(x^2 + y^2)
         * Амплитуда
         * 
         */ 
        public double mod()
        {
            return Math.Sqrt(x * x + y * y);
        }
        /*
         * 
         * Аргумент комплексного числа
         * arg = Arctg(y/x)
         * угол между 0 и прямой комплексного числа
         * 
         */ 
        public double arg()
        {
            double b;
         //  if (x < 0)
         //  {
            b = Math.Atan2(y, x);
         //  }
         //  else
         //  {
         //      b = Math.Atan2(y, x);
         //  }
            return b;
        }
    }
}
