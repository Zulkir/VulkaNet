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
using System.Runtime.InteropServices;

namespace VulkaNet
{
    public interface IVkInstanceCreateInfo
    {
        IVkStructWrapper Next { get; }
        VkInstanceCreateFlags Flags { get; }
        IVkApplicationInfo ApplicationInfo { get; }
        IReadOnlyList<string> EnabledLayerNames { get; }
        IReadOnlyList<string> EnabledExtensionNames { get; }
    }

    public unsafe class VkInstanceCreateInfo : IVkInstanceCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkInstanceCreateFlags Flags { get; set; }
        public IVkApplicationInfo ApplicationInfo { get; set; }
        public IReadOnlyList<string> EnabledLayerNames { get; set; }
        public IReadOnlyList<string> EnabledExtensionNames { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkInstanceCreateFlags flags;
            public VkApplicationInfo.Raw* pApplicationInfo;
            public int enabledLayerCount;
            public byte** ppEnabledLayerNames;
            public int enabledExtensionCount;
            public byte** ppEnabledExtensionNames;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkInstanceCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkInstanceCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.ApplicationInfo.SizeOfMarshalIndirect() +
                s.EnabledLayerNames.SizeOfMarshalIndirect() +
                s.EnabledExtensionNames.SizeOfMarshalIndirect();
        }

        public static VkInstanceCreateInfo.Raw MarshalDirect(this IVkInstanceCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pApplicationInfo = s.ApplicationInfo.MarshalIndirect(ref unmanaged);
            var ppEnabledLayerNames = s.EnabledLayerNames.MarshalIndirect(ref unmanaged);
            var ppEnabledExtensionNames = s.EnabledExtensionNames.MarshalIndirect(ref unmanaged);

            VkInstanceCreateInfo.Raw result;
            result.sType = VkStructureType.InstanceCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.pApplicationInfo = pApplicationInfo;
            result.enabledLayerCount = s.EnabledLayerNames?.Count ?? 0;
            result.ppEnabledLayerNames = ppEnabledLayerNames;
            result.enabledExtensionCount = s.EnabledExtensionNames?.Count ?? 0;
            result.ppEnabledExtensionNames = ppEnabledExtensionNames;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkInstanceCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkInstanceCreateInfo.Raw.SizeInBytes;

        public static VkInstanceCreateInfo.Raw* MarshalIndirect(this IVkInstanceCreateInfo s, ref byte* unmanaged)
        {
            var result = (VkInstanceCreateInfo.Raw*)unmanaged;
            unmanaged += VkInstanceCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkInstanceCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkInstanceCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkInstanceCreateInfo.Raw* MarshalDirect(this IReadOnlyList<IVkInstanceCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkInstanceCreateInfo.Raw*)0;
            var result = (VkInstanceCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkInstanceCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkInstanceCreateInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkInstanceCreateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkInstanceCreateInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkInstanceCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkInstanceCreateInfo.Raw**)0;
            var result = (VkInstanceCreateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkInstanceCreateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
