using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] CharacterController characterController;
    [SerializeField] Animator animator;
    [SerializeField] Transform cam;
    [SerializeField] GameObject freelookRig;

    [Header("Events")]
    public GameEvent OnInteractionDone;

    [Header("Movement Vars")]
    [SerializeField] float speed = 6f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] LayerMask isGround;
    [SerializeField] private float playerSmoothRotation = 0.2f;
    [SerializeField] private float rotationSpeed = 6f;

    bool canMove;
    Vector2 movementAxis;
    Vector3 currentNormal;
    Vector3 lastSurfacePos;
    Vector3 newNormal;
    float turnSmoothVelocity;
    // Start is called before the first frame update
    void Start()
    {
        PauseData data = new PauseData();
        data.pauseState = true;
        MoveControl(this, data);
    }

    // Update is called once per frame
    void Update()
    {

        if (canMove)
        {
            Movement();
           
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

    public void KnockRing(int knockRing, Vector3 pos, Quaternion doorRotation, HouseData hD)
    {
        Vector3 position = new Vector3(pos.x, transform.position.y, pos.z);
        StartCoroutine(KnockRingMovement(position, doorRotation, knockRing, hD));
    }

    private IEnumerator KnockRingMovement(Vector3 targetPosition, Quaternion targetRotation, int isKnock, HouseData hD)
    {
        string animBaseLayer = "Base Layer";
        int knockAnimHash = Animator.StringToHash(animBaseLayer + ".Knock");
        int ringAnimHash = Animator.StringToHash(animBaseLayer + ".Ring");
        var transPos = new Vector3(transform.position.x, 0, transform.position.z);
        // Lerp position
        while (Vector3.Distance(transPos, targetPosition) > 6f)
        {
            var dist = (Vector3.Distance(transPos, targetPosition));
            Debug.Log("Mypos: " + transPos.x + " : " + transPos.y + " : " + transPos.z);
            Debug.Log("targpos: " + targetPosition.x + " : " + targetPosition.y + " : " + targetPosition.z);
            Debug.Log(dist);
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
        if (isKnock == 0)
        {
            Debug.Log("is Knocking");
            animator.SetTrigger("Knock");
        }
        else if (isKnock == 1)
        {
            Debug.Log("is Ringing");
            animator.SetTrigger("Ring");
        }

        float counter = 0;
        float waitTime = 4;
        while (counter < (waitTime))
        {
            counter += Time.deltaTime;
            yield return null;
        }

        OnInteractionDone.Raise(this, hD);
    }

    public void MoveControl(Component sender, object data)
    {
        if(data is PauseData)
        {
            PauseData result = (PauseData)data;

            canMove = !result.pauseState;
        }
        else if(data is HouseData)
        {
            HouseData result = (HouseData)data;
            KnockRing(result.knockRing, result.curPos.position, result.curPos.localRotation,result);
            canMove = result.canPlayerMove;
        }
        else if(data is MovementData)
        {
            MovementData result = (MovementData)data;

            canMove = result.canMove;
        }
    }
}
