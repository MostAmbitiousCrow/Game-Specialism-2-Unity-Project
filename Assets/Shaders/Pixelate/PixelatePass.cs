using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelatePass : ScriptableRenderPass
{
    private PixelateFeature.CustomPassSettings settings;

    private RenderTargetIdentifier colorBuffer, pixelBuffer;
    private int pixelBufferID = Shader.PropertyToID("_PixelBuffer");


    private Material material;
    private int pixelScreenHeight, pixelScreenWidth;

    public PixelatePass(PixelateFeature.CustomPassSettings settings)
    {
        this.settings = settings;
        this.renderPassEvent = settings.renderPassEvent;
        if (material == null) material = CoreUtils.CreateEngineMaterial("Hidden/Pixelize");
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        throw new System.NotImplementedException();
    }
}

// I have absolutely no idea where these go. The tutorial I followed was unspecific.

// public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
// {
//     colorBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle; // cameraColorTarget;
//     RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;


//     pixelScreenHeight = settings.screenHeight;
//     pixelScreenWidth = (int)(pixelScreenHeight * renderingData.cameraData.camera.aspect + 0.5f);

//     material.SetVector("_BlockCount", new Vector2(pixelScreenWidth, pixelScreenHeight));
//     material.SetVector("_BlockSize", new Vector2(1.0f / pixelScreenWidth, 1.0f / pixelScreenHeight));
//     material.SetVector("_HalfBlockSize", new Vector2(0.5f / pixelScreenWidth, 0.5f / pixelScreenHeight));

//     descriptor.height = pixelScreenHeight;
//     descriptor.width = pixelScreenWidth;

//     cmd.GetTemporaryRT(pixelBufferID, descriptor, FilterMode.Point);
//     pixelBuffer = new RenderTargetIdentifier(pixelBufferID);
// }

// public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
// {

// }