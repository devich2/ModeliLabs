using System;

namespace Lab3
{
    public class Processor : Element
    {
        public Mss Parent { get; }

        public Processor(Mss parent): base()
        {
            Parent = parent;
            Name = $"PROCESSOR#{Parent.Processors.Length} of {parent.Name}";
        }
    }
}