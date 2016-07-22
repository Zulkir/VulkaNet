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

using System.Runtime.InteropServices;

namespace VulkaNet
{
    public unsafe interface IVkCommandBufferAllocateInfo
    {
        IVkStructWrapper Next { get; }
        VkCommandBufferLevel Level { get; }
        int CommandBufferCount { get; }

        int MarshalSize();
        VkCommandBufferAllocateInfo.Raw* MarshalTo(ref byte* unmanaged, IVkCommandPool commandPool);
    }

    public unsafe class VkCommandBufferAllocateInfo : IVkCommandBufferAllocateInfo
    {
        public IVkStructWrapper Next { get; set; }
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

        public int MarshalSize() =>
            Next.SafeMarshalSize() +
            Raw.SizeInBytes;

        public Raw* MarshalTo(ref byte* unmanaged, IVkCommandPool commandPool)
        {
            var pNext = Next.SafeMarshalTo(ref unmanaged);

            var result = (Raw*)unmanaged;
            unmanaged += Raw.SizeInBytes;
            result->sType = VkStructureType.CommandBufferAllocateInfo;
            result->pNext = pNext;
            result->commandPool = commandPool.Handle;
            result->level = Level;
            result->commandBufferCount = CommandBufferCount;
            return result;
        }
    }

    public static unsafe class VkCommandBufferAllocateInfoExtensions
    {
        public static int SafeMarshalSize(this IVkCommandBufferAllocateInfo s) =>
            s?.MarshalSize() ?? 0;

        public static VkCommandBufferAllocateInfo.Raw* SafeMarshalTo(this IVkCommandBufferAllocateInfo s, ref byte* unmanaged, IVkCommandPool commandPool) =>
            (VkCommandBufferAllocateInfo.Raw*)(s != null ? s.MarshalTo(ref unmanaged, commandPool) : (void*)0);
    }
}
