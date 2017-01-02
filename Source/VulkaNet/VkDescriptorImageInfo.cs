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
using System.Linq;
using System.Runtime.InteropServices;

namespace VulkaNet
{
    public unsafe struct VkDescriptorImageInfo
    {
        public IVkBuffer Buffer { get; set; }
        public ulong Offset { get; set; }
        public ulong Range { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkBuffer.HandleType buffer;
            public ulong offset;
            public ulong range;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkDescriptorImageInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkDescriptorImageInfo s)
        {
            return 0;
        }

        public static VkDescriptorImageInfo.Raw MarshalDirect(this VkDescriptorImageInfo s, ref byte* unmanaged)
        {

            VkDescriptorImageInfo.Raw result;
            result.buffer = s.Buffer?.Handle ?? VkBuffer.HandleType.Null;
            result.offset = s.Offset;
            result.range = s.Range;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkDescriptorImageInfo s) =>
            s.SizeOfMarshalDirect() + VkDescriptorImageInfo.Raw.SizeInBytes;

        public static VkDescriptorImageInfo.Raw* MarshalIndirect(this VkDescriptorImageInfo s, ref byte* unmanaged)
        {
            var result = (VkDescriptorImageInfo.Raw*)unmanaged;
            unmanaged += VkDescriptorImageInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkDescriptorImageInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkDescriptorImageInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkDescriptorImageInfo.Raw* MarshalDirect(this IReadOnlyList<VkDescriptorImageInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkDescriptorImageInfo.Raw*)0;
            var result = (VkDescriptorImageInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkDescriptorImageInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
