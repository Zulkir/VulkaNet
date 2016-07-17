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
    public interface IVkInstanceCreateInfo : IVkStructWrapper
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

        public int MarshalSize() =>
            Next.SafeMarshalSize() +
            ApplicationInfo.SafeMarshalSize() +
            EnabledLayerNames.SafeMarshalSize() +
            EnabledExtensionNames.SafeMarshalSize() +
            Raw.SizeInBytes;

        public Raw* MarshalTo(ref byte* unmanaged)
        {
            var pNext = Next.SafeMarshalTo(ref unmanaged);
            var pApplicationInfo = ApplicationInfo.SafeMarshalTo(ref unmanaged);
            var ppEnabledLayerNames = EnabledLayerNames.SafeMarshalTo(ref unmanaged);
            var ppEnabledExtensionNames = EnabledExtensionNames.SafeMarshalTo(ref unmanaged);

            var result = (Raw*)unmanaged;
            unmanaged += Raw.SizeInBytes;
            result->sType = VkStructureType.InstanceCreateInfo;
            result->pNext = pNext;
            result->flags = Flags;
            result->pApplicationInfo = pApplicationInfo;
            result->enabledLayerCount = EnabledLayerNames?.Count ?? 0;
            result->ppEnabledLayerNames = ppEnabledLayerNames;
            result->enabledExtensionCount = EnabledExtensionNames?.Count ?? 0;
            result->ppEnabledExtensionNames = ppEnabledExtensionNames;
            return result;
        }

        void* IVkStructWrapper.MarshalTo(ref byte* unmanaged) =>
            MarshalTo(ref unmanaged);
    }

    public static unsafe class VkInstanceCreateInfoExtensions
    {
        public static int SafeMarshalSize(this IVkInstanceCreateInfo s) =>
            s?.MarshalSize() ?? 0;

        public static VkInstanceCreateInfo.Raw* SafeMarshalTo(this IVkInstanceCreateInfo s, ref byte* unmanaged) =>
            (VkInstanceCreateInfo.Raw*)(s != null ? s.MarshalTo(ref unmanaged) : (void*)0);
    }
}
