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
    abstract class ScreenLayer
    {
        public EventHandler OnRedrawing;
        protected Canvas layerScreen;
        protected List<ScreenNeuron> neurons;
        protected Dictionary<NumberRepresentationSettings, double> layerSettings;
        protected Button optionButton;

        protected int markedNeuron;

        protected double hSize;

        public enum NumberRepresentationSettings
        {
            Percent,                                           //     max height of layer
            Spaces,                                             
            NeuronsOnScreen,
            FirstNeuronOnScreen,
            IsWhiteBlack,
            RowNumber,
            Markable
        }

        protected abstract void LayerScreen_MouseWheel(object sender, MouseWheelEventArgs e);
        protected abstract Window SetOptionWindow();
        protected abstract bool CheckingSettingsValue(NumberRepresentationSettings name, double value);
        protected abstract double GetSizeFormPercents(double value);
        public abstract void Redraw();



        public ScreenNeuron this[int i]
        {
            get { return neurons[i];  }
        }

        public int Count()
        {
            return neurons.Count;
        }

        public ScreenNeuron GetMarkedNeuron()
        {
            if (markedNeuron != -1)
                return neurons[markedNeuron];
            else
                return null;
        }

        public int GetMarkedNeuronNum()
        {
            return markedNeuron;
        }

        public ScreenLayer(Canvas screen, int num)
        {
            layerScreen = screen;
            neurons = null;
            layerSettings = null;
            neurons = new List<ScreenNeuron>();
            CreateTestNeurons(num);
            markedNeuron = -1;

            for (int i = 0; i < neurons.Count; i++)
            {
                layerScreen.Children.Add(neurons[i].Representation);
                neurons[i].Representation.MouseLeftButtonDown += (s, e) =>
                {
                    if (GetSetting(NumberRepresentationSettings.Markable) != 0)
                    {
                        if (GetMarkedNeuron() != null)
                            GetMarkedNeuron().DismarkMe();
                        string x = (s as Shape).Name;
                        x = x.Remove(0,1);
                        if(markedNeuron == int.Parse(x))
                            markedNeuron = -1;
                        else
                        {
                            markedNeuron = int.Parse(x);
                            neurons[markedNeuron].MarkMe();
                        }
                        OnRedrawing?.Invoke(this, null);
                    }
                };
            }

            layerScreen.MouseWheel += LayerScreen_MouseWheel;
        }

        public void SetNeurons(double [] v, int beg)
        {
            if (v.Length + beg > neurons.Count)
                throw new ArgumentException();

            for (int i = beg; i < v.Length; i++)
            {
                neurons[i].Value = v[i - beg];
            }
            Redraw();
        }

        public void SetNeuron(double v, int num)
        {
            SetNeurons(new double[] { v }, num);
        } 
        
        public void SetSetting(NumberRepresentationSettings name, double value)
        {
            if(layerSettings.ContainsKey(name))
            {
                if(CheckingSettingsValue(name, value))
                {
                    layerSettings[name] = value;
                    if (name == NumberRepresentationSettings.Percent)
                        hSize = GetSizeFormPercents(value);
                    Redraw();
                }
            }             
            else
                throw new ArgumentException();
        }

        public double GetSetting(NumberRepresentationSettings name)
        {
            if (layerSettings.ContainsKey(name))
            {
                return layerSettings[name];
            }
            else
                throw new ArgumentException();
        }

        public bool IsNeuronOnScreen(ScreenNeuron nn )
        {
            return layerScreen.Children.Contains(nn.Representation);
        }

        protected void DrawBorder(SolidColorBrush color)
        {
            Border b = new Border();
            b.Height = layerScreen.Height;
            b.Width = layerScreen.Width;
            b.BorderThickness = new Thickness(2);
            var br = color;
            b.BorderBrush = br;
            layerScreen.Children.Add(b);

        }

        protected void DrawBox()
        {
            Rectangle b = new Rectangle();
            b.Height = layerScreen.Height;
            b.Width = layerScreen.Width;

            Canvas.SetZIndex(b, -10);
            b.Fill = Brushes.Black;
            layerScreen.Children.Add(b);

        }

        protected void DisplayOptionButton()
        {
            optionButton = new Button();
            optionButton.Width = 20;
            optionButton.Height = 20;
            optionButton.Foreground = Brushes.DarkOrchid;
            LayerOptionWindow w = (LayerOptionWindow)SetOptionWindow();
            
            optionButton.Click += (s, e) => { SetOptionWindow().ShowDialog(); };


            layerScreen.Children.Add(optionButton);
        }

        protected abstract void CreateTestNeurons(int num);

    }
}
