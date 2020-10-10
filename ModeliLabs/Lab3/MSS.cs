﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab3
{
   public class Mss : Element
    {
        public int Queue { get; private set; }
        public int MaxQueue { get; private set; }
        public double MeanQueue { get; set; }
        public double RAver { get; set; }
        private bool FailWhenNoMove { get; set; }

        public Processor[] Processors;

        private Mss(double delay, int processorsAmount, string name) : base(name, delay)
        {
            FailWhenNoMove = true;
            Queue = 0;
            MaxQueue = int.MaxValue;
            MeanQueue = 0.0;
            RAver = 0.0;
            InitializeProcessors(processorsAmount);
        }
        public Mss(double delay, int processorsAmount, int maxQ, string distribution, string name, bool fail) : this(delay, processorsAmount, name)
        {
            Distribution = distribution;
            MaxQueue = maxQ;
            FailWhenNoMove = fail;
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
        public override void OutAct(Element obj)
        {
            Processor freedElement = (Processor)obj;
            freedElement.Tnext = double.MaxValue;
            if (NotCheckedElements.Count != 0)
            {    
                Element nextElement = NotCheckedElements[ChooseNextElement()];  //get element to move
                ResultMove result = nextElement.InAct(freedElement);
                
                if (result == ResultMove.Ok)
                {
                    NotCheckedElements = new List<Element>(NextElements);
                    base.OutAct(null);
                    freedElement.State = 0;
                    if (Queue > 0)
                    {
                        Queue--;
                        freedElement.State = 1;
                        freedElement.Tnext = freedElement.Tcurr + GetDelay();
                        
                        if (Queue < MaxQueue)
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
                    }
                }
                else
                { 
                    NotCheckedElements.Remove(nextElement);
                    OutAct(freedElement);
                }
            }
            else
            {
                if(freedElement.State == 2)
                {
                    Failure++;
                }
                else if(!FailWhenNoMove)
                {
                    freedElement.State = 2;
                }
                else
                {
                    Failure++;
                }
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
            MeanQueue += Queue * delta;
            RAver += GetState() * delta;
        }
        public int GetState()
        {
            int State = 0;
            for (int i = 0; i < Processors.Length; i++)
            {
                if (State != 2)
                {
                    State += Processors[i].State;
                }
            }
            return State;
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