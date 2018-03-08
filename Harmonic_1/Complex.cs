using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harmonic_1
{
    class Complex
    {
        public double re;
        public double im;

        public Complex(double real, double imag)
        {
            re = real;
            im = imag;
        }

        public static Complex plus(Complex a, Complex b)
        {
            return new Complex(a.re + b.re, a.im + b.im);
        }
        public static Complex minus(Complex a, Complex b)
        {
            return new Complex(a.re - b.re, a.im - b.im);
        }
        public static Complex multiply(Complex a, Complex b)
        {
            return new Complex(a.re * b.re - a.im * b.im, a.re * b.im + a.im * b.re);
        }
        public static Complex multiply(Complex a, double b)
        {
            return new Complex(a.re * b, a.im * b);
        }
        public double ampl()
        {
            return Math.Pow(re * re + im * im, 0.5);
        }
        public double phase()
        {
            return Math.Atan(im / re);
        }
    }
}
