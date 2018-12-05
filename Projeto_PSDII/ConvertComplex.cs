using System.Numerics;

namespace DSPLib_Test
{
    public static class ConvertComplex
    {
        public static double[] ToMagnitude(Complex[] rawFFT)
        {
            int np = rawFFT.Length;
            double[] mag = new double[np];
            for (int i = 0; i < np; i++)
            {
                mag[i] = rawFFT[i].Magnitude;
            }

            return mag;
        }
    }
}
