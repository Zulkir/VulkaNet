using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VulkaNet
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VkAttachmentReference
    {
        public int Attachment;
        public VkImageLayout Layout;
    }

    public static unsafe class VkAttachmentReferenceExstensions
    {
        public static int SizeOfMarshalIndirect(this VkAttachmentReference s) => 
            sizeof(VkAttachmentReference);

        public static VkAttachmentReference* MarshalIndirect(this VkAttachmentReference s, ref byte* unmanaged)
        {
            var result = (VkAttachmentReference*)unmanaged;
            unmanaged += sizeof(VkAttachmentReference);
            *result = s;
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkAttachmentReference> list) =>
            list.SizeOfMarshalDirect(sizeof(VkAttachmentReference), x => 0);

        public static VkAttachmentReference* MarshalDirect(this IReadOnlyList<VkAttachmentReference> list, ref byte* unmanaged) =>
            (VkAttachmentReference*)list.MarshalDirect(ref unmanaged, (elem, dst) => { *(VkAttachmentReference*)dst = elem; }, sizeof(VkAttachmentReference));
    }
}