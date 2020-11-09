using System.Collections.Generic;

namespace Lab66
{
    public class Condition
    {
        public string Name { get; }
        private int marking;
        public int Marking { 
            get {
                return marking;
            }
            set {
                marking = value;
                CheckMinMax(value);
            }
        }
        public List<Arc> InputArcs { get; }
        public List<Arc> OutputArcs { get; }
        public int Min { get; private set; }
        public int Max { get; private set; }
        public double Average { get; set; }
        public Condition(string name)
        {
            Name = name;
            InputArcs = new List<Arc>();
            OutputArcs = new List<Arc>();
            marking = 0;
            Min = marking;
            Max = marking;
        }
        public Condition(int marking, string name) : this(name)
        {
            this.marking = marking;
            Min = marking;
            Max = marking;
        }
        public void SetInputArc(Arc arc)
        {
            InputArcs.Add(arc);
        }
        public void SetOutputArc(Arc arc)
        {
            OutputArcs.Add(arc);
        }
        public void CheckMinMax(int value)
        {
            if(value > Max)
            {
                Max = value;
            }
            else if(value < Min)
            {
                Min = value;
            }
        }

    }
}