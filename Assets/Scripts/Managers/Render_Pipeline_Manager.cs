using UnityEngine;
using UnityEngine.Rendering;

public class Render_Pipeline_Manager : MonoBehaviour // By Samuel White
{
    //========================================
    // Used to swap the render pipeline settings to pixelate since transparancy doesn't work :(. (untested)
    //========================================

    public static Render_Pipeline_Manager instance;

    [Header("Render Pipeline Settings")]
    [SerializeField] RenderPipelineAsset defaultRenderPipelineAsset; // The default render pipeline asset
    [SerializeField] RenderPipelineAsset pixelateRenderPipelineAsset; // The render pipeline asset used for pixelation

    private void Awake()
    {
        instance = this;
    }

    public static void SetPixelate(bool enable)
    {
        if (enable)
        {
            GraphicsSettings.renderPipelineAsset = instance.pixelateRenderPipelineAsset;
        }
        else
        {
            GraphicsSettings.renderPipelineAsset = instance.defaultRenderPipelineAsset;
        }
    }
}
