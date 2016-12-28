using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenMemoryAllocateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public ulong allocationSize;
        public int memoryTypeIndex;
    }
}