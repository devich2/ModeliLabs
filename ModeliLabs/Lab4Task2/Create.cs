﻿using System.Collections.Generic;
using System.Linq;

namespace Lab33
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
           base.OutAct(null);
           Tnext = Tcurr + GetDelay();

           while (NotCheckedElements.Any())
           {
               var nextElement = NotCheckedElements[ChooseNextElement()];
               if(nextElement.InAct(this) == ResultMove.Ok)
               {
                   NotCheckedElements = new List<Element>(NextElements);
                   return;
               }
               BLockMove(nextElement);
           }
           NotCheckedElements = new List<Element>(NextElements);
           Failure++;
       } 
   }
}