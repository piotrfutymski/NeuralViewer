using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;

namespace NeuralViewer
{
    class ClassicNeuron:NeuronRepresentation
    {
        public override double Value
        {
            get { return Value; }
            set
            {
                Value = value;
                NeuronShape.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)(Value * 255), (byte)(Value * 255), (byte)(Value * 255)));
            }
        }

        public Ellipse NeuronShape { get; private set; }

        public ClassicNeuron()       // TODO: Create more useful constructor/s whith NeuralNet Lib
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            NeuronShape = new Ellipse();
            Value = r.NextDouble();

            NeuronShape.Stroke = Brushes.AliceBlue;
            NeuronShape.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            SetRadius(32);
        }

        public void SetRadius(double radius)
        {
            NeuronShape.Height = radius;
            NeuronShape.Width = radius;
        }

    }
}
