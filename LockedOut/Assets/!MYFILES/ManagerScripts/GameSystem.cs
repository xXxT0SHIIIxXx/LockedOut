using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using static Cinemachine.DocumentationSortingAttribute;

public struct PauseData
{
    public bool pauseState;
}

public struct MovementData
{
    public bool canMove;
}

public struct MinigameData
{
    public int LevelID;
}

public struct FadeData
{
    public int nextLevelID;
    public bool isOpen;
}

public struct HouseData
{
    public int id;

    public Sprite portraitImg;

    public bool locked;
    public bool inside;
    public bool answered;
    public bool canPlayerMove;

    public string[] voiceLines;
    public int[] password;
    public List<int> passwordInput;

    public int minigameIndex;
    public int knockRing;
    

    public Transform UiPos;
    public Transform doorPos;
    public Transform bellPos;
    public Transform curPos;

    public void AddInputToPassWord(HouseData data)
    {
        data.passwordInput.Add(data.knockRing);
    }

    public bool isPasswordRight()
    {
        bool result = false;

        if(password.Length == passwordInput.Count)
        {
            for(int i = 0; i < password.Length; i++)
            {
                if (password[i] == passwordInput[i])
                {
                    result = true;
                }
                else
                {
                    result = false;
                    return result;
                }
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    public HouseData createDatafromHouse(House house)
    {
        HouseData data = new HouseData();
        data.id = house.minigameIndex;
        data.portraitImg = house.portraitImg;
        data.locked = house.locked;
        data.inside = house.inside;
        data.UiPos = house.UiPos;
        data.doorPos = house.doorPos;
        data.bellPos = house.bellPos;
        data.password = house.password;
        data.passwordInput = house.passwordInput;
        data.voiceLines = house.voiceLines;
        return data;
    }
}

public class GameSystem : MonoBehaviour
{

    [Header("Events")]
    public GameEvent OnPlayerPauseToggle;
    public GameEvent OnPlayerHouseInteract;
    public GameEvent OnDialogBegin;

    bool isGamePaused;
    bool pauseBttnEvent;

    HouseData curHouseData;

    public House[] houses;
    // Start is called before the first frame update
    void Start()
    {
        PauseData pauseData = isPaused();
        OnPlayerPauseToggle.Raise(this, pauseData);
    }

    // Update is called once per frame
    void Update()
    {
        PauseBttnLogic();
        HouseTriggerWatch();
    }

    public void HouseTriggerEvent(Component sender, object data)
    {
        if(data is HouseData)
        {
            HouseData result = (HouseData)data;
            curHouseData = result;
        }
    }

    public void HouseTriggerWatch()
    {
        if (curHouseData.inside && !curHouseData.answered)
        {
            int result = PlayerInput.GetDoorKeyPress();

            if (result == 0)
            {
                curHouseData.knockRing = result;
                curHouseData.curPos = curHouseData.doorPos;
                OnPlayerHouseInteract.Raise(this, curHouseData);
            }
            else if (result == 1)
            {
                curHouseData.knockRing = result;
                curHouseData.curPos = curHouseData.bellPos;
                OnPlayerHouseInteract.Raise(this, curHouseData);
            }
        }
    }

    public void InteractionDone(Component sender, object data)
    {
        if(data is HouseData)
        {
            HouseData result = (HouseData)data;
            curHouseData = result;

            if (curHouseData.password.Length == 1)
            {
                //No Password so Start Dialog.
                curHouseData.answered = true;
                OnDialogBegin.Raise(this, curHouseData);
            }
            else if(curHouseData.password.Length > 1)
            {
                //Has Password so Give Player back movement and input to list and wait for new keypress.
                curHouseData.canPlayerMove = true;

                //Add Input to List
                curHouseData.AddInputToPassWord(curHouseData);

                //Give Player back movement
                PauseData pauseData = isPaused();
                OnPlayerPauseToggle.Raise(this, pauseData);
            }
        }
    }

    void PauseBttnLogic()
    {
        if (PlayerInput.GetPausePress() || pauseBttnEvent)
        {
            if (pauseBttnEvent)
            {
                PauseData pauseData = PauseControl(false);
                OnPlayerPauseToggle.Raise(this, pauseData);
                pauseBttnEvent = false;
            }
            else
            {
                PauseData pauseData = PauseToggle();
                OnPlayerPauseToggle.Raise(this, pauseData);
            }
        }
    }

    PauseData PauseToggle()
    {
        PauseData data = new PauseData();
        isGamePaused = !isGamePaused;
        data.pauseState = isGamePaused;
        return data;
    }

    PauseData PauseControl(bool control)
    {
        PauseData data = new PauseData();
        isGamePaused = control;
        data.pauseState = isGamePaused;
        return data;
    }

    PauseData isPaused()
    {
        PauseData data = new PauseData();
        data.pauseState = isGamePaused;
        return data;
    }

    public void PauseBttnEvent()
    {
        pauseBttnEvent = true;
    }

    public void LoadMinigameScene(Component sender, object data)
    {
        if(data is MinigameData)
        {
            MinigameData result = (MinigameData)data;
            Debug.Log("Loading Minigame" + result.LevelID);
            SceneManager.LoadScene("Minigame" + result.LevelID);
        }
    }
}
