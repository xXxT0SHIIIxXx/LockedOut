using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    AudioSource source;
    public AudioClip knock;
    public AudioClip ring;
    public Transform doorPos;
    public Transform bellPos;
    bool inside;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        EventSystem.OnDoorSound += OnSound;
    }

    // Update is called once per frame
    void Update()
    {
        if(inside)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Tried To Knock");
                EventSystem.OnDoorUse(true, doorPos.position, doorPos.localRotation);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Tried To Ring");
                EventSystem.OnDoorUse(false, bellPos.position, bellPos.localRotation);
            }
        }
    }

    void OnSound(bool knockRing)
    {
        if(knockRing)
        {
            source.PlayOneShot(knock);
        }
        else
        {
            source.PlayOneShot(ring);
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
