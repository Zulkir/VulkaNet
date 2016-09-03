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
    public unsafe struct GenPhysicalDeviceFeatures
    {
        public VkBool32 robustBufferAccess;
        public VkBool32 fullDrawIndexUint32;
        public VkBool32 imageCubeArray;
        public VkBool32 independentBlend;
        public VkBool32 geometryShader;
        public VkBool32 tessellationShader;
        public VkBool32 sampleRateShading;
        public VkBool32 dualSrcBlend;
        public VkBool32 logicOp;
        public VkBool32 multiDrawIndirect;
        public VkBool32 drawIndirectFirstInstance;
        public VkBool32 depthClamp;
        public VkBool32 depthBiasClamp;
        public VkBool32 fillModeNonSolid;
        public VkBool32 depthBounds;
        public VkBool32 wideLines;
        public VkBool32 largePoints;
        public VkBool32 alphaToOne;
        public VkBool32 multiViewport;
        public VkBool32 samplerAnisotropy;
        public VkBool32 textureCompressionETC2;
        public VkBool32 textureCompressionASTC_LDR;
        public VkBool32 textureCompressionBC;
        public VkBool32 occlusionQueryPrecise;
        public VkBool32 pipelineStatisticsQuery;
        public VkBool32 vertexPipelineStoresAndAtomics;
        public VkBool32 fragmentStoresAndAtomics;
        public VkBool32 shaderTessellationAndGeometryPointSize;
        public VkBool32 shaderImageGatherExtended;
        public VkBool32 shaderStorageImageExtendedFormats;
        public VkBool32 shaderStorageImageMultisample;
        public VkBool32 shaderStorageImageReadWithoutFormat;
        public VkBool32 shaderStorageImageWriteWithoutFormat;
        public VkBool32 shaderUniformBufferArrayDynamicIndexing;
        public VkBool32 shaderSampledImageArrayDynamicIndexing;
        public VkBool32 shaderStorageBufferArrayDynamicIndexing;
        public VkBool32 shaderStorageImageArrayDynamicIndexing;
        public VkBool32 shaderClipDistance;
        public VkBool32 shaderCullDistance;
        public VkBool32 shaderFloat64;
        public VkBool32 shaderInt64;
        public VkBool32 shaderInt16;
        public VkBool32 shaderResourceResidency;
        public VkBool32 shaderResourceMinLod;
        public VkBool32 sparseBinding;
        public VkBool32 sparseResidencyBuffer;
        public VkBool32 sparseResidencyImage2D;
        public VkBool32 sparseResidencyImage3D;
        public VkBool32 sparseResidency2Samples;
        public VkBool32 sparseResidency4Samples;
        public VkBool32 sparseResidency8Samples;
        public VkBool32 sparseResidency16Samples;
        public VkBool32 sparseResidencyAliased;
        public VkBool32 variableMultisampleRate;
        public VkBool32 inheritedQueries;
    }
}