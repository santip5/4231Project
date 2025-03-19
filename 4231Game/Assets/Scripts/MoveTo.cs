using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{

    public Transform goal;
    public float attack_distance;

    private Animator animator;
    private NavMeshAgent agent;
    private EnemyLogic enemyLogic;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyLogic = GetComponent<EnemyLogic>();

        agent.stoppingDistance = attack_distance;
    }

    private void Update()
    {
        if (Vector3.Distance(agent.transform.position, goal.position) < attack_distance)
        {
            agent.isStopped = true;
            enemyLogic.do_Attacks();
        } else
        {
            agent.isStopped = false;
            agent.destination = goal.position;
        }
    }
}
