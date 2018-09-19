using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;


namespace NeuralViewer.Screen
{
    class PixelLayer : ScreenLayer
    {
        public PixelLayer(Canvas screen) : base(screen)
        {
            neurons = new List<ScreenNeuron>();
            layerSettings = new Dictionary<NumberRepresentationSettings, double>();

            Redraw();
        }

        public override void Redraw()
        {

            int NIL = CountNumberInLine();
            double fn_L = CountFirstNeuronLeftPos();
            double fn_T = CountFirstNeuronTopPos();
            double pS = CountPixelSize();


            for (int i = 0; i < neurons.Count; i++)
            {

                Canvas.SetLeft(neurons[i].Representation, fn_L + (i%NIL) * pS);
                Canvas.SetTop(neurons[i].Representation, fn_T + (i/NIL) * pS);

                neurons[i].SetSize(pS);

                if (GetSetting(NumberRepresentationSettings.IsWhiteBlack) == 0)
                    neurons[i].ColorType = ScreenNeuron.ColorTypes.GreenRed;
                else
                    neurons[i].ColorType = ScreenNeuron.ColorTypes.WhiteBlack;
            }
        }

        private int CountNumberInLine()
        {
            return neurons.Count / (int)GetSetting(NumberRepresentationSettings.RowNumber);
        }

        private double CountPixelSize()
        {
            return GetSetting(NumberRepresentationSettings.HSize) / GetSetting(NumberRepresentationSettings.RowNumber);
        }

        private double CountFirstNeuronLeftPos()
        {
            double nLenght = CountNumberInLine() * CountPixelSize();
            return (layerScreen.Width - nLenght) / 2;
        }

        private double CountFirstNeuronTopPos()
        {
            return (layerScreen.Height - GetSetting(NumberRepresentationSettings.HSize)) / 2;
        }


        protected override void LayerScreen_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (layerScreen.Children[1].IsMouseOver || layerScreen.IsMouseOver)
            {
                SetSetting(NumberRepresentationSettings.HSize, GetSetting(NumberRepresentationSettings.HSize) + e.Delta / 50);
                Redraw();
            }
        }

        protected override bool CheckingSettingsValue(NumberRepresentationSettings name, double value)
        {
            switch (name)
            {
                case NumberRepresentationSettings.HSize:
                    if (value > layerScreen.Height || 
                        (value / GetSetting(NumberRepresentationSettings.RowNumber) * neurons.Count / (int)GetSetting(NumberRepresentationSettings.RowNumber)) > layerScreen.Width || 
                        value < 0) return false;
                    break;
                case NumberRepresentationSettings.Spaces:
                    if (value != 0) return false;
                    break;
                case NumberRepresentationSettings.NeuronsOnScreen:
                    if (value != 0) return false;
                    break;
                case NumberRepresentationSettings.FirstNeuronOnScreen:
                    if (value != 0) return false;
                    break;
                case NumberRepresentationSettings.IsWhiteBlack:
                    break;
                case NumberRepresentationSettings.RowNumber:
                    if (value > neurons.Count) return false;
                    break;
            }
            return true;
        }


        //Test only
        private void CreateTestNeurons(int num)
        {
            for (int i = 0; i < num; i++)
            {
                neurons.Add(new PixelNeuron());
            }
        }

        public PixelLayer(Canvas screen, int num) : base(screen)
        {
            neurons = new List<ScreenNeuron>();
            CreateTestNeurons(num);

            layerSettings = new Dictionary<NumberRepresentationSettings, double>();
            layerSettings.Add(NumberRepresentationSettings.FirstNeuronOnScreen, 0);
            layerSettings.Add(NumberRepresentationSettings.NeuronsOnScreen, 0);
            layerSettings.Add(NumberRepresentationSettings.HSize, layerScreen.Height / 2);
            layerSettings.Add(NumberRepresentationSettings.Spaces, 0);
            layerSettings.Add(NumberRepresentationSettings.IsWhiteBlack, 1);
            layerSettings.Add(NumberRepresentationSettings.RowNumber, 16);

            for (int i = 0; i < neurons.Count; i++)
            {
                layerScreen.Children.Add(neurons[i].Representation);
            }

            DrawBorder(Brushes.DarkCyan);
            DrawBox();

            Redraw();
        }
    }
}


