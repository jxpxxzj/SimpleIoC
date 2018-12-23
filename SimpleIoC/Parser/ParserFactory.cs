using System;

namespace SimpleIoC.Parser
{
    public static class ParserFactory
    {
        public static Type GetParser(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.SettingExpression:
                    return typeof(SettingExpressionParser); 
                case ExpressionType.Constant:
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
