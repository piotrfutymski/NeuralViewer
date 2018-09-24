using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NeuralNet
{
    class ConectionsInfo : ICloneable
    {
        public double[,] WeightMatrix { get; private set; }
        public double[] BiasVector { get; private set; }
        public int Back { get; private set; }
        public int Fore { get; private set; }

        public ConectionsInfo(BinaryReader br)
        {
            Fore = br.ReadInt32();
            Back = br.ReadInt32();
            WeightMatrix = new double[Fore, Back];
            BiasVector = new double[Fore];
            for (int i = 0; i < Fore; i++)
            {
                for (int j = 0; j < Back; j++)
                {
                    WeightMatrix[i, j] = br.ReadDouble();
                }
                BiasVector[i] = br.ReadDouble();
            }
        }

        public ConectionsInfo(int f, int b)
        {
            WeightMatrix = new double[f, b];
            BiasVector = new double[f];
            Back = b;
            Fore = f;
        }

        public ConectionsInfo(int f, int b, double[,] wm, double[] bv)
        {
            WeightMatrix = (double[,])wm.Clone();
            BiasVector = (double[])bv.Clone();
            Back = b;
            Fore = f;
        }

        public void LoadRandom()
        {
            var randGenerator = new Random();

            for (int i = 0; i < Fore; i++)
            {
                for (int j = 0; j < Back; j++)
                {
                    WeightMatrix[i, j] = randGenerator.NextDouble() * 2 - 1;
                }
                BiasVector[i] = randGenerator.NextDouble() * 2 - 1;
            }
        }

        private void SetWeightMatrixON(int i, int j, double d)
        {
            WeightMatrix[i, j] = d;
        }

        private void SetBiasOn(int n, double x)
        {
            BiasVector[n] = x;
        }

        public void Minus()
        {
            for (int i = 0; i < Fore; i++)
            {
                for (int j = 0; j < Back; j++)
                {
                    WeightMatrix[i, j] = -WeightMatrix[i, j];
                }
                BiasVector[i] = -BiasVector[i];
            }
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(Fore);
            bw.Write(Back);
            for (int i = 0; i < Fore; i++)
            {
                for (int j = 0; j < Back; j++)
                {
                    bw.Write(WeightMatrix[i, j]);
                }
                bw.Write(BiasVector[i]);
            }
        }

        public static ConectionsInfo operator +(ConectionsInfo A, ConectionsInfo B)
        {
            if (A.Fore != B.Fore || A.Back != B.Back)
                throw new ArgumentException("not the same type of ConectionInfo");

            for (int i = 0; i < A.Fore; i++)
            {
                for (int j = 0; j < A.Back; j++)
                {
                    A.SetWeightMatrixON(i, j, A.WeightMatrix[i, j] + B.WeightMatrix[i, j]);
                }
                A.SetBiasOn(i, A.BiasVector[i] + B.BiasVector[i]);
            }
            return A;
        }

        public static ConectionsInfo operator -(ConectionsInfo A, ConectionsInfo B)
        {
            if (A.Fore != B.Fore || A.Back != B.Back)
                throw new ArgumentException("not the same type of ConectionInfo");

            for (int i = 0; i < A.Fore; i++)
            {
                for (int j = 0; j < A.Back; j++)
                {
                    A.SetWeightMatrixON(i, j, A.WeightMatrix[i, j] - B.WeightMatrix[i, j]);
                }
                A.SetBiasOn(i, A.BiasVector[i] - B.BiasVector[i]);
            }
            return A;
        }

        public static ConectionsInfo operator *(ConectionsInfo A, double b)
        {
            for (int i = 0; i < A.Fore; i++)
            {
                for (int j = 0; j < A.Back; j++)
                {
                    A.SetWeightMatrixON(i, j, A.WeightMatrix[i, j] * b);
                }
                A.SetBiasOn(i, A.BiasVector[i] * b);
            }
            return A;
        }

        public object Clone()
        {
            return new ConectionsInfo(Fore, Back, WeightMatrix, BiasVector);
        }
    }
}