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
        // TODO: Add Neurons to the layer  

        protected double baseHeightOnScreen;
        protected double baseWidthOnScreen;
        protected Canvas layerScreen;



        public LayerRepresentation(Canvas screen)
        {
            baseHeightOnScreen = screen.Height;
            baseWidthOnScreen = screen.Width;
            layerScreen = screen;
        }

        abstract public void Redraw();
        
    }
}
