using System;

namespace Lab3
{
    public static class FunRand
    {
        public static double Exp(double timeMean)
        {
            double a = 0;
            Random rand = new Random();
            while (a == 0)
            {
                a = rand.NextDouble();
            }
            a = -timeMean * Math.Log(a);
            return a;
        }
        public static double Unif(double timeMin, double timeMax)
        {
            double a = 0;
            Random rand = new Random();
            
            while (a == 0)
            {
                a = rand.NextDouble();
            }
            a = timeMin + a * (timeMax - timeMin);
            return a;
        }
        public static double Norm(double timeMean, double timeDeviation)
        {
            double a;
            Random rand = new Random();
            a = timeMean + timeDeviation * (rand.NextDouble()*2 - 1);
            return a;
        }
    }
}