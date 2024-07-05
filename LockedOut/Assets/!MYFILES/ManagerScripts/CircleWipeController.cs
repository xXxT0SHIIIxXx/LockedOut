using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CircleWipeController : MonoBehaviour
{
    public Volume manager;
    public CircleWipeVolumeComponent wipeComponent;
    public AudioSource source;
    public AudioClip fadeIn;
    public AudioClip fadeOut;

    int levelIndex;

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
        EventSystem.OnCircleClose += CloseCircle;
    }

    private void Update()
    {
        if (closeOpen && elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / duration;
            float val = Mathf.Lerp(1,0,percentageComplete);
            wipeComponent.radius.Override(val);
            if(val <= 0)
            {
                EventSystem.OnLoadNextLevel(true, levelIndex);
            }
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
    void CloseCircle(bool openClose, int level)
    {
        levelIndex = level;
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
