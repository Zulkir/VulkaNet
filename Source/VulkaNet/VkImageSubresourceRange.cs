namespace VulkaNet
{
    public struct VkImageSubresourceRange
    {
        public VkImageAspectFlags AspectMask;
        public int BaseMipLevel;
        public int LevelCount;
        public int BaseArrayLayer;
        public int LayerCount;
    }
}