using System;
using System.Collections.Generic;

namespace Laba1
{
    public class NormalDistribution: Template
    {
        protected override double TheoreticalHitting(double xCurrent, double xPrevious)
        {
            double intervalSize, x, S = 0;
            double intervalAmount = 100;
            intervalSize = (xCurrent - xPrevious) / intervalAmount;
            x = xPrevious + intervalSize;
            while (x < xCurrent)
            {
                S += 4 * FunctionValue(x);
                x += intervalSize;
                if (x >= xCurrent) break;
                S += 2 * FunctionValue(x);
                x += intervalSize;
            }
            S = (intervalSize / 3) * (S + FunctionValue(xPrevious) + FunctionValue(xCurrent));
            return S;
        }
        protected override double FunctionValue(double x)
        {
            double average = this.CountAverage();
            double dispersion = this.CountDispersion();
            return Math.Pow(Math.E, -(Math.Pow(x - average, 2) / (2 * dispersion)))
                   / Math.Sqrt(2 * dispersion * Math.PI);
        }

        public NormalDistribution(List<double> data) : base(data)
        {
        }
    }
}