using System;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenMirSurfaceCreateInfoKHR
    {
        public VkStructureType sType;
        public void* pNext;
        public VkMirSurfaceCreateFlagsKHR flags;
        public IntPtr connection;
        public IntPtr mirSurface;
    }
}