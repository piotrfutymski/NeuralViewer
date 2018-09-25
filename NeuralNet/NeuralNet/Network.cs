using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace NeuralNet
{
    public class Network
    {
        private List<ConectionsInfo> mConections;
        string fileName;
        Random rand;

        public Network(string fN)
        {
            fileName = fN;
            rand = new Random(DateTime.Now.Millisecond + DateTime.Now.Second);
            mConections = new List<ConectionsInfo>();
            try
            {
                LoadFromFile(fN);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Network(int[] layers, string fN)
        {
            fileName = fN;
            rand = new Random(DateTime.Now.Millisecond + DateTime.Now.Second);
            mConections = new List<ConectionsInfo>();
            for (int i = 0; i < layers.Length - 1; i++)
            {
                mConections.Add(new ConectionsInfo(layers[i + 1], layers[i]));
                mConections[i].LoadRandom();
            }
            try
            {
                Save();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int[] Predict(List<Sample> s)
        {
            var res = new int[s.Count];
            for (int i = 0; i < s.Count; i++)
            {
                res[i] = Predict(s[i]);
            }
            return res;
        }

        public int Predict(Sample s)
        {
            var ns = new NetworkState(mConections, s);
            return ns.Prediction;
        }


        public List<NetworkState> GetNetworkState(List<Sample> s)
        {
            var res = new List<NetworkState>();
            for (int i = 0; i < s.Count; i++)
            {
                res.Add(GetFinalLayer(s[i]));
            }
            return res;
        }

        public NetworkState GetFinalLayer(Sample s)
        {
            return new NetworkState(mConections, s);
        }


        public int Train(int s, List<Sample> sampList, double DELTA = 0.01, int numToSum = 25)
        {
            int how_many_times_upgraded = 0;

            var maxTime = new TimeSpan(0, 0, s);
            var actTime = new TimeSpan(0);
            int num = sampList.Count;

            var ns = GetBeginingNS(sampList[0]);
            Func<Object, List<ConectionsInfo>> act = CountMinusGradient;
            var tasks = GetBeginingTasks(ns, sampList, numToSum);

            while (actTime < maxTime)
            {
                var startTime = DateTime.Now;
                
                Task.Factory.ContinueWhenAny(tasks, (winner) =>
                {
                    var gradientList = winner.Result;
                    for (int i = 0; i < mConections.Count; i++)
                    {
                        gradientList[i] = gradientList[i] * DELTA;
                        mConections[i] += gradientList[i];
                    }
                    CountMinusGradientArgument a = new CountMinusGradientArgument();
                    a.sampList = sampList;
                    a.numToSum = numToSum;
                    a.net = ns[Array.IndexOf(tasks, winner)];
                    winner.Dispose();
                    if (actTime < maxTime)
                        winner = Task<List<ConectionsInfo>>.Factory.StartNew(act, a);

                    how_many_times_upgraded += numToSum;
                });
                Task.Factory.ContinueWhenAll(tasks, (a) =>
                {
                    for (int i = 0; i < a.Length; i++)
                    {
                        a[i].Dispose();
                    } }).Wait();

                actTime += DateTime.Now - startTime;
            }

            Save();
            return how_many_times_upgraded;
        }

        NetworkState[] GetBeginingNS(Sample s)
        {
            int length = Environment.ProcessorCount;
            if (length > 1) length--;
            var res = new NetworkState[length];

            for (int i = 0; i < length; i++)
            {
                res[i] = new NetworkState(mConections, s);
            }
            return res;
        }
        Task<List<ConectionsInfo>>[] GetBeginingTasks(NetworkState[] ns, List<Sample> sampList, int numToSum)
        {
            int length = Environment.ProcessorCount;
            if (length > 1) length--;
            var res = new Task<List<ConectionsInfo>>[length];

            Func<Object, List<ConectionsInfo>> act = CountMinusGradient;

            for (int i = 0; i < length; i++)
            {
                CountMinusGradientArgument a = new CountMinusGradientArgument();
                a.sampList = sampList;
                a.numToSum = numToSum;
                a.net = ns[i];
                res[i] = Task<List<ConectionsInfo>>.Factory.StartNew(act, a);
            }
            return res;
        }

        public int Train(int s, string sampleFileName, double DELTA = 0.01, int numToSum = 25)
        {

            var sr = new StreamReader(sampleFileName);
            int num = int.Parse(sr.ReadLine());
            int dc = int.Parse(sr.ReadLine());
            int pc = int.Parse(sr.ReadLine());
            var sampList = new List<Sample>();
            for (int i = 0; i < num; i++)
            {
                sampList.Add(new Sample(sr, dc, pc));
            }
            sr.Close();

            return Train(s, sampList, DELTA, numToSum);
        }


        private List<ConectionsInfo> CountMinusGradient(object aa)
        {
            CountMinusGradientArgument a = aa as CountMinusGradientArgument;
            int num = a.sampList.Count;
            var gradientList = new List<ConectionsInfo>();
            for (int i = 0; i < mConections.Count; i++)
            {
                gradientList.Add(new ConectionsInfo(mConections[i].Fore, mConections[i].Back));
            }
            for (int i = 0; i < a.numToSum; i++)
            {
                int n = rand.Next() % num;
                a.net.Update(a.sampList[n]);
                var mg = a.net.GetMinusGradient();
                for (int j = 0; j < mConections.Count; j++)
                {
                    gradientList[j] += mg[j];
                }
            }
            return gradientList;
        }

        private void Save()
        {
            var bw = new BinaryWriter(File.Create(fileName));
            bw.Write('F');
            bw.Write('N');
            bw.Write('N');
            bw.Write(' ');
            bw.Write(mConections.Count);
            for (int i = 0; i < mConections.Count; i++)
            {
                mConections[i].Save(bw);
            }
            bw.Close();
        }

        private void LoadFromFile(string fN)
        {
            var br = new BinaryReader(File.Open(fileName, FileMode.Open));

            var head = br.ReadChars(4);
            if (head[0] != 'F' || head[1] != 'N' || head[2] != 'N' || head[3] != ' ')
                throw new Exception("Unable to read the file");
            var lCount = br.ReadInt32();
            for (int i = 0; i < lCount; i++)
            {
                mConections.Add(new ConectionsInfo(br));
            }
            br.Close();
        }
    }

    class CountMinusGradientArgument
    {
        public List<Sample> sampList;
        public int numToSum;
        public NetworkState net;
    }
}
