using System;

namespace VulkaNet
{
    [Flags]
    public enum VkAttachmentDescriptionFlags
    {
        None = 0,
        MayAlias = 0x00000001
    }
}