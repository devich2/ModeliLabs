using System;
using System.Collections.Generic;
using System.Linq;

namespace Laba1
{
    public abstract class Template
    {
        public readonly List<double> Data;
        protected double? Average { get; set; }
        protected double? Dispersion { get; set; }
        protected Template(List<double> data)
        {
            Data = data;
            LogDetails();
        }

        #region Adds

        private void LogDetails()
        {
            Console.WriteLine($"Average:{CountAverage()}, Dispersiya: {CountDispersion()}, Xi^2: {XiSquare()}");
        }

        protected double CountAverage()
        {
            return Data.Sum(x=>x)/Data.Count;
        }

        protected double CountDispersion()
        {
            int size = Data.Count;
            double average = Average ?? CountAverage();
            double sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += Math.Pow(Data[i] - average, 2);
            }
            Dispersion = sum / (size - 1);
            return Dispersion.Value;
        }

        #endregion

        private double XiSquare()
        {
            int theory, fact, intervalCounter = 0;
            int intervalAmount = IntervalAmount();
            double xPrevious, xCurrent;
            double xi = 0;
            double min = Data.Min();
            double max = Data.Max();
            double intervalSize = Math.Abs(min - max) / intervalAmount;
            bool merge = false;
            Data.Sort();;

            xPrevious = min;
            xCurrent = min + intervalSize;
            for (int i = 1; i <= intervalAmount; i++)
            {
                fact = FactHitting(xCurrent, xPrevious);
                theory = (int)(TheoreticalHitting(xCurrent, xPrevious) * Data.Count);
                if (fact > 5 && theory > 5)
                {
                    if (merge)
                    {
                        xPrevious = xCurrent;
                        merge = false;
                    }
                    else
                    {
                        xPrevious += intervalSize;
                    }
                    xCurrent += intervalSize;
                    // Console.WriteLine(xPrevious + " " + xCurrent + " " + theory + " " + fact);
                }
                else
                {
                    xCurrent += intervalSize;
                    merge = true;
                    continue;
                }
                intervalCounter++;
                xi += Math.Pow(fact - theory, 2) / theory;
            }
            Console.WriteLine($"\tInterval amount: {intervalCounter}");
            return xi;
        }


        private int FactHitting(double xCurrent, double xPrevious)
        {
            int counter = 0;
            for (int i = 0; i < Data.Count; i++)
            {
                if (Data[i] > xCurrent)
                {
                    break;
                }
                if (Data[i] > xPrevious)
                {
                    counter++;
                }
            }
            return counter;
        }
        protected virtual double TheoreticalHitting(double xCurrent, double xPrevious)
        {
            return FunctionValue(xCurrent) - FunctionValue(xPrevious);
        }
        private int IntervalAmount()
        {
            int size = Data.Count;
            int amount = 0;
            if (size < 100)
            {
                throw new Exception("Too small sample.");
            }
            do
            {
                size /= 10;
                amount++;
            } while (size >= 10);
            return amount * 10;
        }
        protected abstract double FunctionValue(double x);
    }
}