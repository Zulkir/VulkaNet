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
using System.Text.RegularExpressions;
using VulkaNetGenerator.Dummies;
using VulkaNetGenerator.GenStructs;
using VulkaNetGenerator.Reflection;

namespace VulkaNetGenerator
{
    public class Generator
    {
        private readonly List<RawFunction> allRawFunctions = new List<RawFunction>();

        public void GenerateStruct<T>(bool input, bool output)
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
                var isActuallyStruct = !rawFields.Any(x => x.TypeStr == "VkStructureType");
                var isAlreadyRaw = isActuallyStruct && rawFields.Length == wrapperProperties.Length &&
                                   rawFields.Zip(wrapperProperties, (a, b) => new {a, b}).All(x => x.a.TypeStr == x.b.TypeStr);

                if (isAlreadyRaw)
                    writer.WriteLine("IS ALREADY RAW");

                if (!isActuallyStruct || rawFields.Any(x => x.TypeStr == "IntPtr") || wrapperProperties.Any(x => x.TypeStr == "IntPtr"))
                    writer.WriteLine("using System;");
                if (input || wrapperProperties.Any(x => x.TypeStr.Contains("ReadOnlyList")))
                {
                    writer.WriteLine("using System.Collections.Generic;");
                    writer.WriteLine("using System.Linq;");
                }
                writer.WriteLine("using System.Runtime.InteropServices;");
                writer.WriteLine();

