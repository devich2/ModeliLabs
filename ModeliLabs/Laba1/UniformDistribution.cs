using System.Collections.Generic;

namespace Laba1
{
    public class UniformDistribution: Template
    {
        protected override double FunctionValue(double x)
        {
            return x;
        }

        public UniformDistribution(List<double> data) : base(data)
        {
        }
    }
  
}