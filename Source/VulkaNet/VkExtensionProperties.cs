using System.Runtime.InteropServices;
using VulkaNet.InternalHelpers;

namespace VulkaNet
{
    public interface IVkExtensionProperties
    {
        string ExtensionName { get; }
        uint SpecVersion { get; }
    }

    public unsafe class VkExtensionProperties : IVkExtensionProperties
    {
        public VkExtensionProperties() { }

        public string ExtensionName { get; set; }
        public uint SpecVersion { get; set; }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public fixed byte extensionName[VkConstants.MaxExtensionNameSize];
            public uint specVersion;
        }

        public VkExtensionProperties(Raw* raw)
        {
            ExtensionName = VkHelpers.ToString(raw->extensionName);
            SpecVersion = raw->specVersion;
        }
    }
}
