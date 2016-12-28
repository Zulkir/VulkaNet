using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenBufferViewCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkBufferViewCreateFlags flags;
        public GenBuffer buffer;
        public VkFormat format;
        public ulong offset;
        public ulong range;
    }
}