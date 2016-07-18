#region License
/*
Copyright (c) 2016 VulkaNet Project - Daniil Rodin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

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
                    var commandPool = device.CreateCommandPool(new VkCommandPoolCreateInfo
                    {
                        Flags = VkCommandPoolCreateFlags.Transient | VkCommandPoolCreateFlags.ResetCommandBuffer,
                        QueueFamilyIndex = 0
                    }, null);
                }
            }

            Application.Run(new Form1());
        }
    }
}
