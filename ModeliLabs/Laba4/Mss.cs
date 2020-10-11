using System;
using System.Collections.Generic;
using System.Linq;

namespace Laba4
{
    public class Mss: Element
    {
        public int Queue { get; private set; }
        public int MaxQueue { get; private set; }
        public double MeanQueue { get; set; }
        public int SwapQueue { get; set; }
        public double RAver { get; set; }
        private bool BlockingForbidden { get; set; }

        public Processor[] Processors;
        public List<Mss> NeighbourElements { get; set; }

        private Mss(double delay, int processorsAmount, string name) : base(name, delay)
        {
            BlockingForbidden = true;
            Queue = 0;
            MaxQueue = int.MaxValue;
            MeanQueue = 0.0;
            RAver = 0.0;
            NeighbourElements = new List<Mss>();
            InitializeProcessors(processorsAmount);
        }
        public Mss(double delay, int processorsAmount, int maxQ, string distribution, string name, bool fail) : this(delay, processorsAmount, name)
        {
            Distribution = distribution;
            MaxQueue = maxQ;
            BlockingForbidden = fail;
        }
        private void InitializeProcessors(int processorsAmount)
        {
            Processors = new Processor[processorsAmount];
            for (var i = 0; i < Processors.Length; i++)
            {
                Processors[i] = new Processor(this);
            }
        }
        // Check if we can move into this smo
        public override ResultMove InAct(Element obj)
        {
            ResultMove result = ResultMove.Ok;
            Processor freeProcessor = FindFree();
            if (freeProcessor != null)
            {
                freeProcessor.State = 1;
                freeProcessor.Tnext = Tcurr + GetDelay();
            }
            else
            {
                if (Queue < MaxQueue)
                {
                    Queue++;
                }
                else
                {
                    result = ResultMove.Fail;
                }
            }
            return result;
        }
        
        public void CheckSwap()
        {
            foreach (var t in NeighbourElements)
            {
                if(this.Queue - t.Queue >= 2)
                {
                    t.Queue++;
                    this.Queue--;
                    SwapQueue++;
                    break;
                }
            }
        }
        
        private  void BaseOutAct(Processor freedElement)
        {
            freedElement.Tnext = Double.MaxValue;
            freedElement.State = 0;
            if (Queue > 0)
            {
                Queue--;
                freedElement.State = 1;
                freedElement.Tnext = freedElement.Tcurr + GetDelay();
                CheckSwap();
            }   
        }
        
        private void TryUnblockPrevious()
        {
            Mss smoWithBlockedProcessor = (Mss)PreviousElements.FirstOrDefault(x=>
            {
                if(x is Mss mss)
                {
                    return mss.Processors.Any(p=>p.State == 2);
                }
                return false;
            });
                            
            if (smoWithBlockedProcessor != null)
            {
                Processor blockedProc = smoWithBlockedProcessor.Processors.First(x=>x.State == 2);
                Queue++;
                blockedProc.State = 0;
                smoWithBlockedProcessor.NotCheckedElements = new List<Element>(smoWithBlockedProcessor.NextElements);
            }
        }
        public override void OutAct(Element obj)
        {
            base.OutAct(null);
            Processor freedElement = (Processor)obj;
            
            if(BlockingForbidden)
            {
                BaseOutAct(freedElement);
            }
            
            while (NotCheckedElements.Any())
            {
                Element nextElement = NotCheckedElements[ChooseNextElement()];  //get element to move
                ResultMove result = nextElement.InAct(freedElement);
                if (result == ResultMove.Ok)
                {
                    NotCheckedElements = new List<Element>(NextElements);
                    if(!BlockingForbidden)
                    {
                        BaseOutAct(freedElement);
                        if (Queue < MaxQueue)
                        {
                            TryUnblockPrevious();
                        }
                    }
                    return;
                }
                BLockMove(nextElement);
            }
            
            NotCheckedElements = new List<Element>(NextElements);
            
            if(BlockingForbidden)
            {
                Failure++;
            }
            else if (State == 2)
            {
                Failure++;
            }
            else
            {
                State = 2;
            }
        }
        public override void SetTCurr(double time)
        {
            base.SetTCurr(time);
            foreach (var t in Processors)
            {
                t.SetTCurr(time);
            }
        }
        public override void PrintInfo()
        {
            Console.WriteLine($"{Name}\tqueue = {this.Queue}\tquantity = {GetQuantity()}\tfailures = {Failure}");
            foreach (var t in Processors)
            {
                string timeStr = (t.Tnext == double.MaxValue) ? "inf" : t.Tnext.ToString();
                Console.WriteLine(t.Name + " state = " + t.State + " tnext = " + timeStr );
            }
        }
        public override void DoStatistics(double delta)
        {
            MeanQueue += Queue * Math.Abs(delta);
            RAver += GetState() * Math.Abs(delta);
        }
        public int GetState()
        {
            int state = 0;
            for (int i = 0; i < Processors.Length; i++)
            {
                state += (state != 2 ? Processors[i].State : 1);
            }
            return state;
        }

        private Processor FindFree()
        {
            Processor needed = null;
            foreach (var t in Processors)
            {
                if(t.State == 0)
                {
                    needed = t;
                    break;
                }
            }
            return needed;
        }
        public Processor FindFirstNext()
        {
            return Processors.First(x=>x.Tnext == Processors.Min(r=>r.Tnext));
        }
    }
}