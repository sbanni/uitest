using UITest.Appium;

public class AssemblySetupFixture : UITestContextSetupFixture
{
    AppiumServerContext? _appiumServerContext;

    public override void Initialize()
    {
        _appiumServerContext = new AppiumServerContext();
        _appiumServerContext.CreateAndStartServer();
        _serverContext = _appiumServerContext;
    }
}