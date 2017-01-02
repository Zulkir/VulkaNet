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
    public unsafe class VkBindSparseInfo
    {
        public IVkStructWrapper Next { get; set; }
        public IReadOnlyList<IVkSemaphore> WaitSemaphores { get; set; }
        public IReadOnlyList<VkSparseBufferMemoryBindInfo> BufferBinds { get; set; }
        public IReadOnlyList<VkSparseImageOpaqueMemoryBindInfo> ImageOpaqueBinds { get; set; }
        public IReadOnlyList<VkSparseImageMemoryBindInfo> ImageBinds { get; set; }
        public IReadOnlyList<IVkSemaphore> SignalSemaphores { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public int waitSemaphoreCount;
            public VkSemaphore.HandleType* pWaitSemaphores;
            public int bufferBindCount;
            public VkSparseBufferMemoryBindInfo.Raw* pBufferBinds;
            public int imageOpaqueBindCount;
            public VkSparseImageOpaqueMemoryBindInfo.Raw* pImageOpaqueBinds;
            public int imageBindCount;
            public VkSparseImageMemoryBindInfo.Raw* pImageBinds;
            public int signalSemaphoreCount;
            public VkSemaphore.HandleType* pSignalSemaphores;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkBindSparseInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkBindSparseInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.WaitSemaphores.SizeOfMarshalDirect() +
                s.BufferBinds.SizeOfMarshalDirect() +
                s.ImageOpaqueBinds.SizeOfMarshalDirect() +
                s.ImageBinds.SizeOfMarshalDirect() +
                s.SignalSemaphores.SizeOfMarshalDirect();
        }

        public static VkBindSparseInfo.Raw MarshalDirect(this VkBindSparseInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pWaitSemaphores = s.WaitSemaphores.MarshalDirect(ref unmanaged);
            var pBufferBinds = s.BufferBinds.MarshalDirect(ref unmanaged);
            var pImageOpaqueBinds = s.ImageOpaqueBinds.MarshalDirect(ref unmanaged);
            var pImageBinds = s.ImageBinds.MarshalDirect(ref unmanaged);
            var pSignalSemaphores = s.SignalSemaphores.MarshalDirect(ref unmanaged);

            VkBindSparseInfo.Raw result;
            result.sType = VkStructureType.BindSparseInfo;
            result.pNext = pNext;
            result.waitSemaphoreCount = s.WaitSemaphores?.Count ?? 0;
            result.pWaitSemaphores = pWaitSemaphores;
            result.bufferBindCount = s.BufferBinds?.Count ?? 0;
            result.pBufferBinds = pBufferBinds;
            result.imageOpaqueBindCount = s.ImageOpaqueBinds?.Count ?? 0;
            result.pImageOpaqueBinds = pImageOpaqueBinds;
            result.imageBindCount = s.ImageBinds?.Count ?? 0;
            result.pImageBinds = pImageBinds;
            result.signalSemaphoreCount = s.SignalSemaphores?.Count ?? 0;
            result.pSignalSemaphores = pSignalSemaphores;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkBindSparseInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkBindSparseInfo.Raw.SizeInBytes;

        public static VkBindSparseInfo.Raw* MarshalIndirect(this VkBindSparseInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkBindSparseInfo.Raw*)0;
            var result = (VkBindSparseInfo.Raw*)unmanaged;
            unmanaged += VkBindSparseInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkBindSparseInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkBindSparseInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkBindSparseInfo.Raw* MarshalDirect(this IReadOnlyList<VkBindSparseInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkBindSparseInfo.Raw*)0;
            var result = (VkBindSparseInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkBindSparseInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
