using System;

namespace VulkaNetGenerator.Attributes
{
    public class ByteArraySizeAttribute : Attribute
    {
        public string Property { get; }

        public ByteArraySizeAttribute(string property)
        {
            Property = property;
        }
    }
}