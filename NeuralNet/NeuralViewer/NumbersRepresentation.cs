using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NeuralViewer
{
    abstract class NumbersRepresentation
    {

        protected Canvas layerScreen;
        protected List<OneNumberRepresentation> neurons;

        public NumbersRepresentation(Canvas screen)
        {
            layerScreen = screen;
            neurons = null;
            layerScreen.MouseWheel += LayerScreen_MouseWheel;
        }

        protected abstract void LayerScreen_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e);
        public abstract void Redraw();

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
    }
}
