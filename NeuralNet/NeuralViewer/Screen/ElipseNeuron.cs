using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;

namespace NeuralViewer.Screen
{
    class EllipseNeuron: ScreenNeuron
    {
        public EllipseNeuron()       // TODO: Create more useful constructor/s whith NeuralNet Lib
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            NeuronShape = new Ellipse();
            Value = r.NextDouble();

            NeuronShape.Stroke = Brushes.AliceBlue;
        }
    }
}
