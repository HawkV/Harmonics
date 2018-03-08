using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harmonic_1
{
    public class Harmonics
    {
        int harmNum;
        public Harmonic [] harm; // Массив из гармоник
    
        // parHarm - массив, состоящий из 3-ек: А, f, fi.

        public Harmonics(int kol, double[] parHarm) 
        {
            harmNum = kol;
            harm = new Harmonic[harmNum];               
        
            for(int i = 0; i < harmNum ; i++)
            {            
                harm[i] = new Harmonic(parHarm[3 * i],
                                        parHarm[3 * i + 1],
                                        parHarm[3 * i + 2]); 
            }
        
        }
        public void changeAmp(int numHarm, double a)
        {
            harm[numHarm].a = a;
        }
        public void changeF(int numHarm, double f)
        {
            harm[numHarm].f = f;
        }
        public void changeFi(int numHarm, double fi)
        {
            harm[numHarm].fi = fi;
        }
        public double getSample(double f0, int n)
        {
             double rez = 0;
             for(int i = 0; i < harmNum; i++){ 
                 rez += harm[i].getSample(f0, n);
             }         
             return rez;
        }
    
    
         /* Метод для получения значения 
           сигнала в точке (аналоговое представление) */   

        public double getSample(double time) {
            double rez = 0;
            for(int i = 0; i < harmNum; i++){ 
                 rez += harm[i].getSample(time);
            } 
            return rez;        
        }
    }
}
