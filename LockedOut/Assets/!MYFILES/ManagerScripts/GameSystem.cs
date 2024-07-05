using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.OnLoadNext += LoadMinigameScene;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoadMinigameScene(bool start, int index)
    {
        Debug.Log("Loading Minigame" + index);
        SceneManager.LoadScene("Minigame" + index);
    }
}
