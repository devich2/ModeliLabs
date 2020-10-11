﻿using System.Collections.Generic;
using System.Linq;

namespace Laba4
{
    public class Create: Element
    {
        public Create(double delay) : base(delay) 
        {
            Tnext = 0.0;
        }
        public Create(double delay, string dist, string name) : base(name, delay)
        {
            Tnext = 0.0;
            Distribution = dist;
        }

        public Create()
        {
        }

        public override void OutAct(Element obj)
        {
            if(NotCheckedElements.Count == NextElements.Count) // Create
            {
                base.OutAct(null);  //quantity
                Tnext = Tcurr + GetDelay();
            }
            if(NotCheckedElements.Any())
            {
                var nextElement = NotCheckedElements[ChooseNextElement()];
                if(nextElement.InAct(this) == ResultMove.Ok)
                {
                    NotCheckedElements = new List<Element>(NextElements);
                }
                else
                {
                    BLockMove(nextElement);
                    OutAct(null);
                }
            }
            else
            {
                NotCheckedElements = new List<Element>(NextElements);
                Failure++;
            }
        } 
    }
}