using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI Element References")]
    [SerializeField] RectTransform dialogueUI;
    [SerializeField] GameObject nextUI;
    [SerializeField] GameObject acceptDeclineUI;
    [SerializeField] TMP_Text dialogueTXT;
    [SerializeField] Image portraitIMG;

    [SerializeField] House curHouse;
    [SerializeField] int index;
    [SerializeField] ThirdPersonMovement player;
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
            MoveDialogueUI(280);
            DialogueControls();
        }
        else if (!opened)
        {
            MoveDialogueUI(-285);
        }
    }

    void MoveDialogueUI(float yPos)
    {
        if (dialogueUI.anchoredPosition.y == yPos) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime < duration)
        {
            var value = Mathf.Lerp(dialogueUI.anchoredPosition.y, yPos, elapsedTime / duration);

            dialogueUI.anchoredPosition = new Vector2(0, value);
        }
    }

    void DialogueControls()
    {
        if (PlayerInput.DialogueNextPress() && nextUI.activeSelf == true)
        {
            NextVoiceLine();
        }

        if (acceptDeclineUI.activeSelf == true)
        {
            int result = PlayerInput.DialogueChoicePress();
            Debug.Log(result);
            if (result == 0)
            {
                //Start next Scene
                EventSystem.CircleClose(false, curHouse.minigameIndex);
                canvas.enabled = false;
            }

            if (result == 1)
            {
                DialogueEnd();
                player.CanMove();
                curHouse.locked = false;
                index = 0;
            }
        }
    }

    [ContextMenu("Dialogue Start")]
    public void DialogueStart(House house)
    {
        curHouse = house;
        nextUI.SetActive(true);
        acceptDeclineUI.SetActive(false);
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

