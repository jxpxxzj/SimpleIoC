using SimpleIoC.Attributes;
using System;

namespace SimpleIoCTest
{
    public enum ChartType
    {
        Pie,
        Line
    }

    [Component]
    public class ChartFactory : IChartFactory
    {
        public IChart GetChart(ChartType type)
        {
            switch (type)
            {
                case ChartType.Pie:
                    return new PieChart();
                case ChartType.Line:
                    return new LineChart();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
