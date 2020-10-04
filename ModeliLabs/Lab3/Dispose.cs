namespace Lab3
{
    public class Dispose: Element
    {
        public Dispose(double delay, string name, string distribution) : base(delay, distribution, name) 
        {
            TNext = double.MaxValue;
        }

        public override void InAct()
        {
            base.OutAct();
        }

    }
}