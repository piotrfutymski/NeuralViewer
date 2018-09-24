using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet
{
    class NeuronUtility
    {
        static public double[] CountALayer(double[] back, ConectionsInfo conections)
        {
            var neuronStates = GetZ(back, conections);

            for (int i = 0; i < conections.Fore; i++)
            {
                neuronStates[i] = Sigmoid(neuronStates[i]);
            }
            return neuronStates;
        }

        static public double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        static public double DSigmoid(double x)
        {
            return Sigmoid(x) * (1 - Sigmoid(x));
        }

        static public double[] FirstLayerDerivatives(double[] first, Sample sample)
        {
            if (sample.Predictions.Length != first.Length)
                throw new Exception("bad sample");
            var neuronD = new double[first.Length];
            for (int i = 0; i < first.Length; i++)
            {
                neuronD[i] = 2 * (first[i] - sample.Predictions[i]);
            }
            return neuronD;
        }

        static public double[] LayerDerivatives(out ConectionsInfo conD, double[] foreLayerD, ConectionsInfo con, double[] backLayerA)
        {
            var neuronD = new double[con.Back];
            var weightD = new double[con.Fore, con.Back];
            var biasD = new double[con.Fore];

            var Z = GetZ(backLayerA, con);

            for (int j = 0; j < con.Fore; j++)
            {
                for (int k = 0; k < con.Back; k++)
                {
                    weightD[j, k] = backLayerA[k] * DSigmoid(Z[j]) * foreLayerD[j];
                }
                biasD[j] = DSigmoid(Z[j]) * foreLayerD[j];
            }

            for (int k = 0; k < con.Back; k++)
            {
                neuronD[k] = 0;
                for (int j = 0; j < con.Fore; j++)
                {
                    neuronD[k] += con.WeightMatrix[j, k] * DSigmoid(Z[j]) * foreLayerD[j];
                }
            }

            conD = new ConectionsInfo(con.Fore, con.Back, weightD, biasD);
            return neuronD;
        }

        static double[] GetZ(double[] back, ConectionsInfo conections)
        {
            var Z = new double[conections.Fore];

            for (int i = 0; i < conections.Fore; i++)
            {
                Z[i] = conections.BiasVector[i];
                for (int j = 0; j < conections.Back; j++)
                {
                    Z[i] += conections.WeightMatrix[i, j] * back[j];
                }
            }
            return Z;
        }
    }
}
