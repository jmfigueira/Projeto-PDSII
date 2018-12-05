using System;

namespace DSPLib_Test
{
    public static class Window
    {
        public enum Type
        {
            None,
            Rectangular
        }

        public static class ScaleFactor
        {
            public static double Signal(double[] windowCoefficients)
            {
                double s1 = 0;
                foreach (double coeff in windowCoefficients)
                {
                    s1 += coeff;
                }

                s1 = s1 / windowCoefficients.Length;

                return 1.0 / s1;
            }
        }

        public static double[] Coefficients(Type windowName, uint points)
        {
            double[] winCoeffs = new double[points];
            double N = points;

            switch (windowName)
            {
                case Type.None:
                case Type.Rectangular:
                    for (uint i = 0; i < points; i++)
                        winCoeffs[i] = 1.0;
                    break;
                default:
                    break;
            }

            return winCoeffs;
        }
    }
}
