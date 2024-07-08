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

    [Header("Events")]
    public GameEvent OnLoadMinigame;

    int nextLevelID;
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
            if(val <= 0)
            {
                MinigameData data = new MinigameData();
                data.LevelID = nextLevelID;
                OnLoadMinigame.Raise(this, data);
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


    public void CircleControl(Component sender, object data)
    {
        if(data is FadeData)
        {
            FadeData result = (FadeData)data;

            if(result.isOpen)
            {
                OpenCircle();
            }
            else if(!result.isOpen)
            {
                nextLevelID = result.nextLevelID;
                CloseCircle();
            }
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
