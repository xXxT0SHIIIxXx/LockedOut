using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.Rendering;

public class CursorController : MonoBehaviour
{
    bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EventSystem.OnEscPress += JustCursorControl;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Paused();
        }
    }

    void JustCursorControl(bool control)
    {
        if(control)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(!control)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        isPaused = control;
    }

    void Paused()
    {
        isPaused = !isPaused;
        EventSystem.OnPause(isPaused);
    }
}
