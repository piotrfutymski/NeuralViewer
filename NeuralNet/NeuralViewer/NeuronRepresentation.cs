using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralViewer
{
    abstract class NeuronRepresentation
    {
        public abstract double Value
        {
            get;
            set;
        }
    }
}
