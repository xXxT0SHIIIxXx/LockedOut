using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public ThirdPersonMovement player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NoMove()
    {
        player.CantMove();
    }

    public void YesMove()
    {
        player.CanMove();
    }
}
