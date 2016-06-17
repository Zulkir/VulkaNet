using System.Runtime.InteropServices;
using VulkaNet.InternalHelpers;

namespace VulkaNet
{
    public interface IVkPhysicalDeviceProperties
    {
        VkApiVersion ApiVersion { get; }
        uint DriverVersion { get; }
        uint VendorId { get; }
        uint DeviceId { get; }
        VkPhysicalDeviceType DeviceType { get; }
        string DeviceName { get; }
        VkUuid PipelineCacheUuid { get; }
        IVkPhysicalDeviceLimits Limits { get; }
        VkPhysicalDeviceSparseProperties SparseProperties { get; }
    }

    public class VkPhysicalDeviceProperties : IVkPhysicalDeviceProperties
    {
        public VkPhysicalDeviceProperties() { }

        public VkApiVersion ApiVersion { get; set; }
        public uint DriverVersion { get; set; }
        public uint VendorId { get; set; }
        public uint DeviceId { get; set; }
        public VkPhysicalDeviceType DeviceType { get; set; }
        public string DeviceName { get; set; }
        public VkUuid PipelineCacheUuid { get; set; }
        public IVkPhysicalDeviceLimits Limits { get; set; }
        public VkPhysicalDeviceSparseProperties SparseProperties { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Raw
        {
            public uint apiVersion;
            public uint driverVersion;
            public uint vendorID;
            public uint deviceID;
            public VkPhysicalDeviceType deviceType;
            public fixed byte deviceName[VkConstants.MaxPhysicalDeviceNameSize];
            public fixed byte pipelineCacheUUID[VkConstants.UuidSize];
            public VkPhysicalDeviceLimits.Raw limits;
            public VkPhysicalDeviceSparseProperties.Raw sparseProperties;
        }

        public unsafe VkPhysicalDeviceProperties(Raw* raw)
        {
            ApiVersion = new VkApiVersion(raw->apiVersion);
            DriverVersion = raw->driverVersion;
            VendorId = raw->vendorID;
            DeviceId = raw->deviceID;
            DeviceType = raw->deviceType;
            DeviceName = VkHelpers.ToString(raw->deviceName);
            PipelineCacheUuid = new VkUuid(raw->pipelineCacheUUID);
            Limits = new VkPhysicalDeviceLimits(&raw->limits);
            SparseProperties = new VkPhysicalDeviceSparseProperties(&raw->sparseProperties);
        }
    }
}