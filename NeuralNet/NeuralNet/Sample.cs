using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NeuralNet
{
    public class Sample
    {
        public double[] Data { get; private set; }
        public double[] Predictions { get; private set; }

        public Sample(double[] d, double[] p)
        {
            Data = (double[])d.Clone();
            Predictions = (double[])p.Clone();
        }

        public Sample(StreamReader sr, int dc, int pc)
        {
            Data = new double[dc];
            Predictions = new double[pc];
            for (int i = 0; i < dc; i++)
            {
                Data[i] = double.Parse(sr.ReadLine());
            }
            for (int i = 0; i < pc; i++)
            {
                Predictions[i] = double.Parse(sr.ReadLine());
            }
        }

        public double[] GetFirstLayer()
        {
            return (double[])Data.Clone();
        }
    }
}
