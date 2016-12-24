using System;

namespace VulkaNet
{
    [Flags]
    public enum VkDependencyFlags
    {
        None = 0,
        ByRegion = 0x00000001
    }
}