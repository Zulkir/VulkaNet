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

using System.Runtime.InteropServices;

namespace VulkaNet
{
    public interface IVkLayerProperties
    {
        string LayerName { get; }
        uint SpecVersion { get; }
        uint ImplementationVersion { get; }
        string Description { get; }
    }

    public unsafe class VkLayerProperties : IVkLayerProperties
    {
        public VkLayerProperties() { }

        public string LayerName { get; set; }
        public uint SpecVersion { get; set; }
        public uint ImplementationVersion { get; set; }
        public string Description { get; set; }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public fixed byte layerName[VkConstants.MaxExtensionNameSize];
            public uint specVersion;
            public uint implementationVersion;
            public fixed byte description[VkConstants.MaxDescriptionSize];
        }

        public VkLayerProperties(Raw* raw)
        {
            LayerName = VkHelpers.ToString(raw->layerName);
            SpecVersion = raw->specVersion;
            ImplementationVersion = raw->implementationVersion;
            Description = VkHelpers.ToString(raw->description);
        }
    }
}
