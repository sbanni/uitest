using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using UITest.Core;

namespace UITest.Appium
{
    public class AppiumAndroidApp : AppiumApp, IAndroidApp
    {
        public AppiumAndroidApp(Uri remoteAddress, IConfig config)
            : base(new AndroidDriver(remoteAddress, GetOptions(config)), config)
        {
        }

        public override IElementQueryable Query => new AppiumAndroidQueryable(this);

        public override ApplicationState AppState 
        {
            get
            {
                var appId = Config.GetProperty<string>("AppId") ?? throw new InvalidOperationException($"{nameof(AppState)} could not get the appid property");
                var state = _driver?.ExecuteScript("mobile: queryAppState", new Dictionary<string, object>
                        {
                            { "appId", appId },
                        });

                // https://github.com/appium/appium-uiautomator2-driver#mobile-queryappstate
                if (state == null)
                {
                    return ApplicationState.Unknown;
                }

                return Convert.ToInt32(state) switch
                {
                    0 => ApplicationState.Not_Installed,
                    1 => ApplicationState.Not_Running,
                    3 or
                    4 => ApplicationState.Running,
                    _ => ApplicationState.Unknown,
                };
            }
        }

        private static AppiumOptions GetOptions(IConfig config)
        {
            config.SetProperty("PlatformName", "Android");
            config.SetProperty("AutomationName", "UIAutomator2");

            var options = new AppiumOptions();
            SetGeneralAppiumOptions(config, options);
            return options;
        }
    }
}
