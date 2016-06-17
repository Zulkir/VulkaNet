using System.Runtime.InteropServices;

namespace VulkaNet.InternalHelpers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VkBlob64
    {
        public VkBlob16 Dummy0;
        public VkBlob16 Dummy16;
        public VkBlob16 Dummy32;
        public VkBlob16 Dummy48;
    }
}