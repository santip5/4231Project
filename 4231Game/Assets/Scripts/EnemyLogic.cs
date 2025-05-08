using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static PlayerController;

public class EnemyLogic : MonoBehaviour, IHittable, IAttacker
{
    public int[] attacks_1 = new int[3];
    public int[] attacks_2 = new int[3];
    private int attack_sequence;
    private int current_attack;

    [SerializeField]
    private int attackID = 0;

    [SerializeField]
    private Animator animator;

    [SerializeField] private AudioClip damageSoundClip;
    private AudioSource audioSource;

    private int animID_attackID;
    private int animID_attack;
    private int animID_hit;
    private int animID_Fullhit;
    private int animID_dead;
    private int animID_tired;

    public List<Attack> attackList;
    
    [DoNotSerialize]
    public bool attacking;
    [DoNotSerialize]
    public bool tired;


    public int hitpoints_max;
    private int hitpoints;
    private bool dead;

    public Transform target;
    public float rotateSpeed;

    private NavMeshAgent agent;

    public static event Death OnEnemyDied;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attacking = false;
        tired = false;

        init_attacks();
        attack_sequence = 1;

        animator = GetComponent<Animator>();
        animID_attackID = Animator.StringToHash("attackID");
        animID_attack = Animator.StringToHash("attack");
        animID_hit = Animator.StringToHash("Hit");
        animID_Fullhit = Animator.StringToHash("FullHit");
        animID_dead = Animator.StringToHash("Dead");
        animID_tired = Animator.StringToHash("Tired");

        agent = GetComponent<NavMeshAgent>();


        hitpoints = hitpoints_max;
        dead = false;
        OnEnemyDied += Die;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!tired && !dead && agent.isStopped)
        {
            transform.LookAt(target);
        }
    }

    void OnDisable()
    {
        OnEnemyDied -= Die;
    }


    public void do_Attacks()
    {
        if(tired && !attacking)
        {
            animator.SetTrigger(animID_tired);
            attacking = true;
        }
        else if (!attacking && !dead)
        {
            attacking = true;

            animator.SetTrigger(animID_attack);

            if (attack_sequence == 1)
            {
                current_attack = attacks_1[attackID];
            }
            else if (attack_sequence == 2)
            {
                current_attack = attacks_2[attackID];
            }

            animator.SetInteger(animID_attackID, current_attack);


            attackID++;

            if(attackID >= attacks_1.Length || attackID >= attacks_2.Length)
            {
                attackID = 0;
                tired = true;

                //Get a random attack sequence. Use of range can be adjusted to allow for different probabilities, butfor now is uniform
                int r = UnityEngine.Random.Range(0, 2);
                if (r == 0) attack_sequence = 1;
                else if (r == 1) attack_sequence = 2;
            }
        }
    }
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

        hitpoints -= attack.damage;
        audioSource.clip = damageSoundClip;
        audioSource.Play();
        if(hitpoints <= 0)
        {
            OnEnemyDied?.Invoke();
        }
    }

    private void init_attacks()
    {
        attackList = new List<Attack>();
        attackList.Add(new Attack(10, 2, 1, false, 0));
        attackList.Add(new Attack(15, 3, 2, false, 0));
        attackList.Add(new Attack(10, 2, 0, false, 0));
    }

    public void attackCollision(attackerIdentifier ID, Collider collision)
    {
        if (collision.gameObject.TryGetComponent<IHittable>(out IHittable hit))
        {
            hit.hit(attackList[current_attack]);
        }
    }

    private void Die()
    {
        dead = true;
        animator.SetBool(animID_dead, true);
    }
}
