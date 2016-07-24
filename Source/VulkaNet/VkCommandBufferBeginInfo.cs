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
    }

    public static unsafe class VkCommandBufferBeginInfoExtensions
    {
        public int SizeOfMarshalDirect(this ICommandBufferBeginInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                Next.SizeOfMarshalIndirect() +
                InheritanceInfo.SizeOfMarshalIndirect();
        }

        public Raw* MarshalDirect(this IVkCommandBufferBeginInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pInheritanceInfo = s.InheritanceInfo.MarshalIndirect(ref unmanaged);

            VkCommandBufferBeginInfo.Raw result;
            result.sType = VkStructureType.CommandBufferBeginInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.pInheritanceInfo = pInheritanceInfo;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkCommandBufferBeginInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkCommandBufferBeginInfo.Raw.SizeInBytes;

        public static VkCommandBufferBeginInfo.Raw* MarshalIndirect(this IVkCommandBufferBeginInfo s, ref byte* unmanaged)
        {
            var result = (VkCommandBufferBeginInfo.Raw*)unmanaged;
            unmanaged += VkCommandBufferBeginInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkCommandBufferBeginInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkCommandBufferBeginInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkCommandBufferBeginInfo.Raw* MarshalDirect(this IReadOnlyList<IVkCommandBufferBeginInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkCommandBufferBeginInfo.Raw*)0;
            var result = (VkCommandBufferBeginInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkCommandBufferBeginInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkCommandBufferBeginInfo> list)
            list == null || list.Count == 0
                ? 0
                : sizeof(VkCommandBufferBeginInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkCommandBufferBeginInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkCommandBufferBeginInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkCommandBufferBeginInfo.Raw**)0;
            var result = (VkCommandBufferBeginInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkCommandBufferBeginInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
