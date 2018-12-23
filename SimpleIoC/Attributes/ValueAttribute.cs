using SimpleIoC.Parser;
using System;

namespace SimpleIoC.Attributes
{
    //TODO: implemention from Settings
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class ValueAttribute : Attribute
    {
        public object Value; 
        public ValueAttribute(object value)
        {
            Value = value;
        }

        public ValueAttribute(Type ExpressionParser, string expression)
        {
            if (!typeof(IExpressionParser).IsAssignableFrom(ExpressionParser))
            {
                throw new NotImplementedException();
            }
            var parser = (IExpressionParser)Activator.CreateInstance(ExpressionParser);
            if (parser.IsAcceptable(expression))
            {
                Value = parser.Parse(expression);
            }
            else
            {
                Value = expression;
            }
        }
        
    }
}
