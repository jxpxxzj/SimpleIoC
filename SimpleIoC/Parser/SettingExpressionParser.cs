using SimpleIoC.Application;

namespace SimpleIoC.Parser
{
    public class SettingExpressionParser : IExpressionParser
    {
        public bool IsAcceptable(string value)
        {
            return value.StartsWith("$");
        }

        public string Parse(string value)
        {
            var propName = value.Substring(1);
            var settingsObject = ApplicationContext.Settings;
            return settingsObject.GetType().GetProperty(propName).GetValue(settingsObject, null).ToString();
        }
    }
}
