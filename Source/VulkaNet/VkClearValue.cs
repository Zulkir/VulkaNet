using System.Collections.Generic;

namespace VulkaNet
{
    // todo: implement
    public unsafe struct VkClearValue
    {
        public int X;
        public int Y;
        public int Z;
        public int W;
    }

    public static unsafe class VkClearValueExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<VkClearValue> list) =>
            list.SizeOfMarshalDirect(sizeof(VkClearValue), x => 0);

        public static VkClearValue* MarshalDirect(this IReadOnlyList<VkClearValue> list, ref byte* unmanaged) =>
            (VkClearValue*)list.MarshalDirect(ref unmanaged, (elem, dst) => { *(VkClearValue*)dst = elem; }, sizeof(VkClearValue));
    }
}