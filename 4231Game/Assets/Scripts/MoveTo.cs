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


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyLogic = GetComponent<EnemyLogic>();

        animator = GetComponent<Animator>();
        animID_moveSpeed = Animator.StringToHash("moveSpeed");

        agent.stoppingDistance = attack_distance;
    }

    private void Update()
    {
        if (Vector3.Distance(agent.transform.position, goal.position) < attack_distance)
        {
            enemyLogic.do_Attacks();
        } else
        {
            agent.destination = goal.position;
        }

        animator.SetFloat(animID_moveSpeed,agent.velocity.magnitude);
    }
}
