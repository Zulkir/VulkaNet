namespace VulkaNetGenerator
{
    public unsafe struct GenCommandBufferAllocateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public HndCommandPool commandPool;
        public VkCommandBufferLevel level;
        public int commandBufferCount;
    }
}