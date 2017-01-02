using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public struct GenSparseMemoryBind
    {
        public DeviceSize resourceOffset;
        public DeviceSize size;
        public GenDeviceMemory memory;
        public DeviceSize memoryOffset;
        public VkSparseMemoryBindFlags flags;
    }
}