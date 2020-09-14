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
                            int choise;
                            try
                            {
                                int time = 1000;
                                Console.Write("Кол-во прогонов: ");
                                choise = Convert.ToInt32(Console.ReadLine());
                                if (choise <= 0)
                                {
                                    throw new Exception();
                                }
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
                                var table = new ConsoleTable("сип", "сзп", "одо", "спо м1", "cпо м2", "спо м3", "cво м1", "cво м2", "cво м3", "ктс", "ко м1", "ко м2", "ко м3", "вo", "сдо м1", "сдо м2", "сдо м3");
                                Random rand = new Random();
                                while (choise > 0)
                                {
                                    double delayCreate = rand.NextDouble() * 10;
                                    double delayProcess = rand.NextDouble() * 10;
                                    int maxQ = rand.Next(0, 15);
                                    Model modelExp = new Model(delayCreate, delayProcess, maxQ, 1, false);
                                    Model modelNorm = new Model(delayCreate, delayProcess, maxQ, 2, false);
                                    Model modelUnif = new Model(delayCreate, delayProcess, maxQ, 3, false);
                                    modelExp.Simulate(time);
                                    modelNorm.Simulate(time);
                                    modelUnif.Simulate(time);

                                    table.AddRow(
                                        Math.Round((modelExp.tMean + modelNorm.tMean + modelUnif.tMean) / 3, 2),
                                        Math.Round((modelExp.rAver + modelNorm.rAver + modelUnif.rAver) / 3, 2),
                                        maxQ,
                                        Math.Round(modelExp.tNet, 2),
                                        Math.Round(modelNorm.tNet, 2),
                                        Math.Round(modelUnif.tNet, 2),
                                        Math.Round(modelExp.qAver, 2),
                                        Math.Round(modelNorm.qAver, 2),
                                        Math.Round(modelUnif.qAver, 2),
                                        (modelExp.numCreate + modelNorm.numCreate + modelUnif.numCreate) / 3,
                                        modelExp.failure,
                                        modelNorm.failure,
                                        modelUnif.failure,
                                        Math.Round((modelExp.pFailure + modelNorm.pFailure + modelUnif.pFailure) / 3, 2),
                                        Math.Round(modelExp.lAver, 2),
                                        Math.Round(modelNorm.lAver, 2),
                                        Math.Round(modelUnif.lAver, 2));
                                    choise--;
                                }
                                table.Write(Format.Alternative);
                                Console.WriteLine();

                                Console.ReadKey();
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
                Console.Clear();
            }
        }
    }
}