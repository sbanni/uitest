﻿using System.Diagnostics;
using System.Text;

namespace UITest.Simulator
{
    public static class ToolRunner
    {
        public static string Run(
            string tool,
            string args,
            out int exitCode,
            string workingDirectory = "",
            int timeoutInSeconds = 600)
        {
            var info = new ProcessStartInfo(tool, args);

            if (Directory.Exists(workingDirectory))
                info.WorkingDirectory = workingDirectory;

            return Run(info, out exitCode, timeoutInSeconds);
        }

        public static string Run(
            ProcessStartInfo info,
            out int exitCode,
            int timeoutInSeconds = 600)
        {
            var procOutput = new StringBuilder();
            using (var p = new Process())
            {
                p.StartInfo = info;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;

                p.OutputDataReceived += (sender, o) =>
                {
                    if (!string.IsNullOrEmpty(o?.Data))
                    {
                        lock (procOutput)
                        {
                            procOutput.AppendLine(o.Data);
                        }
                    }
                };

                p.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e?.Data))
                    {
                        lock (procOutput)
                        {
                            procOutput.AppendLine(e.Data);
                        }
                    }
                };

                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                if (p.WaitForExit(timeoutInSeconds * 1000))
                {
                    exitCode = p.ExitCode;
                }
                else
                {
                    exitCode = -1;
                }
            }

            return procOutput.ToString();
        }
    }
}
