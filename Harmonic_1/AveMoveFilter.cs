using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harmonic_1
{
    class AveMoveFilter
    {
        int order;
        double [] buffer;

        public AveMoveFilter(int order)
        {
            this.order = order;
            buffer = new double[order];
        }

        public void Filter(double[] input, double[] output) 
        {
        
            for(int i = 0; i <  input.Length; i++) 
            {
                for(int j = 0; j < order-1; j++)
                {
                    buffer[j] = buffer[j+1];
                }
            
                buffer[order-1] = input[i];
            
                double sum = 0;
            
                for(int j = 0; j < buffer.Length; j++)
                {
                    sum += buffer[j];             
                }
            
                output[i] = sum / order;
            }
    
        }

        public void Filter(Harmonics harm, double[] output, double f0) 
        {
            for(int i = 0; i < output.Length ; i++) 
            {
                for(int j = 0; j < order-1; j++)
                {
                    buffer[j] = buffer[j+1];
                }
            
                buffer[order-1] = harm.getSample(f0, i);
            
                double sum = 0;
            
                for(int j = 0; j < buffer.Length; j++)
                {
                    sum += buffer[j];             
                }
            
                output[i] = sum / order;
            }
    
        }
    }
}
