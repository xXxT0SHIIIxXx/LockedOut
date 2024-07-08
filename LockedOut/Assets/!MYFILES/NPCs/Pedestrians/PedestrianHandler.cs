using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PedestrianHandler : MonoBehaviour
{
    Animator animator;
    public GameObject[] waypoints;
    GameObject curWaypoint;
    public int curIndex;
    bool subtract;
    bool paused;
    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if(curWaypoint == null)
        {
            curWaypoint = waypoints[curIndex];
        }

        StartMovement();
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance <= 0.2)
        {
            SetNewDestination();
        }
    }

    void StartMovement()
    {
        agent.SetDestination(curWaypoint.transform.position);
    }

    void SetNewDestination()
    {
        if(curIndex >= waypoints.Length-1 || curIndex <= 0)
        {
            subtract = !subtract;

            if (curIndex == waypoints.Length - 1)
            {
                curIndex--;
            }
            else if (curIndex == 0)
            {
                curIndex++;
            }
        }
        else
        {
            if (subtract)
            {
                curIndex--;
            }
            else
            {
                curIndex++;
            }
        }

        curWaypoint = waypoints[curIndex];
        agent.SetDestination(curWaypoint.transform.position);
    }

    public void SetAnimSpeed(Component sender, object data)
    {
        if(data is PauseData)
        {
            PauseData pauseData = (PauseData)data;
            paused = pauseData.pauseState;
            if(pauseData.pauseState)
            {
                animator.speed = 0;
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
                
            }
            else if (!pauseData.pauseState)
            {
                animator.speed = 1;
                agent.isStopped = false;
            }
        }
    }
}
