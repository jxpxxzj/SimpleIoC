using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleIoC.Bean
{
    public class BeanManager
    {
        private Dictionary<string, BeanMetadata> beans { get; set; } = new Dictionary<string, BeanMetadata>();

        public BeanManager()
        {
            var selfMeta = new BeanMetadata(typeof(BeanManager).GetConstructors()[0], null, this);
            beans.Add(selfMeta.Name, selfMeta);
        }

        public BeanMetadata GetBeanMetadata(string name)
        {
            if (beans.ContainsKey(name))
            {
                return beans[name];
            }
            else
            {
                var type = Type.GetType(name, true, false);
                return GetBeanMetadata(type);
            }
        }

        public BeanMetadata GetBeanMetadata(Type type)
        {
            return (from bean in beans
                    where type.IsAssignableFrom(bean.Value.BeanType)
                    select bean.Value).First();
        }

        public bool ContainsBean(Type type)
        {
            return (from bean in beans
                    where type.IsAssignableFrom(bean.Value.BeanType)
                    select bean).Count() == 1;
        }

        public bool ContainsBean(string name)
        {
            if (beans.ContainsKey(name))
            {
                return true;
            }
            else
            {
                return ContainsBean(Type.GetType(name, true, false));
            }
        }

        public object GetBean(string name)
        {
            if (beans.ContainsKey(name))
            {
                return beans[name];
            }
            else
            {
                return GetBean(Type.GetType(name, true, false));
            }
        }

        public object GetBean(Type type)
        {
            return GetBeanMetadata(type).Bean;
        }

        public T GetBean<T>()
        {
            return (T)GetBean(typeof(T));
        }

        public Dictionary<string, BeanMetadata> GetBeans()
        {
            return beans;
        }

        public void AddBean(BeanMetadata bean)
        {
            if (!beans.ContainsKey(bean.Name))
            {
                beans.Add(bean.Name, bean);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
