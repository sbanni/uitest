namespace UITest.Simulator
{
    //public static class AppleSimulator
    //{
    //    public static void InstallSimulator(string targetDevice)
    //    {
    //        var xcodePath = "";
    //        var cmdArgs = $"apple simulators install \"{targetDevice}\" --xcode={xcodePath}";
    //        _ = ToolRunner.Run(DevEnvironment.GetXHarnessExePath(), cmdArgs, out _);
    //    }

    //    public static string GetSimulatorUDID(string targetDevice)
    //    {
    //        var xcodePath = "";
    //        var cmdArgs = $"apple device \"{targetDevice}\" --xcode=\"{xcodePath}\"";
    //        return ToolRunner.Run(DevEnvironment.GetXHarnessExePath(), cmdArgs, out _, timeoutInSeconds: 30);
    //    }

    //    public static bool Launch(string id)
    //    {
    //        var cmdArgs = $"simctl boot {GetSimulatorUDID(id)}";
    //        _ = ToolRunner.Run("xcrun", cmdArgs, out int exitCode, timeoutInSeconds: 30);
    //        return exitCode == 0;
    //    }

    //    public static bool Shutdown(string id)
    //    {
    //        var cmdArgs = $"simctl shutdown {GetSimulatorUDID(id)}";
    //        _ = ToolRunner.Run("xcrun", cmdArgs, out int exitCode, timeoutInSeconds: 30);
    //        return exitCode == 0;
    //    }

    //    public static bool ShowWindow()
    //    {
    //        var cmdArgs = $"-a Simulator";
    //        _ = ToolRunner.Run("open", cmdArgs, out int exitCode, timeoutInSeconds: 30);
    //        return exitCode == 0;
    //    }
    //}
}
