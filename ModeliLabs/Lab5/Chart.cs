using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lab5
{
    public partial class Chart : Form
    {
        public Chart(List<(int, double, double)> sizeTime, (double, double) delays)
        {
            InitializeComponent();
            LoadChart(sizeTime, delays);
        }
        private void LoadChart(List<(int, double, double)> sizeTime, (double, double) delays)
        {
            chartExperimental.Series.Clear();
            Series seriesSequence = new Series
            {
                Name = "sequence",
                IsVisibleInLegend = false,
                ChartType = SeriesChartType.Spline
            };
            Series seriesParallel = new Series
            {
                Name = "parallel",
                IsVisibleInLegend = false,
                ChartType = SeriesChartType.Spline
            };
            chartExperimental.Series.Add(seriesSequence);
            chartExperimental.Series.Add(seriesParallel);

            chartExperimental.Legends.Add(new Legend("Legend"));
            chartExperimental.Series["sequence"].Legend = "Legend";
            chartExperimental.Series["sequence"].IsVisibleInLegend = true;
            chartExperimental.Series["parallel"].Legend = "Legend";
            chartExperimental.Series["parallel"].IsVisibleInLegend = true;
            Series seriesTheory = new Series
            {
                Name = "Series2",
                IsVisibleInLegend = false,
                ChartType = SeriesChartType.Spline
            };
            chartTheoretical.Series.Add(seriesTheory);
            seriesSequence.Points.AddXY(sizeTime[0].Item1, sizeTime[0].Item2);
            seriesParallel.Points.AddXY(sizeTime[0].Item1, sizeTime[0].Item3);
            seriesTheory.Points.AddXY(sizeTime[0].Item1, 0);

            for (int i = 1; i < sizeTime.Count; i++)
            {
                seriesSequence.Points.AddXY(sizeTime[i].Item1 + 1, Math.Round(sizeTime[i].Item2, 5));
                seriesParallel.Points.AddXY(sizeTime[i].Item1 + 1, Math.Round(sizeTime[i].Item3, 5));
                seriesTheory.Points.AddXY(sizeTime[i].Item1 + 1, Math.Round((1 / delays.Item1 + 1 / delays.Item2 * sizeTime[i].Item1) * 100000 * 3, 5));
            }
            chartExperimental.Invalidate();
            chartTheoretical.Invalidate();
        }

        private void chartExperimental_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePoint = new Point(e.X, e.Y);

            chartExperimental.ChartAreas[0].CursorX.Interval = 0;
            chartExperimental.ChartAreas[0].CursorY.Interval = 0;

            chartExperimental.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, true);
            chartExperimental.ChartAreas[0].CursorY.SetCursorPixelPosition(mousePoint, true);

            HitTestResult result = chartExperimental.HitTest(e.X, e.Y);
            try
            {
                label1.Text = $"Pixel X Position: {Math.Round(chartExperimental.ChartAreas[0].AxisX.PixelPositionToValue(e.X), 3)}";
                label2.Text = $"Pixel Y Position: {Math.Round(chartExperimental.ChartAreas[0].AxisY.PixelPositionToValue(e.Y), 3)}";

                if (result.PointIndex > -1 && result.ChartArea != null)
                {
                    label3.Text = $"Time: {result.Series.Points[result.PointIndex].XValue}";
                    label4.Text = $"Events: {result.Series.Points[result.PointIndex].YValues[0]}";
                }
            }
            catch (Exception) { }
        }

    }
}