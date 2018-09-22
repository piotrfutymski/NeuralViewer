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
        Conection[][] conections;
        protected Canvas mainScreen;
        double layerScreenHeight;

        double[] markedNeurons;

        public MainScreen(Canvas screen)
        {
            mainScreen = screen;

            testLayers = new List<ScreenLayer>();
            conections = new Conection[4][];
            markedNeurons = new double[4];
            layerScreenHeight = mainScreen.Height / 5;
            for (int i = 0; i < 4; i++)
            {
                Canvas lscreen = new Canvas();
                lscreen.Height = layerScreenHeight;
                lscreen.Width = mainScreen.Width;

                Canvas.SetTop(lscreen, i * lscreen.Height);
                mainScreen.Children.Add(lscreen);

                testLayers.Add(new ClassicLayer(lscreen, 8 + i * 200, i));
                testLayers[i].OnRedrawing += RedrawConections;
                conections[i] = new Conection[208 + i * 200];
                markedNeurons[i] = -1;
                for (int j = 0; j < 208 + i*200; j++)
                {
                    conections[i][j] = new Conection();
                }               
            }

            Canvas lss = new Canvas();
            lss.Height = mainScreen.Height / 5;
            lss.Width = mainScreen.Width;

            Canvas.SetTop(lss, 4 * lss.Height);
            mainScreen.Children.Add(lss);

            testLayers.Add( new PixelLayer(lss, 808, 4));
            testLayers[4].OnRedrawing += RedrawConections;
        }

        private void DrawConection(int back, int front, int layer)
        {
            ScreenNeuron frontNeuron = testLayers[layer][front];
            ScreenNeuron backNeuron = testLayers[layer+1][back];

            double x1 = Canvas.GetLeft(frontNeuron.Representation) + frontNeuron.GetSize()/2;
            double y1 = Canvas.GetTop(frontNeuron.Representation) + layerScreenHeight*layer + frontNeuron.GetSize() / 2;
            double x2 = Canvas.GetLeft(backNeuron.Representation) + backNeuron.GetSize()/2;
            double y2 = Canvas.GetTop(backNeuron.Representation) + layerScreenHeight * (layer + 1) + backNeuron.GetSize() / 2;

            conections[layer][back].SetLinePosition(x1, x2, y1, y2);          
        }

        private void RedrawConections(object s, System.EventArgs e)
        {
            int i = (s as ScreenLayer).Nr;
           
            if (i < conections.Length)
            {
                markedNeurons[i] = testLayers[i].GetMarkedNeuronNum();
                RemoveFromLayer(i);
                DrawLayerConection(i);
                SetNewValues(i);
                AddToLayer(i);
                if(i > 0)
                {
                    RemoveFromLayer(i - 1);
                    DrawLayerConection(i - 1);
                    SetNewValues(i - 1);
                    AddToLayer(i - 1);
                }          
            }
            else
            {
                RemoveFromLayer(i - 1);
                DrawLayerConection(i - 1);
                SetNewValues(i - 1);
                AddToLayer(i - 1);
            }
        }

        private void RemoveFromLayer(int l)
        {
            for (int i = 0; i < conections[l].Length; i++)
            {
                if (testLayers[l + 1].IsNeuronOnScreen(testLayers[l + 1][i]) == false || markedNeurons[l] == -1)
                    mainScreen.Children.Remove(conections[l][i].Representation);
            }
        }

        private void AddToLayer(int l)
        {
            for (int i = 0; i < conections[l].Length; i++)
            {
                if (testLayers[l + 1].IsNeuronOnScreen(testLayers[l + 1][i]) && mainScreen.Children.Contains(conections[l][i].Representation) == false && markedNeurons[l] != -1)
                    mainScreen.Children.Add(conections[l][i].Representation);
            }
        }


        private void DrawLayerConection(int l)
        {
            int n = testLayers[l].GetMarkedNeuronNum();
            if(n!=-1)
            {
                for (int i = 0; i < conections[l].Length; i++)
                {
                    if (testLayers[l + 1].IsNeuronOnScreen(testLayers[l+1][i]))
                        DrawConection(i, n, l);
                }
            }
        }

        private void SetNewValues(int l)
        {

        }

    }
}
