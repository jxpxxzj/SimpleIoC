using SimpleIoC.Attributes;

namespace SimpleIoCTest
{
    [Component]
    public class AnotherChartFactory : IChartFactory
    {
        public IChart GetChart(ChartType type)
        {
            return new PieChart();
        }
    }
}
