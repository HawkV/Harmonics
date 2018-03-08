using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;


namespace Harmonic_1
{
    public class GraphMethods
    {
        PointCollection polygonPoints = new PointCollection();

        public int n1, n, oyn, oxn, ly = 100, lx = 400, xln, l1, l2;
       
        double[] coordArrayX = new double[256];
        double[] coordArrayY = new double[256];
        
        int f0 = 256;
        
        private float hx, ky, y1;
        private double x1;
        
        double[][] amp, ph, ReadAmp, ReadPh;
        double[] param = new double[3];

        Color[] col;

        String[] names;

        public GraphMethods()
        {
            col = new Color[3];

            col[0] = Colors.Red;
            col[1] = Colors.Blue;
            col[2] = Colors.Green;

            names = new String[3];

            names[0] = "A";
            names[1] = "B";
            names[2] = "C";
        }

        //способы заполнения массивов амплитуд и фаз
        
        public double[][] ampInit(Harmonics[] harms)
        {
            amp = new double[3][];
            amp[0] = new double[256];
            amp[1] = new double[256];
            amp[2] = new double[256];

            FFT fft = new FFT();

            Complex[] output = new Complex[256];
            Complex[] s = new Complex[256];

            for (int j = 0; j < 3; j++)
            {
                fft.fft(8);
                for (int i = 0; i < s.Length; i++)
                    s[i] = new Complex(harms[j].getSample(f0, i), 0);

                output = fft.engine(s);

                amp[j] = fft.GetA(output);
            }

            return amp;
        }

        public double[][] ampInit(double[] y)
        {
            ReadAmp = new double[1][];
            ReadAmp[0] = new double[256];

            FFT fft = new FFT();

            Complex[] output = new Complex[256];
            Complex[] s = new Complex[y.Length];

            fft.fft(8);
            for (int i = 0; i < s.Length; i++)
                s[i] = new Complex(y[i], 0);

            output = fft.engine(s);

            ReadAmp[0] = fft.GetA(output);

            return ReadAmp;
        }

        public double[][] phInit(Harmonics[] harms)
        {
            ph = new double[3][];
            ph[0] = new double[256];
            ph[1] = new double[256];
            ph[2] = new double[256];

            FFT fft = new FFT();

            Complex[] output = new Complex[256];
            Complex[] s = new Complex[256];

            for (int j = 0; j < 3; j++)
            {
                fft.fft(8);

                for (int i = 0; i < s.Length; i++)
                    s[i] = new Complex(harms[j].getSample(f0, i), 0);

                output = fft.engine(s);

                ph[j] = fft.GetPh(output);
            }

            return ph;
        }

        public double[][] phInit(double[] y)
        {
            ReadPh = new double[1][];
            ReadPh[0] = new double[256];

            FFT fft = new FFT();

            Complex[] output = new Complex[256];
            Complex[] s = new Complex[y.Length];

            fft.fft(8);
            for (int i = 0; i < s.Length; i++)
                s[i] = new Complex(y[i], 0);

            output = fft.engine(s);

            ReadPh[0] = fft.GetPh(output);

            return ReadPh;
        }

        //методы построения графиков и спектра при различных входных данных

        public int[] funcSin(Harmonics[] harms, Plotter2D sinPlotter, LineGraph[] sinGraph)
        {
            EnumerableDataSource<double> xDataSource;
            EnumerableDataSource<double> yDataSource;

            
            int[] coords = new int[1000];

            double min = harms[0].harm.Min(h => h.f);

            float period = (float)(1 / min);
            hx = 0.01f;

            float x1Binary = 0, y1Binary = 0;
            int count = 0;

            while (count < coords.Length)
            {
                x1Binary += hx;
                y1Binary = (float)harms[0].getSample(x1Binary) * 2; // *2 - амплитуда домножается для того, чтобы убрать минусы
                                                                        // *4 - прост))
                coords[count] = Convert.ToInt32(Math.Abs(y1Binary));
                count++;
            }

            hx = 0.0001f;

            for (int k = 0; k < 3; k++)
            {
                xln = (lx - l2);
                x1 = 0;
                y1 = (float)harms[k].getSample(x1);
                n = lx;
                n1 = ly;

                int a = 0;

                double[] x = new double[(int)Math.Ceiling(period / hx)];
                double[] y = new double[(int)Math.Ceiling(period / hx)];

                while (x1 < period)
                {
                    x1 += hx;
                    y1 = (float)harms[k].getSample(x1);

                    x[a] = x1 * 2;
                    y[a] = y1;
                    a++;
                }

                // Create data sources:
                xDataSource = x.AsXDataSource();
                yDataSource = y.AsYDataSource();

                CompositeDataSource compositeDataSource = xDataSource.Join(yDataSource);

                sinGraph[k] = sinPlotter.AddLineGraph(compositeDataSource, col[k], 3, names[k]);
                sinPlotter.FitToView();
            }

            return coords;
        }

