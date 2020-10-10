using System.Collections.Generic;

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
       public override void OutAct(Element obj)
       {
           base.OutAct(null);
           Tnext = Tcurr + GetDelay();
           if (NotCheckedElements.Count != 0)
           {
               NotCheckedElements[ChooseNextElement()].InAct(this);
           }
           else
           {
               Failure++;
           }
       } 
   }
}