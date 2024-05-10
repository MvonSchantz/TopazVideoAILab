using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Pri.LongPath;
using VideoProcessingLib.AviSynth;

namespace VideoProcessingLib.VirtualDub
{
    public enum VirtualDubVersion
    {
        // ReSharper disable InconsistentNaming
        x86,
        x64,
        // ReSharper restore InconsistentNaming
    }

    public static class VirtualDubProcess
    {
        private static string virtualDubPath32;
        private static string virtualDubPath64;

        public static void SetPath32(string path)
        {
            virtualDubPath32 = path;
        }

        public static void SetPath64(string path)
        {
            virtualDubPath64 = path;
        }

        public static void RunAvisynth(string inputAvsFile, string outputAviFile, ColorSpace colorSpace, VirtualDubVersion version = VirtualDubVersion.x64, ProcessPriorityClass priority = ProcessPriorityClass.Normal)
        {
            var jobsFile = Path.Combine(Path.GetTempPath(), "virtualdubjob_" + Guid.NewGuid() + ".jobs");
            try
            {
                VirtualDub.CreateJobsFile(jobsFile, inputAvsFile, outputAviFile);
                Lagarith.SetColorMode(colorSpace);
                RunJob(jobsFile, version, true, true, priority);
            }
            finally
            {
                File.Delete(jobsFile);
            }
        }

        public static Process RunJob(string jobsFile, VirtualDubVersion version = VirtualDubVersion.x64, bool wait = true, bool hidden = true, ProcessPriorityClass priority = ProcessPriorityClass.Normal)
        {
            var startInfo = new ProcessStartInfo(version == VirtualDubVersion.x86 ? virtualDubPath32 : virtualDubPath64,
                $@"/x /s""{jobsFile}""")
            {
                WindowStyle = ProcessWindowStyle.Minimized,
                //UseShellExecute = false,
            };
            if (hidden)
            {
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            var workingDirectory = Path.GetDirectoryName(jobsFile);
            if (workingDirectory != null)
            {
                startInfo.WorkingDirectory = workingDirectory;
            }
            var vd = Process.Start(startInfo);
            if (vd != null)
            {
                vd.PriorityClass = priority;
            }
            if (wait)
            {
                vd?.WaitForExit();
            }
            return vd;
        }

        public static void RunJobs(IEnumerable<string> jobsFiles, VirtualDubVersion version)
        {
            var tasks = new List<Task>();
            foreach (var jobsFile in jobsFiles)
            {
                string job = jobsFile;
                var task = new Task(() => RunJob(job, version, true, true), TaskCreationOptions.LongRunning);
                task.Start();
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }
    }
}
