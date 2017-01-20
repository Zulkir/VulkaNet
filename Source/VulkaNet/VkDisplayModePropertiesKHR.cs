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
    public unsafe struct VkDisplayModePropertiesKHR
    {
        public IVkDisplayModeKHR DisplayMode { get; set; }
        public VkDisplayModeParametersKHR Parameters { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkDisplayModeKHR.HandleType displayMode;
            public VkDisplayModeParametersKHR parameters;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }

        public VkDisplayModePropertiesKHR(Raw* raw, IVkInstance instance)
        {
            DisplayMode = new VkDisplayModeKHR(instance, raw->displayMode);
            Parameters = raw->parameters;
        }
    }
}
