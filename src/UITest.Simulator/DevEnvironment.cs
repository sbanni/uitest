using System.Runtime.InteropServices;

namespace UITest.Simulator
{
    internal static class DevEnvironment
    {
        const string SDK_ROOT_ENV = "ANDROID_SDK_ROOT";
        const string HOME_ENV = "ANDROID_HOME";
        const string XHARNESS_NAME = "xharness";

        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static string GetXHarnessExePath()
        {
            var dotnetToolUserPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".dotnet", "tools");
            var xharnessToolPath = Path.Combine(dotnetToolUserPath, DevEnvironment.IsWindows ? $"{XHARNESS_NAME}.exe" : XHARNESS_NAME);
            var xharnessTool = File.Exists(xharnessToolPath) ? xharnessToolPath : XHARNESS_NAME;
            return xharnessTool;
        }

        public static string GetAndroidSdkPath()
        {
            var sdkPath = Environment.GetEnvironmentVariable(SDK_ROOT_ENV);
            if (string.IsNullOrEmpty(sdkPath))
            {
                sdkPath = Environment.GetEnvironmentVariable(HOME_ENV);
            }

            if (string.IsNullOrEmpty(sdkPath))
            {
                sdkPath = IsWindows
                    ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Android", "android-sdk")
                    : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library", "Developer", "Xamarin", "android-sdk-macosx");
            }

            return sdkPath;
        }

        public static string GetAndroidCommandLineToolsPath()
        {
            var sdkPath = GetAndroidSdkPath();
            var toolsPath = Path.Combine(sdkPath, "cmdline-tools", "latest", "bin");
            if (!Directory.Exists(toolsPath))
            {
                var toolsTopDir = Path.Combine(sdkPath, "cmdline-tools");
                if (Directory.Exists(toolsTopDir))
                {
                    var versionedDirectories = Directory.EnumerateDirectories(toolsTopDir).Where(d => Version.TryParse(Path.GetFileName(d), out Version? v));
                    var latestVersionDir = versionedDirectories.OrderByDescending(d => Version.Parse(Path.GetFileName(d))).FirstOrDefault();
                    if (Directory.Exists(latestVersionDir))
                        toolsPath = Path.Combine(latestVersionDir, "bin");
                }
            }

            return toolsPath;
        }
    }
}
