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
using System.Runtime.InteropServices;

namespace VulkaNet
{
    public unsafe class VkPhysicalDeviceLimits
    {
        public uint MaxImageDimension1D { get; set; }
        public uint MaxImageDimension2D { get; set; }
        public uint MaxImageDimension3D { get; set; }
        public uint MaxImageDimensionCube { get; set; }
        public uint MaxImageArrayLayers { get; set; }
        public uint MaxTexelBufferElements { get; set; }
        public uint MaxUniformBufferRange { get; set; }
        public uint MaxStorageBufferRange { get; set; }
        public uint MaxPushConstantsSize { get; set; }
        public uint MaxMemoryAllocationCount { get; set; }
        public uint MaxSamplerAllocationCount { get; set; }
        public ulong BufferImageGranularity { get; set; }
        public ulong SparseAddressSpaceSize { get; set; }
        public uint MaxBoundDescriptorSets { get; set; }
        public uint MaxPerStageDescriptorSamplers { get; set; }
        public uint MaxPerStageDescriptorUniformBuffers { get; set; }
        public uint MaxPerStageDescriptorStorageBuffers { get; set; }
        public uint MaxPerStageDescriptorSampledImages { get; set; }
        public uint MaxPerStageDescriptorStorageImages { get; set; }
        public uint MaxPerStageDescriptorInputAttachments { get; set; }
        public uint MaxPerStageResources { get; set; }
        public uint MaxDescriptorSetSamplers { get; set; }
        public uint MaxDescriptorSetUniformBuffers { get; set; }
        public uint MaxDescriptorSetUniformBuffersDynamic { get; set; }
        public uint MaxDescriptorSetStorageBuffers { get; set; }
        public uint MaxDescriptorSetStorageBuffersDynamic { get; set; }
        public uint MaxDescriptorSetSampledImages { get; set; }
        public uint MaxDescriptorSetStorageImages { get; set; }
        public uint MaxDescriptorSetInputAttachments { get; set; }
        public uint MaxVertexInputAttributes { get; set; }
        public uint MaxVertexInputBindings { get; set; }
        public uint MaxVertexInputAttributeOffset { get; set; }
        public uint MaxVertexInputBindingStride { get; set; }
        public uint MaxVertexOutputComponents { get; set; }
        public uint MaxTessellationGenerationLevel { get; set; }
        public uint MaxTessellationPatchSize { get; set; }
        public uint MaxTessellationControlPerVertexInputComponents { get; set; }
        public uint MaxTessellationControlPerVertexOutputComponents { get; set; }
        public uint MaxTessellationControlPerPatchOutputComponents { get; set; }
        public uint MaxTessellationControlTotalOutputComponents { get; set; }
        public uint MaxTessellationEvaluationInputComponents { get; set; }
        public uint MaxTessellationEvaluationOutputComponents { get; set; }
        public uint MaxGeometryShaderInvocations { get; set; }
        public uint MaxGeometryInputComponents { get; set; }
        public uint MaxGeometryOutputComponents { get; set; }
        public uint MaxGeometryOutputVertices { get; set; }
        public uint MaxGeometryTotalOutputComponents { get; set; }
        public uint MaxFragmentInputComponents { get; set; }
        public uint MaxFragmentOutputAttachments { get; set; }
        public uint MaxFragmentDualSrcAttachments { get; set; }
        public uint MaxFragmentCombinedOutputResources { get; set; }
        public uint MaxComputeSharedMemorySize { get; set; }
        public VkUintVector3 MaxComputeWorkGroupCount { get; set; }
        public uint MaxComputeWorkGroupInvocations { get; set; }
        public VkUintVector3 MaxComputeWorkGroupSize { get; set; }
        public uint SubPixelPrecisionBits { get; set; }
        public uint SubTexelPrecisionBits { get; set; }
        public uint MipmapPrecisionBits { get; set; }
        public uint MaxDrawIndexedIndexValue { get; set; }
        public uint MaxDrawIndirectCount { get; set; }
        public float MaxSamplerLodBias { get; set; }
        public float MaxSamplerAnisotropy { get; set; }
        public uint MaxViewports { get; set; }
        public VkUintVector2 MaxViewportDimensions { get; set; }
        public VkVector2 ViewportBoundsRange { get; set; }
        public uint ViewportSubPixelBits { get; set; }
        public IntPtr MinMemoryMapAlignment { get; set; }
        public ulong MinTexelBufferOffsetAlignment { get; set; }
        public ulong MinUniformBufferOffsetAlignment { get; set; }
        public ulong MinStorageBufferOffsetAlignment { get; set; }
        public int MinTexelOffset { get; set; }
        public uint MaxTexelOffset { get; set; }
        public int MinTexelGatherOffset { get; set; }
        public uint MaxTexelGatherOffset { get; set; }
        public float MinInterpolationOffset { get; set; }
        public float MaxInterpolationOffset { get; set; }
        public uint SubPixelInterpolationOffsetBits { get; set; }
        public uint MaxFramebufferWidth { get; set; }
        public uint MaxFramebufferHeight { get; set; }
        public uint MaxFramebufferLayers { get; set; }
        public VkSampleCountFlags FramebufferColorSampleCounts { get; set; }
        public VkSampleCountFlags FramebufferDepthSampleCounts { get; set; }
        public VkSampleCountFlags FramebufferStencilSampleCounts { get; set; }
        public VkSampleCountFlags FramebufferNoAttachmentsSampleCounts { get; set; }
        public uint MaxColorAttachments { get; set; }
        public VkSampleCountFlags SampledImageColorSampleCounts { get; set; }
        public VkSampleCountFlags SampledImageIntegerSampleCounts { get; set; }
        public VkSampleCountFlags SampledImageDepthSampleCounts { get; set; }
        public VkSampleCountFlags SampledImageStencilSampleCounts { get; set; }
        public VkSampleCountFlags StorageImageSampleCounts { get; set; }
        public uint MaxSampleMaskWords { get; set; }
        public bool TimestampComputeAndGraphics { get; set; }
        public float TimestampPeriod { get; set; }
        public uint MaxClipDistances { get; set; }
        public uint MaxCullDistances { get; set; }
        public uint MaxCombinedClipAndCullDistances { get; set; }
        public uint DiscreteQueuePriorities { get; set; }
        public VkVector2 PointSizeRange { get; set; }
        public VkVector2 LineWidthRange { get; set; }
        public float PointSizeGranularity { get; set; }
        public float LineWidthGranularity { get; set; }
        public bool StrictLines { get; set; }
        public bool StandardSampleLocations { get; set; }
        public ulong OptimalBufferCopyOffsetAlignment { get; set; }
        public ulong OptimalBufferCopyRowPitchAlignment { get; set; }
        public ulong NonCoherentAtomSize { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
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
            public fixed uint maxComputeWorkGroupCount[3];
            public uint maxComputeWorkGroupInvocations;
            public fixed uint maxComputeWorkGroupSize[3];
            public uint subPixelPrecisionBits;
            public uint subTexelPrecisionBits;
            public uint mipmapPrecisionBits;
            public uint maxDrawIndexedIndexValue;
            public uint maxDrawIndirectCount;
            public float maxSamplerLodBias;
            public float maxSamplerAnisotropy;
            public uint maxViewports;
            public fixed uint maxViewportDimensions[2];
            public fixed float viewportBoundsRange[2];
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
            public fixed float pointSizeRange[2];
            public fixed float lineWidthRange[2];
            public float pointSizeGranularity;
            public float lineWidthGranularity;
            public VkBool32 strictLines;
            public VkBool32 standardSampleLocations;
            public ulong optimalBufferCopyOffsetAlignment;
            public ulong optimalBufferCopyRowPitchAlignment;
            public ulong nonCoherentAtomSize;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }

