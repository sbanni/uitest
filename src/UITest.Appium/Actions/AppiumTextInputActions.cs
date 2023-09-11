using OpenQA.Selenium.Appium;
using UITest.Core;

namespace UITest.Appium
{
    public class AppiumTextInputActions : ICommandExecutionGroup
    {
        const string SendKeysCommand = "sendKeys";
        const string ClearCommand = "clear";

        readonly List<string> _commands = new()
        {
            SendKeysCommand,
            ClearCommand,
        };

        public void Execute(string commandName, IDictionary<string, object> parameters)
        {
            if (!IsCommandSupported(commandName))
            {
                return;
            }

            switch (commandName)
            {
                case SendKeysCommand:
                    SendKeys(parameters);
                    break;
                case ClearCommand:
                    Clear(parameters);
                    break;
            }
        }

        public bool IsCommandSupported(string commandName)
        {
            return _commands.Contains(commandName);
        }

        void SendKeys(IDictionary<string, object> parameters)
        {
            var element = (AppiumElement)parameters["element"];
            var text = (string)parameters["text"];
            element.SendKeys(text);
        }

        void Clear(IDictionary<string, object> parameters)
        {
            var element = (AppiumElement)parameters["element"];
            element.Clear();
        }
    }
}