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
    class ClassicLayer : NumbersRepresentation<Ellipse>
    {

        public ClassicLayer(Canvas screen) : base(screen)
        {
            neurons = new List<OneNumberRepresentation<Ellipse>>();
            layerSettings = new Dictionary<NumberRepresentationSettings, double>();

            Redraw();
        }

        public override void Redraw()
        {
            var neuronsOnScreen = GetSetting(NumberRepresentationSettings.NeuronsOnScreen);
            var defaultSize = GetSetting(NumberRepresentationSettings.HSize);
            var spaces = GetSetting(NumberRepresentationSettings.Spaces);
            var firstNeuronOnScreen = GetSetting(NumberRepresentationSettings.FirstNeuronOnScreen);

            double horizontalPos;
            double firstNeuronPos = CountFirstNeuronPos();
            double neuronSize = CountNeuronSize();

            for (int i = 0; i < neurons.Count; i++)
            {
                layerScreen.Children.Remove(neurons[i].Representation);
            }

            for (int i = 0; i < neuronsOnScreen; i++)
            {
                if (i == neurons.Count)
                    break;

                horizontalPos = firstNeuronPos + i * (spaces + neuronSize);

                Canvas.SetLeft(neurons[i + (int)firstNeuronOnScreen].Representation, horizontalPos);
                Canvas.SetTop(neurons[i + (int)firstNeuronOnScreen].Representation, (layerScreen.Height - neuronSize) / 2);

                layerScreen.Children.Add(neurons[i + (int)firstNeuronOnScreen].Representation);
                neurons[i + (int)firstNeuronOnScreen].SetSize(neuronSize);

                if (GetSetting(NumberRepresentationSettings.IsWhiteBlack) == 0)
                    neurons[i + (int)firstNeuronOnScreen].ColorType = OneNumberRepresentation<Ellipse>.ColorTypes.GreenRed;
                else
                    neurons[i + (int)firstNeuronOnScreen].ColorType = OneNumberRepresentation<Ellipse>.ColorTypes.WhiteBlack;
            }
        }

        private double CountFirstNeuronPos()
        {
            var neuronsOnScreen = GetSetting(NumberRepresentationSettings.NeuronsOnScreen);
            var defaultSize = GetSetting(NumberRepresentationSettings.HSize);
            var spaces = GetSetting(NumberRepresentationSettings.Spaces);

            double nLenght = neuronsOnScreen * (defaultSize + spaces) + spaces;

            if (nLenght > layerScreen.Width)
                return spaces;
            else
                return ((layerScreen.Width - nLenght) / 2 + spaces);
        }

        private double CountNeuronSize()
        {
            var neuronsOnScreen = GetSetting(NumberRepresentationSettings.NeuronsOnScreen);
            var defaultSize = GetSetting(NumberRepresentationSettings.HSize);
            var spaces = GetSetting(NumberRepresentationSettings.Spaces);

            if (CountFirstNeuronPos() > spaces)
                return defaultSize;
            else
                return (layerScreen.Width - (neuronsOnScreen + 1) * spaces) / neuronsOnScreen;
        }

        protected override void LayerScreen_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(layerScreen.Children[1].IsMouseOver || layerScreen.IsMouseOver)
            {
                if(GetSetting(NumberRepresentationSettings.NeuronsOnScreen) + e.Delta/50 > 0)
                SetSetting(NumberRepresentationSettings.NeuronsOnScreen, GetSetting(NumberRepresentationSettings.NeuronsOnScreen) + e.Delta / 50);
                Redraw();
            }
        }

        protected override bool CheckingSettingsValue(NumberRepresentationSettings name, double value)
        {
            switch (name)
            {
                case NumberRepresentationSettings.HSize:
                    if (value > layerScreen.Height || value < 0) return false;
                    break;
                case NumberRepresentationSettings.Spaces:
                    if (value > layerSettings[NumberRepresentationSettings.HSize]|| value < 0) return false;
                    break;
                case NumberRepresentationSettings.NeuronsOnScreen:
                    if (value > neurons.Count + layerSettings[NumberRepresentationSettings.FirstNeuronOnScreen] || value < 0) return false;
                    break;
                case NumberRepresentationSettings.FirstNeuronOnScreen:
                    if (value > neurons.Count || value < 0) return false;
                    break;
                case NumberRepresentationSettings.IsWhiteBlack:
                    break;
                case NumberRepresentationSettings.RowNumber:
                    if (value != 1) return false;
                    break;
            }
            return true;
        }


        //Test only
        private void CreateTestNeurons(int num)
        {
            for (int i = 0; i < num; i++)
            {
                neurons.Add(new OneNumberRepresentation<Ellipse>());             
            }
        }

        public ClassicLayer(Canvas screen, int num) : base(screen)
        {
            neurons = new List<OneNumberRepresentation<Ellipse>>();
            CreateTestNeurons(num);

            layerSettings = new Dictionary<NumberRepresentationSettings, double>();
            layerSettings.Add(NumberRepresentationSettings.FirstNeuronOnScreen, 0);
            layerSettings.Add(NumberRepresentationSettings.NeuronsOnScreen, 8);
            layerSettings.Add(NumberRepresentationSettings.HSize, layerScreen.Height / 2);
            layerSettings.Add(NumberRepresentationSettings.Spaces, 4);
            layerSettings.Add(NumberRepresentationSettings.IsWhiteBlack, 1);
            layerSettings.Add(NumberRepresentationSettings.RowNumber, 1);

            DrawBorder(Brushes.DarkCyan);
            DrawBox();

            Redraw();
        }
    }
}
