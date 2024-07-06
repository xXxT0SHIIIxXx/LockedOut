using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public WobblyText wobbleText;
    Button bttn;
    private void Start()
    {
        EventSystem.OnEscPress += ToggleButton;
        bttn = this.GetComponent<Button>();
        wobbleText.speed = 0;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        wobbleText.speed = 10;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        wobbleText.speed = 0;
    }

    void ToggleButton(bool disabled)
    {
        if(!disabled)
        {
            bttn.interactable = false;
        }
        else if(disabled)
        {
            bttn.interactable = true;
        }
    }

    public void Resume()
    {
        EventSystem.OnPause(false);
        Debug.Log("Resume Clicked!");
    }

    public void Options()
    {
        Debug.Log("Options Clicked!");
    }

    public void EndGame()
    {
        Application.Quit();
        Debug.Log("Quit Clicked!");
    }
}
