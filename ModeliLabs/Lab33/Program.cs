using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables;

namespace Lab33
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
                      
                        try
                        {
                            
                            double delayCreate = 2, time = 100, delayProcess = 11;
                            int maxQ = 4;
                            string distribution;
                
                            // Console.Write("Enter time: ");
                            // time = Convert.ToDouble(Console.ReadLine());
                            // Console.Write("Enter delayCreate: ");
                            // delayCreate = Convert.ToDouble(Console.ReadLine());
                            // Console.Write("Enter delayProcess: ");
                            // delayProcess = Convert.ToDouble(Console.ReadLine());
                            // Console.Write("Enter maxQ: ");
                            // maxQ = Convert.ToInt32(Console.ReadLine());
                            distribution = "norm";
                            
                            Create c = new Create(delayCreate, distribution, "CREATOR");
                            Mss mss1 = new Mss(delayProcess, 1, maxQ, distribution, "MSS1", false);
                            Mss mss2 = new Mss(delayProcess, 1, maxQ, distribution, "MSS2", false);
                            Mss mss3 = new Mss(delayProcess, 1, maxQ, distribution, "MSS3", false);
                            Mss mss4 = new Mss(delayProcess, 1, maxQ, distribution, "MSS4", false);
                            Despose d1 = new Despose(delayProcess, "DESPOSER1");
                            Despose d2 = new Despose(delayProcess, "DESPOSER2");
                
                            Path helper = new Path();
                            helper.SetPathCreateToMss(c, mss1);
                            helper.SetPathCreateToDespose(c, d2);
                            helper.SetPathMssToMss(mss1, mss2);
                            helper.SetPathMssToMss(mss1, mss3);
                            helper.SetPathMssToDespose(mss2, d1);
                            helper.SetPathMssToMss(mss3, mss4);
                            helper.SetPathMssToDespose(mss4, d2);
                            helper.SetPathMssToMss(mss4, mss1);
                           
                            
                            List<Element> list = new List<Element>
                            {
                                c,
                                mss1,
                                mss2,
                                mss3,
                                mss4,
                                d1,
                                d2
                            };
                            Model model = new Model(list, true);
                            model.Simulate(time);
                            model.PrintResults();
                            model.PrintTotalResult();
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
                        
                            double time = 1000, delayCreate, delayProcess;
                            int maxQ;
                            string distribution;
                            Console.Write("Enter launch amount: ");
                            choice = 10;//Convert.ToInt32(Console.ReadLine());
                            if (choice <= 0)
                            {
                                throw new Exception();
                            }
                        
                            Console.WriteLine(
                                "ктс - количество требований пришедших в сеть в модели\n" +
                                "одо    - ограничение на длину очереди\n" +
                                "сзп - средняя загрузка приборов в модели Х \n" +
                                "ко - количество фейл \n" +
                                "во - вероятность фейла \n" +
                                "сдо - средняя длина очереди в модели Х");
                            var table = new ConsoleTable("ктс", "одо", "сзп", "ko", "vo", "сдо");
                            
                            delayCreate = 1; 
                            delayProcess = 3; // ++
                            maxQ = 10;
                        
                            distribution = "exp";
                            while (choice > 0)
                            {
                            
                                Create c = new Create(delayCreate, distribution, "CREATOR");
                                Mss mss1 = new Mss(delayProcess, 1, maxQ, distribution, "MSS1", false);
                                Mss mss2 = new Mss(delayProcess, 1, maxQ, distribution, "MSS2", false);
                                Mss mss3 = new Mss(delayProcess, 1, maxQ, distribution, "MSS3", false);
                                Mss mss4 = new Mss(delayProcess, 1, maxQ, distribution, "MSS4", false);
                                Despose d1 = new Despose(delayProcess, "DESPOSER1");
                                Despose d2 = new Despose(delayProcess, "DESPOSER2");

                                Path helper = new Path();
                                helper.SetPathCreateToMss(c, mss1);
                                //helper.SetPathCreateToDespose(c, d2);
                                helper.SetPathMssToMss(mss1, mss2);
                                helper.SetPathMssToMss(mss2, mss1);
                                helper.SetPathMssToMss(mss1, mss3);
                                helper.SetPathMssToMss(mss3, mss1);
                                helper.SetPathMssToDespose(mss2, d1);
                                helper.SetPathMssToMss(mss3, mss4);
                                helper.SetPathMssToDespose(mss4, d2);
                                helper.SetPathMssToMss(mss4, mss1);
                           
                            
                                List<Element> list = new List<Element>
                                {
                                    c,
                                    mss1,
                                    mss2,
                                    mss3,
                                    mss4,
                                    d1,
                                    d2
                                };
                                Model model = new Model(list, false);
                                model.Simulate(time);
                                
                        
                                // delayCreate++; //--
                                // delayProcess--;
                                Console.WriteLine(model.PFailure);
                                table.AddRow(
                                    list.First().GetQuantity(),
                                    maxQ,
                                    Math.Round(model.RAver, 5),
                                    model.Failures,
                                    Math.Round(model.PFailure, 3),
                                    Math.Round(model.MeanQueue, 5));
                        
                                choice--;
                            }
                        
                            table.Write(Format.Alternative);
                            Console.WriteLine();
                        
                            Console.ReadKey();
                            
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