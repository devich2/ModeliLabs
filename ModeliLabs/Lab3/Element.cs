using System;
using System.Collections.Generic;
using System.Globalization;

namespace Lab3
{
    public class Element
    {
        public string Name { get; set; }
        public double TNext { get; set; }
        private double DelayMean { get; }
        private double DelayDev { get; set; }
        protected string Distribution { get; set; }
        public double TCurr { get; set; }
        public int State { get; set; }
        public List<Element> NextElements { get; }
        public int Id { get; }
        private int quantity;
        private static int nextId;

        private Element()
        {
            quantity = 0;
            TNext = 0.0;
            DelayMean = 1.0;
            Distribution = "exp";
            TCurr = TNext;
            State = 0;
            NextElements = new List<Element>();
            Id = nextId;
            nextId++;
            Name = "element" + Id;
        }

        protected Element(double delay) : this()
        {
            Name = "anonymus";
            DelayMean = delay;
            Distribution = "";
        }

        protected Element(double delay, string distribution, string nameOfElement) : this()
        {
            Name = nameOfElement;
            DelayMean = delay;
            Distribution = distribution;
        }

        protected double GetDelay()
        {
            double delay = Distribution.ToLower() switch
            {
                "exp" => FunRand.Exp(DelayMean),
                "norm" => FunRand.Norm(DelayMean, DelayDev),
                "unif" => FunRand.Unif(DelayMean, DelayDev),
                "" => DelayMean,
                _ => DelayMean
            };
            return delay;
        }
        public int GetQuantity()
        {
            return quantity;
        }
        public virtual void InAct() { }
        public virtual void OutAct()
        {
            quantity++;
        }
        public void PrintResult()
        {
            Console.WriteLine(Name + "  quantity = " + quantity);
        }
        public virtual void PrintInfo()
        {
            string timeStr = (TNext == double.MaxValue) ? "inf" : TNext.ToString(CultureInfo.CurrentCulture);
            Console.WriteLine(Name + " state = " + State + " quantity = " + quantity + " tnext = " + timeStr);
        }
        public virtual void DoStatistics(double delta) { }
    }
}