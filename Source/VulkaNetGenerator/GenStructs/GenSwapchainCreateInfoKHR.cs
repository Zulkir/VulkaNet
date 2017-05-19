using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenSwapchainCreateInfoKHR
    {
        public VkStructureType sType;
        public void* pNext;
        public VkSwapchainCreateFlagsKHR flags;
        public GenSurfaceKHR surface;
        public int minImageCount;
        public VkFormat imageFormat;
        public VkColorSpaceKHR imageColorSpace;
        public VkExtent2D imageExtent;
        public int imageArrayLayers;
        public VkImageUsageFlags imageUsage;
        public VkSharingMode imageSharingMode;
        [CountFor("QueueFamilyIndices")]
        public int queueFamilyIndexCount;
        [IsArray]
        public int* pQueueFamilyIndices;
        public VkSurfaceTransformFlagBitsKHR preTransform;
        public VkCompositeAlphaFlagsKHR compositeAlpha;
        public VkPresentModeKHR presentMode;
        public VkBool32 clipped;
        public GenSwapchainKHR oldSwapchain;
    }
}