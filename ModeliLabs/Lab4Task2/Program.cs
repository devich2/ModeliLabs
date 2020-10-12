using System;
using System.Collections.Generic;
using ConsoleTables;
using Lab33;

namespace Lab4Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 5, runAmount = 1;
            double time = 100000;
            var table = new ConsoleTable("mean length of queue", "load average", "max load", "max queue", "total time in hospital", "time between arrivings to the lab");

            double meanQueue = 0,
                loadAverage = 0,
                totalTime = 0,
                timeBetween = 0;
            int maxLoad = 0,
                maxQueue = 0;

            for (int j = 0; j < runAmount; j++)
            {
                Create c = new Create(15, "exp", "CREATOR");
                Mss mss1 = new Mss(new double[] { 15, 30, 40 }, new double[] { 0.5, 0.1, 0.4 }, 2, "exp", "MSS1", true);
                Mss mss2 = new Mss(3, 8, 3, "unf", "MSS2", true);
                Mss mss3 = new Mss(2, 5, n, "unf", "MSS3", true);
                Mss mss4 = new Mss(4.5, 3, 1, "erl", "MSS4", true);
                Mss mss5 = new Mss(4, 2, 2, "erl", "MSS5", true);
                Mss mss6 = new Mss(2, 5, n, "unf", "MSS6", true);
                Despose d = new Despose(2.0, "DESPOSER");

                Path helper = new Path();
                helper.SetPathCreateToMss(c, mss1);
                helper.SetPathMssToMss(mss1, mss2);
                helper.SetPathMssToMss(mss1, mss2);
                helper.SetPathMssToMss(mss1, mss3);
                helper.SetPathMssToMss(mss1, mss3);
                helper.SetPathMssToMss(mss1, mss3);
                helper.SetPathMssToDespose(mss2, d);
                helper.SetPathMssToDespose(mss2, d);
                helper.SetPathMssToDespose(mss2, d);
                for (int i = 0; i < n; i++)
                {
                    helper.SetPathMssToMss(mss3, mss4);
                }
                helper.SetPathMssToMss(mss4, mss5);
                helper.SetPathMssToDespose(mss5, d);
                helper.SetPathMssToDespose(mss5, d);
                helper.SetPathMssToMss(mss5, mss6);
                helper.SetPathMssToMss(mss5, mss6);
                for (int i = 0; i < n; i++)
                {
                    helper.SetPathMssToMss(mss6, mss1);
                }
                List<Element> list = new List<Element>
                            {
                                c,
                                mss1,
                                mss2,
                                mss3,
                                mss4,
                                mss5,
                                mss6,
                                d
                            };
                Model model = new Model(list, true);
                model.Simulate(time);

                meanQueue += model.MeanQueue / runAmount;
                loadAverage += model.RAver / runAmount; 
                maxLoad += model.MaxSumStates;
                maxQueue += model.MaxDetectedQueue;
                totalTime += model.TimeForLab / runAmount;
                timeBetween += mss5.DeltaTForLab / mss5.GetQuantity() / runAmount;
                table.AddRow(
                    Math.Round(model.MeanQueue, 10),
                    Math.Round(model.RAver, 10),
                    model.MaxSumStates,
                    model.MaxDetectedQueue,
                    Math.Round(model.TimeForLab, 10),
                    Math.Round(mss5.DeltaTForLab / mss5.GetQuantity(), 10));
            }
            maxLoad = (int)Math.Round(maxLoad / (double)runAmount, 0);
            maxQueue = (int)Math.Round(maxQueue / (double)runAmount, 0);
            table.AddRow(
                Math.Round(meanQueue, 10),
                Math.Round(loadAverage, 10),
                maxLoad,
                maxQueue,
                Math.Round(totalTime, 10),
                Math.Round(timeBetween, 10));

            table.Write(Format.Alternative);
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}