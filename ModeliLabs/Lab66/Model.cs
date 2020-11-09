using System;
using System.Collections.Generic;
using ConsoleTables;

namespace Lab66
{
    public class Model
    {
        private List<Transition> transitionList;
        private List<Condition> conditionList;
        private List<Arc> _arcList;
        private bool showInfo;
        private bool showResult;
        private Random rand;

        private Model(List<Transition> transitions, List<Condition> conditions, List<Arc> arcs)
        {
            rand = new Random();
            transitionList = transitions;
            conditionList = conditions;
            _arcList = arcs;
            showInfo = false;
        }
        public Model(List<Transition> transitions, List<Condition> conditions, List<Arc> arcs, bool showInfo, bool showResult) : this(transitions, conditions, arcs)
        {
            this.showInfo = showInfo;
            this.showResult = showResult;
        }
        public void Simulate(int fireAmount)
        {
            int i;
            for (i = 0; i < fireAmount; i++)
            {
                Transition fired = transitionList.Find(x => x.IsFired);
                if (fired != null)
                {
                    fired.DoOutput();
                    if (showInfo)
                    {
                        Console.WriteLine($"ACTION: {fired.Name} fired");
                    }
                }
                if (showInfo)
                {
                    PrintInfo();
                }
                DoStatictics();
                List<Transition> available = SolveConflict(transitionList.FindAll(x => x.CanMove()));
                if (available.Count == 0)
                {
                    i++;
                    break;
                }
                available[rand.Next(0, available.Count)].DoInput();
            }
            DoStatictics(i);
            if (showResult)
            {
                PrintResults(i);
            }
        }
        private List<Transition> SolveConflict(List<Transition> potentiallyAvailable)
        {
            List<Transition> available = potentiallyAvailable;
            if (potentiallyAvailable.Count > 1) 
            {
                available.Shuffle();
            }
            for (int i = 0; i < potentiallyAvailable.Count; i++)
            {
                List<Condition> placesOfThis = available[i].GetInputConditions();
                for (int j = i + 1; j < potentiallyAvailable.Count; j++)
                {
                    List<Condition> placesOfNext = available[j].GetInputConditions();
                    List<Condition> commonPlaces = placesOfThis.FindAll(x => placesOfNext.Contains(x));
                    if (commonPlaces.Count == 0)
                    {
                        continue;
                    }
                    if (rand.Next(0, 2) == 0)
                    {
                        available.RemoveAt(i);
                        i--;
                        break;
                    }
                    else
                    {
                        available.RemoveAt(j);
                        j--;
                    }
                }
            }
            return available;
        }
        private void PrintInfo()
        {
            Console.WriteLine("STATE:");
            ConsoleTable table = new ConsoleTable("Condition", "Arc", "Transition", "Arc", "Condition");
            List<string> row;
            for (int i = 0; i < conditionList.Count; i++)
            {
                row = new List<string>();
                row.Add($"{conditionList[i].Name}({conditionList[i].Marking})");
                var placeOutput = conditionList[i].OutputArcs;
                if (placeOutput.Count != 0)
                {
                    for (int j = 0; j < placeOutput.Count; j++)
                    {
                        if (j != 0)
                        {
                            row = new List<string>();
                            row.Add("");
                        }
                        row.Add($"---{placeOutput[j].Multiplicity}-->");
                        row.Add($"{placeOutput[j].Transition.Name}");

                        var transitionPutput = placeOutput[j].Transition.OutputArcs;
                        if (transitionPutput.Count != 0)
                        {
                            for (int k = 0; k < transitionPutput.Count; k++)
                            {
                                if (k != 0)
                                {
                                    row = new List<string>();
                                    row.Add("");
                                    row.Add("");
                                    row.Add("");
                                }
                                row.Add($"---{transitionPutput[k].Multiplicity}-->");
                                row.Add($"{transitionPutput[k].Condition.Name}({transitionPutput[k].Condition.Marking})");
                                table.AddRow(row.ToArray());
                            }
                        }
                        else
                        {
                            row.Add("");
                            row.Add("");
                            table.AddRow(row.ToArray());
                        }
                    }
                }
                else
                {
                    row.Add("");
                    row.Add("");
                    row.Add("");
                    row.Add("");
                    table.AddRow(row.ToArray());
                }
            }
            table.Write(Format.Minimal);
        }
        private void PrintResults(int fireAmount)
        {
            var table = new ConsoleTable("", "RESULTS", $"{fireAmount}", "");
            table.AddRow("place", "min", "average", "max");
            for (int i = 0; i < conditionList.Count; i++)
            {
                table.AddRow(conditionList[i].Name, conditionList[i].Min, Math.Round(conditionList[i].Average, 5), conditionList[i].Max);
            }
            table.Write(Format.Alternative);
        }
        private void DoStatictics(int fireAmount)
        {
            for (int i = 0; i < conditionList.Count; i++)
            {
                conditionList[i].Average = (double) conditionList[i].Average / fireAmount;
            }
        }
        private void DoStatictics()
        {
            for (int i = 0; i < conditionList.Count; i++)
            {
                conditionList[i].Average += conditionList[i].Marking;
            }
        }
    }
    static class Extensions
    {
        private static Random rand = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}