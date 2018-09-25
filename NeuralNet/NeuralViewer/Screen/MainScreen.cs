using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using NeuralNet;

namespace NeuralViewer.Screen
{
    /// <summary>
    /// This Class is used to display all screen elements into spacialy prepered canvas
    /// </summary>

    class MainScreen
    {
        List<ScreenLayer> mLayers;
        WeightScreen wScreen;
        Conection[][] conections;
        protected Canvas mainScreen;
        double layerScreenHeight;
        double[] markedNeurons;
        double[][,] conectionValues;

        public MainScreen(Canvas screen, Canvas s, NetworkState state)
        {
            mainScreen = screen;
            mLayers = new List<ScreenLayer>();
            conections = new Conection[state.LayerNumber - 1][];
            markedNeurons = new double[state.LayerNumber - 1];
            conectionValues = new double[state.LayerNumber - 1][,];
            layerScreenHeight = mainScreen.Height / state.LayerNumber;
            wScreen = new WeightScreen(s, state.GetLayer(0).Length);

            for (int i = 0; i < state.LayerNumber; i++)
            {
                bool ic = true;
                if (i == 0) ic = false;
                mLayers.Add(CreateLayer(ic, i, state));

                if(i < state.LayerNumber - 1)
                {
                    conections[i] = new Conection[state.GetLayer(i).Length];
                    markedNeurons[i] = -1;
                    conectionValues[i] = new double[state.GetLayer(i + 1).Length, state.GetLayer(i).Length];
                    for (int j = 0; j < state.GetLayer(i).Length; j++)
                    {
                        conections[i][j] = new Conection();
                    }

                }                              
            }
        }
        
        private ScreenLayer CreateLayer(bool isClassic, int num, NetworkState state)
        {
            ScreenLayer res;
            Canvas lscreen = new Canvas();
            lscreen.Height = layerScreenHeight;
            lscreen.Width = mainScreen.Width;

            Canvas.SetTop(lscreen, num * lscreen.Height);
            mainScreen.Children.Add(lscreen);

            if (isClassic)
                res = new ClassicLayer(lscreen, state.GetLayer(num).Length, num);
            else
                res = new PixelLayer(lscreen, state.GetLayer(num).Length, num);

            res.OnRedrawing += RedrawConections;
            
            if(num == 1)
            {
                res.OnMarked += LoadWeightScreen;
            }
            return res;
        }

        public void UpdateNetwork(NetworkState state)
        {
            for (int i = 0; i < conectionValues.Length; i++)
            {
                for (int j = 0; j < mLayers[i+1].Count(); j++)
                {
                    for (int k = 0; k < mLayers[i].Count(); k++)
                    {
                        conectionValues[i][j, k] = state.GetWeight(i, j, k);
                    }                
                }
                SetNewValues(i + 1);
            }

            for (int i = 0; i < mLayers.Count; i++)
            {
                for (int j = 0; j < mLayers[i].Count(); j++)
                {
                    mLayers[i][j].Value = state.GetNeuronValue(i, j);
                }
            }

        }

        private void DrawConection(int back, int front, int layer)
        {
            ScreenNeuron frontNeuron = mLayers[layer][front];
            ScreenNeuron backNeuron = mLayers[layer-1][back];

            double x1 = Canvas.GetLeft(frontNeuron.Representation) + frontNeuron.GetSize()/2;
            double y1 = Canvas.GetTop(frontNeuron.Representation) + layerScreenHeight*layer + frontNeuron.GetSize() / 2;
            double x2 = Canvas.GetLeft(backNeuron.Representation) + backNeuron.GetSize()/2;
            double y2 = Canvas.GetTop(backNeuron.Representation) + layerScreenHeight * (layer - 1) + backNeuron.GetSize() / 2;

            conections[layer - 1][back].SetLinePosition(x1, x2, y1, y2);          
        }

        private void RedrawConections(object s, System.EventArgs e)
        {
            int i = (s as ScreenLayer).Nr;
           
            if (i > 0)
            {
                markedNeurons[i-1] = mLayers[i].GetMarkedNeuronNum();
                RemoveFromLayer(i);
                DrawLayerConection(i);
                SetNewValues(i);
                AddToLayer(i);
                if(i < conections.Length)
                {
                    RemoveFromLayer(i + 1);
                    DrawLayerConection(i + 1);
                    SetNewValues(i + 1);
                    AddToLayer(i + 1);
                }          
            }
            else
            {
                RemoveFromLayer(i + 1);
                DrawLayerConection(i + 1);
                SetNewValues(i + 1);
                AddToLayer(i + 1);
            }
        }

        private void RemoveFromLayer(int l)
        {
            for (int i = 0; i < conections[l-1].Length; i++)
            {
                if (mLayers[l -1].IsNeuronOnScreen(mLayers[l - 1][i]) == false || markedNeurons[l-1] == -1)
                    mainScreen.Children.Remove(conections[l-1][i].Representation);
            }
        }

        private void AddToLayer(int l)
        {
            for (int i = 0; i < conections[l-1].Length; i++)
            {
                if (mLayers[l - 1].IsNeuronOnScreen(mLayers[l - 1][i]) && mainScreen.Children.Contains(conections[l-1][i].Representation) == false && markedNeurons[l-1] != -1)
                    mainScreen.Children.Add(conections[l-1][i].Representation);
            }
        }


        private void DrawLayerConection(int l)
        {
            int n = mLayers[l].GetMarkedNeuronNum();
            if(n!=-1)
            {
                for (int i = 0; i < conections[l-1].Length; i++)
                {
                    if (mLayers[l - 1].IsNeuronOnScreen(mLayers[l-1][i]))
                        DrawConection(i, n, l);
                }
            }
        }

        private void SetNewValues(int l)
        {
            int n = mLayers[l].GetMarkedNeuronNum();
            if (n != -1)
            {
                for (int i = 0; i < conections[l - 1].Length; i++)
                {
                    conections[l - 1][i].Value = conectionValues[l - 1][n, i];
                }
            }
        }

        private void LoadWeightScreen(object s, System.EventArgs e)
        {
            int n = (s as ScreenLayer).GetMarkedNeuronNum();
            double[] weights = new double[conections[0].Length];
            for (int i = 0; i < conections[0].Length; i++)
            {
                weights[i] = conectionValues[0][n, i];
            }

            wScreen.SetValues(weights);

        }

    }
}
