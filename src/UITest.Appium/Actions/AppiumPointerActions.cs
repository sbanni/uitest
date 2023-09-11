using OpenQA.Selenium.Appium.Interactions;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System.Drawing;
using OpenQA.Selenium.Appium;
using UITest.Core;

namespace UITest.Appium
{
    public class AppiumPointerActions : ICommandExecutionGroup
    {
        const string ClickCommand = "click";
        const string DoubleClickCommand = "doubleClick";
        const string DragAndDropCommand = "dragAndDrop";

        readonly AppiumApp _appiumApp;
        readonly List<string> _commands = new()
        {
            ClickCommand,
            DoubleClickCommand,
            DragAndDropCommand
        };

        public AppiumPointerActions(AppiumApp appiumApp)
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
                case ClickCommand:
                    Click(parameters);
                    break;
                case DoubleClickCommand:
                    DoubleClick(parameters);
                    break;
                case DragAndDropCommand:
                    DragAndDrop(parameters);
                    break;
            }
        }

        void Click(IDictionary<string, object> parameters)
        {
            if (parameters.TryGetValue("element", out var val))
            {
                ClickElement((AppiumElement)val);
            }
            else if (parameters.TryGetValue("x", out var x) &&
                     parameters.TryGetValue("y", out var y))
            {
                ClickCoordinates(Convert.ToSingle(x), Convert.ToSingle(y));
            }
        }

        void ClickElement(AppiumElement element)
        {
            try
            {
                element.Click();
            }
            catch (InvalidOperationException)
            {
                ProcessException();
            }
            catch (WebDriverException)
            {
                ProcessException();
            }

            void ProcessException()
            {
                // Some elements aren't "clickable" from an automation perspective (e.g., Frame renders as a Border
                // with content in it; if the content is just a TextBlock, we'll end up here)

                // All is not lost; we can figure out the location of the element in in the application window and Tap in that spot
                PointF p = ElementToClickablePoint(element);
                ClickCoordinates(p.X, p.Y);
            }
        }

        void ClickCoordinates(float x, float y)
        {
            OpenQA.Selenium.Appium.Interactions.PointerInputDevice touchDevice = new OpenQA.Selenium.Appium.Interactions.PointerInputDevice(PointerKind.Touch);
            var sequence = new ActionSequence(touchDevice, 0);
            sequence.AddAction(touchDevice.CreatePointerMove(CoordinateOrigin.Viewport, (int)x, (int)y, TimeSpan.FromMilliseconds(5)));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            _appiumApp.Driver.PerformActions(new List<ActionSequence> { sequence });
        }

        void DoubleClick(IDictionary<string, object> parameters)
        {
            var element = (AppiumElement)parameters["element"];

            OpenQA.Selenium.Appium.Interactions.PointerInputDevice touchDevice = new OpenQA.Selenium.Appium.Interactions.PointerInputDevice(PointerKind.Touch);
            var sequence = new ActionSequence(touchDevice, 0);
            sequence.AddAction(touchDevice.CreatePointerMove(element, 0, 0, TimeSpan.FromMilliseconds(5)));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePause(TimeSpan.FromMilliseconds(600)));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            _appiumApp.Driver.PerformActions(new List<ActionSequence> { sequence });
        }

        void DragAndDrop(IDictionary<string, object> actionParams)
        {
            var sourceElement = (AppiumElement)actionParams["sourceElement"];
            var destinationElement = (AppiumElement)actionParams["destinationElement"];

            OpenQA.Selenium.Appium.Interactions.PointerInputDevice touchDevice = new OpenQA.Selenium.Appium.Interactions.PointerInputDevice(PointerKind.Touch);
            var sequence = new ActionSequence(touchDevice, 0);
            sequence.AddAction(touchDevice.CreatePointerMove(sourceElement, 0, 0, TimeSpan.FromMilliseconds(5)));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePause(TimeSpan.FromSeconds(1))); // Have to pause so the device doesn't think we are scrolling
            sequence.AddAction(touchDevice.CreatePointerMove(destinationElement, 0, 0, TimeSpan.FromSeconds(1)));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            _appiumApp.Driver.PerformActions(new List<ActionSequence> { sequence });
        }

        static PointF ElementToClickablePoint(AppiumElement element)
        {
            string cpString = element.GetAttribute("ClickablePoint");
            string[] parts = cpString.Split(',');
            float x = float.Parse(parts[0]);
            float y = float.Parse(parts[1]);

            return new PointF(x, y);
        }
    }
}