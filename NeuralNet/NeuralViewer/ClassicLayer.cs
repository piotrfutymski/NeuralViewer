using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NeuralViewer
{
    class ClassicLayer : LayerRepresentation
    {
        int neuronsOnScreen;
        int firstNeuronOnScreen;

        public ClassicLayer(Canvas screen) : base(screen)
        {

        }

        public override void Redraw()
        {

        }
    }
}
