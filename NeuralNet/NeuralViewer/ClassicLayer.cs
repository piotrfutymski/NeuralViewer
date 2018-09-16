using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace NeuralViewer
{
    class ClassicLayer : LayerRepresentation
    {
        int neuronsOnScreen;
        int firstNeuronOnScreen;

        double spaces = 5;
        double defaultSize = 32;

        public ClassicLayer(Canvas screen) : base(screen)
        {
            neurons = new List<NeuronRepresentation>();

            SetDisplayOptions(8, 0);
        }

        public override void Redraw()
        {
            double horizontalPos;
            double firstNeuronPos = CountFirstNeuronPos();
            double neuronSize = CountNeuronSize();

            for (int i = 0; i < neuronsOnScreen; i++)
            {
                horizontalPos = firstNeuronPos + i * (spaces + neuronSize);
                Canvas.SetLeft(neurons[i + firstNeuronOnScreen].Representation, horizontalPos);
                Canvas.SetTop(neurons[i + firstNeuronOnScreen].Representation, (layerScreen.Height - neuronSize) / 2);
                layerScreen.Children.Add(neurons[i + firstNeuronOnScreen].Representation);
                (neurons[i + firstNeuronOnScreen] as ClassicNeuron).SetRadius(neuronSize);
            }
        }

        private void SetDisplayOptions(int n, int f)
        {
            neuronsOnScreen = n;
            firstNeuronOnScreen = f;

            Redraw();
        }

        private double CountFirstNeuronPos()
        {
            double nLenght = neuronsOnScreen * (defaultSize + spaces) + spaces;

            if (nLenght > layerScreen.Width)
                return spaces;
            else
                return ((layerScreen.Width - nLenght) / 2 + spaces);
        }

        private double CountNeuronSize()
        {
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
            b.BorderThickness = new System.Windows.Thickness(2);
            var br = color;
            b.BorderBrush = br;
            layerScreen.Children.Add(b);

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
            neurons = new List<NeuronRepresentation>();
            CreateTestNeurons(num);
            SetDisplayOptions(num, 0);
            DrawBorder(Brushes.DarkCyan);
        }
    }
}
