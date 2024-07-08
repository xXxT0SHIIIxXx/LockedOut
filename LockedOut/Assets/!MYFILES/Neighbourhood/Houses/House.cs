using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour
{

    public Sprite portraitImg;
    public bool inside;
    public bool locked;
    public bool completed;
    public string[] voiceLines;
    public int minigameIndex;

    public int[] password;
    public List<int> passwordInput;

    public Transform UiPos;
    public Transform doorPos;
    public Transform bellPos;

}
