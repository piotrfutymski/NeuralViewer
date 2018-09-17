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

namespace NeuralViewer
{
    class ClassicLayer : NumbersRepresentation
    {

        public ClassicLayer(Canvas screen) : base(screen)
        {
            neurons = new List<OneNumberRepresentation>();
            layerSettings = new Dictionary<NumberRepresentationSettings, double>();

            layerSettings.Add(NumberRepresentationSettings.FirstNeuronOnScreen, 0);
            layerSettings.Add(NumberRepresentationSettings.NeuronsOnScreen, 8);
            layerSettings.Add(NumberRepresentationSettings.Size, layerScreen.Height/2);
            layerSettings.Add(NumberRepresentationSettings.Spaces, 4);

            DrawBorder(Brushes.DarkCyan);
            DrawBox();

            Redraw();
        }

        private double CountFirstNeuronPos()
        {
            var neuronsOnScreen = GetSetting(NumberRepresentationSettings.NeuronsOnScreen);
            var defaultSize = GetSetting(NumberRepresentationSettings.Size);
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
            var defaultSize = GetSetting(NumberRepresentationSettings.Size);
            var spaces = GetSetting(NumberRepresentationSettings.Spaces);

            if (CountFirstNeuronPos() > spaces)
                return defaultSize;
            else
                return (layerScreen.Width - (neuronsOnScreen + 1) * spaces) / neuronsOnScreen;
        }

        private void DrawBorder(SolidColorBrush color)
        {
            Border b = new Border();
            b.Height = layerScreen.Height;
            b.Width = layerScreen.Width;
            b.BorderThickness = new Thickness(2);
            var br = color;
            b.BorderBrush = br;
            layerScreen.Children.Add(b);

        }

        private void DrawBox()
        {
            Rectangle b = new Rectangle();
            b.Height = layerScreen.Height;
            b.Width = layerScreen.Width;

            Canvas.SetZIndex(b, -10);
            b.Fill = Brushes.Black;
            layerScreen.Children.Add(b);

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

        public override void Redraw()
        {
            var neuronsOnScreen = GetSetting(NumberRepresentationSettings.NeuronsOnScreen);
            var defaultSize = GetSetting(NumberRepresentationSettings.Size);
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
            }
        }

        //Test only
        private void CreateTestNeurons(int num)
        {
            for (int i = 0; i < num; i++)
            {
                neurons.Add(new ClassicNeuron());             
            }
        }

        public ClassicLayer(Canvas screen, int num) : base(screen)
        {
            neurons = new List<OneNumberRepresentation>();
            CreateTestNeurons(num);

            layerSettings = new Dictionary<NumberRepresentationSettings, double>();
            layerSettings.Add(NumberRepresentationSettings.FirstNeuronOnScreen, 0);
            layerSettings.Add(NumberRepresentationSettings.NeuronsOnScreen, 8);
            layerSettings.Add(NumberRepresentationSettings.Size, layerScreen.Height / 2);
            layerSettings.Add(NumberRepresentationSettings.Spaces, 4);

            DrawBorder(Brushes.DarkCyan);
            DrawBox();

            Redraw();

        }
    }
}
