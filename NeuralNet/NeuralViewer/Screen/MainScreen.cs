using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NeuralViewer.Screen
{
    /// <summary>
    /// This Class is used to display all screen elements into spacialy prepered canvas
    /// </summary>

    class MainScreen
    {
        List<ScreenLayer> testLayers;
        List<LayerConections> conections;
        protected Canvas mainScreen;
        double layerScreenHeight;

        public MainScreen(Canvas screen)
        {
            mainScreen = screen;

            testLayers = new List<ScreenLayer>();
            conections = new List<LayerConections>();
            layerScreenHeight = mainScreen.Height / 5;
            for (int i = 0; i < 4; i++)
            {
                Canvas lscreen = new Canvas();
                lscreen.Height = layerScreenHeight;
                lscreen.Width = mainScreen.Width;

                Canvas.SetTop(lscreen, i * lscreen.Height);
                mainScreen.Children.Add(lscreen);

                testLayers.Add(new ClassicLayer(lscreen, 8 + i * 20));
                (testLayers[i] as ClassicLayer).OnRedrawing += RedrawConections;
                conections.Add(new LayerConections(8 + i * 20, 28 + i * 20));
            }

            Canvas lss = new Canvas();
            lss.Height = mainScreen.Height / 5;
            lss.Width = mainScreen.Width;

            Canvas.SetTop(lss, 4 * lss.Height);
            mainScreen.Children.Add(lss);

            testLayers.Add( new PixelLayer(lss, 88));

            
        }

        public void DrawConection(int back, int layer)
        {
            ScreenNeuron nn = testLayers[layer].GetMarkedNeuron();
            ScreenNeuron b = testLayers[layer + 1][back];
            if (nn != null && testLayers[layer].IsNeuronOnScreen(nn))
            {
                
                double x1 = Canvas.GetLeft(nn.Representation) + nn.GetSize()/2;
                double y1 = Canvas.GetTop(nn.Representation) + layerScreenHeight*layer + nn.GetSize() / 2;
                double x2 = Canvas.GetLeft(b.Representation) + b.GetSize()/2;
                double y2 = Canvas.GetTop(b.Representation) + layerScreenHeight * (layer + 1) + b.GetSize() / 2;

                conections[layer].Conections[testLayers[layer].GetMarkedNeuronNum(), back].SetLinePosition(x1, x2, y1, y2);
                mainScreen.Children.Add(conections[layer].Conections[testLayers[layer].GetMarkedNeuronNum(), back].Representation);
            }
            
        }

        public void RedrawConections(object s, System.EventArgs e)
        {
            for (int i = 0; i < conections.Count; i++)
            {
                for (int j = 0; j < conections[i].Conections.Length; j++)
                {
                    int d = testLayers[i].Count();
                    mainScreen.Children.Remove(conections[i].Conections[j%d, j/d].Representation);
                }
            }

            for (int i = 0; i < conections.Count; i++)
            {
                for (int j = 0; j < testLayers[i+1].Count(); j++)
                {
                    if(testLayers[i+1].IsNeuronOnScreen(testLayers[i+1][j]))
                        DrawConection(j, i);                   
                }
            }
        }


    }
}
