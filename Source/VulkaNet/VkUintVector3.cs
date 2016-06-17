using System.Runtime.InteropServices;

namespace VulkaNet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VkUintVector3
    {
        public uint X, Y, Z;

        public VkUintVector3(uint x, uint y, uint z)
        {
            X = x;
            Y = y;
            Z = y;
        }

        public unsafe VkUintVector3(uint* rawArray)
        {
            X = rawArray[0];
            Y = rawArray[1];
            Z = rawArray[2];
        }
    }
}