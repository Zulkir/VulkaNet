namespace VulkaNet
{
    public struct VkRect2D
    {
        public VkOffset2D Offset;
        public VkExtent2D Extent;

        public VkRect2D(VkOffset2D offset, VkExtent2D extent)
        {
            Offset = offset;
            Extent = extent;
        }
    }
}
