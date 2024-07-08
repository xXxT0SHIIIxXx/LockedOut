using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public WobblyText wobbleText;
    Button bttn;
    GameSystem gameSys;
    private void Start()
    {
        bttn = this.GetComponent<Button>();
        wobbleText.speed = 0;
        //Change this eventually
        gameSys = FindAnyObjectByType<GameSystem>().GetComponent<GameSystem>();
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
        //Needs to specifically only Unpause Game
        gameSys.PauseBttnEvent();
        Debug.Log("Resume Clicked!");
    }

    public void Options()
    {
        //Needs to open Options menu
        Debug.Log("Options Clicked!");
    }

    public void EndGame()
    {
        Application.Quit();
        Debug.Log("Quit Clicked!");
    }
}
