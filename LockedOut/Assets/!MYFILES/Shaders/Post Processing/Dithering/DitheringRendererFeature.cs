using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DitheringRendererFeature : ScriptableRendererFeature
{
    class DitheringRenderPass : ScriptableRenderPass
    {
        public Material material;
        private RenderTargetIdentifier source;
        private RenderTargetHandle temporaryTexture;

        private DitheringVolumeComponent volumeComponent;

        public DitheringRenderPass(Material material)
        {
            this.material = material;
            temporaryTexture.Init("_TemporaryColorTexture");
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(temporaryTexture.id, cameraTextureDescriptor, FilterMode.Point);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!renderingData.cameraData.postProcessEnabled) return;

            var stack = VolumeManager.instance.stack;
            volumeComponent = stack.GetComponent<DitheringVolumeComponent>();
            if (volumeComponent == null || !volumeComponent.IsActive()) return;

            CommandBuffer cmd = CommandBufferPool.Get("Dither Pass");
            source = renderingData.cameraData.renderer.cameraColorTarget;
            // Set the radius property on the material
            material.SetFloat("_Spread", volumeComponent.spread.value);
            material.SetInt("_BayerLevel", volumeComponent.bayerLevel.value);
            material.SetInt("_RedColorCount", volumeComponent.redColorCount.value);
            material.SetInt("_GreenColorCount", volumeComponent.greenColorCount.value);
            material.SetInt("_BlueColorCount", volumeComponent.blueColorCount.value);

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
    public class DitheringSettings
    {
        public Material material = null;
    }

    public DitheringSettings settings = new DitheringSettings();

    DitheringRenderPass renderPass;

    public override void Create()
    {
        renderPass = new DitheringRenderPass(settings.material)
        {
            renderPassEvent = RenderPassEvent.AfterRendering
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(renderPass);
    }
}