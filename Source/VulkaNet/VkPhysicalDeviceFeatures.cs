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
using System.Linq;
using System.Runtime.InteropServices;

namespace VulkaNet
{
    public unsafe class VkPhysicalDeviceFeatures
    {
        public bool RobustBufferAccess { get; set; }
        public bool FullDrawIndexUint32 { get; set; }
        public bool ImageCubeArray { get; set; }
        public bool IndependentBlend { get; set; }
        public bool GeometryShader { get; set; }
        public bool TessellationShader { get; set; }
        public bool SampleRateShading { get; set; }
        public bool DualSrcBlend { get; set; }
        public bool LogicOp { get; set; }
        public bool MultiDrawIndirect { get; set; }
        public bool DrawIndirectFirstInstance { get; set; }
        public bool DepthClamp { get; set; }
        public bool DepthBiasClamp { get; set; }
        public bool FillModeNonSolid { get; set; }
        public bool DepthBounds { get; set; }
        public bool WideLines { get; set; }
        public bool LargePoints { get; set; }
        public bool AlphaToOne { get; set; }
        public bool MultiViewport { get; set; }
        public bool SamplerAnisotropy { get; set; }
        public bool TextureCompressionETC2 { get; set; }
        public bool TextureCompressionASTC_LDR { get; set; }
        public bool TextureCompressionBC { get; set; }
        public bool OcclusionQueryPrecise { get; set; }
        public bool PipelineStatisticsQuery { get; set; }
        public bool VertexPipelineStoresAndAtomics { get; set; }
        public bool FragmentStoresAndAtomics { get; set; }
        public bool ShaderTessellationAndGeometryPointSize { get; set; }
        public bool ShaderImageGatherExtended { get; set; }
        public bool ShaderStorageImageExtendedFormats { get; set; }
        public bool ShaderStorageImageMultisample { get; set; }
        public bool ShaderStorageImageReadWithoutFormat { get; set; }
        public bool ShaderStorageImageWriteWithoutFormat { get; set; }
        public bool ShaderUniformBufferArrayDynamicIndexing { get; set; }
        public bool ShaderSampledImageArrayDynamicIndexing { get; set; }
        public bool ShaderStorageBufferArrayDynamicIndexing { get; set; }
        public bool ShaderStorageImageArrayDynamicIndexing { get; set; }
        public bool ShaderClipDistance { get; set; }
        public bool ShaderCullDistance { get; set; }
        public bool ShaderFloat64 { get; set; }
        public bool ShaderInt64 { get; set; }
        public bool ShaderInt16 { get; set; }
        public bool ShaderResourceResidency { get; set; }
        public bool ShaderResourceMinLod { get; set; }
        public bool SparseBinding { get; set; }
        public bool SparseResidencyBuffer { get; set; }
        public bool SparseResidencyImage2D { get; set; }
        public bool SparseResidencyImage3D { get; set; }
        public bool SparseResidency2Samples { get; set; }
        public bool SparseResidency4Samples { get; set; }
        public bool SparseResidency8Samples { get; set; }
        public bool SparseResidency16Samples { get; set; }
        public bool SparseResidencyAliased { get; set; }
        public bool VariableMultisampleRate { get; set; }
        public bool InheritedQueries { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
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

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }

        public VkPhysicalDeviceFeatures() { }

