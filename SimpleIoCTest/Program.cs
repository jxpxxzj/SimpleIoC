using SimpleIoC.Application;

namespace SimpleIoCTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationContext.Run<FactoryApplication>(Settings.Default, args);
        }
    }
}
