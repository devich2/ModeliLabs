﻿using System.Collections.Generic;
using System.Linq;
using ConsoleTables;

namespace Laba4
{
    public static class TableHelper
    {
        public static void ShowInfo(Model model)
        {
            var smos = model._list.OfType<Mss>();
            List<string> mainInfo = new List<string>()
            {
                "cquantity", "maxQ",
                "name",
                "quantity",
                "fail",
                "pfail", 
                "meanqueue", "maxqueue", "raver",
            };
             
            var table = new ConsoleTable(mainInfo.ToArray());
            foreach (var smo in smos)
            {
                table.AddRow(model._list.First().GetQuantity(), model.MaxDetectedQueue, smo.Name, smo.GetQuantity(), model.Failures, model.PFailure, smo.MeanQueue, smo.MaxQueue, smo.RAver);
            }
            table.Write(Format.Alternative);
        }
    }
}