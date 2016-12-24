using System;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenShaderModuleCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkShaderModuleCreateFlags flags;
        public Sizet codeSize;
        public IntPtr pCode;
    }
}