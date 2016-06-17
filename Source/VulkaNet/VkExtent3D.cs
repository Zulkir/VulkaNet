namespace VulkaNet
{
    public struct VkExtent3D
    {
        public int Width;
        public int Height;
        public int Depth;

        public VkExtent3D(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }
    }
}
