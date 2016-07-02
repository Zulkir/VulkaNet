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

using System.Collections.Generic;
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
        public static int SafeMarshalSize(this IVkInstanceCreateInfo s)
            => s != null ?
                s.Next.SafeMarshalSize() +
                s.ApplicationInfo.SafeMarshalSize() +
                s.EnabledLayerNames.SafeMarshalSize() +
                s.EnabledExtensionNames.SafeMarshalSize() +
                VkInstanceCreateInfo.Raw.SizeInBytes
            : 0;

        public static VkInstanceCreateInfo.Raw* SafeMarshalTo(this IVkInstanceCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkInstanceCreateInfo.Raw*)0;

            var pNext = s.Next.SafeMarshalTo(ref unmanaged);
            var pApplicationInfo = s.ApplicationInfo.SafeMarshalTo(ref unmanaged);
            var ppEnabledLayerNames = s.EnabledLayerNames.SafeMarshalTo(ref unmanaged);
            var ppEnabledExtensionNames = s.EnabledExtensionNames.SafeMarshalTo(ref unmanaged);

            var result = (VkInstanceCreateInfo.Raw*)unmanaged;
            unmanaged += VkInstanceCreateInfo.Raw.SizeInBytes;
            result->sType = VkStructureType.InstanceCreateInfo;
            result->pNext = pNext;
            result->flags = s.Flags;
            result->pApplicationInfo = pApplicationInfo;
            result->enabledLayerCount = s.EnabledLayerNames?.Count ?? 0;
            result->ppEnabledLayerNames = ppEnabledLayerNames;
            result->enabledExtensionCount = s.EnabledExtensionNames?.Count ?? 0;
            result->ppEnabledExtensionNames = ppEnabledExtensionNames;
            return result;
        }
    }
}
