using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsoleTables;

namespace Lab5
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
           int nStart = 1, nFinish = 1000, interval = 200, runAmount = 5, time = 1000;
            double timeAverageSequence, timeAverageParallel, delayCreate = 2, delayMSS = 0.5;
            var table = new ConsoleTable("size", "timeSequence", "timeParallel");
            List<(int, double, double)> sizeTime = new List<(int, double, double)>();
            sizeTime.Add((0, 0, 0));
            for (int i = nStart; i <= nFinish; i+= interval)
            {
                timeAverageSequence = 0;
                timeAverageParallel = 0;
                for (int j = 0; j < runAmount; j++)     // c => 1
                {
                    Create cSequence = new Create(delayCreate, "exp", "CREATORSequence");
                    List<Mss> mssNSequence = new List<Mss>();
                    Despose dSequence = new Despose(delayCreate, "DESPOSERSequence");
                    Path helper = new Path();
                    List<Element> listSequence = new List<Element>();

                    listSequence.Add(cSequence); // c=> 1 => 2 =>                      // c => (smo1, 2,3)
                    mssNSequence.Add(new Mss(delayMSS, 1, int.MaxValue, "exp", "mss1Sequence", false));
                    listSequence.Add(mssNSequence[0]);
                    helper.SetPathCreateToMss(cSequence, mssNSequence[0]);
                    for (int k = 1; k < i; k++)
                    {
                        mssNSequence.Add(new Mss(0.5, 1, int.MaxValue,"exp", $"mss{k + 1}Sequence", false));
                        helper.SetPathMssToMss(mssNSequence[k - 1], mssNSequence[k]);
                        listSequence.Add(mssNSequence[k]);
                    }
                    helper.SetPathMssToDespose(mssNSequence[mssNSequence.Count - 1], dSequence); // змейка
                    listSequence.Add(dSequence);

                    Model modelSequence = new Model(listSequence, false);
                    modelSequence.Simulate(time);


                    Create cParallel = new Create(delayCreate, "exp", "CREATORParallel");
                    List<Mss> mssNParallel = new List<Mss>();
                    Despose dParallel = new Despose(delayCreate, "DESPOSERParallel");
                    List<Element> listParallel = new List<Element>();

                    listParallel.Add(cParallel);
                    for (int k = 0; k < i; k++)
                    {
                        mssNParallel.Add(new Mss(0.5, 1, int.MaxValue, "exp", $"mss{k + 1}Parallel", false));
                        helper.SetPathCreateToMss(cParallel, mssNParallel[k]);
                        helper.SetPathMssToDespose(mssNParallel[k], dParallel);
                        listParallel.Add(mssNParallel[k]);
                    }
                    listParallel.Add(dParallel);

                    Model modelParallel = new Model(listParallel, false);
                    modelParallel.Simulate(time);

                    timeAverageSequence += modelSequence.TimeWork;
                    timeAverageParallel += modelParallel.TimeWork;
                }
                table.AddRow(i, timeAverageSequence / runAmount, timeAverageParallel / runAmount);
                sizeTime.Add((i, timeAverageSequence / runAmount, timeAverageParallel / runAmount));
            }
            table.Write(Format.Alternative);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Chart(sizeTime, (delayCreate, delayMSS))); // растет количество смо растет время выполенения  // больше операций больше время выполнения  

            Console.WriteLine();
            Console.ReadKey();

        }
    }
}