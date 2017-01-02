#region License
/*
Copyright (c) 2016 VulkaNet Project - Daniil Rodin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

using System.Runtime.InteropServices;

namespace VulkaNet
{
    public unsafe struct VkPhysicalDeviceProperties
    {
        public uint ApiVersion { get; set; }
        public uint DriverVersion { get; set; }
        public uint VendorID { get; set; }
        public uint DeviceID { get; set; }
        public VkPhysicalDeviceType DeviceType { get; set; }
        public string DeviceName { get; set; }
        public VkUuid PipelineCacheUUID { get; set; }
        public VkPhysicalDeviceLimits Limits { get; set; }
        public VkPhysicalDeviceSparseProperties SparseProperties { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
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

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }

        public VkPhysicalDeviceProperties(Raw* raw)
        {
            ApiVersion = raw->apiVersion;
            DriverVersion = raw->driverVersion;
            VendorID = raw->vendorID;
            DeviceID = raw->deviceID;
            DeviceType = raw->deviceType;
            DeviceName = VkHelpers.ToString(raw->deviceName);
            PipelineCacheUUID = new VkUuid(raw->pipelineCacheUUID);
            Limits = new VkPhysicalDeviceLimits(&raw->limits);
            SparseProperties = new VkPhysicalDeviceSparseProperties(&raw->sparseProperties);
        }
    }
}
