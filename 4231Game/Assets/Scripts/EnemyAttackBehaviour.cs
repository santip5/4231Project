using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private attackerIdentifier hitBoxID1; 
    [SerializeField]
    private attackerIdentifier hitBoxID2;
    private AttackCollision hitBox1;
    private AttackCollision hitBox2;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Component[] hitboxes = animator.GetComponentsInChildren<AttackCollision>(false);
        animator.GetComponentInParent<NavMeshAgent>(false).isStopped = true;

        foreach (AttackCollision hitbox in hitboxes)
        {
            if (hitbox.ID == hitBoxID1)
            {
                hitBox1 = hitbox;
                hitBox1.active = true;
            } else if (hitbox.ID == hitBoxID2)
            {
                hitBox2 = hitbox;
                hitBox2.active = true;
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<EnemyLogic>().attacking = false;

        if (hitBox1 != null)
        {
            hitBox1.active = false;
        }
        if (hitBox2 != null)
        {
            hitBox2.active = false;
        }

        animator.GetComponentInParent<NavMeshAgent>(false).isStopped = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
