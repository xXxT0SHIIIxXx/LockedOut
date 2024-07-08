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
    bool inside;

    public GameEvent OnHouseTriggerEnter;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            
            house.inside = true;
            HouseData houseData = new HouseData();
            houseData = houseData.createDatafromHouse(house);
            OnHouseTriggerEnter.Raise(this, houseData);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            house.inside = false;
            HouseData houseData = new HouseData();
            houseData = houseData.createDatafromHouse(house);
            OnHouseTriggerEnter.Raise(this, houseData);
        }
    }
}
