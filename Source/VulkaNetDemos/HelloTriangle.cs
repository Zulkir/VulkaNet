using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VulkaNet;

namespace VulkaNetDemos
{
    public class HelloTriangle : IDisposable
    {
        private unsafe struct Vertex
        {
            public float X, Y;
            public float R, G, B;

            public Vertex(float x, float y, float r, float g, float b)
            {
                X = x;
                Y = y;
                R = r;
                G = g;
                B = b;
            }

            public static VkVertexInputBindingDescription GetBindingDescription(int binding)
            {
                return new VkVertexInputBindingDescription
                {
                    Binding = binding,
                    Stride = sizeof(Vertex),
                    InputRate = VkVertexInputRate.Vertex
                };
            }

            public static VkVertexInputAttributeDescription[] GetAttributeDescriptions(int binding)
            {
                return new[]
                {
                    new VkVertexInputAttributeDescription
                    {
                        Binding = binding,
                        Location = 0,
                        Format = VkFormat.R32G32_SFLOAT,
                        Offset = 0
                    },
                    new VkVertexInputAttributeDescription
                    {
                        Binding = binding,
                        Location = 1,
                        Format = VkFormat.R32G32B32_SFLOAT,
                        Offset = 2 * sizeof(float)
                    },
                };
            }
        }

        private struct QueueFamilyIndices
        {
            public int GraphicsFamily;
            public int PresentFamily;
        }

        private struct SwapChainSupportDetails
        {
            public VkSurfaceCapabilitiesKHR Capabilities;
            public IReadOnlyList<VkSurfaceFormatKHR> Formats;
            public IReadOnlyList<VkPresentModeKHR> PresentModes;
        };

        private const bool EnableValidationLayers = true;

        private static readonly string[] LayerNames = {"VK_LAYER_LUNARG_standard_validation"};
        private static readonly string[] DeviceExtensions = {"VK_KHR_swapchain"};

        private static readonly Vertex[] Vertices = {
            new Vertex(-0.5f, -0.5f, 1, 0, 0),
            new Vertex(0.5f, -0.5f, 0, 1, 0),
            new Vertex(0.5f, 0.5f, 0, 0, 1),
            new Vertex(-0.5f, 0.5f, 1, 1, 1)
        };

        private static readonly ushort[] Indices =
        {
            0, 1, 2, 2, 3, 0
        };

        private IVkGlobal vkGlobal;
        private Form form;
        private IVkInstance instance;
        private static IVkDebugReportCallbackEXT callback;
        private IVkSurfaceKHR surface;
        private IVkPhysicalDevice physicalDevice;
        private IVkDevice device;
        private IVkQueue graphicsQueue;
        private IVkQueue presentQueue;
        private IVkSwapchainKHR swapChain;
        private IReadOnlyList<IVkImage> swapChainImages;
        private VkFormat swapchainImageFormat;
        private VkExtent2D swapChainExtent;
        private IReadOnlyList<IVkImageView> swapChainImageViews;
        private IVkRenderPass renderPass;
        private IVkPipelineLayout pipelineLayout;
        private IVkPipeline graphicsPipeline;
        private IVkFramebuffer[] swapChainFramebuffers;
        private IVkCommandPool commandPool;
        private IReadOnlyList<IVkCommandBuffer> commandBuffers;
        private IVkSemaphore imageAvailableSemaphore;
        private IVkSemaphore renderFinishedSemaphore;
        private IVkBuffer vertexBuffer;
        private IVkDeviceMemory vertexBufferMemory;
        private IVkBuffer indexBuffer;
        private IVkDeviceMemory indexBufferMemory;

        public HelloTriangle(IVkGlobal vkGlobal, Form form)
        {
            this.vkGlobal = vkGlobal;
            this.form = form;
            form.Resize += OnResize;
        }

