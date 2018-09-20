using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;

namespace NeuralViewer.Screen
{
    class PixelNeuron:ScreenNeuron
    {
        public PixelNeuron()       // TODO: Create more useful constructor/s whith NeuralNet Lib
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            NeuronShape = new Rectangle();
            Value = r.NextDouble();

            NeuronShape.StrokeThickness = 0;
            NeuronShape.Stroke = Brushes.Red;
        }

        public override void DismarkMe()
        {
            NeuronShape.StrokeThickness = 0;
        }

        public override void MarkMe()
        {
            NeuronShape.StrokeThickness = 1;
        }
    }
}
