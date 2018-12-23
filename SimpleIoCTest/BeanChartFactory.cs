using SimpleIoC.Attributes;

namespace SimpleIoCTest
{
    [Component]
    public class BeanChartFactory : IChartFactory
    {
        public IChart GetChart(ChartType type)
        {
            return new PieChart();
        }
    }
}
