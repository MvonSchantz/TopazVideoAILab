using System.Collections.Generic;
using System.Diagnostics;

namespace Cybercraft.Common
{
    public static class Process
    {
        public static System.Diagnostics.Process Run(string path, string parameters, ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal, bool useShellExecute = true)
        {
            var startInfo = new ProcessStartInfo(path, parameters)
            {
                UseShellExecute = useShellExecute,
                WindowStyle = windowStyle,
            };

            var process = System.Diagnostics.Process.Start(startInfo);
            return process;
        }

        public static System.Diagnostics.Process Run(string path, string parameters, Dictionary<string, string> environmentVariables, ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal)
        {
            var startInfo = new ProcessStartInfo(path, parameters)
            {
                UseShellExecute = false,
                WindowStyle = windowStyle,
            };
            foreach (var environmentVariable in environmentVariables)
            {
                startInfo.EnvironmentVariables[environmentVariable.Key] = environmentVariable.Value;
            }

            var process = System.Diagnostics.Process.Start(startInfo);
            return process;
        }

        public static void RunAndWait(string path, string parameters, ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal)
        {
            var process = Run(path, parameters, windowStyle);
            process.WaitForExit();
        }
    }
}
