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
    public interface IVkQueueFamilyProperties
    {
        VkQueueFlags QueueFlags { get; }
        int QueueCount { get; }
        int TimestampValidBits { get; }
        VkExtent3D MinImageTransferGranularity { get; }
    }

    public class VkQueueFamilyProperties : IVkQueueFamilyProperties
    {
        public VkQueueFlags QueueFlags { get; }
        public int QueueCount { get; }
        public int TimestampValidBits { get; }
        public VkExtent3D MinImageTransferGranularity { get; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkQueueFlags queueFlags;
            public int queueCount;
            public int timestampValidBits;
            public VkExtent3D minImageTransferGranularity;
        }

        public unsafe VkQueueFamilyProperties(Raw* raw)
        {
            QueueFlags = raw->queueFlags;
            QueueCount = raw->queueCount;
            TimestampValidBits = raw->timestampValidBits;
            MinImageTransferGranularity = raw->minImageTransferGranularity;
        }
    }
}