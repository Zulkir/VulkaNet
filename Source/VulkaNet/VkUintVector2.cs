using System.Runtime.InteropServices;

namespace VulkaNet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VkUintVector2
    {
        public uint X, Y;

        public VkUintVector2(uint x, uint y)
        {
            X = x;
            Y = y;
        }

        public unsafe VkUintVector2(uint* rawArray)
        {
            X = rawArray[0];
            Y = rawArray[1];
        }
    }
}