                writer.WriteLine("namespace VulkaNet");
                using (writer.Curly())
                {
                    var typeTypeStr = isActuallyStruct ? "struct" : "class";

                    writer.WriteLine($"public unsafe {typeTypeStr} Vk{name}");
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

                            if (!isActuallyStruct)
                            {
                                writer.WriteLine($"public Vk{name}() {{ }}");
                                writer.WriteLine();
                            }

                            var displayParamStr = rawFields.Any(x => x.GenType == typeof(GenDisplayKHR))
                                ? ", IVkPhysicalDevice physicalDevice" : "";

                            //var instanceChildParamStr = rawFields.Any(x => typeof(IGenInstanceChild).IsAssignableFrom(x.GenType)) 
                            //    ? ", IVkInstance instance" : "";

                            writer.WriteLine($"public Vk{name}(Raw* raw{displayParamStr})");
                            using (writer.Curly())
                            {
                                foreach (var prop in wrapperProperties)
                                {
                                    var additionalParamStr = typeof(IGenInstanceChild).IsAssignableFrom(prop.Raw.GenType)
                                        ? "instance, " : "";

                                    var fieldVal = $"raw->{prop.Raw.Name}";
                                    var prefix = prop.CreatorFuncTakesPtr ? "&" : "";
                                    if (prop.Raw.GenType == typeof(GenDisplayKHR))
                                        writer.WriteLine($"{prop.Name} = physicalDevice.GetDisplay({fieldVal});");
                                    else if (prop.CreatorFunc != null)
                                        writer.WriteLine($"{prop.Name} = {prop.CreatorFunc}({additionalParamStr}{prefix}{fieldVal});");
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
                            writer.WriteLine($"public static int SizeOfMarshalDirect(this Vk{name} s)");
                            using (writer.Curly())
                            {
                                if (!isActuallyStruct)
                                {
                                    writer.WriteLine("if (s == null)");
                                    writer.Tab();
                                    writer.WriteLine("throw new InvalidOperationException(\"Trying to directly marshal a null.\");");
                                    writer.UnTab();
                                    writer.WriteLine();
                                }
                                
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

                            writer.WriteLine($"public static Vk{name}.Raw MarshalDirect(this Vk{name} s, ref byte* unmanaged)");
                            using (writer.Curly())
                            {
                                if (!isActuallyStruct)
                                {
                                    writer.WriteLine("if (s == null)");
                                    writer.Tab();
                                    writer.WriteLine("throw new InvalidOperationException(\"Trying to directly marshal a null.\");");
                                    writer.UnTab();
                                    writer.WriteLine();
                                }
                                
                                foreach (var prop in unmanagedProps)
                                    writer.WriteLine($"var {prop.Raw.Name} = s.{prop.Name}.{prop.MarshalMethod}(ref unmanaged);");
                                writer.WriteLine();

                                //var byteArrayFields = rawFields.Where(x => x.IsByteArray).ToArray();
                                //foreach (var byteArrayField in byteArrayFields)
                                //{
                                //    var prop = wrapperProperties.SingleOrDefault(x => x.Raw == byteArrayField);
                                //    if (prop != null)
                                //        writer.WriteLine($"fixed (byte* p{prop.Name} = {prop.Name})");
                                //}
                                //var byteArraysCurly = byteArrayFields.Any() ? writer.Curly() : null;

                                writer.WriteLine($"Vk{name}.Raw result;");
                                foreach (var field in rawFields)
                                {
                                    var prop = wrapperProperties.SingleOrDefault(x => x.Raw == field);
                                    var rval = field.Name == "sType" ? $"VkStructureType.{name}" :
                                            field.ShouldMarshal ? $"{field.Name}" :
                                            field.IsHandle ? $"s.{prop?.Name}?.Handle ?? {field.TypeStr}.Null" :
                                            field.IsCountFor != null ? $"s.{field.IsCountFor}?.Count ?? 0" :
                                            field.IsByteArray ? $"p{prop?.Name}" :
                                            field.IsByteArraySizeFor != null ? $"{field.IsByteArraySizeFor}.Length" :
                                            field.TypeStr == "VkBool32" ? $"new VkBool32(s.{prop?.Name})" :
                                            (prop?.NeedsCast ?? false) ? $"({field.TypeStr})s.{prop.Name}" :
                                            field.IsUnmanagedPtr ? $"&s.{prop?.Name}" :
                                            $"s.{prop?.Name}";
                                    writer.WriteLine($"result.{field.Name} = {rval};");
                                }
                                writer.WriteLine("return result;");
                                //byteArraysCurly?.Dispose();
                            }
                            writer.WriteLine();

                            writer.WriteLine($"public static int SizeOfMarshalIndirect(this Vk{name} s) =>");
                            writer.Tab();
                            if (!isActuallyStruct)
                                writer.WriteLine($"s == null ? 0 : s.SizeOfMarshalDirect() + Vk{name}.Raw.SizeInBytes;");
                            else
                                writer.WriteLine($"s.SizeOfMarshalDirect() + Vk{name}.Raw.SizeInBytes;");
                            writer.UnTab();
                            writer.WriteLine();

                            writer.WriteLine($"public static Vk{name}.Raw* MarshalIndirect(this Vk{name} s, ref byte* unmanaged)");
                            using (writer.Curly())
                            {
                                if (!isActuallyStruct)
                                {
                                    writer.WriteLine("if (s == null)");
                                    writer.Tab();
                                    writer.WriteLine($"return (Vk{name}.Raw*)0;");
                                    writer.UnTab();
                                }
                                writer.WriteLine($"var result = (Vk{name}.Raw*)unmanaged;");
                                writer.WriteLine($"unmanaged += Vk{name}.Raw.SizeInBytes;");
                                writer.WriteLine("*result = s.MarshalDirect(ref unmanaged);");
                                writer.WriteLine("return result;");
                            }
                            writer.WriteLine();

                            writer.WriteLine($"public static int SizeOfMarshalDirect(this IReadOnlyList<Vk{name}> list) => ");
                            writer.Tab();
                            writer.WriteLine("list == null || list.Count == 0 ");
                            writer.Tab();
                            writer.WriteLine("? 0");
                            writer.WriteLine($": sizeof(Vk{name}.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());");
                            writer.UnTab();
                            writer.UnTab();
                            writer.WriteLine();

                            writer.WriteLine($"public static Vk{name}.Raw* MarshalDirect(this IReadOnlyList<Vk{name}> list, ref byte* unmanaged)");
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
                        }
                    }
                }
            }   
        }

