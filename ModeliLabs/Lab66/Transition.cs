using System.Collections.Generic;

namespace Lab66
{
    public class Transition
    {
        public string Name { get; }
        private List<Arc> InputArcs { get; }
        public List<Arc> OutputArcs { get; }
        public bool IsFired { get; private set; }
        public Transition(string name)
        {
            InputArcs = new List<Arc>();
            OutputArcs = new List<Arc>();
            Name = name;
        }
        public void SetInputArc(Arc arc)
        {
            InputArcs.Add(arc);
        }
        public void SetOutputArc(Arc arc)
        {
            OutputArcs.Add(arc);
        }
        public bool CanMove()
        {
            return InputArcs.TrueForAll(x => x.CanGiveMark());
        }
        public List<Condition> GetInputConditions()
        {
            List<Condition> places = new List<Condition>();
            InputArcs.ForEach(x => places.Add(x.Condition));
            return places;
        }
        public void DoInput()
        {
            InputArcs.ForEach(x => x.SetInputMarking());
            IsFired = true;
        }
        public void DoOutput()
        {
            OutputArcs.ForEach(x => x.SetOutputMarking());
            IsFired = false;
        }

    }
}