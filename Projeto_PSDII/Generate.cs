using System;

namespace DSPLib_Test
{
    public static class Generate
    {
        public static double[] LinSpace(double startVal, double stopVal, uint points)
        {
            double[] result = new double[points];
            double increment = (stopVal - startVal) / (points - 1.0);

            for (uint i = 0; i < points; i++)
                result[i] = startVal + increment * i;

            return result;
        }

        public static double[] ToneSampling(double amplitudeVrms, double frequencyHz, double samplingFrequencyHz, uint points, double dcV = 0.0, double phaseDeg = 0)
        {
            double ph_r = phaseDeg * Math.PI / 180.0;
            double ampPeak = Math.Sqrt(2) * amplitudeVrms;

            double[] rval = new double[points];
            for (uint i = 0; i < points; i++)
            {
                double time = i / samplingFrequencyHz;
                rval[i] = Math.Sqrt(2) * amplitudeVrms * Math.Sin(2.0 * Math.PI * time * frequencyHz + ph_r) + dcV;
            }
            return rval;
        }
    }
}
