namespace VulkaNetGenerator
{
    public unsafe struct GenDeviceQueueCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkDeviceQueueCreateFlags flags;
        public int queueFamilyIndex;
        [CountFor("QueuePriorities")]
        public int queueCount;
        public float* pQueuePriorities;
    }
}