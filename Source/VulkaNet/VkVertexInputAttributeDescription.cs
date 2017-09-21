using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VulkaNet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VkVertexInputAttributeDescription
    {
        public int Location;
        public int Binding;
        public VkFormat Format;
        public int Offset;
    }

    public static unsafe class VkVertexInputAttributeDescriptionExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<VkVertexInputAttributeDescription> list) =>
            list.SizeOfMarshalDirect(sizeof(VkVertexInputAttributeDescription), x => 0);

        public static VkVertexInputAttributeDescription* MarshalDirect(this IReadOnlyList<VkVertexInputAttributeDescription> list, ref byte* unmanaged) =>
            (VkVertexInputAttributeDescription*)list.MarshalDirect(ref unmanaged, (elem, dst) => { *(VkVertexInputAttributeDescription*)dst = elem; }, sizeof(VkVertexInputAttributeDescription));
    }
}