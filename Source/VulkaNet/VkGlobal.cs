﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using VulkaNet.InternalHelpers;

namespace VulkaNet
{
    public interface IVkGlobal
    {
        VkGlobal.DirectFunctions Direct { get; }
        IReadOnlyList<IVkLayerProperties> InstanceLayerProperties { get; }
        IReadOnlyList<IVkExtensionProperties> InstanceExtensionPropertiesFor(string layerName);
        VkObjectResult<IVkInstance> CreateInstance(VkInstanceCreateInfo createInfo, VkAllocationCallbacks allocator);
    }

    public unsafe class VkGlobal : IVkGlobal
    {
        public DirectFunctions Direct { get; }
        public IReadOnlyList<IVkLayerProperties> InstanceLayerProperties { get; }
        private Dictionary<string, IReadOnlyList<IVkExtensionProperties>> InstanceExtensionProperties { get; }

        public IReadOnlyList<IVkExtensionProperties> InstanceExtensionPropertiesFor(string layerName)
            => InstanceExtensionProperties[layerName];

        public VkGlobal()
        {
            Direct = new DirectFunctions();
            InstanceLayerProperties = EnumerateInstanceLayerProperties();
            InstanceExtensionProperties = InstanceLayerProperties.Select(x => x.LayerName).ToDictionary(
                x => x, EnumerateInstanceExtensionProperties);
        }

        public class DirectFunctions
        {
            [DllImport("vulkan-1.dll")]
            public static extern IntPtr vkGetInstanceProcAddr(IntPtr instance, byte* pName);

            public EnumerateInstanceLayerPropertiesDelegate EnumerateInstanceLayerProperties { get; }
            public delegate VkResult EnumerateInstanceLayerPropertiesDelegate(
                int* pPropertyCount,
                VkLayerProperties.Raw* pProperties);

            public EnumerateInstanceExtensionPropertiesDelegate EnumerateInstanceExtensionProperties { get; }
            public delegate VkResult EnumerateInstanceExtensionPropertiesDelegate(
                byte* pLayerName,
                int* pPropertyCount,
                VkExtensionProperties.Raw* pProperties);
            
            public CreateInstanceDelegate CreateInstance { get; }
            public delegate VkResult CreateInstanceDelegate(
                VkInstanceCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                IntPtr* pInstance);

            public DirectFunctions()
            {
                EnumerateInstanceLayerProperties =
                    VkHelpers.GetDelegate<EnumerateInstanceLayerPropertiesDelegate>(null, "vkEnumerateInstanceLayerProperties");
                EnumerateInstanceExtensionProperties =
                    VkHelpers.GetDelegate<EnumerateInstanceExtensionPropertiesDelegate>(null, "vkEnumerateInstanceExtensionProperties");
                CreateInstance =
                    VkHelpers.GetDelegate<CreateInstanceDelegate>(null, "vkCreateInstance");
            }
        }
        
        public static IntPtr GetInstanceProcAddr(IVkInstance instance, string name)
        {
            fixed (byte* pName = name.ToAnsiArray())
                return DirectFunctions.vkGetInstanceProcAddr(instance.SafeGetHandle(), pName);
        }

        private IReadOnlyList<IVkLayerProperties> EnumerateInstanceLayerProperties()
        {
            int count;
            Direct.EnumerateInstanceLayerProperties(&count, (VkLayerProperties.Raw*)0).CheckSuccess();
            var rawArray = new VkLayerProperties.Raw[count];
            fixed (VkLayerProperties.Raw* pRawArray = rawArray)
            {
                Direct.EnumerateInstanceLayerProperties(&count, pRawArray).CheckSuccess();
                return rawArray.Select(x => new VkLayerProperties(&x)).ToArray();
            }
        }

        private IReadOnlyList<IVkExtensionProperties> EnumerateInstanceExtensionProperties(string layerName)
        {
            var size = layerName.SafeMarshalSize();
            return VkHelpers.RunWithUnamangedData(size, u => EnumerateInstanceExtensionPropertiesInternal(u, layerName));
        }

        private IReadOnlyList<VkExtensionProperties> EnumerateInstanceExtensionPropertiesInternal(IntPtr data, string layerName)
        {
            var unmanaged = (byte*)data;
            var pLayerName = layerName.SafeMarshalTo(ref unmanaged);
            int count;
            Direct.EnumerateInstanceExtensionProperties(pLayerName, &count, (VkExtensionProperties.Raw*)0).CheckSuccess();
            var rawArray = new VkExtensionProperties.Raw[count];
            fixed (VkExtensionProperties.Raw* pRawArray = rawArray)
            {
                Direct.EnumerateInstanceExtensionProperties(pLayerName, &count, pRawArray).CheckSuccess();
                return rawArray.Select(x => new VkExtensionProperties(&x)).ToArray();
            }
        }
        
        public VkObjectResult<IVkInstance> CreateInstance(VkInstanceCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var size =
                createInfo.SafeMarshalSize() +
                allocator.SafeMarshalSize();
            return VkHelpers.RunWithUnamangedData(size, u => CreateInstanceInternal(u, createInfo, allocator));
        }

        private VkObjectResult<IVkInstance> CreateInstanceInternal(IntPtr data, VkInstanceCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanaged = (byte*)data;
            var pCreateInfo = createInfo.SafeMarshalTo(ref unmanaged);
            var pAllocator = allocator.SafeMarshalTo(ref unmanaged);
            IntPtr handle;
            var result = Direct.CreateInstance(pCreateInfo, pAllocator, &handle);
            var instance = result == VkResult.Success ? new VkInstance(handle, allocator) : null;
            return new VkObjectResult<IVkInstance>(result, instance);
        }
    }
}