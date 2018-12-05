using System;

namespace DSPLib_Test
{
    public static class ConvertMagnitude
    {
        public static double[] ToMagnitudeDBV(double[] magnitude)
        {
            uint np = (uint)magnitude.Length;
            double[] magDBV = new double[np];
            for (uint i = 0; i < np; i++)
            {
                double magVal = magnitude[i];
                if (magVal <= 0.0)
                    magVal = double.Epsilon;

                magDBV[i] = 20 * Math.Log10(magVal);
            }

            return magDBV;
        }

    }
}
