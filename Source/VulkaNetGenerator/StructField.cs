using System;

namespace VulkaNetGenerator
{
    public class StructField
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string IsCountOf { get; set; }
        public int? FixedSize { get; set; }
    }
}