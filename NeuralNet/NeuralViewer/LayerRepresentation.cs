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

        public void SetNeurons(int [] v, int beg)
        {
            if (v.Length + beg > neurons.Count)
                throw new ArgumentException();

            for (int i = beg; i < v.Length; i++)
            {
                neurons[i].Value = v[i - beg];
            }
            Redraw();
        }

        public void SetNeuron(int v, int num)
        {
            SetNeurons(new int[] { v }, num);
        }

        abstract public void Redraw();
        
    }
}
