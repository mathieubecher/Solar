
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PostProcessEffect : MonoBehaviour
{
    private VolumeProfile _profile;
    [Header("Bloom")] 
    public float die_threshold = 0.5f;
    public float die_intensity = 0.75f;
    [Header("Exposure")] 
    public float die_exposure = 8f;
    
    // Bloom
    private Bloom _bloom;
    private float threshold;
    private float intensity;
    // Exposure
    private Exposure _exposure;
    private float exposure;

    void Start()
    {
        _profile = GetComponent<Volume>().profile;
        for (int i = 0; i < _profile.components.Count; ++i)
        {
            Debug.Log(_profile.components[i].name);
            if (_profile.components[i].name == "Bloom(Clone)") _bloom = (Bloom) _profile.components[i];
            if (_profile.components[i].name == "Exposure(Clone)") _exposure = (Exposure) _profile.components[i];
        }

        threshold = _bloom.threshold.value;
        intensity = _bloom.intensity.value;

        exposure = _exposure.fixedExposure.value;
        
    }

    void Update()
    {
    }

    public void Interpolate(float value)
    {
        _bloom.threshold.value = Lerp(threshold, die_threshold, value);
        _bloom.intensity.value = Lerp(intensity, die_intensity, value);
        _exposure.fixedExposure.value = Lerp(exposure, die_exposure, value);
    }

    private float Lerp(float a, float b, float value)
    {
        return (b - a) * value + a;
    }
}
