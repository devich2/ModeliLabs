﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Laba4
{
    public class Model
    {
        public int MaxDetectedQueue { get; set; }
        public int SwapQueue { get; set; }
        public double Tleave { get; set; }
        public double TAver { get; set; }
        public double MeanQueue { get; set; }
        public int MaxSumStates { get; set; }
        public double RAver { get; set; }
        public int Failures { get; set; }
        public double PFailure { get; set; }
        public readonly List<Element> _list;
        private readonly bool _showInfo;
        double _tnext, _tcurr;
        int _eventIndex;
        Processor _nextProcessor;
        public Model(List<Element> elements, bool showInfo)
        {
            _list = elements;
            _tnext = 0.0;
            _eventIndex = 0;
            _tcurr = _tnext;
            MaxDetectedQueue = 0;
            MaxSumStates = 0;
            PFailure = 0;
            RAver = 0;
            TAver = 0;
            Tleave = 0;
            Failures = 0;
            SwapQueue = 0;
            _showInfo = showInfo;
        }
        public void Simulate(double time)
        {
            InitNotChecked();

            while (_tcurr < time)
            {
                _tnext = double.MaxValue;
                foreach (Element e in _list)
                {
                    if(e is Mss mss)
                    {
                        Processor foundFirst = mss.FindFirstNext();
                        if (foundFirst.Tnext < _tnext)
                        {
                            _tnext = foundFirst.Tnext;
                            _nextProcessor = foundFirst;
                            _eventIndex = _list.IndexOf(e);
                        }
                    }
                    else if (e.Tnext < _tnext)
                    {
                        _tnext = e.Tnext;
                        _nextProcessor = null;
                        _eventIndex = _list.IndexOf(e);
                    }
                }
                if (_showInfo)
                {
                    if (_nextProcessor != null)
                    {
                        Console.WriteLine($"\nIt's time for event in {_list[_eventIndex].Name} -> " +
                                          $"{_nextProcessor?.Name}, time = {_tnext}");
                    }
                    else
                    {
                        Console.WriteLine($"\nIt's time for event in {_list[_eventIndex].Name}, time = {_tnext}");
                    }
                }
                PickUpStatisticInfo();
                _tcurr = _tnext;
                foreach (Element e in _list)
                {
                    e.SetTCurr(_tcurr);
                }
               
                _list[_eventIndex].OutAct(_nextProcessor);
                
                foreach (Element e in _list)
                {
                    if (e is Mss mss)
                    {
                        Processor nextPr = mss.FindFirstNext();
                        if (nextPr?.Tnext == _tcurr && (_nextProcessor == null || nextPr?.Parent.Id != _nextProcessor.Parent.Id || nextPr.Id != _nextProcessor.Id))
                        {
                            mss.OutAct(nextPr);
                        }
                    }
                    else if (e.Tnext == _tcurr)
                    {
                        e.OutAct(null);
                    }
                }
                if (_showInfo)
                {
                    PrintInfo();
                }
            }
            
            // In the end
            DoStatistics();
            if (_showInfo)
            {
                PrintResults();
                PrintTotalResult();
            }
        }
        
        // Every iteration print info of every element
        private void PrintInfo()
        {
            foreach (Element e in _list)
            {
                e.PrintInfo();
            }
        }
        // For every smo print results in the end
        public void PrintResults()
        {
            Console.WriteLine("\n-------------RESULTS-------------");
            foreach (Element e in _list)
            {
                e.PrintResult();
                if (e is Mss m)
                {
                    Console.WriteLine("mean length of queue = " + m.MeanQueue +
                                      "\nmax length of queue = " + m.MaxQueue +
                                      "\nfailure probability = " + m.Failure / (double)(m.GetQuantity() + m.Failure + m.Queue + m.GetState()) +
                                      "\nload average = " + m.RAver);
                }
            }
        }
        // Total in the end
        public void PrintTotalResult()
        {
            Console.WriteLine("\n-------------TOTAL RESULT-------------");
            Console.WriteLine("mean length of queue = " + MeanQueue +
                "\nmax length of queue =  = " + MaxDetectedQueue +
                "\nfailure probability = " + PFailure +
                "\nload average = " + RAver +
                "\nmax load = " + MaxSumStates+
                "\nfailures = " + Failures);
        }
        
        // Every iteration
        private void PickUpStatisticInfo()
        {
            int states = 0;
            foreach (Element e in _list)
            {
                e.DoStatistics(_tnext - _tcurr);
                if (e is Mss model)
                {
                    if (model.Queue > MaxDetectedQueue)
                    {
                        MaxDetectedQueue = model.Queue;
                    }
                    states += model.GetState();
                }
            }
            if (states > MaxSumStates)
            {
                MaxSumStates = states;
            }
        }

        private void DoStatistics()
        {
            var smos = _list.OfType<Mss>().ToList();

            foreach (var smo in smos)
            {
                smo.RAver/=_tcurr;
                smo.MeanQueue/=_tcurr;
            }
            Failures = _list.Sum(x=>x.Failure);
            PFailure = Failures/(double)_list.First().GetQuantity();
        }

        private void InitNotChecked()
        {
            foreach (var t in _list)
            {
                t.NotCheckedElements = t.NextElements;
            }
        }
        public double GetFinishTime()
        {
            return _tcurr;
        }
    }
}