namespace SimpleIoC.Parser
{
    public class BeanNameExpressionParser : IExpressionParser
    {
        public bool IsAcceptable(string value)
        {
            return value.StartsWith("#");
        }

        public string Parse(string value)
        {
            return value.Substring(1);
        }
    }
}
