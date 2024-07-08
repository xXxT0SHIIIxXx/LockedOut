using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PauseUI : MonoBehaviour
{
    bool opened;
    [SerializeField] RectTransform pauseMenu; //pos.x range is (-490,490) for lerp

    float elapsedTime;
    [SerializeField] float duration;
    // Start is called before the first frame update
    void Start()
    {
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

    public void isPaused(Component sender, object data )
    {
        if(data is PauseData)
        {
            PauseData result = (PauseData)data;

            opened = result.pauseState;
            elapsedTime = 0;
        }
    }
}
