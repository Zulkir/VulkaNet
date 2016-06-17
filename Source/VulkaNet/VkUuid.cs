using VulkaNet.InternalHelpers;

namespace VulkaNet
{
    public struct VkUuid
    {
        public VkBlob16 Raw;

        public unsafe VkUuid(byte* rawArray)
        {
            Raw = *(VkBlob16*)rawArray;
        }
    }
}