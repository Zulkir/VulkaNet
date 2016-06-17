using System.Runtime.InteropServices;

namespace VulkaNet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VkVector2
    {
        public float X, Y;

        public VkVector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public unsafe VkVector2(float* rawArray)
        {
            X = rawArray[0];
            Y = rawArray[1];
        }
    }
}