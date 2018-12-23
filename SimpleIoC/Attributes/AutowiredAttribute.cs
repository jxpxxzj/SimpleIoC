using SimpleIoC.Parser;
using System;

namespace SimpleIoC.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Parameter | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class AutowiredAttribute : Attribute
    {
        private string beanClassName = string.Empty;
        public AutowiredAttribute() : this(string.Empty)
        {

        }

        public AutowiredAttribute(string ClassName) : this(typeof(SettingExpressionParser), ClassName)
        {
            
        }

        public AutowiredAttribute(Type ExpressionParser, string ClassName)
        {
            if (!typeof(IExpressionParser).IsAssignableFrom(ExpressionParser))
            {
                throw new NotImplementedException();
            }
            var parser = (IExpressionParser)Activator.CreateInstance(ExpressionParser);
            if (parser.IsAcceptable(ClassName))
            {
                BeanClassName = parser.Parse(ClassName);
            }
            else
            {
                BeanClassName = ClassName;
            }
        }

        public string BeanClassName { get; set; }
    }
}
