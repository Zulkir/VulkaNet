using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenImageViewCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkImageViewCreateFlags flags;
        public GenImage image;
        public VkImageViewType viewType;
        public VkFormat format;
        public VkComponentMapping components;
        public VkImageSubresourceRange subresourceRange;
    }
}