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

    string[] curLines;

    [Header("Events")]
    public GameEvent OnDialogAccept;
    public GameEvent OnDialogDecline;

    HouseData curHouse;
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
            if(curHouse.rejected && index == curLines.Length - 1)
            {
                //Close Dialog Box and reset
                curHouse.rejected = false;
                curHouse.passwordInput.Clear();
                index = 0;
                curHouse.answered = false;
                MovementData data = new MovementData();
                data.canMove = true;
                DialogueEnd();
                OnDialogDecline.Raise(this, data);
            }
            else
            {
                NextVoiceLine();
            }
        }

        if (acceptDeclineUI.activeSelf == true)
        {
            int result = PlayerInput.DialogueChoicePress();
            if (result == 0)
            {
                //Start next Scene
                canvas.enabled = false;
                FadeData data = new FadeData();
                data.nextLevelID = curHouse.id;
                data.isOpen = false;
                OnDialogAccept.Raise(this,data);
            }

            if (result == 1)
            {
                if(curHouse.passwordInput.Count >0)
                {
                    curHouse.passwordInput.Clear();
                }
                //Close Dialog Box and reset
                DialogueEnd();
                index = 0;
                curHouse.answered = false;
                MovementData data = new MovementData();
                data.canMove = true;
                OnDialogDecline.Raise(this,data);
            }
        }
    }

    [ContextMenu("Dialogue Start")]
    public void DialogueStart(Component sender, object data)
    {
        if(data is HouseData)
        {
            HouseData result = (HouseData)data;
            curHouse = result;
            nextUI.SetActive(true);
            acceptDeclineUI.SetActive(false);
            curLines = LineTypeSelect();
            portraitIMG.sprite = curHouse.portraitImg;
            dialogueTXT.text = curLines[index];
            elapsedTime = 0;
            opened = true;
        }
    }

    string[] LineTypeSelect()
    {
        if (curHouse.rejected)
        {
            return curHouse.rejectionLines;
        }
        else
        {
            return curHouse.voiceLines;
        }
    }

    public void DialogueEnd()
    {
        opened = false;
        index = 0;
        elapsedTime = 0;
    }

    void NextVoiceLine()
    {
        index++;

        if (index >= curLines.Length - 1 && !curHouse.rejected)
        {
            nextUI.SetActive(false);
            acceptDeclineUI.SetActive(true);
        }

        dialogueTXT.text = curLines[index];
    }
}

