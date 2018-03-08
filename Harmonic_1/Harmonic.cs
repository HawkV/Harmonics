using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harmonic_1
{
    public class Harmonic
    {
        public double a {get; set;}
        public double f { get; set; }
        public double fi { get; set; } // Амплитуда, частота, начальная фаза

        public Harmonic(double a, double f, double fi)
        {
            this.a = a;
            this.f = f;
            this.fi = fi;
        }

        public void changeAmp(double a)
        {
            this.a = a;
        }
        public void changeF(double f)
        {
            this.f = f;
        }
        public void changeFi(double fi)
        {
            this.fi = fi;
        }

        /* Метод для получения значения 
           сигнала в точке (дискретное представление) */

        public double getSample(double f0, int n)
        {
            double rez;
            rez = a * Math.Sin(6.28 * f / f0 * n + 3.14 / 180 * fi);
            return rez;
        }

        /* Метод для получения значения 
          сигнала в точке (аналоговое представление) */
        public double getSample(double time)
        {
            double rez;
            rez = a * Math.Sin(6.28 * f * time + 3.14 / 180 * fi);
            return rez;
        }
    }
}
