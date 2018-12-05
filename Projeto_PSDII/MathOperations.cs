namespace DSPLib_Test
{
    public static class MathOperations
    {
        public static double[] Multiply(double[] a, double[] b)
        {
            double[] result = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
                result[i] = a[i] * b[i];

            return result;
        }

        public static double[] Multiply(double[] a, double b)
        {
            double[] result = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
                result[i] = a[i] * b;

            return result;
        }
    }
}