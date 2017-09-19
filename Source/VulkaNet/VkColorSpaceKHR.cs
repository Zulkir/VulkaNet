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

namespace VulkaNet
{
    public enum VkColorSpaceKHR
    {
        SRGB_NONLINEAR_KHR = 0,
        DISPLAY_P3_NONLINEAR_EXT = 1000104001,
        EXTENDED_SRGB_LINEAR_EXT = 1000104002,
        DCI_P3_LINEAR_EXT = 1000104003,
        DCI_P3_NONLINEAR_EXT = 1000104004,
        BT709_LINEAR_EXT = 1000104005,
        BT709_NONLINEAR_EXT = 1000104006,
        BT2020_LINEAR_EXT = 1000104007,
        HDR10_ST2084_EXT = 1000104008,
        DOLBYVISION_EXT = 1000104009,
        HDR10_HLG_EXT = 1000104010,
        ADOBERGB_LINEAR_EXT = 1000104011,
        ADOBERGB_NONLINEAR_EXT = 1000104012,
        PASS_THROUGH_EXT = 1000104013,
    }
}