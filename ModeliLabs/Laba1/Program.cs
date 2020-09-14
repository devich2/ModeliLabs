using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Laba1
{
    class Program
    {
        static void Main(string[] args)
        {
            const double lambda = 5, sigma = 5;
            const int quantity = 10000;
            double a = Math.Pow(5, 13), c = Math.Pow(2, 31);
            var list = new List<double>();
            while(true)
            {
                Console.WriteLine("1.Exp\n2.Normal\n3.Uniform\n4.Exit");
                switch (Console.ReadLine())
                {
                    case"1": 
                        new HistogramBuilder(new ExponentialDistribution(RandomGenerator.GenerateByExpRule(lambda, quantity)).Data, 1);
                        break;
                    case"2": 
                        new HistogramBuilder(new NormalDistribution(RandomGenerator.GenerateByNormalRule(lambda, sigma, 1000)).Data, 2);
                        break;
                    case"3": 
                        new HistogramBuilder(new UniformDistribution(RandomGenerator.GenerateByUniformRule(a, c, 100)).Data, 3);
                        break;
                    case"4":
                        Environment.Exit(0);
                        break;
                }
            }
        }
        
    }
}