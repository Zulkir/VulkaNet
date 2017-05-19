using System;
using System.Runtime.InteropServices;
using VulkaNet;

namespace VulkaNetDemos
{
    public class HelloTriangle : IDisposable
    {
        private IVkGlobal vkGlobal;
        private IVkInstance instance;

        public HelloTriangle(IVkGlobal vkGlobal)
        {
            this.vkGlobal = vkGlobal;
        }

        public void Init()
        {
            CreateInstance();
        }

        private void CreateInstance()
        {
            var appInfo = new VkApplicationInfo
            {
                ApplicationName = "Hello Triangle",
                ApplicationVersion = (uint)new VkApiVersion(1, 0, 0).Raw,
                EngineName = "No Engine",
                EngineVersion = (uint)new VkApiVersion(1, 0, 0).Raw,
                ApiVersion = new VkApiVersion(1, 0, 0)
            };
            var instanceCreateInfo = new VkInstanceCreateInfo
            {
                ApplicationInfo = appInfo,
                EnabledExtensionNames = new[]
                {
                    "VK_KHR_surface",
                    "VK_KHR_win32_surface",
                    VkDefines.VK_EXT_DEBUG_REPORT_EXTENSION_NAME
                },
                EnabledLayerNames = new[]
                {
                    "VK_LAYER_LUNARG_standard_validation"
                }
            };
            instance = vkGlobal.CreateInstance(instanceCreateInfo, null).Object;
        }

        private unsafe static VkBool32 DebugCallback(
            VkDebugReportFlagsEXT flags,
            VkDebugReportObjectTypeEXT objType,
            ulong obj,
            IntPtr location,
            int code,
            byte* layerPrefix,
            byte* msg,
            void* userData)
        {
            var message = Marshal.PtrToStringAnsi((IntPtr)msg);
            Console.WriteLine($"validation layer: {message}");
            return VkDefines.VK_FALSE;
        }

        public void MainLop()
        {
            
        }
        
        public void Dispose()
        {
            instance.Dispose();
        }
    }
}