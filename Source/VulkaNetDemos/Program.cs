using System;
using System.Windows.Forms;
using VulkaNet;

namespace VulkaNetDemos
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Run();
        }

        public static void Run()
        {
            var global = new VkGlobal();
            var instanceCreateInfo = new VkInstanceCreateInfo
            {
                ApplicationInfo = new VkApplicationInfo
                {
                    ApplicationName = "VulkaNetDemos",
                    EngineName = "VulkaNetDemosEngine",
                }
            };
            using (var instance = global.CreateInstance(instanceCreateInfo, null).Object)
            {
                var physicalDevices = instance.PhysicalDevices;
                var deviceCreateInfo = new VkDeviceCreateInfo
                {
                    QueueCreateInfos = new[]
                    {
                        new VkDeviceQueueCreateInfo
                        {
                            QueueFamilyIndex = 0,
                            QueuePriorities = new[]{ 1.0f }
                        }
                    }
                };
                using (var device = physicalDevices[0].CreateDevice(deviceCreateInfo, null).Object)
                {
                    device.WaitIdle();
                }
            }

            Application.Run(new Form1());
        }
    }
}
