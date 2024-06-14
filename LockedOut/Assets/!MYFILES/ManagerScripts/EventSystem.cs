using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public static event Action<bool> OnEscPress;
    public static event Action<bool, Vector3> OnDoorInter;
    public static event Action<bool, Quaternion> OnDoorAct;

    public static void OnEscPressed(bool onEsc)
    {
        OnEscPress?.Invoke(onEsc);
    }

    public static void OnDoorHit(bool enterExit,Vector3 doorpos)
    {
        // true == enter trigger && false == exit trigger
        OnDoorInter?.Invoke(enterExit,doorpos);
    }

    public static void OnDoorUse(bool knockRing, Quaternion doorRotation)
    {
        //true == knock && false == ring
        OnDoorAct?.Invoke(knockRing, doorRotation);
    }
}
