using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public Transform doorPos;
    bool inside;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inside)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Tried To Knock");
                EventSystem.OnDoorUse(true, doorPos.localRotation);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Tried To Ring");
                EventSystem.OnDoorUse(false, doorPos.localRotation);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            EventSystem.OnDoorHit(true, doorPos.position);
            inside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            EventSystem.OnDoorHit(false, Vector3.zero);
            inside = false;
        }
    }
}
