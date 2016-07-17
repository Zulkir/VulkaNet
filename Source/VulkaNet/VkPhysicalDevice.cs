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

        public VkPhysicalDevice(IVkInstance instance, IntPtr handle)
        {
            Instance = instance;
            Handle = handle;
            Direct = new DirectFunctions(instance);
            Properties = GetPhysicalDeviceProperties();
            QueueFamilyProperties = GetPhysicalDeviceQueueFamilyProperties();
            Features = GetPhysicalDeviceFeatures();
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
                IntPtr* pDevice);

            public GetPhysicalDeviceFeaturesDelegate GetPhysicalDeviceFeatures { get; }
            public delegate void GetPhysicalDeviceFeaturesDelegate(
                IntPtr physicalDevice,
                VkPhysicalDeviceFeatures.Raw* pFeatures);

            public DirectFunctions(IVkInstance instance)
            {
                GetPhysicalDeviceProperties =
                    VkHelpers.GetDelegate<GetPhysicalDevicePropertiesDelegate>(instance, "vkGetPhysicalDeviceProperties");
                GetPhysicalDeviceQueueFamilyProperties =
                    VkHelpers.GetDelegate<GetPhysicalDeviceQueueFamilyPropertiesDelegate>(instance, "vkGetPhysicalDeviceQueueFamilyProperties");
                CreateDevice = 
                    VkHelpers.GetDelegate<CreateDeviceDelegate>(instance, "vkCreateDevice");
                GetPhysicalDeviceFeatures =
                    VkHelpers.GetDelegate<GetPhysicalDeviceFeaturesDelegate>(instance, "vkGetPhysicalDeviceFeatures");
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
                createInfo.SafeMarshalSize() +
                allocator.SafeMarshalSize();
            return VkHelpers.RunWithUnamangedData(size, u => CreateDevice(u, createInfo, allocator));
        }

        private VkObjectResult<IVkDevice> CreateDevice(IntPtr data, IVkDeviceCreateInfo createInfo, IVkAllocationCallbacks allocator)
        {
            var unmanaged = (byte*)data;
            var pCreateInfo = createInfo.SafeMarshalTo(ref unmanaged);
            var pAllocator = allocator.SafeMarshalTo(ref unmanaged);
            IntPtr handle;
            var result = Direct.CreateDevice(Handle, pCreateInfo, pAllocator, &handle);
            var device = result == VkResult.Success ? new VkDevice(handle, allocator, this) : null;
            return new VkObjectResult<IVkDevice>(result, device);
        }

        private IVkPhysicalDeviceFeatures GetPhysicalDeviceFeatures()
        {
            VkPhysicalDeviceFeatures.Raw raw;
            Direct.GetPhysicalDeviceFeatures(Handle, &raw);
            return new VkPhysicalDeviceFeatures(&raw);
        }
    }
}