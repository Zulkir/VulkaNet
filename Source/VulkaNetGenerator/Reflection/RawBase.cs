﻿#region License
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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.Reflection
{
    public class RawBase
    {
        public Type GenType { get; }
        public string TypeStr { get; protected set; }
        public string Name { get; }
        public bool IgnoreInWrapper { get; protected set; }
        public bool ShouldMarshal { get; protected set; }
        public bool IsUnmanagedPtr { get; }
        public string IsCountFor { get; }
        public string ExplicitWrapperType { get; }
        public string FixedArraySize { get; }
        public bool IsArray { get; }
        public bool IsHandle { get; }
        public string FromProperty { get; }
        public bool IsActuallyStruct { get; }
        public bool IsByteArray { get; }
        public string IsByteArraySizeFor { get; }
        public bool IsNullable { get; }

        protected readonly Type genType;
        protected readonly CustomAttributeData[] attributes;

        public RawBase(Type genType, string name, CustomAttributeData[] attributes)
        {
            this.genType = genType;
            this.attributes = attributes;

            GenType = genType;
            Name = name;
            IsHandle = DeriveIsHandle(genType);
            FixedArraySize = GetAttrValue<FixedArrayAttribute>(attributes);
            TypeStr = DeriveTypeStr(genType, FixedArraySize, IsHandle);
            IsUnmanagedPtr = genType.IsPointer;
            IsCountFor = GetAttrValue<CountForAttribute>(attributes);
            ExplicitWrapperType = DeriveExplicitWrapperType(genType, attributes);
            IsArray = DeriveIsArray(attributes);
            ShouldMarshal = DeriveSholdMarshal(genType, IsUnmanagedPtr, IsArray);
            FromProperty = GetAttrValue<FromPropertyAttribute>(attributes);
            IsActuallyStruct = DeriveIsActuallyStruct(genType);
            IsByteArray = HasAttribute<ByteArrayAttribute>(attributes);
            IsByteArraySizeFor = GetAttrValue<ByteArraySizeAttribute>(attributes);
            IsNullable = HasAttribute<NullableAttribute>(attributes);
            IgnoreInWrapper = Name == "sType" || IsCountFor != null || IsByteArraySizeFor != null || HasAttribute<ReturnSizeAttribute>(attributes);
        }
        
        private static string DeriveTypeStr(Type type, string fixedBufferSize, bool isHandle)
        {
            if (fixedBufferSize != null)
                return DeriveTypeStr(type.GetElementType(), null, isHandle);

            if (isHandle)
                return type.IsPointer 
                    ? $"Vk{type.Name.Substring(3, type.Name.Length - 4)}.HandleType*" 
                    : $"Vk{type.Name.Substring(3, type.Name.Length - 3)}.HandleType";

            if (type.Name.StartsWith("Gen"))
                return type.IsPointer 
                    ? $"Vk{type.Name.Substring(3, type.Name.Length - 4)}.Raw*" 
                    : $"Vk{type.Name.Substring(3, type.Name.Length - 3)}.Raw";
            
            switch (type.Name)
            {
                case "StrByte": return "byte";
                case "StrByte*": return "byte*";
                case "StrByte**": return "byte**";

                case "Sizet": return "IntPtr";
                case "Sizet*": return "IntPtr*";

                case "DeviceSize": return "ulong";
                case "DeviceSize*": return "ulong*";

                case "Void*": return "void*";
                case "Byte": return "byte";
                case "Byte*": return "byte*";
                case "Byte**": return "byte**";
                case "Single": return "float";
                case "Single*": return "float*";
                case "Single**": return "float**";
                case "Int32": return "int";
                case "Int32*": return "int*";
                case "Int32**": return "int**";
                case "UInt32": return "uint";
                case "UInt32*": return "uint*";
                case "UInt32**": return "uint**";
                case "UInt64": return "ulong";
                case "UInt64*": return "ulong*";
                case "UInt64**": return "ulong**";
            }
            return type.Name;
        }

        private static string DeriveExplicitWrapperType(Type genType, CustomAttributeData[] attributes)
        {
            var attrValue = GetAttrValue<AsTypeAttribute>(attributes);
            if (attrValue != null)
                return attrValue;
            switch (genType.Name)
            {
                case "StrByte*": return "string";
                case "StrByte**": return "IReadOnlyList<string>";
                case "Sizet": return "IntPtr";
                case "DeviceSize": return "ulong";
            }
            return null;
        }

        private static bool DeriveIsArray(CustomAttributeData[] attributes) =>
            HasAttribute<IsArrayAttribute>(attributes) ||
            HasAttribute<FixedArrayAttribute>(attributes);

        private static bool DeriveIsHandle(Type genType)
        {
            var internalType = genType.IsPointer ? genType.GetElementType() : genType;
            return
                typeof(IGenHandledObject).IsAssignableFrom(internalType) ||
                typeof(IGenNonDispatchableHandledObject).IsAssignableFrom(internalType);
        }

        private static bool DeriveSholdMarshal(Type genType, bool isPointer, bool isArray) =>
            //(genType.Name.StartsWith("Gen") || isArray) && (isPointer || DeriveIsStructRaw(genType));
            isPointer || DeriveIsStructRaw(genType);

        private static bool DeriveIsStructRaw(Type genType)
        {
            var internalType = genType.IsPointer ? genType.GetElementType() : genType;
            Debug.Assert(internalType != null, "internalType != null");
            return
                internalType.Name.StartsWith("Gen") &&
                !typeof(IGenHandledObject).IsAssignableFrom(internalType) &&
                !typeof(IGenNonDispatchableHandledObject).IsAssignableFrom(internalType);
        }

        private static bool DeriveIsActuallyStruct(Type genType)
        {
            var cleanType = genType.IsPointer ? genType.GetElementType() : genType;
            return
                cleanType.IsValueType &&
                !typeof(IGenHandledObject).IsAssignableFrom(cleanType) &&
                !typeof(IGenNonDispatchableHandledObject).IsAssignableFrom(cleanType) &&
                cleanType.GetFields().All(x => x.FieldType != typeof(VkStructureType)) &&
                cleanType.Name != "GenAllocationCallbacks";
        }

        protected static bool HasAttribute<T>(CustomAttributeData[] attributes) =>
            attributes.Any(x => x.AttributeType == typeof(T));

        protected static string GetAttrValue<T>(CustomAttributeData[] attributes) =>
            attributes.FirstOrDefault(x => x.AttributeType == typeof(T))?.ConstructorArguments[0].Value as string;
    }
}