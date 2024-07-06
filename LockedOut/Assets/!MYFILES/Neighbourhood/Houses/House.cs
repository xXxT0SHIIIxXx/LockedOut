using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    [Header("UI Elements")]
    public Sprite portraitImg;

    [Header("Data Vars")]
    public bool locked;
    public string[] voiceLines;
    public int minigameIndex;

    [SerializeField] float duration = 5;
    float elapsedTime;
    bool started;
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
                elapsedTime = 0;
                started = false;
            }
            
        }
    }

    void WaitUntilSound(bool KnockRing)
    {
        started = true;
    }

}