        public void Init()
        {
            CreateInstance();
            SetupDebugCallback();
            CreateSurface();
            PickPhysicalDevice();
            CreateLogicalDevice();
            CreateCommandPool();
            CreateSemaphores();
            CreateVertexBuffer();
            CreateIndexBuffer();
            RecreateSwapChain();
        }

        private void RecreateSwapChain()
        {
            CreateSwapChain();
            CreateImageViews();
            CreateRenderPass();
            CreateGraphicsPipeline();
            CreateFramebuffers();
            CreateCommandBuffers();
        }

        public void Dispose()
        {
            CleanupSwapChain();
            indexBuffer.Dispose();
            indexBufferMemory.Dispose();
            vertexBuffer.Dispose();
            vertexBufferMemory.Dispose();
            imageAvailableSemaphore.Dispose();
            renderFinishedSemaphore.Dispose();
            commandPool.Dispose();
            device.Dispose();
            callback.Dispose();
            surface.Dispose();
            instance.Dispose();
        }

        private void CleanupSwapChain()
        {
            device.WaitIdle();
            foreach (var framebuffer in swapChainFramebuffers)
                framebuffer.Dispose();
            commandPool.FreeCommandBuffers(commandBuffers);
            graphicsPipeline.Dispose();
            pipelineLayout.Dispose();
            renderPass.Dispose();
            foreach (var view in swapChainImageViews)
                view.Dispose();
            swapChain.Dispose();
        }

        private void OnResize(object sender, EventArgs eventArgs)
        {
            device.WaitIdle();
            CleanupSwapChain();
            RecreateSwapChain();
        }

        private void CreateInstance()
        {
            var appInfo = new VkApplicationInfo
            {
                ApplicationName = "Hello Triangle",
                ApplicationVersion = (uint)new VkApiVersion(1, 0, 0).Raw,
                EngineName = "No Engine",
                EngineVersion = (uint)new VkApiVersion(1, 0, 0).Raw,
                ApiVersion = new VkApiVersion(1, 0, 0)
            };
            var instanceCreateInfo = new VkInstanceCreateInfo
            {
                ApplicationInfo = appInfo,
                EnabledExtensionNames = new[]
                {
                    "VK_KHR_surface",
                    "VK_KHR_win32_surface",
                    VkDefines.VK_EXT_DEBUG_REPORT_EXTENSION_NAME
                },
                EnabledLayerNames = LayerNames
            };
            instance = vkGlobal.CreateInstance(instanceCreateInfo, null).Object;
        }

        private void SetupDebugCallback()
        {
            if (!EnableValidationLayers)
                return;
            var createInfo = new VkDebugReportCallbackCreateInfoEXT
            {
                Flags = VkDebugReportFlagsEXT.Error | VkDebugReportFlagsEXT.Warning,
                Callback = DebugCallback
            };
            callback = instance.CreateDebugReportCallbackEXT(createInfo, null).Object;
        }

        private static VkBool32 DebugCallback(
            VkDebugReportFlagsEXT flags,
            VkDebugReportObjectTypeEXT objType,
            ulong obj,
            IntPtr location,
            int code,
            IntPtr layerPrefix,
            IntPtr msg,
            IntPtr userData)
        {
            var message = Marshal.PtrToStringAnsi(msg);
            Console.WriteLine($"Validation layer: {message}");
            return VkDefines.VK_FALSE;
        }

        private void CreateSurface()
        {
            var createInfo = new VkWin32SurfaceCreateInfoKHR
            {
                Hwnd = form.Handle,
                Hinstance = Process.GetCurrentProcess().Handle
            };
            surface = instance.CreateWin32SurfaceKHR(createInfo, null).Object;
        }

        private void PickPhysicalDevice()
        {
            var devices = instance.PhysicalDevices;
            if (devices.Count == 0)
                throw new Exception("Failed to find GPUs with Vulkan support.");
            physicalDevice = devices.FirstOrDefault(x => IsDeviceSuitable(x, surface));
            if (physicalDevice == null)
                throw new Exception("Failed to find a suitable GPU.");
        }

