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
        int neuronsOnScreen;
        int firstNeuronOnScreen;

        double spaces = 5;
        double defaultSize = 50;

        public ClassicLayer(Canvas screen) : base(screen)
        {
            neurons = new List<OneNumberRepresentation>();
            SetDisplayOptions(8, 0);
        }

        private void SetDisplayOptions(int n, int f)
        {
            neuronsOnScreen = n;
            firstNeuronOnScreen = f;

            if (neuronsOnScreen > neurons.Count)
                neuronsOnScreen = neurons.Count;

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

        protected override void LayerScreen_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if(layerScreen.Children[1].IsMouseOver)
            {
                if(neuronsOnScreen + e.Delta/50 > 0)
                SetDisplayOptions(neuronsOnScreen + e.Delta / 50, firstNeuronOnScreen);
                Redraw();
            }
        }

        public override void Redraw()
        {
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

                Canvas.SetLeft(neurons[i + firstNeuronOnScreen].Representation, horizontalPos);
                Canvas.SetTop(neurons[i + firstNeuronOnScreen].Representation, (layerScreen.Height - neuronSize) / 2);

                layerScreen.Children.Add(neurons[i + firstNeuronOnScreen].Representation);
                neurons[i + firstNeuronOnScreen].SetSize(neuronSize);
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
            DrawBorder(Brushes.DarkCyan);
            DrawBox();

            SetDisplayOptions(16, 0);
        }
    }
}
