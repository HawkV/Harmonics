using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harmonic_1
{
    class NotRecFilter
    {
        int order;
        double [] buffer;
        double [] b;

        NotRecFilter(int order, double [] b)
        {
            this.b = (double[])b.Clone();
            b.CopyTo(this.b, 0);
            this.order = order;
            buffer = new double[order];
        }

        void Filter(double [] input, double [] output) 
        {
            for(int i = 0; i <  input.Length; i++) 
            {
                for(int j = 0; j < order-1; j++)
                {
                    buffer[j] = buffer[j+1];
                }
            
                buffer[order-1] = input[i];

                for(int j = 0; j < order-1; j++)
                {
                    buffer[j] *= b[j];
                }
            
                double sum = 0;
            
                for(int j = 0; j < buffer.Length; j++)
                {
                    sum += buffer[j];             
                }
            
                output[i] = sum;
            }
        }
    }
}
