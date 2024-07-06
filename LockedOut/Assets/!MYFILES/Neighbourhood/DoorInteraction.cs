using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] House house;
    AudioSource source;
    [SerializeField] AudioClip knock;
    [SerializeField] AudioClip ring;
    [SerializeField] Transform doorPos;
    [SerializeField] Transform bellPos;
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
        if(inside && !house.locked)
        {
            int result = PlayerInput.GetDoorKeyPress();

            if (result == 0)
            {
                Debug.Log("Tried To Knock");
                EventSystem.OnDoorInteract(true, doorPos.position, doorPos.localRotation);
            }

            if (result == 1)
            {
                Debug.Log("Tried To Ring");
                EventSystem.OnDoorInteract(false, bellPos.position, bellPos.localRotation);
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
            EventSystem.DoorTriggerEnter(true, doorPos.position);
            inside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            EventSystem.DoorTriggerEnter(false, Vector3.zero);
            inside = false;
        }
    }
}
