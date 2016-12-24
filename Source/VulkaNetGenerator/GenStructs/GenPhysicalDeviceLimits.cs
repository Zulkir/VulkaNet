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
using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPhysicalDeviceLimits
    {
        public uint maxImageDimension1D;
        public uint maxImageDimension2D;
        public uint maxImageDimension3D;
        public uint maxImageDimensionCube;
        public uint maxImageArrayLayers;
        public uint maxTexelBufferElements;
        public uint maxUniformBufferRange;
        public uint maxStorageBufferRange;
        public uint maxPushConstantsSize;
        public uint maxMemoryAllocationCount;
        public uint maxSamplerAllocationCount;
        public ulong bufferImageGranularity;
        public ulong sparseAddressSpaceSize;
        public uint maxBoundDescriptorSets;
        public uint maxPerStageDescriptorSamplers;
        public uint maxPerStageDescriptorUniformBuffers;
        public uint maxPerStageDescriptorStorageBuffers;
        public uint maxPerStageDescriptorSampledImages;
        public uint maxPerStageDescriptorStorageImages;
        public uint maxPerStageDescriptorInputAttachments;
        public uint maxPerStageResources;
        public uint maxDescriptorSetSamplers;
        public uint maxDescriptorSetUniformBuffers;
        public uint maxDescriptorSetUniformBuffersDynamic;
        public uint maxDescriptorSetStorageBuffers;
        public uint maxDescriptorSetStorageBuffersDynamic;
        public uint maxDescriptorSetSampledImages;
        public uint maxDescriptorSetStorageImages;
        public uint maxDescriptorSetInputAttachments;
        public uint maxVertexInputAttributes;
        public uint maxVertexInputBindings;
        public uint maxVertexInputAttributeOffset;
        public uint maxVertexInputBindingStride;
        public uint maxVertexOutputComponents;
        public uint maxTessellationGenerationLevel;
        public uint maxTessellationPatchSize;
        public uint maxTessellationControlPerVertexInputComponents;
        public uint maxTessellationControlPerVertexOutputComponents;
        public uint maxTessellationControlPerPatchOutputComponents;
        public uint maxTessellationControlTotalOutputComponents;
        public uint maxTessellationEvaluationInputComponents;
        public uint maxTessellationEvaluationOutputComponents;
        public uint maxGeometryShaderInvocations;
        public uint maxGeometryInputComponents;
        public uint maxGeometryOutputComponents;
        public uint maxGeometryOutputVertices;
        public uint maxGeometryTotalOutputComponents;
        public uint maxFragmentInputComponents;
        public uint maxFragmentOutputAttachments;
        public uint maxFragmentDualSrcAttachments;
        public uint maxFragmentCombinedOutputResources;
        public uint maxComputeSharedMemorySize;
        [AsType("VkUintVector3"), FixedArray("3")]
        public uint* maxComputeWorkGroupCount;
        public uint maxComputeWorkGroupInvocations;
        [AsType("VkUintVector3"), FixedArray("3")]
        public uint* maxComputeWorkGroupSize;
        public uint subPixelPrecisionBits;
        public uint subTexelPrecisionBits;
        public uint mipmapPrecisionBits;
        public uint maxDrawIndexedIndexValue;
        public uint maxDrawIndirectCount;
        public float maxSamplerLodBias;
        public float maxSamplerAnisotropy;
        public uint maxViewports;
        [AsType("VkUintVector2"), FixedArray("2")]
        public uint* maxViewportDimensions;
        [AsType("VkVector2"), FixedArray("2")]
        public float* viewportBoundsRange;
        public uint viewportSubPixelBits;
        public IntPtr minMemoryMapAlignment;
        public ulong minTexelBufferOffsetAlignment;
        public ulong minUniformBufferOffsetAlignment;
        public ulong minStorageBufferOffsetAlignment;
        public int minTexelOffset;
        public uint maxTexelOffset;
        public int minTexelGatherOffset;
        public uint maxTexelGatherOffset;
        public float minInterpolationOffset;
        public float maxInterpolationOffset;
        public uint subPixelInterpolationOffsetBits;
        public uint maxFramebufferWidth;
        public uint maxFramebufferHeight;
        public uint maxFramebufferLayers;
        public VkSampleCountFlags framebufferColorSampleCounts;
        public VkSampleCountFlags framebufferDepthSampleCounts;
        public VkSampleCountFlags framebufferStencilSampleCounts;
        public VkSampleCountFlags framebufferNoAttachmentsSampleCounts;
        public uint maxColorAttachments;
        public VkSampleCountFlags sampledImageColorSampleCounts;
        public VkSampleCountFlags sampledImageIntegerSampleCounts;
        public VkSampleCountFlags sampledImageDepthSampleCounts;
        public VkSampleCountFlags sampledImageStencilSampleCounts;
        public VkSampleCountFlags storageImageSampleCounts;
        public uint maxSampleMaskWords;
        public VkBool32 timestampComputeAndGraphics;
        public float timestampPeriod;
        public uint maxClipDistances;
        public uint maxCullDistances;
        public uint maxCombinedClipAndCullDistances;
        public uint discreteQueuePriorities;
        [AsType("VkVector2"), FixedArray("2")]
        public float* pointSizeRange;
        [AsType("VkVector2"), FixedArray("2")]
        public float* lineWidthRange;
        public float pointSizeGranularity;
        public float lineWidthGranularity;
        public VkBool32 strictLines;
        public VkBool32 standardSampleLocations;
        public ulong optimalBufferCopyOffsetAlignment;
        public ulong optimalBufferCopyRowPitchAlignment;
        public ulong nonCoherentAtomSize;
    }
}