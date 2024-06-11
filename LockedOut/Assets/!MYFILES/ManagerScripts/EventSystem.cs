using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public static event Action<bool> OnEscPress;


    public static void OnEscPressed(bool onEsc)
    {
        OnEscPress?.Invoke(onEsc);
    }
}
