using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] CharacterController characterController;
    [SerializeField] Animator animator;
    [SerializeField] Transform cam;
    [SerializeField] GameObject freelookRig;

    [Header("Movement Vars")]
    [SerializeField] float speed = 6f;
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float sprintSpeed = 12f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] LayerMask isGround;
    [SerializeField] private float playerSmoothRotation = 0.2f;
    [SerializeField] private float rotationSpeed = 6f;

    bool isPaused;
    bool canMove;
    Vector2 movementAxis;
    Vector3 currentNormal;
    Vector3 lastSurfacePos;
    Vector3 newNormal;
    float turnSmoothVelocity;
    // Start is called before the first frame update
    void Start()
    {
        CanMove();
        EventSystem.OnEscPress += PauseControl;
        EventSystem.OnDoorAct += KnockRing;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Movement();
            //AlignToSurface();
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
                currentNormal = Vector3.Lerp(currentNormal, newNormal, playerSmoothRotation);
                transform.up = currentNormal;
            }
        }
    }

    void Movement()
    {
        movementAxis = PlayerInput.GetMovementAxis();
        Vector3 direction = new Vector3(movementAxis.x, 0, movementAxis.y).normalized;
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
        Vector3 position = new Vector3(pos.x, transform.position.y, pos.z);
        CantMove();
        StartCoroutine(KnockRingMovement(position, doorRotation, knockRing));
    }

    private IEnumerator KnockRingMovement(Vector3 targetPosition, Quaternion targetRotation, bool isKnock)
    {
        var transPos = new Vector3(transform.position.x, 0, transform.position.z);
        // Lerp position
        while (Vector3.Distance(transPos, targetPosition) > 6f)
        {
            Debug.Log(Vector3.Distance(transPos, targetPosition));
            animator.SetFloat("magnitude", 1f);
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;

        // Slerp rotation
        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        transform.rotation = targetRotation;

        // Trigger animation
        if (isKnock)
        {
            Debug.Log("is Knocking");
            animator.SetTrigger("Knock");
            EventSystem.OnDoorVocal(isKnock);
        }
        else
        {
            Debug.Log("is Ringing");
            animator.SetTrigger("Ring");
            EventSystem.OnDoorVocal(isKnock);
        }
    }

    public void CantMove()

    {
        canMove = false;
    }
    public void CanMove()
    {
        canMove = true;
    }

    public void PauseControl(bool pause)
    {
        isPaused = pause;

        if(isPaused)
        {
            CantMove();
            //freelookRig.SetActive(false);
        }
        else if (!isPaused)
        {
            CanMove();
            //freelookRig.SetActive(true);
        }
    }
}
