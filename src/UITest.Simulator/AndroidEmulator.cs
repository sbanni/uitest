using System.Diagnostics;

namespace UITest.Simulator
{
    public static class AndroidEmulator
    {
        const string DEFAULT_ABI = "x86_64";
        const string DEFAULT_IMAGE_TYPE = "google_apis_playstore";
        const string DEFAULT_DEVICE = "Nexus 5X";
        const int DEFAULT_API_LEVEL = 33;
        const int PORT = 5570;

        static readonly string AdbTool = Path.Combine(DevEnvironment.GetAndroidSdkPath(), "platform-tools", "adb");
        static readonly string AvdManagerTool = Path.Combine(DevEnvironment.GetAndroidCommandLineToolsPath(), DevEnvironment.IsWindows ? "avdmanager.bat" : "avdmanager");
        static readonly string SdkManagerTool = Path.Combine(DevEnvironment.GetAndroidCommandLineToolsPath(), DevEnvironment.IsWindows ? "sdkmanager.bat" : "sdkmanager");
        static readonly string EmulatorTool = Path.Combine(DevEnvironment.GetAndroidSdkPath(), "emulator", "emulator");

        public static void AvdCreate(
            string name,
            string abi = DEFAULT_ABI,
            int apiLevel = DEFAULT_API_LEVEL,
            string imageType = DEFAULT_IMAGE_TYPE,
            string device = DEFAULT_DEVICE,
            bool force = false)
        {
            // avdmanager.bat create avd -n MyTest2 -k system-images;android-33;google_apis_playstore;x86_64 -d "Nexus 5X"
            var systemImage = $"system-images;android-{apiLevel};{imageType};{abi}";
            var args = $"create avd -n \"{name}\" -k \"{systemImage}\" -d \"{device}\"";
            var avdListAvdsOutput = AvdListAvds();

            if (!force && avdListAvdsOutput.Contains(name))
            {
                return;
            }

            if (force)
            {
                args += " -f";
            }

            var avdCreateOutput = ToolRunner.Run(AvdManagerTool, args, out int avdCreateExitCode);
            Debug.WriteLine(avdCreateOutput);

            if (avdCreateExitCode != 0)
            {
                throw new Exception($"AvdCreate failed:\r\n {avdCreateOutput}");
            }

            var avdInstallOutput = AvdInstall(systemImage);
            Debug.WriteLine(avdInstallOutput);
        }

        public static string AvdInstall(string systemImage)
        {
            var args = $"\"{systemImage}\"";
            return ToolRunner.Run(SdkManagerTool, args, out _);
        }

        public static void AvdDelete(string name)
        {
            // delete avd -n MyTestDevice
            var args = $"delete avd -n {name}";
            _ = ToolRunner.Run(AvdManagerTool, args, out _);
        }

        public static string AvdListAvds()
        {
            var args = "list avd -c";
            return ToolRunner.Run(AvdManagerTool, args, out _);
        }

        public static string AvdListTargets()
        {
            var args = "list target -c";
            return ToolRunner.Run(AvdManagerTool, args, out _);
        }

        public static void StartEmulator(string name)
        {
            var launchArgs = $"-verbose -detect-image-hang -port {PORT} -avd {name} -no-boot-anim";
            // Emulator process does not stop once the emulator is running, end it after 15 seconds and then begin polling for boot success
            var startOutput = ToolRunner.Run(EmulatorTool, launchArgs, out _, timeoutInSeconds: 15);
            Debug.WriteLine(startOutput);

            var waitForEmulatorOutput = WaitForEmulator(720, $"emulator-{PORT}");
            Debug.WriteLine(waitForEmulatorOutput);
        }

        public static void InstallPackage(string apkPath, string pkgName, string outputDirectory)
        {
            // This will require xharness to be installed:
            // dotnet tool install Microsoft.DotNet.XHarness.CLI --global --add-source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json --version "8.0.0-prerelease*"
            var cmd = $"android install --app={apkPath} --package-name={pkgName} --verbosity=debug --output-directory={outputDirectory}";
            var installOutput = ToolRunner.Run(DevEnvironment.GetXHarnessExePath(), cmd, out _);
            Debug.WriteLine(installOutput);
        }

        public static void UninstallPackage(string pkgName)
        {
            var cmd = $"android uninstall --package-name={pkgName} --verbosity=debug";
            _ = ToolRunner.Run(DevEnvironment.GetXHarnessExePath(), cmd, out _);
        }

        public static bool WaitForEmulator(int timeout, string deviceId = "")
        {
            var maxWaitTime = DateTime.UtcNow.AddSeconds(timeout);
            int currentWaitTime = 0;
            bool bootCompleted = false;

            var args = "shell getprop sys.boot_completed";
            if (!string.IsNullOrEmpty(deviceId))
            {
                args = $"-s {deviceId} " + args;
            }

            while (DateTime.UtcNow < maxWaitTime && !bootCompleted)
            {
                Debug.WriteLine($"Waiting {currentWaitTime}/{timeout} seconds for the emulator to boot up...");
                var adbOutput = ToolRunner.Run(AdbTool, args, out _);
                if (int.TryParse(adbOutput, out int bootCompletedPropValue))
                {
                    bootCompleted = bootCompletedPropValue == 1;
                }
                Thread.Sleep(3000);
                currentWaitTime += 3;
            }

            if (bootCompleted)
            {
                _ = ToolRunner.Run(AdbTool, "shell setprop debug.mono.log default,mono_log_level=debug,mono_log_mask=all", out _);
            }

            return bootCompleted;
        }
    }
}
