using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VulkaNet;

namespace VulkaNetDemos
{
    public class HelloTriangle : IDisposable
    {
        private struct QueueFamilyIndices
        {
            public int GraphicsFamily;
            public int PresentFamily;
        }

        private struct SwapChainSupportDetails
        {
            public VkSurfaceCapabilitiesKHR Capabilities;
            public IReadOnlyList<VkSurfaceFormatKHR> Formats;
            public IReadOnlyList<VkPresentModeKHR> PresentModes;
        };

        private const bool EnableValidationLayers = true;

        private static readonly string[] LayerNames = {"VK_LAYER_LUNARG_standard_validation"};
        private static readonly string[] DeviceExtensions = {"VK_KHR_swapchain"};

        private IVkGlobal vkGlobal;
        private Form form;
        private IVkInstance instance;
        private IVkDebugReportCallbackEXT callback;
        private IVkSurfaceKHR surface;
        private IVkPhysicalDevice physicalDevice;
        private IVkDevice device;
        private IVkQueue graphicsQueue;
        private IVkQueue presentQueue;
        private IVkSwapchainKHR swapChain;
        private IReadOnlyList<IVkImage> swapChainImages;
        private VkFormat swapchainImageFormat;
        private VkExtent2D swapChainExtent;
        private IReadOnlyList<IVkImageView> swapChainImageViews;

        public HelloTriangle(IVkGlobal vkGlobal, Form form)
        {
            this.vkGlobal = vkGlobal;
            this.form = form;
        }

        public void Init()
        {
            CreateInstance();
            SetupDebugCallback();
            CreateSurface();
            PickPhysicalDevice();
            CreateLogicalDevice();
            CreateSwapChain();
            CreateImageViews();
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
                EnabledLayerNames = LayerNames
            };
            instance = vkGlobal.CreateInstance(instanceCreateInfo, null).Object;
        }

        private void SetupDebugCallback()
        {
            if (!EnableValidationLayers)
                return;
            var createInfo = new VkDebugReportCallbackCreateInfoEXT
            {
                Flags = VkDebugReportFlagsEXT.Error | VkDebugReportFlagsEXT.Warning,
                Callback = DebugCallback
            };
            callback = instance.CreateDebugReportCallbackEXT(createInfo, null).Object;
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
            Console.WriteLine($"Validation layer: {message}");
            return VkDefines.VK_FALSE;
        }

        private void CreateSurface()
        {
            var createInfo = new VkWin32SurfaceCreateInfoKHR
            {
                Hwnd = form.Handle,
                Hinstance = Process.GetCurrentProcess().Handle
            };
            surface = instance.CreateWin32SurfaceKHR(createInfo, null).Object;
        }

        private void PickPhysicalDevice()
        {
            var devices = instance.PhysicalDevices;
            if (devices.Count == 0)
                throw new Exception("Failed to find GPUs with Vulkan support.");
            physicalDevice = devices.FirstOrDefault(x => IsDeviceSuitable(x, surface));
            if (physicalDevice == null)
                throw new Exception("Failed to find a suitable GPU.");
        }

        private static QueueFamilyIndices? FindQueueFamilies(IVkPhysicalDevice physicalDevice, IVkSurfaceKHR surface)
        {
            var graphicsFamily = physicalDevice.QueueFamilyProperties
                .Select((p, i) => new {p, i})
                .FirstOrDefault(x => x.p.QueueCount > 0 && (x.p.QueueFlags & VkQueueFlags.Graphics) != 0)
                ?.i;
            var presentFamily = physicalDevice.QueueFamilyProperties
                .Select((p, i) => new { p, i })
                .FirstOrDefault(x => x.p.QueueCount > 0 && physicalDevice.GetSurfaceSupportKHR(x.i, surface).Object)
                ?.i;

            return graphicsFamily.HasValue && presentFamily.HasValue ? new QueueFamilyIndices
            {
                GraphicsFamily = graphicsFamily.Value,
                PresentFamily = presentFamily.Value
            } : (QueueFamilyIndices?)null;
        }

