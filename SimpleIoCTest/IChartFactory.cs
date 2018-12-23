namespace SimpleIoCTest
{
    public interface IChartFactory
    {
        IChart GetChart(ChartType type);
    }
}
