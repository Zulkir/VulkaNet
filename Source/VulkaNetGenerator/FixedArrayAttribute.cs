using System;

namespace VulkaNetGenerator
{
    public class FixedArrayAttribute : Attribute
    {
        public string SizeConst { get; }

        public FixedArrayAttribute(string sizeConst) { SizeConst = sizeConst; }
    }
}