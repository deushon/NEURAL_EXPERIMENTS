using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Kovcheg
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        const int NeuronCount = 20;
        const int LayerCount = 20;
        int Inhibitor = 100;
        public class Neuron
        {//добавит ьмассив связей
            public bool[] sinopses = new bool[5];//массив смещений связанных нейронов отностиельно координат текущего.
            public int id;
            public int x;
            public int y;
            public bool state;

            public void act()
            {
                state = !state;
            }
        }
        WriteableBitmap wb = new WriteableBitmap((int)50, 50, 50, 50, PixelFormats.Bgra32, null);
        Neuron[,] NeuronsLayer = new Neuron[LayerCount + 1, NeuronCount + 1];
        Random r = new Random();
        private void a1()
        {


            for (var ii = 0; ii < LayerCount; ii++)
                for (var i = 0; i < NeuronCount; i++)
                {
                    new System.Threading.Thread(delegate () { NeuronsLayer[ii, i] = new Neuron(); a2(ii, i); }).Start();
                    System.Threading.Thread.Sleep(80);
                }
        }

        private void a2(int ii, int i)
        {
            System.Threading.Thread.Sleep(100);
            //  NeuronsLayer[ii, i] = new Neuron();
            System.Threading.Thread.Sleep(100);
            NeuronsLayer[ii, i].x = i;
            NeuronsLayer[ii, i].y = ii;
            byte blue;
            byte green;
            byte red;
            byte alpha;
            for (int u = 0; u < 5; u++)
            {

                int rr = r.Next(0, 3);
                if (rr == 1) NeuronsLayer[ii, i].sinopses[u] = true;
                else NeuronsLayer[ii, i].sinopses[u] = false;
            }
            while (true)
            {
                if (ii < LayerCount - 1)
                {
                   if (NeuronsLayer[ii, i].state)
                    {
                        NeuronsLayer[ii, i].act();
                       if (NeuronsLayer[ii, i].sinopses[0])
                           try
                            {
                                NeuronsLayer[ii-1, i+1].act();
                           }
                            catch { }
                        if (NeuronsLayer[ii, i].sinopses[1])
                            try
                            {
                                NeuronsLayer[ii-1, i - 1].act();
                            }
                            catch { }
                        if (NeuronsLayer[ii, i].sinopses[2])
                            try
                            {
                                NeuronsLayer[ii + 1, i - 1].act();
                            }
                            catch { }
                        if (NeuronsLayer[ii, i].sinopses[3])
                            try
                            {
                                NeuronsLayer[ii + 1, i].act();
                            }
                            catch { }
                        if (NeuronsLayer[ii, i].sinopses[4])
                            try
                            {
                                NeuronsLayer[ii + 1, i + 1].act();
                            }
                            catch { }
                    }
                }
                else
                {

                }

                System.Threading.Thread.Sleep(Inhibitor);


                if (NeuronsLayer[ii, i].state)
                {
                    alpha = 255;
                    red = 255;
                    green = 0;
                    blue = 0;
                }
                else
                {
                    alpha = 255;
                    red = 0;
                    green = 0;
                    blue = 0;
                }

                byte[] colorData = { (byte)blue, (byte)green, (byte)red, (byte)alpha }; // B G R

                Int32Rect rect = new Int32Rect(i, ii, 1, 1);
                img.Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    int stride = (wb.PixelWidth * wb.Format.BitsPerPixel) / 8;

                    wb.WritePixels(rect, colorData, stride, 0);

                    img.Source = wb;
                }));

            }

        }


        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            new System.Threading.Thread(delegate () { a1(); }).Start();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (textint.Text)
            {
                case "0":
                    NeuronsLayer[1, 1].act();
                    textint.Text = "";
                    break;
                case "1":
                    NeuronsLayer[1, 2].act();
                    textint.Text = "";
                    break;
                default: break;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i < 16; i++)
            {
                CheckBox ch = (CheckBox)FindName("c" + i);
                if (Convert.ToBoolean(ch.IsChecked)) NeuronsLayer[0, i - 1].act();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            tout.Text = "";
            for (int i = 0; i < NeuronCount; i++)
            {
                if (NeuronsLayer[LayerCount - 1, i].state)
                    tout.Text = tout.Text + "1";
                else
                    tout.Text = tout.Text + "0";

            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            for (var ii = 0; ii < LayerCount; ii++)
                for (var i = 0; i < NeuronCount; i++)
                {
                    int cs=0;
                    for (int u = 0; u < 5; u++)
                    {
                        int rr = r.Next(0,3);
                        if (rr == 1) { NeuronsLayer[ii, i].sinopses[u] = true; cs++; }
                        else NeuronsLayer[ii, i].sinopses[u] = false;
                    }
                    if (cs == 0) { int rr = r.Next(0, 4); NeuronsLayer[ii, i].sinopses[rr] = true; }
                }
        
    
        }
        private void obuchenie()
        {
            string to="1", tc="2";
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
               (ThreadStart)delegate ()
               {
                   to = tout.Text; tc = textconst.Text;
               }
             );
            do
            {
                System.Threading.Thread.Sleep(200);
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {                   
                    if (tout.Text != textconst.Text)
                    {
                        Button_Click_3(null, null);
                        for (int i = 0; i < NeuronCount; i++)
                            NeuronsLayer[LayerCount - 1, i].state = false;
                        Button_Click_1(null, null);
                    }
                }
             );
                System.Threading.Thread.Sleep(Inhibitor*LayerCount*2);
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    Button_Click_2(null, null);
                    to = tout.Text; tc = textconst.Text;
                }
             );
                System.Threading.Thread.Sleep(200);
            }
            while (to != tc);
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
               (ThreadStart)delegate ()
               {
                Button_Click_5(null, null);
                MessageBox.Show("Сеть обучена! Это произошло в " + System.DateTime.Now.ToLongTimeString());
               }
             );
        }
            private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(obuchenie);
            thread.Start();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string sinps="";
            for (var ii = 0; ii < LayerCount; ii++)
            {
                for (var i = 0; i < NeuronCount; i++)
                {
                    for (int u = 0; u < 5; u++)
                    {
                        if (NeuronsLayer[ii, i].sinopses[u])
                            sinps = sinps + "1";
                        else
                            sinps = sinps + "0";
                    }
                    sinps = sinps + "|";
                }
                sinps = sinps + Environment.NewLine;
            }
            if (!System.IO.File.Exists("neuroset.neuro"))
            System.IO.File.WriteAllText("neuroset.neuro", sinps);
            else
                System.IO.File.WriteAllText("neuroset2.neuro", sinps);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            string sinps = "";
            sinps=System.IO.File.ReadAllText("neuroset.neuro");
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
           
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            Inhibitor += 10;
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if(Inhibitor>10) Inhibitor -= 10;
        }
    }
}
