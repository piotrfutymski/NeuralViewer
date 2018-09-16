using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeuralViewer
{
    abstract class NeuronRepresentation
    {
        protected double num;

        public abstract double Value
        {
            get;
            set;
        }

        public abstract UIElement Representation
        {
            get;
        }
    }
}
