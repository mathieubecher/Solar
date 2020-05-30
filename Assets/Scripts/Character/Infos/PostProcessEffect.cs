
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PostProcessEffect : MonoBehaviour
{
    [Serializable]
    struct BloomEffect
    {
        public float threshold;
        public float intensity;
    }

    [Serializable]
    struct ExposureEffect
    {
        public float exposure;
    }
    
    private VolumeProfile _profile;
    
    private Bloom _bloom;
    [Header("Bloom")]
    [SerializeField] private BloomEffect _bloomEffect;
    private BloomEffect _originBloom;
    
    private Exposure _exposure;
    [Header("Exposure")] 
    [SerializeField] private ExposureEffect _exposureEffect;
    private ExposureEffect _originExposure;
    // Exposure
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

        _originBloom.threshold = _bloom.threshold.value;
        _originBloom.intensity = _bloom.intensity.value;

        _originExposure.exposure = _exposure.fixedExposure.value;
        
    }

    void Update()
    {
    }

    public void Interpolate(float value)
    {
        _bloom.threshold.value = Lerp(_originBloom.threshold, _bloomEffect.threshold, value);
        _bloom.intensity.value = Lerp(_originBloom.intensity, _bloomEffect.intensity, value);
        _exposure.fixedExposure.value = Lerp(_originExposure.exposure, _exposureEffect.exposure, value);
    }

    private float Lerp(float a, float b, float value)
    {
        return (b - a) * value + a;
    }
}
