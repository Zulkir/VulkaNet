using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenImageMemoryBarrier
    {
        public VkStructureType sType;
        public void* pNext;
        public VkAccessFlags srcAccessMask;
        public VkAccessFlags dstAccessMask;
        public VkImageLayout oldLayout;
        public VkImageLayout newLayout;
        public int srcQueueFamilyIndex;
        public int dstQueueFamilyIndex;
        public GenImage image;
        public VkImageSubresourceRange subresourceRange;
    }
}