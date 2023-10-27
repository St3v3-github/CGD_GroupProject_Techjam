using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColourBlitRendererFeature : ScriptableRendererFeature
{
    public Shader b_Shader;
    public float b_Intensity;

    Material blood_Material;

    ColorBlitPass b_RenderPass = null;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if(renderingData.cameraData.cameraType == CameraType.Game)
        {
            renderer.EnqueuePass(b_RenderPass);
        }
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            b_RenderPass.ConfigureInput(ScriptableRenderPassInput.Color);
            b_RenderPass.SetTarget(renderer.cameraDepthTargetHandle, b_Intensity);
        }
    }

    public override void Create()
    {
        blood_Material = CoreUtils.CreateEngineMaterial(b_Shader);
        b_RenderPass = new ColorBlitPass(blood_Material);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(blood_Material);
    }
}
