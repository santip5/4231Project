using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IAttacker, IHittable
{
    public float moveSpeed;
    public float rotateSpeed;
    private Vector2 moveDirection;
    public InputActionReference move;
    public InputActionReference attack;
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

    [DoNotSerialize]
    public bool attacking;
    [DoNotSerialize]
    public bool doNotIcrementAttacks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animID_running = Animator.StringToHash("running");
        animID_attack = Animator.StringToHash("Attack");
        animID_attackID = Animator.StringToHash("AttackID");

        attacking = false;
        doNotIcrementAttacks = false;
        attackIndex = 0;
        LoadAttackList();
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
            currentAttack = attackList[Attack1IDList[attackIndex]];

            animator.SetTrigger(animID_attack);
            animator.SetInteger(animID_attackID, currentAttack.attackID);

            if (!doNotIcrementAttacks)
            {
                attackIndex++;

                if (attackIndex > Attack1IDList.Length - 1)
                {
                    attackIndex = 0;
                }

                if(attacking)
                {
                    doNotIcrementAttacks = true;
                }
            }

            attacking = true;
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
        Debug.Log($"Damge: {attack.damage}\n Stun: {attack.stun}\n Revenge: {attack.revenge}\n ID: {attack.attackID}\n Special: {attack.isSpecial}");
    }
}
