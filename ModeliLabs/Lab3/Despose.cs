using System;

namespace Lab3
{
    public class Despose : Element
    {
        public Despose(double delay, string name, string distribution) : base(name, delay)  { Distribution = distribution; Tnext = Double.MaxValue; }
        public override ResultMove InAct(Element obj)
        {
            //Console.WriteLine("1t's time for event in " + this.Name + ", time =    " + this.Tcurr);
            base.OutAct(null);
            return ResultMove.Ok;
        }
    }
}