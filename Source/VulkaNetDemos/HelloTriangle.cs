using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using VulkaNet;

namespace VulkaNetDemos
{
    public class HelloTriangle : IDisposable
    {
        private readonly Form1 form;
        private readonly IVkGlobal vkGlobal;
        private IVkInstance instance;
        private IVkDebugReportCallbackEXT callback;
        private IVkPhysicalDevice physicalDevice;
        private IVkDevice device;
        private IVkQueue graphicsQueue;
        private IVkQueue presentQueue;
        private IVkSurfaceKHR surface;

        public HelloTriangle(Form1 form, IVkGlobal vkGlobal)
        {
            this.form = form;
            this.vkGlobal = vkGlobal;
        }

        public void Dispose()
        {
            device.Dispose();
            callback.Dispose();
            surface.Dispose();
            instance.Dispose();
        }

        public void Init()
        {
            CreateInstance();
            SetupDebugCallback();
            CreateSurface();
            PickPhysicalDevice();
            CreateLogicalDevice();
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

        private void SetupDebugCallback()
        {
            var callbackCreateInfo = new VkDebugReportCallbackCreateInfoEXT
            {
                Flags = VkDebugReportFlagsEXT.Error | VkDebugReportFlagsEXT.Warning | VkDebugReportFlagsEXT.PerformanceWarning,
                Callback = DebugCallback
            };
            callback = instance.CreateDebugReportCallbackEXT(callbackCreateInfo, null).Object;
        }

        private static VkBool32 DebugCallback(
            VkDebugReportFlagsEXT flags,
            VkDebugReportObjectTypeEXT objType,
            ulong obj,
            IntPtr location,
            int code,
            IntPtr layerPrefix,
            IntPtr msg,
            IntPtr userData)
        {
            var message = Marshal.PtrToStringAnsi(msg);
            Console.WriteLine(message);
            return VkDefines.VK_FALSE;
        }

        private void PickPhysicalDevice()
        {
            physicalDevice = instance.PhysicalDevices.First(x => x.QueueFamilyProperties.Any(y => (y.QueueFlags & VkQueueFlags.Graphics) != 0));
        }

        private void CreateLogicalDevice()
        {
            var queueFamilyIndex = physicalDevice.QueueFamilyProperties.Select((q, i) => new {q, i}).First(x => (x.q.QueueFlags & VkQueueFlags.Graphics) != 0).i;
            var queueCreateInfo = new VkDeviceQueueCreateInfo
            {
                QueueFamilyIndex = queueFamilyIndex,
                QueuePriorities = new [] { 1f }
            };
            var deviceFeatures = new VkPhysicalDeviceFeatures();
            var deviceCreateInfo = new VkDeviceCreateInfo
            {
                QueueCreateInfos = new[] {queueCreateInfo},
                EnabledFeatures = deviceFeatures,
                //EnabledExtensionNames = new[]
                //{
                //    "VK_KHR_surface",
                //    "VK_KHR_win32_surface",
                //    VkDefines.VK_EXT_DEBUG_REPORT_EXTENSION_NAME
                //},
            };
            device = physicalDevice.CreateDevice(deviceCreateInfo, null).Object;
            graphicsQueue = device.GetDeviceQueue(queueFamilyIndex, 0);
            presentQueue = device.GetDeviceQueue(queueFamilyIndex, 0);
        }

        private void CreateSurface()
        {
            var surfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR
            {
                Hwnd = form.Handle,
                Hinstance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().ManifestModule)
            };
            surface = instance.CreateWin32SurfaceKHR(surfaceCreateInfo, null).Object;
        }

        public void MainLop()
        {
            
        }
    }
}