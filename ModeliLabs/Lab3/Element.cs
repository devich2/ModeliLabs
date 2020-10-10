using System;
using System.Collections.Generic;
using System.Globalization;

namespace Lab3
{
    public class Element
    {
        public string Name { get; set; }
        public double Tnext { get; set; }
        public double DelayMean { get; set; }
        public double DelayDev { get; set; }
        public string Distribution { get; set; }
        public double Tcurr { get; private set; }
        public int State { get; set; }
        public int Failure { get; set; }
        public List<Element> NextElements { get; set; }
        public List<Element> PreviousElements { get; set; }
        public List<Element> NotCheckedElements { get; set; }
        
        
        public int Id { get; set; }
        private int quantity;
        private static int nextId = 0;
        public Element()
        {
            quantity = 0;
            DelayMean = 1.0;
            Distribution = "exp";
            Tnext = GetDelay();
            Tcurr = 0.0;
            State = 0;
            NextElements = new List<Element>();
            PreviousElements = new List<Element>();
            Id = nextId;
            nextId++;
            Name = "element" + Id;
        }
        public Element(double delay) : this()
        {
            Name = "anonymus";
            DelayMean = delay;
            Distribution = "";
        }
        public Element(string name, double delay) : this(delay)
        {
            Name = name;
        }
        public Element(double delayMean, double delayDev) : this(delayMean)
        {
            Distribution = "exp";
            DelayDev = delayDev;
        }
        public double GetDelay()
        {
            double delay = DelayMean;
            switch (Distribution.ToLower())
            {
                case "exp":
                    delay = FunRand.Exp(DelayMean);
                    break;
                case "norm":
                    delay = FunRand.Norm(DelayMean, DelayDev);
                    break;
                case "unif":
                    delay = FunRand.Unif(DelayMean, DelayDev);
                    break;
                case "":
                    delay = DelayMean;
                    break;
            }
            return delay;
        }
        public int GetQuantity()
        {
            return quantity;
        }
        public virtual void SetTCurr(double time)
        {
            Tcurr = time;
        }
        public virtual ResultMove InAct(Element obj) 
        {
            return ResultMove.Ok;
        }
        public virtual void OutAct(Element obj)
        {
            quantity++;
        }
        public virtual void BLockMove(Element obj)
        {
            NotCheckedElements.Remove(obj);
        }

        protected int ChooseNextElement()
        {
            return new Random().Next(0, NotCheckedElements.Count);
        }
        public void PrintResult()
        {
            Console.WriteLine(Name + "  quantity = " + quantity);
        }
        public virtual void PrintInfo()
        {
            string timeStr = (Tnext == double.MaxValue) ? "inf" : Tnext.ToString();
            Console.WriteLine(Name + " state = " + State + " quantity = " + quantity + " tnext = " + timeStr);
        }
        public virtual void DoStatistics(double delta) { }
    }
}