        private static bool IsDeviceSuitable(IVkPhysicalDevice physicalDevice, IVkSurfaceKHR surface)
        {
            var extensionProps = physicalDevice.EnumerateDeviceExtensionProperties(null).Object;
            if (!DeviceExtensions.All(e => extensionProps.Any(p => p.ExtensionName == e)))
                return false;
            var swapChainSupportDetails = QuerySwapChainSupport(physicalDevice, surface);
            return
                swapChainSupportDetails.Formats.Any() &&
                swapChainSupportDetails.PresentModes.Any() &&
                FindQueueFamilies(physicalDevice, surface).HasValue;
        }

        private static SwapChainSupportDetails QuerySwapChainSupport(IVkPhysicalDevice physicalDevice, IVkSurfaceKHR surface)
        {
            return new SwapChainSupportDetails
            {
                Capabilities = physicalDevice.GetSurfaceCapabilitiesKHR(surface).Object,
                Formats = physicalDevice.GetSurfaceFormatsKHR(surface).Object,
                PresentModes = physicalDevice.GetSurfacePresentModesKHR(surface).Object
            };
        }

        private void CreateLogicalDevice()
        {
            var queueFamilies = FindQueueFamilies(physicalDevice, surface).Value;

            var queueCreateInfos = queueFamilies.GraphicsFamily == queueFamilies.PresentFamily
                ? new[]
                {   new VkDeviceQueueCreateInfo
                    {
                        QueueFamilyIndex = queueFamilies.GraphicsFamily,
                        QueuePriorities = new [] { 1f }
                    }
                }  
                : new [] 
                {
                    new VkDeviceQueueCreateInfo
                    {
                        QueueFamilyIndex = queueFamilies.GraphicsFamily,
                        QueuePriorities = new[] { 1f }
                    },
                    new VkDeviceQueueCreateInfo
                    {
                        QueueFamilyIndex = queueFamilies.PresentFamily,
                        QueuePriorities = new[] { 1f }
                    }
                };
            var deviceFeatures = new VkPhysicalDeviceFeatures
            {
                
            };
            var createInfo = new VkDeviceCreateInfo
            {
                QueueCreateInfos = queueCreateInfos,
                EnabledFeatures = deviceFeatures,
                EnabledExtensionNames = DeviceExtensions,
                EnabledLayerNames = LayerNames
            };
            device = physicalDevice.CreateDevice(createInfo, null).Object;
            graphicsQueue = device.GetDeviceQueue(queueFamilies.GraphicsFamily, 0);
            presentQueue = device.GetDeviceQueue(queueFamilies.PresentFamily, 0);
        }

        private void CreateSwapChain()
        {
            var swapChainSupport = QuerySwapChainSupport(physicalDevice, surface);
            var surfaceFormat = ChooseSwapSurfaceFormat(swapChainSupport.Formats);
            var presentMode = ChooseSwapPresentMode(swapChainSupport.PresentModes);
            var extent = ChooseSwapExtent(swapChainSupport.Capabilities);
            var minImages = swapChainSupport.Capabilities.MinImageCount;
            var maxImages = swapChainSupport.Capabilities.MaxImageCount > 0 
                ? swapChainSupport.Capabilities.MaxImageCount 
                : int.MaxValue;
            var imageCount = Math.Max(minImages, Math.Min(maxImages, 2));
            var indices = FindQueueFamilies(physicalDevice, surface).Value;
            var separateQueues = indices.GraphicsFamily != indices.PresentFamily;
            var createInfo = new VkSwapchainCreateInfoKHR
            {
                Surface = surface,
                MinImageCount = imageCount,
                ImageFormat = surfaceFormat.Format,
                ImageColorSpace = surfaceFormat.ColorSpace,
                ImageExtent = extent,
                ImageArrayLayers = 1,
                ImageUsage = VkImageUsageFlags.ColorAttachment,
                ImageSharingMode = separateQueues ? VkSharingMode.Concurrent : VkSharingMode.Exclusive,
                QueueFamilyIndices = separateQueues ? new[] {indices.GraphicsFamily, indices.PresentFamily} : null,
                PreTransform = swapChainSupport.Capabilities.CurrentTransform,
                CompositeAlpha = VkCompositeAlphaFlagsKHR.Opaque,
                PresentMode = presentMode,
                Clipped = true,
                OldSwapchain = null
            };
            swapChain = device.CreateSwapchainKHR(createInfo, null).Object;
            swapChainImages = swapChain.GetImagesKHR().Object;
            swapchainImageFormat = surfaceFormat.Format;
            swapChainExtent = extent;
        }

