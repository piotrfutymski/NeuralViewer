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
    class Conection
    {
        double num;
        Line representation;

        public Conection()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            representation = new Line();
            representation.StrokeThickness = 3;
            Value = r.NextDouble()*2 -1;
        }

        public double Value
        {
            get { return num; }
            set
            {
                num = value;
                if (num <= 0 && num > -3)
                    representation.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, (byte)((3d + num) * 255), (byte)((3d + num) * 255)));
                else if(num < -3)
                    representation.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                else if(num > 0 && num < 3)
                    representation.Stroke = new SolidColorBrush(Color.FromArgb(255, (byte)((3d - num) * 255), 255, (byte)((3d - num) * 255)));
                else
                    representation.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            }
        }

        public Line Representation { get => representation;}

        public void SetLinePosition(double x1, double x2, double y1, double y2)
        {
            representation.X1 = x1;
            representation.X2 = x2;
            representation.Y1 = y1;
            representation.Y2 = y2;
        }

    }
}
