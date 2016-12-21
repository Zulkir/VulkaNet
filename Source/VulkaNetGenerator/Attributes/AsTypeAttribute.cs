using System;

namespace VulkaNetGenerator.Attributes
{
    public class AsTypeAttribute : Attribute
    {
        public string TypeStr { get; }

        public AsTypeAttribute(string typeStr) { TypeStr = typeStr; }
    }
}