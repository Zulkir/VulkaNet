using System.Collections.Generic;

namespace VulkaNet
{
    public struct VkRectLayerKHR
    {
        public VkOffset2D Offset;
        public VkExtent2D Extent;
        public int Layer;
    }

    public static unsafe class VkRectLayerKHRExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<VkRectLayerKHR> list) =>
            list.SizeOfMarshalDirect(sizeof(VkRectLayerKHR), x => 0);

        public static VkRectLayerKHR* MarshalDirect(this IReadOnlyList<VkRectLayerKHR> list, ref byte* unmanaged) =>
            (VkRectLayerKHR*)list.MarshalDirect(ref unmanaged, (e, d) => { *(VkRectLayerKHR*)d = e; }, sizeof(VkRectLayerKHR));
    }
}