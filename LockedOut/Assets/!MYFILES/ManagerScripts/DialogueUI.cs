using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI Element References")]
    public RectTransform dialogueUI;
    public GameObject nextUI;
    public GameObject acceptDeclineUI;
    public TMP_Text dialogueTXT;
    public Image portraitIMG;

    public House curHouse;
    [SerializeField] int index;
    public ThirdPersonMovement player;
    [Header("Utility Vars")]
    [SerializeField] bool opened;
    [SerializeField] float elapsedTime;
    [SerializeField] float duration;

    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.OnDialogueBegin += DialogueStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (opened)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < duration)
            {
                var value = Mathf.Lerp(dialogueUI.anchoredPosition.y, 280, elapsedTime / duration);

                dialogueUI.anchoredPosition = new Vector2(0, value);
            }

            if(Input.GetKeyDown(KeyCode.Tab) && nextUI.activeSelf == true)
            {
                NextVoiceLine();
            }

            if(acceptDeclineUI.activeSelf == true)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    //Start next Scene
                    EventSystem.CircleClose(false,curHouse.minigameIndex);
                    canvas.enabled = false;
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    DialogueEnd();
                    player.CanMove();
                }
            }
        }
        else if (!opened)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < duration)
            {
                var value = Mathf.Lerp(dialogueUI.anchoredPosition.y, -285, elapsedTime / duration);

                dialogueUI.anchoredPosition = new Vector2(0, value);
            }
        }
    }

    [ContextMenu("Dialogue Start")]
    public void DialogueStart(House house)
    {
        curHouse = house;
        //nextUI.SetActive(true);
        //acceptDeclineUI.SetActive(false);
        portraitIMG.sprite = curHouse.portraitImg;
        dialogueTXT.text = curHouse.voiceLines[index];
        elapsedTime = 0;
        opened = true;
    }

    public void DialogueEnd()
    {
        opened = false;
        elapsedTime = 0;
    }

    void NextVoiceLine()
    {
        index++;

        if(index >= curHouse.voiceLines.Length-1)
        {
            nextUI.SetActive(false);
            acceptDeclineUI.SetActive(true);
        }

        dialogueTXT.text = curHouse.voiceLines[index];
    }
}

