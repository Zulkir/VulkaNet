﻿using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDescriptorSetLayoutBinding
    {
        // todo: fill automatically
        public int binding;
        public VkDescriptorType descriptorType;
        public int descriptorCount;
        public VkShaderStageFlagBits stageFlags;
        [IsArray] public GenSampler* pImmutableSamplers;
    }
}