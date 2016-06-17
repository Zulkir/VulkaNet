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
            var instance = global.CreateInstance(new VkInstanceCreateInfo
            {
                ApplicationInfo = new VkApplicationInfo
                {
                    ApplicationName = "VulkaNetDemos",
                    EngineName = "VulkaNetDemosEngine",
                }
            }, null).Object;
            var physicalDevices = instance.PhysicalDevices;
            
            Application.Run(new Form1());
        }
    }
}