        private static QueueFamilyIndices? FindQueueFamilies(IVkPhysicalDevice physicalDevice, IVkSurfaceKHR surface)
        {
            var graphicsFamily = physicalDevice.QueueFamilyProperties
                .Select((p, i) => new {p, i})
                .FirstOrDefault(x => x.p.QueueCount > 0 && (x.p.QueueFlags & VkQueueFlags.Graphics) != 0)
                ?.i;
            var presentFamily = physicalDevice.QueueFamilyProperties
                .Select((p, i) => new { p, i })
                .FirstOrDefault(x => x.p.QueueCount > 0 && physicalDevice.GetSurfaceSupportKHR(x.i, surface).Object)
                ?.i;

            return graphicsFamily.HasValue && presentFamily.HasValue ? new QueueFamilyIndices
            {
                GraphicsFamily = graphicsFamily.Value,
                PresentFamily = presentFamily.Value
            } : (QueueFamilyIndices?)null;
        }

        private static bool IsDeviceSuitable(IVkPhysicalDevice physicalDevice, IVkSurfaceKHR surface)
        {
            var extensionProps = physicalDevice.EnumerateDeviceExtensionProperties(null).Object;
            if (!DeviceExtensions.All(e => extensionProps.Any(p => p.ExtensionName == e)))
                return false;
            var swapChainSupportDetails = QuerySwapChainSupport(physicalDevice, surface);
            return
                swapChainSupportDetails.Formats.Any() &&
                swapChainSupportDetails.PresentModes.Any() &&
                FindQueueFamilies(physicalDevice, surface).HasValue;
        }

        private static SwapChainSupportDetails QuerySwapChainSupport(IVkPhysicalDevice physicalDevice, IVkSurfaceKHR surface)
        {
            return new SwapChainSupportDetails
            {
                Capabilities = physicalDevice.GetSurfaceCapabilitiesKHR(surface).Object,
                Formats = physicalDevice.GetSurfaceFormatsKHR(surface).Object,
                PresentModes = physicalDevice.GetSurfacePresentModesKHR(surface).Object
            };
        }

        private void CreateLogicalDevice()
        {
            var queueFamilies = FindQueueFamilies(physicalDevice, surface).Value;

            var queueCreateInfos = queueFamilies.GraphicsFamily == queueFamilies.PresentFamily
                ? new[]
                {   new VkDeviceQueueCreateInfo
                    {
                        QueueFamilyIndex = queueFamilies.GraphicsFamily,
                        QueuePriorities = new [] { 1f }
                    }
                }  
                : new [] 
                {
                    new VkDeviceQueueCreateInfo
                    {
                        QueueFamilyIndex = queueFamilies.GraphicsFamily,
                        QueuePriorities = new[] { 1f }
                    },
                    new VkDeviceQueueCreateInfo
                    {
                        QueueFamilyIndex = queueFamilies.PresentFamily,
                        QueuePriorities = new[] { 1f }
                    }
                };
            var deviceFeatures = new VkPhysicalDeviceFeatures
            {
                
            };
            var createInfo = new VkDeviceCreateInfo
            {
                QueueCreateInfos = queueCreateInfos,
                EnabledFeatures = deviceFeatures,
                EnabledExtensionNames = DeviceExtensions,
                EnabledLayerNames = LayerNames
            };
            device = physicalDevice.CreateDevice(createInfo, null).Object;
            graphicsQueue = device.GetDeviceQueue(queueFamilies.GraphicsFamily, 0);
            presentQueue = device.GetDeviceQueue(queueFamilies.PresentFamily, 0);
        }

