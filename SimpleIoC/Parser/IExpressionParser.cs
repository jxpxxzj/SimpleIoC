namespace SimpleIoC.Parser
{
    public interface IExpressionParser
    {
        string Parse(string value);
        bool IsAcceptable(string value);
    }
}
