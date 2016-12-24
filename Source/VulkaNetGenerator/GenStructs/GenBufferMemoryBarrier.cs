using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenBufferMemoryBarrier
    {
        public VkStructureType sType;
        public void* pNext;
        public VkAccessFlags srcAccessMask;
        public VkAccessFlags dstAccessMask;
        public int srcQueueFamilyIndex;
        public int dstQueueFamilyIndex;
        public GenBuffer buffer;
        public ulong offset;
        public ulong size;
    }
}