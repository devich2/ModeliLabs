using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileTables;
using Lab66;

namespace Lab8
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
                switch (Console.ReadLine().ToCharArray()[0])
                {
                    case '1':
                            List<Transition> transitions = new List<Transition>();
                    {
                            Console.Clear();
                            List<Condition> conditions = new List<Condition>();
                            List<Arc> arcs = new List<Arc>();

                            transitions.Add(new Transition("Генератор для А(FROM)")); // T1
                            transitions.Add(new Transition("Генератор для А(BACK)")); // T2
                            transitions.Add(new Transition("Хендшейк")); // T3
                            transitions.Add(new Transition("Обробка повідомлення пунктом B")); // T4
                            transitions.Add(new Transition("Генератор для В(FROM)")); // T5
                            transitions.Add(new Transition("Генератор для B(BACK)")); // T6
                            transitions.Add(new Transition("Хендшейк")); // T7
                            transitions.Add(new Transition("Обробка повідомлення пунктом A")); // T8

                            conditions.Add(new Condition(1, "надходження в А")); // 0
                            conditions.Add(new Condition("черга А")); // 1
                            conditions.Add(new Condition("буфер Б")); // 2
                            conditions.Add(new Condition("оброблено пунктом Б")); // 3
                            
                            conditions.Add(new Condition(1, "надходження в Б")); // 4
                            conditions.Add(new Condition("черга Б")); // 5
                            conditions.Add(new Condition("буфер А")); // 6
                            conditions.Add(new Condition("оброблено пунктом А")); // 7
                            //--------
                            conditions.Add(new Condition(1, "вільний канал")); // 8
                            //--------
                            
                            arcs.Add(new Arc(transitions[0], conditions[0])); //t1=>p1
                            arcs.Add(new Arc(conditions[0], transitions[0])); //p1=>t1


                            arcs.Add(new Arc(transitions[0], conditions[1])); //t1=>p2
                            arcs.Add(new Arc(conditions[1], transitions[1])); //p2=>t2
                            arcs.Add(new Arc(transitions[1], conditions[2])); //t2=>p3
                            arcs.Add(new Arc(conditions[2], transitions[2])); //p3=>t3
                            arcs.Add(new Arc(transitions[2], conditions[3])); //t3=>p4
                            
                            arcs.Add(new Arc(conditions[4], transitions[3])); //p5=>t4
                            arcs.Add(new Arc(transitions[3], conditions[4])); //t4=>p5
                            
                            arcs.Add(new Arc(transitions[3], conditions[5])); //t4=>p6
                            arcs.Add(new Arc(conditions[5], transitions[4])); //p6=>t5
                            arcs.Add(new Arc(transitions[4], conditions[6])); //t5=>p7
                            arcs.Add(new Arc(conditions[6], transitions[5])); //p7=>t6
                            arcs.Add(new Arc(transitions[5], conditions[7])); //t6=>p8
                            
                            arcs.Add(new Arc(transitions[2], conditions[8])); //t3=>p9
                            arcs.Add(new Arc(transitions[5], conditions[8])); //t6=>p9
                            arcs.Add(new Arc(conditions[8], transitions[1])); //p9=>t2
                            arcs.Add(new Arc(conditions[8], transitions[4])); //p9=>t5
                            

                            Model model = new Model(transitions, conditions, arcs, true, true);
                            model.Simulate(100);

                            Console.ReadKey();
                            break;
                        }
                    case '2':
                        {
                            Console.Clear();

                            Console.Write("Enter buffer size: ");
                            int n = Convert.ToInt32(Console.ReadLine());

                            transitions = new List<Transition>(); 
                            List<Condition> conditions = new List<Condition>();
                            List<Arc> arcs = new List<Arc>();

                            transitions.Add(new Transition("Producer")); // 0
                            transitions.Add(new Transition("Consumer")); // 1

                            conditions.Add(new Condition(1, "надходження")); // 0
                            conditions.Add(new Condition("буфер")); // 1
                            conditions.Add(new Condition(n, "вiльне мiсце в буферi")); // 2
                            conditions.Add(new Condition("обробленi")); // 3

                            arcs.Add(new Arc(conditions[0], transitions[0]));
                            arcs.Add(new Arc(transitions[0], conditions[0]));
                            arcs.Add(new Arc(transitions[0], conditions[1]));
                            arcs.Add(new Arc(conditions[1], transitions[1]));
                            arcs.Add(new Arc(transitions[1], conditions[2]));
                            arcs.Add(new Arc(conditions[2], transitions[0]));
                            arcs.Add(new Arc(transitions[1], conditions[3]));

                            Model model = new Model(transitions, conditions, arcs, true, true);
                            model.Simulate(100);

                            Console.ReadKey();
                            break;
                        }
                    case '3':
                        {
                            Console.Clear();
                            int k;
                            do
                            {
                                Console.Write("Enter processor number (it should divide by 6): ");
                                k = Convert.ToInt32(Console.ReadLine());
                            } while (k % 6 != 0 || k <= 0);
                            
                            transitions = new List<Transition>();
                            List<Condition> conditions = new List<Condition>();
                            List<Arc> arcs = new List<Arc>();

                            transitions.Add(new Transition("обробка типу 1")); // 0
                            transitions.Add(new Transition("обробка типу 2")); // 1
                            transitions.Add(new Transition("обробка типу 3")); // 2

                            conditions.Add(new Condition(1, "задачi типу 1")); // 0
                            conditions.Add(new Condition(1, "задачi типу 2")); // 1
                            conditions.Add(new Condition(1, "задачi типу 3")); // 2
                            conditions.Add(new Condition(k, "вiльнi процесори")); // 3
                            conditions.Add(new Condition("обробленi типу 1")); // 4
                            conditions.Add(new Condition("обробленi типу 2")); // 5
                            conditions.Add(new Condition("обробленi типу 3")); // 6

                            arcs.Add(new Arc(conditions[0], transitions[0]));
                            arcs.Add(new Arc(transitions[0], conditions[0]));
                            arcs.Add(new Arc(conditions[1], transitions[1]));
                            arcs.Add(new Arc(transitions[1], conditions[1]));
                            arcs.Add(new Arc(conditions[2], transitions[2]));
                            arcs.Add(new Arc(transitions[2], conditions[2]));
                            arcs.Add(new Arc(transitions[0], conditions[4]));
                            arcs.Add(new Arc(transitions[1], conditions[5]));
                            arcs.Add(new Arc(transitions[2], conditions[6]));
                            arcs.Add(new Arc(conditions[3], transitions[0], k));
                            arcs.Add(new Arc(transitions[0], conditions[3], k));
                            arcs.Add(new Arc(conditions[3], transitions[1], k / 3));
                            arcs.Add(new Arc(transitions[1], conditions[3], k / 3));
                            arcs.Add(new Arc(conditions[3], transitions[2], k / 2));
                            arcs.Add(new Arc(transitions[2], conditions[3], k / 2));

                            Model model = new Model(transitions, conditions, arcs, true, true);
                            model.Simulate(100);

                            List<double> values = new List<double> { conditions[4].Marking, conditions[5].Marking, conditions[6].Marking };
                            double min = values.Min();
                            Console.WriteLine($"Виконані завдання за типами співвідносяться приблизно як {Math.Round(values[0] / min, 0)} : {Math.Round(values[1] / min, 0)} : {Math.Round(values[2] / min, 0)}");

                            Console.ReadKey();
                            break;
                        }
                    case '4':
                        {
                            Environment.Exit(0);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("\nIncorrect choise");
                            Console.ReadKey();
                            break;
                        }
                }
            }
        }
    }
}