using System;

namespace VulkaNetGenerator
{
    public class CountForAttribute : Attribute
    {
        public string Property { get; }

        public CountForAttribute(string property)
        {
            Property = property;
        }
    }
}