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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VulkaNetGenerator
{
    public class StructGenerator
    {
        public void Generate<T>(bool input, bool output)
        {
            var type = typeof(T);
            var name = type.Name.Substring(3);

            if (!Directory.Exists("GeneratedSource"))
                Directory.CreateDirectory("GeneratedSource");

            using (var stream = File.Open($"GeneratedSource/Vk{name}.cs", FileMode.Create))
            using (var writer = new CodeWriter(stream))
            {
                WriteLicense(writer);
                writer.WriteLine();

                var rawFields = BuildRawFields(type);
                var wrapperProperties = BuildWrapperProps(rawFields);

                //if (rawFields.Any(x => x.TypeStr.Contains("IntPtr")) || wrapperProperties.Any(x => x.TypeStr.Contains("IntPtr")))
                writer.WriteLine("using System;");
                //if (wrapperProperties.Any(x => x.TypeStr.Contains("ReadOnlyList")))
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("using System.Linq;");
                writer.WriteLine("using System.Runtime.InteropServices;");
                writer.WriteLine();

                writer.WriteLine("namespace VulkaNet");
                using (writer.Curly())
                {
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
                                if (field.FixedArraySize != null)
                                    writer.WriteLine($"public fixed {field.TypeStr} {field.Name}[{field.FixedArraySize}];");
                                else
                                    writer.WriteLine($"public {field.TypeStr} {field.Name};");
                            writer.WriteLine();

                            writer.WriteLine("public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();");
                        }
                        if (output)
                        {
                            writer.WriteLine();

                            writer.WriteLine($"public Vk{name}() {{ }}");
                            writer.WriteLine();

                            writer.WriteLine($"public Vk{name}(Raw* raw)");
                            using (writer.Curly())
                            {
                                foreach (var prop in wrapperProperties)
                                {
                                    var fieldVal = $"raw->{prop.RawField.Name}";
                                    var prefix = prop.CreatorFuncTakesPtr ? "&" : "";
                                    if (prop.CreatorFunc != null)
                                        writer.WriteLine($"{prop.Name} = {prop.CreatorFunc}({prefix}{fieldVal});");
                                    else if (prop.NeedsCast)
                                        writer.WriteLine($"{prop.Name} = ({prop.TypeStr}){fieldVal};");
                                    else
                                        writer.WriteLine($"{prop.Name} = {fieldVal};");
                                }
                            }
                        }
                    }
                    if (input)
                    {
                        writer.WriteLine();
                        
                        writer.WriteLine($"public static unsafe class Vk{name}Extensions");
                        using (writer.Curly())
                        {
                            var unmanagedProps = wrapperProperties.Where(x => x.MarshalledAsUnmanaged).ToArray();
                            writer.WriteLine($"public static int SizeOfMarshalDirect(this IVk{name} s)");
                            using (writer.Curly())
                            {
                                writer.WriteLine("if (s == null)");
                                writer.Tab();
                                writer.WriteLine("throw new InvalidOperationException(\"Trying to directly marshal a null.\");");
                                writer.UnTab();
                                writer.WriteLine();

                                if (unmanagedProps.Any())
                                {
                                    writer.WriteLine("return");
                                    writer.Tab();
                                    foreach (var prop in unmanagedProps.Take(unmanagedProps.Length - 1))
                                        writer.WriteLine($"s.{prop.Name}.{prop.SizeMethod}() +");
                                    var lastProp = unmanagedProps.Last();
                                    writer.WriteLine($"s.{lastProp.Name}.{lastProp.SizeMethod}();");
                                    writer.UnTab();
                                }
                                else
                                {
                                    writer.WriteLine("return 0;");
                                }
                            }
                            writer.WriteLine();

                            writer.WriteLine($"public static Vk{name}.Raw MarshalDirect(this IVk{name} s, ref byte* unmanaged)");
                            using (writer.Curly())
                            {
                                writer.WriteLine("if (s == null)");
                                writer.Tab();
                                writer.WriteLine("throw new InvalidOperationException(\"Trying to directly marshal a null.\");");
                                writer.UnTab();
                                writer.WriteLine();

                                foreach (var prop in unmanagedProps)
                                    writer.WriteLine($"var {prop.RawField.Name} = s.{prop.Name}.{prop.MarshalMethod}(ref unmanaged);");
                                writer.WriteLine();

                                writer.WriteLine($"Vk{name}.Raw result;");
                                foreach (var field in rawFields)
                                {
                                    var prop = wrapperProperties.SingleOrDefault(x => x.RawField == field);
                                    var rval = field.Name == "sType" ? $"VkStructureType.{name}" :
                                               field.IsUnmanagedPtr ? $"{field.Name}" :
                                               field.IsHandle ? $"s.{prop?.Name}.Handle" :
                                               field.IsCountFor != null ? $"s.{field.IsCountFor}?.Count ?? 0" :
                                               field.TypeStr == "VkBool32" ? $"new VkBool32(s.{prop?.Name})" :
                                               $"s.{prop?.Name}";
                                    writer.WriteLine($"result.{field.Name} = {rval};");
                                }
                                writer.WriteLine("return result;");
                            }
                            writer.WriteLine();

                            writer.WriteLine($"public static int SizeOfMarshalIndirect(this IVk{name} s) =>");
                            writer.Tab();
                            writer.WriteLine($"s == null ? 0 : s.SizeOfMarshalDirect() + Vk{name}.Raw.SizeInBytes;");
                            writer.UnTab();
                            writer.WriteLine();

                            writer.WriteLine($"public static Vk{name}.Raw* MarshalIndirect(this IVk{name} s, ref byte* unmanaged)");
                            using (writer.Curly())
                            {
                                writer.WriteLine($"var result = (Vk{name}.Raw*)unmanaged;");
                                writer.WriteLine($"unmanaged += Vk{name}.Raw.SizeInBytes;");
                                writer.WriteLine("*result = s.MarshalDirect(ref unmanaged);");
                                writer.WriteLine("return result;");
                            }
                            writer.WriteLine();

                            writer.WriteLine($"public static int SizeOfMarshalDirect(this IReadOnlyList<IVk{name}> list) => ");
                            writer.Tab();
                            writer.WriteLine("list == null || list.Count == 0 ");
                            writer.Tab();
                            writer.WriteLine("? 0");
                            writer.WriteLine($": sizeof(Vk{name}.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());");
                            writer.UnTab();
                            writer.UnTab();
                            writer.WriteLine();

                            writer.WriteLine($"public static Vk{name}.Raw* MarshalDirect(this IReadOnlyList<IVk{name}> list, ref byte* unmanaged)");
                            using (writer.Curly())
                            {
                                writer.WriteLine("if (list == null || list.Count == 0)");
                                writer.Tab();
                                writer.WriteLine($"return (Vk{name}.Raw*)0;");
                                writer.UnTab();
                                writer.WriteLine($"var result = (Vk{name}.Raw*)unmanaged;");
                                writer.WriteLine($"unmanaged += sizeof(Vk{name}.Raw) * list.Count;");
                                writer.WriteLine("for (int i = 0; i < list.Count; i++)");
                                writer.Tab();
                                writer.WriteLine("result[i] = list[i].MarshalDirect(ref unmanaged);");
                                writer.UnTab();
                                writer.WriteLine("return result;");
                            }
                            writer.WriteLine();

                            writer.WriteLine($"public static int SizeOfMarshalIndirect(this IReadOnlyList<IVk{name}> list) =>");
                            writer.Tab();
                            writer.WriteLine("list == null || list.Count == 0");
                            writer.Tab();
                            writer.WriteLine("? 0");
                            writer.WriteLine($": sizeof(Vk{name}.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());");
                            writer.UnTab();
                            writer.UnTab();
                            writer.WriteLine();

                            writer.WriteLine($"public static Vk{name}.Raw** MarshalIndirect(this IReadOnlyList<IVk{name}> list, ref byte* unmanaged)");
                            using (writer.Curly())
                            {
                                writer.WriteLine("if (list == null || list.Count == 0)");
                                writer.Tab();
                                writer.WriteLine($"return (Vk{name}.Raw**)0;");
                                writer.UnTab();
                                writer.WriteLine($"var result = (Vk{name}.Raw**)unmanaged;");
                                writer.WriteLine($"unmanaged += sizeof(Vk{name}.Raw*) * list.Count;");
                                writer.WriteLine("for (int i = 0; i < list.Count; i++)");
                                writer.Tab();
                                writer.WriteLine("result[i] = list[i].MarshalIndirect(ref unmanaged);");
                                writer.UnTab();
                                writer.WriteLine("return result;");
                            }
                        }
                    }
                }
            }   
        }

        private static RawField[] BuildRawFields(Type type) => 
            type.GetFields().Select(x => new RawField(x)).ToArray();

        private static WrapperProperty[] BuildWrapperProps(IReadOnlyList<RawField> rawFields) => 
            rawFields.Where(x => !x.IgnoreInWrapper).Select(x => new WrapperProperty(x, null)).ToArray();

        private static void WriteLicense(CodeWriter writer)
        {
            writer.WriteLine(@"#region License
/*
Copyright (c) 2016 VulkaNet Project - Daniil Rodin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion");
        }
    }
}