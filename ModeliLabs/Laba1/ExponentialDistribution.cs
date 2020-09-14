using System;
using System.Collections.Generic;

namespace Laba1
{
    public class ExponentialDistribution: Template
    {
        protected override double FunctionValue(double x)
        {
            double average = this.CountAverage();
            double dispersion = this.CountDispersion();
            return Math.Pow(Math.E, -(Math.Pow(x - average, 2) / (2 * dispersion)))
                   / Math.Sqrt(2 * dispersion * Math.PI);
        }

        public ExponentialDistribution(List<double> data) : base(data)
        {
        }
    }
}