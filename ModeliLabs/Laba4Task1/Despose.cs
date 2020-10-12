﻿using System;

namespace Laba4
{
    public class Despose : Element
    {
        public double TPrevious { get; set; }
        public double DeltaT { get; set; }
        public Despose(double delay, string name) : base(name, delay)  {
            Tnext = Double.MaxValue;
            TPrevious = 0;
            DeltaT = 0;
        }
        public override ResultMove InAct(Element obj)
        {
            DeltaT += Tcurr - TPrevious;
            TPrevious = Tcurr;
            //Console.WriteLine("1t's time for event in " + this.Name + ", time =    " + this.Tcurr);
            base.OutAct(null);
            return ResultMove.Ok;
        }
    }
}