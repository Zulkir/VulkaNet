using System;

namespace VulkaNet
{
    [Flags]
    public enum VkImageAspectFlags
    {
        None = 0,
        Color = 0x00000001,
        Depth = 0x00000002,
        Stencil = 0x00000004,
        Metadata = 0x00000008,
    }
}