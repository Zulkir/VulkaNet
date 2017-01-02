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
using System.Collections.Generic;
using System.Linq;

namespace VulkaNet
{
    public interface IVkPhysicalDevice : IVkInstanceChild
    {
        IVkPhysicalDeviceProperties Properties { get; }
        IReadOnlyList<IVkQueueFamilyProperties> QueueFamilyProperties { get; }
        IVkPhysicalDeviceFeatures Features { get; }
        IVkPhysicalDeviceMemoryProperties MemoryProperties { get; }
        IReadOnlyList<VkSparseImageFormatProperties> GetSparseImageFormatProperties(VkFormat format, VkImageType type, VkSampleCountFlagBits samples, VkImageUsageFlags usage, VkImageTiling tiling);
        VkObjectResult<IVkDevice> CreateDevice(IVkDeviceCreateInfo createInfo, IVkAllocationCallbacks allocator);
    }

    public unsafe class VkPhysicalDevice : IVkPhysicalDevice
    {
        public IVkInstance Instance { get; }
        public IntPtr Handle { get; }
        public DirectFunctions Direct { get; }
        public IVkPhysicalDeviceProperties Properties { get; }
        public IReadOnlyList<IVkQueueFamilyProperties> QueueFamilyProperties { get; }
        public IVkPhysicalDeviceFeatures Features { get; }
        public IVkPhysicalDeviceMemoryProperties MemoryProperties { get; }

        public VkPhysicalDevice(IVkInstance instance, IntPtr handle)
        {
            Instance = instance;
            Handle = handle;
            Direct = new DirectFunctions(instance);
            Properties = GetPhysicalDeviceProperties();
            QueueFamilyProperties = GetPhysicalDeviceQueueFamilyProperties();
            Features = GetPhysicalDeviceFeatures();
            MemoryProperties = GetPhysicalDeviceMemoryProperties();
        }

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

            public DirectFunctions(IVkInstance instance)
            {
                GetPhysicalDeviceProperties =
                    VkHelpers.GetInstanceDelegate<GetPhysicalDevicePropertiesDelegate>(instance, "vkGetPhysicalDeviceProperties");
                GetPhysicalDeviceQueueFamilyProperties =
                    VkHelpers.GetInstanceDelegate<GetPhysicalDeviceQueueFamilyPropertiesDelegate>(instance, "vkGetPhysicalDeviceQueueFamilyProperties");
                CreateDevice = 
                    VkHelpers.GetInstanceDelegate<CreateDeviceDelegate>(instance, "vkCreateDevice");
                GetPhysicalDeviceFeatures =
                    VkHelpers.GetInstanceDelegate<GetPhysicalDeviceFeaturesDelegate>(instance, "vkGetPhysicalDeviceFeatures");
                GetPhysicalDeviceMemoryProperties =
                    VkHelpers.GetInstanceDelegate<GetPhysicalDeviceMemoryPropertiesDelegate>(instance, "vkGetPhysicalDeviceMemoryProperties");
                GetPhysicalDeviceSparseImageFormatProperties =
                    VkHelpers.GetInstanceDelegate<GetPhysicalDeviceSparseImageFormatPropertiesDelegate>(instance, "vkGetPhysicalDeviceSparseImageFormatProperties");
            }
        }

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
        
        public VkObjectResult<IVkDevice> CreateDevice(IVkDeviceCreateInfo createInfo, IVkAllocationCallbacks allocator)
        {
            var size =
                createInfo.SizeOfMarshalDirect() +
                allocator.SizeOfMarshalIndirect();
            return VkHelpers.RunWithUnamangedData(size, u => CreateDevice(u, createInfo, allocator));
        }

        private VkObjectResult<IVkDevice> CreateDevice(IntPtr data, IVkDeviceCreateInfo createInfo, IVkAllocationCallbacks allocator)
        {
            var unmanaged = (byte*)data;
            var createInfoRaw = createInfo.MarshalDirect(ref unmanaged);
            var pAllocator = allocator.MarshalIndirect(ref unmanaged);
            VkDevice.HandleType handle;
            var result = Direct.CreateDevice(Handle, &createInfoRaw, pAllocator, &handle);
            var device = result == VkResult.Success ? new VkDevice(this, handle, allocator) : null;
            return new VkObjectResult<IVkDevice>(result, device);
        }

        private IVkPhysicalDeviceFeatures GetPhysicalDeviceFeatures()
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
    }
}