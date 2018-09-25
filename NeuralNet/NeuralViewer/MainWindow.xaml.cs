using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Runtime.InteropServices;
using NeuralNet;


namespace NeuralViewer
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Screen.MainScreen mScreenDisplayer;
        Network network;
        List<Sample> learnSamples;
        List<Sample> testSamples;

        public MainWindow()
        {
            InitializeComponent();
            mScreenDisplayer = new Screen.MainScreen(n_screen, w_screen);
            network = new Network(new int[] {784, 16, 16, 10}, @"F:\projects\NeuralViewer\Data\test.fnn");
            learnSamples = AppLogic.SampleListProvider.GetSamplesFromIDX(@"..\..\..\..\Data\train-images.idx3-ubyte", @"..\..\..\..\Data\train-labels.idx1-ubyte", 59900);
            testSamples = AppLogic.SampleListProvider.GetSamplesFromIDX(@"..\..\..\..\Data\t10k-images.idx3-ubyte", @"..\..\..\..\Data\t10k-labels.idx1-ubyte", 9900);

            Task t = Task.Factory.StartNew(() => { network.Train(60, learnSamples); learnSamples = null; });
        }

    }
}
