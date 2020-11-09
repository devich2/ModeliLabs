using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileTables;

namespace Lab66
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Default;
            while (true)
            {
                Console.Clear();
                Console.Write("Choose:\n1. Task1\n2. Task2\n3. Task3\n4. Exit\n\nYour choise: ");
                switch (Console.ReadKey().KeyChar)
                {
                    case '1':
                        List<Transition> transitions = new List<Transition>();
                    {
                        Console.Clear();
                        List<Transition> actions = new List<Transition>();
                        List<Condition> conditions = new List<Condition>();
                        List<Arc> arcs = new List<Arc>();
                        for (int i = 0; i < 8; i++)
                        {
                            actions.Add(new Transition($"Trans{i + 1}"));
                        }

                        for (int i = 0; i < 10; i++)
                        {
                            if (i != 0 && i != 4)
                            {
                                conditions.Add(new Condition($"Condition{i + 1}"));
                            }
                            else
                            {
                                conditions.Add(new Condition(1, $"Condition{i + 1}"));
                            }
                        }

                        arcs.Add(new Arc(actions[1 - 1], conditions[1 - 1])); // p1 => t1
                        arcs.Add(new Arc(actions[1 - 1], conditions[2 - 1])); // t1 => p2
                        arcs.Add(new Arc(conditions[2 - 1], actions[2 - 1])); // p2 => t2
                        arcs.Add(new Arc(actions[2 - 1], conditions[1 - 1])); // t2 => p1
                        arcs.Add(new Arc(actions[2 - 1], conditions[3 - 1])); // t2 => p3
                        arcs.Add(new Arc(conditions[3 - 1], actions[3 - 1])); // p3 => t3
                        arcs.Add(new Arc(actions[3 - 1], conditions[4 - 1])); // t3 => p4
                        arcs.Add(new Arc(conditions[4 - 1], actions[4 - 1])); // p4 => t4
                        arcs.Add(new Arc(actions[4 - 1], conditions[5 - 1])); // t4 => p5
                        arcs.Add(new Arc(conditions[5 - 1], actions[3 - 1])); // p5 => t3
                        arcs.Add(new Arc(actions[4 - 1], conditions[6 - 1])); // t4 => p6
                        arcs.Add(new Arc(conditions[6 - 1], actions[5 - 1])); // p6 => t5
                        arcs.Add(new Arc(conditions[6 - 1], actions[6 - 1])); // p6 => t6
                        arcs.Add(new Arc(conditions[6 - 1], actions[7 - 1])); // p6 => t7
                        arcs.Add(new Arc(conditions[6 - 1], actions[8 - 1])); // p6 => t8
                        arcs.Add(new Arc(actions[5 - 1], conditions[7 - 1])); // t5 => p7
                        arcs.Add(new Arc(actions[6 - 1], conditions[8 - 1])); // t6 => p8
                        arcs.Add(new Arc(actions[7 - 1], conditions[9 - 1])); // t7 => p9
                        arcs.Add(new Arc(actions[8 - 1], conditions[10 - 1])); // t8 => p10

                        Model model = new Model(actions, conditions, arcs, true, true);
                        model.Simulate(1000);


                        Console.ReadKey();
                        break;
                    }
                    case '2':
                    {
                        Console.Clear();

                        Console.Clear();
                        if (File.Exists("verification.txt"))
                        {
                            File.Delete("verification.txt");
                        }

                        List<int> marked = new List<int>();
                        Random rand = new Random();
                        FileTable table = new FileTable("#", "state");
                        for (int k = 0; k < 10; k++)
                        {
                            int randIndex = rand.Next(0, 10);
                            while (marked.Contains(randIndex))
                            {
                                randIndex = rand.Next(0, 10);
                            }

                            marked.Add(randIndex);

                            transitions = new List<Transition>();
                            List<Condition> places = new List<Condition>();
                            List<Arc> arcs = new List<Arc>();

                            for (int i = 0; i < 8; i++)
                            {
                                transitions.Add(new Transition($"T{i + 1}"));
                            }

                            List<string> statistic = new List<string>();
                            statistic.Add($"{k + 1}");
                            string state = "";
                            for (int i = 0; i < 10; i++)
                            {
                                if (!marked.Contains(i))
                                {
                                    places.Add(new Condition($"P{i + 1}"));
                                }
                                else
                                {
                                    places.Add(new Condition(1, $"P{i + 1}"));
                                }

                                if (k == 0)
                                {
                                    table.AddColumn(new List<string>
                                        {$"P{i + 1} min", $"P{i + 1} aver", $"P{i + 1} max"});
                                }

                                state += $"P{i + 1}({places[i].Marking}) ";
                            }

                            statistic.Add(state);
                            arcs.Add(new Arc(transitions[1 - 1], places[1 - 1]));
                            arcs.Add(new Arc(places[2 - 1], transitions[1 - 1]));
                            arcs.Add(new Arc(places[1 - 1], transitions[2 - 1]));
                            arcs.Add(new Arc(transitions[2 - 1], places[2 - 1]));
                            arcs.Add(new Arc(transitions[2 - 1], places[3 - 1]));
                            arcs.Add(new Arc(places[3 - 1], transitions[3 - 1]));
                            arcs.Add(new Arc(places[4 - 1], transitions[3 - 1]));
                            arcs.Add(new Arc(transitions[3 - 1], places[5 - 1]));
                            arcs.Add(new Arc(places[5 - 1], transitions[4 - 1]));
                            arcs.Add(new Arc(transitions[4 - 1], places[4 - 1]));
                            arcs.Add(new Arc(transitions[4 - 1], places[6 - 1]));
                            arcs.Add(new Arc(places[6 - 1], transitions[5 - 1]));
                            arcs.Add(new Arc(places[6 - 1], transitions[6 - 1]));
                            arcs.Add(new Arc(places[6 - 1], transitions[7 - 1]));
                            arcs.Add(new Arc(places[6 - 1], transitions[8 - 1]));
                            arcs.Add(new Arc(transitions[5 - 1], places[7 - 1]));
                            arcs.Add(new Arc(transitions[6 - 1], places[8 - 1]));
                            arcs.Add(new Arc(transitions[7 - 1], places[9 - 1]));
                            arcs.Add(new Arc(transitions[8 - 1], places[10 - 1]));

                            Model model = new Model(transitions, places, arcs, false, false);
                            model.Simulate(100);

                            for (int i = 0; i < places.Count; i++)
                            {
                                statistic.Add($"{places[i].Min}");
                                statistic.Add($"{Math.Round(places[i].Average, 5)}");
                                statistic.Add($"{places[i].Max}");
                            }

                            table.AddRow(statistic.ToArray());
                        }

                        table.Write(Format.Alternative);
                        Console.WriteLine("Read verification.txt");
                        Console.ReadKey();
                        break;
                    }
                    case '3':
                    {
                        Environment.Exit(0);
                        break;
                    }
                    default:
                        Console.WriteLine("\nWrong choice");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
