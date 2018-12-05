using System;
using System.Numerics;

namespace DSPLib_Test
{
    public class FFT
    {
        public FFT() { }

        private double mFFTScale = 1.0;
        private uint mLogN = 0;
        private uint mN = 0;
        private uint mLengthTotal;
        private uint mLengthHalf;
        private FFTElement[] mX;

        private class FFTElement
        {
            public double re = 0.0;
            public double im = 0.0;
            public FFTElement next;
            public uint revTgt;
        }

        public void Initialize(uint inputDataLength, uint zeroPaddingLength = 0)
        {
            mN = inputDataLength;

            bool foundIt = false;
            for (mLogN = 1; mLogN <= 32; mLogN++)
            {
                double n = Math.Pow(2.0, mLogN);
                if ((inputDataLength + zeroPaddingLength) == n)
                {
                    foundIt = true;
                    break;
                }
            }

            if (foundIt == false)
                throw new ArgumentOutOfRangeException("inputDataLength + zeroPaddingLength não foi uma potência igual a 2! A FFT não pode continuar.");

            mLengthTotal = inputDataLength + zeroPaddingLength;
            mLengthHalf = (mLengthTotal / 2) + 1;

            mFFTScale = Math.Sqrt(2) / mLengthTotal;
            mFFTScale *= mLengthTotal / (double)inputDataLength;

            mX = new FFTElement[mLengthTotal];
            for (uint k = 0; k < (mLengthTotal); k++)
                mX[k] = new FFTElement();

            for (uint k = 0; k < (mLengthTotal) - 1; k++)
                mX[k].next = mX[k + 1];

            for (uint k = 0; k < (mLengthTotal); k++)
                mX[k].revTgt = BitReverse(k, mLogN);
        }

        public Complex[] Execute(double[] timeSeries)
        {
            uint numFlies = mLengthTotal >> 1;
            uint span = mLengthTotal >> 1;
            uint spacing = mLengthTotal;
            uint wIndexStep = 1;

            FFTElement x = mX[0];
            uint k = 0;
            for (uint i = 0; i < mN; i++)
            {
                x.re = timeSeries[k];
                x.im = 0.0;
                x = x.next;
                k++;
            }

            if (mN != mLengthTotal)
            {
                for (uint i = mN; i < mLengthTotal; i++)
                {
                    x.re = 0.0;
                    x.im = 0.0;
                    x = x.next;
                }
            }

            for (uint stage = 0; stage < mLogN; stage++)
            {
                double wAngleInc = wIndexStep * -2.0 * Math.PI / (mLengthTotal);
                double wMulRe = Math.Cos(wAngleInc);
                double wMulIm = Math.Sin(wAngleInc);

                for (uint start = 0; start < (mLengthTotal); start += spacing)
                {
                    FFTElement xTop = mX[start];
                    FFTElement xBot = mX[start + span];

                    double wRe = 1.0;
                    double wIm = 0.0;

                    for (uint flyCount = 0; flyCount < numFlies; ++flyCount)
                    {
                        double xTopRe = xTop.re;
                        double xTopIm = xTop.im;
                        double xBotRe = xBot.re;
                        double xBotIm = xBot.im;

                        xTop.re = xTopRe + xBotRe;
                        xTop.im = xTopIm + xBotIm;

                        xBotRe = xTopRe - xBotRe;
                        xBotIm = xTopIm - xBotIm;
                        xBot.re = xBotRe * wRe - xBotIm * wIm;
                        xBot.im = xBotRe * wIm + xBotIm * wRe;

                        xTop = xTop.next;
                        xBot = xBot.next;

                        double tRe = wRe;
                        wRe = wRe * wMulRe - wIm * wMulIm;
                        wIm = tRe * wMulIm + wIm * wMulRe;
                    }
                }

                numFlies >>= 1;
                span >>= 1;
                spacing >>= 1;
                wIndexStep <<= 1;
            }

            x = mX[0];
            Complex[] unswizzle = new Complex[mLengthTotal];
            while (x != null)
            {
                uint target = x.revTgt;
                unswizzle[target] = new Complex(x.re * mFFTScale, x.im * mFFTScale);
                x = x.next;
            }

            Complex[] result = new Complex[mLengthHalf];
            Array.Copy(unswizzle, result, mLengthHalf);

            result[0] = new Complex(result[0].Real / Math.Sqrt(2), 0.0);
            result[mLengthHalf - 1] = new Complex(result[mLengthHalf - 1].Real / Math.Sqrt(2), 0.0);

            return result;
        }

        private uint BitReverse(uint x, uint numBits)
        {
            uint y = 0;
            for (uint i = 0; i < numBits; i++)
            {
                y <<= 1;
                y |= x & 0x0001;
                x >>= 1;
            }
            return y;
        }

        public double[] FrequencySpan(double samplingFrequencyHz)
        {
            uint points = mLengthHalf;
            double[] result = new double[points];
            double stopValue = samplingFrequencyHz / 2.0;
            double increment = stopValue / (points - 1.0);

            for (int i = 0; i < points; i++)
                result[i] += increment * i;

            return result;
        }
    }
}
