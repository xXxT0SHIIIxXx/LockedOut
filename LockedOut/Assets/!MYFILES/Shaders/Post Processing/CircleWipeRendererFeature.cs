using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CircleWipeRendererFeature : ScriptableRendererFeature
{
    class CircleWipeRenderPass : ScriptableRenderPass
    {
        public Material material;
        private RenderTargetIdentifier source;
        private RenderTargetHandle temporaryTexture;

        private CircleWipeVolumeComponent volumeComponent;

        public CircleWipeRenderPass(Material material)
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
            volumeComponent = stack.GetComponent<CircleWipeVolumeComponent>();
            if (volumeComponent == null || !volumeComponent.IsActive()) return;

            CommandBuffer cmd = CommandBufferPool.Get("Circle Radius Pass");
            source = renderingData.cameraData.renderer.cameraColorTarget;
            // Set the radius property on the material
            material.SetFloat("_Radius", volumeComponent.radius.value);

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
    public class CircleRadiusSettings
    {
        public Material material = null;
    }

    public CircleRadiusSettings settings = new CircleRadiusSettings();

    CircleWipeRenderPass renderPass;

    public override void Create()
    {
        renderPass = new CircleWipeRenderPass(settings.material)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(renderPass);
    }
}