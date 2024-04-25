namespace BlurHelper
{
    public static class MathHelper
    {
        public static double Transform01(double s, double a, double b)
        {
            return (s - a) / (b - a);
        }
        public static double Lerp(double s, double a, double b)
        {
            return a + ((b - a) * s);
        }
        public static double Scale(double s, double a0, double b0, double a1, double b1)
        {
            return Lerp(Transform01(s, a0, b0), a1, b1);
        }
    }
}