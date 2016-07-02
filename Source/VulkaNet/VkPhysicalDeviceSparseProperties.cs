﻿#region License
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
    public interface IVkPhysicalDeviceSparseProperties
    {
        bool ResidencyStandard2DBlockShape { get; }
        bool ResidencyStandard2DMultisampleBlockShape { get; }
        bool ResidencyStandard3DBlockShape { get; }
        bool ResidencyAlignedMipSize { get; }
        bool ResidencyNonResidentStrict { get; }
    }

    public class VkPhysicalDeviceSparseProperties : IVkPhysicalDeviceSparseProperties
    {
        public VkPhysicalDeviceSparseProperties() { }

        public bool ResidencyStandard2DBlockShape { get; set; }
        public bool ResidencyStandard2DMultisampleBlockShape { get; set; }
        public bool ResidencyStandard3DBlockShape { get; set; }
        public bool ResidencyAlignedMipSize { get; set; }
        public bool ResidencyNonResidentStrict { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkBool32 residencyStandard2DBlockShape;
            public VkBool32 residencyStandard2DMultisampleBlockShape;
            public VkBool32 residencyStandard3DBlockShape;
            public VkBool32 residencyAlignedMipSize;
            public VkBool32 residencyNonResidentStrict;
        }

        public unsafe VkPhysicalDeviceSparseProperties(Raw* raw)
        {
            ResidencyStandard2DBlockShape = raw->residencyStandard2DBlockShape.Value;
            ResidencyStandard2DMultisampleBlockShape = raw->residencyStandard2DMultisampleBlockShape.Value;
            ResidencyStandard3DBlockShape = raw->residencyStandard3DBlockShape.Value;
            ResidencyAlignedMipSize = raw->residencyAlignedMipSize.Value;
            ResidencyNonResidentStrict = raw->residencyNonResidentStrict.Value;
        }
    }
}