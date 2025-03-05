using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IAttacker
{
    public float moveSpeed;
    public float rotateSpeed;
    private Vector2 moveDirection;
    public InputActionReference move;
    public InputActionReference attack;
    public Animator animator;
    public CharacterController characterController;
    public Collider rightFootAttack;

    public attack currentAttack;
    
    private int animID_running;
    private int animID_attack;
    private int animID_attackID;

    [DoNotSerialize]
    public bool attacking;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animID_running = Animator.StringToHash("running");
        animID_attack = Animator.StringToHash("Attack");
        animID_attackID = Animator.StringToHash("AttackID");

        attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!attacking) {
            moveDirection = move.action.ReadValue<Vector2>();
            MoveRelativeToCamera();
        }

        if (attack.action.WasPerformedThisFrame())
        {
            attacking = true;
            animator.SetTrigger(animID_attack);
            animator.SetInteger(animID_attackID, currentAttack.attackID);
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

        animator.SetFloat(animID_running, relativeMove.magnitude);

        characterController.Move(relativeMove * Time.deltaTime * moveSpeed);

        if (moveDirection != Vector2.zero) {
            Quaternion rotateTo = Quaternion.LookRotation(relativeMove, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, rotateSpeed * Time.deltaTime);
        }
    }

    public void OnFootstep()
    {

    }

    public void attackCollision(attackerIdentifier ID, Collider collision)
    {
        if (collision.gameObject.TryGetComponent<IHittable>(out IHittable hit)) {
            hit.hit(currentAttack);
        }
    }
}
