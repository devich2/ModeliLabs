using System;
using System.Collections.Generic;
using System.Linq;
using Lab33;

namespace Lab4Task2
{
   public class Mss : Element
    {
        public int Queue { get; private set; }
        public int MaxQueue { get; private set; }
        public double MeanQueue { get; set; }
        public double RAver { get; set; }
        private bool FailWhenNoMove { get; set; }

        public Processor[] Processors;
        public int[] Queues { get; private set; }
        public double[] Delays { get; private set; }
        public double[] Frequency { get; private set; }
        public double DeltaTForLab { get; set; }
        private double PreviousForLab { get; set; }
        private readonly bool _isUnique;
        private int _nextType;
        private bool isFirstInTheProcess;
        private Mss(double delay, int processorsAmount, string name) : base(name, delay)
        {
            FailWhenNoMove = true;
            Queue = 0;
            MaxQueue = int.MaxValue;
            MeanQueue = 0.0;
            _isUnique = false;
            RAver = 0.0;
            InitializeProcessors(processorsAmount);
        }
        public Mss(double delay, int processorsAmount, string distribution, string name, bool fail) : this(delay, processorsAmount, name)
        {
            Distribution = distribution;
            FailWhenNoMove = fail;
        }
        public Mss(double[] delays, double[] frequency, int processorsAmount, string distribution, string name, bool fail) : this(delays[0], processorsAmount, distribution, name, fail)
        {
            _isUnique = true;
            Delays = delays;
            Queues = new int[Delays.Length];
            double sum = 0;
            for(int i = 0; i < frequency.Length; i++)
            {
                sum += frequency[i];
            }
            if(sum != 1)
            {
                throw new Exception("Frequency sum is not equal to 1");
            }
            if(frequency.Length != delays.Length)
            {
                throw new Exception("Frequencies amount is not equal to delays amount");
            }
            Frequency = frequency;
        }
        public Mss(double delayMean, double delayDev, int processorsAmount, string distribution, string name, bool fail) : this(delayMean, processorsAmount, distribution, name, fail)
        {
            DelayDev = delayDev;
        }

        public Mss()
        {
        }

        private void InitializeProcessors(int processorsAmount)
        {
            Processors = new Processor[processorsAmount];
            for (var i = 0; i < Processors.Length; i++)
            {
                Processors[i] = new Processor(this);
            }
        }
        // Check if we can move into this smo
        public override void InAct(Element obj)
        {
            if(this.Name.ToLower() == "mss5")
            {
                DeltaTForLab += Tcurr - PreviousForLab;
                PreviousForLab = Tcurr;
            }
            Processor freeProcessor = FindFree();

            if (!_isUnique)
            {
                if (freeProcessor != null)
                {
                    freeProcessor.State = 1;
                    freeProcessor.Tnext = Tcurr + GetDelay();
                }
                else
                {
                    Queue++;
                }
            }
            else
            {
                int i = 0;
                if (obj.GetType() != new Processor().GetType() || ((Processor)obj).Parent.Name.ToLower() != "mss6")
                {
                    Random rand = new Random();
                    double valueStart = 0, valueFinish = 0, valueRand;
                    valueRand = rand.NextDouble();
                    for (; i < Frequency.Length; i++)
                    {
                        valueFinish += Frequency[i];
                        if (valueRand >= valueStart && valueRand < valueFinish)
                        {
                            break;
                        }
                        valueStart = valueFinish;
                    }
                }
                if (freeProcessor != null)
                {
                    freeProcessor.State = 1;
                    DelayMean = Delays[i];
                    freeProcessor.Tnext = Tcurr + GetDelay();
                    if (i == 0)
                    {
                        isFirstInTheProcess = true;
                    }
                }
                else
                {
                    SetQueue(GetQueue(i+1) + 1, i+1);
                }
            }
        }
        public override void OutAct(Element obj)
        {
            Processor freedProcessor = (Processor)obj;
            freedProcessor.Tnext = double.MaxValue;
            freedProcessor.State = 0;
            base.OutAct(null);

            int indexToPass = 0;
            if (this.Name.ToLower() != "mss1")
            {
                Random rand = new Random();
                indexToPass = rand.Next(0, NextElements.Count);
            }
            else
            {
                if (isFirstInTheProcess)
                {
                    indexToPass = NextElements.FindIndex(x => x.Name.ToLower() == "mss2");
                    isFirstInTheProcess = false;
                }
                else
                {
                    indexToPass = NextElements.FindIndex(x => x.Name.ToLower() == "mss3");
                }
            }
            NextElements[indexToPass].InAct(freedProcessor);

            if (GetQueue() > 0)
            {
                if (!_isUnique)
                {
                    Queue--;
                    freedProcessor.State = 1;
                    freedProcessor.Tnext = Tcurr + GetDelay();
                }
                else
                {
                    if (Queues[0] > 0)
                    {
                        freedProcessor.State = 1;
                        DelayMean = Delays[0];
                        freedProcessor.Tnext = Tcurr + GetDelay();
                        isFirstInTheProcess = true;
                        SetQueue(GetQueue(1) - 1, 1);
                    }
                    else
                    {
                        List<int> notEmptyQueues = new List<int>();
                        for(int i = 0; i < Queues.Length; i++)
                        {
                            if(Queues[i] > 0)
                            {
                                notEmptyQueues.Add(i);
                            }
                        }
                        Random rand = new Random();
                        int index = rand.Next(0, notEmptyQueues.Count);
                        index = notEmptyQueues[index];
                        freedProcessor.State = 1;
                        DelayMean = Delays[index];
                        freedProcessor.Tnext = Tcurr + GetDelay();
                        isFirstInTheProcess = false;
                        SetQueue(GetQueue(index + 1) - 1, index + 1);
                    }
                }
            }
        }
        public override void SetTCurr(double time)
        {
            base.SetTCurr(time);
            foreach (var t in Processors)
            {
                t.SetTCurr(time);
            }
        }
        public override void PrintInfo()
        {
            Console.WriteLine($"{Name}\tqueue = {this.Queue}\tquantity = {GetQuantity()}\tfailures = {Failure}");
            foreach (var t in Processors)
            {
                string timeStr = (t.Tnext == double.MaxValue) ? "inf" : t.Tnext.ToString();
                Console.WriteLine(t.Name + " state = " + t.State + " tnext = " + timeStr );
            }
        }
        public override void DoStatistics(double delta)
        {
            MeanQueue += GetQueue() * Math.Abs(delta);
            RAver += GetState() * Math.Abs(delta);
        }
        public int GetState()
        {
            int state = 0;
            for (int i = 0; i < Processors.Length; i++)
            {
                state += (state != 2 ? Processors[i].State : 1);
            }
            return state;
        }

        public int GetQueue()
        {
            return _isUnique ? Queues.Sum() : Queue;
        }

        private int GetQueue(int type)
        {
            return Queues[type-1];
        }

        private void SetQueue(int queue, int type)
        {
            Queues[type - 1] = queue;
        }

        private Processor FindFree()
        {
            return Processors.FirstOrDefault(x=>x.State == 0);
        }
        public Processor FindFirstNext()
        {
            return Processors.First(x=>x.Tnext == Processors.Min(r=>r.Tnext));
        }
    }
}