using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenImageCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkImageCreateFlags flags;
        public VkImageType imageType;
        public VkFormat format;
        public VkExtent3D extent;
        public int mipLevels;
        public int arrayLayers;
        public VkSampleCount samples;
        public VkImageTiling tiling;
        public VkImageUsageFlags usage;
        public VkSharingMode sharingMode;
        [CountFor("QueueFamilyIndices")] public int queueFamilyIndexCount;
        [IsArray] public int* pQueueFamilyIndices;
        public VkImageLayout initialLayout;
    }
}