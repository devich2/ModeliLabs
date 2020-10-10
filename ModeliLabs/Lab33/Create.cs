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
       }
       public override void OutAct(Element obj)
       {
           base.OutAct(null);
           Tnext = Tcurr + GetDelay();
           if (NotCheckedElements.Count != 0)
           {
               if(NotCheckedElements[ChooseNextElement()].InAct(this) != ResultMove.Ok)
               {
                   Failure++;
               };
           }
           else
           {
               Failure++;
           }
       } 
   }
}