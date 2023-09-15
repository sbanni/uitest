using NUnit.Framework;
using UITest.Appium.NUnit;
using UITest.Core;

namespace SampleTestLibrary
{
    [TestFixture(TestDevice.Android)]
    public class SampleBaseClassAndroid : UITestBase
    {
        public SampleBaseClassAndroid(TestDevice testDevice)
            : base(testDevice)
        {
        }

        public override IConfig GetTestConfig()
        {
            var apkPath = @"C:\maui\src\Controls\samples\Controls.Sample.UITests\bin\Release\net7.0-android\com.microsoft.maui.uitests-Signed.apk";
            var sampleAppToLaunch = Environment.GetEnvironmentVariable("SAMPLE_APP_EXE_PATH")
                ?? apkPath;
            var sampleAppId = Environment.GetEnvironmentVariable("SAMPLE_APP_ID")
                ?? "com.microsoft.maui.uitests";

            IConfig config = new Config();
            config.SetProperty("AppPath", sampleAppToLaunch);
            config.SetProperty("AppId", sampleAppId);
            config.SetProperty("EmulatorDeviceName", "MyTest2");
            return config;
        }
    }
}
