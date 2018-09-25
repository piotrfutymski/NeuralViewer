using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NeuralNet;

namespace NeuralViewer.AppLogic
{
    static class SampleListProvider
    {
        public static List<Sample> GetSamplesFromIDX(string fileNameIDX3, string fileNameIDX1, int num)
        {
            var res = new List<Sample>();
            var brIDX3 = new BinaryReader(File.Open(fileNameIDX3, FileMode.Open));
            var brIDX1 = new BinaryReader(File.Open(fileNameIDX1, FileMode.Open));
            brIDX1.ReadInt32();
            brIDX1.ReadInt32();
            brIDX3.ReadInt32();
            brIDX3.ReadInt32();
            brIDX3.ReadInt32();
            brIDX3.ReadInt32();

            for (int i = 0; i < num; i++)
            {
                var d = new double[784];
                var p = new double[10];

                for (int j = 0; j < 784; j++)
                {
                    d[j] = (double)brIDX3.ReadByte() / 255;
                }
                int t = brIDX1.ReadByte();
                p[t] = 1;

                res.Add(new Sample(d, p));
            }
            brIDX3.Close();
            brIDX3.Dispose();
            brIDX1.Close();
            brIDX1.Dispose();
            return res;
        }
    }
}
