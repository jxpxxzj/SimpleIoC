using SimpleIoC.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleIoC.Bean
{
    public class AttributeScanner
    {
        private BeanManager manager;
        public AttributeScanner(BeanManager beanManager)
        {
            manager = beanManager;
            scanComponents();
        }

        private BeanMetadata mapToBeanMeta(Type T, string name)
        {
            var constructors = T.GetConstructors();
            var noParamConsturctor = (from constructor in constructors
                                      let paramList = constructor.GetParameters()
                                      where paramList.Count() == 0
                                      select constructor);

            if (noParamConsturctor.Count() == 0)
            {
                return new BeanMetadata((from constructor in constructors
                                         let attrs = constructor.GetCustomAttributes<AutowiredAttribute>()
                                         where attrs != null && attrs.Count() > 0
                                         select constructor).First(), name);
            }
            else
            {
                return new BeanMetadata(noParamConsturctor.First(), name);
            }
        }

        private void scanComponents()
        {
            var beans = new List<BeanMetadata>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    var componentAttr = type.GetCustomAttributes<ComponentAttribute>();
                    if (componentAttr != null && componentAttr.Count() > 0)
                    {
                        beans.Add(mapToBeanMeta(type, componentAttr.First().Name));
                    }
                    foreach (var method in type.GetMethods())
                    {
                        var methodAttr = method.GetCustomAttributes<BeanAttribute>();
                        if (methodAttr != null && methodAttr.Count() > 0)
                        {
                            beans.Add(new BeanMetadata(method, methodAttr.First().Name));
                        }
                    }
                }
            }

            var list = beans
                .Distinct(BeanMetadata.EqualityComparer)
                .OrderBy(meta => meta.Parameters.Count)
                .ThenBy(meta => meta.IsMethodConstructor);

            foreach (var bean in list)
            {
                manager.AddBean(bean);
            }
        }
    }
}
