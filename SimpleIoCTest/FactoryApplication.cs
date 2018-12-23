using SimpleIoC.Application;
using SimpleIoC.Attributes;
using SimpleIoC.Bean;
using System;

namespace SimpleIoCTest
{
    [Component]
    public class FactoryApplication : IApplication
    {
        [Autowired("$FactoryName")]
        private IChartFactory Factory { get; set; }

        [Autowired]
        private BeanManager beanManager;

        [Bean]
        public BeanChartFactory createBeanChartFactory()
        {
            return new BeanChartFactory();
        }
        
        public void Run(string[] args)
        {
            var type = ChartType.Line;
            var chart = Factory.GetChart(type);
            chart.Draw();
            Console.ReadLine();
        }
    }
}
