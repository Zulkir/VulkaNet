using System;

namespace VulkaNet
{
    public static class VkHandledObjectExtensions
    {
        public static IntPtr SafeGetHandle(this IVkHandledObject obj) { return obj?.Handle ?? IntPtr.Zero; }
    }
}