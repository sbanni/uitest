using NUnit.Framework;
using UITest.Appium.NUnit;
using UITest.Core;

namespace SampleTestLibrary
{
    [TestFixture(TestDevice.Windows)]
    public class SampleBaseClass : UITestBase
    {
        public SampleBaseClass(TestDevice testDevice) 
            : base(testDevice)
        {
        }

        public override IConfig GetTestConfig()
        {
            var sampleAppToLaunch = Environment.GetEnvironmentVariable("SAMPLE_APP_EXE_PATH") 
                ?? @"C:\maui\src\Controls\samples\Controls.Sample.UITests\bin\Release\net7.0-windows10.0.20348\win10-x64\Controls.Sample.UITests.exe";
            var sampleAppId = Environment.GetEnvironmentVariable("SAMPLE_APP_ID") 
                ?? "com.microsoft.maui.uitests";

            IConfig config = new Config();
            config.SetProperty("AppPath", sampleAppToLaunch);
            config.SetProperty("AppId", sampleAppId);
            return config;
        }
    }
}