        public VkPhysicalDeviceFeatures(Raw* raw)
        {
            RobustBufferAccess = (bool)raw->robustBufferAccess;
            FullDrawIndexUint32 = (bool)raw->fullDrawIndexUint32;
            ImageCubeArray = (bool)raw->imageCubeArray;
            IndependentBlend = (bool)raw->independentBlend;
            GeometryShader = (bool)raw->geometryShader;
            TessellationShader = (bool)raw->tessellationShader;
            SampleRateShading = (bool)raw->sampleRateShading;
            DualSrcBlend = (bool)raw->dualSrcBlend;
            LogicOp = (bool)raw->logicOp;
            MultiDrawIndirect = (bool)raw->multiDrawIndirect;
            DrawIndirectFirstInstance = (bool)raw->drawIndirectFirstInstance;
            DepthClamp = (bool)raw->depthClamp;
            DepthBiasClamp = (bool)raw->depthBiasClamp;
            FillModeNonSolid = (bool)raw->fillModeNonSolid;
            DepthBounds = (bool)raw->depthBounds;
            WideLines = (bool)raw->wideLines;
            LargePoints = (bool)raw->largePoints;
            AlphaToOne = (bool)raw->alphaToOne;
            MultiViewport = (bool)raw->multiViewport;
            SamplerAnisotropy = (bool)raw->samplerAnisotropy;
            TextureCompressionETC2 = (bool)raw->textureCompressionETC2;
            TextureCompressionASTC_LDR = (bool)raw->textureCompressionASTC_LDR;
            TextureCompressionBC = (bool)raw->textureCompressionBC;
            OcclusionQueryPrecise = (bool)raw->occlusionQueryPrecise;
            PipelineStatisticsQuery = (bool)raw->pipelineStatisticsQuery;
            VertexPipelineStoresAndAtomics = (bool)raw->vertexPipelineStoresAndAtomics;
            FragmentStoresAndAtomics = (bool)raw->fragmentStoresAndAtomics;
            ShaderTessellationAndGeometryPointSize = (bool)raw->shaderTessellationAndGeometryPointSize;
            ShaderImageGatherExtended = (bool)raw->shaderImageGatherExtended;
            ShaderStorageImageExtendedFormats = (bool)raw->shaderStorageImageExtendedFormats;
            ShaderStorageImageMultisample = (bool)raw->shaderStorageImageMultisample;
            ShaderStorageImageReadWithoutFormat = (bool)raw->shaderStorageImageReadWithoutFormat;
            ShaderStorageImageWriteWithoutFormat = (bool)raw->shaderStorageImageWriteWithoutFormat;
            ShaderUniformBufferArrayDynamicIndexing = (bool)raw->shaderUniformBufferArrayDynamicIndexing;
            ShaderSampledImageArrayDynamicIndexing = (bool)raw->shaderSampledImageArrayDynamicIndexing;
            ShaderStorageBufferArrayDynamicIndexing = (bool)raw->shaderStorageBufferArrayDynamicIndexing;
            ShaderStorageImageArrayDynamicIndexing = (bool)raw->shaderStorageImageArrayDynamicIndexing;
            ShaderClipDistance = (bool)raw->shaderClipDistance;
            ShaderCullDistance = (bool)raw->shaderCullDistance;
            ShaderFloat64 = (bool)raw->shaderFloat64;
            ShaderInt64 = (bool)raw->shaderInt64;
            ShaderInt16 = (bool)raw->shaderInt16;
            ShaderResourceResidency = (bool)raw->shaderResourceResidency;
            ShaderResourceMinLod = (bool)raw->shaderResourceMinLod;
            SparseBinding = (bool)raw->sparseBinding;
            SparseResidencyBuffer = (bool)raw->sparseResidencyBuffer;
            SparseResidencyImage2D = (bool)raw->sparseResidencyImage2D;
            SparseResidencyImage3D = (bool)raw->sparseResidencyImage3D;
            SparseResidency2Samples = (bool)raw->sparseResidency2Samples;
            SparseResidency4Samples = (bool)raw->sparseResidency4Samples;
            SparseResidency8Samples = (bool)raw->sparseResidency8Samples;
            SparseResidency16Samples = (bool)raw->sparseResidency16Samples;
            SparseResidencyAliased = (bool)raw->sparseResidencyAliased;
            VariableMultisampleRate = (bool)raw->variableMultisampleRate;
            InheritedQueries = (bool)raw->inheritedQueries;
        }
    }

    public static unsafe class VkPhysicalDeviceFeaturesExtensions
    {
        public static int SizeOfMarshalDirect(this VkPhysicalDeviceFeatures s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return 0;
        }