        private void CreateSwapChain()
        {
            var swapChainSupport = QuerySwapChainSupport(physicalDevice, surface);
            var surfaceFormat = ChooseSwapSurfaceFormat(swapChainSupport.Formats);
            var presentMode = ChooseSwapPresentMode(swapChainSupport.PresentModes);
            var extent = ChooseSwapExtent(swapChainSupport.Capabilities);
            var minImages = swapChainSupport.Capabilities.MinImageCount;
            var maxImages = swapChainSupport.Capabilities.MaxImageCount > 0 
                ? swapChainSupport.Capabilities.MaxImageCount 
                : int.MaxValue;
            var imageCount = Math.Max(minImages, Math.Min(maxImages, 2));
            var indices = FindQueueFamilies(physicalDevice, surface).Value;
            var separateQueues = indices.GraphicsFamily != indices.PresentFamily;
            var createInfo = new VkSwapchainCreateInfoKHR
            {
                Surface = surface,
                MinImageCount = imageCount,
                ImageFormat = surfaceFormat.Format,
                ImageColorSpace = surfaceFormat.ColorSpace,
                ImageExtent = extent,
                ImageArrayLayers = 1,
                ImageUsage = VkImageUsageFlags.ColorAttachment,
                ImageSharingMode = separateQueues ? VkSharingMode.Concurrent : VkSharingMode.Exclusive,
                QueueFamilyIndices = separateQueues ? new[] {indices.GraphicsFamily, indices.PresentFamily} : null,
                PreTransform = swapChainSupport.Capabilities.CurrentTransform,
                CompositeAlpha = VkCompositeAlphaFlagsKHR.Opaque,
                PresentMode = presentMode,
                Clipped = true,
                OldSwapchain = null
            };
            swapChain = device.CreateSwapchainKHR(createInfo, null).Object;
            swapChainImages = swapChain.GetImagesKHR().Object;
            swapchainImageFormat = surfaceFormat.Format;
            swapChainExtent = extent;
        }

        private static VkSurfaceFormatKHR ChooseSwapSurfaceFormat(IReadOnlyList<VkSurfaceFormatKHR> availableFormats)
        {
            var preferred = new VkSurfaceFormatKHR
            {
                Format = VkFormat.B8G8R8A8_UNORM,
                ColorSpace = VkColorSpaceKHR.SRGB_NONLINEAR_KHR
            };
            if (availableFormats.Count == 1 && availableFormats[0].Format == VkFormat.Undefined)
                return preferred;
            if (availableFormats.Any(x => x.Format == preferred.Format && x.ColorSpace == preferred.ColorSpace))
                return preferred;
            return availableFormats.First();
        }

        private static VkPresentModeKHR ChooseSwapPresentMode(IReadOnlyList<VkPresentModeKHR> availableModes)
        {
            if (availableModes.Contains(VkPresentModeKHR.Mailbox))
                return VkPresentModeKHR.Mailbox;
            if (availableModes.Contains(VkPresentModeKHR.Fifo))
                return VkPresentModeKHR.Fifo;
            if (availableModes.Contains(VkPresentModeKHR.Immediate))
                return VkPresentModeKHR.Immediate;
            return availableModes.First();
        }

        private VkExtent2D ChooseSwapExtent(VkSurfaceCapabilitiesKHR capabilities)
        {
            const int special = -1;
            if (capabilities.CurrentExtent.Width != special)
                return capabilities.CurrentExtent;
            var width = Math.Max(capabilities.MinImageExtent.Width, Math.Min(capabilities.MaxImageExtent.Width, form.Width));
            var height = Math.Max(capabilities.MinImageExtent.Height, Math.Min(capabilities.MaxImageExtent.Height, form.Height));
            return new VkExtent2D(width, height);
        }

