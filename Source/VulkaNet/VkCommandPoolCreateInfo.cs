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
    public interface IVkCommandPoolCreateInfo : IVkStructWrapper
    {
        IVkStructWrapper Next { get; }
        VkCommandPoolCreateFlags Flags { get; }
        int QueueFamilyIndex { get; }
    }

    public unsafe class VkCommandPoolCreateInfo : IVkCommandPoolCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkCommandPoolCreateFlags Flags { get; set; }
        public int QueueFamilyIndex { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkCommandPoolCreateFlags flags;
            public int queueFamilyIndex;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkCommandPoolCreateInfoExtensions
    {
        public int SizeOfMarshalDirect(this ICommandPoolCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                Next.SizeOfMarshalIndirect();
        }

        public Raw* MarshalDirect(this IVkCommandPoolCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkCommandPoolCreateInfo.Raw result;
            result.sType = VkStructureType.CommandPoolCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.queueFamilyIndex = s.QueueFamilyIndex;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkCommandPoolCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkCommandPoolCreateInfo.Raw.SizeInBytes;

        public static VkCommandPoolCreateInfo.Raw* MarshalIndirect(this IVkCommandPoolCreateInfo s, ref byte* unmanaged)
        {
            var result = (VkCommandPoolCreateInfo.Raw*)unmanaged;
            unmanaged += VkCommandPoolCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkCommandPoolCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkCommandPoolCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkCommandPoolCreateInfo.Raw* MarshalDirect(this IReadOnlyList<IVkCommandPoolCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkCommandPoolCreateInfo.Raw*)0;
            var result = (VkCommandPoolCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkCommandPoolCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkCommandPoolCreateInfo> list)
            list == null || list.Count == 0
                ? 0
                : sizeof(VkCommandPoolCreateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkCommandPoolCreateInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkCommandPoolCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkCommandPoolCreateInfo.Raw**)0;
            var result = (VkCommandPoolCreateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkCommandPoolCreateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
