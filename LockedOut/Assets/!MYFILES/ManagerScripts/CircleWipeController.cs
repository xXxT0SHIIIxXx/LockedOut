using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CircleWipeController : MonoBehaviour
{
    public Volume manager;
    CircleWipeVolumeComponent wipeComponent;
    public AudioSource source;
    public AudioClip fadeIn;
    public AudioClip fadeOut;

    bool closeOpen;

    float elapsedTime;
    public float duration;
    //close == true Open == false
    // Start is called before the first frame update
    void Start()
    {
        manager.profile.TryGet<CircleWipeVolumeComponent>(out CircleWipeVolumeComponent circ);
        wipeComponent = circ;
        wipeComponent.radius.Override(0f);
    }

    private void Update()
    {
        if (closeOpen && elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / duration;
            float val = Mathf.Lerp(1,0,percentageComplete);
            wipeComponent.radius.Override(val);
            
        }
        else if (!closeOpen && elapsedTime<=duration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / duration;
            float val = Mathf.Lerp(0, 1, percentageComplete);
            wipeComponent.radius.Override(val);
        }
    }


    [ContextMenu("Close Circle")]
    void CloseCircle()
    {
        elapsedTime = 0;
        closeOpen = true;
        source.PlayOneShot(fadeOut);
    }

    [ContextMenu("Open Circle")]
    void OpenCircle()
    {
        elapsedTime = 0;
        closeOpen = false;
        source.PlayOneShot(fadeIn);
    }
}
