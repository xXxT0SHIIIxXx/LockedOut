using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    bool paused;
    bool opened;
    [SerializeField] RectTransform pauseMenu; //pos.x range is (-490,490) for lerp

    float elapsedTime;
    [SerializeField] float duration;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.OnEscPress += isPaused;
    }

    // Update is called once per frame
    void Update()
    {
        if (opened)
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime < duration)
            {
                var value = Mathf.Lerp(pauseMenu.anchoredPosition.x, 490,elapsedTime/duration);

                pauseMenu.anchoredPosition = new Vector2(value, 0);
            }
        }
        else if (!opened)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < duration)
            {
                var value = Mathf.Lerp(pauseMenu.anchoredPosition.x, -490, elapsedTime / duration);

                pauseMenu.anchoredPosition = new Vector2(value, 0);
            }
        }
    }

    public void isPaused(bool isPaused)
    {
        paused = isPaused;

        if(paused)
        {
            opened = true;
            elapsedTime = 0;
        }
        else if(!paused)
        {
            opened = false;
            elapsedTime = 0;
        }
    }
}