        private void CreateImageViews()
        {
            swapChainImageViews = swapChainImages.Select(x =>
            {
                var createInfo = new VkImageViewCreateInfo
                {
                    Image = x,
                    ViewType = VkImageViewType.Single2D,
                    Format = swapchainImageFormat,
                    Components = new VkComponentMapping
                    {
                        R = VkComponentSwizzle.Identity,
                        G = VkComponentSwizzle.Identity,
                        B = VkComponentSwizzle.Identity,
                        A = VkComponentSwizzle.Identity,
                    },
                    SubresourceRange = new VkImageSubresourceRange
                    {
                        AspectMask = VkImageAspectFlags.Color,
                        BaseMipLevel = 0,
                        LevelCount = 1,
                        BaseArrayLayer = 0,
                        LayerCount = 1
                    }
                };
                return device.CreateImageView(createInfo, null).Object;
            }).ToArray();
        }

        private void CreateRenderPass()
        {
            var colorAttachment = new VkAttachmentDescription
            {
                Format = swapchainImageFormat,
                Samples = VkSampleCount.B1,
                LoadOp = VkAttachmentLoadOp.Clear,
                StoreOp = VkAttachmentStoreOp.Store,
                StencilLoadOp = VkAttachmentLoadOp.DontCare,
                StencilStoreOp = VkAttachmentStoreOp.DontCare,
                InitialLayout = VkImageLayout.Undefined,
                FinalLayout = VkImageLayout.PresentSrcKHR
            };
            var colorAttachmentRef = new VkAttachmentReference
            {
                Attachment = 0,
                Layout = VkImageLayout.ColorAttachmentOptimal
            };
            var subpass = new VkSubpassDescription
            {
                PipelineBindPoint = VkPipelineBindPoint.Graphics,
                ColorAttachments = new [] {colorAttachmentRef},
                //DepthStencilAttachment = null
            };
            var dependency = new VkSubpassDependency
            {
                SrcSubpass = VkDefines.VK_SUBPASS_EXTERNAL,
                SrcStageMask = VkPipelineStageFlags.ColorAttachmentOutput,
                SrcAccessMask = VkAccessFlags.None,
                DstSubpass = 0,
                DstStageMask = VkPipelineStageFlags.ColorAttachmentOutput,
                DstAccessMask = VkAccessFlags.ColorAttachmentRead | VkAccessFlags.ColorAttachmentWrite
            };
            var renderPassInfo = new VkRenderPassCreateInfo
            {
                Attachments = new[] {colorAttachment},
                Subpasses = new[] {subpass},
                Dependencies = new[] {dependency}
            };
            renderPass = device.CreateRenderPass(renderPassInfo, null).Object;
        }

