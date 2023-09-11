using OpenQA.Selenium.Appium;
using UITest.Core;

namespace UITest.Appium
{
    public class AppiumDriverElement : IElement
    {
        readonly AppiumElement _element;
        readonly AppiumApp _appiumApp;

        public AppiumDriverElement(AppiumElement element, AppiumApp appiumApp)
        {
            _appiumApp = appiumApp;
            _element = element ?? throw new ArgumentNullException(nameof(element));
        }

        public IElement ById(string id)
        {
            return AppiumQuery.ById(id).FindElement(_element, _appiumApp);
        }

        public IElement ByClass(string className)
        {
            return AppiumQuery.ByClass(className).FindElement(_element, _appiumApp);
        }

        public IElement ByName(string name)
        {
            return AppiumQuery.ByName(name).FindElement(_element, _appiumApp);
        }

        public IElement ByAccessibilityId(string id)
        {
            return AppiumQuery.ByAccessibilityId(id).FindElement(_element, _appiumApp);
        }

        public IElement ByQuery(string query)
        {
            return new AppiumQuery(query).FindElement(_element, _appiumApp);
        }

        public virtual void Click()
        {
            _appiumApp.CommandExecutor.Execute("click", new Dictionary<string, object>()
            {
                { "element", _element }
            });
        }

        public virtual void SendKeys(string text)
        {
            _appiumApp.CommandExecutor.Execute("sendKeys", new Dictionary<string, object>()
            {
                { "element", _element },
                { "text", text }
            });
        }

        public virtual void Clear()
        {
            _appiumApp.CommandExecutor.Execute("clear", new Dictionary<string, object>()
            {
                { "element", _element },
            });
        }
    }
}
