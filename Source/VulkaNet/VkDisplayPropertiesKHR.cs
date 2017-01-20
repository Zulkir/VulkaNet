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
    public unsafe struct VkDisplayPropertiesKHR
    {
        public IVkDisplayKHR Display { get; set; }
        public string DisplayName { get; set; }
        public VkExtent2D PhysicalDimensions { get; set; }
        public VkExtent2D PhysicalResolution { get; set; }
        public VkSurfaceTransformFlagsKHR SupportedTransforms { get; set; }
        public bool PlaneReorderPossible { get; set; }
        public bool PersistentContent { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkDisplayKHR.HandleType display;
            public byte* displayName;
            public VkExtent2D physicalDimensions;
            public VkExtent2D physicalResolution;
            public VkSurfaceTransformFlagsKHR supportedTransforms;
            public VkBool32 planeReorderPossible;
            public VkBool32 persistentContent;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }

        public VkDisplayPropertiesKHR(Raw* raw, IVkPhysicalDevice physicalDevice)
        {
            Display = physicalDevice.GetDisplay(raw->display);
            DisplayName = VkHelpers.ToString(raw->displayName);
            PhysicalDimensions = raw->physicalDimensions;
            PhysicalResolution = raw->physicalResolution;
            SupportedTransforms = raw->supportedTransforms;
            PlaneReorderPossible = (bool)raw->planeReorderPossible;
            PersistentContent = (bool)raw->persistentContent;
        }
    }
}
