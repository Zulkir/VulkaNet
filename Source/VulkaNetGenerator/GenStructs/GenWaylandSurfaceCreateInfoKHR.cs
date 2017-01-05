using System;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenWaylandSurfaceCreateInfoKHR
    {
        public VkStructureType sType;
        public void* pNext;
        public VkWaylandSurfaceCreateFlagsKHR flags;
        public IntPtr display;
        public IntPtr surface;
    }
}