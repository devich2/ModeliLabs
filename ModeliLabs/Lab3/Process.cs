using System;

namespace Lab3
{
    public class Process: Element
    {
        public int Queue { get; set; }
        public int MaxQueue { get; }
        private int _failure;
        public double MeanQueue { get; set; }
        public double RAver { get; set; }

        public Process(double delay, string distribution, string name, int maxQueue ) : base(delay, distribution, name)
        {
            TNext = double.MaxValue;
            Queue = 0;
            MaxQueue = maxQueue;
            MeanQueue = 0.0;
            RAver = 0.0;
        }

        public override void InAct()
        {
            //If free, lap time
            if (State == 0)
            {
                State = 1;
                TNext = TCurr + GetDelay();
            }
            else
            {
                if (Queue < MaxQueue)
                {
                    Queue++;
                }
                else
                {
                   _failure ++;
                }
            }
        }
        public override void OutAct()
        {
            base.OutAct();
            TNext = double.MaxValue;
            State = 0;
            Random rand = new Random();
            // Move to next element
            int index = rand.Next(0, NextElements.Count);
            NextElements[index].InAct();
            
            // Process current queue
            if (Queue > 0)
            {
                Queue--;
                State = 1;
                TNext = TCurr + GetDelay();
            }
        }
        public int GetFailure()
        {
            return _failure;
        }
        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"queue = {Queue}");
        }
        public override void DoStatistics(double delta)
        {
            MeanQueue += Queue * delta;
            RAver += State * delta;
        }
    }
}