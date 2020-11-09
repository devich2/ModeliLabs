﻿using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables;

namespace Laba4
{
    class Program
    {
        static void Main(string[] args)
        {
              while (true)
              {
                  Console.Clear();
                  Console.Write("Choose:\n1. Default launch\n2. Verification\n3. Verify exact\n4. Exit\nYour choice: ");
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
                              Mss mss1 = new Mss(delayProcess, 1, maxQ, distribution, "MSS1", true);
                              Mss mss2 = new Mss(delayProcess, 1, maxQ, distribution, "MSS2", true);
                              Mss mss3 = new Mss(delayProcess, 1, maxQ, distribution, "MSS3", true);
                              Mss mss4 = new Mss(delayProcess, 1, maxQ, distribution, "MSS4", true);
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
                          delayProcess = 2;
                          maxQ = 10;
                        
                          distribution = "exp";
                          while (choice > 0)
                          {
                            
                              Create c = new Create(delayCreate, distribution, "CREATOR");
                              Mss mss1 = new Mss(delayProcess, 1, maxQ, distribution, "MSS1", true);
                              Mss mss2 = new Mss(delayProcess, 1, maxQ, distribution, "MSS2", true);
                              Mss mss3 = new Mss(delayProcess, 1, maxQ, distribution, "MSS3", true);
                              Mss mss4 = new Mss(delayProcess, 1, maxQ, distribution, "MSS4", true);
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
                              //delayProcess++;
                              choice--;
                          }
                        
                          table.Write(Format.Alternative);
                          Console.WriteLine();
                        
                          Console.ReadKey();
                            
                          break;
                      }
        
                      #endregion
                    
                      #region 3

                      case '3':
                          var tableVer = new ConsoleTable("mean queue MSS1", "mean queue MSS2", "mean queue MSS3",
                              "mean queue MSS4", "load average MSS1", "load average MSS2", "load average MSS3",
                              "load average MSS4");

                          double mss1MeanQueue = 0,
                              mss2MeanQueue = 0,
                              mss3MeanQueue = 0,
                              mss4MeanQueue = 0,
                              mss1RAver = 0,
                              mss2RAver = 0,
                              mss3RAver = 0,
                              mss4RAver = 0;
                          int runAmount = 100;
                          for (int i = 0; i < runAmount; i++)
                          {
                              Create c = new Create(2, "exp", "CREATOR");
                              Mss mss1 = new Mss(0.6, 1, 100, "exp", "MSS1", true);
                              Mss mss2 = new Mss(0.3, 1, 100, "exp", "MSS2", true);
                              Mss mss3 = new Mss(0.4, 1, 100, "exp", "MSS3", true);
                              Mss mss4 = new Mss(0.1, 2, 100, "exp", "MSS4", true);
                              Despose d = new Despose(2.0, "DESPOSER");

                              Path helper = new Path();
                              helper.SetPathCreateToMss(c, mss1);
                              helper.SetPathMssToDespose(mss1, d);
                              helper.SetPathMssToMss(mss1, mss2);
                              helper.SetPathMssToMss(mss1, mss3);
                              helper.SetPathMssToMss(mss1, mss4);
                              mss1.SetChoiceValue(true, new List<double> {0.42, 0.15, 0.13, 0.3});
                              helper.SetPathMssToMss(mss2, mss1);
                              helper.SetPathMssToMss(mss3, mss1);
                              helper.SetPathMssToMss(mss4, mss1);
                              helper.SetPathMssToMss(mss4, mss1);

                              List<Element> list = new List<Element>
                              {
                                  c,
                                  mss1,
                                  mss2,
                                  mss3,
                                  mss4,
                                  d
                              };
                              Model model = new Model(list, false);
                              model.Simulate(10000);
                              mss1MeanQueue += mss1.MeanQueue / runAmount;
                              mss2MeanQueue += mss2.MeanQueue / runAmount;
                              mss3MeanQueue += mss3.MeanQueue / runAmount;
                              mss4MeanQueue += mss4.MeanQueue / runAmount;
                              mss1RAver += mss1.RAver / runAmount;
                              mss2RAver += mss2.RAver / runAmount;
                              mss3RAver += mss3.RAver / runAmount;
                              mss4RAver += mss4.RAver / runAmount;
                              tableVer.AddRow(
                                  Math.Round(mss1.MeanQueue, 10),
                                  Math.Round(mss2.MeanQueue, 10),
                                  Math.Round(mss3.MeanQueue, 10),
                                  Math.Round(mss4.MeanQueue, 10),
                                  Math.Round(mss1.RAver, 10),
                                  Math.Round(mss2.RAver, 10),
                                  Math.Round(mss3.RAver, 10),
                                  Math.Round(mss4.RAver, 10));
                          }

                          mss1MeanQueue = Math.Round(mss1MeanQueue, 10);
                          mss2MeanQueue = Math.Round(mss2MeanQueue, 10);
                          mss3MeanQueue = Math.Round(mss3MeanQueue, 10);
                          mss4MeanQueue = Math.Round(mss4MeanQueue, 8);
                          mss1RAver = Math.Round(mss1RAver, 10);
                          mss2RAver = Math.Round(mss2RAver, 10);
                          mss3RAver = Math.Round(mss3RAver, 10);
                          mss4RAver = Math.Round(mss4RAver, 10);

                          tableVer.AddRow(
                              mss1MeanQueue,
                              mss2MeanQueue,
                              mss3MeanQueue,
                              mss4MeanQueue,
                              mss1RAver,
                              mss2RAver,
                              mss3RAver,
                              mss4RAver);
                          tableVer.AddRow(
                              1.786,
                              0.003,
                              0.004,
                              0.00001,
                              0.714,
                              0.054,
                              0.062,
                              0.036);
                          tableVer.AddRow(
                              100 * Math.Round(Math.Abs(mss1MeanQueue-1.786)/1.786, 5),
                              100 * Math.Round(Math.Abs(mss2MeanQueue-0.003)/0.003, 5),
                              100 * Math.Round(Math.Abs(mss3MeanQueue-0.004)/0.004, 5),
                              100 * Math.Round(Math.Abs(mss4MeanQueue-0.00001)/0.00001, 5),
                              100 * Math.Round(Math.Abs(mss1RAver-0.714)/0.714, 5),
                              100 * Math.Round(Math.Abs(mss2RAver-0.054)/0.054, 5),
                              100 * Math.Round(Math.Abs(mss3RAver-0.062)/0.062, 5),
                              100 * Math.Round(Math.Abs(mss4RAver-0.036)/0.036, 5));
                          tableVer.Write(Format.Alternative);
                          Console.WriteLine();
                          Console.ReadKey();
                          break;

                      #endregion
                    
                      #region Other
        
                      case '7':
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