        public void GenerateClass<T>()
        {
            var type = typeof(T);
            var name = type.Name.Substring(3);
            var isDevice = name == "Device";
            var isDispatchable = typeof(IGenHandledObject).IsAssignableFrom(type);
            var isNonDispatchable = typeof(IGenNonDispatchableHandledObject).IsAssignableFrom(type);

            var rawFunctions = BuildRawFunctions(type);
            allRawFunctions.AddRange(rawFunctions);
            var wrapperMethods = BuildWrapperMethods(rawFunctions);

            var isDisposable = wrapperMethods.Any(x => x.Name == "Dispose");
            var isInstanceChild = typeof(IGenInstanceChild).IsAssignableFrom(type);

            if (!Directory.Exists("GeneratedSource"))
                Directory.CreateDirectory("GeneratedSource");

            using (var stream = File.Open($"GeneratedSource/Vk{name}.cs", FileMode.Create))
            using (var writer = new CodeWriter(stream))
            {
                WriteLicense(writer);
                writer.WriteLine();
                
                if (isDispatchable || isDisposable || wrapperMethods.Any(x => x.ReturnTypeStr == "VkObjectResult<byte[]>"))
                    writer.WriteLine("using System;");
                if (isDevice)
                    writer.WriteLine("using System.Collections.Concurrent;");
                writer.WriteLine("using System.Collections.Generic;");
                if (rawFunctions.SelectMany(x => x.Parameters).Any(x => x.IsReturnParam && x.IsArray))
                    writer.WriteLine("using System.Linq;");
                if (isDevice)
                    writer.WriteLine("using System.Runtime.InteropServices;");
                if (isDevice)
                    writer.WriteLine("using VulkaNet.InternalHelpers;");
                writer.WriteLine();

                writer.WriteLine("namespace VulkaNet");
                using (writer.Curly())
                {
                    var isAllocatable = rawFunctions.SelectMany(x => x.Parameters).Any(x => x.FromProperty == "Allocator");

                    var interfaces = new List<string>();
                    if (isDispatchable)
                        interfaces.Add("IVkHandledObject");
                    if (isNonDispatchable)
                        interfaces.Add("IVkNonDispatchableHandledObject");
                    if (!isDevice)
                        if (isInstanceChild)
                            interfaces.Add("IVkInstanceChild");
                        else
                            interfaces.Add("IVkDeviceChild");
                    if (isDisposable)
                        interfaces.Add("IDisposable");
                    if (isDevice)
                        interfaces.Add("IVkInstanceChild");
                    var interfacesString = interfaces.Any()
                        ? " : " + string.Join(", ", interfaces)
                        : "";

                    writer.WriteLine($"public interface IVk{name}{interfacesString}");
                    using (writer.Curly())
                    {
                        if (isDevice)
                            writer.WriteLine("IVkPhysicalDevice PhysicalDevice { get; }");
                        writer.WriteLine($"Vk{name}.HandleType Handle {{ get; }}");
                        if (isDevice)
                            writer.WriteLine("VkDevice.DirectFunctions Direct { get; }");
                        if (isAllocatable)
                            writer.WriteLine("IVkAllocationCallbacks Allocator { get; }");
                        if (isDevice)
                            writer.WriteLine("IVkQueue GetDeviceQueue(int queueFamilyIndex, int queueIndex);");
                        foreach (var wm in wrapperMethods.Where(x => x.Name != "Dispose"))
                        {
                            var paramsStr = string.Join(", ", wm.Parameters.Where(x => !x.IsFromProperty).Select(x => $"{x.TypeStr} {x.Name}"));
                            var paramsLine = string.Join(", ", paramsStr);
                            writer.WriteLine($"{wm.ReturnTypeStr} {wm.Name}({paramsLine});");
                        }
                    }
                    writer.WriteLine();

                    writer.WriteLine($"public unsafe class Vk{name} : IVk{name}");
                    using (writer.Curly())
                    {
                        var rawHandleTypeStr = isDispatchable ? "IntPtr" : "ulong";

                        if (isDevice)
                        {
                            writer.WriteLine("public IVkInstance Instance { get; }");
                            writer.WriteLine("public IVkPhysicalDevice PhysicalDevice { get; }");
                            writer.WriteLine("public HandleType Handle { get; }");
                            writer.WriteLine("public IVkAllocationCallbacks Allocator { get; }");
                            writer.WriteLine("public DirectFunctions Direct { get; }");
                            writer.WriteLine();

                            writer.WriteLine("public IntPtr RawHandle => Handle.InternalHandle;");
                            writer.WriteLine();

                            writer.WriteLine("private readonly ConcurrentDictionary<ValuePair<int, int>, IVkQueue> queues;");
                            writer.WriteLine();

                            writer.WriteLine("public VkDevice(IVkPhysicalDevice physicalDevice, HandleType handle, IVkAllocationCallbacks allocator)");
                            using (writer.Curly())
                            {
                                writer.WriteLine("PhysicalDevice = physicalDevice;");
                                writer.WriteLine("Instance = physicalDevice.Instance;");
                                writer.WriteLine("Handle = handle;");
                                writer.WriteLine("Allocator = allocator;");
                                writer.WriteLine("Direct = new DirectFunctions(this);");
                                writer.WriteLine("queues = new ConcurrentDictionary<ValuePair<int, int>, IVkQueue>();");
                            }
                            writer.WriteLine();
                        }
                        else
                        {
                            var parentStr = isInstanceChild
                                ? "Instance"
                                : "Device";

                            writer.WriteLine($"public IVk{parentStr} {parentStr} {{ get; }}");
                            writer.WriteLine("public HandleType Handle { get; }");
                            if (isAllocatable)
                                writer.WriteLine("public IVkAllocationCallbacks Allocator { get; }");
                            writer.WriteLine();

                            writer.WriteLine($"private Vk{parentStr}.DirectFunctions Direct => {parentStr}.Direct;");
                            writer.WriteLine();

                            writer.WriteLine($"public {rawHandleTypeStr} RawHandle => Handle.InternalHandle;");
                            writer.WriteLine();
                            
                            if (isAllocatable)
                            {
                                writer.WriteLine($"public Vk{name}(IVk{parentStr} {parentStr.ToLower()}, HandleType handle, IVkAllocationCallbacks allocator)");
                                using (writer.Curly())
                                {
                                    writer.WriteLine($"{parentStr} = {parentStr.ToLower()};");
                                    writer.WriteLine("Handle = handle;");
                                    writer.WriteLine("Allocator = allocator;");
                                }
                                writer.WriteLine();
                            }
                            else
                            {
                                writer.WriteLine($"public Vk{name}(IVk{parentStr} {parentStr.ToLower()}, HandleType handle)");
                                using (writer.Curly())
                                {
                                    writer.WriteLine($"{parentStr} = {parentStr.ToLower()};");
                                    writer.WriteLine("Handle = handle;");
                                }
                                writer.WriteLine();
                            }
                        }

                        writer.WriteLine("public struct HandleType");
                        using (writer.Curly())
                        {
                            writer.WriteLine($"public readonly {rawHandleTypeStr} InternalHandle;");
                            writer.WriteLine($"public HandleType({rawHandleTypeStr} internalHandle) {{ InternalHandle = internalHandle; }}");
                            writer.WriteLine("public override string ToString() => InternalHandle.ToString();");
                            var sizeString = isDispatchable ? "IntPtr.Size" : "sizeof(ulong)";
                            writer.WriteLine($"public static int SizeInBytes {{ get; }} = {sizeString};");
                            writer.WriteLine($"public static HandleType Null => new HandleType(default({rawHandleTypeStr}));");
                        }
                        writer.WriteLine();

                        if (isDevice)
                        {
                            writer.WriteLine("public class DirectFunctions");
                            using (writer.Curly())
                            {
                                writer.WriteLine("private readonly IVkDevice device;");
                                writer.WriteLine();

                                writer.WriteLine("public GetDeviceProcAddrDelegate GetDeviceProcAddr { get; }");
                                writer.WriteLine("public delegate IntPtr GetDeviceProcAddrDelegate(");
                                writer.Tab();
                                writer.WriteLine("HandleType device,");
                                writer.WriteLine("byte* pName);");
                                writer.UnTab();
                                writer.WriteLine();
                                
                                writer.WriteLine("public GetDeviceQueueDelegate GetDeviceQueue { get; }");
                                writer.WriteLine("public delegate void GetDeviceQueueDelegate(");
                                writer.Tab();
                                writer.WriteLine("HandleType device,");
                                writer.WriteLine("uint queueFamilyIndex,");
                                writer.WriteLine("uint queueIndex,");
                                writer.WriteLine("VkQueue.HandleType* pQueue);");
                                writer.UnTab();
                                writer.WriteLine();

                                foreach (var func in allRawFunctions)
                                {
                                    writer.WriteLine($"public {func.Name}Delegate {func.Name} {{ get; }}");
                                    if (func.Parameters.Any())
                                    {
                                        writer.WriteLine($"public delegate {func.ReturnTypeStr} {func.Name}Delegate(");
                                        writer.Tab();
                                        foreach (var parameter in func.Parameters.Take(func.Parameters.Count - 1))
                                        {
                                            var typeStr = parameter.TypeStr == "VkDevice.HandleType" ? "HandleType" : parameter.TypeStr;
                                            writer.WriteLine($"{typeStr} {parameter.Name},");
                                        }
                                        var lastParam = func.Parameters.Last();
                                        writer.WriteLine($"{lastParam.TypeStr} {lastParam.Name});");
                                        writer.UnTab();
                                    }
                                    else
                                    {
                                        writer.WriteLine($"public delegate {func.ReturnTypeStr} {func.Name}Delegate();");
                                    }
                                    writer.WriteLine();
                                }

                                writer.WriteLine("public DirectFunctions(IVkDevice device)");
                                using (writer.Curly())
                                {
                                    writer.WriteLine("this.device = device;");
                                    writer.WriteLine();

                                    writer.WriteLine("GetDeviceProcAddr = VkHelpers.GetInstanceDelegate<GetDeviceProcAddrDelegate>(device.Instance, \"vkGetDeviceProcAddr\");");
                                    writer.WriteLine("GetDeviceQueue = GetDeviceDelegate<GetDeviceQueueDelegate>(\"vkGetDeviceQueue\");");
                                    foreach (var func in allRawFunctions)
                                        writer.WriteLine($"{func.Name} = GetDeviceDelegate<{func.Name}Delegate>(\"vk{func.Name}\");");
                                }
                                writer.WriteLine();

                                writer.WriteLine("public TDelegate GetDeviceDelegate<TDelegate>(string name)");
                                using (writer.Curly())
                                {
                                    writer.WriteLine("IntPtr funPtr;");
                                    writer.WriteLine("fixed (byte* pName = name.ToAnsiArray())");
                                    writer.Tab();
                                    writer.WriteLine("funPtr = GetDeviceProcAddr(device.Handle, pName);");
                                    writer.UnTab();
                                    writer.WriteLine("return Marshal.GetDelegateForFunctionPointer<TDelegate>(funPtr);");
                                }
                            }
                            writer.WriteLine();

                            writer.WriteLine("public IVkQueue GetDeviceQueue(int queueFamilyIndex, int queueIndex) =>");
                            writer.Tab();
                            writer.WriteLine("queues.GetOrAdd(new ValuePair<int, int>(queueFamilyIndex, queueIndex), DoGetDeviceQueue);");
                            writer.UnTab();
                            writer.WriteLine();

                            writer.WriteLine("private IVkQueue DoGetDeviceQueue(ValuePair<int, int> key)");
                            using (writer.Curly())
                            {
                                writer.WriteLine("VkQueue.HandleType handle;");
                                writer.WriteLine("Direct.GetDeviceQueue(Handle, (uint)key.First, (uint)key.Second, &handle);");
                                writer.WriteLine("return new VkQueue(this, handle);");
                            }
                            writer.WriteLine();
                        }
                        
                        foreach (var wrapper in wrapperMethods)
                        {
                            var raw = wrapper.Raw;
                            var paramsStr = string.Join(", ", wrapper.Parameters.Where(x => !x.IsFromProperty).Select(x => $"{x.TypeStr} {x.Name}"));
                            writer.WriteLine($"public {wrapper.ReturnTypeStr} {wrapper.Name}({paramsStr})");
                            using (writer.Curly())
                            {
                                var unmanagedParams = wrapper.Parameters.Where(x => x.MarshalledAsUnmanaged).ToArray();
                                if (unmanagedParams.Any())
                                {
                                    writer.WriteLine($"var unmanagedSize =");
                                    writer.Tab();
                                    foreach (var parameter in unmanagedParams.Take(unmanagedParams.Length - 1))
                                        writer.WriteLine($"{parameter.Name}.{parameter.SizeMethod}() +");
                                    var lastParam = unmanagedParams.Last();
                                    writer.WriteLine($"{lastParam.Name}.{lastParam.SizeMethod}();");
                                    writer.UnTab();
                                    writer.WriteLine("var unmanagedArray = new byte[unmanagedSize];");
                                    writer.WriteLine("fixed (byte* unmanagedStart = unmanagedArray)");
                                    writer.WriteLine("{");
                                    writer.Tab();
                                    writer.WriteLine("var unmanaged = unmanagedStart;");
                                }

                                var rawReturnParam = raw.Parameters.SingleOrDefault(x => x.IsReturnParam);
                                var returnSizeParam = raw.Parameters.SingleOrDefault(x => x.IsReturnSize);

                                foreach (var rParam in raw.Parameters)
                                {
                                    if (rParam.IsReturnParam)
                                        continue;

                                    var wParam = wrapper.Parameters.SingleOrDefault(x => x.Raw == rParam);
                                    var rval = wParam != null && wParam.MarshalledAsUnmanaged ? $"{wParam.Name}.{wParam.MarshalMethod}(ref unmanaged)" :
                                               rParam.FromProperty == "Device" ? "Device.Handle" :
                                               rParam.FromProperty == "this" ? "Handle" :
                                               rParam.IsHandle ? $"{wParam?.Name}?.Handle ?? {rParam.TypeStr}.Null" :
                                               rParam.IsCountFor != null ? $"{rParam.IsCountFor}?.Count ?? 0" :
                                               rParam.TypeStr == "VkBool32" ? $"new VkBool32({wParam?.Name})" :
                                               rParam.IsReturnSize ? $"({rParam.TypeStr.Substring(0, rParam.TypeStr.Length - 1)})0" :
                                               rParam.IsUnmanagedPtr ? $"&{wParam?.Name}" :
                                               $"{wParam?.Name}";
                                    writer.WriteLine($"var _{rParam.Name} = {rval};");
                                }

                                if (rawReturnParam != null && returnSizeParam == null)
                                {
                                    if (rawReturnParam.IsArray)
                                    {
                                        var rawElementTypeStr = rawReturnParam.TypeStr.Substring(0, rawReturnParam.TypeStr.Length - 1);
                                        writer.WriteLine($"var handleArray = new {rawElementTypeStr}[{rawReturnParam.ReturnCount}];");
                                        writer.WriteLine($"fixed ({rawElementTypeStr}* _{rawReturnParam.Name} = handleArray)");
                                        writer.WriteLine("{");
                                        writer.Tab();
                                    }
                                    else if (wrapper.ReturnTypeStr != "VkObjectResult<byte[]>")
                                        writer.WriteLine($"{rawReturnParam.TypeStr.Substring(0, rawReturnParam.TypeStr.Length - 1)} _{rawReturnParam.Name};");
                                }

                                var rawParamsStr = string.Join(", ", raw.Parameters.Select(x => ((x.IsReturnParam && !x.IsArray) || x.IsReturnSize ? "&_" : "_") + x.Name));

                                if (wrapper.ReturnTypeStr == "void" && raw.ReturnTypeStr == "void")
                                {
                                    writer.WriteLine($"Direct.{raw.Name}({rawParamsStr});");
                                }
                                else if (wrapper.ReturnTypeStr == "VkResult" && raw.ReturnTypeStr == "VkResult")
                                {
                                    writer.WriteLine($"return Direct.{raw.Name}({rawParamsStr});");
                                }
                                else if (returnSizeParam != null)
                                {
                                    var wrapperTypeStr = wrapper.ReturnTypeStr == "VkObjectResult<byte[]>"
                                        ? "byte"
                                        : Regex.Match(new WrapperParameter(rawReturnParam).TypeStr, @"^IReadOnlyList<(.+)>$").Groups[1].Value;
                                    var firstParamStr = string.Join(", ", raw.Parameters
                                        .Select(x => 
                                            x.IsReturnSize ? $"&_{x.Name}" : 
                                            x.IsReturnParam ? $"({x.TypeStr})0" :
                                            $"_{x.Name}"));
                                    writer.WriteLine($"Direct.{raw.Name}({firstParamStr});");
                                    writer.WriteLine($"var resultArray = new {wrapperTypeStr}[(int)_{returnSizeParam.Name}];");
                                    writer.WriteLine($"fixed ({wrapperTypeStr}* pResultArray = resultArray)");
                                    using (writer.Curly())
                                    {
                                        var secondParamStr = string.Join(", ", raw.Parameters
                                            .Select(x =>
                                                x.IsReturnSize ? $"&_{x.Name}" :
                                                x.IsReturnParam ? $"({x.TypeStr})pResultArray" :
                                                $"_{x.Name}"));
                                        if (raw.ReturnTypeStr != "void")
                                        {
                                            writer.WriteLine($"var result = Direct.{raw.Name}({secondParamStr});");
                                            writer.WriteLine($"return new {wrapper.ReturnTypeStr}(result, resultArray);");
                                        }
                                        else
                                        {
                                            writer.WriteLine($"Direct.{raw.Name}({secondParamStr});");
                                            writer.WriteLine($"return resultArray;");
                                        }
                                    }
                                }
                                else if (wrapper.ReturnTypeStr == "VkObjectResult<IntPtr>" && raw.ReturnTypeStr == "VkResult")
                                {
                                    writer.WriteLine($"var result = Direct.{raw.Name}({rawParamsStr});");
                                    writer.WriteLine($"return new VkObjectResult<IntPtr>(result, _{rawReturnParam.Name});");
                                }
                                else if (rawReturnParam != null && raw.ReturnTypeStr == "VkResult")
                                {
                                    var objTypeStr = Regex.Match(wrapper.ReturnTypeStr, @"^VkObjectResult<(.+)>$").Groups[1].Value;
                                    writer.WriteLine($"var result = Direct.{raw.Name}({rawParamsStr});");
                                    var ctrParams = new List<string>();
                                    ctrParams.Add(isDevice ? "this" : "Device");
                                    if (rawReturnParam.IsArray)
                                        ctrParams.Add($"handleArray[i]");
                                    else
                                        ctrParams.Add($"_{rawReturnParam.Name}");
                                    if (wrapper.Parameters.Any(x => x.Name == "allocator"))
                                        ctrParams.Add("allocator");
                                    var ctrParamsStr = string.Join(", ", ctrParams);
                                    var interfaceTypeStr = rawReturnParam.IsArray
                                        ? Regex.Match(objTypeStr, @"^IReadOnlyList<(.+)>$").Groups[1].Value
                                        : objTypeStr;
                                    var classTypeStr = interfaceTypeStr.Substring(1);
                                    var ctrString = rawReturnParam.IsArray
                                        ? $"Enumerable.Range(0, handleArray.Length).Select(i => ({interfaceTypeStr})new {classTypeStr}({ctrParamsStr})).ToArray()"
                                        : $"new {classTypeStr}({ctrParamsStr})";
                                    writer.WriteLine($"var instance = result == VkResult.Success ? {ctrString} : null;");
                                    writer.WriteLine($"return new VkObjectResult<{objTypeStr}>(result, instance);");
                                }
                                else if (rawReturnParam != null && raw.ReturnTypeStr == "void")
                                {
                                    writer.WriteLine($"Direct.{raw.Name}({rawParamsStr});");
                                    writer.WriteLine($"return _{rawReturnParam.Name};");
                                }
                                else
                                {
                                    throw new NotSupportedException("Unexpected return type combination.");
                                }

                                if ((rawReturnParam?.IsArray ?? false) && returnSizeParam == null)
                                {
                                    writer.UnTab();
                                    writer.WriteLine("}");
                                }

                                if (unmanagedParams.Any())
                                {
                                    writer.UnTab();
                                    writer.WriteLine("}");
                                }
                            }
                            writer.WriteLine();
                        }
                    }
                    writer.WriteLine();

                    writer.WriteLine($"public static unsafe class Vk{name}Extensions");
                    using (writer.Curly())
                    {
                        var suffix = isDispatchable ? "Dispatchable" : "NonDispatchable";

                        writer.WriteLine($"public static int SizeOfMarshalDirect(this IReadOnlyList<IVk{name}> list) =>");
                        writer.Tab();
                        writer.WriteLine($"list.SizeOfMarshalDirect{suffix}();");
                        writer.UnTab();
                        writer.WriteLine();

                        writer.WriteLine($"public static Vk{name}.HandleType* MarshalDirect(this IReadOnlyList<IVk{name}> list, ref byte* unmanaged) =>");
                        writer.Tab();
                        writer.WriteLine($"(Vk{name}.HandleType*)list.MarshalDirect{suffix}(ref unmanaged);");
                        writer.UnTab();
                    }
                }
            }
        }

        private static RawField[] BuildRawFields(Type type) => 
            type.GetFields().Select(x => new RawField(x)).ToArray();

        private static WrapperProperty[] BuildWrapperProps(IReadOnlyList<RawField> rawFields) => 
            rawFields.Where(x => !x.IgnoreInWrapper).Select(x => new WrapperProperty(x)).ToArray();

        private static RawFunction[] BuildRawFunctions(Type type) =>
            type.GetMethods().Where(x => x.DeclaringType == type).Select(x => new RawFunction(x)).ToArray();

        private static WrapperMethod[] BuildWrapperMethods(IReadOnlyList<RawFunction> rawFunctions) =>
            rawFunctions.Where(x => true).Select(x => new WrapperMethod(x)).ToArray();

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