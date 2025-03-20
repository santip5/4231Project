using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyLogic : MonoBehaviour, IHittable, IAttacker
{
    public int[] attacks_1 = new int[3];
    public int[] attacks_2 = new int[3];

    [SerializeField]
    private int attackID = 0;

    [SerializeField]
    private Animator animator;

    private int animID_attackID;
    private int animID_attack;
    private int animID_hit;
    private int animID_Fullhit;

    public List<Attack> attackList;
    
    [DoNotSerialize]
    public bool attacking;

    public void hit(Attack attack)
    {
        Debug.Log($"Damge: {attack.damage}\n Stun: {attack.stun}\n Revenge: {attack.revenge}\n ID: {attack.attackID}\n Special: {attack.isSpecial}");

        if(attacking)
        {
            animator.SetTrigger(animID_hit);
        } else
        {
            animator.SetTrigger(animID_Fullhit);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attacking = false;

        init_attacks();

        animator = GetComponent<Animator>();
        animID_attackID = Animator.StringToHash("attackID");
        animID_attack = Animator.StringToHash("attack");
        animID_hit = Animator.StringToHash("Hit");
        animID_Fullhit = Animator.StringToHash("FullHit");
    }

    // Update is called once per frame
    void Update()
    {
        //do_Attacks();
    }

    public void do_Attacks()
    {
        if (!attacking)
        {
            attacking = true;

            animator.SetTrigger(animID_attack);
            animator.SetInteger(animID_attackID, attacks_1[attackID]);

            attackID++;

            if(attackID >= attacks_1.Length || attackID >= attacks_2.Length)
            {
                attackID = 0;
            }
        }
    }

    private void init_attacks()
    {
        attackList = new List<Attack>();
        attackList.Add(new Attack(10, 2, 1, false, 0));
        attackList.Add(new Attack(15, 3, 2, false, 0));
    }

    public void attackCollision(attackerIdentifier ID, Collider collision)
    {
        if (collision.gameObject.TryGetComponent<IHittable>(out IHittable hit))
        {
            hit.hit(attackList[attacks_1[attackID]]);
        }
    }
}
