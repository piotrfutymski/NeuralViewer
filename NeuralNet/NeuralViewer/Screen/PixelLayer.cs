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
            return hSize / GetSetting(NumberRepresentationSettings.RowNumber);
        }

        private double CountFirstNeuronLeftPos()
        {
            double nLenght = CountNumberInLine() * CountPixelSize();
            return (layerScreen.Width - nLenght) / 2;
        }

        private double CountFirstNeuronTopPos()
        {
            return (layerScreen.Height - hSize) / 2;
        }


        protected override void LayerScreen_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (layerScreen.Children[1].IsMouseOver || layerScreen.IsMouseOver)
            {
                SetSetting(NumberRepresentationSettings.Percent, GetSetting(NumberRepresentationSettings.Percent) + e.Delta / 100);
                Redraw();
            }
        }

        protected override Window SetOptionWindow()
        {
            LayerOptionWindow w = new LayerOptionWindow();
            Slider sizeSlieder = w.FindName("SizeOption") as Slider;
            Slider advancedSlieder = w.FindName("AdvancedOption") as Slider;

            w.SizeInt = (int)GetSetting(NumberRepresentationSettings.Percent);
            w.AdvInt = (int)GetSetting(NumberRepresentationSettings.RowNumber);

            sizeSlieder.Maximum = 100;
            sizeSlieder.Value = GetSetting(NumberRepresentationSettings.Percent);
            sizeSlieder.ValueChanged += (e, s) =>
            {
                if (GetSetting(NumberRepresentationSettings.Percent) != (int)sizeSlieder.Value)
                {
                    SetSetting(NumberRepresentationSettings.Percent, (int)sizeSlieder.Value);
                    SetSetting(NumberRepresentationSettings.RowNumber, (int)advancedSlieder.Value);
                    w.SizeInt = (int)GetSetting(NumberRepresentationSettings.Percent);
                }
            };

            advancedSlieder.Maximum = neurons.Count / 4;
            advancedSlieder.Value = GetSetting(NumberRepresentationSettings.RowNumber);

            advancedSlieder.ValueChanged += (e, s) =>
            {
                if (GetSetting(NumberRepresentationSettings.RowNumber) != (int)advancedSlieder.Value)
                {
                    SetSetting(NumberRepresentationSettings.Percent, (int)sizeSlieder.Value);
                    SetSetting(NumberRepresentationSettings.RowNumber, (int)advancedSlieder.Value);
                    w.AdvInt = (int)GetSetting(NumberRepresentationSettings.RowNumber);
                }
            };
            return w;
        }

        protected override bool CheckingSettingsValue(NumberRepresentationSettings name, double value)
        {
            switch (name)
            {
                case NumberRepresentationSettings.Percent:
                    if (value > 100 ||  value < 0) return false;
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
                    if (value > neurons.Count ||
                        (hSize / value * neurons.Count /  value) > layerScreen.Width)
                        return false;
                    break;
            }
            return true;
        }

        protected override double GetSizeFormPercents(double value)
        {
            double normal = value / 100 * layerScreen.Height;
            double widthalert = value / 100 * layerScreen.Width * GetSetting(NumberRepresentationSettings.RowNumber) * GetSetting(NumberRepresentationSettings.RowNumber) / neurons.Count;
            if ((normal / GetSetting(NumberRepresentationSettings.RowNumber) * neurons.Count / (int)GetSetting(NumberRepresentationSettings.RowNumber)) > layerScreen.Width)
                return widthalert;
            else
                return normal;
        }


        //Test only
       

        public PixelLayer(Canvas screen, int num) : base(screen, num)
        {          

            layerSettings = new Dictionary<NumberRepresentationSettings, double>();
            layerSettings.Add(NumberRepresentationSettings.FirstNeuronOnScreen, 0);
            layerSettings.Add(NumberRepresentationSettings.NeuronsOnScreen, 0);
            layerSettings.Add(NumberRepresentationSettings.Percent, 80);
            layerSettings.Add(NumberRepresentationSettings.Spaces, 0);
            layerSettings.Add(NumberRepresentationSettings.IsWhiteBlack, 1);
            layerSettings.Add(NumberRepresentationSettings.RowNumber, 8);
            layerSettings.Add(NumberRepresentationSettings.Markable, 1);
            hSize = GetSizeFormPercents(80);

            DrawBorder(Brushes.DarkCyan);
            DrawBox();
            DisplayOptionButton();

            Redraw();
        }

        protected override void CreateTestNeurons(int num)
        {
           for (int i = 0; i < num; i++)
           {
                neurons.Add(new PixelNeuron());
                neurons[i].Representation.Name = "n" + i.ToString();
           }

        }
    }
}


