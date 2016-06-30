namespace VulkaNetGenerator
{
    public unsafe struct GenInstanceCreateInfo
    {
        public GenStructureType sType;
        public void* pNext;
        public GenInstanceCreateFlags flags;
        public GenApplicationInfo* pApplicationInfo;
        public int enabledLayerCount;
        public byte** ppEnabledLayerNames;
        public int enabledExtensionCount;
        public byte** ppEnabledExtensionNames;
    }
}