        private void CreateGraphicsPipeline()
        {
            var vertShaderModule = CreateShaderModule(File.ReadAllBytes("../../Resources/Shaders/vert.spv"));
            var fragShaderModule = CreateShaderModule(File.ReadAllBytes("../../Resources/Shaders/frag.spv"));

            var vertShaderStageInfo = new VkPipelineShaderStageCreateInfo()
            {
                Stage = VkShaderStage.Vertex,
                Module = vertShaderModule,
                Name = "main"
            };

            var fragShaderStageInfo = new VkPipelineShaderStageCreateInfo()
            {
                Stage = VkShaderStage.Fragment,
                Module = fragShaderModule,
                Name = "main"
            };

            var shaderStages = new[] {vertShaderStageInfo, fragShaderStageInfo};
            var vertexInputInfo = new VkPipelineVertexInputStateCreateInfo
            {
                VertexBindingDescriptions = new[] {Vertex.GetBindingDescription(0)},
                VertexAttributeDescriptions = Vertex.GetAttributeDescriptions(0)
            };
            var inputAssemblyInfo = new VkPipelineInputAssemblyStateCreateInfo
            {
                Topology = VkPrimitiveTopology.TriangleList,
                PrimitiveRestartEnable = false
            };
            var viewport = new VkViewport
            {
                X = 0,
                Y = 0,
                Width = swapChainExtent.Width,
                Height = swapChainExtent.Height,
                MinDepth = 0,
                MaxDepth = 1
            };
            var scissor = new VkRect2D(new VkOffset2D(0, 0), swapChainExtent);
            var viewportStateInfo = new VkPipelineViewportStateCreateInfo
            {
                Viewports = new [] {viewport},
                Scissors = new [] {scissor}
            };
            var rasterizerInfo = new VkPipelineRasterizationStateCreateInfo
            {
                DepthClampEnable = false,
                RasterizerDiscardEnable = false,
                PolygonMode = VkPolygonMode.Fill,
                LineWidth = 1f,
                CullMode = VkCullMode.Back,
                FrontFace = VkFrontFace.Clockwise,
                DepthBiasEnable = false,
                DepthBiasConstantFactor = 0f,
                DepthBiasClamp = 0f,
                DepthBiasSlopeFactor = 0f
            };
            var multisamplingInfo = new VkPipelineMultisampleStateCreateInfo
            {
                SampleShadingEnable = false,
                RasterizationSamples = VkSampleCount.B1,
                MinSampleShading = 1f,
                SampleMask = null,
                AlphaToCoverageEnable = false,
                AlphaToOneEnable = false
            };
            var colorBlendAttachment = new VkPipelineColorBlendAttachmentState
            {
                ColorWriteMask = VkColorComponent.R | VkColorComponent.G | VkColorComponent.B | VkColorComponent.A,
                BlendEnable = (VkBool32)false,
                SrcColorBlendFactor = VkBlendFactor.One,
                SrcAlphaBlendFactor = VkBlendFactor.One,
                ColorBlendOp = VkBlendOp.Add,
                AlphaBlendOp = VkBlendOp.Add,
                DstColorBlendFactor = VkBlendFactor.Zero,
                DstAlphaBlendFactor = VkBlendFactor.Zero
            };
            var colorBlendingInfo = new VkPipelineColorBlendStateCreateInfo
            {
                LogicOpEnable = false,
                LogicOp = VkLogicOp.Copy,
                Attachments = new[] {colorBlendAttachment},
                BlendConstants = new VkColor4(0, 0, 0, 0)
            };
            var dynamicStates = new[]
            {
                VkDynamicState.Viewport,
                VkDynamicState.LineWidth,
            };
            var dynamicStateInfo = new VkPipelineDynamicStateCreateInfo
            {
                DynamicStates = dynamicStates
            };
            var pipelineLayoutInfo = new VkPipelineLayoutCreateInfo
            {
                SetLayouts = null,
                PushConstantRanges = null
            };
            pipelineLayout = device.CreatePipelineLayout(pipelineLayoutInfo, null).Object;

            var pipelineInfo = new VkGraphicsPipelineCreateInfo
            {
                Stages = shaderStages,
                VertexInputState = vertexInputInfo,
                InputAssemblyState = inputAssemblyInfo,
                TessellationState = null,
                ViewportState = viewportStateInfo,
                RasterizationState = rasterizerInfo,
                MultisampleState = multisamplingInfo,
                DepthStencilState = null,
                ColorBlendState = colorBlendingInfo,
                DynamicState = null,
                Layout = pipelineLayout,
                RenderPass = renderPass,
                Subpass = 0,
                BasePipelineHandle = null,
                BasePipelineIndex = -1,
                Flags = VkPipelineCreateFlags.None
            };

            graphicsPipeline = device.CreateGraphicsPipelines(null, new[] {pipelineInfo}, null).Object.Single();

            vertShaderModule.Dispose();
            fragShaderModule.Dispose();
        }

        private unsafe IVkShaderModule CreateShaderModule(byte[] code)
        {
            fixed (byte* pCode = code)
            {
                var createinfo = new VkShaderModuleCreateInfo
                {
                    Code = (IntPtr)pCode,
                    CodeSize = (IntPtr)code.Length
                };
                return device.CreateShaderModule(createinfo, null).Object;
            }
        }

