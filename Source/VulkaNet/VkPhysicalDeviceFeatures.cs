using System.Runtime.InteropServices;

namespace VulkaNet
{
    public interface IVkPhysicalDeviceFeatures
    {
        VkBool32 RobustBufferAccess { get; }
        VkBool32 FullDrawIndexUint32 { get; }
        VkBool32 ImageCubeArray { get; }
        VkBool32 IndependentBlend { get; }
        VkBool32 GeometryShader { get; }
        VkBool32 TessellationShader { get; }
        VkBool32 SampleRateShading { get; }
        VkBool32 DualSrcBlend { get; }
        VkBool32 LogicOp { get; }
        VkBool32 MultiDrawIndirect { get; }
        VkBool32 DrawIndirectFirstInstance { get; }
        VkBool32 DepthClamp { get; }
        VkBool32 DepthBiasClamp { get; }
        VkBool32 FillModeNonSolid { get; }
        VkBool32 DepthBounds { get; }
        VkBool32 WideLines { get; }
        VkBool32 LargePoints { get; }
        VkBool32 AlphaToOne { get; }
        VkBool32 MultiViewport { get; }
        VkBool32 SamplerAnisotropy { get; }
        VkBool32 TextureCompressionEtc2 { get; }
        VkBool32 TextureCompressionAstcLdr { get; }
        VkBool32 TextureCompressionBc { get; }
        VkBool32 OcclusionQueryPrecise { get; }
        VkBool32 PipelineStatisticsQuery { get; }
        VkBool32 VertexPipelineStoresAndAtomics { get; }
        VkBool32 FragmentStoresAndAtomics { get; }
        VkBool32 ShaderTessellationAndGeometryPointSize { get; }
        VkBool32 ShaderImageGatherExtended { get; }
        VkBool32 ShaderStorageImageExtendedFormats { get; }
        VkBool32 ShaderStorageImageMultisample { get; }
        VkBool32 ShaderStorageImageReadWithoutFormat { get; }
        VkBool32 ShaderStorageImageWriteWithoutFormat { get; }
        VkBool32 ShaderUniformBufferArrayDynamicIndexing { get; }
        VkBool32 ShaderSampledImageArrayDynamicIndexing { get; }
        VkBool32 ShaderStorageBufferArrayDynamicIndexing { get; }
        VkBool32 ShaderStorageImageArrayDynamicIndexing { get; }
        VkBool32 ShaderClipDistance { get; }
        VkBool32 ShaderCullDistance { get; }
        VkBool32 ShaderFloat64 { get; }
        VkBool32 ShaderInt64 { get; }
        VkBool32 ShaderInt16 { get; }
        VkBool32 ShaderResourceResidency { get; }
        VkBool32 ShaderResourceMinLod { get; }
        VkBool32 SparseBinding { get; }
        VkBool32 SparseResidencyBuffer { get; }
        VkBool32 SparseResidencyImage2D { get; }
        VkBool32 SparseResidencyImage3D { get; }
        VkBool32 SparseResidency2Samples { get; }
        VkBool32 SparseResidency4Samples { get; }
        VkBool32 SparseResidency8Samples { get; }
        VkBool32 SparseResidency16Samples { get; }
        VkBool32 SparseResidencyAliased { get; }
        VkBool32 VariableMultisampleRate { get; }
        VkBool32 InheritedQueries { get; }
    }

