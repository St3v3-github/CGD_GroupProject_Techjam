using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorBlitPass : ScriptableRenderPass
{
    ProfilingSampler blood_ProfilingSampler = new ProfilingSampler("ColorBlit");
    Material blood_Material;
    RTHandle b_CameraColorTarget;
    float b_Intensity;

    public ColorBlitPass(Material material)
    {
        blood_Material = material;
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public void SetTarget(RTHandle colorHandle, float Intensity)
    {
        b_CameraColorTarget = colorHandle;
        b_Intensity = Intensity;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        ConfigureTarget(b_CameraColorTarget);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var cameraData = renderingData.cameraData;
        if(cameraData.camera.cameraType != CameraType.Game)
        {
            return;
        }
        if(blood_Material == null)
        {
            return;
        }
        CommandBuffer cmd = CommandBufferPool.Get();
        using(new ProfilingScope(cmd, blood_ProfilingSampler))
        {
            blood_Material.SetFloat("_Intensity", b_Intensity);
            Blitter.BlitCameraTexture(cmd, b_CameraColorTarget, b_CameraColorTarget, blood_Material, 0);
        }
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }
}
