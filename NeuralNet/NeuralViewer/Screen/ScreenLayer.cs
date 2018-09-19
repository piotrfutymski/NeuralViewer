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

        protected Canvas layerScreen;
        protected List<ScreenNeuron> neurons;
        protected Dictionary<NumberRepresentationSettings, double> layerSettings;
        protected Button optionButton;
        protected Window optionWindow;

        protected double hSize;

        public enum NumberRepresentationSettings
        {
            Percent,                                           //     max height of layer
            Spaces,                                             
            NeuronsOnScreen,
            FirstNeuronOnScreen,
            IsWhiteBlack,
            RowNumber
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

        public ScreenLayer(Canvas screen)
        {
            layerScreen = screen;
            neurons = null;
            layerSettings = null;

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
            
            optionWindow = SetOptionWindow();
            optionButton.Click += (s, e) => { optionWindow.ShowDialog(); };


            layerScreen.Children.Add(optionButton);
        }
    }
}
