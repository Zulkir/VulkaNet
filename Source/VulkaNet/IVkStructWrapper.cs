﻿namespace VulkaNet
{
    public unsafe interface IVkStructWrapper
    {
        int MarshalSize();
        void* MarshalTo(ref byte* dst);
    }

    public static class VkStructWrapperExtensions
    {
        public static int SafeMarshalSize(this IVkStructWrapper s) 
            => s?.MarshalSize() ?? 0;
        public static unsafe void* SafeMarshalTo(this IVkStructWrapper s, ref byte* dst) 
            => s != null ? s.MarshalTo(ref dst) : (void*)0;
    }
}