        public static VkPhysicalDeviceFeatures.Raw MarshalDirect(this VkPhysicalDeviceFeatures s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");


            VkPhysicalDeviceFeatures.Raw result;
            result.robustBufferAccess = new VkBool32(s.RobustBufferAccess);
            result.fullDrawIndexUint32 = new VkBool32(s.FullDrawIndexUint32);
            result.imageCubeArray = new VkBool32(s.ImageCubeArray);
            result.independentBlend = new VkBool32(s.IndependentBlend);
            result.geometryShader = new VkBool32(s.GeometryShader);
            result.tessellationShader = new VkBool32(s.TessellationShader);
            result.sampleRateShading = new VkBool32(s.SampleRateShading);
            result.dualSrcBlend = new VkBool32(s.DualSrcBlend);
            result.logicOp = new VkBool32(s.LogicOp);
            result.multiDrawIndirect = new VkBool32(s.MultiDrawIndirect);
            result.drawIndirectFirstInstance = new VkBool32(s.DrawIndirectFirstInstance);
            result.depthClamp = new VkBool32(s.DepthClamp);
            result.depthBiasClamp = new VkBool32(s.DepthBiasClamp);
            result.fillModeNonSolid = new VkBool32(s.FillModeNonSolid);
            result.depthBounds = new VkBool32(s.DepthBounds);
            result.wideLines = new VkBool32(s.WideLines);
            result.largePoints = new VkBool32(s.LargePoints);
            result.alphaToOne = new VkBool32(s.AlphaToOne);
            result.multiViewport = new VkBool32(s.MultiViewport);
            result.samplerAnisotropy = new VkBool32(s.SamplerAnisotropy);
            result.textureCompressionETC2 = new VkBool32(s.TextureCompressionETC2);
            result.textureCompressionASTC_LDR = new VkBool32(s.TextureCompressionASTC_LDR);
            result.textureCompressionBC = new VkBool32(s.TextureCompressionBC);
            result.occlusionQueryPrecise = new VkBool32(s.OcclusionQueryPrecise);
            result.pipelineStatisticsQuery = new VkBool32(s.PipelineStatisticsQuery);
            result.vertexPipelineStoresAndAtomics = new VkBool32(s.VertexPipelineStoresAndAtomics);
            result.fragmentStoresAndAtomics = new VkBool32(s.FragmentStoresAndAtomics);
            result.shaderTessellationAndGeometryPointSize = new VkBool32(s.ShaderTessellationAndGeometryPointSize);
            result.shaderImageGatherExtended = new VkBool32(s.ShaderImageGatherExtended);
            result.shaderStorageImageExtendedFormats = new VkBool32(s.ShaderStorageImageExtendedFormats);
            result.shaderStorageImageMultisample = new VkBool32(s.ShaderStorageImageMultisample);
            result.shaderStorageImageReadWithoutFormat = new VkBool32(s.ShaderStorageImageReadWithoutFormat);
            result.shaderStorageImageWriteWithoutFormat = new VkBool32(s.ShaderStorageImageWriteWithoutFormat);
            result.shaderUniformBufferArrayDynamicIndexing = new VkBool32(s.ShaderUniformBufferArrayDynamicIndexing);
            result.shaderSampledImageArrayDynamicIndexing = new VkBool32(s.ShaderSampledImageArrayDynamicIndexing);
            result.shaderStorageBufferArrayDynamicIndexing = new VkBool32(s.ShaderStorageBufferArrayDynamicIndexing);
            result.shaderStorageImageArrayDynamicIndexing = new VkBool32(s.ShaderStorageImageArrayDynamicIndexing);
            result.shaderClipDistance = new VkBool32(s.ShaderClipDistance);
            result.shaderCullDistance = new VkBool32(s.ShaderCullDistance);
            result.shaderFloat64 = new VkBool32(s.ShaderFloat64);
            result.shaderInt64 = new VkBool32(s.ShaderInt64);
            result.shaderInt16 = new VkBool32(s.ShaderInt16);
            result.shaderResourceResidency = new VkBool32(s.ShaderResourceResidency);
            result.shaderResourceMinLod = new VkBool32(s.ShaderResourceMinLod);
            result.sparseBinding = new VkBool32(s.SparseBinding);
            result.sparseResidencyBuffer = new VkBool32(s.SparseResidencyBuffer);
            result.sparseResidencyImage2D = new VkBool32(s.SparseResidencyImage2D);
            result.sparseResidencyImage3D = new VkBool32(s.SparseResidencyImage3D);
            result.sparseResidency2Samples = new VkBool32(s.SparseResidency2Samples);
            result.sparseResidency4Samples = new VkBool32(s.SparseResidency4Samples);
            result.sparseResidency8Samples = new VkBool32(s.SparseResidency8Samples);
            result.sparseResidency16Samples = new VkBool32(s.SparseResidency16Samples);
            result.sparseResidencyAliased = new VkBool32(s.SparseResidencyAliased);
            result.variableMultisampleRate = new VkBool32(s.VariableMultisampleRate);
            result.inheritedQueries = new VkBool32(s.InheritedQueries);
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkPhysicalDeviceFeatures s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkPhysicalDeviceFeatures.Raw.SizeInBytes;

        public static VkPhysicalDeviceFeatures.Raw* MarshalIndirect(this VkPhysicalDeviceFeatures s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkPhysicalDeviceFeatures.Raw*)0;
            var result = (VkPhysicalDeviceFeatures.Raw*)unmanaged;
            unmanaged += VkPhysicalDeviceFeatures.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkPhysicalDeviceFeatures> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkPhysicalDeviceFeatures.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkPhysicalDeviceFeatures.Raw* MarshalDirect(this IReadOnlyList<VkPhysicalDeviceFeatures> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPhysicalDeviceFeatures.Raw*)0;
            var result = (VkPhysicalDeviceFeatures.Raw*)unmanaged;
            unmanaged += sizeof(VkPhysicalDeviceFeatures.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<VkPhysicalDeviceFeatures> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkPhysicalDeviceFeatures.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkPhysicalDeviceFeatures.Raw** MarshalIndirect(this IReadOnlyList<VkPhysicalDeviceFeatures> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPhysicalDeviceFeatures.Raw**)0;
            var result = (VkPhysicalDeviceFeatures.Raw**)unmanaged;
            unmanaged += sizeof(VkPhysicalDeviceFeatures.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
