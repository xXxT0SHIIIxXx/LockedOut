using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    bool paused;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.OnEscPress += isPaused;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void isPaused(bool isPaused)
    {
        paused = isPaused;

        if(paused)
        {
            pauseMenu.SetActive(true);
        }
        else if(!paused)
        {
            pauseMenu.SetActive(false);
        }
    }
}
