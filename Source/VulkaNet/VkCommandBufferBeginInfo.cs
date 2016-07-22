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
    public interface IVkCommandBufferBeginInfo : IVkStructWrapper
    {
        IVkStructWrapper Next { get; }
        VkCommandBufferUsageFlags Flags { get; }
        IVkCommandBufferInheritanceInfo InheritanceInfo { get; }
    }

    public unsafe class VkCommandBufferBeginInfo : IVkCommandBufferBeginInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkCommandBufferUsageFlags Flags { get; set; }
        public IVkCommandBufferInheritanceInfo InheritanceInfo { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkCommandBufferUsageFlags flags;
            public VkCommandBufferInheritanceInfo.Raw* pInheritanceInfo;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }

        public int MarshalSize() =>
            Next.SafeMarshalSize() +
            InheritanceInfo.SafeMarshalSize() +
            Raw.SizeInBytes;

        public Raw* MarshalTo(ref byte* unmanaged)
        {
            var pNext = Next.SafeMarshalTo(ref unmanaged);
            var pInheritanceInfo = InheritanceInfo.SafeMarshalTo(ref unmanaged);

            var result = (Raw*)unmanaged;
            unmanaged += Raw.SizeInBytes;
            result->sType = VkStructureType.CommandBufferBeginInfo;
            result->pNext = pNext;
            result->flags = Flags;
            result->pInheritanceInfo = pInheritanceInfo;
            return result;
        }

        void* IVkStructWrapper.MarshalTo(ref byte* unmanaged) =>
            MarshalTo(ref unmanaged);
    }

    public static unsafe class VkCommandBufferBeginInfoExtensions
    {
        public static int SafeMarshalSize(this IVkCommandBufferBeginInfo s) =>
            s?.MarshalSize() ?? 0;

        public static VkCommandBufferBeginInfo.Raw* SafeMarshalTo(this IVkCommandBufferBeginInfo s, ref byte* unmanaged) =>
            (VkCommandBufferBeginInfo.Raw*)(s != null ? s.MarshalTo(ref unmanaged) : (void*)0);
    }
}
