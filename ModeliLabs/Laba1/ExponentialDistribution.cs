using System;
using System.Collections.Generic;

namespace Laba1
{
    public class ExponentialDistribution: Template
    {
        protected override double FunctionValue(double x)
        {
            double average = CountAverage();
            double dispersion = Math.Sqrt(CountDispersion());
            double lambda = (1 / average + 1 / dispersion) / 2;
            return 1 - Math.Pow(Math.E, -x * lambda);
        }

        public ExponentialDistribution(List<double> data) : base(data)
        {
        }
    }
}