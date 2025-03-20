using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IAttacker, IHittable
{
    public SaveManager SaveManager;

    public float moveSpeed;
    public float rotateSpeed;
    private Vector2 moveDirection;
    public InputActionReference move;
    public InputActionReference attack;
    public InputActionReference dash;
    public Animator animator;
    public CharacterController characterController;
    public Collider rightFootAttack;

    public Attack currentAttack;
    public List<Attack> attackList;
    public int[] Attack1IDList;
    //public int[] Attack2IDList;
    private int attackIndex;

    private int animID_running;
    private int animID_attack;
    private int animID_attackID;
    private int animID_hit;
    private int animID_dash;
    private int animID_armBlock;

    [DoNotSerialize]
    public bool attacking;
    [DoNotSerialize]
    public bool doNotIcrementAttacks;
    [DoNotSerialize]
    public bool dashing;
    [DoNotSerialize]
    public bool dashSpeedOn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animID_running = Animator.StringToHash("running");
        animID_attack = Animator.StringToHash("Attack");
        animID_attackID = Animator.StringToHash("AttackID");
        animID_hit = Animator.StringToHash("hit");
        animID_dash = Animator.StringToHash("dash");
        animID_armBlock = Animator.StringToHash("ArmBlock");

        attacking = false;
        doNotIcrementAttacks = false;
        dashing = false;
        attackIndex = 0;
        LoadAttackList();
    }

    // Update is called once per frame
    void Update()
    {
        if (!attacking)
        {
            moveDirection = move.action.ReadValue<Vector2>();

            if (dashSpeedOn)
            {
                if (moveDirection.magnitude == 0)
                {
                    moveDirection = transform.forward;
                }
                moveDirection *= 2;
            }

            MoveRelativeToCamera();
        }

        if (attack.action.WasPerformedThisFrame() && !dashing)
        {
            currentAttack = attackList[SaveManager.Instance.passedAttacks[attackIndex]];

            animator.SetTrigger(animID_attack);
            animator.SetInteger(animID_attackID, currentAttack.attackID);

            if (!doNotIcrementAttacks)
            {
                attackIndex++;

                if (attackIndex > Attack1IDList.Length - 1)
                {
                    attackIndex = 0;
                }

                if (attacking)
                {
                    doNotIcrementAttacks = true;
                }
            }

            attacking = true;
        }

        if (!attacking && !dashing && dash.action.WasPerformedThisFrame())
        {
            dashing = true;
            animator.SetTrigger(animID_dash);
            animator.SetTrigger(animID_armBlock);
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

        if (moveDirection != Vector2.zero)
        {
            Quaternion rotateTo = Quaternion.LookRotation(relativeMove, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, rotateSpeed * Time.deltaTime);
        }
    }

    public void OnFootstep()
    {

    }

    public void attackCollision(attackerIdentifier ID, Collider collision)
    {
        if (collision.gameObject.TryGetComponent<IHittable>(out IHittable hit))
        {
            hit.hit(currentAttack);
        }
    }

    /* Failed to get JSON to work with structs, hard coding for now
    private void SaveAttackList()
    {
        string json = JsonUtility.ToJson(attackList);
        using (StreamWriter writer = new StreamWriter(Application.dataPath + Path.AltDirectorySeparatorChar + "AttackData.json"))
        {
            writer.Write(json);
        }
    }
    */

    private void LoadAttackList()
    {
        /* Failed to get JSON working with structs, hard coding for now
        string json = string.Empty;
        using (StreamReader reader = new StreamReader(Application.dataPath + Path.AltDirectorySeparatorChar + "AtttackData.json"))
        {
            json = reader.ReadToEnd();
        }

        attackList = JsonUtility.FromJson<List<Attack>>(json);
        */

        attackList = new List<Attack>();
        attackList.Add(new Attack(10, 2, 1, false, 0));
        attackList.Add(new Attack(20, 1, 3, false, 1));
        attackList.Add(new Attack(30, 0, 5, false, 2));
    }

    public void hit(Attack attack)
    {
        if (!dashing)
        {
            Debug.Log($"Damge: {attack.damage}\n Stun: {attack.stun}\n Revenge: {attack.revenge}\n ID: {attack.attackID}\n Special: {attack.isSpecial}");
            animator.SetTrigger(animID_hit);
        }
    }

    public void DashSpeed()
    {
        dashSpeedOn = !dashSpeedOn;
    }
}
