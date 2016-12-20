namespace VulkaNetGenerator
{
    public unsafe struct GenFenceCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkFenceCreateFlags flags;
    }
}