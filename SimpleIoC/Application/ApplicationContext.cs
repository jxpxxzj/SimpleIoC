using SimpleIoC.Bean;
using System;

namespace SimpleIoC.Application
{
    public class ApplicationContext
    {
        internal static dynamic Settings;

        public static void Run<T>() where T : IApplication
        {
            Run(typeof(T), null, null);
        }

        public static void Run<T>(dynamic settings) where T : IApplication
        {
            Run(typeof(T), settings, null);
        }

        public static void Run<T>(dynamic settings, string[] args) where T : IApplication
        {
            Run(typeof(T), settings, args);
        }

        public static void Run(Type applicationType, dynamic settingObject, string[] args)
        {
            if (!typeof(IApplication).IsAssignableFrom(applicationType)) {
                throw new NotSupportedException();
            }

            Settings = settingObject;

            var beanManager = new BeanManager();

            var attrScanner = new AttributeScanner(beanManager);

            var beanFactory = new BeanFactory(beanManager);

            beanFactory.CreateAllBeans();

            var application = beanManager.GetBean<IApplication>();

            application.Run(args);

            Console.ReadLine();
        }
    }
}
