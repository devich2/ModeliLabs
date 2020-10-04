using System;

namespace Lab3
{
    public class Create: Element
    {
        public Create(double delay, string name, string distribution) : base(delay) { Name = name; Distribution = distribution; }

        public override void OutAct()
        {
            base.OutAct();
            // Register time
            TNext = TCurr + GetDelay();

            Random rand = new Random();
            // Let in
            NextElements[rand.Next(0, NextElements.Count)].InAct();
        }
    }
}