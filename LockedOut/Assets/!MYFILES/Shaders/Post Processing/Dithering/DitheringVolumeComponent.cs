using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable, VolumeComponentMenu("Custom/Dithering")]
public class DitheringVolumeComponent : VolumeComponent, IPostProcessComponent
{
    public FloatParameter spread = new FloatParameter(1f);
    public IntParameter bayerLevel = new IntParameter(1);
    public IntParameter redColorCount = new IntParameter(1);
    public IntParameter greenColorCount = new IntParameter(1);
    public IntParameter blueColorCount = new IntParameter(1);

    public bool IsActive() => true;

    public bool IsTileCompatible() => false;
}