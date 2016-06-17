using System.Runtime.InteropServices;

namespace VulkaNet
{
    public interface IVkPhysicalDeviceSparseProperties
    {
        bool ResidencyStandard2DBlockShape { get; }
        bool ResidencyStandard2DMultisampleBlockShape { get; }
        bool ResidencyStandard3DBlockShape { get; }
        bool ResidencyAlignedMipSize { get; }
        bool ResidencyNonResidentStrict { get; }
    }

    public class VkPhysicalDeviceSparseProperties : IVkPhysicalDeviceSparseProperties
    {
        public VkPhysicalDeviceSparseProperties() { }

        public bool ResidencyStandard2DBlockShape { get; set; }
        public bool ResidencyStandard2DMultisampleBlockShape { get; set; }
        public bool ResidencyStandard3DBlockShape { get; set; }
        public bool ResidencyAlignedMipSize { get; set; }
        public bool ResidencyNonResidentStrict { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkBool32 residencyStandard2DBlockShape;
            public VkBool32 residencyStandard2DMultisampleBlockShape;
            public VkBool32 residencyStandard3DBlockShape;
            public VkBool32 residencyAlignedMipSize;
            public VkBool32 residencyNonResidentStrict;
        }

        public unsafe VkPhysicalDeviceSparseProperties(Raw* raw)
        {
            ResidencyStandard2DBlockShape = raw->residencyStandard2DBlockShape.Value;
            ResidencyStandard2DMultisampleBlockShape = raw->residencyStandard2DMultisampleBlockShape.Value;
            ResidencyStandard3DBlockShape = raw->residencyStandard3DBlockShape.Value;
            ResidencyAlignedMipSize = raw->residencyAlignedMipSize.Value;
            ResidencyNonResidentStrict = raw->residencyNonResidentStrict.Value;
        }
    }
}