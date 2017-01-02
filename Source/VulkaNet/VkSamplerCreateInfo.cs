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
    public unsafe class VkSamplerCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkSamplerCreateFlags Flags { get; set; }
        public VkFilter MagFilter { get; set; }
        public VkFilter MinFilter { get; set; }
        public VkSamplerMipmapMode MipmapMode { get; set; }
        public VkSamplerAddressMode AddressModeU { get; set; }
        public VkSamplerAddressMode AddressModeV { get; set; }
        public VkSamplerAddressMode AddressModeW { get; set; }
        public float MipLodBias { get; set; }
        public bool AnisotropyEnable { get; set; }
        public float MaxAnisotropy { get; set; }
        public bool CompareEnable { get; set; }
        public VkCompareOp CompareOp { get; set; }
        public float MinLod { get; set; }
        public float MaxLod { get; set; }
        public VkBorderColor BorderColor { get; set; }
        public bool UnnormalizedCoordinates { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkSamplerCreateFlags flags;
            public VkFilter magFilter;
            public VkFilter minFilter;
            public VkSamplerMipmapMode mipmapMode;
            public VkSamplerAddressMode addressModeU;
            public VkSamplerAddressMode addressModeV;
            public VkSamplerAddressMode addressModeW;
            public float mipLodBias;
            public VkBool32 anisotropyEnable;
            public float maxAnisotropy;
            public VkBool32 compareEnable;
            public VkCompareOp compareOp;
            public float minLod;
            public float maxLod;
            public VkBorderColor borderColor;
            public VkBool32 unnormalizedCoordinates;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkSamplerCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkSamplerCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkSamplerCreateInfo.Raw MarshalDirect(this VkSamplerCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkSamplerCreateInfo.Raw result;
            result.sType = VkStructureType.SamplerCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.magFilter = s.MagFilter;
            result.minFilter = s.MinFilter;
            result.mipmapMode = s.MipmapMode;
            result.addressModeU = s.AddressModeU;
            result.addressModeV = s.AddressModeV;
            result.addressModeW = s.AddressModeW;
            result.mipLodBias = s.MipLodBias;
            result.anisotropyEnable = new VkBool32(s.AnisotropyEnable);
            result.maxAnisotropy = s.MaxAnisotropy;
            result.compareEnable = new VkBool32(s.CompareEnable);
            result.compareOp = s.CompareOp;
            result.minLod = s.MinLod;
            result.maxLod = s.MaxLod;
            result.borderColor = s.BorderColor;
            result.unnormalizedCoordinates = new VkBool32(s.UnnormalizedCoordinates);
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkSamplerCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkSamplerCreateInfo.Raw.SizeInBytes;

        public static VkSamplerCreateInfo.Raw* MarshalIndirect(this VkSamplerCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkSamplerCreateInfo.Raw*)0;
            var result = (VkSamplerCreateInfo.Raw*)unmanaged;
            unmanaged += VkSamplerCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkSamplerCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkSamplerCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkSamplerCreateInfo.Raw* MarshalDirect(this IReadOnlyList<VkSamplerCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSamplerCreateInfo.Raw*)0;
            var result = (VkSamplerCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkSamplerCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

    }
}
