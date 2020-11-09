namespace Lab66
{
    public class Arc
    {
        public Condition Condition { get; private set; }
        public Transition Transition { get; private set; }
        public int Multiplicity { get; private set; }
        public int Type { get; private set; }
        public Arc(Condition condition, Transition transition)
        {
            Condition = condition;
            Transition = transition;
            Condition.SetOutputArc(this);
            Transition.SetInputArc(this);
            Multiplicity = 1;
        }
        public Arc(Transition transition, Condition condition)
        {
            Condition = condition;
            Transition = transition;
            Transition.SetOutputArc(this);
            Condition.SetInputArc(this);
            Multiplicity = 1;
        }
        public Arc(Condition condition, Transition transition, int multiplicity) : this(condition, transition)
        {
            Multiplicity = multiplicity;
        }
        public Arc(Transition transition, Condition condition, int multiplicity) : this(transition, condition)
        {
            this.Multiplicity = multiplicity;
        }
        public bool CanGiveMark()
        {
            return Condition.Marking >= Multiplicity;
        }
        public void SetOutputMarking()
        {
            Condition.Marking += Multiplicity;
        }
        public void SetInputMarking()
        {
            Condition.Marking -= Multiplicity;
        }

    }
}