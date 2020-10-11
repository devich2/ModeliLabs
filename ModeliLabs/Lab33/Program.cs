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
                            
                            double delayCreate = 1, time = 1000, delayProcess = 2;
                            int maxQ = 10;
                            string distribution;
                
                            // Console.Write("Enter time: ");
                            // time = Convert.ToDouble(Console.ReadLine());
                            // Console.Write("Enter delayCreate: ");
                            // delayCreate = Convert.ToDouble(Console.ReadLine());
                            // Console.Write("Enter delayProcess: ");
                            // delayProcess = Convert.ToDouble(Console.ReadLine());
                            // Console.Write("Enter maxQ: ");
                            // maxQ = Convert.ToInt32(Console.ReadLine());
                            distribution = "exp";
                            
                            Create c = new Create(delayCreate, distribution, "CREATOR");
                            Mss mss1 = new Mss(delayProcess, 1, maxQ, distribution, "MSS1", false);
                            Mss mss2 = new Mss(delayProcess, 1, maxQ, distribution, "MSS2", false);
                            Mss mss3 = new Mss(delayProcess, 1, maxQ, distribution, "MSS3", false);
                            Mss mss4 = new Mss(delayProcess, 1, maxQ, distribution, "MSS4", false);
                            Despose d1 = new Despose(delayProcess, "DESPOSER1");
                            Despose d2 = new Despose(delayProcess, "DESPOSER2");
                
                            Path helper = new Path();
                            helper.SetPathCreateToMss(c, mss1);
                           // helper.SetPathMssToDespose(mss1, d2);
                           // helper.SetPathCreateToDespose(c, d2);
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
                            var table = new ConsoleTable("quantity", "max queue", "fails", "pfail", "meanquee1", "raver1", "meanquee2", "raver2", "meanquee3", "raver3", "meanquee4", "raver4");
                            
                            delayCreate = 1; 
                            delayProcess = 1;
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
                                Model model = new Model(list, false);
                                model.Simulate(time);
                              
                               // TableHelper.ShowInfo(model);
                                table.AddRow(
                                    list.First().GetQuantity(),
                                    model.MaxDetectedQueue,
                                    model.Failures,
                                    Math.Round(model.PFailure, 3),
                                    mss1.MeanQueue,
                                    mss1.RAver,
                                    mss2.MeanQueue,
                                    mss2.RAver,
                                    mss3.MeanQueue,
                                    mss3.RAver,
                                    mss4.MeanQueue,
                                    mss4.RAver);
                                delayProcess++;
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