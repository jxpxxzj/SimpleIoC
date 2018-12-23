using SimpleIoC.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleIoC.Bean
{
    public class BeanFactory
    {
        private BeanManager manager;

        public BeanFactory(BeanManager beanManager) {
            manager = beanManager;
        }

        private List<Type> getAssignableTypes(Type interfaceType)
        {
            return (from bean in manager.GetBeans()
                    where interfaceType.IsAssignableFrom(bean.Value.BeanType)
                    select bean.Value.BeanType).ToList();
        }

        private Type getTypeByAutowiredAttributeClassName(AutowiredAttribute attr)
        {
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    from type in assembly.GetTypes()
                    where type.FullName == attr.BeanClassName || type.Name == attr.BeanClassName
                    select type).First();
        }

        private object createWithMethod(BeanMetadata meta, object baseObject)
        {
            if (manager.GetBeanMetadata(meta.Name).IsCreated) // check by name
            {
                return meta.Bean;
            }

            var parameters = meta.Parameters;
            var ctorParams = new List<object>();
            foreach (var parameter in parameters)
            {
                // inject [Value]
                var valueAttr = parameter.GetCustomAttribute<ValueAttribute>();
                if (valueAttr != null)
                {
                    ctorParams.Add(valueAttr.Value);
                    continue;
                }

                var types = getAssignableTypes(parameter.GetType());
                Type certainType = null;
                if (types.Count == 1) // no multiply interface implemention
                {
                    certainType = types[0];
                }
                else // scan [Autowired] in parameters
                {
                    var paraAttr = parameter.GetCustomAttribute<AutowiredAttribute>();
                    certainType = getTypeByAutowiredAttributeClassName(paraAttr);
                }
                if (manager.ContainsBean(certainType))
                {
                    ctorParams.Add(createBean(manager.GetBeanMetadata(certainType)));
                }

            }

            object bean;
            if (meta.IsClassConstructor)
            {
                bean = ((ConstructorInfo)meta.Constructor).Invoke(ctorParams.ToArray());
            }
            else if (meta.IsMethodConstructor)
            {
                bean = ((MethodInfo)meta.Constructor).Invoke(baseObject, ctorParams.ToArray());
            }
            else
            {
                throw new NotImplementedException();
            }

            meta.Bean = bean;

            return bean;
        }

        public void CreateAllBeans()
        {
            foreach(var bean in manager.GetBeans())
            {
                createBean(bean.Value);
            }
        }

        private object createBean(BeanMetadata meta)
        {
            if (manager.GetBeanMetadata(meta.Name).IsCreated) // check by name
            {
                return meta.Bean;
            }

            object bean = null;

            if (meta.IsClassConstructor) // a class constructor
            {
                bean = createWithMethod(meta, null);
            }
            else // a bean method
            {
                var parentObject = createBean(manager.GetBeanMetadata(meta.ParentType));
                bean = createWithMethod(meta, parentObject);
            }

            // scan [Autowired] / [Value] for properties

            var properties = meta.BeanType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            foreach (var property in properties)
            {
                // [Value]
                var propAttrValue = property.GetCustomAttribute<ValueAttribute>();
                if (propAttrValue != null)
                {
                    property.SetValue(bean, propAttrValue.Value);
                    continue;
                }

                // [Autowired]
                var propAttrAutowired = property.GetCustomAttribute<AutowiredAttribute>();
                if (propAttrAutowired != null)
                {
                    var propInterfaceType = property.PropertyType;
                    var propTypes = getAssignableTypes(propInterfaceType);
                    Type propType;
                    if (propTypes.Count == 1)
                    {
                        propType = propTypes[0];
                    }
                    else
                    {
                        propType = getTypeByAutowiredAttributeClassName(propAttrAutowired);
                    }

                    var propBeanMeta = manager.GetBeanMetadata(propType);
                    var propBean = createBean(propBeanMeta);
                    property.SetValue(bean, propBean);
                }
            }

            // scan for fields

            var fields = meta.BeanType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            foreach(var field in fields)
            {
                var fieldAttrValue = field.GetCustomAttribute<ValueAttribute>();
                if (fieldAttrValue != null)
                {
                    field.SetValue(bean, fieldAttrValue.Value);
                    continue;
                }

                var fieldAttrAutowired = field.GetCustomAttribute<AutowiredAttribute>();
                if (fieldAttrAutowired != null)
                {
                    var fieldInterfaceType = field.FieldType;
                    var fieldTypes = getAssignableTypes(fieldInterfaceType);
                    Type fieldType;
                    if (fieldTypes.Count == 1)
                    {
                        fieldType = fieldTypes[0];
                    }
                    else
                    {
                        fieldType = getTypeByAutowiredAttributeClassName(fieldAttrAutowired);
                    }

                    var fieldBeanMeta = manager.GetBeanMetadata(fieldType);
                    var fieldBean = createBean(fieldBeanMeta);
                    field.SetValue(bean, fieldBean);
                }
            }

            return bean;
        }
    }
}
