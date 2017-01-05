using System;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenXcbSurfaceCreateInfoKHR
    {
        public VkStructureType sType;
        public void* pNext;
        public VkXcbSurfaceCreateFlagsKHR flags;
        public IntPtr connection;
        public int window;
    }
}