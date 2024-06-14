using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUI : MonoBehaviour
{
    public GameObject uiPrefab;
    GameObject doorUI;
    Vector3 doorPos;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.OnDoorInter += OnDoorInteract;
    }

    // Update is called once per frame
    void Update()
    {
        if(doorUI != null)
        {
            Vector3 UIpos = Vector3.Lerp(doorUI.transform.position, doorPos, 1);
            doorUI.transform.position = Camera.main.WorldToScreenPoint(UIpos);
        }
    }


    void OnDoorInteract(bool enterExit,Vector3 doorpos)
    {
        if(enterExit)
        {
            doorPos = doorpos;
            doorUI = Instantiate(uiPrefab, FindObjectOfType<Canvas>().transform);
        }
        else
        {
            doorPos = doorpos;
            Destroy(doorUI);
        }
    }
}
