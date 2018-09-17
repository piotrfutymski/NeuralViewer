using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace NeuralViewer
{
    class ClassicNeuron:OneNumberRepresentation
    {
        public override double Value
        {
            get { return num; }
            set
            {
                num = value;
                NeuronShape.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)(Value * 255), (byte)(Value * 255), (byte)(Value * 255)));
            }
        }

        public override UIElement Representation 
        {
            get { return NeuronShape; }
        }

        public Visibility Visibility
        {
            get { return NeuronShape.Visibility; }
            set { NeuronShape.Visibility = value; }
        }

        public Ellipse NeuronShape { get; private set; }

        public ClassicNeuron()       // TODO: Create more useful constructor/s whith NeuralNet Lib
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            NeuronShape = new Ellipse();
            Value = r.NextDouble();

            NeuronShape.Stroke = Brushes.AliceBlue;

            SetSize(32);
        }

        public override void SetSize(double radius)
        {
            NeuronShape.Height = radius;
            NeuronShape.Width = radius;
        }

    }
}
