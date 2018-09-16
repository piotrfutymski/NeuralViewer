using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NeuralViewer
{
    abstract class LayerRepresentation
    {

        protected Canvas layerScreen;
        protected List<NeuronRepresentation> neurons;

        public LayerRepresentation(Canvas screen)
        {
            layerScreen = screen;
            neurons = null;
        }

        abstract public void Redraw();
        
    }
}
