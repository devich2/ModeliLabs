using System;

namespace Laba4
{
    public class Despose : Element
    {
        public Despose(double delay, string name) : base(name, delay)  { Tnext = Double.MaxValue; }
        public override ResultMove InAct(Element obj)
        {
            //Console.WriteLine("1t's time for event in " + this.Name + ", time =    " + this.Tcurr);
            base.OutAct(null);
            //Tnext = Tcurr + GetDelay();
            return ResultMove.Ok;
        }
    }
}