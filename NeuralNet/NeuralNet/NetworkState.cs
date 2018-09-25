using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet
{
    public class NetworkState
    {
        private List<ConectionsInfo> mConections;
        private List<double[]> mLayers;
        private Sample mSample;

        public int Prediction
        {
            get
            {
               return Array.IndexOf(mLayers.Last(), mLayers.Last().Max());
            }
        }

        public int LayerNumber
        {
            get
            {
                return mLayers.Count;
            }
        }

        public NetworkState(List<ConectionsInfo> c, Sample s)
        {
            mConections = new List<ConectionsInfo>();
            for (int i = 0; i < c.Count; i++)
            {
                mConections.Add((ConectionsInfo)c[i].Clone());
            }
            mLayers = new List<double[]>
            {
                s.GetFirstLayer()
            };
            mSample = s;
            if (mConections[0].Back != s.Data.Length)
                throw new Exception("Bad Sample");

            for (int i = 0; i < mConections.Count; i++)
            {
                mLayers.Add(NeuronUtility.CountALayer(mLayers[i], mConections[i]));
            }
        }

        public void Update(Sample s)
        {
            mLayers[0] = s.GetFirstLayer();
            mSample = s;

            if (mConections[0].Back != s.Data.Length)
                throw new Exception("Bad Sample");

            for (int i = 0; i < mConections.Count; i++)
            {
                mLayers[i+1] = NeuronUtility.CountALayer(mLayers[i], mConections[i]);
            }
        }

        public List<ConectionsInfo> GetMinusGradient()
        {
            var res = new List<ConectionsInfo>();
            var flD = NeuronUtility.FirstLayerDerivatives(mLayers.Last(), mSample);

            for (int i = 0; i < mConections.Count; i++)
            {
                flD = NeuronUtility.LayerDerivatives(out ConectionsInfo c, flD, mConections[mConections.Count - i - 1], mLayers[mLayers.Count - i - 2]);
                c.Minus();
                res.Add(c);
            }
            res.Reverse();

            return res;
        }

        public double GetWeight(int cnumber, int fore, int back)
        {
            try
            {
                return mConections[cnumber].WeightMatrix[fore, back];
            }
            catch
            {
                return 0;
            }
        }

        public double GetNeuronValue(int lnumber, int neuron)
        {
            try
            {
                return mLayers[lnumber][neuron];
            }
            catch
            {
                return 0;
            }           
        }

        public double[] GetLayer(int n)
        {
            return mLayers[n];
        }

        public void Write()
        {
            string layerstring;
            layerstring = "Layer L-0: \t";
            for (int j = 0; j < mLayers.Last().Length; j++)
            {
                layerstring += Math.Round(mLayers.Last()[j], 2);
                layerstring += "\t";
            }

            Console.WriteLine("Neural State with given sample:");
            Console.WriteLine(layerstring);

            string predstring = "Should be:\t";
            for (int i = 0; i < mSample.Predictions.Length; i++)
            {
                predstring += Math.Round(mSample.Predictions[i], 2);
                predstring += "\t";
            }
            Console.WriteLine(predstring);
        }
    }
}
