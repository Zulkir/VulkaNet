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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace VulkaNet
{
    public interface IVkPhysicalDevice : IVkInstanceChild
    {
        VkPhysicalDeviceProperties Properties { get; }
        IReadOnlyList<IVkQueueFamilyProperties> QueueFamilyProperties { get; }
        VkPhysicalDeviceFeatures Features { get; }
        IVkPhysicalDeviceMemoryProperties MemoryProperties { get; }
        IReadOnlyList<IVkDisplayKHRAggregate> DisplayAggregatesKHR { get; }
        VkObjectResult<IVkDevice> CreateDevice(VkDeviceCreateInfo createInfo, IVkAllocationCallbacks allocator);
        IReadOnlyList<VkSparseImageFormatProperties> GetSparseImageFormatProperties(VkFormat format, VkImageType type, VkSampleCountFlagBits samples, VkImageUsageFlags usage, VkImageTiling tiling);
        IVkDisplayKHR GetDisplay(VkDisplayKHR.HandleType handle);
        IVkDisplayModeKHR GetDisplayMode(VkDisplayModeKHR.HandleType handle);
        VkObjectResult<IVkDisplayModeKHR> CreateDisplayMode(IVkDisplayKHR display, VkDisplayModeCreateInfoKHR createInfo, IVkAllocationCallbacks allocator);
        // todo: cache this like in VkGlobal
        VkObjectResult<IReadOnlyList<IVkExtensionProperties>> EnumerateDeviceExtensionProperties(string layerName);
        VkObjectResult<bool> GetSurfaceSupportKHR(int queueFamiltyIndex, IVkSurfaceKHR surface);
        bool GetMirPresentationSupportKHR(int queueFamilyIndex, IntPtr connection);
        bool GetWaylandPresentationSupportKHR(int queueFamilyIndex, IntPtr display);
        bool GetWin32PresentationSupportKHR(int queueFamilyIndex);
        bool GetXcbPresentationSupportKHR(int queueFamilyIndex, IntPtr connection, int visual_id);
        bool GetXlibPresentationSupportKHR(int queueFamilyIndex, IntPtr dpy, IntPtr visualID);
        VkObjectResult<VkSurfaceCapabilitiesKHR> GetSurfaceCapabilitiesKHR(IVkSurfaceKHR surface);
        VkObjectResult<IReadOnlyList<VkSurfaceFormatKHR>> GetSurfaceFormatsKHR(IVkSurfaceKHR surface);
        VkObjectResult<IReadOnlyList<VkPresentModeKHR>> GetSurfacePresentModesKHR(IVkSurfaceKHR surface);
    }

    public unsafe class VkPhysicalDevice : IVkPhysicalDevice
    {
        public IVkInstance Instance { get; }
        public IntPtr Handle { get; }
        public DirectFunctions Direct { get; }
        public VkPhysicalDeviceProperties Properties { get; }
        public IReadOnlyList<IVkQueueFamilyProperties> QueueFamilyProperties { get; }
        public VkPhysicalDeviceFeatures Features { get; }
        public IVkPhysicalDeviceMemoryProperties MemoryProperties { get; }
        public IReadOnlyList<IVkDisplayKHRAggregate> DisplayAggregatesKHR { get; }

        private ConcurrentDictionary<VkDisplayKHR.HandleType, IVkDisplayKHR> displays = new ConcurrentDictionary<VkDisplayKHR.HandleType, IVkDisplayKHR>();
        private ConcurrentDictionary<VkDisplayModeKHR.HandleType, IVkDisplayModeKHR> displayModes = new ConcurrentDictionary<VkDisplayModeKHR.HandleType, IVkDisplayModeKHR>();

        public VkPhysicalDevice(IVkInstance instance, IntPtr handle)
        {
            Instance = instance;
            Handle = handle;
            Direct = new DirectFunctions(instance);
            Properties = GetPhysicalDeviceProperties();
            QueueFamilyProperties = GetPhysicalDeviceQueueFamilyProperties();
            Features = GetPhysicalDeviceFeatures();
            MemoryProperties = GetPhysicalDeviceMemoryProperties();
            DisplayAggregatesKHR = GetDisplayAggregatesKHR();
        }

        // todo: move to Instance
        // todo: use VkPhysicalDevice.HandleType
        public class DirectFunctions
        {
            public GetPhysicalDevicePropertiesDelegate GetPhysicalDeviceProperties { get; }
            public delegate VkResult GetPhysicalDevicePropertiesDelegate(
                IntPtr physicalDevice,
                VkPhysicalDeviceProperties.Raw* pProperties);

            public GetPhysicalDeviceQueueFamilyPropertiesDelegate GetPhysicalDeviceQueueFamilyProperties { get; }
            public delegate void GetPhysicalDeviceQueueFamilyPropertiesDelegate(
                IntPtr physicalDevice,
                int* pQueueFamilyPropertyCount,
                VkQueueFamilyProperties.Raw* pQueueFamilyProperties);

            public CreateDeviceDelegate CreateDevice { get; }
            public delegate VkResult CreateDeviceDelegate(
                IntPtr physicalDevice,
                VkDeviceCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkDevice.HandleType* pDevice);

            public GetPhysicalDeviceFeaturesDelegate GetPhysicalDeviceFeatures { get; }
            public delegate void GetPhysicalDeviceFeaturesDelegate(
                IntPtr physicalDevice,
                VkPhysicalDeviceFeatures.Raw* pFeatures);

            public GetPhysicalDeviceMemoryPropertiesDelegate GetPhysicalDeviceMemoryProperties { get; }
            public delegate void GetPhysicalDeviceMemoryPropertiesDelegate(
                IntPtr physicalDevice,
                VkPhysicalDeviceMemoryProperties.Raw* pMemoryProperties);

            public GetPhysicalDeviceSparseImageFormatPropertiesDelegate GetPhysicalDeviceSparseImageFormatProperties { get; }
            public delegate void GetPhysicalDeviceSparseImageFormatPropertiesDelegate(
                IntPtr physicalDevice,
                VkFormat format,
                VkImageType type,
                VkSampleCountFlagBits samples,
                VkImageUsageFlags usage,
                VkImageTiling tiling,
                int* pPropertyCount,
                VkSparseImageFormatProperties* pProperties);

            public GetPhysicalDeviceDisplayPropertiesKHRDelegate GetPhysicalDeviceDisplayPropertiesKHR { get; }
            public delegate VkResult GetPhysicalDeviceDisplayPropertiesKHRDelegate(
                IntPtr physicalDevice,
                int* pPropertyCount,
                VkDisplayPropertiesKHR.Raw* pProperties);

            public GetPhysicalDeviceDisplayPlanePropertiesKHRDelegate GetPhysicalDeviceDisplayPlanePropertiesKHR { get; }
            public delegate VkResult GetPhysicalDeviceDisplayPlanePropertiesKHRDelegate(
                IntPtr physicalDevice,
                int* pPropertyCount,
                VkDisplayPlanePropertiesKHR.Raw* pProperties);

            public GetDisplayPlaneSupportedDisplaysKHRDelegate GetDisplayPlaneSupportedDisplaysKHR { get; }
            public delegate VkResult GetDisplayPlaneSupportedDisplaysKHRDelegate(
                IntPtr physicalDevice,
                int planeIndex,
                int* pDisplayCount,
                VkDisplayKHR.HandleType* pDisplays);

            public GetDisplayModePropertiesKHRDelegate GetDisplayModePropertiesKHR { get; }
            public delegate VkResult GetDisplayModePropertiesKHRDelegate(
                IntPtr physicalDevice,
                VkDisplayKHR.HandleType display,
                int* pPropertyCount,
                VkDisplayModePropertiesKHR.Raw* pProperties);

            public CreateDisplayModeKHRDelegate CreateDisplayModeKHR { get; }
            public delegate VkResult CreateDisplayModeKHRDelegate(
                IntPtr physicalDevice,
                VkDisplayKHR.HandleType display,
                VkDisplayModeCreateInfoKHR.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkDisplayModeKHR.HandleType* pMode);
            
            public GetDisplayPlaneCapabilitiesKHRDelegate GetDisplayPlaneCapabilitiesKHR { get; }
            public delegate VkResult GetDisplayPlaneCapabilitiesKHRDelegate(
                IntPtr physicalDevice,
                VkDisplayModeKHR.HandleType mode,
                int planeIndex,
                VkDisplayPlaneCapabilitiesKHR* pCapabilities);

            public GetPhysicalDeviceSurfaceSupportKHRDelegate GetPhysicalDeviceSurfaceSupportKHR { get; }
            public delegate VkResult GetPhysicalDeviceSurfaceSupportKHRDelegate(
                IntPtr physicalDevice,
                int queueFamilyIndex,
                VkSurfaceKHR.HandleType surface,
                VkBool32* pSupported);

            public GetPhysicalDeviceMirPresentationSupportKHRDelegate GetPhysicalDeviceMirPresentationSupportKHR { get; }
            public delegate VkBool32 GetPhysicalDeviceMirPresentationSupportKHRDelegate(
                IntPtr physicalDevice,
                int queueFamilyIndex,
                IntPtr connection);

            public GetPhysicalDeviceWaylandPresentationSupportKHRDelegate GetPhysicalDeviceWaylandPresentationSupportKHR { get; }
            public delegate VkBool32 GetPhysicalDeviceWaylandPresentationSupportKHRDelegate(
                IntPtr physicalDevice,
                int queueFamilyIndex,
                IntPtr display);

            public GetPhysicalDeviceWin32PresentationSupportKHRDelegate GetPhysicalDeviceWin32PresentationSupportKHR { get; }
            public delegate VkBool32 GetPhysicalDeviceWin32PresentationSupportKHRDelegate(
                IntPtr physicalDevice,
                int queueFamilyIndex);

            public GetPhysicalDeviceXcbPresentationSupportKHRDelegate GetPhysicalDeviceXcbPresentationSupportKHR { get; }
            public delegate VkBool32 GetPhysicalDeviceXcbPresentationSupportKHRDelegate(
                IntPtr physicalDevice,
                int queueFamilyIndex,
                IntPtr connection,
                int visual_id);

            public GetPhysicalDeviceXlibPresentationSupportKHRDelegate GetPhysicalDeviceXlibPresentationSupportKHR { get; }
            public delegate VkBool32 GetPhysicalDeviceXlibPresentationSupportKHRDelegate(
                IntPtr physicalDevice,
                int queueFamilyIndex,
                IntPtr dpy,
                IntPtr visualID);

            public GetPhysicalDeviceSurfaceCapabilitiesKHRDelegate GetPhysicalDeviceSurfaceCapabilitiesKHR { get; }
            public delegate VkResult GetPhysicalDeviceSurfaceCapabilitiesKHRDelegate(
                IntPtr physicalDevice,
                VkSurfaceKHR.HandleType surface,
                VkSurfaceCapabilitiesKHR* pSurfaceCapabilities);

            public GetPhysicalDeviceSurfaceFormatsKHRDelegate GetPhysicalDeviceSurfaceFormatsKHR { get; }
            public delegate VkResult GetPhysicalDeviceSurfaceFormatsKHRDelegate(
                IntPtr physicalDevice,
                VkSurfaceKHR.HandleType surface,
                int* pSurfaceFormatCount,
                VkSurfaceFormatKHR* pSurfaceFormats);

            public GetPhysicalDeviceSurfacePresentModesKHRDelegate GetPhysicalDeviceSurfacePresentModesKHR { get; }
            public delegate VkResult GetPhysicalDeviceSurfacePresentModesKHRDelegate(
                IntPtr physicalDevice,
                VkSurfaceKHR.HandleType surface,
                int* pPresentModeCount,
                VkPresentModeKHR* pPresentModes);

            public EnumerateDeviceExtensionPropertiesDelegate EnumerateDeviceExtensionProperties { get; }
            public delegate VkResult EnumerateDeviceExtensionPropertiesDelegate(
                IntPtr physicalDevice,
                byte* pLayerName,
                int* pPropertyCount,
                VkExtensionProperties.Raw* pProperties);

            public DirectFunctions(IVkInstance instance)
            {
                GetPhysicalDeviceProperties = VkHelpers.GetInstanceDelegate<GetPhysicalDevicePropertiesDelegate>(instance, "vkGetPhysicalDeviceProperties");
                GetPhysicalDeviceQueueFamilyProperties = VkHelpers.GetInstanceDelegate<GetPhysicalDeviceQueueFamilyPropertiesDelegate>(instance, "vkGetPhysicalDeviceQueueFamilyProperties");
                CreateDevice =  VkHelpers.GetInstanceDelegate<CreateDeviceDelegate>(instance, "vkCreateDevice");
                GetPhysicalDeviceFeatures = VkHelpers.GetInstanceDelegate<GetPhysicalDeviceFeaturesDelegate>(instance, "vkGetPhysicalDeviceFeatures");
                GetPhysicalDeviceMemoryProperties = VkHelpers.GetInstanceDelegate<GetPhysicalDeviceMemoryPropertiesDelegate>(instance, "vkGetPhysicalDeviceMemoryProperties");
                GetPhysicalDeviceSparseImageFormatProperties = VkHelpers.GetInstanceDelegate<GetPhysicalDeviceSparseImageFormatPropertiesDelegate>(instance, "vkGetPhysicalDeviceSparseImageFormatProperties");
                GetPhysicalDeviceDisplayPropertiesKHR = VkHelpers.GetInstanceDelegate<GetPhysicalDeviceDisplayPropertiesKHRDelegate>(instance, "vkGetPhysicalDeviceDisplayPropertiesKHR");
                GetPhysicalDeviceDisplayPlanePropertiesKHR = VkHelpers.GetInstanceDelegate<GetPhysicalDeviceDisplayPlanePropertiesKHRDelegate>(instance, "vkGetPhysicalDeviceDisplayPlanePropertiesKHR");
                GetDisplayPlaneSupportedDisplaysKHR = VkHelpers.GetInstanceDelegate<GetDisplayPlaneSupportedDisplaysKHRDelegate>(instance, "vkGetDisplayPlaneSupportedDisplaysKHR");
                GetDisplayModePropertiesKHR = VkHelpers.GetInstanceDelegate<GetDisplayModePropertiesKHRDelegate>(instance, "vkGetDisplayModePropertiesKHR");
                CreateDisplayModeKHR = VkHelpers.GetInstanceDelegate<CreateDisplayModeKHRDelegate>(instance, "vkCreateDisplayModeKHR");
                GetDisplayPlaneCapabilitiesKHR = VkHelpers.GetInstanceDelegate<GetDisplayPlaneCapabilitiesKHRDelegate>(instance, "vkGetDisplayPlaneCapabilitiesKHR");
                GetPhysicalDeviceSurfaceSupportKHR = VkHelpers.GetInstanceDelegate<GetPhysicalDeviceSurfaceSupportKHRDelegate>(instance, "vkGetPhysicalDeviceSurfaceSupportKHR");
                GetPhysicalDeviceMirPresentationSupportKHR = VkHelpers.GetInstanceDelegate<GetPhysicalDeviceMirPresentationSupportKHRDelegate>(instance, "vkGetPhysicalDeviceMirPresentationSupportKHR");
                GetPhysicalDeviceWaylandPresentationSupportKHR = VkHelpers.GetInstanceDelegate<GetPhysicalDeviceWaylandPresentationSupportKHRDelegate>(instance, "vkGetPhysicalDeviceWaylandPresentationSupportKHR");
                GetPhysicalDeviceWin32PresentationSupportKHR = VkHelpers.GetInstanceDelegate<GetPhysicalDeviceWin32PresentationSupportKHRDelegate>(instance, "vkGetPhysicalDeviceWin32PresentationSupportKHR");
                GetPhysicalDeviceXcbPresentationSupportKHR = VkHelpers.GetInstanceDelegate<GetPhysicalDeviceXcbPresentationSupportKHRDelegate>(instance, "vkGetPhysicalDeviceXcbPresentationSupportKHR");
                GetPhysicalDeviceXlibPresentationSupportKHR = VkHelpers.GetInstanceDelegate<GetPhysicalDeviceXlibPresentationSupportKHRDelegate>(instance, "vkGetPhysicalDeviceXlibPresentationSupportKHR");
                GetPhysicalDeviceSurfaceCapabilitiesKHR = VkHelpers.GetInstanceDelegate<GetPhysicalDeviceSurfaceCapabilitiesKHRDelegate>(instance, "vkGetPhysicalDeviceSurfaceCapabilitiesKHR");
                GetPhysicalDeviceSurfaceFormatsKHR = VkHelpers.GetInstanceDelegate<GetPhysicalDeviceSurfaceFormatsKHRDelegate>(instance, "vkGetPhysicalDeviceSurfaceFormatsKHR");
                GetPhysicalDeviceSurfacePresentModesKHR = VkHelpers.GetInstanceDelegate<GetPhysicalDeviceSurfacePresentModesKHRDelegate>(instance, "vkGetPhysicalDeviceSurfacePresentModesKHR");
                EnumerateDeviceExtensionProperties = VkHelpers.GetInstanceDelegate<EnumerateDeviceExtensionPropertiesDelegate>(instance, "vkEnumerateDeviceExtensionProperties");
            }
        }

        public IVkDisplayKHR GetDisplay(VkDisplayKHR.HandleType handle) => 
            displays.GetOrAdd(handle, h => new VkDisplayKHR(Instance, h));

        public IVkDisplayModeKHR GetDisplayMode(VkDisplayModeKHR.HandleType handle) => 
            displayModes.GetOrAdd(handle, h => new VkDisplayModeKHR(Instance, h));

        private VkPhysicalDeviceProperties GetPhysicalDeviceProperties()
        {
            VkPhysicalDeviceProperties.Raw raw;
            Direct.GetPhysicalDeviceProperties(Handle, &raw);
            return new VkPhysicalDeviceProperties(&raw);
        }

        private IReadOnlyList<IVkQueueFamilyProperties> GetPhysicalDeviceQueueFamilyProperties()
        {
            int count;
            Direct.GetPhysicalDeviceQueueFamilyProperties(Handle, &count, (VkQueueFamilyProperties.Raw*)0);
            var rawArray = new VkQueueFamilyProperties.Raw[count];
            fixed (VkQueueFamilyProperties.Raw* pRawArray = rawArray)
            {
                Direct.GetPhysicalDeviceQueueFamilyProperties(Handle, &count, pRawArray);
                return rawArray.Select(x => new VkQueueFamilyProperties(&x)).ToArray();
            }
        }
        
        public VkObjectResult<IVkDevice> CreateDevice(VkDeviceCreateInfo createInfo, IVkAllocationCallbacks allocator)
        {
            var size =
                createInfo.SizeOfMarshalDirect() +
                allocator.SizeOfMarshalIndirect();
            return VkHelpers.RunWithUnamangedData(size, u => CreateDevice(u, createInfo, allocator));
        }

        private VkObjectResult<IVkDevice> CreateDevice(IntPtr data, VkDeviceCreateInfo createInfo, IVkAllocationCallbacks allocator)
        {
            var unmanaged = (byte*)data;
            var createInfoRaw = createInfo.MarshalDirect(ref unmanaged);
            var pAllocator = allocator.MarshalIndirect(ref unmanaged);
            VkDevice.HandleType handle;
            var result = Direct.CreateDevice(Handle, &createInfoRaw, pAllocator, &handle);
            var device = result == VkResult.Success ? new VkDevice(this, handle, allocator) : null;
            return new VkObjectResult<IVkDevice>(result, device);
        }

        private VkPhysicalDeviceFeatures GetPhysicalDeviceFeatures()
        {
            VkPhysicalDeviceFeatures.Raw raw;
            Direct.GetPhysicalDeviceFeatures(Handle, &raw);
            return new VkPhysicalDeviceFeatures(&raw);
        }

        private IVkPhysicalDeviceMemoryProperties GetPhysicalDeviceMemoryProperties()
        {
            VkPhysicalDeviceMemoryProperties.Raw raw;
            Direct.GetPhysicalDeviceMemoryProperties(Handle, &raw);
            return new VkPhysicalDeviceMemoryProperties(&raw);
        }

        public IReadOnlyList<VkSparseImageFormatProperties> GetSparseImageFormatProperties(VkFormat format, VkImageType type, VkSampleCountFlagBits samples, VkImageUsageFlags usage,
            VkImageTiling tiling)
        {
            int count;
            Direct.GetPhysicalDeviceSparseImageFormatProperties(Handle, format, type, samples, usage, tiling, &count, (VkSparseImageFormatProperties*)0);
            var resultArray = new VkSparseImageFormatProperties[count];
            fixed (VkSparseImageFormatProperties* pResultArray = resultArray)
            {
                Direct.GetPhysicalDeviceSparseImageFormatProperties(Handle, format, type, samples, usage, tiling, &count, pResultArray);
                return resultArray;
            }
        }

        private IReadOnlyList<IVkDisplayKHRAggregate> GetDisplayAggregatesKHR()
        {
            return
                (from displayProps in GetDisplayPropertiesKHR()
                 let modes = 
                        (from modeProps in GetDisplayModePropertiesKHR(displayProps.Display)
                         let planeCaps = 
                                (from planeProps in GetDisplayPlanePropertiesKHR()
                                 select GetDisplayPlaneCapabilitiesKHR(modeProps.DisplayMode, planeProps.CurrentStackIndex))
                                .ToArray()
                         select new VkDisplayModeKHRAggregate(modeProps.DisplayMode, modeProps.Parameters, planeCaps))
                        .ToArray()
                 select new VkDisplayKHRAggregate(displayProps.Display, displayProps, modes))
                .ToArray();
        }

        private IReadOnlyList<VkDisplayPropertiesKHR> GetDisplayPropertiesKHR()
        {
            if (Direct.GetPhysicalDeviceDisplayPropertiesKHR == null)
                return new VkDisplayPropertiesKHR[0];
            int count;
            Direct.GetPhysicalDeviceDisplayPropertiesKHR(Handle, &count, (VkDisplayPropertiesKHR.Raw*)0);
            var resultArray = new VkDisplayPropertiesKHR.Raw[count];
            fixed (VkDisplayPropertiesKHR.Raw* pResultArray = resultArray)
            {
                Direct.GetPhysicalDeviceDisplayPropertiesKHR(Handle, &count, pResultArray);
                return resultArray.Select(x => new VkDisplayPropertiesKHR(&x, this)).ToArray();
            }
        }

        private IReadOnlyList<VkDisplayPlanePropertiesKHR> GetDisplayPlanePropertiesKHR()
        {
            if (Direct.GetPhysicalDeviceDisplayPlanePropertiesKHR == null)
                return new VkDisplayPlanePropertiesKHR[0];
            int count;
            Direct.GetPhysicalDeviceDisplayPlanePropertiesKHR(Handle, &count, (VkDisplayPlanePropertiesKHR.Raw*)0);
            var resultArray = new VkDisplayPlanePropertiesKHR.Raw[count];
            fixed (VkDisplayPlanePropertiesKHR.Raw* pResultArray = resultArray)
            {
                Direct.GetPhysicalDeviceDisplayPlanePropertiesKHR(Handle, &count, pResultArray);
                return resultArray.Select(x => new VkDisplayPlanePropertiesKHR(&x, this)).ToArray();
            }
        }

        private IReadOnlyList<IVkDisplayKHR> GetDisplayPlaneSupportedDisplaysKHR(int planeIndex)
        {
            if (Direct.GetDisplayPlaneSupportedDisplaysKHR == null)
                return new IVkDisplayKHR[0];
            int count;
            Direct.GetDisplayPlaneSupportedDisplaysKHR(Handle, planeIndex, &count, (VkDisplayKHR.HandleType*)0);
            var resultArray = new VkDisplayKHR.HandleType[count];
            fixed (VkDisplayKHR.HandleType* pResultArray = resultArray)
            {
                Direct.GetDisplayPlaneSupportedDisplaysKHR(Handle, planeIndex, &count, pResultArray);
                return resultArray.Select(GetDisplay).ToArray();
            }
        }

        private IReadOnlyList<VkDisplayModePropertiesKHR> GetDisplayModePropertiesKHR(IVkDisplayKHR display)
        {
            if (Direct.GetDisplayModePropertiesKHR == null)
                return new VkDisplayModePropertiesKHR[0];
            int count;
            Direct.GetDisplayModePropertiesKHR(Handle, display.Handle, &count, (VkDisplayModePropertiesKHR.Raw*)0);
            var resultArray = new VkDisplayModePropertiesKHR.Raw[count];
            fixed (VkDisplayModePropertiesKHR.Raw* pResultArray = resultArray)
            {
                Direct.GetDisplayModePropertiesKHR(Handle, display.Handle, &count, pResultArray);
                return resultArray.Select(x => new VkDisplayModePropertiesKHR(&x, Instance)).ToArray();
            }
        }

        public VkObjectResult<IVkDisplayModeKHR> CreateDisplayMode(IVkDisplayKHR display, VkDisplayModeCreateInfoKHR createInfo, IVkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var displayHandle = display?.Handle ?? VkDisplayKHR.HandleType.Null;
                var pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkDisplayModeKHR.HandleType displayModeHandle;
                var result = Direct.CreateDisplayModeKHR(Handle, displayHandle, pCreateInfo, pAllocator, &displayModeHandle);
                var instance = result == VkResult.Success ? new VkDisplayModeKHR(Instance, displayModeHandle) : null;
                return new VkObjectResult<IVkDisplayModeKHR>(result, instance);
            }
        }

        private VkDisplayPlaneCapabilitiesKHR GetDisplayPlaneCapabilitiesKHR(IVkDisplayModeKHR mode, int planeIndex)
        {
            VkDisplayPlaneCapabilitiesKHR result;
            Direct.GetDisplayPlaneCapabilitiesKHR(Handle, mode.Handle, planeIndex, &result);
            return result;
        }

        public VkObjectResult<bool> GetSurfaceSupportKHR(int queueFamiltyIndex, IVkSurfaceKHR surface)
        {
            var surfaceHandle = surface?.Handle ?? VkSurfaceKHR.HandleType.Null;
            VkBool32 supported;
            var result = Direct.GetPhysicalDeviceSurfaceSupportKHR(Handle, queueFamiltyIndex, surfaceHandle, &supported);
            return new VkObjectResult<bool>(result, supported.Value);
        }

        public bool GetMirPresentationSupportKHR(int queueFamilyIndex, IntPtr connection)
        {
            return Direct.GetPhysicalDeviceMirPresentationSupportKHR(Handle, queueFamilyIndex, connection).Value;
        }

        public bool GetWaylandPresentationSupportKHR(int queueFamilyIndex, IntPtr display)
        {
            return Direct.GetPhysicalDeviceWaylandPresentationSupportKHR(Handle, queueFamilyIndex, display).Value;
        }

        public bool GetWin32PresentationSupportKHR(int queueFamilyIndex)
        {
            return Direct.GetPhysicalDeviceWin32PresentationSupportKHR(Handle, queueFamilyIndex).Value;
        }

        public bool GetXcbPresentationSupportKHR(int queueFamilyIndex, IntPtr connection, int visual_id)
        {
            return Direct.GetPhysicalDeviceXcbPresentationSupportKHR(Handle, queueFamilyIndex, connection, visual_id).Value;
        }

        public bool GetXlibPresentationSupportKHR(int queueFamilyIndex, IntPtr dpy, IntPtr visualID)
        {
            return Direct.GetPhysicalDeviceXlibPresentationSupportKHR(Handle, queueFamilyIndex, dpy, visualID).Value;
        }

        public VkObjectResult<VkSurfaceCapabilitiesKHR> GetSurfaceCapabilitiesKHR(IVkSurfaceKHR surface)
        {
            VkSurfaceCapabilitiesKHR capabilities;
            var result = Direct.GetPhysicalDeviceSurfaceCapabilitiesKHR(Handle, surface.Handle, &capabilities);
            return new VkObjectResult<VkSurfaceCapabilitiesKHR>(result, capabilities);
        }

        public VkObjectResult<IReadOnlyList<VkSurfaceFormatKHR>> GetSurfaceFormatsKHR(IVkSurfaceKHR surface)
        {
            int count;
            Direct.GetPhysicalDeviceSurfaceFormatsKHR(Handle, surface.Handle, &count, null).CheckSuccess();
            var resultArray = new VkSurfaceFormatKHR[count];
            fixed (VkSurfaceFormatKHR* pResults = resultArray)
            {
                var result = Direct.GetPhysicalDeviceSurfaceFormatsKHR(Handle, surface.Handle, &count, pResults);
                return new VkObjectResult<IReadOnlyList<VkSurfaceFormatKHR>>(result, resultArray);
            }
        }

        public VkObjectResult<IReadOnlyList<VkPresentModeKHR>> GetSurfacePresentModesKHR(IVkSurfaceKHR surface)
        {
            int count;
            Direct.GetPhysicalDeviceSurfacePresentModesKHR(Handle, surface.Handle, &count, null).CheckSuccess();
            var resultArray = new VkPresentModeKHR[count];
            fixed (VkPresentModeKHR* pResults = resultArray)
            {
                var result = Direct.GetPhysicalDeviceSurfacePresentModesKHR(Handle, surface.Handle, &count, pResults);
                return new VkObjectResult<IReadOnlyList<VkPresentModeKHR>>(result, resultArray);
            }
        }

        public VkObjectResult<IReadOnlyList<IVkExtensionProperties>> EnumerateDeviceExtensionProperties(string layerName)
        {
            var unmanagedSize =
                layerName.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var pLayerName = layerName.MarshalIndirect(ref unmanaged);

                int propertyCount;
                var result = Direct.EnumerateDeviceExtensionProperties(Handle, pLayerName, &propertyCount, null);
                if (result != VkResult.Success)
                    return new VkObjectResult<IReadOnlyList<IVkExtensionProperties>>(result, null);
                var rawArray = new VkExtensionProperties.Raw[propertyCount];
                fixed (VkExtensionProperties.Raw* pRawArray = rawArray)
                    result = Direct.EnumerateDeviceExtensionProperties(Handle, pLayerName, &propertyCount, pRawArray);
                if (result != VkResult.Success)
                    return new VkObjectResult<IReadOnlyList<IVkExtensionProperties>>(result, null);
                var properties = rawArray.Select(x => new VkExtensionProperties(&x)).ToArray();
                return new VkObjectResult<IReadOnlyList<IVkExtensionProperties>>(result, properties);
            }
            
        }
    }
}