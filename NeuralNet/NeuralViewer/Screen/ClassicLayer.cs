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
    class ClassicLayer : ScreenLayer
    {

        public override void Redraw()
        {
            var neuronsOnScreen = GetSetting(NumberRepresentationSettings.NeuronsOnScreen);
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
                    neurons[i + (int)firstNeuronOnScreen].ColorType = ScreenNeuron.ColorTypes.GreenRed;
                else
                    neurons[i + (int)firstNeuronOnScreen].ColorType = ScreenNeuron.ColorTypes.WhiteBlack;
            }

            OnRedrawing?.Invoke(this, null);
        }

        private double CountFirstNeuronPos()
        {
            var neuronsOnScreen = GetSetting(NumberRepresentationSettings.NeuronsOnScreen);
            var spaces = GetSetting(NumberRepresentationSettings.Spaces);

            double nLenght = neuronsOnScreen * (hSize + spaces) + spaces;

            if (nLenght > layerScreen.Width)
                return spaces;
            else
                return ((layerScreen.Width - nLenght) / 2 + spaces);
        }

        private double CountNeuronSize()
        {
            var neuronsOnScreen = GetSetting(NumberRepresentationSettings.NeuronsOnScreen);
            var spaces = GetSetting(NumberRepresentationSettings.Spaces);

            if (CountFirstNeuronPos() > spaces)
                return hSize;
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

        protected override Window SetOptionWindow()
        {
            LayerOptionWindow w = new LayerOptionWindow();
            Slider sizeSlieder = w.FindName("SizeOption") as Slider;
            Slider advancedSlieder = w.FindName("AdvancedOption") as Slider;

            sizeSlieder.Maximum = neurons.Count;
            sizeSlieder.Value = GetSetting(NumberRepresentationSettings.NeuronsOnScreen);
            w.AdvInt = (int)GetSetting(NumberRepresentationSettings.FirstNeuronOnScreen);
            w.SizeInt = (int)GetSetting(NumberRepresentationSettings.NeuronsOnScreen);
            sizeSlieder.ValueChanged += (e, s) =>
            {
                w.SizeInt = (int)GetSetting(NumberRepresentationSettings.NeuronsOnScreen);
                if (GetSetting(NumberRepresentationSettings.NeuronsOnScreen) != (int)sizeSlieder.Value)
                    SetSetting(NumberRepresentationSettings.NeuronsOnScreen, (int)sizeSlieder.Value);
            
            };


            advancedSlieder.Maximum = neurons.Count;
            advancedSlieder.Value = GetSetting(NumberRepresentationSettings.FirstNeuronOnScreen);

            advancedSlieder.ValueChanged += (e, s) =>
            {
                w.AdvInt = (int)GetSetting(NumberRepresentationSettings.FirstNeuronOnScreen);
                if (GetSetting(NumberRepresentationSettings.FirstNeuronOnScreen) != (int)advancedSlieder.Value)
                    SetSetting(NumberRepresentationSettings.FirstNeuronOnScreen, (int)advancedSlieder.Value);
            };
            return w;

        }

        protected override bool CheckingSettingsValue(NumberRepresentationSettings name, double value)
        {
            switch (name)
            {
                case NumberRepresentationSettings.Percent:
                    if (value > 100 || value < 0) return false;
                    break;
                case NumberRepresentationSettings.Spaces:
                    if (value > hSize || value < 0) return false;
                    break;
                case NumberRepresentationSettings.NeuronsOnScreen:
                    if (value > neurons.Count - layerSettings[NumberRepresentationSettings.FirstNeuronOnScreen] || value < 0) return false;
                    break;
                case NumberRepresentationSettings.FirstNeuronOnScreen:
                    if (value > neurons.Count - layerSettings[NumberRepresentationSettings.NeuronsOnScreen] || value < 0) return false;
                    break;
                case NumberRepresentationSettings.IsWhiteBlack:
                    break;
                case NumberRepresentationSettings.RowNumber:
                    if (value != 1) return false;
                    break;
            }
            return true;
        }

        protected override double GetSizeFormPercents(double value)
        {
            return value / 100 * layerScreen.Height;
        }


        //Test only

        public ClassicLayer(Canvas screen, int num, int nr) : base(screen, num, nr)
        {
            layerSettings = new Dictionary<NumberRepresentationSettings, double>();
            layerSettings.Add(NumberRepresentationSettings.FirstNeuronOnScreen, 0);
            layerSettings.Add(NumberRepresentationSettings.NeuronsOnScreen, 8);
            layerSettings.Add(NumberRepresentationSettings.Percent, 75);
            layerSettings.Add(NumberRepresentationSettings.Spaces, 4);
            layerSettings.Add(NumberRepresentationSettings.IsWhiteBlack, 1);
            layerSettings.Add(NumberRepresentationSettings.RowNumber, 1);
            layerSettings.Add(NumberRepresentationSettings.Markable, 1);
            hSize = GetSizeFormPercents(75);


            DrawBorder(Brushes.DarkCyan);
            DrawBox();
            DisplayOptionButton();

            Redraw();
        }

        protected override void CreateTestNeurons(int num)
        {
            for (int i = 0; i < num; i++)
            {
                neurons.Add(new EllipseNeuron());
                neurons[i].Representation.Name ="n" + i.ToString();
            }

        }
    }
}
