using System;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPipelineCacheCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkPipelineCacheCreateFlags flags;
        public Sizet initialDataSize;
        public IntPtr pInitialData;
    }
}