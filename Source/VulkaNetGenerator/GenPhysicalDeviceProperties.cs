namespace VulkaNetGenerator
{
    public unsafe struct GenPhysicalDeviceProperties
    {
        public uint apiVersion;
        public uint driverVersion;
        public uint vendorID;
        public uint deviceID;
        public VkPhysicalDeviceType deviceType;
        [AsType("string"), FixedArray("VkConstants.MaxPhysicalDeviceNameSize")]
        public byte* deviceName;
        [AsType("VkUuid"), FixedArray("VkConstants.UuidSize")]
        public byte* pipelineCacheUUID;
        public GenPhysicalDeviceLimits limits;
        public GenPhysicalDeviceSparseProperties sparseProperties;
    }
}