        private void CreateFramebuffers()
        {
            swapChainFramebuffers = swapChainImageViews.Select((v, i) =>
            {
                var createInfo = new VkFramebufferCreateInfo
                {
                    RenderPass = renderPass,
                    Attachments = new[] {v},
                    Width = swapChainExtent.Width,
                    Height = swapChainExtent.Height,
                    Layers = 1
                };
                return device.CreateFramebuffer(createInfo, null).Object;
            }).ToArray();
        }

        private void CreateCommandPool()
        {
            var queueFamilyIndices = FindQueueFamilies(physicalDevice, surface).Value;
            var poolInfo = new VkCommandPoolCreateInfo
            {
                QueueFamilyIndex = queueFamilyIndices.GraphicsFamily,
                Flags = VkCommandPoolCreateFlags.None
            };
            commandPool = device.CreateCommandPool(poolInfo, null).Object;
        }

        private void CreateCommandBuffers()
        {
            var allocInfo = new VkCommandBufferAllocateInfo
            {
                CommandPool = commandPool,
                Level = VkCommandBufferLevel.Primary,
                CommandBufferCount = swapChainImages.Count
            };
            commandBuffers = device.AllocateCommandBuffers(allocInfo).Object;

            for (int i = 0; i < commandBuffers.Count; i++)
            {
                var buffer = commandBuffers[i];
                var framebuffer = swapChainFramebuffers[i];

                var beginInfo = new VkCommandBufferBeginInfo
                {
                    Flags = VkCommandBufferUsageFlags.SimultaneousUse,
                    InheritanceInfo = null
                };
                buffer.Begin(beginInfo);
                var renderPassInfo = new VkRenderPassBeginInfo
                {
                    RenderPass = renderPass,
                    Framebuffer = framebuffer,
                    RenderArea = new VkRect2D(new VkOffset2D(0, 0), swapChainExtent),
                    ClearValues = new[]
                    {
                        new VkClearValue(new VkClearColorValue(new VkColor4(0, 0, 0, 1)))
                    }
                };
                buffer.CmdBeginRenderPass(renderPassInfo, VkSubpassContents.Inline);
                buffer.CmdBindPipeline(VkPipelineBindPoint.Graphics, graphicsPipeline);
                buffer.CmdBindVertexBuffers(0, new []{vertexBuffer}, new ulong[]{0});
                buffer.CmdBindIndexBuffer(indexBuffer, 0, VkIndexType.Uint16);
                buffer.CmdDrawIndexed(Indices.Length, 1, 0, 0, 0);
                buffer.CmdEndRenderPass();
                buffer.End().CheckSuccess();
            }
        }

        private void CreateSemaphores()
        {
            var semaphoreInfo = new VkSemaphoreCreateInfo();
            imageAvailableSemaphore = device.CreateSemaphore(semaphoreInfo, null).Object;
            renderFinishedSemaphore = device.CreateSemaphore(semaphoreInfo, null).Object;
        }

        private void CreateBuffer(ulong size, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, out IVkBuffer buffer, out IVkDeviceMemory bufferMemory)
        {
            buffer = device.CreateBuffer(new VkBufferCreateInfo
            {
                Size = size,
                Usage = usage,
                SharingMode = VkSharingMode.Exclusive
            }, null).Object;

            var memRequirements = buffer.GetMemoryRequirements();
            bufferMemory = device.AllocateMemory(new VkMemoryAllocateInfo
            {
                AllocationSize = memRequirements.Size,
                MemoryTypeIndex = FindMemoryType(memRequirements.MemoryTypeBits, properties)
            }, null).Object;

            buffer.BindMemory(bufferMemory, 0).CheckSuccess();
        }

        private int FindMemoryType(int typeFilter, VkMemoryPropertyFlags properties)
        {
            return physicalDevice.MemoryProperties.MemoryTypes
                .Select((p, i) => new { p, i })
                .First(x => (typeFilter & (1 << x.i)) != 0 && (x.p.PropertyFlags & properties) == properties).i;
        }

