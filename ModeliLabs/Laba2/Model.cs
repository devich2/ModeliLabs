using System;

namespace Laba2
{
    public class Model
    {
        private int distribution;
        private double tnext;
        private double tcurr;
        private double tnew, tfinish;
        private double delayCreate, delayProcess;
        public double rAver, qAver, lAver, tNet, pFailure, tMean;
        public int numCreate, numProcess, failure;
        private int state, maxqueue, queue;
        private int nextEvent;
        private bool showFinish, showFailure, print;

        public Model(double delay0, double delay1, int distr, bool print)
        {
            delayCreate = delay0;
            delayProcess = delay1;
            tnext = 0.0;
            tcurr = tnext;
            tnew = tcurr; tfinish = double.MaxValue;
            maxqueue = int.MaxValue;
            rAver = 0;
            qAver = 0;
            lAver = 0;
            tNet = 0;
            pFailure = 0;
            tMean = 0;
            distribution = distr;
            this.print = print;
        }
        public Model(double delay0, double delay1, int maxQ, int distr, bool print)
             :this(delay0, delay1, distr, print)
        {
            maxqueue = maxQ;
        }
        public void Simulate(double timeModeling)
        {
            while (tcurr < timeModeling)
            {
                tnext = tnew;
                nextEvent = 0;
                showFinish = false;
                showFailure = false;

                if (tfinish < tnext)
                {
                    tnext = tfinish;
                    nextEvent = 1;
                }

                qAver += (tnext - tcurr) * queue;
                rAver += (tnext - tcurr) * state;
                tcurr = tnext;

                if (nextEvent == 0)
                    Event0();
                else if (nextEvent == 1) Event1();

                if (print)
                    PrintInfo();
            }
            SumUpStatistica();
        }
        
        private void SumUpStatistica()
        {
            tNet = rAver / numProcess;
            rAver /= tcurr;
            lAver = qAver / tcurr;
            qAver /= numProcess;
            pFailure = (double)failure / numCreate;
            tMean = tcurr / numCreate;
            if (print)
                PrintStatistic();
        }
        
        private void PrintStatistic()
        {
            Console.WriteLine(" Поступило на обслуживание: " + numCreate + 
            "\n Было обработано: " + numProcess + 
            "\n Отказов: " + failure);
            Console.WriteLine(" Средняя загрузка: " + rAver); // все рабочее время на фул время
            Console.WriteLine(" Среднее ожидание: " + qAver); // среднее ожидание в очереди
            Console.WriteLine(" Средняя длина очереди: " + lAver); // количество стоячих в очереди в единицу времени
            Console.WriteLine(" Среднее время обслуживания: " + tNet);
            Console.WriteLine(" Вероятность отказа: " + pFailure);
            Console.WriteLine(" Средний интервал поступления: " + tMean);
        }

        private void PrintInfo()
        {
            Console.Write($" Время: {tcurr:f10} " +
                $"\tВ обработке: {state} " +
                $"\t   В очереди: {queue}");
            if (showFinish)
            {
                Console.Write($"\t    Конец обработки текущего: {tfinish:f5}");
            }
            else if (showFailure)
            {
                Console.Write("\t    Отказ");
            }
            Console.WriteLine();
        }

        private void Event0()
        {
            tnew = tcurr + GetDelayOfCreate(distribution);
            numCreate++;
            if (state == 0)
            {
                state = 1;
                tfinish = tcurr + GetDelayOfProcess(distribution);
                showFinish = true;
            }
            else
            {
                if (queue < maxqueue)
                {
                    queue++;
                }
                else
                {
                    failure++;
                    showFailure = true;
                }
            }
        }

        private void Event1()
        {
            state = 0;
            if (queue > 0)
            {
                state = 1;
                tfinish = tcurr + GetDelayOfProcess(distribution);
                queue--;
                showFinish = true;
            }
            else
            {
                tfinish = double.MaxValue;
            }
            numProcess++;
        }
        private double GetDelayOfCreate(int choice)
        {
            double number = choice switch
            {
                1 => FunRand.Exp(delayCreate),
                2 => FunRand.Norm(delayCreate, delayProcess),
                3 => FunRand.Unif(delayCreate, delayProcess),
                _ => 0
            };
            return number;
        }
        private double GetDelayOfProcess(int choice)
        {
            {
                double number = choice switch
                {
                    1 => FunRand.Exp(delayProcess),
                    2 => FunRand.Norm(delayProcess, delayCreate),
                    3 => FunRand.Unif(delayProcess, delayCreate),
                    _ => 0
                };
                return number;
            }
        }

        private class FunRand
        {
            public static double Exp(double timeMean)
            {
                double a = 0;
                Random rand = new Random();
                while (a == 0)
                {
                    a = rand.NextDouble();
                }
                a = -timeMean * Math.Log(a);

                return a;
            }
            public static double Unif(double timeMin, double timeMax)
            {
                double a = 0;
                Random rand = new Random();
                while (a == 0)
                {
                    a = rand.NextDouble();
                }
                a = timeMin + a * (timeMax - timeMin);
                return a;
            }
            public static double Norm(double timeMean, double timeDeviation)
            {
                double a;
                Random rand = new Random();
                a = timeMean + timeDeviation * rand.NextDouble();
                return a;
            }
        }
    }
}