using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController characterController;
    public Animator animator;
    public Transform cam;
    public GameObject freelookRig;
    public float speed = 6f;
    bool paused;
    public float walkSpeed = 6f;
    public float sprintSpeed = 12f;
    public float turnSmoothTime = 0.1f;

    float turnSmoothVelocity;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.OnEscPress += isPaused;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            Movement();
        }
    }

    void Movement()
    {
        float hori = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(hori, 0, vert).normalized;
        animator.SetFloat("magnitude", direction.magnitude);
        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    public void isPaused(bool pause)
    {
        paused = pause;

        if(paused)
        {
            freelookRig.SetActive(false);
        }
        else if (!paused)
        {
            freelookRig.SetActive(true);
        }
    }
}
