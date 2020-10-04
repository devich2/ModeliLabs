using System;
using ConsoleTables;

namespace Laba2
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.Write("1 прогон модельки з вводимыми параметрами \n2 несколько прогонов\n");
                switch (Console.ReadLine().ToCharArray()[0])
                {
                    #region 1

                    case '1':
                    {
                        Console.Clear();
                        double delayCreate, delayProcess;
                        int maxQ, distribution, time;
                        try
                        {
                            Console.Write("Задержка создания: ");
                            delayCreate = Convert.ToDouble(Console.ReadLine());
                            Console.Write("Задержка обработка: ");
                            delayProcess = Convert.ToDouble(Console.ReadLine());
                            Console.Write("Кол-во единиц модельного времени: ");
                            time = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Максимальная длину очереди(-1 если без разницы): ");
                            maxQ = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Распределение рандомайзера(-1 если без разницы): ");
                            distribution = Convert.ToInt32(Console.ReadLine());
                            if (distribution == -1)
                            {
                                distribution = new Random().Next(1, 3);
                            }

                            Console.Clear();
                            Console.Write(" Распределение: ");
                            if (distribution == 1)
                                Console.WriteLine("экспоненциальное\n");
                            else if (distribution == 2)
                                Console.WriteLine("нормальное\n");
                            else
                                Console.WriteLine("равномерное\n");
                            if (maxQ == -1)
                            {
                                Model model = new Model(delayCreate, delayProcess, distribution, true);
                                model.Simulate(time);
                            }
                            else
                            {
                                Model model = new Model(delayCreate, delayProcess, maxQ, distribution, true);
                                model.Simulate(time);
                                Console.ReadKey();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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
                        int repeat;
                        try
                        {
                            int time = 1000;
                            Console.WriteLine(
                                "сип    - средний интервал поступления\n" +
                                "сзп    - средняя загрузка прибора\n" +
                                "одо    - ограничение на длину очереди\n" +
                                "спо мХ - средняя продолжительность обслуживания в модели Х\n" +
                                "cво мХ - среднее время ожидания в модели Х\n" +
                                "ктс    - количество требований пришедших в сеть\n" +
                                "ко мХ  - количество отказов в модели Х\n" +
                                "вo     - вероятность отказа\n" +
                                "сдо мХ - средняя длина очереди в модели Х");
                            var table = new ConsoleTable("сип", "сзп", "одо", "спо м1",  "cво м1", "ктс", "ко м1", "вo", "сдо м1");
                            
                            while(true)
                            {
                                Console.WriteLine("WhatToChange?\n1 DelayCreate \n2 DelayProcess \n3 MaxQuee");
                                int choice = Convert.ToInt32(Console.ReadLine());
                                if(choice == '3')
                                    break;
                                Console.Clear();
                                Console.Write("Кол-во прогонов: ");
                                repeat = Convert.ToInt32(Console.ReadLine());
                                Experimenta(choice, table, repeat, time, 5);
                                Console.Read();
                                Console.Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.ReadKey();
                            continue;
                        }

                        break;
                    }

                    #endregion


                    #region 3

                    case '3':
                    {
                        Environment.Exit(0);
                        break;
                    }

                    #endregion
                }
                
                Console.ReadKey();
            }
        }

        private static void Experimenta(int paramToChange, ConsoleTable table, int repeat, int time, double koef)
        {
            double delayCreate = 2;
            double delayProcess = 4;
            int maxQ = 2;
            
            while (repeat > 0)
            {
                Model modelExp = new Model(delayCreate, delayProcess, maxQ, 1, false);
                modelExp.Simulate(time);
                table.AddRow(
                    Math.Round(modelExp.tMean, 2),
                    Math.Round(modelExp.rAver, 2),
                    maxQ,
                    Math.Round(modelExp.tNet, 2),
                    Math.Round(modelExp.qAver, 2),
 
                    modelExp.numCreate,
                    modelExp.failure,
                    modelExp.pFailure,
                    Math.Round(modelExp.lAver, 2));
                Console.WriteLine(maxQ);
                switch (paramToChange)
                {
                    case 1:
                        delayCreate *= koef;
                        break;
                    case 2:
                        delayProcess *= koef;
                        break;
                    case 3:
                        maxQ*=(int)koef;
                        break;
                }
                repeat--;
            }

            table.Write(Format.Alternative);
            Console.WriteLine();
        }
    }
}