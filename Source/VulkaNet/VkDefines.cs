namespace VulkaNet
{
    public static class VkDefines
    {
        public static VkBool32 VK_FALSE { get; } = new VkBool32(false);
        public static VkBool32 VK_TRUE { get; } = new VkBool32(true);

        public const float VK_LOD_CLAMP_NONE = 1000.0f;
        public const int VK_REMAINING_MIP_LEVELS = unchecked((int)~0U);
        public const int VK_REMAINING_ARRAY_LAYERS = unchecked((int)~0U);
        public const ulong VK_WHOLE_SIZE = ~0UL;
        public const int VK_ATTACHMENT_UNUSED = unchecked((int)~0U);
        public const int VK_QUEUE_FAMILY_IGNORED = unchecked((int)~0U);
        public const int VK_SUBPASS_EXTERNAL = unchecked((int)~0U);
        public const int VK_MAX_PHYSICAL_DEVICE_NAME_SIZE = 256;
        public const int VK_UUID_SIZE = 16;
        public const int VK_MAX_MEMORY_TYPES = 32;
        public const int VK_MAX_MEMORY_HEAPS = 16;
        public const int VK_MAX_EXTENSION_NAME_SIZE = 256;
        public const int VK_MAX_DESCRIPTION_SIZE = 256;

        public const string VK_EXT_DEBUG_REPORT_EXTENSION_NAME = "VK_EXT_debug_report";
    }
}