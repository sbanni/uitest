using OpenQA.Selenium.Appium;
using UITest.Core;

namespace UITest.Appium
{
    public class AppiumIOSPointerActions : ICommandExecutionGroup
    {
        const string DoubleClickCommand = "doubleClick";
        const string DragAndDropCommand = "dragAndDrop";

        readonly List<string> _commands = new()
        {
            DoubleClickCommand
        };
        readonly AppiumApp _appiumApp;

        public AppiumIOSPointerActions(AppiumApp appiumApp)
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

            _appiumApp.Driver.ExecuteScript("mobile: doubleTap", new Dictionary<string, object>
                {
                    { "elementId", element.GetProperty("id") },
                });
        }

        void DragAndDrop(IDictionary<string, object> actionParams)
        {
            var sourceElement = (AppiumElement)actionParams["sourceElement"];
            var destinationElement = (AppiumElement)actionParams["destinationElement"];

            var sourceCenterX = sourceElement.Location.X + (sourceElement.Size.Width / 2);
            var sourceCenterY = sourceElement.Location.Y + (sourceElement.Size.Height / 2);
            var destCenterX = destinationElement.Location.X + (destinationElement.Size.Width / 2);
            var destCenterY = destinationElement.Location.Y + (destinationElement.Size.Height / 2);

            // iOS doesn't seem to work with the action API, so we are using script calls
            _appiumApp.Driver.ExecuteScript("mobile: dragFromToWithVelocity", new Dictionary<string, object>
                {
                    { "pressDuration", 1 }, // Length of time to hold after click before start dragging
					{ "holdDuration", .1 }, // Length of time to hold before releasing
					{ "velocity", CalculateDurationForSwipe(sourceCenterX, sourceCenterY, destCenterX,destCenterY, 500) }, // How fast to drag
					// from/to are absolute screen coordinates unless 'element' is specified then everything will be relative
					{ "fromX", sourceCenterX},
                    { "fromY", sourceCenterY },
                    { "toX", destCenterX },
                    { "toY", destCenterY }
                });
        }

        static int CalculateDurationForSwipe(int startX, int startY, int endX, int endY, int pixelsPerSecond)
        {
            var distance = Math.Sqrt(Math.Pow(startX - endX, 2) + Math.Pow(startY - endY, 2));

            return (int)(distance / (pixelsPerSecond / 1000.0));
        }
    }
}