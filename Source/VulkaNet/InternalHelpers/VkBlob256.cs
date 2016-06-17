using System.Runtime.InteropServices;

namespace VulkaNet.InternalHelpers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VkBlob256
    {
        public VkBlob64 Dummy0;
        public VkBlob64 Dummy64;
        public VkBlob64 Dummy128;
        public VkBlob64 Dummy192;
    }
}