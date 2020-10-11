﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Laba4
{
    public class Element
    {
         public string Name { get; set; }
        public double Tnext { get; set; }
        private double DelayMean { get; set; }
        private double DelayDev { get; set; }
        protected string Distribution { get; set; }
        public double Tcurr { get; private set; }
        public int State { get; set; }
        public int Failure { get; set; }
        public List<Element> NextElements { get; set; }
        public List<Element> PreviousElements { get; set; }
        public List<Element> NotCheckedElements { get; set; }
        private bool? _isByProbabilityChosen;
        private List<double> ChoiceValues { get; set; }

        public int Id { get; set; }
        private int quantity;
        private static int nextId = 0;
        public Element()
        {
            quantity = 0;
            Distribution = "exp";
            Tnext = Double.MaxValue;
            Tcurr = 0.0;
            State = 0;
            NextElements = new List<Element>();
            PreviousElements = new List<Element>();
            NotCheckedElements = new List<Element>();
            Id = nextId;
            nextId++;
            Name = "element" + Id;
        }
        public Element(double delay) : this()
        {
            Name = "anonymus";
            DelayMean = delay;
            Distribution = "";
            Tnext = GetDelay();
        }
        public Element(string name, double delay) : this(delay)
        {
            Name = name;
        }
        
        public void SetChoiceValue(bool choiceProbability, List<double> values)
        {
            if(values.Count != NextElements.Count)
            {
                throw new Exception("Choice values amount is not equal to next elements amount");
            }
            
            if (_isByProbabilityChosen == true)
            {
                if(values.Sum(x=>x) != 1)
                {
                    throw new Exception("Probability sum is not equal to 1");
                }
            }
            _isByProbabilityChosen = choiceProbability;
            ChoiceValues = values;
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
            NotCheckedElements = NotCheckedElements.Except(new []{obj}).ToList();
        }

        protected int ChooseNextElement()
        {
            int indexResult = -1;
            if (NotCheckedElements.Count == 0)
            {
                return indexResult;
            }
            double sum = 0;
            if (_isByProbabilityChosen.HasValue)
            {
                List<double> values = new List<double>(ChoiceValues);
                for (int i = 0; i < NextElements.Count; i++)
                {
                    if (NotCheckedElements.Find(x => x == NextElements[i]) == null)
                    {
                        values = values.Except(new []{values.Find(x => x == ChoiceValues[i])}).ToList();
                        continue;
                    }
                    sum += ChoiceValues[i];
                }
                if (_isByProbabilityChosen == true)
                {
                    Random rand = new Random();
                    double valueStart, valueFinish, valueRand;
                    valueStart = 0;
                    valueFinish = values[0];
                    valueRand = rand.NextDouble() * sum;

                    for (int i = 0; i < NotCheckedElements.Count - 1; i++)
                    {
                        if (valueRand >= valueStart && valueRand < valueFinish)
                        {
                            return i;
                        }
                        valueStart = valueFinish;
                        valueFinish += values[i + 1];
                    }
                    return NotCheckedElements.Count - 1;
                }

                values.Sort();
                indexResult = NotCheckedElements.FindIndex(x => x == NextElements[ChoiceValues.FindIndex(r => r == values[0])]);
            }
            else
            {
                Random rand = new Random();
                indexResult = rand.Next(0, NotCheckedElements.Count);
            }
            return indexResult;
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