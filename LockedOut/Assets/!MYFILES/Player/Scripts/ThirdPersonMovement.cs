using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
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

    public LayerMask isGround;
    private Vector3 lastSurfacePos;
    [SerializeField] private float playerSmoothRotation;
    private Vector3 newNormal;
    public Vector3 currentNormal;
    float turnSmoothVelocity;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.OnEscPress += isPaused;
        EventSystem.OnDoorAct += KnockRing;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            Movement();
            AlignToSurface();
        }
    }

    void AlignToSurface()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, isGround))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                newNormal = hit.normal;
                lastSurfacePos = hit.point;
                transform.position = new Vector3(transform.position.x, hit.point.y + 1f, transform.position.z);
                //currentNormal = Vector3.Lerp(currentNormal, newNormal, playerSmoothRotation);
                //transform.up = currentNormal;
            }
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

    public void KnockRing(bool knockRing, Vector3 pos, Quaternion doorRotation)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, doorRotation, 1f);
        Vector3 newPos = new Vector3(pos.x, transform.position.y, pos.z);
        transform.position = Vector3.Lerp(transform.position, newPos, 1f);
        if (knockRing)
        {
            Debug.Log("is Knocking");
            animator.SetTrigger("Knock");
            EventSystem.OnDoorVocal(knockRing);
        }
        else
        {
            Debug.Log("is Ringing");
            animator.SetTrigger("Ring");
            EventSystem.OnDoorVocal(knockRing);
        }
    }

    public void CantMove()

    {
        paused = true;
    }

    public void CanMove()
    {
        paused = false;
    }

    public void isPaused(bool pause)
    {
        paused = pause;

        if(paused)
        {
            //freelookRig.SetActive(false);
        }
        else if (!paused)
        {
            //freelookRig.SetActive(true);
        }
    }
}
