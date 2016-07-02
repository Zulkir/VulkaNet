namespace VulkaNetGenerator
{
    public unsafe struct GenInstanceCreateInfo
    {
        public GenStructureType sType;
        public void* pNext;
        public GenInstanceCreateFlags flags;
        public GenApplicationInfo* pApplicationInfo;
        [CountFor("EnabledLayerNames")]
        public int enabledLayerCount;
        public byte** ppEnabledLayerNames;
        [CountFor("EnabledExtensionNames")]
        public int enabledExtensionCount;
        public byte** ppEnabledExtensionNames;
    }
}