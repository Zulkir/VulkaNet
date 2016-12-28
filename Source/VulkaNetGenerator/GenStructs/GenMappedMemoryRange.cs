using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenMappedMemoryRange
    {
        public VkStructureType sType;
        public void* pNext;
        public GenDeviceMemory memory;
        public ulong offset;
        public ulong size;
    }
}