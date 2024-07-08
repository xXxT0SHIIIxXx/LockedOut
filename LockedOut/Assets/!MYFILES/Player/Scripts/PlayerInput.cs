using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput
{
    public static KeyCode pauseKey = KeyCode.Escape;
    public static KeyCode knockKey = KeyCode.E;
    public static KeyCode doorBellKey = KeyCode.Q;
    public static KeyCode acceptKey = KeyCode.E;
    public static KeyCode declineKey = KeyCode.Q;
    public static KeyCode nextKey = KeyCode.Tab;

    public static Vector2 GetMovementAxis()
    {
        float hori = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        return new Vector2(hori, vert);
    }

    public static bool GetPausePress()
    {
        return Input.GetKeyDown(pauseKey);
    }

    public static int GetDoorKeyPress()
    {
        int keyPress = 999;
        if (Input.GetKeyDown(knockKey))
        {
            keyPress = 0;
        }
        else if(Input.GetKeyDown(doorBellKey))
        {
            keyPress = 1; 
        }

        return keyPress;
    }

    public static bool DialogueNextPress()
    {
        return Input.GetKeyDown(nextKey);
    }

    public static int DialogueChoicePress()
    {
        int keyPress = 999;
        if (Input.GetKeyDown(acceptKey))
        {
            keyPress = 0;
        }
        else if (Input.GetKeyDown(declineKey))
        {
            keyPress = 1;
        }

        return keyPress;
    }
}
