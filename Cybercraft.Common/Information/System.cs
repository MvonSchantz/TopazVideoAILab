using System;
using System.Management;

namespace Cybercraft.Common.Information
{
    public static class System
    {
        private static int systemMemoryMb = 0;
        public static int SystemMemoryMb
        {
            get
            {
                /*if (systemMemoryMb > 0)
                    return systemMemoryMb;
                var ci = new Microsoft.VisualBasic.Devices.ComputerInfo();
                systemMemoryMb = (int)(ci.TotalPhysicalMemory / (1024ul * 1024ul));
                return systemMemoryMb;*/

                if (systemMemoryMb > 0)
                    return systemMemoryMb;

                ulong memory = 0;
                foreach (var item in new ManagementObjectSearcher("Select * from Win32_PhysicalMemory").Get())
                {
                    memory += (ulong)item["Capacity"];
                }

                systemMemoryMb = (int)(memory / (1024 * 1024));
                return systemMemoryMb;
            }
        }

        public static int HardwareThreads => Environment.ProcessorCount;

        private static int hardwareCores = 0;
        public static int HardwareCores
        {
            get
            {
                if (hardwareCores > 0)
                    return hardwareCores;

                int coreCount = 0;
                foreach (var item in new ManagementObjectSearcher("Select * from Win32_Processor").Get())
                {
                    coreCount += int.Parse(item["NumberOfCores"].ToString());
                    cpuName = item["Name"].ToString();
                }

                hardwareCores = coreCount;
                return hardwareCores;
            }
        }

        private static string cpuName = null;
        public static string CpuName
        {
            get
            {
                if (cpuName != null)
                    return cpuName;

               int _ = HardwareCores;
                
                return cpuName;
            }
        }
    }
}
