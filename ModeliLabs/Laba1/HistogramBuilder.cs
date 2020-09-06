using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Laba1
{
    public class HistogramBuilder
    {
         private const int X = 8600;
        private const int Y = 5000;
        private const int stableY = Y - 400;
        private const int stableX = 300;
        private const int columnWidth = 500;
        private const int space = 281;

        private Bitmap bmp = new Bitmap(X, Y);
        private Graphics g;

        private Pen scalePen = new Pen(Brushes.Gray);
        private Font scaleFont = new Font("Arial", 70);
        private Font legendFont = new Font("Arial", 80);
        private Pen pen = new Pen(Brushes.Blue);
        private Brush brush = Brushes.Blue;
        public HistogramBuilder(List<double> list, int num)
        {
            g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            scalePen.Width = 3.0F;

            Create(list);
            SaveJPG(num);
        }
        private void Create(List<double> list)
        {
            DrawAxes("x", "frequency");
            list.Sort();
            
            var step = (list.Max() - list.Min()) / 10;

            var columns = GetValues(list, step);
            var currentX = stableX + space;
            double[] intervals = new double[2];
            intervals[0] = list.Min();
            intervals[1] = list.Min() + step;
            foreach (var x in columns)
            {
                g.FillRectangle(brush,
                    currentX, stableY - x,
                    columnWidth, x);
                g.DrawString(x.ToString(), scaleFont, Brushes.Gray,
                    currentX + (columnWidth / 3), Y - 280);
                g.DrawString(String.Format("{0:0.00}", intervals[0]) + " - " +
                    String.Format("{0:0.00}", intervals[1]), scaleFont, Brushes.Gray,
                    currentX, Y - 370);
                currentX += (columnWidth + space);
                intervals[0] = intervals[1];
                intervals[1] += step;
            }
            g.DrawString($"Step is {String.Format("{0:0.00}", step)}", 
                legendFont, Brushes.Black, 150, Y - 150);
        }
        private List<int> GetValues(List<double> list, double step)
        {
            var columns = new List<int>();
            var counter = 0;
            var k = 1;
            foreach (var x in list)
            {
                if (x <= (list.Min() + step * k))
                    counter++;
                else
                {
                    k++;
                    columns.Add(counter);
                    counter = 0;
                }
            }
            return columns;
        }
        private void DrawAxes(string x, string y)
        {
            g.DrawLine(scalePen, stableX, stableY, X - 120, stableY);
            g.DrawLine(scalePen, stableX, stableY, stableX, 200);

            var format = new StringFormat
            {
                FormatFlags = StringFormatFlags.DirectionVertical
            };
            g.DrawString(x, legendFont, Brushes.Gray, X - 150, Y - 400);
            g.DrawString(y, legendFont, Brushes.Gray, 150, 300, format);
        }
        private void SaveJPG(int num)
        {
            string name = $"Histogram_{num}_{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}.jpg";
            Console.WriteLine($"Created : {name}");
            bmp.Save(name);
        }
    }
}