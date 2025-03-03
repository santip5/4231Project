using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    private Vector2 moveDirection;
    public InputActionReference move;
    public InputActionReference attack;
    public Animator animator;
    public CharacterController characterController;
    
    private int runningID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        runningID = Animator.StringToHash("running");
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
        MoveRelativeToCamera();

        if (attack.action.WasPerformedThisFrame())
        {
            animator.SetTrigger("Test");
        }
    }

    void MoveRelativeToCamera()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 relativeForward = cameraForward * moveDirection.y;
        Vector3 relativeRight = cameraRight * moveDirection.x;

        Vector3 relativeMove = relativeForward + relativeRight;

        animator.SetFloat(runningID, relativeMove.magnitude);

        characterController.Move(relativeMove * Time.deltaTime * moveSpeed);

        if (moveDirection != Vector2.zero) {
            Quaternion rotateTo = Quaternion.LookRotation(relativeMove, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, rotateSpeed * Time.deltaTime);
        }
    }

    public void OnFootstep()
    {

    }
}
