using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelizationRendererFeature : ScriptableRendererFeature
{
    class PixelizationRenderPass : ScriptableRenderPass
    {
        public Material material;
        private RenderTargetIdentifier source;
        private RenderTargetHandle temporaryTexture;

        private PixelizationVolumeComponent volumeComponent;

        public PixelizationRenderPass(Material material)
        {
            this.material = material;
            temporaryTexture.Init("_TemporaryColorTexture");
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(temporaryTexture.id, cameraTextureDescriptor);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!renderingData.cameraData.postProcessEnabled) return;

            var stack = VolumeManager.instance.stack;
            volumeComponent = stack.GetComponent<PixelizationVolumeComponent>();
            if (volumeComponent == null || !volumeComponent.IsActive()) return;

            CommandBuffer cmd = CommandBufferPool.Get("Pixelization Pass");
            source = renderingData.cameraData.renderer.cameraColorTarget;
            // Set the radius property on the material
            material.SetFloat("_Pixels", volumeComponent.resolution.value);
            material.SetFloat("_Pw", volumeComponent.pixelWidth.value);
            material.SetFloat("_Ph", volumeComponent.pixelHeight.value);

            // Copy the camera color target to a temporary render texture
            Blit(cmd, source, temporaryTexture.Identifier());

            // Apply the material and copy the result back to the camera color target
            Blit(cmd, temporaryTexture.Identifier(), source, material);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (cmd == null) throw new System.ArgumentNullException("cmd");
            cmd.ReleaseTemporaryRT(temporaryTexture.id);
        }
    }

    [System.Serializable]
    public class PixelizationSettings
    {
        public Material material = null;
    }

    public PixelizationSettings settings = new PixelizationSettings();

    PixelizationRenderPass renderPass;

    public override void Create()
    {
        renderPass = new PixelizationRenderPass(settings.material)
        {
            renderPassEvent = RenderPassEvent.AfterRendering
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(renderPass);
    }
}