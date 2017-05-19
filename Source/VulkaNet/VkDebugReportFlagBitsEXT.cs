using System;

namespace VulkaNet
{
    [Flags]
    public enum VkDebugReportFlagBitsEXT
    {
        Information = 0x00000001,
        Warning = 0x00000002,
        PerformanceWarning = 0x00000004,
        Error = 0x00000008,
        Debug = 0x00000010,
    }
}