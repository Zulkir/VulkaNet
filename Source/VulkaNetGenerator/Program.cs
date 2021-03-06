﻿#region License
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

using VulkaNetGenerator.GenStructs;

namespace VulkaNetGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new Generator();
            generator.GenerateStruct<GenApplicationInfo>(true, false);
            generator.GenerateStruct<GenInstanceCreateInfo>(true, false);
            generator.GenerateStruct<GenPhysicalDeviceSparseProperties>(false, true);
            generator.GenerateStruct<GenPhysicalDeviceLimits>(false, true);
            generator.GenerateStruct<GenPhysicalDeviceProperties>(false, true);
            generator.GenerateStruct<GenDeviceQueueCreateInfo>(true, false);
            generator.GenerateStruct<GenCommandPoolCreateInfo>(true, false);
            generator.GenerateStruct<GenCommandBufferAllocateInfo>(true, false);
            generator.GenerateStruct<GenCommandBufferBeginInfo>(true, false);
            generator.GenerateStruct<GenCommandBufferInheritanceInfo>(true, false);
            generator.GenerateStruct<GenSubmitInfo>(true, false);
            generator.GenerateStruct<GenPhysicalDeviceFeatures>(true, true);
            generator.GenerateStruct<GenDeviceCreateInfo>(true, false);
            generator.GenerateStruct<GenFenceCreateInfo>(true, false);
            generator.GenerateStruct<GenSemaphoreCreateInfo>(true, false);
            generator.GenerateStruct<GenEventCreateInfo>(true, false);
            generator.GenerateStruct<GenMemoryBarrier>(true, false);
            generator.GenerateStruct<GenBufferMemoryBarrier>(true, false);
            generator.GenerateStruct<GenImageMemoryBarrier>(true, false);
            generator.GenerateStruct<GenRenderPassCreateInfo>(true, false);
            //generator.GenerateStruct<GenAttachmentDescription>(true, false);
            generator.GenerateStruct<GenSubpassDescription>(true, false);
            //generator.GenerateStruct<GenSubpassDependency>(true, false);
            generator.GenerateStruct<GenFramebufferCreateInfo>(true, false);
            generator.GenerateStruct<GenRenderPassBeginInfo>(true, false);
            generator.GenerateStruct<GenShaderModuleCreateInfo>(true, false);
            generator.GenerateStruct<GenComputePipelineCreateInfo>(true, false);
            generator.GenerateStruct<GenPipelineShaderStageCreateInfo>(true, false);
            generator.GenerateStruct<GenSpecializationInfo>(true, false);
            generator.GenerateStruct<GenGraphicsPipelineCreateInfo>(true, false);
            generator.GenerateStruct<GenPipelineVertexInputStateCreateInfo>(true, false);
            generator.GenerateStruct<GenPipelineInputAssemblyStateCreateInfo>(true, false);
            generator.GenerateStruct<GenPipelineTessellationStateCreateInfo>(true, false);
            generator.GenerateStruct<GenPipelineViewportStateCreateInfo>(true, false);
            generator.GenerateStruct<GenPipelineRasterizationStateCreateInfo>(true, false);
            generator.GenerateStruct<GenPipelineMultisampleStateCreateInfo>(true, false);
            generator.GenerateStruct<GenPipelineDepthStencilStateCreateInfo>(true, false);
            generator.GenerateStruct<GenPipelineColorBlendStateCreateInfo>(true, false);
            generator.GenerateStruct<GenPipelineDynamicStateCreateInfo>(true, false);
            generator.GenerateStruct<GenPipelineCacheCreateInfo>(true, false);
            //generator.GenerateStruct<GenPhysicalDeviceMemoryProperties>(false, true);
            generator.GenerateStruct<GenMemoryAllocateInfo>(true, false);
            generator.GenerateStruct<GenMappedMemoryRange>(true, false);
            generator.GenerateStruct<GenBufferCreateInfo>(true, false);
            generator.GenerateStruct<GenBufferViewCreateInfo>(true, false);
            generator.GenerateStruct<GenImageCreateInfo>(true, false);
            generator.GenerateStruct<GenImageViewCreateInfo>(true, false);
            generator.GenerateStruct<GenSamplerCreateInfo>(true, false);
            generator.GenerateStruct<GenDescriptorSetLayoutCreateInfo>(true, false);
            generator.GenerateStruct<GenDescriptorSetLayoutBinding>(true, false);
            generator.GenerateStruct<GenPipelineLayoutCreateInfo>(true, false);
            generator.GenerateStruct<GenDescriptorPoolCreateInfo>(true, false);
            generator.GenerateStruct<GenDescriptorSetAllocateInfo>(true, false);
            generator.GenerateStruct<GenWriteDescriptorSet>(true, false);
            generator.GenerateStruct<GenDescriptorImageInfo>(true, false);
            generator.GenerateStruct<GenDescriptorBufferInfo>(true, false);
            generator.GenerateStruct<GenCopyDescriptorSet>(true, false);
            generator.GenerateStruct<GenQueryPoolCreateInfo>(true, false);
            generator.GenerateStruct<GenSparseBufferMemoryBindInfo>(true, false);
            generator.GenerateStruct<GenSparseMemoryBind>(true, false);
            generator.GenerateStruct<GenSparseImageMemoryBindInfo>(true, false);
            generator.GenerateStruct<GenSparseImageOpaqueMemoryBindInfo>(true, false);
            generator.GenerateStruct<GenBindSparseInfo>(true, false);
            generator.GenerateStruct<GenSparseImageMemoryBind>(true, false);
            generator.GenerateStruct<GenAndroidSurfaceCreateInfoKHR>(true, false);
            generator.GenerateStruct<GenMirSurfaceCreateInfoKHR>(true, false);
            generator.GenerateStruct<GenWaylandSurfaceCreateInfoKHR>(true, false);
            generator.GenerateStruct<GenWin32SurfaceCreateInfoKHR>(true, false);
            generator.GenerateStruct<GenXcbSurfaceCreateInfoKHR>(true, false);
            generator.GenerateStruct<GenXlibSurfaceCreateInfoKHR>(true, false);
            generator.GenerateStruct<GenDisplayPropertiesKHR>(false, true);
            generator.GenerateStruct<GenDisplayPlanePropertiesKHR>(false, true);
            generator.GenerateStruct<GenDisplayModePropertiesKHR>(false, true);
            generator.GenerateStruct<GenDisplayModeCreateInfoKHR>(true, false);
            generator.GenerateStruct<GenDisplaySurfaceCreateInfoKHR>(true, false);
            generator.GenerateStruct<GenSwapchainCreateInfoKHR>(true, false);
            generator.GenerateStruct<GenPresentInfoKHR>(true, false);
            generator.GenerateStruct<GenDisplayPresentInfoKHR>(true, false);
            generator.GenerateStruct<GenPresentRegionsKHR>(true, false);
            generator.GenerateStruct<GenPresentRegionKHR>(true, false);
            generator.GenerateStruct<GenDebugMarkerObjectNameInfoEXT>(true, false);
            generator.GenerateStruct<GenDebugMarkerObjectTagInfoEXT>(true, false);
            generator.GenerateStruct<GenDebugMarkerMarkerInfoEXT>(true, false);
            generator.GenerateStruct<GenDebugReportCallbackCreateInfoEXT>(true, false);

            generator.GenerateClass<GenQueue>();
            generator.GenerateClass<GenCommandPool>();
            generator.GenerateClass<GenCommandBuffer>();
            generator.GenerateClass<GenFence>();
            generator.GenerateClass<GenSemaphore>();
            generator.GenerateClass<GenEvent>();
            generator.GenerateClass<GenBuffer>();
            generator.GenerateClass<GenImage>();
            generator.GenerateClass<GenRenderPass>();
            generator.GenerateClass<GenFramebuffer>();
            generator.GenerateClass<GenImageView>();
            generator.GenerateClass<GenShaderModule>();
            generator.GenerateClass<GenPipeline>();
            generator.GenerateClass<GenPipelineLayout>();
            generator.GenerateClass<GenPipelineCache>();
            generator.GenerateClass<GenDeviceMemory>();
            generator.GenerateClass<GenBufferView>();
            generator.GenerateClass<GenSampler>();
            generator.GenerateClass<GenDescriptorSetLayout>();
            generator.GenerateClass<GenDescriptorPool>();
            generator.GenerateClass<GenDescriptorSet>();
            generator.GenerateClass<GenQueryPool>();
            generator.GenerateClass<GenSurfaceKHR>();
            generator.GenerateClass<GenDisplayKHR>();
            generator.GenerateClass<GenDisplayModeKHR>();
            generator.GenerateClass<GenSwapchainKHR>();
            generator.GenerateClass<GenDebugReportCallbackEXT>();

            generator.GenerateClass<GenDevice>();
        }
    }
}
