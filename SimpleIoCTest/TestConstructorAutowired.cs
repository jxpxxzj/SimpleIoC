using SimpleIoC.Attributes;
using SimpleIoC.Parser;

namespace SimpleIoCTest
{
    [Component("NamedBean")]
    public class TestConstructorAutowired
    {
        private IChartFactory factory;

        [Value(typeof(SettingExpressionParser), "$FactoryName")]
        private string valueTest;

        [Autowired]
        public TestConstructorAutowired([Autowired("$FactoryName")] IChartFactory chartFactory,
                                        [Value(typeof(SettingExpressionParser), "$FactoryName")] string value)
        {
            factory = chartFactory;
        }
    }
}
