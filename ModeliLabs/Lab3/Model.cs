using System;
using System.Collections.Generic;

namespace Lab3
{
    public class Model
    {
        private int MaxDetectedQueue { get; set; }
        private int MaxSumStates { get; set; }
        private int Failures { get; set; }
        public double PFailure { get; set; }
        public double MeanQueue { get; set; }
        public double RAver { get; set; }
        private List<Element> list;
        private readonly bool showInfo;
        double tnext, tcurr;
        int _event;
        public Model(List<Element> elements, bool showInfo)
        {
            list = elements;
            tnext = 0.0;
            _event = 0;
            tcurr = tnext;
            MaxDetectedQueue = 0;
            MaxSumStates = 0;
            PFailure = 0;
            RAver = 0;
            Failures = 0;
            this.showInfo = showInfo;
        }
        public void Simulate(double time)
        {
            while (tcurr < time)
            {
                tnext = double.MaxValue; 
                
                // Closest event time
                foreach (Element e in list)
                {
                    if (e.TNext < tnext) 
                    {
                        tnext = e.TNext;
                        _event = e.Id - list[0].Id;
                    }
                }
                
                if (showInfo)
                {
                    Console.WriteLine("\nIt's time for event in " + list[_event].Name + ", time =   " + tnext);
                }
                PickUpStatisticInfo();
                tcurr = tnext;
                
                // Inform about time lap
                foreach (Element e in list)
                {
                    e.TCurr = tcurr;
                }
                // 
                list[_event].OutAct();
                foreach (Element e in list)
                {
                    if (e.TNext == tcurr && tcurr != 0)
                    {
                         e.OutAct();
                    }
                }
                if (showInfo)
                {
                    PrintInfo();
                }
            }
            DoStatistics();
            if (showInfo)
            {
                PrintResults();
                PrintTotalResult();
            }
        }

        private void PrintInfo()
        {
            foreach (Element e in list)
            {
                e.PrintInfo();
            }
        }

        private void PrintResults()
        {
            Console.WriteLine("\n-------------RESULTS-------------");
            foreach (Element e in list)
            {
                e.PrintResult();
                if (e is Process p)
                {
                    Console.WriteLine("mean length of queue = " + p.MeanQueue / tcurr +
                                      "\nmax length of queue =  = " + p.MaxQueue +
                                      "\nfailure probability = " + p.GetFailure() / (double) p.GetQuantity() +
                                      "\nload average = " + p.RAver / tcurr);
                }
            }
        }

        private void PrintTotalResult()
        {
            Console.WriteLine("\n-------------TOTAL RESULT-------------");
            Console.WriteLine("mean length of queue = " + MeanQueue +
                "\nmax length of queue =  = " + MaxDetectedQueue +
                "\nfailure probability = " + PFailure +
                "\nload average = " + RAver +
                "\nmax load = " + MaxSumStates);
        }

        private void PickUpStatisticInfo()
        {
            int states = 0;
            foreach (Element e in list)
            {
                e.DoStatistics(tnext - tcurr);
                if (e is Process process)
                {
                    if (process.Queue > MaxDetectedQueue)
                    {
                        MaxDetectedQueue = process.Queue;
                    }
                    states += process.State;
                }
            }
            if (states > MaxSumStates)
            {
                MaxSumStates = states;
            }
        }

        private void DoStatistics()
        {
            int countProcess = 0;
            foreach (Element e in list)
            {
                if (e is Process process)
                {
                    countProcess++;
                    MeanQueue += process.MeanQueue / tcurr;
                    PFailure += process.GetFailure() / (double)process.GetQuantity();
                    RAver += process.RAver / tcurr;
                }
            }
            MeanQueue /= countProcess;
            PFailure /= countProcess;
            RAver /= countProcess;
        }
    }
}