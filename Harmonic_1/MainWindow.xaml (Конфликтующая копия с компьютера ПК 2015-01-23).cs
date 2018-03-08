using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.IO.Ports;
using System.Threading;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using Firmata;

namespace Harmonic_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        double[][] amp, ph;

        List<ObservableDataSource<Point>>[] source1 = null;
        ObservableDataSource<Point> source2 = null;

        Harmonics[] harms = new Harmonics[3];


        List<LineGraph>[] vectGraph = new List<LineGraph>[3];
        List<LineGraph>[] ampGraph = new List<LineGraph>[128];
        LineGraph[] phGraph = new LineGraph[128];
        LineGraph[] sinGraph = new LineGraph[3];
        LineGraph cir = new LineGraph();

        String[] PortNames;
        
        Color[] col = new Color[3];
        Brush[] br = new Brush[3];

        FirmataVB port = null;

        GraphMethods GM;

        TabItem[] tabs;
        DataGrid[] signals;
        ObservableCollection<Harm>[] gridData;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += Window_Loaded;
            
            GM = new GraphMethods();

            br[0] = Brushes.Red;
            br[1] = Brushes.Blue;
            br[2] = Brushes.Green;

            col[0] = Colors.Red;
            col[1] = Colors.Blue;
            col[2] = Colors.Green;

            for (int i = 0; i < 3; i++)
            {
                harms[i] = new Harmonics(1, new double[] { i + 1, 1, i * 120 });
            }

            

            PortNames = SerialPort.GetPortNames();

            if (PortNames.Length > 0)
            {
                port = new FirmataVB();
                port.Baud = 9800;
                port.COMPortName = PortNames[0];
                port.BoardType = FirmataVB.Board.OTHER;
            }

            ph = GM.phInit(harms);
            amp = GM.ampInit(harms);

            int[][] buffer = GM.funcSin(harms, sinPlotter, sinGraph);
            GM.funcAmp(amp, specPlotter, ampGraph);

            if (port != null)
            {
                port.Connect();
            }

            analogWrite(buffer, port);

            tabsInit();
            
        }

        private void tabsInit()
        {
            string[] headers = new string[4];

            headers[0] = "Num";
            headers[1] = "Amp";
            headers[2] = "Freq";
            headers[3] = "Phase";

            tabs = new TabItem[3];
            signals = new DataGrid[3];
            gridData = new ObservableCollection<Harm>[3];

            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i] = new TabItem();

                DataGridTextColumn[] columns = new DataGridTextColumn[4];

                signals[i] = new DataGrid();

                gridData[i] = new ObservableCollection<Harm>();

                signals[i].ItemsSource = gridData[i];
                signals[i].KeyUp += cellEdited;

                gridData[i].Add(new Harm(1)
                {
                    Amp = harms[i].harm[0].a,
                    Freq = harms[i].harm[0].f,
                    Phase = harms[i].harm[0].fi,
                });

                tabs[i].Header = Convert.ToChar(65 + i);
                tabs[i].Content = signals[i];

                Select_Cont.Items.Add(tabs[i]);
            }
        }

        private void Simulation()
        {
            for (int j = 0; j < harms.Length; j++)
            {
                addVector(j);
            }

            double i = 0;
            double delta = 0.01f;

            circle();
            
            while (true)
            {
                for (int j = 0; j < 3; j++)
	    		{
                    int b = 0;

                    for (int m = 0; m < amp[0].Length / 2; m++)
                    {
                        if ((amp[j][m] > 0.5) && b < source1[j].Count)
                        {
                            Point p = new Point();
                            Dispatcher.BeginInvoke(new Action(delegate
                            {
                                source1[j][b].Collection.RemoveAt(source1[j][b].Collection.Count - 1);
                            }));
                            
                            p = new Point(amp[j][m] * Math.Cos((ph[j][m] + i) * (m + 1)), -amp[j][m] * Math.Sin((ph[j][m] + i) * (m + 1)));

                            source1[j][b].AppendAsync(Dispatcher, p);

                            b++;
                        }
                    }
    			}
            
                i += delta; 

                Thread.Sleep(10);
            }
        }

        private void circle() 
        {
            double a = amp.Max(h => h.Max(x => x));
            double delta = 0.01f;
            double i = 0;
            
            bool circle = false;

            source2 = new ObservableDataSource<Point>();
            source2.SetXYMapping(p => p);

            while (!circle)
            {
                Point p = new Point(a * Math.Sin(i), a * Math.Cos(i));

                source2.AppendAsync(Dispatcher, p);

                if (source2.Collection.Count > 2 * Math.PI / delta + 1)
                {
                    circle = true;

                    Dispatcher.BeginInvoke(new Action(delegate()
                    {
                        vectPlotter.Children.Remove(cir);
                        cir = vectPlotter.AddLineGraph(source2, Colors.Gray, 2);

                        source2.Collection[source2.Collection.Count - 1] = source2.Collection[0];
                        vectPlotter.FitToView();
                    }));
                }

                i += delta;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            source1 = new List<ObservableDataSource<Point>>[3];

            for (int i = 0; i < vectGraph.Length; i++)
            {
                vectGraph[i] = new List<LineGraph>();    
            }

            for (int i = 0; i < source1.Length; i++)
			{
                source1[i] = new List<ObservableDataSource<Point>>();
			}

            // Start computation process in second thread

            Thread simThread = new Thread(new ThreadStart(Simulation));
            simThread.IsBackground = true;
            simThread.Start();
        }

        

        private void Add_Harm(object sender, RoutedEventArgs e)
        {
            int index = Select_Cont.SelectedIndex;
            int lastElem = gridData[index].Count - 1;

            gridData[index].Add(new Harm(gridData[index][lastElem].Num + 1)
            {
                Amp = 1,
                Freq = 1,
                Phase = 0,
            });

            int harmNum = gridData[index].Count;

            double[] parHarm = new double[3 * (harmNum + 1)];

            for (int i = 0; i < harmNum; i++)
            {
                parHarm[3 * i] = gridData[index][i].Amp;
                parHarm[3 * i + 1] = gridData[index][i].Freq;
                parHarm[3 * i + 2] = gridData[index][i].Phase;
            }

            plotSin(index, parHarm);

            addVector(index);

            Dispatcher.BeginInvoke(new Action(delegate()
            {
                circle();
            }));
        }


        private void Delete_Harm(object sender, RoutedEventArgs e)
        {
            int index = Select_Cont.SelectedIndex;
            int harmNum = gridData[index].Count;

            DataGrid grid = signals[index];

            int delInd = grid.SelectedIndex;

            double[] parHarm = new double[3 * (harmNum - 1)];

            if (harmNum > 1 && delInd != -1)
            {
                int k = 0;

                for (int i = 0; i < harmNum; i++)
                {
                    if (i != delInd)
                    {
                        parHarm[3 * k] = harms[index].harm[i].a;
                        parHarm[3 * k + 1] = harms[index].harm[i].f;
                        parHarm[3 * k + 2] = harms[index].harm[i].fi;

                        k++;
                    }
                }

                harmNum--;

                gridData[index].RemoveAt(delInd);

                harms[index] = new Harmonics(harmNum, parHarm);

                amp = GM.ampInit(harms);
                ph = GM.phInit(harms);

                plotSin(index, parHarm);

                removeVector(index, delInd);
            }
        }

        private void addVector(int index)
        {
            ObservableDataSource<Point> plist = new ObservableDataSource<Point>();
            plist.AppendAsync(Dispatcher, new Point(0, 0));
            plist.AppendAsync(Dispatcher, new Point(0, 0));
            plist.SetXYMapping(p => p);

            source1[index].Add(plist);

            int vectNum = source1[index].Count - 1;

            int thickness = (vectNum == 0) ? 3 : 2; 

            Dispatcher.BeginInvoke(new Action(delegate()
            {
                vectGraph[index].Add(vectPlotter.AddLineGraph(source1[index][vectNum], col[index], thickness));
            }));
        }

        private void removeVector(int index, int delInd)
        {
            source1[index].RemoveAt(delInd);
            
            if(vectPlotter.Children.Count > 1)
            {
                vectPlotter.Children.Remove(vectGraph[index][delInd]);
            }

            vectGraph[index].RemoveAt(delInd);
        }

        private void plotSin(int index, double[] parHarm)
        {
            int harmNum = gridData[index].Count;

            foreach (LineGraph sg in sinGraph)
                sinPlotter.Children.Remove(sg);

            foreach (List<LineGraph> a in ampGraph)
            {
                if (a != null)
                {
                    foreach (LineGraph graph in a)
                    {
                        specPlotter.Children.Remove(graph);
                    }
                }
            }
            
            harms[index] = new Harmonics(harmNum, parHarm);

            int [][] buffer = GM.funcSin(harms, sinPlotter, sinGraph);

            analogWrite(buffer, port);

            ph = GM.phInit(harms);
            amp = GM.ampInit(harms);

            GM.funcAmp(amp, specPlotter, ampGraph);

            //DrawAxis(Brushes.Gray);
        }

        public class Harm
        {
            public double Num { get; private set; }
            public double Amp { get; set; }
            public double Freq { get; set; }
            public double Phase { get; set; }

            public Harm(double num)
            {
                Num = num;
            }
        }

        private void analogWrite(int[][] buffer, FirmataVB port)
        {
            if (port != null)
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    for (int j = 0; j < buffer[0].Length; j++)
                    {
                        port.AnalogWrite(i, buffer[i][j]);
                    }
                }
            }    
        }

        private void cellEdited(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Return))
            {
                DataGrid grid = (DataGrid)sender;
                int index = Array.IndexOf(signals, grid);
                int harmNum = gridData[index].Count;

                double[] parHarm = new double[3 * (harmNum + 1)];

                if (harms.Max(a => a.harm.Max(x => x.a)) < gridData[index].Max(g => g.Amp))
                {
                    Dispatcher.BeginInvoke(new Action(delegate()
                    {
                        circle();
                    }));
                }
                
                for (int i = 0; i < harmNum; i++)
                {
                    parHarm[3 * i] = gridData[index][i].Amp;
                    parHarm[3 * i + 1] = gridData[index][i].Freq;
                    parHarm[3 * i + 2] = gridData[index][i].Phase;
                }

                
                plotSin(index, parHarm);
            }
        }
    }
}
