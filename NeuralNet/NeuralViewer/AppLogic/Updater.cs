using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNet;
using System.Windows;
using System.Windows.Controls;


namespace NeuralViewer.AppLogic
{
    class Updater
    {
        Screen.MainScreen mScreenDisplayer;
        Network network;
        List<Sample> learnSamples;
        List<Sample> testSamples;

        int s = 0;

        public Updater(Canvas n_screen, Canvas w_screen)
        {
            network = new Network(new int[] {784, 16, 16, 10 }, @"..\..\..\..\Data\testNetwork.fnn");
            learnSamples = AppLogic.SampleListProvider.GetSamplesFromIDX(@"..\..\..\..\Data\train-images.idx3-ubyte", @"..\..\..\..\Data\train-labels.idx1-ubyte", 59900);
            testSamples = AppLogic.SampleListProvider.GetSamplesFromIDX(@"..\..\..\..\Data\t10k-images.idx3-ubyte", @"..\..\..\..\Data\t10k-labels.idx1-ubyte", 9900);
            mScreenDisplayer = new Screen.MainScreen(n_screen, w_screen, network.GetNetworkState(testSamples[0]));

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0,990);
            dispatcherTimer.Start();

            Task.Factory.StartNew(() => { network.TrainAsync(1000, learnSamples); });
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            mScreenDisplayer.UpdateNetwork(network.GetNetworkState(testSamples[s++]));
        }

    }
}
