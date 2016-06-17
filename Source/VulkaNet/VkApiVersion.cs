using System;

namespace VulkaNet
{
    public struct VkApiVersion : IEquatable<VkApiVersion>, IComparable<VkApiVersion>
    {
        public int Raw { get; }

        public int Major => ((1 << 10) - 1) & (Raw >> 22);
        public int Minor => ((1 << 10) - 1) & (Raw >> 12);
        public int Patch => ((1 << 12) - 1) & Raw;

        public VkApiVersion(uint raw)
        {
            Raw = (int)raw;
        }

        public VkApiVersion(int major, int minor, int patch)
        {
            Raw = patch | (minor << 12) | (major << 22);
        }

        #region Equality, Hash, String
        public override int GetHashCode() => Raw.GetHashCode();
        public override string ToString() => $"{Major}.{Minor}.{Patch}";
        public static bool Equals(VkApiVersion s1, VkApiVersion s2) => s1.Raw == s2.Raw;
        public static bool operator ==(VkApiVersion s1, VkApiVersion s2) => Equals(s1, s2);
        public static bool operator !=(VkApiVersion s1, VkApiVersion s2) => !Equals(s1, s2);
        public bool Equals(VkApiVersion other) => Equals(this, other);
        public override bool Equals(object obj) => obj is VkApiVersion && Equals((VkApiVersion)obj);
        #endregion

        public int CompareTo(VkApiVersion other) => ((uint)Raw).CompareTo((uint)other.Raw);
    }
}