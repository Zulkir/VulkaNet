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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VulkaNetGenerator
{
    public class InStructGenerator
    {
        public void Generate<T>()
        {
            var type = typeof(T);
            var name = type.Name.Substring(3);

            if (!Directory.Exists("GeneratedSource"))
                Directory.CreateDirectory("GeneratedSource");

            using (var stream = File.Open($"GeneratedSource/Vk{name}.cs", FileMode.Create))
            using (var writer = new CodeWriter(stream))
            {
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("using System.Runtime.InteropServices;");
                writer.WriteLine();

                writer.WriteLine("namespace VulkaNet");
                using (writer.Curly())
                {
                    var rawFields = BuildRawFields(type);
                    var wrapperProperties = BuildWrapperProps(rawFields);

                    writer.WriteLine($"public interface IVk{name}");
                    using (writer.Curly())
                    {
                        foreach (var prop in wrapperProperties)
                            writer.WriteLine($"{prop.TypeStr} {prop.Name} {{ get; }}");
                    }
                    writer.WriteLine();

                    writer.WriteLine($"public unsafe class Vk{name} : IVk{name}");
                    using (writer.Curly())
                    {
                        foreach (var prop in wrapperProperties)
                            writer.WriteLine($"public {prop.TypeStr} {prop.Name} {{ get; set; }}");
                        writer.WriteLine();

                        writer.WriteLine("[StructLayout(LayoutKind.Sequential)]");
                        writer.WriteLine("public struct Raw");
                        using (writer.Curly())
                        {
                            foreach (var field in rawFields)
                                writer.WriteLine($"public {field.TypeStr} {field.Name};");
                            writer.WriteLine();

                            writer.WriteLine("public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();");
                        }
                    }
                    writer.WriteLine();

                    writer.WriteLine($"public static unsafe class Vk{name}Extensions");
                    using (writer.Curly())
                    {
                        writer.WriteLine($"public static int SafeMarshalSize(this IVk{name} s)");
                        writer.Tab();
                        writer.WriteLine("=> s != null ?");
                        writer.Tab();
                        foreach (var prop in wrapperProperties.Where(x => x.MarshalledAsUnmanaged))
                            writer.WriteLine($"s.{prop.Name}.SafeMarshalSize() +");
                        writer.WriteLine($"Vk{name}.Raw.SizeInBytes");
                        writer.UnTab();
                        writer.WriteLine(": 0;");
                        writer.UnTab();
                        writer.WriteLine();

                        writer.WriteLine($"public static Vk{name}.Raw* SafeMarshalTo(this IVk{name} s, ref byte* unmanaged)");
                        using (writer.Curly())
                        {
                            writer.WriteLine("if (s == null)");
                            writer.Tab();
                            writer.WriteLine($"return (Vk{name}.Raw*)0;");
                            writer.UnTab();
                            writer.WriteLine();

                            foreach (var prop in wrapperProperties.Where(x => x.MarshalledAsUnmanaged))
                                writer.WriteLine($"var {prop.RawField.Name} = s.{prop.Name}.SafeMarshalTo(ref unmanaged);");
                            writer.WriteLine();

                            writer.WriteLine($"var result = (Vk{name}.Raw*)unmanaged;");
                            writer.WriteLine($"unmanaged += Vk{name}.Raw.SizeInBytes;");
                            foreach (var field in rawFields)
                            {
                                var rval = field.Name == "sType" ? $"VkStructureType.{name}" :
                                           field.IsUnmanagedPtr ? $"{field.Name}" :
                                           field.IsCountFor != null ? $"s.{field.IsCountFor}?.Count ?? 0" :
                                           $"s.{wrapperProperties.Single(x => x.RawField == field).Name}";
                                writer.WriteLine($"result->{field.Name} = {rval};");
                            }
                            writer.WriteLine("return result;");
                        }
                    }
                }
            }   
        }

        private static RawField[] BuildRawFields(Type type) => 
            type.GetFields().Select(x => new RawField(x)).ToArray();

        private static WrapperProperty[] BuildWrapperProps(IReadOnlyList<RawField> rawFields) => 
            rawFields.Where(x => !x.IgnoreInWrapper).Select(x => new WrapperProperty(x, null)).ToArray();
    }
}