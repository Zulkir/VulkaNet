using System.Runtime.InteropServices;
using VulkaNet.InternalHelpers;

namespace VulkaNet
{
    public interface IVkLayerProperties
    {
        string LayerName { get; }
        uint SpecVersion { get; }
        uint ImplementationVersion { get; }
        string Description { get; }
    }

    public unsafe class VkLayerProperties : IVkLayerProperties
    {
        public VkLayerProperties() { }

        public string LayerName { get; set; }
        public uint SpecVersion { get; set; }
        public uint ImplementationVersion { get; set; }
        public string Description { get; set; }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public fixed byte layerName[VkConstants.MaxExtensionNameSize];
            public uint specVersion;
            public uint implementationVersion;
            public fixed byte description[VkConstants.MaxDescriptionSize];
        }

        public VkLayerProperties(Raw* raw)
        {
            LayerName = VkHelpers.ToString(raw->layerName);
            SpecVersion = raw->specVersion;
            ImplementationVersion = raw->implementationVersion;
            Description = VkHelpers.ToString(raw->description);
        }
    }
}
