using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public struct GenSparseImageMemoryBind
    {
        public VkImageSubresource subresource;
        public VkOffset3D offset;
        public VkExtent3D extent;
        public GenDeviceMemory memory;
        public DeviceSize memoryOffset;
        public VkSparseMemoryBindFlags flags;
    }
}