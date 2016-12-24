using System;

namespace VulkaNet
{
    [Flags]
    public enum VkSampleCountFlagBits
    {
        None = 0,
        B1 = 0x00000001,
        B2 = 0x00000002,
        B4 = 0x00000004,
        B8 = 0x00000008,
        B16 = 0x00000010,
        B32 = 0x00000020,
        B64 = 0x00000040,
    }
}