using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/SunDeform")]
public sealed class SunDeform : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [Tooltip("Controls the intensity of the effect.")]
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);
    
    [Tooltip("Controls the distance of the effect.")]
    public ClampedFloatParameter distance = new ClampedFloatParameter(50f, 0f, 500f);
    
    [Tooltip("Controls the pow of the effect.")]
    public ClampedFloatParameter power = new ClampedFloatParameter(1f, 0f, 5f);
    
    [Tooltip("Controls the pow of the effect.")]
    public ClampedFloatParameter deformPower = new ClampedFloatParameter(1f, 1f, 20f);
    
    Material m_Material;
    public bool IsActive() => m_Material != null && intensity.value > 0f;

    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > HDRP Default Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    const string kShaderName = "Hidden/Shader/SunDeform";
    
    public override void Setup()
    {
        if (Shader.Find(kShaderName) != null)
            m_Material = new Material(Shader.Find(kShaderName));
        else
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume SunDeform is unable to load.");
    }
    
    
    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        
        if (m_Material == null)
            return;
        
       
        m_Material.SetTexture("_InputTexture", source);
        m_Material.SetFloat("_Intensity", intensity.value);
        m_Material.SetFloat("_Distance",distance.value);
        m_Material.SetFloat("_Pow",power.value);
        m_Material.SetFloat("_DeformPow",deformPower.value);
        m_Material.SetMatrix("unity_ViewToWorldMatrix",  camera.camera.cameraToWorldMatrix);
        m_Material.SetMatrix("unity_InverseProjectionMatrix", GL.GetGPUProjectionMatrix(camera.camera.projectionMatrix, false).inverse);
        HDUtils.DrawFullScreen(cmd, m_Material, destination);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
    }
}
