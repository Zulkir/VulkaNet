namespace VulkaNetGenerator
{
    public unsafe struct GenCommandBufferBeginInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkCommandBufferUsageFlags flags;
        public GenCommandBufferInheritanceInfo* pInheritanceInfo;
    }
}