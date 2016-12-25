using System.Runtime.InteropServices;

namespace VulkaNet
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct VkColor4
    {
        public float R;
        public float G;
        public float B;
        public float A;
    }
}