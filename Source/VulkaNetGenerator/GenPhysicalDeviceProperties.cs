namespace VulkaNetGenerator
{
    public unsafe struct GenPhysicalDeviceProperties
    {
        public uint apiVersion;
        public uint driverVersion;
        public uint vendorID;
        public uint deviceID;
        public VkPhysicalDeviceType deviceType;
        [FixedArray("VkConstants.MaxPhysicalDeviceNameSize")]
        public StrByte* deviceName;
        [AsType("VkUuid"), FixedArray("VkConstants.UuidSize")]
        public byte* pipelineCacheUUID;
        public GenPhysicalDeviceLimits limits;
        public GenPhysicalDeviceSparseProperties sparseProperties;
    }
}