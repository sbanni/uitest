using OpenQA.Selenium.Appium;
using UITest.Core;

namespace UITest.Appium
{
    public class AppiumCatalystPointerActions : ICommandExecutionGroup
    {
        const string DoubleClickCommand = "doubleClick";
        const string DragAndDropCommand = "dragAndDrop";

        readonly List<string> _commands = new()
        {
            DoubleClickCommand
        };
        readonly AppiumApp _appiumApp;

        public AppiumCatalystPointerActions(AppiumApp appiumApp)
        {
            _appiumApp = appiumApp;
        }

        public bool IsCommandSupported(string commandName)
        {
            return _commands.Contains(commandName.ToLowerInvariant());
        }

        public void Execute(string commandName, IDictionary<string, object> parameters)
        {
            if (!IsCommandSupported(commandName))
            {
                return;
            }

            switch (commandName)
            {
                case DoubleClickCommand:
                    DoubleClick(parameters);
                    break;
                case DragAndDropCommand:
                    DragAndDrop(parameters);
                    break;
            }
        }

        void DoubleClick(IDictionary<string, object> parameters)
        {
            var element = (AppiumElement)parameters["element"];

            _appiumApp.Driver.ExecuteScript("macos: doubleClick", new Dictionary<string, object>
                {
                    { "elementId", element.GetProperty("id") },
                });
        }

        void DragAndDrop(IDictionary<string, object> actionParams)
        {
            var sourceElement = (AppiumElement)actionParams["sourceElement"];
            var destinationElement = (AppiumElement)actionParams["destinationElement"];

            _appiumApp.Driver.ExecuteScript("macos: clickAndDragAndHold", new Dictionary<string, object>
                {
                    { "holdDuration", .1 }, // Length of time to hold before releasing
                    { "duration", 1 }, // Length of time to hold after click before start dragging
                    { "velocity", 2500 }, // How fast to drag
                    { "sourceElementId", sourceElement.Id },
                    { "destinationElementId", destinationElement.Id },
                });
        }
    }
}