        private void CopyBuffer(IVkBuffer srcBuffer, IVkBuffer dstBuffer, ulong size)
        {
            var buffer = device.AllocateCommandBuffers(new VkCommandBufferAllocateInfo
            {
                Level = VkCommandBufferLevel.Primary,
                CommandPool = commandPool,
                CommandBufferCount = 1
            }).Object.Single();

            buffer.Begin(new VkCommandBufferBeginInfo
            {
                Flags = VkCommandBufferUsageFlags.OneTimeSubmit
            });

            buffer.CmdCopyBuffer(srcBuffer, dstBuffer, new []{new VkBufferCopy
            {
                SrcOffset = 0,
                DstOffset = 0,
                Size = size
            }});

            buffer.End();

            graphicsQueue.Submit(new[]{new VkSubmitInfo
            {
                CommandBuffers = new[]{buffer}
            }}, null);

            graphicsQueue.WaitIdle();
            commandPool.FreeCommandBuffers(new []{buffer});
        }

        private unsafe void CreateVertexBuffer()
        {
            var size = (ulong)(sizeof(Vertex) * Vertices.Length);
            CreateBuffer(size, VkBufferUsageFlags.TransferSrc, VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent, 
                out var stagingBuffer, out var stagingBufferMemory);
            var data = (Vertex*)stagingBufferMemory.Map(0, size, VkMemoryMapFlags.None).Object;
            for (int i = 0; i < Vertices.Length; i++)
                data[i] = Vertices[i];
            stagingBufferMemory.Unmap();
            CreateBuffer(size, VkBufferUsageFlags.TransferDst | VkBufferUsageFlags.VertexBuffer, VkMemoryPropertyFlags.DeviceLocal,
                out vertexBuffer, out vertexBufferMemory);
            CopyBuffer(stagingBuffer, vertexBuffer, size);
            stagingBuffer.Dispose();
            stagingBufferMemory.Dispose();
        }

        private unsafe void CreateIndexBuffer()
        {
            var size = (ulong)(sizeof(ushort) * Indices.Length);
            CreateBuffer(size, VkBufferUsageFlags.TransferSrc, VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent,
                out var stagingBuffer, out var stagingBufferMemory);
            var data = (ushort*)stagingBufferMemory.Map(0, size, VkMemoryMapFlags.None).Object;
            for (int i = 0; i < Indices.Length; i++)
                data[i] = Indices[i];
            stagingBufferMemory.Unmap();
            CreateBuffer(size, VkBufferUsageFlags.TransferDst | VkBufferUsageFlags.IndexBuffer, VkMemoryPropertyFlags.DeviceLocal,
                out indexBuffer, out indexBufferMemory);
            CopyBuffer(stagingBuffer, indexBuffer, size);
            stagingBuffer.Dispose();
            stagingBufferMemory.Dispose();
        }

        public void DrawFrame()
        {
            var imageIndex =  swapChain.AcquireNextImageKHR(ulong.MaxValue, imageAvailableSemaphore, null).Object;
            var submitInfo = new VkSubmitInfo
            {
                WaitSemaphores = new[] {imageAvailableSemaphore},
                WaitDstStageMask = new[] {VkPipelineStageFlags.ColorAttachmentOutput},
                CommandBuffers = new[] {commandBuffers[imageIndex]},
                SignalSemaphores = new[] {renderFinishedSemaphore}
            };
            graphicsQueue.Submit(new [] {submitInfo}, null).CheckSuccess();
            var presentInfo = new VkPresentInfoKHR
            {
                WaitSemaphores = new[] {renderFinishedSemaphore},
                Swapchains = new[] {swapChain},
                ImageIndices = new[] {imageIndex},
                Results = null
            };
            presentQueue.PresentKHR(presentInfo).CheckSuccess();
        }
    }
}