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
    public interface IVkBufferMemoryBarrier
    {
    }

    public unsafe class VkBufferMemoryBarrier : IVkBufferMemoryBarrier
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkBufferMemoryBarrierExtensions
    {
        public static int SizeOfMarshalDirect(this IVkBufferMemoryBarrier s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return 0;
        }

        public static VkBufferMemoryBarrier.Raw MarshalDirect(this IVkBufferMemoryBarrier s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");


            VkBufferMemoryBarrier.Raw result;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkBufferMemoryBarrier s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkBufferMemoryBarrier.Raw.SizeInBytes;

        public static VkBufferMemoryBarrier.Raw* MarshalIndirect(this IVkBufferMemoryBarrier s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkBufferMemoryBarrier.Raw*)0;
            var result = (VkBufferMemoryBarrier.Raw*)unmanaged;
            unmanaged += VkBufferMemoryBarrier.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkBufferMemoryBarrier> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkBufferMemoryBarrier.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkBufferMemoryBarrier.Raw* MarshalDirect(this IReadOnlyList<IVkBufferMemoryBarrier> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkBufferMemoryBarrier.Raw*)0;
            var result = (VkBufferMemoryBarrier.Raw*)unmanaged;
            unmanaged += sizeof(VkBufferMemoryBarrier.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkBufferMemoryBarrier> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkBufferMemoryBarrier.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkBufferMemoryBarrier.Raw** MarshalIndirect(this IReadOnlyList<IVkBufferMemoryBarrier> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkBufferMemoryBarrier.Raw**)0;
            var result = (VkBufferMemoryBarrier.Raw**)unmanaged;
            unmanaged += sizeof(VkBufferMemoryBarrier.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
