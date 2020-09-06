using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Laba1
{
    class Program
    {
        private static List<double> Process(List<double> data, Distribution distribution)
        {
            double u = data.Sum(x=>x) / data.Count;
            double dispers = data.Sum(x=>Math.Pow(x - u, 2))/(data.Count-1);
            Console.WriteLine($"Average:{u}, Dispersiya: {dispers}");
            
            switch (distribution)
            {
                case Distribution.Exp:
                    DistributionChecker.CheckExp(data, u, dispers);
                    break;
                case Distribution.Normal:
                    DistributionChecker.CheckNormal(data, u, dispers);
                    break;
                case Distribution.Uniform:
                    DistributionChecker.CheckUniform(data, u, dispers);
                    break;
            }
        }
        
        static void Main(string[] args)
        {
            const double lambda = 2, sigma = 3;
            const int quantity = 10000;
            double a = Math.Pow(5, 13), c = Math.Pow(2, 31);
            var list = new List<double>();
            while(true)
            {
                Console.WriteLine("1.Exp\n2.Normal\n3.Uniform\n4.Exit");
                switch (Console.ReadLine())
                {
                    case"1": 
                        new HistogramBuilder(list = Process(RandomGenerator.GenerateByExpRule(lambda, quantity), Distribution.Exp), 1);
                        break;
                    case"2": 
                        new HistogramBuilder(list = Process(RandomGenerator.GenerateByNormalRule(lambda, sigma, quantity), Distribution.Normal), 2);
                        break;
                    case"3": 
                        new HistogramBuilder(list = Process(RandomGenerator.GenerateByExpRule(lambda, quantity), Distribution.Uniform), 3);
                        break;
                    case"4":
                        Environment.Exit(0);
                        break;
                }
            }
        }
        
    }
}