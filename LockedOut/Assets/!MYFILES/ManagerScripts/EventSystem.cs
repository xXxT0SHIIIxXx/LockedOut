using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventSystem
{
    // Actions passed Alongside functions
    public static event Action<bool> OnEscPress;

    public static event Action<bool, Vector3> OnDoorEnter;
    public static event Action<bool, Vector3, Quaternion> OnDoorAct;
    public static event Action<bool> OnDoorSound;

    public static event Action<bool,int> OnCircleClose;

    public static event Action<bool,int> OnLoadNext;

    public static event Action<House> OnDialogueBegin;

    public static void OnPause(bool isPaused)
    {
        //True == Paused && False == UnPause
        OnEscPress?.Invoke(isPaused);
    }

    public static void DoorTriggerEnter(bool enterExit,Vector3 doorpos)
    {
        // true == enter trigger && false == exit trigger
        OnDoorEnter?.Invoke(enterExit,doorpos);
    }

    public static void OnDoorInteract(bool knockRing, Vector3 pos, Quaternion doorRotation)
    {
        //true == knock && false == ring
        OnDoorAct?.Invoke(knockRing, pos, doorRotation);
    }

    public static void OnDoorVocal(bool knockRing)
    {
        OnDoorSound?.Invoke(knockRing);
    }

    public static void OnLoadNextLevel(bool start, int levelIndex)
    {
        OnLoadNext?.Invoke(start, levelIndex);
    }

    public static void CircleClose(bool openClose, int levelIndex)
    {
        OnCircleClose?.Invoke(openClose,levelIndex);
    }

    public static void OnDialogueStart(House house)
    {
        OnDialogueBegin?.Invoke(house);
    }
}
