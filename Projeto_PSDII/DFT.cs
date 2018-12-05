using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DSPLib_Test
{
    public class DFT
    {
        public DFT() { }

        private double mDFTScale;
        private uint mLengthTotal;
        private uint mLengthHalf;

        private double[,] mCosTerm;
        private double[,] mSinTerm;
        private bool mOutOfMemory;

        public void Inicializar(uint inputDataLength, uint zeroPaddingLength = 0, bool forceNoCache = false)
        {
            mLengthTotal = inputDataLength + zeroPaddingLength;
            mLengthHalf = (mLengthTotal / 2) + 1;

            mDFTScale = Math.Sqrt(2) / (inputDataLength + zeroPaddingLength);
            mDFTScale *= (inputDataLength + zeroPaddingLength) / (double)inputDataLength;

            if (forceNoCache == true)
            {
                mOutOfMemory = true;
                return;
            }

            mOutOfMemory = false;
            try
            {
                mCosTerm = new double[mLengthTotal, mLengthTotal];
                mSinTerm = new double[mLengthTotal, mLengthTotal];

                double scaleFactor = 2.0 * Math.PI / mLengthTotal;

                for (int j = 0; j < mLengthHalf; j++)
                {
                    double a = j * scaleFactor;
                    for (int k = 0; k < mLengthTotal; k++)
                    {
                        mCosTerm[j, k] = Math.Cos(a * k) * mDFTScale;
                        mSinTerm[j, k] = Math.Sin(a * k) * mDFTScale;
                    }
                }
            }
            catch (OutOfMemoryException)
            {
                mOutOfMemory = true;
            }
        }

        public Complex[] Executar(double[] timeSeries)
        {
            double[] totalInputData = new double[mLengthTotal];
            Array.Copy(timeSeries, totalInputData, timeSeries.Length);

            Complex[] output;
            if (mOutOfMemory)
                output = Dft(totalInputData);
            else
                output = DftCached(totalInputData);

            return output;
        }

        private Complex[] Dft(double[] timeSeries)
        {
            uint n = mLengthTotal;
            uint m = mLengthHalf;
            double[] re = new double[m];
            double[] im = new double[m];
            Complex[] result = new Complex[m];
            double sf = 2.0 * Math.PI / n;

            Parallel.For(0, m, (j) =>
            {
                double a = j * sf;
                for (uint k = 0; k < n; k++)
                {
                    re[j] += timeSeries[k] * Math.Cos(a * k) * mDFTScale;
                    im[j] -= timeSeries[k] * Math.Sin(a * k) * mDFTScale;
                }

                result[j] = new Complex(re[j], im[j]);
            });

            result[0] = new Complex(result[0].Real / Math.Sqrt(2), 0.0);
            result[mLengthHalf - 1] = new Complex(result[mLengthHalf - 1].Real / Math.Sqrt(2), 0.0);

            return result;
        }

        private Complex[] DftCached(double[] timeSeries)
        {
            uint n = mLengthTotal;
            uint m = mLengthHalf;
            double[] re = new double[m];
            double[] im = new double[m];
            Complex[] result = new Complex[m];

            Parallel.For(0, m, (j) =>
            {
                for (uint k = 0; k < n; k++)
                {
                    re[j] += timeSeries[k] * mCosTerm[j, k];
                    im[j] -= timeSeries[k] * mSinTerm[j, k];
                }
                result[j] = new Complex(re[j], im[j]);
            });

            result[0] = new Complex(result[0].Real / Math.Sqrt(2), 0.0);
            result[mLengthHalf - 1] = new Complex(result[mLengthHalf - 1].Real / Math.Sqrt(2), 0.0);

            return result;
        }

        public double[] FrequencySpan(double samplingFrequencyHz)
        {
            uint points = mLengthHalf;
            double[] result = new double[points];
            double stopValue = samplingFrequencyHz / 2.0;
            double increment = stopValue / (points - 1.0);

            for (uint i = 0; i < points; i++)
                result[i] += increment * i;

            return result;
        }

    }
}
