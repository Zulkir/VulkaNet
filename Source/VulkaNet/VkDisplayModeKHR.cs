#region License
/*
Copyright (c) 2016 VulkaNet Project - Daniil Rodin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

using System.Collections.Generic;

namespace VulkaNet
{
    public interface IVkDisplayModeKHR : IVkNonDispatchableHandledObject, IVkInstanceChild
    {
        VkDisplayModeKHR.HandleType Handle { get; }
    }

    public unsafe class VkDisplayModeKHR : IVkDisplayModeKHR
    {
        public IVkInstance Instance { get; }
        public HandleType Handle { get; }

        private VkInstance.DirectFunctions Direct => Instance.Direct;

        public ulong RawHandle => Handle.InternalHandle;

        public VkDisplayModeKHR(IVkInstance instance, HandleType handle)
        {
            Instance = instance;
            Handle = handle;
        }

        public struct HandleType
        {
            public readonly ulong InternalHandle;
            public HandleType(ulong internalHandle) { InternalHandle = internalHandle; }
            public override string ToString() => InternalHandle.ToString();
            public static int SizeInBytes { get; } = sizeof(ulong);
            public static HandleType Null => new HandleType(default(ulong));
        }

    }

    public static unsafe class VkDisplayModeKHRExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkDisplayModeKHR> list) =>
            list.SizeOfMarshalDirectNonDispatchable();

        public static VkDisplayModeKHR.HandleType* MarshalDirect(this IReadOnlyList<IVkDisplayModeKHR> list, ref byte* unmanaged) =>
            (VkDisplayModeKHR.HandleType*)list.MarshalDirectNonDispatchable(ref unmanaged);
    }
}
