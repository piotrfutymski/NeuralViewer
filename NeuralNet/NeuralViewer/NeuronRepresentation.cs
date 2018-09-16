using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralViewer
{
    class NeuronRepresentation
    {
        public double Value { get; private set; }

        public NeuronRepresentation()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            Value = r.NextDouble();
        }
    }
}
