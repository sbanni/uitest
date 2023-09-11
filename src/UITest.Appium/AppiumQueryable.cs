using UITest.Core;

namespace UITest.Appium
{
    public class AppiumQueryable : IElementQueryable
    {
        protected readonly AppiumApp _appiumApp;

        public AppiumQueryable(AppiumApp appiumApp)
        {
            _appiumApp = appiumApp ?? throw new ArgumentNullException(nameof(appiumApp));
        }

        public virtual IElement ById(string id)
        {
            return AppiumQuery.ById(id).FindElement(_appiumApp);
        }

        public virtual IElement ByAccessibilityId(string name)
        {
            return AppiumQuery.ByAccessibilityId(name).FindElement(_appiumApp);
        }

        public virtual IElement ByClass(string className)
        {
            return AppiumQuery.ByClass(className).FindElement(_appiumApp);
        }

        public virtual IElement ByName(string name)
        {
            return AppiumQuery.ByName(name).FindElement(_appiumApp);
        }

        public virtual IElement ByQuery(string query)
        {
            var appiumQuery = new AppiumQuery(query);
            return appiumQuery.FindElement(_appiumApp);
        }
    }
}
