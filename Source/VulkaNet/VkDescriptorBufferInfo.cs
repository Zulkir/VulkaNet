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
    public unsafe struct VkDescriptorBufferInfo
    {
        public IVkSampler Sampler { get; set; }
        public IVkImageView ImageView { get; set; }
        public VkImageLayout ImageLayout { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkSampler.HandleType sampler;
            public VkImageView.HandleType imageView;
            public VkImageLayout imageLayout;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkDescriptorBufferInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkDescriptorBufferInfo s)
        {
            return 0;
        }

        public static VkDescriptorBufferInfo.Raw MarshalDirect(this VkDescriptorBufferInfo s, ref byte* unmanaged)
        {

            VkDescriptorBufferInfo.Raw result;
            result.sampler = s.Sampler?.Handle ?? VkSampler.HandleType.Null;
            result.imageView = s.ImageView?.Handle ?? VkImageView.HandleType.Null;
            result.imageLayout = s.ImageLayout;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkDescriptorBufferInfo s) =>
            s.SizeOfMarshalDirect() + VkDescriptorBufferInfo.Raw.SizeInBytes;

        public static int SizeOfMarshalIndirect(this VkDescriptorBufferInfo? s) =>
            s?.SizeOfMarshalIndirect() ?? 0;

        public static VkDescriptorBufferInfo.Raw* MarshalIndirect(this VkDescriptorBufferInfo s, ref byte* unmanaged)
        {
            var result = (VkDescriptorBufferInfo.Raw*)unmanaged;
            unmanaged += VkDescriptorBufferInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static VkDescriptorBufferInfo.Raw* MarshalIndirect(this VkDescriptorBufferInfo? s, ref byte* unmanaged) =>
            s.HasValue ? s.Value.MarshalIndirect(ref unmanaged) : null;

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkDescriptorBufferInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkDescriptorBufferInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkDescriptorBufferInfo.Raw* MarshalDirect(this IReadOnlyList<VkDescriptorBufferInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkDescriptorBufferInfo.Raw*)0;
            var result = (VkDescriptorBufferInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkDescriptorBufferInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
