using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VulkaNet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VkVertexInputBindingDescription
    {
        public int Binding;
        public int Stride;
        public VkVertexInputRate InputRate;
    }

    public static unsafe class VkVertexInputBindingDescriptionExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<VkVertexInputBindingDescription> list) =>
            list.SizeOfMarshalDirect(sizeof(VkVertexInputBindingDescription), x => 0);

        public static VkVertexInputBindingDescription* MarshalDirect(this IReadOnlyList<VkVertexInputBindingDescription> list, ref byte* unmanaged) =>
            (VkVertexInputBindingDescription*)list.MarshalDirect(ref unmanaged, (elem, dst) => { *(VkVertexInputBindingDescription*)dst = elem; }, sizeof(VkVertexInputBindingDescription));
    }
}