using System;
using System.Collections.Generic;

namespace VulkaNet
{
    public struct VkSpecializationMapEntry
    {
        public int ConstantId;
        public int Offset;
        private IntPtr size;
        public int Size {  get { return (int)size; } set { size = (IntPtr)value; } }
    }

    public static unsafe class VkSpecializationMapEntryExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<VkSpecializationMapEntry> list) =>
            list.SizeOfMarshalDirect(sizeof(VkSpecializationMapEntry), x => 0);

        public static VkSpecializationMapEntry* MarshalDirect(this IReadOnlyList<VkSpecializationMapEntry> list, ref byte* unmanaged) =>
            (VkSpecializationMapEntry*)list.MarshalDirect(ref unmanaged, (elem, dst) => { *(VkSpecializationMapEntry*)dst = elem; }, sizeof(VkSpecializationMapEntry));
    }
}