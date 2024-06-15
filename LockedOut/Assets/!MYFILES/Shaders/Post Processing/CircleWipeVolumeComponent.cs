using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable, VolumeComponentMenu("Custom/Circle Wipe")]
public class CircleWipeVolumeComponent : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter radius = new ClampedFloatParameter(0.5f, 0f, 1f);

    public bool IsActive() => true;

    public bool IsTileCompatible() => false;
}