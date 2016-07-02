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

namespace VulkaNetGenerator
{
    public class WrapperProperty
    {
        public RawField RawField { get; }
        public RawField CountField { get; }
        public string TypeStr { get; }
        public string Name { get; }
        public bool MarshalledAsUnmanaged { get; }

        public WrapperProperty(RawField rawField, RawField countField)
        {
            RawField = rawField;
            CountField = countField;

            TypeStr = DeriveTypeStr(rawField);
            Name = DeriveName(rawField);
            MarshalledAsUnmanaged = IsMarshalledAsUnamanaged(rawField);
        }

        private static string DeriveTypeStr(RawField rawField)
        {
            if (rawField.Name == "pNext")
                return "IVkStructWrapper";
            return DeriveTypeInternal(rawField.TypeStr);
        }

        private static string DeriveTypeInternal(string rawTypeStr)
        {
            if (rawTypeStr.EndsWith("**"))
                return $"IReadOnlyList<{DeriveTypeInternal(rawTypeStr.Substring(0, rawTypeStr.Length - 1))}>";
            if (rawTypeStr.EndsWith(".Raw*"))
                return "I" + rawTypeStr.Substring(0, rawTypeStr.Length - 5);
            if (rawTypeStr == "byte*")
                return "string";
            return rawTypeStr;
        }

        private static string DeriveName(RawField rawField)
        {
            if (IsDoublePointerName(rawField.Name))
                return rawField.Name.Substring(2);
            if (IsSinglePointerName(rawField.Name))
                return rawField.Name.Substring(1);
            return "" + char.ToUpper(rawField.Name[0]) + rawField.Name.Substring(1);
        }

        private static bool IsDoublePointerName(string name) =>
            name[0] == 'p' && name[1] == 'p' && char.IsUpper(name[2]);

        private static bool IsSinglePointerName(string name) =>
            name[0] == 'p' && char.IsUpper(name[1]);

        private static bool IsMarshalledAsUnamanaged(RawField rawField) => 
            rawField.IsUnmanagedPtr;
    }
}