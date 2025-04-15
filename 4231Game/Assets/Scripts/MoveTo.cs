using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{

    public Transform goal;
    public float attack_distance;

    private Animator animator;
    private int animID_moveSpeed;
    private NavMeshAgent agent;
    private EnemyLogic enemyLogic;

    private bool playerDead;
    private bool dead;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyLogic = GetComponent<EnemyLogic>();

        animator = GetComponent<Animator>();
        animID_moveSpeed = Animator.StringToHash("moveSpeed");

        agent.stoppingDistance = attack_distance;

        playerDead = false;
        PlayerController.OnPlayerDied += PlayerDies;

        EnemyLogic.OnEnemyDied += Die;

        dead = false;
    }

    private void Update()
    {
        if (!dead)
        {
            if (playerDead)
            {
                agent.destination = new Vector3(0, 0, 0);
            }
            else if (Vector3.Distance(agent.transform.position, goal.position) < attack_distance)
            {
                enemyLogic.do_Attacks();
            }
            else
            {
                agent.destination = goal.position;
            } 
        }

        animator.SetFloat(animID_moveSpeed,agent.velocity.magnitude);
    }
    void OnDisable()
    {
        PlayerController.OnPlayerDied -= PlayerDies;
        EnemyLogic.OnEnemyDied -= Die;
    }

    public void PlayerDies()
    {
        playerDead = true;
    }

    public void Die()
    {
        dead = true;
        agent.isStopped = true;
    }
}
