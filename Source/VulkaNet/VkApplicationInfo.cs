using System.Collections.Generic;
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
