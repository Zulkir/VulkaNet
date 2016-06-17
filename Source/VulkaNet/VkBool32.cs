using System.Runtime.InteropServices;

namespace VulkaNet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VkBool32
    {
        private int raw;

        public bool Value {  get { return raw != 0; } set { raw = value ? 1 : 0; } }

        public VkBool32(int raw) { this.raw = raw; }
        public VkBool32(bool value) { raw = value ? 1 : 0; }
    }
}