using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harmonic_1
{
    class FFT
    {
        int stadies;
       
        public Complex [] w;

        public void fft(int stadies)
        {
            this.stadies = stadies;
            w = new Complex[(int)Math.Pow(2, stadies-1)];
            int len = w.Length;
            for (int i = 0; i < len; i++)
            {
                double r = (- Math.PI * i)/len;
                w[i] = new Complex (Math.Cos(r),Math.Sin(r));
            }
        }
    
        public double [] GetA(Complex [] input)
        {
            double[] A = new double[input.Length];
            for (int i = 0; i < input.Length; i++)
                A[i] = input[i].ampl();
            return A;
        }
        
        public double[] GetPh(Complex[] input)
        {
            double[] Ph = new double[input.Length];
            for (int i = 0; i < input.Length; i++)
                Ph[i] = 2*input[i].phase();
            return Ph;
        }
    
        public class Pr
        {
            public Complex a;
            public Complex b;
            
            public Pr (Complex a,Complex b)
            {
                this.a = a;
                this.b = b;
            }
        }

        
       public Pr butterfly(Complex a, Complex b, Complex w)    
       {
            Complex a1 = Complex.plus(a, b);
            b = Complex.minus(a, b);
            b = Complex.multiply(b, w);
            a = a1;
            a = Complex.multiply(a, 0.5);
            b = Complex.multiply(b, 0.5);
            return new Pr(a, b);
       }

       public Complex[] engine(Complex[] input)
       {
           Complex [] output = new Complex[input.Length];
           for(int i=0; i < input.Length;i++)
           {          
              output[i] = new Complex(0,0);
           }
       
           int groups = 1;
           int butterflies = (int)Math.Pow(2, stadies-1);
       
           for (int i = 0; i < stadies; i++) //номер стадии
           {
               for (int j = 0; j < groups ; j++) //номер группы
               {
                   for (int k = 0; k < butterflies ; k++) // номер элемента 
                   {
                     int index = butterflies * 2 * j + k;
                     int index1 = index + butterflies;
                     Complex a = input[index];
                     Complex b = input[index1];
                 
                     Pr variable = butterfly(a, b, w[k*groups]);
                     input[index] = variable.a;
                     input[index1] = variable.b;
                   }             
               }
               groups *= 2;
               butterflies /= 2;
           }
       
           for (int i = 0; i < (int)Math.Pow(2, stadies); i++) //формирование битреверсного индекса
           {
               int j = 0; // i - исходный индекс, j - битреверсный индекс 
               for (int nbit = 0; nbit < stadies; nbit++)
               {
                 j = j | ((i&(1<<nbit)) >> nbit) << (stadies - nbit - 1);
             
               }
               //System.out.println("j="+j);
               output[j] = Complex.multiply(input[i], 2);
           }
       
           return output;
       }
    }
}