    public unsafe class VkPhysicalDeviceFeatures : IVkPhysicalDeviceFeatures
    {
        public VkBool32 RobustBufferAccess { get; }
        public VkBool32 FullDrawIndexUint32 { get; }
        public VkBool32 ImageCubeArray { get; }
        public VkBool32 IndependentBlend { get; }
        public VkBool32 GeometryShader { get; }
        public VkBool32 TessellationShader { get; }
        public VkBool32 SampleRateShading { get; }
        public VkBool32 DualSrcBlend { get; }
        public VkBool32 LogicOp { get; }
        public VkBool32 MultiDrawIndirect { get; }
        public VkBool32 DrawIndirectFirstInstance { get; }
        public VkBool32 DepthClamp { get; }
        public VkBool32 DepthBiasClamp { get; }
        public VkBool32 FillModeNonSolid { get; }
        public VkBool32 DepthBounds { get; }
        public VkBool32 WideLines { get; }
        public VkBool32 LargePoints { get; }
        public VkBool32 AlphaToOne { get; }
        public VkBool32 MultiViewport { get; }
        public VkBool32 SamplerAnisotropy { get; }
        public VkBool32 TextureCompressionEtc2 { get; }
        public VkBool32 TextureCompressionAstcLdr { get; }
        public VkBool32 TextureCompressionBc { get; }
        public VkBool32 OcclusionQueryPrecise { get; }
        public VkBool32 PipelineStatisticsQuery { get; }
        public VkBool32 VertexPipelineStoresAndAtomics { get; }
        public VkBool32 FragmentStoresAndAtomics { get; }
        public VkBool32 ShaderTessellationAndGeometryPointSize { get; }
        public VkBool32 ShaderImageGatherExtended { get; }
        public VkBool32 ShaderStorageImageExtendedFormats { get; }
        public VkBool32 ShaderStorageImageMultisample { get; }
        public VkBool32 ShaderStorageImageReadWithoutFormat { get; }
        public VkBool32 ShaderStorageImageWriteWithoutFormat { get; }
        public VkBool32 ShaderUniformBufferArrayDynamicIndexing { get; }
        public VkBool32 ShaderSampledImageArrayDynamicIndexing { get; }
        public VkBool32 ShaderStorageBufferArrayDynamicIndexing { get; }
        public VkBool32 ShaderStorageImageArrayDynamicIndexing { get; }
        public VkBool32 ShaderClipDistance { get; }
        public VkBool32 ShaderCullDistance { get; }
        public VkBool32 ShaderFloat64 { get; }
        public VkBool32 ShaderInt64 { get; }
        public VkBool32 ShaderInt16 { get; }
        public VkBool32 ShaderResourceResidency { get; }
        public VkBool32 ShaderResourceMinLod { get; }
        public VkBool32 SparseBinding { get; }
        public VkBool32 SparseResidencyBuffer { get; }
        public VkBool32 SparseResidencyImage2D { get; }
        public VkBool32 SparseResidencyImage3D { get; }
        public VkBool32 SparseResidency2Samples { get; }
        public VkBool32 SparseResidency4Samples { get; }
        public VkBool32 SparseResidency8Samples { get; }
        public VkBool32 SparseResidency16Samples { get; }
        public VkBool32 SparseResidencyAliased { get; }
        public VkBool32 VariableMultisampleRate { get; }
        public VkBool32 InheritedQueries { get; }
        
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
            RobustBufferAccess = raw->robustBufferAccess;
            FullDrawIndexUint32 = raw->fullDrawIndexUint32;
            ImageCubeArray = raw->imageCubeArray;
            IndependentBlend = raw->independentBlend;
            GeometryShader = raw->geometryShader;
            TessellationShader = raw->tessellationShader;
            SampleRateShading = raw->sampleRateShading;
            DualSrcBlend = raw->dualSrcBlend;
            LogicOp = raw->logicOp;
            MultiDrawIndirect = raw->multiDrawIndirect;
            DrawIndirectFirstInstance = raw->drawIndirectFirstInstance;
            DepthClamp = raw->depthClamp;
            DepthBiasClamp = raw->depthBiasClamp;
            FillModeNonSolid = raw->fillModeNonSolid;
            DepthBounds = raw->depthBounds;
            WideLines = raw->wideLines;
            LargePoints = raw->largePoints;
            AlphaToOne = raw->alphaToOne;
            MultiViewport = raw->multiViewport;
            SamplerAnisotropy = raw->samplerAnisotropy;
            TextureCompressionEtc2 = raw->textureCompressionETC2;
            TextureCompressionAstcLdr = raw->textureCompressionASTC_LDR;
            TextureCompressionBc = raw->textureCompressionBC;
            OcclusionQueryPrecise = raw->occlusionQueryPrecise;
            PipelineStatisticsQuery = raw->pipelineStatisticsQuery;
            VertexPipelineStoresAndAtomics = raw->vertexPipelineStoresAndAtomics;
            FragmentStoresAndAtomics = raw->fragmentStoresAndAtomics;
            ShaderTessellationAndGeometryPointSize = raw->shaderTessellationAndGeometryPointSize;
            ShaderImageGatherExtended = raw->shaderImageGatherExtended;
            ShaderStorageImageExtendedFormats = raw->shaderStorageImageExtendedFormats;
            ShaderStorageImageMultisample = raw->shaderStorageImageMultisample;
            ShaderStorageImageReadWithoutFormat = raw->shaderStorageImageReadWithoutFormat;
            ShaderStorageImageWriteWithoutFormat = raw->shaderStorageImageWriteWithoutFormat;
            ShaderUniformBufferArrayDynamicIndexing = raw->shaderUniformBufferArrayDynamicIndexing;
            ShaderSampledImageArrayDynamicIndexing = raw->shaderSampledImageArrayDynamicIndexing;
            ShaderStorageBufferArrayDynamicIndexing = raw->shaderStorageBufferArrayDynamicIndexing;
            ShaderStorageImageArrayDynamicIndexing = raw->shaderStorageImageArrayDynamicIndexing;
            ShaderClipDistance = raw->shaderClipDistance;
            ShaderCullDistance = raw->shaderCullDistance;
            ShaderFloat64 = raw->shaderFloat64;
            ShaderInt64 = raw->shaderInt64;
            ShaderInt16 = raw->shaderInt16;
            ShaderResourceResidency = raw->shaderResourceResidency;
            ShaderResourceMinLod = raw->shaderResourceMinLod;
            SparseBinding = raw->sparseBinding;
            SparseResidencyBuffer = raw->sparseResidencyBuffer;
            SparseResidencyImage2D = raw->sparseResidencyImage2D;
            SparseResidencyImage3D = raw->sparseResidencyImage3D;
            SparseResidency2Samples = raw->sparseResidency2Samples;
            SparseResidency4Samples = raw->sparseResidency4Samples;
            SparseResidency8Samples = raw->sparseResidency8Samples;
            SparseResidency16Samples = raw->sparseResidency16Samples;
            SparseResidencyAliased = raw->sparseResidencyAliased;
            VariableMultisampleRate = raw->variableMultisampleRate;
            InheritedQueries = raw->inheritedQueries;
        }
    }

    public static class VkPhysicalDeviceFeaturesExtensions
    {
        public static int SafeMarshalSize(this IVkPhysicalDeviceFeatures s)
            => s != null 
                ? VkPhysicalDeviceFeatures.Raw.SizeInBytes 
                : 0;

        public static unsafe VkPhysicalDeviceFeatures.Raw* SafeMarshalTo(this IVkPhysicalDeviceFeatures s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkPhysicalDeviceFeatures.Raw*)0;

            var result = (VkPhysicalDeviceFeatures.Raw*)unmanaged;
            unmanaged += VkPhysicalDeviceFeatures.Raw.SizeInBytes;
            result->robustBufferAccess = s.RobustBufferAccess;
            result->fullDrawIndexUint32 = s.FullDrawIndexUint32;
            result->imageCubeArray = s.ImageCubeArray;
            result->independentBlend = s.IndependentBlend;
            result->geometryShader = s.GeometryShader;
            result->tessellationShader = s.TessellationShader;
            result->sampleRateShading = s.SampleRateShading;
            result->dualSrcBlend = s.DualSrcBlend;
            result->logicOp = s.LogicOp;
            result->multiDrawIndirect = s.MultiDrawIndirect;
            result->drawIndirectFirstInstance = s.DrawIndirectFirstInstance;
            result->depthClamp = s.DepthClamp;
            result->depthBiasClamp = s.DepthBiasClamp;
            result->fillModeNonSolid = s.FillModeNonSolid;
            result->depthBounds = s.DepthBounds;
            result->wideLines = s.WideLines;
            result->largePoints = s.LargePoints;
            result->alphaToOne = s.AlphaToOne;
            result->multiViewport = s.MultiViewport;
            result->samplerAnisotropy = s.SamplerAnisotropy;
            result->textureCompressionETC2 = s.TextureCompressionEtc2;
            result->textureCompressionASTC_LDR = s.TextureCompressionAstcLdr;
            result->textureCompressionBC = s.TextureCompressionBc;
            result->occlusionQueryPrecise = s.OcclusionQueryPrecise;
            result->pipelineStatisticsQuery = s.PipelineStatisticsQuery;
            result->vertexPipelineStoresAndAtomics = s.VertexPipelineStoresAndAtomics;
            result->fragmentStoresAndAtomics = s.FragmentStoresAndAtomics;
            result->shaderTessellationAndGeometryPointSize = s.ShaderTessellationAndGeometryPointSize;
            result->shaderImageGatherExtended = s.ShaderImageGatherExtended;
            result->shaderStorageImageExtendedFormats = s.ShaderStorageImageExtendedFormats;
            result->shaderStorageImageMultisample = s.ShaderStorageImageMultisample;
            result->shaderStorageImageReadWithoutFormat = s.ShaderStorageImageReadWithoutFormat;
            result->shaderStorageImageWriteWithoutFormat = s.ShaderStorageImageWriteWithoutFormat;
            result->shaderUniformBufferArrayDynamicIndexing = s.ShaderUniformBufferArrayDynamicIndexing;
            result->shaderSampledImageArrayDynamicIndexing = s.ShaderSampledImageArrayDynamicIndexing;
            result->shaderStorageBufferArrayDynamicIndexing = s.ShaderStorageBufferArrayDynamicIndexing;
            result->shaderStorageImageArrayDynamicIndexing = s.ShaderStorageImageArrayDynamicIndexing;
            result->shaderClipDistance = s.ShaderClipDistance;
            result->shaderCullDistance = s.ShaderCullDistance;
            result->shaderFloat64 = s.ShaderFloat64;
            result->shaderInt64 = s.ShaderInt64;
            result->shaderInt16 = s.ShaderInt16;
            result->shaderResourceResidency = s.ShaderResourceResidency;
            result->shaderResourceMinLod = s.ShaderResourceMinLod;
            result->sparseBinding = s.SparseBinding;
            result->sparseResidencyBuffer = s.SparseResidencyBuffer;
            result->sparseResidencyImage2D = s.SparseResidencyImage2D;
            result->sparseResidencyImage3D = s.SparseResidencyImage3D;
            result->sparseResidency2Samples = s.SparseResidency2Samples;
            result->sparseResidency4Samples = s.SparseResidency4Samples;
            result->sparseResidency8Samples = s.SparseResidency8Samples;
            result->sparseResidency16Samples = s.SparseResidency16Samples;
            result->sparseResidencyAliased = s.SparseResidencyAliased;
            result->variableMultisampleRate = s.VariableMultisampleRate;
            result->inheritedQueries = s.InheritedQueries;
            return result;
        }
    }
}