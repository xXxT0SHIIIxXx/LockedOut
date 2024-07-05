using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    public Sprite portraitImg;

    public string[] voiceLines;
    public int minigameIndex;
    float elapsedTime;
    [SerializeField] float duration = 5;
    bool started;
    public bool locked;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.OnDoorSound += WaitUntilSound;
    }

    private void Update()
    {
        if (started)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime > duration)
            {
                EventSystem.OnDialogueStart(this);
                locked = true;
            }
            
        }
    }

    void WaitUntilSound(bool KnockRing)
    {
        started = true;
    }

}
