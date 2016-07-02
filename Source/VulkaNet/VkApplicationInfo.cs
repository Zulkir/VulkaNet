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

using System.Runtime.InteropServices;

namespace VulkaNet
{
    public interface IVkApplicationInfo
    {
        IVkStructWrapper Next { get; }
        string ApplicationName { get; }
        uint ApplicationVersion { get; }
        string EngineName { get; }
        uint EngineVersion { get; }
        VkApiVersion ApiVersion { get; }
    }

    public unsafe class VkApplicationInfo : IVkApplicationInfo
    {
        public IVkStructWrapper Next { get; set; }
        public string ApplicationName { get; set; }
        public uint ApplicationVersion { get; set; }
        public string EngineName { get; set; }
        public uint EngineVersion { get; set; }
        public VkApiVersion ApiVersion { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public byte* pApplicationName;
            public uint applicationVersion;
            public byte* pEngineName;
            public uint engineVersion;
            public VkApiVersion apiVersion;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkApplicationInfoExtensions
    {
        public static int SafeMarshalSize(this IVkApplicationInfo s)
            => s != null ?
                s.Next.SafeMarshalSize() +
                s.ApplicationName.SafeMarshalSize() +
                s.EngineName.SafeMarshalSize() +
                VkApplicationInfo.Raw.SizeInBytes
            : 0;

        public static VkApplicationInfo.Raw* SafeMarshalTo(this IVkApplicationInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkApplicationInfo.Raw*)0;

            var pNext = s.Next.SafeMarshalTo(ref unmanaged);
            var pApplicationName = s.ApplicationName.SafeMarshalTo(ref unmanaged);
            var pEngineName = s.EngineName.SafeMarshalTo(ref unmanaged);

            var result = (VkApplicationInfo.Raw*)unmanaged;
            unmanaged += VkApplicationInfo.Raw.SizeInBytes;
            result->sType = VkStructureType.ApplicationInfo;
            result->pNext = pNext;
            result->pApplicationName = pApplicationName;
            result->applicationVersion = s.ApplicationVersion;
            result->pEngineName = pEngineName;
            result->engineVersion = s.EngineVersion;
            result->apiVersion = s.ApiVersion;
            return result;
        }
    }
}
