using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenEventCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkEventCreateFlags flags;
    }
}