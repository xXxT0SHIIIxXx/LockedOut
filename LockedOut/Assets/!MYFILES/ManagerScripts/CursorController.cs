using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.Rendering;

public class CursorController : MonoBehaviour
{
    bool paused;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Paused();
        }

        //manager.profile.TryGet<CircleWipeVolumeComponent>(out CircleWipeVolumeComponent circ);
        //circ.radius.Override(0.3f);
    }

    void Paused()
    {
        paused = !paused;

        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (!paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        EventSystem.OnEscPressed(paused);
    }
}
