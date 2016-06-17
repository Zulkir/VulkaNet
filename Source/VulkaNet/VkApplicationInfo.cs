using System.Runtime.InteropServices;

namespace VulkaNet
{
    public class VkApplicationInfo
    {
        public IVkStructWrapper Next { get; set; }
        public string ApplicationName { get; set; }
        public uint ApplicationVersion { get; set; }
        public string EngineName { get; set; }
        public uint EngineVersion { get; set; }
        public VkApiVersion ApiVersion { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public byte* pApplicationName;
            public uint applicationVersion;
            public byte* pEngineName;
            public uint engineVersion;
            public int apiVersion;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static class VkApplicationInfoExtensions
    {
        public static int SafeMarshalSize(this VkApplicationInfo s)
            => s != null
                ? s.Next.SafeMarshalSize() +
                  s.ApplicationName.SafeMarshalSize() +
                  s.EngineName.SafeMarshalSize() +
                  VkApplicationInfo.Raw.SizeInBytes
                : 0;

        public static unsafe VkApplicationInfo.Raw* SafeMarshalTo(this VkApplicationInfo s, ref byte* unmanged)
        {
            if (s == null)
                return (VkApplicationInfo.Raw*)0;

            var pNext = s.Next.SafeMarshalTo(ref unmanged);
            var pApplicationName = s.ApplicationName.SafeMarshalTo(ref unmanged);
            var pEngineName = s.EngineName.SafeMarshalTo(ref unmanged);

            var result = (VkApplicationInfo.Raw*)unmanged;
            unmanged += VkApplicationInfo.Raw.SizeInBytes;
            result->sType = VkStructureType.InstanceCreateInfo;
            result->pNext = pNext;
            result->pApplicationName = pApplicationName;
            result->applicationVersion = s.ApplicationVersion;
            result->pEngineName = pEngineName;
            result->engineVersion = s.EngineVersion;
            result->apiVersion = s.ApiVersion.Raw;
            return result;
        }
    }
}