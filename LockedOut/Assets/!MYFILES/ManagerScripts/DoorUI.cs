using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUI : MonoBehaviour
{
    public GameObject uiPrefab;
    [SerializeField] Canvas canvas;

    GameObject doorUI;
    Vector3 doorPos;
    // Start is called before the first frame update
    void Start()
    {

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


    public void ManageDoorUI(Component sender, object data)
    {
        if(data is HouseData)
        {
            HouseData houseData = (HouseData)data;

            if (houseData.inside)
            {
                doorPos = houseData.doorPos.position;
                doorUI = Instantiate(uiPrefab, canvas.transform);
            }
            else if (!houseData.inside)
            {
                doorPos = Vector3.zero;
                Destroy(doorUI);
            }
        }
    }
}
