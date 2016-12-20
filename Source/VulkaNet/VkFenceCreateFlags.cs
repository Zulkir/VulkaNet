using System;

namespace VulkaNet
{
    [Flags]
    public enum VkFenceCreateFlags
    {
        None = 0,
        Signaled = 0x00000001
    }
}