        public VkPhysicalDeviceLimits() { }

        public VkPhysicalDeviceLimits(Raw* raw)
        {
            MaxImageDimension1D = raw->maxImageDimension1D;
            MaxImageDimension2D = raw->maxImageDimension2D;
            MaxImageDimension3D = raw->maxImageDimension3D;
            MaxImageDimensionCube = raw->maxImageDimensionCube;
            MaxImageArrayLayers = raw->maxImageArrayLayers;
            MaxTexelBufferElements = raw->maxTexelBufferElements;
            MaxUniformBufferRange = raw->maxUniformBufferRange;
            MaxStorageBufferRange = raw->maxStorageBufferRange;
            MaxPushConstantsSize = raw->maxPushConstantsSize;
            MaxMemoryAllocationCount = raw->maxMemoryAllocationCount;
            MaxSamplerAllocationCount = raw->maxSamplerAllocationCount;
            BufferImageGranularity = raw->bufferImageGranularity;
            SparseAddressSpaceSize = raw->sparseAddressSpaceSize;
            MaxBoundDescriptorSets = raw->maxBoundDescriptorSets;
            MaxPerStageDescriptorSamplers = raw->maxPerStageDescriptorSamplers;
            MaxPerStageDescriptorUniformBuffers = raw->maxPerStageDescriptorUniformBuffers;
            MaxPerStageDescriptorStorageBuffers = raw->maxPerStageDescriptorStorageBuffers;
            MaxPerStageDescriptorSampledImages = raw->maxPerStageDescriptorSampledImages;
            MaxPerStageDescriptorStorageImages = raw->maxPerStageDescriptorStorageImages;
            MaxPerStageDescriptorInputAttachments = raw->maxPerStageDescriptorInputAttachments;
            MaxPerStageResources = raw->maxPerStageResources;
            MaxDescriptorSetSamplers = raw->maxDescriptorSetSamplers;
            MaxDescriptorSetUniformBuffers = raw->maxDescriptorSetUniformBuffers;
            MaxDescriptorSetUniformBuffersDynamic = raw->maxDescriptorSetUniformBuffersDynamic;
            MaxDescriptorSetStorageBuffers = raw->maxDescriptorSetStorageBuffers;
            MaxDescriptorSetStorageBuffersDynamic = raw->maxDescriptorSetStorageBuffersDynamic;
            MaxDescriptorSetSampledImages = raw->maxDescriptorSetSampledImages;
            MaxDescriptorSetStorageImages = raw->maxDescriptorSetStorageImages;
            MaxDescriptorSetInputAttachments = raw->maxDescriptorSetInputAttachments;
            MaxVertexInputAttributes = raw->maxVertexInputAttributes;
            MaxVertexInputBindings = raw->maxVertexInputBindings;
            MaxVertexInputAttributeOffset = raw->maxVertexInputAttributeOffset;
            MaxVertexInputBindingStride = raw->maxVertexInputBindingStride;
            MaxVertexOutputComponents = raw->maxVertexOutputComponents;
            MaxTessellationGenerationLevel = raw->maxTessellationGenerationLevel;
            MaxTessellationPatchSize = raw->maxTessellationPatchSize;
            MaxTessellationControlPerVertexInputComponents = raw->maxTessellationControlPerVertexInputComponents;
            MaxTessellationControlPerVertexOutputComponents = raw->maxTessellationControlPerVertexOutputComponents;
            MaxTessellationControlPerPatchOutputComponents = raw->maxTessellationControlPerPatchOutputComponents;
            MaxTessellationControlTotalOutputComponents = raw->maxTessellationControlTotalOutputComponents;
            MaxTessellationEvaluationInputComponents = raw->maxTessellationEvaluationInputComponents;
            MaxTessellationEvaluationOutputComponents = raw->maxTessellationEvaluationOutputComponents;
            MaxGeometryShaderInvocations = raw->maxGeometryShaderInvocations;
            MaxGeometryInputComponents = raw->maxGeometryInputComponents;
            MaxGeometryOutputComponents = raw->maxGeometryOutputComponents;
            MaxGeometryOutputVertices = raw->maxGeometryOutputVertices;
            MaxGeometryTotalOutputComponents = raw->maxGeometryTotalOutputComponents;
            MaxFragmentInputComponents = raw->maxFragmentInputComponents;
            MaxFragmentOutputAttachments = raw->maxFragmentOutputAttachments;
            MaxFragmentDualSrcAttachments = raw->maxFragmentDualSrcAttachments;
            MaxFragmentCombinedOutputResources = raw->maxFragmentCombinedOutputResources;
            MaxComputeSharedMemorySize = raw->maxComputeSharedMemorySize;
            MaxComputeWorkGroupCount = new VkUintVector3(raw->maxComputeWorkGroupCount);
            MaxComputeWorkGroupInvocations = raw->maxComputeWorkGroupInvocations;
            MaxComputeWorkGroupSize = new VkUintVector3(raw->maxComputeWorkGroupSize);
            SubPixelPrecisionBits = raw->subPixelPrecisionBits;
            SubTexelPrecisionBits = raw->subTexelPrecisionBits;
            MipmapPrecisionBits = raw->mipmapPrecisionBits;
            MaxDrawIndexedIndexValue = raw->maxDrawIndexedIndexValue;
            MaxDrawIndirectCount = raw->maxDrawIndirectCount;
            MaxSamplerLodBias = raw->maxSamplerLodBias;
            MaxSamplerAnisotropy = raw->maxSamplerAnisotropy;
            MaxViewports = raw->maxViewports;
            MaxViewportDimensions = new VkUintVector2(raw->maxViewportDimensions);
            ViewportBoundsRange = new VkVector2(raw->viewportBoundsRange);
            ViewportSubPixelBits = raw->viewportSubPixelBits;
            MinMemoryMapAlignment = raw->minMemoryMapAlignment;
            MinTexelBufferOffsetAlignment = raw->minTexelBufferOffsetAlignment;
            MinUniformBufferOffsetAlignment = raw->minUniformBufferOffsetAlignment;
            MinStorageBufferOffsetAlignment = raw->minStorageBufferOffsetAlignment;
            MinTexelOffset = raw->minTexelOffset;
            MaxTexelOffset = raw->maxTexelOffset;
            MinTexelGatherOffset = raw->minTexelGatherOffset;
            MaxTexelGatherOffset = raw->maxTexelGatherOffset;
            MinInterpolationOffset = raw->minInterpolationOffset;
            MaxInterpolationOffset = raw->maxInterpolationOffset;
            SubPixelInterpolationOffsetBits = raw->subPixelInterpolationOffsetBits;
            MaxFramebufferWidth = raw->maxFramebufferWidth;
            MaxFramebufferHeight = raw->maxFramebufferHeight;
            MaxFramebufferLayers = raw->maxFramebufferLayers;
            FramebufferColorSampleCounts = raw->framebufferColorSampleCounts;
            FramebufferDepthSampleCounts = raw->framebufferDepthSampleCounts;
            FramebufferStencilSampleCounts = raw->framebufferStencilSampleCounts;
            FramebufferNoAttachmentsSampleCounts = raw->framebufferNoAttachmentsSampleCounts;
            MaxColorAttachments = raw->maxColorAttachments;
            SampledImageColorSampleCounts = raw->sampledImageColorSampleCounts;
            SampledImageIntegerSampleCounts = raw->sampledImageIntegerSampleCounts;
            SampledImageDepthSampleCounts = raw->sampledImageDepthSampleCounts;
            SampledImageStencilSampleCounts = raw->sampledImageStencilSampleCounts;
            StorageImageSampleCounts = raw->storageImageSampleCounts;
            MaxSampleMaskWords = raw->maxSampleMaskWords;
            TimestampComputeAndGraphics = (bool)raw->timestampComputeAndGraphics;
            TimestampPeriod = raw->timestampPeriod;
            MaxClipDistances = raw->maxClipDistances;
            MaxCullDistances = raw->maxCullDistances;
            MaxCombinedClipAndCullDistances = raw->maxCombinedClipAndCullDistances;
            DiscreteQueuePriorities = raw->discreteQueuePriorities;
            PointSizeRange = new VkVector2(raw->pointSizeRange);
            LineWidthRange = new VkVector2(raw->lineWidthRange);
            PointSizeGranularity = raw->pointSizeGranularity;
            LineWidthGranularity = raw->lineWidthGranularity;
            StrictLines = (bool)raw->strictLines;
            StandardSampleLocations = (bool)raw->standardSampleLocations;
            OptimalBufferCopyOffsetAlignment = raw->optimalBufferCopyOffsetAlignment;
            OptimalBufferCopyRowPitchAlignment = raw->optimalBufferCopyRowPitchAlignment;
            NonCoherentAtomSize = raw->nonCoherentAtomSize;
        }
    }
}
