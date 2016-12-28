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
    public interface IVkPhysicalDeviceMemoryProperties
    {
        int MemoryTypeCount { get; }
        IReadOnlyList<VkMemoryType> MemoryTypes { get; }
        int MemoryHeapCount { get; }
        IReadOnlyList<VkMemoryHeap> MemoryHeaps { get; }
    }

    public unsafe class VkPhysicalDeviceMemoryProperties : IVkPhysicalDeviceMemoryProperties
    {
        public int MemoryTypeCount { get; set; }
        public IReadOnlyList<VkMemoryType> MemoryTypes { get; set; }
        public int MemoryHeapCount { get; set; }
        public IReadOnlyList<VkMemoryHeap> MemoryHeaps { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public int memoryTypeCount;
            public fixed byte memoryTypes[32 * 8];
            public int memoryHeapCount;
            public fixed byte memoryHeaps[16 * 16];

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }

        public VkPhysicalDeviceMemoryProperties(Raw* raw)
        {
            MemoryTypeCount = raw->memoryTypeCount;
            MemoryTypes = Enumerable.Range(0, MemoryTypeCount).Select(i => ((VkMemoryType*)raw->memoryTypeCount)[i]).ToArray();
            MemoryHeapCount = raw->memoryHeapCount;
            MemoryHeaps = Enumerable.Range(0, MemoryHeapCount).Select(i => ((VkMemoryHeap*)raw->memoryHeapCount)[i]).ToArray();
        }
    }
}
