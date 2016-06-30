using System;
using System.Reflection;

namespace VulkaNetGenerator
{
    public class RawField
    {
        //public Type Type { get; }
        public string TypeStr { get; }
        public string Name { get; }
        public bool IgnoreInWrapper { get; }
        public bool IsUnmanagedPtr { get; }

        public RawField(FieldInfo fieldInfo)
        {
            //Type = fieldInfo.FieldType;
            TypeStr = DeriveTypeStr(fieldInfo.FieldType);
            Name = fieldInfo.Name;
            if (Name == "sType")
                IgnoreInWrapper = true;
            else
                IgnoreInWrapper = false;
            IsUnmanagedPtr = fieldInfo.FieldType.IsPointer;
        }

        private static string DeriveTypeStr(Type type)
        {
            if (type.Name.StartsWith("Gen"))
                return "Vk" + type.Name.Substring(3);
            switch (type.Name)
            {
                case "Void*": return "void*";
                case "Byte": return "byte";
                case "Byte*": return "byte*";
                case "Byte**": return "byte**";
                case "Int32": return "int";
                case "Int32*": return "int*";
                case "Int32**": return "int**";
                case "UInt32": return "uint";
                case "UInt32*": return "uint*";
                case "UInt32**": return "uint**";
            }
            return type.Name;
        }
    }
}