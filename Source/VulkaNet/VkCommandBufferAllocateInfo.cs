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
    public interface IVkCommandBufferAllocateInfo
    {
        IVkStructWrapper Next { get; }
        IVkCommandPool CommandPool { get; }
        VkCommandBufferLevel Level { get; }
        int CommandBufferCount { get; }
    }

    public unsafe class VkCommandBufferAllocateInfo : IVkCommandBufferAllocateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public IVkCommandPool CommandPool { get; set; }
        public VkCommandBufferLevel Level { get; set; }
        public int CommandBufferCount { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkCommandPool.HandleType commandPool;
            public VkCommandBufferLevel level;
            public int commandBufferCount;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkCommandBufferAllocateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkCommandBufferAllocateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkCommandBufferAllocateInfo.Raw MarshalDirect(this IVkCommandBufferAllocateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkCommandBufferAllocateInfo.Raw result;
            result.sType = VkStructureType.CommandBufferAllocateInfo;
            result.pNext = pNext;
            result.commandPool = s.CommandPool.Handle;
            result.level = s.Level;
            result.commandBufferCount = s.CommandBufferCount;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkCommandBufferAllocateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkCommandBufferAllocateInfo.Raw.SizeInBytes;

        public static VkCommandBufferAllocateInfo.Raw* MarshalIndirect(this IVkCommandBufferAllocateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkCommandBufferAllocateInfo.Raw*)0;
            var result = (VkCommandBufferAllocateInfo.Raw*)unmanaged;
            unmanaged += VkCommandBufferAllocateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkCommandBufferAllocateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkCommandBufferAllocateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkCommandBufferAllocateInfo.Raw* MarshalDirect(this IReadOnlyList<IVkCommandBufferAllocateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkCommandBufferAllocateInfo.Raw*)0;
            var result = (VkCommandBufferAllocateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkCommandBufferAllocateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkCommandBufferAllocateInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkCommandBufferAllocateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkCommandBufferAllocateInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkCommandBufferAllocateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkCommandBufferAllocateInfo.Raw**)0;
            var result = (VkCommandBufferAllocateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkCommandBufferAllocateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
