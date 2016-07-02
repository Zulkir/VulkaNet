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