        private static VkSurfaceFormatKHR ChooseSwapSurfaceFormat(IReadOnlyList<VkSurfaceFormatKHR> availableFormats)
        {
            var preferred = new VkSurfaceFormatKHR
            {
                Format = VkFormat.B8G8R8A8_UNORM,
                ColorSpace = VkColorSpaceKHR.SRGB_NONLINEAR_KHR
            };
            if (availableFormats.Count == 1 && availableFormats[0].Format == VkFormat.Undefined)
                return preferred;
            if (availableFormats.Any(x => x.Format == preferred.Format && x.ColorSpace == preferred.ColorSpace))
                return preferred;
            return availableFormats.First();
        }

        private static VkPresentModeKHR ChooseSwapPresentMode(IReadOnlyList<VkPresentModeKHR> availableModes)
        {
            if (availableModes.Contains(VkPresentModeKHR.Mailbox))
                return VkPresentModeKHR.Mailbox;
            if (availableModes.Contains(VkPresentModeKHR.Fifo))
                return VkPresentModeKHR.Fifo;
            if (availableModes.Contains(VkPresentModeKHR.Immediate))
                return VkPresentModeKHR.Immediate;
            return availableModes.First();
        }

        private VkExtent2D ChooseSwapExtent(VkSurfaceCapabilitiesKHR capabilities)
        {
            const int special = -1;
            if (capabilities.CurrentExtent.Width != special)
                return capabilities.CurrentExtent;
            var width = Math.Max(capabilities.MinImageExtent.Width, Math.Min(capabilities.MaxImageExtent.Width, form.Width));
            var height = Math.Max(capabilities.MinImageExtent.Height, Math.Min(capabilities.MaxImageExtent.Height, form.Height));
            return new VkExtent2D(width, height);
        }

        private void CreateImageViews()
        {
            swapChainImageViews = swapChainImages.Select(x =>
            {
                var createInfo = new VkImageViewCreateInfo
                {
                    Image = x,
                    ViewType = VkImageViewType.Single2D,
                    Format = swapchainImageFormat,
                    Components = new VkComponentMapping
                    {
                        R = VkComponentSwizzle.Identity,
                        G = VkComponentSwizzle.Identity,
                        B = VkComponentSwizzle.Identity,
                        A = VkComponentSwizzle.Identity,
                    },
                    SubresourceRange = new VkImageSubresourceRange
                    {
                        AspectMask = VkImageAspectFlags.Color,
                        BaseMipLevel = 0,
                        LevelCount = 1,
                        BaseArrayLayer = 0,
                        LayerCount = 1
                    }
                };
                return device.CreateImageView(createInfo, null).Object;
            }).ToArray();
        }

        public void MainLop()
        {
            
        }
        
        public void Dispose()
        {
            foreach (var view in swapChainImageViews)
                view.Dispose();
            swapChain?.Dispose();
            device?.Dispose();
            callback?.Dispose();
            surface?.Dispose();
            instance?.Dispose();
        }
    }
}