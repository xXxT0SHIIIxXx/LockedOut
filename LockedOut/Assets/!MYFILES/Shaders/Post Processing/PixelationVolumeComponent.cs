using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable, VolumeComponentMenu("Custom/Pixelization")]
public class PixelizationVolumeComponent : VolumeComponent, IPostProcessComponent
{
    public FloatParameter resolution = new FloatParameter(512);
    public FloatParameter pixelWidth = new FloatParameter(64);
    public FloatParameter pixelHeight = new FloatParameter(64);

    public bool IsActive() => true;

    public bool IsTileCompatible() => false;
}