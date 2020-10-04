using System;
using System.Collections.Generic;
using ConsoleTables;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Choose:\n1. Default launch\n2. Verification\n3. Exit\n\nYour choice: ");
                switch (Console.ReadKey().KeyChar)
                {
                    #region 1

                    case '1':
                    {
                        Console.Clear();
                        double delayCreate, time, delayProcess;
                        int maxQ;
                        string distribution;
                        try
                        {
                            Console.Write("Enter time: ");
                            time = Convert.ToDouble(Console.ReadLine());
                            Console.Write("Enter delayCreate: ");
                            delayCreate = Convert.ToDouble(Console.ReadLine());
                            Console.Write("Enter delayProcess: ");
                            delayProcess = Convert.ToDouble(Console.ReadLine());
                            Console.Write("Enter maxQ: ");
                            maxQ = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter distribution: ");
                            distribution = Console.ReadLine();

                            Create c = new Create(delayCreate, "CREATOR", distribution);
                            Process p1 = new Process(delayProcess, distribution, "PROCESSOR1", maxQ);
                            Process p2 = new Process(delayProcess, distribution, "PROCESSOR2", maxQ);
                            Process p3 = new Process(delayProcess, distribution, "PROCESSOR3", maxQ);
                            Process p4 = new Process(delayProcess, distribution, "PROCESSOR4", maxQ);
                            Dispose d1 = new Dispose(2.0, "DESPOSER1", distribution);
                            Dispose d2 = new Dispose(2.0, "DESPOSER2", distribution);

                            c.NextElements.AddRange(new Element[]
                            {
                                p1, p4, d2
                            });

                            p1.NextElements.AddRange(new Element[]
                            {
                                p2, p3, p4, d2
                            });

                            p2.NextElements.AddRange(new Element[]
                            {
                                p1, p3, d1
                            });

                            p3.NextElements.AddRange(new Element[]
                            {
                                p1, p2, p4,
                            });

                            p4.NextElements.AddRange(new Element[]
                            {
                                p1, p3, d2
                            });

                            List<Element> list = new List<Element>
                            {
                                c,
                                p1,
                                p2,
                                p3,
                                p4,
                                d1,
                                d2
                            };
                            Model model = new Model(list, true);
                            model.Simulate(time);
                            Console.ReadKey();
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("\nThere is mistake. Try again");
                            Console.ReadKey();
                            continue;
                        }

                        break;
                    }

                    #endregion

                    #region 2

                    case '2':
                    {
                        Console.Clear();
                        int choice;
                        try
                        {
                            double time = 100, delayCreate, delayProcess;
                            int maxQ;
                            string distribution;
                            Console.Write("Enter launch amount: ");
                            choice = Convert.ToInt32(Console.ReadLine());
                            if (choice <= 0)
                            {
                                throw new Exception();
                            }

                            Console.WriteLine(
                                "ктс - количество требований пришедших в сеть в модели\n" +
                                "одо    - ограничение на длину очереди\n" +
                                "сзп - средняя загрузка приборов в модели Х \n" +
                                "вo  - вероятность отказов в модели Х\n" +
                                "сдо - средняя длина очереди в модели Х");
                            var table = new ConsoleTable("ктс", "одо", "сзп", "вo", "сдо");
                            
                            delayCreate = 1;
                            delayProcess = choice + 1;
                            maxQ = 10;

                            distribution = "exp";
                            while (choice > 0)
                            {
                                Create c_exp = new Create(delayCreate, "C", distribution);
                                Process p1_exp = new Process(delayProcess, distribution, "P1", maxQ);
                                Process p2_exp = new Process(delayProcess, distribution, "P2", maxQ);
                                Process p3_exp = new Process(delayProcess, distribution, "P3", maxQ);
                                Process p4_exp = new Process(delayProcess, distribution, "P4", maxQ);
                                Dispose d1_exp = new Dispose(2.0, "D1", distribution);
                                Dispose d2_exp = new Dispose(2.0, "D2", distribution);

                                c_exp.NextElements.AddRange(new List<Element>()
                                {
                                    p1_exp,
                                    p4_exp,
                                    d2_exp
                                });

                                p1_exp.NextElements.AddRange(new List<Element>()
                                {
                                    p2_exp,
                                    p3_exp,
                                    p4_exp,
                                    d2_exp
                                });

                                p2_exp.NextElements.AddRange(new List<Element>()
                                {
                                    p1_exp,
                                    p3_exp,
                                    d1_exp
                                });
                                p3_exp.NextElements.AddRange(new List<Element>()
                                {
                                    p1_exp,
                                    p2_exp,
                                    p4_exp
                                });

                                p4_exp.NextElements.AddRange(new List<Element>()
                                {
                                    p1_exp,
                                    p3_exp,
                                    d2_exp
                                });
                                List<Element> list_exp = new List<Element>
                                {
                                    c_exp,
                                    p1_exp,
                                    p2_exp,
                                    p3_exp,
                                    p4_exp,
                                    d1_exp,
                                    d2_exp
                                };
                                Model model_exp = new Model(list_exp, false);
                                model_exp.Simulate(time);

                                delayCreate++;
                                delayProcess--;


                                table.AddRow(
                                    c_exp.GetQuantity(),
                                    maxQ,
                                    Math.Round(model_exp.RAver, 5),
                                    Math.Round(model_exp.PFailure, 5),
                                    Math.Round(model_exp.MeanQueue, 5));

                                choice--;
                            }

                            table.Write(Format.Alternative);
                            Console.WriteLine();

                            Console.ReadKey();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadKey();
                            continue;
                        }

                        break;
                    }

                    #endregion

                    #region Other

                    case '3':
                    {
                        Environment.Exit(0);
                        break;
                    }
                    default:
                    {
                        Console.WriteLine("\nIncorrect choice");
                        Console.ReadKey();
                        break;
                    }

                    #endregion
                }

                Console.Clear();
            }
        }
    }
}