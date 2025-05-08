using System;
using System.Collections;
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

    [SerializeField] private AudioClip damageSoundClip;
    private AudioSource audioSource;

    [SerializeField]
    private Animator animator;


    private int animID_attackID;
    private int animID_attack;
    private int animID_hit;
    private int animID_Fullhit;
    private int animID_dead;
    private int animID_tired;
    private int animID_stunned;
    private int animID_revenge;

    public List<Attack> attackList;

    [DoNotSerialize]
    public bool attacking;
    [DoNotSerialize]
    public bool tired;

    [SerializeField] private RectTransform healthFillRect;
    [SerializeField] private float maxWidth = 500f;

    public int hitpoints_max;
    private int hitpoints;
    public int stun_max;
    public int stun;
    public int revenge_threshold;
    public int revenge_value;
    public bool stunned;
    private bool revenge_accumulate;
    public bool revenge_move;
    private bool dead;
    private bool runningRevenge;

    public Transform target;
    private Vector3 revenge_move_target;
    public float rotateSpeed;

    private NavMeshAgent agent;

    public static event Death OnEnemyDied;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attacking = false;
        tired = false;
        revenge_accumulate = false;
        dead = false;
        revenge_move = false;
        stunned = false;
        runningRevenge = false;

        init_attacks();
        attack_sequence = 1;

        animator = GetComponent<Animator>();
        animID_attackID = Animator.StringToHash("attackID");
        animID_attack = Animator.StringToHash("attack");
        animID_hit = Animator.StringToHash("Hit");
        animID_Fullhit = Animator.StringToHash("FullHit");
        animID_dead = Animator.StringToHash("Dead");
        animID_tired = Animator.StringToHash("Tired");
        animID_stunned = Animator.StringToHash("stunned");
        animID_revenge = Animator.StringToHash("Revenge");

        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        hitpoints = hitpoints_max;
        stun = stun_max;
        revenge_value = 0;
        OnEnemyDied += Die;

        maxWidth = healthFillRect.rect.width;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!tired && !dead && agent.isStopped)
        {
            transform.LookAt(target);
        }

        if (tired && !attacking && !stunned)
        {
            animator.SetTrigger(animID_tired);
            stun = 0;
            attacking = true;
        }

        if (!dead && revenge_move)
        {
            transform.position = Vector3.MoveTowards(transform.position, revenge_move_target, 10 * Time.deltaTime);
        }
        float percent = Mathf.Clamp01((float)hitpoints / hitpoints_max);
        healthFillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth * percent);
    }

    void OnDisable()
    {
        OnEnemyDied -= Die;
    }


    public void do_Attacks()
    {
        if (!attacking && !dead && !stunned)
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

            if (attackID >= attacks_1.Length || attackID >= attacks_2.Length)
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

        if (!revenge_move && !runningRevenge)
        {
            if (attacking && !stunned && !dead)
            {
                animator.SetTrigger(animID_hit);
            }
            else if (!stunned && !dead)
            {
                animator.SetTrigger(animID_Fullhit);
            }

            hitpoints -= attack.damage;
            audioSource.clip = damageSoundClip;
            audioSource.Play();
            if (!stunned && !dead)
            {
                stun -= attack.stun;
            }

            if (hitpoints <= 0)
            {
                OnEnemyDied?.Invoke();
            }
            else if (stun <= 0)
            {
                getStunned();
            }
            else if (revenge_accumulate)
            {
                revenge_value += attack.revenge;

                if (revenge_value >= revenge_threshold)
                {
                    animator.SetTrigger(animID_revenge);
                }
            }
        }
    }

    public void takeRevenge()
    {
        animator.SetTrigger(animID_revenge);
    }

    //IEnumerator WaitRevengeMove()
    //{
    //    runningRevenge = true;
    //    yield return new WaitForSeconds(3f);
    //    animator.SetTrigger(animID_revenge);
    //    revenge_move = true;
    //    revenge_move_target = -transform.forward * 5;
    //    yield return new WaitForSeconds(1f);
    //    revenge_move = false;
    //    agent.isStopped = false;
    //    runningRevenge = false;
    //}

    private void getStunned()
    {
        animator.SetTrigger(animID_stunned);
        stun = stun_max;
        revenge_value = 0;
        revenge_accumulate = true;
        agent.isStopped = true;
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

    public void startRevengeMove()
    {
        stun = stun_max;
        stunned = false;
        revenge_move = true;
        revenge_value=0;
        revenge_move_target = -transform.forward * 5;
    }

    public void stopRevengeMove()
    {
        revenge_move = false;
        agent.isStopped = false;
    }
}
