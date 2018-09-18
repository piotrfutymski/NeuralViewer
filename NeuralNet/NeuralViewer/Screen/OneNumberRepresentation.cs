using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace NeuralViewer.Screen
{
    class OneNumberRepresentation<T> where T : Shape, new()
    {
        public enum ColorTypes
        {
            GreenRed,
            WhiteBlack
        }

        private double num;
        private ColorTypes colorType;

        public ColorTypes ColorType
        {
            get { return colorType; }
            set
            {
                colorType = value;
                Value = num;
            }
        }

        public double Value
        {
            get { return num; }
            set
            {
                num = value;
                if(colorType == ColorTypes.WhiteBlack)
                    Representation.Fill= new SolidColorBrush(Color.FromArgb(255, (byte)(num * 255), (byte)(num * 255), (byte)(num * 255)));
                else if(colorType == ColorTypes.GreenRed)
                {
                    if(num <= 0)
                        Representation.Fill = new SolidColorBrush(Color.FromArgb(255, 255, (byte)((1d+num) * 255), (byte)((1d + num) * 255) ));
                    else
                        Representation.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)((1d - num) * 255), 255, (byte)((1d - num) * 255)));
                }
            }
        }

        public  Shape Representation
        {
            get { return NeuronShape; }
        }

        public T NeuronShape { get; private set; }

        public OneNumberRepresentation()       // TODO: Create more useful constructor/s whith NeuralNet Lib
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            NeuronShape =  new T();
            Value = r.NextDouble();

            NeuronShape.Stroke = Brushes.AliceBlue;

        }

        public void SetSize(double x)
        {
            NeuronShape.Height = x;
            NeuronShape.Width = x;
        }

    };
}