        public void funcSin(double[] x, double[] y, Plotter2D ReadSinPlotter, LineGraph readSinGraph)
        {
            EnumerableDataSource<double> xDataSource = x.AsXDataSource();
            EnumerableDataSource<double> yDataSource = y.AsYDataSource();

            CompositeDataSource compositeDataSource = xDataSource.Join(yDataSource);

            readSinGraph = ReadSinPlotter.AddLineGraph(compositeDataSource, Colors.AliceBlue, 3, "Sin");
            ReadSinPlotter.FitToView();
        }

        public void funcAmp(double[][] amp, ChartPlotter specPlotter, List<LineGraph>[] ampGraph)
        {
            EnumerableDataSource<double> xDataSource;
            EnumerableDataSource<double> yDataSource;

            int a = 0;

            ky = 0.1f;
            x1 = 0;
            hx = 0.01f;

            for (int i = 0; i < amp[0].Length / 2; i++)
            {
                List<LineGraph> cds = new List<LineGraph>();

                for (int k = 0; k < amp.Length; k++)
                {
                    double[] x = new double[2];
                    double[] y = new double[2];

                    y1 = (float)(amp[k][i]);

                    if (y1 > 0.1)
                    {
                        x[0] = i;

                        x[1] = x[0];
                        y[0] = 0;
                        y[1] = y1;

                        xDataSource = x.AsXDataSource();
                        yDataSource = y.AsYDataSource();

                        CompositeDataSource compositeDataSource = xDataSource.Join(yDataSource);

                        LineGraph graph = new LineGraph(compositeDataSource);
                        Brush brush = new SolidColorBrush(col[k]);
                        graph.LinePen = new Pen(brush, 3);
                        graph.Description = new PenDescription(names[k]);
                        
                        cds.Add(graph);
                    }


                    if (i == 127)
                    {
                        x1 = 0;
                    }
                }

                if (cds.Count > 0)
                {
                    cds.Sort((p, p1) => p1.DataSource.GetPoints().Last().Y.CompareTo(p.DataSource.GetPoints().Last().Y));
                    
                    ampGraph[i] = cds;

                    foreach (LineGraph lg in cds)
                    {
                       lg.AddToPlotter(specPlotter);
                    }
                }
            }
         
            specPlotter.FitToView();
        }

        public void readFuncAmp(double[][] amp, Plotter2D specPlotter, LineGraph[] readAmpGraph)
        {
            EnumerableDataSource<double> xDataSource;
            EnumerableDataSource<double> yDataSource;

            int a = 0;

            ky = 0.1f;
            x1 = 0;
            hx = 0.01f;

            for (int k = 0; k < amp.Length; k++)
            {
                for (int i = 0; i < amp[0].Length / 2; i++)
                {
                    double[] x = new double[2];
                    double[] y = new double[2];

                    y1 = (float)(amp[k][i]);

                    if (y1 > 0.1)
                    {
                        x[0] = i;

                        x[1] = x[0];
                        y[0] = 0;
                        y[1] = y1;

                        xDataSource = x.AsXDataSource();
                        yDataSource = y.AsYDataSource();

                        CompositeDataSource compositeDataSource = xDataSource.Join(yDataSource);

                        readAmpGraph[a] = specPlotter.AddLineGraph(compositeDataSource, col[k], 3, names[k]);
                        a++;
                    }

                    if (i == 127)
                    {
                        x1 = 0;
                    }
                }
            }

            specPlotter.FitToView();
        }
    }
}
