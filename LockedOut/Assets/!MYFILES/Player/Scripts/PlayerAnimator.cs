using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetAnimSpeed(Component sender, object data)
    {
        if(data is PauseData)
        {
            PauseData pauseData = (PauseData)data;

            if(pauseData.pauseState)
            {
                animator.speed = 0;
            }
            else if (!pauseData.pauseState)
            {
                animator.speed = 1;
            }
        }
    }
}
