using System;
using System.Collections.Generic;
using System.Linq;

namespace Laba1
{
    public class RandomGenerator
    {
        private static Random _rnd = new Random();
        
        public static List<double> GenerateByExpRule(double lambda, int quantity)
        {
            var list = new List<double>(quantity);
            for(int i = 0; i<quantity; i++)
            {
                list.Add(- Math.Log(_rnd.NextDouble())/lambda);
            }
            return list;
        }
        public static List<double> GenerateByNormalRule(double lambda, double sigma, int quantity)
        {
            var list = new List<double>(quantity);
            for(int i = 0; i<quantity; i++)
            {
                double u = GenerateRandomDoubleList(12).Sum(x=>x) - 6;
                list.Add(sigma*u + lambda);
            }
            return list;
        }
        
        public static List<double> GenerateByUniformRule(double a, double c, int quantity)
        {
            var list = new List<double>(quantity);
            double z = _rnd.NextDouble();
            for(int i = 0; i<quantity; i++)
            {
                z = (a * z)%c;
                list.Add(z/c);
            }
            return list;
        }
        
        private static List<Double> GenerateRandomDoubleList(int count = 0)
        {
            var list = new List<double>();
            for(int i = 0; i<count; i++)
            {
                list.Add(_rnd.NextDouble());
            }
            return list;
        }
    }
}